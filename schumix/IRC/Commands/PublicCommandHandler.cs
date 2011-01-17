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

// nincs egyenlőre adat

using System;
using Schumix.Config;

namespace Schumix.IRC.Commands
{
	public partial class CommandHandler
	{
		public void HandleXbot()
		{
			sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("3Verzió: 10{0}", SchumixBot.revision));
			sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("3Parancsok: {0}info | {1}help | {2}ido | {3}datum | {4}irc | {5}roll | {6}keres | {7}sha1 | {8}md5 | {9}uzenet | {10}whois | {11}calc", IRCConfig.Parancselojel, IRCConfig.Parancselojel, IRCConfig.Parancselojel, IRCConfig.Parancselojel, IRCConfig.Parancselojel, IRCConfig.Parancselojel, IRCConfig.Parancselojel, IRCConfig.Parancselojel, IRCConfig.Parancselojel, IRCConfig.Parancselojel, IRCConfig.Parancselojel, IRCConfig.Parancselojel));
			sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Programmed by: 3Csaba");
		}
	}
}
