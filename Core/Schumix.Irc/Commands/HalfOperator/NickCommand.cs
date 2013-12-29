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

namespace Schumix.Irc.Commands
{
	public abstract partial class CommandHandler
	{
		protected void HandleNick(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.HalfOperator))
				return;

			var text = sLManager.GetCommandTexts("nick", sIRCMessage.Channel, sIRCMessage.ServerName);
			if(text.Length < 2)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
				return;
			}
			
			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}
			
			sIrcBase.Networks[sIRCMessage.ServerName].NewNick = true;
			string nick = sIRCMessage.Info[4];

			if(!Rfc2812Util.IsValidNick(nick))
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaNickNameHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			if(sMyNickInfo.NickStorage == nick)
			{
				sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				return;
			}

			if(sChannelList.List[sIRCMessage.Channel.ToLower()].Names.ContainsKey(nick.ToLower()) && sMyNickInfo.NickStorage.ToLower() != nick.ToLower())
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLConsole.MessageHandler("Text14", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
				return;
			}

			NewNickPrivmsg = sIRCMessage.Channel;
			sMyNickInfo.ChangeNick(nick);
			sSendMessage.SendChatMessage(sIRCMessage, text[0], nick);
			sSender.Nick(nick);
		}
	}
}