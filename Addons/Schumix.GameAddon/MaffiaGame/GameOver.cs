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

namespace Schumix.GameAddon.MaffiaGames
{
	sealed partial class MaffiaGame
	{
		public void GameOver(string Name)
		{
			if(!Running)
			{
				sSendMessage.SendCMPrivmsg(_channel, "{0}: Nem megy játék!", Name);
				return;
			}

			if(!Started)
			{
				sSendMessage.SendCMPrivmsg(_channel, "{0}: Még nem kezdődött el játék!", Name);
				return;
			}

			if(_gameoverlist.Contains(Name.ToLower()))
			{
				sSendMessage.SendCMPrivmsg(_channel, "{0}: Te már kérelmezted a leállítást!", Name);
				return;
			}

			_gameoverlist.Add(Name.ToLower());
			sSendMessage.SendCMPrivmsg(_channel, "{0} arra szavazott, hogy vége legyen a játéknak!", Name);
			sSendMessage.SendCMPrivmsg(_channel, "Jelenleg {0} játékos kívánja leállítani!", _gameoverlist.Count);

			if(_gameoverlist.Count >= (_playerlist.Count/2)+1)
			{
				RemoveRanks();
				sSendMessage.SendCMPrivmsg(_channel, "A többség megszavazta a játék leállítását!");
				sSendMessage.SendCMPrivmsg(_channel, "A játék befejeződött.");
				StopThread();
			}
		}
	}
}