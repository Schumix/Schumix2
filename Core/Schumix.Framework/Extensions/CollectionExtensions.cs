/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Twl
 * Copyright (C) 2010-2012 Megax <http://www.megaxx.info/>
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

using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Schumix.Framework.Extensions
{
	/// <summary>
	/// Collection related extension methods.
	/// </summary>
	public static class CollectionExtensions
	{
		/// <summary>
		/// Returns the entry in this list at the given index, or the default value of the element
		/// type if the index was out of bounds.
		/// </summary>
		/// <typeparam name="T">The type of the elements in the list.</typeparam>
		/// <param name="list">The list to retrieve from.</param>
		/// <param name="index">The index to try to retrieve at.</param>
		/// <returns>The value, or the default value of the element type.</returns>
		public static T TryGet<T>(this IList<T> list, int index)
		{
			Contract.Requires(list != null);
			Contract.Requires(index >= 0);
			return index >= list.Count ? default(T) : list[index];
		}

		/// <summary>
		/// Returns the entry in this dictionary at the given key, or the default value of the key
		/// if none.
		/// </summary>
		/// <typeparam name="TKey">The key type.</typeparam>
		/// <typeparam name="TValue">The value type.</typeparam>
		/// <param name="dict">The dictionary to operate on.</param>
		/// <param name="key">The key of the element to retrieve.</param>
		/// <returns>The value (if any).</returns>
		public static TValue TryGet<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
		{
			Contract.Requires(dict != null);
			Contract.Requires(key != null);
			TValue val;
			return dict.TryGetValue(key, out val) ? val : default(TValue);
		}
	}
}
