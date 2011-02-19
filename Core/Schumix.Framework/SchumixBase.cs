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
using Schumix.Framework.Config;
using Schumix.Framework.Database;

namespace Schumix.Framework
{
	public class SchumixBase : Config.Config
	{
        ///***START***********************************///
        /// <summary>
        ///     A MySQL class-t hívja meg.
        ///     Dekralálja a MySQL kapcsolódást.
        /// </summary>
		public static Mysql mSQLConn { get; private set; }
		public static Time time { get; private set; }
		public static bool IIdo = true;
		public static string Title = "Schumix2 IRC Bot";

		public SchumixBase(string config) : base(config)
		{
			try
			{
				Log.Debug("SchumixBase", "Time indul...");
				time = new Time();
				Log.Debug("SchumixBae", "Mysql indul...");
				mSQLConn = new Mysql(MysqlConfig.Host, MysqlConfig.User, MysqlConfig.Password, MysqlConfig.Database);
				Log.Notice("SchumixBase", "Mysql adatbazishoz sikeres a kapcsolodas.");

				if(PluginsConfig.Allapot)
				{
					ScriptManager.Initialize();
					ScriptManager.LoadPlugins();
				}
			}
			catch(Exception e)
			{
				Log.Error("SchumixBase", "Hiba oka: {0}", e.ToString());
				Thread.Sleep(100);
			}
		}

        /// <summary>
        ///     Ha lefut, akkor leáll a class.
        /// </summary>
		~SchumixBase()
		{
			Log.Debug("SchumixBase", "~SchumixBase()");
		}
	}
}

