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
using Schumix.Irc;
using Schumix.Framework;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.CalendarAddon
{
	sealed class Ban
	{
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly Sender sSender = Singleton<Sender>.Instance;
		private Ban() {}

		public string BanName(string name, string channel, string reason, DateTime time)
		{
			if(sUtilities.IsValueBiggerDateTimeNow(time.Year, time.Month, time.Day, time.Hour, time.Minute))
				return sLManager.GetWarningText("GaveExpiredDateTime", channel);
	
			var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM banned WHERE Name = '{0}' AND Channel = '{1}'", sUtilities.SqlEscape(name.ToLower()), channel.ToLower());
			if(!db.IsNull())
				return sLManager.GetWarningText("BanList", channel);

			sSender.Ban(channel, name);
			sSender.Kick(channel, name, reason);
			SchumixBase.DManager.Insert("`banned`(Name, Channel, Reason, Year, Month, Day, Hour, Minute)", sUtilities.SqlEscape(name.ToLower()), channel.ToLower(), sUtilities.SqlEscape(reason), time.Year, time.Month, time.Day, time.Hour, time.Minute);
			return sLManager.GetWarningText("BanList1", channel);
		}

		public string BanName(string name, string channel, string reason, int hour, int minute)
		{
			var time = DateTime.Now;

			if(sUtilities.IsValueBiggerDateTimeNow(time.Year, time.Month, time.Day, hour, minute))
				return sLManager.GetWarningText("GaveExpiredDateTime", channel);

			var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM banned WHERE Name = '{0}' AND Channel = '{1}'", sUtilities.SqlEscape(name.ToLower()), channel.ToLower());
			if(!db.IsNull())
				return sLManager.GetWarningText("BanList", channel);

			sSender.Ban(channel, name);
			sSender.Kick(channel, name, reason);
			SchumixBase.DManager.Insert("`banned`(Name, Channel, Reason, Year, Month, Day, Hour, Minute)", sUtilities.SqlEscape(name.ToLower()), channel.ToLower(), sUtilities.SqlEscape(reason), time.Year, time.Month, time.Day, hour, minute);
			return sLManager.GetWarningText("BanList1", channel);
		}

		public string BanName(string name, string channel, string reason, int year, int month, int day, int hour, int minute)
		{
			if(sUtilities.IsValueBiggerDateTimeNow(year, month, day, hour, minute))
				return sLManager.GetWarningText("GaveExpiredDateTime", channel);

			var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM banned WHERE Name = '{0}' AND Channel = '{1}'", sUtilities.SqlEscape(name.ToLower()), channel.ToLower());
			if(!db.IsNull())
				return sLManager.GetWarningText("BanList", channel);

			sSender.Ban(channel, name);
			sSender.Kick(channel, name, reason);
			SchumixBase.DManager.Insert("`banned`(Name, Channel, Reason, Year, Month, Day, Hour, Minute)", sUtilities.SqlEscape(name.ToLower()), channel.ToLower(), sUtilities.SqlEscape(reason), year, month, day, hour, minute);
			return sLManager.GetWarningText("BanList1", channel);
		}
	}
}