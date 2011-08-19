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
using Schumix.Framework.Localization;

namespace Schumix.Framework
{
	/// <summary>
	/// Class used to manage (load, unload, reload) plugins dynamically.
	/// </summary>
	public sealed class AddonManager
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly List<ISchumixAddon> _addons = new List<ISchumixAddon>();
		private readonly object LoadLock = new object();

		/// <summary>
		/// List of found assemblies.
		/// </summary>
		public readonly List<Assembly> Assemblies = new List<Assembly>();
		public List<ISchumixAddon> GetPlugins() { return _addons; }

		private AddonManager() {}

		/// <summary>
		/// Initializes the Plugin manager.
		/// </summary>
		public void Initialize()
		{
			SetupAppDomainDebugHandlers();
		}

		/// <summary>
		/// Loads plugins from the specified directory.
		/// </summary>
		/// <param name="directory">The directory to check in</param>
		public bool LoadPluginsFromDirectory(DirectoryInfo directory)
		{
			return LoadPluginsFromDirectory(directory.FullName);
		}

		/// <summary>
		/// Loads plugins from the specified directory.
		/// </summary>
		/// <param name="directory">The directory to check in</param>
		public bool LoadPluginsFromDirectory(string directory)
		{
			try
			{
				string file = string.Empty;
				string[] ignore = AddonsConfig.Ignore.Split(SchumixBase.Comma);
				var dir = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, directory));
				Log.Notice("AddonManager", sLConsole.AddonManager("Text"), dir.FullName);

				foreach(var dll in dir.GetFiles("*.dll").AsParallel())
				{
					if(!dll.Name.Contains("Addon"))
						continue;

					bool enabled = true;

					if(ignore.Length > 1)
					{
						foreach(var _ignore in ignore)
						{
							file = _ignore + ".dll";

							if(dll.Name.ToLower() == file.ToLower())
								enabled = false;
						}
					}
					else
					{
						file = AddonsConfig.Ignore + ".dll";

						if(dll.Name.ToLower() == file.ToLower())
							enabled = false;
					}

					if(!enabled)
						continue;

					var asm = Assembly.LoadFrom(dll.FullName);

					if(asm.IsNull())
						continue;

					ISchumixAddon pl;

					foreach(var type in asm.GetTypesWithInterface(typeof(ISchumixAddon)))
					{
						pl = (ISchumixAddon)Activator.CreateInstance(type);

						if(pl.IsNull() || Assemblies.Contains(asm))
							continue;

						pl.Setup();

						lock(LoadLock)
						{
							_addons.Add(pl);
							Assemblies.Add(asm);
						}

						Log.Success("AddonManager", sLConsole.AddonManager("Text2"), pl.Name, asm.GetName().Version.ToString(), pl.Author, pl.Website);
					}
				}

				if(AddonsConfig.Ignore.Length > 1)
					Log.Notice("AddonManager", sLConsole.AddonManager("Text3"), AddonsConfig.Ignore);

				return true;
			}
			catch(Exception e)
			{
				Log.Error("AddonManager", sLConsole.AddonManager("Text4"), e.Message);
				return false;
			}
		}

		/// <summary>
		/// Unloads all addons.
		/// </summary>
		public bool UnloadPlugins()
		{
			lock(LoadLock)
			{
				foreach(var addon in _addons)
					addon.Destroy();

				_addons.Clear();
				Assemblies.Clear();
			}

			Log.Notice("AddonManager", sLConsole.AddonManager("Text5"));
			return true;
		}

		private void SetupAppDomainDebugHandlers()
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
