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
	public sealed partial class MaffiaGame
	{
		public void Left(string Name)
		{
			Left(Name, string.Empty);
		}

		public void Left(string Name, string NickName)
		{
			if(!Running)
			{
				sSendMessage.SendCMPrivmsg(_channel, "{0}: Nem megy játék!", Name);
				return;
			}

			if(NickName == string.Empty)
			{
				if(!_playerlist.ContainsValue(Name))
				{
					sSendMessage.SendCMPrivmsg(_channel, "{0}: Te már nem vagy játékos. Kérlek maradj csendben!", Name);
					return;
				}
			}
			else
			{
				if(!_playerlist.ContainsValue(NickName))
				{
					sSendMessage.SendCMPrivmsg(_channel, "{0}: Te már nem vagy játékos. Kérlek maradj csendben!", NickName);
					return;
				}
			}

			_rank = string.Empty;

			if(_killerlist.ContainsKey(Name.ToLower()))
			{
				_killerlist.Remove(Name.ToLower());
				_rank = "killer";
			}
			else if(_detectivelist.ContainsKey(Name.ToLower()))
			{
				_detectivelist.Remove(Name.ToLower());
				_ghostdetective = true;
				_rank = "detective";
			}
			else if(_normallist.ContainsKey(Name.ToLower()))
			{
				_normallist.Remove(Name.ToLower());
				_rank = "normal";
			}

			int i = 0;
			foreach(var player in _playerlist)
			{
				if(player.Value == Name)
				{
					i = player.Key;
					break;
				}
			}

			_playerlist.Remove(i);

			if(Started)
				_ghostlist.Add(Name.ToLower(), Name);

			sSender.Mode(_channel, "-v", Name);
			sSendMessage.SendCMPrivmsg(_channel, "{0} eltűnt egy különös féreglyukban.", Name);

			if(_rank == "killer")
				sSendMessage.SendCMPrivmsg(_channel, "{0}-nak izgalmas szerepe volt a játékban, mint gyilkos. Remélhetőleg halála izgalmasabb lesz.", Name);
			else if(_rank == "detective")
				sSendMessage.SendCMPrivmsg(_channel, "{0}-nak izgalmas szerepe volt a játékban, mint nyomozó. Remélhetőleg halála izgalmasabb lesz.", Name);
			else if(_rank == "normal")
				sSendMessage.SendCMPrivmsg(_channel, "{0}-nak unalmas szerepe volt a játékban, mint civil. Remélhetőleg halála izgalmasabb lesz.", Name);
			else
				sSendMessage.SendCMPrivmsg(_channel, "{0}-nak nem volt szerepe még a játékban. Remélhetőleg halála izgalmasabb lesz.", Name);

			if(Started)
				EndGame();
		}
	}
}