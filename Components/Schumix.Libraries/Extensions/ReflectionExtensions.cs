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
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Schumix.Libraries.Extensions
{
	/// <summary>
	/// Reflection related extension methods.
	/// </summary>
	public static class ReflectionExtensions
	{
		/// <summary>
		/// Checks if the type is a simple type.
		/// 
		/// Simple types are primitive types and strings.
		/// </summary>
		[Pure]
		public static bool IsSimple(this Type type)
		{
			Contract.Requires(!type.IsNull());
			return type.IsEnum || type.IsNumeric() || type == typeof(string) || type == typeof(char) || type == typeof(bool);
		}

		/// <summary>
		/// Determines whether the specified type is numeric.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>
		/// 	<c>true</c> if the specified type is numeric; otherwise, <c>false</c>.
		/// </returns>
		[Pure]
		public static bool IsNumeric(this Type type)
		{
			Contract.Requires(!type.IsNull());
			return type.IsInteger() || type.IsFloatingPoint();
		}

		/// <summary>
		/// Determines whether the specified type is floating point.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>
		/// 	<c>true</c> if floating point; otherwise, <c>false</c>.
		/// </returns>
		[Pure]
		public static bool IsFloatingPoint(this Type type)
		{
			Contract.Requires(!type.IsNull());
			return type == typeof(float) || type == typeof(double) || type == typeof(decimal);
		}

		/// <summary>
		/// Determines whether the specified type is integer.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>
		/// 	<c>true</c> if the specified type is integer; otherwise, <c>false</c>.
		/// </returns>
		[Pure]
		public static bool IsInteger(this Type type)
		{
			Contract.Requires(!type.IsNull());
			return type == typeof(int) || type == typeof(uint) || type == typeof(short) || type == typeof(ushort) ||
				type == typeof(byte) || type == typeof(sbyte) || type == typeof(long) || type == typeof(ulong);
		}

		/// <summary>
		/// Gets the custom attributes of the specified type.
		/// </summary>
		/// <typeparam name="T">Type of attribute to check</typeparam>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		[Pure]
		public static T[] GetCustomAttributes<T>(this ICustomAttributeProvider type)
			where T : Attribute
		{
			Contract.Requires(!type.IsNull());
			Contract.Ensures(!Contract.Result<T[]>().IsNull());
			var attribs = type.GetCustomAttributes(typeof(T), false) as T[];
			Contract.Assume(!attribs.IsNull());
			return attribs;
		}

		/// <summary>
		/// Gets the custom attribute of the specified type.
		/// </summary>
		/// <typeparam name="T">The attribute to check</typeparam>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		[Pure]
		public static T GetCustomAttribute<T>(this ICustomAttributeProvider type)
			where T : Attribute
		{
			Contract.Requires(!type.IsNull());
			return type.GetCustomAttributes<T>().TryGet(0);
		}

		/// <summary>
		/// Gets the types that implement the specified interface in the assembly.
		/// </summary>
		/// <param name="asm">The assembly to check in.</param>
		/// <param name="interfaceType">The type to check.</param>
		/// <returns></returns>
		public static List<Type> GetTypesWithInterface(this Assembly asm, Type interfaceType)
		{
			Contract.Requires(!interfaceType.IsNull());
			var types = (from t in asm.GetTypes().AsParallel() where t.GetInterfaces().Contains(interfaceType) select t).ToList();
			return types;
		}
	}
}