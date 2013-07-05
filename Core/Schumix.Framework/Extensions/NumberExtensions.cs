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

namespace Schumix.Framework.Extensions
{
	public static class NumberExtensions
	{
		public static bool IsNumber(this string Text)
		{
			double number;
			return double.TryParse(Text, out number);
		}

		public static double ToNumber(this string Text)
		{
			double number;
			return double.TryParse(Text, out number) ? number : 0;
		}

		public static double ToNumber(this string Text, int Else)
		{
			double number;
			return double.TryParse(Text, out number) ? number : Else;
		}

		public static int ToInt(this double Double)
		{
			return Convert.ToInt32(Double);
		}
	}
}