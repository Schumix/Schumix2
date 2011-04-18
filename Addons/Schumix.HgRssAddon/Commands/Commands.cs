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
using Schumix.Framework.Extensions;

namespace Schumix.HgRssAddon.Commands
{
	public partial class RssCommand : CommandInfo
	{
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;

		public void HandleHg()
		{
			if(!IsAdmin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Operator))
				return;

			CNick();

			if(Network.IMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs paraméter!");
				return;
			}

			if(Network.IMessage.Info[4].ToLower() == "info")
			{
				var db = SchumixBase.DManager.Query("SELECT Name, Channel FROM hginfo");
				if(!db.IsNull())
				{
					for(int i = 0; i < db.Rows.Count; ++i)
					{
						var row = db.Rows[i];
						string nev = row["Name"].ToString();
						string[] csatorna = row["Channel"].ToString().Split(',');

						if(csatorna.Length < 1)
							return;

						string adat = string.Empty;

						for(int x = 0; x < csatorna.Length; x++)
							adat += " " + csatorna[x];

						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "3{0} Channel:2{1}", nev, adat);
					};
				}
				else
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Hibás lekérdezés!");
			}
			else if(Network.IMessage.Info[4].ToLower() == "lista")
			{
				var db = SchumixBase.DManager.Query("SELECT Name FROM hginfo");
				if(!db.IsNull())
				{
					string lista = string.Empty;

					for(int i = 0; i < db.Rows.Count; ++i)
					{
						var row = db.Rows[i];
						lista += " " + row["Name"].ToString();
					};

					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "2Lista:3{0}", lista);
				}
				else
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Hibás lekérdezés!");
			}
			else if(Network.IMessage.Info[4].ToLower() == "start")
			{
				/*if(res.size() < 3)
				{
					sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Nincs név megadva!");
					res.clear();
					return;
				}

				QueryResultPointer db = sVezerlo.GetSQLConn()->Query("SELECT id FROM svninfo WHERE nev = '%s'", sVezerlo.GetSQLConn()->EscapeString(res[2]).c_str());
				if(db)
				{
					sSvnInfo.NewThread(db->Fetch()[0].GetUInt32());
					sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "%s thread hozzáadva", res[2].c_str());
				}
				else
					sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Sikertelen hozzáadás!");*/
				//SvnRssAddon.RssList
			}
			else if(Network.IMessage.Info[4].ToLower() == "stop")
			{
				/*if(res.size() < 3)
				{
					sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Nincs név megadva!");
					res.clear();
					return;
				}

				QueryResultPointer db = sVezerlo.GetSQLConn()->Query("SELECT id FROM svninfo WHERE nev = '%s'", sVezerlo.GetSQLConn()->EscapeString(res[2]).c_str());
				if(db)
				{
					sSvnInfo.StopThread(db->Fetch()[0].GetUInt32());
					sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "%s thread leállitva", res[2].c_str());
				}
				else
					sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Sikertelen leállitás!");*/
			}
			else if(Network.IMessage.Info[4].ToLower() == "reload")
			{
				/*if(res.size() < 3)
				{
					sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Nincs név vagy all megadva!");
					res.clear();
					return;
				}

				if(res[2] == "all")
				{
					sSvnInfo.ReloadAllThread();
					sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "SvnInfo újrainditás kész.");
				}
				else
				{
					QueryResultPointer db = sVezerlo.GetSQLConn()->Query("SELECT id FROM svninfo WHERE nev = '%s'", sVezerlo.GetSQLConn()->EscapeString(res[2]).c_str());
					if(db)
					{
						sSvnInfo.ReloadThread(db->Fetch()[0].GetUInt32());
						sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "%s thread újrainditva.", res[2].c_str());
					}
					else
						sIRCSession.SendChatMessage(PRIVMSG, recvData.GetChannel(), "Sikertelen újrainditás!");
				}*/
			}
			else if(Network.IMessage.Info[4].ToLower() == "channel")
			{
				if(Network.IMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs megadva a parancs!");
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
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs a csatorna név megadva!");
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT Channel FROM hginfo WHERE Name = '{0}'", Network.IMessage.Info[6].ToLower());
					if(!db.IsNull())
					{
						string[] csatorna = db["Channel"].ToString().Split(',');
						string adat = string.Empty;

						for(int x = 0; x < csatorna.Length; x++)
							adat += "," + csatorna[x];

						if(adat.Length > 0 && adat.Substring(0, 1) == ",")
							adat = adat.Remove(0, 1);

						if(csatorna.Length == 1 && adat == string.Empty)
							adat += Network.IMessage.Info[7].ToLower();
						else
							adat += "," + Network.IMessage.Info[7].ToLower();

						SchumixBase.DManager.QueryFirstRow("UPDATE hginfo SET Channel = '{0}' WHERE Name = '{1}'", adat, Network.IMessage.Info[6].ToLower());
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Csatorna sikeresen hozzáadva.");
					}
					else
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nem létezik ilyen név!");
				}
				else if(Network.IMessage.Info[5].ToLower() == "del")
				{
					if(Network.IMessage.Info.Length < 7)
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs név megadva!");
						return;
					}

					if(Network.IMessage.Info.Length < 8)
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs a csatorna név megadva!");
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT Channel FROM hginfo WHERE Name = '{0}'", Network.IMessage.Info[6].ToLower());
					if(!db.IsNull())
					{
						string[] csatorna = db["Channel"].ToString().Split(',');
						string adat = string.Empty;

						for(int x = 0; x < csatorna.Length; x++)
						{
							if(csatorna[x] == Network.IMessage.Info[7].ToLower())
								continue;

							adat += "," + csatorna[x];
						}

						if(adat.Length > 0 && adat.Substring(0, 1) == ",")
							adat = adat.Remove(0, 1);

						SchumixBase.DManager.QueryFirstRow("UPDATE hginfo SET Channel = '{0}' WHERE Name = '{1}'", adat, Network.IMessage.Info[6].ToLower());
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Csatorna sikeresen törölve.");
					}
					else
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nem létezik ilyen név!");
				}
			}
		}
	}
}