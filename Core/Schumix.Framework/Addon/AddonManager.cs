/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Twl
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
using System.Collections.Generic;
using Schumix.Framework;
using Schumix.Framework.Util;
using Schumix.Framework.Logger;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Framework.Addon
{
	/// <summary>
	/// Class used to manage (load, unload, reload) plugins dynamically.
	/// </summary>
	public sealed class AddonManager : DefaultConfig
	{
		public readonly Dictionary<string, AddonBase> Addons = new Dictionary<string, AddonBase>();
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		public bool IsLoadingAddons() { return _loading; }
		private readonly object LoadLock = new object();
		private bool _loading;
		private AddonManager() {}

		/// <summary>
		/// Initializes the Plugin manager.
		/// </summary>
		public void Initialize()
		{
			SetupAppDomainDebugHandlers();
		}

		public bool IsAddon(string ServerName, string AddonName)
		{
			return Addons[ServerName].Addons.ContainsKey(AddonName);
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
				if(!Directory.Exists(directory))
				{
					Log.Warning("AddonManager", sLConsole.GetString("File path not found, default will be used."));
					directory = Path.Combine(Environment.CurrentDirectory, d_addondirectory);
				}

				string file = string.Empty;
				string[] ignore = AddonsConfig.Ignore.Split(SchumixBase.Comma);
				var dir = new DirectoryInfo(directory);

				if(!dir.Exists)
				{
					_loading = false;
					Log.Warning("AddonManager", sLConsole.GetString("There are no addons folder, no addon will be loaded."));

					foreach(var sn in IRCConfig.List)
					{
						if(!Addons.ContainsKey(sn.Key))
							Addons.Add(sn.Key, new AddonBase());
					}

					return false;
				}

				Log.Notice("AddonManager", sLConsole.GetString("Loading addons from: {0}"), dir.FullName);

				foreach(var dll in dir.GetFiles("*.dll").AsParallel())
				{
					if(!dll.Name.ToLower().Contains("addon"))
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

					var asm = Assembly.LoadFrom(dll.FullName);

					if(asm.IsNull())
						continue;

					bool isenabble = true;

					foreach(var sn in IRCConfig.List)
					{
						if(!Addons.ContainsKey(sn.Key))
							Addons.Add(sn.Key, new AddonBase());

						if(enabled && IsIgnore(dll.Name.Substring(0, dll.Name.IndexOf(".dll")), sn.Key))
							enabled = false;

						foreach(var pl in asm.GetTypesWithInterface(typeof(ISchumixAddon)).Select(type => (ISchumixAddon)Activator.CreateInstance(type)))
						{
							if(Addons[sn.Key].Assemblies.ContainsValue(asm) || Addons[sn.Key].IgnoreAssemblies.ContainsValue(asm))
								continue;

							if(Addons[sn.Key].Addons.ContainsKey(pl.Name.ToLower()))
							{
								Log.Error("AddonManager", sLConsole.GetString("This addon name already exists in the system so not loaded!"));
								continue;
							}

							if(enabled)
								pl.Setup(sn.Key);

							lock(LoadLock)
							{
								Addons[sn.Key].Addons.Add(pl.Name.ToLower(), pl);
	
								if(enabled)
									Addons[sn.Key].Assemblies.Add(pl.Name.ToLower(), asm);
								else
									Addons[sn.Key].IgnoreAssemblies.Add(pl.Name.ToLower(), asm);
							}

							if(enabled && isenabble)
								Log.Success("AddonManager", sLConsole.GetString("Loaded plugin: {0} {1} by {2} ({3})"), pl.Name, asm.GetName().Version.ToString(), pl.Author, pl.Website);

							if(enabled)
								isenabble = false;
						}
					}
				}

				if(AddonsConfig.Ignore.Length > 1)
					Log.Notice("AddonManager", sLConsole.GetString("Ignoring plugins (config): {0}"), AddonsConfig.Ignore);

				_loading = true;
				return true;
			}
			catch(Exception e)
			{
				_loading = false;
				Log.Error("AddonManager", sLConsole.GetString("Error while loading one of directories! Detail: {0}"), e.Message);
				return false;
			}
		}

		/// <summary>
		/// Unloads all addons.
		/// </summary>
		public bool UnloadPlugins(string ServerName = "")
		{
			lock(LoadLock)
			{
				if(ServerName.IsNullOrEmpty())
				{
					foreach(var ad in Addons)
					{
						foreach(var addon in ad.Value.Addons)
						{
							if(!ad.Value.IgnoreAssemblies.ContainsKey(addon.Key))
								addon.Value.Destroy();
						}

						ad.Value.Addons.Clear();
						ad.Value.Assemblies.Clear();
						ad.Value.IgnoreAssemblies.Clear();
					}

					Addons.Clear();
				}
				else
				{
					foreach(var addon in Addons[ServerName].Addons)
					{
						if(!Addons[ServerName].IgnoreAssemblies.ContainsKey(addon.Key))
							addon.Value.Destroy();
					}

					Addons[ServerName].Addons.Clear();
					Addons[ServerName].Assemblies.Clear();
					Addons[ServerName].IgnoreAssemblies.Clear();
					Addons.Remove(ServerName);
				}

				_loading = false;
			}

			Log.Notice("AddonManager", sLConsole.GetString("Unload plugins"));
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
				Log.Debug("AddonManager", "AppDomain::AssemblyResolve, sender: {0}, name: {1}, asm: {2}", sender.GetHashCode(), eargs.Name, eargs.RequestingAssembly.FullName);
				return null;
			};
		}

		private bool IsIgnore(string Name, string ServerName)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM ignore_addons WHERE Addon = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(Name.ToLower()), ServerName);
			return !db.IsNull();
		}
	}
}