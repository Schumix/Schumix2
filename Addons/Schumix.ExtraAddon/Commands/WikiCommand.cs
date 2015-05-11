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
using Schumix.Framework;
using Schumix.Framework.Irc;
using Schumix.Framework.Extensions;

namespace Schumix.ExtraAddon.Commands
{
	partial class Functions
	{
		public void HandleWiki(IRCMessage sIRCMessage)
		{
			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;
			var text = sLManager.GetCommandTexts("wiki", sIRCMessage.Channel, sIRCMessage.ServerName);
			if(text.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
				return;
			}

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoGoogleText", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			string url = sUtilities.GetUrl("http://" + sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName).Substring(0, 2).ToLower()
				+ ".wikipedia.org/w/api.php?action=opensearch&format=xml&search=", sIRCMessage.Info.SplitToString(4, SchumixBase.Space));

			if(url.Contains("<Text xml:space=\"preserve\">"))
			{
				url = url.Replace("&quot;", "\"");
				url = url.Remove(0, url.IndexOf("<Text xml:space=\"preserve\">") + "<Text xml:space=\"preserve\">".Length);
				string _text = url.Substring(0, url.IndexOf("</Text>"));
				url = url.Remove(0, url.IndexOf("<Description xml:space=\"preserve\">") + "<Description xml:space=\"preserve\">".Length);
				string _des = url.Substring(0, url.IndexOf("</Description>"));
				url = url.Remove(0, url.IndexOf("<Url xml:space=\"preserve\">") + "<Url xml:space=\"preserve\">".Length);
				string _url = url.Substring(0, url.IndexOf("</Url>"));

				sSendMessage.SendChatMessage(sIRCMessage, text[0], _text);
				sSendMessage.SendChatMessage(sIRCMessage, text[1], _url);

				if(_des.Length <= 200)
					sSendMessage.SendChatMessage(sIRCMessage, text[2], _des);
				else
					sSendMessage.SendChatMessage(sIRCMessage, text[3], _des.Substring(0, 200));
			}
			else
				sSendMessage.SendChatMessage(sIRCMessage, text[4]);
		}
	}
}