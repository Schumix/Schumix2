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
using Schumix.Irc.Util;
using Schumix.Framework;
using Schumix.Framework.Irc;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;

namespace Schumix.Irc.Commands
{
	public abstract partial class CommandHandler
	{
		protected void HandleAdmin(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick))
				return;

			bool status = true;

			if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "access")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoPassword", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				var text = sLManager.GetCommandTexts("admin/access", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 4)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}

				string name = sIRCMessage.Nick.ToLower();
				var db = SchumixBase.DManager.QueryFirstRow("SELECT Password FROM admins WHERE Name = '{0}' And ServerName = '{1}'", name, sIRCMessage.ServerName);
				if(!db.IsNull())
				{
					if(db["Password"].ToString() == sUtilities.Sha1(sIRCMessage.Info[5]))
					{
						SchumixBase.DManager.Update("admins", string.Format("Vhost = '{0}'", sIRCMessage.Host), string.Format("Name = '{0}' And ServerName = '{1}'", name, sIRCMessage.ServerName));
						UpdateLCDB("UPDATE admins SET {0} WHERE {1}", string.Format("Vhost = '{0}'", sIRCMessage.Host), string.Format("Name = '{0}' And ServerName = '{1}'", name, sIRCMessage.ServerName));
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
					}
					else
						sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				}

				if(!sChannelList.IsChannelList(name))
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[2]);
					sSendMessage.SendChatMessage(sIRCMessage, text[3]);
					sChannelList.NewThread(sIRCMessage.ServerName, name);
				}

				status = false;
			}
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "newpassword")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoOldPassword", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sIRCMessage.Info.Length < 7)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoNewPassword", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				var text = sLManager.GetCommandTexts("admin/newpassword", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 2)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}

				string name = sIRCMessage.Nick.ToLower();
				var db = SchumixBase.DManager.QueryFirstRow("SELECT Password FROM admins WHERE Name = '{0}' And ServerName = '{1}'", name, sIRCMessage.ServerName);
				if(!db.IsNull())
				{
					if(db["Password"].ToString() == sUtilities.Sha1(sIRCMessage.Info[5]))
					{
						SchumixBase.DManager.Update("admins", string.Format("Password = '{0}'", sUtilities.Sha1(sIRCMessage.Info[6])), string.Format("Name = '{0}' And ServerName = '{1}'", name, sIRCMessage.ServerName));
						UpdateLCDB("UPDATE admins SET {0} WHERE {1}", string.Format("Password = '{0}'", sUtilities.Sha1(sIRCMessage.Info[6])), string.Format("Name = '{0}' And ServerName = '{1}'", name, sIRCMessage.ServerName));
						sSendMessage.SendChatMessage(sIRCMessage, text[0], sIRCMessage.Info[6]);
					}
					else
						sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				}

				status = false;
			}

			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.HalfOperator))
				return;

			if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "info")
			{
				var text = sLManager.GetCommandTexts("admin/info", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 3)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}

				string name = sIRCMessage.Nick;

				if(sIRCMessage.Info.Length >= 6)
					name = sIRCMessage.Info[5];

				if(!Rfc2812Util.IsValidNick(name))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaNickNameHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				var db = SchumixBase.DManager.QueryFirstRow("SELECT Flag FROM admins WHERE Name = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(name.ToLower()), sIRCMessage.ServerName);
				int flag = !db.IsNull() ? db["Flag"].ToInt32() : -1;

				if((AdminFlag)flag == AdminFlag.HalfOperator)
					sSendMessage.SendChatMessage(sIRCMessage, text[0]);
				else if((AdminFlag)flag == AdminFlag.Operator)
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				else if((AdminFlag)flag == AdminFlag.Administrator)
					sSendMessage.SendChatMessage(sIRCMessage, text[2]);
			}
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "list")
			{
				var db = SchumixBase.DManager.Query("SELECT Name FROM admins WHERE ServerName = '{0}'", sIRCMessage.ServerName);
				if(!db.IsNull())
				{
					string admins = string.Empty;

					foreach(DataRow row in db.Rows)
					{
						string name = row["Name"].ToString();
						admins += ", " + name;
					}

					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("admin/list", sIRCMessage.Channel, sIRCMessage.ServerName), admins.Remove(0, 2, ", "));
				}
				else
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel, sIRCMessage.ServerName));
			}
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "add")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				var text = sLManager.GetCommandTexts("admin/add", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 5)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}

				string name = sIRCMessage.Info[5];
				if(!Rfc2812Util.IsValidNick(name))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaNickNameHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM admins WHERE Name = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(name.ToLower()), sIRCMessage.ServerName);
				if(!db.IsNull())
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[0]);
					return;
				}

				string pass = sUtilities.GetRandomString();
				SchumixBase.DManager.Insert("`admins`(ServerId, ServerName, Name, Password)", sIRCMessage.ServerId, sIRCMessage.ServerName, sUtilities.SqlEscape(name.ToLower()), sUtilities.Sha1(pass));
				UpdateLCDB("INSERT INTO `admins`(ServerId, ServerName, Name, Password) VALUES ('{0}', '{1}', '{2}', '{3}')", sIRCMessage.ServerId, sIRCMessage.ServerName, sUtilities.SqlEscape(name.ToLower()), sUtilities.Sha1(pass));

				if(SchumixBase.DManager.IsCreatedTable("hlmessage"))
					SchumixBase.DManager.Insert("`hlmessage`(ServerId, ServerName, Name, Enabled)", sIRCMessage.ServerId, sIRCMessage.ServerName, sUtilities.SqlEscape(name.ToLower()), "off");

				sSendMessage.SendChatMessage(sIRCMessage, text[1], name);
				sSendMessage.SendChatMessage(sIRCMessage.MessageType, name, text[2], pass);
				sSendMessage.SendChatMessage(sIRCMessage.MessageType, name, text[3], IRCConfig.List[sIRCMessage.ServerName].CommandPrefix);
				sSendMessage.SendChatMessage(sIRCMessage.MessageType, name, text[4], IRCConfig.List[sIRCMessage.ServerName].CommandPrefix);
			}
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "remove")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				var text = sLManager.GetCommandTexts("admin/remove", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 2)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}

				string name = sIRCMessage.Info[5];
				if(!Rfc2812Util.IsValidNick(name))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaNickNameHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM admins WHERE Name = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(name.ToLower()), sIRCMessage.ServerName);
				if(db.IsNull())
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[0]);
					return;
				}

				if(IsAdmin(sIRCMessage.Nick, AdminFlag.HalfOperator) && IsAdmin(name, AdminFlag.Operator))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoOperator", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(IsAdmin(sIRCMessage.Nick, AdminFlag.HalfOperator) && IsAdmin(name, AdminFlag.Administrator))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoAdministrator", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(IsAdmin(sIRCMessage.Nick, AdminFlag.Operator) && IsAdmin(name, AdminFlag.Administrator))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoAdministrator", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				SchumixBase.DManager.Delete("admins", string.Format("Name = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(name.ToLower()), sIRCMessage.ServerName));
				UpdateLCDB("DELETE FROM admins WHERE {0}", string.Format("Name = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(name.ToLower()), sIRCMessage.ServerName));

				if(SchumixBase.DManager.IsCreatedTable("hlmessage"))
					SchumixBase.DManager.Delete("hlmessage", string.Format("Name = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(name.ToLower()), sIRCMessage.ServerName));

				if(SchumixBase.DManager.IsCreatedTable("birthday"))
				{
					var db1 = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM birthday WHERE Name = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(name.ToLower()), sIRCMessage.ServerName);
					if(!db1.IsNull())
						SchumixBase.DManager.Delete("birthday", string.Format("Name = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(name.ToLower()), sIRCMessage.ServerName));
				}

				sSendMessage.SendChatMessage(sIRCMessage, text[1], name);
			}
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "rank")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sIRCMessage.Info.Length < 7)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoRank", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				var text = sLManager.GetCommandTexts("admin/rank", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 2)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}

				string name = sIRCMessage.Info[5].ToLower();
				if(!Rfc2812Util.IsValidNick(name))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaNickNameHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(IsAdmin(sIRCMessage.Nick, AdminFlag.HalfOperator) && IsAdmin(name, AdminFlag.Operator))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoOperator", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(IsAdmin(sIRCMessage.Nick, AdminFlag.HalfOperator) && IsAdmin(name, AdminFlag.Administrator))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoAdministrator", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(IsAdmin(sIRCMessage.Nick, AdminFlag.Operator) && IsAdmin(name, AdminFlag.Administrator))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoAdministrator", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				int rank = sIRCMessage.Info[6].ToInt32();

				if(IsAdmin(sIRCMessage.Nick, AdminFlag.HalfOperator) && IsAdmin(name, AdminFlag.HalfOperator) && (AdminFlag)rank == AdminFlag.Operator)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoOperator", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(IsAdmin(sIRCMessage.Nick, AdminFlag.HalfOperator) && IsAdmin(name, AdminFlag.HalfOperator) && (AdminFlag)rank == AdminFlag.Administrator)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoAdministrator", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(IsAdmin(sIRCMessage.Nick, AdminFlag.Operator) && IsAdmin(name, AdminFlag.Operator) && (AdminFlag)rank == AdminFlag.Administrator)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoAdministrator", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}
		
				if((AdminFlag)rank == AdminFlag.Administrator || (AdminFlag)rank == AdminFlag.Operator || (AdminFlag)rank == AdminFlag.HalfOperator)
				{
					SchumixBase.DManager.Update("admins", string.Format("Flag = '{0}'", rank), string.Format("Name = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(name), sIRCMessage.ServerName));
					UpdateLCDB("UPDATE admins SET {0} WHERE {1}", string.Format("Flag = '{0}'", rank), string.Format("Name = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(name), sIRCMessage.ServerName));
					sSendMessage.SendChatMessage(sIRCMessage, text[0]);
				}
				else
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
			}
			else
			{
				if(!status)
					return;

				var text = sLManager.GetCommandTexts("admin", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}

				if(IsAdmin(sIRCMessage.Nick, AdminFlag.HalfOperator))
				{
					string commands = string.Empty;

					foreach(var command in sIrcBase.Networks[sIRCMessage.ServerName].CommandMethodMap)
					{
						if(command.Value.Permission != CommandPermission.HalfOperator)
							continue;

						if(command.Key == "admin")
							continue;

						if(sIgnoreCommand.IsIgnore(command.Key))
							continue;

						commands += " | " + IRCConfig.List[sIRCMessage.ServerName].CommandPrefix + command.Key;
					}

					sSendMessage.SendChatMessage(sIRCMessage, text[0]);
					sSendMessage.SendChatMessage(sIRCMessage, text[1], commands.Remove(0, 3, " | "));
				}
				else if(IsAdmin(sIRCMessage.Nick, AdminFlag.Operator))
				{
					string commands = string.Empty;

					foreach(var command in sIrcBase.Networks[sIRCMessage.ServerName].CommandMethodMap)
					{
						if(command.Value.Permission == CommandPermission.Normal || 
						   command.Value.Permission == CommandPermission.Administrator)
							continue;

						if(command.Key == "admin")
							continue;

						if(sIgnoreCommand.IsIgnore(command.Key))
							continue;

						commands += " | " + IRCConfig.List[sIRCMessage.ServerName].CommandPrefix + command.Key;
					}

					sSendMessage.SendChatMessage(sIRCMessage, text[2]);
					sSendMessage.SendChatMessage(sIRCMessage, text[3], commands.Remove(0, 3, " | "));
				}
				else if(IsAdmin(sIRCMessage.Nick, AdminFlag.Administrator))
				{
					string commands = string.Empty;

					foreach(var command in sIrcBase.Networks[sIRCMessage.ServerName].CommandMethodMap)
					{
						if(command.Value.Permission == CommandPermission.Normal)
							continue;

						if(command.Key == "admin")
							continue;

						if(sIgnoreCommand.IsIgnore(command.Key))
							continue;

						commands += " | " + IRCConfig.List[sIRCMessage.ServerName].CommandPrefix + command.Key;
					}

					sSendMessage.SendChatMessage(sIRCMessage, text[4]);
					sSendMessage.SendChatMessage(sIRCMessage, text[5], commands.Remove(0, 3, " | "));
				}
			}
		}
	}
}