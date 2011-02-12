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
using System.Text.RegularExpressions;
using Schumix.Framework;

namespace Schumix.Irc.Commands
{
	public class CommandManager : CommandHandler
	{
		private static readonly Dictionary<string, Action> _CommandHandler = new Dictionary<string, Action>();

		protected CommandManager()
		{
			Log.Notice("CommandManager", "CommandManager elindult.");
			InitHandler();
		}

		private void InitHandler()
		{
			// Public
			RegisterHandler("xbot",       HandleXbot);
			RegisterHandler("info",       HandleInfo);
			RegisterHandler("help",       HandleHelp);
			RegisterHandler("ido",        HandleIdo);
			RegisterHandler("datum",      HandleDatum);
			RegisterHandler("roll",       HandleRoll);
			RegisterHandler("calc",       HandleCalc);
			RegisterHandler("sha1",       HandleSha1);
			RegisterHandler("md5",        HandleMd5);
			RegisterHandler("irc",        HandleIrc);
			RegisterHandler("whois",      HandleWhois);
			RegisterHandler("uzenet",     HandleUzenet);
			RegisterHandler("keres",      HandleKeres);
			RegisterHandler("fordit",     HandleFordit);
			RegisterHandler("prime",      HandlePrime);

			// Operator
			RegisterHandler("admin",      HandleAdmin);
			RegisterHandler("funkcio",    HandleFunkcio);
			RegisterHandler("channel",    HandleChannel);
			RegisterHandler("sznap",      HandleSznap);
			RegisterHandler("szinek",     HandleSzinek);
			RegisterHandler("nick",       HandleNick);
			RegisterHandler("join",       HandleJoin);
			RegisterHandler("left",       HandleLeft);
			RegisterHandler("kick",       HandleKick);
			RegisterHandler("mode",       HandleMode);

			// Admin
			RegisterHandler("plugin",     HandlePlugin);
			RegisterHandler("kikapcs",    HandleKikapcs);

			Log.Notice("CommandManager", "Osszes Command handler regisztralva.");
		}

		private static void RegisterHandler(string code, Action method)
		{
			_CommandHandler.Add(code, method);
		}

		private static void RemoveHandler(string code)
		{
			_CommandHandler.Remove(code);
		}

		public static void PublicRegisterHandler(string code, Action method)
		{
			RegisterHandler(code, method);
		}

		public static void PublicRemoveHandler(string code)
		{
			RemoveHandler(code);
		}

		protected void BejovoInfo(string handler)
		{
			try
			{
				if(_CommandHandler.ContainsKey(handler))
					_CommandHandler[handler].Invoke();
			}
			catch(Exception e)
			{
				Log.Error("BejovoInfo", "Hiba oka: {0}", e.ToString());
			}
		}
	}
}