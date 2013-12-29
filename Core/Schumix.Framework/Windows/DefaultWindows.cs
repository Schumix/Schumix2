/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013-2014 Schumix Team <http://schumix.eu/>
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
using System.Runtime.InteropServices;
using Schumix.Framework;
using Schumix.Framework.Logger;
using Schumix.Framework.Network;
using Schumix.Framework.Localization;

namespace Schumix.Framework.Windows
{
	public class DefaultWindows
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;

		[DllImport("Kernel32")]
		protected static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);
		protected delegate bool EventHandler(CtrlType sig);
		private EventHandler _handler;

		public void Init()
		{
			_handler += new EventHandler(Handler);
			SetConsoleCtrlHandler(_handler, true);
		}

		public virtual void Action()
		{
		}

		protected virtual bool Handler(CtrlType sig)
		{
			switch(sig)
			{
				case CtrlType.CTRL_C_EVENT:
				case CtrlType.CTRL_BREAK_EVENT:
				case CtrlType.CTRL_CLOSE_EVENT:
					Log.Notice("Windows", sLConsole.GetString("Daemon killed."));
					Action();
					break;
				case CtrlType.CTRL_LOGOFF_EVENT:
				case CtrlType.CTRL_SHUTDOWN_EVENT:
					Log.Notice("Windows", sLConsole.GetString("User is logging off."));
					Action();
					break;
				default:
					break;
			}

			return true;
		}
	}
}