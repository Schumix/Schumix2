/*
 * This file is part of Schumix.
 * 
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
using System.Data;
using System.Collections.Generic;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Framework.Clean
{
	public sealed class CleanDatabase
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private bool _clean;
		public bool IsClean() { return _clean; }

		public CleanDatabase()
		{
			try
			{
				Log.Notice("CleanDatabase", sLConsole.CleanDatabase("Text"));
				if(!Schumix.Framework.Config.CleanConfig.Database)
				{
					_clean = true;
					return;
				}

				CleanCoreTable();
			}
			catch(Exception e)
			{
				Log.Error("CleanDatabase", sLConsole.Exception("Error"), e.Message);
				_clean = false;
			}

			_clean = true;
		}

		public void CleanTable(string table)
		{
			Log.Debug("CleanDatabase", sLConsole.CleanDatabase("Text2"), table);

			var db = SchumixBase.DManager.Query("SELECT ServerName FROM {0} GROUP BY ServerName", table);
			if(!db.IsNull())
			{
				foreach(DataRow row in db.Rows)
				{
					string name = row["ServerName"].ToString();

					if(!IRCConfig.List.ContainsKey(name))
					{
						SchumixBase.DManager.Delete(table, string.Format("ServerName = '{0}'", name));
						Log.Debug("CleanDatabase", sLConsole.CleanDatabase("Text3"), name, table);
					}
				}
			}

			Log.Debug("CleanDatabase", sLConsole.CleanDatabase("Text4"), table);
		}

		private void CleanCoreTable()
		{
			Log.Notice("CleanDatabase", sLConsole.CleanDatabase("Text5"));
			CleanTable("channels");
			CleanTable("schumix");
			CleanTable("hlmessage");
			CleanTable("admins");
			CleanTable("ignore_addons");
			CleanTable("ignore_channels");
			CleanTable("ignore_commands");
			CleanTable("ignore_irc_commands");
			CleanTable("ignore_nicks");
			Log.Notice("CleanDatabase", sLConsole.CleanDatabase("Text6"));
		}
	}
}