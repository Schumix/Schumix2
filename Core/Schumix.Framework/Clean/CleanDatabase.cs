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
				// szöveg hogy elindult
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
			// ez a függvény fogja ellátni az addonokat a törlés lehetőségével
		}

		private void CleanCoreTable()
		{
			// ide jön minden tábla ami a magban van
		}
	}
}