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
using System.Threading;
using System.Text.RegularExpressions;
using MySql.Data;
using MySql.Data.MySqlClient;
using Schumix.API;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Framework.Database
{
	public sealed class MySql
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private MySqlConnection Connection;

		public MySql(string host, string username, string password, string database, string charset)
		{
			if(!Initialize(host, username, password, database, charset))
			{
				Log.Error("MySql", sLConsole.MySql("Text"));
				Thread.Sleep(200);
				Environment.Exit(1);
			}
			else
				Log.Success("MySql", sLConsole.MySql("Text2"));
		}

		~MySql()
		{
			Log.Debug("MySql", "~MySql()");
			Connection.Close();
		}

		private bool Initialize(string host, string username, string password, string database, string charset)
		{
			try
			{
				Connection = new MySqlConnection(string.Format("SERVER={0};DATABASE={1};UID={2};PWD={3};charset={4};", host, database, username, password, charset));
				Connection.Open();
				return true;
			}
			catch(MySqlException m)
			{
				Log.Error("MySql", m.Message);
				return false;
			}
		}

		public DataTable Query(string query)
		{
			try
			{
				var adapter = new MySqlDataAdapter();
				var command = Connection.CreateCommand();
				command.CommandText = query;
				adapter.SelectCommand = command;

				var table = new DataTable();
				adapter.Fill(table);

				command.Dispose();
				adapter.Dispose();

				return table;
			}
			catch(MySqlException m)
			{
				if(m.Message.Contains("Fatal error encountered during command execution."))
				{
					Log.Error("MySql", sLConsole.MySql("Text3"), m.Message);
					Log.Warning("MySql", sLConsole.MySql("Text4"));
					SchumixBase.ExitStatus = true;

					if(!INetwork.Writer.IsNull())
						INetwork.Writer.WriteLine("QUIT :Sql connection crash.");

					Thread.Sleep(1000);
					Environment.Exit(1);
				}

				if(m.Message.Contains("Timeout expired."))
				{
					Log.Error("MySql", sLConsole.MySql("Text3"), m.Message);
					Log.Warning("MySql", sLConsole.MySql("Text4"));
					SchumixBase.ExitStatus = true;

					if(!INetwork.Writer.IsNull())
						INetwork.Writer.WriteLine("QUIT :Sql connection timeout.");

					Thread.Sleep(1000);
					Environment.Exit(1);
				}

				Log.Error("MySql", sLConsole.MySql("Text3"), m.Message);
				return null;
			}
		}

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
			catch(MySqlException m)
			{
				if(m.Message.Contains("Fatal error encountered during command execution."))
				{
					Log.Error("MySql", sLConsole.MySql("Text3"), m.Message);
					Log.Warning("MySql", sLConsole.MySql("Text4"));
					SchumixBase.ExitStatus = true;

					if(!INetwork.Writer.IsNull())
						INetwork.Writer.WriteLine("QUIT :Sql connection crash.");

					Thread.Sleep(1000);
					Environment.Exit(1);
				}

				if(m.Message.Contains("Timeout expired."))
				{
					Log.Error("MySql", sLConsole.MySql("Text3"), m.Message);
					Log.Warning("MySql", sLConsole.MySql("Text4"));
					SchumixBase.ExitStatus = true;

					if(!INetwork.Writer.IsNull())
						INetwork.Writer.WriteLine("QUIT :Sql connection timeout.");

					Thread.Sleep(1000);
					Environment.Exit(1);
				}

				Log.Error("MySql", sLConsole.MySql("Text3"), m.Message);
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