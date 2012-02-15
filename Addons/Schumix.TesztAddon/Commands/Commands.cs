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
using System.Data;
using Schumix.API;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.TesztAddon.Commands
{
	class TesztCommand : CommandInfo
	{
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;

		protected void HandleTest(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Administrator))
				return;

			if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "adat")
			{
				sSendMessage.SendChatMessage(sIRCMessage, "Teszt probálkozás");
			}
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "db")
			{
				var db = SchumixBase.DManager.Query("SELECT Name FROM admins");
				if(!db.IsNull())
				{
					foreach(DataRow row in db.Rows)
					{
						string admin = row["Name"].ToString();
						sSendMessage.SendChatMessage(sIRCMessage, "{0}", admin);
					}
				}
				else
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel));
			}
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "rss")
			{
				var rss = new XmlDocument();
				rss.Load("http://www.assembla.com/spaces/Sandshroud/stream.rss");
				//rss.Load("http://github.com/megax/Schumix2/commits/master.atom");

				var node = rss.SelectSingleNode("rss/channel/item/title");
				//var node = rss.SelectSingleNode("feed/title");
				string title = !node.IsNull() ? node.InnerText : string.Empty;
				sSendMessage.SendChatMessage(sIRCMessage, title);
			}
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "vhost")
				sSendMessage.SendChatMessage(sIRCMessage, sIRCMessage.Host);
			/*else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "amsg")
				sSendMessage.SendCMAmsg("teszt");*/
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "me")
				sSendMessage.SendCMAction(sIRCMessage.Channel, sIRCMessage.Info.SplitToString(5, SchumixBase.Space));
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "ctcprequest")
				sSendMessage.SendCMCtcpRequest(sIRCMessage.Channel, sIRCMessage.Info.SplitToString(5, SchumixBase.Space));
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "ctcpreply")
				sSendMessage.SendCMCtcpReply(sIRCMessage.Channel, sIRCMessage.Info.SplitToString(5, SchumixBase.Space));
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "text")
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
			else
				sSendMessage.SendChatMessage(sIRCMessage, "{0}", sIRCMessage.Info.Length);
		}
	}
}
