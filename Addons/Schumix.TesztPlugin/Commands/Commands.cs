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
//using System.IO;
//using System.Collections.Generic;
//using System.Text;
using System.Xml;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;
/*using Atom.Core;
using Atom.Utils;
using Atom.AdditionalElements;
using Atom.Core.Collections;*/

namespace Schumix.TesztPlugin.Commands
{
	public class TesztCommand : CommandInfo
	{
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;
		//private readonly Utility sUtility = Singleton<Utility>.Instance;

        protected void Teszt()
        {
			if(!Admin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Administrator))
				return;

			CNick();

			if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4] == "adat")
			{
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Teszt probálkozás");
			}
			else if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4] == "db")
			{
				var db = SchumixBase.DManager.Query("SELECT nev FROM adminok");
				if(db != null)
				{
					for(int i = 0; i < db.Rows.Count; ++i)
					{
						var row = db.Rows[i];
						string admin = row["nev"].ToString();
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "{0}", admin);
					}
				}
				else
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Hibás lekérdezés!");
			}
			else if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4] == "rss")
			{
				//string url = sUtility.GetUrl("https://github.com/megax/Schumix2/commits/master.atom");
				/*var feed = AtomFeed.Load(new Uri("https://github.com/megax/Schumix2/commits/master.atom"));

				var entry = feed.Entries[0]; // the first entry.

				var tm = entry.Links[0].HRef.ToString().Split('/');
				var hash = tm[(tm.Length-1)];*/
				/*XmlDocument RSSXml = new XmlDocument();
				RSSXml.Load("https://github.com/megax/Schumix2/commits/master.atom");
				XmlNodeList RSSNodeList = RSSXml.SelectNodes("feed");
				XmlNode RSSDesc = RSSXml.SelectSingleNode("feed");

				XmlNode RSSSubNode;
				RSSSubNode = RSSXml.SelectSingleNode("feed/title");
				string title = RSSSubNode != null ? RSSSubNode.InnerText : "";*/
				//sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, hash);
			}
			else if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4] == "vhost")
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, Network.IMessage.Host);
			else
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "{0}", Network.IMessage.Info.Length);
		}
	}
}
