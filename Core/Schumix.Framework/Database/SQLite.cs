/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Twl
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013 Schumix Team <http://schumix.eu/>
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
using System.Data;
using System.Text.RegularExpressions;
using Community.CsharpSqlite.SQLiteClient;
using Schumix.Framework.Irc;
using Schumix.Framework.Logger;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Framework.Database
{
	public sealed class SQLite
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly Runtime sRuntime = Singleton<Runtime>.Instance;
		private SqliteConnection Connection;
		private bool _crash = false;
#if DEBUG
		private DebugLog _debuglog;
#endif

		public SQLite(string file)
		{
			if(!Initialize(file))
			{
				Log.Error("SQLite", sLConsole.GetString("Error was handled when tried to connect to the database!"));
				SchumixBase.ServerDisconnect(false);
				Thread.Sleep(1000);
				sRuntime.Exit();
			}
			else
				Log.Notice("SQLite", sLConsole.GetString("Successfully connected to the SQLite database."));
		}

		~SQLite()
		{
			Log.Debug("SQLite", "~SQLite()");
			Connection.Close();
		}

		private bool Initialize(string file)
		{
			try
			{
#if DEBUG
				_debuglog = new DebugLog("SQLite.log");
#endif
				Connection = new SqliteConnection("Data Source=" + file);
				Connection.Open();
				return true;
			}
			catch(SqliteException s)
			{
				Log.Error("SQLite", s.Message);
				return false;
			}
		}

		/// <summary>
		/// Executes the given query on the database.
		/// </summary>
		/// <param name="sql">The query</param>
		/// <returns>Result from the database.</returns>
		public DataTable Query(string query, bool logerror = true)
		{
			try
			{
				if(_crash)
					return null;

				IsConnect();
				var adapter = new SqliteDataAdapter();
				var command = Connection.CreateCommand();
#if DEBUG
				_debuglog.LogInFile(query);
#endif
				command.CommandText = query;
				adapter.SelectCommand = command;

				var table = new DataTable();
				adapter.Fill(table);

				command.Dispose();
				adapter.Dispose();

				return table;
			}
			catch(SqliteException s)
			{
				Crash(s, logerror);
				return null;
			}
		}

		/// <summary>
		/// Executes the given query on the database and returns the result's first row.
		/// </summary>
		/// <param name="query">Query to execute</param>
		/// <returns>The row</returns>
		public DataRow QueryFirstRow(string query)
		{
			var table = Query(query);
			return !table.Equals(null) && table.Rows.Count > 0 ? table.Rows[0] : null;
		}

		public void ExecuteNonQuery(string sql, bool logerror = true)
		{
			try
			{
				if(_crash)
					return;

				IsConnect();
				var command = Connection.CreateCommand();
#if DEBUG
				_debuglog.LogInFile(sql);
#endif
				command.CommandText = sql;
				command.ExecuteNonQuery();
			}
			catch(SqliteException s)
			{
				Crash(s, logerror);
			}
		}

		private void IsConnect()
		{
			try
			{
				if(Connection.State != ConnectionState.Open)
					Connection.Open();
			}
			catch(SqliteException s)
			{
				Crash(s, true, true);
			}
		}

		private void Crash(SqliteException s, bool logerror, bool c = false)
		{
			if(c)
			{
				_crash = true;
				Log.Error("SQLite", sLConsole.GetString("Query error: {0}"), s.Message);
				Log.Warning("SQLite", sLConsole.GetString("Program shutting down!"));
				SchumixBase.Quit(false);

				foreach(var nw in INetwork.WriterList)
				{
					if(!nw.Value.IsNull())
						nw.Value.WriteLine("QUIT :Sql connection crash.");
				}

				Thread.Sleep(1000);
				sRuntime.Exit();
			}

			if(s.Message.Contains("Fatal error encountered during command execution."))
			{
				_crash = true;
				Log.Error("SQLite", sLConsole.GetString("Query error: {0}"), s.Message);
				Log.Warning("SQLite", sLConsole.GetString("Program shutting down!"));
				SchumixBase.Quit(false);

				foreach(var nw in INetwork.WriterList)
				{
					if(!nw.Value.IsNull())
						nw.Value.WriteLine("QUIT :Sql connection crash.");
				}

				Thread.Sleep(1000);
				sRuntime.Exit();
			}

			if(s.Message.Contains("Timeout expired."))
			{
				_crash = true;
				Log.Error("SQLite", sLConsole.GetString("Query error: {0}"), s.Message);
				Log.Warning("SQLite", sLConsole.GetString("Program shutting down!"));
				SchumixBase.Quit(false);

				foreach(var nw in INetwork.WriterList)
				{
					if(!nw.Value.IsNull())
						nw.Value.WriteLine("QUIT :Sql connection timeout.");
				}

				Thread.Sleep(1000);
				sRuntime.Exit();
			}

			if(logerror)
				Log.Error("SQLite", sLConsole.GetString("Query error: {0}"), s.Message);
		}

		public bool Update(string sql)
		{
			try
			{
				ExecuteNonQuery("UPDATE " + sql);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public bool Update(string TableName, string Set)
		{
			try
			{
				return Update(TableName + " SET " + Set);
			}
			catch
			{
				return false;
			}
		}

		public bool Update(string TableName, string Set, string Where)
		{
			try
			{
				return Update(TableName + " SET " + Set + " WHERE " + Where);
			}
			catch
			{
				return false;
			}
		}

		public bool Insert(string sql)
		{
			try
			{
				ExecuteNonQuery("INSERT INTO " + sql);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public bool Insert(string TableName, string Values)
		{
			try
			{
				return Insert(TableName + " VALUES (" + Values + ")");
			}
			catch
			{
				return false;
			}
		}

		public bool Delete(string sql)
		{
			try
			{
				ExecuteNonQuery("DELETE FROM " + sql);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public bool Delete(string TableName, string Where)
		{
			try
			{
				return Delete(TableName + " WHERE " + Where);
			}
			catch
			{
				return false;
			}
		}

		public bool RemoveTable(string Table)
		{
			try
			{
				ExecuteNonQuery(string.Format("DROP TABLE IF EXISTS `{0}`", Table));
				return true;
			}
			catch
			{
				return false;
			}
		}

		public bool IsCreatedTable(string Table)
		{
			return !Query(string.Format("SELECT 1 FROM {0}", Table), false).IsNull();
		}
	}
}