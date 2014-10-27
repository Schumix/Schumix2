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
using System.Data;
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
		///     Alias parancs függvénye.
		/// </summary>
		protected void HandleAlias(ConsoleMessage sConsoleMessage)
		{
			if(sConsoleMessage.Info.Length < 2)
			{
				Log.Error("Console", sLManager.GetConsoleWarningText("NoValue"));
				return;
			}

			if(sConsoleMessage.Info[1].ToLower() == "command")
			{
				if(sConsoleMessage.Info.Length < 3)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("No1Value"));
					return;
				}

				if(sConsoleMessage.Info[2].ToLower() == "add")
				{
					var text = sLManager.GetConsoleCommandTexts("alias/command/add");
					if(text.Length < 6)
					{
						Log.Error("Console", sLConsole.Translations("NoFound2"));
						return;
					}

					if(sConsoleMessage.Info.Length < 4)
					{
						Log.Error("Console", text[0]);
						return;
					}

					if(sConsoleMessage.Info.Length < 5)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoCommand"));
						return;
					}

					string newcommand = sConsoleMessage.Info[3].ToLower();
					string basecommand = sConsoleMessage.Info[4].ToLower();

					var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM alias_console_command WHERE NewCommand = '{0}'", sUtilities.SqlEscape(newcommand));
					if(!db.IsNull())
					{
						Log.Warning("Console", text[1]);
						return;
					}

					var db1 = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM alias_console_command WHERE NewCommand = '{0}'", sUtilities.SqlEscape(basecommand));
					if(!db1.IsNull())
					{
						Log.Warning("Console", text[2]);
						return;
					}

					if(CCommandManager.GetCommandHandler().ContainsKey(newcommand))
					{
						Log.Warning("Console", text[3], newcommand);
						return;
					}

					if(!CCommandManager.GetCommandHandler().ContainsKey(basecommand))
					{
						Log.Error("Console", text[4], basecommand);
						return;
					}

					CCommandManager.RegisterHandler(newcommand, CCommandManager.GetCommandHandler()[basecommand].Method);
					SchumixBase.DManager.Insert("`alias_console_command`(NewCommand, BaseCommand)", sUtilities.SqlEscape(newcommand), sUtilities.SqlEscape(basecommand));
					Log.Notice("Console", text[5], newcommand, basecommand);
				}
				else if(sConsoleMessage.Info[2].ToLower() == "remove")
				{
					var text = sLManager.GetConsoleCommandTexts("alias/command/remove");
					if(text.Length < 2)
					{
						Log.Error("Console", sLConsole.Translations("NoFound2"));
						return;
					}

					if(sConsoleMessage.Info.Length < 4)
					{
						Log.Error("Console", sLManager.GetWarningText("NoCommand"));
						return;
					}

					string command = sConsoleMessage.Info[3].ToLower();

					var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM alias_console_command WHERE NewCommand = '{0}'", sUtilities.SqlEscape(command));
					if(db.IsNull())
					{
						Log.Error("Console", text[0]);
						return;
					}

					CCommandManager.RemoveHandler(command, CCommandManager.GetCommandHandler()[command].Method);
					SchumixBase.DManager.Delete("alias_console_command", string.Format("NewCommand = '{0}'", sUtilities.SqlEscape(command)));
					Log.Notice("Console", text[1], command);
				}
				else if(sConsoleMessage.Info[2].ToLower() == "list")
				{
					var text = sLManager.GetConsoleCommandTexts("alias/command/list");
					if(text.Length < 2)
					{
						Log.Error("Console", sLConsole.Translations("NoFound2"));
						return;
					}

					var db = SchumixBase.DManager.Query("SELECT NewCommand, BaseCommand FROM alias_console_command");
					if(!db.IsNull())
					{
						string commandlist = string.Empty;

						foreach(DataRow row in db.Rows)
						{
							string newcommand = row["NewCommand"].ToString();
							string basecommand = row["BaseCommand"].ToString();
							commandlist += ", " + newcommand + "->" + basecommand;
						}

						if(commandlist.Length > 0)
							Log.Notice("Console", text[0], commandlist.Remove(0, 2, ", "));
						else
							Log.Notice("Console", text[1]);
					}
					else
						Log.Error("Console", sLManager.GetConsoleWarningText("FaultyQuery"));
				}
			}
		}
	}
}