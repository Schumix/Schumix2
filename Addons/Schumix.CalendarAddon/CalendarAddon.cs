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
using System.Threading.Tasks;
using System.Collections.Generic;
using Schumix.API;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Localization;
using Schumix.CalendarAddon.Config;
using Schumix.CalendarAddon.Commands;
using Schumix.CalendarAddon.Localization;

namespace Schumix.CalendarAddon
{
	class CalendarAddon : ISchumixAddon
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly CalendarCommand sCalendarCommand = Singleton<CalendarCommand>.Instance;
		private readonly PLocalization sLocalization = Singleton<PLocalization>.Instance;
		private readonly ChannelInfo sChannelInfo = Singleton<ChannelInfo>.Instance;
		private readonly BanCommand sBanCommand = Singleton<BanCommand>.Instance;
		private Calendar _calendar;
		public static readonly List<Flood> FloodList = new List<Flood>();
#pragma warning disable 414
		private AddonConfig _config;
#pragma warning restore 414

		public void Setup()
		{
			sLocalization.Locale = sLConsole.Locale;
			_config = new AddonConfig(Name + ".xml");
			_calendar = new Calendar();
			_calendar.Start();

			Network.IrcRegisterHandler("PRIVMSG", HandlePrivmsg);
			InitIrcCommand();
		}

		public void Destroy()
		{
			_calendar.Stop();
			Network.IrcRemoveHandler("PRIVMSG",   HandlePrivmsg);
			RemoveIrcCommand();
		}

		public int Reload(string RName, string SName = "")
		{
			try
			{
				switch(RName.ToLower())
				{
					case "config":
						_config = new AddonConfig(Name + ".xml");
						return 1;
					case "command":
						InitIrcCommand();
						RemoveIrcCommand();
						return 1;
				}
			}
			catch(Exception e)
			{
				Log.Error("CalendarAddon", "Reload: " + sLConsole.Exception("Error"), e.Message);
				return 0;
			}

			return -1;
		}

		private void InitIrcCommand()
		{
			CommandManager.SchumixRegisterHandler("ban",      sBanCommand.HandleBan, CommandPermission.Operator);
			CommandManager.SchumixRegisterHandler("unban",    sBanCommand.HandleUnban, CommandPermission.Operator);
			CommandManager.SchumixRegisterHandler("calendar", sCalendarCommand.HandleCalendar);
		}

		private void RemoveIrcCommand()
		{
			CommandManager.SchumixRemoveHandler("ban",        sBanCommand.HandleBan);
			CommandManager.SchumixRemoveHandler("unban",      sBanCommand.HandleUnban);
			CommandManager.SchumixRemoveHandler("calendar",   sCalendarCommand.HandleCalendar);
		}

		private void HandlePrivmsg(IRCMessage sIRCMessage)
		{
			Task.Factory.StartNew(() =>
			{
				string channel = sIRCMessage.Channel.ToLower();

				if(sChannelInfo.FSelect(IFunctions.Antiflood) && sChannelInfo.FSelect(IChannelFunctions.Antiflood, channel))
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
			get { return Consts.SchumixProgrammedBy; }
		}

		/// <summary>
		/// Website where the addon is available.
		/// </summary>
		public string Website
		{
			get { return Consts.SchumixWebsite; }
		}
	}
}