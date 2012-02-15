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
using Schumix.Framework;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Console.Commands
{
	/// <summary>
	///     CommandHandler class.
	/// </summary>
	partial class CommandHandler
	{
		/// <summary>
		///     Help parancs függvénye.
		/// </summary>
		protected void HandleHelp()
		{
			if(Info.Length == 1)
			{
				var text = sLManager.GetConsoleCommandTexts("help");
				if(text.Length < 2)
				{
					Log.Error("Console", sLConsole.Translations("NoFound2"));
					return;
				}

				string commands = string.Empty;

				foreach(var command in CCommandManager.GetCommandHandler())
				{
					if(command.Key == "help")
						continue;

					commands += ", " + command.Key;
				}

				Log.Notice("Console", text[0]);
				Log.Notice("Console", text[1], commands.Remove(0, 2, ", "));
				return;
			}

			foreach(var t in sLManager.GetConsoleCommandHelpTexts(Info.SplitToString(1, "/")))
			{
				Log.Notice("Console", t);
			}
		}
	}
}