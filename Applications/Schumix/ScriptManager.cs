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
using System.Collections.Generic;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Localization;

namespace Schumix
{
	/// <summary>
	///   A script manager for the IRC connections.
	///   Loads plugins, manages events etc.
	/// </summary>
	public sealed class ScriptManager
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
#if MONO
#pragma warning disable 414
		private readonly string _scriptsPath;
#pragma warning restore 414
#else
		private readonly string _scriptsPath;
#endif
		private LuaEngine.LuaEngine _luaEngine;

		/// <summary>
		/// Gets the Lua engine.
		/// </summary>
		public LuaEngine.LuaEngine Lua { get { return _luaEngine; } }

		/// <summary>
		///   Creates a new instance of ScriptManager
		/// </summary>
		/// <param name="scriptPath">Path to scripts.</param>
		public ScriptManager(string scriptPath)
		{
			_scriptsPath = scriptPath;

			if(ScriptsConfig.Lua)
			{
				sUtilities.CreateDirectory(scriptPath);
				_luaEngine = new LuaEngine.LuaEngine(scriptPath);
			}
			else
				Log.Warning("ScriptManager", sLConsole.ScriptManager("Text"));
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