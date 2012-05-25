/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Twl
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
using System.Collections.Generic;
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
		private readonly Dictionary<string, CommandDelegate> _RegisteredCommand = new Dictionary<string, CommandDelegate>();
		private readonly Dictionary<string, IRCDelegate> _RegisteredHandler = new Dictionary<string, IRCDelegate>();
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly LuaInterface.Lua _lua;
		private readonly Mono.LuaInterface.Lua _monolua;

		#region Properites

		/// <summary>
		/// Events registered by Lua on Handler.
		/// </summary>
		public Dictionary<string, IRCDelegate> RegisteredHandler
		{
			get { return _RegisteredHandler; }
		}

		/// <summary>
		/// Events registered by Lua on Command.
		/// </summary>
		public Dictionary<string, CommandDelegate> RegisteredCommand
		{
			get { return _RegisteredCommand; }
		}

		#endregion

		/// <summary>
		/// Creates a new instance of <c>LuaFunctions</c>
		/// </summary>
		/// <param name="vm">Lua VM</param>
		/// <param name="conn">IRC connection</param>
		public LuaFunctions(ref LuaInterface.Lua vm)
		{
			_lua = vm;
		}

		/// <summary>
		/// Creates a new instance of <c>LuaFunctions</c>
		/// </summary>
		/// <param name="vm">Lua VM</param>
		/// <param name="conn">IRC connection</param>
		public LuaFunctions(ref Mono.LuaInterface.Lua vm)
		{
			_monolua = vm;
		}

		public void Clean()
		{
			_RegisteredCommand.Clear();
			_RegisteredHandler.Clear();
		}

		/// <summary>
		/// Registers a function hook.
		/// </summary>
		/// <param name="HandlerName">Irc to listen to.</param>
		/// <param name="LuaName">Lua function to call.</param>
		[LuaFunction("RegisterHook", "Registers a handler hook.")]
		public void RegisterHandlerHook(string HandlerName, string LuaName)
		{
			if(sUtilities.GetPlatformType() == PlatformType.Windows)
			{
				var func = _lua.GetFunction(typeof(IRCDelegate), LuaName);

				if(func.IsNull())
					return;

				var handler = func as IRCDelegate;
				_RegisteredHandler.Add(HandlerName, handler);
				Network.IrcRegisterHandler(HandlerName, handler);
			}
			else if(sUtilities.GetPlatformType() == PlatformType.Linux)
			{
				var func = _monolua.GetFunction(typeof(IRCDelegate), LuaName);

				if(func.IsNull())
					return;

				var handler = func as IRCDelegate;
				_RegisteredHandler.Add(HandlerName, handler);
				Network.IrcRegisterHandler(HandlerName, handler);
			}
		}

		/// <summary>
		/// Registers a public command hook.
		/// </summary>
		/// <param name="CommandName">Command to listen to.</param>
		/// <param name="LuaName">Lua function to call.</param>
		[LuaFunction("RegisterSchumixCommandHook", "Registers a schumix command hook.")]
		public void RegisterSchumixCommandHook(string CommandName, string LuaName)
		{
			if(sUtilities.GetPlatformType() == PlatformType.Windows)
			{
				var func = _lua.GetFunction(typeof(CommandDelegate), LuaName);

				if(func.IsNull())
					return;

				var handler = func as CommandDelegate;
				_RegisteredCommand.Add(CommandName.ToLower(), handler);
				CommandManager.SchumixRegisterHandler(CommandName, handler);
			}
			else if(sUtilities.GetPlatformType() == PlatformType.Linux)
			{
				var func = _monolua.GetFunction(typeof(CommandDelegate), LuaName);

				if(func.IsNull())
					return;

				var handler = func as CommandDelegate;
				_RegisteredCommand.Add(CommandName.ToLower(), handler);
				CommandManager.SchumixRegisterHandler(CommandName, handler);
			}
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