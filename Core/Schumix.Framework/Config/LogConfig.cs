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

namespace Schumix.Framework.Config
{
	public sealed class LogConfig
	{
		public static string FileName { get; private set; }
		public static bool DateFileName { get; private set; }
		public static int MaxFileSize { get; private set; }
		public static int LogLevel { get; private set; }
		public static string LogDirectory { get; private set; }
		public static string IrcLogDirectory { get; private set; }
		public static bool IrcLog { get; private set; }

		public LogConfig(string filename, bool datefilename, int maxfilesize, int loglevel, string logdirectory, string irclogdirectory, bool irclog)
		{
			FileName        = filename;
			DateFileName    = datefilename;
			MaxFileSize     = maxfilesize;
			LogLevel        = loglevel;
			LogDirectory    = logdirectory;
			IrcLogDirectory = irclogdirectory;
			IrcLog          = irclog;
		}
	}
}