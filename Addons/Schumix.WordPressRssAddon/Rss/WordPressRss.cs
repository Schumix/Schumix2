/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Megax <http://www.megaxx.info/>
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
using System.Threading;
using Schumix.API;
using Schumix.Irc;
using Schumix.Framework;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using Schumix.WordPressRssAddon.Config;
using Schumix.WordPressRssAddon.Localization;

namespace Schumix.WordPressRssAddon
{
	sealed class WordPressRss
	{
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly PLocalization sLocalization = Singleton<PLocalization>.Instance;
		private readonly ChannelInfo sChannelInfo = Singleton<ChannelInfo>.Instance;
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;
		private XmlNamespaceManager _ns;
		private Thread _thread;
		private readonly string _name;
		private readonly string _url;
		private string _oldguid;
		private string _guid;
		private string _title;
		private string _author;
		private string _username;
		private string _password;
		public bool Started { get; private set; }

		public WordPressRss(string name, string url)
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

			Init();

			if(_username != string.Empty && _password != string.Empty)
			{
				using(var client = new WebClient())
				{
					client.Credentials = new NetworkCredential(_username, _password);
					client.Encoding = Encoding.UTF8;
					string xml = client.DownloadString(_url);
					var rss = new XmlDocument();
					rss.LoadXml(xml);
					xml = string.Empty;
					_ns = new XmlNamespaceManager(rss.NameTable);
					_ns.AddNamespace("dc", "http://purl.org/dc/elements/1.1/");
				}
			}
			else
			{
				using(var client = new WebClient())
				{
					client.Encoding = Encoding.UTF8;
					string xml = client.DownloadString(_url);
					var rss = new XmlDocument();
					rss.LoadXml(xml);
					xml = string.Empty;
					_ns = new XmlNamespaceManager(rss.NameTable);
					_ns.AddNamespace("dc", "http://purl.org/dc/elements/1.1/");
				}
			}
		}

		private void Init()
		{
			_guid = "rss/channel/item/guid";
			_title = "rss/channel/item/title";
			_author = "rss/channel/item/dc:creator";
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
				string newguid;
				string title;
				string author;

				url = GetUrl();
				if(!url.IsNull())
					_oldguid = Guid(url);

				while(true)
				{
					try
					{
						if(sChannelInfo.FSelect(IFunctions.Wordpress))
						{
							url = GetUrl();
							if(url.IsNull())
							{
								Thread.Sleep(RssConfig.QueryTime*1000);
								continue;
							}

							newguid = Guid(url);
							if(newguid == "no text")
							{
								Thread.Sleep(RssConfig.QueryTime*1000);
								continue;
							}

							if(_oldguid != newguid)
							{
								title = Title(url);
								if(title == "no text")
								{
									Thread.Sleep(RssConfig.QueryTime*1000);
									continue;
								}

								author = Author(url);
								if(author == "no text")
								{
									Thread.Sleep(RssConfig.QueryTime*1000);
									continue;
								}

								Informations(newguid, title, author);
								_oldguid = newguid;
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
						Log.Error("WordPressRss", sLocalization.Exception("Error"), _name, e.Message);
						Thread.Sleep(RssConfig.QueryTime*1000);
					}
				}
			}
			catch(Exception e)
			{
				Log.Error("WordPressRss", sLocalization.Exception("Error2"), _name, e.Message);
				Thread.Sleep(RssConfig.QueryTime*1000);

				if(e.Message != "Thread was being aborted")
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
						client.Encoding = Encoding.UTF8;
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
					using(var client = new WebClient())
					{
						client.Encoding = Encoding.UTF8;
						string xml = client.DownloadString(_url);
						var rss = new XmlDocument();
						rss.LoadXml(xml);
						xml = string.Empty;
						return rss;
					}
				}
			}
			catch(Exception e)
			{
				Log.Error("WordPressRss", sLocalization.Exception("Error"), _name, e.Message);
				Thread.Sleep(RssConfig.QueryTime*1000);
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
			var author = rss.SelectSingleNode(_author, _ns);
			return author.IsNull() ? "no text" : author.InnerText;
		}

		private string Guid(XmlDocument rss)
		{
			var guid = rss.SelectSingleNode(_guid);
			return guid.IsNull() ? "no text" : guid.InnerText;
		}

		private void Informations(string guid, string title, string author)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Channel FROM wordpressinfo WHERE Name = '{0}'", _name);
			if(!db.IsNull())
			{
				string[] channel = db["Channel"].ToString().Split(SchumixBase.Comma);

				foreach(var chan in channel)
				{
					string language = sLManager.GetChannelLocalization(chan);
					sSendMessage.SendCMPrivmsg(chan, sLocalization.WordPressRss("WordPress", language), _name, author, guid);
					sSendMessage.SendCMPrivmsg(chan, sLocalization.WordPressRss("WordPress2", language), _name, title);

					Thread.Sleep(1000);
				}
			}
		}
	}
}