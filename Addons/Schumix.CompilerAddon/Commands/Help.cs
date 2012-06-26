/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Megax <http://www.megaxx.info/>
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
using Schumix.Framework;
using Schumix.Framework.Config;

namespace Schumix.CompilerAddon.Commands
{
	partial class SCompiler
	{
		public bool Help(IRCMessage sIRCMessage)
		{
			string command = IRCConfig.NickName + SchumixBase.Comma;

			if(sIRCMessage.Info[4].ToLower() == command.ToLower())
			{
				if(sIRCMessage.Info.Length < 6)
					return false;

				if(sIRCMessage.Info[5].ToLower() == "csc" || sIRCMessage.Info[5].ToLower() == "c#compiler")
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandHelpText("schumix2/csc", sIRCMessage.Channel));
					return true;
				}
			}

			return false;
		}
	}
}