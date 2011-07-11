/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2011 Twl
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
using System.Data;
using System.Data.SQLite;
using System.Threading;
using MySql.Data;
using Schumix.Framework.Config;
using Schumix.Framework.Localization;

namespace Schumix.Framework.Database
{
	public sealed class DatabaseManager
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly object _lock = new object();
		private readonly SQLite sdatabase;
		private readonly MySql mdatabase;

		public DatabaseManager()
		{
			byte x = 0;
			Log.Debug("DatabaseManager", sLConsole.DatabaseManager("Text"));

			if(SQLiteConfig.Enabled)
			{
				x++;
				sdatabase = new SQLite(SQLiteConfig.FileName);
			}

			if(MySqlConfig.Enabled)
			{
				x++;
				mdatabase = new MySql(MySqlConfig.Host, MySqlConfig.User, MySqlConfig.Password, MySqlConfig.Database, MySqlConfig.Charset);
			}

			Log.Debug("DatabaseManager", sLConsole.DatabaseManager("Text2"));

			if(x == 0)
			{
				Log.Error("DatabaseManager", sLConsole.DatabaseManager("Text3"));
				Thread.Sleep(200);
				Environment.Exit(1);
			}
			else if(x == 2)
			{
				Log.Error("DatabaseManager", sLConsole.DatabaseManager("Text4"));
				Thread.Sleep(200);
				Environment.Exit(1);
			}
		}

		~DatabaseManager()
		{
			Log.Debug("DatabaseManager", "~DatabaseManager()");
		}

		public DataTable Query(string sql)
		{
			lock(_lock)
			{
				return SQLiteConfig.Enabled ? sdatabase.Query(sql) : mdatabase.Query(sql);
			}
		}

		public DataTable Query(string query, params object[] args)
		{
			return Query(string.Format(query, args));
		}

		public DataRow QueryFirstRow(string query)
		{
			lock(_lock)
			{
				return SQLiteConfig.Enabled ? sdatabase.QueryFirstRow(query) : mdatabase.QueryFirstRow(query);
			}
		}

		public DataRow QueryFirstRow(string query, params object[] args)
		{
			return QueryFirstRow(string.Format(query, args));
		}
	}
}