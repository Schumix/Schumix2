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
using System.Text.RegularExpressions;
using Schumix.API.Irc;
using Schumix.Framework;
using Schumix.Framework.Extensions;

namespace Schumix.Irc.Commands
{
	public abstract partial class CommandHandler
	{
		protected void HandleTranslate(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoTranslateLanguage", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}
			
			if(!sIRCMessage.Info[4].Contains("|"))
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoTranslateLanguage", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}
			
			if(sIRCMessage.Info.Length < 6)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoTranslateText", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}
			
			string url = sUtilities.GetUrl("http://www.google.com/translate_t?hl=en&ie=UTF8&text=", sIRCMessage.Info.SplitToString(5, SchumixBase.Space), "&langpair=" + sIRCMessage.Info[4]);
			var Regex = new Regex("mouseout=\"this.style.backgroundColor='#fff'\">(?<text>.+)</span></span></div></div>");
			
			if(!Regex.IsMatch(url))
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("translate", sIRCMessage.Channel, sIRCMessage.ServerName));
			else
				sSendMessage.SendChatMessage(sIRCMessage, "{0}", Regex.Match(url).Groups["text"].ToString().Replace("&#39;", "'"));
		}
	}
}