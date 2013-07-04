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
using Schumix.Framework;
using Schumix.Framework.Irc;
using Schumix.Framework.Config;

namespace Schumix.CompilerAddon.Commands
{
	partial class SCompiler
	{
		public bool Help(IRCMessage sIRCMessage)
		{
			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;
			string command = IRCConfig.List[sIRCMessage.ServerName].NickName + SchumixBase.Comma;

			if(sIRCMessage.Info[4].ToLower() == command.ToLower())
			{
				if(sIRCMessage.Info.Length < 6)
					return false;

				if(sIRCMessage.Info[5].ToLower() == "csc" || sIRCMessage.Info[5].ToLower() == "c#compiler")
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandHelpText("schumix2/csc", sIRCMessage.Channel, sIRCMessage.ServerName));
					return true;
				}
			}

			return false;
		}
	}
}