/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Twl
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013-2015 Schumix Team <http://schumix.eu/>
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
using Schumix.Config.Util;
using Schumix.Config.Logger;

namespace Schumix.Config
{
	/// <summary>
	/// Used to create crash dumps which are very useful for investigation of problems.
	/// </summary>
	class CrashDumper
	{
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private string _dir = "Dumps";
		private CrashDumper() {}

		/// <summary>
		/// Creates the crash dump.
		/// </summary>
		public void CreateCrashDump(object Object)
		{
			sUtilities.CreateDirectory(_dir);
			Log.Debug("CrashDumper", "Creating crash dump...");

			try
			{
				using(var fs = File.Open(Path.Combine(Environment.CurrentDirectory, _dir,
					string.Format("{0}.acd", DateTime.Now.ToString("yyyy_MM_dd_HH_mm"))), FileMode.Create))
				{
					var formatter = new BinaryFormatter();
					formatter.Serialize(fs, Object);
				}
			}
			catch(Exception e)
			{
				Log.Error("CrashDumper", "Failed to write crash dump! ({0})", e.Message);
				return;
			}

			Log.Debug("CrashDumper", "Crash dump created.");
		}

		public void SetDirectory(string dir)
		{
			_dir = dir;
		}
	}
}