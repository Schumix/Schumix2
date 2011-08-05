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

namespace Schumix.GameAddon.IJAGames
{
	public sealed partial class IJAGame
	{
		public void Stats()
		{
			if(!Running)
			{
				sSendMessage.SendCMPrivmsg(_channel, "Nem megy játék!");
				return;
			}

			if(!Started)
			{
				string names = string.Empty;
				foreach(var name in _playerlist)
					names += ", " + name.Value;

				sSendMessage.SendCMPrivmsg(_channel, "Új játék indul. Játékosok: {0}", names.Remove(0, 2, ", "));
				return;
			}
			else
			{
				sSendMessage.SendCMPrivmsg(_channel, "A játék állása a következő:");
				string names = string.Empty;
				foreach(var name in _playerlist)
					names += ", " + name.Value;

				sSendMessage.SendCMPrivmsg(_channel, "A következő személyek vannak még életben: {0}", names.Remove(0, 2, ", "));

				names = string.Empty;
				foreach(var name in _ghostlist)
					names += ", " + name.Value;

				sSendMessage.SendCMPrivmsg(_channel, "A következő személyek halottak: {0}", names.Remove(0, 2, ", "));
			}
		}
	}
}