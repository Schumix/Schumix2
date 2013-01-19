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
using Schumix.Irc.Util;

namespace Schumix.Irc.Commands
{
	public abstract partial class CommandHandler
	{
		protected void HandleLeave(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.HalfOperator))
				return;
			
			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}
			
			if(!Rfc2812Util.IsValidChannelName(sIRCMessage.Info[4]))
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaChannelHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}
			
			if(!sChannelList.List.ContainsKey(sIRCMessage.Info[4].ToLower()))
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ImNotOnThisChannel", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}
			
			sSender.Part(sIRCMessage.Info[4]);
			sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("leave", sIRCMessage.Channel, sIRCMessage.ServerName), sIRCMessage.Info[4]);
		}
	}
}