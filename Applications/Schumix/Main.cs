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
using System.Text;
using Schumix.Irc;
using Schumix.Updater;
using Schumix.Framework;
using Schumix.Framework.Config;
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
		/// <summary>
		///     Hozzáférést biztosít singleton-on keresztül a megadott class-hoz.
		///     Utilities sokféle függvényt tartalmaz melyek hasznosak lehetnek.
		/// </summary>
		private static readonly Utilities sUtilities = Singleton<Utilities>.Instance;
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
			Runtime.SetProcessName("Schumix");
			string configdir = "Configs";
			string configfile = "Schumix.xml";
			string console_encoding = "utf-8";
			string localization = "start";
			bool serverenabled = false;
			int serverport = -1;
			string serverhost = "0.0.0.0";
			string serverpassword = "0";
			System.Console.BackgroundColor = ConsoleColor.Black;
			System.Console.ForegroundColor = ConsoleColor.Gray;

			for(int i = 0; i < args.Length; i++)
			{
				string arg = args[i];

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
			}

			double Num;
			bool isNum = double.TryParse(console_encoding, out Num);

			if(!isNum)
				System.Console.OutputEncoding = Encoding.GetEncoding(console_encoding);
			else
				System.Console.OutputEncoding = Encoding.GetEncoding(Convert.ToInt32(Num)); // Magyar karakterkódolás windows xp-n: 852

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

			new Config(configdir, configfile);

			if(localization == "start")
				sLConsole.Locale = LocalizationConfig.Locale;
			else if(localization != "start")
				sLConsole.Locale = localization;

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
			System.Console.CancelKeyPress += (sender, e) =>
			{
				sSender.Quit("Daemon killed.");
				SchumixBase.timer.SaveUptime();
				SchumixBase.ServerDisconnect();
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
		}
	}
}