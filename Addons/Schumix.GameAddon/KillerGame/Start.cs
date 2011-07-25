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
using System.Threading;
using System.Collections.Generic;

namespace Schumix.GameAddon.KillerGames
{
	public sealed partial class KillerGame
	{
		public void Start()
		{
			if(_playerlist.Count < 4)
			{
				sSendMessage.SendCMPrivmsg(_channel, "teszt");
				return;
			}

			var list = new Dictionary<int, string>();
			foreach(var l in _playerlist)
				list.Add(l.Key, l.Value);

			if(list.Count < 8)
			{
				var rand = new Random();
				int number = rand.Next(1, list.Count);
				bool killer = true;
				bool detective = true;

				for(;;)
				{
					number = rand.Next(1, list.Count);

					if(killer)
					{
						string name = string.Empty;
						list.TryGetValue(number, out name);
						_killerlist.Add(name);
						list.Remove(number);
						killer = false;
						continue;
					}
					else if(detective)
					{
						string name = string.Empty;
						list.TryGetValue(number, out name);
						_detectivelist.Add(name);
						list.Remove(number);
						detective = false;
						continue;
					}
					else
					{
						foreach(var llist in list)
							_normalmanlist.Add(llist.Value);

						break;
					}
				}

				foreach(var name in _killerlist)
					sSendMessage.SendCMPrivmsg(name, "Te egy gyilkos vagy. Célod megölni minden falusit. Csak viselkedj természetesen!");

				foreach(var name in _detectivelist)
					sSendMessage.SendCMPrivmsg(name, "Te vagy a nyomozó. A te dolgod éjszakánként követni 1-1 embert, hogy megtudd, ki is ő valójában, mielőtt még túl késő lenne. Ha szerencséd van, a falusiak hisznek neked - és talán nem lincselnek meg...");

				foreach(var name in _normalmanlist)
					sSendMessage.SendCMPrivmsg(name, "Te egy teljesen hétköznapi civil vagy. Nincs más dolgod, mint kiválasztani nappal, hogy ki lehet a gyilkos, akit meglincseltek, éjszakánként pedig imádkozni az életedért...");
			}

			sSendMessage.SendCMPrivmsg(_channel, "Új játék lett indítva! Most mindenki megkapja a szerepét.");
			sSender.Mode(_channel, "+m");
			Started = true;
			Thread.Sleep(1000);
			StartThread();
		}
	}
}