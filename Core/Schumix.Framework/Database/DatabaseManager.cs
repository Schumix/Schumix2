/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Twl
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
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.Threading;
using MySql.Data;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
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
				Log.LargeError(sLConsole.Exception("MajorError"));
				Log.Error("DatabaseManager", sLConsole.DatabaseManager("Text3"));
				SchumixBase.ServerDisconnect(false);
				Thread.Sleep(1000);
				Environment.Exit(1);
			}
			else if(x == 2)
			{
				Log.LargeError(sLConsole.Exception("MajorError"));
				Log.Error("DatabaseManager", sLConsole.DatabaseManager("Text4"));
				SchumixBase.ServerDisconnect(false);
				Thread.Sleep(1000);
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
			lock(_lock)
			{
				return Query(string.Format(query, args));
			}
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
			lock(_lock)
			{
				return QueryFirstRow(string.Format(query, args));
			}
		}

		public bool Update(string sql)
		{
			lock(_lock)
			{
				return SQLiteConfig.Enabled ? sdatabase.Update(sql) : mdatabase.Update(sql);
			}
		}

		public bool Update(string TableName, string Set)
		{
			lock(_lock)
			{
				return SQLiteConfig.Enabled ? sdatabase.Update(TableName, Set) : mdatabase.Update(TableName, Set);
			}
		}

		public bool Update(string TableName, string Set, string Where)
		{
			lock(_lock)
			{
				return SQLiteConfig.Enabled ? sdatabase.Update(TableName, Set, Where) : mdatabase.Update(TableName, Set, Where);
			}
		}

		public bool Insert(string sql)
		{
			lock(_lock)
			{
				return SQLiteConfig.Enabled ? sdatabase.Insert(sql) : mdatabase.Insert(sql);
			}
		}

		public bool Insert(string TableName, params object[] Values)
		{
			lock(_lock)
			{
				var sb = new StringBuilder();

				foreach(var value in Values)
					sb.Append(",'" + value + "'");

				return SQLiteConfig.Enabled ? sdatabase.Insert(TableName, sb.ToString().Remove(0, 1, SchumixBase.Comma)) : mdatabase.Insert(TableName, sb.ToString().Remove(0, 1, SchumixBase.Comma));
			}
		}

		public bool Delete(string sql)
		{
			lock(_lock)
			{
				return SQLiteConfig.Enabled ? sdatabase.Delete(sql) : mdatabase.Delete(sql);
			}
		}

		public bool Delete(string TableName, string Where)
		{
			lock(_lock)
			{
				return SQLiteConfig.Enabled ? sdatabase.Delete(TableName, Where) : mdatabase.Delete(TableName, Where);
			}
		}

		public bool RemoveTable(string Table)
		{
			lock(_lock)
			{
				return SQLiteConfig.Enabled ? sdatabase.RemoveTable(Table) : mdatabase.RemoveTable(Table);
			}
		}

		public bool IsCreatedTable(string Table)
		{
			lock(_lock)
			{
				return SQLiteConfig.Enabled ? sdatabase.IsCreatedTable(Table) : mdatabase.IsCreatedTable(Table);
			}
		}

		public void ExecuteNonQuery(string Sql)
		{
			lock(_lock)
			{
				if(SQLiteConfig.Enabled)
					sdatabase.ExecuteNonQuery(Sql);
				else
					mdatabase.ExecuteNonQuery(Sql);
			}
		}
	}
}