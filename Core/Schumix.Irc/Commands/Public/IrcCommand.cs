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
using System.Data;
using Schumix.Api.Irc;
using Schumix.Framework;
using Schumix.Framework.Extensions;

namespace Schumix.Irc.Commands
{
	public abstract partial class CommandHandler
	{
		protected void HandleIrc(IRCMessage sIRCMessage)
		{
			var text = sLManager.GetCommandTexts("irc", sIRCMessage.Channel, sIRCMessage.ServerName);
			if(text.Length < 2)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
				return;
			}
			
			if(sIRCMessage.Info.Length == 4)
			{
				var db = SchumixBase.DManager.Query("SELECT Command FROM irc_commands WHERE Language = '{0}'", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName));
				if(!db.IsNull())
				{
					string commands = string.Empty;
					
					foreach(DataRow row in db.Rows)
						commands += " | " + row["Command"].ToString();
					
					if(commands == string.Empty)
						sSendMessage.SendChatMessage(sIRCMessage, text[0], "none");
					else
						sSendMessage.SendChatMessage(sIRCMessage, text[0], commands.Remove(0, 3, " | "));
				}
				else
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel, sIRCMessage.ServerName));
			}
			else
			{
				var db = SchumixBase.DManager.QueryFirstRow("SELECT Text FROM irc_commands WHERE Command = '{0}' AND Language = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[4]), sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName));
				if(!db.IsNull())
					sSendMessage.SendChatMessage(sIRCMessage, db["Text"].ToString());
				else
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
			}
		}
	}
}