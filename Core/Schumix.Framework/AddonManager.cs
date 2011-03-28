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
using System.Reflection;
using Schumix.API;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;

namespace Schumix.Framework
{
	/// <summary>
	/// Class used to manage (load, unload, reload) plugins dynamically.
	/// </summary>
	public static class AddonManager
	{
		private static readonly List<ISchumixAddon> _addons = new List<ISchumixAddon>();
		private static readonly object LoadLock = new object();

		/// <summary>
		/// List of found assemblies.
		/// </summary>
		public static readonly List<Assembly> Assemblies = new List<Assembly>();
		public static List<ISchumixAddon> GetPlugins() { return _addons; }

		/// <summary>
		/// Initializes the Plugin manager.
		/// </summary>
		public static void Initialize()
		{
			SetupAppDomainDebugHandlers();
		}

		/// <summary>
		/// Loads plugins from the specified directory.
		/// </summary>
		/// <param name="directory">The directory to check in</param>
		public static void LoadPluginsFromDirectory(DirectoryInfo directory) { LoadPluginsFromDirectory(directory.FullName);}

		/// <summary>
		/// Loads plugins from the specified directory.
		/// </summary>
		/// <param name="directory">The directory to check in</param>
		public static bool LoadPluginsFromDirectory(string directory)
		{
			try
			{
				var dir = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, directory));
				Log.Notice("AddonManager", "Loading addons from: {0}", dir.FullName);

				foreach(var dll in dir.GetFiles("*.dll").AsParallel())
				{
					if(!dll.FullName.Contains("Addon"))
						continue;

					var asm = Assembly.LoadFrom(dll.FullName);

					if(asm == null)
						continue;

					ISchumixAddon pl = null;

					foreach(var type in from t in asm.GetTypes().AsParallel() where t.GetInterfaces().Contains(typeof(ISchumixAddon)) select t)
					{
						pl = Activator.CreateInstance(type).Cast<ISchumixAddon>();

						if(pl == null)
							continue;

						pl.Setup();

						lock(LoadLock)
						{
							_addons.Add(pl);
							Assemblies.Add(asm);
						}

						Log.Success("AddonManager", "Loaded plugin: {0} {1} by {2} ({3})", pl.Name, asm.GetName().Version.ToString(), pl.Author, pl.Website);
					}
				}

				return true;
			}
			catch(Exception e)
			{
				Log.Error("AddonManager", "Error while loading one of directories! Detail: {0}", e.Message);
				return false;
			}
		}

		/// <summary>
		/// Unloads all addons.
		/// </summary>
		public static bool UnloadPlugins()
		{
			lock(LoadLock)
			{
				for(var i = 0; i < _addons.Count; ++i)
				{
					var pl = _addons[i];
					pl.Destroy();
					_addons.Remove(pl);
				}

				Assemblies.Clear();
			}

			return true;
		}

		private static void SetupAppDomainDebugHandlers()
		{
			AppDomain.CurrentDomain.DomainUnload += (sender, args) =>
				Log.Debug("AddonManager", "AppDomain::DomainUnload, hash: {0}", AppDomain.CurrentDomain.GetHashCode());

			AppDomain.CurrentDomain.AssemblyLoad += (sender, ea) =>
				Log.Debug("AddonManager", "AppDomain::AssemblyLoad, sender is: {0}, loaded assembly: {1}.", sender.GetHashCode(), ea.LoadedAssembly.FullName);

			AppDomain.CurrentDomain.AssemblyResolve += (sender, eargs) =>
			{
				Log.Debug("AddonManager", "AppDomain::AssemblyResolve, sender: {0}, name: {1}, asm: {2}", sender.GetHashCode(), eargs.Name, eargs.RequestingAssembly.FullName );
				return null;
			};
		}
	}
}
