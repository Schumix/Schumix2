/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013-2014 Schumix Team <http://schumix.eu/>
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
using System.Reflection;
using System.Globalization;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Schumix.Irc.Commands.Method;
using Schumix.Framework;
using Schumix.Framework.Irc;
using Schumix.Framework.Addon;
using Schumix.Framework.Logger;
using Schumix.Framework.Delegate;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Irc.Commands
{
	public abstract class CommandManager : CommandHandler
	{
		public readonly Dictionary<string, CommandMethod> CommandMethodMap = new Dictionary<string, CommandMethod>();
		private readonly object MapLock = new object();
		private string _servername;

		protected CommandManager(string ServerName) : base(ServerName)
		{
			_servername = ServerName;
		}

		~CommandManager()
		{
			Log.Debug("CommandManager", "~CommandManager() {0}", sLConsole.GetString("[ServerName: {0}]", _servername));
		}

		public void InitializeCommandMgr()
		{
			Log.Notice("CommandManager", sLConsole.GetString("Successfully started the CommandManager."));
			CreateMappings();
			sAntiFlood.Start();
		}

		private void CreateMappings(bool Reload = false)
		{
			// Public
			SchumixRegisterHandler("xbot",         HandleXbot);
			SchumixRegisterHandler("info",         HandleInfo);
			SchumixRegisterHandler("help",         HandleHelp);
			SchumixRegisterHandler("time",         HandleTime);
			SchumixRegisterHandler("date",         HandleDate);
			SchumixRegisterHandler("irc",          HandleIrc);
			SchumixRegisterHandler("whois",        HandleWhois);
			SchumixRegisterHandler("warning",      HandleWarning);
			SchumixRegisterHandler("google",       HandleGoogle);
			SchumixRegisterHandler("translate",    HandleTranslate);
			SchumixRegisterHandler("online",       HandleOnline);

			// Half Operator
			SchumixRegisterHandler("admin",        HandleAdmin,    CommandPermission.HalfOperator);
			SchumixRegisterHandler("colors",       HandleColors,   CommandPermission.HalfOperator);
			SchumixRegisterHandler("nick",         HandleNick,     CommandPermission.HalfOperator);
			SchumixRegisterHandler("join",         HandleJoin,     CommandPermission.HalfOperator);
			SchumixRegisterHandler("leave",        HandleLeave,    CommandPermission.HalfOperator);

			// Operator
			SchumixRegisterHandler("function",     HandleFunction, CommandPermission.Operator);
			SchumixRegisterHandler("channel",      HandleChannel,  CommandPermission.Operator);
			SchumixRegisterHandler("kick",         HandleKick,     CommandPermission.Operator);
			SchumixRegisterHandler("mode",         HandleMode,     CommandPermission.Operator);
			SchumixRegisterHandler("ignore",       HandleIgnore,   CommandPermission.Operator);

			// Admin
			SchumixRegisterHandler("plugin",       HandlePlugin,   CommandPermission.Administrator);
			SchumixRegisterHandler("reload",       HandleReload,   CommandPermission.Administrator);
			SchumixRegisterHandler("quit",         HandleQuit,     CommandPermission.Administrator);

			Task.Factory.StartNew(() =>
			{
				var tasm = Assembly.GetExecutingAssembly();
				var asms = sAddonManager.Addons[_servername].Assemblies.ToDictionary(v => v.Key, v => v.Value);
				asms.Add("currentassembly", tasm);
				int i = 0;

				foreach(var a in from asm in AppDomain.CurrentDomain.GetAssemblies()
						where asm.GetName().FullName.ToLower(CultureInfo.InvariantCulture).Contains("schumix")
						select asm)
				{
					i++;
					asms.Add("currentassembly" + i.ToString(), a);
				}

				Parallel.ForEach(asms, asm =>
				{
					var types = asm.Value.GetTypes();
					Parallel.ForEach(types, type =>
					{
						var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
						ProcessMethods(methods);
					});
				});
			});

			if(!Reload)
				Log.Notice("CommandManager", sLConsole.GetString("Successfuly registered all of Command Handlers."));
		}

		private void DeleteMappings(bool Reload = false)
		{
			// Public
			SchumixRemoveHandler("xbot",          HandleXbot);
			SchumixRemoveHandler("info",          HandleInfo);
			SchumixRemoveHandler("help",          HandleHelp);
			SchumixRemoveHandler("time",          HandleTime);
			SchumixRemoveHandler("date",          HandleDate);
			SchumixRemoveHandler("irc",           HandleIrc);
			SchumixRemoveHandler("whois",         HandleWhois);
			SchumixRemoveHandler("warning",       HandleWarning);
			SchumixRemoveHandler("google",        HandleGoogle);
			SchumixRemoveHandler("translate",     HandleTranslate);
			SchumixRemoveHandler("online",        HandleOnline);

			// Half Operator
			SchumixRemoveHandler("admin",         HandleAdmin);
			SchumixRemoveHandler("colors",        HandleColors);
			SchumixRemoveHandler("nick",          HandleNick);
			SchumixRemoveHandler("join",          HandleJoin);
			SchumixRemoveHandler("leave",         HandleLeave);

			// Operator
			SchumixRemoveHandler("function",      HandleFunction);
			SchumixRemoveHandler("channel",       HandleChannel);
			SchumixRemoveHandler("kick",          HandleKick);
			SchumixRemoveHandler("mode",          HandleMode);
			SchumixRemoveHandler("ignore",        HandleIgnore);

			// Admin
			SchumixRemoveHandler("plugin",        HandlePlugin);
			SchumixRemoveHandler("reload",        HandleReload);
			SchumixRemoveHandler("quit",          HandleQuit);

			if(SchumixBase.ExitStatus)
				CommandMethodMap.Clear();

			if(!Reload)
				Log.Notice("CommandManager", sLConsole.GetString("All Command Handler were deleted."));
		}

		private void ProcessMethods(IEnumerable<MethodInfo> methods)
		{
			Parallel.ForEach(methods, method =>
			{
				foreach(var attribute in Attribute.GetCustomAttributes(method))
				{
					if(attribute.IsOfType(typeof(SchumixCommandAttribute)))
					{
						var attr = (SchumixCommandAttribute)attribute;
						lock(MapLock)
						{
							var del = Delegate.CreateDelegate(typeof(CommandDelegate), method) as CommandDelegate;
							SchumixRegisterHandler(attr.Command, del, attr.Permission);
						}
					}
				}
			});
		}

		public void SchumixRegisterHandler(string code, CommandDelegate method, CommandPermission permission = CommandPermission.Normal)
		{
			if(sIgnoreCommand.IsIgnore(code))
			   return;

			if(CommandMethodMap.ContainsKey(code.ToLower()))
				CommandMethodMap[code.ToLower()].Method += method;
			else
				CommandMethodMap.Add(code.ToLower(), new CommandMethod(method, permission));
		}

		public void SchumixRemoveHandler(string code)
		{
			if(CommandMethodMap.ContainsKey(code.ToLower()))
				CommandMethodMap.Remove(code.ToLower());
		}

		public void SchumixRemoveHandler(string code, CommandDelegate method)
		{
			if(CommandMethodMap.ContainsKey(code.ToLower()))
			{
				CommandMethodMap[code.ToLower()].Method -= method;

				if(CommandMethodMap[code.ToLower()].Method.IsNull())
					CommandMethodMap.Remove(code.ToLower());
			}
		}

		protected void IncomingInfo(string handler, IRCMessage sIRCMessage)
		{
			try
			{
				sAntiFlood.FloodCommand(sIRCMessage);

				if(sIgnoreCommand.IsIgnore(handler))
					return;

				if(sAntiFlood.Ignore(sIRCMessage))
					return;

				if(CommandMethodMap.ContainsKey(handler))
					CommandMethodMap[handler].Method.Invoke(sIRCMessage);
			}
			catch(Exception e)
			{
				Log.Error("IncomingInfo", sLConsole.GetString("Failure details: {0}"), e.Message);
			}
		}
	}
}