/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013-2014 Schumix Team <http://schumix.eu/>
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
using Schumix.Framework.Irc;
using Schumix.Framework.Extensions;

namespace Schumix.Irc.Commands
{
	public abstract partial class CommandHandler
	{
		public void HandleAlias(IRCMessage sIRCMessage)
		{
			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;

			if(IsWarningAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("WarningAdmin", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
				return;

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoValue", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			if(sIRCMessage.Info[4].ToLower() == "command")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("No1Value", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "add")
				{
					var text = sLManager.GetCommandTexts("alias/command/add", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 8)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}

					if(sIRCMessage.Info.Length < 8)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoCommand", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					string newcommand = sIRCMessage.Info[6].ToLower();
					string basecommand = sIRCMessage.Info[7].ToLower();

					var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM alias_irc_command WHERE NewCommand = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(newcommand), sIRCMessage.ServerName);
					if(!db.IsNull())
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[1]);
						return;
					}

					var db1 = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM alias_irc_command WHERE NewCommand = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(basecommand), sIRCMessage.ServerName);
					if(!db1.IsNull())
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[2]);
						return;
					}

					if(sIrcBase.Networks[sIRCMessage.ServerName].CommandMethodMap.ContainsKey(newcommand))
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[3], newcommand);
						return;
					}

					if(!sIrcBase.Networks[sIRCMessage.ServerName].CommandMethodMap.ContainsKey(basecommand))
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[4], basecommand);
						return;
					}

					if(sIgnoreCommand.IsIgnore(basecommand))
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[5], basecommand);
						sSendMessage.SendChatMessage(sIRCMessage, text[6]);
						return;
					}

					sIrcBase.Networks[sIRCMessage.ServerName].SchumixRegisterHandler(newcommand, sIrcBase.Networks[sIRCMessage.ServerName].CommandMethodMap[basecommand].Method, sIrcBase.Networks[sIRCMessage.ServerName].CommandMethodMap[basecommand].Permission);
					SchumixBase.DManager.Insert("`alias_irc_command`(ServerId, ServerName, NewCommand, BaseCommand)", sIRCMessage.ServerId, sIRCMessage.ServerName, sUtilities.SqlEscape(newcommand), sUtilities.SqlEscape(basecommand));
					sSendMessage.SendChatMessage(sIRCMessage, text[7], newcommand, basecommand);
				}
				else if(sIRCMessage.Info[5].ToLower() == "remove")
				{
					var text = sLManager.GetCommandTexts("alias/command/remove", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoCommand", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					string command = sIRCMessage.Info[6].ToLower();

					var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM alias_irc_command WHERE NewCommand = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(command), sIRCMessage.ServerName);
					if(db.IsNull())
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}

					sIrcBase.Networks[sIRCMessage.ServerName].SchumixRemoveHandler(command, sIrcBase.Networks[sIRCMessage.ServerName].CommandMethodMap[command].Method);
					SchumixBase.DManager.Delete("alias_irc_command", string.Format("NewCommand = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(command), sIRCMessage.ServerName));
					sSendMessage.SendChatMessage(sIRCMessage, text[1], command);
				}
				else if(sIRCMessage.Info[5].ToLower() == "list")
				{
					var text = sLManager.GetCommandTexts("alias/command/list", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					var db = SchumixBase.DManager.Query("SELECT NewCommand, BaseCommand FROM alias_irc_command WHERE ServerName = '{0}'", sIRCMessage.ServerName);
					if(!db.IsNull())
					{
						string commandlist = string.Empty;

						foreach(DataRow row in db.Rows)
						{
							string newcommand = row["NewCommand"].ToString();
							string basecommand = row["BaseCommand"].ToString();
							commandlist += ", " + newcommand + "\u0002->\u000f" + basecommand;
						}

						if(commandlist.Length > 0)
							sSendMessage.SendChatMessage(sIRCMessage, text[0], commandlist.Remove(0, 2, ", "));
						else
							sSendMessage.SendChatMessage(sIRCMessage, text[1]);
					}
					else
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel, sIRCMessage.ServerName));
				}
			}
		}
	}
}