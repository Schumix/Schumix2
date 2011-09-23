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
		public void See(string Name, string NickName)
		{
			if(!Running)
			{
				sSendMessage.SendCMPrivmsg(_channel, "{0}: Nem megy játék!", Name);
				return;
			}

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

			if((detective_.ToLower() == NickName.ToLower() && _detective) ||
				(detective2_.ToLower() == NickName.ToLower() && _detective2))
			{
				sSendMessage.SendCMPrivmsg(NickName, "Ma este már kikérdeztél valakit!");
				return;
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

			_rank = string.Empty;

			if(_killerlist.ContainsKey(Name.ToLower()))
				_rank = "killer";
			else if(_detectivelist.ContainsKey(Name.ToLower()))
				_rank = "detective";
			else if(_doctorlist.ContainsKey(Name.ToLower()))
				_rank = "doctor";
			else if(_normallist.ContainsKey(Name.ToLower()))
				_rank = "normal";

			if(_rank == "killer")
				sSendMessage.SendCMPrivmsg(NickName, "Most már bebizonyosodott, hogy ő a gyilkos! Buktasd le mielőtt még túl késő lenne...");
			else if(_rank == "normal")
				sSendMessage.SendCMPrivmsg(NickName, "Most már bebizonyosodott, hogy ő egy hétköznapi falusi.");
			else if(_rank == "doctor")
				sSendMessage.SendCMPrivmsg(NickName, "Most már bebizonyosodott, hogy ő a falu orvosa.");
			else if(_rank == "detective")
				sSendMessage.SendCMPrivmsg(NickName, "Most már bebizonyosodott, hogy te vagy az :D");

			if(detective_.ToLower() == NickName.ToLower())
				_detective = true;
			else if(detective2_.ToLower() == NickName.ToLower())
				_detective2 = true;
		}
	}
}