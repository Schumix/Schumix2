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
using System.Linq;
using System.Collections.Generic;
using Schumix.API;
using Schumix.Framework;
using Schumix.Framework.Localization;

namespace Schumix.Irc.Commands
{
	public class CommandManager : CommandHandler
	{
		private static readonly Dictionary<string, CommandDelegate> _PublicCommandHandler = new Dictionary<string, CommandDelegate>();
		private static readonly Dictionary<string, CommandDelegate> _HalfOperatorCommandHandler = new Dictionary<string, CommandDelegate>();
		private static readonly Dictionary<string, CommandDelegate> _OperatorCommandHandler = new Dictionary<string, CommandDelegate>();
		private static readonly Dictionary<string, CommandDelegate> _AdminCommandHandler = new Dictionary<string, CommandDelegate>();

		public static Dictionary<string, CommandDelegate> GetPublicCommandHandler()
		{
			return _PublicCommandHandler;
		}

		public static Dictionary<string, CommandDelegate> GetHalfOperatorCommandHandler()
		{
			return _HalfOperatorCommandHandler;
		}

		public static Dictionary<string, CommandDelegate> GetOperatorCommandHandler()
		{
			return _OperatorCommandHandler;
		}

		public static Dictionary<string, CommandDelegate> GetAdminCommandHandler()
		{
			return _AdminCommandHandler;
		}

		protected CommandManager()
		{
			Log.Notice("CommandManager", sLConsole.CommandManager("Text"));
			InitHandler();
			sAntiFlood.Start();
		}

		private void InitHandler(bool Reload = false)
		{
			// Public
			PublicCRegisterHandler("xbot",         HandleXbot);
			PublicCRegisterHandler("info",         HandleInfo);
			PublicCRegisterHandler("help",         HandleHelp);
			PublicCRegisterHandler("time",         HandleTime);
			PublicCRegisterHandler("date",         HandleDate);
			PublicCRegisterHandler("irc",          HandleIrc);
			PublicCRegisterHandler("whois",        HandleWhois);
			PublicCRegisterHandler("warning",      HandleWarning);
			PublicCRegisterHandler("google",       HandleGoogle);
			PublicCRegisterHandler("translate",    HandleTranslate);
			PublicCRegisterHandler("online",       HandleOnline);

			// Half Operator
			HalfOperatorCRegisterHandler("admin",  HandleAdmin);
			HalfOperatorCRegisterHandler("colors", HandleColors);
			HalfOperatorCRegisterHandler("nick",   HandleNick);
			HalfOperatorCRegisterHandler("join",   HandleJoin);
			HalfOperatorCRegisterHandler("leave",  HandleLeave);

			// Operator
			OperatorCRegisterHandler("function",   HandleFunction);
			OperatorCRegisterHandler("channel",    HandleChannel);
			OperatorCRegisterHandler("sznap",      HandleSznap);
			OperatorCRegisterHandler("kick",       HandleKick);
			OperatorCRegisterHandler("mode",       HandleMode);
			OperatorCRegisterHandler("ignore",     HandleIgnore);

			// Admin
			AdminCRegisterHandler("plugin",        HandlePlugin);
			AdminCRegisterHandler("reload",        HandleReload);
			AdminCRegisterHandler("quit",          HandleQuit);

			if(!Reload)
				Log.Notice("CommandManager", sLConsole.CommandManager("Text2"));
		}

		private void RemoveHandler(bool Reload = false)
		{
			// Public
			PublicCRemoveHandler("xbot",         HandleXbot);
			PublicCRemoveHandler("info",         HandleInfo);
			PublicCRemoveHandler("help",         HandleHelp);
			PublicCRemoveHandler("time",         HandleTime);
			PublicCRemoveHandler("date",         HandleDate);
			PublicCRemoveHandler("irc",          HandleIrc);
			PublicCRemoveHandler("whois",        HandleWhois);
			PublicCRemoveHandler("warning",      HandleWarning);
			PublicCRemoveHandler("google",       HandleGoogle);
			PublicCRemoveHandler("translate",    HandleTranslate);
			PublicCRemoveHandler("online",       HandleOnline);

			// Half Operator
			HalfOperatorCRemoveHandler("admin",  HandleAdmin);
			HalfOperatorCRemoveHandler("colors", HandleColors);
			HalfOperatorCRemoveHandler("nick",   HandleNick);
			HalfOperatorCRemoveHandler("join",   HandleJoin);
			HalfOperatorCRemoveHandler("leave",  HandleLeave);

			// Operator
			OperatorCRemoveHandler("function",   HandleFunction);
			OperatorCRemoveHandler("channel",    HandleChannel);
			OperatorCRemoveHandler("sznap",      HandleSznap);
			OperatorCRemoveHandler("kick",       HandleKick);
			OperatorCRemoveHandler("mode",       HandleMode);
			OperatorCRemoveHandler("ignore",     HandleIgnore);

			// Admin
			AdminCRemoveHandler("plugin",        HandlePlugin);
			AdminCRemoveHandler("reload",        HandleReload);
			AdminCRemoveHandler("quit",          HandleQuit);

			if(!Reload)
				Log.Notice("CommandManager", sLConsole.CommandManager("Text3"));
		}

