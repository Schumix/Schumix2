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
using System.Threading;
using Schumix.Irc;
using Schumix.Framework;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.CalendarAddon
{
	sealed class CalendarFunctions
	{
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private CalendarFunctions() {}

		public string Add(string name, string channel, string message, DateTime time, bool Loop = false)
		{
			if(Loop)
				SchumixBase.DManager.Insert("`calendar`(Name, Channel, Message, Loops, Year, Month, Day, Hour, Minute)", sUtilities.SqlEscape(name.ToLower()), channel.ToLower(), sUtilities.SqlEscape(message), true, 0, 0, 0, time.Hour, time.Minute);
			else
				SchumixBase.DManager.Insert("`calendar`(Name, Channel, Message, Year, Month, Day, Hour, Minute)", sUtilities.SqlEscape(name.ToLower()), channel.ToLower(), sUtilities.SqlEscape(message), time.Year, time.Month, time.Day, time.Hour, time.Minute);

			return sLManager.GetWarningText("Calendar", channel);
		}

		public string Add(string name, string channel, string message, int hour, int minute, bool Loop = false)
		{
			var time = DateTime.Now;
			SchumixBase.DManager.Insert("`calendar`(Name, Channel, Message, Year, Month, Day, Hour, Minute)", sUtilities.SqlEscape(name.ToLower()), channel.ToLower(), sUtilities.SqlEscape(message), time.Year, time.Month, time.Day, hour, minute);
			return sLManager.GetWarningText("Calendar", channel);
		}

		public string Add(string name, string channel, string message, int year, int month, int day, int hour, int minute, bool Loop = false)
		{
			SchumixBase.DManager.Insert("`calendar`(Name, Channel, Message, Year, Month, Day, Hour, Minute)", sUtilities.SqlEscape(name.ToLower()), channel.ToLower(), sUtilities.SqlEscape(message), year, month, day, hour, minute);
			return sLManager.GetWarningText("Calendar", channel);
		}

		public void Write(string Name, string Channel, string Message)
		{
			sSendMessage.SendCMPrivmsg(Channel, string.Format("Feljegyzett üzenet {0} számára!", Name));
			sSendMessage.SendCMPrivmsg(Channel, Message);
			Thread.Sleep(400);
		}

		public void Remove(int Id)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM calendar WHERE Id = '{0}'", Id);
			if(db.IsNull())
				return;

			SchumixBase.DManager.Delete("calendar", string.Format("Id = '{0}'", Id));
		}
	}
}