/*
 * This file is part of Schumix.
 * 
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
using System.Threading;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.API;

namespace Schumix.Irc.Commands
{
	public partial class CommandHandler
	{
		protected void HandlePlugin(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Administrator))
				return;

			CNick(sIRCMessage);

			if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "load")
			{
				var text = sLManager.GetCommandTexts("plugin/load", sIRCMessage.Channel);
				if(text.Length < 2)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "No translations found!");
					return;
				}

				if(sAddonManager.LoadPluginsFromDirectory(AddonsConfig.Directory))
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
				else
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1]);
			}
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "unload")
			{
				var text = sLManager.GetCommandTexts("plugin/unload", sIRCMessage.Channel);
				if(text.Length < 2)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "No translations found!");
					return;
				}

				if(sAddonManager.UnloadPlugins())
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
				else
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1]);
			}
			else
			{
				foreach(var plugin in sAddonManager.GetPlugins())
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("plugin", sIRCMessage.Channel), plugin.Name.Replace("Plugin", string.Empty));
			}
		}

		protected void HandleReload(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Administrator))
				return;

			CNick(sIRCMessage);

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
				return;
			}

			var text = sLManager.GetCommandTexts("reload", sIRCMessage.Channel);
			if(text.Length < 2)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "No translations found!");
				return;
			}

			bool status = false;

			switch(sIRCMessage.Info[4].ToLower())
			{
				case "config":
					new Config(SchumixConfig.ConfigDirectory, SchumixConfig.ConfigFile);
					status = true;
					break;
			}

			foreach(var plugin in sAddonManager.GetPlugins())
			{
				if(plugin.Reload(sIRCMessage.Info[4]))
					status = true;
			}

			if(status)
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0], sIRCMessage.Info[4]);
			else
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1]);
		}

		protected void HandleQuit(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Administrator))
				return;

			CNick(sIRCMessage);
			var text = sLManager.GetCommandTexts("quit", sIRCMessage.Channel);
			if(text.Length < 2)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "No translations found!");
				return;
			}

			SchumixBase.ExitStatus = true;
			SchumixBase.timer.SaveUptime();
			sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
			sSender.Quit(string.Format(text[1], sIRCMessage.Nick));
			Thread.Sleep(1000);
			Environment.Exit(1);
		}
	}
}
