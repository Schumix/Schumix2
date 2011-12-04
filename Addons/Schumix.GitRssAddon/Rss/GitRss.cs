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
using Schumix.GitRssAddon.Config;
using Schumix.GitRssAddon.Localization;

namespace Schumix.GitRssAddon
{
	public sealed class GitRss
	{
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly PLocalization sLocalization = Singleton<PLocalization>.Instance;
		private readonly ChannelInfo sChannelInfo = Singleton<ChannelInfo>.Instance;
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;
		private XmlNamespaceManager _ns;
		private Thread _thread;
		private readonly string _name;
		private readonly string _type;
		private readonly string _url;
		private readonly string _website;
		private string _oldrev;
		private string _id;
		private string _title;
		private string _author;
		private string _username;
		private string _password;
		public bool Started { get; private set; }

		public GitRss(string name, string type, string url, string website)
		{
			_name = name;
			_type = type;

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

			if(_username != string.Empty && _password != string.Empty)
			{
				using(var client = new WebClient())
				{
					client.Credentials = new NetworkCredential(_username, _password);
					string xml = client.DownloadString(_url);
					var rss = new XmlDocument();
					rss.LoadXml(xml);
					xml = string.Empty;
					_ns = new XmlNamespaceManager(rss.NameTable);
					_ns.AddNamespace("ga", "http://www.w3.org/2005/Atom");
				}
			}
			else
			{
				var rss = new XmlDocument();
				rss.Load(_url);
				_ns = new XmlNamespaceManager(rss.NameTable);
				_ns.AddNamespace("ga", "http://www.w3.org/2005/Atom");
			}
		}

		private void Init()
		{
			if(_website == "github")
			{
				_id = "ga:feed/ga:entry/ga:id";
				_title = "ga:feed/ga:entry/ga:title";
				_author = string.Empty;
			}
			else if(_website == "gitweb")
			{
				_id = "ga:feed/ga:entry/ga:id";
				_title = "ga:feed/ga:entry/ga:title";
				_author = "ga:feed/ga:entry/ga:author/ga:name";
			}
			else
			{
				_id = string.Empty;
				_title = string.Empty;
				_author = string.Empty;
			}
		}

		public string Name
		{
			get { return _name; }
		}

		public string Type
		{
			get { return _type; }
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
						if(sChannelInfo.FSelect(IFunctions.Git))
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

								if(_website != "github")
								{
									author = Author(url);
									if(author == "no text")
										continue;

									Informations(newrev, title, author);
								}
								else
									Informations(newrev, title, string.Empty);

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
						Log.Error("GitRss", sLocalization.Exception("Error"), _name, _type, e.Message);
						Thread.Sleep(RssConfig.QueryTime*1000);
					}
				}
			}
			catch(Exception e)
			{
				Log.Error("GitRss", sLocalization.Exception("Error2"), _name, _type, e.Message);
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
						xml = string.Empty;
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
				Log.Error("GitRss", sLocalization.Exception("Error"), _name, _type, e.Message);
			}

			return null;
		}

		private string Title(XmlDocument rss)
		{
			var title = rss.SelectSingleNode(_title, _ns);
			return title.IsNull() ? "no text" : title.InnerText;
		}

		private string Author(XmlDocument rss)
		{
			var author = rss.SelectSingleNode(_author, _ns);
			return author.IsNull() ? "no text" : author.InnerText;
		}

		private string Revision(XmlDocument rss)
		{
			if(_website == "github")
			{
				var id = rss.SelectSingleNode(_id, _ns);
				if(id.IsNull())
					return "no text";

				string rev = id.InnerText;

				if(!rev.Contains("Commit/"))
					return "no text";

				return rev.Substring(rev.IndexOf("Commit/") + "Commit/".Length);
			}
			else if(_website == "gitweb")
			{
				var id = rss.SelectSingleNode(_id, _ns);
				if(id.IsNull())
					return "no text";

				string rev = id.InnerText;

				if(!rev.Contains("a=commitdiff;h="))
					return "no text";

				return rev.Substring(rev.IndexOf("a=commitdiff;h=") + "a=commitdiff;h=".Length);
			}

			return "no text";
		}

		private void Informations(string rev, string title, string author)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Channel FROM gitinfo WHERE Name = '{0}' AND Type = '{1}'", _name, _type);
			if(!db.IsNull())
			{
				string[] channel = db["Channel"].ToString().Split(SchumixBase.Comma);

				foreach(var chan in channel)
				{
					string language = sLManager.GetChannelLocalization(chan);

					if(_website == "github")
					{
						sSendMessage.SendCMPrivmsg(chan, sLocalization.GitRss("github", language), _name, _type, rev.Substring(0, 10));
						sSendMessage.SendCMPrivmsg(chan, sLocalization.GitRss("github2", language), _name, title);
					}
					else if(_website == "gitweb")
					{
						sSendMessage.SendCMPrivmsg(chan, sLocalization.GitRss("gitweb", language), _name, _type, rev.Substring(0, 10), author);
						sSendMessage.SendCMPrivmsg(chan, sLocalization.GitRss("gitweb2", language), _name, title);
					}

					Thread.Sleep(1000);
				}
			}
		}
	}
}