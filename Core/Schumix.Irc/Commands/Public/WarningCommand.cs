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

namespace Schumix.Irc.Commands
{
	public abstract partial class CommandHandler
	{
		protected void HandleWarning(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}
			
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host) && sIRCMessage.Info[4].Length > 0 && sIRCMessage.Info[4].Substring(0, 1) == "#")
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoOperator", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}
			
			if(sIRCMessage.Info.Length == 5)
				sSendMessage.SendChatMessage(sIRCMessage.MessageType, sIRCMessage.Info[4], sLManager.GetCommandText("warning", sIRCMessage.Channel, sIRCMessage.ServerName), sIRCMessage.Nick, sIRCMessage.Channel);
			else if(sIRCMessage.Info.Length >= 6)
				sSendMessage.SendChatMessage(sIRCMessage.MessageType, sIRCMessage.Info[4], "{0}", sIRCMessage.Info.SplitToString(5, SchumixBase.Space));
		}
	}
}