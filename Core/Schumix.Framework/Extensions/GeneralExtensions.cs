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
using System.Globalization;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using YamlDotNet.RepresentationModel;

namespace Schumix.Framework.Extensions
{
	/// <summary>
	/// Some random extension stuff.
	/// </summary>
	public static class GeneralExtensions
	{
		private static readonly Utilities sUtilities = Singleton<Utilities>.Instance;

		/// <summary>
		/// Casts the object to the specified type.
		/// </summary>
		/// <typeparam name="T">The type to cast to.</typeparam>
		/// <param name="ob">Object to cast</param>
		/// <returns>The casted object.</returns>
		public static T Cast<T>(this object ob)
		{
			Contract.Requires(!ob.IsNull());
			Contract.Ensures(!Contract.Result<T>().IsNull());
			var value = (T)Cast(ob, typeof(T));
			Contract.Assume(!value.IsNull());
			return value;
		}

		/// <summary>
		/// Casts the specified object to the specified type.
		/// </summary>
		/// <param name="ob">The object to cast.</param>
		/// <param name="targetType">Type of the target.</param>
		/// <returns></returns>
		public static object Cast(this object ob, Type targetType)
		{
			Contract.Requires(!ob.IsNull());
			Contract.Requires(!targetType.IsNull());
			Contract.Ensures(!Contract.Result<object>().IsNull());

			if(targetType.IsEnum)
			{
				var str = ob as string;
				return !str.IsNull() ? Enum.Parse(targetType, str) : Enum.ToObject(targetType, ob);
			}

			var currentType = ob.GetType();

			if(currentType.IsInteger() && targetType == typeof(bool))
				return ob.Equals(0.Cast(targetType)) ? false : true;

			var end = Convert.ChangeType(ob, targetType, CultureInfo.InvariantCulture);
			Contract.Assume(!end.IsNull());
			return end;
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
			return arr.Concatenate(SchumixBase.Space.ToString());
		}

		public static string SplitToString(this string[] split, char c)
		{
			string ss = string.Empty;

			for(int x = 0; x < split.Length; x++)
				ss += c + split[x];

			if(ss.Length > 0 && ss.Substring(0, c.ToString().Length) == c.ToString())
				ss = ss.Remove(0, c.ToString().Length);

			return ss;
		}

		public static string SplitToString(this string[] split, int min, char c)
		{
			string ss = string.Empty;

			for(int x = min; x < split.Length; x++)
				ss += c + split[x];

			if(ss.Length > 0 && ss.Substring(0, c.ToString().Length) == c.ToString())
				ss = ss.Remove(0, c.ToString().Length);

			return ss;
		}

		public static string SplitToString(this string[] split)
		{
			string ss = string.Empty;

			foreach(var s in split)
				ss += s;

			return ss;
		}

