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
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Schumix.Installer.Extensions
{
	/// <summary>
	/// Some random extension stuff.
	/// </summary>
	public static class GeneralExtensions
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

		/// <summary>
		/// Determines whether the specified obj is a type of the specified type.
		/// </summary>
		/// <param name="obj">The obj.</param>
		/// <param name="type">The type.</param>
		/// <returns>
		/// 	<c>true</c> if the specified obj is a type of the specified type; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsOfType(this object obj, Type type)
		{
			if(obj.IsNull())
				return false;

			return (obj.GetType() == type);
		}

		/// <summary>
		/// Determines whether this instance can be casted to the specified type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj">The obj.</param>
		/// <returns>
		/// 	<c>true</c> if this instance can be casted to the specified type; otherwise, <c>false</c>.
		/// </returns>
		public static bool CanBeCastedTo<T>(this object obj)
		{
			if(obj.IsNull())
				throw new ArgumentNullException("obj");

			return (obj is T);
		}

		/// <summary>
		/// Concatenates the string in the specified array and returns the sum string.
		/// </summary>
		/// <param name="arr"></param>
		/// <returns></returns>
		public static string Concatenate(this IEnumerable<string> arr)
		{
			var sb = new StringBuilder();

			foreach(var str in arr)
				sb.Append(str);

			return sb.ToString();
		}

		/// <summary>
		/// Concatenates the string in the specified array and returns the sum string.
		/// </summary>
		/// <param name="arr"></param>
		/// <param name="separator">The separator to use between parts.</param>
		/// <returns></returns>
		public static string Concatenate(this IEnumerable<string> arr, string separator)
		{
			var sb = new StringBuilder();
			var warr = arr.ToArray();

			for(var index = 0; index < warr.Length; index++)
			{
				var str = warr[index];

				if(index == warr.Length - 1)
					sb.AppendFormat("{0}", str);
				else
					sb.AppendFormat("{0}{1}", str, separator);
			}

			return sb.ToString();
		}

		/// <summary>
		/// Concatenates the string in the specified array and returns the sum string.
		/// Uses spaces as separators.
		/// </summary>
		/// <param name="arr"></param>
		/// <returns></returns>
		public static string ConcatenateWithSpaces(this IEnumerable<string> arr)
		{
			return arr.Concatenate(" ");
		}

		/// <summary>
		/// Waits for the pending tasks in the specified collection.
		/// </summary>
		/// <param name="coll">The collection.</param>
		public static void WaitTasks(this IEnumerable<Task> coll)
		{
			if(coll.IsNull())
				throw new ArgumentNullException("coll");

			Task.WaitAll(coll.ToArray());
		}
	}
}