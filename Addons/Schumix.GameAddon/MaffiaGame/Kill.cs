/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013-2014 Schumix Team <http://schumix.eu/>
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
	sealed partial class MaffiaGame
	{
		public void Kill(string Name, string Killer)
		{
			var sSendMessage = sIrcBase.Networks[_servername].sSendMessage;

			if(!IsRunning(_channel, Killer))
				return;

			if(!IsStarted(_channel, Killer))
				return;

			var text = sLManager.GetCommandTexts("maffiagame/basecommand/kill", _channel, _servername);
			if(text.Length < 7)
			{
				sSendMessage.SendCMPrivmsg(_channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(_channel, _servername)));
				return;
			}

			if(_ghostlist.ContainsKey(Killer.ToLower()))
			{
				sSendMessage.SendCMPrivmsg(Killer, sLManager.GetCommandText("maffiagame/base/ghost", _channel, _servername));
				return;
			}

			if(!_killerlist.ContainsKey(Killer.ToLower()))
			{
				sSendMessage.SendCMPrivmsg(Killer, text[2]);
				return;
			}

			if(_day)
			{
				sSendMessage.SendCMPrivmsg(Killer, text[1]);
				return;
			}

			if(_ghostlist.ContainsKey(Name.ToLower()))
			{
				sSendMessage.SendCMPrivmsg(Killer, text[3]);
				return;
			}

			if(!_killerlist.ContainsKey(Name.ToLower()) && !_detectivelist.ContainsKey(Name.ToLower()) &&
				!_doctorlist.ContainsKey(Name.ToLower()) && !_normallist.ContainsKey(Name.ToLower()))
			{
				sSendMessage.SendCMPrivmsg(Killer, text[4]);
				return;
			}

			if(Name.ToLower() == Killer.ToLower())
				sSendMessage.SendCMPrivmsg(Killer, text[5]);

			sSendMessage.SendCMPrivmsg(Killer, text[6]);

			foreach(var function in _playerflist)
			{
				if(function.Key == Killer.ToLower())
					function.Value.RName = Name.ToLower();
			}
		}
	}
}