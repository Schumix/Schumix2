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
using System.IO;
using System.Linq;
using Schumix.Framework.Config;
using Schumix.Framework.Localization;

namespace Schumix.Framework.Clean
{
	public sealed class CleanConfig
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private bool _clean;
		public bool IsClean() { return _clean; }

		public CleanConfig()
		{
			try
			{
				Log.Notice("CleanConfig", "CleanConfig sikeresen elindult.");
				CleanOldConfigFile();
			}
			catch(Exception e)
			{
				Log.Error("CleanConfig", sLConsole.Exception("Error"), e.Message);
				_clean = false;
			}

			_clean = true;
		}

		private void CleanOldConfigFile()
		{
			Log.Notice("CleanConfig", "Régi konfig fájlok keresése elindult.");
			var dir = new DirectoryInfo(SchumixConfig.ConfigDirectory);
			Log.Notice("CleanConfig", "Konfig mapa elérhetőse: {0}", dir.FullName);
			int i = 0;

			foreach(var yml in dir.GetFiles("_*.yml").AsParallel())
			{
				i++;
				File.Delete(yml.FullName);
				Log.Debug("CleanConfig", "Eezn régi konfig fájl törölve lett: {0}", yml.Name);
			}

			foreach(var xml in dir.GetFiles("_*.xml").AsParallel())
			{
				i++;
				File.Delete(xml.FullName);
				Log.Debug("CleanConfig", "Ezen régi konfig fájl törölve lett: {0}", xml.Name);
			}

			if(i > 0)
				Log.Notice("CleanConfig", "Régi konfig fájlok sikeresen törölve.");
			else
				Log.Warning("CleanConfig", "Nincsenek régi konfig fájlok!");
		}
	}
}