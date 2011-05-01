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
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.CalendarAddon;
using Schumix.Framework;
using Schumix.Framework.Extensions;

namespace Schumix.CalendarAddon.Commands
{
	public partial class BannedCommand : CommandInfo
	{
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;
		private readonly Banned sBanned = Singleton<Banned>.Instance;
		private readonly Unbanned sUnbanned = Singleton<Unbanned>.Instance;
		private BannedCommand() {}

		public void HandleBanned()
		{
			if(!IsAdmin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Operator))
				return;

			CNick();

			if(Network.IMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs megadva az idő!");
				return;
			}

			if(Network.IMessage.Info.Length < 6)
			{
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs a kitiltandó név vagy vhost!");
				return;
			}

			if(Network.IMessage.Info[5].Contains(":"))
			{
				if(Network.IMessage.Info.Length < 7)
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs az idő!");
					return;
				}

				int ora = Convert.ToInt32(Network.IMessage.Info[5].Substring(0, Network.IMessage.Info[5].IndexOf(":")));
				if(ora > 24)
					return;

				int perc = Convert.ToInt32(Network.IMessage.Info[5].Substring(Network.IMessage.Info[5].IndexOf(":")+1));
				if(perc > 60)
					return;

				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, sBanned.BannedName(Network.IMessage.Info[4].ToLower(), Network.IMessage.Channel, Network.IMessage.Info.SplitToString(6, " "), ora, perc));
			}
			else
			{
				if(Network.IMessage.Info.Length < 7)
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs a kitiltandó név vagy vhost!");
					return;
				}

				if(Network.IMessage.Info.Length < 8)
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs az idő!");
					return;
				}

				string[] s = Network.IMessage.Info[5].Split('.');
				if(s.Length < 3)
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Helytelen dátum formátum!");
					return;
				}

				int ev = Convert.ToInt32(s[0]);
				int honap = Convert.ToInt32(s[1]);
				if(honap > 12)
					return;

				int nap = Convert.ToInt32(s[2]);
				if(nap > 31)
					return;

				int ora = Convert.ToInt32(Network.IMessage.Info[6].Substring(0, Network.IMessage.Info[6].IndexOf(":")));
				if(ora > 24)
					return;

				int perc = Convert.ToInt32(Network.IMessage.Info[6].Substring(Network.IMessage.Info[6].IndexOf(":")+1));
				if(perc > 60)
					return;

				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, sBanned.BannedName(Network.IMessage.Info[4].ToLower(), Network.IMessage.Channel, Network.IMessage.Info.SplitToString(7, " "), ev, honap, nap, ora, perc));
			}
		}

		public void HandleUnbanned()
		{
			if(!IsAdmin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Operator))
				return;

			CNick();

			if(Network.IMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs a kitiltandó neve vagy vhost!");
				return;
			}

			sUnbanned.UnbannedName(Network.IMessage.Info[4], Network.IMessage.Channel);
		}
	}
}