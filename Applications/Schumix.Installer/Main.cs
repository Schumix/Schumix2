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
using System.Threading;
using System.Diagnostics;
using Schumix.Installer.Clean;
using Schumix.Installer.UnZip;
using Schumix.Installer.CopyTo;
using Schumix.Installer.Compiler;
using Schumix.Installer.Download;
using Schumix.Installer.Localization;

namespace Schumix.Installer
{
	class MainClass
	{
		private static readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private static readonly Utilities sUtilities = Singleton<Utilities>.Instance;

		/// <summary>
		///     A Main függvény. Itt indul el a program.
		/// </summary>
		public static void Main(string[] args)
		{
			Console.Title = "Schumix2 Installer";
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("[Installer]");
			Console.WriteLine("================================================================================"); // 80
			Console.ForegroundColor = ConsoleColor.Gray;
			//Console.WriteLine();
			Log.Initialize("Installer.log");

			if(sUtilities.GetPlatformType() == PlatformType.Linux)
				System.Net.ServicePointManager.ServerCertificateValidationCallback += (s,ce,ca,p) => true;

			Log.Notice("Installer", sLConsole.Installer("Text2"));
			string version = sUtilities.GetUrl("https://github.com/megax/Schumix2/tags");
			version = version.Remove(0, version.IndexOf("<span class=\"alt-download-links\">"));
			version = version.Remove(0, version.IndexOf("<a href=\"") + "<a href=\"".Length);
			version = version.Substring(0, version.IndexOf("\" rel=\"nofollow\">"));
			version = version.Substring(version.IndexOf("zipball/") + "zipball/".Length);

			try
			{
				new DownloadFile("https://github.com/megax/Schumix2/zipball/" + version);
				Log.Success("Installer", sLConsole.Installer("Text6"));
			}
			catch
			{
				Log.Error("Installer", sLConsole.Installer("Text7"));
				Log.Warning("Installer", sLConsole.Installer("Text8"));
				Thread.Sleep(5*1000);
				Environment.Exit(1);
			}

			Log.Notice("Installer", sLConsole.Installer("Text9"));
			GZip gzip = null;

			try
			{
				gzip = new GZip();
				Log.Success("Installer", sLConsole.Installer("Text10"));
			}
			catch
			{
				Log.Error("Installer", sLConsole.Installer("Text11"));
				Log.Warning("Installer", sLConsole.Installer("Text8"));
				Thread.Sleep(5*1000);
				Environment.Exit(1);
			}

			string dir = gzip.DirectoryName;
			Log.Notice("Installer", sLConsole.Installer("Text12"));
			var build = new Build(dir);

			if(build.HasError)
			{
				Log.Error("Installer", sLConsole.Installer("Text13"));
				Log.Warning("Installer", sLConsole.Installer("Text8"));
				Thread.Sleep(5*1000);
				Environment.Exit(1);
			}

			Log.Success("Installer", sLConsole.Installer("Text14"));
			Log.Notice("Installer", sLConsole.Installer("Text3"));
			new Copy(dir);
			Log.Notice("Installer", sLConsole.Installer("Text4"));
			new FileClean();
			new DirectoryClean(dir);

			Log.Success("Installer", sLConsole.Installer("Text15"));
			Environment.Exit(0);
		}
	}
}