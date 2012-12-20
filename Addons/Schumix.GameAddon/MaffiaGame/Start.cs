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
using System.Collections.Generic;
using Schumix.Framework;
using Schumix.Framework.Config;

namespace Schumix.GameAddon.MaffiaGames
{
	sealed partial class MaffiaGame
	{
		public void Start()
		{
			if(!IsRunning(_channel))
				return;

			var sSendMessage = sIrcBase.Networks[_servername].sSendMessage;
			var sSender = sIrcBase.Networks[_servername].sSender;

			if(Started || _start)
			{
				sSendMessage.SendCMPrivmsg(_channel, "A játék már megy!");
				return;
			}

			var text = sLManager.GetCommandTexts("maffiagame/base/start", _channel, _servername);
			if(text.Length < 7)
			{
				sSendMessage.SendCMPrivmsg(_channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(_channel, _servername)));
				return;
			}

			if(_playerlist.Count < 4)
			{
				sSendMessage.SendCMPrivmsg(_channel, text[1]);
				return;
			}

			_joinstop = true;
			_start = true;

			var list = new Dictionary<int, string>();
			foreach(var l in _playerlist)
				list.Add(l.Key, l.Value);

			_detectivelist.Clear();
			_killerlist.Clear();
			_doctorlist.Clear();
			_normallist.Clear();
			_joinlist.Clear();

			var rand = new Random();
			int number = 0;
			int i = 0, x = 0;
			bool killer = true;
			bool doctor = true;
			bool detective = true;
			int count = list.Count;

			for(;;)
			{
				number = rand.Next(1, list.Count+1);

				if(killer)
				{
					if(list.ContainsKey(number))
					{
						string name = string.Empty;
						list.TryGetValue(number, out name);
						_killerlist.Add(name.ToLower(), name);

						if(Adminflag(name.ToLower()) == 2)
							_playerflist.Add(name.ToLower(), new Player(Rank.Killer, true));
						else
							_playerflist.Add(name.ToLower(), new Player(Rank.Killer));

						SchumixBase.DManager.Insert("`maffiagame`(ServerId, ServerName, Game, Name, Job)", IRCConfig.List[_servername].ServerId, _servername, _gameid, name, Convert.ToInt32(Rank.Killer));
						list.Remove(number);

						if(count < 8)
							killer = false;
						else if(count >= 8 && count < 15)
						{
							i++;
							if(i == 2)
								killer = false;
						}
						else
						{
							i++;
							if(i == 3)
								killer = false;
						}
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

						if(Adminflag(name.ToLower()) == 2)
							_playerflist.Add(name.ToLower(), new Player(Rank.Detective, true));
						else
							_playerflist.Add(name.ToLower(), new Player(Rank.Detective));

						SchumixBase.DManager.Insert("`maffiagame`(ServerId, ServerName, Game, Name, Job)", IRCConfig.List[_servername].ServerId, _servername, _gameid, name, Convert.ToInt32(Rank.Detective));
						list.Remove(number);

						if(count < 15)
							detective = false;
						else
						{
							x++;
							if(x == 2)
								detective = false;
						}
					}

					continue;
				}
				else if(doctor && count >= 8)
				{
					if(list.ContainsKey(number))
					{
						string name = string.Empty;
						list.TryGetValue(number, out name);
						_doctorlist.Add(name.ToLower(), name);

						if(Adminflag(name.ToLower()) == 2)
							_playerflist.Add(name.ToLower(), new Player(Rank.Doctor, true));
						else
							_playerflist.Add(name.ToLower(), new Player(Rank.Doctor));

						SchumixBase.DManager.Insert("`maffiagame`(ServerId, ServerName, Game, Name, Job)", IRCConfig.List[_servername].ServerId, _servername, _gameid, name, Convert.ToInt32(Rank.Doctor));
						list.Remove(number);
						doctor = false;
					}

					continue;
				}
				else
				{
					foreach(var llist in list)
					{
						_normallist.Add(llist.Value.ToLower(), llist.Value);

						if(Adminflag(llist.Value.ToLower()) == 2)
							_playerflist.Add(llist.Value.ToLower(), new Player(Rank.Normal, true));
						else
							_playerflist.Add(llist.Value.ToLower(), new Player(Rank.Normal));

						SchumixBase.DManager.Insert("`maffiagame`(ServerId, ServerName, Game, Name, Job)", IRCConfig.List[_servername].ServerId, _servername, _gameid, llist.Value, Convert.ToInt32(Rank.Normal));
					}

					break;
				}
			}

			foreach(var name in _killerlist)
				sSendMessage.SendCMPrivmsg(name.Key, text[2]);

			foreach(var name in _detectivelist)
				sSendMessage.SendCMPrivmsg(name.Key, text[3]);

			if(count >= 8)
			{
				foreach(var name in _doctorlist)
					sSendMessage.SendCMPrivmsg(name.Key, text[4]);
			}

			foreach(var name in _normallist)
				sSendMessage.SendCMPrivmsg(name.Key, text[5]);

			list.Clear();
			Started = true;
			_start = false;
			_players = _playerlist.Count;
			sSendMessage.SendCMPrivmsg(_channel, text[6]);
			_joinstop = false;
			sSender.Mode(_channel, "+m");
			Thread.Sleep(1000);

			if(_leftlist.Count > 0)
			{
				foreach(var name in _leftlist)
					Leave(name);

				_leftlist.Clear();
				EndGame();
			}

			if(_players >= 15)
			{
				_killerchannel = "#" + sUtilities.GetRandomString();
				sSender.Join(_killerchannel);
				sSender.Mode(_killerchannel, "+s");
				SchumixBase.DManager.Insert("`channels`(ServerId, ServerName, Channel, Password, Language)", IRCConfig.List[_servername].ServerId, _servername, sUtilities.SqlEscape(_killerchannel), string.Empty, sLManager.Locale);
				SchumixBase.DManager.Update("channels", "Enabled = 'true'", string.Format("Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(_killerchannel), _servername));
			}

			StartThread();
		}
	}
}