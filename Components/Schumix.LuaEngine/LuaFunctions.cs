/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Twl
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
using System.Collections.Generic;
using Schumix.API;
using Schumix.API.Delegate;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Extensions;
using LuaInterface;

namespace Schumix.LuaEngine
{
	/// <summary>
	/// Class containing most of the exported Lua functions.
	/// It as also a wrapper to the Main API.
	/// </summary>
	public sealed class LuaFunctions : CommandInfo
	{
		private readonly Dictionary<string, SchumixCommandMethod> _RegisteredSchumix = new Dictionary<string, SchumixCommandMethod>();
		private readonly Dictionary<string, IRCDelegate> _RegisteredIrc = new Dictionary<string, IRCDelegate>();
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private readonly Lua _lua;

		#region Properites

		/// <summary>
		/// Events registered by Lua on Handler.
		/// </summary>
		public Dictionary<string, IRCDelegate> RegisteredIrc
		{
			get { return _RegisteredIrc; }
		}

		/// <summary>
		/// Events registered by Lua on Command.
		/// </summary>
		public Dictionary<string, SchumixCommandMethod> RegisteredSchumix
		{
			get { return _RegisteredSchumix; }
		}

		#endregion

		/// <summary>
		/// Creates a new instance of <c>LuaFunctions</c>
		/// </summary>
		/// <param name="vm">Lua VM</param>
		/// <param name="conn">IRC connection</param>
		public LuaFunctions(ref Lua vm) : base(string.Empty)
		{
			_lua = vm;
		}

		public void Clean()
		{
			_RegisteredSchumix.Clear();
			_RegisteredIrc.Clear();
		}

		/// <summary>
		/// Registers a function hook.
		/// </summary>
		/// <param name="HandlerName">Irc to listen to.</param>
		/// <param name="LuaName">Lua function to call.</param>
		[LuaFunction("RegisterIrcHook", "Registers a irc hook.")]
		public void RegisterIrcHook(string HandlerName, string LuaName)
		{
			var func = _lua.GetFunction(typeof(IRCDelegate), LuaName);

			if(func.IsNull())
				return;

			var handler = func as IRCDelegate;
			if(_RegisteredIrc.ContainsKey(HandlerName))
				_RegisteredIrc[HandlerName] += handler;
			else
				_RegisteredIrc.Add(HandlerName, handler);

			sIrcBase.IrcRegisterHandler(HandlerName, handler);
		}

		/// <summary>
		/// Registers a public command hook.
		/// </summary>
		/// <param name="CommandName">Command to listen to.</param>
		/// <param name="LuaName">Lua function to call.</param>
		[LuaFunction("RegisterSchumixHook", "Registers a schumix command hook.")]
		public void RegisterSchumixHook(string CommandName, string LuaName, CommandPermission permission = CommandPermission.Normal)
		{
			var func = _lua.GetFunction(typeof(CommandDelegate), LuaName);

			if(func.IsNull())
				return;

			var handler = func as CommandDelegate;
			if(_RegisteredSchumix.ContainsKey(CommandName.ToLower()))
				_RegisteredSchumix[CommandName.ToLower()].Method += handler;
			else
				_RegisteredSchumix.Add(CommandName.ToLower(), new SchumixCommandMethod(handler, permission));

			sIrcBase.SchumixRegisterHandler(CommandName, handler, permission);
		}

		/// <summary>
		/// Sends a message to the IRC server.
		/// </summary>
		/// <param name="message">Message.</param>
		[LuaFunction("WriteLine", "Sends a message to the IRC server.")]
		public void WriteLine(string ServerName, string message)
		{
			sIrcBase.Networks[ServerName].sSendMessage.WriteLine(message);
		}

		/// <summary>
		/// Sends a message to the IRC server.
		/// </summary>
		/// <param name="channel">Channel to send to.</param>
		/// <param name="message">Message.</param>
		[LuaFunction("SendMsg", "Sends a message to the IRC server.")]
		public void SendMessage(string ServerName, string channel, string message)
		{
			sIrcBase.Networks[ServerName].sSendMessage.SendCMPrivmsg(channel, message);
		}

		[LuaFunction("IsAdmin", "Is Admin.")]
		public bool BaseIsAdmin(string ServerName, string Name)
		{
			servername = ServerName;
			return IsAdmin(Name);
		}

		[LuaFunction("IsAdmin2", "Is Admin.")]
		public bool BaseIsAdmin(string ServerName, string Name, int Flag)
		{
			servername = ServerName;
			return IsAdmin(Name, (AdminFlag)Flag);
		}

		[LuaFunction("IsAdmin3", "Is Admin.")]
		public bool BaseIsAdmin(string ServerName, string Name, string Vhost, int Flag)
		{
			servername = ServerName;
			return IsAdmin(Name, Vhost, (AdminFlag)Flag);
		}

		[LuaFunction("IsChannel", "Is Channel.")]
		public bool BaseIsChannel(string ServerName, string Name)
		{
			servername = ServerName;
			return sUtilities.IsChannel(Name);
		}

		[LuaFunction("Adminflag", "Admin rank.")]
		public AdminFlag BaseAdminflag(string ServerName, string Name)
		{
			servername = ServerName;
			return Adminflag(Name);
		}

		[LuaFunction("Adminflag2", "Admin rank.")]
		public AdminFlag BaseAdminflag(string ServerName, string Name, string Vhost)
		{
			servername = ServerName;
			return Adminflag(Name, Vhost);
		}

		[LuaFunction("GetCommandPermission", "Command Permission.")]
		public CommandPermission GetCommandPermission(string permission)
		{
			CommandPermission p = CommandPermission.Normal;

			switch(permission.ToLower())
			{
				case "normal":
					p = CommandPermission.Normal;
					break;
				case "halfoperator":
					p = CommandPermission.HalfOperator;
					break;
				case "operator":
					p = CommandPermission.Operator;
					break;
				case "administrator":
					p = CommandPermission.Administrator;
					break;
			}

			return p;
		}
	}
}