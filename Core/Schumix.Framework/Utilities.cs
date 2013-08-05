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
using System.Globalization;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using Schumix.Framework.Logger;
using Schumix.Framework.Config;
using Schumix.Framework.Platforms;
using Schumix.Framework.Functions;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Framework
{
	public sealed class Utilities
	{
		private readonly DateTimeFormatInfo dtfi = new DateTimeFormatInfo { ShortDatePattern = "yyyy-MM-dd HH:mm", DateSeparator = "-" };
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly DateTime UnixTimeStart = new DateTime(1970, 1, 1, 0, 0, 0);
		private readonly Platform sPlatform = Singleton<Platform>.Instance;
		private const long TicksSince1970 = 621355968000000000; // .NET ticks for 1970
		private readonly object WriteLock = new object();
		private const int TicksPerSecond = 10000;

		/*	Miscellaneous description --- */
		private const string DOMAIN = @"[a-z0-9][-a-z0-9]*(\.[-a-z0-9]+)*\.";
		private const string TLD = "[a-z][-a-z0-9]*[a-z]";
		private const string IPADDR = @"[0-9]+(\.[0-9]+){3}";
		private static string HOST = "(" + DOMAIN + TLD + "|" + IPADDR + ")";
		private const string OPT_PORT = "(:[1-9][0-9]{0,4})?";

		/*	URL description --- */
		private const string SCHEME = @"(www\.|http://|https://|)";
		private const string LPAR = @"\(";
		private const string RPAR = @"\)";
		private const string NOPARENS = @"[^() 	]*";
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
				if(!args.IsNullOrEmpty() && noencode.IsNullOrEmpty())
					url = url + HttpUtility.UrlEncode(args);
				else if(!args.IsNullOrEmpty() && !noencode.IsNullOrEmpty())
					url = url + HttpUtility.UrlEncode(args) + noencode;

				var request = (HttpWebRequest)WebRequest.Create(url);
				request.AllowAutoRedirect = true;
				request.UserAgent = Consts.SchumixUserAgent;
				request.Referer = Consts.SchumixReferer;

				int length = 0;
				byte[] buf = new byte[1024];
				var sb = new StringBuilder();

				using(var response = (HttpWebResponse)request.GetResponse())
				{
					using(var stream = response.GetResponseStream())
					{
						while((length = stream.Read(buf, 0, buf.Length)) != 0)
						{
							buf = Encoding.Convert(Encoding.GetEncoding(response.CharacterSet), Encoding.UTF8, buf);
							sb.Append(Encoding.UTF8.GetString(buf, 0, length));
						}
					}
				}

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
				if(!args.IsNullOrEmpty() && noencode.IsNullOrEmpty())
					url = url + HttpUtility.UrlEncode(args);
				else if(!args.IsNullOrEmpty() && !noencode.IsNullOrEmpty())
					url = url + HttpUtility.UrlEncode(args) + noencode;

				using(var client = new WebClient())
				{
					double Num;
					bool isNum = double.TryParse(encoding, out Num);

					if(!isNum)
						client.Encoding = Encoding.GetEncoding(encoding);
					else
						client.Encoding = Encoding.GetEncoding(Num.ToInt32());

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
				var urlFind = new Regex("(?<url>"
				+ "("													/* URL or HOST */
					+ "("
						+ SCHEME + HOST + OPT_PORT
						+ "("											/* Optional "/path?query_string#fragment_id" */
							+ "/"										/* Must start with slash */
							+ "("	
								+ "(" + LPAR + NOPARENS + RPAR + ")"
								+ "|"
								+ "(" + NOPARENS + ")"
							+ ")*"										/* Zero or more occurrences of either of these */
							+ @"(?<![.,?!\]])"							/* Not allowed to end with these */
						+ ")?"											/* Zero or one of this /path?query_string#fragment_id thing */
					+ ")|("
						+ HOST + OPT_PORT + "/"
						+ "("											/* Optional "path?query_string#fragment_id" */
							+ "("
								+ "(" + LPAR + NOPARENS + RPAR + ")"
								+ "|"
								+ "(" + NOPARENS + ")"
							+ ")*"										/* Zero or more occurrences of either of these */
							+ @"(?<![.,?!\]])"							/* Not allowed to end with these */
						+ ")?"											/* Zero or one of this /path?query_string#fragment_id thing */
					+ ")"
				+ @"))");

				if(urlFind.IsMatch(text))
				{
					var matches = urlFind.Matches(text);

					foreach(var url in from Match match in matches select match.Groups["url"].ToString())
					{
						string lurl = url;

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
#if false
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
			var data = Encoding.UTF8.GetBytes(value);
			data = x.ComputeHash(data);
#if false
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
#if false
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
			if(text.IsNull() || text.IsNullOrEmpty())
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
				return MySqlHelper.EscapeString(text);

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
			if(sPlatform.IsWindows)
			{
				var mos = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
				return (from ManagementObject mo in mos.Get() select (Regex.Replace(Convert.ToString(mo["Name"]), @"\s+", SchumixBase.Space.ToString()))).FirstOrDefault();
			}
			else if(sPlatform.IsLinux)
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
				case "czCZ":
				{
					Nameday = new string[,]
					{
						// januar
						{ "Nový rok","Karina","Radmila","Diana","Dalimil","Tři králové","Vilma","Čestmír","Vladan","Břetislav","Bohdana","Pravoslav","Edita","Radovan","Alice","Ctirad","Drahoslav","Vladislav","Doubravka","Ilona","Běla","Slavomír","Zdeněk","Milena","Miloš","Zora","Ingrid","Otýlie","Zdislava","Robin","Marika" },
						// Februar
						{ "Hynek","Nela","Blažej","Jarmila","Dobromila","Vanda","Veronika","Milada","Apolena","Mojmír","Božena","Slavěna","Věnceslav","Valentýn","Jiřina","Ljuba","Miloslava","Gizela","Patrik","Oldřich","Lenka","Petr","Svatopluk","Matěj","Liliana","Dorota","Alexandr","Lumír","---","","" },
						// Marec
						{ "Bedřich","Anežka","Kamil","Stela","Kazimir","Miroslav","Tomáš","Gabriela","Františka","Viktorie","Anděla","Řehoř","Růžena","Růt a Matylda","Ida","Elena a Herbert","Vlastimil","Eduard","Josef","Světlana","Radek","Leona","Ivona","Gabriel","Marian","Emanuel","Dita","Soňa","Taťána","Arnošt","Kvido" },
						// April
						{ "Hugo","Erika","Richard","Ivana","Miroslava","Vendula","Heřman a Hermína","Ema","Dušan","Darja","Izabela","Julius","Aleš","Vincenc","Anastázie","Irena","Rudolf","Valérie","Rostislav","Marcela","Alexandra","Evženie","Vojtěch","Jiří","Marek","Oto","Jaroslav","Vlastislav","Robert","Blahoslav","" },
						// Maj
						{ "Státní svátek","Zikmund","Alexej","Květoslav","Klaudie","Radoslav","Stanislav","Státní svátek","Ctibor","Blažena","Svatava","Pankrác","Servác","Bonifác","Žofie","Přemysl","Aneta","Nataša","Ivo","Zbyšek","Monika","Emil","Vladimír","Jana","Viola","Filip","Valdemar","Vilém","Maxmilián","Ferdinand","Kamila" },
						// červen
						{ "Laura","Jarmil","Tamara","Dalibor","Dobroslav","Norbert","Iveta a Slavoj","Medard","Stanislava","Gita","Bruno","Antonie","Antonín","Roland","Vít","Zbyněk","Adolf","Milan","Leoš","Květa","Alois","Pavla","Zdeňka","Jan","Ivan","Adriana","Ladislav","Lubomír","Petr a Pavel","Šárka","" },
						// červenec
						{ "Jaroslava","Patricie","Radomír","Prokop","Cyril a Metoděj","Mistr Jan Hus","Bohuslava","Nora","Drahoslava","Libuše a Amálie","Olga","Bořek","Markéta","Karolína","Jindřich","Luboš","Martina","Drahomíra","Čeněk","Ilja","Vítězslav","Magdaléna","Libor","Kristýna","Jakub","Anna","Věroslav","Viktor","Marta","Bořivoj","Ignác" },
						// srpen
						{ "Oskar","Gustav","Miluše","Dominik","Kristian","Oldřiška","Lada","Soběslav","Roman","Vavřinec","Zuzana","Klára","Alena","Alan","Hana","Jáchym","Petra","Helena","Ludvík","Bernard","Johana","Bohuslav","Sandra","Bartoloměj","Radim","Luděk","Otakar","Augustýn","Evelína","Vladěna","Pavlína" },
						// zari
						{ "Linda a Samuel","Adéla","Bronislav","Jindřiška","Boris","Boleslav","Regína","Mariana","Daniela","Irma","Denisa","Marie","Lubor","Radka","Jolana","Ludmila","Naděžda","Kryštof","Zita","Oleg","Matouš","Darina","Berta","Jaromír","Zlata","Andrea","Jonáš","Václav","Michal","Jeroným","" },
						// rijen
						{ "Igor","Olivie a Oliver","Bohumil","František","Eliška","Hanuš","Justýna","Věra","Štefan a Sára","Marina","Andrej","Marcel","Renáta","Agáta","Tereza","Havel","Hedvika","Lukáš","Michaela","Vendelín","Brigita","Sabina","Teodor","Nina","Beáta","Erik","Šarlota a Zoe","Státní svátek","Silvie","Tadeáš","Štěpánka" },
						// listopad
						{ "Felix","Dušičky","Hubert","Karel","Miriam","Liběna","Saskie","Bohumír","Bohdan","Evžen","Martin","Benedikt","Tibor","Sáva","Leopold","Otmar","Mahulena","Romana","Alžběta","Nikola","Albert","Cecílie","Klement","Emílie","Kateřina","Artur","Xenie","René","Zina","Ondřej","" },
						// prosinec
						{ "Iva","Blanka","Svatoslav","Barbora","Jitka","Mikuláš","Ambrož a Benjamín","Květoslava","Vratislav","Julie","Dana","Simona","Lucie","Lýdie","Radana a Radan","Albína","Daniel","Miloslav","Ester","Dagmar","Natálie","Šimon","Vlasta","Adam a Eva","2. vánoční svátek","Štěpán","Žaneta","Bohumila","Judita","David","Silvester" },
					};
					break;
				}
				case "deDE":
				{
					Nameday = new string[,]
					{
						// Januar
						{ "Neujahr, Maria","Makarius, Gregor, Otfried, Dietmar","Genoveva, Odilo, Irma","Angelika, Christiane","Emilia, Johann Nep.","Heilige 3 Könige, Raimund","Reinhold, Valentin","Severin, Erhard, Gudula, Heiko","Adrian, Julian, Alice","Paul Eins., Leonie","Thomas v.C.","Ernst, Tatjana, Xenia","Jutta, Hilmar, Hilarius","Rainer, Felix, Engelmar","Arnold, Romedius, Mauro, Arno","Marcel, Tilman, Dietwald, Uli","Anton Eins., Rosalind","Margitta, Ulfried, Uwe","Mario, Pia, Martha","Fabian, Sebastian, Ursula","Agnes, Meinrad, Ines","Vinzenz, Dietlinde, Jana","Hartmut, Emerentia, Guido","Franz v. S., Vera, Thurid, Bernd","Pauli Bekehrung., Wolfram","Timotheus u. Titus, Paula","Angela, Alrun, Gerd","Manfred, Thomas v. A., Karl, Karolina","Gerhard, Gerd, Josef Fr.","Martina, Adelgunde","Johannes B., Marcella, Rudbert" },
						// Februar
						{ "Brigitta, Brigitte, Reginald, Barbara","Mariä Lichtmess, Bodo, Stephan","Blasius, Ansgar, Oskar, Michael","Andreas C., Veronika, Jenny","Agatha, Albuin","Dorothea, Doris, Paul M.","Richard, Ava, Ronan","Elfrieda, Hieronymus. Philipp","Apollonia, Anne-Kathrin, Anna, Katharina","Scholastika, Siegmar, Bruno","Maria Lourdes, Theodora, Theodor","Benedikt, Eulalia","Christina, Irmhild, Adolf, Gisela","Valentin, Cyrill, Method","Siegfried, Jovita, Georgia","Juliana, Liane","Alexis, Benignus","Constanze, Simon, Simone","Irmgard, Irma, Hedwig","Corona, Falko, Jacinta","Petrus D., Gunhild, Enrica, Peter","Petri Stuhlfeier, Isabella, Pit","Romana, Raffaela, Polyk.","Matthias","Walburga, Edeltraud","Gerlinde, Ottokar, Edigna, Denis, Mechthild","Gabriel, Marko, Baldur","Roman, Silvana, Oswald, Detlev","Schalttag, Oswald","","" },
						// Marec
						{ "Albin, Roger, Leontina","Volker, Agnes, Karl","Kunigunde, Camilla, Leif, Friedrich","Kasimir, Edwin, Humbert","Gerda, Olivia, Dietmar, Tim","Fridolin, Nicola, Rosa, Nicole","Reinhard, Felicitas, Perpet., Volker","Johannes v.G., Gerhard","Franziska, Bruno, Barbara, Dominik","Emil, Gustav, 40 Märtyrer","Rosina, Alram, Ulrich","Beatrix, Almut, Serafina","Judith, Pauline, Leander","Mathilde, Eva, Evelyn","Klemens, Louise","Herbert, Rüdiger","Gertrud, Gertraud, Patrick","Edward, Sibylle, Cyrill","Josef, Josefa, Josefine","Claudia, Wolfram","Christian, Axel, Emilia","Lea, Elmar, Reinhilde","Otto, Rebekka, Toribio","Karin, Elias, Heidelinde","Verkündigung d. Herrn, Lucia","Ludger, Manuel, Manuela, Lara","Augusta, Heimo, Ernst","Guntram, Ingbert, Willy","Helmut, Ludolf, Berthold","Amadeus, Diemut","Cornelia, Conny, Nelly, Ben" },
						// April
						{ "Irene, Irina, Hugo","Franz v.P., Mirjam, Sandra, Frank","Richard, Lisa","Isidor, Konrad, Kurt","Crescentia, Vinzenz F., Juliane","Sixtus, William","Ralph, Johann Baptist","Walter, Beate, Rose-Marie","Waltraud, Casilda, Hugo","Gernot, Holda, Ezechiel, Engelbert","Stanislaus, Hildebrand, Reiner","Herta, Julius, Zeno","Ida, Hermenegild, Gilda, Martin","Ernestine, Erna, Elmo","Anastasia, Una, Damian","Bernadette, Magnus, Joachim","Eberhard, Wanda, Isadora, Max","Werner, Wigbert","Gerold, Emma, Leo, Timo","Odetta, Hildegund","Alexandra, Anselm","Alfred, Kaj, Leonidas","Georg, Jörg, Jürgen","Wilfried, Egbert, Virginia, Marion","Markus Ev., Erwin","Helene, Consuela","Zita, Petrus C, Montserrat","Hugo, Pierre, Ludwig","Katharina v.S., Roswitha, Katja","Pauline, Silvio, Pius V.","" },
						// Maj
						{ "Josef d. Arbeiter, Arnold","Siegmund, Boris, Zoë","Philipp u. Jakob, Viola, Alexander","Florian, Guido, Valeria","Gotthard, Sigrid, Jutta","Gundula, Antonia, Britto","Gisela, Silke, Notker, Helga","Ida, Ulrike, Ulla, Klara","Beat, Caroline, Volkmar, Theresia","Isidor, Gordian, Liliana, Damian de Veuster","Joachim, Mamertus","Pankratius, Imelda, Joana","Servatius, Rolanda","Bonifatius, Ismar, Pascal, Christian","Sophie, Sonja, Hertraud","Johann Nepomuk, Adolf","Dietmar, Pascal,Antonella","Erich, Erika, Johannes I., Felix","Ivo, Yvonne, Kuno","Bernhardin, Elfriede,Mira","Hermann, Wiltrud, Konst.","Julia, Rita, Ortwin, Renate","Renate, Désirée, Alma","Dagmar, Esther","Urban, Beda, Magdalene, Miriam","Marianne, Philipp N.","August, Bruno, Randolph","Wilhelm, German","Erwin, Irmtraud, Maximin","Ferdinand, Johanna","Petra, Mechthild, Helma" },	
						// Jun
						{ "Simeon, Silka, Silvana","Armin, Erasmus, Blandina","Karl, Silvia, Hildburg, Karoline","Christa, Klothilde, Iona, Eva","Winfried Bonifatius, Erika","Norbert, Bertrand, Kevin, Alice","Robert, Gottlieb, Anita","Medardus, Elga, Chlodwig","Grazia, Annamaria, Ephr., Diana","Diana, Heinrich, Heinz, Olivia","Paula, Barnabas, Alice, Udo","Guido, Leo III., Florinda","Antonius v.P., Bernhard","Hartwig, Meinrad","Veit, Lothar, Gebhard, Bernhard","Benno, Luitgard, Quirin, Julietta","Adolf, Volker, Alena","Elisabeth, Ilsa, Marina, Isabella"," Juliana, Romuald","Adalbert, Florentina, Margot","Alois, Aloisia, Alban, Ralf","Rotraud, Thomas M.","Edeltraud, Ortrud, Marion","Johannes d.T., Reingard","Eleonora, Ella, Dorothea, Doris","David, Konstantin, Vigil., Paul","Hemma, Heimo, Cyrill, Daniel","Harald, Ekkehard, Irenäus, Senta","Peter u. Paul, Gero","Otto, Bertram, Ehrentrud","" },
						// Jul
						{ "Dietrich, Aaron, Theobald, Regina","Mariä Heimsuchg, Wiltrud, Jakob","Thomas Ap., Ramon, Ramona","Ulrich, Berta, Elisabeth, Else","Albrecht, Kira, Letizia","Marietta G., Goar, Isaias","Willibald, Edda, Firmin","Kilian, Amalia, Edgar","Veronika, Hermine, Hannes","Knud, Engelbert, Raphael, Sascha","Olga, Oliver, Benedikt","Siegbert, Henriette, Felix, Eleonore","Heinrich, Sarah, Arno","Roland, Camillo, Goswin","Bonaventura, Egon, Björn","Carmen, Irmgard","Gabriella, Charlotte","Arnulf, Ulf, Friedrich","Marina, Reto, Bernold","Margaretha, Greta, Elias","Daniel, Daniela, Stella, Julia","Magdalena, Marlene, Verena","Birgitta, Birgit, Liborius","Christoph, Sieglinde, Luise","Jakob d.Ä., Valentina","Anna u. Joachim, Gloria","Rudolf, Rolf, Pantaleon, Natalie","Adele, Ada, Innozenz, Benno","Martha, Olaf, Ladislaus, Flora","Ingeborg, Inga, Petrus C.","Ignatius, Joseph v. Ar., Herrmann" },
						// August
						{ "Alfons, Kenneth, Peter F., Uwe","Eusebius, Adriana, Julian, Julan","Lydia, August, Nikodemus","Johannes M.V., Rainer, Reinhard","Oswald, Maria Schnee","Christi Verklärung, Gilbert","Cajetan, Afra, Albert","Dominik, Cyriak, Elgar","Edith, Altmann, Roman","Laurenz, Lars, Astrid","Klara, Philomena, Donald","Radegunde, Innozenz XI., Andreas","Hippolyt, Marko, Cassian","Meinhard, Maximilian K.","Mariä Himmelfahrt, Steven","Stefan, Rochus, Alfried, Stephanie","Gudrun, Hyazinth, Janine, Clara","Helena, Rainald, Claudia","Sebald, Johann E., Julius, Bert","Bernhard, Bernd, Ronald, Samuel","Pius X., Maximilian, Pia","Regina, Maria Regina, Sigfried","Rosa, Isolde, Zachäus","Bartholomäus, Michaela, Isolde","Ludwig, Elvira, Ebba, Patricia","Patricia, Miriam, Teresa, Margarita","Monika, Gebhard, Vivian","Augustin, Adelinde, Aline, Vivian","Johannes Enthauptung, Beatrice","Felix, Heribert, Rebekka, Alma","Raimund, Aidan, Paulinus, Anja" },
						// September
						{ "Verena, Ruth, Ägidius","Ingrid, René, Salomon, Franz","Gregor, Silvia, Phoebe, Sonja","Rosalie, Ida, Iris, Irmgard, Sven","Roswitha, Urs, Hermine","Magnus, Gundolf, Bertram, Beate","Regina, Otto, Ralph","Mariä Geburt, Adrian, Otmar","Otmar, Edgar, Pedro Cl.","Diethard, Isabella, Carlo, Niels","Helga, Felix u. Regula, Louis","Maria Namen, Gerfried","Notburga, Tobias, Johann.","Kreuzerhöhung, Albert, Jens","Dolores, Melitta, Melissa","Ludmilla, Cornelius","Hildegard, Robert, Ariane","Lambert, Herlinde, Rica","Wilhelmine, Januarius, Thorsten","Hertha, Eustach., Candida, Susanna","Matthäus, Deborah, Jonas","Mauritius, Emmeram, Gundula","Linus, Thekla, Gerhild","Rupert, Virgil, Gerhard","Klaus, Serge, Irmfried","Kosmas, Damian, Cosima","Vinzenz, Hiltrud, Dietrich","Wenzel, Lioba, Giselher","Michael, Michaela, Gabriel, Gabriela, Gabi","Hieronymus, Urs, Victor","" },
						// Oktober
						{ "Remigius, Theresia v.L., Werner, Andrea","Schutzengelfest, Gideon, Bianca, Jacqueline","Ewald, Udo, Bianca, Paulina","Franz v.A., Edwin, Aurora, Emma, Thea","Herwig, Meinolf, Gallina","Bruno, Adalbero, Melanie, Brunhild, Gerald","Rosa Maria, Justina, Jörg, Denise, Marc","Günther, Laura, Hannah, Gerda","Sibylle, Sara, Dionys, Elfriede","Viktor, Samuel, Gereon, Valerie","Alexander, Manuela, Georg","Maximilian, Horst, Pilár, David","Koloman, Edward, Andre","Burkhard, Calixtus, Alan, Otilie","Theresia v.A., Aurelia, Franziska","Hedwig, Gallus, Gordon, Carlo","Rudolf, Marie-Louise, Adelheid","Lukas, Gwenn, Justus, Viviana","Frieda, Frida, Isaak, Paul v. K.","Wendelin, Ira, Irina, Jessica","Ursula, Ulla, Celina, Holger","Cordula, Salome, Ingbert","Johannes C., Severin, Uta","Anton, Armella, Alois, Aloisia, Victoria","Ludwig, Lutz, Darja, Hans","Amand., Albin, Wieland, Anastacia, Josephine","Sabina, Wolfhard, Christa, Stefan","Simon u. J. Thaddäus, Freddy","Ermelinda, Melinda, Franco, Grete","Dieter, Alfons, Angelo, Sabine","Wolfgang, Quentin, Melanie" },
						// November
						{ "Allerheiligen, Harald","Allerseelen, Angela","Hubert, Pirmin, Martin P., Silvia","Karl, Karla, Modesta, Charles","Emmerich, Zacharias, Hardy","Leonhard, Christine, Nina","Engelbert, Carina, Willibr., Tina","Gottfried, Willehad, Karina","Theodor, Herfried, Roland, Gregor","Leo, Andrea, Andreas, Jens, Ted","Martin, Senta, Mennas, Leonie","Christian, Kunibert","Eugen, Stanislaus, Livia, Rene","Sidonia, Nikolaus T., Karl","Leopold, Leopoldine, Albert, Nikolaus","Margarita, Otmar, Arthur","Gertrud, Hilda, Florin, Walter","Odo, Alda, Roman, Bettina","Elisabeth, Bettina, Lisa, Roman","Edmund, Corbinian, Felix, Elisabetz","Amalie, Amelia, Rufus, Edmund","Cäcilia, Silja, Salvator, Rufus","Clemens, Detlef, Columb., Salvator","Flora, Albert, Chrysogon, Clemens","Katharina, Kathrin, Katja, Jasmin","Konrad, Kurt, Anneliese","Uta, Brunhilde, Albrecht, Ida","Berta, Jakob, Albrecht","Friedrich, Friederike, Berta","Andreas, Andrea, Volkert, Kerstin","" },
						// December
						{ "Blanka, Natalie, Eligius","Bibiana, Lucius, Jan","Franz Xaver, Jason","Barbara, Johannes v.D.","Gerald, Reinhard, Niels","Nikolaus, Denise, Henrike","Ambros, Farah, Benedikte","Mariä Empfängnis, Edith","Valerie, Liborius, Reinmar","Emma, Imma, Loretta","Arthur, Damasus, Tassilo","Johanna, Hartmann","Lucia, Ottilia, Jodok, Johanna","Berthold, Johannes v.K.","Christiane, Nina, Paola","Adelheid, Heidi, Elke","Lazarus, Jolanda, Viviana","Esperanza, Luise, Gratian","Susanna, Benjamin","Julius, Holger, Eike","Ingmar, Ingo, Hagar","Jutta, Francesca-Saveria","Victoria, Johannes C.","Hl. Abend, Adam u. Eva","Christfest (Weihnachten)","Stephan, Stephanie","Johannes Ev., Fabiola","Unschuldige Kinder, John","David, Tamara, Jessica","Hermine, Minna, Herma","Silvester, Melanie" },
					};
					break;
				}
				case "fiFI":
				{
					Nameday = new string[,]
					{
						// January
						{ "---","Aapeli","Elmer, Elmo","Ruut","Lea, Leea","Harri","Aku, Aukusti","Hilppa, Titta","Veijo, Veikko, Veli","Nyyrikki","Kari, Karri","Toini","Nuutti","Sakari, Saku","Solja","Ilmari, Ilmo","Anttoni, Toni","Laura","Heikki, Henrik","Henna, Henni","Aune, Oona","Visa","Eine, Eini, Enni","Senja","Paavo, Pauli","Joonatan","Viljo","Kaarlo, Kalle","Valtteri","Irja","Alli" },
						// February
						{ "Riitta","Aamu","Valo","Armi","Asser","Teija, Terhi, Tiia","Rikhard, Riku","Laina","Raija, Raisa","Elina","Talvikki","Elma","Sulo","Voitto","Sipi, Sippo","Kai","Väinö","Kaino","Eija","Heli, Helinä","Keijo","Tuuli, Tuulikki","Aslak","Matias, Matti","Tuija, Tuire","Nestori","Torsti","Onni","---","","" },
						// March
						{ "Alpo","Virva, Virve","Kauko","Ari","Laila, Leila","Tarmo","Tarja, Taru","Vilppu","Auvo","Aura, Auri, Aurora","Kalervo","Reijo, Reko","Erno, Tarvo","Matilda","Risto","Ilkka","Kerttu","Edvard, Eetu","Jooseppi, Juuso","Aki, Joakim, Kim","Pentti","Vihtori","Akseli","Gabriel, Kaapo","Aija","Immanuel, Manu","Sauli","Armas","Joni, Joonas, Jouni","Usko","Irma, Irmeli" },
						// April
						{ "Pulmu, Raita","Pellervo","Sampo","Ukko","Irene, Irina","Vilho, Ville","Ahvo, Allan","Suoma","Elias","Tero","Verna","Julia, Julius","Tellervo","Taito","Linda, Tuomi","Jalo, Patrik","Otto","Valdemar, Valto","Pälvi, Pilvi","Lauha","Anselmi, Anssi","Alina","Jyri, Jyrki, Yrjö","Pertti","Markku, Marko, Markus","Teresa, Terttu","Merja","Ilpo, Ilppo","Teijo","Miia, Mira, Mirja, Mirva","" },
						// May
						{ "Vappu","Viivi, Vuokko","Outi","Roosa, Ruusu","Maini","Ylermi","Helmi","Heino","Timo","Aina, Aini, Aino","Osmo","Lotta","Kukka","Tuula","Sofia, Sonja","Essi, Esteri","Maila","Eero, Erkki","Emilia, Emma","Karoliina, Lilja","Konsta, Kosti","Hemminki, Hemmo","Lyydia, Lyyli","Touko, Tuukka","Urpo","Minna, Vilhelmiina","Ritva","Alma","Oiva, Oivi","Pasi","Helga, Helka" },
						// June
						{ "Teemu","Venla","Orvokki","Toivo","Sulevi","Kustaa, Kyösti","Suvi","Salomo","Ensio","Seppo","Impi","Esko","Raila, Raili","Kielo","Viena, Vieno","Päivi, Päivikki","Urho","Tapio","Siiri","Into","Ahti, Ahto","Liina, Paula","Aatto, Aatu","Johannes, Juhani","Uuno","Jarmo, Jorma","Elvi, Elviira","Leo","Pekka, Petra, Petri, Pietari","Päiviö","" },
						// July
						{ "Aaro","Maija, Mari, Maria, Meeri","Arvo","Ulla, Ulpu","Unto","Esa","Klaus, Launo","Turkka, Turo","Ilta, Jasmin","Saima, Saimi","Elli, Noora","Herkko, Hermanni","Ilari, Joel, Lari","Aliisa","Rauna, Rauni","Reino","Ossi","Riikka","Saara, Salla, Salli, Sari","Maarit, Marketta, Reeta","Hanna, Johanna","Leena","Oili","Kirsi, Kirsti, Tiina","Jaakko","Martta","Heidi","Atso","Olavi, Olli","Asta","Elena, Helena" },
						// August
						{ "Maire","Kimmo","Linnea, Nea, Vanamo","Veera","Salme, Sanelma","Keimo, Toimi","Lahja","Sylvi","Eira, Erja","Lauri","Sanna, Susanna","Klaara","Jesse","Kanerva, Onerva","Jaana, Marja, Marjatta","Aulis","Verneri","Leevi","Mauno, Maunu","Sami, Samuli","Soini, Veini","Iivari, Iivo","Signe, Varma","Perttu","Loviisa","Ilma, Ilmi","Rauli","Tauno","Iina, Iines","Eemeli, Eemil","Arvi" },
						// September
						{ "Pirkka","Sini, Sinikka","Soile, Soili","Ansa","Mainio","Asko","Arho","Taimi","Eevert, Isto","Kalevi","Aleksanteri","Valma, Vilja","Orvo","Iida","Sirpa","Hellevi","Aila, Aili","Tytti, Tyyne","Reija","Varpu, Vaula","Mervi","Mauri","Mielikki","Alvar, Auno","Kullervo","Kuisma","Vesa","Arja","Mika, Mikael, Mikko","Sirja, Sorja","" },
						// October
						{ "Raine, Rainer, Rauno","Valio","Raimo","Saija, Saila","Inka, Inkeri","Minttu, Pinja","Pirjo, Pirkko","Hilja","Ilona","Aleksi","Ohto, Otso","Aarre, Aarto","Taija, Taina, Tanja","Elsa, Else, Elsi","Helvi, Heta","Sirkka","Saana, Saini","Säde, Satu","Uljas","Kasperi, Kauno","Ursula","Anita, Anja","everi","Asmo","Sointu","Amanda, Niina","Hellä, Helli","Simo","Alfred, Urmas","Eila","Arto, Artturi" },
						// November
						{ "Lyly, Pyry","Topi, Topias","Terho","Hertta","Reima","Aadolf, Kustaa","Taisto","Aatos","Teuvo","Martti","Panu","Virpi","Ano, Kristian","Iiris","Janika, Janita","Aarne, Aarno","Einari, Eino","Tenho","Elisabet, Liisa","Jalmari, Jari","Hilma","Selja, Silja","Ismo","Lempi","Kaija, Kaisa, Katri","Sisko","Hilkka","Heini","Aimo","Antero, Antti","" },
						// December
						{ "Oskari","Anelma, Unelma","Meri, Vellamo","Aira, Airi","Selma","Niilo, Niko","Sampsa","Kyllikki","Anna, Anne, Anni","Jutta","Taneli, Tatu","Tuovi","Seija","Jouko","Heimo","Auli, Aulikki","Raakel","Aapo, Aappo, Rami","Iikka, Iiro","Benjamin, Kerkko","Tuomas, Tuomo","Raafael","Senni","Aatami, Eeva","---","Tapani, Teppo","Hannes, Hannu","Piia","Rauha","Daavid, Taavetti","Sylvester" },
					};
					break;
				}
				case "frFR":
				{
					Nameday = new string[,]
					{
						// January
						{ "jour de l'An","Basile","Geneviève","Odilon","Edouard","Balthazar, Mélaine, Melchior, Tiffany","Aldric, Cédric, Raymond","Lucien","Alix","Guillaume","Hortense, Pauline","Tatiana","Hilaire, Yvette","Nina","Rachel, Rémi","Marcel","Roseline","Gwendal, Prisca","Marius","Fabien, Sébastien","Agnès","Vincent","Banard","François","---","Pauline, Timothé","Angèle","Manfred, Thomas","Gildas","Jacinthe, Martine","Marcelle" },
						// February
						{ "Ella, Siméon","Théophane","Blaise, Nelson, Oscar","Véronique","Agathe","Dorothée, Gaston","Eugénie","Jacqueline","Apolline","Arnaud","Lourdes","Félix","Béatrice","Valentin","Claude, Georgina, Jordan","Julienne, Lucile, Onésime","Alexis","Bernadette","Gabin","Aimée","Damien","Isabelle","Lazare","Modeste","Roméo","Nestor","Honorine, Léandre","Romain","Auguste","","" },
						// March
						{ "Albin, Aubin, Jonathan","Charles","Guénolé, Marin","Casimir","Olivia","Colette","Félicie, Nathan","Jean","Françoise","Vivien","Rosine","Justine, Pol","Rodrigue","Mathilde","Louise","Bénédicte","Patrice, Patrick","Cyrille","Joseph","Printemps","Axelle, Clémence","Léa","Rébecca, Victorien","Catherine, Karine","Humbert","Larissa","Habib","Gontran","Gladys","Amédée","Benjamin" },
						// April
						{ "Hugues, Valéry","Sandrine","Richard","Isidore","Irène","Marcellin","Clotaire, Jean-Baptiste","Julie","Gautier","Fulbert","Stanislas","Jules","Ida","Ludivine, Maxime","César","Rameaux","Anicet","Parfait","Emma","Odette, Théotime","Anselme","Alexandre","Georges","Fidèle","Marc","Alida","Zita","Valérie","Catherine","Robert","" },
						// May
						{ "Brieuc, Florine, Jérémie, Tamara","Boris, Zoé","Ewen, Jacques, Philippe","Florian, Sylvain","Judith","Marien, Prudence","Domitille, Gisèle","Désiré","Pacôme","Solange","Estelle, Mayeul","Achille","Maël, Orlane, Rolande","Aglaé, Matthias","Denise","Brendan, Honoré","Pascal","Eric","Célestin, Erwan, Yves","Bernardin","Constantin","Emile, Quitterie, Rita","Didier","Donatien","Sophie","Bérenger","Augustin","Germain","Aymar, Géraldine, Maximin","Ferdinand, Jeanne, Lorraine","Pétronille" },
						// June
						{ "Justin, Ronan","Blandine","Kévin","Clotilde","Igor","Norbert","Gilbert","Médard","Diane","Landry","Barnabé","Guy","Antoine","Elisée, Valère","Germaine","François-Régis, Régis","Hervé","Léonce","Gervais, Romuald","Silvère","Eté","Alban","Audrey","Jean-Baptiste","Aliénor, Eléonore, Prosper, Salomon","Anthelme","Fernand","Irénée","Paul, Pierre","Adolphe, Martial","" },
						// July
						{ "Aaron, Esther, Goulwen, Thierry","Martinien","Thomas","Florent","Antoine","Mariette, Nolwen","Raoul","Edgar, Killian, Priscillia, Thibault","Amandine, Hermine, Iphigénie, Marianne","Ulrich","Benoit, Olga, Yolande","Jason, Olivier","Enzo, Eugène, Henri, Joël","Camille","Donald, Vladimir","Elvire","Arlette, Charlotte, Marcelline","Frédéric","Arsène, Micheline","Elie, Marina","Rodolphe, Térence, Victor","Madeleine, Wandrille","Brigitte","Christine, Ségolène","Jacques, Valentine","Anne, Hannah, Joachin","Aurèle, Nathalie","Samson","Beatrix, Loup, Marthe","Juliette","Ignace" },
						// August
						{ "Alphonse","Julien","Lydie","Vianney","Abel","----","Gaétan","Dominique","Amour","Laurent","Claire, Gilberte, Suzanne","Clarisse","Hippolyte","Evrard","Alfred, Marie","Armel, Roch","Hyacinthe","Hélène, Laétitia","Jean","Bernard, Samuel","Christophe, Grâce, Ombeline","Fabrice","Rose","Barthélémy","Louis","Natacha","Monique","Augustin, Elouan","Médéric, Sabine","Fiacre","Aristide" },
						// September
						{ "Gilles, Jossué","Ingrid","Grégoire","Iris, Moïse, Rosalie","Raïssa","Bertrand, Eva","Reine","Adrien, Béline, Nativité","Alain, Omer","Inès","Adelphe, Glenn, Vinciane","Apollinaire","Aimé","----","Dolores, Roland","Edith","Hildegarde, Lambert, Renaud","Nadège, Véra","Emilie","Davy","Déborah, Jonas, Matthieu, Mélissa","Maurice","Faustine","Thècle","Hermann","Côme, Damien","Vincent","Venceslas","Gabriel, Michel, Raphaël","Jérôme","" },
						// October
						{ "Ariel, Mélodie, Muriel, Thérèse","Léger, Ruth","Gérard, Sybille","Aure, Bérénice, François, Frank, Orianne, Sarah","Camélia, Capucine, Daphne, Eglantin, Fleur, Placide","Bruno","Gustave, Serge","Pélagie, Thaïs","Denis","Ghislain, Virgile","Firmin","Edwin, Séraphin, Wilfried","Géraud","Céleste, Gwendoline, Juste","Thérèse","Edwige","Baudoin, Solène","Luc","Cléo, René","Adeline, Aline","Céline, Ursule","Elodie, Salomé, Sara","Jean, Simon","Florentin","Crépin","Dimitri","Emeline","Jude","Narcisse","Bienvenue, Maéva","Quentin" },
						// November
						{ "Toussaint","Défunts","Gwenaël, Hubert","Aymeric, Charles, Jessé","Sylvie, Zacharie","Bertille, Léonard","Carine","Dora, Geoffroy","Maturin, Théodore","Léon, Noé","Martin, Vérane","Christian","Brice","Sidoine","Albert, Arthur, Léopold, Malo, Victoire","Gertrude, Marguerite, Mégane","Elisabeth, Elise, Hilda","Aude","Tanguy","Edmond, Octave","------","Cécile","Clément","Flora","Catherine","Delphine","Séverin","Jacques","Saturnin","André, Tugdual","" },
						// December
						{ "Florence","Viviane","Xavier","Barbara","Gérald, Gérard","Nicolas","Ambroise","-----","Pierre","Eulaire, Romaric","Daniel","Chantal","Jocelyn, Lucie","Odile","Ninon","Alice","Adélaïde, Gaël, Judicaël, Olympe","Briac, Gatien","Urbain","Isaac, Jacob, Théophile","Pierre","Françoise-Xavière, Gratien","Armand","Adèle","Manuel","Etienne","Fabiola, Jean","Gaspard","David","Roger","Colombe, Sylvestre" },
					};
					break;
				}
				case "huHU":
				{
					Nameday = new string[,]
					{
						// Január
						{ "Fruzsina","Ábel","Genovéva, Benjámin","Titusz, Leona, Angel","Simon","Boldizsár","Attila, Ramóna","Gyöngyvér","Marcell","Melánia","Ágota","Ernő","Veronika","Bódog","Lóránt, Loránd","Gusztáv","Antal, Antónia","Piroska","Sára, Márió","Fábián, Sebestyén","Ágnes","Vince, Artúr","Zelma, Rajmund","Timót","Pál","Vanda, Paula","Angelika","Károly, Karola","Adél","Martina, Gerda","Marcella" },
						// Február
						{ "Ignác","Karolina, Aida","Balázs","Ráhel, Csenge","Ágota, Ingrid","Dorottya, Dóra","Tódor, Rómeó","Aranka","Abigél, Alex","Elvira","Bertold, Marietta","Lídia, Lívia","Ella, Linda","Bálint, Valentin","Kolos, Georgina","Julianna, Lilla","Donát","Bernadett","Zsuzsanna","Aladár, Álmos","Eleonóra","Gerzson","Alfréd","Mátyás","Géza","Edina","Ákos, Bátor","Elemér","---","","" },
						// Március
						{ "Albin","Lujza","Kornélia","Kázmér","Adorján, Adrián","Leonóra, Inez","Tamás","Zoltán","Franciska, Fanni","Ildikó","Szilárd","Gergely","Krisztián, Ajtony","Matild","Kristóf","Henrietta","Gertrúd, Patrik","Sándor, Ede","József, Bánk","Klaudia, Tubs, Jessica","Benedek","Beáta, Izolda, Lea","Emőke","Gábor, Karina","Irén, Írisz","Emánuel","Hajnalka","Gedeon, Johanna","Auguszta","Zalán","Árpád" },
						// Április
						{ "Hugó","Áron","Buda, Richárd","Izidor","Vince","Vilmos, Bíborka","Herman","Dénes","Erhard","Zsolt","Leó, Szaniszló","Gyula","Ida","Tibor","Anasztázia, Tas","Csongor","Rudolf","Andrea, Ilma","Emma","Tivadar","Konrád","Csilla, Noémi","Béla","György","Márk","Ervin","Zita","Valéria","Péter","Katalin, Kitti","" },
						// Május
						{ "Fülöp, Jakab, Zsaklin","Zsigmond","Tímea, Irma","Mónika, Flórián","Györgyi","Ivett, Frida","Gizella","Mihály","Gergely","Ármin, Pálma","Ferenc","Pongrác","Szervác, Imola","Bonifác","Zsófia, Szonja","Mózes, Botond","Paszkál","Erik, Alexandra","Ivó, Milán","Bernát, Felícia","Konstantin","Júlia, Rita","Dezső","Eszter, Eliza","Orbán","Fülöp, Evelin","Hella","Emil, Csanád","Magdolna","Janka, Zsanett","Angéla, Petronella" },
						// Június
						{ "Tünde","Kármen, Anita","Klotild, Cecília","Bulcsú","Fatime, Fatima","Norbert, Cintia","Róbert","Medárd","Félix","Margit, Gréta","Barnabás","Villő","Antal, Anett","Vazul","Jolán, Vid","Jusztin","Laura, Alida","Arnold, Levente","Gyárfás","Rafael","Alajos, Leila","Paulina","Zoltán","Iván","Vilmos","János, Pál","László","Levente, Irén","Péter, Pál","Pál","" },
						// Július
						{ "Tihamér, Annamária","Ottó","Kornél, Soma","Ulrik","Emese, Sarolta","Csaba","Apollónia","Ellák","Lukrécia","Amália","Nóra, Lili","Izabella, Dalma","Jenő","Örs, Stella","Örkény, Henrik, Roland","Valter","Endre, Elek","Frigyes","Emília","Illés","Dániel, Daniella","Magdolna","Lenke","Kinga, Kincső","Kristóf, Jakab","Anna, Anikó","Olga, Liliána","Szabolcs","Márta, Flóra","Judit, Xénia","Oszkár" },
						// Augusztus
						{ "Boglárka","Lehel","Hermina","Domonkos, Dominika","Krisztina","Berta, Bettina","Ibolya","László","Emőd","Lőrinc","Zsuzsanna, Tiborc","Klára","Ipoly","Marcell","Mária","Ábrahám","Jácint","Ilona","Huba","István","Sámuel, Hajna","Menyhért, Mirjam","Bence","Bertalan","Lajos, Patrícia","Izsó","Gáspár","Ágoston","Beatrix, Erna","Rózsa","Erika, Bella" },
						// Szeptember
						{ "Egyed, Egon","Rebeka, Dorina","Hilda","Rozália","Viktor, Lőrinc","Zakariás","Regina","Mária, Adrienn","Ádám","Nikolett, Hunor","Teodóra","Mária","Kornél","Szeréna, Roxána","Enikő, Melitta","Edit","Zsófia","Diána","Vilhelmina","Friderika","Máté, Mirella","Móric","Tekla, Líviusz","Gellért, Mercédesz","Eufrozina, Kende","Jusztina","Adalbert","Vencel","Mihály","Jeromos","" },
						// Oktober
						{ "Malvin","Petra","Helga","Ferenc","Aurél","Brúnó, Renáta","Amália","Koppány","Dénes","Gedeon","Brigitta","Miksa","Kálmán, Ede","Helén","Teréz","Gál","Hedvig","Lukács","Nándor","Vendel","Orsolya","Előd","Gyöngyi","Salamon","Blanka, Bianka","Dömötör","Szabina","Simon, Szimonetta","Nárcisz","Alfonz","Farkas" },
						// November
						{ "Marianna","Achilles","Győző","Károly","Imre","Lénárd","Rezső","Zsombor","Tivadar","Réka","Márton","Jónás, Renátó","Szilvia","Aliz","Albert, Lipót","Ödön","Hortenzia, Gergő","Jenő","Erzsébet","Jolán","Olivér","Cecília","Kelemen, Klementina","Emma","Katalin","Virág","Virgil","Stefánia","Taksony","András, Andor","" },
						// December
						{ "Elza","Melinda, Vivien","Ferenc","Borbála, Barbara","Vilma","Miklós","Ambrus","Mária","Natália","Judit","Árpád, Árpádina","Gabriella","Luca, Otília","Szilárda","Valér","Etelka, Aletta","Lázár, Olimpia","Auguszta","Viola","Teofil","Tamás","Zénó","Viktória","Ádám, Éva","Eugénia","István","János","Kamilla","Tamás, Tamara","Dávid","Szilveszter" },
					};
					break;
				}
				case "enGB":
				{
					Nameday = null;
					break;
				}
				case "enUS":
				{
					Nameday = null;
					break;
				}
				case "lvLV":
				{
					Nameday = new string[,]
					{
						// January
						{ "Laimnesis, Solvija, Solvita","Indulis, Iva, Ivis, Ivo","Miervalda, Miervaldis, Ringolds","Ilva, Ilvita, Spodra","Sīmanis, Zintis","Arnita, Spulga","Digmārs, Juliāns, Rota, Zigmārs","Gatis, Ivanda","Aksels, Alta, Kaspars","Dorisa, Tatjana","Franciska, Smaida","Reina, Reinholds, Reinis, Renāts","Aira, Ārijs, Āris, Harijs","Raitis, Raits, Roberta, Roberts","Felicita, Fēlikss","Lida, Lidija","Dravis, Tenis","Antis, Antons","Alnis, Andulis","Alģis, Aļģirds, Oļģerts, Orests","Agne, Agnese, Agnija","Austris","Grieta, Strauta","Eglons, Krišs, Ksenija","Sigurds, Zigurds","Agneta, Agnis, Ansis","Ildze, Ilze, Izolde","Kārlis, Spodris","Aivars, Valērijs","Pārsla, Tīna, Valentīna","Tekla, Violeta" },
						// Februar
						{ "Brigita, Indars, Indra, Indris","Sonora, Spīdola","Aīda, Ida, Vida","Daila, Veronika","Agate, Selga, Silga, Sinilga","Dace, Dārta, Dora","Nelda, Ričards, Rihards","Aldona, Česlavs","Apolonija, Simona","Paula, Paulīne","Laima, Laimdota","Karlīna, Līna","Malda, Melita","Valentīns","Aloizs, Alvils, Olafs, Olavs","Džuljeta, Jūlija","Donats, Konstance","Kintija, Kora","Zane, Zuzanna","Smuidra, Smuidris, Vitauts","Ariadne, Eleonora","Adrians, Ārija, Rigonda","Almants, Haralds","Diāna, Dina, Dins","Alma, Annemarija","Aurēlija, Evelīna, Mētra","Andra, Līva, Līvija","Justs, Skaidra, Skaidrīte","---","","" },
						// Marec
						{ "Ilgvars, Ivars","Laila, Lavīze, Luīze","Marts, Tālavs, Tālis","Alise, Auce","Aurora, Austra","Centis, Gotfrīds, Vents","Ella, Elmīra","Dagmāra, Marga, Margita","Ēvalds","Laimrota, Liliāna, Silvija","Agita, Konstantīns","Aija, Aiva, Aivis","Balvis, Ernests","Matilde, Ulrika","Amalda, Amilda, Imalda","Guntars, Guntis, Guntris","Gerda, Gertrūde, Ģertrūde","Adelīna, Ilona","Jāzeps","Irbe, Made","Benedikts, Dzelme, Una, Unigunde","Dziedra, Tamāra","Mirdza, Žanete, Žanna","Izidors, Kazimirs","Marita, Māra, Mārīte","Eiženija, Ženija","Gustavs, Gusts, Tālrīts","Ginta, Gunda, Gunta","Agija, Aldonis","Igmārs, Ilgmārs, Nanija","Atvars, Gvido" },
						// April
						{ "Dagne, Dagnis","Imgarde, Irmgarde","Daira, Dairis","Ārvalda, Ārvaldis, Ārvalds, Herta, Valda","Aivija, Vidaga, Vija","Dzinta, Filips, Vīlips, Zinta","Helmuts, Zina, Zinaīda","Dana, Dans, Danute, Edgars","Alla, Valērija, Žubīte","Anita, Anitra, Zīle","Hermanis, Vilmārs","Ainis, Jūlijs","Egils, Egīls, Nauris","Gudrīte, Strauja","Aelita, Gastons","Alfs, Bernadeta, Mintauts","Rūdis, Rūdolfs, Viviāna","Jadviga, Laura","Fanija, Vēsma","Mirta, Ziedīte","Anastasija, Marģers","Armanda, Armands","Georgs, Jurģis, Juris","Nameda, Ritvaldis, Visvaldis","Bārbala, Līksma","Alīna, Rūsiņš, Sandris","Klementīne, Raimonda, Raina, Tāle","Gundega, Terēze","Laine, Raimonds, Vilnis","Liāna, Lilija","" },
						// Maj
						{ "Ziedonis","Sigmunds, Zigismunds, Zigmunds","Gints, Uvis","Vijolīte, Viola, Vizbulīte","Ģederts, Ģirts","Didzis, Gaidis","Henriete, Henrijs, Jete","Staņislava, Staņislavs, Stefānija","Einārs, Ervīns, Klāvs","Maija, Paija","Karmena, Manfreds, Milda","Ina, Ināra, Inārs, Valija","Ira, Iraīda, Irēna, Irina","Aivita, Elfa, Elvita, Krišjānis","Airita, Arita, Sofija, Taiga","Edijs, Edvīns","Dailis, Herberts, Umberts","Ēriks, Inese, Inesis","Lita, Sibilla, Teika","Salvis, Selva, Venta","Akvelīna, Ernestīne, Ingmārs","Emīlija","Leokādija, Leontīne, Ligija, Lonija","Ilvija, Marlēna, Ziedone","Anšlavs, Junora","Eduards, Edvards, Varis","Dzidra, Dzidris, Gunita, Loreta","Vilhelms, Vilis","Maksis, Raivis, Raivo","Lolita, Vitolds","Alīda, Jūsma" },
						// Jun
						{ "Bernedīne, Biruta, Mairita","Emma, Lība","Ineta, Inta, Intra","Elfrīda, Sindija, Sintija","Igors, Ingvars, Margots","Ardis, Ingrīda","Arnis, Gaida","Frīda, Frīdis, Mundra","Gita, Ligita","Anatolijs, Anatols, Malva","Ingus, Mairis, Vidvuds","Ija, Lenora, Nora","Ainārs, Uva, Zigfrīds","Saiva, Saivis, Santis, Sentis, Tija","Baņuta, Vilija, Vits, Žermēna","Justīne, Juta","Artis, Artūrs","Alberts, Madis","Nils, Viktors","Maira, Rasa, Rasma","Egita, Emīls, Monvīds","Laimdots, Laimiņš, Ludmila","Līga","Jānis","Maiga, Milija","Ausma, Ausmis, Inguna, Inguns","Malvis, Malvīne","Kitija, Viestards, Viesturs","Paulis, Pauls, Pāvils, Pēteris","Mareks, Tālivaldis","" },
						// Jul
						{ "Imants, Ingars, Intars, Rimants","Halina, Ilvars, Lauma","Benita, Everita, Verita","Sandijs, Sandis, Uldis, Ulvis","Andžejs, Andžs, Edīte, Esmeralda","Anrijs, Arkādijs","Alda, Maruta","Ada, Adele, Antra","Asna, Asne, Zaiga","Lija, Olīvija","Leonora, Svens","Indriķis, Ints, Namejs","Margarita, Margrieta","Anvars, Oskars, Ritvars","Egija, Egmonts, Egons, Henrihs, Henriks","Estere, Hermīne","Aleksejs, Aleksis","Rozālija, Roze","Digna, Jautrīte, Kamila","Ramona, Ritma","Meldra, Meldris, Melisa","Marija, Marika, Marina","Magda, Magdalēna, Magone, Mērija","Krista, Kristiāna, Kristiāns, Kristīne","Jēkabs, Žaklīna","Ance, Anna, Annija","Dita, Marta","Cecīlija, Cilda","Edmunds, Edžus, Vidmants","Renārs, Valters","Angelika, Ruta, Rūta, Sigita" },
						// August
						{ "Albīna, Albīns","Normunds, Stefans","Augusts","Romāns, Romualda, Romualds","Arvils, Osvalds","Aisma, Askolds","Alfrēds, Fredis, Madars","Mudīte, Vladislava, Vladislavs","Genoveva, Madara","Audris, Brencis, Inuta","Liega, Olga, Zigita, Zita","Klāra, Vizma","Elvīra, Rēzija, Velga","Virma, Zelma, Zemgus","Dzelde, Zelda, Zenta","Astra, Astrīda","Oļegs, Vineta","Elena, Ellena, Helēna, Liena, Liene","Imanta, Melānija","Bernhards, Boriss","Janīna, Linda","Everts, Rudīte","Ralfs, Valgudis, Vitālijs","Bērtulis, Boļeslavs","Ivonna, Ludis, Ludvigs, Patrīcija","Broņislava, Broņislavs, Natālija, Tālija","Alens, Jorens, Žanis","Auguste, Guste","Aiga, Armīns, Vismants","Alvis, Jolanta, Samanta","Aigars, Vilma" },
						// September
						{ "Austrums, Ilmārs, Iluta","Elīza, Lizete, Zete","Bella, Berta","Dzintara, Dzintars, Dzintra","Klaudija, Persijs, Vaida","Magnuss, Maigonis, Mariuss","Ermīns, Regīna","Ilga","Bruno, Telma","Albertīne, Jausma","Signe, Signija","Erna, Eva, Evita","Iza, Izabella","Sanda, Sandija, Sanija, Sanita, Santa","Gunvaldis, Gunvaris, Sandra, Sondra","Asja, Asnate, Dāgs, Dārgs","Vaira, Vairis, Vera","Alita, Elita, Liesma","Muntis, Verners","Ginters, Guntra, Marianna","Mariss, Matīss, Modris","Maigurs, Mārica, Māris","Vanda, Veneranda, Venija","Agris, Agrita","Rauls, Rodrigo","Gundars, Knuts, Kurts","Ādolfs, Ilgonis","Lana, Sergejs, Svetlana","Miģelis, Mihails, Miks, Mikus, Miķelis","Elma, Elna, Menarda","" },
						// Oktober
						{ "Lāsma, Zanda, Zandis","Ilma, Skaidris","Elza, Ilizana","Francis, Modra","Amālija","Monika, Zilga, Zilgma","Daumants, Druvvaldis","Aina, Anete","Elga, Elgars, Helga","Arvis, Arvīds, Druvis","Monta, Silva, Tince","Kira, Valfrīds","Irma, Mirga","Minna, Vilhelmīne","Eda, Hedviga, Helvijs","Daiga, Dinija","Gaits, Karīna","Erlends, Rolanda, Rolands, Ronalds","Drosma, Drosmis, Elīna","Leonīda, Leonīds","Severīns, Urzula","Irīda, Īrisa","Daina, Dainida, Dainis","Modrīte, Mudrīte, Renāte","Beatrise, Beāte","Amanda, Amanta, Kaiva","Irita, Ita, Lilita","Antoņina, Ninona, Ņina, Oksana","Elva, Elvijs, Elvis, Laimis, Laimonis","Adīna, Nadīna, Ulla","Rinalda, Rinalds, Valts" },
						// November
						{ "Ikars","Dzīle, Viva, Vivita","Dagnija, Ērika","Atis, Oto, Otomārs","Lote, Šarlote","Leo, Leonarda, Leonards, Leons, Linards","Helma, Lotārs","Agra, Aleksandra","Teodors","Markuss, Mārcis, Mārtiņš","Nellija, Ojārs, Rainers","Kaija, Kornēlija","Eižens, Jevgēņija, Jevgēņijs","Fricis, Vikentijs","Leopolds, Unda, Undīne","Banga, Glorija","Hugo, Uga, Uģis","Aleksandrs, Doloresa","Betija, Elizabete, Liza, Līze","Anda, Andīna","Andis, Zeltīte","Aldis, Aldris, Alfons","Zigfrīda, Zigrīda, Zigrīds","Velda, Velta","Kadrija, Kate, Katrīna, Trīne","Konrāds, Sebastians, Sebastijans","Lauris, Norberts","Olita, Rita, Vita","Ignats, Virgīnija","Andrejs, Andrievs, Andris","" },
						// December
						{ "Arnolds, Emanuels","Meta, Sniedze","Evija, Jogita, Raita","Baiba, Barba, Barbara","Klaudijs, Sabīne, Sarma","Niklāvs, Nikolajs, Niks","Anta, Antonija, Dzirkstīte","Gunārs, Gunis, Vladimirs","Sarmīte, Tabita","Guna, Judīte","Valdemārs, Valdis, Voldemārs","Iveta, Otīlija","Lūcija, Veldze","Auseklis, Gaisma","Hanna, Jana, Johanna","Alvīne","Hilda, Teiksma","Klinta, Kristaps, Kristers, Kristofers, Krists","Lelde, Sarmis","Arta, Minjona","Saulcerīte, Tomass, Toms","Saulvedis","Balva, Viktorija","Ādams, Ieva","Larisa, Stella","Dainuvīte, Gija, Megija","Elmārs, Helmārs, Inita","Inga, Ingeborga, Irvita, Ivita","Ilgona, Solveiga","Daniela, Dāniels, Dāvids, Dāvis","Kalvis, Silvestrs, Silvis" },
					};
					break;
				}
				case "plPL":
				{
					Nameday = new string[,]
					{
						// Január
						{ "Mieszka Mieczyslawa","Izydora Grzegorza","Danuty Zdzislawa","Elzbiety Anieli","Hanny Edwarda Szymona","Kacpra Mekchiora Baltazara","Lucjana Juliana","Seweryna Juliusza","Adriana Marceliny","Danuty Wilhelma","Honoraty Matyldy","Ady Benedykta Arkadiusza","Bogumily Weroniki","Feliksa Niny","Pawla Arnolda Izydora","Marcelego Wlodzimierza","Antoniego Jana","Malgorzaty Piotra","Henryka Marty","Fabiana Sebastiana","Agnieszki Jaroslawa","Anastazego Wincentego","Ildefonsas Rajmunda","Rafala Felicji Franciszska","Pawla Milosza","Tymoteusza Michala","Anieli Jerzego","Walerego Karola","Franciszka Jozefa","Macieja Martyny","Jana Marceli Ludwiki" },
						// Február
						{ "Brygidy Ignacego","Marii Miroslawa","Blazeja Oskara","Andrzeja Jozefa Tytusa","Agaty Adelajdy","Doroty Bogdana Pawla","Ryszarda Teodora","Hieronima Sebastiana","Apoliny Eryki Cyryla","Elwiry Jacka","Grzegorza Lucjana","Radoslawa Damiana","Grzegorza Katarzyny","Cyryla Metodego","Jowity Faustyna","Danuty Juliana","Aleksego Lukasza","Szymona Konstancji","Arnolda Jozefa Konrada","Leona Ludomira","Roberta Eleonory","Marty Malgorzaty","Romany Damiana","Macieja Marka","Cezarego Donata","Miroslawa Aleksandra","Gabriela Anastazji","Romana Ludomira","---","","" },
						// Marec
						{ "Albina Antoniego","Heleny Halszki","Maryny Kunegundy","Arkadiusza Eugeniusza Kazimierza","Adryjana Fryderyka","Rozy Wiktora","Tomasza Felicyty","Beaty Wincentego","Franciszki Dominiki","Cypriana Aleksandra","Benedykta Konstantego","Alojzego Bernarda","Bozeny Krystyny","Leona Martyny","Longina Klemensa","Izabeli Oktawii","Patryka Zbigniewa","Cyryla Edwarda","Jozefa Bogdana","Klaudii Eufemii","Ludomira Benedykta","Katarzyny Boguslawa","Pelagii Feliksa","Marka Gabriela","Marioli Wienczyslawa","Emanuela Larysy Teodora","Lidii Ernesta","Anieli Jana","Viktoryna Helmuta","Anieli Leonarda","Beniamina Balbiny" },
						// Apríl
						{ "Grazyny Ireny","Wladyslawa Franciszka","Ryszarda Ireny","Benedykta Izydora","Katarzyny Wincentego","Izoldy Ireneusza","Rufina Donata","Dionizego Julii","Marii Dymitra","Michala Makarego","Filipa Leona","Damiana Juliusza","Przemyslawa Idy","Bereniki Waleriana","Ludwiny Waclawy","Cecyliana Bernadety","Roberta Rudolfa","Alicji Boguslawy","Adolfa Tymona","Czeslawa Agnieszki","Bartosza Feliksa","Kai Lukasza","Jerzego Wojciecha Idziego","Aleksego Horacego","Marka Jaroslawa","Marzeny Marii Klaudiusza","Ludwika Piotra","Pawla Walerii","Rity Donaty","Mariana Katarzyny","" },
						// Máj
						{ "Jozefa Jeremiasza","Zygmunta Atanazego","Marii Marioli","Moniki Floriana","Ireny Waldemara","Filipa Judyty","Benedykta Gizeli","Ilzy Stanislawa Wiktora","Bozydara Grzegorza","Izydora Antoniny","Igi Ignacego","Joanny Achillesa","Glori Gerwazego","Bonifacego Dobieslawa","Zofii Nadziei","Andrzeja Jedrzeja","Brunona Paschalisa","Eryka Feliksa","Piotra Iwa","Aleksandra Bazylego","Jana Wiktora","Heleny Wieslawy","Emilii Iwony","Joanny Zuzanny","Boryslawa Grzegorza","Filipa Pauliny","Augustyna Juliana","Jaromira Justa","Magdaleny Bogumily","Karola Ferdynanda","Anieli" },
						// Jún
						{ "Jakuba Justyna","Erazma Marianny","Leszka Tamary","Franciszka Karola","Bonifacego Waltera","Norberta Laurentego","Roberta Wieslawa","Maksyma Medarda","Anny Felicjana","Bogumila Malgorzaty","Barnaby Radomila","Janiny Jana","Lucjana Antoniego","Bazylego Elizy","Wita Jolanty","Aliny Benona","Alberta Ignacego","Marka Elzbiety","Gerwazego Protazego","Diny Bogny","Alicji Alojzego","Pauliny Tomasza","Wandy Zenona","Jana Danuty","Lucji Wilhelma","Jana Pawla","Maryli Wladyslawa","Leona Ireneusza","Piotra Pawla","Emilii","" },
						// Júl
						{ "Haliny Mariana","Jagody Urbana","Jacka Anatola","Malwiny Odona","Marii Antoniego","Dominiki Gotarda","Benedykta Cyryla","Adryiany Eugeniusza","Lukrecji Weroniki","Olafa Witalisa","Olgi Kaliny","Jana Brunona","Ernesta Malgorzaty","Bonawentury Stelii","Dawida Henryka","Eustachego Marii","Anety Bogdana","Emila Erwina","Wincentego Wodzislawa","Czeslawa Fryderyka","Daniela Dalidy","Marii Magdaleny","Bogny Apolinarego","Kingi Krystyny","Walentyny Krzysztofa","Anny Miroslawy","Celestyna Lilii","Aidy Innocentego","Olafa Marty","Julity Piotra","Ignacego Lubomira" },
						// August
						{ "Alfonsa Nadii","Kariny Gustawa","Lidii Augusta","Dominika Jana","Marii Oswalda","Slawy Jakuba","Klaudii Kajetana","Cypriana Dominika","Romana Ryszarda","Bogdana Borysa","Klary Lidii","Lecha Euzebii","Diany Hipolita","Alfreda Euzebiusza","Marii Napoleona","Stefana Rocha","Anity Elizy","Ilony Klary","Jana Boleslawa","Bernarda Samuela","Franciszka Joanny","Marii Cezarego","Rozy Apolinarego","Emilii Jerzego","Luizy Ludwika","Marii Zefiryny","Moniki Cezarego","Patrycji Wyszomira","Beaty Jana","Rozy Szczesnego","Izabeli" },
						// September
						{ "Bronislawy Idziego","Juliana Stefana","Izabeli Szymona","Jdy Lilianny","Doroty Wawrzynca","Beaty Eugeniusza","Reginy Melchiora","Marii Adriany","Piotra Sergiusza","Lukasza Mikolaja","Jacka Dagny","Radzimira Gwidona","Eugeni Aureliusza","Roksana Bernarda","Albina Nikodema","Edyty Kornela","Franciszka Hildegardy","Irmy Jozefa","Januarego Konstancji","Filipiny Eustachego","Jonasza Mateusza","Tomasza Maurycego","Boguslawa Tekli","Gerarda Teodora","Aurelii Ladyslawa","Justyny Cypriana","Damiana Amadeusza","Luby Waclawa","Michala Michaliny","Wery","" },
						// Október
						{ "Danuty Remigiusza","Teofila Dinozji","Teresy Heliodora","Rozalii Edwina","Igora Flawii","Artura Brunona","Marii Marka","Pelagii Brygidy","Arnolda Dionizego","Pauliny Franciszka","Emila Aldony","Eustachego Maksymiliana","Gerarda Edwarda","Alana Kaliksta","Teresy Jadwigi","Gawla Florentyny","Malgorzaty Wiktora","Juliusza Lukasza","Pelagii Piotra","Ireny Jana","Urszuli Hilarego","Filipa Korduli","Marleny Seweryna","Rafala Marcina","Darii Wilhelminy","Lucjana Ewarysta","Jwony Sabiny","Szymona Tadeusza","Euzebii Wioletty","Zenobii Przemyslawa","Urbana Saturnina" },
						// November
						{ "Juliana Lukasza","Bohdany Tobiasza","Sylwii Huberta","Karola Olgierda","Elzbiety Slawomira","Feliksa Leonarda","Antoniego Zytomira","Sewera Hadriany","Ursyna Todora","Leny Ludomira","Marcina Bartlomieja","Renaty Witolda","Mikolaja Stanislawa","Rogera Serafiny","Alberta Leopolda","Gertrudy Edmunda","Grzegorza Salomei","Romana Klaudyny","Elzbiety Seweryna","Anatola Sedzimira","Janusza Konrada","Marka Cecylii","Adeli Klemensa","Flory Emmy","Katarzyny Erazma","Delfiny Sylwestra","Waleriana Wilgiusza","Leslawa Zdzislawa","Blazeja Saturnina","Maury","" },
						// December
						{ "Natalii Eligiusza","Balbiny Bibianny","Franciszka Ksawerego","Barbary Krystiana","Saby Kryspina","Mikolaja Jaremy","Marcina Ambrozy","Marii Swiatozara","Wieslawa Leokadii","Julii Danieli","Damazego Waldemara","Dagmary Aleksandry","Lucji Otylii","Alfreda Izydora","Niny Celiny","Albiny Zdzislawy","Olimpii Lazarza","Gracjana Boguslawa","Gabrieli Dariusza","Bogumily Dominika","Tomasza Tomislawa","Zenona Honoraty","Wiktorii Slawomiry","Adama Ewy","Anastazji Eualii","Jana Zanety","Teofili Godzislawa","Jana Maksyma","Dawida Tomasza","Irminy Eugeniusza","Melanii Sylwestra" },
					};
					break;
				}
				case "seSE":
				{
					Nameday = new string[,]
					{
						// January
						{ "Nyårsdagen","Svea","Alfred, Alfrida","Rut","Hanna, Hannele","Kasper,Melker,Baltsar/13-dag jul","August, Augusta","Erland","Gunnar, Gunder","Sigurd, Sigbritt","Jan, Jannike","Frideborg, Fridolf","Knut / Tjugondedag jul","Felix, Felicia","Laura, Lorentz","Hjalmar, Helmer","Anton, Tony","Hilda, Hildur","Henrik","Fabian, Sebastian","Agnes, Agneta","Vincent, Viktor","Frej, Freja","Erika","Paul, Pål","Bodil, Boel","Göte, Göta","Karl, Karla","Diana","Gunilla, Gunhild","Ivar, Joar" },
						// February
						{ "Max, Maximilian","Kyndelsmässodagen","Disa, Hjördis","Ansgar, Anselm","Agata, Agda","Dorotea, Doris","Rikard, Dick","Berta, Bert","Fanny, Franciska","Iris","Yngve, Inge","Evelina, Evy","Agne, Ove","Valentin","Sigfrid","Julia, Julius","Alexandra, Sandra","Frida, Fritiof","Gabriella, Ella","Vivianne","Hilding","Pia","Torsten, Torun","Mattias, Mats","Sigvard, Sivert","Torgny, Torkel","Lage","Maria","Skottdagen","","" },
						// March
						{ "Albin, Elvira","Ernst, Erna","Gunborg, Gunvor","Adrian, Adriana","Tora, Tove","Ebba, Ebbe","Camilla","Siv","Torbjörn, Torleif","Edla, Ada","Edvin, Egon","Viktoria","Greger","Matilda, Maud","Kristoffer, Christel","Herbert, Gilbert","Gertrud","Edvard, Edmund","Josef, Josefina","Josef, Josefina","Bengt","Kennet, Kent","Gerda, Gerd","Gabriel, Rafael","Marie bebådelsedag","Emanuel","Rudolf, Ralf","Malkolm, Morgan","Jonas Jens","Holger Holmfrid","Ester" },
						// April
						{ "Harald, Hervor","Gudmund, Ingeund","Ferdinand, Nanna","Marianne, Marlene","Irene, Irja","Vilhelm, Helmi","Irma, Irmelin","Nadja, Tanja","Otto, Ottilia","Ingvar, Ingvor","Ulf, Ylva","Liv","Artur, Douglas","Tiburtius","Olivia, Oliver","Patrik, Patricia","Elias, Elis","Valdemar, Volmar","Olaus, Ola","Amalia, Amelie","Anneli, Annika","Allan, Glenn","Georg, Göran","Vega","Markus","Teresia, Terese","Engelbrekt","Ture, Tyra","Tyko","Mariana","" },
						// May
						{ "Valborg","Filip, Filippa","John, Jane","Monika, Mona","Gotthard, Erhard","Marit, Rita","Carina, Carita","Åke","Reidar, Reidun","Esbjörn, Styrbjörn","Märta, Märit","Charlotta, Lotta","Linnea, Linn","Halvard, Halvar","Sofia, Sonja","Ronald, Ronny","Rebecka, Ruben","Erik","Maj, Majken","Karolina, Carola","Konstantin, Conny","Hemming, Henning","Desideria, Desiree","Ivan, Vanja","Urban","Vilhelmina, Vilma","Beda, Blenda","Ingeborg, Borghild","Yvonne, Jeanette","Vera, Veronika","Petronella, Pernilla" },
						// June
						{ "Gun, Gunnel","Rutger, Roger","Ingemar, Gudmar","Solbritt, Solveig","Bo","Gustav, Gösta","Robert, Robin","Eivor, Majvor","Börje, Birger","Svante, Boris","Bertil, Berthold","Eskil","Aina, Aino","Håkan, Hakon","Margit, Margot","Axel, Axelina","Torborg, Torvald","Björn, Bjarne","Germund, Görel","Linda","Alf, Alvar","Paulina, Paula","Adolf, Alice","Johannes Döparens dag","David, Salomon","Rakel, Lea","Selma, Fingal","Leo","Peter, Petra","Elof, Leif","" },
						// July
						{ "Aron, Mirjam","Rosa, Rosita","Aurora","Ulrika, Ulla","Laila, Ritva","Esaias, Jessika","Klas","Kjell","Jörgen, Örjan","Andre, Andrea","Eleonora, Ellinor","Herman, Hermine","Joel, Judit","Folke","Ragnhild, Ragnvald","Reinhold, Reine","Bruno","Fredrik, Fritz","Sara","Margareta, Greta","Johanna","Magdalena, Madeleine","Emma","Kristina, Kerstin","Jakob","Jesper","Marta","Botvid, Seved","Olof","Algot","Helena, Elin" },
						// August
						{ "Per","Karin, Kajsa","Tage","Arne, Arnold","Ulrik, Alrik","Alfons, Inez","Dennis, Denise","Silvia, Sylvia","Roland","Lars","Susanna","Klara","Kaj","Uno","Stella, Estelle","Brynolf","Verner, Valter","Ellen, Lena","Magnus, Måns","Bernhard, Bernt","Jon, Jonna","Henrietta, Henrika","Signe, Signhild","Bartolomeus","Lovisa, Louise","Östen","Rolf, Raoul","Gurli, Leila","Hans, Hampus","Albert, Albertina","Arvid, Vidar" },
						// September
						{ "Samuel","Justus, Justina","Alfhild, Alva","Gisela","Adela, Heidi","Lilian, Lilly","Regina, Roy","Alma, Hulda","Anita, Annette","Tord, Turid","Dagny, Helny","Åsa, Åslög","Sture","Ida","Sigrid, Siri","Dag, Daga","Hildegard, Magnhild","Orvar","Fredrika","Elise, Lisa","Matteus","Maurits, Moritz","Tekla, Tea","Gerhard, Gert","Tryggve","Enar, Einar","Dagmar, Rigmor","Lennart, Leonard","Mikael, Mikaela","Helge","" },
						// October
						{ "Ragnar, Ragna","Ludvig, Love","Evald, Osvald","Frans, Frank","Bror","Jenny, Jennifer","Birgitta, Britta","Nils","Ingrid, Inger","Harry, Harriet","Erling, Jarl","Valfrid, Manfred","Berit, Birgit","Stellan","Hedvig, Hillevi","Finn","Antonia, Toini","Lukas","Tore, Tor","Sibylla","Ursula, Yrsa","Marika, Marita","Severin, Sören","Evert, Eilert","Inga, Ingalill","Amanda, Rasmus","Sabina","Simon, Simone","Viola","Elsa, Isabella","Edit, Edgar" },
						// November
						{ "Allhelgonadagen","Tobias","Hubert, Hugo","Sverker","Eugen, Eugenia","Gustav, Adolf","Ingegerd, Ingela","Vendela","Teodor, Teodora","Martin Martina","Mårten","Konrad, Kurt","Kristian, Krister","Emil, Emilia","Leopold","Vibeke, Viveka","Naemi, Naima","Lillemor, Moa","Elisabet, Lisbet","Pontus, Marina","Helga, Olga","Cecilia, Sissela","Klemens","Gudrun, Rune","Katarina, Katja","Linus","Astrid, Asta","Malte","Sune","Andreas, Anders","" },
						// December
						{ "Oskar, Ossian","Beata, Beatrice","Lydia","Barbara, Barbro","Sven","Nikolaus, Niklas","Angela, Angelika","Virginia","Anna","Malin, Malena","Daniel, Daniela","Alexander, Alexis","Lucia","Sten, Sixten","Gottfrid","Assar","Stig","Abraham","Isak","Israel, Moses","Tomas","Natanael, Jonatan","Adam","Eva","Juldagen","Stefan, Staffan","Johannes, Johan","Benjamin / Värnlösa barns dag","Natalia, Natalie","Abel, Set","Sylvester" },
					};
					break;
				}
				default:
				{
					// huHU lesz az alapértelmezett amíg nincs angol verzió
					Nameday = new string[,]
					{
						// Január
						{ "Fruzsina","Ábel","Genovéva, Benjámin","Titusz, Leona, Angel","Simon","Boldizsár","Attila, Ramóna","Gyöngyvér","Marcell","Melánia","Ágota","Ernő","Veronika","Bódog","Lóránt, Loránd","Gusztáv","Antal, Antónia","Piroska","Sára, Márió","Fábián, Sebestyén","Ágnes","Vince, Artúr","Zelma, Rajmund","Timót","Pál","Vanda, Paula","Angelika","Károly, Karola","Adél","Martina, Gerda","Marcella" },
						// Február
						{ "Ignác","Karolina, Aida","Balázs","Ráhel, Csenge","Ágota, Ingrid","Dorottya, Dóra","Tódor, Rómeó","Aranka","Abigél, Alex","Elvira","Bertold, Marietta","Lídia, Lívia","Ella, Linda","Bálint, Valentin","Kolos, Georgina","Julianna, Lilla","Donát","Bernadett","Zsuzsanna","Aladár, Álmos","Eleonóra","Gerzson","Alfréd","Mátyás","Géza","Edina","Ákos, Bátor","Elemér","---","","" },
						// Március
						{ "Albin","Lujza","Kornélia","Kázmér","Adorján, Adrián","Leonóra, Inez","Tamás","Zoltán","Franciska, Fanni","Ildikó","Szilárd","Gergely","Krisztián, Ajtony","Matild","Kristóf","Henrietta","Gertrúd, Patrik","Sándor, Ede","József, Bánk","Klaudia, Tubs, Jessica","Benedek","Beáta, Izolda, Lea","Emőke","Gábor, Karina","Irén, Írisz","Emánuel","Hajnalka","Gedeon, Johanna","Auguszta","Zalán","Árpád" },
						// Április
						{ "Hugó","Áron","Buda, Richárd","Izidor","Vince","Vilmos, Bíborka","Herman","Dénes","Erhard","Zsolt","Leó, Szaniszló","Gyula","Ida","Tibor","Anasztázia, Tas","Csongor","Rudolf","Andrea, Ilma","Emma","Tivadar","Konrád","Csilla, Noémi","Béla","György","Márk","Ervin","Zita","Valéria","Péter","Katalin, Kitti","" },
						// Május
						{ "Fülöp, Jakab, Zsaklin","Zsigmond","Tímea, Irma","Mónika, Flórián","Györgyi","Ivett, Frida","Gizella","Mihály","Gergely","Ármin, Pálma","Ferenc","Pongrác","Szervác, Imola","Bonifác","Zsófia, Szonja","Mózes, Botond","Paszkál","Erik, Alexandra","Ivó, Milán","Bernát, Felícia","Konstantin","Júlia, Rita","Dezső","Eszter, Eliza","Orbán","Fülöp, Evelin","Hella","Emil, Csanád","Magdolna","Janka, Zsanett","Angéla, Petronella" },
						// Június
						{ "Tünde","Kármen, Anita","Klotild, Cecília","Bulcsú","Fatime, Fatima","Norbert, Cintia","Róbert","Medárd","Félix","Margit, Gréta","Barnabás","Villő","Antal, Anett","Vazul","Jolán, Vid","Jusztin","Laura, Alida","Arnold, Levente","Gyárfás","Rafael","Alajos, Leila","Paulina","Zoltán","Iván","Vilmos","János, Pál","László","Levente, Irén","Péter, Pál","Pál","" },
						// Július
						{ "Tihamér, Annamária","Ottó","Kornél, Soma","Ulrik","Emese, Sarolta","Csaba","Apollónia","Ellák","Lukrécia","Amália","Nóra, Lili","Izabella, Dalma","Jenő","Örs, Stella","Örkény, Henrik, Roland","Valter","Endre, Elek","Frigyes","Emília","Illés","Dániel, Daniella","Magdolna","Lenke","Kinga, Kincső","Kristóf, Jakab","Anna, Anikó","Olga, Liliána","Szabolcs","Márta, Flóra","Judit, Xénia","Oszkár" },
						// Augusztus
						{ "Boglárka","Lehel","Hermina","Domonkos, Dominika","Krisztina","Berta, Bettina","Ibolya","László","Emőd","Lőrinc","Zsuzsanna, Tiborc","Klára","Ipoly","Marcell","Mária","Ábrahám","Jácint","Ilona","Huba","István","Sámuel, Hajna","Menyhért, Mirjam","Bence","Bertalan","Lajos, Patrícia","Izsó","Gáspár","Ágoston","Beatrix, Erna","Rózsa","Erika, Bella" },
						// Szeptember
						{ "Egyed, Egon","Rebeka, Dorina","Hilda","Rozália","Viktor, Lőrinc","Zakariás","Regina","Mária, Adrienn","Ádám","Nikolett, Hunor","Teodóra","Mária","Kornél","Szeréna, Roxána","Enikő, Melitta","Edit","Zsófia","Diána","Vilhelmina","Friderika","Máté, Mirella","Móric","Tekla, Líviusz","Gellért, Mercédesz","Eufrozina, Kende","Jusztina","Adalbert","Vencel","Mihály","Jeromos","" },
						// Oktober
						{ "Malvin","Petra","Helga","Ferenc","Aurél","Brúnó, Renáta","Amália","Koppány","Dénes","Gedeon","Brigitta","Miksa","Kálmán, Ede","Helén","Teréz","Gál","Hedvig","Lukács","Nándor","Vendel","Orsolya","Előd","Gyöngyi","Salamon","Blanka, Bianka","Dömötör","Szabina","Simon, Szimonetta","Nárcisz","Alfonz","Farkas" },
						// November
						{ "Marianna","Achilles","Győző","Károly","Imre","Lénárd","Rezső","Zsombor","Tivadar","Réka","Márton","Jónás, Renátó","Szilvia","Aliz","Albert, Lipót","Ödön","Hortenzia, Gergő","Jenő","Erzsébet","Jolán","Olivér","Cecília","Kelemen, Klementina","Emma","Katalin","Virág","Virgil","Stefánia","Taksony","András, Andor","" },
						// December
						{ "Elza","Melinda, Vivien","Ferenc","Borbála, Barbara","Vilma","Miklós","Ambrus","Mária","Natália","Judit","Árpád, Árpádina","Gabriella","Luca, Otília","Szilárda","Valér","Etelka, Aletta","Lázár, Olimpia","Auguszta","Viola","Teofil","Tamás","Zénó","Viktória","Ádám, Éva","Eugénia","István","János","Kamilla","Tamás, Tamara","Dávid","Szilveszter" },
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
					var th = new Thread(() =>
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

					using(var response = (HttpWebResponse)request.GetResponse())
					{
						using(var stream = response.GetResponseStream())
						{
							while((length = stream.Read(buf, 0, buf.Length)) != 0)
							{
								if(sb.Length >= maxlength)
									break;

								buf = Encoding.Convert(Encoding.GetEncoding(response.CharacterSet), Encoding.UTF8, buf);
								sb.Append(Encoding.UTF8.GetString(buf, 0, length));
							}
						}
					}

					if(!th.IsNull())
					{
						th.Interrupt();
						th = null;
					}

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
					var th = new Thread(() =>
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

					using(var response = (HttpWebResponse)request.GetResponse())
					{
						using(var stream = response.GetResponseStream())
						{
							if(maxlength == 0)
								maxlength = 10000;

							while((length = stream.Read(buf, 0, buf.Length)) != 0)
							{
								if(sb.ToString().Contains(Contains) || sb.Length >= 10000)
									break;

								buf = Encoding.Convert(Encoding.GetEncoding(response.CharacterSet), Encoding.UTF8, buf);
								sb.Append(Encoding.UTF8.GetString(buf, 0, length));
							}
						}
					}

					if(!th.IsNull())
					{
						th.Interrupt();
						th = null;
					}

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
					var th = new Thread(() =>
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

					using(var response = (HttpWebResponse)request.GetResponse())
					{
						using(var stream = response.GetResponseStream())
						{
							if(maxlength == 0)
								maxlength = 10000;

							while((length = stream.Read(buf, 0, buf.Length)) != 0)
							{
								if(regex.Match(sb.ToString()).Success || sb.Length >= maxlength)
									break;

								buf = Encoding.Convert(Encoding.GetEncoding(response.CharacterSet), Encoding.UTF8, buf);
								sb.Append(Encoding.UTF8.GetString(buf, 0, length));
							}
						}
					}

					if(!th.IsNull())
					{
						th.Interrupt();
						th = null;
					}

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
			var nowtime = DateTime.Now;
			nowtime = string.Format("{0}-{1}-{2} {3}:{4}", nowtime.Year, nowtime.Month, nowtime.Day, nowtime.Hour, nowtime.Minute).ToDateTime(dtfi);
			var newtime = string.Format("{0}-{1}-{2} {3}:{4}", Year, Month, Day, Hour, Minute).ToDateTime(dtfi);
			var compare = DateTime.Compare(nowtime, newtime);
			return compare == 1 || compare == 0;
		}

		public string GetUserName()
		{
			return Environment.UserName;
		}

		public string GetSpecialDirectory(string data)
		{
			if(sPlatform.IsWindows)
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
			else if(sPlatform.IsLinux)
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
			else if(sPlatform.IsWindows && dir.Contains("$localappdata"))
				return true;
			else
				return false;
		}

		public string DirectoryToSpecial(string dir, string file)
		{
			if(sPlatform.IsWindows)
			{
				if(dir.Length > 2 && dir.Substring(1, 2) == @":\")
					return string.Format(@"{0}\{1}", dir, file);
				else if(dir.Length > 2 && dir.Substring(0, 2) == "//")
					return string.Format("//{0}/{1}", dir, file);
				else
					return string.Format("./{0}/{1}", dir, file);
			}
			else if(sPlatform.IsLinux)
				return (dir.Length > 0 && dir.Substring(0, 1) == "/") ? string.Format("{0}/{1}", dir, file) : string.Format("./{0}/{1}", dir, file);
			else
				return string.Format("{0}/{1}", dir, file);
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

		public void ClearAttributes(string currentDir)
		{
			if(Directory.Exists(currentDir))
			{
				var subDirs = Directory.GetDirectories(currentDir);

				foreach(string dir in subDirs)
					ClearAttributes(dir);

				var files = Directory.GetFiles(currentDir);

				foreach(string file in files)
					File.SetAttributes(file, FileAttributes.Normal);
			}
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

#if RELEASE
			if(File.Exists("KeraLua.dll.mdb"))
				File.Delete("KeraLua.dll.mdb");

			if(File.Exists("KopiLua.dll.mdb"))
				File.Delete("KopiLua.dll.mdb");
#endif
		}
	}
}