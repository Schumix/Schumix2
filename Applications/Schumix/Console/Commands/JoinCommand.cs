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
using Schumix.Irc.Util;
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
		/// <summary>
		///     Join parancs függvénye.
		/// </summary>
		protected void HandleJoin(ConsoleMessage sConsoleMessage)
		{
			if(sConsoleMessage.Info.Length < 2)
			{
				Log.Error("Console", sLManager.GetConsoleWarningText("NoChannelName"));
				return;
			}

			if(!Rfc2812Util.IsValidChannelName(sConsoleMessage.Info[1]))
			{
				Log.Error("Console", sLManager.GetConsoleWarningText("NotaChannelHasBeenSet"));
				return;
			}

			if(sIrcBase.Networks[_servername].sChannelList.List.ContainsKey(sConsoleMessage.Info[1].ToLower()))
			{
				Log.Error("Console", sLManager.GetConsoleWarningText("ImAlreadyOnThisChannel"));
				return;
			}

			if(sIrcBase.Networks[_servername].sIgnoreChannel.IsIgnore(sConsoleMessage.Info[1].ToLower()))
			{
				Log.Error("Console", sLManager.GetConsoleWarningText("ThisChannelBlockedByAdmin"));
				return;
			}

			if(sConsoleMessage.Info.Length == 2)
				sIrcBase.Networks[_servername].sSender.Join(sConsoleMessage.Info[1]);
			else if(sConsoleMessage.Info.Length == 3)
				sIrcBase.Networks[_servername].sSender.Join(sConsoleMessage.Info[1], sConsoleMessage.Info[2]);

			Log.Notice("Console", sLManager.GetConsoleCommandText("join"), sConsoleMessage.Info[1]);
		}
	}
}