/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2011 Megax <http://www.megaxx.info/>
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
		public const string SchumixDescription = "Schumix IRC bot";
#if DEBUG
#if MONO
		public const string SchumixConfiguration = "Mono-Debug";
#else
		public const string SchumixConfiguration = "Debug";
#endif
#else
#if MONO
		public const string SchumixConfiguration = "Mono-Release";
#else
		public const string SchumixConfiguration = "Release";
#endif
#endif
		public const string SchumixCompany = "Megax Productions";
		public const string SchumixProduct = "Schumix";
		public const string SchumixCopyright = "Copyright (C) 2010-2011 Megax <http://www.megaxx.info/>";
		public const string SchumixTrademark = "GNU General Public License";
		public const string SchumixVersion = "3.0.7";
		public const string SchumixFileVersion = "3.0.7.0";
	}
}