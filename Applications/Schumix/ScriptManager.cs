/*
 * This file is part of Schumix.
 * 
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
using System.IO;
using System.Collections.Generic;
using Schumix.Framework;
using Schumix.Framework.Logger;
using Schumix.Framework.Config;
using Schumix.Framework.Localization;

namespace Schumix
{
	/// <summary>
	///   A script manager for the IRC connections.
	///   Loads plugins, manages events etc.
	/// </summary>
	sealed class ScriptManager
	{
		/// <summary>
		///     Hozzáférést biztosít singleton-on keresztül a megadott class-hoz.
		///     LocalizationConsole segítségével állíthatók be a konzol nyelvi tulajdonságai.
		/// </summary>
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		/// <summary>
		///     Hozzáférést biztosít singleton-on keresztül a megadott class-hoz.
		///     Utilities sokféle függvényt tartalmaz melyek hasznosak lehetnek.
		/// </summary>
		private static readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private PythonEngine.PythonEngine _pythonEngine;
		private LuaEngine.LuaEngine _luaEngine;
		private readonly string _scriptsPath;
		/// <summary>
		/// Gets the Python engine.
		/// </summary>
		public PythonEngine.PythonEngine Python { get { return _pythonEngine; } }
		/// <summary>
		/// Gets the Lua engine.
		/// </summary>
		public LuaEngine.LuaEngine Lua { get { return _luaEngine; } }
		public string ScriptsPath { get { return _scriptsPath; } }

		/// <summary>
		///   Creates a new instance of ScriptManager
		/// </summary>
		/// <param name="scriptPath">Path to scripts.</param>
		public ScriptManager(string scriptPath)
		{
			_scriptsPath = scriptPath;
			sUtilities.CreateDirectory(scriptPath);

			if(ScriptsConfig.Lua)
			{
				sUtilities.CreateDirectory(Path.Combine(scriptPath, "Lua"));
				_luaEngine = new LuaEngine.LuaEngine(Path.Combine(scriptPath, "Lua"));
			}
			else
				Log.Warning("ScriptManager", sLConsole.GetString("Lua support is disabled!"));

			if(ScriptsConfig.Python)
			{
				sUtilities.CreateDirectory(Path.Combine(scriptPath, "Python"));
				_pythonEngine = new PythonEngine.PythonEngine(Path.Combine(scriptPath, "Python"));
			}
			else
				Log.Warning("ScriptManager", sLConsole.GetString("Python support is disabled!"));
		}

		/// <summary>
		///   Releases unmanaged resources and performs other cleanup operations before the
		///   <see cref = "ScriptManager" /> is reclaimed by garbage collection.
		/// </summary>
		~ScriptManager()
		{
			Log.Debug("ScriptManager", "~ScriptManager()");
		}
	}
}