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
using System.Threading.Tasks;
using System.Collections.Generic;
using Schumix.API;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.CalendarAddon.Commands;
using Schumix.CalendarAddon.Config;

namespace Schumix.CalendarAddon
{
	public class CalendarAddon : ISchumixAddon
	{
		private readonly ChannelInfo sChannelInfo = Singleton<ChannelInfo>.Instance;
		private readonly BanCommand sBanCommand = Singleton<BanCommand>.Instance;
		private Calendar _calendar;
		public static readonly List<Flood> FloodList = new List<Flood>();

		public void Setup()
		{
			new AddonConfig(Name + ".xml");
			_calendar = new Calendar();
			_calendar.Start();

			CommandManager.OperatorCRegisterHandler("ban",   new Action<IRCMessage>(sBanCommand.HandleBan));
			CommandManager.OperatorCRegisterHandler("unban", new Action<IRCMessage>(sBanCommand.HandleUnban));
		}

		public void Destroy()
		{
			_calendar.Stop();
			CommandManager.OperatorCRemoveHandler("ban");
			CommandManager.OperatorCRemoveHandler("unban");
		}

		public void HandlePrivmsg(IRCMessage sIRCMessage)
		{
			Task.Factory.StartNew(() =>
			{
				string channel = sIRCMessage.Channel.ToLower();

				if(sChannelInfo.FSelect("antiflood") && sChannelInfo.FSelect("antiflood", channel))
				{
					string nick = sIRCMessage.Nick.ToLower();
					int i = 0;

					foreach(var list in FloodList)
					{
						if(nick == list.Name && channel == list.Channel)
						{
							list.Message++;
							i++;
						}
					}

					if(nick == "py-ctcp")
						return;

					if(i > 0)
						return;

					FloodList.Add(new Flood(nick, channel));
				}
			});
		}

		public void HandleNotice(IRCMessage sIRCMessage)
		{

		}

		public bool HandleHelp(IRCMessage sIRCMessage)
		{
			return false;
		}

		/// <summary>
		/// Name of the addon
		/// </summary>
		public string Name
		{
			get { return "CalendarAddon"; }
		}

		/// <summary>
		/// Author of the addon.
		/// </summary>
		public string Author
		{
			get { return "Megax"; }
		}

		/// <summary>
		/// Website where the addon is available.
		/// </summary>
		public string Website
		{
			get { return "http://www.github.com/megax/Schumix2"; }
		}
	}
}