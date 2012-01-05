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
using Schumix.Framework;
using Schumix.Framework.Extensions;

namespace Schumix.GameAddon.MaffiaGames
{
	sealed partial class MaffiaGame
	{
		public void Lynch(string Name, string NickName, string Channel)
		{
			if(_lynch)
				return;

			if(!Running)
			{
				sSendMessage.SendCMPrivmsg(Channel, "{0}: Nem megy játék!", Name);
				return;
			}

			if(!Started)
			{
				sSendMessage.SendCMPrivmsg(Channel, "{0}: Még nem kezdődött el játék!", Name);
				return;
			}

			if(!_killerlist.ContainsKey(NickName.ToLower()) && !_detectivelist.ContainsKey(NickName.ToLower()) &&
				!_normallist.ContainsKey(NickName.ToLower()) && !_doctorlist.ContainsKey(NickName.ToLower()))
			{
				sSendMessage.SendCMPrivmsg(Channel, "{0}: Kérlek maradj csendben amíg a játék véget ér.", NickName);
				return;
			}

			if(!_day)
			{
				sSendMessage.SendCMPrivmsg(Channel, "{0}: Csak nappal lehet lincselni!", NickName);
				return;
			}

			if(!_killerlist.ContainsKey(Name.ToLower()) && !_detectivelist.ContainsKey(Name.ToLower()) &&
				!_normallist.ContainsKey(Name.ToLower()) && !_ghostlist.ContainsKey(Name.ToLower()) &&
				!_doctorlist.ContainsKey(Name.ToLower()))
			{
				sSendMessage.SendCMPrivmsg(Channel, "{0}: Ilyen játékos nincs. Kérlek válasz mást!", NickName);
				return;
			}

			if(_ghostlist.ContainsKey(Name.ToLower()))
			{
				sSendMessage.SendCMPrivmsg(Channel, "{0}: Ő már halott. Szavazz másra!", NickName);
				return;
			}

			if(Name.ToLower() == NickName.ToLower())
			{
				sSendMessage.SendCMPrivmsg(Channel, "{0}: Önmagadat lincselnéd meg? Hülye vagy?", NickName);
				return;
			}

			foreach(var function in _playerflist)
			{
				if(function.Key == Name.ToLower())
				{
					if(function.Value.Lynch.Contains(NickName.ToLower()))
					{
						sSendMessage.SendCMPrivmsg(_channel, "{0}: Már szavaztál rá!", NickName);
						return;
					}
					else
						function.Value.Lynch.Add(NickName.ToLower());
				}
				else
				{
					if(function.Value.Lynch.Contains(NickName.ToLower()))
						function.Value.Lynch.Remove(NickName.ToLower());
				}
			}

			sSendMessage.SendCMPrivmsg(_channel, "{0} arra szavazott, hogy {1} legyen meglincselve!", NickName, Name);

			string namess = string.Empty;
			foreach(var function in _playerflist)
			{
				var sp = function.Value.Lynch.Count;
				if(sp > _lynchmaxnumber && sp <= (_playerlist.Count/2)+1)
					_lynchmaxnumber = sp;

				if(sp > 0)
					namess += " (" + function.Key + ": " + sp + " szavazat)";
			}

			if(_lynch)
				return;

			sSendMessage.SendCMPrivmsg(_channel, "{0} szavazat kell a többséghez. Jelenlegi szavazatok:{1}", (_playerlist.Count/2)+1, namess);

			if((_playerlist.Count/2)+1 <= _lynchmaxnumber)
			{
				_lynch = true;

				foreach(var function in _playerflist)
				{
					if(_lynchmaxnumber == function.Value.Lynch.Count)
					{
						namess = function.Key;
						break;
					}
				}

				_lynchmaxnumber = 0;
				namess = GetPlayerName(namess);
				RemovePlayer(namess);
				sSendMessage.SendCMPrivmsg(_channel, "A többség 4{0} lincselése mellett döntött! Elszabadulnak az indulatok. Ő mostantól már halott.", namess);

				if(GetPlayerMaster(namess))
					sSendMessage.SendCMPrivmsg(_channel, "Megölték a főnököt! Szemetek!!!");

				Corpse();
				Thread.Sleep(400);
				EndGame();

				if(_playerlist.Count >= 2 && Running)
				{
					sSendMessage.SendCMPrivmsg(_channel, "({0} meghalt, és nem szólhat hozzá a játékhoz.)", namess);
					sSendMessage.SendCMPrivmsg(namess, "Meghaltál. Kérlek maradj csendben amíg a játék véget ér.");
					_day = false;
					_stop = false;
				}

				_lynch = false;
			}
		}
	}
}