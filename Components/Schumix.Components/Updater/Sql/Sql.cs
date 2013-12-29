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
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Schumix.Framework;
using Schumix.Framework.Logger;
using Schumix.Framework.Config;
using Schumix.Framework.Database;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Components.Updater.Sql
{
	sealed class SqlUpdate
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private Dictionary<string, string> SQLiteMap = new Dictionary<string, string>();
		private Dictionary<string, string> MySqlMap = new Dictionary<string, string>();
		private DatabaseManager DManager;
		private string Directory;

		public SqlUpdate(string dir)
		{
			Directory = dir;
		}

		public void Connect()
		{
			DManager = new DatabaseManager();
		}

		public void Update()
		{
			var v2 = new Version(Schumix.Framework.Config.Consts.SchumixVersion);
			var dir = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, Directory));

			foreach(var file in dir.GetFiles("*.sql").AsParallel())
			{
				bool sqlite = false;
				var sqlname = file.Name.ToLower();

				if(sqlname.Contains("sqlite"))
					sqlite = true;

				if(sqlname.Contains("_") && !sqlname.Contains("base"))
				{
					try
					{
						var v1 = new Version(sqlname.Substring(0, sqlname.IndexOf("_")));
						if(v1.CompareTo(v2) == 1)
						{
							if(sqlite)
								SQLiteMap.Add(sqlname, string.Empty);
							else
								MySqlMap.Add(sqlname, string.Empty);
						}
					}
					catch(Exception e)
					{
						Log.Error("SqlUpdate", sLConsole.GetString("Failure details: {0}"), e.Message);
					}
				}
				else if(!sqlname.Contains("base"))
				{
					try
					{
						var v1 = new Version(sqlname.Substring(0, sqlname.IndexOf(".sql")));
						if(v1.CompareTo(v2) == 1)
						{
							if(sqlite)
								SQLiteMap.Add(sqlname, string.Empty);
							else
								MySqlMap.Add(sqlname, string.Empty);
						}
					}
					catch(Exception e)
					{
						Log.Error("SqlUpdate", sLConsole.GetString("Failure details: {0}"), e.Message);
					}
				}
			}

			foreach(var file in dir.GetFiles("*.sql").AsParallel())
			{
				var sqlname = file.Name.ToLower();
				if(sqlname.Contains("_") && sqlname.Contains("base"))
				{
					try
					{
						var v1 = new Version(sqlname.Substring(0, sqlname.IndexOf("_")));

						if(v1.CompareTo(v2) == 1)
						{
							string s = sqlname.Substring(0, sqlname.IndexOf("_base.sql"));
							if(SQLiteMap.ContainsKey(s + "_sqlite.sql"))
								SQLiteMap[(s + "_sqlite.sql")] = sqlname;
							else if(MySqlMap.ContainsKey(s + ".sql"))
								MySqlMap[(s + ".sql")] = sqlname;
							else
							{
								if(SQLiteConfig.Enabled)
									SQLiteMap.Add(sqlname.Substring(0, sqlname.IndexOf("_base")), sqlname);
								else
									MySqlMap.Add(sqlname.Substring(0, sqlname.IndexOf("_base")), sqlname);
							}	
						}
					}
					catch(Exception e)
					{
						Log.Error("SqlUpdate", sLConsole.GetString("Failure details: {0}"), e.Message);
					}
				}
			}

			if(SQLiteConfig.Enabled)
			{
				foreach(var item in SQLiteMap.OrderBy(key => key.Value))
				{
					if(!item.Key.Contains(".sql"))
						DManager.ExecuteNonQuery(new StreamReader(Directory + "/" + item.Value).ReadToEnd());
					else
					{
						DManager.ExecuteNonQuery(new StreamReader(Directory + "/" + item.Key).ReadToEnd());

						if(!item.Value.IsNullOrEmpty())
							DManager.ExecuteNonQuery(new StreamReader(Directory + "/" + item.Value).ReadToEnd());
					}
				}
			}
			else
			{
				foreach(var item in MySqlMap.OrderBy(key => key.Value))
				{
					if(!item.Key.Contains(".sql"))
						DManager.ExecuteNonQuery(new StreamReader(Directory + "/" + item.Value).ReadToEnd());
					else
					{
						DManager.ExecuteNonQuery(new StreamReader(Directory + "/" + item.Key).ReadToEnd());

						if(!item.Value.IsNullOrEmpty())
							DManager.ExecuteNonQuery(new StreamReader(Directory + "/" + item.Value).ReadToEnd());
					}
				}
			}
		}
	}
}