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
using System.Threading;
using System.Diagnostics;
using System.Globalization;
using Schumix.Irc;
using Schumix.Updater;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Options;
using Schumix.Framework.Platforms;
using Schumix.Framework.Exceptions;
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
		/// <summary>
		///     Hozzáférést biztosít singleton-on keresztül a megadott class-hoz.
		///     A hibákat lehet feljegyezni egy olyan fájlba melyből az infórmációk alapján feldolgozhatók a hibák. Főként Visual Studioval lehet használni.
		/// </summary>
		private static readonly CrashDumper sCrashDumper = Singleton<CrashDumper>.Instance;
		/// <summary>
		///     Hozzáférést biztosít singleton-on keresztül a megadott class-hoz.
		///     Utilities sokféle függvényt tartalmaz melyek hasznosak lehetnek.
		/// </summary>
		private static readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private static readonly Platform sPlatform = Singleton<Platform>.Instance;
		/// <summary>
		///     Hozzáférést biztosít singleton-on keresztül a megadott class-hoz.
		///     Futással kapcsolatos információkat táról.
		/// </summary>
		private static readonly Runtime sRuntime = Singleton<Runtime>.Instance;
		/// <summary>
		///     Hozzáférést biztosít singleton-on keresztül a megadott class-hoz.
		///     Csak windows alatt müködő függvények.
		/// </summary>
		private static readonly Windows sWindows = Singleton<Windows>.Instance;
		private static readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		/// <summary>
		///     Hozzáférést biztosít singleton-on keresztül a megadott class-hoz.
		///     Csak linux alatt müködő függvények.
		/// </summary>
		private static readonly Linux sLinux = Singleton<Linux>.Instance;

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
			bool help = false;
			string configdir = "Configs";
			string configfile = "Schumix.yml";
			string console_encoding = Encoding.UTF8.BodyName;
			string localization = "start";
			bool serverenabled = false;
			int serverport = -1;
			string serverhost = "0.0.0.0";
			string serverpassword = "0";
			string serveridentify = string.Empty;
			bool colorbindmode = false;
			System.Console.BackgroundColor = ConsoleColor.Black;
			System.Console.ForegroundColor = ConsoleColor.Gray;

			var os = new OptionSet()
			{
				{ "h|?|help", "Display help.", v => help = true },
				{ "config-dir=", "Set up the config folder's path and name.", v => configdir = v },
				{ "config-file=", "Set up the config file's place.", v => configfile = v },
				{ "console-encoding=", "Set up the program's character encoding.", v => console_encoding = v },
				{ "console-localization=", "Set up the program's console language settings.", v => localization  = v },
				{ "server-enabled=", "Premition to join the server.", v => serverenabled = Convert.ToBoolean(v) },
				{ "server-host=", "Set server host.", v => serverhost = v },
				{ "server-port=", "Set server port.", v => serverport = v.ToNumber(-1).ToInt() },
				{ "server-password=", "Set password.", v => serverpassword = v },
				{ "server-identify=", "Set identify.", v => serveridentify = v },
				{ "colorbind-mode=", "Set colorbind.", v => colorbindmode = Convert.ToBoolean(v) },
			};

			try
			{
				os.Parse(args);

				if(help)
				{
					ShowHelp(os);
					return;
				}
			}
			catch(OptionException oe)
			{
				System.Console.WriteLine("{0} for options '{1}'", oe.Message, oe.OptionName);
				return;
			}

			if(!console_encoding.IsNumber())
				System.Console.OutputEncoding = Encoding.GetEncoding(console_encoding);
			else
				System.Console.OutputEncoding = Encoding.GetEncoding(Convert.ToInt32(console_encoding));

			sLConsole.SetLocale(localization);
			System.Console.Title = SchumixBase.Title;

			if(colorbindmode)
				System.Console.ForegroundColor = ConsoleColor.Gray;
			else
				System.Console.ForegroundColor = ConsoleColor.Blue;

			System.Console.WriteLine("[Schumix2]");
			System.Console.WriteLine(sLConsole.GetString("To shut down the program use the <Ctrl+C> or the <quit> command!"));
			System.Console.WriteLine(sLConsole.GetString("Schumix Version: {0}"), sUtilities.GetVersion());
			System.Console.WriteLine(sLConsole.GetString("Website: {0}"), Consts.SchumixWebsite);
			System.Console.WriteLine(sLConsole.GetString("Programmed by: {0}"), Consts.SchumixProgrammedBy);
			System.Console.WriteLine(sLConsole.GetString("Developers: {0}"), Consts.SchumixDevelopers);
			System.Console.WriteLine("================================================================================"); // 80
			System.Console.ForegroundColor = ConsoleColor.Gray;
			System.Console.WriteLine();

			new Config(configdir, configfile, colorbindmode);
			sUtilities.CreatePidFile(SchumixConfig.ConfigFile);

			if(!serveridentify.IsNullOrEmpty())
				SchumixBase.ServerIdentify = serveridentify;

			if(localization == "start")
				sLConsole.SetLocale(LocalizationConfig.Locale);
			else if(localization != "start")
				sLConsole.SetLocale(localization);

			if(sPlatform.GetPlatformType() == PlatformType.Windows && console_encoding == Encoding.UTF8.BodyName &&
			   CultureInfo.CurrentCulture.Name == "hu-HU" && sLConsole.Locale == "huHU")
				System.Console.OutputEncoding = Encoding.GetEncoding(852);

			new ServerConfig(serverenabled ? serverenabled : ServerConfig.Enabled, serverhost != "0.0.0.0" ? serverhost : ServerConfig.Host,
				serverport != -1 ? serverport : ServerConfig.Port, serverpassword != "0" ? serverpassword : ServerConfig.Password);

			System.Console.WriteLine();
			Log.Notice("Main", sLConsole.GetString("System is starting..."));

			if(colorbindmode)
				Log.Notice("Main", sLConsole.GetString("Colorblind mode is on!"));

			if(sPlatform.GetPlatformType() == PlatformType.Windows)
				sWindows.Init();
			else if(sPlatform.GetPlatformType() == PlatformType.Linux)
				sLinux.Init();

			AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
			{
				Log.LargeError(sLConsole.GetString("FATAL ERROR"));
				Log.Error("Main", sLConsole.GetString("An unhandled exception has been thrown. ({0})"), (eventArgs.ExceptionObject as Exception).Message);
				sCrashDumper.CreateCrashDump(eventArgs.ExceptionObject);
				Shutdown("Crash.", true);
			};

			if(!ServerConfig.Enabled)
				new Update(SchumixConfig.ConfigDirectory);

			sUtilities.CleanHomeDirectory();
			new SchumixBot();
		}

		/// <summary>
		///     Segítséget nyújt a kapcsolokhoz.
		/// </summary>
		private static void ShowHelp(OptionSet os)
		{
			System.Console.WriteLine("[Schumix2] Version: {0}", sUtilities.GetVersion());
			System.Console.WriteLine("Options:");
			os.WriteOptionDescriptions(System.Console.Out);
		}

		public static void Shutdown(string Message, bool Crash = false)
		{
			if(!SchumixBot.sSchumixBase.IsNull())
			{
				bool e = false;
				foreach(var nw in sIrcBase.Networks)
				{
					if(!sIrcBase.Networks[nw.Key].IsNull() && sIrcBase.Networks[nw.Key].Online)
						e = true;
				}

				if(e)
					SchumixBase.Quit();
				else
					Process.GetCurrentProcess().Kill();
			}
			else
				Process.GetCurrentProcess().Kill();

			if(Crash && SchumixBase.ExitStatus)
				return;

			sIrcBase.Shutdown(Message);
		}
	}
}