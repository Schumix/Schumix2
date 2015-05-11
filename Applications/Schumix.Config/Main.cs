/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013-2015 Schumix Team <http://schumix.eu/>
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
using System.Reflection;
using Schumix.Config.Util;
using Schumix.Config.Config;
using Schumix.Config.Logger;
using Schumix.Config.Options;
using Schumix.Config.Platforms;
using Schumix.Config.Exceptions;
using Schumix.Config.Extensions;

namespace Schumix.Config
{
	class MainClass
	{
		private static readonly CrashDumper sCrashDumper = Singleton<CrashDumper>.Instance;
		private static readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private static readonly Platform sPlatform = Singleton<Platform>.Instance;
		private static readonly Runtime sRuntime = Singleton<Runtime>.Instance;

		/// <summary>
		///     A Main függvény. Itt indul el a program.
		/// </summary>
		public static void Main(string[] args)
		{
			sRuntime.SetProcessName(Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().ManifestModule.Name));
			bool help = false;
			string console_encoding = Encoding.UTF8.BodyName;
			string dumpsdir = "Dumps";
			string logsdir = "Logs";
			int loglevel = 3;
			string schumix2dir = "Schumix2";
			string addonsdir = "Addons";
			string configdir = "Configs";
			Console.CursorVisible = false;
			Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = ConsoleColor.Gray;

			var os = new OptionSet()
			{
				{ "h|?|help", "Display help.", v => help = true },
				{ "console-encoding=", "Set up the program's character encoding.", v => console_encoding = v },
				{ "dumps-dir=", "Set up the dumps folder's path and name.", v => dumpsdir = v },
				{ "logs-dir=", "Set up the logs folder's path and name.", v => dumpsdir = v },
				{ "loglevel=", "Log level's setting.", v => loglevel = v.ToInt32() },
				{ "schumix2-dir=", "Set up the Schumix2 folder's path and name.", v => schumix2dir = v },
				{ "addons-dir=", "Set up the addons folder's path and name.", v => addonsdir = v },
				{ "config-dir=", "Set up the config folder's path and name.", v => configdir = v },
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
				Console.Error.WriteLine("{0} for options '{1}'", oe.Message, oe.OptionName);
				return;
			}

			if(!console_encoding.IsNumber())
				Console.OutputEncoding = Encoding.GetEncoding(console_encoding);
			else
				Console.OutputEncoding = Encoding.GetEncoding(console_encoding.ToInt32());

			sCrashDumper.SetDirectory(dumpsdir);

			Console.Title = "Schumix2 Config";
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("[Config]");
			Console.WriteLine("To shut down the program use the <Ctrl+C> command!");
			Console.WriteLine("Config Version: {0}", sUtilities.GetVersion());
			Console.WriteLine("Website: {0}", Consts.ConfigWebsite);
			Console.WriteLine("Programmed by: {0}", Consts.ConfigProgrammedBy);
			Console.WriteLine("================================================================================"); // 80
			Console.ForegroundColor = ConsoleColor.Gray;

			Log.SetLogLevel(loglevel);
			Log.Initialize("Config.log", logsdir);

			if(!sPlatform.IsWindows)
				Console.WriteLine();

			Log.Notice("Main", "System is starting...");

			AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
			{
				Log.LargeError("FATAL ERROR");
				Log.Error("Main", "An unhandled exception has been thrown. ({0})", (eventArgs.ExceptionObject as Exception).Message);
				sCrashDumper.CreateCrashDump(eventArgs.ExceptionObject);
				Shutdown();
			};

			var cbase = new ConfigBase();
			cbase.Clean(schumix2dir, addonsdir, configdir);
			sRuntime.Exit();
		}

		/// <summary>
		///     Segítséget nyújt a kapcsolokhoz.
		/// </summary>
		private static void ShowHelp(OptionSet os)
		{
			Console.WriteLine("[Config] Version: {0}", sUtilities.GetVersion());
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