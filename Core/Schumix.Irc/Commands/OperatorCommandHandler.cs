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

			if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4].ToLower() == "hozzaferes")
			{
				if(Network.IMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs megadva a jelszó!");
					return;
				}

				string nev = Network.IMessage.Nick;
				var db = SchumixBase.DManager.QueryFirstRow("SELECT Password FROM adminok WHERE Name = '{0}'", nev.ToLower());
				if(db != null)
				{
					string JelszoSql = db["Password"].ToString();

					if(JelszoSql == sUtility.Sha1(Network.IMessage.Info[5]))
					{
						SchumixBase.DManager.QueryFirstRow("UPDATE adminok SET Vhost = '{0}' WHERE Name = '{1}'", Network.IMessage.Host, nev.ToLower());
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Hozzáférés engedélyezve");
					}
					else
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Hozzáférés megtagadva");
				}

				allapot = false;
			}
			else if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4].ToLower() == "ujjelszo")
			{
				if(Network.IMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs megadva a régi jelszó!");
					return;
				}

				if(Network.IMessage.Info.Length < 7)
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs megadva az új jelszó!");
					return;
				}

				string nev = Network.IMessage.Nick;
				var db = SchumixBase.DManager.QueryFirstRow("SELECT Password FROM adminok WHERE Name = '{0}'", nev.ToLower());
				if(db != null)
				{
					string JelszoSql = db["Password"].ToString();

					if(JelszoSql == sUtility.Sha1(Network.IMessage.Info[5]))
					{
						SchumixBase.DManager.QueryFirstRow("UPDATE adminok SET Password = '{0}' WHERE Name = '{1}'", sUtility.Sha1(Network.IMessage.Info[6]), nev.ToLower());
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Jelszó sikereset meg lett változtatva erre: {0}", Network.IMessage.Info[6]);
					}
					else
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "A mostani jelszó nem egyezik, modósitás megtagadva");
				}

				allapot = false;
			}

			if(!Admin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Operator))
				return;

			if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4].ToLower() == "info")
			{
				int flag;
				string nev = Network.IMessage.Nick;

				var db = SchumixBase.DManager.QueryFirstRow("SELECT Flag FROM adminok WHERE Name = '{0}'", nev.ToLower());
				if(db != null)
					flag = Convert.ToInt32(db["Flag"].ToString());
				else
					flag = -1;

				if((AdminFlag)flag == AdminFlag.Operator)
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Jelenleg Operátor vagy.");
				else if((AdminFlag)flag == AdminFlag.Administrator)
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Jelenleg Adminisztrátor vagy.");
			}
			else if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4].ToLower() == "lista")
			{
				var db = SchumixBase.DManager.Query("SELECT Name FROM adminok");
				if(db != null)
				{
					string adminok = string.Empty;

					for(int i = 0; i < db.Rows.Count; ++i)
					{
						var row = db.Rows[i];
						string nev = row["Name"].ToString();
						adminok += ", " + nev;
					}

					if(adminok.Substring(0, 2) == ", ")
						adminok = adminok.Remove(0, 2);

					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "2Adminok: {0}", adminok);
				}
				else
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Hibás lekérdezés!");
			}
			else if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4].ToLower() == "add")
			{
				if(Network.IMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs név megadva!");
					return;
				}

				string nev = Network.IMessage.Info[5];

				var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM adminok WHERE Name = '{0}'", nev.ToLower());
				if(db != null)
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "A név már szerepel az admin listán!");
					return;
				}

				string pass = sUtility.GetRandomString();
				SchumixBase.DManager.QueryFirstRow("INSERT INTO `adminok`(Name, Password) VALUES ('{0}', '{1}')", nev.ToLower(), sUtility.Sha1(pass));
				SchumixBase.DManager.QueryFirstRow("INSERT INTO `hlmessage`(Name, Enabled) VALUES ('{0}', 'ki')", nev.ToLower());

				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Admin hozzáadva: {0}", nev);
				sSendMessage.SendCMPrivmsg(nev, "Mostantól Schumix adminja vagy. A te mostani jelszavad: {0}", pass);
				sSendMessage.SendCMPrivmsg(nev, "Ha megszeretnéd változtatni használd az {0}admin ujjelszo parancsot. Használata: {0}admin ujjelszo <régi> <új>", IRCConfig.CommandPrefix);
				sSendMessage.SendCMPrivmsg(nev, "Admin nick élesítése: {0}admin hozzaferes <jelszó>", IRCConfig.CommandPrefix);
			}
			else if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4].ToLower() == "del")
			{
				if(Network.IMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs név megadva!");
					return;
				}

				string nev = Network.IMessage.Info[5];

				if(Admin(Network.IMessage.Nick, AdminFlag.Operator) && Admin(nev, AdminFlag.Administrator))
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nem vagy Adminisztrátor!");
					return;
				}

				SchumixBase.DManager.QueryFirstRow("DELETE FROM `adminok` WHERE Name = '{0}'", nev.ToLower());
				SchumixBase.DManager.QueryFirstRow("DELETE FROM `hluzenet` WHERE nick = '{0}'", nev.ToLower());
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Admin törölve: {0}", nev);
			}
			else if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4].ToLower() == "rang")
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
					SchumixBase.DManager.QueryFirstRow("UPDATE adminok SET Flag = '{0}' WHERE Name = '{1}'", rang, nev);
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Rang sikeresen módosítva.");
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

						parancsok += " | " + IRCConfig.CommandPrefix + command.Key;
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

						parancsok += " | " + IRCConfig.CommandPrefix + command.Key;
					}

					if(parancsok.Substring(0, 3) == " | ")
						parancsok = parancsok.Remove(0, 3);

					foreach(var command in CommandManager.GetAdminCommandHandler())
						parancsok2 += " | " + IRCConfig.CommandPrefix + command.Key;

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
			{
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs paraméter!");
				return;
			}

			CNick();

			if(Network.IMessage.Info[4].ToLower() == "info")
			{
				string[] ChannelInfo = Network.sChannelInfo.ChannelFunkciokInfo(Network.IMessage.Channel).Split('|');
				if(ChannelInfo.Length < 2)
					return;
			
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "2Bekapcsolva: {0}", ChannelInfo[0]);
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "2Kikapcsolva: {0}", ChannelInfo[1]);
			}
			else if(Network.IMessage.Info[4].ToLower() == "all")
			{
				if(Network.IMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs megadva egy paraméter!");
					return;
				}

				if(Network.IMessage.Info[5].ToLower() == "info")
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
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs megadva a funkció neve!");
						return;
					}

					if(Network.IMessage.Info[5].ToLower() == "be" || Network.IMessage.Info[5].ToLower() == "ki")
					{
						if(Network.IMessage.Info.Length >= 8)
						{
							string alomany = string.Empty;

							for(int i = 6; i < Network.IMessage.Info.Length; i++)
							{
								alomany += ", " + Network.IMessage.Info[i].ToLower();
								SchumixBase.DManager.QueryFirstRow("UPDATE schumix SET funkcio_status = '{0}' WHERE funkcio_nev = '{1}'", Network.IMessage.Info[5].ToLower(), Network.IMessage.Info[i].ToLower());
							}

							if(alomany.Substring(0, 2) == ", ")
								alomany = alomany.Remove(0, 2);

							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "{0}: {1}kapcsolva",  alomany, Network.IMessage.Info[5].ToLower());
						}
						else
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "{0}: {1}kapcsolva", Network.IMessage.Info[6].ToLower(), Network.IMessage.Info[5].ToLower());
							SchumixBase.DManager.QueryFirstRow("UPDATE schumix SET funkcio_status = '{0}' WHERE funkcio_nev = '{1}'", Network.IMessage.Info[5].ToLower(), Network.IMessage.Info[6].ToLower());
						}
					}
				}
			}
			else if(Network.IMessage.Info[4].ToLower() == "channel")
			{
				if(Network.IMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs megadva a csatorna neve!");
					return;
				}

				if(Network.IMessage.Info.Length < 7)
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs megadva egy paraméter!");
					return;
				}
			
				string channelinfo = Network.IMessage.Info[5].ToLower();
				string status = Network.IMessage.Info[6].ToLower();
			
				if(Network.IMessage.Info[6].ToLower() == "info")
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
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs megadva a funkció neve!");
						return;
					}

					if(Network.IMessage.Info.Length >= 9)
					{
						string alomany = string.Empty;

						for(int i = 7; i < Network.IMessage.Info.Length; i++)
						{
							alomany += ", " + Network.IMessage.Info[i].ToLower();
							SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Functions = '{0}' WHERE Channel = '{1}'", Network.sChannelInfo.ChannelFunkciok(Network.IMessage.Info[i].ToLower(), status, channelinfo), channelinfo);
							Network.sChannelInfo.ChannelFunkcioReload();
						}

						if(alomany.Substring(0, 2) == ", ")
							alomany = alomany.Remove(0, 2);

						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "{0}: {1}kapcsolva",  alomany, status);
					}
					else
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "{0}: {1}kapcsolva", Network.IMessage.Info[7].ToLower(), status);
						SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Functions = '{0}' WHERE Channel = '{1}'", Network.sChannelInfo.ChannelFunkciok(Network.IMessage.Info[7], status, channelinfo), channelinfo);
						Network.sChannelInfo.ChannelFunkcioReload();
					}
				}
			}
			else if(Network.IMessage.Info[4].ToLower() == "update")
			{
				if(Network.IMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Sikeresen frissitve {0} csatornán a funkciók.", Network.IMessage.Channel);
					SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Functions = ',koszones:ki,log:be,rejoin:be,parancsok:be,hl:ki' WHERE Channel = '{0}'", Network.IMessage.Channel);
					Network.sChannelInfo.ChannelFunkcioReload();
					return;
				}

				if(Network.IMessage.Info[5].ToLower() == "all")
				{
					var db = SchumixBase.DManager.Query("SELECT Channel FROM channel");
					if(db != null)
					{
						for(int i = 0; i < db.Rows.Count; ++i)
						{
							var row = db.Rows[i];
							string szoba = row["Channel"].ToString();
							SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Functions = ',koszones:ki,log:be,rejoin:be,parancsok:be,hl:ki' WHERE Channel = '{0}'", szoba);
						}

						Network.sChannelInfo.ChannelFunkcioReload();
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Sikeresen frissitve minden csatornán a funkciók.");
					}
					else
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Hibás lekérdezés!");
				}
				else
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Sikeresen frissitve {0} csatornán a funkciók.", Network.IMessage.Info[5].ToLower());
					SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Functions = ',koszones:ki,log:be,rejoin:be,parancsok:be,hl:ki' WHERE Channel = '{0}'", Network.IMessage.Info[5].ToLower());
					Network.sChannelInfo.ChannelFunkcioReload();
				}
			}
			else
			{
				if(Network.IMessage.Info.Length < 5)
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs megadva a funkció állapota!");
					return;
				}

				if(Network.IMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs megadva a funkció neve!");
					return;
				}

				string status = Network.IMessage.Info[4].ToLower();

				if(status == "be" || status == "ki")
				{
					if(Network.IMessage.Info.Length >= 7)
					{
						string alomany = string.Empty;

						for(int i = 5; i < Network.IMessage.Info.Length; i++)
						{
							alomany += ", " + Network.IMessage.Info[i].ToLower();
							SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Functions = '{0}' WHERE Channel = '{1}'", Network.sChannelInfo.ChannelFunkciok(Network.IMessage.Info[i].ToLower(), status, Network.IMessage.Channel), Network.IMessage.Channel);
							Network.sChannelInfo.ChannelFunkcioReload();
						}

						if(alomany.Substring(0, 2) == ", ")
							alomany = alomany.Remove(0, 2);

						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "{0}: {1}kapcsolva",  alomany, status);
					}
					else
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "{0}: {1}kapcsolva", Network.IMessage.Info[5].ToLower(), status);
						SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Functions = '{0}' WHERE Channel = '{1}'", Network.sChannelInfo.ChannelFunkciok(Network.IMessage.Info[5].ToLower(), status, Network.IMessage.Channel), Network.IMessage.Channel);
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

			if(Network.IMessage.Info[4].ToLower() == "add")
			{
				if(Network.IMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs megadva a csatorna neve!");
					return;
				}

				string csatornainfo = Network.IMessage.Info[5].ToLower();

				var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM channel WHERE Channel = '{0}'", csatornainfo);
				if(db != null)
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "A név már szerepel a csatorna listán!");
					return;
				}

				if(Network.IMessage.Info.Length == 7)
				{
					ChannelPrivmsg = Network.IMessage.Channel;
					string jelszo = Network.IMessage.Info[6];
					sSender.Join(csatornainfo, jelszo);
					SchumixBase.DManager.QueryFirstRow("INSERT INTO `channel`(Channel, Password) VALUES ('{0}', '{1}')", csatornainfo, jelszo);
					SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Enabled = 'true' WHERE Channel = '{0}'", csatornainfo);
				}
				else
				{
					ChannelPrivmsg = Network.IMessage.Channel;
					sSender.Join(csatornainfo);
					SchumixBase.DManager.QueryFirstRow("INSERT INTO `channel`(Channel, Password) VALUES ('{0}', '')", csatornainfo);
					SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Enabled = 'true' WHERE Channel = '{0}'", csatornainfo);
				}

				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Csatorna hozzáadva: {0}", csatornainfo);

				Network.sChannelInfo.ChannelListaReload();
				Network.sChannelInfo.ChannelFunkcioReload();
			}
			else if(Network.IMessage.Info[4].ToLower() == "del")
			{
				if(Network.IMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs megadva a csatorna neve!");
					return;
				}

				string csatornainfo = Network.IMessage.Info[5].ToLower();
				sSender.Part(csatornainfo);
				SchumixBase.DManager.QueryFirstRow("DELETE FROM `channel` WHERE Channel = '{0}'", csatornainfo);
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Csatorna eltávolítva: {0}", csatornainfo);

				Network.sChannelInfo.ChannelListaReload();
				Network.sChannelInfo.ChannelFunkcioReload();
			}
			else if(Network.IMessage.Info[4].ToLower() == "update")
			{
				Network.sChannelInfo.ChannelListaReload();
				Network.sChannelInfo.ChannelFunkcioReload();
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "A csatorna információk frissitésre kerültek.");
			}
			else if(Network.IMessage.Info[4].ToLower() == "info")
			{
				var db = SchumixBase.DManager.Query("SELECT Channel, Enabled, Error FROM channel");
				if(db != null)
				{
					string AktivCsatornak = string.Empty, DeAktivCsatornak = string.Empty;
					bool AdatCsatorna = false, AdatCsatorna1 = false;

					for(int i = 0; i < db.Rows.Count; ++i)
					{
						var row = db.Rows[i];
						string csatorna = row["Channel"].ToString();
						string aktivitas = row["Enabled"].ToString();
						string error = row["Error"].ToString();

						if(aktivitas == "true")
						{
							AktivCsatornak += ", " + csatorna;
							AdatCsatorna = true;
						}
						else if(aktivitas == "false")
						{
							DeAktivCsatornak += ", " + csatorna + ":" + error;
							AdatCsatorna1 = true;
						}
					}

					if(AdatCsatorna)
					{
						if(AktivCsatornak.Substring(0, 2) == ", ")
							AktivCsatornak = AktivCsatornak.Remove(0, 2);

						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "3Aktiv: {0}", AktivCsatornak);
					}
					else
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "3Aktiv: Nincs adat.");

					if(AdatCsatorna1)
					{
						if(DeAktivCsatornak.Substring(0, 2) == ", ")
							DeAktivCsatornak = DeAktivCsatornak.Remove(0, 2);

						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "3Deaktiv: {0}", DeAktivCsatornak);
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
			{
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs név megadva!");
				return;
			}

			CNick();

			var db = SchumixBase.DManager.QueryFirstRow("SELECT nev, honap, nap FROM sznap WHERE nev = '{0}'", Network.IMessage.Info[4]);
			if(db != null)
			{
				string nev = db["nev"].ToString();
				string honap = db["honap"].ToString();
				int nap = Convert.ToInt32(db["nap"]);
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "{0} születés napja: {1} {2}", nev, honap, nap);
			}
			else
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs ilyen ember!");
		}

		protected void HandleNick()
		{
			if(!Admin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Operator))
				return;

			if(Network.IMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs név megadva!");
				return;
			}

			string nick = Network.IMessage.Info[4];
			sNickInfo.ChangeNick(nick);
			sSender.Nick(nick);
			sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nick megváltoztatása erre: {0}", nick);
		}

		protected void HandleJoin()
		{
			if(!Admin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Operator))
				return;

			if(Network.IMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs megadva a csatorna neve!");
				return;
			}

			ChannelPrivmsg = Network.IMessage.Channel;

			if(Network.IMessage.Info.Length == 5)
				sSender.Join(Network.IMessage.Info[4]);
			else if(Network.IMessage.Info.Length == 6)
				sSender.Join(Network.IMessage.Info[4], Network.IMessage.Info[5]);

			sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kapcsolodás ehez a csatonához: {0}", Network.IMessage.Info[4]);
		}

		protected void HandleLeft()
		{
			if(!Admin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Operator))
				return;

			if(Network.IMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs megadva a csatorna neve!");
				return;
			}

			sSender.Part(Network.IMessage.Info[4]);
			sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Lelépés erről a csatornáról: {0}", Network.IMessage.Info[4]);
		}

		protected void HandleKick()
		{
			if(!Admin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Operator))
				return;

			if(Network.IMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs név megadva!");
				return;
			}

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

			if(Network.IMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs a rang megadva!");
				return;
			}

			if(Network.IMessage.Info.Length < 6)
			{
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs név megadva!");
				return;
			}

			string rang = Network.IMessage.Info[4].ToLower();
			string tnick = sNickInfo.NickStorage;
			string nev = string.Empty;

			for(int i = 5; i < Network.IMessage.Info.Length; i++)
				nev += Network.IMessage.Info[i] + " ";

			if(nev.Substring(0, 1) == " ")
				nev = nev.Remove(0, 1);

			nev.ToLower();

			if(!nev.Contains(tnick.ToLower()))
				sSender.Mode(Network.IMessage.Channel, rang, nev);
		}
	}
}
