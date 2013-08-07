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
using System.Net;
using System.Threading;
using System.Diagnostics;
using Schumix.Installer.Clean;
using Schumix.Installer.CopyTo;
using Schumix.Installer.Logger;
using Schumix.Installer.Compiler;
using Schumix.Installer.Download;
using Schumix.Installer.Platforms;
using Schumix.Installer.Extensions;
using Schumix.Installer.Localization;

namespace Schumix.Installer
{
	class MainClass
	{
		private static readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private static readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private static readonly Platform sPlatform = Singleton<Platform>.Instance;
		private static readonly Runtime sRuntime = Singleton<Runtime>.Instance;
		private const string GitUrl = "https://github.com/Schumix/Schumix2";
		private const string _dir = "Schumix2";

		/// <summary>
		///     A Main függvény. Itt indul el a program.
		/// </summary>
		public static void Main(string[] args)
		{
			sRuntime.SetProcessName("Installer");
			Console.Title = "Schumix2 Installer";
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("[Installer]");
			Console.WriteLine(sLConsole.GetString("To shut down the program use the <Ctrl+C> command!"));
			Console.WriteLine(sLConsole.GetString("Installer Version: {0}"), sUtilities.GetVersion());
			Console.WriteLine("================================================================================"); // 80
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.WriteLine();
			Log.Initialize("Installer.log");

			if(sPlatform.IsLinux)
				ServicePointManager.ServerCertificateValidationCallback += (s,ce,ca,p) => true;

			WebRequest.DefaultWebProxy = null;

			Log.Notice("Installer", sLConsole.GetString("Installer started."));
			string url = GitUrl.Remove(0, "http://".Length, "http://");
			url = url.Remove(0, "https://".Length, "https://");
			string version = sUtilities.GetUrl("https://raw." + url + "/stable" +
			                                   "/Core/Schumix.Framework/Config/Consts.cs");
			version = version.Remove(0, version.IndexOf("SchumixVersion = \"") + "SchumixVersion = \"".Length);
			version = version.Substring(0, version.IndexOf("\";"));

			try
			{
				new CloneSchumix("git://" + url, _dir);
				Log.Success("Installer", sLConsole.GetString("Successfully downloaded new version."));
			}
			catch
			{
				Log.Error("Installer", sLConsole.GetString("Downloading unsuccessful."));
				Log.Warning("Installer", sLConsole.GetString("Installation successful!"));
				Thread.Sleep(5*1000);
				Environment.Exit(1);
			}

			Log.Notice("Installer", sLConsole.GetString("Started translating."));
			var build = new Build(_dir);

			if(build.HasError)
			{
				Log.Error("Installer", sLConsole.GetString("Error was handled while translated."));
				Log.Warning("Installer", sLConsole.GetString("Installation successful!"));
				Thread.Sleep(5*1000);
				Environment.Exit(1);
			}

			Log.Success("Installer", sLConsole.GetString("Successfully finished the translation."));
			Log.Notice("Installer", sLConsole.GetString("Copy files."));
			new Copy(_dir);
			Log.Notice("Installer", sLConsole.GetString("Remove unneeded datas."));

			try
			{
				new DirectoryClean(_dir);
			}
			catch(Exception e)
			{
				Log.Warning("Installer", e.Message);
			}

			Log.Success("Installer", sLConsole.GetString("The installation is finished. The program shutting down!"));
			Environment.Exit(0);
		}
	}
}