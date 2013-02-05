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
using Schumix.Api.Irc;
using Schumix.Framework;
using Schumix.Framework.Config;

namespace Schumix.Irc.Commands
{
	public abstract partial class CommandHandler
	{
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
				new Config(SchumixConfig.ConfigDirectory, SchumixConfig.ConfigFile, SchumixConfig.ColorBindMode);
				sIgnoreAddon.RemoveConfig();
				sIgnoreAddon.LoadConfig();
				sIgnoreChannel.RemoveConfig();
				sIgnoreChannel.LoadConfig();
				sIgnoreNickName.RemoveConfig();
				sIgnoreNickName.LoadConfig();
				sIrcBase.Networks[sIRCMessage.ServerName].ReloadMessageHandlerConfig();
				sLConsole.SetLocale(LocalizationConfig.Locale);
				sCtcpSender.ClientInfoResponse = sLConsole.GetString("This client supports: UserInfo, Finger, Version, Source, Ping, Time and ClientInfo");
				i = 1;
				break;
			case "cachedb":
				SchumixBase.sCacheDB.ReLoad();
				i = 1;
				break;
			}
			
			foreach(var plugin in sAddonManager.Addons[sIRCMessage.ServerName].Addons)
			{
				if(!sAddonManager.Addons[sIRCMessage.ServerName].IgnoreAssemblies.ContainsKey(plugin.Key) &&
				   plugin.Value.Reload(sIRCMessage.Info[4].ToLower()) == 1)
					i = 1;
				else if(!sAddonManager.Addons[sIRCMessage.ServerName].IgnoreAssemblies.ContainsKey(plugin.Key) &&
				        plugin.Value.Reload(sIRCMessage.Info[4].ToLower()) == 0)
					i = 0;
			}
			
			if(i == -1)
				sSendMessage.SendChatMessage(sIRCMessage, text[0]);
			else if(i == 0)
				sSendMessage.SendChatMessage(sIRCMessage, text[1]);
			else if(i == 1)
				sSendMessage.SendChatMessage(sIRCMessage, text[2], sIRCMessage.Info[4]);
		}
	}
}