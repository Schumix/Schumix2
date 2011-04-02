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
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;

namespace Schumix.ExtraAddon.Commands
{
	public partial class Functions : CommandInfo
	{
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;
		private readonly Sender sSender = Singleton<Sender>.Instance;

		private Functions() {}

		public void HandleAutoFunkcio()
		{
			if(!Admin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Operator))
				return;

			CNick();

			if(Network.IMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "3Parancsok: kick | mode | hluzenet");
				return;
			}

			if(Network.IMessage.Info[4].ToLower() == "kick")
			{
				if(Network.IMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs megadva az első paraméter!");
					return;
				}

				if(Network.IMessage.Info[5].ToLower() == "add")
				{
					if(Network.IMessage.Info.Length < 7)
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs név megadva!");
						return;
					}

					if(Network.IMessage.Info.Length < 8)
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs ok megadva!");
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM kicklist WHERE Name = '{0}'", Network.IMessage.Info[6].ToLower());
					if(db != null)
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "A név már szerepel a kick listán!");
						return;
					}

					string adat = string.Empty;
					for(int i = 7; i < Network.IMessage.Info.Length; i++)
						adat += " " + Network.IMessage.Info[i];

					if(adat.Substring(0, 1) == " ")
						adat = adat.Remove(0, 1);

					SchumixBase.DManager.QueryFirstRow("INSERT INTO `kicklist`(Name, Channel, Reason) VALUES ('{0}', '{1}', '{2}')", Network.IMessage.Info[6].ToLower(), Network.IMessage.Channel, adat);
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kick listához a név hozzáadva: {0}", Network.IMessage.Info[6]);
				}
				else if(Network.IMessage.Info[5].ToLower() == "del")
				{
					if(Network.IMessage.Info.Length < 7)
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs név megadva!");
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM kicklist WHERE Name = '{0}'", Network.IMessage.Info[6].ToLower());
					if(db == null)
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Ilyen név nem létezik!");
						return;
					}

					SchumixBase.DManager.QueryFirstRow("DELETE FROM `kicklist` WHERE Name = '{0}'", Network.IMessage.Info[6].ToLower());
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kick listából a név eltávólításra került: {0}", Network.IMessage.Info[6]);
				}
				else if(Network.IMessage.Info[5].ToLower() == "info")
				{
					var db = SchumixBase.DManager.Query("SELECT Name FROM kicklist WHERE Channel = '{0}'", Network.IMessage.Channel);
					if(db != null)
					{
						string Nevek = string.Empty;

						for(int i = 0; i < db.Rows.Count; ++i)
						{
							var row = db.Rows[i];
							string nev = row["Name"].ToString();
							Nevek += ", " + nev + ":" + Network.IMessage.Channel;
						};

						if(Nevek.Length > 1 && Nevek.Substring(0, 2) == ", ")
							Nevek = Nevek.Remove(0, 2);

						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kick listán lévők: {0}", Nevek);
					}
					else
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Hibás lekérdezés!");
				}
				else if(Network.IMessage.Info[5].ToLower() == "channel")
				{
					if(Network.IMessage.Info.Length < 7)
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs megadva az első paraméter!");
						return;
					}

					if(Network.IMessage.Info[6].ToLower() == "add")
					{
						if(Network.IMessage.Info.Length < 8)
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs név megadva!");
							return;
						}

						if(Network.IMessage.Info.Length < 9)
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs a csatorna megadva!");
							return;
						}

						if(Network.IMessage.Info.Length < 10)
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs ok megadva!");
							return;
						}
	
						var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM kicklist WHERE Name = '{0}'", Network.IMessage.Info[7].ToLower());
						if(db != null)
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "A név már szerepel a kick listán!");
							return;
						}

						string adat = string.Empty;
						for(int i = 9; i < Network.IMessage.Info.Length; i++)
							adat += " " + Network.IMessage.Info[i];

						if(adat.Substring(0, 1) == " ")
							adat = adat.Remove(0, 1);

						SchumixBase.DManager.QueryFirstRow("INSERT INTO `kicklist`(Name, Channel, Reason) VALUES ('{0}', '{1}', '{2}')", Network.IMessage.Info[7].ToLower(), Network.IMessage.Info[8], adat);
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kick listához a név hozzáadva: {0}", Network.IMessage.Info[7]);
					}
					else if(Network.IMessage.Info[6].ToLower() == "del")
					{
						if(Network.IMessage.Info.Length < 8)
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs név megadva!");
							return;
						}

						var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM kicklist WHERE Name = '{0}'", Network.IMessage.Info[7].ToLower());
						if(db == null)
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Ilyen név nem létezik!");
							return;
						}

						SchumixBase.DManager.QueryFirstRow("DELETE FROM `kicklist` WHERE Name = '{0}'", Network.IMessage.Info[7].ToLower());
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kick listából a név eltávólításra került: {0}", Network.IMessage.Info[7]);
					}
					else if(Network.IMessage.Info[6].ToLower() == "info")
					{
						var db = SchumixBase.DManager.Query("SELECT Name, Channel FROM kicklist");
						if(db != null)
						{
							string Nevek = string.Empty;

							for(int i = 0; i < db.Rows.Count; ++i)
							{
								var row = db.Rows[i];
								string nev = row["Name"].ToString();
								string csatorna = row["Channel"].ToString();
								Nevek += ", " + nev + ":" + csatorna;
							};

							if(Nevek.Length > 1 && Nevek.Substring(0, 2) == ", ")
								Nevek = Nevek.Remove(0, 2);

							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kick listán lévők: {0}", Nevek);
						}
						else
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Hibás lekérdezés!");
					}
				}
			}
			else if(Network.IMessage.Info[4].ToLower() == "mode")
			{
				if(Network.IMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs megadva az első paraméter!");
					return;
				}
		
				if(Network.IMessage.Info[5].ToLower() == "add")
				{
					if(Network.IMessage.Info.Length < 7)
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs név megadva!");
						return;
					}
		
					if(Network.IMessage.Info.Length < 8)
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs rang megadva!");
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM modelist WHERE Name = '{0}'", Network.IMessage.Info[6].ToLower());
					if(db != null)
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "A név már szerepel a mode listán!");
						return;
					}

					SchumixBase.DManager.QueryFirstRow("INSERT INTO `modelist`(Name, Channel, Rank) VALUES ('{0}', '{1}', '{2}')", Network.IMessage.Info[6].ToLower(), Network.IMessage.Channel, Network.IMessage.Info[7].ToLower());
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Mode listához a név hozzáadva: {0}", Network.IMessage.Info[6]);
				}
				else if(Network.IMessage.Info[5].ToLower() == "del")
				{
					if(Network.IMessage.Info.Length < 7)
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs név megadva!");
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM modelist WHERE Name = '{0}'", Network.IMessage.Info[6].ToLower());
					if(db == null)
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Ilyen név nem létezik!");
						return;
					}

					SchumixBase.DManager.QueryFirstRow("DELETE FROM `modelist` WHERE Name = '{0}'", Network.IMessage.Info[6].ToLower());
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Mode listából a név eltávólításra került: {0}", Network.IMessage.Info[6]);
				}
				else if(Network.IMessage.Info[5].ToLower() == "info")
				{
					var db = SchumixBase.DManager.Query("SELECT Name FROM modelist WHERE Channel = '{0}'", Network.IMessage.Channel);
					if(db != null)
					{
						string Nevek = string.Empty;

						for(int i = 0; i < db.Rows.Count; ++i)
						{
							var row = db.Rows[i];
							string nev = row["Name"].ToString();
							Nevek += ", " + nev + ":" + Network.IMessage.Channel;
						};

						if(Nevek.Length > 1 && Nevek.Substring(0, 2) == ", ")
							Nevek = Nevek.Remove(0, 2);

						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Mode listán lévők: {0}", Nevek);
					}
					else
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Hibás lekérdezés!");
				}
				else if(Network.IMessage.Info[5].ToLower() == "channel")
				{
					if(Network.IMessage.Info.Length < 7)
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs megadva az első paraméter!");
						return;
					}
		
					if(Network.IMessage.Info[6].ToLower() == "add")
					{
						if(Network.IMessage.Info.Length < 8)
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs név megadva!");
							return;
						}
		
						if(Network.IMessage.Info.Length < 9)
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs a csatorna megadva!");
							return;
						}
		
						if(Network.IMessage.Info.Length < 10)
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs rang megadva!");
							return;
						}

						var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM modelist WHERE Name = '{0}'", Network.IMessage.Info[7].ToLower());
						if(db != null)
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "A név már szerepel a mode listán!");
							return;
						}

						SchumixBase.DManager.QueryFirstRow("INSERT INTO `modelist`(Name, Channel, Rank) VALUES ('{0}', '{1}', '{2}')", Network.IMessage.Info[7].ToLower(), Network.IMessage.Info[8], Network.IMessage.Info[9]);
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Mode listához a név hozzáadva: {0}", Network.IMessage.Info[7]);
					}
					else if(Network.IMessage.Info[6].ToLower() == "del")
					{
						if(Network.IMessage.Info.Length < 8)
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs név megadva!");
							return;
						}
		
						var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM modelist WHERE Name = '{0}'", Network.IMessage.Info[7].ToLower());
						if(db == null)
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Ilyen név nem létezik!");
							return;
						}

						SchumixBase.DManager.QueryFirstRow("DELETE FROM `modelist` WHERE Name = '{0}'", Network.IMessage.Info[7].ToLower());
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Mode listából a név eltávólításra került: {0}", Network.IMessage.Info[7]);
					}
					else if(Network.IMessage.Info[6].ToLower() == "info")
					{
						var db = SchumixBase.DManager.Query("SELECT Name, Channel FROM modelist");
						if(db != null)
						{
							string Nevek = string.Empty;

							for(int i = 0; i < db.Rows.Count; ++i)
							{
								var row = db.Rows[i];
								string nev = row["Name"].ToString();
								string csatorna = row["Channel"].ToString();
								Nevek += ", " + nev + ":" + csatorna;
							};

							if(Nevek.Length > 1 && Nevek.Substring(0, 2) == ", ")
								Nevek = Nevek.Remove(0, 2);

							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Mode listán lévők: {0}", Nevek);
						}
						else
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Hibás lekérdezés!");
					}
				}
			}
			else if(Network.IMessage.Info[4].ToLower() == "hluzenet")
			{
				if(Network.IMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs megadva az első paraméter!");
					return;
				}

				if(Network.IMessage.Info[5].ToLower() == "info")
				{
					var db = SchumixBase.DManager.Query("SELECT Name, Enabled FROM hlmessage");
					if(db != null)
					{
						string Nevek = string.Empty;

						for(int i = 0; i < db.Rows.Count; ++i)
						{
							var row = db.Rows[i];
							string nev = row["Name"].ToString();
							string allapot = row["Enabled"].ToString();
							Nevek += ", " + nev + ":" + allapot;
						}

						if(Nevek.Length > 1 && Nevek.Substring(0, 2) == ", ")
							Nevek = Nevek.Remove(0, 2);

						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "3Létező nickek: {0}", Nevek);
					}
					else
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Hibás lekérdezés!");
				}
				else if(Network.IMessage.Info[5].ToLower() == "update")
				{
					var db = SchumixBase.DManager.Query("SELECT Name FROM adminok");
					if(db != null)
					{
						for(int i = 0; i < db.Rows.Count; ++i)
						{
							var row = db.Rows[i];
							string nev = row["Name"].ToString();

							var db1 = SchumixBase.DManager.QueryFirstRow("SELECT* FROM hlmessage WHERE Name = '{0}'", nev);
							if(db1 == null)
								SchumixBase.DManager.QueryFirstRow("INSERT INTO `hlmessage`(Name, Enabled) VALUES ('{0}', 'ki')", nev);
						}

						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Az adatbázis sikeresen frissitésre került.");
					}
					else
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Hibás lekérdezés!");
				}
				else if(Network.IMessage.Info[5].ToLower() == "funkcio")
				{
					if(Network.IMessage.Info.Length < 7)
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs a funkció név megadva!");
						return;
					}

					string nev = Network.IMessage.Nick.ToLower();
					SchumixBase.DManager.QueryFirstRow("UPDATE `hlmessage` SET `Enabled` = '{0}' WHERE Name = '{1}'", Network.IMessage.Info[6], nev);
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "{0}: {1}kapcsolva", nev, Network.IMessage.Info[6]);
				}
				else
				{
					string adat = string.Empty;
					for(int i = 5; i < Network.IMessage.Info.Length; i++)
						adat += " " + Network.IMessage.Info[i];

					if(adat.Substring(0, 1) == " ")
						adat = adat.Remove(0, 1);

					string nev = Network.IMessage.Nick.ToLower();
					SchumixBase.DManager.QueryFirstRow("UPDATE `hlmessage` SET `Info` = '{0}', `Enabled` = 'be' WHERE Name = '{1}'", adat, nev);
					SchumixBase.DManager.QueryFirstRow("UPDATE `schumix` SET `funkcio_status` = 'be' WHERE funkcio_nev = 'hl'");
					SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Functions = '{0}' WHERE Channel = '{1}'", Network.sChannelInfo.ChannelFunkciok("hl", "be", Network.IMessage.Channel), Network.IMessage.Channel);
					Network.sChannelInfo.ChannelFunkcioReload();
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Az üzenet módosításra került.");
				}
			}
		}
	}
}