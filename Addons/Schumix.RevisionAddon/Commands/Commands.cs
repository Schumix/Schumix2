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

namespace Schumix.RevisionAddon.Commands
{
	partial class Revision : CommandInfo
	{
		protected void HandleXrev(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info.Length < 5)
			{
				//sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs paraméter!");
				return;
			}

			if(sIRCMessage.Info[4].ToLower() == "add")
			{
			}
			else if(sIRCMessage.Info[4].ToLower() == "remove")
			{
			}
			else if(sIRCMessage.Info[4].ToLower() == "info")
			{
			}
			else
			{
			}
		}
	}
}