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
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Globalization;
using Schumix.Framework;
using Schumix.Framework.Clean;
using Schumix.Framework.Logger;
using Schumix.Framework.Config;
using Schumix.Framework.Network;
using Schumix.Framework.Options;
using Schumix.Framework.Platforms;
using Schumix.Framework.Exceptions;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using Schumix.Components.Updater;
using Schumix.Server.Config;

namespace Schumix.Server
{
	class MainClass
	{
		private static readonly ServerPacketHandler sServerPacketHandler = Singleton<ServerPacketHandler>.Instance;
		private static readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private static readonly CrashDumper sCrashDumper = Singleton<CrashDumper>.Instance;
		private static readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private static readonly Platform sPlatform = Singleton<Platform>.Instance;
		private static readonly Runtime sRuntime = Singleton<Runtime>.Instance;
		private static readonly Windows sWindows = Singleton<Windows>.Instance;
		private static readonly Linux sLinux = Singleton<Linux>.Instance;
		public static CleanManager sCleanManager { get; private set; }
		public static ServerListener sListener { get; private set; }

		/// <summary>
		///     A Main függvény. Itt indul el a program.
		/// </summary>
		private static void Main(string[] args)
		{
			sRuntime.SetProcessName("Server");
			bool help = false;
			string configdir = "Configs";
			string configfile = "Server.yml";
			string console_encoding = Encoding.UTF8.BodyName;
			string localization = "start";
			bool colorbindmode = false;
			bool updateignore = false;
			System.Console.CursorVisible = false;
			System.Console.BackgroundColor = ConsoleColor.Black;
			System.Console.ForegroundColor = ConsoleColor.Gray;

			var os = new OptionSet()
			{
				{ "h|?|help", "Display help.", v => help = true },
				{ "config-dir=", "Set up the config folder's path and name.", v => configdir = v },
				{ "config-file=", "Set up the config file's place.", v => configfile = v },
				{ "console-encoding=", "Set up the program's character encoding.", v => console_encoding = v },
				{ "console-localization=", "Set up the program's console language settings.", v => localization  = v },
				{ "colorbind-mode=", "Set colorbind.", v => colorbindmode = v.ToBoolean() },
				{ "update-ignore", "Update ignore.", v => updateignore = true },
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
				System.Console.OutputEncoding = Encoding.GetEncoding(console_encoding.ToInt32());

			System.Console.Title = "Schumix2 Server";

			if(colorbindmode)
				System.Console.ForegroundColor = ConsoleColor.Gray;
			else
				System.Console.ForegroundColor = ConsoleColor.Blue;

			if(localization != "start")
				sLConsole.SetLocale(localization);

			System.Console.WriteLine("[Server]");
			System.Console.WriteLine(sLConsole.GetString("To shut down the program use the <Ctrl+C> or the <quit> command!"));
			System.Console.WriteLine(sLConsole.GetString("Schumix Version: {0}"), sUtilities.GetVersion());
			System.Console.WriteLine(sLConsole.GetString("Website: {0}"), Consts.SchumixWebsite);
			System.Console.WriteLine(sLConsole.GetString("Programmed by: {0}"), Consts.SchumixProgrammedBy);
			System.Console.WriteLine(sLConsole.GetString("Developers: {0}"), Consts.SchumixDevelopers);
			System.Console.WriteLine("================================================================================"); // 80
			System.Console.ForegroundColor = ConsoleColor.Gray;
			System.Console.WriteLine();

			new Server.Config.Config(configdir, configfile, colorbindmode);
			sUtilities.CreatePidFile(Server.Config.ServerConfig.ConfigFile);

			if(localization == "start")
				sLConsole.SetLocale(Server.Config.LocalizationConfig.Locale);

			if(sPlatform.IsWindows && console_encoding == Encoding.UTF8.BodyName &&
			   CultureInfo.CurrentCulture.Name == "hu-HU" && sLConsole.Locale == "huHU")
				System.Console.OutputEncoding = Encoding.GetEncoding(852);

			Log.Notice("Main", sLConsole.GetString("System is starting..."));
			
			if(colorbindmode)
				Log.Notice("Main", sLConsole.GetString("Colorblind mode is on!"));

			Log.Debug("Main", sLConsole.GetString("CleanManager is starting..."));
			sCleanManager = new CleanManager(true);
			sCleanManager.Initialize();

			if(updateignore)
				Log.Warning("Main", sLConsole.GetString("The automatic update is disabled."));
			else
				new Update(Server.Config.ServerConfig.ConfigDirectory);

			sUtilities.CleanHomeDirectory(true);

			if(sPlatform.IsWindows)
				sWindows.Init();
			else if(sPlatform.IsLinux)
				sLinux.Init();

			AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
			{
				if(sListener.Exit)
				{
					Log.LargeError(sLConsole.GetString("FATAL ERROR"));
					Log.Error("Main", sLConsole.GetString("An unhandled exception has been thrown. ({0})"), eventArgs.ExceptionObject as Exception);
					sCrashDumper.CreateCrashDump(eventArgs.ExceptionObject);
					sRuntime.Exit();
				}
				else
					Shutdown(eventArgs.ExceptionObject as Exception);
			};

			sListener = new ServerListener(ServerConfigs.ListenerPort);
			sListener.Listen();
		}

		/// <summary>
		///     Segítséget nyújt a kapcsolokhoz.
		/// </summary>
		private static void ShowHelp(OptionSet os)
		{
			System.Console.WriteLine("[Server] Version: {0}", sUtilities.GetVersion());
			System.Console.WriteLine("Options:");
			os.WriteOptionDescriptions(System.Console.Out);
			System.Console.CursorVisible = true;
		}

		public static void Shutdown(Exception eventArgs = null)
		{
			sUtilities.RemovePidFile();
			sListener.Exit = true;
			System.Console.CursorVisible = true;

			if(!eventArgs.IsNull())
			{
				Log.Error("Main", sLConsole.GetString("An unhandled exception has been thrown. ({0})"), eventArgs.Message);
				sCrashDumper.CreateCrashDump(eventArgs);
			}

			var packet = new SchumixPacket();
			packet.Write<int>((int)Opcode.SMSG_CLOSE_CONNECTION);
			packet.Write<int>((int)0);
			sServerPacketHandler.SendPacketBackAllHost(packet);

			Thread.Sleep(2000);
			sRuntime.Exit();
		}
	}
}