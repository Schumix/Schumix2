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
using Schumix.Framework.Irc;
using Schumix.Framework.Config;

namespace Schumix.Irc.Commands
{
	public abstract partial class CommandHandler
	{
		protected void HandleInfo(IRCMessage sIRCMessage)
		{
			var text = sLManager.GetCommandTexts("info", sIRCMessage.Channel, sIRCMessage.ServerName);
			if(text.Length < 4)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
				return;
			}
			
			sSendMessage.SendChatMessage(sIRCMessage, text[0], Consts.SchumixProgrammedBy);
			sSendMessage.SendChatMessage(sIRCMessage, text[1], Consts.SchumixDevelopers);
			sSendMessage.SendChatMessage(sIRCMessage, text[2], Consts.SchumixWebsite);
			sSendMessage.SendChatMessage(sIRCMessage, text[3]);
		}
	}
}