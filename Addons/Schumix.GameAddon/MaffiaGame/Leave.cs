/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Megax <http://www.megaxx.info/>
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
using Schumix.Framework.Extensions;

namespace Schumix.GameAddon.MaffiaGames
{
	sealed partial class MaffiaGame
	{
		public void Leave(string Name)
		{
			Leave(Name, string.Empty);
		}

		public void Leave(string Name, string NickName)
		{
			if(!IsRunning(_channel, NickName))
				return;

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

			if(_start)
			{
				_leftlist.Add(Name);
				return;
			}

			if(_playerflist.ContainsKey(Name.ToLower()))
			{
				foreach(var function in _playerflist)
				{
					if(function.Value.Lynch.Contains(Name.ToLower()))
						function.Value.Lynch.Remove(Name.ToLower());
				}

				_playerflist[Name.ToLower()].Lynch.Clear();
				//_playerflist.Remove(Name.ToLower());		
			}

			sSender.Mode(_channel, "-v", Name);
			RemovePlayer(Name);
			sSendMessage.SendCMPrivmsg(_channel, "{0} eltűnt egy különös féreglyukban.", Name);
			var rank = GetRank(Name);

			if(rank == Rank.Killer)
				sSendMessage.SendCMPrivmsg(_channel, "{0}-nak izgalmas szerepe volt a játékban, mint gyilkos. Remélhetőleg halála izgalmasabb lesz.", Name);
			else if(rank == Rank.Detective)
				sSendMessage.SendCMPrivmsg(_channel, "{0}-nak izgalmas szerepe volt a játékban, mint nyomozó. Remélhetőleg halála izgalmasabb lesz.", Name);
			else if(rank == Rank.Doctor)
				sSendMessage.SendCMPrivmsg(_channel, "{0}-nak izgalmas szerepe volt a játékban, mint orvos. Remélhetőleg halála izgalmasabb lesz.", Name);
			else if(rank == Rank.Normal)
				sSendMessage.SendCMPrivmsg(_channel, "{0}-nak unalmas szerepe volt a játékban, mint civil. Remélhetőleg halála izgalmasabb lesz.", Name);
			else
				sSendMessage.SendCMPrivmsg(_channel, "{0}-nak nem volt szerepe még a játékban. Remélhetőleg halála izgalmasabb lesz.", Name);

			if(_owner == Name)
			{
				_owner = string.Empty;

				if(_timerowner.Enabled)
				{
					_timerowner.Enabled = false;
					_timerowner.Elapsed -= HandleIsOwnerAfk;
					_timerowner.Stop();
				}

				if(_playerlist.Count > 0)
					sSendMessage.SendCMPrivmsg(_channel, "A játék indítója lelépett. A játékot mostantól bárki írányíthatja!");
			}

			if(Started)
			{
				_lynchmaxnumber = 0;

				foreach(var function in _playerflist)
				{
					var sp = function.Value.Lynch.Count;
					if(sp > _lynchmaxnumber && sp <= (_playerlist.Count/2)+1)
						_lynchmaxnumber = sp;
				}

				EndGame();
			}
			else
			{
				if(_playerlist.Count == 0)
				{
					EndGameText();
					StopThread();
				}
			}
		}
	}
}