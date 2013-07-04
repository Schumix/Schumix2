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
using System.Threading;
using System.Diagnostics;
using Schumix.Framework;
using Schumix.Framework.Logger;
using Schumix.Framework.Config;
using Schumix.Framework.Platforms;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using Schumix.Components.Updater.Sql;
using Schumix.Components.Updater.Compiler;
using Schumix.Components.Updater.Download;

namespace Schumix.Components.Updater
{
	public sealed class Update
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly Platform sPlatform = Singleton<Platform>.Instance;
		private const string _dir = "Schumix2";

		public Update(string ConfigDirectory)
		{
			if(!UpdateConfig.Enabled)
			{
				Log.Notice("Update", sLConsole.GetString("Automatic updater is off."));
				return;
			}

			if(sPlatform.IsLinux)
				System.Net.ServicePointManager.ServerCertificateValidationCallback += (s,ce,ca,p) => true;

			if(UpdateConfig.Version == "stable")
			{
				Log.Notice("Update", sLConsole.GetString("Searching for new stable version is started."));
				string url = UpdateConfig.WebPage.Remove(0, "http://".Length, "http://");
				url = url.Remove(0, "https://".Length, "https://");
				string version = sUtilities.GetUrl("https://raw." + url + "/stable" +
				                                   "/Core/Schumix.Framework/Config/Consts.cs");
				version = version.Remove(0, version.IndexOf("SchumixVersion = \"") + "SchumixVersion = \"".Length);
				version = version.Substring(0, version.IndexOf("\";"));
				
				var v1 = new Version(version);
				var v2 = new Version(Schumix.Framework.Config.Consts.SchumixVersion);

				switch(v1.CompareTo(v2))
				{
					case 0:
						Log.Warning("Update", sLConsole.GetString("Currently no newer version!"));
						Log.Notice("Update", sLConsole.GetString("The program starts is continuing."));
						return;
					case 1:
						Log.Success("Update", sLConsole.GetString("I found a newer version. The update to {0} version is starting."), v1.ToString());
						break;
					case -1:
						Log.Warning("Update", sLConsole.GetString("Older version found, update interrupted!"));
						Log.Notice("Update", sLConsole.GetString("The program starts is continuing."));
						return;
				}

				Log.Notice("Update", sLConsole.GetString("Downloading new version."));

				try
				{
					new CloneSchumix("git://" + url, _dir, "stable");
					Log.Success("Update", sLConsole.GetString("Successfully downloaded new version."));
				}
				catch
				{
					Log.Error("Update", sLConsole.GetString("Downloading unsuccessful!"));
					Log.Warning("Update", sLConsole.GetString("Updating successful!"));
					Thread.Sleep(5*1000);
					Environment.Exit(1);
				}
			}
			else if(UpdateConfig.Version == "current")
			{
				Log.Notice("Update", sLConsole.GetString("Searching for the last version is started."));
				string url = UpdateConfig.WebPage.Remove(0, "http://".Length, "http://");
				url = url.Remove(0, "https://".Length, "https://");
				string version = sUtilities.GetUrl("https://raw." + url + "/" + UpdateConfig.Branch +
				                                   "/Core/Schumix.Framework/Config/Consts.cs");
				version = version.Remove(0, version.IndexOf("SchumixVersion = \"") + "SchumixVersion = \"".Length);
				version = version.Substring(0, version.IndexOf("\";"));

				var v1 = new Version(version);
				var v2 = new Version(Schumix.Framework.Config.Consts.SchumixVersion);

				switch(v1.CompareTo(v2))
				{
					case 0:
						Log.Warning("Update", sLConsole.GetString("Currently no newer version!"));
						Log.Notice("Update", sLConsole.GetString("The program starts is continuing."));
						return;
					case 1:
						Log.Success("Update", sLConsole.GetString("I found a newer version. The update to {0} version is starting."), v1.ToString());
						break;
					case -1:
						Log.Warning("Update", sLConsole.GetString("Older version found, update interrupted!"));
						Log.Notice("Update", sLConsole.GetString("The program starts is continuing."));
						return;
				}

				Log.Notice("Update", sLConsole.GetString("Downloading new version."));

				try
				{
					new CloneSchumix("git://" + url, _dir, UpdateConfig.Branch);
					Log.Success("Update", sLConsole.GetString("Successfully downloaded new version."));
				}
				catch
				{
					Log.Error("Update", sLConsole.GetString("Downloading unsuccessful!"));
					Log.Warning("Update", sLConsole.GetString("Updating successful!"));
					Thread.Sleep(5*1000);
					Environment.Exit(1);
				}
			}
			else
			{
				Log.Warning("Update", sLConsole.GetString("No such version like this, update interrupted!"));
				return;
			}

			Log.Notice("Update", sLConsole.GetString("Started translating."));
			var build = new Build(_dir);

			if(build.HasError)
			{
				Log.Error("Update", sLConsole.GetString("Error was handled while translated!"));
				Log.Warning("Update", sLConsole.GetString("Updating successful!"));
				Thread.Sleep(5*1000);
				Environment.Exit(1);
			}

			Log.Success("Update", sLConsole.GetString("Successfully finished the translation."));
			Log.Notice("Update", sLConsole.GetString("Sql files update is started. The setted database will be updated."));
			var sql = new SqlUpdate(_dir + "/Sql/Updates");
			sql.Connect();
			sql.Update();
			Log.Success("Update", sLConsole.GetString("Sql update finished."));

			if(File.Exists("Config.exe"))
				File.Delete("Config.exe");

			File.Move(_dir + "/Run/Release/Config.exe", "Config.exe");
			var config = new Process();
			config.StartInfo.UseShellExecute = false;
			config.StartInfo.RedirectStandardOutput = true;
			config.StartInfo.RedirectStandardError = true;

			if(sPlatform.IsLinux)
			{
				config.StartInfo.FileName = "mono";
				config.StartInfo.Arguments = "Config.exe " + _dir + SchumixBase.Space + AddonsConfig.Directory + SchumixBase.Space + ConfigDirectory;
			}
			else if(sPlatform.IsWindows)
			{
				config.StartInfo.FileName = "Config.exe";
				config.StartInfo.Arguments = _dir + SchumixBase.Space + AddonsConfig.Directory + SchumixBase.Space + ConfigDirectory;
			}

			Log.Notice("Update", sLConsole.GetString("This step of updateing is finished. Continue with next step."));
			config.Start();

			if(sPlatform.IsLinux)
			{
				config.WaitForExit();
				Log.Success("Update", sLConsole.GetString("The update is finished. The program shutting down!"));
			}

			Environment.Exit(0);
		}
	}
}