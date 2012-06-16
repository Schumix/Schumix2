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

namespace Schumix.GameAddon.MaffiaGames
{
	sealed partial class MaffiaGame
	{
		public void See(string Name, string NickName)
		{
			if(!IsRunning(_channel, NickName))
				return;

			if(!IsStarted(_channel, NickName))
				return;

			if(_day)
			{
				sSendMessage.SendCMPrivmsg(NickName, "Csak este nyomozhatsz!");
				return;
			}

			if(!_detectivelist.ContainsKey(NickName.ToLower()))
			{
				sSendMessage.SendCMPrivmsg(NickName, "Nem vagy nyomozó!");
				return;
			}

			foreach(var function in _playerflist)
			{
				if(function.Key == NickName.ToLower() && function.Value.Detective)
				{
					sSendMessage.SendCMPrivmsg(NickName, "Ma este már kikérdeztél valakit!");
					return;
				}
			}

			if(_ghostlist.ContainsKey(Name.ToLower()))
			{
				sSendMessage.SendCMPrivmsg(NickName, "Ő már halott. Válasz mást!");
				return;
			}

			if(!_killerlist.ContainsKey(Name.ToLower()) && !_detectivelist.ContainsKey(Name.ToLower()) &&
				!_normallist.ContainsKey(Name.ToLower()) && !_doctorlist.ContainsKey(Name.ToLower()))
			{
				sSendMessage.SendCMPrivmsg(NickName, "Kit akarsz kikérdezni?");
				return;
			}

			if(Name.ToLower() == NickName.ToLower())
				sSendMessage.SendCMPrivmsg(NickName, "Önmagadat akarod kikérdezni? Te tudod :P");

			var rank = GetRank(Name);

			if(_killerlist.ContainsKey(Name.ToLower()))
				rank = Rank.Killer;
			else if(_detectivelist.ContainsKey(Name.ToLower()))
				rank = Rank.Detective;
			else if(_doctorlist.ContainsKey(Name.ToLower()))
				rank = Rank.Doctor;
			else if(_normallist.ContainsKey(Name.ToLower()))
				rank = Rank.Normal;

			sSendMessage.SendCMPrivmsg(NickName, "A jelentést reggel kapod meg!");

			foreach(var function in _playerflist)
			{
				if(function.Key == NickName.ToLower())
				{
					function.Value.Detective = true;
					function.Value.DRank = rank;
				}
			}
		}
	}
}