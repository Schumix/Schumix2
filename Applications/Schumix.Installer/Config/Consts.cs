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

namespace Schumix.Installer.Config
{
	public static class Consts
	{
		private static readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		public const string SchumixDescription = "Schumix2 IRC Bot and Framework";
#if DEBUG
		public const string SchumixConfiguration = "Debug";
#else
		public const string SchumixConfiguration = "Release";
#endif
		public const string SchumixCompany = "Schumix Productions";
		public const string SchumixProduct = "Schumix";
		public const string SchumixCopyright = "Copyright (C) 2013 Schumix Team <http://schumix.eu/>";
		public const string SchumixTrademark = "GNU General Public License";
		public const string SchumixVersion = "0.0.9";
		public const string SchumixFileVersion = "0.0.9.0";
		public static string SchumixUserAgent = "Schumix2 Installer " + sUtilities.GetVersion() + " / .NET " + Environment.Version;
		public const string SchumixReferer = "http://yeahunter.hu";
	}
}