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
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.HgRssAddon.Commands
{
	public partial class RssCommand : CommandInfo
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;

		public void HandleHg(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
				return;

			CNick(sIRCMessage);

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoValue", sIRCMessage.Channel));
				return;
			}

			if(sIRCMessage.Info[4].ToLower() == "info")
			{
				var db = SchumixBase.DManager.Query("SELECT Name, Channel FROM hginfo");
				if(!db.IsNull())
				{
					foreach(DataRow row in db.Rows)
					{
						string name = row["Name"].ToString();
						string[] channel = row["Channel"].ToString().Split(SchumixBase.Comma);
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("hg/info", sIRCMessage.Channel), name, channel.SplitToString(" "));
					}
				}
				else
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel));
			}
			else if(sIRCMessage.Info[4].ToLower() == "list")
			{
				var db = SchumixBase.DManager.Query("SELECT Name FROM hginfo");
				if(!db.IsNull())
				{
					string list = string.Empty;

					foreach(DataRow row in db.Rows)
						list += SchumixBase.Space + row["Name"].ToString();

					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("hg/list", sIRCMessage.Channel), list);
				}
				else
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel));
			}
			else if(sIRCMessage.Info[4].ToLower() == "start")
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
			else if(sIRCMessage.Info[4].ToLower() == "stop")
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
			else if(sIRCMessage.Info[4].ToLower() == "reload")
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
			else if(sIRCMessage.Info[4].ToLower() == "channel")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoCommand", sIRCMessage.Channel));
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "add")
				{
					var text = sLManager.GetCommandTexts("hg/channel/add", sIRCMessage.Channel);
					if(text.Length < 2)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
						return;
					}

					if(sIRCMessage.Info.Length < 8)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel));
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT Channel FROM hginfo WHERE Name = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()));
					if(!db.IsNull())
					{
						string[] channel = db["Channel"].ToString().Split(SchumixBase.Comma);
						string data = channel.SplitToString(SchumixBase.Comma);

						if(channel.Length == 1 && data == string.Empty)
							data += sIRCMessage.Info[7].ToLower();
						else
							data += SchumixBase.Comma + sIRCMessage.Info[7].ToLower();

						SchumixBase.DManager.QueryFirstRow("UPDATE hginfo SET Channel = '{0}' WHERE Name = '{1}'", data, sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()));
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
					}
					else
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1]);
				}
				else if(sIRCMessage.Info[5].ToLower() == "remove")
				{
					var text = sLManager.GetCommandTexts("hg/channel/remove", sIRCMessage.Channel);
					if(text.Length < 2)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
						return;
					}

					if(sIRCMessage.Info.Length < 8)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel));
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT Channel FROM hginfo WHERE Name = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()));
					if(!db.IsNull())
					{
						string[] channel = db["Channel"].ToString().Split(SchumixBase.Comma);
						string data = string.Empty;

						for(int x = 0; x < channel.Length; x++)
						{
							if(channel[x] == sIRCMessage.Info[7].ToLower())
								continue;

							data += SchumixBase.Comma + channel[x];
						}

						SchumixBase.DManager.QueryFirstRow("UPDATE hginfo SET Channel = '{0}' WHERE Name = '{1}'", data.Remove(0, 1, SchumixBase.Comma), sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()));
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
					}
					else
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1]);
				}
			}
		}
	}
}