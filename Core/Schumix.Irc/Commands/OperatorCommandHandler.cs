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
using Schumix.Framework;
using Schumix.Framework.Config;

namespace Schumix.Irc.Commands
{
	public partial class CommandHandler
	{
		protected void HandleAdmin()
		{
			if(!Admin(Network.IMessage.Nick))
				return;

			CNick();
			bool allapot = true;

			if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4] == "hozzaferes")
			{
				if(Network.IMessage.Info.Length < 6)
					return;

				string nev = Network.IMessage.Nick;
				var db = SchumixBase.mSQLConn.QueryFirstRow("SELECT jelszo FROM adminok WHERE nev = '{0}'", nev.ToLower());
				if(db != null)
				{
					string JelszoSql = db["jelszo"].ToString();

					if(JelszoSql == sUtility.Sha1(Network.IMessage.Info[5]))
					{
						SchumixBase.mSQLConn.QueryFirstRow("UPDATE adminok SET vhost = '{0}' WHERE nev = '{1}'", Network.IMessage.Host, nev.ToLower());
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Hozzáférés engedélyezve");
					}
					else
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Hozzáférés megtagadva");
				}

				allapot = false;
			}
			else if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4] == "ujjelszo")
			{
				if(Network.IMessage.Info.Length < 7)
					return;

				string nev = Network.IMessage.Nick;
				var db = SchumixBase.mSQLConn.QueryFirstRow("SELECT nev, jelszo FROM adminok WHERE nev = '{0}'", nev.ToLower());
				if(db != null)
				{
					string JelszoSql = db["jelszo"].ToString();

					if(JelszoSql == sUtility.Sha1(Network.IMessage.Info[5]))
					{
						SchumixBase.mSQLConn.QueryFirstRow("UPDATE adminok SET jelszo = '{0}' WHERE nev = '{1}'", sUtility.Sha1(Network.IMessage.Info[6]), nev.ToLower());
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Jelszó sikereset meg lett változtatva erre: {0}", Network.IMessage.Info[6]);
					}
					else
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "A mostani jelszó nem egyezik, modósitás megtagadva");
				}

				allapot = false;
			}

			if(!Admin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Operator))
				return;

			if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4] == "info")
			{
				int flag;
				string nev = Network.IMessage.Nick;

				var db = SchumixBase.mSQLConn.QueryFirstRow("SELECT flag FROM adminok WHERE nev = '{0}'", nev.ToLower());
				if(db != null)
					flag = Convert.ToInt32(db["flag"].ToString());
				else
					flag = -1;

				if((AdminFlag)flag == AdminFlag.Operator)
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Jelenleg Operátor vagy.");
				else if((AdminFlag)flag == AdminFlag.Administrator)
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Jelenleg Adminisztrátor vagy.");
			}
			else if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4] == "lista")
			{
				var db = SchumixBase.mSQLConn.QueryRow("SELECT nev FROM adminok");
				if(db != null)
				{
					string adminok = string.Empty;

					for(int i = 0; i < db.Rows.Count; ++i)
					{
						var row = db.Rows[i];
						string nev = row["nev"].ToString();
						adminok += ", " + nev;
					}

					if(adminok.Substring(0, 2) == ", ")
						adminok = adminok.Remove(0, 2);

					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "2Adminok: {0}", adminok);
				}
				else
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Hibas lekerdezes!");
			}
			else if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4] == "add")
			{
				if(Network.IMessage.Info.Length < 6)
					return;

				string nev = Network.IMessage.Info[5];
				string pass = sUtility.GetRandomString();

				SchumixBase.mSQLConn.QueryFirstRow("INSERT INTO `adminok`(nev, jelszo) VALUES ('{0}', '{1}')", nev.ToLower(), sUtility.Sha1(pass));

				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Admin hozzáadva: {0}", nev);
				sSendMessage.SendCMPrivmsg(nev, "Mostantól Schumix adminja vagy. A te mostani jelszavad: {0}", pass);
				sSendMessage.SendCMPrivmsg(nev, "Ha megszeretnéd változtatni használd az {0}admin ujjelszo parancsot. Használata: {0}admin ujjelszo <régi> <új>", IRCConfig.Parancselojel);
				sSendMessage.SendCMPrivmsg(nev, "Admin nick élesítése: {0}admin hozzaferes <jelszó>", IRCConfig.Parancselojel);
			}
			else if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4] == "del")
			{
				if(Network.IMessage.Info.Length < 6)
					return;

				string nev = Network.IMessage.Info[5];

				if(Admin(Network.IMessage.Nick, AdminFlag.Operator) && Admin(nev, AdminFlag.Administrator))
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nem vagy Adminisztrátor!");
					return;
				}

				SchumixBase.mSQLConn.QueryFirstRow("DELETE FROM `adminok` WHERE nev = '{0}'", nev.ToLower());
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Admin törölve: {0}", nev);
			}
			else if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4] == "rang")
			{
				if(Network.IMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs név megadva!");
					return;
				}

				if(Network.IMessage.Info.Length < 7)
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs rang megadva!");
					return;
				}

				string nev = Network.IMessage.Info[5].ToLower();
				if(Admin(Network.IMessage.Nick, AdminFlag.Operator) && Admin(nev, AdminFlag.Administrator))
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nem vagy Adminisztrátor!");
					return;
				}

				int rang = Convert.ToInt32(Network.IMessage.Info[6]);
		
				if(Admin(Network.IMessage.Nick, AdminFlag.Operator) && Admin(nev, AdminFlag.Operator) && (AdminFlag)rang == AdminFlag.Administrator)
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nem vagy Adminisztrátor!");
					return;
				}
		
				if((AdminFlag)rang == AdminFlag.Administrator || (AdminFlag)rang == AdminFlag.Operator)
				{
					SchumixBase.mSQLConn.QueryFirstRow("UPDATE adminok SET flag = '{0}' WHERE nev = '{1}'", rang, nev);
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Rang sikeresen modósitva.");
				}
				else
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Hibás rang!");
			}
			else
			{
				if(!allapot)
					return;

				if(Admin(Network.IMessage.Nick, AdminFlag.Operator))
				{
					string parancsok = string.Empty;

					foreach(var command in CommandManager.GetOperatorCommandHandler())
					{
						if(command.Key == "admin")
							continue;

						parancsok += " | " + IRCConfig.Parancselojel + command.Key;
					}

					if(parancsok.Substring(0, 3) == " | ")
						parancsok = parancsok.Remove(0, 3);

					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "3Operátor parancsok!");
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "3Parancsok: {0}", parancsok);
				}
				else if(Admin(Network.IMessage.Nick, AdminFlag.Administrator))
				{
					string parancsok = string.Empty;
					string parancsok2 = string.Empty;

					foreach(var command in CommandManager.GetOperatorCommandHandler())
					{
						if(command.Key == "admin")
							continue;

						parancsok += " | " + IRCConfig.Parancselojel + command.Key;
					}

					if(parancsok.Substring(0, 3) == " | ")
						parancsok = parancsok.Remove(0, 3);

					foreach(var command in CommandManager.GetAdminCommandHandler())
						parancsok2 += " | " + IRCConfig.Parancselojel + command.Key;

					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "3Adminisztrátor parancsok!");
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "3Parancsok: {0}{1}", parancsok, parancsok2);
				}
			}
		}

		protected void HandleFunkcio()
		{
			if(!Admin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Operator))
				return;

			if(Network.IMessage.Info.Length < 5)
				return;

			CNick();

			if(Network.IMessage.Info[4] == "info")
			{
				string[] ChannelInfo = Network.sChannelInfo.ChannelFunkciokInfo(Network.IMessage.Channel).Split('|');
				if(ChannelInfo.Length < 2)
					return;
			
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "2Bekapcsolva: {0}", ChannelInfo[0]);
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "2Kikapcsolva: {0}", ChannelInfo[1]);
			}
			else if(Network.IMessage.Info[4] == "all")
			{
				if(Network.IMessage.Info.Length < 6)
					return;

				if(Network.IMessage.Info[5] == "info")
				{
					string f = Network.sChannelInfo.FunkciokInfo();
					if(f == "Hibás lekérdezés!")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Hibás lekérdezés!");
						return;
					}

					string[] FunkcioInfo = f.Split('|');
					if(FunkcioInfo.Length < 2)
						return;

					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "2Bekapcsolva: {0}", FunkcioInfo[0]);
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "2Kikapcsolva: {0}", FunkcioInfo[1]);
				}
				else
				{
					if(Network.IMessage.Info.Length < 7)
						return;

					if(Network.IMessage.Info[5] == "be" || Network.IMessage.Info[5] == "ki")
					{
						if(Network.IMessage.Info.Length >= 8)
						{
							string alomany = string.Empty;
	
							for(int i = 6; i < Network.IMessage.Info.Length; i++)
							{
								alomany += ", " + Network.IMessage.Info[i];
								SchumixBase.mSQLConn.QueryFirstRow("UPDATE schumix SET funkcio_status = '{0}' WHERE funkcio_nev = '{1}'", Network.IMessage.Info[5], Network.IMessage.Info[i]);
							}
	
							if(alomany.Substring(0, 2) == ", ")
								alomany = alomany.Remove(0, 2);
	
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "{0}: {1}kapcsolva",  alomany, Network.IMessage.Info[5]);
						}
						else
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "{0}: {1}kapcsolva", Network.IMessage.Info[6], Network.IMessage.Info[5]);
							SchumixBase.mSQLConn.QueryFirstRow("UPDATE schumix SET funkcio_status = '{0}' WHERE funkcio_nev = '{1}'", Network.IMessage.Info[5], Network.IMessage.Info[6]);
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
					string[] ChannelInfo = Network.sChannelInfo.ChannelFunkciokInfo(channelinfo).Split('|');
					if(ChannelInfo.Length < 2)
						return;

					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "2Bekapcsolva: {0}", ChannelInfo[0]);
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "2Kikapcsolva: {0}", ChannelInfo[1]);
				}
				else if(status == "be" || status == "ki")
				{
					if(Network.IMessage.Info.Length < 8)
						return;

					if(Network.IMessage.Info.Length >= 9)
					{
						string alomany = string.Empty;

						for(int i = 7; i < Network.IMessage.Info.Length; i++)
						{
							alomany += ", " + Network.IMessage.Info[i];
							SchumixBase.mSQLConn.QueryFirstRow("UPDATE channel SET funkciok = '{0}' WHERE szoba = '{1}'", Network.sChannelInfo.ChannelFunkciok(Network.IMessage.Info[i], status, channelinfo), channelinfo);
							Network.sChannelInfo.ChannelFunkcioReload();
						}

						if(alomany.Substring(0, 2) == ", ")
							alomany = alomany.Remove(0, 2);

						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "{0}: {1}kapcsolva",  alomany, status);
					}
					else
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "{0}: {1}kapcsolva", Network.IMessage.Info[7], status);
						SchumixBase.mSQLConn.QueryFirstRow("UPDATE channel SET funkciok = '{0}' WHERE szoba = '{1}'", Network.sChannelInfo.ChannelFunkciok(Network.IMessage.Info[7], status, channelinfo), channelinfo);
						Network.sChannelInfo.ChannelFunkcioReload();
					}
				}
			}
			else if(Network.IMessage.Info[4] == "update")
			{
				if(Network.IMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Sikeresen frissitve {0} channel funkciók.", Network.IMessage.Channel);
					SchumixBase.mSQLConn.QueryFirstRow("UPDATE channel SET funkciok = ',koszones:ki,log:be,rejoin:be,parancsok:be' WHERE szoba = '{0}'", Network.IMessage.Channel);
					Network.sChannelInfo.ChannelFunkcioReload();
					return;
				}

				if(Network.IMessage.Info[5] == "all")
				{
					var db = SchumixBase.mSQLConn.QueryRow("SELECT szoba FROM channel");
					if(db != null)
					{
						for(int i = 0; i < db.Rows.Count; ++i)
						{
							var row = db.Rows[i];
							string szoba = row["szoba"].ToString();
							SchumixBase.mSQLConn.QueryFirstRow("UPDATE channel SET funkciok = ',koszones:ki,log:be,rejoin:be,parancsok:be' WHERE szoba = '{0}'", szoba);
						}

						Network.sChannelInfo.ChannelFunkcioReload();
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Sikeresen frissitve minden channelen a funkciók.");
					}
					else
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Hibás lekérdezés!");
				}
				else
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Sikeresen frissitve {0} channel funkciók.", Network.IMessage.Info[5]);
					SchumixBase.mSQLConn.QueryFirstRow("UPDATE channel SET funkciok = ',koszones:ki,log:be,rejoin:be,parancsok:be' WHERE szoba = '{0}'", Network.IMessage.Info[5]);
					Network.sChannelInfo.ChannelFunkcioReload();
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
						string alomany = string.Empty;

						for(int i = 5; i < Network.IMessage.Info.Length; i++)
						{
							alomany += ", " + Network.IMessage.Info[i];
							SchumixBase.mSQLConn.QueryFirstRow("UPDATE channel SET funkciok = '{0}' WHERE szoba = '{1}'", Network.sChannelInfo.ChannelFunkciok(Network.IMessage.Info[i], status, Network.IMessage.Channel), Network.IMessage.Channel);
							Network.sChannelInfo.ChannelFunkcioReload();
						}

						if(alomany.Substring(0, 2) == ", ")
							alomany = alomany.Remove(0, 2);

						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "{0}: {1}kapcsolva",  alomany, status);
					}
					else
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "{0}: {1}kapcsolva", Network.IMessage.Info[5], status);
						SchumixBase.mSQLConn.QueryFirstRow("UPDATE channel SET funkciok = '{0}' WHERE szoba = '{1}'", Network.sChannelInfo.ChannelFunkciok(Network.IMessage.Info[5], status, Network.IMessage.Channel), Network.IMessage.Channel);
						Network.sChannelInfo.ChannelFunkcioReload();
					}
				}
			}
		}

		protected void HandleChannel()
		{
			if(!Admin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Operator))
				return;

			CNick();

			if(Network.IMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "3Parancsok: add | del | info | update");
				return;
			}

			if(Network.IMessage.Info[4] == "add")
			{
				if(Network.IMessage.Info.Length < 6)
					return;

				string szobainfo = Network.IMessage.Info[5];
			
				if(Network.IMessage.Info.Length == 7)
				{
					ChannelPrivmsg = Network.IMessage.Channel;
					string jelszo = Network.IMessage.Info[6];
					sSender.Join(szobainfo, jelszo);
					SchumixBase.mSQLConn.QueryFirstRow("INSERT INTO `channel`(szoba, jelszo) VALUES ('{0}', '{1}')", szobainfo, jelszo);
					SchumixBase.mSQLConn.QueryFirstRow("UPDATE channel SET aktivitas = 'aktiv' WHERE szoba = '{0}'", szobainfo);
				}
				else
				{
					ChannelPrivmsg = Network.IMessage.Channel;
					sSender.Join(szobainfo);
					SchumixBase.mSQLConn.QueryFirstRow("INSERT INTO `channel`(szoba, jelszo) VALUES ('{0}', '')", szobainfo);
					SchumixBase.mSQLConn.QueryFirstRow("UPDATE channel SET aktivitas = 'aktiv' WHERE szoba = '{0}'", szobainfo);
				}

				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Channel hozzáadva: {0}", szobainfo);

				Network.sChannelInfo.ChannelListaReload();
				Network.sChannelInfo.ChannelFunkcioReload();
			}
			else if(Network.IMessage.Info[4] == "del")
			{
				if(Network.IMessage.Info.Length < 6)
					return;

				string szobainfo = Network.IMessage.Info[5];
				sSender.Part(szobainfo);
				SchumixBase.mSQLConn.QueryFirstRow("DELETE FROM `channel` WHERE szoba = '{0}'", szobainfo);
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Channel eltávolítva: {0}", szobainfo);

				Network.sChannelInfo.ChannelListaReload();
				Network.sChannelInfo.ChannelFunkcioReload();
			}
			else if(Network.IMessage.Info[4] == "update")
			{
				Network.sChannelInfo.ChannelListaReload();
				Network.sChannelInfo.ChannelFunkcioReload();
			}
			else if(Network.IMessage.Info[4] == "info")
			{
				var db = SchumixBase.mSQLConn.QueryRow("SELECT szoba, aktivitas, error FROM channel");
				if(db != null)
				{
					string Aktivszobak = string.Empty;
					string DeAktivszobak = string.Empty;
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

						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "3Aktiv: {0}", Aktivszobak);
					}
					else
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "3Aktiv: Nincs adat.");

					if(adatszoba1)
					{
						if(DeAktivszobak.Substring(0, 2) == ", ")
							DeAktivszobak = DeAktivszobak.Remove(0, 2);

						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "3Deaktiv: {0}", DeAktivszobak);
					}
					else
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "3Deaktiv: Nincs adat.");
				}
				else
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Hibás lekérdezés!");
			}
		}

		protected void HandleSzinek()
		{
			if(!Admin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Operator))
				return;

			CNick();
			sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "1teszt1 2teszt2 3teszt3 4teszt4 5teszt5 6teszt6 7teszt7 8teszt8");
			sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "9teszt9 10teszt10 11teszt11 12teszt12 13teszt13 14teszt14 15teszt15");
		}

		protected void HandleSznap()
		{
			if(!Admin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Operator))
				return;

			if(Network.IMessage.Info.Length < 5)
				return;

			CNick();

			var db = SchumixBase.mSQLConn.QueryFirstRow("SELECT nev, honap, nap FROM sznap WHERE nev = '{0}'", Network.IMessage.Info[4]);
			if(db != null)
			{
				string nev = db["nev"].ToString();
				string honap = db["honap"].ToString();
				int nap = Convert.ToInt32(db["nap"]);
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "{0} születés napja: {1} {2}", nev, honap, nap);
			}
			else
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs ilyen ember.");
		}

		protected void HandleNick()
		{
			if(!Admin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Operator))
				return;

			if(Network.IMessage.Info.Length < 5)
				return;

			string nick = Network.IMessage.Info[4];
			sNickInfo.ChangeNick(nick);
			sSender.Nick(nick);
		}

		protected void HandleJoin()
		{
			if(!Admin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Operator))
				return;

			ChannelPrivmsg = Network.IMessage.Channel;

			if(Network.IMessage.Info.Length == 5)
				sSender.Join(Network.IMessage.Info[4]);
			else if(Network.IMessage.Info.Length == 6)
				sSender.Join(Network.IMessage.Info[4], Network.IMessage.Info[5]);
		}

		protected void HandleLeft()
		{
			if(!Admin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Operator))
				return;

			if(Network.IMessage.Info.Length < 5)
				return;

			sSender.Part(Network.IMessage.Info[4]);
		}

		protected void HandleKick()
		{
			if(!Admin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Operator))
				return;

			if(Network.IMessage.Info.Length < 5)
				return;

			string kick = Network.IMessage.Info[4].ToLower();
			int szam = Network.IMessage.Info.Length;
			string nick = sNickInfo.NickStorage;

			if(szam == 5)
			{
				if(kick != nick.ToLower())
					sSender.Kick(Network.IMessage.Channel, kick);
			}
			else if(szam >= 6)
			{
				string oka = string.Empty;
				for(int i = 5; i < Network.IMessage.Info.Length; i++)
					oka += Network.IMessage.Info[i] + " ";

				if(oka.Substring(0, 1) == ",")
					oka = oka.Remove(0, 1);

				if(kick != nick.ToLower())
					sSender.Kick(Network.IMessage.Channel, kick, oka);
			}
		}

		protected void HandleMode()
		{
			if(!Admin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Operator))
				return;

			if(Network.IMessage.Info.Length < 6)
				return;

			string rang = Network.IMessage.Info[4].ToLower();
			string tnick = sNickInfo.NickStorage;
			string nev = string.Empty;

			for(int i = 5; i < Network.IMessage.Info.Length; i++)
				nev += Network.IMessage.Info[i] + " ";

			if(nev.Substring(0, 1) == " ")
				nev = nev.Remove(0, 1);

			nev.ToLower();

			if(nev.IndexOf(tnick.ToLower()) == -1)
				sSender.Mode(Network.IMessage.Channel, rang, nev);
		}
	}
}
