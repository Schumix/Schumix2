/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013-2014 Schumix Team <http://schumix.eu/>
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
using System.Linq;

namespace Schumix.Config.Extensions
{
	public static class StringExtensions
	{
		public static string Remove(this string s, int min, int max, char value)
		{
			return (s.Length >= max && s.Substring(min, max) == value.ToString()) ? s.Remove(min, max) : s;
		}

		public static string Remove(this string s, int min, int max, string value)
		{
			return (s.Length >= max && s.Substring(min, max) == value) ? s.Remove(min, max) : s;
		}

		public static bool IsNullOrEmpty(this string Value)
		{
			return string.IsNullOrEmpty(Value.IsNull() ? Value : Value.Trim());
		}
	}
}