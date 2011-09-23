/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2011 Twl
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
using System.Collections.Generic;
using LuaInterface;
using Schumix.API;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Extensions;

namespace Schumix.LuaEngine
{
	/// <summary>
	/// Class containing most of the exported Lua functions.
	/// It as also a wrapper to the Main API.
	/// </summary>
	public sealed class LuaFunctions : CommandInfo
	{
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;
		private readonly List<string> _RegisteredCommand = new List<string>();
		private readonly List<string> _RegisteredHandler = new List<string>();
		private readonly Lua _lua;

		#region Properites

		/// <summary>
		/// Events registered by Lua on Handler.
		/// </summary>
		public IEnumerable<string> RegisteredHandler
		{
			get { return _RegisteredHandler; }
		}

		/// <summary>
		/// Events registered by Lua on Command.
		/// </summary>
		public IEnumerable<string> RegisteredCommand
		{
			get { return _RegisteredCommand; }
		}

		#endregion

		/// <summary>
		/// Creates a new instance of <c>LuaFunctions</c>
		/// </summary>
		/// <param name="vm">Lua VM</param>
		/// <param name="conn">IRC connection</param>
		public LuaFunctions(ref Lua vm)
		{
			_lua = vm;
		}

		/// <summary>
		/// Registers a function hook.
		/// </summary>
		/// <param name="HandlerName">Irc to listen to.</param>
		/// <param name="LuaName">Lua function to call.</param>
		[LuaFunction("RegisterHook", "Registers a handler hook.")]
		public void RegisterHandlerHook(string HandlerName, string LuaName)
		{
			var func = _lua.GetFunction(typeof(Action<IRCMessage>), LuaName);

			if(func.IsNull())
				return;

			var handler = func as Action<IRCMessage>;
			_RegisteredHandler.Add(HandlerName.ToLower());
			Network.PublicRegisterHandler(HandlerName.ToLower(), handler);
		}

		/// <summary>
		/// Registers a public command hook.
		/// </summary>
		/// <param name="CommandName">Command to listen to.</param>
		/// <param name="LuaName">Lua function to call.</param>
		[LuaFunction("RegisterPublicCommandHook", "Registers a public command hook.")]
		public void RegisterPublicCommandHook(string CommandName, string LuaName)
		{
			var func = _lua.GetFunction(typeof(Action<IRCMessage>), LuaName);

			if(func.IsNull())
				return;

			var handler = func as Action<IRCMessage>;
			_RegisteredCommand.Add(CommandName.ToLower());
			CommandManager.PublicCRegisterHandler(CommandName.ToLower(), handler);
		}

		/// <summary>
		/// Registers a halfoperator command hook.
		/// </summary>
		/// <param name="CommandName">Command to listen to.</param>
		/// <param name="LuaName">Lua function to call.</param>
		[LuaFunction("RegisterHalfOperatorCommandHook", "Registers a halfoperator command hook.")]
		public void RegisterHalfOperatorCommandHook(string CommandName, string LuaName)
		{
			var func = _lua.GetFunction(typeof(Action<IRCMessage>), LuaName);

			if(func.IsNull())
				return;

			var handler = func as Action<IRCMessage>;
			_RegisteredCommand.Add(CommandName.ToLower());
			CommandManager.HalfOperatorCRegisterHandler(CommandName.ToLower(), handler);
		}

		/// <summary>
		/// Registers a operator command hook.
		/// </summary>
		/// <param name="CommandName">Command to listen to.</param>
		/// <param name="LuaName">Lua function to call.</param>
		[LuaFunction("RegisterOperatorCommandHook", "Registers a operator command hook.")]
		public void RegisterOperatorCommandHook(string CommandName, string LuaName)
		{
			var func = _lua.GetFunction(typeof(Action<IRCMessage>), LuaName);

			if(func.IsNull())
				return;

			var handler = func as Action<IRCMessage>;
			_RegisteredCommand.Add(CommandName.ToLower());
			CommandManager.OperatorCRegisterHandler(CommandName.ToLower(), handler);
		}

		/// <summary>
		/// Registers a admin command hook.
		/// </summary>
		/// <param name="CommandName">Command to listen to.</param>
		/// <param name="LuaName">Lua function to call.</param>
		[LuaFunction("RegisterAdminCommandHook", "Registers a admin command hook.")]
		public void RegisterAdminCommandHook(string CommandName, string LuaName)
		{
			var func = _lua.GetFunction(typeof(Action<IRCMessage>), LuaName);

			if(func.IsNull())
				return;

			var handler = func as Action<IRCMessage>;
			_RegisteredCommand.Add(CommandName.ToLower());
			CommandManager.AdminCRegisterHandler(CommandName.ToLower(), handler);
		}

		/// <summary>
		/// Sends a message to the IRC server.
		/// </summary>
		/// <param name="message">Message.</param>
		[LuaFunction("WriteLine", "Sends a message to the IRC server.")]
		public void WriteLine(string message)
		{
			sSendMessage.WriteLine(message);
		}

		/// <summary>
		/// Sends a message to the IRC server.
		/// </summary>
		/// <param name="channel">Channel to send to.</param>
		/// <param name="message">Message.</param>
		[LuaFunction("SendMsg", "Sends a message to the IRC server.")]
		public void SendMessage(string channel, string message)
		{
			sSendMessage.SendCMPrivmsg(channel, message);
		}

		[LuaFunction("IsAdmin", "Is Admin.")]
		public bool BaseIsAdmin(string Name)
		{
			return IsAdmin(Name);
		}

		[LuaFunction("IsAdmin2", "Is Admin.")]
		public bool BaseIsAdmin(string Name, int Flag)
		{
			return IsAdmin(Name, (AdminFlag)Flag);
		}

		[LuaFunction("IsAdmin3", "Is Admin.")]
		public bool BaseIsAdmin(string Name, string Vhost, int Flag)
		{
			return IsAdmin(Name, Vhost, (AdminFlag)Flag);
		}

		[LuaFunction("IsChannel", "Is Channel.")]
		public bool BaseIsChannel(string Name)
		{
			return IsChannel(Name);
		}

		[LuaFunction("Adminflag", "Admin rank.")]
		public int BaseAdminflag(string Name)
		{
			return Adminflag(Name);
		}

		[LuaFunction("Adminflag2", "Admin rank.")]
		public int BaseAdminflag(string Name, string Vhost)
		{
			return Adminflag(Name, Vhost);
		}
	}
}