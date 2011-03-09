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

namespace Schumix.Framework.Database
{
	public sealed class MySql
	{
		private MySqlConnection Connection;

		public MySql(string host, string username, string password, string database)
		{
			if(!Initialize(host, username, password, database))
			{
				Log.Error("Mysql", "Hiba tortent az adatbazishoz valo kapcsolodas soran!");
				Thread.Sleep(200);
				Environment.Exit(1);
			}
			else
				Log.Success("Mysql", "Mysql rendszer elindult.");
		}

		~MySql()
		{
			Log.Debug("Mysql", "~Mysql()");
			Connection.Close();
		}

		private bool Initialize(string host, string username, string password, string database)
		{
			try
			{
				Connection = new MySqlConnection(string.Format("SERVER={0};DATABASE={1};UID={2};PWD={3};", host, database, username, password));
				Connection.Open();
				return true;
			}
			catch(MySqlException ex)
			{
				Log.Error("Mysql", "{0}", ex.Message);
				return false;
			}
		}

		public DataTable Query(string query)
		{
			try
			{
				var adapter = new MySqlDataAdapter();
				var command = Connection.CreateCommand();
				MySqlEscape(query);
				command.CommandText = query;
				adapter.SelectCommand = command;

				var table = new DataTable();
				adapter.Fill(table);

				command.Dispose();
				adapter.Dispose();

				return table;
			}
			catch(MySqlException ex)
			{
				Log.Error("Mysql", "Query hiba: {0}", ex.Message);
				return null;
			}
		}

		public DataRow QueryFirstRow(string query)
		{
			var table = Query(query);
			return !table.Equals(null) && table.Rows.Count > 0 ? table.Rows[0] : null;
		}

		private string MySqlEscape(string usString)
		{
			if(usString == null)
				return null;

			return Regex.Replace(usString, @"[\r\n\x00\x1a\\'""]", @"\$0");
		}
	}
}