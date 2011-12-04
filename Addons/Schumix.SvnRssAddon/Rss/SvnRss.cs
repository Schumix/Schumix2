/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2011 Megax <http://www.megaxx.info/>
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
using System.Threading;
using Schumix.API;
using Schumix.Irc;
using Schumix.Framework;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using Schumix.SvnRssAddon.Config;
using Schumix.SvnRssAddon.Localization;

namespace Schumix.SvnRssAddon
{
	public sealed class SvnRss
	{
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly PLocalization sLocalization = Singleton<PLocalization>.Instance;
		private readonly ChannelInfo sChannelInfo = Singleton<ChannelInfo>.Instance;
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;
		private Thread _thread;
		private readonly string _name;
		private readonly string _url;
		private readonly string _website;
		private string _oldrev;
		private string _title;
		private string _author;
		private string[] info;
		private string _username;
		private string _password;
		public bool Started { get; private set; }

		public SvnRss(string name, string url, string website)
		{
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
			}
			else
			{
				_url = url;
				_username = string.Empty;
				_password = string.Empty;
			}

			_website = website;
			Init();
		}

		private void Init()
		{
			if(_website == "assembla")
			{
				_title = "rss/channel/item/title";
				_author = "rss/channel/item/author";
			}
			else
			{
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
			_thread = new Thread(Update);
			_thread.Start();
		}

		public void Stop()
		{
			Started = false;
			_thread.Abort();
		}

		public void Reload()
		{
			_thread.Abort();
			_thread = new Thread(Update);
			_thread.Start();
		}

		private void Update()
		{
			try
			{
				XmlDocument url;
				string newrev;
				string title;
				string author;

				url = GetUrl();
				if(!url.IsNull())
					_oldrev = Revision(url);

				while(true)
				{
					try
					{
						if(sChannelInfo.FSelect(IFunctions.Svn))
						{
							url = GetUrl();
							if(url.IsNull())
								continue;

							newrev = Revision(url);
							if(newrev == "no text")
								continue;

							if(_oldrev != newrev)
							{
								title = Title(url);
								if(title == "no text")
									continue;

								author = Author(url);
								if(author == "no text")
									continue;

								Informations(newrev, title, author);
								_oldrev = newrev;
							}

							url.RemoveAll();
							url = null;
							Thread.Sleep(RssConfig.QueryTime*1000);
						}
						else 
							Thread.Sleep(1000);
					}
					catch(Exception e)
					{
						Log.Error("SvnRss", sLocalization.Exception("Error"), _name, e.Message);
						Thread.Sleep(RssConfig.QueryTime*1000);
					}
				}
			}
			catch(Exception e)
			{
				Log.Error("SvnRss", sLocalization.Exception("Error2"), _name, e.Message);
				Update();
			}
		}

		private XmlDocument GetUrl()
		{
			try
			{
				if(_username != string.Empty && _password != string.Empty)
				{
					using(var client = new WebClient())
					{
						client.Credentials = new NetworkCredential(_username, _password);
						string xml = client.DownloadString(_url);
						var rss = new XmlDocument();
						rss.LoadXml(xml);
						client.Dispose();
						return rss;
					}
				}
				else
				{
					var rss = new XmlDocument();
					rss.Load(_url);
					return rss;
				}
			}
			catch(Exception e)
			{
				Log.Error("SvnRss", sLocalization.Exception("Error"), _name, e.Message);
			}

			return null;
		}

		private string Title(XmlDocument rss)
		{
			var title = rss.SelectSingleNode(_title);
			return title.IsNull() ? "no text" : title.InnerText;
		}

		private string Author(XmlDocument rss)
		{
			var author = rss.SelectSingleNode(_author);
			return author.IsNull() ? "no text" : author.InnerText;
		}

		private string Revision(XmlDocument rss)
		{
			if(_website == "assembla")
			{
				var title = rss.SelectSingleNode(_title);
				if(title.IsNull())
					return "no text";

				string rev = title.InnerText;
				info = rev.Split('[');

				if(info.Length < 2)
					return "no text";

				if(info[0] != "Changeset ")
					return "no text";

				string i = info[1];

				if(!i.Contains("]"))
					return "no text";

				return i.Substring(0, i.IndexOf("]"));
			}

			return "no text";
		}

		private void Informations(string rev, string title, string author)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Channel FROM svninfo WHERE Name = '{0}'", _name);
			if(!db.IsNull())
			{
				string[] channel = db["Channel"].ToString().Split(SchumixBase.Comma);

				foreach(var chan in channel)
				{
					string language = sLManager.GetChannelLocalization(chan);

					if(_website == "assembla")
					{
						if(title.Contains(SchumixBase.Colon.ToString()))
						{
							sSendMessage.SendCMPrivmsg(chan, sLocalization.SvnRss("assembla", language), _name, rev, author);
							sSendMessage.SendCMPrivmsg(chan, sLocalization.SvnRss("assembla2", language), _name, title.Substring(title.IndexOf(SchumixBase.Colon)+1));
						}
					}

					Thread.Sleep(1000);
				}
			}
		}
	}
}