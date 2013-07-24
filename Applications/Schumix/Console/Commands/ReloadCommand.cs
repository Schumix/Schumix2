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
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Logger;
using Schumix.Console.Delegate;

namespace Schumix.Console
{
	/// <summary>
	///     CommandHandler class.
	/// </summary>
	partial class CommandHandler
	{
		/// <summary>
		///     Reload parancs függvénye.
		/// </summary>
		protected void HandleReload(ConsoleMessage sConsoleMessage)
		{
			if(sConsoleMessage.Info.Length < 2)
			{
				Log.Error("Console", sLManager.GetConsoleWarningText("NoName"));
				return;
			}

			var text = sLManager.GetConsoleCommandTexts("reload");
			if(text.Length < 2)
			{
				Log.Error("Console", sLConsole.Translations("NoFound2"));
				return;
			}

			int i = -1;

			switch(sConsoleMessage.Info[1].ToLower())
			{
				case "config":
					new Config(SchumixConfig.ConfigDirectory, SchumixConfig.ConfigFile, SchumixConfig.ColorBindMode);
					sIrcBase.Networks[_servername].sIgnoreAddon.RemoveConfig();
					sIrcBase.Networks[_servername].sIgnoreAddon.LoadConfig();
					sIrcBase.Networks[_servername].sIgnoreChannel.RemoveConfig();
					sIrcBase.Networks[_servername].sIgnoreChannel.LoadConfig();
					sIrcBase.Networks[_servername].sIgnoreNickName.RemoveConfig();
					sIrcBase.Networks[_servername].sIgnoreNickName.LoadConfig();
					sIrcBase.Networks[_servername].ReloadMessageHandlerConfig();
					sLConsole.SetLocale(LocalizationConfig.Locale);
					sIrcBase.Networks[_servername].sCtcpSender.ClientInfoResponse = sLConsole.GetString("This client supports: UserInfo, Finger, Version, Source, Ping, Time and ClientInfo");
					i = 1;
					break;
				case "cachedb":
					SchumixBase.sCacheDB.Reload();
					i = 1;
					break;
				case "irc":
					sIrcBase.Reload();
					i = 1;
					break;
			}

			bool load = true;

			foreach(var plugin in sAddonManager.Addons[_servername].Addons)
			{
				if(!sAddonManager.Addons[_servername].IgnoreAssemblies.ContainsKey(plugin.Key) &&
				   plugin.Value.Reload(sConsoleMessage.Info[1].ToLower(), load) == 1)
					i = 1;
				else if(!sAddonManager.Addons[_servername].IgnoreAssemblies.ContainsKey(plugin.Key) &&
				        plugin.Value.Reload(sConsoleMessage.Info[1].ToLower(), load) == 0)
					i = 0;

				if(load)
					load = false;
			}

			if(i == -1)
				Log.Error("Console", text[0]);
			else if(i == 0)
				Log.Error("Console", text[1]);
			else if(i == 1)
				Log.Notice("Console", text[2], sConsoleMessage.Info[1]);
		}
	}
}