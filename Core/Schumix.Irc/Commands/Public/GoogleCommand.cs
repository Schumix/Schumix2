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
using Schumix.Irc.Commands.GoogleWebSearch;

namespace Schumix.Irc.Commands
{
	public abstract partial class CommandHandler
	{
		protected void HandleGoogle(IRCMessage sIRCMessage)
		{
			var text = sLManager.GetCommandTexts("google", sIRCMessage.Channel, sIRCMessage.ServerName);
			if(text.Length < 4)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
				return;
			}
			
			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoGoogleText", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}
			
			string url = sUtilities.GetUrl("http://ajax.googleapis.com/ajax/services/search/web?v=1.0&q=", sIRCMessage.Info.SplitToString(4, SchumixBase.Space));
			var google = new GoogleWebResponseData();
			google = JsonHelper.Deserialise<GoogleWebResponseData>(url);
			
			if(google.ResultSet.Results.Length > 0)
			{
				sSendMessage.SendChatMessage(sIRCMessage, text[2], google.ResultSet.Results[0].TitleNoFormatting);
				sSendMessage.SendChatMessage(sIRCMessage, text[3], google.ResultSet.Results[0].UnescapedUrl);
			}
			else
			{
				sSendMessage.SendChatMessage(sIRCMessage, text[0]);
				sSendMessage.SendChatMessage(sIRCMessage, text[1]);
			}
		}
	}
}