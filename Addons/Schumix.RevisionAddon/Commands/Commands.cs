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
using Schumix.API;
using Schumix.API.Irc;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Localization;
using Schumix.RevisionAddon.Githubs;

namespace Schumix.RevisionAddon.Commands
{
	class Revision : CommandInfo
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;

		public Revision(string ServerName) : base(ServerName)
		{
		}

		public void HandleXrev(IRCMessage sIRCMessage)
		{
			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoValue", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			if(sIRCMessage.Info[4].ToLower() == "github")
			{
				var text = sLManager.GetCommandTexts("xrev/github", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 7)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}

				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[0]);
					return;
				}

				if(sIRCMessage.Info.Length < 7)
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
					return;
				}

				if(sIRCMessage.Info.Length < 8)
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[2]);
					return;
				}

				var github = new Github(sIRCMessage.Info[5], sIRCMessage.Info[6], sIRCMessage.Info[7]);

				if(github.UserName == string.Empty || github.Message == string.Empty || github.Url == string.Empty)
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[3]);
					return;
				}

				sSendMessage.SendChatMessage(sIRCMessage, text[4], github.Message);
				sSendMessage.SendChatMessage(sIRCMessage, text[5], github.Url);
				sSendMessage.SendChatMessage(sIRCMessage, text[6], github.UserName);
			}
		}
	}
}