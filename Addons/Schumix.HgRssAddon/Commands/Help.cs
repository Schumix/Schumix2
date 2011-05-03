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
using Schumix.API;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework.Config;

namespace Schumix.HgRssAddon.Commands
{
	public partial class RssCommand : CommandInfo
	{
		public void Help(IRCMessage sIRCMessage)
		{
			// Operátor parancsok segítségei
			if(IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
			{
				if(sIRCMessage.Info[4].ToLower() == "hg")
				{
					if(sIRCMessage.Info.Length < 6)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Hg rss-ek kezelése.");
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Hg parancsai: channel | info | lista | start | stop | reload");
						return;
					}

					if(sIRCMessage.Info[5].ToLower() == "channel")
					{
						if(sIRCMessage.Info.Length < 7)
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Rss csatornákra való kiirásának kezelése.");
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Channel parancsai: add | del");
							return;
						}
						if(sIRCMessage.Info[6].ToLower() == "add")
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Új csatorna hozzáadása az rss-hez.");
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Használata: {0}hg channel add <rss neve> <csatorna>", IRCConfig.CommandPrefix);
						}
						else if(sIRCMessage.Info[6].ToLower() == "del")
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nem használatos csatorna eltávolítása az rss-ből.");
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Használata: {0}hg channel del <rss neve> <csatorna>", IRCConfig.CommandPrefix);
						}
					}
					else if(sIRCMessage.Info[5].ToLower() == "info")
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Kiirja az rss-ek állapotát.");
					}
					else if(sIRCMessage.Info[5].ToLower() == "lista")
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Választható rss-ek listája.");
					}
					else if(sIRCMessage.Info[5].ToLower() == "start")
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Új rss betöltése.");
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Használata: {0}hg start <rss neve>", IRCConfig.CommandPrefix);
					}
					else if(sIRCMessage.Info[5].ToLower() == "stop")
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Rss leállítása.");
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Használata: {0}hg stop <rss neve>", IRCConfig.CommandPrefix);
					}
					else if(sIRCMessage.Info[5].ToLower() == "reload")
					{
						if(sIRCMessage.Info.Length < 7)
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Megadott rss újratöltése.");
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Hg reload parancsai: all");
							return;
						}

						else if(sIRCMessage.Info[6].ToLower() == "all")
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Minden rss újratöltése.");
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Használata: {0}hg reload <rss neve>", IRCConfig.CommandPrefix);
						}
					}
				}
			}
		}
	}
}

