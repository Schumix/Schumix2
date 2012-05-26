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
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Diagnostics;
using System.Globalization;
using Schumix.Updater;
using Schumix.Framework;
using Schumix.Framework.Client;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using Schumix.Server.Config;

namespace Schumix.Server
{
	class MainClass
	{
		private static readonly ServerPacketHandler sServerPacketHandler = Singleton<ServerPacketHandler>.Instance;
		private static readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private static readonly CrashDumper sCrashDumper = Singleton<CrashDumper>.Instance;
		private static readonly New.Schumix sSchumix = Singleton<New.Schumix>.Instance;
		private static readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private static readonly Runtime sRuntime = Singleton<Runtime>.Instance;
		private static readonly Windows sWindows = Singleton<Windows>.Instance;
		private static readonly Linux sLinux = Singleton<Linux>.Instance;
		public static ServerListener sListener { get; private set; }

		/// <summary>
		///     A Main függvény. Itt indul el a program.
		/// </summary>
		private static void Main(string[] args)
		{
			sRuntime.SetProcessName("Server");
			string configdir = "Configs";
			string configfile = "Server.xml";
			string console_encoding = "utf-8";
			string localization = "start";
			System.Console.CursorVisible = false;
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
			}

			if(!console_encoding.IsNumber())
				System.Console.OutputEncoding = Encoding.GetEncoding(console_encoding);
			else
				System.Console.OutputEncoding = Encoding.GetEncoding(Convert.ToInt32(console_encoding));

			sLConsole.Locale = localization;
			System.Console.Title = "Schumix2 Server";
			System.Console.ForegroundColor = ConsoleColor.Blue;
			System.Console.WriteLine("[Server]");
			System.Console.WriteLine(sLConsole.MainText("StartText"));
			System.Console.WriteLine(sLConsole.MainText("StartText2"), sUtilities.GetVersion());
			System.Console.WriteLine(sLConsole.MainText("StartText2-2"), Consts.SchumixWebsite);
			System.Console.WriteLine(sLConsole.MainText("StartText2-3"), Consts.SchumixProgrammedBy);
			System.Console.WriteLine(sLConsole.MainText("StartText2-4"), Consts.SchumixDevelopers);
			System.Console.WriteLine("================================================================================"); // 80
			System.Console.ForegroundColor = ConsoleColor.Gray;
			System.Console.WriteLine();

			new Server.Config.Config(configdir, configfile);
			sUtilities.CreatePidFile(Server.Config.ServerConfig.ConfigFile);

			if(localization == "start")
				sLConsole.Locale = Server.Config.LocalizationConfig.Locale;
			else if(localization != "start")
				sLConsole.Locale = localization;

			if(sUtilities.GetPlatformType() == PlatformType.Windows && console_encoding == "utf-8" &&
			   CultureInfo.CurrentCulture.Name == "hu-HU" && sLConsole.Locale == "huHU")
				System.Console.OutputEncoding = Encoding.GetEncoding(852);

			Log.Notice("Main", sLConsole.MainText("StartText3"));

			new Update();

			if(File.Exists("Config.exe"))
				File.Delete("Config.exe");

			if(File.Exists("Installer.exe"))
				File.Delete("Installer.exe");

			if(sUtilities.GetPlatformType() == PlatformType.Windows)
				sWindows.Init();
			else if(sUtilities.GetPlatformType() == PlatformType.Linux)
				sLinux.Init();

			AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
			{
				sUtilities.RemovePidFile();
				sListener.Exit = true;
				System.Console.CursorVisible = true;
				Log.Error("Main", sLConsole.MainText("StartText4"), eventArgs.ExceptionObject as Exception);
				sCrashDumper.CreateCrashDump(eventArgs.ExceptionObject);
				var packet = new SchumixPacket();
				packet.Write<int>((int)Opcode.SMSG_CLOSE_CONNECTION);
				packet.Write<int>((int)0);

				foreach(var list in sServerPacketHandler.HostList)
					sServerPacketHandler.SendPacketBack(packet, list.Value, list.Key.Split(SchumixBase.Colon)[0], Convert.ToInt32(list.Key.Split(SchumixBase.Colon)[1]));

				Thread.Sleep(2000);
				KillAllSchumixProccess();
			};

			sListener = new ServerListener(ServerConfigs.ListenerPort);
			sListener.Listen();
		}

		/// <summary>
		///     Segítséget nyújt a kapcsolokhoz.
		/// </summary>
		private static void Help()
		{
			System.Console.WriteLine("[Server] Version: {0}", sUtilities.GetVersion());
			System.Console.WriteLine("Options:");
			System.Console.WriteLine("\t-h, --help\t\t\tShow help");
			System.Console.WriteLine("\t--config-dir=<dir>\t\tSet up the config folder's path and 'name");
			System.Console.WriteLine("\t--config-file=<file>\t\tSet up the config file's place");
			System.Console.WriteLine("\t--console-encoding=Value\tSet up the program's character encoding");
			System.Console.WriteLine("\t--console-localization=Value\tSet up the program's console language settings");
		}

		public static void KillAllSchumixProccess()
		{
			foreach(var list in sSchumix._processlist)
			{
				if(list.Value.Process.IsNull())
					continue;

				if(sUtilities.GetPlatformType() == PlatformType.Linux)
				{
					foreach(var p in Process.GetProcessesByName("mono"))
					{
						if(p.Id == list.Value.Process.Id)
							p.Kill();
					}
				}
				else if(sUtilities.GetPlatformType() == PlatformType.Windows)
				{
					foreach(var p in Process.GetProcessesByName("Schumix"))
					{
						if(p.Id == list.Value.Process.Id)
							p.Kill();
					}
				}
			}
		}
	}
}