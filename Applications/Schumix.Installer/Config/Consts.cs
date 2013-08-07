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
		public const string InstallerDescription = "Schumix2 IRC Bot and Framework";
#if DEBUG
		public const string InstallerConfiguration = "Debug";
#else
		public const string InstallerConfiguration = "Release";
#endif
		public const string InstallerCompany = "Schumix Productions";
		public const string InstallerProduct = "Schumix";
		public const string InstallerCopyright = "Copyright (C) 2013 Schumix Team <http://schumix.eu/>";
		public const string InstallerTrademark = "GNU General Public License";
		public const string InstallerVersion = "0.4.0";
		public const string InstallerFileVersion = "0.4.0";
		public static string InstallerUserAgent = "Schumix2 Installer " + sUtilities.GetVersion() + " / .NET " + Environment.Version;
		public const string InstallerReferer = "http://yeahunter.hu";
	}
}