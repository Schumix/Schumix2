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
using System.Threading;
using Schumix.Api.Irc;
using Schumix.Irc.Util;
using Schumix.Framework;
using Schumix.Framework.Extensions;

namespace Schumix.Irc.Commands
{
	public abstract partial class CommandHandler
	{
		protected void HandleKick(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
				return;

			if(!Rfc2812Util.IsValidChannelName(sIRCMessage.Channel))
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ThisIsNotAChannelDoNotWriteInPM", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			bool iskick = false;
			string kick = sIRCMessage.Info[4].ToLower();

			if(!Rfc2812Util.IsValidNick(kick))
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaNickNameHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			if(sIRCMessage.Info.Length == 5)
			{
				if(kick == sMyNickInfo.NickStorage.ToLower())
					sSendMessage.SendChatMessage(sIRCMessage, "Önmagamat nem rúghatom ki a csatornáról!"/*sLManager.GetWarningText("", sIRCMessage.Channel, sIRCMessage.ServerName)*/);
				else
				{
					iskick = true;
					KickPrivmsg = sIRCMessage.Channel;
					sSender.Kick(sIRCMessage.Channel, kick);
				}
			}
			else if(sIRCMessage.Info.Length >= 6)
			{
				if(kick == sMyNickInfo.NickStorage.ToLower())
					sSendMessage.SendChatMessage(sIRCMessage, "Önmagamat nem rúghatom ki a csatornáról!"/*sLManager.GetWarningText("", sIRCMessage.Channel, sIRCMessage.ServerName)*/);
				else
				{
					iskick = true;
					KickPrivmsg = sIRCMessage.Channel;
					sSender.Kick(sIRCMessage.Channel, kick, sIRCMessage.Info.SplitToString(5, SchumixBase.Space));
				}
			}

			if(iskick)
			{
				// Clean
				Thread.Sleep(5*1000);

				if(sIRCMessage.Channel.ToLower() == KickPrivmsg.ToLower() || !KickPrivmsg.IsNullOrEmpty())
					KickPrivmsg = string.Empty;
			}
		}
	}
}