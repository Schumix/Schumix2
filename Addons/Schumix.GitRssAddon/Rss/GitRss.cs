/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013 Schumix Team <http://schumix.eu/>
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
using Schumix.Framework.Bitly;
using Schumix.Framework.Logger;
using Schumix.Framework.Functions;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using Schumix.GitRssAddon.Config;
using Schumix.GitRssAddon.Localization;

namespace Schumix.GitRssAddon
{
	sealed class GitRss
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
		private readonly string _type;
		private readonly string _url;
		private readonly string _website;
		private string _oldrev;
		private string _id;
		private string _title;
		private string _author;
		private string _link;
		private string _username;
		private string _password;
		private string _servername;
		public bool Started { get; private set; }

		public GitRss(string ServerName, string name, string type, string url, string website)
		{
			_servername = ServerName;
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
					rss.LoadXml(DownloadToXml(sUtilities.DownloadString(_url, "</entry>", _credential)));
					_ns = new XmlNamespaceManager(rss.NameTable);
					_ns.AddNamespace("ga", "http://www.w3.org/2005/Atom");
				}
				else
				{
					var rss = new XmlDocument();
					rss.LoadXml(DownloadToXml(sUtilities.DownloadString(_url, "</entry>")));
					_ns = new XmlNamespaceManager(rss.NameTable);
					_ns.AddNamespace("ga", "http://www.w3.org/2005/Atom");
				}
			}
			catch(Exception e)
			{
				Log.Error("GitRss", sLConsole.GetString("[{0} {1}] Failure details: {2}"), _name, _type, e.Message);
			}
		}

		private void Init()
		{
			if(_website == "github")
			{
				_id = "ga:feed/ga:entry/ga:id";
				_link = "ga:feed/ga:entry/ga:link";
				_title = "ga:feed/ga:entry/ga:title";
				_author = "ga:feed/ga:entry/ga:author/ga:name";
			}
			else if(_website == "gitweb")
			{
				_id = "ga:feed/ga:entry/ga:id";
				_link = "ga:feed/ga:entry/ga:link";
				_title = "ga:feed/ga:entry/ga:title";
				_author = "ga:feed/ga:entry/ga:author/ga:name";
			}
			else
			{
				_id = string.Empty;
				_link = string.Empty;
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
					_oldrev = Revision(url);

				while(true)
				{
					try
					{
						if(!sIrcBase.Networks[_servername].sMyChannelInfo.IsNull() && sIrcBase.Networks[_servername].sMyChannelInfo.FSelect(IFunctions.Git) && errornumber < 20)
						{
							url = GetUrl();
							if(url.IsNull())
							{
								Clean(url);
								Thread.Sleep(RssConfig.QueryTime*1000);
								continue;
							}

							string newrev = Revision(url);
							if(newrev == "no text")
							{
								Clean(url);
								Thread.Sleep(RssConfig.QueryTime*1000);
								continue;
							}

							if(_oldrev != newrev)
							{
								string title = Title(url);
								if(title == "no text")
								{
									Clean(url);
									Thread.Sleep(RssConfig.QueryTime*1000);
									continue;
								}

								string author = Author(url);
								if(author == "no text" && _website != "github")
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

								Informations(newrev, title, author, curl);
								_oldrev = newrev;
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
						Log.Error("GitRss", sLConsole.GetString("[{0} {1}] Failure details: {2}"), _name, _type, e.Message);
						Thread.Sleep(RssConfig.QueryTime*1000);
					}
				}
			}
			catch(Exception e)
			{
				if(SchumixBase.ExitStatus)
					return;

				errornumber++;
				Log.Error("GitRss", sLConsole.GetString("[{0} {1}] Fatal failure details: {2}"), _name, _type, e.Message);
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
					rss.LoadXml(DownloadToXml(sUtilities.DownloadString(_url, "</entry>", _credential)));
					return rss;
				}
				else
				{
					var rss = new XmlDocument();
					rss.LoadXml(DownloadToXml(sUtilities.DownloadString(_url, "</entry>")));
					return rss;
				}
			}
			catch(Exception e)
			{
				Log.Error("GitRss", sLConsole.GetString("[{0} {1}] Failure details: {2}"), _name, _type, e.Message);
				Thread.Sleep(RssConfig.QueryTime*1000);
			}

			return null;
		}

		private string DownloadToXml(string data)
		{
			if(data.IsNullOrEmpty())
				return string.Empty;

			data = data.Substring(0, data.IndexOf("</entry>") + "</entry>".Length);
			data += "</feed>";
			return data;
		}

		private string Title(XmlDocument rss)
		{
			var title = rss.SelectSingleNode(_title, _ns);
			return title.IsNull() ? "no text" : (title.InnerText.StartsWith(SchumixBase.NewLine.ToString()) ? title.InnerText.Substring(2).TrimStart() : title.InnerText);
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

				if(!rev.Contains("commitdiff/"))
					return "no text";

				return rev.Substring(rev.IndexOf("commitdiff/") + "commitdiff/".Length);
			}

			return "no text";
		}

		private string CommitUrl(XmlDocument rss)
		{
			var curl = rss.SelectSingleNode(_link, _ns);
			return curl.IsNull() ? "no text" : (((XmlElement)curl).HasAttribute("href") ? ((XmlElement)curl).GetAttribute("href") : "no text");
		}

		private void Informations(string rev, string title, string author, string commiturl)
		{
			if(!sIrcBase.Networks[_servername].Online)
				return;

			var db = SchumixBase.DManager.QueryFirstRow("SELECT Channel, ShortUrl, Colors FROM gitinfo WHERE Name = '{0}' AND Type = '{1}' And ServerName = '{2}'", _name, _type, _servername);
			if(!db.IsNull())
			{
				string[] channel = db["Channel"].ToString().Split(SchumixBase.Comma);

				foreach(var chan in channel)
				{
					string language = sLManager.GetChannelLocalization(chan, _servername);

					if(Convert.ToBoolean(db["ShortUrl"].ToString()))
						commiturl = BitlyApi.ShortenUrl(commiturl).ShortUrl;

					if(Convert.ToBoolean(db["Colors"].ToString()))
					{
						if(_website == "github")
						{
							sIrcBase.Networks[_servername].sSendMessage.SendCMPrivmsg(chan, sLocalization.GitRss("github", language), _name, (author == "no text" ? "?" : author), _type, commiturl);
							sIrcBase.Networks[_servername].sSendMessage.SendCMPrivmsg(chan, sLocalization.GitRss("github2", language), _name, _type, rev.Substring(0, 10), (author == "no text" ? "?" : author), title);
						}
						else if(_website == "gitweb")
						{
							sIrcBase.Networks[_servername].sSendMessage.SendCMPrivmsg(chan, sLocalization.GitRss("gitweb", language), _name, (author == "no text" ? "?" : author), _type, commiturl);
							sIrcBase.Networks[_servername].sSendMessage.SendCMPrivmsg(chan, sLocalization.GitRss("gitweb2", language), _name, _type, rev.Substring(0, 10), (author == "no text" ? "?" : author), title);
						}
					}
					else
					{
						if(_website == "github")
						{
							sIrcBase.Networks[_servername].sSendMessage.SendCMPrivmsg(chan, sLocalization.GitRss("nocolorsgithub", language), _name, (author == "no text" ? "?" : author), _type, commiturl);
							sIrcBase.Networks[_servername].sSendMessage.SendCMPrivmsg(chan, sLocalization.GitRss("nocolorsgithub2", language), _name, _type, rev.Substring(0, 10), (author == "no text" ? "?" : author), title);
						}
						else if(_website == "gitweb")
						{
							sIrcBase.Networks[_servername].sSendMessage.SendCMPrivmsg(chan, sLocalization.GitRss("nocolorsgitweb", language), _name, (author == "no text" ? "?" : author), _type, commiturl);
							sIrcBase.Networks[_servername].sSendMessage.SendCMPrivmsg(chan, sLocalization.GitRss("nocolorsgitweb2", language), _name, _type, rev.Substring(0, 10), (author == "no text" ? "?" : author), title);
						}
					}

					Thread.Sleep(1000);
				}
			}
		}
	}
}