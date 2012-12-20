/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Megax <http://www.megaxx.info/>
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
					sSendMessage.SendCMPrivmsg(Channel, text[1], NickName);
					return;
				}

				if(!_killerlist.ContainsKey(NickName.ToLower()) && !_detectivelist.ContainsKey(NickName.ToLower()) &&
					!_normallist.ContainsKey(NickName.ToLower()) && !_doctorlist.ContainsKey(NickName.ToLower()))
				{
					sSendMessage.SendCMPrivmsg(Channel, text[2], NickName);
					return;
				}

				if(!_day)
				{
					sSendMessage.SendCMPrivmsg(Channel, text[3], NickName);
					return;
				}

				if(!_killerlist.ContainsKey(Name.ToLower()) && !_detectivelist.ContainsKey(Name.ToLower()) &&
					!_normallist.ContainsKey(Name.ToLower()) && !_ghostlist.ContainsKey(Name.ToLower()) &&
					!_doctorlist.ContainsKey(Name.ToLower()))
				{
					sSendMessage.SendCMPrivmsg(Channel, text[4], NickName);
					return;
				}

				if(_ghostlist.ContainsKey(Name.ToLower()))
				{
					sSendMessage.SendCMPrivmsg(Channel, text[5], NickName);
					return;
				}

				if(Name.ToLower() == NickName.ToLower())
				{
					sSendMessage.SendCMPrivmsg(Channel, text[6], NickName);
					return;
				}

				foreach(var function in _playerflist)
				{
					if(function.Key == Name.ToLower())
					{
						if(function.Value.Lynch.Contains(NickName.ToLower()))
						{
							sSendMessage.SendCMPrivmsg(_channel, text[7], NickName);
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

				sSendMessage.SendCMPrivmsg(_channel, text[8], NickName, Name);

				string namess = string.Empty;
				foreach(var function in _playerflist)
				{
					var sp = function.Value.Lynch.Count;
					if(sp > _lynchmaxnumber && sp <= (_playerlist.Count/2)+1)
						_lynchmaxnumber = sp;

					if(sp > 0)
						namess += " (" + function.Key + ": " + sp + SchumixBase.Space + text[9] + ")";
				}

				if(_lynch)
					return;

				sSendMessage.SendCMPrivmsg(_channel, text[10], (_playerlist.Count/2)+1, namess);

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
					RemovePlayer(namess, namess);
					sSendMessage.SendCMPrivmsg(_channel, text[11], namess);

					if(GetPlayerMaster(namess))
						sSendMessage.SendCMPrivmsg(_channel, text[12]);

					Corpse(namess);
					Thread.Sleep(400);
					EndGame();

					if(_playerlist.Count >= 2 && Running)
					{
						sSendMessage.SendCMPrivmsg(_channel, text[13], namess);
						_day = false;
						_stop = false;
					}

					_lynch = false;
				}
			}
		}
	}
}