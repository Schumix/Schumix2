/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013-2014 Schumix Team <http://schumix.eu/>
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
using Schumix.Irc.Util;
using Schumix.Framework;
using Schumix.Framework.Irc;
using Schumix.Framework.Extensions;

namespace Schumix.ExtraAddon.Commands
{
	partial class Functions
	{
		public void HandleMessage(IRCMessage sIRCMessage)
		{
			var sMyNickInfo = sIrcBase.Networks[sIRCMessage.ServerName].sMyNickInfo;
			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;
			var sMyChannelInfo = sIrcBase.Networks[sIRCMessage.ServerName].sMyChannelInfo;

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("No1Value", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			if(!sMyChannelInfo.FSelect("message") || !sMyChannelInfo.FSelect("message", sIRCMessage.Channel))
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoMessageFunction", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			if(sIRCMessage.Info[4].ToLower() == "channel")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sIRCMessage.Info.Length < 7)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sIRCMessage.Info.Length < 8)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoMessage", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(!Rfc2812Util.IsValidChannelName(sIRCMessage.Info[5]))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaChannelHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(!Rfc2812Util.IsValidNick(sIRCMessage.Info[6]))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaNickNameHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sMyNickInfo.NickStorage.ToLower() == sIRCMessage.Info[6].ToLower())
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ICantLeftAMessageForMyself", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				SchumixBase.DManager.Insert("`message`(ServerId, ServerName, Name, Channel, Message, Wrote, UnixTime)", sIRCMessage.ServerId, sIRCMessage.ServerName, sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[5].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info.SplitToString(7, SchumixBase.Space)), sIRCMessage.SqlEscapeNick, sUtilities.UnixTime);
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("message/channel", sIRCMessage.Channel, sIRCMessage.ServerName));
			}
			else
			{
				if(sIRCMessage.Info.Length < 5)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoMessage", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(!Rfc2812Util.IsValidNick(sIRCMessage.Info[4]))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaNickNameHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sMyNickInfo.NickStorage.ToLower() == sIRCMessage.Info[4].ToLower())
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ICantLeftAMessageForMyself", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				SchumixBase.DManager.Insert("`message`(ServerId, ServerName, Name, Channel, Message, Wrote, UnixTime)", sIRCMessage.ServerId, sIRCMessage.ServerName, sUtilities.SqlEscape(sIRCMessage.Info[4].ToLower()), sIRCMessage.Channel.ToLower(), sUtilities.SqlEscape(sIRCMessage.Info.SplitToString(5, SchumixBase.Space)), sIRCMessage.SqlEscapeNick, sUtilities.UnixTime);
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("message", sIRCMessage.Channel, sIRCMessage.ServerName));
			}
		}
	}
}