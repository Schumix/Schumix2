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
		public void HandleAdmin()
		{
			if(!MessageHandler.CManager.Admin(Network.IMessage.Nick))
				return;

			MessageHandler.CNick();
			bool allapot = true;

			if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4] == "hozzaferes")
			{
				if(Network.IMessage.Info.Length < 6)
					return;

				string nev = Network.IMessage.Nick;
				var db = SchumixBot.mSQLConn.QueryFirstRow("SELECT jelszo FROM adminok WHERE nev = '{0}'", nev.ToLower());
				if(db != null)
				{
					string JelszoSql = db["jelszo"].ToString();

					if(JelszoSql == sUtility.Sha1(Network.IMessage.Info[5]))
					{
						SchumixBot.mSQLConn.QueryFirstRow("UPDATE adminok SET vhost = '{0}' WHERE nev = '{1}'", Network.IMessage.Host, nev.ToLower());
						sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Hozzáférés engedélyezve");
					}
					else
						sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Hozzáférés megtagadva");
				}

				allapot = false;
			}
			else if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4] == "ujjelszo")
			{
				if(Network.IMessage.Info.Length < 7)
					return;

				string nev = Network.IMessage.Nick;
				var db = SchumixBot.mSQLConn.QueryFirstRow("SELECT nev, jelszo FROM adminok WHERE nev = '{0}'", nev.ToLower());
				if(db != null)
				{
					string JelszoSql = db["jelszo"].ToString();

					if(JelszoSql == sUtility.Sha1(Network.IMessage.Info[5]))
					{
						SchumixBot.mSQLConn.QueryFirstRow("UPDATE adminok SET jelszo = '{0}' WHERE nev = '{1}'", sUtility.Sha1(Network.IMessage.Info[6]), nev.ToLower());
						sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Jelszó sikereset meg lett változtatva erre: {0}", Network.IMessage.Info[6]);
					}
					else
						sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "A mostani jelszó nem egyezik, modósitás megtagadva");
				}

				allapot = false;
			}

			if(!MessageHandler.CManager.Admin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Operator))
				return;

			if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4] == "help")
			{
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Alparancsok használata:");
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Admin lista: {0}admin lista", IRCConfig.Parancselojel);
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Hozzáadás: {0}admin add <admin neve>", IRCConfig.Parancselojel);
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Eltávolítás: {0}admin del <admin neve>", IRCConfig.Parancselojel);
			}
			else if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4] == "info")
			{
				int flag;
				string nev = Network.IMessage.Nick;

				var db = SchumixBot.mSQLConn.QueryFirstRow("SELECT flag FROM adminok WHERE nev = '{0}'", nev.ToLower());
				if(db != null)
					flag = Convert.ToInt32(db["flag"].ToString());
				else
					flag = -1;

				if((AdminFlag)flag == AdminFlag.Operator)
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Jelenleg Operátor vagy.");
				else if((AdminFlag)flag == AdminFlag.Administrator)
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Jelenleg Adminisztrátor vagy.");
			}
			else if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4] == "lista")
			{
				var db = SchumixBot.mSQLConn.QueryRow("SELECT nev FROM adminok");
				if(db != null)
				{
					string adminok = "";

					for(int i = 0; i < db.Rows.Count; ++i)
					{
						var row = db.Rows[i];
						string nev = row["nev"].ToString();
						adminok += ", " + nev;
					}

					if(adminok.Substring(0, 2) == ", ")
						adminok = adminok.Remove(0, 2);

					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "2Adminok: {0}", adminok);
				}
				else
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Hibas lekerdezes!");
			}
			else if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4] == "add")
			{
				if(Network.IMessage.Info.Length < 6)
					return;

				string nev = Network.IMessage.Info[5];
				string pass = sUtility.GetRandomString();

				SchumixBot.mSQLConn.QueryFirstRow("INSERT INTO `adminok`(nev, jelszo) VALUES ('{0}', '{1}')", nev.ToLower(), sUtility.Sha1(pass));

				sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Admin hozzáadva: {0}", nev);
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, nev, "Mostantól Schumix adminja vagy. A te mostani jelszavad: {0}", pass);
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, nev, "Ha megszeretnéd változtatni használd az {0}admin ujjelszo parancsot. Használata: {0}admin ujjelszo <régi> <új>", IRCConfig.Parancselojel);
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, nev, "Admin nick élesítése: {0}admin hozzaferes <jelszó>", IRCConfig.Parancselojel);
			}
			else if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4] == "del")
			{
				if(Network.IMessage.Info.Length < 6)
					return;

				string nev = Network.IMessage.Info[5];
				SchumixBot.mSQLConn.QueryFirstRow("DELETE FROM `adminok` WHERE nev = '{0}'", nev.ToLower());
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Admin törölve: {0}", nev);
			}
			else if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4] == "rang")
			{
				if(Network.IMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Nincs név megadva!");
					return;
				}

				if(Network.IMessage.Info.Length < 7)
				{
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Nincs rang megadva!");
					return;
				}

				string nev = Network.IMessage.Info[5].ToLower();
				if(MessageHandler.CManager.Admin(Network.IMessage.Nick, AdminFlag.Operator) && MessageHandler.CManager.Admin(nev, AdminFlag.Administrator))
				{
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Nem vagy Adminisztrátor!");
					return;
				}

				int rang = Convert.ToInt32(Network.IMessage.Info[6]);
		
				if(MessageHandler.CManager.Admin(Network.IMessage.Nick, AdminFlag.Operator) && MessageHandler.CManager.Admin(nev, AdminFlag.Operator) && (AdminFlag)rang == AdminFlag.Administrator)
				{
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Nem vagy Adminisztrátor!");
					return;
				}
		
				if((AdminFlag)rang == AdminFlag.Administrator || (AdminFlag)rang == AdminFlag.Operator)
				{
					SchumixBot.mSQLConn.QueryFirstRow("UPDATE adminok SET flag = '{0}' WHERE nev = '{1}'", rang, nev);
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Rang sikeresen modósitva.");
				}
				else
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Hibás rang!");
			}
			else
			{
				if(!allapot)
					return;

				if(MessageHandler.CManager.Admin(Network.IMessage.Nick, AdminFlag.Operator))
				{
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "3Parancsok: {0}nick | {0}join | {0}left | {0}kick | {0}mode", IRCConfig.Parancselojel);
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "3Parancsok: {0}szinek | {0}funkcio | {0}sznap | {0}channel", IRCConfig.Parancselojel);
				}
				else if(MessageHandler.CManager.Admin(Network.IMessage.Nick, AdminFlag.Administrator))
				{
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "3Parancsok: {0}nick | {0}join | {0}left | {0}kick | {0}mode", IRCConfig.Parancselojel);
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "3Parancsok: {0}szinek | {0}funkcio | {0}sznap | {0}channel | {0}kikapcs", IRCConfig.Parancselojel);
				}
			}
		}

		public void HandleFunkcio()
		{
			if(!MessageHandler.CManager.Admin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Operator))
				return;

			if(Network.IMessage.Info.Length < 5)
				return;

			MessageHandler.CNick();

			if(Network.IMessage.Info[4] == "help")
			{
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Alparancsok használata:");
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Channel kezelés: {0}funkcio <be vagy ki> <funkcio név>", IRCConfig.Parancselojel);
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Channel kezelés máshonnét: {0}funkcio channel <channel név> <be vagy ki> <funkcio név>", IRCConfig.Parancselojel);
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Együtes kezelés: {0}funkcio all <be vagy ki> <funkcio név>", IRCConfig.Parancselojel);
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Update kezelése: {0}funkcio update", IRCConfig.Parancselojel);
			}
			else if(Network.IMessage.Info[4] == "info")
			{
				string[] ChannelInfo = MessageHandler.ChannelFunkciokInfo(Network.IMessage.Channel).Split('|');
				if(ChannelInfo.Length < 2)
					return;
			
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "2Bekapcsolva: {0}", ChannelInfo[0]);
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "2Kikapcsolva: {0}", ChannelInfo[1]);
			}
			else if(Network.IMessage.Info[4] == "all")
			{
				if(Network.IMessage.Info.Length < 6)
					return;

				if(Network.IMessage.Info[5] == "info")
				{
					string f = MessageHandler.FunkciokInfo();
					if(f == "Hibás lekérdezés!")
					{
						sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Hibás lekérdezés!");
						return;
					}

					string[] FunkcioInfo = f.Split('|');
					if(FunkcioInfo.Length < 2)
						return;

					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "2Bekapcsolva: {0}", FunkcioInfo[0]);
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "2Kikapcsolva: {0}", FunkcioInfo[1]);
				}
				else
				{
					if(Network.IMessage.Info.Length < 7)
						return;

					if(Network.IMessage.Info[5] == "be" || Network.IMessage.Info[5] == "ki")
					{
						if(Network.IMessage.Info.Length >= 8)
						{
							string alomany = "";
	
							for(int i = 6; i < Network.IMessage.Info.Length; i++)
							{
								alomany += ", " + Network.IMessage.Info[i];
								SchumixBot.mSQLConn.QueryFirstRow("UPDATE schumix SET funkcio_status = '{0}' WHERE funkcio_nev = '{1}'", Network.IMessage.Info[5], Network.IMessage.Info[i]);
							}
	
							if(alomany.Substring(0, 2) == ", ")
								alomany = alomany.Remove(0, 2);
	
							sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "{0}: {1}kapcsolva",  alomany, Network.IMessage.Info[5]);
						}
						else
						{
							sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "{0}: {1}kapcsolva", Network.IMessage.Info[6], Network.IMessage.Info[5]);
							SchumixBot.mSQLConn.QueryFirstRow("UPDATE schumix SET funkcio_status = '{0}' WHERE funkcio_nev = '{1}'", Network.IMessage.Info[5], Network.IMessage.Info[6]);
						}
					}
				}
			}
			else if(Network.IMessage.Info[4] == "channel")
			{
				if(Network.IMessage.Info.Length < 7)
					return;
			
				string channelinfo = Network.IMessage.Info[5];
				string status = Network.IMessage.Info[6];
			
				if(Network.IMessage.Info[6] == "info")
				{
					string[] ChannelInfo = MessageHandler.ChannelFunkciokInfo(channelinfo).Split('|');
					if(ChannelInfo.Length < 2)
						return;

					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "2Bekapcsolva: {0}", ChannelInfo[0]);
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "2Kikapcsolva: {0}", ChannelInfo[1]);
				}
				else if(status == "be" || status == "ki")
				{
					if(Network.IMessage.Info.Length < 8)
						return;

					if(Network.IMessage.Info.Length >= 9)
					{
						string alomany = "";

						for(int i = 7; i < Network.IMessage.Info.Length; i++)
						{
							alomany += ", " + Network.IMessage.Info[i];
							SchumixBot.mSQLConn.QueryFirstRow("UPDATE channel SET funkciok = '{0}' WHERE szoba = '{1}'", MessageHandler.ChannelFunkciok(Network.IMessage.Info[i], status, channelinfo), channelinfo);
							MessageHandler.ChannelFunkcioReload();
						}

						if(alomany.Substring(0, 2) == ", ")
							alomany = alomany.Remove(0, 2);

						sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "{0}: {1}kapcsolva",  alomany, status);
					}
					else
					{
						sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "{0}: {1}kapcsolva", Network.IMessage.Info[7], status);
						SchumixBot.mSQLConn.QueryFirstRow("UPDATE channel SET funkciok = '{0}' WHERE szoba = '{1}'", MessageHandler.ChannelFunkciok(Network.IMessage.Info[7], status, channelinfo), channelinfo);
						MessageHandler.ChannelFunkcioReload();
					}
				}
			}
			else if(Network.IMessage.Info[4] == "update")
			{
				if(Network.IMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Sikeresen frissitve {0} channel funkciók.", Network.IMessage.Channel);
					SchumixBot.mSQLConn.QueryFirstRow("UPDATE channel SET funkciok = ',koszones:be,log:be,rejoin:be,parancsok:be' WHERE szoba = '{0}'", Network.IMessage.Channel);
					MessageHandler.ChannelFunkcioReload();
					return;
				}

				if(Network.IMessage.Info[5] == "all")
				{
					var db = SchumixBot.mSQLConn.QueryRow("SELECT szoba FROM channel");
					if(db != null)
					{
						for(int i = 0; i < db.Rows.Count; ++i)
						{
							var row = db.Rows[i];
							string szoba = row["szoba"].ToString();
							SchumixBot.mSQLConn.QueryFirstRow("UPDATE channel SET funkciok = ',koszones:be,log:be,rejoin:be,parancsok:be' WHERE szoba = '{0}'", szoba);
						}

						MessageHandler.ChannelFunkcioReload();
						sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Sikeresen frissitve minden channelen a funkciók.");
					}
					else
						sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Hibás lekérdezés!");
				}
				else
				{
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Sikeresen frissitve {0} channel funkciók.", Network.IMessage.Info[5]);
					SchumixBot.mSQLConn.QueryFirstRow("UPDATE channel SET funkciok = ',koszones:be,log:be,rejoin:be,parancsok:be' WHERE szoba = '{0}'", Network.IMessage.Info[5]);
					MessageHandler.ChannelFunkcioReload();
				}
			}
			else
			{
				if(Network.IMessage.Info.Length < 6)
					return;

				string status = Network.IMessage.Info[4];

				if(status == "be" || status == "ki")
				{
					if(Network.IMessage.Info.Length >= 7)
					{
						string alomany = "";

						for(int i = 5; i < Network.IMessage.Info.Length; i++)
						{
							alomany += ", " + Network.IMessage.Info[i];
							SchumixBot.mSQLConn.QueryFirstRow("UPDATE channel SET funkciok = '{0}' WHERE szoba = '{1}'", MessageHandler.ChannelFunkciok(Network.IMessage.Info[i], status, Network.IMessage.Channel), Network.IMessage.Channel);
							MessageHandler.ChannelFunkcioReload();
						}

						if(alomany.Substring(0, 2) == ", ")
							alomany = alomany.Remove(0, 2);

						sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "{0}: {1}kapcsolva",  alomany, status);
					}
					else
					{
						sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "{0}: {1}kapcsolva", Network.IMessage.Info[5], status);
						SchumixBot.mSQLConn.QueryFirstRow("UPDATE channel SET funkciok = '{0}' WHERE szoba = '{1}'", MessageHandler.ChannelFunkciok(Network.IMessage.Info[5], status, Network.IMessage.Channel), Network.IMessage.Channel);
						MessageHandler.ChannelFunkcioReload();
					}
				}
			}
		}

		public void HandleChannel()
		{
			if(!MessageHandler.CManager.Admin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Operator))
				return;

			if(Network.IMessage.Info.Length < 5)
				return;

			MessageHandler.CNick();

			if(Network.IMessage.Info[4] == "help")
			{
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Alparancsok használata:");
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Hozzáadás: {0}channel add <channel> <ha van pass akkor az>", IRCConfig.Parancselojel);
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Eltávolítás: {0}channel del <channel>", IRCConfig.Parancselojel);
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Frissítés: {0}channel update", IRCConfig.Parancselojel);
			}
			else if(Network.IMessage.Info[4] == "add")
			{
				if(Network.IMessage.Info.Length < 6)
					return;

				string szobainfo = Network.IMessage.Info[5];
			
				if(Network.IMessage.Info.Length == 7)
				{
					MessageHandler.ChannelPrivmsg = Network.IMessage.Channel;
					string jelszo = Network.IMessage.Info[6];
					sSendMessage.WriteLine("JOIN {0} {1}", szobainfo, jelszo);
					SchumixBot.mSQLConn.QueryFirstRow("INSERT INTO `channel`(szoba, jelszo) VALUES ('{0}', '{1}')", szobainfo, jelszo);
					SchumixBot.mSQLConn.QueryFirstRow("UPDATE channel SET aktivitas = 'aktiv' WHERE szoba = '{0}'", szobainfo);
				}
				else
				{
					MessageHandler.ChannelPrivmsg = Network.IMessage.Channel;
					sSendMessage.WriteLine("JOIN {0}", szobainfo);
					SchumixBot.mSQLConn.QueryFirstRow("INSERT INTO `channel`(szoba, jelszo) VALUES ('{0}', '')", szobainfo);
					SchumixBot.mSQLConn.QueryFirstRow("UPDATE channel SET aktivitas = 'aktiv' WHERE szoba = '{0}'", szobainfo);
				}

				sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Channel hozzáadva: {0}", szobainfo);

				MessageHandler.ChannelListaReload();
				MessageHandler.ChannelFunkcioReload();
			}
			else if(Network.IMessage.Info[4] == "del")
			{
				if(Network.IMessage.Info.Length < 6)
					return;

				string szobainfo = Network.IMessage.Info[5];
				sSendMessage.WriteLine("PART {0}", szobainfo);
				SchumixBot.mSQLConn.QueryFirstRow("DELETE FROM `channel` WHERE szoba = '{0}'", szobainfo);
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Channel eltávolítva: {0}", szobainfo);

				MessageHandler.ChannelListaReload();
				MessageHandler.ChannelFunkcioReload();
			}
			else if(Network.IMessage.Info[4] == "update")
			{
				MessageHandler.ChannelListaReload();
				MessageHandler.ChannelFunkcioReload();
			}
			else if(Network.IMessage.Info[4] == "info")
			{
				var db = SchumixBot.mSQLConn.QueryRow("SELECT szoba, aktivitas, error FROM channel");
				if(db != null)
				{
					string Aktivszobak = "";
					string DeAktivszobak = "";
					bool adatszoba = false;
					bool adatszoba1 = false;

					for(int i = 0; i < db.Rows.Count; ++i)
					{
						var row = db.Rows[i];
						string szoba = row["szoba"].ToString();
						string aktivitas = row["aktivitas"].ToString();
						string error = row["error"].ToString();

						if(aktivitas == "aktiv")
						{
							Aktivszobak += ", " + szoba;
							adatszoba = true;
						}
						else if(aktivitas == "nem aktiv")
						{
							DeAktivszobak += ", " + szoba + ":" + error;
							adatszoba1 = true;
						}
					}

					if(adatszoba)
					{
						if(Aktivszobak.Substring(0, 2) == ", ")
							Aktivszobak = Aktivszobak.Remove(0, 2);

						sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "3Aktiv: {0}", Aktivszobak);
					}
					else
						sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "3Aktiv: Nincs adat.");

					if(adatszoba1)
					{
						if(DeAktivszobak.Substring(0, 2) == ", ")
							DeAktivszobak = DeAktivszobak.Remove(0, 2);

						sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "3Deaktiv: {0}", DeAktivszobak);
					}
					else
						sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "3Deaktiv: Nincs adat.");
				}
				else
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Hibás lekérdezés!");
			}
		}

		public void HandleSzinek()
		{
			if(!MessageHandler.CManager.Admin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Operator))
				return;

			MessageHandler.CNick();
			sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "1teszt1 2teszt2 3teszt3 4teszt4 5teszt5 6teszt6 7teszt7 8teszt8");
			sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "9teszt9 10teszt10 11teszt11 12teszt12 13teszt13 14teszt14 15teszt15");
		}

		public void HandleSznap()
		{
			if(!MessageHandler.CManager.Admin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Operator))
				return;

			if(Network.IMessage.Info.Length < 5)
				return;

			MessageHandler.CNick();

			var db = SchumixBot.mSQLConn.QueryFirstRow("SELECT nev, honap, nap FROM sznap WHERE nev = '{0}'", Network.IMessage.Info[4]);
			if(db != null)
			{
				string nev = db["nev"].ToString();
				string honap = db["honap"].ToString();
				int nap = Convert.ToInt32(db["nap"]);
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "{0} születés napja: {1} {2}", nev, honap, nap);
			}
			else
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Nincs ilyen ember.");
		}

		public void HandleNick()
		{
			if(!MessageHandler.CManager.Admin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Operator))
				return;

			if(Network.IMessage.Info.Length < 5)
				return;

			string nick = Network.IMessage.Info[4];
			SchumixBot.NickTarolo = nick;
			sSendMessage.WriteLine("NICK {0}", nick);
		}

		public void HandleJoin()
		{
			if(!MessageHandler.CManager.Admin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Operator))
				return;

			MessageHandler.ChannelPrivmsg = Network.IMessage.Channel;

			if(Network.IMessage.Info.Length == 5)
				sSendMessage.WriteLine("JOIN {0}", Network.IMessage.Info[4]);
			else if(Network.IMessage.Info.Length == 6)
				sSendMessage.WriteLine("JOIN {0} {1}", Network.IMessage.Info[4], Network.IMessage.Info[5]);
		}

		public void HandleLeft()
		{
			if(!MessageHandler.CManager.Admin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Operator))
				return;

			if(Network.IMessage.Info.Length < 5)
				return;

			sSendMessage.WriteLine("PART {0}", Network.IMessage.Info[4]);
		}

		public void HandleKick()
		{
			if(!MessageHandler.CManager.Admin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Operator))
				return;

			if(Network.IMessage.Info.Length < 5)
				return;

			string kick = Network.IMessage.Info[4].ToLower();
			int szam = Network.IMessage.Info.Length;
			string nick = SchumixBot.NickTarolo;

			if(szam == 5)
			{
				if(kick != nick.ToLower())
					sSendMessage.WriteLine("KICK {0} {1}", Network.IMessage.Channel, kick);
			}
			else if(szam >= 6)
			{
				string oka = "";
				for(int i = 5; i < Network.IMessage.Info.Length; i++)
					oka += Network.IMessage.Info[i] + " ";

				if(oka.Substring(0, 1) == ",")
					oka = oka.Remove(0, 1);

				if(kick != nick.ToLower())
					sSendMessage.WriteLine("KICK {0} {1} :{2}", Network.IMessage.Channel, kick, oka);
			}
		}

		public void HandleMode()
		{
			if(!MessageHandler.CManager.Admin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Operator))
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

			if(nev.IndexOf(tnick.ToLower()) == -1)
				sSendMessage.WriteLine("MODE {0} {1} {2}", Network.IMessage.Channel, rang, nev);
		}
	}
}
