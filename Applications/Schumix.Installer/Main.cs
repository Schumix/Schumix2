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

		/// <summary>
		///     A Main függvény. Itt indul el a program.
		/// </summary>
		public static void Main(string[] args)
		{
			Console.BackgroundColor = ConsoleColor.Black;
			string url = "http://megax.uw.hu/Schumix2/";
			string version = new DownloadVersion(url).GetNewVersion();

			try
			{
				new DownloadFile(url + version + ".tar", version);
				Console.WriteLine("[Installer] {0}", sLConsole.Update("Text6"));
			}
			catch
			{
				Console.WriteLine("[Installer] {0}", sLConsole.Update("Text7"));
				Console.WriteLine("[Installer] {0}", sLConsole.Update("Text8"));
				Thread.Sleep(5*1000);
				Environment.Exit(1);
			}

			Console.WriteLine("[Installer] {0}", sLConsole.Update("Text9"));

			try
			{
				new GZip(version);
				Console.WriteLine("[Installer] {0}", sLConsole.Update("Text10"));
			}
			catch
			{
				Console.WriteLine("[Installer] {0}", sLConsole.Update("Text11"));
				Console.WriteLine("[Installer] {0}", sLConsole.Update("Text8"));
				Thread.Sleep(5*1000);
				Environment.Exit(1);
			}

			Console.WriteLine("[Installer] {0}", sLConsole.Update("Text12"));
			var build = new Build(version);

			if(build.HasError)
			{
				Console.WriteLine("[Installer] {0}", sLConsole.Update("Text13"));
				Console.WriteLine("[Installer] {0}", sLConsole.Update("Text8"));
				Thread.Sleep(5*1000);
				Environment.Exit(1);
			}

			Console.WriteLine("[Installer] {0}", sLConsole.Update("Text14"));
			new Copy(version);
			new FileClean(version);
			new DirectoryClean(version);
		}
	}
}