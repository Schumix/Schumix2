/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013-2014 Schumix Team <http://schumix.eu/>
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
using System.Threading;
using System.Collections.Generic;
using Schumix.Framework.Util;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Framework.Logger
{
	public sealed class Log : DefaultConfig
	{
		private static readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private static readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private static readonly object WriteLock = new object();
		private static bool _ColorblindMode;
		private static string _LogDirectory;
		private static bool _DateFileName;
		private static string _FileName;
		private static bool _started;

		private Log()
		{
			_started = false;
		}

		/// <returns>
		///		A visszatérési érték az aktuális dátum.
		/// </returns>
		private static string GetTime()
		{
			return DateTime.Now.ToString("HH:mm:ss");
		}

		public static void LogInFile(string log)
		{
			if(LogConfig.LogDirectory.IsNullOrEmpty())
				return;

			lock(WriteLock)
			{
				string filename = sUtilities.DirectoryToSpecial(LogConfig.LogDirectory, _FileName);
				var filesize = new FileInfo(filename);

				if(filesize.Length >= LogConfig.MaxFileSize * 1024 * 1024)
				{
					File.Delete(filename);
					sUtilities.CreateFile(filename);
				}

				var time = DateTime.Now;
				var file = new StreamWriter(filename, true) { AutoFlush = true };
				file.WriteLine("{0} {1} {2}", time.ToString("yyyy. MM. dd."), GetTime(), log);
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

		public static string GetTypeCharacter(LogType type)
		{
			string character = "N";

			switch(type)
			{
			case LogType.Success:
				character = "S";
				break;
			case LogType.Warning:
				character = "W";
				break;
			case LogType.Error:
				character = "E";
				break;
			case LogType.Debug:
				character = "D";
				break;
			}

			return character;
		}

		public static void SetForegroundColor(ConsoleColor color)
		{
			if(!_ColorblindMode)
				Console.ForegroundColor = color;
		}

		public static void Initialize()
		{
			Initialize(d_logfilename, false);
		}

		public static void Initialize(string FileName)
		{
			Initialize(FileName, false);
		}

		public static void Initialize(string FileName, bool ColorblindMode)
		{
			string oldfile = _FileName;
			bool olddatefilename = _DateFileName;
			string oldlogdirectory = _LogDirectory;
			_FileName = FileName;
			_LogDirectory = LogConfig.LogDirectory;
			_DateFileName = LogConfig.DateFileName;
			_ColorblindMode = ColorblindMode;
			var time = DateTime.Now;
			sUtilities.CreateDirectory(LogConfig.LogDirectory);

			if(LogConfig.DateFileName)
			{
				string f = _FileName;

				if(f.ToLower().Contains(".log"))
					f = f.Substring(0, f.IndexOf(".log"));

				if(_started && olddatefilename && oldfile.Substring(0, oldfile.IndexOf("/")) == f)
				{
					_FileName = oldfile;
					return;
				}

				if(_FileName.ToLower().Contains(".log"))
					_FileName = _FileName.Substring(0, _FileName.IndexOf(".log"));

				sUtilities.CreateDirectory(LogConfig.LogDirectory + "/" + _FileName);
				_FileName = _FileName + "/" + time.ToString("yyyy_MM_dd-HH_mm_ss") + ".log";

				bool isfile = false;
				string logfile = sUtilities.DirectoryToSpecial(LogConfig.LogDirectory, _FileName);

				if(File.Exists(logfile))
					isfile = true;

				sUtilities.CreateFile(logfile);

				if((_started && !olddatefilename) || (_started && olddatefilename && oldfile != f))
				{
					string oldlogfile = sUtilities.DirectoryToSpecial(oldlogdirectory, oldfile);
					var ofile = new StreamWriter(oldlogfile, true) { AutoFlush = true };
					ofile.Write(sLConsole.GetString("The log's file name changed. From now in it will be here: {0}\n"), logfile);
					ofile.Close();

					var file = new StreamWriter(logfile, true) { AutoFlush = true };

					if(!isfile)
						file.Write(sLConsole.GetString("The log file's name changed. {0} was the old. From now on the logs will be here.\n"), oldfile);
					else
						file.Write(sLConsole.GetString("\nThe log file's name changed. {0} was the old. From now on the logs will be here.\n"), oldfile);

					file.Close();
				}
			}
			else
			{
				if(oldfile == FileName)
					return;

				bool isfile = false;
				string logfile = sUtilities.DirectoryToSpecial(LogConfig.LogDirectory, _FileName);

				if(File.Exists(logfile))
					isfile = true;

				sUtilities.CreateFile(logfile);
				var file = new StreamWriter(logfile, true) { AutoFlush = true };

				if(!_started)
				{
					if(!isfile)
						file.Write(sLConsole.GetString("Started time: [{0}]\n"), time.ToString("yyyy. MM. dd. HH:mm:ss"));
					else
						file.Write(sLConsole.GetString("\nStarted time: [{0}]\n"), time.ToString("yyyy. MM. dd. HH:mm:ss"));
				}
				else
				{
					string oldlogfile = sUtilities.DirectoryToSpecial(oldlogdirectory, oldfile);
					var ofile = new StreamWriter(oldlogfile, true) { AutoFlush = true };
					ofile.Write(sLConsole.GetString("The log's file name changed. From now in it will be here: {0}\n"), logfile);
					ofile.Close();

					if(!isfile)
						file.Write(sLConsole.GetString("The log file's name changed. {0} was the old. From now on the logs will be here. Change's time: [{1}]\n"), oldfile, time.ToString("yyyy. MM. dd. HH:mm:ss"));
					else
						file.Write(sLConsole.GetString("\nThe log file's name changed. {0} was the old. From now on the logs will be here. Change's time: [{1}]\n"), oldfile, time.ToString("yyyy. MM. dd. HH:mm:ss"));
				}


				file.Close();
			}

			if(!_started)
				_started = true;
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
				if(_ColorblindMode)
				{
					ColorblindMode(source, format, LogType.Notice);
					return;
				}
				
				WaitOutputRedirected();
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.Write(GetTime());
				Console.ForegroundColor = ConsoleColor.White;
				Console.Write(" N {0}: ", source);
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.Write("{0}\n", format);
				LogInFile("N {0}: {1}", source, format);
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
				if(_ColorblindMode)
				{
					ColorblindMode(source, format, LogType.Success);
					return;
				}
				
				WaitOutputRedirected();
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.Write(GetTime());
				Console.ForegroundColor = ConsoleColor.Green;
				Console.Write(" S");
				Console.ForegroundColor = ConsoleColor.White;
				Console.Write(" {0}: ", source);
				Console.ForegroundColor = ConsoleColor.Green;
				Console.Write("{0}\n", format);
				Console.ForegroundColor = ConsoleColor.Gray;
				LogInFile("S {0}: {1}", source, format);
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
				if(LogConfig.LogLevel < 1)
					return;

				if(_ColorblindMode)
				{
					ColorblindMode(source, format, LogType.Warning);
					return;
				}
				
				WaitOutputRedirected();
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.Write(GetTime());
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.Write(" W");
				Console.ForegroundColor = ConsoleColor.White;
				Console.Write(" {0}: ", source);
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.Write("{0}\n", format);
				Console.ForegroundColor = ConsoleColor.Gray;
				LogInFile("W {0}: {1}", source, format);
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
				if(LogConfig.LogLevel < 2)
					return;

				if(_ColorblindMode)
				{
					ColorblindMode(source, format, LogType.Error);
					return;
				}
				
				WaitOutputRedirected();
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.Error.Write(GetTime());
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Error.Write(" E");
				Console.ForegroundColor = ConsoleColor.White;
				Console.Error.Write(" {0}: ", source);
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Error.Write("{0}\n", format);
				Console.ForegroundColor = ConsoleColor.Gray;
				LogInFile("E {0}: {1}", source, format);
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
				if(LogConfig.LogLevel < 3)
					return;

				if(_ColorblindMode)
				{
					ColorblindMode(source, format, LogType.Debug);
					return;
				}

				WaitOutputRedirected();
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.Write(GetTime());
				Console.ForegroundColor = ConsoleColor.Blue;
				Console.Write(" D");
				Console.ForegroundColor = ConsoleColor.White;
				Console.Write(" {0}: ", source);
				Console.ForegroundColor = ConsoleColor.Blue;
				Console.Write("{0}\n", format);
				Console.ForegroundColor = ConsoleColor.Gray;
				LogInFile("D {0}: {1}", source, format);
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
				if(_ColorblindMode)
				{
					ColorblindMode(message);
					return;
				}

				WaitOutputRedirected();
				var sp = message.Split(SchumixBase.NewLine);
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
							Console.Write(SchumixBase.Space);
						
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
				if(_ColorblindMode)
				{
					ColorblindMode(message);
					return;
				}

				WaitOutputRedirected();
				var sp = message.Split(SchumixBase.NewLine);
				var lines = new List<string>(50);

				foreach(string s in sp)
				{
					if(!string.IsNullOrEmpty(s))
						lines.Add(s);
				}

				Console.ForegroundColor = ConsoleColor.Red;
				Console.Error.WriteLine();
				Console.Error.WriteLine("**************************************************"); // 51
				
				foreach(string item in lines)
				{
					uint len = (uint)item.Length;
					uint diff = (48-len);
					Console.Error.Write("* {0}", item);

					if(diff > 0)
					{
						for(uint u = 1; u < diff; ++u)
							Console.Error.Write(SchumixBase.Space);
						
						Console.Error.Write("*\n");
					}
				}
				
				Console.Error.WriteLine("**************************************************");
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

		public static void ColorblindMode(string message)
		{
			lock(WriteLock)
			{
				WaitOutputRedirected();
				var sp = message.Split(SchumixBase.NewLine);
				var lines = new List<string>(50);

				foreach(string s in sp)
				{
					if(!string.IsNullOrEmpty(s))
						lines.Add(s);
				}

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
							Console.Write(SchumixBase.Space);
						
						Console.Write("*\n");
					}
				}
				
				Console.WriteLine("**************************************************");
			}
		}

		public static void ColorblindMode(StringBuilder message)
		{
			lock(WriteLock)
			{
				ColorblindMode(message.ToString());
			}
		}

		public static void ColorblindMode(string source, string format, LogType type = LogType.Notice)
		{
			lock(WriteLock)
			{
				WaitOutputRedirected();

				if(type == LogType.Error)
				{
					Console.Error.Write(GetTime());
					Console.Error.Write(" {0} {1}: ", GetTypeCharacter(type), source);
					Console.Error.Write("{0}\n", format);
				}
				else
				{
					Console.Write(GetTime());
					Console.Write(" {0} {1}: ", GetTypeCharacter(type), source);
					Console.Write("{0}\n", format);
				}

				LogInFile("{0} {1}: {2}", GetTypeCharacter(type), source, format);
			}
		}

		public static void ColorblindMode(string source, StringBuilder format, LogType type = LogType.Notice)
		{
			lock(WriteLock)
			{
				ColorblindMode(source, format.ToString(), type);
			}
		}

		public static void Write(StringBuilder message)
		{
			lock(WriteLock)
			{
				Write(message.ToString());
			}
		}

		// No log in file
		public static void Write(string message)
		{
			lock(WriteLock)
			{
				WaitOutputRedirected();
				Console.Write(message);
			}
		}

		public static void Write(string message, params object[] args)
		{
			lock(WriteLock)
			{
				Write(string.Format(message, args));
			}
		}

		public static void WriteLine(StringBuilder message)
		{
			lock(WriteLock)
			{
				WriteLine(message.ToString());
			}
		}

		public static void WriteLine(string message = "")
		{
			lock(WriteLock)
			{
				WaitOutputRedirected();

				if(message.IsNullOrEmpty())
					Console.WriteLine(message);
				else
					Console.WriteLine("{0} {1}", GetTime(), message);

				if(_started)
					LogInFile(message);
			}
		}

		public static void WriteLine(string message, params object[] args)
		{
			lock(WriteLock)
			{
				WriteLine(string.Format(message, args));
			}
		}

		private static void WaitOutputRedirected()
		{
#if false
			lock(WriteLock)
			{
				if(Console.IsOutputRedirected)
				{
					while(Console.IsOutputRedirected)
					{
						if(!Console.IsOutputRedirected)
							break;

						Thread.Sleep(100);
					}
				}
			}
#endif
		}
	}
}