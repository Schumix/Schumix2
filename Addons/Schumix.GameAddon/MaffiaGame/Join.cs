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

namespace Schumix.GameAddon.MaffiaGames
{
	sealed partial class MaffiaGame
	{
		public void Join(string Name)
		{
			var sSendMessage = sIrcBase.Networks[_servername].sSendMessage;
			var sSender = sIrcBase.Networks[_servername].sSender;

			var text = sLManager.GetCommandTexts("maffiagame/basecommand/join", _channel, _servername);
			if(text.Length < 6)
			{
				sSendMessage.SendCMPrivmsg(_channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(_channel, _servername)));
				return;
			}

			if(_joinlist.Contains(Name.ToLower()))
			{
				sSendMessage.SendCMPrivmsg(_channel, text[5], DisableHl(Name));
				return;
			}

			_joinlist.Add(Name.ToLower());

			if(!IsRunning(_channel, Name))
				return;

			if(_joinstop)
			{
				sSendMessage.SendCMPrivmsg(_channel, text[1], DisableHl(Name));
				return;
			}

			if(Started)
			{
				sSendMessage.SendCMPrivmsg(_channel, text[2], DisableHl(Name));
				return;
			}

			if(!_playerlist.ContainsValue(Name))
			{
				int i = 0;
				foreach(var player in _playerlist)
				{
					if(player.Key > i)
						i = player.Key;
				}

				_playerlist.Add(i+1, Name);
				sSender.Mode(_channel, "+v", Name);
				sSendMessage.SendCMPrivmsg(_channel, text[3], DisableHl(Name));
			}
			else
				sSendMessage.SendCMPrivmsg(_channel, text[4], DisableHl(Name));
		}
	}
}