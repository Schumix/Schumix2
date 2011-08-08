/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2011 Twl
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
using System.Net;
using System.Web;
using System.Linq;
using System.Reflection;
using System.Management;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Framework
{
	public sealed class Utilities
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly DateTime UnixTimeStart = new DateTime(1970, 1, 1, 0, 0, 0);
		private const int TicksPerSecond = 10000;
		private const long TicksSince1970 = 621355968000000000; // .NET ticks for 1970
		private Utilities() {}

		public string GetUrl(string url)
		{
			using(var client = new WebClient())
			{
				return client.DownloadString(url);
			}
		}

		public string GetUrl(string url, string args)
		{
			using(var client = new WebClient())
			{
				return client.DownloadString(new Uri(url + HttpUtility.UrlEncode(args)));
			}
		}

		public string GetUrl(string url, string args, string noencode)
		{
			using(var client = new WebClient())
			{
				return client.DownloadString(new Uri(url + HttpUtility.UrlEncode(args) + noencode));
			}
		}

		/// <summary>
		/// Gets the URLs in the specified text.
		/// </summary>
		/// <param name = "text">
		/// The text to search in.
		/// </param>
		/// <returns>
		/// The list of urls.
		/// </returns>
		public List<string> GetUrls(string text)
		{
			var urls = new List<string>();

			try
			{
				var urlFind = new Regex(@"(?<url>(http://)?(www\.)?\S+\.\S{2,6}([/]*\S+))", RegexOptions.Compiled | RegexOptions.IgnoreCase);

				if(urlFind.IsMatch(text))
				{
					var matches = urlFind.Matches(text);

					foreach(var url in from Match match in matches select match.Groups["url"].ToString())
					{
						var lurl = url;
						if(!lurl.StartsWith("http://") && !url.StartsWith("https://"))
							lurl = string.Format("http://{0}", url);

						Log.Debug("Utilities", sLConsole.Utilities("Text"), url);
						urls.Add(lurl);
					}
				}
			}
			catch(Exception e)
			{
				Log.Error("Utilities", sLConsole.Exception("Error"), e.Message);
			}

			return urls;
		}

		public string GetRandomString()
		{
			string path = Path.GetRandomFileName();
			return path.Replace(".", string.Empty);
		}

		public string Sha1(string value)
		{
			if(value.IsNull())
				throw new ArgumentNullException("value");

			var x = new SHA1CryptoServiceProvider();
			var data = Encoding.ASCII.GetBytes(value);
			data = x.ComputeHash(data);
#if !MONO
			x.Dispose();
#endif
			var ret = string.Empty;

			for(var i = 0; i < data.Length; i++)
				ret += data[i].ToString("x2").ToLower();

			return ret;
		}

		public string Md5(string value)
		{
			if(value.IsNull())
				throw new ArgumentNullException("value");

			var x = new MD5CryptoServiceProvider();
			var data = Encoding.ASCII.GetBytes(value);
			data = x.ComputeHash(data);
#if !MONO
			x.Dispose();
#endif
			var ret = string.Empty;

			for(var i = 0; i < data.Length; i++)
				ret += data[i].ToString("x2").ToLower();

			return ret;
		}

		public string MD5File(string fileName)
		{
			if(fileName.IsNull())
				throw new ArgumentNullException("fileName");

			byte[] retVal;

			using(var file = new FileStream(fileName, FileMode.Open))
			{
				var md5 = new MD5CryptoServiceProvider();
				retVal = md5.ComputeHash(file);
#if !MONO
				md5.Dispose();
#endif
			}

			var sb = new StringBuilder();

			if(!retVal.IsNull())
			{
				for(var i = 0; i < retVal.Length; i++)
					sb.Append(retVal[i].ToString("x2"));
			}

			return sb.ToString();
		}

		public bool IsPrime(long x)
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

		public string GetPlatform()
		{
			string Platform = string.Empty;
			var pid = Environment.OSVersion.Platform;

			switch(pid)
			{
				case PlatformID.Win32NT:
				case PlatformID.Win32S:
				case PlatformID.Win32Windows:
				case PlatformID.WinCE:
					Platform = "Windows";
					break;
				case PlatformID.Unix:
					Platform = "Linux";
					break;
				case PlatformID.MacOSX:
					Platform = "MacOSX";
					break;
				case PlatformID.Xbox:
					Platform = "Xbox";
					break;
				default:
					Platform = "Unknown";
					break;
			}

			return Platform;
		}

		public string GetVersion()
		{
			return Schumix.Framework.Config.Consts.SchumixVersion;
		}

		public string GetFunctionUpdate()
		{
			return ",koszones:off,log:on,rejoin:on,commands:on,autohl:off,autokick:off,automode:off,antiflood:off,message:off,compiler:off,gamecommands:off,webtitle:off,randomkick:off";
		}

		public string SqlEscape(string text)
		{
			if(text.IsNull() || text == string.Empty)
				return string.Empty;

			text = Regex.Replace(text, @"'", @"\'");
			text = Regex.Replace(text, @"\\'", @" \'");
			text = Regex.Replace(text, @"`", @"\`");
			text = Regex.Replace(text, @"\\`", @" \`");
			return text;
		}

		/// <summary>
		///   Gets the cpu brand string.
		/// </summary>
		/// <returns>
		///   The CPU brand string.
		/// </returns>
		public string GetCpuId()
		{
#if !MONO
			var mos = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
			return (from ManagementObject mo in mos.Get() select (Regex.Replace(Convert.ToString(mo["Name"]), @"\s+", " "))).FirstOrDefault();
#else
			var reader = new StreamReader("/proc/cpuinfo");
			string content = reader.ReadToEnd();
			reader.Close();
			reader.Dispose();
			var getBrandRegex = new Regex(@"model\sname\s:\s*(?<first>.+\sCPU)\s*(?<second>.+)", RegexOptions.IgnoreCase);

			if(!getBrandRegex.IsMatch(content))
			{
				// not intel
				var amdRegex = new Regex(@"model\sname\s:\s*(?<cpu>.+)");

				if(!amdRegex.IsMatch(content))
					return "Not found";

				var amatch = amdRegex.Match(content);
				string amd = amatch.Groups["cpu"].ToString();
				return amd;
			}

			var match = getBrandRegex.Match(content);
			string cpu = (match.Groups["first"].ToString() + SchumixBase.Space + match.Groups["second"].ToString());
			return cpu;
#endif
        }

		/// <summary>
		///   The current unix time.
		/// </summary>
		public double UnixTime
		{
			get
			{
				var elapsed = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
				return (elapsed.TotalSeconds);
			}
		}

#if MONO
		/// <summary>
		/// Converts DateTime to miliseconds.
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		public int ToMilliSecondsInt(this DateTime time)
		{
			return (int)(time.Ticks/TicksPerSecond);
		}

		/// <summary>
		/// Converts TimeSpan to miliseconds.
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		public int ToMilliSecondsInt(this TimeSpan time)
		{
			return (int)(time.Ticks)/TicksPerSecond;
		}
#endif

		/// <summary>
		/// Converts ticks to miliseconds.
		/// </summary>
		/// <param name="ticks"></param>
		/// <returns></returns>
		public int ToMilliSecondsInt(int ticks)
		{
			return ticks/TicksPerSecond;
		}

		/// <summary>
		///   Gets the system uptime.
		/// </summary>
		/// <returns>the system uptime in milliseconds</returns>
		public long GetSystemTime()
		{
			return (long)Environment.TickCount;
		}

		/// <summary>
		///   Gets the time since the Unix epoch.
		/// </summary>
		/// <returns>the time since the unix epoch in seconds</returns>
		public long GetEpochTime()
		{
			return (long)((DateTime.UtcNow.Ticks - TicksSince1970)/TimeSpan.TicksPerSecond);
		}

		/// <summary>
		/// Gets the date time from unix time.
		/// </summary>
		/// <param name="unixTime">The unix time.</param>
		/// <returns></returns>
		public DateTime GetDateTimeFromUnixTime(long unixTime)
		{
			return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(unixTime);
		}

		/// <summary>
		/// Gets the UTC time from seconds.
		/// </summary>
		/// <param name="seconds">The seconds.</param>
		/// <returns></returns>
		public DateTime GetUTCTimeSeconds(long seconds)
		{
			return UnixTimeStart.AddSeconds(seconds);
		}

		/// <summary>
		/// Gets the UTC time from millis.
		/// </summary>
		/// <param name="millis">The millis.</param>
		/// <returns></returns>
		public DateTime GetUTCTimeMillis(long millis)
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
		public long GetSystemTimeLong()
		{
			return (long)Environment.TickCount;
		}

		/// <summary>
		///   Gets the time between the Unix epich and a specific <see cref = "DateTime">time</see>.
		/// </summary>
		/// <returns>the time between the unix epoch and the supplied <see cref = "DateTime">time</see> in seconds</returns>
		public long GetEpochTimeFromDT()
		{
			return GetEpochTimeFromDT(DateTime.Now);
		}

		/// <summary>
		///   Gets the time between the Unix epich and a specific <see cref = "DateTime">time</see>.
		/// </summary>
		/// <param name = "time">the end time</param>
		/// <returns>the time between the unix epoch and the supplied <see cref = "DateTime">time</see> in seconds</returns>
		public long GetEpochTimeFromDT(DateTime time)
		{
			return (long)((time.Ticks - TicksSince1970)/10000000L);
		}

		public void CreateDirectory(string Name)
		{
			if(!Directory.Exists(Name))
				Directory.CreateDirectory(Name);
		}

		public void CreateFile(string Name)
		{
			if(!File.Exists(Name))
				File.Create(Name);
		}
	}
}