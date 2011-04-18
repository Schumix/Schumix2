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
using System.Threading;
using Schumix.Irc;
using Schumix.Framework;
using Schumix.Framework.Extensions;
using Schumix.HgRssAddon.Config;

namespace Schumix.HgRssAddon
{
	public sealed class HgRss
	{
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;
		private Thread _thread;
		private readonly string _name;
		private readonly string _url;
		private readonly string _website;
		private string _oldrev;
		private string _id;
		private string _title;
		private string _author;

		public HgRss(string name, string url, string website)
		{
			_name = name;
			_url = url;
			_website = website;
			Init();
		}

		private void Init()
		{
			if(_website == "google")
			{
				_id = "feed/entry/id";
				_title = "feed/entry/title";
				_author = "feed/entry/author/name";
			}
			else if(_website == "bitbucket")
			{
				_id = "rss/channel/item/link";
				_title = "rss/channel/item/title";
				_author = "rss/channel/item/author";
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

		public void Start()
		{
			_thread = new Thread(Update);
			_thread.Start();
		}

		public void Stop()
		{
			_thread.Abort();
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
					if(Network.sChannelInfo.FSelect("hg"))
					{
						url = GetUrl();
						if(url.IsNull())
							continue;

						newrev = Revision(url);
						if(newrev == "nincs adat")
							continue;

						if(_oldrev != newrev)
						{
							title = Title(url);
							if(title == "nincs adat")
								continue;

							author = Author(url);
							if(author == "nincs adat")
								continue;

							Informations(newrev, title, author);
							_oldrev = newrev;
						}

						Thread.Sleep(RssConfig.QueryTime*1000);
					}
					else 
						Thread.Sleep(1000);
				}
			}
			catch(Exception e)
			{
				Log.Error("HgRss", "[{0}] Hiba oka: {1}", _name, e.Message);
				Update();
			}
		}

		private XmlDocument GetUrl()
		{
			try
			{
				var rss = new XmlDocument();
				rss.Load(_url);
				return rss;
			}
			catch(Exception e)
			{
				Log.Error("HgRss", "[{0}] Hiba oka: {1}", _name, e.Message);
			}

			return null;
		}

		private string Title(XmlDocument rss)
		{
			var title = rss.SelectSingleNode(_title);
			if(title.IsNull())
				return "nincs adat";
			else
				return title.InnerText;
		}

		private string Author(XmlDocument rss)
		{
			var author = rss.SelectSingleNode(_author);
			if(author.IsNull())
				return "nincs adat";
			else
				return author.InnerText;
		}

		private string Revision(XmlDocument rss)
		{
			if(_website == "google")
			{
				var id = rss.SelectSingleNode(_id);
				if(id.IsNull())
					return "nincs adat";

				string rev = id.InnerText;

				if(rev.IndexOf("hgchanges/basic/") == -1)
					return "nincs adat";

				return rev.Substring(rev.IndexOf("hgchanges/basic/")+1);
			}
			else if(_website == "bitbucket")
			{
				var id = rss.SelectSingleNode(_id);
				if(id.IsNull())
					return "nincs adat";

				string rev = id.InnerText;

				if(rev.IndexOf("changeset/") == -1)
					return "nincs adat";

				return rev.Substring(rev.IndexOf("changeset/")+1);
			}

			return "nincs adat";
		}

		private void Informations(string rev, string title, string author)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Channel FROM hginfo WHERE Name = '{0}'", _name);
			if(!db.IsNull())
			{
				string[] csatorna = db["Channel"].ToString().Split(',');

				if(csatorna.Length < 1)
					return;

				for(int x = 0; x < csatorna.Length; x++)
				{
					if(_website == "google")
					{
						if(title.IndexOf(":") != -1)
						{
							string kiiras = title.Substring(title.IndexOf(":")+1);
							sSendMessage.SendCMPrivmsg(csatorna[x], "3{0} Revision: 10{1} by {2}", _name, rev.Substring(0, 10), author);
							sSendMessage.SendCMPrivmsg(csatorna[x], "3{0} Info:{1}", _name, kiiras);
						}
					}
					else if(_website == "bitbucket")
					{
						sSendMessage.SendCMPrivmsg(csatorna[x], "3{0} Revision: 10{1} by {2}", _name, rev.Substring(0, 10), author);
						sSendMessage.SendCMPrivmsg(csatorna[x], "3{0} Info:{1}", _name, title);
					}

					Thread.Sleep(1000);
				}
			}
		}
	}
}