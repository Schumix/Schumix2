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
		///     Nick parancs függvénye.
		/// </summary>
		protected void HandleNick(ConsoleMessage sConsoleMessage)
		{
			var text = sLManager.GetConsoleCommandTexts("nick");
			if(text.Length < 2)
			{
				Log.Error("Console", sLConsole.Translations("NoFound2"));
				return;
			}

			if(sConsoleMessage.Info.Length < 2)
			{
				Log.Error("Console", sLManager.GetConsoleWarningText("NoName"));
				return;
			}

			sIrcBase.Networks[_servername].NewNick = true;
			string nick = sConsoleMessage.Info[1];

			if(!Rfc2812Util.IsValidNick(nick))
			{
				Log.Error("Console", sLManager.GetConsoleWarningText("NotaNickNameHasBeenSet"));
				return;
			}

			if(sIrcBase.Networks[_servername].sMyNickInfo.NickStorage == nick)
			{
				Log.Error("Console", text[1]);
				return;
			}

			if(sIrcBase.Networks[_servername].sChannelList.List[_channel].Names.ContainsKey(nick.ToLower()) && sIrcBase.Networks[_servername].sMyNickInfo.NickStorage.ToLower() != nick.ToLower())
			{
				Log.Error("Console", sLConsole.MessageHandler("Text14"));
				return;
			}

			sIrcBase.Networks[_servername].sMyNickInfo.ChangeNick(nick);
			sIrcBase.Networks[_servername].sSender.Nick(nick);
			Log.Notice("Console", text[0], nick);
		}
	}
}