		public static void PublicCRegisterHandler(string code, CommandDelegate method)
		{
			if(sIgnoreCommand.IsIgnore(code))
			   return;

			if(_PublicCommandHandler.ContainsKey(code.ToLower()))
				_PublicCommandHandler[code.ToLower()] += method;
			else
				_PublicCommandHandler.Add(code.ToLower(), method);
		}

		public static void PublicCRemoveHandler(string code)
		{
			if(_PublicCommandHandler.ContainsKey(code.ToLower()))
				_PublicCommandHandler.Remove(code.ToLower());
		}

		public static void PublicCRemoveHandler(string code, CommandDelegate method)
		{
			if(_PublicCommandHandler.ContainsKey(code.ToLower()))
				_PublicCommandHandler[code.ToLower()] -= method;
		}

		public static void HalfOperatorCRegisterHandler(string code, CommandDelegate method)
		{
			if(sIgnoreCommand.IsIgnore(code))
			   return;

			if(_HalfOperatorCommandHandler.ContainsKey(code.ToLower()))
				_HalfOperatorCommandHandler[code.ToLower()] += method;
			else
				_HalfOperatorCommandHandler.Add(code.ToLower(), method);
		}

		public static void HalfOperatorCRemoveHandler(string code)
		{
			if(_HalfOperatorCommandHandler.ContainsKey(code.ToLower()))
				_HalfOperatorCommandHandler.Remove(code.ToLower());
		}

		public static void HalfOperatorCRemoveHandler(string code, CommandDelegate method)
		{
			if(_HalfOperatorCommandHandler.ContainsKey(code.ToLower()))
				_HalfOperatorCommandHandler[code.ToLower()] -= method;
		}

		public static void OperatorCRegisterHandler(string code, CommandDelegate method)
		{
			if(sIgnoreCommand.IsIgnore(code))
			   return;

			if(_OperatorCommandHandler.ContainsKey(code.ToLower()))
				_OperatorCommandHandler[code.ToLower()] += method;
			else
				_OperatorCommandHandler.Add(code.ToLower(), method);
		}

		public static void OperatorCRemoveHandler(string code)
		{
			if(_OperatorCommandHandler.ContainsKey(code.ToLower()))
				_OperatorCommandHandler.Remove(code.ToLower());
		}

		public static void OperatorCRemoveHandler(string code, CommandDelegate method)
		{
			if(_OperatorCommandHandler.ContainsKey(code.ToLower()))
				_OperatorCommandHandler[code.ToLower()] -= method;
		}

		public static void AdminCRegisterHandler(string code, CommandDelegate method)
		{
			if(sIgnoreCommand.IsIgnore(code))
			   return;

			if(_AdminCommandHandler.ContainsKey(code.ToLower()))
				_AdminCommandHandler[code.ToLower()] += method;
			else
				_AdminCommandHandler.Add(code.ToLower(), method);
		}

		public static void AdminCRemoveHandler(string code)
		{
			if(_AdminCommandHandler.ContainsKey(code.ToLower()))
				_AdminCommandHandler.Remove(code.ToLower());
		}

		public static void AdminCRemoveHandler(string code, CommandDelegate method)
		{
			if(_AdminCommandHandler.ContainsKey(code.ToLower()))
				_AdminCommandHandler[code.ToLower()] -= method;
		}

		protected void IncomingInfo(string handler, IRCMessage sIRCMessage)
		{
			try
			{
				if(sIgnoreCommand.IsIgnore(handler))
					return;

				if(sAntiFlood.Ignore(sIRCMessage))
					return;

				if(_PublicCommandHandler.ContainsKey(handler))
				{
					_PublicCommandHandler[handler].Invoke(sIRCMessage);
					sAntiFlood.FloodCommand(sIRCMessage);
				}
				else if(_HalfOperatorCommandHandler.ContainsKey(handler))
				{
					_HalfOperatorCommandHandler[handler].Invoke(sIRCMessage);
					sAntiFlood.FloodCommand(sIRCMessage);
				}
				else if(_OperatorCommandHandler.ContainsKey(handler))
				{
					_OperatorCommandHandler[handler].Invoke(sIRCMessage);
					sAntiFlood.FloodCommand(sIRCMessage);
				}
				else if(_AdminCommandHandler.ContainsKey(handler))
				{
					_AdminCommandHandler[handler].Invoke(sIRCMessage);
					sAntiFlood.FloodCommand(sIRCMessage);
				}
			}
			catch(Exception e)
			{
				Log.Error("IncomingInfo", sLConsole.Exception("Error"), e.Message);
			}
		}
	}
}