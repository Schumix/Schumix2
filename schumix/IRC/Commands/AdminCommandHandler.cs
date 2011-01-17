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

// nincs egyenlőre adat

using System;

namespace Schumix.IRC.Commands
{
	public partial class CommandHandler
	{
		public void HandleHozzaferes()
		{
			if(!MessageHandler.CManager.Admin(MessageHandler.CManager.CMessage.Nick))
				return;

			if(MessageHandler.CManager.CMessage.Info.Length < 5)
				return;

			string jelszo = MessageHandler.CManager.CMessage.Info[4];
			string ip = MessageHandler.CManager.CMessage.Host;
			string admin_nev = MessageHandler.CManager.CMessage.Nick;
			string Nev = "";
			string JelszoSql = "";

			var db = SchumixBot.mSQLConn.QueryFirstRow(String.Format("SELECT nev, jelszo FROM adminok WHERE nev = '{0}'", admin_nev));
			if(db != null)
			{
				Nev = db["nev"].ToString();
				JelszoSql = db["jelszo"].ToString();
			}

			if(JelszoSql == jelszo)
			{
				SchumixBot.mSQLConn.QueryFirstRow(String.Format("UPDATE adminok SET ip = '{0}' WHERE nev = '{1}'", ip, Nev));
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, Nev, "Hozzáférés engedélyezve");
			}
			else
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, Nev, "Hozzáférés megtagadva");
		}
	}
}
