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
			if(!_killerlist.Contains(Killer))
			{
				sSendMessage.SendCMPrivmsg(Killer, "{0}: Nem vagy gyilkos!", Killer);
				return;
			}

			if(!_playerlist.ContainsValue(Name))
			{
				sSendMessage.SendCMPrivmsg(Killer, "Kit akarsz megölni?");
				return;
			}

			if(_playerlist.Count < 8)
			{
				if(_killerlist.Contains(Name))
					_killerlist.Remove(Name);
				else if(_detectivelist.Contains(Name))
					_detectivelist.Remove(Name);
				else if(_normalmanlist.Contains(Name))
					_normalmanlist.Remove(Name);

				_ghostlist.Add(Name);
				sSendMessage.SendCMPrivmsg(Killer, "Elkönyveltem a szavazatodat.");
			}

			newghost = Name;
			_killer = true;
		}
	}
}