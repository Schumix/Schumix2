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
using Schumix.RssAddon.Config;
using Schumix.RssAddon.Localization;

namespace Schumix.RssAddon
{
	sealed class Rss
	{
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly PLocalization sLocalization = Singleton<PLocalization>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private System.Timers.Timer _timer = new System.Timers.Timer();
		private NetworkCredential _credential;
		private XmlNamespaceManager _ns;
		private Thread _thread;
		private int errornumber;
		private int errorday;
		private readonly string _name;
		private readonly string _url;
		private readonly string _website;
		private string _oldguid;
		private string _guid;
		private string _title;
		private string _author;
		private string _link;
		private string _username;
		private string _password;
		private string _servername;
		public bool Started { get; private set; }

		public Rss(string ServerName, string name, string url, string website)
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

			_website = website;
			Init();

			try
			{
				if(!_username.IsNullOrEmpty() && !_password.IsNullOrEmpty())
				{
					var rss = new XmlDocument();
					rss.LoadXml(DownloadToXml(sUtilities.DownloadString(_url, "</item>", _credential)));
					_ns = new XmlNamespaceManager(rss.NameTable);
					_ns.AddNamespace("dc", "http://purl.org/dc/elements/1.1/");
				}
				else
				{
					var rss = new XmlDocument();
					rss.LoadXml(DownloadToXml(sUtilities.DownloadString(_url, "</item>")));
					_ns = new XmlNamespaceManager(rss.NameTable);
					_ns.AddNamespace("dc", "http://purl.org/dc/elements/1.1/");
				}
			}
			catch(Exception e)
			{
				Log.Error("Rss", sLConsole.GetString("[{0}] Failure details: {1}"), _name, e.Message);
			}
		}

		private void Init()
		{
			if(_website == "default")
			{
				_guid = "rss/channel/item/guid";
				_link = "rss/channel/item/link";
				_title = "rss/channel/item/title";
				_author = "rss/channel/item/dc:creator";
			}
			if(_website == "default2")
			{
				_guid = "rss/channel/item/guid";
				_link = "rss/channel/item/link";
				_title = "rss/channel/item/title";
				_author = "rss/channel/item/creator";
			}
			else if(_website == "rdf")
			{
				_guid = "rdf/item/date";
				_link = "rdf/item/link";
				_title = "rdf/item/title";
				_author = "rdf/item/creator";
			}
			else
			{
				_guid = string.Empty;
				_link = string.Empty;
				_title = string.Empty;
				_author = string.Empty;
			}
		}

		public string Name
		{
			get { return _name; }
		}

		public void Start()
		{
			Started = true;
			errorday = 0;
			errornumber = 0;
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
					_oldguid = Guid(url);

				while(true)
				{
					try
					{
						if(!sIrcBase.Networks[_servername].sMyChannelInfo.IsNull() && sIrcBase.Networks[_servername].sMyChannelInfo.FSelect(IFunctions.Rss) && errornumber < 20)
						{
							url = GetUrl();
							if(url.IsNull())
							{
								Clean(url);
								Thread.Sleep(RssConfig.QueryTime*1000);
								continue;
							}

							string newguid = Guid(url);
							if(newguid == "no text")
							{
								Clean(url);
								Thread.Sleep(RssConfig.QueryTime*1000);
								continue;
							}

							if(_oldguid != newguid)
							{
								string title = Title(url);
								if(title == "no text")
								{
									Clean(url);
									Thread.Sleep(RssConfig.QueryTime*1000);
									continue;
								}

								string author = Author(url);
								if(author == "no text" && _website != "default2")
								{
									Clean(url);
									Thread.Sleep(RssConfig.QueryTime*1000);
									continue;
								}

								string curl = CommitUrl(url);
								if(curl == "no text")
								{
									Clean(url);
									Thread.Sleep(RssConfig.QueryTime*1000);
									continue;
								}
								
								Informations(newguid, title, author, curl);
								_oldguid = newguid;
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
						Log.Error("Rss", sLConsole.GetString("[{0}] Failure details: {1}"), _name, e.Message);
						Thread.Sleep(RssConfig.QueryTime*1000);
					}
				}
			}
			catch(Exception e)
			{
				if(SchumixBase.ExitStatus)
					return;

				errornumber++;
				Log.Error("Rss", sLConsole.GetString("[{0}] Fatal failure details: {1}"), _name, e.Message);
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
					rss.LoadXml(DownloadToXml(sUtilities.DownloadString(_url, 60*1000, "</item>", _credential)));
					return rss;
				}
				else
				{
					var rss = new XmlDocument();
					rss.LoadXml(DownloadToXml(sUtilities.DownloadString(_url, 60*1000, "</item>")));
					return rss;
				}
			}
			catch(Exception e)
			{
				Log.Error("Rss", sLConsole.GetString("[{0}] Failure details: {1}"), _name, e.Message);
				Thread.Sleep(RssConfig.QueryTime*1000);
			}

			return null;
		}

