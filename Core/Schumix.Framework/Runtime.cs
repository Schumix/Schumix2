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
using System.Text;
using System.Timers;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Schumix.API;
using Schumix.API.Irc;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Framework
{
	public sealed class Runtime
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private System.Timers.Timer _timer = new System.Timers.Timer();

		private Runtime()
		{
			_timer.Interval = 5*1000;
			_timer.Elapsed += HandleTimerMemory;
			_timer.Enabled = true;
			_timer.Start();
		}

		private void HandleTimerMemory(object sender, ElapsedEventArgs e)
		{
			if(ShutdownConfig.MaxMemory == 0)
				return;

			int ircnetwork = IRCConfig.List.Count > 1 ? 40 * IRCConfig.List.Count : 0;

			if((Process.GetCurrentProcess().WorkingSet64/1024/1024 >= ShutdownConfig.MaxMemory && IRCConfig.List.IsNull()) ||
			   Process.GetCurrentProcess().WorkingSet64/1024/1024 >= ShutdownConfig.MaxMemory + ircnetwork)
			{
				Log.Warning("Runtime", sLConsole.Runtime("Text3"), ShutdownConfig.MaxMemory + ircnetwork);
				Log.Warning("Runtime", sLConsole.Runtime("Text4"));

				if(!IRCConfig.List.IsNull())
				{
					SchumixBase.Quit();

					foreach(var nw in INetwork.WriterList)
					{
						if(!nw.Value.IsNull())
							nw.Value.WriteLine("QUIT :Memory over-consumption.");
					}

					Thread.Sleep(1000);
				}

				Process.GetCurrentProcess().Kill();
			}
		}

		public void SetProcessName(string Name)
		{
			if(sUtilities.GetPlatformType() != PlatformType.Linux || sUtilities.GetPlatformType() != PlatformType.MacOSX)
				return;

			if(Environment.OSVersion.Platform == PlatformID.Unix)
			{
				try
				{
					unixSetProcessName(Name);
				}
				catch(Exception e)
				{
					Log.Error("Runtime", sLConsole.Runtime("Text"), e.Message);
				}
			}
		}

		[DllImport("libc")] // Linux
		private static extern int prctl(int option, byte[] arg2, IntPtr arg3, IntPtr arg4, IntPtr arg5);

		[DllImport("libc")] // BSD
		private static extern void setproctitle(byte[] fmt, byte[] str_arg);

		//this is from http://abock.org/2006/02/09/changing-process-name-in-mono/
		private void unixSetProcessName(string Name)
		{
			try
			{
				if(prctl(15, Encoding.ASCII.GetBytes(Name + "\0"), IntPtr.Zero, IntPtr.Zero, IntPtr.Zero) != 0)
					Log.Error("Runtime", sLConsole.Runtime("Text2"));
			}
			catch(EntryPointNotFoundException)
			{
				try
				{
					// Not every BSD has setproctitle
					setproctitle(Encoding.ASCII.GetBytes("%s\0"), Encoding.ASCII.GetBytes(Name + "\0"));
				}
				catch(EntryPointNotFoundException)
				{

				}
      		}
		}
	}
}