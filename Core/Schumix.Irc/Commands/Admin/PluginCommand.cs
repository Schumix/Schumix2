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
using System.Threading.Tasks;
using Schumix.Framework.Irc;
using Schumix.Framework.Config;
using Schumix.Framework.Delegate;
using Schumix.Framework.Extensions;

namespace Schumix.Irc.Commands
{
	public abstract partial class CommandHandler
	{
		protected void HandlePlugin(IRCMessage sIRCMessage)
		{
			if(IsWarningAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Administrator))
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("WarningAdmin", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Administrator))
				return;

			if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "load")
			{
				var text = sLManager.GetCommandTexts("plugin/load", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 3)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}

				if(sAddonManager.IsLoadingAddons())
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[2]);
					return;
				}

				if(sAddonManager.LoadPluginsFromDirectory(AddonsConfig.Directory))
				{
					foreach(var nw in sIrcBase.Networks)
						sIrcBase.LoadProcessMethods(nw.Key);

					sSendMessage.SendChatMessage(sIRCMessage, text[0]);
				}
				else
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
			}
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "unload")
			{
				var text = sLManager.GetCommandTexts("plugin/unload", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 3)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}

				if(!sAddonManager.IsLoadingAddons())
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[2]);
					return;
				}

				foreach(var nw in sIrcBase.Networks)
					sIrcBase.UnloadProcessMethods(nw.Key);

				if(sAddonManager.UnloadPlugins())
					sSendMessage.SendChatMessage(sIRCMessage, text[0]);
				else
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
			}
			else
			{
				var text = sLManager.GetCommandTexts("plugin", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 3)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}

				string Plugins = string.Empty;
				string IgnorePlugins = string.Empty;

				foreach(var plugin in sAddonManager.Addons[sIRCMessage.ServerName].Addons)
				{
					if(!sIgnoreAddon.IsIgnore(plugin.Key))
						Plugins += ", " + plugin.Value.Name;
					else
						IgnorePlugins += ", " + plugin.Value.Name;
				}

				if(!Plugins.IsNullOrEmpty())
					sSendMessage.SendChatMessage(sIRCMessage, text[0], Plugins.Remove(0, 2, ", "));

				if(!IgnorePlugins.IsNullOrEmpty())
					sSendMessage.SendChatMessage(sIRCMessage, text[1], IgnorePlugins.Remove(0, 2, ", "));

				if(Plugins.IsNullOrEmpty() && IgnorePlugins.IsNullOrEmpty())
					sSendMessage.SendChatMessage(sIRCMessage, text[2]);
			}
		}
	}
}