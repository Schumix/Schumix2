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
using Schumix.Framework.Localization;

namespace Schumix.Framework.Config
{
	public sealed class UpdateConfig
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		public static bool Enabled { get; private set; }
		public static string Version { get; private set; }
		public static string Branch { get; private set; }
		public static string WebPage { get; private set; }

		public UpdateConfig(bool enabled, string version, string branch, string webpage)
		{
			Enabled = enabled;
			Version = version;
			Branch  = branch;
			WebPage = webpage;
			Log.Notice("UpdateConfig", sLConsole.UpdateConfig("Text"));
		}
	}
}