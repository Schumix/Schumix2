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
using Schumix.Framework.Extensions;

namespace Schumix.GameAddon.MaffiaGames
{
	sealed partial class MaffiaGame
	{
		public void Stats()
		{
			if(!IsRunning(_channel))
				return;

			var sSendMessage = sIrcBase.Networks[_servername].sSendMessage;

			var text = sLManager.GetCommandTexts("maffiagame/basecommand/stats", _channel, _servername);
			if(text.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(_channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(_channel, _servername)));
				return;
			}

			if(!Started)
			{
				string names = string.Empty;
				foreach(var name in _playerlist)
					names += ", " + DisableHl(name.Value);

				sSendMessage.SendCMPrivmsg(_channel, text[0], _playerlist.Count);
				sSendMessage.SendCMPrivmsg(_channel, text[1], names.Remove(0, 2, ", "));
				return;
			}
			else
			{
				sSendMessage.SendCMPrivmsg(_channel, text[2]);
				string names = string.Empty;
				foreach(var name in _playerlist)
					names += ", " + name.Value;

				sSendMessage.SendCMPrivmsg(_channel, text[3], names.Remove(0, 2, ", "));

				names = string.Empty;
				foreach(var name in _ghostlist)
					names += ", " + DisableHl(name.Value);

				sSendMessage.SendCMPrivmsg(_channel, text[4], names.Remove(0, 2, ", "));
			}
		}
	}
}