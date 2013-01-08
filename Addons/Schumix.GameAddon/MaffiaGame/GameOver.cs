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

namespace Schumix.GameAddon.MaffiaGames
{
	sealed partial class MaffiaGame
	{
		public void GameOver(string Name)
		{
			var sSendMessage = sIrcBase.Networks[_servername].sSendMessage;

			if(!IsRunning(_channel, Name))
				return;

			if(!IsStarted(_channel, Name))
				return;

			var text = sLManager.GetCommandTexts("maffiagame/basecommand/gameover", _channel, _servername);
			if(text.Length < 4)
			{
				sSendMessage.SendCMPrivmsg(_channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(_channel, _servername)));
				return;
			}

			if(_gameoverlist.Contains(Name.ToLower()))
			{
				sSendMessage.SendCMPrivmsg(_channel, text[0], DisableHl(Name));
				return;
			}

			_gameoverlist.Add(Name.ToLower());
			sSendMessage.SendCMPrivmsg(_channel, text[1], DisableHl(Name));
			sSendMessage.SendCMPrivmsg(_channel, text[2], _gameoverlist.Count, (_playerlist.Count/2)+1);

			if(_gameoverlist.Count >= (_playerlist.Count/2)+1)
			{
				RemoveRanks();
				sSendMessage.SendCMPrivmsg(_channel, text[3]);
				EndGameText();
				StopThread();
			}
		}
	}
}