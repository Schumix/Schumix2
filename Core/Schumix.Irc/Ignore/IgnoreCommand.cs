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
using Schumix.Framework;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Irc.Ignore
{
	public sealed class IgnoreCommand
	{
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private IgnoreCommand() {}

		public bool IsIgnore(string Name)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM ignore_commands WHERE Command = '{0}'", sUtilities.SqlEscape(Name.ToLower()));
			return !db.IsNull() ? true : false;
		}

		public void Add(string Name)
		{
			if(Name.Trim() == string.Empty)
				return;

			var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM ignore_commands WHERE Command = '{0}'", sUtilities.SqlEscape(Name.ToLower()));
			if(!db.IsNull())
				return;

			SchumixBase.DManager.Insert("`ignore_commands`(Command)", sUtilities.SqlEscape(Name.ToLower()));
		}

		public void Remove(string Name)
		{
			if(Name.Trim() == string.Empty)
				return;

			var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM ignore_commands WHERE Command = '{0}'", sUtilities.SqlEscape(Name.ToLower()));
			if(db.IsNull())
				return;

			SchumixBase.DManager.Delete("ignore_commands", string.Format("Command = '{0}'", sUtilities.SqlEscape(Name.ToLower())));
		}
	}
}