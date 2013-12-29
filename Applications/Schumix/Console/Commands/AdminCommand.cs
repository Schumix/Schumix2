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
using Schumix.Irc.Util;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Config;
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
		///     Admin parancs függvénye.
		/// </summary>
		protected void HandleAdmin(ConsoleMessage sConsoleMessage)
		{
			if(sConsoleMessage.Info.Length >= 2 && sConsoleMessage.Info[1].ToLower() == "info")
			{
				if(sConsoleMessage.Info.Length < 3)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NoName"));
					return;
				}

				var text = sLManager.GetConsoleCommandTexts("admin/info");
				if(text.Length < 3)
				{
					Log.Error("Console", sLConsole.Translations("NoFound2"));
					return;
				}

				var db = SchumixBase.DManager.QueryFirstRow("SELECT Flag FROM admins WHERE Name = '{0}' And ServerName = '{1}'", sConsoleMessage.Info[2].ToLower(), _servername);
				int flag = !db.IsNull() ? db["Flag"].ToInt32() : -1;

				if((AdminFlag)flag == AdminFlag.HalfOperator)
					Log.Notice("Console", text[0]);		
				else if((AdminFlag)flag == AdminFlag.Operator)
					Log.Notice("Console", text[1]);
				else if((AdminFlag)flag == AdminFlag.Administrator)
					Log.Notice("Console", text[2]);
			}
			else if(sConsoleMessage.Info.Length >= 2 && sConsoleMessage.Info[1].ToLower() == "list")
			{
				var db = SchumixBase.DManager.Query("SELECT Name FROM admins WHERE ServerName = '{0}'", _servername);
				if(!db.IsNull())
				{
					string admins = string.Empty;

					foreach(DataRow row in db.Rows)
					{
						string name = row["Name"].ToString();
						admins += ", " + name;
					}

					Log.Notice("Console", sLManager.GetConsoleCommandText("admin/list"), admins.Remove(0, 2, ", "));
				}
				else
					Log.Error("Console", sLManager.GetConsoleWarningText("FaultyQuery"));
			}
			else if(sConsoleMessage.Info.Length >= 2 && sConsoleMessage.Info[1].ToLower() == "add")
			{
				if(sConsoleMessage.Info.Length < 3)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NoName"));
					return;
				}

				var text = sLManager.GetConsoleCommandTexts("admin/add");
				if(text.Length < 3)
				{
					Log.Error("Console", sLConsole.Translations("NoFound2"));
					return;
				}

				string name = sConsoleMessage.Info[2];
				if(!Rfc2812Util.IsValidNick(name))
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NotaNickNameHasBeenSet"));
					return;
				}

				var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM admins WHERE Name = '{0}' And ServerName = '{1}'", name.ToLower(), _servername);
				if(!db.IsNull())
				{
					Log.Warning("Console", text[0]);
					return;
				}

				string pass = sUtilities.GetRandomString();
				SchumixBase.DManager.Insert("`admins`(ServerId, ServerName, Name, Password)", IRCConfig.List[_servername].ServerId, _servername, name.ToLower(), sUtilities.Sha1(pass));

				if(SchumixBase.DManager.IsCreatedTable("hlmessage"))
					SchumixBase.DManager.Insert("`hlmessage`(ServerId, ServerName, Name, Enabled)", IRCConfig.List[_servername].ServerId, _servername, name.ToLower(), SchumixBase.Off);

				Log.Notice("Console", text[1], name);
				Log.Notice("Console", text[2], pass);
			}
			else if(sConsoleMessage.Info.Length >= 2 && sConsoleMessage.Info[1].ToLower() == "remove")
			{
				if(sConsoleMessage.Info.Length < 3)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NoName"));
					return;
				}

				var text = sLManager.GetConsoleCommandTexts("admin/remove");
				if(text.Length < 2)
				{
					Log.Error("Console", sLConsole.Translations("NoFound2"));
					return;
				}

				string name = sConsoleMessage.Info[2];
				if(!Rfc2812Util.IsValidNick(name))
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NotaNickNameHasBeenSet"));
					return;
				}

				var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM admins WHERE Name = '{0}' And ServerName = '{1}'", name.ToLower(), _servername);
				if(db.IsNull())
				{
					Log.Warning("Console", text[0]);
					return;
				}

				SchumixBase.DManager.Delete("admins", string.Format("Name = '{0}' And ServerName = '{1}'", name.ToLower(), _servername));

				if(SchumixBase.DManager.IsCreatedTable("hlmessage"))
					SchumixBase.DManager.Delete("hlmessage", string.Format("Name = '{0}' And ServerName = '{1}'", name.ToLower(), _servername));

				if(SchumixBase.DManager.IsCreatedTable("birthday"))
				{
					var db1 = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM birthday WHERE Name = '{0}' And ServerName = '{1}'", name.ToLower(), _servername);
					if(!db1.IsNull())
						SchumixBase.DManager.Delete("birthday", string.Format("Name = '{0}' And ServerName = '{1}'", name.ToLower(), _servername));
				}

				Log.Notice("Console", text[1], name);
			}
			else if(sConsoleMessage.Info.Length >= 2 && sConsoleMessage.Info[1].ToLower() == "rank")
			{
				if(sConsoleMessage.Info.Length < 3)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NoName"));
					return;
				}

				if(sConsoleMessage.Info.Length < 4)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NoRank"));
					return;
				}

				var text = sLManager.GetConsoleCommandTexts("admin/rank");
				if(text.Length < 2)
				{
					Log.Error("Console", sLConsole.Translations("NoFound2"));
					return;
				}

				string name = sConsoleMessage.Info[2].ToLower();
				if(!Rfc2812Util.IsValidNick(name))
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NotaNickNameHasBeenSet"));
					return;
				}

				int rank = sConsoleMessage.Info[3].ToInt32();

				if((AdminFlag)rank == AdminFlag.Administrator || (AdminFlag)rank == AdminFlag.Operator || (AdminFlag)rank == AdminFlag.HalfOperator)
				{
					SchumixBase.DManager.Update("admins", string.Format("Flag = '{0}'", rank), string.Format("Name = '{0}' And ServerName = '{1}'", name, _servername));
					Log.Notice("Console", text[0]);
				}
				else
					Log.Error("Console", text[1]);
			}
			else
				Log.Notice("Console", sLManager.GetConsoleCommandText("admin"));
		}

	}
}