/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2011 Megax <http://www.megaxx.info/>
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
using System.Linq;
using System.Collections.Generic;
using Schumix.Irc;
using Schumix.Framework;

namespace Schumix.Console.Commands
{
	public sealed class CCommandManager : CommandHandler
	{
		private static readonly Dictionary<string, Action> _CommandHandler = new Dictionary<string, Action>();
		public static Dictionary<string, Action> GetCommandHandler()
		{
			return _CommandHandler;
		}

		public CCommandManager(Network network) : base(network)
		{
			Log.Notice("CCommandManager", "CCommandManager elindult.");
			InitHandler();
		}

		private void InitHandler()
		{
			RegisterHandler("help",       HandleHelp);
			RegisterHandler("consolelog", HandleConsoleLog);
			RegisterHandler("sys",        HandleSys);
			RegisterHandler("csatorna",   HandleCsatorna);
			RegisterHandler("admin",      HandleAdmin);
			RegisterHandler("funkcio",    HandleFunkcio);
			RegisterHandler("channel",    HandleChannel);
			RegisterHandler("connect",    HandleConnect);
			RegisterHandler("disconnect", HandleDisConnect);
			RegisterHandler("reconnect",  HandleReConnect);
			RegisterHandler("nick",       HandleNick);
			RegisterHandler("join",       HandleJoin);
			RegisterHandler("left",       HandleLeft);
			RegisterHandler("kikapcs",    HandleKikapcs);

			Log.Notice("CCommandManager", "Osszes Command handler regisztralva.");
		}

		private void RegisterHandler(string code, Action method)
		{
			_CommandHandler.Add(code, method);
		}

		private void RemoveHandler(string code)
		{
			_CommandHandler.Remove(code);
		}

		public bool CBejovoInfo(string info)
		{
			try
			{
				Info = info.Split(' ');
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
				Log.Error("CBejovoInfo", "Hiba oka: {0}", e.ToString());
				return false;
			}
		}
	}
}