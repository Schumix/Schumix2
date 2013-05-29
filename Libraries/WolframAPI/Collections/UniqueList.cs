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
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Schumix.Framework;
using Schumix.Framework.Localization;

namespace WolframAPI.Collections
{
	/// <summary>
	/// Provides a list type which supports only unique list of elements.
	/// </summary>
	[Serializable]
	public sealed class UniqueList<T> : List<T> where T : IEquatable<T>, IEquatable<string>
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		//private readonly Utilities sUtilities = Singleton<Utilities>.Instance;

		/// <summary>
		/// Adds the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		public new void Add(T item)
		{
			//if(sPlatform.GetPlatformType() != PlatformType.Linux)
			//	Contract.Requires(!Equals(item, null));
            
			if(Contains(item) || FindIndex(it => it.Equals(item)) != -1)
				throw new InvalidOperationException(sLConsole.GetString("The unique list already contains that element."));

			base.Add(item);
        }

		/// <summary>
		/// Gets or sets the element with the specified index.
		/// </summary>
		/// <value></value>
		public T this[T ind]
		{
			get
			{
				//if(sPlatform.GetPlatformType() != PlatformType.Linux)
				//	Contract.Requires(!Equals(ind, null));

				return (from elem in this where elem.Equals(ind) select elem).FirstOrDefault();
			}
			set
			{
				//if(sPlatform.GetPlatformType() != PlatformType.Linux)
				//{
				//	Contract.Requires(!Equals(ind, null));
				//	Contract.Requires(!Equals(value, null));
				//}

				int index;
				if((index = FindIndex(it => it.Equals(ind))) != -1)
					this[index] = value;
			}
		}

		/// <summary>
		/// Gets or sets the element with the specified index.
		/// </summary>
		/// <value></value>
		public T this[string ind]
		{
			get
			{
				return (from elem in this where elem.Equals(ind) select elem).FirstOrDefault();
			}
			set
			{
				int index;
				if((index = FindIndex(it => it.Equals(ind))) != -1)
					this[index] = value;
			}
		}
	}
}