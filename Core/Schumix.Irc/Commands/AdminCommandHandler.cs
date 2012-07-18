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
using System.Threading;
using Schumix.API;
using Schumix.API.Irc;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;

namespace Schumix.Irc.Commands
{
	public partial class CommandHandler
	{
		protected void HandlePlugin(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Administrator))
				return;

			if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "load")
			{
				var text = sLManager.GetCommandTexts("plugin/load", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 2)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}

				if(sAddonManager.LoadPluginsFromDirectory(AddonsConfig.Directory))
					sSendMessage.SendChatMessage(sIRCMessage, text[0]);
				else
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
			}
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "unload")
			{
				var text = sLManager.GetCommandTexts("plugin/unload", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 2)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}

				if(sAddonManager.UnloadPlugins())
					sSendMessage.SendChatMessage(sIRCMessage, text[0]);
				else
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
			}
			else
			{
				var text = sLManager.GetCommandTexts("plugin", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 2)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}

				string Plugins = string.Empty;
				string IgnorePlugins = string.Empty;

				foreach(var plugin in sAddonManager.GetPlugins())
				{
					if(!sIgnoreAddon.IsIgnore(plugin.Key))
						Plugins += ", " + plugin.Value.Name;
					else
						IgnorePlugins += ", " + plugin.Value.Name;
				}

				sSendMessage.SendChatMessage(sIRCMessage, text[0], Plugins.Remove(0, 2, ", "));
				sSendMessage.SendChatMessage(sIRCMessage, text[1], IgnorePlugins.Remove(0, 2, ", "));
			}
		}

		protected void HandleReload(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Administrator))
				return;

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			var text = sLManager.GetCommandTexts("reload", sIRCMessage.Channel, sIRCMessage.ServerName);
			if(text.Length < 3)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
				return;
			}

			int i = -1;

			switch(sIRCMessage.Info[4].ToLower())
			{
				case "config":
					new Config(SchumixConfig.ConfigDirectory, SchumixConfig.ConfigFile);
					i = 1;
					break;
			}

			foreach(var plugin in sAddonManager.GetPlugins())
			{
				if(plugin.Value.Reload(sIRCMessage.Info[4].ToLower()) == 1)
					i = 1;
				else if(plugin.Value.Reload(sIRCMessage.Info[4].ToLower()) == 0)
					i = 0;
			}

			if(i == -1)
				sSendMessage.SendChatMessage(sIRCMessage, text[0]);
			else if(i == 0)
				sSendMessage.SendChatMessage(sIRCMessage, text[1]);
			else if(i == 1)
				sSendMessage.SendChatMessage(sIRCMessage, text[2], sIRCMessage.Info[4]);
		}

		protected void HandleQuit(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Administrator))
				return;

			var text = sLManager.GetCommandTexts("quit", sIRCMessage.Channel, sIRCMessage.ServerName);
			if(text.Length < 2)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
				return;
			}

			sSendMessage.SendChatMessage(sIRCMessage, text[0]);
			SchumixBase.Quit();
			sSender.Quit(string.Format(text[1], sIRCMessage.Nick));
		}
	}
}