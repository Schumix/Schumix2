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
using Schumix.Irc;
using Schumix.Framework;
using Schumix.Framework.Util;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.CalendarAddon
{
	sealed class Unban
	{
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private string _servername;

		public Unban(string ServerName)
		{
			_servername = ServerName;
		}

		public string UnbanName(string name, string channel)
		{
			var sSender = sIrcBase.Networks[_servername].sSender;
			var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM banned WHERE Name = '{0}' AND Channel = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(name.ToLower()), channel.ToLower(), _servername);
			if(db.IsNull())
				return sLManager.GetWarningText("UnbanList", channel, _servername);

			sSender.Unban(channel, name);
			SchumixBase.DManager.Delete("banned", string.Format("Name = '{0}' AND Channel = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(name.ToLower()), channel.ToLower(), _servername));
			return sLManager.GetWarningText("UnbanList1", channel, _servername);
		}
	}
}