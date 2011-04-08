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
	public class SchumixBase
	{
		public static DatabaseManager DManager { get; private set; }
		public static Time time { get; private set; }
		public static bool IIdo = true;
		public static string Title = "Schumix2 IRC Bot";

		public SchumixBase()
		{
			try
			{
				Log.Debug("SchumixBase", "Time indul...");
				time = new Time();
				Log.Debug("SchumixBase", "Mysql indul...");
				DManager = new DatabaseManager();
				Log.Notice("SchumixBase", "Az adatbazishoz sikeres a kapcsolodas.");

				SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Channel = '{0}' WHERE Id = '1'", IRCConfig.MasterChannel);
				Log.Notice("SchumixBase", "Mester csatorna frissitve lett erre: {0}", IRCConfig.MasterChannel);

				if(AddonsConfig.Enabled)
				{
					AddonManager.Initialize();
					AddonManager.LoadPluginsFromDirectory(AddonsConfig.Directory);
				}
			}
			catch(Exception e)
			{
				Log.Error("SchumixBase", "Hiba oka: {0}", e.Message);
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

