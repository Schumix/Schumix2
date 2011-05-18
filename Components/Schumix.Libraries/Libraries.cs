/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2011 Megax <http://www.megaxx.info/>
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
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace Schumix.Libraries
{
	public static class Lib
	{
		public static bool IsPrime(long x)
		{
			x = Math.Abs(x);

			if(x == 1 || x == 0)
				return false;

			if(x == 2)
				return true;

			if(x % 2 == 0)
				return false;

			bool p = true;

			for(var i = 3; i <= Math.Floor(Math.Sqrt(x)); i += 2)
			{
				if(x % i == 0)
				{
					p = false;
					break;
				}
			}

			return p;
		}

		public static string Regex(this string text, string regex)
		{
			var x = new Regex(regex);

			if(x.IsMatch(text))
			{
				string s = string.Empty;

				for(int a = 1; a < x.Match(text).Length; a++)
					s += " " + x.Match(text).Groups[a].ToString();

				s = s.Remove(0, 1);
				return s;
			}
			else
				return "Hibás regex!";
		}

		public static string Regex(this string text, string regex, string groups)
		{
			var x = new Regex(regex);

			if(x.IsMatch(text))
				return x.Match(text).Groups[groups].ToString();
			else
				return "Hibás regex!";
		}

		/// <summary>
		/// Casts the object to the specified type.
		/// </summary>
		/// <typeparam name="T">The type to cast to.</typeparam>
		/// <param name="ob">Object to cast</param>
		/// <returns>The casted object.</returns>
		public static T Cast<T>(this object ob)
		{
			return (T)ob;
		}

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

			foreach(var str in arr)
				sb.AppendFormat("{0}{1}", str, separator);

			return sb.ToString();
		}

		public static string SplitToString(this string[] split, int min, string s)
		{
			string ss = string.Empty;

			for(int x = min; x < split.Length; x++)
				ss += s + split[x];

			if(ss.Length > 0 && ss.Substring(0, s.Length) == s)
				ss = ss.Remove(0, s.Length);

			return ss;
		}

		public static string Remove(this string s, int min, int max, string value)
		{
			if(s.Length >= max && s.Substring(min, max) == value)
				return s.Remove(min, max);
			else
				return s;
		}

		public static bool IsUpper(this string value)
		{
			// Consider string to be uppercase if it has no lowercase letters.
			for(int i = 0; i < value.Length; i++)
			{
				if(char.IsLower(value[i]))
					return false;
			}

			return true;
		}

		public static bool IsLower(this string value)
		{
			// Consider string to be lowercase if it has no uppercase letters.
			for(int i = 0; i < value.Length; i++)
			{
				if(char.IsUpper(value[i]))
					return false;
			}

			return true;
		}
	}
}

