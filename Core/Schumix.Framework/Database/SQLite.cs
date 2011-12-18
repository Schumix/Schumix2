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
using System.Threading;
using System.Data;
using System.Data.SQLite;
using System.Text.RegularExpressions;
using Schumix.API;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Framework.Database
{
	public sealed class SQLite
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private SQLiteConnection Connection;

		public SQLite(string file)
		{
			if(!Initialize(file))
			{
				Log.Error("SQLite", sLConsole.SQLite("Text"));
				SchumixBase.ServerDisconnect(false);
				Thread.Sleep(1000);
				Environment.Exit(1);
			}
			else
				Log.Notice("SQLite", sLConsole.SQLite("Text2"));
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
				Connection = new SQLiteConnection("Data Source=" + file);
				Connection.Open();
				return true;
			}
			catch(SQLiteException s)
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
		public DataTable Query(string query)
		{
			try
			{
				var adapter = new SQLiteDataAdapter();
				var command = Connection.CreateCommand();
				command.CommandText = query;
				adapter.SelectCommand = command;

				var table = new DataTable();
				adapter.Fill(table);

				command.Dispose();
				adapter.Dispose();

				return table;
			}
			catch(SQLiteException s)
			{
				if(s.Message.Contains("Fatal error encountered during command execution."))
				{
					Log.Error("SQLite", sLConsole.SQLite("Text3"), s.Message);
					Log.Warning("SQLite", sLConsole.SQLite("Text4"));
					SchumixBase.ServerDisconnect(false);
					SchumixBase.ExitStatus = true;

					if(!INetwork.Writer.IsNull())
						INetwork.Writer.WriteLine("QUIT :Sql connection crash.");

					Thread.Sleep(1000);
					Environment.Exit(1);
				}

				if(s.Message.Contains("Timeout expired."))
				{
					Log.Error("SQLite", sLConsole.SQLite("Text3"), s.Message);
					Log.Warning("SQLite", sLConsole.SQLite("Text4"));
					SchumixBase.ServerDisconnect(false);
					SchumixBase.ExitStatus = true;

					if(!INetwork.Writer.IsNull())
						INetwork.Writer.WriteLine("QUIT :Sql connection timeout.");

					Thread.Sleep(1000);
					Environment.Exit(1);
				}

				Log.Error("SQLite", sLConsole.SQLite("Text3"), s.Message);
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

		private void ExecuteNonQuery(string sql)
		{
			try
			{
				var command = Connection.CreateCommand();
				command.CommandText = sql;
				command.ExecuteNonQuery();
			}
			catch(SQLiteException s)
			{
				if(s.Message.Contains("Fatal error encountered during command execution."))
				{
					Log.Error("SQLite", sLConsole.SQLite("Text3"), s.Message);
					Log.Warning("SQLite", sLConsole.SQLite("Text4"));
					SchumixBase.ServerDisconnect(false);
					SchumixBase.ExitStatus = true;

					if(!INetwork.Writer.IsNull())
						INetwork.Writer.WriteLine("QUIT :Sql connection crash.");

					Thread.Sleep(1000);
					Environment.Exit(1);
				}

				if(s.Message.Contains("Timeout expired."))
				{
					Log.Error("SQLite", sLConsole.SQLite("Text3"), s.Message);
					Log.Warning("SQLite", sLConsole.SQLite("Text4"));
					SchumixBase.ServerDisconnect(false);
					SchumixBase.ExitStatus = true;

					if(!INetwork.Writer.IsNull())
						INetwork.Writer.WriteLine("QUIT :Sql connection timeout.");

					Thread.Sleep(1000);
					Environment.Exit(1);
				}

				Log.Error("SQLite", sLConsole.SQLite("Text3"), s.Message);
				return;
			}
		}

		private void IsConnect()
		{
			try
			{
				if(Connection.State != ConnectionState.Open)
					Connection.Open();
			}
			catch(SQLiteException s)
			{
				if(s.Message.Contains("Fatal error encountered during command execution."))
				{
					Log.Error("SQLite", sLConsole.SQLite("Text3"), s.Message);
					Log.Warning("SQLite", sLConsole.SQLite("Text4"));
					SchumixBase.ServerDisconnect(false);
					SchumixBase.ExitStatus = true;

					if(!INetwork.Writer.IsNull())
						INetwork.Writer.WriteLine("QUIT :Sql connection crash.");

					Thread.Sleep(1000);
					Environment.Exit(1);
				}

				if(s.Message.Contains("Timeout expired."))
				{
					Log.Error("SQLite", sLConsole.SQLite("Text3"), s.Message);
					Log.Warning("SQLite", sLConsole.SQLite("Text4"));
					SchumixBase.ServerDisconnect(false);
					SchumixBase.ExitStatus = true;

					if(!INetwork.Writer.IsNull())
						INetwork.Writer.WriteLine("QUIT :Sql connection timeout.");

					Thread.Sleep(1000);
					Environment.Exit(1);
				}

				Log.Error("SQLite", sLConsole.SQLite("Text3"), s.Message);
				return;
			}
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
	}
}