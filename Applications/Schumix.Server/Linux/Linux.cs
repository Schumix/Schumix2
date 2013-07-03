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
using System.Threading;
using Mono.Unix;
using Mono.Unix.Native;
using Schumix.Framework;
using Schumix.Framework.Logger;
using Schumix.Framework.Network;

namespace Schumix.Server
{
	class Linux
	{
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
			MainClass.Shutdown();
		}
	}
}