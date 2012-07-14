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
using Mono.Unix;
using Mono.Unix.Native;
using Schumix.Irc;
using Schumix.Framework;

namespace Schumix
{
	class Linux
	{
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private readonly Sender sSender = Singleton<Sender>.Instance;
		private Linux() {}

		public void Init()
		{
			new Thread(LinuxHandler).Start();
		}
	
		private void LinuxHandler()
		{
			Log.Notice("Linux", "Initializing Handler for SIGINT, SIGHUP");
			var signals = new UnixSignal[]
			{
				new UnixSignal(Signum.SIGINT),
				new UnixSignal(Signum.SIGHUP)
			};

			int which = UnixSignal.WaitAny(signals, -1);
			Log.Debug("Linux", "Got a {0} signal!", signals[which].Signum);
			Log.Notice("Linux", "Handler Terminated.");
			SchumixBase.Quit();

			foreach(var nw in sIrcBase.Networks)
				sSender.Quit(nw.Key, "Daemon killed.");
			Thread.Sleep(5*1000);
		}
	}
}