		private string DownloadToXml(string data)
		{
			if(data.IsNullOrEmpty())
				return string.Empty;

			data = data.Substring(0, data.IndexOf("</item>") + "</item>".Length);

			if(_website == "rdf")
			{
				string data2 = "<rdf><item>";
				data = data.Remove(0, data.IndexOf("<item rdf:about=") + "<item rdf:about=".Length);
				data2 += data.Substring(data.IndexOf("\">") + "\">".Length);
				data = data2;
				data += "</rdf>";
				data = data.Replace("dc:", "");
				data = data.Replace("dc-unixtime", "date.x-unixtime");

				if(data.Contains("<content:encoded>"))
				{
					data2 = data.Substring(0, data.IndexOf("<content:encoded>"));
					data = data.Remove(0, data.IndexOf("<content:encoded>") + "<content:encoded>".Length);
					data2 += data.Substring(data.IndexOf("</content:encoded>") + "</content:encoded>".Length);
					data = data2;
				}
			}
			else
				data += "</channel></rss>";

			return data;
		}

		private string Title(XmlDocument rss)
		{
			var title = rss.SelectSingleNode(_title);
			return title.IsNull() ? "no text" : (title.InnerText.StartsWith(SchumixBase.NewLine.ToString()) ? title.InnerText.Substring(2).TrimStart() : title.InnerText);
		}

		private string Author(XmlDocument rss)
		{
			if(_website == "rdf")
			{
				var author = rss.SelectSingleNode(_author);
				return author.IsNull() ? "no text" : author.InnerText;
			}
			else
			{
				var author = rss.SelectSingleNode(_author, _ns);
				return author.IsNull() ? "no text" : author.InnerText;
			}
		}

		private string Guid(XmlDocument rss)
		{
			var guid = rss.SelectSingleNode(_guid);
			return guid.IsNull() ? "no text" : guid.InnerText;
		}

		private string CommitUrl(XmlDocument rss)
		{
			var curl = rss.SelectSingleNode(_link);
			return curl.IsNull() ? "no text" : (curl.InnerText.StartsWith(SchumixBase.NewLine.ToString()) ? curl.InnerText.Substring(2).TrimStart() : curl.InnerText);
		}
		
		private void Informations(string guid, string title, string author, string commiturl)
		{
			if(!sIrcBase.Networks[_servername].Online)
				return;

			var db = SchumixBase.DManager.QueryFirstRow("SELECT Channel, ShortUrl, Colors FROM rssinfo WHERE Name = '{0}' And ServerName = '{1}'", _name, _servername);
			if(!db.IsNull())
			{
				string[] channel = db["Channel"].ToString().Split(SchumixBase.Comma);

				foreach(var chan in channel)
				{
					string language = sLManager.GetChannelLocalization(chan, _servername);

					if(db["ShortUrl"].ToBoolean())
						commiturl = BitlyApi.ShortenUrl(commiturl).ShortUrl;

					if(db["Colors"].ToBoolean())
					{
						sIrcBase.Networks[_servername].sSendMessage.SendCMPrivmsg(chan, sLocalization.Rss("Rss", language), _name, (author == "no text" ? "?" : author), commiturl);
						sIrcBase.Networks[_servername].sSendMessage.SendCMPrivmsg(chan, sLocalization.Rss("Rss2", language), _name, title);
					}
					else
					{
						sIrcBase.Networks[_servername].sSendMessage.SendCMPrivmsg(chan, sLocalization.Rss("nocolorsRss", language), _name, (author == "no text" ? "?" : author), commiturl);
						sIrcBase.Networks[_servername].sSendMessage.SendCMPrivmsg(chan, sLocalization.Rss("nocolorsRss2", language), _name, title);
					}

					Thread.Sleep(1000);
				}
			}
		}
	}
}