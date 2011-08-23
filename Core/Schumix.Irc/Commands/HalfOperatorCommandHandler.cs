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
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;

namespace Schumix.Irc.Commands
{
	public partial class CommandHandler
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
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoPassword", sIRCMessage.Channel));
					return;
				}

				var text = sLManager.GetCommandTexts("admin/access", sIRCMessage.Channel);
				if(text.Length < 2)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
					return;
				}

				string name = sIRCMessage.Nick.ToLower();
				var db = SchumixBase.DManager.QueryFirstRow("SELECT Password FROM admins WHERE Name = '{0}'", name);
				if(!db.IsNull())
				{
					if(db["Password"].ToString() == sUtilities.Sha1(sIRCMessage.Info[5]))
					{
						SchumixBase.DManager.Update("admins", string.Format("Vhost = '{0}'", sIRCMessage.Host), string.Format("Name = '{0}'", name));
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
					}
					else
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1]);
				}

				status = false;
			}
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "newpassword")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoOldPassword", sIRCMessage.Channel));
					return;
				}

				if(sIRCMessage.Info.Length < 7)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoNewPassword", sIRCMessage.Channel));
					return;
				}

				var text = sLManager.GetCommandTexts("admin/newpassword", sIRCMessage.Channel);
				if(text.Length < 2)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
					return;
				}

				string name = sIRCMessage.Nick.ToLower();
				var db = SchumixBase.DManager.QueryFirstRow("SELECT Password FROM admins WHERE Name = '{0}'", name);
				if(!db.IsNull())
				{
					if(db["Password"].ToString() == sUtilities.Sha1(sIRCMessage.Info[5]))
					{
						SchumixBase.DManager.Update("admins", string.Format("Password = '{0}'", sUtilities.Sha1(sIRCMessage.Info[6])), string.Format("Name = '{0}'", name));
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0], sIRCMessage.Info[6]);
					}
					else
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1]);
				}

				status = false;
			}

			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.HalfOperator))
				return;

			if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "info")
			{
				var text = sLManager.GetCommandTexts("admin/info", sIRCMessage.Channel);
				if(text.Length < 3)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
					return;
				}

				var db = SchumixBase.DManager.QueryFirstRow("SELECT Flag FROM admins WHERE Name = '{0}'", sIRCMessage.Nick.ToLower());
				int flag = !db.IsNull() ? Convert.ToInt32(db["Flag"].ToString()) : -1;

				if((AdminFlag)flag == AdminFlag.HalfOperator)
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
				else if((AdminFlag)flag == AdminFlag.Operator)
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1]);
				else if((AdminFlag)flag == AdminFlag.Administrator)
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[2]);
			}
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "list")
			{
				var db = SchumixBase.DManager.Query("SELECT Name FROM admins");
				if(!db.IsNull())
				{
					string admins = string.Empty;

					foreach(DataRow row in db.Rows)
					{
						string name = row["Name"].ToString();
						admins += ", " + name;
					}

					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("admin/list", sIRCMessage.Channel), admins.Remove(0, 2, ", "));
				}
				else
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel));
			}
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "add")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
					return;
				}

				var text = sLManager.GetCommandTexts("admin/add", sIRCMessage.Channel);
				if(text.Length < 5)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
					return;
				}

				string name = sIRCMessage.Info[5];
				var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM admins WHERE Name = '{0}'", sUtilities.SqlEscape(name.ToLower()));
				if(!db.IsNull())
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
					return;
				}

				string pass = sUtilities.GetRandomString();
				SchumixBase.DManager.Insert("`admins`(Name, Password)", sUtilities.SqlEscape(name.ToLower()), sUtilities.Sha1(pass));
				SchumixBase.DManager.Insert("`hlmessage`(Name, Enabled)", sUtilities.SqlEscape(name.ToLower()), "off");

				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1], name);
				sSendMessage.SendCMPrivmsg(name, text[2], pass);
				sSendMessage.SendCMPrivmsg(name, text[3], IRCConfig.CommandPrefix);
				sSendMessage.SendCMPrivmsg(name, text[4], IRCConfig.CommandPrefix);
			}
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "remove")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
					return;
				}

				var text = sLManager.GetCommandTexts("admin/remove", sIRCMessage.Channel);
				if(text.Length < 2)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
					return;
				}

				string name = sIRCMessage.Info[5];
				var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM admins WHERE Name = '{0}'", sUtilities.SqlEscape(name.ToLower()));
				if(db.IsNull())
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
					return;
				}

				if(IsAdmin(sIRCMessage.Nick, AdminFlag.HalfOperator) && IsAdmin(name, AdminFlag.Operator))
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoOperator", sIRCMessage.Channel));
					return;
				}

				if(IsAdmin(sIRCMessage.Nick, AdminFlag.HalfOperator) && IsAdmin(name, AdminFlag.Administrator))
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoAdministrator", sIRCMessage.Channel));
					return;
				}

				if(IsAdmin(sIRCMessage.Nick, AdminFlag.Operator) && IsAdmin(name, AdminFlag.Administrator))
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoAdministrator", sIRCMessage.Channel));
					return;
				}

				SchumixBase.DManager.Delete("admins", string.Format("Name = '{0}'", sUtilities.SqlEscape(name.ToLower())));
				SchumixBase.DManager.Delete("hlmessage", string.Format("Name = '{0}'", sUtilities.SqlEscape(name.ToLower())));
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1], name);
			}
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "rank")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
					return;
				}

				if(sIRCMessage.Info.Length < 7)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoRank", sIRCMessage.Channel));
					return;
				}

				var text = sLManager.GetCommandTexts("admin/rank", sIRCMessage.Channel);
				if(text.Length < 2)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
					return;
				}

				string name = sIRCMessage.Info[5].ToLower();
				if(IsAdmin(sIRCMessage.Nick, AdminFlag.HalfOperator) && IsAdmin(name, AdminFlag.Operator))
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoOperator", sIRCMessage.Channel));
					return;
				}

				if(IsAdmin(sIRCMessage.Nick, AdminFlag.HalfOperator) && IsAdmin(name, AdminFlag.Administrator))
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoAdministrator", sIRCMessage.Channel));
					return;
				}

				if(IsAdmin(sIRCMessage.Nick, AdminFlag.Operator) && IsAdmin(name, AdminFlag.Administrator))
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoAdministrator", sIRCMessage.Channel));
					return;
				}

				int rank = Convert.ToInt32(sIRCMessage.Info[6]);

				if(IsAdmin(sIRCMessage.Nick, AdminFlag.HalfOperator) && IsAdmin(name, AdminFlag.HalfOperator) && (AdminFlag)rank == AdminFlag.Operator)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoOperator", sIRCMessage.Channel));
					return;
				}

				if(IsAdmin(sIRCMessage.Nick, AdminFlag.HalfOperator) && IsAdmin(name, AdminFlag.HalfOperator) && (AdminFlag)rank == AdminFlag.Administrator)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoAdministrator", sIRCMessage.Channel));
					return;
				}

				if(IsAdmin(sIRCMessage.Nick, AdminFlag.Operator) && IsAdmin(name, AdminFlag.Operator) && (AdminFlag)rank == AdminFlag.Administrator)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoAdministrator", sIRCMessage.Channel));
					return;
				}
		
				if((AdminFlag)rank == AdminFlag.Administrator || (AdminFlag)rank == AdminFlag.Operator || (AdminFlag)rank == AdminFlag.HalfOperator)
				{
					SchumixBase.DManager.Update("admins", string.Format("Flag = '{0}'", rank), string.Format("Name = '{0}'", sUtilities.SqlEscape(name)));
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
				}
				else
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1]);
			}
			else
			{
				if(!status)
					return;

				var text = sLManager.GetCommandTexts("admin", sIRCMessage.Channel);
				if(text.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
					return;
				}

				if(IsAdmin(sIRCMessage.Nick, AdminFlag.HalfOperator))
				{
					string commands = string.Empty;

					foreach(var command in CommandManager.GetHalfOperatorCommandHandler())
					{
						if(command.Key == "admin")
							continue;

						commands += " | " + IRCConfig.CommandPrefix + command.Key;
					}

					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1], commands.Remove(0, 3, " | "));
				}
				else if(IsAdmin(sIRCMessage.Nick, AdminFlag.Operator))
				{
					string commands = string.Empty;

					foreach(var command in CommandManager.GetHalfOperatorCommandHandler())
					{
						if(command.Key == "admin")
							continue;

						commands += " | " + IRCConfig.CommandPrefix + command.Key;
					}

					foreach(var command in CommandManager.GetOperatorCommandHandler())
						commands += " | " + IRCConfig.CommandPrefix + command.Key;

					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[2]);
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[3], commands.Remove(0, 3, " | "));
				}
				else if(IsAdmin(sIRCMessage.Nick, AdminFlag.Administrator))
				{
					string commands = string.Empty;

					foreach(var command in CommandManager.GetHalfOperatorCommandHandler())
					{
						if(command.Key == "admin")
							continue;

						commands += " | " + IRCConfig.CommandPrefix + command.Key;
					}

					foreach(var command in CommandManager.GetOperatorCommandHandler())
						commands += " | " + IRCConfig.CommandPrefix + command.Key;

					foreach(var command in CommandManager.GetAdminCommandHandler())
						commands += " | " + IRCConfig.CommandPrefix + command.Key;

					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[4]);
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[5], commands.Remove(0, 3, " | "));
				}
			}
		}

		protected void HandleColors(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.HalfOperator))
				return;

			sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("colors", sIRCMessage.Channel));
		}

		protected void HandleNick(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.HalfOperator))
				return;

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
				return;
			}

			string nick = sIRCMessage.Info[4];
			sNickInfo.ChangeNick(nick);
			sSender.Nick(nick);
			sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("nick", sIRCMessage.Channel), nick);
		}

		protected void HandleJoin(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.HalfOperator))
				return;

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel));
				return;
			}

			if(!IsChannel(sIRCMessage.Info[4]))
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NotaChannelHasBeenSet", sIRCMessage.Channel));
				return;
			}

			ChannelPrivmsg = sIRCMessage.Channel;

			if(sIRCMessage.Info.Length == 5)
				sSender.Join(sIRCMessage.Info[4]);
			else if(sIRCMessage.Info.Length == 6)
				sSender.Join(sIRCMessage.Info[4], sIRCMessage.Info[5]);

			sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("join", sIRCMessage.Channel), sIRCMessage.Info[4]);
		}

		protected void HandleLeave(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.HalfOperator))
				return;

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel));
				return;
			}

			if(!IsChannel(sIRCMessage.Info[4]))
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NotaChannelHasBeenSet", sIRCMessage.Channel));
				return;
			}

			sSender.Part(sIRCMessage.Info[4]);
			sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("leave", sIRCMessage.Channel), sIRCMessage.Info[4]);
		}
	}
}