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
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using Schumix.Api;
using Schumix.Api.Delegate;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Addon;
using Schumix.Framework.Logger;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Irc.Ignore
{
	public sealed class IgnoreAddon
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly AddonManager sAddonManager = Singleton<AddonManager>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private readonly List<string> _ignorelist = new List<string>();
		private string _servername;

		public IgnoreAddon(string ServerName)
		{
			_servername = ServerName;
		}

		public bool IsIgnore(string Name)
		{
			return Contains(Name);
		}

		public void LoadConfig()
		{
			string[] ignore = AddonsConfig.Ignore.Split(SchumixBase.Comma);

			if(ignore.Length > 1)
			{
				foreach(var name in ignore)
					Add(name.ToLower());
			}
			else
				Add(AddonsConfig.Ignore.ToLower());
		}

		public void LoadSql()
		{
			var db = SchumixBase.DManager.Query("SELECT Addon FROM ignore_addons WHERE ServerName = '{0}'", _servername);
			if(!db.IsNull())
			{
				foreach(DataRow row in db.Rows)
				{
					string name = row["Addon"].ToString();

					if(!Contains(name))
						_ignorelist.Add(name.ToLower());
				}
			}
		}

		public void Add(string Name)
		{
			if(Name.IsNullOrEmpty())
				return;

			var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM ignore_addons WHERE Addon = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(Name.ToLower()), _servername);
			if(!db.IsNull())
				return;

			_ignorelist.Add(Name.ToLower());
			SchumixBase.DManager.Insert("`ignore_addons`(ServerId, ServerName, Addon)", IRCConfig.List[_servername].ServerId, _servername, sUtilities.SqlEscape(Name.ToLower()));
		}

		public void Remove(string Name)
		{
			if(Name.IsNullOrEmpty())
				return;

			var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM ignore_addons WHERE Addon = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(Name.ToLower()), _servername);
			if(db.IsNull())
				return;

			_ignorelist.Remove(Name.ToLower());
			SchumixBase.DManager.Delete("ignore_addons", string.Format("Addon = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(Name.ToLower()), _servername));
		}

		public bool Contains(string Name)
		{
			if(Name.IsNullOrEmpty())
				return false;

			return _ignorelist.Contains(Name.ToLower());
		}

		public void RemoveConfig()
		{
			string[] ignore = AddonsConfig.Ignore.Split(SchumixBase.Comma);

			if(ignore.Length > 1)
			{
				foreach(var name in ignore)
					Remove(name.ToLower());
			}
			else
				Remove(AddonsConfig.Ignore.ToLower());
		}

		public void LoadPlugin(string plugin)
		{
			if(sAddonManager.Addons[_servername].IgnoreAssemblies.ContainsKey(plugin))
			{
				Log.Debug("IgnoreAddon", sLConsole.GetString("{0} plugin is now loading."), plugin);
				ISchumixAddon pl;
				sAddonManager.Addons[_servername].Addons.TryGetValue(plugin, out pl);

				if(pl.IsNull())
				{
					Log.Error("IgnoreAddon", sLConsole.GetString("Plugin can not be loaded!"));
					return;
				}

				pl.Setup(_servername, true);
				sAddonManager.Addons[_servername].Assemblies.Add(plugin, sAddonManager.Addons[_servername].IgnoreAssemblies[plugin]);
				var types = sAddonManager.Addons[_servername].IgnoreAssemblies[plugin].GetTypes();
				sIrcBase.LoadProcessMethods(_servername, types);
				Log.Success("IgnoreAddon", sLConsole.GetString("Loaded plugin: {0} {1} by {2} ({3})"), pl.Name, sAddonManager.Addons[_servername].IgnoreAssemblies[plugin].GetName().Version.ToString(), pl.Author, pl.Website);
				sAddonManager.Addons[_servername].IgnoreAssemblies.Remove(plugin);
			}
		}

		public void UnloadPlugin(string plugin)
		{
			if(!sAddonManager.Addons[_servername].IgnoreAssemblies.ContainsKey(plugin))
			{
				Log.Debug("IgnoreAddon", sLConsole.GetString("{0} plugin denying and removing is now started."), plugin);
				ISchumixAddon pl;
				sAddonManager.Addons[_servername].Addons.TryGetValue(plugin, out pl);

				if(pl.IsNull())
				{
					Log.Error("IgnoreAddon", sLConsole.GetString("Plugin can not be loaded!"));
					return;
				}

				pl.Destroy();
				sAddonManager.Addons[_servername].IgnoreAssemblies.Add(plugin, sAddonManager.Addons[_servername].Assemblies[plugin]);
				var types = sAddonManager.Addons[_servername].Assemblies[plugin].GetTypes();
				sIrcBase.UnloadProcessMethods(_servername, types);
				Log.Success("IgnoreAddon", sLConsole.GetString("{0} plugin successfully removed."), plugin);
				sAddonManager.Addons[_servername].Assemblies.Remove(plugin);
			}
		}

		public void Clean()
		{
			_ignorelist.Clear();
		}
	}
}