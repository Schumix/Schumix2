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

namespace Schumix.ExtraAddon.Commands
{
	public partial class Functions
	{
		public bool Help(IRCMessage sIRCMessage)
		{
			// Fél Operátor parancsok segítségei
			if(IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.HalfOperator))
			{
				if(sIRCMessage.Info[4].ToLower() == "autofunction")
				{
					if(sIRCMessage.Info.Length < 6)
					{
						var text = sLManager.GetCommandHelpTexts("autofunction", sIRCMessage.Channel);
						if(text.Length < 4)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
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
			}

			return false;
		}
	}
}