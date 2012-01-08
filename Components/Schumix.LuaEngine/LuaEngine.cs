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
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Localization;

namespace Schumix.LuaEngine
{
	/// <summary>
	/// Class used to load Lua script files.
	/// This script engine is an optional one and can be disabled by commenting a single line of code.
	/// </summary>
	public sealed class LuaEngine
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly Dictionary<string, LuaFunctionDescriptor> _luaFunctions;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly FileSystemWatcher _watcher;
		private readonly LuaFunctions _functions;
		private readonly string _scriptPath;
		private readonly LuaInterface.Lua _lua;
		private readonly Mono.LuaInterface.Lua _monolua;

		#region Properties

		/// <summary>
		/// Gets the Lua virtual machine.
		/// </summary>
		//public Lua LuaVM
		//{
		//	get { return _lua; }
		//}

		/// <summary>
		/// Gets the Lua virtual machine.
		/// </summary>
		//public Mono.LuaInterface.Lua MonoLuaVM
		//{
		//	get { return _monolua; }
		//}

		#endregion

		/// <summary>
		/// Creates a new instance of <c>LuaEngine</c>
		/// </summary>
		/// <param name="conn">IRC Connection</param>
		/// <param name="scriptsPath">The directory where the Lua scripts are located.</param>
		public LuaEngine(string scriptsPath)
		{
			Log.Notice("LuaEngine", sLConsole.LuaEngine("Text"));

			if(sUtilities.GetCompiler() == Compiler.VisualStudio)
				_lua = new LuaInterface.Lua();
			else if(sUtilities.GetCompiler() == Compiler.Mono)
				_monolua = new Mono.LuaInterface.Lua();

			_luaFunctions = new Dictionary<string, LuaFunctionDescriptor>();
			_scriptPath = scriptsPath;

			if(sUtilities.GetCompiler() == Compiler.VisualStudio)
			{
				_functions = new LuaFunctions(ref _lua);
				LuaHelper.RegisterLuaFunctions(_lua, ref _luaFunctions, _functions);
			}
			else if(sUtilities.GetCompiler() == Compiler.Mono)
			{
				_functions = new LuaFunctions(ref _monolua);
				LuaHelper.RegisterLuaFunctions(_monolua, ref _luaFunctions, _functions);
			}
			else
			{
				_functions = new LuaFunctions(ref _monolua);
				LuaHelper.RegisterLuaFunctions(_monolua, ref _luaFunctions, _functions);
			}

			LoadScripts();

			_watcher = new FileSystemWatcher(_scriptPath)
			{
				NotifyFilter = NotifyFilters.FileName | NotifyFilters.Attributes | NotifyFilters.LastAccess |
				NotifyFilters.LastWrite | NotifyFilters.Security | NotifyFilters.Size,
				EnableRaisingEvents = true
			};

			_watcher.Created += (s, e) => LoadScripts(true);
			_watcher.Changed += (s, e) => LoadScripts(true);
			_watcher.Deleted += (s, e) => LoadScripts(true);
			_watcher.Renamed += (s, e) => LoadScripts(true);
		}

		/// <summary>
		/// Loads the Lua scripts.
		/// </summary>
		/// <param name="reload">Is it a reload or not.</param>
		public void LoadScripts(bool reload = false)
		{
			if(reload)
			{
				foreach(var func in _functions.RegisteredCommand)
				{
					if(CommandManager.GetPublicCommandHandler().ContainsKey(func.Key))
						CommandManager.PublicCRemoveHandler(func.Key, func.Value);
					else if(CommandManager.GetHalfOperatorCommandHandler().ContainsKey(func.Key))
						CommandManager.HalfOperatorCRemoveHandler(func.Key, func.Value);
					else if(CommandManager.GetOperatorCommandHandler().ContainsKey(func.Key))
						CommandManager.OperatorCRemoveHandler(func.Key, func.Value);
					else if(CommandManager.GetAdminCommandHandler().ContainsKey(func.Key))
						CommandManager.AdminCRemoveHandler(func.Key, func.Value);
				}

				foreach(var func in _functions.RegisteredHandler)
					Network.PublicRemoveHandler(func.Key, func.Value);
			}

			var di = new DirectoryInfo(_scriptPath);

			foreach(var file in di.GetFiles("*.lua").AsParallel())
			{
				Log.Notice("LuaEngine", sLConsole.LuaEngine("Text2"), file.Name);

				try
				{
					if(sUtilities.GetCompiler() == Compiler.VisualStudio)
						_lua.DoFile(file.FullName);
					else if(sUtilities.GetCompiler() == Compiler.Mono)
						_monolua.DoFile(file.FullName);
				}
				catch(Exception x)
				{
					Log.Error("LuaEngine", sLConsole.LuaEngine("Text3"), file.Name, x);
				}
			}
		}

		/// <summary>
		/// Frees up resources.
		/// </summary>
		public void Free()
		{
			if(sUtilities.GetCompiler() == Compiler.VisualStudio)
				_lua.Dispose();
			else if(sUtilities.GetCompiler() == Compiler.Mono)
				_monolua.Dispose();

			_functions.Clean();
		}
	}
}