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
using Schumix.Framework.Util;
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
		private readonly Runtime sRuntime = Singleton<Runtime>.Instance;
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

			if(!SearchingForNewVersion())
				return;

			BuildSourceCode();
			SqlUpdate();
			var process = ConfigCopyAndStart(ConfigDirectory);

			if(sPlatform.IsLinux)
			{
				process.WaitForExit();
				Log.Success("Update", sLConsole.GetString("The update is finished. The program shutting down!"));
			}

			Environment.Exit(0);
		}

		~Update()
		{
			Log.Debug("Update", "~Update()");
		}

		private bool SearchingForNewVersion()
		{
			if(UpdateConfig.Version == "stable")
			{
				Log.Notice("Update", sLConsole.GetString("Searching for new stable version is started."));
				string url = GetUrl();
				string version = GetVersion(url, "stable");

				if(!VersionCompare(version))
					return false;

				Log.Notice("Update", sLConsole.GetString("Downloading new version."));
				CloneSourceCode(url, "stable");
				return true;
			}
			else if(UpdateConfig.Version == "current")
			{
				Log.Notice("Update", sLConsole.GetString("Searching for the last version is started."));
				string url = GetUrl();
				string version = GetVersion(url, UpdateConfig.Branch);

				if(!VersionCompare(version))
					return false;

				Log.Notice("Update", sLConsole.GetString("Downloading new version."));
				CloneSourceCode(url, UpdateConfig.Branch);
				return true;
			}
			else
			{
				Log.Warning("Update", sLConsole.GetString("No such version like this, update interrupted!"));
				return false;
			}
		}

		private string GetUrl()
		{
			string url = UpdateConfig.WebPage.Remove(0, "http://".Length, "http://");
			return url.Remove(0, "https://".Length, "https://");
		}

		private string GetVersion(string url, string branch)
		{
			string version = sUtilities.GetUrl(string.Format("https://raw.{0}/{1}/Core/Schumix.Framework/Config/Consts.cs", url, branch));
			version = version.Remove(0, version.IndexOf("SchumixVersion = \"") + "SchumixVersion = \"".Length);
			return version.Substring(0, version.IndexOf("\";"));
		}

		private bool VersionCompare(string version)
		{
			var v1 = new Version(version);
			var v2 = new Version(Schumix.Framework.Config.Consts.SchumixVersion);

			switch(v1.CompareTo(v2))
			{
				case 0:
					Log.Warning("Update", sLConsole.GetString("Currently no newer version!"));
					Log.Notice("Update", sLConsole.GetString("The program starts is continuing."));
					return false;
				case 1:
					Log.Success("Update", sLConsole.GetString("I found a newer version. The update to {0} version is starting."), v1.ToString());
					return true;
				case -1:
					Log.Warning("Update", sLConsole.GetString("Older version found, update interrupted!"));
					Log.Notice("Update", sLConsole.GetString("The program starts is continuing."));
					return false;
			}

			return false;
		}

		private void CloneSourceCode(string url, string branch)
		{
			try
			{
				new CloneSchumix("git://" + url, _dir, branch);
				Log.Success("Update", sLConsole.GetString("Successfully downloaded new version."));
			}
			catch
			{
				Log.Error("Update", sLConsole.GetString("Downloading unsuccessful!"));
				Log.Warning("Update", sLConsole.GetString("Updating successful!"));
				Thread.Sleep(5*1000);
				sRuntime.Exit();
			}
		}

		private void BuildSourceCode()
		{
			Log.Notice("Update", sLConsole.GetString("Started translating."));
			var build = new Build(_dir);

			if(build.HasError)
			{
				Log.Error("Update", sLConsole.GetString("Error was handled while translated!"));
				Log.Warning("Update", sLConsole.GetString("Updating successful!"));
				Thread.Sleep(5*1000);
				sRuntime.Exit();
			}

			Log.Success("Update", sLConsole.GetString("Successfully finished the translation."));
		}

		private void SqlUpdate()
		{
			Log.Notice("Update", sLConsole.GetString("Sql files update is started. The setted database will be updated."));
			var sql = new SqlUpdate(_dir + "/Sql/Updates");
			sql.Connect();
			sql.Update();
			Log.Success("Update", sLConsole.GetString("Sql update finished."));
		}

		private Process ConfigCopyAndStart(string ConfigDirectory)
		{
			if(File.Exists("Config.exe"))
				File.Delete("Config.exe");

			File.Move(_dir + "/Run/Release/Config.exe", "Config.exe");
			var config = new Process();
			config.StartInfo.UseShellExecute = true;

			if(sPlatform.IsLinux)
			{
				config.StartInfo.FileName = "mono";
				config.StartInfo.Arguments = string.Format("Config.exe --schumix2-dir={0} --addons-dir={1} --config-dir={2} --logs-dir={3} --dumps-dir={4}", _dir, AddonsConfig.Directory, ConfigDirectory, LogConfig.LogDirectory, CrashConfig.Directory);
			}
			else if(sPlatform.IsWindows)
			{
				config.StartInfo.FileName = "Config.exe";
				config.StartInfo.Arguments = string.Format("--schumix2-dir={0} --addons-dir={1} --config-dir={2} --logs-dir={3} --dumps-dir={4}", _dir, AddonsConfig.Directory, ConfigDirectory, LogConfig.LogDirectory, CrashConfig.Directory);
			}

			Log.Notice("Update", sLConsole.GetString("This step of updateing is finished. Continue with next step."));
			config.Start();
			return config;
		}
	}
}