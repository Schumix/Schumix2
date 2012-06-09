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
		///     Hozzáférést biztosít singleton-on keresztül a megadott class-hoz.
		///     LocalizationConsole segítségével állíthatók be a konzol nyelvi tulajdonságai.
		/// </summary>
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		/// <summary>
		///     Tárolja a parancsokat és a hozzá tartozó függvényeket.
		/// </summary>
		private static readonly Dictionary<string, Action> _CommandHandler = new Dictionary<string, Action>();
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
		public CCommandManager(Network network) : base(network)
		{
			Log.Notice("CCommandManager", sLConsole.CCommandManager("Text"));
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

		/// <summary>
		///     Regisztrálja a kódban tárolt parancsokat.
		/// </summary>
		private void InitHandler()
		{
			RegisterHandler("help",       HandleHelp);
			RegisterHandler("consolelog", HandleConsoleLog);
			RegisterHandler("sys",        HandleSys);
			RegisterHandler("csatorna",   HandleCsatorna);
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

			Log.Notice("CCommandManager", sLConsole.CCommandManager("Text2"));
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
				Log.Error("CIncomingInfo", sLConsole.Exception("Error"), e.Message);
				return true;
			}
		}
	}
}