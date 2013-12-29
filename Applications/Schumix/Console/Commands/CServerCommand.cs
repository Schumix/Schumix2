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
using Schumix.Framework;
using Schumix.Framework.Logger;
using Schumix.Console.Delegate;

namespace Schumix.Console
{
	/// <summary>
	///     CommandHandler class.
	/// </summary>
	partial class CommandHandler
	{
		protected void HandleOldServerToNewServer(ConsoleMessage sConsoleMessage)
		{
			if(sConsoleMessage.Info.Length < 2)
			{
				Log.Error("Console", sLManager.GetConsoleWarningText("NoServerName"));
				return;
			}

			if(!sIrcBase.Networks.ContainsKey(sConsoleMessage.Info[1].ToLower()))
			{
				Log.Error("Console", sLManager.GetConsoleWarningText("ThereIsNoSuchAServerName"));
				return;
			}

			if(_servername == sConsoleMessage.Info[1].ToLower())
			{
				Log.Warning("Console", sLManager.GetConsoleWarningText("ServerAlreadyBeenUsed"));
				return;
			}

			_servername = sConsoleMessage.Info[1].ToLower();
			Log.Notice("Console", sLManager.GetConsoleCommandText("cserver"), sConsoleMessage.Info[1]);
			System.Console.Title = SchumixBase.Title + " || Console Writing Channel: " + sConsoleMessage.Info[1] + SchumixBase.Colon + _channel;
		}
	}
}