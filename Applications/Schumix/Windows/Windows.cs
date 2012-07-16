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
using System.Diagnostics;
using System.Runtime.InteropServices;
using Schumix.Irc;
using Schumix.Framework;
using Schumix.Framework.Extensions;

namespace Schumix
{
	class Windows
	{
		[DllImport("Kernel32")]
		private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private delegate bool EventHandler(CtrlType sig);
		private EventHandler _handler;
		private Windows() {}

		public void Init()
		{
			_handler += new EventHandler(Handler);
			SetConsoleCtrlHandler(_handler, true);
		}

		private bool Handler(CtrlType sig)
		{
			switch(sig)
			{
				case CtrlType.CTRL_C_EVENT:
				case CtrlType.CTRL_BREAK_EVENT:
				case CtrlType.CTRL_CLOSE_EVENT:
					Log.Notice("Windows", "Daemon killed.");

					if(!SchumixBot.sSchumixBase.IsNull())
					{
						bool e = false;
						foreach(var nw in sIrcBase.Networks)
						{
							if(!sIrcBase.Networks[nw.Key].IsNull() && sIrcBase.Networks[nw.Key].Online)
								e = true;
						}

						if(e)
							SchumixBase.Quit();
						else
							Process.GetCurrentProcess().Kill();
					}
					else
						Process.GetCurrentProcess().Kill();

					foreach(var nw in sIrcBase.Networks)
						sIrcBase.Networks[nw.Key].sSender.Quit("Daemon killed.");
					break;
				case CtrlType.CTRL_LOGOFF_EVENT:
				case CtrlType.CTRL_SHUTDOWN_EVENT:
					Log.Notice("Windows", "User is logging off.");
					if(!SchumixBot.sSchumixBase.IsNull())
					{
						bool e = false;
						foreach(var nw in sIrcBase.Networks)
						{
							if(!sIrcBase.Networks[nw.Key].IsNull() && sIrcBase.Networks[nw.Key].Online)
								e = true;
						}

						if(e)
							SchumixBase.Quit();
						else
							Process.GetCurrentProcess().Kill();
					}
					else
						Process.GetCurrentProcess().Kill();

					foreach(var nw in sIrcBase.Networks)
						sIrcBase.Networks[nw.Key].sSender.Quit("User is logging off.");
					break;
				default:
					break;
			}

			Thread.Sleep(5*1000);
			return true;
		}
	}
}