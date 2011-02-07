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
using System.Text.RegularExpressions;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Schumix.Framework.Database
{
	public class Mysql
	{
        /// <summary>
        ///     A MySQL adatbázishoz való csatlakozás deklarációja.
        /// </summary>
		private readonly MySqlConnection Connection;

        /// <summary>
        ///     A MySQL-hez való kapcsolódás.
        /// </summary>
        /// <param name="host">A MySQL hostja.</param>
        /// <param name="username">A MySQL bejelentkezõ felhasználónév.</param>
        /// <param name="password">A MySQL bejelentkezõ jelszó.</param>
        /// <param name="database">A MySQL adatbázis neve.</param>
		public Mysql(string host, string username, string password, string database)
		{
			Connection = new MySqlConnection(String.Format("SERVER={0};DATABASE={1};UID={2};PWD={3};", host, database, username, password));
			Connection.Open();

			Log.Notice("Mysql", "Mysql rendszer elindult.");
		}

        /// <summary>
        ///     Ha lefut, akkor leáll a class.
        /// </summary>
		~Mysql()
		{
			Log.Debug("Mysql", "~Mysql()");
			Connection.Close();
		}

        /// <summary>
        ///     Soronként olvassa a táblát.
        /// </summary>
        /// <remarks>
        ///     Egy sort egészben kiszed és feldolgozásra küldd, majd a következõt
        /// </remarks>
        /// <param name="query">A MySQL parancs.</param>
        /// <returns>
        ///     "return table" : Visszatér a MySQL táblához.
        ///     "return null"  : Null értékhez tér vissza.
        /// </returns>
		private DataTable Query(string query)
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

        /// <summary>
        ///     A MySQL tábla elsõ sorát adja vissza.
        /// </summary>
        /// <param name="query">A MySQL parancs.</param>
        /// <returns>
        ///     Ha "return null;" van akkor a visszatérési értéke null.
        /// </returns>
		public DataRow QueryFirstRow(string query)
		{
			var table = Query(query);

			if(!table.Equals(null) && table.Rows.Count > 0)
				return table.Rows[0];
			else
				return null;
		}

		public DataRow QueryFirstRow(string query, params object[] args)
		{
			return QueryFirstRow(String.Format(query, args));
		}

        /// <param name="query">A MySQL parancs.</param>
        /// <returns>
        ///     Az egész táblát adja vissza.
        /// </returns>
		public DataTable QueryRow(string query)
		{
			return Query(query);
		}

		public DataTable QueryRow(string query, params object[] args)
		{
			return QueryRow(String.Format(query, args));
		}

		private string MySqlEscape(string usString)
		{
			if(usString == null)
				return null;

			return Regex.Replace(usString, @"[\r\n\x00\x1a\\'""]", @"\$0");
		}
	}
}