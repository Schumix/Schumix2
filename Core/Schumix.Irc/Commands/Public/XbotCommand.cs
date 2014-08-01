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
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;

namespace Schumix.Irc.Commands
{
	public abstract partial class CommandHandler
	{
		protected void HandleXbot(IRCMessage sIRCMessage)
		{
			var text = sLManager.GetCommandTexts("xbot", sIRCMessage.Channel, sIRCMessage.ServerName);
			if(text.Length < 3)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
				return;
			}
			
			sSendMessage.SendChatMessage(sIRCMessage, text[0], sUtilities.GetVersion());
			string commands = string.Empty;
			string aliascommands = string.Empty;
			
			foreach(var command in sIrcBase.Networks[sIRCMessage.ServerName].CommandMethodMap)
			{
				if(command.Value.Permission != CommandPermission.Normal)
					continue;
				
				if(command.Key == "xbot")
					continue;
				
				if(sIgnoreCommand.IsIgnore(command.Key))
					continue;

				var db = SchumixBase.DManager.QueryFirstRow("SELECT BaseCommand FROM alias_irc_command WHERE NewCommand = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(command.Key), sIRCMessage.ServerName);
				if(!db.IsNull())
				{
					string basecommand = db["BaseCommand"].ToString();
					aliascommands += " | " + IRCConfig.List[sIRCMessage.ServerName].CommandPrefix + command.Key + "->" + IRCConfig.List[sIRCMessage.ServerName].CommandPrefix + basecommand;
					continue;
				}
				
				commands += " | " + IRCConfig.List[sIRCMessage.ServerName].CommandPrefix + command.Key;
			}
			
			sSendMessage.SendChatMessage(sIRCMessage, text[1], Consts.SchumixProgrammedBy);
			sSendMessage.SendChatMessage(sIRCMessage, text[2], commands.Remove(0, 3, " | "));

			if(!aliascommands.IsNullOrEmpty())
				sSendMessage.SendChatMessage(sIRCMessage, "\u0002\u00033Alias parancsok:\u000f\u000f \u0002{0}\u000f", aliascommands.Remove(0, 3, " | "));
		}
	}
}