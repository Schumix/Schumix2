/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Megax <http://www.megaxx.info/>
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
using System.Globalization;
using Mono.Unix;
using Mono.Unix.Native;
using Schumix.Irc;
using Schumix.Updater;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix
{
	/// <summary>
	///     Main class.
	/// </summary>
	class MainClass
	{
		/// <summary>
		///     Hozzáférést biztosít singleton-on keresztül a megadott class-hoz.
		///     LocalizationConsole segítségével állíthatók be a konzol nyelvi tulajdonságai.
		/// </summary>
		private static readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private static readonly CrashDumper sCrashDumper = Singleton<CrashDumper>.Instance;
		/// <summary>
		///     Hozzáférést biztosít singleton-on keresztül a megadott class-hoz.
		///     Utilities sokféle függvényt tartalmaz melyek hasznosak lehetnek.
		/// </summary>
		private static readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private static readonly Runtime sRuntime = Singleton<Runtime>.Instance;
		/// <summary>
		///     Hozzáférést biztosít singleton-on keresztül a megadott class-hoz.
		///     Üzenet küldés az irc szerver felé.
		/// </summary>
		private static readonly Sender sSender = Singleton<Sender>.Instance;

		/// <summary>
		///     A Main függvény. Itt indul el a program.
		/// </summary>
		/// <remarks>
		///     Schumix2 IRC bot
		///     <para>
		///         Készítette Megaxxx és Jackneill.
		///     </para>
		/// </remarks>
		private static void Main(string[] args)
		{
			sRuntime.SetProcessName("Schumix");
			string s = string.Empty;
			string configdir = "Configs";
			string configfile = "Schumix.xml";
			string console_encoding = "utf-8";
			string localization = "start";
			bool serverenabled = false;
			int serverport = -1;
			string serverhost = "0.0.0.0";
			string serverpassword = "0";
			string serverconfig = string.Empty;
			string serveridentify = string.Empty;
			System.Console.BackgroundColor = ConsoleColor.Black;
			System.Console.ForegroundColor = ConsoleColor.Gray;

			for(int i = 0; i < args.Length; i++)
			{
				string arg = args[i];
				s += SchumixBase.Space + arg;

				if(arg == "-h" || arg == "--help")
				{
					Help();
					return;
				}
				else if(arg.Contains("--config-dir="))
				{
					configdir = arg.Substring(arg.IndexOf("=")+1);
					continue;
				}
				else if(arg.Contains("--config-file="))
				{
					configfile = arg.Substring(arg.IndexOf("=")+1);
					continue;
				}
				else if(arg.Contains("--console-encoding="))
				{
					console_encoding = arg.Substring(arg.IndexOf("=")+1);
					continue;
				}
				else if(arg.Contains("--console-localization="))
				{
					localization = arg.Substring(arg.IndexOf("=")+1);
					continue;
				}
				else if(arg.Contains("--server-enabled="))
				{
					serverenabled = Convert.ToBoolean(arg.Substring(arg.IndexOf("=")+1));
					continue;
				}
				else if(arg.Contains("--server-host="))
				{
					serverhost = arg.Substring(arg.IndexOf("=")+1);
					continue;
				}
				else if(arg.Contains("--server-port="))
				{
					serverport = Convert.ToInt32(arg.Substring(arg.IndexOf("=")+1));
					continue;
				}
				else if(arg.Contains("--server-password="))
				{
					serverpassword = arg.Substring(arg.IndexOf("=")+1);
					continue;
				}
				else if(arg.Contains("--server-identify="))
				{
					serveridentify = arg.Substring(arg.IndexOf("=")+1);
					continue;
				}
			}

			s = s.Remove(0, 1, SchumixBase.Space);

			if(s.Contains("--server-configs="))
			{
				serverconfig = s.Remove(0, s.IndexOf("--server-configs=") + "--server-configs=".Length);

				if(serverconfig.Contains("; "))
					serverconfig = serverconfig.Substring(0, serverconfig.IndexOf("; "));
				else
					serverconfig = serverconfig.Substring(0, serverconfig.Length);
			}

			if(!console_encoding.IsNumber())
				System.Console.OutputEncoding = Encoding.GetEncoding(console_encoding);
			else
				System.Console.OutputEncoding = Encoding.GetEncoding(Convert.ToInt32(console_encoding));

			sLConsole.Locale = localization;
			System.Console.Title = SchumixBase.Title;
			System.Console.ForegroundColor = ConsoleColor.Blue;
			System.Console.WriteLine("[Schumix2]");
			System.Console.WriteLine(sLConsole.MainText("StartText"));
			System.Console.WriteLine(sLConsole.MainText("StartText2"), sUtilities.GetVersion());
			System.Console.WriteLine(sLConsole.MainText("StartText2-2"), Consts.SchumixWebsite);
			System.Console.WriteLine(sLConsole.MainText("StartText2-3"), Consts.SchumixProgrammedBy);
			System.Console.WriteLine(sLConsole.MainText("StartText2-4"), Consts.SchumixDevelopers);
			System.Console.WriteLine("================================================================================"); // 80
			System.Console.ForegroundColor = ConsoleColor.Gray;
			System.Console.WriteLine();

			if(serverconfig != string.Empty && serverenabled)
				new Config(serverconfig.Split(';'));
			else
				new Config(configdir, configfile);

			if(serveridentify != string.Empty)
				SchumixBase.ServerIdentify = serveridentify;

			if(localization == "start")
				sLConsole.Locale = LocalizationConfig.Locale;
			else if(localization != "start")
				sLConsole.Locale = localization;

			if(sUtilities.GetCompiler() == Compiler.VisualStudio && console_encoding == "utf-8" &&
			   CultureInfo.CurrentCulture.Name == "hu-HU" && sLConsole.Locale == "huHU")
				System.Console.OutputEncoding = Encoding.GetEncoding(852);

			new ServerConfig(serverenabled ? serverenabled : ServerConfig.Enabled, serverhost != "0.0.0.0" ? serverhost : ServerConfig.Host,
				serverport != -1 ? serverport : ServerConfig.Port, serverpassword != "0" ? serverpassword : ServerConfig.Password);

			Log.Notice("Main", sLConsole.MainText("StartText3"));

			if(!ServerConfig.Enabled)
				new Update();

			if(File.Exists("Config.exe"))
				File.Delete("Config.exe");

			if(File.Exists("Installer.exe"))
				File.Delete("Installer.exe");

			new SchumixBot();

			if(sUtilities.GetCompiler() == Compiler.Mono)
				StartHandler();
			else
			{
				System.Console.CancelKeyPress += (sender, e) =>
				{
					SchumixBase.Quit();
					sSender.Quit("Daemon killed.");
					Thread.Sleep(5*1000);
				};
			}

			AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
			{
				Log.Error("Main", sLConsole.MainText("StartText4"), eventArgs.ExceptionObject as Exception);
				sCrashDumper.CreateCrashDump(eventArgs.ExceptionObject);
				SchumixBase.Quit();
				sSender.Quit("Crash.");
				Thread.Sleep(5*1000);
			};
		}

		/// <summary>
		///     Segítséget nyújt a kapcsolokhoz.
		/// </summary>
		private static void Help()
		{
			System.Console.WriteLine("[Schumix2] Version: {0}", sUtilities.GetVersion());
			System.Console.WriteLine("Options:");
			System.Console.WriteLine("\t-h, --help\t\t\tShow help");
			System.Console.WriteLine("\t--config-dir=<dir>\t\tSet up the config folder's path and 'name");
			System.Console.WriteLine("\t--config-file=<file>\t\tSet up the config file's place");
			System.Console.WriteLine("\t--console-encoding=Value\tSet up the program's character encoding");
			System.Console.WriteLine("\t--console-localization=Value\tSet up the program's console language settings");
			System.Console.WriteLine("\t--server-enabled=Value\tPremition to join the server.");
			System.Console.WriteLine("\t--server-host=<host>\tSet server host.");
			System.Console.WriteLine("\t--server-port=<port>\tSet server port.");
			System.Console.WriteLine("\t--server-password=<pass>\tSet password.");
			System.Console.WriteLine("\t--server-identify=Value\tSet identify.");
			System.Console.WriteLine("\t--server-configs=Value\tSend Schumix's parameters at all.");
		}

		private static void StartHandler()
		{
			new Thread(TerminateHandler).Start();
		}

		private static void TerminateHandler()
		{
			Log.Notice("Main", "Initializing Handler for SIGINT");
			var signal = new UnixSignal(Signum.SIGINT);
			signal.WaitOne();

			Log.Notice("Main", "Handler Terminated");
			SchumixBase.Quit();
			sSender.Quit("Daemon killed.");
			Thread.Sleep(5*1000);
		}
	}
}