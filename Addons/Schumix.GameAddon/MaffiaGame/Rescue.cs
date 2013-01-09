/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013 Schumix Team <http://schumix.eu/>
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
		public void Rescue(string Name, string NickName)
		{
			if(!IsRunning(_channel, NickName))
				return;

			if(!IsStarted(_channel, NickName))
				return;

			var sSendMessage = sIrcBase.Networks[_servername].sSendMessage;

			var text = sLManager.GetCommandTexts("maffiagame/basecommand/rescue", _channel, _servername);
			if(text.Length < 7)
			{
				sSendMessage.SendCMPrivmsg(_channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(_channel, _servername)));
				return;
			}

			if(!_doctorlist.ContainsKey(NickName.ToLower()))
			{
				sSendMessage.SendCMPrivmsg(NickName, text[2]);
				return;
			}

			if(_day)
			{
				sSendMessage.SendCMPrivmsg(NickName, text[1]);
				return;
			}

			if(_ghostlist.ContainsKey(Name.ToLower()))
			{
				sSendMessage.SendCMPrivmsg(NickName, text[3]);
				return;
			}

			if(!_killerlist.ContainsKey(Name.ToLower()) && !_detectivelist.ContainsKey(Name.ToLower()) &&
				!_doctorlist.ContainsKey(Name.ToLower()) && !_normallist.ContainsKey(Name.ToLower()))
			{
				sSendMessage.SendCMPrivmsg(NickName, text[4]);
				return;
			}

			if(Name.ToLower() == NickName.ToLower())
				sSendMessage.SendCMPrivmsg(NickName, text[5]);

			sSendMessage.SendCMPrivmsg(NickName, text[6]);

			string rescued = string.Empty;

			if(_killerlist.ContainsKey(Name.ToLower()))
				rescued = "áá(%[[]][[]]killer[[]][[]]%)áá";
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