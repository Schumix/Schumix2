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

namespace Schumix.GameAddon.KillerGames
{
	public sealed partial class KillerGame
	{
		public void Kill(string Name, string Killer)
		{
			if(_day)
			{
				sSendMessage.SendCMPrivmsg(Killer, "Csak este ölhetsz!");
				return;
			}

			if(!_killerlist.ContainsKey(Killer.ToLower()))
			{
				sSendMessage.SendCMPrivmsg(Killer, "Nem vagy gyilkos!");
				return;
			}

			if(!_killerlist.ContainsKey(Name.ToLower()) && !_detectivelist.ContainsKey(Name.ToLower()) &&
				!_normallist.ContainsKey(Name.ToLower()))
			{
				sSendMessage.SendCMPrivmsg(Killer, "Kit akarsz megölni?");
				return;
			}

			if(Name.ToLower() == Killer.ToLower())
				sSendMessage.SendCMPrivmsg(Killer, "Önmagadat akarod megölni? Te tudod :P");

			sSendMessage.SendCMPrivmsg(Killer, "Elkönyveltem a szavazatodat.");

			if(_killerlist.ContainsKey(Name.ToLower()))
				newghost = _killerlist[Name.ToLower()];
			else if(_detectivelist.ContainsKey(Name.ToLower()))
				newghost = _detectivelist[Name.ToLower()];
			else if(_normallist.ContainsKey(Name.ToLower()))
				newghost = _normallist[Name.ToLower()];

			_killer = true;
		}
	}
}