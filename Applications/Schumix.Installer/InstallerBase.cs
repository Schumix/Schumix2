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
using System.Text;
using System.Threading;
using System.Diagnostics;
using Schumix.Installer.Util;
using Schumix.Installer.CopyTo;
using Schumix.Installer.Logger;
using Schumix.Installer.Options;
using Schumix.Installer.Compiler;
using Schumix.Installer.Download;
using Schumix.Installer.Platforms;
using Schumix.Installer.Extensions;
using Schumix.Installer.Localization;

namespace Schumix.Installer
{
	sealed class InstallerBase
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly Platform sPlatform = Singleton<Platform>.Instance;
		private readonly Runtime sRuntime = Singleton<Runtime>.Instance;
		private const string GitUrl = "https://github.com/Schumix/Schumix2";
		private const string _dir = "Schumix2";

		public InstallerBase()
		{
			if(sPlatform.IsLinux)
				ServicePointManager.ServerCertificateValidationCallback += (s,ce,ca,p) => true;

			WebRequest.DefaultWebProxy = null;
			Log.Notice("InstallerBase", sLConsole.GetString("Installer started."));

			string url = GetUrl();
			CloneSourceCode(url);
			BuildSourceCode();
			CopyNewFiles();
			Clean();

			Log.Success("Installer", sLConsole.GetString("The installation is finished. The program shutting down!"));
			sRuntime.Exit();
		}

		~InstallerBase()
		{
			Log.Debug("InstallerBase", "~InstallerBase()");
		}

		private string GetUrl()
		{
			string url = GitUrl.Remove(0, "http://".Length, "http://");
			return url.Remove(0, "https://".Length, "https://");
		}

		private void CloneSourceCode(string url)
		{
			try
			{
				new CloneSchumix("git://" + url, _dir);
				Log.Success("Installer", sLConsole.GetString("Successfully downloaded new version."));
			}
			catch(Exception e)
			{
				Log.Error("Installer", sLConsole.GetString("Downloading unsuccessful!"));
				Log.Debug("Installer", sLConsole.GetString("Failure details: {0}"), e.Message);
				Log.Warning("Installer", sLConsole.GetString("Installation successful!"));
				Thread.Sleep(5*1000);
				sRuntime.Exit();
			}
		}

		private void BuildSourceCode()
		{
			Log.Notice("Installer", sLConsole.GetString("Started translating."));
			var build = new Build(_dir);

			if(build.HasError)
			{
				Log.Error("Installer", sLConsole.GetString("Error was handled while translated!"));
				Log.Warning("Installer", sLConsole.GetString("Installation successful!"));
				Thread.Sleep(5*1000);
				sRuntime.Exit();
			}

			Log.Success("Installer", sLConsole.GetString("Successfully finished the translation."));
		}

		private void CopyNewFiles()
		{
			Log.Notice("Installer", sLConsole.GetString("Copy files."));
			new Copy(_dir);
		}

		private void Clean()
		{
			Log.Notice("Installer", sLConsole.GetString("Remove unneeded datas."));

			try
			{
				if(Directory.Exists(_dir))
				{
					sUtilities.ClearAttributes(_dir);
					Directory.Delete(_dir, true);
				}
			}
			catch(Exception e)
			{
				Log.Warning("Installer", e.Message);
			}
		}
	}
}