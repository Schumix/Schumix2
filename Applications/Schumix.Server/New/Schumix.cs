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
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Generic;
using Schumix.Framework;
using Schumix.Framework.Localization;
using Schumix.Framework.Extensions;

namespace Schumix.Server.New
{
	class Settings
	{
		public string File { get; set; }
		public string Dir { get; set; }
		public string Encoding { get; set; }
		public string Locale { get; set; }
		public string Configs { get; set; }
		public string Identify { get; set; }
		public Process Process { get; set; }
	}

	class Schumix
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		public readonly Dictionary<string, Settings> _processlist = new Dictionary<string, Settings>();

		private Schumix()
		{
			new Thread(Run).Start();
		}

		private void Run()
		{
			var l = new List<string>();

			while(true)
			{
				try
				{
					if(!MainClass.sListener.IsNull() && MainClass.sListener.Exit)
						return;

					foreach(var list in _processlist)
					{
						if(list.Value.Process.IsNull())
						{
							l.Add(list.Key);
							continue;
						}

						bool run = false;

						if(sUtilities.GetPlatformType() == PlatformType.Linux)
						{
							foreach(var p in Process.GetProcessesByName("mono"))
							{
								if(p.Id == list.Value.Process.Id)
									run = true;
							}
						}
						else if(sUtilities.GetPlatformType() == PlatformType.Windows)
						{
							foreach(var p in Process.GetProcessesByName("Schumix"))
							{
								if(p.Id == list.Value.Process.Id)
									run = true;
							}
						}

						if(run)
							continue;

						l.Add(list.Key);

						//if(list.Value.Process.IsNull() || IsRunnig(list.Value.Process))
						//	l.Add(list.Key);
					}

					foreach(var ll in l)
					{
						Log.Notice("Schumix", sLConsole.Schumix("Text"));

						_processlist[ll].Process.Kill();

						//if(!_processlist[ll].Process.IsNull())
						//	_processlist[ll].Process.Dispose();

						Task.Factory.StartNew(() => Start(_processlist[ll].File, _processlist[ll].Dir, _processlist[ll].Encoding, _processlist[ll].Locale, _processlist[ll].Identify, _processlist[ll].Configs));
						Thread.Sleep(10*1000);
						_processlist.Remove(ll);
					}

					l.Clear();
				}
				catch(Exception e)
				{
					Log.Error("Schumix", sLConsole.Exception("Error"), e.Message);
				}

				Thread.Sleep(10*60*1000);
			}
		}

		private bool IsRunnig(Process process)
		{
			return process.HasExited;
		}

		public void Start(string File, string Dir, string Encoding, string Locale, string Identify, string Configs = "")
		{
			var exe = new Process();
			exe.StartInfo.UseShellExecute = false;
			exe.StartInfo.RedirectStandardOutput = true;
			exe.StartInfo.RedirectStandardError = true;

			if(sUtilities.GetPlatformType() == PlatformType.Linux)
			{
				exe.StartInfo.FileName = "mono";

				if(Configs == string.Empty)
					exe.StartInfo.Arguments = string.Format("Schumix.exe --config-dir={0} --config-file={1} --console-encoding={2} --console-localization={3} --server-enabled={4} --server-host={5} --server-port={6} --server-password={7} --server-identify={8}", Dir, File, Encoding, Locale, true, "127.0.0.1", Config.ServerConfigs.ListenerPort, Config.ServerConfigs.Password, Identify);
				else
					exe.StartInfo.Arguments = string.Format("Schumix.exe --config-dir={0} --config-file={1} --console-encoding={2} --console-localization={3} --server-enabled={4} --server-host={5} --server-port={6} --server-password={7} --server-configs={8} --server-identify={9}", Dir, File, Encoding, Locale, true, "127.0.0.1", Config.ServerConfigs.ListenerPort, Config.ServerConfigs.Password, Configs, Identify);
			}
			else if(sUtilities.GetPlatformType() == PlatformType.Windows)
			{
				exe.StartInfo.FileName = "Schumix.exe";

				if(Configs == string.Empty)
					exe.StartInfo.Arguments = string.Format("--config-dir={0} --config-file={1} --console-encoding={2} --console-localization={3} --server-enabled={4} --server-host={5} --server-port={6} --server-password={7} --server-identify={8}", Dir, File, Encoding, Locale, true, "127.0.0.1", Config.ServerConfigs.ListenerPort, Config.ServerConfigs.Password, Identify);
				else
					exe.StartInfo.Arguments = string.Format("--config-dir={0} --config-file={1} --console-encoding={2} --console-localization={3} --server-enabled={4} --server-host={5} --server-port={6} --server-password={7} --server-configs={8} --server-identify={9}", Dir, File, Encoding, Locale, true, "127.0.0.1", Config.ServerConfigs.ListenerPort, Config.ServerConfigs.Password, Configs, Identify);
			}

			exe.Start();
			exe.PriorityClass = ProcessPriorityClass.Normal;
			var settings      = new Settings();
			settings.File     = File;
			settings.Dir      = Dir;
			settings.Encoding = Encoding;
			settings.Locale   = Locale;
			settings.Configs  = Configs;
			settings.Identify = Identify;
			settings.Process  = exe;
			_processlist.Add(Identify, settings);
			//exe.Dispose();
		}
	}
}