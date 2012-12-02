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
using System.Data;
using System.Collections.Generic;
using Schumix.Framework.Extensions;

namespace Schumix.Framework.Database.Cache
{
	public sealed class CacheDB
	{
		private readonly Dictionary<string, LocalizedConsoleCommand> _LocalizedConsoleCommandMap = new Dictionary<string, LocalizedConsoleCommand>();
		private readonly Dictionary<string, LocalizedCommand> _LocalizedCommandMap = new Dictionary<string, LocalizedCommand>();

		public Dictionary<string, LocalizedConsoleCommand> LocalizedConsoleCommandMap()
		{
			return _LocalizedConsoleCommandMap;
		}

		public Dictionary<string, LocalizedCommand> LocalizedCommandMap()
		{
			return _LocalizedCommandMap;
		}

		public CacheDB()
		{
			// valami üzenet kéne ide
		}

		public void Load() // megoldani hogy a db neve alapján (inkább _ jel nélkül) lehessen betölteni db-t
		{
			// valami üzenet kéne ide
			LoadLocalizedConsoleCommand();
			LoadLocalizedCommand();
		}

		public void UnLoad() // megoldani hogy a db neve alapján (inkább _ jel nélkül) lehessen törölni db-t
		{
			// valami üzenet kéne ide
			Clean();
		}

		public void Clean() // megoldani hogy a db neve alapján (inkább _ jel nélkül) lehessen törölni db-t
		{
			_LocalizedConsoleCommandMap.Clear();
			_LocalizedCommandMap.Clear();
		}

		private void LoadLocalizedConsoleCommand()
		{
			var db = SchumixBase.DManager.Query("SELECT Id, Language, Command, Text FROM localized_console_command");
			if(!db.IsNull())
			{
				foreach(DataRow row in db.Rows)
				{
					var map = new LocalizedConsoleCommand();
					map.Id = Convert.ToInt32(row["Id"].ToString());
					map.Language = row["Language"].ToString();
					map.Command = row["Command"].ToString();
					map.Text = row["Text"].ToString();
					_LocalizedConsoleCommandMap.Add(map.Language + map.Command, map);
				}
			}
		}

		private void LoadLocalizedCommand()
		{
			var db = SchumixBase.DManager.Query("SELECT Id, Language, Command, Text FROM localized_command");
			if(!db.IsNull())
			{
				foreach(DataRow row in db.Rows)
				{
					var map = new LocalizedCommand();
					map.Id = Convert.ToInt32(row["Id"].ToString());
					map.Language = row["Language"].ToString();
					map.Command = row["Command"].ToString();
					map.Text = row["Text"].ToString();
					_LocalizedCommandMap.Add(map.Language + map.Command, map);
				}
			}
		}
	}

/*`localized_console_command`,
`localized_console_command_help`,
`localized_console_warning`,
`localized_command`,
`localized_command_help`,
`localized_warning`,*/
}