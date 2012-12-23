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
using Schumix.Framework.Localization;
using Schumix.CalendarAddon;

namespace Schumix.CalendarAddon.Commands
{
	class BirthdayCommand : CommandInfo
	{
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		//private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		//private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private CalendarFunctions sCalendarFunctions;

		public BirthdayCommand(string ServerName) : base(ServerName)
		{
			sCalendarFunctions = new CalendarFunctions(ServerName);
		}

		/*protected void HandleSznap(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
				return;

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			// INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'sznap', '1', 'Kiírja a megadott név születésnapjának dátumát.\nHasználata: {0}sznap <név>');

			var db = SchumixBase.DManager.QueryFirstRow("SELECT nev, honap, nap FROM sznap WHERE nev = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[4]));
			if(!db.IsNull())
			{
				string name = db["nev"].ToString();
				string month = db["honap"].ToString();
				int day = Convert.ToInt32(db["nap"]);
				sSendMessage.SendChatMessage(sIRCMessage, "{0} születés napja: {1} {2}", name, month, day);
			}
			else
				sSendMessage.SendChatMessage(sIRCMessage, "Nincs ilyen ember!");
		}*/

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
				// kiírja hogy be van-e kapcsolva a születésnap
				// kiírja mikor lesz a születésnap
				// egy alparancs hogy látható legyen más születésnapja is (nevet kell hozzá megadni)
			}
			else if(sIRCMessage.Info[4].ToLower() == "change")
			{
				// status (parancs)
				// új születésnap (parancs)
			}
			else if(sIRCMessage.Info[4].ToLower() == "register")
			{
			}
		}
	}
}