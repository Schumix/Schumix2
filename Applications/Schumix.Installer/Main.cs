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
using System.Text;
using System.Globalization;
using Schumix.Installer.Config;
using Schumix.Installer.Logger;
using Schumix.Installer.Options;
using Schumix.Installer.Platforms;
using Schumix.Installer.Exceptions;
using Schumix.Installer.Extensions;
using Schumix.Installer.Localization;

namespace Schumix.Installer
{
	class MainClass
	{
		private static readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private static readonly CrashDumper sCrashDumper = Singleton<CrashDumper>.Instance;
		private static readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private static readonly Platform sPlatform = Singleton<Platform>.Instance;
		private static readonly Runtime sRuntime = Singleton<Runtime>.Instance;
		private static readonly Windows sWindows = Singleton<Windows>.Instance;
		private static readonly Linux sLinux = Singleton<Linux>.Instance;

		/// <summary>
		///     A Main függvény. Itt indul el a program.
		/// </summary>
		public static void Main(string[] args)
		{
			sRuntime.SetProcessName("Installer");
			bool help = false;
			string console_encoding = Encoding.UTF8.BodyName;
			string localization = "start";
			string dumpsdir = "Dumps";
			Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = ConsoleColor.Gray;

			var os = new OptionSet()
			{
				{ "h|?|help", "Display help.", v => help = true },
				{ "console-encoding=", "Set up the program's character encoding.", v => console_encoding = v },
				{ "console-localization=", "Set up the program's console language settings.", v => localization  = v },
				{ "dumps-dir=", "Set up the dumps folder's path and name.", v => dumpsdir = v },
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
				Console.WriteLine("{0} for options '{1}'", oe.Message, oe.OptionName);
				return;
			}

			if(!console_encoding.IsNumber())
				Console.OutputEncoding = Encoding.GetEncoding(console_encoding);
			else
				Console.OutputEncoding = Encoding.GetEncoding(console_encoding.ToInt32());

			if(localization != "start")
				sLConsole.SetLocale(localization);

			sCrashDumper.SetDirectory(dumpsdir);

			Console.Title = "Schumix2 Installer";
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("[Installer]");
			Console.WriteLine(sLConsole.GetString("To shut down the program use the <Ctrl+C> command!"));
			Console.WriteLine(sLConsole.GetString("Installer Version: {0}"), sUtilities.GetVersion());
			Console.WriteLine(sLConsole.GetString("Website: {0}"), Consts.InstallerWebsite);
			Console.WriteLine(sLConsole.GetString("Programmed by: {0}"), Consts.InstallerProgrammedBy);
			Console.WriteLine("================================================================================"); // 80
			Console.ForegroundColor = ConsoleColor.Gray;
			Log.Initialize("Installer.log");

			if(sPlatform.IsWindows && console_encoding == Encoding.UTF8.BodyName &&
			   CultureInfo.CurrentCulture.Name == "hu-HU" && sLConsole.Locale == "huHU")
				Console.OutputEncoding = Encoding.GetEncoding(852);

			Console.WriteLine();
			Log.Notice("Main", sLConsole.GetString("System is starting..."));

			if(sPlatform.IsWindows)
				sWindows.Init();
			else if(sPlatform.IsLinux)
				sLinux.Init();

			AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
			{
				Log.LargeError(sLConsole.GetString("FATAL ERROR"));
				Log.Error("Main", sLConsole.GetString("An unhandled exception has been thrown. ({0})"), (eventArgs.ExceptionObject as Exception).Message);
				sCrashDumper.CreateCrashDump(eventArgs.ExceptionObject);
				Shutdown();
			};

			new InstallerBase();
		}

		/// <summary>
		///     Segítséget nyújt a kapcsolokhoz.
		/// </summary>
		private static void ShowHelp(OptionSet os)
		{
			Console.WriteLine("[Installer] Version: {0}", sUtilities.GetVersion());
			Console.WriteLine("Options:");
			os.WriteOptionDescriptions(Console.Out);
		}

		public static void Shutdown()
		{
			Console.CursorVisible = true;
			sRuntime.Exit();
		}
	}
}