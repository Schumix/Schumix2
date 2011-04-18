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
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Extensions;

namespace Schumix.TesztAddon.Commands
{
	public class TesztCommand : CommandInfo
	{
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;

		protected void Teszt()
		{
			if(!IsAdmin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Administrator))
				return;

			CNick();

			if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4].ToLower() == "adat")
			{
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Teszt probálkozás");
			}
			else if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4].ToLower() == "db")
			{
				var db = SchumixBase.DManager.Query("SELECT Name FROM adminok");
				if(!db.IsNull())
				{
					for(int i = 0; i < db.Rows.Count; ++i)
					{
						var row = db.Rows[i];
						string admin = row["Name"].ToString();
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "{0}", admin);
					}
				}
				else
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Hibás lekérdezés!");
			}
			else if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4].ToLower() == "rss")
			{
				var rss = new XmlDocument();
				rss.Load("http://www.assembla.com/spaces/Sandshroud/stream.rss");
				//rss.Load("http://github.com/megax/Schumix2/commits/master.atom");

				var node = rss.SelectSingleNode("rss/channel/item/title");
				//var node = rss.SelectSingleNode("feed/title");
				string title = !node.IsNull() ? node.InnerText : string.Empty;
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, title);
			}
			else if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4].ToLower() == "vhost")
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, Network.IMessage.Host);
			else
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "{0}", Network.IMessage.Info.Length);
		}
	}
}
