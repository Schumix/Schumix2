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
//using System.IO;
//using System.Collections.Generic;
//using System.Text;
//using System.Xml;
using System.Threading;
using Schumix.Framework;
using Schumix.Framework.Config;
/*using Atom.Core;
using Atom.Utils;
using Atom.AdditionalElements;
using Atom.Core.Collections;*/

namespace Schumix.Irc.Commands
{
	public partial class CommandHandler
	{
		protected void HandlePlugin()
		{
			if(!Admin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Administrator))
				return;

			CNick();

			if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4] == "load")
			{
				if(Network.IMessage.Info.Length < 6)
					return;

				string name = Network.IMessage.Info[5];

				if(name == "all")
				{
					if(ScriptManager.LoadPlugins())
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "2[Load]: All plugins 3done.");
					}
					else
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "2[Load]: All plugins 5failed.");
				}
				else
				{
					if(ScriptManager.LoadPlugin(name))
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "2[Load]: {0} 3done.", name);
					}
					else
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "2[Load]: {0} 5failed.", name);
				}
			}
			else if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4] == "unload")
			{
				string name = Network.IMessage.Info[5];

				if(name == "all")
				{
					/*if(ScriptManager.UnloadPlugins())
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "2[Unload]: All plugins 3done.");
					}
					else
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "2[Unload]: All plugins 5failed.");*/
				}
				else
				{
					if(ScriptManager.UnloadPlugin(name))
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "2[Unload]: {0} 3done.", name);
					}
					else
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "2[Unload]: {0} 5failed.", name);
				}
			}
			else
			{
				foreach(var plugin in ScriptManager.GetPlugins())
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "{0}: 3loaded.", plugin.Name.Replace("Plugin", string.Empty));
				}
			}
		}

		protected void HandleKikapcs()
		{
			if(!Admin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Administrator))
				return;

			CNick();
			SchumixBase.SaveUptime();
			sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Viszlát :(");
			sSender.Quit(String.Format("{0} leállított parancsal.", Network.IMessage.Nick));
			Thread.Sleep(1000);
			Environment.Exit(1);
		}
	}
}
