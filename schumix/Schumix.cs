/*
 * This file is part of Schumix.
 * 
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
using System.Collections.Generic;
using Schumix.Config;

namespace Schumix
{
	class SchumixBot
	{
        /// <summary>
        ///     A Schumix bot revision száma.
        /// </summary>
		public static string revision = "0.2.2";

        /// <summary>
        ///     Az éppen használt nick.
        /// </summary>
		public static string NickTarolo;

        /// <summary>
        ///     A bot elindításának ideje.
        /// </summary>
		public static DateTime StartTime;

        /// <summary>
        ///     Ebbe a változóba töltödik bele a szoba neve és a jelszava, ha van.
        /// </summary>
		public static Dictionary<string,string> m_ChannelLista = new Dictionary<string,string>();

        ///***START***********************************///
        /// <summary>
        ///     A MySQL class-t hívja meg.
        ///     Dekralálja a MySQL kapcsolódást.
        /// </summary>
		public static Mysql mSQLConn;

		public SchumixBot()
		{
			try
			{
				//ConfigFajl();
				new Config.Config("schumix.xml");
				mSQLConn = new Mysql(MysqlConfig.Host, MysqlConfig.User, MysqlConfig.Password, MysqlConfig.Database);
				Log.Notice("SchumixBot", "Mysql adatbazishoz sikeres a kapcsolodas.");

				var db = mSQLConn.QueryRow(String.Format("SELECT szoba, jelszo FROM channel"));
				for(int i = 0; i < db.Rows.Count; ++i)
				{
					var row = db.Rows[i];
					string channel = row["szoba"].ToString();
					string jelszo = row["jelszo"].ToString();
	
					m_ChannelLista.Add(channel, jelszo);
				}

				Log.Debug("SchumixBot", "Consol thread indul...");
				new Consol();
				NickTarolo = IRCConfig.NickName;
				StartTime = DateTime.Now;

				new Network(IRCConfig.Server, IRCConfig.Port);
			}
			catch(Exception e)
			{
				Log.Error("SchumixBot", String.Format("Hiba oka: {0}", e.ToString()));
				Thread.Sleep(50);
			}
		}

        /// <summary>
        ///     Ha lefut, akkor leáll a class.
        /// </summary>
		~SchumixBot()
		{
			Log.Debug("SchumixBot", "~SchumixBot()");
		}

        /// <returns>
        ///     Megmutatja mennyi ideje üzemel a program.
        /// </returns>
		public static string Uptime()
		{
			var Time = DateTime.Now - StartTime;
			return String.Format("{0} nap, {1} óra, {2} perc, {3} másodperc.", Time.Days, Time.Hours, Time.Minutes, Time.Seconds);
		}
	}
}
