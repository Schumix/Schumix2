/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Twl
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
using System.Runtime.Serialization.Formatters.Binary;
using Schumix.Framework.Config;
using Schumix.Framework.Localization;

namespace Schumix.Framework
{
	/// <summary>
	/// Used to create crash dumps which are very useful for investigation of problems.
	/// </summary>
	public class CrashDumper
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private CrashDumper() {}

		/// <summary>
		/// Creates the crash dump.
		/// </summary>
		public void CreateCrashDump(object Object)
		{
			sUtilities.CreateDirectory(CrashConfig.Directory);
			Log.Debug("CrashDumper", sLConsole.CrashDumper("Text"));

			try
			{
				using(var fs = File.Open(Path.Combine(Environment.CurrentDirectory, CrashConfig.Directory,
					string.Format("{0}.acd", DateTime.Now.ToString("yyyy_MM_dd_HH_mm"))), FileMode.Create))
				{
					var formatter = new BinaryFormatter();
					formatter.Serialize(fs, Object);
				}
			}
			catch(Exception e)
			{
				Log.Error("CrashDumper", sLConsole.CrashDumper("Text2"), e.Message);
				return;
			}

			Log.Debug("CrashDumper", sLConsole.CrashDumper("Text3"));
		}
	}
}