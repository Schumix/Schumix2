/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013-2015 Schumix Team <http://schumix.eu/>
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
		private readonly object Lock = new object();

		public void Lynch(string Name, string NickName, string Channel)
		{
			lock(Lock)
			{
				var sSendMessage = sIrcBase.Networks[_servername].sSendMessage;

				if(_lynch)
					return;

				if(!IsRunning(Channel, NickName))
					return;

				if(!IsStarted(Channel, NickName))
					return;

				var text = sLManager.GetCommandTexts("maffiagame/basecommand/lynch", _channel, _servername);
				if(text.Length < 14)
				{
					sSendMessage.SendCMPrivmsg(_channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(_channel, _servername)));
					return;
				}

				if(NoLynch)
				{
					sSendMessage.SendCMPrivmsg(Channel, text[1], DisableHl(NickName));
					return;
				}

				if(_ghostlist.ContainsKey(NickName.ToLower()))
				{
					sSendMessage.SendCMPrivmsg(Channel, text[2], DisableHl(NickName));
					return;
				}

				if(!_day)
				{
					sSendMessage.SendCMPrivmsg(Channel, text[3], DisableHl(NickName));
					return;
				}

				if(!_killerlist.ContainsKey(Name.ToLower()) && !_detectivelist.ContainsKey(Name.ToLower()) &&
					!_normallist.ContainsKey(Name.ToLower()) && !_ghostlist.ContainsKey(Name.ToLower()) &&
					!_doctorlist.ContainsKey(Name.ToLower()))
				{
					sSendMessage.SendCMPrivmsg(Channel, text[4], DisableHl(NickName));
					return;
				}

				if(_ghostlist.ContainsKey(Name.ToLower()))
				{
					sSendMessage.SendCMPrivmsg(Channel, text[5], DisableHl(NickName));
					return;
				}

				if(Name.ToLower() == NickName.ToLower())
				{
					sSendMessage.SendCMPrivmsg(Channel, text[6], DisableHl(NickName));
					return;
				}

				foreach(var function in _playerflist)
				{
					if(function.Key == Name.ToLower())
					{
						if(function.Value.Lynch.Contains(NickName.ToLower()))
						{
							sSendMessage.SendCMPrivmsg(_channel, text[7], DisableHl(NickName));
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

				sSendMessage.SendCMPrivmsg(_channel, text[8], DisableHl(NickName), DisableHl(Name));

				string namess = string.Empty;
				foreach(var function in _playerflist)
				{
					var sp = function.Value.Lynch.Count;
					if(sp > _lynchmaxnumber && sp <= (_playerlist.Count/2)+1)
						_lynchmaxnumber = sp;

					if(sp > 0)
						namess += " (" + DisableHl(function.Key) + ": " + sp + SchumixBase.Space + text[9] + ")";
				}

				if(_lynch)
					return;

				sSendMessage.SendCMPrivmsg(_channel, text[10], (_playerlist.Count/2)+1, namess);
				LynchPlayer(namess);
			}
		}
	}
}