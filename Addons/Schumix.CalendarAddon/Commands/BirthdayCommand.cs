/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Megax <http://www.megaxx.info/>
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
using Schumix.API;
using Schumix.API.Irc;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using Schumix.CalendarAddon;

namespace Schumix.CalendarAddon.Commands
{
	class BirthdayCommand : CommandInfo
	{
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;

		public BirthdayCommand(string ServerName) : base(ServerName)
		{

		}

		public void HandleBirthday(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
				return;

			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoValue", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			if(sIRCMessage.Info[4].ToLower() == "info")
			{
				var text = sLManager.GetCommandTexts("birthday/info", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 4)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}

				string name = sIRCMessage.Info.Length < 6 ? sIRCMessage.Nick.ToLower() : sUtilities.SqlEscape(sIRCMessage.Info[5].ToLower());
				var db = SchumixBase.DManager.QueryFirstRow("SELECT Enabled, Month, Day FROM birthday WHERE Name = '{0}' And ServerName = '{1}'", name, sIRCMessage.ServerName);
				if(!db.IsNull())
				{
					bool enabled = Convert.ToBoolean(db["Enabled"].ToString());
					int month = Convert.ToInt32(db["Month"].ToString());
					int day = Convert.ToInt32(db["Day"].ToString());

					sSendMessage.SendChatMessage(sIRCMessage, text[0], enabled ? SchumixBase.On : SchumixBase.Off);
					sSendMessage.SendChatMessage(sIRCMessage, text[1], month, day);
				}
				else
				{
					if(sIRCMessage.Nick.ToLower() == name)
						sSendMessage.SendChatMessage(sIRCMessage, text[2]);
					else
						sSendMessage.SendChatMessage(sIRCMessage, text[3]);
				}
			}
			else if(sIRCMessage.Info[4].ToLower() == "change")
			{
				if(!Warning(sIRCMessage))
					return;

				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoValue", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "status")
				{
					var text = sLManager.GetCommandTexts("birthday/change/status", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoFunctionStatus", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					string status = sIRCMessage.Info[6].ToLower();
					if(status == SchumixBase.On || status == SchumixBase.Off)
					{
						SchumixBase.DManager.Update("birthday", string.Format("Enabled = '{0}'", status == SchumixBase.On), string.Format("Name = '{0}' And ServerName = '{1}'", sIRCMessage.Nick.ToLower(), sIRCMessage.ServerName));
	
						if(status == SchumixBase.On)
							sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						else
							sSendMessage.SendChatMessage(sIRCMessage, text[1]);
					}
					else
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("WrongSwitch", sIRCMessage.Channel, sIRCMessage.ServerName));
				}
				else if(sIRCMessage.Info[5].ToLower() == "birthday")
				{
					var text = sLManager.GetCommandTexts("birthday/change/birthday", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 3)
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
						sSendMessage.SendChatMessage(sIRCMessage, text[1]);
						return;
					}

					int month = sIRCMessage.Info[6].ToNumber(13).ToInt();
					if(month > 12 || month <= 0)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMonth", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					int day = sIRCMessage.Info[7].ToNumber(32).ToInt();
					if(!sUtilities.IsDay(2012, month, day))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorDay", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					SchumixBase.DManager.Update("birthday", string.Format("Month = '{0}', Day = '{1}'", month, day), string.Format("Name = '{0}' And ServerName = '{1}'", sIRCMessage.Nick.ToLower(), sIRCMessage.ServerName));
					sSendMessage.SendChatMessage(sIRCMessage, text[2]);
				}
			}
			else if(sIRCMessage.Info[4].ToLower() == "register")
			{
				var text = sLManager.GetCommandTexts("birthday/register", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 4)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}

				var db = SchumixBase.DManager.QueryFirstRow("SELECT * FROM birthday WHERE Name = '{0}' And ServerName = '{1}'", sIRCMessage.Nick.ToLower(), sIRCMessage.ServerName);
				if(!db.IsNull())
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[0]);
					return;
				}

				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
					return;
				}

				if(sIRCMessage.Info.Length < 7)
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[2]);
					return;
				}

				int month = sIRCMessage.Info[5].ToNumber(13).ToInt();
				if(month > 12 || month <= 0)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMonth", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				int day = sIRCMessage.Info[6].ToNumber(32).ToInt();
				if(!sUtilities.IsDay(2012, month, day))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorDay", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				SchumixBase.DManager.Insert("`birthday`(ServerId, ServerName, Name, Month, Day)", IRCConfig.List[sIRCMessage.ServerName].ServerId, sIRCMessage.ServerName, sIRCMessage.Nick.ToLower(), month, day);
				sSendMessage.SendChatMessage(sIRCMessage, text[3]);
			}
			else if(sIRCMessage.Info[4].ToLower() == "remove")
			{
				var text = sLManager.GetCommandTexts("birthday/remove", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 2)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}

				var db = SchumixBase.DManager.QueryFirstRow("SELECT * FROM birthday WHERE Name = '{0}' And ServerName = '{1}'", sIRCMessage.Nick.ToLower(), sIRCMessage.ServerName);
				if(db.IsNull())
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[0]);
					return;
				}

				SchumixBase.DManager.Delete("birthday", string.Format("Name = '{0}' And ServerName = '{1}'", sIRCMessage.Nick.ToLower(), sIRCMessage.ServerName));
				sSendMessage.SendChatMessage(sIRCMessage, text[1]);
			}
		}

		private bool Warning(IRCMessage sIRCMessage)
		{
			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;
			var db = SchumixBase.DManager.QueryFirstRow("SELECT * FROM birthday WHERE Name = '{0}' And ServerName = '{1}'", sIRCMessage.Nick.ToLower(), sIRCMessage.ServerName);
			if(db.IsNull())
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("birthday", sIRCMessage.Channel, sIRCMessage.ServerName));
				return false;
			}

			return true;
		}
	}
}