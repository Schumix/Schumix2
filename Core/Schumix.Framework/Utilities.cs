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
using System.IO;
using System.Net;
using System.Web;
using System.Linq;
using System.Threading;
using System.Reflection;
using System.Management;
using System.Diagnostics;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Schumix.Api.Functions;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Framework
{
	public sealed class Utilities
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly DateTime UnixTimeStart = new DateTime(1970, 1, 1, 0, 0, 0);
		private const long TicksSince1970 = 621355968000000000; // .NET ticks for 1970
		private readonly object WriteLock = new object();
		private const int TicksPerSecond = 10000;
		private Utilities() {}

		public DateTime GetUnixTimeStart()
		{
			return UnixTimeStart;
		}

		public string GetUrl(string url)
		{
			lock(WriteLock)
			{
				return GetUrl(url, string.Empty, string.Empty);
			}
		}

		public string GetUrl(string url, string args)
		{
			lock(WriteLock)
			{
				return GetUrl(url, args, string.Empty);
			}
		}

		public string GetUrl(string url, string args, string noencode)
		{
			lock(WriteLock)
			{
				if(!args.IsEmpty() && noencode.IsEmpty())
					url = url + HttpUtility.UrlEncode(args);
				else if(!args.IsEmpty() && !noencode.IsEmpty())
					url = url + HttpUtility.UrlEncode(args) + noencode;

				var request = (HttpWebRequest)WebRequest.Create(url);
				request.AllowAutoRedirect = true;
				request.UserAgent = Consts.SchumixUserAgent;
				request.Referer = Consts.SchumixReferer;

				int length = 0;
				byte[] buf = new byte[1024];
				var sb = new StringBuilder();
				var response = (HttpWebResponse)request.GetResponse();
				var stream = response.GetResponseStream();

				while((length = stream.Read(buf, 0, buf.Length)) != 0)
				{
					buf = Encoding.Convert(Encoding.GetEncoding(response.CharacterSet), Encoding.UTF8, buf);
					sb.Append(Encoding.UTF8.GetString(buf, 0, length));
				}

				response.Close();
				return sb.ToString();
			}
		}

		public string GetUrlEncoding(string url, string encoding)
		{
			lock(WriteLock)
			{
				return GetUrlEncoding(url, string.Empty, string.Empty, encoding);
			}
		}

		public string GetUrlEncoding(string url, string args, string encoding)
		{
			lock(WriteLock)
			{
				return GetUrlEncoding(url, args, string.Empty, encoding);
			}
		}

		public string GetUrlEncoding(string url, string args, string noencode, string encoding)
		{
			lock(WriteLock)
			{
				if(!args.IsEmpty() && noencode.IsEmpty())
					url = url + HttpUtility.UrlEncode(args);
				else if(!args.IsEmpty() && !noencode.IsEmpty())
					url = url + HttpUtility.UrlEncode(args) + noencode;

				using(var client = new WebClient())
				{
					double Num;
					bool isNum = double.TryParse(encoding, out Num);

					if(!isNum)
						client.Encoding = Encoding.GetEncoding(encoding);
					else
						client.Encoding = Encoding.GetEncoding(Convert.ToInt32(Num));

					client.Headers.Add("referer", Consts.SchumixReferer);
					client.Headers.Add("user-agent", Consts.SchumixUserAgent);
					return client.DownloadString(new Uri(url));
				}
			}
		}

		public void DownloadFile(string url, string filename)
		{
			using(var client = new WebClient())
			{
				client.Headers.Add("referer", Consts.SchumixReferer);
				client.Headers.Add("user-agent", Consts.SchumixUserAgent);
				client.DownloadFile(url, filename);
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
				var urlFind = new Regex("(?<url>(http[s]?://)"					// http[s]
				+ "?(([0-9a-z_!~*'().&=+$%-]+: )?[0-9a-z_!~*'().&=+$%-]+@)?"	// user@
				+ @"(([0-9]{1,3}\.){3}[0-9]{1,3}"								// IP- 199.194.52.184
				+ "|"															// allows either IP or domain
				+ @"([0-9a-z_!~*'()-]+\.)*"										// tertiary domain(s)- www.
				+ @"([0-9a-z][0-9a-z-]{0,61})?[0-9a-z]\."						// second level domain
				+ "[a-z]{2,6})"													// first level domain- .com or .museum
				+ "(:[0-9]{1,8})?"												// port number- :80
				+ "(( )|(/ )|"													// a slash isn't required if there is no file name
				+ "(/[0-9a-z_!~*'().;?:@&=+$,%#-]+)+/?)?)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

				if(urlFind.IsMatch(text))
				{
					var matches = urlFind.Matches(text);
					urlFind = new Regex(@"(?<ip>([0-9]{1,3}\.){3}[0-9]{1,3})", RegexOptions.Compiled | RegexOptions.IgnoreCase);

					foreach(var url in from Match match in matches select match.Groups["url"].ToString())
					{
						string lurl = url;

						if(urlFind.IsMatch(lurl))
						{
							bool valid = false;
							string sip = urlFind.Match(lurl).Groups["ip"].ToString();

							try
							{
								IPAddress ip;
								valid = IPAddress.TryParse(sip, out ip);
							}
							catch(ArgumentException ae)
							{
								Log.Error("Utilities", sLConsole.GetString("Failure details: {0}"), ae.Message);
							}

							if(!valid)
								continue;
						}

						if(!lurl.StartsWith("http://") && !url.StartsWith("https://"))
							lurl = string.Format("http://{0}", url);

						Log.Debug("Utilities", sLConsole.GetString("Checking: {0}"), url);
						urls.Add(lurl);
					}
				}
			}
			catch(Exception e)
			{
				Log.Error("Utilities", sLConsole.GetString("Failure details: {0}"), e.Message);
			}

			return urls;
		}

		public string GetRandomString()
		{
			string path = Path.GetRandomFileName();
			return path.Replace(SchumixBase.Point.ToString(), string.Empty);
		}

		public string Sha1(string value)
		{
			if(value.IsNull())
				throw new ArgumentNullException("value");

			var x = new SHA1CryptoServiceProvider();
			var data = Encoding.UTF8.GetBytes(value);
			data = x.ComputeHash(data);
//#if !MONO
			//x.Dispose();
//#endif
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
			var data = Encoding.UTF8.GetBytes(value);
			data = x.ComputeHash(data);
//#if !MONO
			//x.Dispose();
//#endif
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
//#if !MONO
				//md5.Dispose();
//#endif
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
			string platform = string.Empty;
			var pid = Environment.OSVersion.Platform;

			switch(pid)
			{
				case PlatformID.Win32NT:
				case PlatformID.Win32S:
				case PlatformID.Win32Windows:
				case PlatformID.WinCE:
					platform = "Windows";
					break;
				case PlatformID.Unix:
					platform = "Linux";
					break;
				case PlatformID.MacOSX:
					platform = "MacOSX";
					break;
				case PlatformID.Xbox:
					platform = "Xbox";
					break;
				default:
					platform = "Unknown";
					break;
			}

			return platform;
		}

		/// <summary>
		/// Returns the name of the operating system running on this computer.
		/// </summary>
		/// <returns>A string containing the the operating system name.</returns>
		public string GetOSName()
		{
			var Info = Environment.OSVersion;
			string Name = string.Empty;
			
			switch(Info.Platform)
			{
				case PlatformID.Win32Windows:
				{
					switch(Info.Version.Minor)
					{
						case 0:
						{
							Name = "Windows 95";
							break;
						}
						case 10:
						{
							if(Info.Version.Revision.ToString() == "2222A")
								Name = "Windows 98 Second Edition";
							else
								Name = "Windows 98";

							break;
						}
						case 90:
						{
							Name = "Windows Me";
							break;
						}
					}

					break;
				}
				case PlatformID.Win32NT:
				{
					switch(Info.Version.Major)
					{
						case 3:
						{
							Name = "Windows NT 3.51";
							break;
						}
						case 4:
						{
							Name = "Windows NT 4.0";
							break;
						}
						case 5:
						{
							if(Info.Version.Minor == 0)
								Name = "Windows 2000";
							else if(Info.Version.Minor == 1)
								Name = "Windows XP";
							else if(Info.Version.Minor == 2)
								Name = "Windows Server 2003";
							break;
						}
						case 6:
						{
							if(Info.Version.Minor == 0)
								Name = "Windows Vista";
							else if(Info.Version.Minor == 1)
								Name = "Windows 7";
							break;
						}
					}

					break;
				}
				case PlatformID.WinCE:
				{
					Name = "Windows CE";
					break;
				}
				case PlatformID.Unix:
				{
					Name = "Linux " + Info.Version;
					break;
				}
				case PlatformID.MacOSX:
				{
					Name = "MacOSX";
					break;
				}
				case PlatformID.Xbox:
				{
					Name = "Xbox";
					break;
				}
				default:
				{
					Name = "Unknown";
					break;
				}
			}

			return Name;
		}

		public PlatformType GetPlatformType()
		{
			PlatformType platform = PlatformType.None;
			var pid = Environment.OSVersion.Platform;

			switch(pid)
			{
				case PlatformID.Win32NT:
				case PlatformID.Win32S:
				case PlatformID.Win32Windows:
				case PlatformID.WinCE:
					platform = PlatformType.Windows;
					break;
				case PlatformID.Unix:
					platform = PlatformType.Linux;
					break;
				case PlatformID.MacOSX:
					platform = PlatformType.MacOSX;
					break;
				case PlatformID.Xbox:
					platform = PlatformType.Xbox;
					break;
				default:
					platform = PlatformType.None;
					break;
			}

			return platform;
		}

		public string GetVersion()
		{
			return Schumix.Framework.Config.Consts.SchumixVersion;
		}

		public string GetFunctionUpdate()
		{
			string functions = string.Empty;

			foreach(var function in Enum.GetNames(typeof(IChannelFunctions)))
			{
				if(function == IChannelFunctions.Log.ToString() || function == IChannelFunctions.Rejoin.ToString() ||
					function == IChannelFunctions.Commands.ToString())
					functions += SchumixBase.Comma + function.ToString().ToLower() + SchumixBase.Colon + SchumixBase.On;
				else
					functions += SchumixBase.Comma + function.ToString().ToLower() + SchumixBase.Colon + SchumixBase.Off;
			}

			return functions;
		}

		public string SqlEscape(string text)
		{
			if(text.IsNull() || text.IsEmpty())
				return string.Empty;

			if(SQLiteConfig.Enabled)
			{
				var split = text.Split('\'');
				if(split.Length > 1)
				{
					int x = 0;
					var sb = new StringBuilder();

					foreach(var s in split)
					{
						x++;

						if(s.Length == 0 && x != split.Length)
							sb.Append(@"''");
						else if(s.Length == 1)
						{
							if(s.Substring(0, 1) != @"'")
							{
								sb.Append(s);
								sb.Append(@"''");
							}
							else
								sb.Append(@"' ''");
						}
						else
						{
							int i = 0;
							string ss = s;

							for(;;)
							{
								if(ss.Length > 0 && ss.Substring(ss.Length-1) != @"'")
								{
									if(ss.Length-1 > 0)
										sb.Append(ss.Substring(0, ss.Length));

									for(int a = 0; a < i; a++)
										sb.Append(@"'");

									if(x != split.Length && i % 2 == 0)
										sb.Append(@"''");
									else if(x != split.Length)
										sb.Append(@" ''");

									break;
								}
								else if(ss.Length <= 0)
								{
									for(int a = 0; a < i; a++)
										sb.Append(@"'");

									if(x != split.Length && i % 2 == 0)
										sb.Append(@"''");
									else if(x != split.Length)
										sb.Append(@" ''");

									break;
								}

								i++;
								ss = ss.Remove(ss.Length-1);
							}
						}
					}

					text = sb.ToString();
				}
			}
			else
			{
				var split = text.Split('\'');
				if(split.Length > 1)
				{
					int x = 0;
					var sb = new StringBuilder();

					foreach(var s in split)
					{
						x++;

						if(s.Length == 0 && x != split.Length)
							sb.Append(@"\'");
						else if(s.Length == 1)
						{
							if(s.Substring(0, 1) != @"\")
							{
								sb.Append(s);
								sb.Append(@"\'");
							}
							else
								sb.Append(@"\ \'");
						}
						else
						{
							int i = 0;
							string ss = s;

							for(;;)
							{
								if(ss.Length > 0 && ss.Substring(ss.Length-1) != @"\")
								{
									if(ss.Length-1 > 0)
										sb.Append(ss.Substring(0, ss.Length));

									for(int a = 0; a < i; a++)
										sb.Append(@"\");

									if(x != split.Length && i % 2 == 0)
										sb.Append(@"\'");
									else if(x != split.Length)
										sb.Append(@" \'");

									break;
								}
								else if(ss.Length <= 0)
								{
									for(int a = 0; a < i; a++)
										sb.Append(@"\");

									if(x != split.Length && i % 2 == 0)
										sb.Append(@"\'");
									else if(x != split.Length)
										sb.Append(@" \'");

									break;
								}

								i++;
								ss = ss.Remove(ss.Length-1);
							}
						}
					}

					text = sb.ToString();
				}
			}

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
			if(GetPlatformType() == PlatformType.Windows)
			{
				var mos = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
				return (from ManagementObject mo in mos.Get() select (Regex.Replace(Convert.ToString(mo["Name"]), @"\s+", SchumixBase.Space.ToString()))).FirstOrDefault();
			}
			else if(GetPlatformType() == PlatformType.Linux)
			{
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
						return sLConsole.Other("Notfound");

					var amatch = amdRegex.Match(content);
					string amd = amatch.Groups["cpu"].ToString();
					return amd;
				}

				var match = getBrandRegex.Match(content);
				string cpu = (match.Groups["first"].ToString() + SchumixBase.Space + match.Groups["second"].ToString());
				return cpu;
			}

			return sLConsole.Other("Notfound");
		}

		/// <summary>
		///   The current unix time.
		/// </summary>
		public double UnixTime
		{
			get
			{
				var elapsed = (DateTime.UtcNow - UnixTimeStart);
				return elapsed.TotalSeconds;
			}
		}

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
			return UnixTimeStart.AddSeconds(unixTime);
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
				new FileStream(Name, FileMode.Append, FileAccess.Write, FileShare.Write).Close();
		}

		public string NameDay(string Language)
		{
			string[,] Nameday = null;

			switch(Language)
			{
				case "huHU":
				{
					Nameday = new string[,] {
						{ "ÚJÉV","Ábel","Genovéva","Titusz","Simon","Boldizsár","Attila","Gyöngyvér","Marcell","Melánia","Ágota","Ernő","Veronika","Bódog","Lóránt","Gusztáv","Antal","Piroska","Sára","Sebestyén","Ágnes","Vince","Zelma","Timót","Pál","Vanda","Angelika","Károly","Adél","Martina","Marcella" },
						{ "Ignác","Karolina","Balázs","Ráhel","Ágota","Dóra","Tódor","Aranka","Abigél","Elvira","Bertold","Lívia","Ella, Linda","Bálint","Kolos","Julianna","Donát","Bernadett","Zsuzsanna","Álmos","Eleonóra","Gerzson","Alfréd","Mátyás","Géza","Edina","Ákos, Bátor","Elemér","","","" },
						{ "Albin","Lujza","Kornélia","Kázmér","Adorján","Leonóra","Tamás","Zoltán","Franciska","Ildikó","Szilárd","Gergely","Krisztián, Ajtony","Matild","Kristóf","Henrietta","Gertrúd","Sándor","József","Klaudia","Benedek","Beáta","Emőke","Gábor","Irén","Emánuel","Hajnalka","Gedeon","Auguszta","Zalán","Árpád" },
						{ "Hugó","Áron","Buda, Richárd","Izidor","Vince","Vilmos, Bíborka","Herman","Dénes","Erhard","Zsolt","Zsolt, Leó","Gyula","Ida","Tibor","Tas, Anasztázia","Csongor","Rudolf","Andrea","Emma","Konrád, Tivadar","Konrád","Csilla","Béla","György","Márk","Ervin","Zita","Valéria","Péter","Katalin, Kitti","" },
						{ "Fülöp","Zsigmond","Tímea","Mónika","Györgyi","Ivett","Gizella","Mihály","Gergely","Ármin","Ferenc","Pongrác","Szervác","Bonifác","Zsófia","Botond, Mózes","Paszkál","Erik","Ivó, Milán","Bernát, Felícia","Konstantin","Júlia, Rita","Dezső","Eszter","Orbán","Fülöp","Hella","Emil, Csanád","Magdolna","Zsanett, Janka","Angéla" },
						{ "Tünde","Anita, Kármen","Klotild","Bulcsú","Fatime","Norbert","Róbert","Medárd","Félix","Margit","Barnabás","Villő","Antal, Anett","Vazul","Jolán","Jusztin","Laura","Levente","Gyárfás","Rafael","Alajos","Paulina","Zoltán","Iván","Vilmos","János","László","Levente, Irén","Péter, Pál","Pál","" },
						{ "Annamária","Ottó","Kornél","Ulrik","Sarolta, Emese","Csaba","Apolónia","Ellák","Lukrécia","Amália","Nóra, Lili","Izabella","Jenő","Örs","Henrik","Valter","Endre, Elek","Frigyes","Emília","Illés","Dániel","Magdolna","Lenke","Kinga, Kincső","Kristóf, Jakab","Anna, Anikó","Olga","Szabolcs","Márta","Judit","Oszkár" },
						{ "Boglárka","Lehel","Hermina","Domonkos","Krisztina","Berta","Ibolya","László","Emőd","Lörinc","Zsuzsanna","Klára","Ipoly","Marcell","Mária","Ábrahám","Jácint","Ilona","Huba","István","Sámuel","Menyhért","Bence","Bertalan","Lajos","Izsó","Gáspár","Ágoston","Beatrix","Rózsa","Erika" },
						{ "Egon","Rebeka","Hilda","Rozália","Viktor, Lőrinc","Zakariás","Regina","Mária","Ádám","Nikolett, Hunor","Teodóra","Mária","Kornél","Szeréna","Enikő","Edit","Zsófia","Diána","Vilhelmina","Friderika","Máté","Móric","Tekla","Gellért","Eufrozina","Jusztina","Adalbert","Vencel","Mihály","Jeromos","" },
						{ "Malvin","Petra","Helga","Ferenc","Aurél","Renáta","Amália","Koppány","Dénes","Gedeon","Brigitta","Miksa","Kálmán","Helén","Teréz","Gál","Hedvig","Lukács","Nándor","Vendel","Orsolya","Előd","Gyöngyi","Salamon","Bianka","Dömötör","Szabina","Simon","Nárcisz","Alfonz","Farkas" },
						{ "Marianna","Achilles","Győző","Károly","Imre","Lénárd","Rezső","Zsombor","Tivadar","Réka","Márton","Jónás, Renátó","Szilvia","Aliz","Albert, Lipót","Ödön","Hortenzia, Gergő","Jenő","Erzsébet","Jolán","Olivér","Cecília","Kelemen","Emma","Katalin","Virág","Virgil","Stefánia","Taksony","András, Andor","" },
						{ "Elza","Melinda","Ferenc","Barbara, Borbála","Vilma","Miklós","Ambrus","Mária","Natália","Judit","Árpád","Gabriella","Luca","Szilárda","Valér","Etelka","Lázár","Auguszta","Viola","Teofil","Tamás","Zéno","Viktória","Ádám, Éva","KARÁCSONY","KARÁCSONY","János","Kamilla","Tamás","Dávid","Szilveszter" },
					};
					break;
				}
				//case "enUS":
				//{
				//	Nameday = null;
				//	break;
				//}
				default:
				{
					Nameday = new string[,] {
						{ "ÚJÉV","Ábel","Genovéva","Titusz","Simon","Boldizsár","Attila","Gyöngyvér","Marcell","Melánia","Ágota","Ernő","Veronika","Bódog","Lóránt","Gusztáv","Antal","Piroska","Sára","Sebestyén","Ágnes","Vince","Zelma","Timót","Pál","Vanda","Angelika","Károly","Adél","Martina","Marcella" },
						{ "Ignác","Karolina","Balázs","Ráhel","Ágota","Dóra","Tódor","Aranka","Abigél","Elvira","Bertold","Lívia","Ella, Linda","Bálint","Kolos","Julianna","Donát","Bernadett","Zsuzsanna","Álmos","Eleonóra","Gerzson","Alfréd","Mátyás","Géza","Edina","Ákos, Bátor","Elemér","","","" },
						{ "Albin","Lujza","Kornélia","Kázmér","Adorján","Leonóra","Tamás","Zoltán","Franciska","Ildikó","Szilárd","Gergely","Krisztián, Ajtony","Matild","Kristóf","Henrietta","Gertrúd","Sándor","József","Klaudia","Benedek","Beáta","Emőke","Gábor","Irén","Emánuel","Hajnalka","Gedeon","Auguszta","Zalán","Árpád" },
						{ "Hugó","Áron","Buda, Richárd","Izidor","Vince","Vilmos, Bíborka","Herman","Dénes","Erhard","Zsolt","Zsolt, Leó","Gyula","Ida","Tibor","Tas, Anasztázia","Csongor","Rudolf","Andrea","Emma","Konrád, Tivadar","Konrád","Csilla","Béla","György","Márk","Ervin","Zita","Valéria","Péter","Katalin, Kitti","" },
						{ "Fülöp","Zsigmond","Tímea","Mónika","Györgyi","Ivett","Gizella","Mihály","Gergely","Ármin","Ferenc","Pongrác","Szervác","Bonifác","Zsófia","Botond, Mózes","Paszkál","Erik","Ivó, Milán","Bernát, Felícia","Konstantin","Júlia, Rita","Dezső","Eszter","Orbán","Fülöp","Hella","Emil, Csanád","Magdolna","Zsanett, Janka","Angéla" },
						{ "Tünde","Anita, Kármen","Klotild","Bulcsú","Fatime","Norbert","Róbert","Medárd","Félix","Margit","Barnabás","Villő","Antal, Anett","Vazul","Jolán","Jusztin","Laura","Levente","Gyárfás","Rafael","Alajos","Paulina","Zoltán","Iván","Vilmos","János","László","Levente, Irén","Péter, Pál","Pál","" },
						{ "Annamária","Ottó","Kornél","Ulrik","Sarolta, Emese","Csaba","Apolónia","Ellák","Lukrécia","Amália","Nóra, Lili","Izabella","Jenő","Örs","Henrik","Valter","Endre, Elek","Frigyes","Emília","Illés","Dániel","Magdolna","Lenke","Kinga, Kincső","Kristóf, Jakab","Anna, Anikó","Olga","Szabolcs","Márta","Judit","Oszkár" },
						{ "Boglárka","Lehel","Hermina","Domonkos","Krisztina","Berta","Ibolya","László","Emőd","Lörinc","Zsuzsanna","Klára","Ipoly","Marcell","Mária","Ábrahám","Jácint","Ilona","Huba","István","Sámuel","Menyhért","Bence","Bertalan","Lajos","Izsó","Gáspár","Ágoston","Beatrix","Rózsa","Erika" },
						{ "Egon","Rebeka","Hilda","Rozália","Viktor, Lőrinc","Zakariás","Regina","Mária","Ádám","Nikolett, Hunor","Teodóra","Mária","Kornél","Szeréna","Enikő","Edit","Zsófia","Diána","Vilhelmina","Friderika","Máté","Móric","Tekla","Gellért","Eufrozina","Jusztina","Adalbert","Vencel","Mihály","Jeromos","" },
						{ "Malvin","Petra","Helga","Ferenc","Aurél","Renáta","Amália","Koppány","Dénes","Gedeon","Brigitta","Miksa","Kálmán","Helén","Teréz","Gál","Hedvig","Lukács","Nándor","Vendel","Orsolya","Előd","Gyöngyi","Salamon","Bianka","Dömötör","Szabina","Simon","Nárcisz","Alfonz","Farkas" },
						{ "Marianna","Achilles","Győző","Károly","Imre","Lénárd","Rezső","Zsombor","Tivadar","Réka","Márton","Jónás, Renátó","Szilvia","Aliz","Albert, Lipót","Ödön","Hortenzia, Gergő","Jenő","Erzsébet","Jolán","Olivér","Cecília","Kelemen","Emma","Katalin","Virág","Virgil","Stefánia","Taksony","András, Andor","" },
						{ "Elza","Melinda","Ferenc","Barbara, Borbála","Vilma","Miklós","Ambrus","Mária","Natália","Judit","Árpád","Gabriella","Luca","Szilárda","Valér","Etelka","Lázár","Auguszta","Viola","Teofil","Tamás","Zéno","Viktória","Ádám, Éva","KARÁCSONY","KARÁCSONY","János","Kamilla","Tamás","Dávid","Szilveszter" },
					};
					break;
				}
			}

			if(Nameday.IsNull())
				return string.Empty;

			return Nameday[DateTime.Now.Month-1, DateTime.Now.Day-1];
		}

		public bool IsDay(int Year, int Month, int Day)
		{
			if(DateTime.IsLeapYear(Year))
			{
				switch(Month)
				{
					case 1:
					case 3:
					case 5:
					case 7:
					case 8:
					case 10:
					case 12:
						return Day <= 31;
					case 4:
					case 6:
					case 9:
					case 11:
						return Day <= 30;
					case 2:
						return Day <= 29;
				}
			}
			else
			{
				switch(Month)
				{
					case 1:
					case 3:
					case 5:
					case 7:
					case 8:
					case 10:
					case 12:
						return Day <= 31;
					case 4:
					case 6:
					case 9:
					case 11:
						return Day <= 30;
					case 2:
						return Day <= 28;
				}
			}

			return false;
		}

		public string DownloadString(Uri url, int maxlength)
		{
			lock(WriteLock)
			{
				return DownloadString(url.ToString(), maxlength);
			}
		}

		public string DownloadString(string url, int maxlength)
		{
			lock(WriteLock)
			{
				try
				{
					var request = (HttpWebRequest)WebRequest.Create(url);
					new Thread(() =>
					{
						Thread.Sleep(13*1000);

						if(!request.IsNull())
							request.Abort();
					});

					request.AllowAutoRedirect = true;
					request.UserAgent = Consts.SchumixUserAgent;
					request.Referer = Consts.SchumixReferer;
					request.Timeout = 10*1000;
					request.ReadWriteTimeout = 10*1000;

					int length = 0;
					byte[] buf = new byte[1024];
					var sb = new StringBuilder();
					var response = (HttpWebResponse)request.GetResponse();
					var stream = response.GetResponseStream();

					while((length = stream.Read(buf, 0, buf.Length)) != 0)
					{
						if(sb.Length >= maxlength)
							break;

						buf = Encoding.Convert(Encoding.GetEncoding(response.CharacterSet), Encoding.UTF8, buf);
						sb.Append(Encoding.UTF8.GetString(buf, 0, length));
					}

					response.Close();
					return sb.ToString();
				}
				catch(Exception e)
				{
					Log.Debug("Utilities", sLConsole.GetString("Failure details: {0}"), "(DownloadString) " + e.Message);
					return string.Empty;
				}
			}
		}

		public string DownloadString(Uri url, string Contains, int maxlength = 0)
		{
			lock(WriteLock)
			{
				return DownloadString(url.ToString(), 0, Contains, null, maxlength);
			}
		}

		public string DownloadString(string url, string Contains, int maxlength = 0)
		{
			lock(WriteLock)
			{
				return DownloadString(url, 0, Contains, null, maxlength);
			}
		}

		public string DownloadString(Uri url, int timeout, string Contains, int maxlength = 0)
		{
			lock(WriteLock)
			{
				return DownloadString(url.ToString(), timeout, Contains, null, maxlength);
			}
		}

		public string DownloadString(string url, int timeout, string Contains, int maxlength = 0)
		{
			lock(WriteLock)
			{
				return DownloadString(url, timeout, Contains, null, maxlength);
			}
		}

		public string DownloadString(Uri url, string Contains, NetworkCredential credential, int maxlength = 0)
		{
			lock(WriteLock)
			{
				return DownloadString(url.ToString(), 0, Contains, credential, maxlength);
			}
		}

		public string DownloadString(string url, string Contains, NetworkCredential credential, int maxlength = 0)
		{
			lock(WriteLock)
			{
				return DownloadString(url, 0, Contains, credential, maxlength);
			}
		}

		public string DownloadString(Uri url, int timeout, string Contains, NetworkCredential credential, int maxlength = 0)
		{
			lock(WriteLock)
			{
				return DownloadString(url.ToString(), timeout, Contains, credential, maxlength);
			}
		}

		public string DownloadString(string url, int timeout, string Contains, NetworkCredential credential, int maxlength = 0)
		{
			lock(WriteLock)
			{
				try
				{
					var request = (HttpWebRequest)WebRequest.Create(url);
					new Thread(() =>
					{
						if(timeout != 0)
							Thread.Sleep(timeout+3);
						else
							Thread.Sleep(13*1000);

						if(!request.IsNull())
							request.Abort();
					});

					if(timeout != 0)
					{
						request.Timeout = timeout;
						request.ReadWriteTimeout = timeout;
					}
					else
					{
						request.Timeout = 10*1000;
						request.ReadWriteTimeout = 10*1000;
					}

					request.AllowAutoRedirect = true;
					request.UserAgent = Consts.SchumixUserAgent;
					request.Referer = Consts.SchumixReferer;

					if(!credential.IsNull())
						request.Credentials = credential;

					int length = 0;
					byte[] buf = new byte[1024];
					var sb = new StringBuilder();
					var response = (HttpWebResponse)request.GetResponse();
					var stream = response.GetResponseStream();

					if(maxlength == 0)
						maxlength = 10000;

					while((length = stream.Read(buf, 0, buf.Length)) != 0)
					{
						if(sb.ToString().Contains(Contains) || sb.Length >= 10000)
							break;

						buf = Encoding.Convert(Encoding.GetEncoding(response.CharacterSet), Encoding.UTF8, buf);
						sb.Append(Encoding.UTF8.GetString(buf, 0, length));
					}

					response.Close();
					return sb.ToString();
				}
				catch(Exception e)
				{
					Log.Debug("Utilities", sLConsole.GetString("Failure details: {0}"), "(DownloadString) " + e.Message);
					return string.Empty;
				}
			}
		}

		public string DownloadString(Uri url, Regex regex, int maxlength = 0)
		{
			lock(WriteLock)
			{
				return DownloadString(url.ToString(), 0, regex, maxlength);
			}
		}

		public string DownloadString(string url, Regex regex, int maxlength = 0)
		{
			lock(WriteLock)
			{
				return DownloadString(url, 0, regex, maxlength);
			}
		}

		public string DownloadString(Uri url, int timeout, Regex regex, int maxlength = 0)
		{
			lock(WriteLock)
			{
				return DownloadString(url.ToString(), timeout, regex, maxlength);
			}
		}

		public string DownloadString(string url, int timeout, Regex regex, int maxlength = 0)
		{
			lock(WriteLock)
			{
				try
				{
					var request = (HttpWebRequest)WebRequest.Create(url);
					new Thread(() =>
					{
						if(timeout != 0)
							Thread.Sleep(timeout+3);
						else
							Thread.Sleep(13*1000);

						if(!request.IsNull())
							request.Abort();
					});

					if(timeout != 0)
					{
						request.Timeout = timeout;
						request.ReadWriteTimeout = timeout;
					}
					else
					{
						request.Timeout = 10*1000;
						request.ReadWriteTimeout = 10*1000;
					}

					request.AllowAutoRedirect = true;
					request.UserAgent = Consts.SchumixUserAgent;
					request.Referer = Consts.SchumixReferer;

					int length = 0;
					byte[] buf = new byte[1024];
					var sb = new StringBuilder();
					var response = (HttpWebResponse)request.GetResponse();
					var stream = response.GetResponseStream();

					if(maxlength == 0)
						maxlength = 10000;

					while((length = stream.Read(buf, 0, buf.Length)) != 0)
					{
						if(regex.Match(sb.ToString()).Success || sb.Length >= maxlength)
							break;

						buf = Encoding.Convert(Encoding.GetEncoding(response.CharacterSet), Encoding.UTF8, buf);
						sb.Append(Encoding.UTF8.GetString(buf, 0, length));
					}

					response.Close();
					return sb.ToString();
				}
				catch(Exception e)
				{
					Log.Debug("Utilities", sLConsole.GetString("Failure details: {0}"), "(DownloadString) " + e.Message);
					return string.Empty;
				}
			}
		}

		public bool IsValueBiggerDateTimeNow(int Year, int Month, int Day, int Hour, int Minute)
		{
			var time = DateTime.Now;
			return (time.Year >= Year && time.Month >= Month && time.Day >= Day && time.Hour >= Hour && time.Minute >= Minute);
		}

		public string GetUserName()
		{
			return Environment.UserName;
		}

		public string GetSpecialDirectory(string data)
		{
			if(GetPlatformType() == PlatformType.Windows)
			{
				string text = data.ToLower();

				if(text.Length >= "$home".Length && text.Substring(0, "$home".Length) == "$home")
				{
					if(data.Contains("/") && data.Substring(data.IndexOf("/")).Length > 1 && data.Substring(data.IndexOf("/")).Substring(0, 1) == "/")
						return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + data.Substring(data.IndexOf("/")+1);
					else if(data.Contains(@"\") && data.Substring(data.IndexOf(@"\")).Length > 1 && data.Substring(data.IndexOf(@"\")).Substring(0, 1) == @"\")
						return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + data.Substring(data.IndexOf(@"\")+1);
					else
						return data;
				}
				else if(text.Length >= "$localappdata".Length && text.Substring(0, "$localappdata".Length) == "$localappdata")
				{
					if(data.Contains("/") && data.Substring(data.IndexOf("/")).Length > 1 && data.Substring(data.IndexOf("/")).Substring(0, 1) == "/")
						return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\" + data.Substring(data.IndexOf("/")+1);
					else if(data.Contains(@"\") && data.Substring(data.IndexOf(@"\")).Length > 1 && data.Substring(data.IndexOf(@"\")).Substring(0, 1) == @"\")
						return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\" + data.Substring(data.IndexOf(@"\")+1);
					else
						return data;
				}
				else if(text.Length >= "$userappdata".Length && text.Substring(0, "$userappdata".Length) == "$userappdata")
				{
					if(data.Contains("/") && data.Substring(data.IndexOf("/")).Length > 1 && data.Substring(data.IndexOf("/")).Substring(0, 1) == "/")
						return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\" + data.Substring(data.IndexOf("/")+1);
					else if(data.Contains(@"\") && data.Substring(data.IndexOf(@"\")).Length > 1 && data.Substring(data.IndexOf(@"\")).Substring(0, 1) == @"\")
						return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\" + data.Substring(data.IndexOf(@"\")+1);
					else
						return data;
				}
				else
					return data;
			}
			else if(GetPlatformType() == PlatformType.Linux)
			{
				string text = data.ToLower();
				return (text.Length >= "$home".Length && text.Substring(0, "$home".Length) == "$home") ?
					"/home/" + GetUserName() + "/" + data.Substring(data.IndexOf("/")+1) : data;
			}
			else
				return data;
		}

		public bool IsSpecialDirectory(string dir)
		{
			dir = dir.ToLower();

			if(dir.Contains("$home"))
				return true;
			else if(GetPlatformType() == PlatformType.Windows && dir.Contains("$localappdata"))
				return true;
			else
				return false;
		}

		public string DirectoryToSpecial(string dir, string file)
		{
			if(GetPlatformType() == PlatformType.Windows)
			{
				if(dir.Length > 2 && dir.Substring(1, 2) == @":\")
					return string.Format(@"{0}\{1}", dir, file);
				else if(dir.Length > 2 && dir.Substring(0, 2) == "//")
					return string.Format("//{0}/{1}", dir, file);
				else
					return string.Format("./{0}/{1}", dir, file);
			}
			else if(GetPlatformType() == PlatformType.Linux)
				return (dir.Length > 0 && dir.Substring(0, 1) == "/") ? string.Format("{0}/{1}", dir, file) : string.Format("./{0}/{1}", dir, file);
			else
				return string.Format("{0}/{1}", dir, file);
		}

		public string GetDirectoryName(string data)
		{
			if(GetPlatformType() == PlatformType.Windows)
			{
				var split = data.Split('\\');
				return split.Length > 1 ? split[split.Length-1] : data;
			}
			else if(GetPlatformType() == PlatformType.Linux)
			{
				var split = data.Split('/');
				return split.Length > 1 ? split[split.Length-1] : data;
			}
			else
				return data;
		}

		public void CreatePidFile(string Name)
		{
			string pidfile = Name;

			if(!pidfile.Contains(".pid"))
			{
				if(pidfile.Contains(".xml"))
					pidfile = pidfile.Remove(pidfile.IndexOf(".xml")) + ".pid";
				else if(pidfile.Contains(".yml"))
					pidfile = pidfile.Remove(pidfile.IndexOf(".yml")) + ".pid";
				else
					pidfile = pidfile + ".pid";
			}

			pidfile = DirectoryToSpecial(LogConfig.LogDirectory, pidfile);
			SchumixBase.PidFile = pidfile;
			RemovePidFile();
			CreateFile(pidfile);
			var file = new StreamWriter(pidfile, true) { AutoFlush = true };
			file.WriteLine("{0}", Process.GetCurrentProcess().Id);
			file.Close();
		}

		public void RemovePidFile()
		{
			if(File.Exists(SchumixBase.PidFile))
				File.Delete(SchumixBase.PidFile);
		}

		public void CleanHomeDirectory(bool server = false)
		{
			if(File.Exists("Config.exe"))
				File.Delete("Config.exe");

			if(File.Exists("Installer.exe"))
				File.Delete("Installer.exe");

			if(File.Exists("xbuild.exe"))
				File.Delete("xbuild.exe");

			if(File.Exists("MSBuild.exe"))
				File.Delete("MSBuild.exe");

			if(server)
				return;

			if(File.Exists(AddonsConfig.Directory + "/Schumix.db3"))
				File.Delete(AddonsConfig.Directory + "/Schumix.db3");

			if(File.Exists(AddonsConfig.Directory + "/sqlite3.dll"))
				File.Delete(AddonsConfig.Directory + "/sqlite3.dll");

			if(File.Exists(AddonsConfig.Directory + "/System.Data.SQLite.dll"))
				File.Delete(AddonsConfig.Directory + "/System.Data.SQLite.dll");

			if(File.Exists(AddonsConfig.Directory + "/MySql.Data.dll"))
				File.Delete(AddonsConfig.Directory + "/MySql.Data.dll");

			if(File.Exists(AddonsConfig.Directory + "/Schumix.Irc.dll"))
				File.Delete(AddonsConfig.Directory + "/Schumix.Irc.dll");

			if(File.Exists(AddonsConfig.Directory + "/Schumix.Api.dll"))
				File.Delete(AddonsConfig.Directory + "/Schumix.Api.dll");

			if(File.Exists(AddonsConfig.Directory + "/Schumix.Framework.dll"))
				File.Delete(AddonsConfig.Directory + "/Schumix.Framework.dll");
		}
	}
}