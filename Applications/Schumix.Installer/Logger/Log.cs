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
using System.Text;
using System.Collections.Generic;
using Schumix.Installer.Extensions;
using Schumix.Installer.Localization;

namespace Schumix.Installer.Logger
{
	public sealed class Log
	{
		private static readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private static readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private static readonly object WriteLock = new object();
		private static string _Directory;
		private static string _FileName;
		private static int _LogLevel;

		public Log()
		{
			_LogLevel = 3;
		}

		/// <returns>
		///		A visszatérési érték az aktuális dátum.
		/// </returns>
		private static string GetTime()
		{
			return DateTime.Now.ToString("HH:mm:ss");
		}

		public static void SetLogLevel(int level)
		{
			_LogLevel = level;
		}
		
		public static void LogInFile(string log)
		{
			lock(WriteLock)
			{
				string filename = _Directory + "/" + _FileName;
				var filesize = new FileInfo(filename);

				if(filesize.Length >= 10 * 1024 * 1024)
				{
					File.Delete(filename);
					sUtilities.CreateFile(filename);
				}

				var time = DateTime.Now;
				var file = new StreamWriter(filename, true) { AutoFlush = true };
				file.Write("{0} {1} {2}", time.ToString("yyyy. MM. dd."), GetTime(), log);
				file.Close();
			}
		}

		public static void LogInFile(string log, params object[] args)
		{
			lock(WriteLock)
			{
				LogInFile(string.Format(log, args));
			}
		}

		public static void Initialize()
		{
			Initialize("Installer.log");
		}

		public static void Initialize(string FileName)
		{
			Initialize(FileName, "Logs");
		}

		public static void Initialize(string FileName, string Directory)
		{
			bool isfile = false;
			_FileName = FileName;
			_Directory = Directory;
			var time = DateTime.Now;
			sUtilities.CreateDirectory(Directory);
			string logfile = Directory + "/" + _FileName;

			if(File.Exists(logfile))
				isfile = true;

			sUtilities.CreateFile(logfile);
			var file = new StreamWriter(logfile, true) { AutoFlush = true };

			if(!isfile)
				file.Write(sLConsole.GetString("Started time: [{0}]\n"), time.ToString("yyyy. MM. dd. HH:mm:ss"));
			else
				file.Write(sLConsole.GetString("\nStarted time: [{0}]\n"), time.ToString("yyyy. MM. dd. HH:mm:ss"));
				
			file.Close();
		}

		/// <summary>
		///	 Dátummal logolja a szöveget meghatározva honnan származik. 
		///	 Lehet ez egyénileg meghatározott függvény vagy class névvel ellátva.
		///	 Logol a Console-ra.
		/// </summary>
		/// <param name="source">
		///	 Meghatározza honnan származik a log.
		///	 <example>
		///		 17:28 N <c>Config:</c> Config file betöltése...
		///	 </example>
		/// </param>
		/// <param name="format">
		///	 A szöveg amit kiírunk.
		///	 <example>
		///		 17:28 N Config: <c>Config file betöltése...</c>
		///	 </example>
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
				LogInFile("N {0}: {1}\n", source, format);
			}
		}

		public static void Notice(string source, StringBuilder format)
		{
			lock(WriteLock)
			{
				Notice(source, format.ToString());
			}
		}

		public static void Notice(string source, string format, params object[] args)
		{
			lock(WriteLock)
			{
				Notice(source, string.Format(format, args));
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
				LogInFile("S {0}: {1}\n", source, format);
			}
		}

		public static void Success(string source, StringBuilder format)
		{
			lock(WriteLock)
			{
				Success(source, format.ToString());
			}
		}

		public static void Success(string source, string format, params object[] args)
		{
			lock(WriteLock)
			{
				Success(source, string.Format(format, args));
			}
		}

		public static void Warning(string source, string format)
		{
			lock(WriteLock)
			{
				if(_LogLevel < 1)
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
				LogInFile("W {0}: {1}\n", source, format);
			}
		}

		public static void Warning(string source, StringBuilder format)
		{
			lock(WriteLock)
			{
				Warning(source, format.ToString());
			}
		}

		public static void Warning(string source, string format, params object[] args)
		{
			lock(WriteLock)
			{
				Warning(source, string.Format(format, args));
			}
		}

		public static void Error(string source, string format)
		{
			lock(WriteLock)
			{
				if(_LogLevel < 2)
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
				LogInFile("E {0}: {1}\n", source, format);
			}
		}

		public static void Error(string source, StringBuilder format)
		{
			lock(WriteLock)
			{
				Error(source, format.ToString());
			}
		}

		public static void Error(string source, string format, params object[] args)
		{
			lock(WriteLock)
			{
				Error(source, string.Format(format, args));
			}
		}

		public static void Debug(string source, string format)
		{
			lock(WriteLock)
			{
				if(_LogLevel < 3)
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
				LogInFile("D {0}: {1}\n", source, format);
			}
		}

		public static void Debug(string source, StringBuilder format)
		{
			lock(WriteLock)
			{
				Debug(source, format.ToString());
			}
		}

		public static void Debug(string source, string format, params object[] args)
		{
			lock(WriteLock)
			{
				Debug(source, string.Format(format, args));
			}
		}

		public static void LargeWarning(string message)
		{
			lock(WriteLock)
			{
				var sp = message.Split('\n');
				var lines = new List<string>(50);

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
				Console.ForegroundColor = ConsoleColor.Gray;
			}
		}

		
		public static void LargeWarning(StringBuilder message)
		{
			lock(WriteLock)
			{
				LargeWarning(message.ToString());
			}
		}

		public static void LargeWarning(string message, params object[] args)
		{
			lock(WriteLock)
			{
				LargeWarning(string.Format(message, args));
			}
		}

		public static void LargeError(string message)
		{
			lock(WriteLock)
			{
				var sp = message.Split('\n');
				var lines = new List<string>(50);

				foreach(string s in sp)
				{
					if(!string.IsNullOrEmpty(s))
						lines.Add(s);
				}

				Console.ForegroundColor = ConsoleColor.Red;
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
				Console.ForegroundColor = ConsoleColor.Gray;
			}
		}

		public static void LargeError(StringBuilder message)
		{
			lock(WriteLock)
			{
				LargeError(message.ToString());
			}
		}

		public static void LargeError(string message, params object[] args)
		{
			lock(WriteLock)
			{
				LargeError(string.Format(message, args));
			}
		}
	}
}