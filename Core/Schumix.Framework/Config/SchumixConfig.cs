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
	public sealed class SchumixConfig
	{
		public static string ConfigDirectory { get; private set; }
		public static string ConfigFile { get; private set; }
		public static bool ColorBindMode { get; private set; }

		public SchumixConfig(string configdirectory, string configfile, bool colorbindmode)
		{
			ConfigDirectory = configdirectory;
			ConfigFile      = configfile;
			ColorBindMode   = colorbindmode;
		}
	}
}