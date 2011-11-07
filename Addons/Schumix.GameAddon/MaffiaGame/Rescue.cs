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
		public void Rescue(string Name, string NickName)
		{
			if(!Running)
			{
				sSendMessage.SendCMPrivmsg(_channel, "{0}: Nem megy játék!", NickName);
				return;
			}

			if(!Started)
			{
				sSendMessage.SendCMPrivmsg(_channel, "{0}: Még nem kezdődött el játék!", NickName);
				return;
			}

			if(_day)
			{
				sSendMessage.SendCMPrivmsg(NickName, "Csak este menthetsz életet!");
				return;
			}

			if(!_doctorlist.ContainsKey(NickName.ToLower()))
			{
				sSendMessage.SendCMPrivmsg(NickName, "Nem vagy orvos!");
				return;
			}

			if(_ghostlist.ContainsKey(Name.ToLower()))
			{
				sSendMessage.SendCMPrivmsg(NickName, "Ő már halott. Válasz mást!");
				return;
			}

			if(!_killerlist.ContainsKey(Name.ToLower()) && !_detectivelist.ContainsKey(Name.ToLower()) &&
				!_doctorlist.ContainsKey(Name.ToLower()) && !_normallist.ContainsKey(Name.ToLower()))
			{
				sSendMessage.SendCMPrivmsg(NickName, "Kit akarsz megmenteni?");
				return;
			}

			if(Name.ToLower() == NickName.ToLower())
				sSendMessage.SendCMPrivmsg(NickName, "Önmagadat akarod megmenteni? Ennyire nem lehetsz félős!");

			sSendMessage.SendCMPrivmsg(NickName, "Elkönyveltem a kérésedet.");

			string rescued = string.Empty;

			if(_killerlist.ContainsKey(Name.ToLower()))
				rescued = string.Empty;
			else if(_detectivelist.ContainsKey(Name.ToLower()))
				rescued = Name.ToLower();
			else if(_doctorlist.ContainsKey(Name.ToLower()))
				rescued = Name.ToLower();
			else if(_normallist.ContainsKey(Name.ToLower()))
				rescued = Name.ToLower();

			_playerflist[NickName.ToLower()].RName = rescued;
		}
	}
}