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

namespace Schumix.Framework
{
	/// <summary>
	/// Class used to manage (load, unload, reload) plugins dynamically.
	/// </summary>
	public static class ScriptManager
	{
		private static readonly Utility sUtility = Singleton<Utility>.Instance;

		/// <summary>
		/// List of found assemblies.
		/// </summary>
		public static readonly List<Assembly> Assemblies = new List<Assembly>();
		private static List<ISchumixBase> _plugins = new List<ISchumixBase>();

		/// <summary>
		/// Gets the list of loaded plugins.
		/// </summary>
		/// <returns>
		/// A list of plugins.
		/// </returns>
		public static List<ISchumixBase> GetPlugins() { return _plugins; }

		/// <summary>
		/// Initializes the Plugin manager.
		/// </summary>
		/// <param name="con">IRC connection</param>
		/// <param name="channels">IRC channel list</param>
		public static void Initialize() 
		{
			SetupAppDomainDebugHandlers();
		}

		/// <summary>
		/// Loads the plugins.
		/// </summary>
		public static bool LoadPlugins()
		{
			Log.Notice("ScriptManager", "Loading plugins...");
			var info = new DirectoryInfo("./" + PluginsConfig.Dir);
			FileInfo[] files = info.GetFiles();
			
			foreach(FileInfo f in files)
			{
				if(f.Extension != ".dll" || !f.Name.Contains("Plugin"))
					continue;
				
				try 
				{
					var asm = Assembly.LoadFrom("./" + PluginsConfig.Dir + "/" + f.Name);
					var plugin = asm.CreateInstance("Schumix." + f.Name.Replace(".dll", string.Empty) + ".SchumixPlugin");
					var usable = plugin as ISchumixBase;
					
					if(_plugins.Contains(usable))
						continue;

					usable.Setup();
					_plugins.Add(usable);
				}
				catch(Exception x)
				{
					Log.Error("ScriptManager", "Failed to load: {0} ({1})", f.Name, x.Message);
					return false;
				}
				
				Log.Success("ScriptManager", "Loaded plugin: {0} (hash: {1})", f.Name, sUtility.MD5File("./" + PluginsConfig.Dir + "/" + f.Name));
			}

			return true;
		}
		
		/// <summary>
		/// Loads the specified plugin.
		/// </summary>
		/// <param name="name">
		/// The name of the plugin to load.
		/// </param>
		public static bool LoadPlugin(string name)
		{
			if(!name.EndsWith("Plugin"))
				name += "Plugin";
			
			if(!File.Exists("./" + PluginsConfig.Dir + "/" + name + ".dll"))
				return false;
			
			var fi = new FileInfo("./" + PluginsConfig.Dir + "/" + name + ".dll");
			
			try 
			{
				var asm = Assembly.LoadFrom(fi.FullName);
				var plugin = asm.CreateInstance("Schumix." + fi.Name.Replace(".dll", string.Empty) + ".SchumixPlugin");
				var usable = plugin as ISchumixBase;
				
				if(_plugins.Contains(usable))
					return false;

				usable.Setup();
				_plugins.Add(usable);
			}
			catch(Exception)
			{
				Log.Error("ScriptManager", "Failed to load: {0}", fi.Name);
				return false;
			}
				
			Log.Success("ScriptManager", "Loaded plugin: {0} (hash: {1})", fi.Name, sUtility.MD5File("./" + PluginsConfig.Dir + "/" + fi.Name));
			return true;
		}

		/*public static bool UnloadPlugins()
		{
			foreach(var plugin in _plugins)
			{
				string name = plugin.Name.Replace("Plugin", string.Empty);
				plugin.Destroy();
				_plugins.Remove(plugin);
				Log.Success("ScriptManager", "Successfully unloaded plugin: {0}", name);
			}

			return true;
		}*/
		
		/// <summary>
		/// Unloads the specified plugin.
		/// </summary>
		/// <param name="name">
		/// The name of the plugin to unload.
		/// </param>
		public static bool UnloadPlugin(string name)
		{
			bool removed = false;
			foreach(var plugin in _plugins)
			{
				if(plugin.Name.StartsWith(name))
				{
					plugin.Destroy();
					_plugins.Remove(plugin);
					Log.Success("ScriptManager", "Successfully unloaded plugin: {0}", name);
					removed = true;
					break;
				}
			}

			if(!removed)
				Log.Error("ScriptManager", "Couldn't unload plugin: {0}", name);

			return removed;
		}

		private static void SetupAppDomainDebugHandlers()
		{
			AppDomain.CurrentDomain.DomainUnload += (sender, args) =>
				Log.Debug("ScriptManager", "AppDomain::DomainUnload, hash: {0}", AppDomain.CurrentDomain.GetHashCode());

			AppDomain.CurrentDomain.AssemblyLoad += (sender, ea) =>
				Log.Debug("ScriptManager", "AppDomain::AssemblyLoad, sender is: {0}, loaded assembly: {1}.", sender.GetHashCode(), ea.LoadedAssembly.FullName);

			AppDomain.CurrentDomain.AssemblyResolve += (sender, eargs) =>
			{
				Log.Debug("ScriptManager", "AppDomain::AssemblyResolve, sender: {0}, name: {1}, asm: {2}", sender.GetHashCode(), eargs.Name, eargs.RequestingAssembly.FullName );
				return null;
			};
		}
	}
}
