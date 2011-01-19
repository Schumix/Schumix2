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
using Schumix.Config;

namespace Schumix.IRC.Commands
{
	public partial class CommandHandler
	{
		public void HandleTeszt()
		{
			if(!MessageHandler.CManager.Admin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Administrator))
				return;

			MessageHandler.CNick();

			if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4] == "adat")
			{
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Teszt probálkozás");
			}
			else if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4] == "db")
			{
				var db = SchumixBot.mSQLConn.QueryRow(String.Format("SELECT nev FROM adminok"));
				if(db != null)
				{
					for(int i = 0; i < db.Rows.Count; ++i)
					{
						var row = db.Rows[i];
						string admin = row["nev"].ToString();
						sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("{0}", admin));
					}
				}
				else
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Hibás lekérdezés!");
			}
			else if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4] == "vhost")
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, Network.IMessage.Host);
			else
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("{0}", Network.IMessage.Info.Length));
		}

		public void HandleKikapcs()
		{
			if(!MessageHandler.CManager.Admin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Administrator))
				return;

			MessageHandler.CNick();
			sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Viszlát :(");
			Network.writer.WriteLine("QUIT :{0} leallitott parancsal.", Network.IMessage.Nick);
			Environment.Exit(1);
		}
	}
}
