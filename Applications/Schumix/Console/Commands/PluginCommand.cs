/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013-2015 Schumix Team <http://schumix.eu/>
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
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Logger;
using Schumix.Framework.Extensions;
using Schumix.Console.Delegate;

namespace Schumix.Console
{
	/// <summary>
	///     CommandHandler class.
	/// </summary>
	partial class CommandHandler
	{
		/// <summary>
		///     Plugin parancs függvénye.
		/// </summary>
		protected void HandlePlugin(ConsoleMessage sConsoleMessage)
		{
			if(sConsoleMessage.Info.Length >= 2 && sConsoleMessage.Info[1].ToLower() == "load")
			{
				var text = sLManager.GetConsoleCommandTexts("plugin/load");
				if(text.Length < 3)
				{
					Log.Error("Console", sLConsole.Translations("NoFound2"));
					return;
				}

				if(sAddonManager.IsLoadingAddons())
				{
					Log.Error("Console", text[2]);
					return;
				}

				if(sAddonManager.LoadPluginsFromDirectory(AddonsConfig.Directory))
				{
					foreach(var nw in sIrcBase.Networks)
						sIrcBase.LoadProcessMethods(nw.Key);

					Log.Notice("Console", text[0]);
				}
				else
					Log.Error("Console", text[1]);
			}
			else if(sConsoleMessage.Info.Length >= 2 && sConsoleMessage.Info[1].ToLower() == "unload")
			{
				var text = sLManager.GetConsoleCommandTexts("plugin/unload");
				if(text.Length < 3)
				{
					Log.Error("Console", sLConsole.Translations("NoFound2"));
					return;
				}

				if(!sAddonManager.IsLoadingAddons())
				{
					Log.Error("Console", text[2]);
					return;
				}

				foreach(var nw in sIrcBase.Networks)
					sIrcBase.UnloadProcessMethods(nw.Key);

				if(sAddonManager.UnloadPlugins())
					Log.Notice("Console", text[0]);
				else
					Log.Error("Console", text[1]);
			}
			else
			{
				var text = sLManager.GetConsoleCommandTexts("plugin");
				if(text.Length < 3)
				{
					Log.Error("Console", sLConsole.Translations("NoFound2"));
					return;
				}

				string Plugins = string.Empty;
				string IgnorePlugins = string.Empty;

				foreach(var plugin in sAddonManager.Addons[_servername].Addons)
				{
					if(!sIrcBase.Networks[_servername].sIgnoreAddon.IsIgnore(plugin.Key))
						Plugins += ", " + plugin.Value.Name;
					else
						IgnorePlugins += ", " + plugin.Value.Name;
				}

				if(!Plugins.IsNullOrEmpty())
					Log.Notice("Console", text[0], Plugins.Remove(0, 2, ", "));

				if(!IgnorePlugins.IsNullOrEmpty())
					Log.Notice("Console", text[1], IgnorePlugins.Remove(0, 2, ", "));

				if(Plugins.IsNullOrEmpty() && IgnorePlugins.IsNullOrEmpty())
					Log.Warning("Console", text[2]);
			}
		}
	}
}