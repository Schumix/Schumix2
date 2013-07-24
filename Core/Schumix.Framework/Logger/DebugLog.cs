/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013 Schumix Team <http://schumix.eu/>
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
using Schumix.Framework.Config;
using Schumix.Framework.Localization;

namespace Schumix.Framework.Logger
{
	public sealed class DebugLog
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private string _debuglogfile;

		public DebugLog(string dlfilename)
		{
			sUtilities.CreateDirectory(Path.Combine(LogConfig.LogDirectory, "DebugLog"));
			_debuglogfile = sUtilities.DirectoryToSpecial(Path.Combine(LogConfig.LogDirectory, "DebugLog"), dlfilename);

			bool isfile = false;
			if(File.Exists(_debuglogfile))
				isfile = true;

			var time = DateTime.Now;
			sUtilities.CreateFile(_debuglogfile);
			var file = new StreamWriter(_debuglogfile, true) { AutoFlush = true };

			if(!isfile)
				file.Write(sLConsole.GetString("Started time: [{0}]\n"), time.ToString("yyyy. MM. dd. HH:mm:ss"));
			else
				file.Write(sLConsole.GetString("\nStarted time: [{0}]\n"), time.ToString("yyyy. MM. dd. HH:mm:ss"));

			file.Close();
		}

		public void LogInFile(string log)
		{
			var filesize = new FileInfo(_debuglogfile);

			if(filesize.Length >= LogConfig.MaxFileSize * 1024 * 1024)
			{
				File.Delete(_debuglogfile);
				sUtilities.CreateFile(_debuglogfile);
			}

			var time = DateTime.Now;
			var file = new StreamWriter(_debuglogfile, true) { AutoFlush = true };
			file.WriteLine("{0} {1}", time.ToString("yyyy. MM. dd. HH:mm:ss"), log);
			file.Close();
		}
	}
}