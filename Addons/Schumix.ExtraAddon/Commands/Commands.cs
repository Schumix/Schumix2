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
using Schumix.Framework.Config;

namespace Schumix.ExtraAddon.Commands
{
	public class Functions : CommandInfo
	{
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;
		//private readonly Sender sSender = Singleton<Sender>.Instance;

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
				/*if(res.size() < 3)
				{
					sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Nincs megadva az 1. paraméter!");
					res.clear();
					return;
				}
		
				if(res[2] == added)
				{
					if(res.size() < 4)
					{
						sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Nincs név megadva!");
						res.clear();
						return;
					}
		
					if(res.size() < 5)
					{
						sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Nincs ok megadva!");
						res.clear();
						return;
					}
		
					string alomany;
					int resAdat = res.size();
		
					for(int i = 4; i < resAdat; i++)
						alomany += " " + res[i];
		
					transform(res[3].begin(), res[3].end(), res[3].begin(), ::tolower);
					sVezerlo.GetSQLConn()->Query("INSERT INTO `kicklista`(nick, channel, oka) VALUES ('%s', '%s', '%s')", sVezerlo.GetSQLConn()->EscapeString(res[3]).c_str(), recvData.GetChannel(), sVezerlo.GetSQLConn()->EscapeString(alomany.substr(1)).c_str());
					sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Kick listához a név hozzáadva: %s", res[3].c_str());
				}
				else if(res[2] == delet)
				{
					if(res.size() < 4)
					{
						sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Nincs név megadva!");
						res.clear();
						return;
					}
		
					transform(res[3].begin(), res[3].end(), res[3].begin(), ::tolower);
					sVezerlo.GetSQLConn()->Query("DELETE FROM `kicklista` WHERE nick = '%s'", sVezerlo.GetSQLConn()->EscapeString(res[3]).c_str());
					sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Kick listából a név eltávólitva: %s", res[3].c_str());
		
				}
		
				else if(res[2] == INFO)
				{
					QueryResultPointer db = sVezerlo.GetSQLConn()->Query("SELECT nick, channel FROM kicklista WHERE channel = '%s'", recvData.GetChannel());
					if(db)
					{
						string Nevek;
		
						do
						{
							string nev = db->Fetch()[0].GetString();
							Nevek += ", " + nev + ":" + recvData.Channel;
						} while(db->NextRow());
		
						sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Kick listán lévök: %s", Nevek.substr(2).c_str());
					}
					else
						sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Hibás lekérdezés!");
				}
				else if(res[2] == "channel")
				{
					if(res.size() < 4)
					{
						sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Nincs megadva az 1. paraméter!");
						res.clear();
						return;
					}
		
					if(res[3] == added)
					{
						if(res.size() < 5)
						{
							sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Nincs név megadva!");
							res.clear();
							return;
						}
		
						if(res.size() < 6)
						{
							sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Nincs channel megadva!");
							res.clear();
							return;
						}
		
						if(res.size() < 7)
						{
							sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Nincs ok megadva!");
							res.clear();
							return;
						}
		
						string alomany;
						int resAdat = res.size();
		
						for(int i = 6; i < resAdat; i++)
							alomany += " " + res[i];
		
						transform(res[4].begin(), res[4].end(), res[4].begin(), ::tolower);
						sVezerlo.GetSQLConn()->Query("INSERT INTO `kicklista`(nick, channel, oka) VALUES ('%s', '%s', '%s')", sVezerlo.GetSQLConn()->EscapeString(res[4]).c_str(), sVezerlo.GetSQLConn()->EscapeString(res[5]).c_str(), sVezerlo.GetSQLConn()->EscapeString(alomany.substr(1)).c_str());
						sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Kick listához a név hozzáadva: %s", res[4].c_str());
					}
					else if(res[3] == delet)
					{
						if(res.size() < 5)
						{
							sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Nincs név megadva!");
							res.clear();
							return;
						}
		
						transform(res[4].begin(), res[4].end(), res[4].begin(), ::tolower);
						sVezerlo.GetSQLConn()->Query("DELETE FROM `kicklista` WHERE nick = '%s'", sVezerlo.GetSQLConn()->EscapeString(res[4]).c_str());
						sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Kick listából a név eltávólitva: %s", res[4].c_str());
					}
					else if(res[3] == INFO)
					{
						QueryResultPointer db = sVezerlo.GetSQLConn()->Query("SELECT nick, channel FROM kicklista");
						if(db)
						{
							string Nevek;
		
							do
							{
								string nev = db->Fetch()[0].GetString();
								string szoba = db->Fetch()[1].GetString();
								Nevek += ", " + nev + ":" + szoba;
							} while(db->NextRow());
		
							sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Kick listán lévők: %s", Nevek.substr(2).c_str());
						}
						else
							sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Hibás lekérdezés!");
					}
				}*/
			}
			else if(Network.IMessage.Info[4].ToLower() == "mode")
			{
				/*if(res.size() < 3)
				{
					sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Nincs megadva az 1. paraméter!");
					res.clear();
					return;
				}
		
				if(res[2] == added)
				{
					if(res.size() < 4)
					{
						sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Nincs név megadva!");
						res.clear();
						return;
					}
		
					if(res.size() < 5)
					{
						sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Nincs rang megadva!");
						res.clear();
						return;
					}
		
					transform(res[3].begin(), res[3].end(), res[3].begin(), ::tolower);
					sVezerlo.GetSQLConn()->Query("INSERT INTO `modelista`(nick, channel, rang) VALUES ('%s', '%s', '%s')", sVezerlo.GetSQLConn()->EscapeString(res[3]).c_str(), recvData.GetChannel(), sVezerlo.GetSQLConn()->EscapeString(res[4]).c_str());
					sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Mode listához a név hozzáadva: %s", res[3].c_str());
				}
				else if(res[2] == delet)
				{
					if(res.size() < 4)
					{
						sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Nincs név megadva!");
						res.clear();
						return;
					}
		
					transform(res[3].begin(), res[3].end(), res[3].begin(), ::tolower);
					sVezerlo.GetSQLConn()->Query("DELETE FROM `modelista` WHERE nick = '%s'", sVezerlo.GetSQLConn()->EscapeString(res[3]).c_str());
					sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Mode listából a név eltávólitva: %s", res[3].c_str());
				}
				else if(res[2] == INFO)
				{
					QueryResultPointer db = sVezerlo.GetSQLConn()->Query("SELECT nick FROM modelista WHERE channel = '%s'", recvData.GetChannel());
					if(db)
					{
						string Nevek;
		
						do
						{
							string nev = db->Fetch()[0].GetString();
							Nevek += ", " + nev + ":" + recvData.Channel;
						} while(db->NextRow());
		
						sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Mode listán lévök: %s", Nevek.substr(2).c_str());
					}
					else
						sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Hibás lekérdezés!");
				}
				else if(res[2] == "channel")
				{
					if(res.size() < 4)
					{
						sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Nincs megadva az 1. paraméter!");
						res.clear();
						return;
					}
		
					if(res[3] == added)
					{
						if(res.size() < 5)
						{
							sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Nincs név megadva!");
							res.clear();
							return;
						}
		
						if(res.size() < 6)
						{
							sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Nincs channel megadva!");
							res.clear();
							return;
						}
		
						if(res.size() < 7)
						{
							sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Nincs rang megadva!");
							res.clear();
							return;
						}
		
						transform(res[4].begin(), res[4].end(), res[4].begin(), ::tolower);
						sVezerlo.GetSQLConn()->Query("INSERT INTO `modelista`(nick, channel, rang) VALUES ('%s', '%s', '%s')", sVezerlo.GetSQLConn()->EscapeString(res[4]).c_str(), sVezerlo.GetSQLConn()->EscapeString(res[5]).c_str(), sVezerlo.GetSQLConn()->EscapeString(res[6]).c_str());
						sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Mode listához a név hozzáadva: %s", res[4].c_str());
					}
					else if(res[3] == delet)
					{
						if(res.size() < 5)
						{
							sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Nincs név megadva!");
							res.clear();
							return;
						}
		
						transform(res[4].begin(), res[4].end(), res[4].begin(), ::tolower);
						sVezerlo.GetSQLConn()->Query("DELETE FROM `modelista` WHERE nick = '%s'", sVezerlo.GetSQLConn()->EscapeString(res[3]).c_str());
						sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Mode listából a név eltávólitva: %s", res[4].c_str());
					}
					else if(res[3] == INFO)
					{
						QueryResultPointer db = sVezerlo.GetSQLConn()->Query("SELECT nick, channel FROM modelista");
						if(db)
						{
							string Nevek;
		
							do
							{
								string nev = db->Fetch()[0].GetString();
								string szoba = db->Fetch()[1].GetString();
								Nevek += ", " + nev + ":" + szoba;
							} while(db->NextRow());
		
							sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Mode listán lévök: %s", Nevek.substr(2).c_str());
						}
						else
							sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Hibás lekérdezés!");
					}
				}*/
			}
			else if(Network.IMessage.Info[4].ToLower() == "hluzenet")
			{
				if(Network.IMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs megadva az 1. paraméter!");
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
							string nev = row["Channel"].ToString();
							string allapot = row["Enabled"].ToString();
							Nevek += ", " + nev + ":" + allapot;
						}

						if(Nevek.Substring(0, 2) == ", ")
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

		public void HLUzenet()
		{
			if(Network.sChannelInfo.FSelect("hl") && Network.sChannelInfo.FSelect("hl", Network.IMessage.Channel))
			{
				for(int i = 3; i < Network.IMessage.Info.Length; i++)
				{
					if(i == 3)
					{
						if(Network.IMessage.Info[3].Substring(0, 1) == ":")
							Network.IMessage.Info[3] = Network.IMessage.Info[3].Remove(0, 1);
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT Info, Enabled FROM hlmessage WHERE Name = '{0}'", Network.IMessage.Info[i].ToLower());
					if(db != null)
					{
						string info = db["Info"].ToString();
						string allapot = db["Enabled"].ToString();

						if(allapot != "be")
							return;

						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "{0}", info);
						break;
					}
				}
			}
		}

		public void Help()
		{
			// Operátor parancsok segítségei
			if(Admin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Operator))
			{
				if(Network.IMessage.Info[4].ToLower() == "autofunkcio")
				{
					if(Network.IMessage.Info.Length < 6)
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Autómatikusan müködő kódrészek kezelése.");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Autofunkcio parancsai: kick | mode | hlfunkcio");
						return;
					}
		
					if(Network.IMessage.Info[5].ToLower() == "kick")
					{
						/*if(Network.IMessage.Info.Length < 7)
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Autómatikusan kirúgásra kerülő nick-ek kezelése.");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kick parancsai: add | del | info | channel");
							return;
						}
		
						if(Network.IMessage.Info[6].ToLower() == "add")
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kirúgandó nevének hozzáadása ahol tartozkodsz.");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}autofunkcio kick add <nev> <oka>", IRCConfig.CommandPrefix);
						}
						else if(Network.IMessage.Info[6].ToLower() == "del")
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kirúgandó nevének eltávolítása ahol tartozkodsz.");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}autofunkcio kick del <nev>", IRCConfig.CommandPrefix);
						}
						else if(Network.IMessage.Info[6].ToLower() == "info")
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kiirja a kirúgandok állapotát.");
						}
						else if(Network.IMessage.Info[6].ToLower() == "channel")
						{
							if(res.size() < 5)
							{
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Autómatikusan kirúgásra kerülő nick-ek kezelése megadot channelen.");
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kick channel parancsai: add | del | info");
								return;
							}
		
							if(res[4].ToLower() == "add")
							{
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kirúgandó nevének hozzáadása megadott channelen.");
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}autofunkcio kick channel add <nev> <channel> <oka>", IRCConfig.CommandPrefix);
							}
							else if(res[4].ToLower() == "del")
							{
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kirúgandó nevének eltávolítása megadott channelen.");
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}autofunkcio kick channel del <nev>", IRCConfig.CommandPrefix);
							}
							else if(res[4].ToLower() == "info")
							{
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kiirja a kirúgandok állapotát.");
							}
						}*/
					}
					else if(Network.IMessage.Info[5].ToLower() == "mode")
					{
						/*if(Network.IMessage.Info.Length < 7)
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Autómatikusan rangot kapó nick-ek kezelése.");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Mode parancsai: add | del | info | channel");
							return;
						}
		
						if(Network.IMessage.Info[6].ToLower() == "add")
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Rangot kapó nevének hozzáadása ahol tartozkodsz.");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}autofunkcio mode add <nev> <rang>", IRCConfig.CommandPrefix);
						}
						else if(Network.IMessage.Info[6].ToLower() == "del")
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Rangot kapó nevének eltávolítása ahol tartozkodsz.");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}autofunkcio mode del <nev>", IRCConfig.CommandPrefix);
						}
						else if(Network.IMessage.Info[6].ToLower() == "info")
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kiirja a rangot kapók állapotát.");
						}
						else if(Network.IMessage.Info[6].ToLower() == "channel")
						{
							if(res.size() < 5)
							{
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Autómatikusan rangot kapó nick-ek kezelése megadot channelen.");
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Mode channel parancsai: add | del | info");
								return;
							}
		
							if(res[4].ToLower() == "add")
							{
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Rangot kapó nevének hozzáadása megadott channelen.");
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}autofunkcio mode channel add <nev> <channel> <rang>", IRCConfig.CommandPrefix);
							}
							else if(res[4].ToLower() == "del")
							{
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Rangot kapó nevének eltávolítása megadott channelen.");
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}autofunkcio mode channel del <nev>", IRCConfig.CommandPrefix);
							}
							else if(res[4].ToLower() == "info")
							{
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kiirja a rangot kapók állapotát.");
							}
						}*/
					}
					else if(Network.IMessage.Info[5].ToLower() == "hluzenet")
					{
						if(Network.IMessage.Info.Length < 7)
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Autómatikusan hl-t kapó nick-ek kezelése.");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kick channel parancsai: funkcio | update | info");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}autofunkcio hluzenet <üzenet>", IRCConfig.CommandPrefix);
							return;
						}
		
						if(Network.IMessage.Info[6].ToLower() == "funkcio")
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Ezzel a parancsal állitható a hl állapota.");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}hluzenet funkcio <állapot>", IRCConfig.CommandPrefix);
						}
						else if(Network.IMessage.Info[6].ToLower() == "update")
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Frissiti az adatbázisban szereplő hl listát!");
						}
						else if(Network.IMessage.Info[6].ToLower() == "info")
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kiirja a hl-ek állapotát.");
						}
					}
				}
			}
		}
	}
}