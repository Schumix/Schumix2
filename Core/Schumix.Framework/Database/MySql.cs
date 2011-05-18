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
using Schumix.Framework.Extensions;

namespace Schumix.Framework.Database
{
	public sealed class MySql
	{
		private MySqlConnection Connection;

		public MySql(string host, string username, string password, string database, string charset)
		{
			if(!Initialize(host, username, password, database, charset))
			{
				Log.Error("MySql", "Hiba tortent az adatbazishoz valo kapcsolodas soran!");
				Thread.Sleep(200);
				Environment.Exit(1);
			}
			else
				Log.Success("MySql", "MySql adatbazishoz sikeres a kapcsolodas.");
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
				Log.Error("MySql", "{0}", m.Message);
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
				Log.Error("MySql", "Query hiba: {0}", m.Message);
				return null;
			}
		}

		public DataRow QueryFirstRow(string query)
		{
			var table = Query(query);
			return !table.Equals(null) && table.Rows.Count > 0 ? table.Rows[0] : null;
		}
	}
}