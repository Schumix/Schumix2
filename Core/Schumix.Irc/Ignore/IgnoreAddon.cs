/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Megax <http://www.megaxx.info/>
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
using System.Reflection;
using System.Collections.Generic;
using System.Threading.Tasks;
using Schumix.API;
using Schumix.API.Delegate;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Addon;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Irc.Ignore
{
	public sealed class IgnoreAddone
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly AddonManager sAddonManager = Singleton<AddonManager>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private readonly object Lock = new object();
		private string _servername;

		public IgnoreAddone(string ServerName)
		{
			_servername = ServerName;
		}

		public bool IsIgnore(string Name)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM ignore_addons WHERE Addon = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(Name.ToLower()), _servername);
			return !db.IsNull() ? true : false;
		}

		public void AddConfig()
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

		public void Add(string Name)
		{
			if(Name.Trim() == string.Empty)
				return;

			var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM ignore_addons WHERE Addon = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(Name.ToLower()), _servername);
			if(!db.IsNull())
				return;

			SchumixBase.DManager.Insert("`ignore_addons`(ServerId, ServerName, Addon)", IRCConfig.List[_servername].ServerId, _servername, sUtilities.SqlEscape(Name.ToLower()));
		}

		public void Remove(string Name)
		{
			if(Name.Trim() == string.Empty)
				return;

			var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM ignore_addons WHERE Addon = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(Name.ToLower()), _servername);
			if(db.IsNull())
				return;

			SchumixBase.DManager.Delete("ignore_addons", string.Format("Addon = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(Name.ToLower()), _servername));
		}

		public bool Contains(string Name)
		{
			if(Name.Trim() == string.Empty)
				return false;

			var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM ignore_addons WHERE Addon = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(Name.ToLower()), _servername);
			return !db.IsNull();
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
				Log.Debug("IgnoreAddon", sLConsole.IgnoreAddon("Text"), plugin);
				ISchumixAddon pl;
				sAddonManager.Addons[_servername].Addons.TryGetValue(plugin, out pl);

				if(pl.IsNull())
				{
					Log.Error("IgnoreAddon", sLConsole.IgnoreAddon("Text2"));
					return;
				}

				pl.Setup(_servername);
				sAddonManager.Addons[_servername].Assemblies.Add(plugin, sAddonManager.Addons[_servername].IgnoreAssemblies[plugin]);
				var types = sAddonManager.Addons[_servername].IgnoreAssemblies[plugin].GetTypes();

				Parallel.ForEach(types, type =>
				{
					var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
					Parallel.ForEach(methods, method =>
					{
						foreach(var attribute in Attribute.GetCustomAttributes(method))
						{
							if(attribute.IsOfType(typeof(IrcCommandAttribute)))
							{
								var attr = (IrcCommandAttribute)attribute;
								lock(Lock)
								{
									var del = Delegate.CreateDelegate(typeof(IRCDelegate), method) as IRCDelegate;
									sIrcBase.Networks[_servername].IrcRegisterHandler(attr.Command, del);
								}
							}

							if(attribute.IsOfType(typeof(SchumixCommandAttribute)))
							{
								var attr = (SchumixCommandAttribute)attribute;
								lock(Lock)
								{
									var del = Delegate.CreateDelegate(typeof(CommandDelegate), method) as CommandDelegate;
									sIrcBase.Networks[_servername].SchumixRegisterHandler(attr.Command, del, attr.Permission);
								}
							}
						}
					});
				});

				Log.Success("IgnoreAddon", sLConsole.IgnoreAddon("Text3"), pl.Name, sAddonManager.Addons[_servername].IgnoreAssemblies[plugin].GetName().Version.ToString(), pl.Author, pl.Website);
				sAddonManager.Addons[_servername].IgnoreAssemblies.Remove(plugin);
			}
		}

		public void UnloadPlugin(string plugin)
		{
			if(!sAddonManager.Addons[_servername].IgnoreAssemblies.ContainsKey(plugin))
			{
				Log.Debug("IgnoreAddon", sLConsole.IgnoreAddon("Text4"), plugin);
				ISchumixAddon pl;
				sAddonManager.Addons[_servername].Addons.TryGetValue(plugin, out pl);

				if(pl.IsNull())
				{
					Log.Error("IgnoreAddon", sLConsole.IgnoreAddon("Text2"));
					return;
				}

				pl.Destroy();
				sAddonManager.Addons[_servername].IgnoreAssemblies.Add(plugin, sAddonManager.Addons[_servername].Assemblies[plugin]);
				var types = sAddonManager.Addons[_servername].Assemblies[plugin].GetTypes();

				Parallel.ForEach(types, type =>
				{
					var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
					Parallel.ForEach(methods, method =>
					{
						foreach(var attribute in Attribute.GetCustomAttributes(method))
						{
							if(attribute.IsOfType(typeof(IrcCommandAttribute)))
							{
								var attr = (IrcCommandAttribute)attribute;
								lock(Lock)
								{
									var del = Delegate.CreateDelegate(typeof(IRCDelegate), method) as IRCDelegate;
									sIrcBase.Networks[_servername].IrcRemoveHandler(attr.Command, del);
								}
							}

							if(attribute.IsOfType(typeof(SchumixCommandAttribute)))
							{
								var attr = (SchumixCommandAttribute)attribute;
								lock(Lock)
								{
									var del = Delegate.CreateDelegate(typeof(CommandDelegate), method) as CommandDelegate;
									sIrcBase.Networks[_servername].SchumixRemoveHandler(attr.Command, del);
								}
							}
						}
					});
				});

				Log.Success("IgnoreAddon", sLConsole.IgnoreAddon("Text5"), plugin);
				sAddonManager.Addons[_servername].Assemblies.Remove(plugin);
			}
		}
	}
}