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
using System.IO;
using System.Linq;
using LuaInterface;

namespace Schumix.LuaEngine
{
    /// <summary>
    /// Class used to load Lua script files.
    /// This script engine is an optional one and can be disabled by commenting a single line of code.
    /// </summary>
    public sealed class LuaEngine
    {
        private readonly Lua _lua;
        private readonly Dictionary<string, LuaFunctionDescriptor> _luaFunctions;
        private readonly string _scriptPath;
        private readonly LuaFunctions _functions;
        //private readonly Connection _connection;
        private readonly FileSystemWatcher _watcher;

        #region Properties

        /// <summary>
        /// Gets the Lua virtual machine.
        /// </summary>
        public Lua LuaVM
        {
            get
            {
                return _lua;
            }
        }

        #endregion

        /// <summary>
        /// Creates a new instance of <c>LuaEngine</c>
        /// </summary>
        /// <param name="conn">IRC Connection</param>
        /// <param name="scriptsPath">The directory where the Lua scripts are located.</param>
        public LuaEngine(/*ref Connection conn, */string scriptsPath)
        {
            Log.Info("Initializing Lua engine");
            _lua = new Lua();
            _luaFunctions = new Dictionary<string, LuaFunctionDescriptor>();
            _scriptPath = scriptsPath;
            _connection = conn;
            _functions = new LuaFunctions(ref _lua, ref _connection);

            LuaHelper.RegisterLuaFunctions(_lua, ref _luaFunctions, _functions);

            LoadScripts();

            _watcher = new FileSystemWatcher(_scriptPath)
                           {
                               NotifyFilter =
                                   NotifyFilters.FileName | NotifyFilters.Attributes | NotifyFilters.LastAccess |
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
                foreach(var handler in _functions.RegisteredOnPM.AsParallel())
                {
                    _connection.Listener.OnPublic -= handler;
                }
            }

            var di = new DirectoryInfo(_scriptPath);

            foreach(var file in di.GetFiles("*.lua").AsParallel())
            {
                Log.Info("Loading Lua script: {0}", file.Name);
                try {_lua.DoFile(file.FullName);}
                catch(Exception x)
                {
                    Log.ErrorException(string.Format("Exception thrown while loading Lua script {0}", file.Name), x);
                }
                
            }
        }

        /// <summary>
        /// Frees up resources.
        /// </summary>
        public void Free()
        {
            _lua.Dispose();
        }
    }
}
