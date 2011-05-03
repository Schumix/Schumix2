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

namespace Schumix.CalendarAddon.Commands
{
	public partial class BannedCommand
	{
		public void Help(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info[4].ToLower() == "banned")
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Tiltást rak a megadott névre vagy vhost-ra.");
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Használata:");
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Óra és perc: {0}banned <név> <óó:pp> <oka>", IRCConfig.CommandPrefix);
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Dátum, Óra és perc: {0}banned <név> <éééé.hh.nn> <óó:pp> <oka>", IRCConfig.CommandPrefix);
			}
			else if(sIRCMessage.Info[4].ToLower() == "unbanned")
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Feloldja a tiltást a névről vagy vhost-ról ha szerepel a bot rendszerében.");
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Használata: {0}unbanned <név vagy vhost>", IRCConfig.CommandPrefix);
			}
		}
	}
}