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
using Schumix.Framework;
using Schumix.Framework.Irc;
using Schumix.Framework.Extensions;

namespace Schumix.ExtraAddon.Commands
{
	partial class Functions
	{
		public void HandlePrime(IRCMessage sIRCMessage)
		{
			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;
			var text = sLManager.GetCommandTexts("prime", sIRCMessage.Channel, sIRCMessage.ServerName);
			if(text.Length < 3)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
				return;
			}

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoNumber", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			if(!sIRCMessage.Info[4].IsNumber())
			{
				sSendMessage.SendChatMessage(sIRCMessage, text[0]);
				return;
			}

			bool prim = sUtilities.IsPrime(sIRCMessage.Info[4].ToInt32());

			if(!prim)
				sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Info[4]);
			else
				sSendMessage.SendChatMessage(sIRCMessage, text[2], sIRCMessage.Info[4]);
		}
	}
}