/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Twl
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
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Schumix.Libraries.Maths.Types
{
	/// <summary>
	/// An array which automatically sorts itself.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public sealed class AutoSortedArray<T> where T: IComparable
	{
		private List<T> _array;

		/// <summary>
		/// Gets the sorted array.
		/// </summary>
		public List<T> Array { get; private set; }

		/// <summary>
		/// Creates a new instance of <see>AutoSortedArray</see>
		/// </summary>
		/// <param name="list">List of items it should contain.</param>
		public AutoSortedArray(IEnumerable<T> list)
		{
			_array = list as List<T>;
			//sort here.
			Sort();
		}

		/// <summary>
		/// Creates a new instance of <see>AutoSortedArray</see> with an empty list.
		/// </summary>
		public AutoSortedArray()
		{
			_array = new List<T>();
		}

		/// <summary>
		/// Add the specified item to the sorted array.
		/// </summary>
		/// <param name="item">The item to add.</param>
		public void Add(T item)
		{
			_array.Add(item);
			Sort();
		}

		/// <summary>
		/// Adds the specified items to the sorted array.
		/// </summary>
		/// <param name="items">Items to add.</param>
		public void Add(params T[] items)
		{
			foreach(var item in items)
				_array.Add(item);

			Sort();
		}

		/// <summary>
		/// Adds the specified item to the array without running sort.
		/// </summary>
		/// <param name="item">The item to add</param>
		public void SimpleAdd(T item)
		{
			_array.Add(item);
		}

		/// <summary>
		/// Runs the sort algorithm manually.
		/// </summary>
		public void Sort()
		{
			_array = MathFunctions.QuickSort(_array);	
		}

		/// <summary>
		/// Converts the array to a readable string.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			var builder = new StringBuilder();
			builder.Append('{');

			for(var index = 0; index < _array.Count; index++)
			{
				var item = _array[index];
				builder.AppendFormat(index == _array.Count - 1 ? "{0}" : "{0}, ", Convert.ToString(item));
			}

			builder.Append('}');
			return builder.ToString();
		}
	}
}