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
using System.Data;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;

namespace Schumix.Irc.Commands
{
	public partial class CommandHandler
	{
		protected void HandleAdmin()
		{
			if(!IsAdmin(Network.IMessage.Nick))
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
				if(!db.IsNull())
				{
					if(db["Password"].ToString() == sUtilities.Sha1(Network.IMessage.Info[5]))
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
				if(!db.IsNull())
				{
					if(db["Password"].ToString() == sUtilities.Sha1(Network.IMessage.Info[5]))
					{
						SchumixBase.DManager.QueryFirstRow("UPDATE adminok SET Password = '{0}' WHERE Name = '{1}'", sUtilities.Sha1(Network.IMessage.Info[6]), nev.ToLower());
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Jelszó sikereset meg lett változtatva erre: {0}", Network.IMessage.Info[6]);
					}
					else
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "A mostani jelszó nem egyezik, modósitás megtagadva");
				}

				allapot = false;
			}

			if(!IsAdmin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.HalfOperator))
				return;

			if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4].ToLower() == "info")
			{
				int flag;
				string nev = Network.IMessage.Nick;

				var db = SchumixBase.DManager.QueryFirstRow("SELECT Flag FROM adminok WHERE Name = '{0}'", nev.ToLower());
				if(!db.IsNull())
					flag = Convert.ToInt32(db["Flag"].ToString());
				else
					flag = -1;

				if((AdminFlag)flag == AdminFlag.HalfOperator)
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Jelenleg Fél Operátor vagy.");
				else if((AdminFlag)flag == AdminFlag.Operator)
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Jelenleg Operátor vagy.");
				else if((AdminFlag)flag == AdminFlag.Administrator)
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Jelenleg Adminisztrátor vagy.");
			}
			else if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4].ToLower() == "lista")
			{
				var db = SchumixBase.DManager.Query("SELECT Name FROM adminok");
				if(!db.IsNull())
				{
					string adminok = string.Empty;

					foreach(DataRow row in db.Rows)
					{
						string nev = row["Name"].ToString();
						adminok += ", " + nev;
					}

					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "2Adminok: {0}", adminok.Remove(0, 2, ", "));
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
				if(!db.IsNull())
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "A név már szerepel az admin listán!");
					return;
				}

				string pass = sUtilities.GetRandomString();
				SchumixBase.DManager.QueryFirstRow("INSERT INTO `adminok`(Name, Password) VALUES ('{0}', '{1}')", nev.ToLower(), sUtilities.Sha1(pass));
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
				var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM adminok WHERE Name = '{0}'", nev.ToLower());
				if(db.IsNull())
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Ilyen név nem létezik!");
					return;
				}

				if(IsAdmin(Network.IMessage.Nick, AdminFlag.HalfOperator) && IsAdmin(nev, AdminFlag.Operator))
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nem vagy Operátor!");
					return;
				}

				if(IsAdmin(Network.IMessage.Nick, AdminFlag.HalfOperator) && IsAdmin(nev, AdminFlag.Administrator))
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nem vagy Adminisztrátor!");
					return;
				}

				if(IsAdmin(Network.IMessage.Nick, AdminFlag.Operator) && IsAdmin(nev, AdminFlag.Administrator))
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nem vagy Adminisztrátor!");
					return;
				}

				SchumixBase.DManager.QueryFirstRow("DELETE FROM `adminok` WHERE Name = '{0}'", nev.ToLower());
				SchumixBase.DManager.QueryFirstRow("DELETE FROM `hlmessage` WHERE Name = '{0}'", nev.ToLower());
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
				if(IsAdmin(Network.IMessage.Nick, AdminFlag.HalfOperator) && IsAdmin(nev, AdminFlag.Operator))
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nem vagy Operátor!");
					return;
				}

				if(IsAdmin(Network.IMessage.Nick, AdminFlag.HalfOperator) && IsAdmin(nev, AdminFlag.Administrator))
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nem vagy Adminisztrátor!");
					return;
				}

				if(IsAdmin(Network.IMessage.Nick, AdminFlag.Operator) && IsAdmin(nev, AdminFlag.Administrator))
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nem vagy Adminisztrátor!");
					return;
				}

				int rang = Convert.ToInt32(Network.IMessage.Info[6]);

				if(IsAdmin(Network.IMessage.Nick, AdminFlag.HalfOperator) && IsAdmin(nev, AdminFlag.HalfOperator) && (AdminFlag)rang == AdminFlag.Operator)
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nem vagy Operátor!");
					return;
				}

				if(IsAdmin(Network.IMessage.Nick, AdminFlag.HalfOperator) && IsAdmin(nev, AdminFlag.HalfOperator) && (AdminFlag)rang == AdminFlag.Administrator)
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nem vagy Adminisztrátor!");
					return;
				}

				if(IsAdmin(Network.IMessage.Nick, AdminFlag.Operator) && IsAdmin(nev, AdminFlag.Operator) && (AdminFlag)rang == AdminFlag.Administrator)
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nem vagy Adminisztrátor!");
					return;
				}
		
				if((AdminFlag)rang == AdminFlag.Administrator || (AdminFlag)rang == AdminFlag.Operator || (AdminFlag)rang == AdminFlag.HalfOperator)
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

				if(IsAdmin(Network.IMessage.Nick, AdminFlag.HalfOperator))
				{
					string parancsok = string.Empty;

					foreach(var command in CommandManager.GetHalfOperatorCommandHandler())
					{
						if(command.Key == "admin")
							continue;

						parancsok += " | " + IRCConfig.CommandPrefix + command.Key;
					}

					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "3Fél Operátor parancsok!");
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "3Parancsok: {0}", parancsok.Remove(0, 3, " | "));
				}
				else if(IsAdmin(Network.IMessage.Nick, AdminFlag.Operator))
				{
					string parancsok = string.Empty;
					string parancsok2 = string.Empty;

					foreach(var command in CommandManager.GetHalfOperatorCommandHandler())
					{
						if(command.Key == "admin")
							continue;

						parancsok += " | " + IRCConfig.CommandPrefix + command.Key;
					}

					foreach(var command in CommandManager.GetOperatorCommandHandler())
						parancsok2 += " | " + IRCConfig.CommandPrefix + command.Key;

					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "3Operátor parancsok!");
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "3Parancsok: {0}{1}", parancsok.Remove(0, 3, " | "), parancsok2);
				}
				else if(IsAdmin(Network.IMessage.Nick, AdminFlag.Administrator))
				{
					string parancsok = string.Empty;
					string parancsok2 = string.Empty;
					string parancsok3 = string.Empty;

					foreach(var command in CommandManager.GetHalfOperatorCommandHandler())
					{
						if(command.Key == "admin")
							continue;

						parancsok += " | " + IRCConfig.CommandPrefix + command.Key;
					}

					foreach(var command in CommandManager.GetOperatorCommandHandler())
						parancsok2 += " | " + IRCConfig.CommandPrefix + command.Key;

					foreach(var command in CommandManager.GetAdminCommandHandler())
						parancsok3 += " | " + IRCConfig.CommandPrefix + command.Key;

					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "3Adminisztrátor parancsok!");
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "3Parancsok: {0}{1}{2}", parancsok.Remove(0, 3, " | "), parancsok2, parancsok3);
				}
			}
		}

		protected void HandleSzinek()
		{
			if(!IsAdmin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.HalfOperator))
				return;

			CNick();
			sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "1teszt1 2teszt2 3teszt3 4teszt4 5teszt5 6teszt6 7teszt7 8teszt8");
			sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "9teszt9 10teszt10 11teszt11 12teszt12 13teszt13 14teszt14 15teszt15");
		}

		protected void HandleNick()
		{
			if(!IsAdmin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.HalfOperator))
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
			if(!IsAdmin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.HalfOperator))
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
			if(!IsAdmin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.HalfOperator))
				return;

			if(Network.IMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs megadva a csatorna neve!");
				return;
			}

			sSender.Part(Network.IMessage.Info[4]);
			sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Lelépés erről a csatornáról: {0}", Network.IMessage.Info[4]);
		}
	}
}