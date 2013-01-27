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
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Schumix.Irc;
using Schumix.Framework;
using Schumix.Framework.Localization;

namespace Schumix.Console.Commands
{
	/// <summary>
	///     ConsoleCommandManager class.
	/// </summary>
	sealed class CCommandManager : CommandHandler
	{
		/// <summary>
		///     Tárolja a parancsokat és a hozzá tartozó függvényeket.
		/// </summary>
		private static readonly Dictionary<string, Action> _CommandHandler = new Dictionary<string, Action>();
		/// <summary>
		///     Hozzáférést biztosít singleton-on keresztül a megadott class-hoz.
		///     LocalizationConsole segítségével állíthatók be a konzol nyelvi tulajdonságai.
		/// </summary>
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		/// <summary>
		///     Kimenetként kiírja a parancsokat és a hozzá tartozó függvényeket.
		/// </summary>
		public static Dictionary<string, Action> GetCommandHandler()
		{
			return _CommandHandler;
		}

		/// <summary>
		///     Indulási függvény.
		/// </summary>
		public CCommandManager() : base()
		{
			Log.Notice("CCommandManager", sLConsole.GetString("Successfully started the CCommandManager."));
			InitHandler();
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

			Log.Notice("CCommandManager", sLConsole.GetString("All Command Handler are registered."));
		}

		/// <summary>
		///     Parancs regisztráló függvény.
		/// </summary>
		private void RegisterHandler(string code, Action method)
		{
			_CommandHandler.Add(code, method);
		}

		/// <summary>
		///     Parancs eltávolító függvény.
		/// </summary>
		private void RemoveHandler(string code)
		{
			_CommandHandler.Remove(code);
		}

		/// <summary>
		///     A bejövő információkat dolgozza fel és meghívja a parancsot ha létezik olyan.
		/// </summary>
		public bool CIncomingInfo(string info)
		{
			try
			{
				Info = info.Split(SchumixBase.Space);
				string cmd = Info[0].ToLower();

				if(_CommandHandler.ContainsKey(cmd))
				{
					_CommandHandler[cmd].Invoke();
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