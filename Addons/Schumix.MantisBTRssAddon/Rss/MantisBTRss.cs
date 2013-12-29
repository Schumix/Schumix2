/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013-2014 Schumix Team <http://schumix.eu/>
 * 
 * Schumix is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * Schumix is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with Schumix.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Xml;
using System.Net;
using System.Text;
using System.Timers;
using System.Threading;
using Schumix.Irc;
using Schumix.Framework;
using Schumix.Framework.Util;
using Schumix.Framework.Bitly;
using Schumix.Framework.Logger;
using Schumix.Framework.Functions;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using Schumix.MantisBTRssAddon.Config;
using Schumix.MantisBTRssAddon.Localization;

namespace Schumix.MantisBTRssAddon
{
	sealed class MantisBTRss
	{
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly PLocalization sLocalization = Singleton<PLocalization>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private System.Timers.Timer _timer = new System.Timers.Timer();
		private NetworkCredential _credential;
		private Thread _thread;
		private int errornumber;
		private int errorday;
		private readonly string _name;
		private readonly string _url;
		private string _oldbug;
		private string _title;
		private string _link;
		private string _username;
		private string _password;
		private string _servername;
		public bool Started { get; private set; }

		public MantisBTRss(string ServerName, string name, string url)
		{
			_servername = ServerName;
			_name = name;

			if(url.Contains(SchumixBase.Colon.ToString()) && url.Contains("@"))
			{
				_url = url.Substring(0, url.IndexOf("//")+2);
				url = url.Remove(0, url.IndexOf("//")+2);
				_username = url.Substring(0, url.IndexOf(SchumixBase.Colon));
				url = url.Remove(0, url.IndexOf(SchumixBase.Colon)+1);
				_password = url.Substring(0, url.IndexOf("@"));
				url = url.Remove(0, url.IndexOf("@")+1);
				_url += url;
				_credential = new NetworkCredential(_username, _password);
			}
			else
			{
				_url = url;
				_username = string.Empty;
				_password = string.Empty;
			}

			Init();
		}

		private void Init()
		{
			_title = "rss/channel/item/title";
			_link = "rss/channel/item/link";
		}

		public string Name
		{
			get { return _name; }
		}

		public void Start()
		{
			errorday = 0;
			errornumber = 0;
			Started = true;
			_timer.Interval = 24*60*60*1000;
			_timer.Elapsed += HandleTimerError;
			_timer.Enabled = true;
			_timer.Start();

			_thread = new Thread(Update);
			_thread.Start();
		}

		public void Stop()
		{
			Started = false;
			_timer.Enabled = false;
			_timer.Elapsed -= HandleTimerError;
			_timer.Stop();
			_thread.Abort();
		}

		public void Reload()
		{
			_thread.Abort();
			_thread = new Thread(Update);
			_thread.Start();
		}

		private void HandleTimerError(object sender, ElapsedEventArgs e)
		{
			if(errornumber >= 20)
			{
				errorday++;
				errornumber = 0;
			}

			if(errorday > 3)
				Stop();
		}

		private void Update()
		{
			try
			{
				var url = GetUrl();
				if(!url.IsNull())
					_oldbug = BugCode(url);

				while(true)
				{
					try
					{
						if(!sIrcBase.Networks[_servername].sMyChannelInfo.IsNull() && sIrcBase.Networks[_servername].sMyChannelInfo.FSelect(IFunctions.Mantisbt) && errornumber < 20)
						{
							url = GetUrl();
							if(url.IsNull())
							{
								Clean(url);
								Thread.Sleep(RssConfig.QueryTime*1000);
								continue;
							}

							string newbug = BugCode(url);
							if(newbug == "no text")
							{
								Clean(url);
								Thread.Sleep(RssConfig.QueryTime*1000);
								continue;
							}

							if(_oldbug != newbug)
							{
								string title = Title(url);
								if(title == "no text")
								{
									Clean(url);
									Thread.Sleep(RssConfig.QueryTime*1000);
									continue;
								}

								string link = Link(url);
								if(link == "no text")
								{
									Clean(url);
									Thread.Sleep(RssConfig.QueryTime*1000);
									continue;
								}

								Informations(newbug, title, link);
								_oldbug = newbug;
							}

							Clean(url);
							Thread.Sleep(RssConfig.QueryTime*1000);
						}
						else 
							Thread.Sleep(1000);
					}
					catch(Exception e)
					{
						if(SchumixBase.ExitStatus)
							return;

						errornumber++;
						Log.Error("MantisBTRss", sLConsole.GetString("[{0}] Failure details: {1}"), _name, e.Message);
						Thread.Sleep(RssConfig.QueryTime*1000);
					}
				}
			}
			catch(Exception e)
			{
				if(SchumixBase.ExitStatus)
					return;

				errornumber++;
				Log.Error("MantisBTRss", sLConsole.GetString("[{0}] Fatal failure details: {1}"), _name, e.Message);
				Thread.Sleep(RssConfig.QueryTime*1000);

				if(e.Message != "Thread was being aborted")
					Update();
			}
		}

