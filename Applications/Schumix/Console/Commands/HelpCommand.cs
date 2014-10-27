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
using Schumix.Framework.Extensions;
using Schumix.Console.Delegate;

namespace Schumix.Console
{
	/// <summary>
	///     CommandHandler class.
	/// </summary>
	partial class CommandHandler
	{
		/// <summary>
		///     Help parancs függvénye.
		/// </summary>
		protected void HandleHelp(ConsoleMessage sConsoleMessage)
		{
			if(sConsoleMessage.Info.Length == 1)
			{
				var text = sLManager.GetConsoleCommandTexts("help");
				if(text.Length < 3)
				{
					Log.Error("Console", sLConsole.Translations("NoFound2"));
					return;
				}

				string commands = string.Empty;
				string aliascommands = string.Empty;

				foreach(var command in CCommandManager.GetCommandHandler())
				{
					if(command.Key == "help")
						continue;

					var db = SchumixBase.DManager.QueryFirstRow("SELECT BaseCommand FROM alias_console_command WHERE NewCommand = '{0}'", sUtilities.SqlEscape(command.Key));
					if(!db.IsNull())
					{
						string basecommand = db["BaseCommand"].ToString();
						aliascommands += " | " + command.Key + "->" + basecommand;
						continue;
					}

					commands += ", " + command.Key;
				}

				Log.Notice("Console", text[0]);
				Log.Notice("Console", text[1], commands.Remove(0, 2, ", "));

				if(!aliascommands.IsNullOrEmpty())
					Log.Notice("Console", text[2], aliascommands.Remove(0, 3, " | "));

				return;
			}

			string aliascommand = string.Empty;

			var db2 = SchumixBase.DManager.QueryFirstRow("SELECT BaseCommand FROM alias_console_command WHERE NewCommand = '{0}'", sUtilities.SqlEscape(sConsoleMessage.Info[1].ToLower()));
			if(!db2.IsNull())
			{
				aliascommand = sConsoleMessage.Info[1].ToLower();
				string basecommand = db2["BaseCommand"].ToString();
				sConsoleMessage.Info[1] = basecommand;
			}

			foreach(var t in sLManager.GetConsoleCommandHelpTexts(sConsoleMessage.Info.SplitToString(1, "/")))
			{
				if(!aliascommand.IsNullOrEmpty())
					Log.Notice("Console", t.Replace(string.Format(" {0} ", sConsoleMessage.Info[1]), string.Format(" {0} ", aliascommand)));
				else
					Log.Notice("Console", t);
			}
		}
	}
}