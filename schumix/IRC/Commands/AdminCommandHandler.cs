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

		public void HandleSzinek()
		{
			if(!MessageHandler.CManager.Admin(Network.IMessage.Nick, Network.IMessage.Host))
				return;

			sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "1teszt1 2teszt2 3teszt3 4teszt4 5teszt5 6teszt6 7teszt7 8teszt8");
			sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "9teszt9 10teszt10 11teszt11 12teszt12 13teszt13 14teszt14 15teszt15");
		}

		public void HandleNick()
		{
			if(!MessageHandler.CManager.Admin(Network.IMessage.Nick, Network.IMessage.Host))
				return;

			if(Network.IMessage.Info.Length < 5)
				return;

			string nick = Network.IMessage.Info[4];
			SchumixBot.NickTarolo = nick;
			Network.writer.WriteLine(String.Format("NICK {0}", nick));
		}

		public void HandleJoin()
		{
			if(!MessageHandler.CManager.Admin(Network.IMessage.Nick, Network.IMessage.Host))
				return;

			MessageHandler.ChannelPrivmsg = Network.IMessage.Channel;

			if(Network.IMessage.Info.Length == 5)
				Network.writer.WriteLine(String.Format("JOIN {0}", Network.IMessage.Info[4]));
			else if(Network.IMessage.Info.Length == 6)
				Network.writer.WriteLine(String.Format("JOIN {0} {1}", Network.IMessage.Info[4], Network.IMessage.Info[5]));
		}

		public void HandleLeft()
		{
			if(!MessageHandler.CManager.Admin(Network.IMessage.Nick, Network.IMessage.Host))
				return;

			if(Network.IMessage.Info.Length < 5)
				return;

			Network.writer.WriteLine(String.Format("PART {0}", Network.IMessage.Info[4]));
		}

		public void HandleKick()
		{
			if(!MessageHandler.CManager.Admin(Network.IMessage.Nick, Network.IMessage.Host))
				return;

			if(Network.IMessage.Info.Length < 5)
				return;

			string kick = Network.IMessage.Info[4].ToLower();
			int szam = Network.IMessage.Info.Length;
			string nick = SchumixBot.NickTarolo;

			if(szam == 5)
			{
				if(kick != nick.ToLower())
					Network.writer.WriteLine(String.Format("KICK {0} {1}", Network.IMessage.Channel, kick));
			}
			else if(szam >= 6)
			{
				string oka = "";
				for(int i = 5; i < Network.IMessage.Info.Length; i++)
					oka += Network.IMessage.Info[i] + " ";

				if(oka.Substring(0, 1) == ",")
					oka = oka.Remove(0, 1);

				if(kick != nick.ToLower())
					Network.writer.WriteLine(String.Format("KICK {0} {1} :{2}", Network.IMessage.Channel, kick, oka));
			}
		}

		public void HandleMode()
		{
			if(!MessageHandler.CManager.Admin(Network.IMessage.Nick, Network.IMessage.Host))
				return;

			if(Network.IMessage.Info.Length < 6)
				return;

			string rang = Network.IMessage.Info[4].ToLower();
			string tnick = SchumixBot.NickTarolo;
			string nev = "";

			for(int i = 5; i < Network.IMessage.Info.Length; i++)
				nev += Network.IMessage.Info[i] + " ";

			if(nev.Substring(0, 1) == " ")
				nev = nev.Remove(0, 1);

			nev.ToLower();
			bool nick = nev.StartsWith(tnick.ToLower());

			if(!nick)
				Network.writer.WriteLine(String.Format("MODE {0} {1} {2}", Network.IMessage.Channel, rang, nev));
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

		public void HandleTeszt()
		{
			if(!MessageHandler.CManager.Admin(Network.IMessage.Nick, Network.IMessage.Host))
				return;

			if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4] == "adat")
			{
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Teszt probálkozás");
			}
			else if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4] == "db")
			{
				var db = SchumixBot.mSQLConn.QueryRow(String.Format("SELECT nev FROM adminok"));
				for(int i = 0; i < db.Rows.Count; ++i)
				{
					var row = db.Rows[i];

					string admin = row["nev"].ToString();
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("{0}", admin));
				}
			}
			else
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("{0}", Network.IMessage.Info.Length));
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
