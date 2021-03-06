/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013-2015 Schumix Team <http://schumix.eu/>
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
using Schumix.Framework.Irc;
using Schumix.Framework.Config;

namespace Schumix.ExtraAddon.Commands
{
	partial class Functions
	{
		public bool Help(IRCMessage sIRCMessage)
		{
			// Fél Operátor parancsok segítségei
			if(sIRCMessage.Info[4].ToLower() == "autofunction")
			{
				var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;

				if(IsWarningAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.HalfOperator))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("WarningAdmin", sIRCMessage.Channel, sIRCMessage.ServerName));
					return true;
				}

				if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.HalfOperator))
					return false;

				if(sIRCMessage.Info.Length < 6)
				{
					var text = sLManager.GetCommandHelpTexts("autofunction", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 4)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return true;
					}

					if(IsAdmin(sIRCMessage.Nick, AdminFlag.HalfOperator))
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						sSendMessage.SendChatMessage(sIRCMessage, text[1]);
						return true;
					}
					else if(IsAdmin(sIRCMessage.Nick, AdminFlag.Operator))
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[2]);
						sSendMessage.SendChatMessage(sIRCMessage, text[3]);
						return true;
					}
					else if(IsAdmin(sIRCMessage.Nick, AdminFlag.Administrator))
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[2]);
						sSendMessage.SendChatMessage(sIRCMessage, text[3]);
						return true;
					}
				}
			}

			return false;
		}
	}
}