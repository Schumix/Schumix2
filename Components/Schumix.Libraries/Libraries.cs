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
using System.IO;
using System.Linq;
#if !MONO
using System.Management;
#endif
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Collections.Generic;
using Schumix.Framework;

namespace Schumix.Libraries
{
	public static class Lib
	{
		private static readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private static readonly DateTime UnixTimeStart = new DateTime(1970, 1, 1, 0, 0, 0);
		private const int TicksPerSecond = 10000;
		private const long TicksSince1970 = 621355968000000000; // .NET ticks for 1970
		private static readonly object WriteLock = new object();

		public static bool IsPrime(long x)
		{
			return sUtilities.IsPrime(x);
		}

		public static string Regex(this string text, string regex)
		{
			var x = new Regex(regex);

			if(x.IsMatch(text))
			{
				string s = string.Empty;

				for(int a = 1; a < x.Match(text).Length; a++)
					s += " " + x.Match(text).Groups[a].ToString();

				return s.Remove(0, 1);
			}
			else
				return "Regex error!";
		}

		public static string Regex(this string text, string regex, string groups)
		{
			var x = new Regex(regex);
			return x.IsMatch(text) ? x.Match(text).Groups[groups].ToString() : "Regex error!";
		}

		/// <summary>
		/// Casts the object to the specified type.
		/// </summary>
		/// <typeparam name="T">The type to cast to.</typeparam>
		/// <param name="ob">Object to cast</param>
		/// <returns>The casted object.</returns>
		public static T Cast<T>(this object ob)
		{
			if(ob == null)
				throw new ArgumentNullException("ob");

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
			if(obj == null)
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
			if(obj == null)
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
			if(coll == null)
				throw new ArgumentNullException("coll");

			Task.WaitAll(coll.ToArray());
		}

		/// <summary>
		///   Gets the cpu brand string.
		/// </summary>
		/// <returns>
		///   The CPU brand string.
		/// </returns>
		public static string GetCpuId()
		{
			return sUtilities.GetCpuId();
        }

		/// <summary>
		///   The current unix time.
		/// </summary>
		public static double UnixTime
		{
			get
			{
				var elapsed = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
				return (elapsed.TotalSeconds);
			}
		}

		/// <summary>
		/// Converts DateTime to miliseconds.
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		public static int ToMilliSecondsInt(this DateTime time)
		{
			return (int)(time.Ticks/TicksPerSecond);
		}

		/// <summary>
		/// Converts TimeSpan to miliseconds.
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		public static int ToMilliSecondsInt(this TimeSpan time)
		{
			return (int)(time.Ticks)/TicksPerSecond;
		}

		/// <summary>
		/// Converts ticks to miliseconds.
		/// </summary>
		/// <param name="ticks"></param>
		/// <returns></returns>
		public static int ToMilliSecondsInt(int ticks)
		{
			return ticks/TicksPerSecond;
		}

		/// <summary>
		///   Gets the system uptime.
		/// </summary>
		/// <returns>the system uptime in milliseconds</returns>
		public static long GetSystemTime()
		{
			return (long)Environment.TickCount;
		}

		/// <summary>
		///   Gets the time since the Unix epoch.
		/// </summary>
		/// <returns>the time since the unix epoch in seconds</returns>
		public static long GetEpochTime()
		{
			return (long)((DateTime.UtcNow.Ticks - TicksSince1970)/TimeSpan.TicksPerSecond);
		}

		/// <summary>
		/// Gets the date time from unix time.
		/// </summary>
		/// <param name="unixTime">The unix time.</param>
		/// <returns></returns>
		public static DateTime GetDateTimeFromUnixTime(long unixTime)
		{
			return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(unixTime);
		}

		/// <summary>
		/// Gets the UTC time from seconds.
		/// </summary>
		/// <param name="seconds">The seconds.</param>
		/// <returns></returns>
		public static DateTime GetUTCTimeSeconds(long seconds)
		{
			return UnixTimeStart.AddSeconds(seconds);
		}

		/// <summary>
		/// Gets the UTC time from millis.
		/// </summary>
		/// <param name="millis">The millis.</param>
		/// <returns></returns>
		public static DateTime GetUTCTimeMillis(long millis)
		{
			return UnixTimeStart.AddMilliseconds(millis);
		}

		/// <summary>
		///   Gets the system uptime.
		/// </summary>
		/// <remarks>
		///   Even though this returns a long, the original value is a 32-bit integer,
		///   so it will wrap back to 0 after approximately 49 and half days of system uptime.
		/// </remarks>
		/// <returns>the system uptime in milliseconds</returns>
		public static long GetSystemTimeLong()
		{
			return (long)Environment.TickCount;
		}

		/// <summary>
		///   Gets the time between the Unix epich and a specific <see cref = "DateTime">time</see>.
		/// </summary>
		/// <returns>the time between the unix epoch and the supplied <see cref = "DateTime">time</see> in seconds</returns>
		public static long GetEpochTimeFromDT()
		{
			return GetEpochTimeFromDT(DateTime.Now);
		}

		/// <summary>
		///   Gets the time between the Unix epich and a specific <see cref = "DateTime">time</see>.
		/// </summary>
		/// <param name = "time">the end time</param>
		/// <returns>the time between the unix epoch and the supplied <see cref = "DateTime">time</see> in seconds</returns>
		public static long GetEpochTimeFromDT(DateTime time)
		{
			return (long)((time.Ticks - TicksSince1970)/10000000L);
		}

		public static string GetVersion()
		{
			return sUtilities.GetVersion();
		}

		public static string GetNetVersion()
		{
			return Environment.Version.ToString();
		}

		public static void print(string text)
		{
			lock(WriteLock)
			{
				Console.Write(text);
			}
		}

		public static void print(object o)
		{
			lock(WriteLock)
			{
				Console.Write(o);
			}
		}

		public static void print(string text, params object[] args)
		{
			lock(WriteLock)
			{
				print(string.Format(text, args));
			}
		}

		public static void printf(string text)
		{
			lock(WriteLock)
			{
				Console.Write(text);
			}
		}

		public static void printf(object o)
		{
			lock(WriteLock)
			{
				Console.Write(o);
			}
		}

		public static void printf(string text, params object[] args)
		{
			lock(WriteLock)
			{
				printf(string.Format(text, args));
			}
		}
	}
}