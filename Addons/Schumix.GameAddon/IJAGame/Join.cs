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

namespace Schumix.GameAddon.IJAGames
{
	public sealed partial class IJAGame
	{
		public void Join(string Name)
		{
			if(_joinlist.Contains(Name.ToLower()))
			{
				sSendMessage.SendCMPrivmsg(_channel, "{0}: Már játékban vagy!", Name);
				return;
			}

			_joinlist.Add(Name.ToLower());

			if(!Running)
			{
				sSendMessage.SendCMPrivmsg(_channel, "{0}: Nem megy játék!", Name);
				return;
			}

			if(_joinstop)
			{
				sSendMessage.SendCMPrivmsg(_channel, "{0}: A játék épp most indult. Kérlek ne zavard a játékosokat!", Name);
				return;
			}

			if(Started)
			{
				sSendMessage.SendCMPrivmsg(_channel, "{0}: A játék már megy. Kérlek ne zavard a játékosokat!", Name);
				return;
			}

			if(!_playerlist.ContainsValue(Name))
			{
				sSendMessage.SendCMPrivmsg(_channel, string.Empty);

				int i = 0;
				foreach(var player in _playerlist)
				{
					if(player.Key > i)
						i = player.Key;
				}

				_playerlist.Add(i+1, Name);
				sSender.Mode(_channel, "+v", Name);
				sSendMessage.SendCMPrivmsg(_channel, "{0}: Bekerültél a játékba!", Name);
			}
			else
				sSendMessage.SendCMPrivmsg(_channel, "{0}: Már játékban vagy!", Name);
		}
	}
}