		private void Clean(XmlDocument xml)
		{
			xml.RemoveAll();
			xml = null;
		}

		private XmlDocument GetUrl()
		{
			try
			{
				if(!_username.IsNullOrEmpty() && !_password.IsNullOrEmpty())
				{
					var rss = new XmlDocument();
					rss.LoadXml(DownloadToXml(sUtilities.DownloadString(_url, "</item>", _credential)));
					return rss;
				}
				else
				{
					var rss = new XmlDocument();
					rss.LoadXml(DownloadToXml(sUtilities.DownloadString(_url, "</item>")));
					return rss;
				}
			}
			catch(Exception e)
			{
				Log.Error("MantisBTRss", sLConsole.GetString("[{0}] Failure details: {1}"), _name, e.Message);
				Thread.Sleep(RssConfig.QueryTime*1000);
			}

			return null;
		}

		private string DownloadToXml(string data)
		{
			if(data.IsNullOrEmpty())
				return string.Empty;

			data = data.Substring(0, data.IndexOf("</item>") + "</item>".Length);
			data += "</channel></rss>";
			return data;
		}

		private string Title(XmlDocument rss)
		{
			var title = rss.SelectSingleNode(_title);
			return title.IsNull() ? "no text" : (title.InnerText.StartsWith(SchumixBase.NewLine.ToString()) ? title.InnerText.Substring(2).TrimStart() : title.InnerText);
		}

		private string Link(XmlDocument rss)
		{
			var link = rss.SelectSingleNode(_link);
			return link.IsNull() ? "no text" : link.InnerText;
		}

		private string BugCode(XmlDocument rss)
		{
			var title = rss.SelectSingleNode(_title);
			if(title.IsNull())
				return "no text";

			return title.InnerText.Substring(0, title.InnerText.IndexOf(SchumixBase.Colon));
		}

		private void Informations(string bugcode, string title, string link)
		{
			if(!sIrcBase.Networks[_servername].Online)
				return;

			var db = SchumixBase.DManager.QueryFirstRow("SELECT Channel, ShortUrl, Colors FROM mantisbt WHERE Name = '{0}' And ServerName = '{1}'", _name, _servername);
			if(!db.IsNull())
			{
				string[] channel = db["Channel"].ToString().Split(SchumixBase.Comma);

				foreach(var chan in channel)
				{
					string language = sLManager.GetChannelLocalization(chan, _servername);

					if(db["ShortUrl"].ToBoolean())
						link = BitlyApi.ShortenUrl(link).ShortUrl;
					
					if(db["Colors"].ToBoolean())
					{
						if(title.Contains(SchumixBase.Colon.ToString()))
						{
							sIrcBase.Networks[_servername].sSendMessage.SendCMPrivmsg(chan, sLocalization.MantisBTRss("Text", language), _name, bugcode, link);
							sIrcBase.Networks[_servername].sSendMessage.SendCMPrivmsg(chan, sLocalization.MantisBTRss("Text2", language), _name, title.Substring(title.IndexOf(SchumixBase.Colon)+1));
						}
					}
					else
					{
						if(title.Contains(SchumixBase.Colon.ToString()))
						{
							sIrcBase.Networks[_servername].sSendMessage.SendCMPrivmsg(chan, sLocalization.MantisBTRss("Text3", language), _name, bugcode, link);
							sIrcBase.Networks[_servername].sSendMessage.SendCMPrivmsg(chan, sLocalization.MantisBTRss("Text4", language), _name, title.Substring(title.IndexOf(SchumixBase.Colon)+1));
						}
					}

					Thread.Sleep(1000);
				}
			}
		}
	}
}