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
using System.IO;
using System.Text;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using Schumix.Irc;
using Schumix.Framework;
using Schumix.Framework.Logger;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using Schumix.Console.Method;
using Schumix.Console.Delegate;

namespace Schumix.Console
{
	/// <summary>
	///     ConsoleCommandManager class.
	/// </summary>
	sealed class CCommandManager : CommandHandler
	{
		/// <summary>
		///     Tárolja a parancsokat és a hozzá tartozó függvényeket.
		/// </summary>
		private static readonly Dictionary<string, ConsoleMethod> ConsoleMethodMap = new Dictionary<string, ConsoleMethod>();
		/// <summary>
		///     Hozzáférést biztosít singleton-on keresztül a megadott class-hoz.
		///     LocalizationConsole segítségével állíthatók be a konzol nyelvi tulajdonságai.
		/// </summary>
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		/// <summary>
		///     Kimenetként kiírja a parancsokat és a hozzá tartozó függvényeket.
		/// </summary>
		public static Dictionary<string, ConsoleMethod> GetCommandHandler()
		{
			return ConsoleMethodMap;
		}

		/// <summary>
		///     Indulási függvény.
		/// </summary>
		public CCommandManager()
		{
			Log.Notice("CCommandManager", sLConsole.GetString("Successfully started the CCommandManager."));
			InitHandler();
		}

		~CCommandManager()
		{
			Log.Debug("CCommandManager", "~CCommandManager()");
		}

		/// <summary>
		///     Csatorna nevét tárolja.
		/// </summary>
		public string Channel
		{
			get { return _channel; }
			set { _channel = value; }
		}

		public string ServerName
		{
			get { return _servername; }
			set { _servername = value; }
		}

		/// <summary>
		///     Regisztrálja a kódban tárolt parancsokat.
		/// </summary>
		private void InitHandler()
		{
			RegisterHandler("help",       HandleHelp);
			RegisterHandler("consolelog", HandleConsoleLog);
			RegisterHandler("sys",        HandleSys);
			RegisterHandler("cchannel",   HandleConsoleToChannel);
			RegisterHandler("cserver",    HandleOldServerToNewServer);
			RegisterHandler("admin",      HandleAdmin);
			RegisterHandler("alias",      HandleAlias);
			RegisterHandler("function",   HandleFunction);
			RegisterHandler("channel",    HandleChannel);
			RegisterHandler("connect",    HandleConnect);
			RegisterHandler("disconnect", HandleDisConnect);
			RegisterHandler("reconnect",  HandleReConnect);
			RegisterHandler("nick",       HandleNick);
			RegisterHandler("join",       HandleJoin);
			RegisterHandler("leave",      HandleLeave);
			RegisterHandler("reload",     HandleReload);
			RegisterHandler("ignore",     HandleIgnore);
			RegisterHandler("plugin",     HandlePlugin);
			RegisterHandler("quit",       HandleQuit);

			var db = SchumixBase.DManager.Query("SELECT NewCommand, BaseCommand FROM alias_console_command");
			if(!db.IsNull())
			{
				foreach(DataRow row in db.Rows)
				{
					string newcommand = row["NewCommand"].ToString();
					string basecommand = row["BaseCommand"].ToString();
					RegisterHandler(newcommand, ConsoleMethodMap[basecommand].Method);
				}
			}

			Log.Notice("CCommandManager", sLConsole.GetString("All Command Handler are registered."));
		}

		/// <summary>
		///     Parancs regisztráló függvény.
		/// </summary>
		public static void RegisterHandler(string command, ConsoleDelegate method)
		{
			if(ConsoleMethodMap.ContainsKey(command))
				ConsoleMethodMap[command].Method += method;
			else
				ConsoleMethodMap.Add(command, new ConsoleMethod(method));
		}

		/// <summary>
		///     Parancs eltávolító függvény.
		/// </summary>
		public static void RemoveHandler(string command)
		{
			if(ConsoleMethodMap.ContainsKey(command))
				ConsoleMethodMap.Remove(command);
		}

		/// <summary>
		///     Parancs eltávolító függvény.
		/// </summary>
		public static void RemoveHandler(string command, ConsoleDelegate method)
		{
			if(ConsoleMethodMap.ContainsKey(command))
			{
				ConsoleMethodMap[command].Method -= method;

				if(ConsoleMethodMap[command].Method.IsNull())
					ConsoleMethodMap.Remove(command);
			}
		}

		/// <summary>
		///     A bejövő információkat dolgozza fel és meghívja a parancsot ha létezik olyan.
		/// </summary>
		public bool CIncomingInfo(string info)
		{
			try
			{
				var CMessage = new ConsoleMessage();
				CMessage.Info = info.Split(SchumixBase.Space);
				string cmd = CMessage.Info[0].ToLower();

				if(ConsoleMethodMap.ContainsKey(cmd))
				{
					ConsoleMethodMap[cmd].Method.Invoke(CMessage);
					return true;
				}
				else
					return false;
			}
			catch(Exception e)
			{
				Log.Error("CIncomingInfo", sLConsole.GetString("Failure details: {0}"), e.Message);
				return true;
			}
		}
	}
}