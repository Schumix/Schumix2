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
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography;
using Schumix.Framework;
using Schumix.Framework.Util;
using Schumix.Framework.Logger;
using Schumix.Framework.Platforms;
using Schumix.Framework.Extensions;

namespace Schumix.Libraries
{
	public static class Lib
	{
		private static readonly DateTime UnixTimeStart = new DateTime(1970, 1, 1, 0, 0, 0);
		private static readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private static readonly Platform sPlatform = Singleton<Platform>.Instance;
		private static readonly object WriteLock = new object();
		private const long TicksSince1970 = 621355968000000000; // .NET ticks for 1970
		private const int TicksPerSecond = 10000;

		public static bool IsPrime(this int x)
		{
			return sUtilities.IsPrime((long)x);
		}

		public static bool IsPrime(this long x)
		{
			return sUtilities.IsPrime(x);
		}

		public static string Regex(this string text, string regex)
		{
			try
			{
				var x = new Regex(regex);

				if(x.IsMatch(text))
				{
					string s = string.Empty;

					for(int a = 1; a < x.Match(text).Length; a++)
					{
						if(!x.Match(text).Groups[a].ToString().IsNullOrEmpty())
							s += SchumixBase.Space + x.Match(text).Groups[a].ToString();
					}

					return s.Remove(0, 1, SchumixBase.Space);
				}
				else
					return "No Match!";
			}
			catch(Exception e)
			{
				return e.Message;
			}
		}

		public static string Regex(this string text, string regex, string groups)
		{
			try
			{
				var x = new Regex(regex);
				return x.IsMatch(text) ? x.Match(text).Groups[groups].ToString() : "No Match!";
			}
			catch(Exception e)
			{
				return e.Message;
			}
		}

		/// <summary>
		///   Gets the cpu brand string.
		/// </summary>
		/// <returns>
		///   The CPU brand string.
		/// </returns>
		[Obsolete]
		public static string GetCpuId() // TODO: Majd törölni kell mert Sandbox-xal nem használható.
		{
			return sUtilities.GetCpuId();
		}

		public static DateTime GetUnixTimeStart()
		{
			return UnixTimeStart;
		}

		/// <summary>
		///   The current unix time.
		/// </summary>
		public static double UnixTime
		{
			get
			{
				var elapsed = (DateTime.UtcNow - UnixTimeStart);
				return elapsed.TotalSeconds;
			}
		}

		/// <summary>
		/// Converts DateTime to miliseconds.
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		public static int ToMilliSecondsInt(this DateTime time)
		{
			try
			{
				return (int)(time.Ticks/TicksPerSecond);
			}
			catch(Exception)
			{
				return 0;
			}
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
			try
			{
				return UnixTimeStart.AddSeconds(unixTime);
			}
			catch(Exception)
			{
				return new DateTime(0);
			}
		}

		/// <summary>
		/// Gets the UTC time from seconds.
		/// </summary>
		/// <param name="seconds">The seconds.</param>
		/// <returns></returns>
		public static DateTime GetUTCTimeSeconds(long seconds)
		{
			try
			{
				return UnixTimeStart.AddSeconds(seconds);
			}
			catch(Exception)
			{
				return new DateTime(0);
			}
		}

		/// <summary>
		/// Gets the UTC time from millis.
		/// </summary>
		/// <param name="millis">The millis.</param>
		/// <returns></returns>
		public static DateTime GetUTCTimeMillis(long millis)
		{
			try
			{
				return UnixTimeStart.AddMilliseconds(millis);
			}
			catch(Exception)
			{
				return new DateTime(0);
			}
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
				Log.Write(text);
			}
		}

		public static void print(object o)
		{
			lock(WriteLock)
			{
				Log.Write(o.ToString());
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
				Log.Write(text);
			}
		}

		public static void printf(object o)
		{
			lock(WriteLock)
			{
				Log.Write(o.ToString());
			}
		}

		public static void printf(string text, params object[] args)
		{
			lock(WriteLock)
			{
				printf(Tools.sprintf(text, args));
			}
		}

		public static string GetPlatform()
		{
			return sPlatform.GetPlatform();
		}

		/// <summary>
		/// Returns the name of the operating system running on this computer.
		/// </summary>
		/// <returns>A string containing the the operating system name.</returns>
		public static string GetOSName()
		{
			return sPlatform.GetOSName();
		}

		public static bool IsDay(int Year, int Month, int Day)
		{
			return sUtilities.IsDay(Year, Month, Day);
		}
	}
}