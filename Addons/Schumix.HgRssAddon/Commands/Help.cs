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
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework.Config;

namespace Schumix.HgRssAddon.Commands
{
	public partial class RssCommand : CommandInfo
	{
		public void Help()
		{
			// Operátor parancsok segítségei
			if(IsAdmin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Operator))
			{
				if(Network.IMessage.Info[4].ToLower() == "hg")
				{
					if(Network.IMessage.Info.Length < 6)
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Hg rss-ek kezelése.");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Hg parancsai: channel | info | lista | start | stop | reload");
						return;
					}

					if(Network.IMessage.Info[5].ToLower() == "channel")
					{
						if(Network.IMessage.Info.Length < 7)
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Rss csatornákra való kiirásának kezelése.");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Channel parancsai: add | del");
							return;
						}
						if(Network.IMessage.Info[6].ToLower() == "add")
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Új csatorna hozzáadása az rss-hez.");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}hg channel add <rss neve> <csatorna>", IRCConfig.CommandPrefix);
						}
						else if(Network.IMessage.Info[6].ToLower() == "del")
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nem használatos csatorna eltávolítása az rss-ből.");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}hg channel del <rss neve> <csatorna>", IRCConfig.CommandPrefix);
						}
					}
					else if(Network.IMessage.Info[5].ToLower() == "info")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kiirja az rss-ek állapotát.");
					}
					else if(Network.IMessage.Info[5].ToLower() == "lista")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Választható rss-ek listája.");
					}
					else if(Network.IMessage.Info[5].ToLower() == "start")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Új rss betöltése.");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}hg start <rss neve>", IRCConfig.CommandPrefix);
					}
					else if(Network.IMessage.Info[5].ToLower() == "stop")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Rss leállítása.");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}hg stop <rss neve>", IRCConfig.CommandPrefix);
					}
					else if(Network.IMessage.Info[5].ToLower() == "reload")
					{
						if(Network.IMessage.Info.Length < 7)
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Megadott rss újratöltése.");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Hg reload parancsai: all");
							return;
						}

						else if(Network.IMessage.Info[6].ToLower() == "all")
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Minden rss újratöltése.");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}hg reload <rss neve>", IRCConfig.CommandPrefix);
						}
					}
				}
			}
		}
	}
}

