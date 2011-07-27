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
			if(Started)
			{
				sSendMessage.SendCMPrivmsg(_channel, "A játék már megy!");
				return;
			}

			if(_playerlist.Count < 4)
			{
				sSendMessage.SendCMPrivmsg(_channel, "A játék indításához minimum 4 játékos kell!");
				return;
			}

			_joinstop = true;

			var list = new Dictionary<int, string>();
			foreach(var l in _playerlist)
				list.Add(l.Key, l.Value);

			_detectivelist.Clear();
			_killerlist.Clear();
			_doctorlist.Clear();
			_normallist.Clear();

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
						if(list.ContainsKey(number))
						{
							string name = string.Empty;
							list.TryGetValue(number, out name);
							_killerlist.Add(name.ToLower(), name);
							list.Remove(number);
							killer = false;
							killer_ = name;
						}

						continue;
					}
					else if(detective)
					{
						if(list.ContainsKey(number))
						{
							string name = string.Empty;
							list.TryGetValue(number, out name);
							_detectivelist.Add(name.ToLower(), name);
							list.Remove(number);
							detective = false;
							detective_ = name;
						}

						continue;
					}
					else
					{
						foreach(var llist in list)
							_normallist.Add(llist.Value.ToLower(), llist.Value);
						break;
					}
				}

				foreach(var name in _killerlist)
					sSendMessage.SendCMPrivmsg(name.Key, "Te egy gyilkos vagy. Célod megölni minden falusit. Csak viselkedj természetesen!");

				foreach(var name in _detectivelist)
					sSendMessage.SendCMPrivmsg(name.Key, "Te vagy a nyomozó. A te dolgod éjszakánként követni 1-1 embert, hogy megtudd, ki is ő valójában, mielőtt még túl késő lenne. Ha szerencséd van, a falusiak hisznek neked - és talán nem lincselnek meg...");

				foreach(var name in _normallist)
					sSendMessage.SendCMPrivmsg(name.Key, "Te egy teljesen hétköznapi civil vagy. Nincs más dolgod, mint kiválasztani nappal, hogy ki lehet a gyilkos, akit meglincseltek, éjszakánként pedig imádkozni az életedért...");
			}

			list.Clear();
			Started = true;
			_players = _playerlist.Count;
			sSendMessage.SendCMPrivmsg(_channel, "Új játék lett indítva! Most mindenki megkapja a szerepét.");
			_joinstop = false;
			sSender.Mode(_channel, "+m");
			Thread.Sleep(1000);
			StartThread();
		}
	}
}