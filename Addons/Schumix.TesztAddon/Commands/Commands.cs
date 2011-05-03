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
using System.Data;
using Schumix.API;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Extensions;

namespace Schumix.TesztAddon.Commands
{
	public class TesztCommand : CommandInfo
	{
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;

		protected void Teszt(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Administrator))
				return;

			CNick(sIRCMessage);

			if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "adat")
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Teszt probálkozás");
			}
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "db")
			{
				var db = SchumixBase.DManager.Query("SELECT Name FROM adminok");
				if(!db.IsNull())
				{
					foreach(DataRow row in db.Rows)
					{
						string admin = row["Name"].ToString();
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "{0}", admin);
					}
				}
				else
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Hibás lekérdezés!");
			}
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "rss")
			{
				var rss = new XmlDocument();
				rss.Load("http://www.assembla.com/spaces/Sandshroud/stream.rss");
				//rss.Load("http://github.com/megax/Schumix2/commits/master.atom");

				var node = rss.SelectSingleNode("rss/channel/item/title");
				//var node = rss.SelectSingleNode("feed/title");
				string title = !node.IsNull() ? node.InnerText : string.Empty;
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, title);
			}
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "vhost")
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sIRCMessage.Host);
			/*else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "amsg")
				sSendMessage.SendCMAmsg("teszt");
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "me")
				sSendMessage.SendCMMe("teszt");*/
			else
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "{0}", sIRCMessage.Info.Length);
		}
	}
}
