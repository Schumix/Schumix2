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
using Schumix.Framework;

namespace Schumix.Irc.Commands
{
	public class CommandManager : CommandHandler
	{
		private static readonly Dictionary<string, Action> _PublicCommandHandler = new Dictionary<string, Action>();
		private static readonly Dictionary<string, Action> _HalfOperatorCommandHandler = new Dictionary<string, Action>();
		private static readonly Dictionary<string, Action> _OperatorCommandHandler = new Dictionary<string, Action>();
		private static readonly Dictionary<string, Action> _AdminCommandHandler = new Dictionary<string, Action>();
		public static Dictionary<string, Action> GetPublicCommandHandler()
		{
			return _PublicCommandHandler;
		}

		public static Dictionary<string, Action> GetHalfOperatorCommandHandler()
		{
			return _HalfOperatorCommandHandler;
		}

		public static Dictionary<string, Action> GetOperatorCommandHandler()
		{
			return _OperatorCommandHandler;
		}

		public static Dictionary<string, Action> GetAdminCommandHandler()
		{
			return _AdminCommandHandler;
		}

		protected CommandManager()
		{
			Log.Notice("CommandManager", "CommandManager sikeresen elindult.");
			InitHandler();
		}

		private void InitHandler()
		{
			// Public
			PublicCRegisterHandler("xbot",         HandleXbot);
			PublicCRegisterHandler("info",         HandleInfo);
			PublicCRegisterHandler("help",         HandleHelp);
			PublicCRegisterHandler("ido",          HandleIdo);
			PublicCRegisterHandler("datum",        HandleDatum);
			PublicCRegisterHandler("roll",         HandleRoll);
			PublicCRegisterHandler("calc",         HandleCalc);
			PublicCRegisterHandler("sha1",         HandleSha1);
			PublicCRegisterHandler("md5",          HandleMd5);
			PublicCRegisterHandler("irc",          HandleIrc);
			PublicCRegisterHandler("whois",        HandleWhois);
			PublicCRegisterHandler("uzenet",       HandleUzenet);
			PublicCRegisterHandler("keres",        HandleKeres);
			PublicCRegisterHandler("fordit",       HandleFordit);
			PublicCRegisterHandler("prime",        HandlePrime);

			// Half Operator
			HalfOperatorCRegisterHandler("admin",  HandleAdmin);
			HalfOperatorCRegisterHandler("szinek", HandleSzinek);
			HalfOperatorCRegisterHandler("nick",   HandleNick);
			HalfOperatorCRegisterHandler("join",   HandleJoin);
			HalfOperatorCRegisterHandler("left",   HandleLeft);

			// Operator
			OperatorCRegisterHandler("funkcio",    HandleFunkcio);
			OperatorCRegisterHandler("channel",    HandleChannel);
			OperatorCRegisterHandler("sznap",      HandleSznap);
			OperatorCRegisterHandler("kick",       HandleKick);
			OperatorCRegisterHandler("mode",       HandleMode);

			// Admin
			AdminCRegisterHandler("plugin",        HandlePlugin);
			AdminCRegisterHandler("kikapcs",       HandleKikapcs);

			Log.Notice("CommandManager", "Osszes Command handler regisztralva.");
		}

		public static void PublicCRegisterHandler(string code, Action method)
		{
			_PublicCommandHandler.Add(code, method);
		}

		public static void PublicCRemoveHandler(string code)
		{
			_PublicCommandHandler.Remove(code);
		}

		public static void HalfOperatorCRegisterHandler(string code, Action method)
		{
			_HalfOperatorCommandHandler.Add(code, method);
		}

		public static void HalfOperatorCRemoveHandler(string code)
		{
			_HalfOperatorCommandHandler.Remove(code);
		}

		public static void OperatorCRegisterHandler(string code, Action method)
		{
			_OperatorCommandHandler.Add(code, method);
		}

		public static void OperatorCRemoveHandler(string code)
		{
			_OperatorCommandHandler.Remove(code);
		}

		public static void AdminCRegisterHandler(string code, Action method)
		{
			_AdminCommandHandler.Add(code, method);
		}

		public static void AdminCRemoveHandler(string code)
		{
			_AdminCommandHandler.Remove(code);
		}

		protected void BejovoInfo(string handler)
		{
			try
			{
				if(_PublicCommandHandler.ContainsKey(handler))
					_PublicCommandHandler[handler].Invoke();
				else if(_HalfOperatorCommandHandler.ContainsKey(handler))
					_HalfOperatorCommandHandler[handler].Invoke();
				else if(_OperatorCommandHandler.ContainsKey(handler))
					_OperatorCommandHandler[handler].Invoke();
				else if(_AdminCommandHandler.ContainsKey(handler))
					_AdminCommandHandler[handler].Invoke();
			}
			catch(Exception e)
			{
				Log.Error("BejovoInfo", "Hiba oka: {0}", e.Message);
			}
		}
	}
}