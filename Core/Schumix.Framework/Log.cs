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
using System.Xml;
using System.Collections.Generic;
using Schumix.Framework.Config;

namespace Schumix.Framework
{
	public sealed class Log
	{
		private static readonly object WriteLock = new object();
		private static string ConfigFile;

        /// <returns>
        ///     A visszatérési érték az aktuális dátum.
        /// </returns>
		private static string GetTime()
		{
			return string.Format("{0}:{1}:{2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
		}

		private static void LogToFile(string log)
		{
			try
			{
				string loghelye;
				if(LogConfig.LogHelye == null)
					loghelye = InitConfig();
				else
					loghelye = LogConfig.LogHelye;

				var file = new StreamWriter(string.Format("./{0}/{1}", loghelye, "Schumix.log"), true) { AutoFlush = true };
				file.Write(log);
				file.Close();
			}
			catch(Exception/* e*/)
			{
				// semmi
			}
		}

		private static string InitConfig()
		{
			var xml = new XmlDocument();
			xml.Load(ConfigFile);
			return xml.SelectSingleNode("Schumix/Log/LogHelye").InnerText;
		}

		public static void Indulas(string configfile)
		{
			try
			{
				ConfigFile = configfile;
				string loghelye = InitConfig();

				if(!Directory.Exists(loghelye))
					Directory.CreateDirectory(loghelye);

				var time = DateTime.Now;
				string logfile = "Schumix.log";

				if(!File.Exists(string.Format("./{0}/{1}", loghelye, logfile)))
					File.Create(string.Format("./{0}/{1}", loghelye, logfile));

				var file = new StreamWriter(string.Format("./{0}/{1}", loghelye, logfile), true) { AutoFlush = true };
				file.Write("\nIndulási időpont: [{0}. {1}. {2}. {3}:{4}:{5}]\n", time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second);
				file.Close();
			}
			catch(Exception/* e*/)
			{
				Indulas(ConfigFile);
			}
		}

        /// <summary>
        ///     Dátummal logolja a szöveget meghatározva honnan származik. 
        ///     Lehet ez egyénileg meghatározott függvény vagy class névvel ellátva.
        ///     Logol a Console-ra.
        /// </summary>
        /// <param name="source">
        ///     Meghatározza honnan származik a log.
        ///     <example>
        ///         17:28 N <c>Config:</c> Config file betöltése...
        ///     </example>
        /// </param>
        /// <param name="format">
        ///     A szöveg amit kiírunk.
        ///     <example>
        ///         17:28 N Config: <c>Config file betöltése...</c>
        ///     </example>
        /// </param>
		public static void Notice(string source, string format)
		{
			lock(WriteLock)
			{
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.Write(GetTime());
				Console.ForegroundColor = ConsoleColor.White;
				Console.Write(" N {0}: ", source);
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.Write("{0}\n", format);
				LogToFile(GetTime() + string.Format(" N {0}: {1}\n", source, format));
			}
		}

		public static void Success(string source, string format)
		{
			lock(WriteLock)
			{
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.Write(GetTime());
				Console.ForegroundColor = ConsoleColor.Green;
				Console.Write(" S");
				Console.ForegroundColor = ConsoleColor.White;
				Console.Write(" {0}: ", source);
				Console.ForegroundColor = ConsoleColor.Green;
				Console.Write("{0}\n", format);
				Console.ForegroundColor = ConsoleColor.Gray;
				LogToFile(GetTime() + string.Format(" S {0}: {1}\n", source, format));
			}
		}

		public static void Warning(string source, string format)
		{
			lock(WriteLock)
			{
				if(LogConfig.LogLevel < 1)
					return;

				Console.ForegroundColor = ConsoleColor.Gray;
				Console.Write(GetTime());
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.Write(" W");
				Console.ForegroundColor = ConsoleColor.White;
				Console.Write(" {0}: ", source);
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.Write("{0}\n", format);
				Console.ForegroundColor = ConsoleColor.Gray;
				LogToFile(GetTime() + string.Format(" W {0}: {1}\n", source, format));
			}
		}

		public static void Error(string source, string format)
		{
			lock(WriteLock)
			{
				if(LogConfig.LogLevel < 2)
					return;

				Console.ForegroundColor = ConsoleColor.Gray;
				Console.Write(GetTime());
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write(" E");
				Console.ForegroundColor = ConsoleColor.White;
				Console.Write(" {0}: ", source);
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write("{0}\n", format);
				Console.ForegroundColor = ConsoleColor.Gray;
				LogToFile(GetTime() + string.Format(" E {0}: {1}\n", source, format));
			}
		}

		public static void Debug(string source, string format)
		{
			lock(WriteLock)
			{
				if(LogConfig.LogLevel < 3)
					return;

				Console.ForegroundColor = ConsoleColor.Gray;
				Console.Write(GetTime());
				Console.ForegroundColor = ConsoleColor.Blue;
				Console.Write(" D");
				Console.ForegroundColor = ConsoleColor.White;
				Console.Write(" {0}: ", source);
				Console.ForegroundColor = ConsoleColor.Blue;
				Console.Write("{0}\n", format);
				Console.ForegroundColor = ConsoleColor.Gray;
				LogToFile(GetTime() + string.Format(" D {0}: {1}\n", source, format));
			}
		}

		public static void LargeWarning(string message)
		{
			lock(WriteLock)
			{
				string[] sp = message.Split('\n');
				List<string> lines = new List<string>(50);

				foreach(string s in sp)
				{
					if(!string.IsNullOrEmpty(s))
						lines.Add(s);
				}

				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine();
				Console.WriteLine("**************************************************"); // 51
				
				foreach(string item in lines)
				{
					uint len = (uint)item.Length;
					uint diff = (48-len);
					Console.Write("* {0}", item);

					if(diff > 0)
					{
						for(uint u = 1; u < diff; ++u)
							Console.Write(" ");
						
						Console.Write("*\n");
					}
				}
				
				Console.WriteLine("**************************************************");
			}
		}

		public static void Notice(string source, string format, params object[] args)
		{
			lock(WriteLock)
			{
				Notice(source, string.Format(format, args));
			}
		}

		public static void Success(string source, string format, params object[] args)
		{
			lock(WriteLock)
			{
				Success(source, string.Format(format, args));
			}
		}

		public static void Warning(string source, string format, params object[] args)
		{
			lock(WriteLock)
			{
				Warning(source, string.Format(format, args));
			}
		}

		public static void Error(string source, string format, params object[] args)
		{
			lock(WriteLock)
			{
				Error(source, string.Format(format, args));
			}
		}

		public static void Debug(string source, string format, params object[] args)
		{
			lock(WriteLock)
			{
				Debug(source, string.Format(format, args));
			}
		}

		public static void LargeWarning(string message, params object[] args)
		{
			lock(WriteLock)
			{
				LargeWarning(string.Format(message, args));
			}
		}
	}
}