/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013-2015 Schumix Team <http://schumix.eu/>
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
using Schumix.Framework.Util;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.CalendarAddon
{
	sealed class Ban
	{
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private string _servername;

		public Ban(string ServerName)
		{
			_servername = ServerName;
		}

		public string BanName(string name, string channel, string reason, DateTime time)
		{
			var sSender = sIrcBase.Networks[_servername].sSender;

			if(sUtilities.IsValueBiggerDateTimeNow(time.Year, time.Month, time.Day, time.Hour, time.Minute))
				return sLManager.GetWarningText("GaveExpiredDateTime", channel, _servername);
	
			var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM banned WHERE Name = '{0}' AND Channel = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(name.ToLower()), channel.ToLower(), _servername);
			if(!db.IsNull())
				return sLManager.GetWarningText("BanList", channel, _servername);

			sSender.Ban(channel, name);
			sSender.Kick(channel, name, reason);
			SchumixBase.DManager.Insert("`banned`(ServerId, ServerName, Name, Channel, Reason, Year, Month, Day, Hour, Minute)", IRCConfig.List[_servername].ServerId, _servername, sUtilities.SqlEscape(name.ToLower()), channel.ToLower(), sUtilities.SqlEscape(reason), time.Year, time.Month, time.Day, time.Hour, time.Minute);
			return sLManager.GetWarningText("BanList1", channel, _servername);
		}

		public string BanName(string name, string channel, string reason, int hour, int minute)
		{
			var time = DateTime.Now;
			var sSender = sIrcBase.Networks[_servername].sSender;

			if(sUtilities.IsValueBiggerDateTimeNow(time.Year, time.Month, time.Day, hour, minute))
				return sLManager.GetWarningText("GaveExpiredDateTime", channel, _servername);

			var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM banned WHERE Name = '{0}' AND Channel = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(name.ToLower()), channel.ToLower(), _servername);
			if(!db.IsNull())
				return sLManager.GetWarningText("BanList", channel, _servername);

			sSender.Ban(channel, name);
			sSender.Kick(channel, name, reason);
			SchumixBase.DManager.Insert("`banned`(ServerId, ServerName, Name, Channel, Reason, Year, Month, Day, Hour, Minute)", IRCConfig.List[_servername].ServerId, _servername, sUtilities.SqlEscape(name.ToLower()), channel.ToLower(), sUtilities.SqlEscape(reason), time.Year, time.Month, time.Day, hour, minute);
			return sLManager.GetWarningText("BanList1", channel, _servername);
		}

		public string BanName(string name, string channel, string reason, int year, int month, int day, int hour, int minute)
		{
			var sSender = sIrcBase.Networks[_servername].sSender;

			if(sUtilities.IsValueBiggerDateTimeNow(year, month, day, hour, minute))
				return sLManager.GetWarningText("GaveExpiredDateTime", channel, _servername);

			var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM banned WHERE Name = '{0}' AND Channel = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(name.ToLower()), channel.ToLower(), _servername);
			if(!db.IsNull())
				return sLManager.GetWarningText("BanList", channel, _servername);

			sSender.Ban(channel, name);
			sSender.Kick(channel, name, reason);
			SchumixBase.DManager.Insert("`banned`(ServerId, ServerName, Name, Channel, Reason, Year, Month, Day, Hour, Minute)", IRCConfig.List[_servername].ServerId, _servername, sUtilities.SqlEscape(name.ToLower()), channel.ToLower(), sUtilities.SqlEscape(reason), year, month, day, hour, minute);
			return sLManager.GetWarningText("BanList1", channel, _servername);
		}
	}
}