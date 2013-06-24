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

namespace Schumix.Framework.Config
{
	public static class Consts
	{
		private static readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		public const string SchumixDescription = "Schumix IRC bot";
#if DEBUG
		public const string SchumixConfiguration = "Debug";
#else
		public const string SchumixConfiguration = "Release";
#endif
		public const string SchumixCompany = "Schumix Productions";
		public const string SchumixProduct = "Schumix";
		public const string SchumixCopyright = "Copyright (C) 2013 Schumix Team <http://schumix.eu/>";
		public const string SchumixTrademark = "GNU General Public License";
		public const string SchumixVersion = "4.1.1";
		public const string SchumixFileVersion = "4.1.1.0";
		public const string SchumixProgrammedBy = "Csaba Jakosa (Megax)";
		public const string SchumixDevelopers = "Csaba Jakosa (Megax), Twl, Jackneill, Invisible, mqmq0, AgeNt";
		public const string SchumixWebsite = "https://github.com/Schumix/Schumix2";
		public static string SchumixUserAgent = SchumixBase.Title + SchumixBase.Space + sUtilities.GetVersion() + " / .NET " + Environment.Version;
		public const string SchumixReferer = "http://yeahunter.hu";
	}
}