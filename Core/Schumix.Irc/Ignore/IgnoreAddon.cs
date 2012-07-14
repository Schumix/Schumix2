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
		private readonly object Lock = new object();
		private IgnoreAddon() {}

		public bool IsIgnore(string Name)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM ignore_addons WHERE Addon = '{0}'", sUtilities.SqlEscape(Name.ToLower()));
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

			var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM ignore_addons WHERE Addon = '{0}'", sUtilities.SqlEscape(Name.ToLower()));
			if(!db.IsNull())
				return;

			SchumixBase.DManager.Insert("`ignore_addons`(Addon)", sUtilities.SqlEscape(Name.ToLower()));
		}

		public void Remove(string Name)
		{
			if(Name.Trim() == string.Empty)
				return;

			var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM ignore_addons WHERE Addon = '{0}'", sUtilities.SqlEscape(Name.ToLower()));
			if(db.IsNull())
				return;

			SchumixBase.DManager.Delete("ignore_addons", string.Format("Addon = '{0}'", sUtilities.SqlEscape(Name.ToLower())));
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

		public void LoadPlugin(string plugin, string servername)
		{
			if(AddonManager.IgnoreAssemblies.ContainsKey(plugin))
			{
				Log.Debug("IgnoreAddon", sLConsole.IgnoreAddon("Text"), plugin);
				ISchumixAddon pl;
				sAddonManager.GetPlugins().TryGetValue(plugin, out pl);

				if(pl.IsNull())
				{
					Log.Error("IgnoreAddon", sLConsole.IgnoreAddon("Text2"));
					return;
				}

				pl.Setup();
				AddonManager.Assemblies.Add(plugin, AddonManager.IgnoreAssemblies[plugin]);
				var types = AddonManager.IgnoreAssemblies[plugin].GetTypes();

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
									sIrcBase.Networks[servername].IrcRegisterHandler(attr.Command, del);
								}
							}

							if(attribute.IsOfType(typeof(SchumixCommandAttribute)))
							{
								var attr = (SchumixCommandAttribute)attribute;
								lock(Lock)
								{
									var del = Delegate.CreateDelegate(typeof(CommandDelegate), method) as CommandDelegate;
									sIrcBase.Networks[servername].SchumixRegisterHandler(attr.Command, del, attr.Permission);
								}
							}
						}
					});
				});

				Log.Success("IgnoreAddon", sLConsole.IgnoreAddon("Text3"), pl.Name, AddonManager.IgnoreAssemblies[plugin].GetName().Version.ToString(), pl.Author, pl.Website);
				AddonManager.IgnoreAssemblies.Remove(plugin);
			}
		}

		public void UnloadPlugin(string plugin, string servername)
		{
			if(!AddonManager.IgnoreAssemblies.ContainsKey(plugin))
			{
				Log.Debug("IgnoreAddon", sLConsole.IgnoreAddon("Text4"), plugin);
				ISchumixAddon pl;
				sAddonManager.GetPlugins().TryGetValue(plugin, out pl);

				if(pl.IsNull())
				{
					Log.Error("IgnoreAddon", sLConsole.IgnoreAddon("Text2"));
					return;
				}

				pl.Destroy();
				AddonManager.IgnoreAssemblies.Add(plugin, AddonManager.Assemblies[plugin]);
				var types = AddonManager.Assemblies[plugin].GetTypes();

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
									sIrcBase.Networks[servername].IrcRemoveHandler(attr.Command, del);
								}
							}

							if(attribute.IsOfType(typeof(SchumixCommandAttribute)))
							{
								var attr = (SchumixCommandAttribute)attribute;
								lock(Lock)
								{
									var del = Delegate.CreateDelegate(typeof(CommandDelegate), method) as CommandDelegate;
									sIrcBase.Networks[servername].SchumixRemoveHandler(attr.Command, del);
								}
							}
						}
					});
				});

				Log.Success("IgnoreAddon", sLConsole.IgnoreAddon("Text5"), plugin);
				AddonManager.Assemblies.Remove(plugin);
			}
		}
	}
}