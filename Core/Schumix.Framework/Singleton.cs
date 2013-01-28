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
using System.Reflection;
using Schumix.Framework.Exceptions;
using Schumix.Framework.Extensions;

namespace Schumix.Framework
{
	/// <summary>
	/// Manages the single instance of a class.
	/// </summary>
	/// <remarks>
	/// Generic variant of the strategy presented here : http://geekswithblogs.net/akraus1/articles/90803.aspx.
	/// Prefered to http://www.yoda.arachsys.com/csharp/singleton.html, where static initialization doesn't allow
	/// proper handling of exceptions, and doesn't allow retrying type initializers initialization later
	/// (once a type initializer fails to initialize in .NET, it can't be re-initialized again).
	/// </remarks>
	/// <typeparam name="T">Type of the singleton class.</typeparam>
	public class Singleton<T> where T : class
	{
		/// <summary>
		/// The single instance of the target class.
		/// </summary>
		/// <remarks>
		/// The volatile keyword makes sure to remove any compiler optimization that could make concurrent 
		/// threads reach a race condition with the double-checked lock pattern used in the Instance property.
		/// See http://www.bluebytesoftware.com/blog/PermaLink,guid,543d89ad-8d57-4a51-b7c9-a821e3992bf6.aspx
		/// </remarks>
		private static volatile T _instance;

		/// <summary>
		/// The dummy object used for locking.
		/// </summary>
		private static object _lock = new object();

		/// <summary>
		/// Type-initializer to prevent type to be marked with beforefieldinit.
		/// </summary>
		/// <remarks>
		/// This simply makes sure that static fields initialization occurs 
		/// when Instance is called the first time and not before.
		/// </remarks>
		public Singleton() {}

		/// <summary>
		/// Gets the single instance of the class.
		/// </summary>
		public static T Instance
		{
			get
			{
				if(_instance.IsNull())
				{
					lock(_lock)
					{
						ConstructorInfo constructor = null;

						try
						{
							// Binding flags exclude public constructors.
							constructor = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[0], null);
						}
						catch(Exception e)
						{
							Log.Error("Singleton", "{0}", e.Message);
						}

						if(constructor.IsNull() || constructor.IsAssembly) // Also exclude internal constructors.
							Log.Error("Singleton", "A private or protected constructor is missing for '{0}'.", typeof(T).Name);

						_instance = (T)constructor.Invoke(null);
					}
				}

				return _instance;
			}
		}
	}
}