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
using Schumix.API;
using Schumix.Framework;

namespace Schumix.Irc.Commands
{
	public class CommandManager : CommandHandler
	{
		private static readonly Dictionary<string, Action<IRCMessage>> _PublicCommandHandler = new Dictionary<string, Action<IRCMessage>>();
		private static readonly Dictionary<string, Action<IRCMessage>> _HalfOperatorCommandHandler = new Dictionary<string, Action<IRCMessage>>();
		private static readonly Dictionary<string, Action<IRCMessage>> _OperatorCommandHandler = new Dictionary<string, Action<IRCMessage>>();
		private static readonly Dictionary<string, Action<IRCMessage>> _AdminCommandHandler = new Dictionary<string, Action<IRCMessage>>();
		public static Dictionary<string, Action<IRCMessage>> GetPublicCommandHandler()
		{
			return _PublicCommandHandler;
		}

		public static Dictionary<string, Action<IRCMessage>> GetHalfOperatorCommandHandler()
		{
			return _HalfOperatorCommandHandler;
		}

		public static Dictionary<string, Action<IRCMessage>> GetOperatorCommandHandler()
		{
			return _OperatorCommandHandler;
		}

		public static Dictionary<string, Action<IRCMessage>> GetAdminCommandHandler()
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
			PublicCRegisterHandler("xbot",         new Action<IRCMessage>(HandleXbot));
			PublicCRegisterHandler("info",         new Action<IRCMessage>(HandleInfo));
			PublicCRegisterHandler("help",         new Action<IRCMessage>(HandleHelp));
			PublicCRegisterHandler("time",         new Action<IRCMessage>(HandleTime));
			PublicCRegisterHandler("date",         new Action<IRCMessage>(HandleDate));
			PublicCRegisterHandler("roll",         new Action<IRCMessage>(HandleRoll));
			PublicCRegisterHandler("calc",         new Action<IRCMessage>(HandleCalc));
			PublicCRegisterHandler("sha1",         new Action<IRCMessage>(HandleSha1));
			PublicCRegisterHandler("md5",          new Action<IRCMessage>(HandleMd5));
			PublicCRegisterHandler("irc",          new Action<IRCMessage>(HandleIrc));
			PublicCRegisterHandler("whois",        new Action<IRCMessage>(HandleWhois));
			PublicCRegisterHandler("warning",      new Action<IRCMessage>(HandleWarning));
			PublicCRegisterHandler("google",       new Action<IRCMessage>(HandleGoogle));
			PublicCRegisterHandler("translate",    new Action<IRCMessage>(HandleTranslate));
			PublicCRegisterHandler("prime",        new Action<IRCMessage>(HandlePrime));
			PublicCRegisterHandler("weather",      new Action<IRCMessage>(HandleWeather));

			// Half Operator
			HalfOperatorCRegisterHandler("admin",  new Action<IRCMessage>(HandleAdmin));
			HalfOperatorCRegisterHandler("colors", new Action<IRCMessage>(HandleColors));
			HalfOperatorCRegisterHandler("nick",   new Action<IRCMessage>(HandleNick));
			HalfOperatorCRegisterHandler("join",   new Action<IRCMessage>(HandleJoin));
			HalfOperatorCRegisterHandler("left",   new Action<IRCMessage>(HandleLeft));

			// Operator
			OperatorCRegisterHandler("function",   new Action<IRCMessage>(HandleFunction));
			OperatorCRegisterHandler("channel",    new Action<IRCMessage>(HandleChannel));
			OperatorCRegisterHandler("sznap",      new Action<IRCMessage>(HandleSznap));
			OperatorCRegisterHandler("kick",       new Action<IRCMessage>(HandleKick));
			OperatorCRegisterHandler("mode",       new Action<IRCMessage>(HandleMode));

			// Admin
			AdminCRegisterHandler("plugin",        new Action<IRCMessage>(HandlePlugin));
			AdminCRegisterHandler("reload",        new Action<IRCMessage>(HandleReload));
			AdminCRegisterHandler("quit",          new Action<IRCMessage>(HandleQuit));

			Log.Notice("CommandManager", "Osszes Command handler regisztralva.");
		}

		public static void PublicCRegisterHandler(string code, Action<IRCMessage> method)
		{
			_PublicCommandHandler.Add(code, method);
		}

		public static void PublicCRemoveHandler(string code)
		{
			_PublicCommandHandler.Remove(code);
		}

		public static void HalfOperatorCRegisterHandler(string code, Action<IRCMessage> method)
		{
			_HalfOperatorCommandHandler.Add(code, method);
		}

		public static void HalfOperatorCRemoveHandler(string code)
		{
			_HalfOperatorCommandHandler.Remove(code);
		}

		public static void OperatorCRegisterHandler(string code, Action<IRCMessage> method)
		{
			_OperatorCommandHandler.Add(code, method);
		}

		public static void OperatorCRemoveHandler(string code)
		{
			_OperatorCommandHandler.Remove(code);
		}

		public static void AdminCRegisterHandler(string code, Action<IRCMessage> method)
		{
			_AdminCommandHandler.Add(code, method);
		}

		public static void AdminCRemoveHandler(string code)
		{
			_AdminCommandHandler.Remove(code);
		}

		protected void IncomingInfo(string handler, IRCMessage sIRCMessage)
		{
			try
			{
				if(_PublicCommandHandler.ContainsKey(handler))
					_PublicCommandHandler[handler].Invoke(sIRCMessage);
				else if(_HalfOperatorCommandHandler.ContainsKey(handler))
					_HalfOperatorCommandHandler[handler].Invoke(sIRCMessage);
				else if(_OperatorCommandHandler.ContainsKey(handler))
					_OperatorCommandHandler[handler].Invoke(sIRCMessage);
				else if(_AdminCommandHandler.ContainsKey(handler))
					_AdminCommandHandler[handler].Invoke(sIRCMessage);
			}
			catch(Exception e)
			{
				Log.Error("IncomingInfo", sLConsole.Exception("Error"), e.Message);
			}
		}
	}
}