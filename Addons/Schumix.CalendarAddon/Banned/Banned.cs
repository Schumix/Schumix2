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
using Schumix.Irc;
using Schumix.Framework;
using Schumix.Framework.Extensions;

namespace Schumix.CalendarAddon
{
	public sealed class Banned
	{
		private readonly Sender sSender = Singleton<Sender>.Instance;
		private Banned() {}

		public string BannedName(string name, string channel, string reason, DateTime time)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM banned WHERE Name = '{0}' AND Channel = '{1}'", name.ToLower(), channel.ToLower());
			if(!db.IsNull())
				return "Már szerepel a tiltó listán!";

			sSender.Banned(channel, name);
			sSender.Kick(channel, name, reason);
			SchumixBase.DManager.QueryFirstRow("INSERT INTO `banned`(Name, Channel, Reason, Year, Month, Day, Hour, Minute) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}')", name.ToLower(), channel.ToLower(), reason, time.Year, time.Month, time.Day, time.Hour, time.Minute);
			return "Sikeresen hozzá lett adva a tiltó listához.";
		}

		public string BannedName(string name, string channel, string reason, int hour, int minute)
		{
			var time = DateTime.Now;
			var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM banned WHERE Name = '{0}' AND Channel = '{1}'", name.ToLower(), channel.ToLower());
			if(!db.IsNull())
				return "Már szerepel a tiltó listán!";

			sSender.Banned(channel, name);
			sSender.Kick(channel, name, reason);
			SchumixBase.DManager.QueryFirstRow("INSERT INTO `banned`(Name, Channel, Reason, Year, Month, Day, Hour, Minute) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}')", name.ToLower(), channel.ToLower(), reason, time.Year, time.Month, time.Day, hour, minute);
			return "Sikeresen hozzá lett adva a tiltó listához.";
		}

		public string BannedName(string name, string channel, string reason, int year, int month, int day, int hour, int minute)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM banned WHERE Name = '{0}' AND Channel = '{1}'", name.ToLower(), channel.ToLower());
			if(!db.IsNull())
				return "Már szerepel a tiltó listán!";

			sSender.Banned(channel, name);
			sSender.Kick(channel, name, reason);
			SchumixBase.DManager.QueryFirstRow("INSERT INTO `banned`(Name, Channel, Reason, Year, Month, Day, Hour, Minute) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}')", name.ToLower(), channel.ToLower(), reason, year, month, day, hour, minute);
			return "Sikeresen hozzá lett adva a tiltó listához.";
		}
	}
}