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
using Schumix.Framework.Extensions;

namespace Schumix.GameAddon.MaffiaGames
{
	public sealed partial class MaffiaGame
	{
		public void Lynch(string Name, string NickName)
		{
			if(!Running)
			{
				sSendMessage.SendCMPrivmsg(_channel, "{0}: Nem megy játék!", Name);
				return;
			}

			if(!_killerlist.ContainsKey(NickName.ToLower()) && !_detectivelist.ContainsKey(NickName.ToLower()) &&
				!_normallist.ContainsKey(NickName.ToLower()) && !_doctorlist.ContainsKey(NickName.ToLower()))
			{
				sSendMessage.SendCMPrivmsg(_channel, "{0}: Kérlek maradj csendben amíg a játék véget ér.", NickName);
				return;
			}

			if(!_killerlist.ContainsKey(Name.ToLower()) && !_detectivelist.ContainsKey(Name.ToLower()) &&
				!_normallist.ContainsKey(Name.ToLower()) && !_ghostlist.ContainsKey(Name.ToLower()) &&
				!_doctorlist.ContainsKey(Name.ToLower()))
			{
				sSendMessage.SendCMPrivmsg(_channel, "{0}: Ilyen játékos nincs. Kérlek válasz mást!", NickName);
				return;
			}

			if(_ghostlist.ContainsKey(Name.ToLower()))
			{
				sSendMessage.SendCMPrivmsg(_channel, "{0}: Ő már halott. Szavazz másra!", NickName);
				return;
			}

			if(Name.ToLower() == NickName.ToLower())
			{
				sSendMessage.SendCMPrivmsg(_channel, "{0}: Önmagadat lincselnéd meg? Hülye vagy?", NickName);
				return;
			}

			if(!_day)
			{
				sSendMessage.SendCMPrivmsg(_channel, "{0}: Csak nappal lehet lincselni!", NickName);
				return;
			}

			if(!_lynchlist.ContainsKey(Name.ToLower()))
			{
				string name = string.Empty;
				string names = string.Empty;
				string[] split = { string.Empty };

				foreach(var list in _lynchlist)
				{
					if(list.Value.Contains(NickName.ToLower()))
					{
						name = list.Key;

						if(Name.ToLower() == name)
						{
							sSendMessage.SendCMPrivmsg(_channel, "{0}: Már szavaztál rá!", NickName);
							return;
						}

						split = list.Value.Split(',');
					}
				}

				_lynchlist.Remove(name);
				names = string.Empty;

				if(split.Length > 1)
				{
					foreach(var spl in split)
					{
						if(NickName.ToLower() == spl)
							continue;
						else
							names += "," + spl;
					}
	
					_lynchlist.Add(name, names.Remove(0, 1, ","));
				}
				else
					_lynchlist.Remove(name);

				_lynchlist.Add(Name.ToLower(), NickName.ToLower());
			}
			else
			{
				string name = string.Empty;
				string names = string.Empty;
				string[] split = { string.Empty };

				foreach(var list in _lynchlist)
				{
					if(list.Key == Name.ToLower())
						names = list.Value;

					if(list.Value.Contains(NickName.ToLower()))
					{
						name = list.Key;

						if(Name.ToLower() == name)
						{
							sSendMessage.SendCMPrivmsg(_channel, "{0}: Már szavaztál rá!", NickName);
							return;
						}

						split = list.Value.Split(',');
					}
				}

				_lynchlist.Remove(Name.ToLower());
				_lynchlist.Add(Name.ToLower(), names + "," + NickName.ToLower());
				_lynchlist.Remove(name);
				names = string.Empty;

				if(split.Length > 1)
				{
					foreach(var spl in split)
					{
						if(NickName.ToLower() == spl)
							continue;
						else
							names += "," + spl;
					}
	
					_lynchlist.Add(name, names.Remove(0, 1, ","));
				}
			}

			sSendMessage.SendCMPrivmsg(_channel, "{0} arra szavazott, hogy {1} legyen meglincselve!", NickName, Name);

			string namess = string.Empty;
			foreach(var list in _lynchlist)
			{
				var sp = list.Value.Split(',').Length;
				if(sp > _lynchmaxnumber)
					_lynchmaxnumber = sp;

				namess += " (" + list.Key + ": " + sp + " szavazat)";
			}

			sSendMessage.SendCMPrivmsg(_channel, "{0} szavazat kell a többséghez. Jelenlegi szavazatok:{1}", (_playerlist.Count/2)+1, namess);

			if((_playerlist.Count/2)+1 == _lynchmaxnumber)
			{
				_lynchmaxnumber = 0;
				sSender.Mode(_channel, "-v", Name);

				foreach(var list in _lynchlist)
				{
					if(_lynchmaxnumber == list.Value.Split(',').Length)
						namess = list.Key;
				}

				_rank = string.Empty;

				if(_killerlist.ContainsKey(Name.ToLower()))
				{
					newghost = _killerlist[Name.ToLower()];
					_killerlist.Remove(Name.ToLower());
					_rank = "killer";
				}
				else if(_detectivelist.ContainsKey(Name.ToLower()))
				{
					newghost = _detectivelist[Name.ToLower()];
					_detectivelist.Remove(Name.ToLower());
					_ghostdetective = true;
					_detective = true;
					_rank = "detective";
				}
				else if(_doctorlist.ContainsKey(Name.ToLower()))
				{
					newghost = _doctorlist[Name.ToLower()];
					_doctorlist.Remove(Name.ToLower());
					_ghostdoctor = true;
					_doctor = true;
					_rank = "doctor";
				}
				else if(_normallist.ContainsKey(Name.ToLower()))
				{
					newghost = _normallist[Name.ToLower()];
					_normallist.Remove(Name.ToLower());
					_rank = "normal";
				}

				int i = 0;
				foreach(var player in _playerlist)
				{
					if(player.Value == newghost)
					{
						i = player.Key;
						break;
					}
				}

				_playerlist.Remove(i);
				_ghostlist.Add(newghost.ToLower(), newghost);
				sSendMessage.SendCMPrivmsg(_channel, "A többség 4{0} lincselése mellett döntött! Elszabadulnak az indulatok. Ő mostantól már halott.", newghost);

				if(_rank == "killer")
					sSendMessage.SendCMPrivmsg(_channel, "*** A holttest megvizsgálása után kiderült, hogy 4gyilkos volt.");
				else if(_rank == "detective")
					sSendMessage.SendCMPrivmsg(_channel, "*** A holttest megvizsgálása után kiderült, hogy 4nyomozó volt.");
				else if(_rank == "doctor")
					sSendMessage.SendCMPrivmsg(_channel, "*** A holttest megvizsgálása után kiderült, hogy 4orvos volt.");
				else if(_rank == "normal")
					sSendMessage.SendCMPrivmsg(_channel, "*** A holttest megvizsgálása után kiderült, hogy egy ártatlan falusi volt.");

				EndGame();

				if(_playerlist.Count >= 2)
				{
					sSendMessage.SendCMPrivmsg(_channel, "({0} meghalt, és nem szólhat hozzá a játékhoz.)", newghost);
					_day = false;
					_stop = false;
				}
			}
		}
	}
}