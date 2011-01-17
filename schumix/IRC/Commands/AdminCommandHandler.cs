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
using Schumix.Config;

namespace Schumix.IRC.Commands
{
	public partial class CommandHandler
	{
		public void HandleHozzaferes()
		{
			if(!MessageHandler.CManager.Admin(Network.IMessage.Nick))
				return;

			if(Network.IMessage.Info.Length < 5)
				return;

			string jelszo = Network.IMessage.Info[4];
			string ip = Network.IMessage.Host;
			string admin_nev = Network.IMessage.Nick;
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

		public void HandleUjjelszo()
		{
			if(!MessageHandler.CManager.Admin(Network.IMessage.Nick))
				return;

			if(Network.IMessage.Info.Length < 6)
				return;

			string admin_nev = Network.IMessage.Nick;
			string jelszo = Network.IMessage.Info[4];
			string ujjelszo = Network.IMessage.Info[5];
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
				SchumixBot.mSQLConn.QueryFirstRow(String.Format("UPDATE adminok SET jelszo = '{0}' WHERE nev = '{1}'", ujjelszo, Nev));
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, Nev, String.Format("Jelszó sikereset meg lett változtatva erre: {0}", ujjelszo));
			}
			else
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, Nev, "A mostani jelszó nem egyezik, modósitás megtagadva");
		}

		public void HandleSzoba()
		{
			if(!MessageHandler.CManager.Admin(Network.IMessage.Nick, Network.IMessage.Host))
				return;

			if(Network.IMessage.Info.Length < 5)
				return;

			if(Network.IMessage.Info[4] == "help")
			{
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Segitség a konzol szoba váltásához!");
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("Funkció használata: {0}szoba <ide jön a szoba>", IRCConfig.Parancselojel));
			}
			else
				SchumixBot.mSQLConn.QueryFirstRow(String.Format("UPDATE schumix SET irc_cim = '{0}' WHERE entry = '1'", Network.IMessage.Info[4]));
		}

		public void HandleKikapcs()
		{
			if(!MessageHandler.CManager.Admin(Network.IMessage.Nick, Network.IMessage.Host))
				return;

			sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Viszlát :(");
			Network.writer.WriteLine("QUIT :{0} leallitott parancsal.", Network.IMessage.Nick);
			Environment.Exit(1);
		}
	}
}
