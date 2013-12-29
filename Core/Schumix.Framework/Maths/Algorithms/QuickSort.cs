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

namespace Schumix.Framework.Maths.Algorithms
{
	/// <summary>
	/// Provides a generic implementation of the quick sort algorithm
	/// </summary>
	/// <typeparam name="T">Type of the array which is to be processed.</typeparam>
	public class QuickSort<T> where T : IComparable
	{
		private readonly T[] _input;

		/// <summary>
		/// Creates a new instance of QuickSort
		/// </summary>
		/// <param name="values">List of values the instance should process.</param>
		public QuickSort(IList<T> values)
		{
			_input = new T[values.Count];

			for(var i = 0; i < values.Count; i++)
				_input[i] = values[i];
		}

		/// <summary>
		/// The sorted output.
		/// </summary>
		public T[] Output
		{
			get
			{
				return _input;
			}
		}

		/// <summary>
		/// Sorts the list manually.
		/// </summary>
		public void Sort()
		{
			Sorting(0, _input.Length - 1);
		}

		private int GetPivotPoint(int begPoint, int endPoint)
		{
			var pivot = begPoint;
			var m = begPoint + 1;
			var n = endPoint;

			while((m < endPoint) && (_input[pivot].CompareTo(_input[m]) >= 0))
				m++;

			while((n > begPoint) && (_input[pivot].CompareTo(_input[n]) <= 0))
				n--;

			while(m < n)
			{
				var temp = _input[m];
				_input[m] = _input[n];
				_input[n] = temp;

				while((m < endPoint) && (_input[pivot].CompareTo(_input[m]) >= 0))
					m++;

				while((n > begPoint) && (_input[pivot].CompareTo(_input[n]) <= 0))
					n--;
			}

			if(pivot != n)
			{
				var temp2 = _input[n];
				_input[n] = _input[pivot];
				_input[pivot] = temp2;
			}

			return n;
		}

		private void Sorting(int beg, int end)
		{
			if(end == beg)
				return;

			var pivot = GetPivotPoint(beg, end);

			if(pivot > beg)
				Sorting(beg, pivot - 1);

			if(pivot < end)
				Sorting(pivot + 1, end);
		}
	}
}