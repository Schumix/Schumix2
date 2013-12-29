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
using System.Collections.Generic;
using Schumix.Libraries.Maths.Algorithms;

namespace Schumix.Libraries.Maths
{
    /// <summary>
    /// Provides simple mathematic methods.
    /// </summary>
    public static class MathFunctions
    {
		/// <summary>
		/// Applies the quicksort algorithm to a specified list.
		/// </summary>
		/// <typeparam name="T">Type of the list</typeparam>
		/// <param name="list">The list to process.</param>
		/// <returns>The sorted list.</returns>
		public static List<T> QuickSort<T>(IList<T> list) where T: IComparable
		{
			var qs = new QuickSort<T>(list);
			qs.Sort();
			return new List<T>(qs.Output);
		}

		/// <summary>
		/// Applies the quicksort algorithm to a specified array.
		/// </summary>
		/// <typeparam name="T">Type of the array</typeparam>
		/// <param name="arr">The array to process.</param>
		/// <returns>The sorted array</returns>
		public static T[] QuickSort<T>(T[] arr) where T: IComparable
		{
			var qs = new QuickSort<T>(arr);
			qs.Sort();
			return qs.Output;
		}
	}
}