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
using Schumix.API;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.CalendarAddon;
using Schumix.Framework;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.CalendarAddon.Commands
{
	public partial class BanCommand : CommandInfo
	{
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;
		private readonly Ban sBan = Singleton<Ban>.Instance;
		private readonly Unban sUnban = Singleton<Unban>.Instance;
		private BanCommand() {}

		public void HandleBan(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
				return;

			CNick(sIRCMessage);

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoTime", sIRCMessage.Channel));
				return;
			}

			if(sIRCMessage.Info.Length < 6)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoBanNameOrVhost", sIRCMessage.Channel));
				return;
			}

			if(sIRCMessage.Info[5].Contains(":"))
			{
				if(sIRCMessage.Info.Length < 7)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoTime", sIRCMessage.Channel));
					return;
				}

				int hour = Convert.ToInt32(sIRCMessage.Info[5].Substring(0, sIRCMessage.Info[5].IndexOf(":")));
				if(hour > 24)
					return;

				int minute = Convert.ToInt32(sIRCMessage.Info[5].Substring(sIRCMessage.Info[5].IndexOf(":")+1));
				if(minute > 60)
					return;

				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sBan.BanName(sIRCMessage.Info[4].ToLower(), sIRCMessage.Channel, sIRCMessage.Info.SplitToString(6, " "), hour, minute));
			}
			else
			{
				if(sIRCMessage.Info.Length < 7)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoBanNameOrVhost", sIRCMessage.Channel));
					return;
				}

				if(sIRCMessage.Info.Length < 8)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoTime", sIRCMessage.Channel));
					return;
				}

				string[] s = sIRCMessage.Info[5].Split('.');
				if(s.Length < 3)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("ban", sIRCMessage.Channel));
					return;
				}

				int ev = Convert.ToInt32(s[0]);
				int month = Convert.ToInt32(s[1]);
				if(month > 12)
					return;

				int day = Convert.ToInt32(s[2]);
				if(day > 31)
					return;

				int hour = Convert.ToInt32(sIRCMessage.Info[6].Substring(0, sIRCMessage.Info[6].IndexOf(":")));
				if(hour > 24)
					return;

				int minute = Convert.ToInt32(sIRCMessage.Info[6].Substring(sIRCMessage.Info[6].IndexOf(":")+1));
				if(minute > 60)
					return;

				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sBan.BanName(sIRCMessage.Info[4].ToLower(), sIRCMessage.Channel, sIRCMessage.Info.SplitToString(7, " "), ev, month, day, hour, minute));
			}
		}

		public void HandleUnban(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
				return;

			CNick(sIRCMessage);

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoUnbanNameOrVhost", sIRCMessage.Channel));
				return;
			}

			sUnban.UnbanName(sIRCMessage.Info[4], sIRCMessage.Channel);
		}
	}
}