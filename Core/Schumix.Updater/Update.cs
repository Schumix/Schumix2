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
using System.Threading;
using System.Diagnostics;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Localization;
using Schumix.Updater.UnZip;
using Schumix.Updater.Compiler;
using Schumix.Updater.Download;

namespace Schumix.Updater
{
	public sealed class Update
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;

		public Update()
		{
			if(!UpdateConfig.Enabled)
			{
				Log.Notice("Update", sLConsole.Update("Text"));
				return;
			}

			Log.Notice("Update", sLConsole.Update("Text2"));
			var version = new DownloadVersion(UpdateConfig.WebPage).GetVersion();

			if(sUtilities.GetVersion() == version)
			{
				Log.Warning("Update", sLConsole.Update("Text3"));
				return;
			}

			Log.Notice("Update", sLConsole.Update("Text4"), version);
			Log.Notice("Update", sLConsole.Update("Text5"));

			try
			{
				new DownloadFile(UpdateConfig.WebPage + version, version);
				Log.Success("Update", sLConsole.Update("Text6"));
			}
			catch
			{
				Log.Error("Update", sLConsole.Update("Text7"));
				Log.Warning("Update", sLConsole.Update("Text8"));
				Thread.Sleep(5*1000);
				Environment.Exit(1);
			}

			Log.Notice("Update", sLConsole.Update("Text9"));

			try
			{
				new GZip(version);
				Log.Success("Update", sLConsole.Update("Text10"));
			}
			catch
			{
				Log.Error("Update", sLConsole.Update("Text11"));
				Log.Warning("Update", sLConsole.Update("Text8"));
				Thread.Sleep(5*1000);
				Environment.Exit(1);
			}

			Log.Notice("Update", sLConsole.Update("Text12"));
			var build = new Build(version);

			if(build.HasError)
			{
				Log.Error("Update", "Hiba történt a fordítás közben!");
				Log.Warning("Update", sLConsole.Update("Text13"));
				Thread.Sleep(5*1000);
				Environment.Exit(1);
			}

			Log.Success("Update", sLConsole.Update("Text14"));

			if(File.Exists("Schumix.Config.exe"))
				File.Delete("Schumix.Config.exe");

			File.Move(version + "/Run/Release/Schumix.Config.exe", "Schumix.Config.exe");
			var config = new Process();
			config.StartInfo.UseShellExecute = false;
			config.StartInfo.RedirectStandardOutput = true;
			config.StartInfo.RedirectStandardError = true;
#if MONO
			config.StartInfo.FileName = "mono";
			config.StartInfo.Arguments = "Schumix.Config.exe " + version;
#else
			config.StartInfo.FileName = "Schumix.Config.exe " + version;
#endif
			Log.Notice("Update", sLConsole.Update("Text15"));
			config.Start();
			Environment.Exit(0);
		}
	}
}