/*
 * This file is part of Schumix.
 * 
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

namespace Schumix.Config.Config
{
	static class Consts
	{
		public const string ConfigDescription = "Schumix2 IRC Bot and Framework";
#if DEBUG
		public const string ConfigConfiguration = "Debug";
#else
		public const string ConfigConfiguration = "Release";
#endif
		public const string ConfigCompany = "Schumix Productions";
		public const string ConfigProduct = "Schumix";
		public const string ConfigCopyright = "Copyright (C) 2013 Schumix Team <http://schumix.eu/>";
		public const string ConfigTrademark = "GNU General Public License";
		public const string ConfigVersion = "0.5.2";
		public const string ConfigFileVersion = "0.5.2.0";
		public const string ConfigProgrammedBy = "Csaba Jakosa (Megax)";
		public const string ConfigWebsite = "https://github.com/Schumix/Schumix2";
	}
}