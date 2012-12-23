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
		//private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
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
				string name = sIRCMessage.Info.Length < 6 ? sIRCMessage.Nick.ToLower() : sUtilities.SqlEscape(sIRCMessage.Info[5].ToLower());
				var db = SchumixBase.DManager.QueryFirstRow("SELECT Enabled, Month, Day FROM birthday WHERE Name = '{0}' And ServerName = '{1}'", name, sIRCMessage.ServerName);
				if(!db.IsNull())
				{
					bool enabled = Convert.ToBoolean(db["Enabled"].ToString());
					int month = Convert.ToInt32(db["Month"].ToString());
					int day = Convert.ToInt32(db["Day"].ToString());

					sSendMessage.SendChatMessage(sIRCMessage, "3Születésnap funkció állapota: {0}", enabled);
					sSendMessage.SendChatMessage(sIRCMessage, "3Születésnap időpontja: 2[Hónap] {0}, 2[Nap] {1}", month, day);
				}
				else
				{
					if(sIRCMessage.Nick.ToLower() == name)
						sSendMessage.SendChatMessage(sIRCMessage, "Nem vagy regisztrálva!");
					else
						sSendMessage.SendChatMessage(sIRCMessage, "Nincs regisztrálva!");
				}
			}
			else if(sIRCMessage.Info[4].ToLower() == "change")
			{
				// status (parancs)
				// új születésnap (parancs)
			}
			else if(sIRCMessage.Info[4].ToLower() == "register")
			{
				var db = SchumixBase.DManager.QueryFirstRow("SELECT * FROM birthday WHERE Name = '{0}' And ServerName = '{1}'", sIRCMessage.Nick.ToLower(), sIRCMessage.ServerName);
				if(!db.IsNull())
				{
					sSendMessage.SendChatMessage(sIRCMessage, "Már regisztrálva vagy!");
					return;
				}

				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, "Nincs megadva a születési hónap!");
					return;
				}

				if(sIRCMessage.Info.Length < 7)
				{
					sSendMessage.SendChatMessage(sIRCMessage, "Nincs megadva a születési nap!");
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
				sSendMessage.SendChatMessage(sIRCMessage, "Sikeresen hozzáadásra került a születésnapod.");
			}
			else if(sIRCMessage.Info[4].ToLower() == "remove")
			{
				var db = SchumixBase.DManager.QueryFirstRow("SELECT * FROM birthday WHERE Name = '{0}' And ServerName = '{1}'", sIRCMessage.Nick.ToLower(), sIRCMessage.ServerName);
				if(db.IsNull())
				{
					sSendMessage.SendChatMessage(sIRCMessage, "Nem szerepelsz a listán!");
					return;
				}

				SchumixBase.DManager.Delete("birthday", string.Format("Name = '{0}' And ServerName = '{1}'", sIRCMessage.Nick.ToLower(), sIRCMessage.ServerName));
				sSendMessage.SendChatMessage(sIRCMessage, "Törölve lett a születésnapod!");
			}
		}
	}
}