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
using System.Timers;
using System.Threading.Tasks;
using System.Collections.Generic;
using Schumix.API;
using Schumix.Framework;
using Schumix.Framework.Localization;

namespace Schumix.Irc.Flood
{
	public sealed class AntiFlood
	{
		private readonly Dictionary<string, CommandFlood> CommandFloodList = new Dictionary<string, CommandFlood>();
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;
		private readonly ChannelInfo sChannelInfo = Singleton<ChannelInfo>.Instance;
		private System.Timers.Timer _timerflood = new System.Timers.Timer();
		private int flood;
		private AntiFlood() {}

		public void Start()
		{
			// Flood
			_timerflood.Interval = 1000;
			_timerflood.Elapsed += HandleTimerFloodElapsed;
			_timerflood.Enabled = true;
			_timerflood.Start();
		}

		public void Stop()
		{
			// Flood
			_timerflood.Enabled = false;
			_timerflood.Elapsed -= HandleTimerFloodElapsed;
			_timerflood.Stop();
		}
		
		private void HandleTimerFloodElapsed(object sender, ElapsedEventArgs e)
		{
			UpdateFlood();
		}

		private void UpdateFlood()
		{
			try
			{
				Task.Factory.StartNew(() => Flood());
			}
			catch(Exception e)
			{
				Log.Error("AntiFlood", sLConsole.Exception("Error"), e.Message);
			}
		}

		public void FloodCommand(IRCMessage sIRCMessage)
		{
			Task.Factory.StartNew(() =>
			{
				if(sChannelInfo.FSelect(IFunctions.Antiflood) && sChannelInfo.FSelect(IChannelFunctions.Antiflood, sIRCMessage.Channel.ToLower()))
				{
					string nick = sIRCMessage.Nick.ToLower();
					int i = 0;

					foreach(var list in CommandFloodList)
					{
						if(nick == list.Value.Name)
						{
							list.Value.Message++;
							i++;
						}
					}

					if(nick == "py-ctcp")
						return;

					if(i > 0)
						return;

					CommandFloodList.Add(nick, new CommandFlood(nick));
				}
			});
		}

		public bool Ignore(IRCMessage sIRCMessage)
		{
			if(CommandFloodList.ContainsKey(sIRCMessage.Nick.ToLower()))
			{
				if(CommandFloodList[sIRCMessage.Nick.ToLower()].IsIgnore)
				{
					if(CommandFloodList[sIRCMessage.Nick.ToLower()].Warring)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("CommandsDisabled", sIRCMessage.Channel), 3);
						CommandFloodList[sIRCMessage.Nick.ToLower()].Warring = false;
						return true;
					}
					else
						return true;
				}
			}

			return false;
		}

		private void Flood()
		{
			flood++;

			if(flood == /*Config.Seconds*/3)
			{
				flood = 0;

				foreach(var list in CommandFloodList)
				{
					if(list.Value.IsIgnore)
					{
						if(DateTime.Now >= list.Value.BanTime)
						{
							list.Value.IsIgnore = false;
							list.Value.Warring = false;
						}

						continue;
					}
					if(list.Value.Message >= /*Config.NumberOfMessages*/2)
					{
						list.Value.IsIgnore = true;
						list.Value.Warring = true;
						list.Value.BanTime = DateTime.Now.AddMinutes(1);
						list.Value.Message = 0;
						sSendMessage.SendCMPrivmsg(list.Value.Name, sLManager.GetWarningText("CommandsDisabled2"));
					}
					else
						list.Value.Message = 0;
				}
			}
		}
	}
}