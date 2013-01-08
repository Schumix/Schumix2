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
using Schumix.Framework;
using Schumix.Framework.Extensions;

namespace Schumix.GameAddon.MaffiaGames
{
	sealed partial class MaffiaGame
	{
		public void Leave(string Name)
		{
			Leave(Name, string.Empty);
		}

		public void Leave(string Name, string NickName)
		{
			if(!IsRunning(_channel, NickName))
				return;

			var sSendMessage = sIrcBase.Networks[_servername].sSendMessage;
			var sSender = sIrcBase.Networks[_servername].sSender;

			var text = sLManager.GetCommandTexts("maffiagame/basecommand/leave", _channel, _servername);
			if(text.Length < 11)
			{
				sSendMessage.SendCMPrivmsg(_channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(_channel, _servername)));
				return;
			}

			if(NickName == string.Empty)
			{
				if(!_playerlist.ContainsValue(Name))
				{
					sSendMessage.SendCMPrivmsg(_channel, text[2], DisableHl(Name));
					return;
				}
			}
			else
			{
				if(!_playerlist.ContainsValue(NickName))
				{
					sSendMessage.SendCMPrivmsg(_channel, text[3], DisableHl(NickName));
					return;
				}
			}

			if(_start)
			{
				_leftlist.Add(Name);
				return;
			}

			if(_playerflist.ContainsKey(Name.ToLower()))
			{
				foreach(var function in _playerflist)
				{
					if(function.Value.Lynch.Contains(Name.ToLower()))
						function.Value.Lynch.Remove(Name.ToLower());
				}

				_playerflist[Name.ToLower()].Lynch.Clear();
				//_playerflist.Remove(Name.ToLower());		
			}

			sSender.Mode(_channel, "-v", Name);
			RemovePlayer(Name, string.Empty);
			sSendMessage.SendCMPrivmsg(_channel, text[4], DisableHl(Name));
			var rank = GetRank(Name);

			if(rank == Rank.Killer)
				sSendMessage.SendCMPrivmsg(_channel, text[5], DisableHl(Name));
			else if(rank == Rank.Detective)
				sSendMessage.SendCMPrivmsg(_channel, text[6], DisableHl(Name));
			else if(rank == Rank.Doctor)
				sSendMessage.SendCMPrivmsg(_channel, text[7], DisableHl(Name));
			else if(rank == Rank.Normal)
				sSendMessage.SendCMPrivmsg(_channel, text[8], DisableHl(Name));
			else
				sSendMessage.SendCMPrivmsg(_channel, text[9], DisableHl(Name));

			if(_owner == Name)
			{
				_owner = string.Empty;

				if(_timerowner.Enabled)
				{
					_timerowner.Enabled = false;
					_timerowner.Elapsed -= HandleIsOwnerAfk;
					_timerowner.Stop();
				}

				if(_playerlist.Count > 0)
					sSendMessage.SendCMPrivmsg(_channel, text[10]);
			}

			if(Started)
			{
				_lynchmaxnumber = 0;

				foreach(var function in _playerflist)
				{
					var sp = function.Value.Lynch.Count;
					if(sp > _lynchmaxnumber && sp <= (_playerlist.Count/2)+1)
						_lynchmaxnumber = sp;
				}

				EndGame();
			}
			else
			{
				if(_playerlist.Count == 0)
				{
					EndGameText();
					StopThread();
				}
			}
		}
	}
}