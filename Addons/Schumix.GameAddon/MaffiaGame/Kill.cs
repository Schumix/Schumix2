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
		public void Kill(string Name, string Killer)
		{
			if(!Running)
			{
				sSendMessage.SendCMPrivmsg(_channel, "{0}: Nem megy játék!", Name);
				return;
			}

			if(_killer && _players >= 8 && _killerlist.Count == 2)
			{
				sSendMessage.SendCMPrivmsg(Killer, "Már megegyeztek a gyilkosok!");
				return;
			}

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

			if(_ghostlist.ContainsKey(Name.ToLower()))
			{
				sSendMessage.SendCMPrivmsg(Killer, "Ő már halott. Válasz mást!");
				return;
			}

			if(!_killerlist.ContainsKey(Name.ToLower()) && !_detectivelist.ContainsKey(Name.ToLower()) &&
				!_doctorlist.ContainsKey(Name.ToLower()) && !_normallist.ContainsKey(Name.ToLower()))
			{
				sSendMessage.SendCMPrivmsg(Killer, "Kit akarsz megölni?");
				return;
			}

			if(Name.ToLower() == Killer.ToLower())
				sSendMessage.SendCMPrivmsg(Killer, "Önmagadat akarod megölni? Te tudod :P");

			sSendMessage.SendCMPrivmsg(Killer, "Elkönyveltem a szavazatodat.");

			if(_players < 8 || _killerlist.Count == 1)
			{
				if(_killerlist.ContainsKey(Name.ToLower()))
					newghost = _killerlist[Name.ToLower()];
				else if(_detectivelist.ContainsKey(Name.ToLower()))
					newghost = _detectivelist[Name.ToLower()];
				else if(_doctorlist.ContainsKey(Name.ToLower()))
					newghost = _doctorlist[Name.ToLower()];
				else if(_normallist.ContainsKey(Name.ToLower()))
					newghost = _normallist[Name.ToLower()];
	
				_killer = true;
			}
			else
			{
				if(killer_.ToLower() == Killer.ToLower())
					_newghost = Name.ToLower();

				if(killer2_.ToLower() == Killer.ToLower())
					_newghost2 = Name.ToLower();

				if(_newghost == _newghost2)
				{
					if(_killerlist.ContainsKey(_newghost))
						newghost = _killerlist[_newghost];
					else if(_detectivelist.ContainsKey(_newghost))
						newghost = _detectivelist[_newghost];
					else if(_doctorlist.ContainsKey(_newghost))
						newghost = _doctorlist[_newghost];
					else if(_normallist.ContainsKey(_newghost))
						newghost = _normallist[_newghost];

					foreach(var name in _killerlist)
						sSendMessage.SendCMPrivmsg(name.Key, "A gyilkosok megegyeztek!");

					_killer = true;
				}
			}
		}
	}
}