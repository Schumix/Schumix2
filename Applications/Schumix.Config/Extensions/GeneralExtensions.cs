/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Twl
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
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Schumix.Config.Extensions
{
	/// <summary>
	/// Some random extension stuff.
	/// </summary>
	static class GeneralExtensions
	{
		/// <summary>
		/// Determines whether the specified obj is null.
		/// </summary>
		/// <param name="obj">The obj.</param>
		/// <returns>
		/// 	<c>true</c> if the specified obj is null; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsNull(this object obj)
		{
			return (obj == null);
		}

		public static bool IsNull(this IntPtr ptr)
		{
			return (ptr.Equals(IntPtr.Zero));
		}

		public static bool IsNumber(this string value)
		{
			double number;
			return double.TryParse(value, out number);
		}

		public static bool IsBoolean(this string value)
		{
			bool b;
			return bool.TryParse(value, out b);
		}
	}
}