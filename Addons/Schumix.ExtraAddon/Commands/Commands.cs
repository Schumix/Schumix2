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
using Schumix.API;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;

namespace Schumix.ExtraAddon.Commands
{
	public partial class Functions : CommandInfo
	{
		private readonly ChannelInfo sChannelInfo = Singleton<ChannelInfo>.Instance;
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;
		private readonly Sender sSender = Singleton<Sender>.Instance;
		private Functions() {}

		public void HandleAutoFunkcio(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.HalfOperator))
				return;

			CNick(sIRCMessage);

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "3Parancsok: kick | mode | hluzenet");
				return;
			}

			if(sIRCMessage.Info[4].ToLower() == "hluzenet")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs megadva az első paraméter!");
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "info")
				{
					var db = SchumixBase.DManager.Query("SELECT Name, Enabled FROM hlmessage");
					if(!db.IsNull())
					{
						string Nevek = string.Empty;

						foreach(DataRow row in db.Rows)
						{
							string nev = row["Name"].ToString();
							string allapot = row["Enabled"].ToString();
							Nevek += ", " + nev + ":" + allapot;
						}

						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "3Létező nickek: {0}", Nevek.Remove(0, 2, ", "));
					}
					else
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Hibás lekérdezés!");
				}
				else if(sIRCMessage.Info[5].ToLower() == "update")
				{
					var db = SchumixBase.DManager.Query("SELECT Name FROM adminok");
					if(!db.IsNull())
					{
						foreach(DataRow row in db.Rows)
						{
							string nev = row["Name"].ToString();

							var db1 = SchumixBase.DManager.QueryFirstRow("SELECT* FROM hlmessage WHERE Name = '{0}'", nev);
							if(db1.IsNull())
								SchumixBase.DManager.QueryFirstRow("INSERT INTO `hlmessage`(Name, Enabled) VALUES ('{0}', 'ki')", nev);
						}

						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Az adatbázis sikeresen frissitésre került.");
					}
					else
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Hibás lekérdezés!");
				}
				else if(sIRCMessage.Info[5].ToLower() == "funkcio")
				{
					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs a funkció név megadva!");
						return;
					}

					string nev = sIRCMessage.Nick.ToLower();
					SchumixBase.DManager.QueryFirstRow("UPDATE `hlmessage` SET `Enabled` = '{0}' WHERE Name = '{1}'", sIRCMessage.Info[6], nev);
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "{0}: {1}kapcsolva", nev, sIRCMessage.Info[6]);
				}
				else
				{
					string nev = sIRCMessage.Nick.ToLower();
					SchumixBase.DManager.QueryFirstRow("UPDATE `hlmessage` SET `Info` = '{0}', `Enabled` = 'be' WHERE Name = '{1}'", sIRCMessage.Info.SplitToString(5, " "), nev);
					SchumixBase.DManager.QueryFirstRow("UPDATE `schumix` SET `funkcio_status` = 'be' WHERE funkcio_nev = 'hl'");
					SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Functions = '{0}' WHERE Channel = '{1}'", sChannelInfo.ChannelFunkciok("hl", "be", sIRCMessage.Channel), sIRCMessage.Channel.ToLower());
					sChannelInfo.ChannelFunkcioReload();
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Az üzenet módosításra került.");
				}
			}

			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
				return;

			if(sIRCMessage.Info[4].ToLower() == "kick")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs megadva az első paraméter!");
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "add")
				{
					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs név megadva!");
						return;
					}

					if(sIRCMessage.Info.Length < 8)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs ok megadva!");
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM kicklist WHERE Name = '{0}'", sIRCMessage.Info[6].ToLower());
					if(!db.IsNull())
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "A név már szerepel a kick listán!");
						return;
					}

					SchumixBase.DManager.QueryFirstRow("INSERT INTO `kicklist`(Name, Channel, Reason) VALUES ('{0}', '{1}', '{2}')", sIRCMessage.Info[6].ToLower(), sIRCMessage.Channel.ToLower(), sIRCMessage.Info.SplitToString(7, " "));
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Kick listához a név hozzáadva: {0}", sIRCMessage.Info[6]);
				}
				else if(sIRCMessage.Info[5].ToLower() == "del")
				{
					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs név megadva!");
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM kicklist WHERE Name = '{0}'", sIRCMessage.Info[6].ToLower());
					if(db.IsNull())
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Ilyen név nem létezik!");
						return;
					}

					SchumixBase.DManager.QueryFirstRow("DELETE FROM `kicklist` WHERE Name = '{0}'", sIRCMessage.Info[6].ToLower());
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Kick listából a név eltávólításra került: {0}", sIRCMessage.Info[6]);
				}
				else if(sIRCMessage.Info[5].ToLower() == "info")
				{
					var db = SchumixBase.DManager.Query("SELECT Name FROM kicklist WHERE Channel = '{0}'", sIRCMessage.Channel.ToLower());
					if(!db.IsNull())
					{
						string Nevek = string.Empty;

						foreach(DataRow row in db.Rows)
						{
							string nev = row["Name"].ToString();
							Nevek += ", " + nev + ":" + sIRCMessage.Channel;
						}

						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Kick listán lévők: {0}", Nevek.Remove(0, 2, ", "));
					}
					else
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Hibás lekérdezés!");
				}
				else if(sIRCMessage.Info[5].ToLower() == "channel")
				{
					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs megadva az első paraméter!");
						return;
					}

					if(sIRCMessage.Info[6].ToLower() == "add")
					{
						if(sIRCMessage.Info.Length < 8)
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs név megadva!");
							return;
						}

						if(sIRCMessage.Info.Length < 9)
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs a csatorna megadva!");
							return;
						}

						if(sIRCMessage.Info.Length < 10)
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs ok megadva!");
							return;
						}
	
						var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM kicklist WHERE Name = '{0}'", sIRCMessage.Info[7].ToLower());
						if(!db.IsNull())
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "A név már szerepel a kick listán!");
							return;
						}

						SchumixBase.DManager.QueryFirstRow("INSERT INTO `kicklist`(Name, Channel, Reason) VALUES ('{0}', '{1}', '{2}')", sIRCMessage.Info[7].ToLower(), sIRCMessage.Info[8].ToLower(), sIRCMessage.Info.SplitToString(9, " "));
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Kick listához a név hozzáadva: {0}", sIRCMessage.Info[7]);
					}
					else if(sIRCMessage.Info[6].ToLower() == "del")
					{
						if(sIRCMessage.Info.Length < 8)
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs név megadva!");
							return;
						}

						var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM kicklist WHERE Name = '{0}'", sIRCMessage.Info[7].ToLower());
						if(db.IsNull())
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Ilyen név nem létezik!");
							return;
						}

						SchumixBase.DManager.QueryFirstRow("DELETE FROM `kicklist` WHERE Name = '{0}'", sIRCMessage.Info[7].ToLower());
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Kick listából a név eltávólításra került: {0}", sIRCMessage.Info[7]);
					}
					else if(sIRCMessage.Info[6].ToLower() == "info")
					{
						var db = SchumixBase.DManager.Query("SELECT Name, Channel FROM kicklist");
						if(!db.IsNull())
						{
							string Nevek = string.Empty;

							foreach(DataRow row in db.Rows)
							{
								string nev = row["Name"].ToString();
								string csatorna = row["Channel"].ToString();
								Nevek += ", " + nev + ":" + csatorna;
							}

							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Kick listán lévők: {0}", Nevek.Remove(0, 2, ", "));
						}
						else
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Hibás lekérdezés!");
					}
				}
			}
			else if(sIRCMessage.Info[4].ToLower() == "mode")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs megadva az első paraméter!");
					return;
				}
		
				if(sIRCMessage.Info[5].ToLower() == "add")
				{
					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs név megadva!");
						return;
					}
		
					if(sIRCMessage.Info.Length < 8)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs rang megadva!");
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM modelist WHERE Name = '{0}'", sIRCMessage.Info[6].ToLower());
					if(!db.IsNull())
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "A név már szerepel a mode listán!");
						return;
					}

					SchumixBase.DManager.QueryFirstRow("INSERT INTO `modelist`(Name, Channel, Rank) VALUES ('{0}', '{1}', '{2}')", sIRCMessage.Info[6].ToLower(), sIRCMessage.Channel.ToLower(), sIRCMessage.Info[7].ToLower());
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Mode listához a név hozzáadva: {0}", sIRCMessage.Info[6]);
				}
				else if(sIRCMessage.Info[5].ToLower() == "del")
				{
					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs név megadva!");
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM modelist WHERE Name = '{0}'", sIRCMessage.Info[6].ToLower());
					if(db.IsNull())
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Ilyen név nem létezik!");
						return;
					}

					SchumixBase.DManager.QueryFirstRow("DELETE FROM `modelist` WHERE Name = '{0}'", sIRCMessage.Info[6].ToLower());
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Mode listából a név eltávólításra került: {0}", sIRCMessage.Info[6]);
				}
				else if(sIRCMessage.Info[5].ToLower() == "info")
				{
					var db = SchumixBase.DManager.Query("SELECT Name FROM modelist WHERE Channel = '{0}'", sIRCMessage.Channel.ToLower());
					if(!db.IsNull())
					{
						string Nevek = string.Empty;

						foreach(DataRow row in db.Rows)
						{
							string nev = row["Name"].ToString();
							Nevek += ", " + nev + ":" + sIRCMessage.Channel;
						}

						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Mode listán lévők: {0}", Nevek.Remove(0, 2, ", "));
					}
					else
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Hibás lekérdezés!");
				}
				else if(sIRCMessage.Info[5].ToLower() == "channel")
				{
					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs megadva az első paraméter!");
						return;
					}
		
					if(sIRCMessage.Info[6].ToLower() == "add")
					{
						if(sIRCMessage.Info.Length < 8)
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs név megadva!");
							return;
						}
		
						if(sIRCMessage.Info.Length < 9)
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs a csatorna megadva!");
							return;
						}
		
						if(sIRCMessage.Info.Length < 10)
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs rang megadva!");
							return;
						}

						var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM modelist WHERE Name = '{0}'", sIRCMessage.Info[7].ToLower());
						if(!db.IsNull())
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "A név már szerepel a mode listán!");
							return;
						}

						SchumixBase.DManager.QueryFirstRow("INSERT INTO `modelist`(Name, Channel, Rank) VALUES ('{0}', '{1}', '{2}')", sIRCMessage.Info[7].ToLower(), sIRCMessage.Info[8].ToLower(), sIRCMessage.Info[9]);
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Mode listához a név hozzáadva: {0}", sIRCMessage.Info[7]);
					}
					else if(sIRCMessage.Info[6].ToLower() == "del")
					{
						if(sIRCMessage.Info.Length < 8)
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs név megadva!");
							return;
						}
		
						var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM modelist WHERE Name = '{0}'", sIRCMessage.Info[7].ToLower());
						if(db.IsNull())
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Ilyen név nem létezik!");
							return;
						}

						SchumixBase.DManager.QueryFirstRow("DELETE FROM `modelist` WHERE Name = '{0}'", sIRCMessage.Info[7].ToLower());
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Mode listából a név eltávólításra került: {0}", sIRCMessage.Info[7]);
					}
					else if(sIRCMessage.Info[6].ToLower() == "info")
					{
						var db = SchumixBase.DManager.Query("SELECT Name, Channel FROM modelist");
						if(!db.IsNull())
						{
							string Nevek = string.Empty;

							foreach(DataRow row in db.Rows)
							{
								string nev = row["Name"].ToString();
								string csatorna = row["Channel"].ToString();
								Nevek += ", " + nev + ":" + csatorna;
							}

							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Mode listán lévők: {0}", Nevek.Remove(0, 2, ", "));
						}
						else
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Hibás lekérdezés!");
					}
				}
			}
		}

		public void HandleUzenet(IRCMessage sIRCMessage)
		{
			CNick(sIRCMessage);

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs megadva egy paraméter!");
				return;
			}

			if(sIRCMessage.Info[4].ToLower() == "channel")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "A csatorna nincs megadva!");
					return;
				}

				if(sIRCMessage.Info.Length < 7)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "A név nincs megadva!");
					return;
				}

				if(sIRCMessage.Info.Length < 8)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Üzenet nincs megadva!");
					return;
				}

				SchumixBase.DManager.QueryFirstRow("INSERT INTO `message`(Name, Channel, Message, Wrote) VALUES ('{0}', '{1}', '{2}', '{3}')", sIRCMessage.Info[6].ToLower(), sIRCMessage.Info[5].ToLower(), sIRCMessage.Info.SplitToString(7, " "), sIRCMessage.Nick);
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Az üzenet sikeresen feljegyzésre került.");
			}
			else
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Üzenet nincs megadva!");
					return;
				}

				SchumixBase.DManager.QueryFirstRow("INSERT INTO `message`(Name, Channel, Message, Wrote) VALUES ('{0}', '{1}', '{2}', '{3}')", sIRCMessage.Info[4].ToLower(), sIRCMessage.Channel.ToLower(), sIRCMessage.Info.SplitToString(5, " "), sIRCMessage.Nick);
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Az üzenet sikeresen feljegyzésre került.");
			}
		}
	}

	public sealed class Jegyzet : CommandInfo
	{
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private Jegyzet() {}

		public void HandleJegyzet(IRCMessage sIRCMessage)
		{
			CNick(sIRCMessage);

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs paraméter!");
				return;
			}

			if(sIRCMessage.Info[4].ToLower() == "info")
			{
				if(!Figyelmeztetes(sIRCMessage))
					return;

				if(!IsUser(sIRCMessage.Nick, sIRCMessage.Host))
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Az adataid nem megfelelőek ezért nem folytatható a parancs!");
					return;
				}

				var db = SchumixBase.DManager.Query("SELECT Code FROM notes");
				if(!db.IsNull())
				{
					string kodok = string.Empty;

					foreach(DataRow row in db.Rows)
					{
						string kod = row["Code"].ToString();
						kodok += ", " + kod;
					}

					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Jegyzetek kódjai: {0}", kodok.Remove(0, 2, ", "));
				}
				else
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Hibás lekérdezés!");
			}
			else if(sIRCMessage.Info[4].ToLower() == "user")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs paraméter!");
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "hozzaferes")
				{
					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs megadva a jelszó!");
						return;
					}

					string nev = sIRCMessage.Nick;
					var db = SchumixBase.DManager.QueryFirstRow("SELECT Password FROM notes_users WHERE Name = '{0}'", nev.ToLower());
					if(!db.IsNull())
					{
						if(db["Password"].ToString() == sUtilities.Sha1(sIRCMessage.Info[6]))
						{
							SchumixBase.DManager.QueryFirstRow("UPDATE notes_users SET Vhost = '{0}' WHERE Name = '{1}'", sIRCMessage.Host, nev.ToLower());
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Hozzáférés engedélyezve");
						}
						else
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Hozzáférés megtagadva");
					}
				}
				else if(sIRCMessage.Info[5].ToLower() == "ujjelszo")
				{
					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs megadva a régi jelszó!");
						return;
					}

					if(sIRCMessage.Info.Length < 8)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs megadva az új jelszó!");
						return;
					}

					string nev = sIRCMessage.Nick;
					var db = SchumixBase.DManager.QueryFirstRow("SELECT Password FROM notes_users WHERE Name = '{0}'", nev.ToLower());
					if(!db.IsNull())
					{
						if(db["Password"].ToString() == sUtilities.Sha1(sIRCMessage.Info[6]))
						{
							SchumixBase.DManager.QueryFirstRow("UPDATE notes_users SET Password = '{0}' WHERE Name = '{1}'", sUtilities.Sha1(sIRCMessage.Info[7]), nev.ToLower());
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Jelszó sikereset meg lett változtatva erre: {0}", sIRCMessage.Info[7]);
						}
						else
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "A mostani jelszó nem egyezik, modósitás megtagadva");
					}
				}
				else if(sIRCMessage.Info[5].ToLower() == "register")
				{
					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs jelszó megadva!");
						return;
					}

					string nev = sIRCMessage.Nick;
					var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM notes_users WHERE Name = '{0}'", nev.ToLower());
					if(!db.IsNull())
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Már szerepelsz a felhasználói listán!");
						return;
					}

					string pass = sIRCMessage.Info[6];
					SchumixBase.DManager.QueryFirstRow("INSERT INTO `notes_users`(Name, Password, Vhost) VALUES ('{0}', '{1}', '{2}')", nev.ToLower(), sUtilities.Sha1(pass), sIRCMessage.Host);
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Sikeresen hozzá vagy adva a felhasználói listához.");
				}
				else if(sIRCMessage.Info[5].ToLower() == "remove")
				{
					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs megadva a jelszó a törlés megerősítéséhez!");
						return;
					}

					string nev = sIRCMessage.Nick;
					var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM notes_users WHERE Name = '{0}'", nev.ToLower());
					if(db.IsNull())
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nem szerepelsz a felhasználói listán!");
						return;
					}

					db = SchumixBase.DManager.QueryFirstRow("SELECT Password FROM notes_users WHERE Name = '{0}'", nev.ToLower());
					if(!db.IsNull())
					{
						if(db["Password"].ToString() != sUtilities.Sha1(sIRCMessage.Info[6]))
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "A jelszó nem egyezik meg az adatbázisban tárolttal!");
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Törlés meg lett szakítva!");
							return;
						}
					}

					SchumixBase.DManager.QueryFirstRow("DELETE FROM `notes_users` WHERE Name = '{0}'", nev.ToLower());
					SchumixBase.DManager.QueryFirstRow("DELETE FROM `notes` WHERE Name = '{0}'", nev.ToLower());
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Sikeresen törölve lett a felhasználód.");
				}
			}
			else if(sIRCMessage.Info[4].ToLower() == "kod")
			{
				if(!Figyelmeztetes(sIRCMessage))
					return;

				if(!IsUser(sIRCMessage.Nick, sIRCMessage.Host))
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Az adataid nem megfelelőek ezért nem folytatható a parancs!");
					return;
				}

				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "A kód nincs megadva!");
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "del")
				{
					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "A kód nincs megadva!");
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM notes WHERE Code = '{0}'", sIRCMessage.Info[6].ToLower());
					if(db.IsNull())
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Ilyen kód nem szerepel a listán!");
						return;
					}

					SchumixBase.DManager.QueryFirstRow("DELETE FROM `notes` WHERE Code = '{0}'", sIRCMessage.Info[6].ToLower());
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "A jegyzet sikeresen törlésre került.");
				}
				else
				{
					var db = SchumixBase.DManager.QueryFirstRow("SELECT Note FROM notes WHERE Code = '{0}'", sIRCMessage.Info[5].ToLower());
					if(!db.IsNull())
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Jegyzet: {0}", db["Note"].ToString());
					else
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Hibás lekérdezés!");
				}
			}
			else
			{
				if(!Figyelmeztetes(sIRCMessage))
					return;

				if(!IsUser(sIRCMessage.Nick, sIRCMessage.Host))
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Az adataid nem megfelelőek ezért nem folytatható a parancs!");
					return;
				}

				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs megadva jegyzetnek semmi se!");
					return;
				}

				string kod = sIRCMessage.Info[4];

				var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM notes WHERE Code = '{0}'", kod.ToLower());
				if(!db.IsNull())
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "A jegyzet kódneve már szerepel az adatbázisban!");
					return;
				}

				SchumixBase.DManager.QueryFirstRow("INSERT INTO `notes`(Code, Name, Note) VALUES ('{0}', '{1}', '{2}')", kod.ToLower(), sIRCMessage.Nick.ToLower(), sIRCMessage.Info.SplitToString(5, " "));
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Jegyzet kódja: {0}", kod);
			}
		}

		private bool IsUser(string Name)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT * FROM notes_users WHERE Name = '{0}'", Name.ToLower());
			if(!db.IsNull())
				return true;

			return false;
		}

		private bool IsUser(string Name, string Vhost)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Vhost FROM notes_users WHERE Name = '{0}'", Name.ToLower());
			if(!db.IsNull())
			{
				string vhost = db["Vhost"].ToString();

				if(Vhost != vhost)
					return false;

				return true;
			}

			return false;
		}

		private bool Figyelmeztetes(IRCMessage sIRCMessage)
		{
			if(!IsUser(sIRCMessage.Nick))
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Jelenleg nem szerepelsz a jegyzetek felhasználói listáján!");
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Ahoz hogy hozzáad magad nem kell mást tenned mint az alábbi parancsot végrehajtani. (Lehetöleg privát üzenetként ne hogy más megtudja.)");
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "{0}jegyzet user register <jelszó>", IRCConfig.CommandPrefix);
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Felhasználói adatok frissitése (ha nem fogadná el adataidat) pedig: {0}jegyzet user hozzaferes <jelszó>", IRCConfig.CommandPrefix);
				return false;
			}

			return true;
		}
	}
}