		public static string SplitToString(this string[] split, string s)
		{
			string ss = string.Empty;

			for(int x = 0; x < split.Length; x++)
				ss += s + split[x];

			if(ss.Length > 0 && ss.Substring(0, s.Length) == s)
				ss = ss.Remove(0, s.Length);

			return ss;
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

		public static string SplitToString(this char[] split, char c)
		{
			string ss = string.Empty;

			for(int x = 0; x < split.Length; x++)
				ss += c + split[x];

			if(ss.Length > 0 && ss.Substring(0, c.ToString().Length) == c.ToString())
				ss = ss.Remove(0, c.ToString().Length);

			return ss;
		}

		public static string SplitToString(this char[] split, int min, char c)
		{
			string ss = string.Empty;

			for(int x = min; x < split.Length; x++)
				ss += c + split[x];

			if(ss.Length > 0 && ss.Substring(0, c.ToString().Length) == c.ToString())
				ss = ss.Remove(0, c.ToString().Length);

			return ss;
		}

		public static string SplitToString(this char[] split)
		{
			string ss = string.Empty;

			foreach(var s in split)
				ss += s;

			return ss;
		}

		public static string SplitToString(this char[] split, string s)
		{
			string ss = string.Empty;

			for(int x = 0; x < split.Length; x++)
				ss += s + split[x];

			if(ss.Length > 0 && ss.Substring(0, s.Length) == s)
				ss = ss.Remove(0, s.Length);

			return ss;
		}

		public static string SplitToString(this char[] split, int min, string s)
		{
			string ss = string.Empty;

			for(int x = min; x < split.Length; x++)
				ss += s + split[x];

			if(ss.Length > 0 && ss.Substring(0, s.Length) == s)
				ss = ss.Remove(0, s.Length);

			return ss;
		}

		public static string Reverse(this string value)
		{
			return value.Reverse().ToArray().SplitToString();
		}

		public static string Remove(this string s, int min, int max, char value)
		{
			return (s.Length >= max && s.Substring(min, max) == value.ToString()) ? s.Remove(min, max) : s;
		}

		public static string Remove(this string s, int min, int max, string value)
		{
			return (s.Length >= max && s.Substring(min, max) == value) ? s.Remove(min, max) : s;
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

		public static bool CompareDataInBlock(this string[] split)
		{
			int i = 0;
			string ss = string.Empty;

			foreach(var s in split)
			{
				if(i == 0)
					ss = s;
				else
				{
					if(ss != s)
						return false;
				}

				i++;
			}

			return true;
		}

		public static bool CompareDataInBlock<T>(this List<T> list)
		{
			int i = 0;
			string ss = string.Empty;

			foreach(var s in list)
			{
				if(i == 0)
					ss = s.ToString();
				else
				{
					if(ss != s.ToString())
						return false;
				}

				i++;
			}

			return true;
		}

		public static bool Contains(this string Text, string Name, char Parameter)
		{
			var s = Text.Split(Parameter);

			foreach(var ss in s)
			{
				if(ss.ToLower() == Name.ToLower())
					return true;
			}

			return false;
		}

		public static bool IsNumber(this string Text)
		{
			double number;
			return double.TryParse(Text, out number);
		}

		public static double ToNumber(this string Text)
		{
			double number;
			return double.TryParse(Text, out number) ? number : 0;
		}

		public static double ToNumber(this string Text, int Else)
		{
			double number;
			return double.TryParse(Text, out number) ? number : Else;
		}

		public static int ToInt(this double Double)
		{
			return Convert.ToInt32(Double);
		}

		public static string ToIrcOpcode(this int number)
		{
			if(number < 10)
				return "00" + number.ToString();
			else if(number < 100)
				return "0" + number.ToString();
			else
				return number.ToString();
		}

		public static string ToString(this IDictionary<YamlNode, YamlNode> Nodes, string FileName = "")
		{
			var text = new StringBuilder();

			foreach(var child in Nodes)
			{
				if(((YamlMappingNode)child.Value).GetType() == typeof(YamlMappingNode))
					text.Append(child.Key).Append(":\n").Append(child.Value);
				else
					text.Append("    ").Append(child.Key).Append(": ").Append(child.Value);
			}

			if(sUtilities.GetPlatformType() == PlatformType.Windows)
			{
				text = text.Replace("\r", string.Empty);
				var split = text.ToString().Split('\n');
				text.Remove(0, text.Length);

				foreach(var line in split)
				{
					if(line.Trim() == string.Empty)
						continue;

					text.Append("    ").AppendLine(line);
				}
			}

			return FileName == string.Empty ? "# Schumix config file (yaml)\n" + text.ToString() : "# " + FileName + " config file (yaml)\n" + text.ToString();
		}

		public static bool ContainsKey(this IDictionary<YamlNode, YamlNode> Nodes, string Key)
		{
			return Nodes.ContainsKey(new YamlScalarNode(Key));
		}

		public static YamlScalarNode ToYamlNode(this string Text)
		{
			return new YamlScalarNode(Text);
		}

		public static string TrimMessage(this string value, int number = 150)
		{
			return value.Length > number ? value.Substring(0, number) + " ..." : value;
		}
	}
}