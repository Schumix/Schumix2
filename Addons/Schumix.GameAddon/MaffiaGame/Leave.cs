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
		public void Leave(string Name)
		{
			Leave(Name, string.Empty);
		}

		public void Leave(string Name, string NickName)
		{
			if(!Running)
			{
				sSendMessage.SendCMPrivmsg(_channel, "{0}: Nem megy játék!", Name);
				return;
			}

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

			RemovePlayer(Name);
			sSendMessage.SendCMPrivmsg(_channel, "{0} eltűnt egy különös féreglyukban.", Name);

			if(_rank == "killer")
				sSendMessage.SendCMPrivmsg(_channel, "{0}-nak izgalmas szerepe volt a játékban, mint gyilkos. Remélhetőleg halála izgalmasabb lesz.", Name);
			else if(_rank == "detective")
				sSendMessage.SendCMPrivmsg(_channel, "{0}-nak izgalmas szerepe volt a játékban, mint nyomozó. Remélhetőleg halála izgalmasabb lesz.", Name);
			else if(_rank == "doctor")
				sSendMessage.SendCMPrivmsg(_channel, "{0}-nak izgalmas szerepe volt a játékban, mint orvos. Remélhetőleg halála izgalmasabb lesz.", Name);
			else if(_rank == "normal")
				sSendMessage.SendCMPrivmsg(_channel, "{0}-nak unalmas szerepe volt a játékban, mint civil. Remélhetőleg halála izgalmasabb lesz.", Name);
			else
				sSendMessage.SendCMPrivmsg(_channel, "{0}-nak nem volt szerepe még a játékban. Remélhetőleg halála izgalmasabb lesz.", Name);

			if(_owner == Name)
			{
				_owner = string.Empty;
				sSendMessage.SendCMPrivmsg(_channel, "A játékot mostantól bárki írányíthatja!");
			}

			if(Started)
			{
				string name = string.Empty;
				string names = string.Empty;
				string[] split = { string.Empty };

				foreach(var list in _lynchlist)
				{
					if(list.Value.Contains(Name.ToLower()))
					{
						name = list.Key;
						split = list.Value.Split(',');
					}
				}

				_lynchlist.Remove(name);
				_lynchlist.Remove(Name.ToLower());

				if(split.Length > 1)
				{
					foreach(var spl in split)
					{
						if(Name.ToLower() == spl)
							continue;
						else
							names += "," + spl;
					}

					_lynchlist.Add(name, names.Remove(0, 1, ","));
				}

				_lynchmaxnumber = 0;

				foreach(var list in _lynchlist)
				{
					var sp = list.Value.Split(',').Length;
					if(sp > _lynchmaxnumber)
						_lynchmaxnumber = sp;
				}

				EndGame();
			}
			else
			{
				if(_playerlist.Count == 0)
				{
					sSendMessage.SendCMPrivmsg(_channel, "A játék befejeződött.");
					StopThread();
				}
			}
		}
	}
}