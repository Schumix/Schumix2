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
using System.Timers;
using System.Threading.Tasks;
using System.Collections.Generic;
using Schumix.API;
using Schumix.API.Irc;
using Schumix.API.Functions;
using Schumix.Irc.Channel;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Localization;

namespace Schumix.Irc.Flood
{
	public sealed class AntiFlood
	{
		private readonly Dictionary<string, CommandFlood> CommandFloodList = new Dictionary<string, CommandFlood>();
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private System.Timers.Timer _timerflood = new System.Timers.Timer();
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private readonly SendMessage sSendMessage;
		private readonly ChannelInfo sChannelInfo;

		public AntiFlood(string ServerName)
		{
			sChannelInfo = sIrcBase.Networks[ServerName].sChannelInfo;
			sSendMessage = sIrcBase.Networks[ServerName].sSendMessage;
		}

		public void Start()
		{
			// Flood
			_timerflood.Interval = FloodingConfig.Seconds*1000;
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
			if(sChannelInfo.FSelect(IFunctions.Antiflood) && sChannelInfo.FSelect(IChannelFunctions.Antiflood, sIRCMessage.Channel.ToLower()))
			{
				string nick = sIRCMessage.Nick.ToLower();

				if(nick == "py-ctcp")
					return;

				if(CommandFloodList.ContainsKey(nick) && !CommandFloodList[nick].IsIgnore)
					CommandFloodList[nick].Message++;
				else if(!CommandFloodList.ContainsKey(nick))
					CommandFloodList.Add(nick, new CommandFlood());
			}
		}

		public bool Ignore(IRCMessage sIRCMessage)
		{
			if(CommandFloodList.ContainsKey(sIRCMessage.Nick.ToLower()))
			{
				if(!CommandFloodList[sIRCMessage.Nick.ToLower()].IsIgnore &&
				   CommandFloodList[sIRCMessage.Nick.ToLower()].Message >= FloodingConfig.NumberOfCommands)
				{
					CommandFloodList[sIRCMessage.Nick.ToLower()].IsIgnore = true;
					CommandFloodList[sIRCMessage.Nick.ToLower()].Warring = true;
					CommandFloodList[sIRCMessage.Nick.ToLower()].BanTime = DateTime.Now.AddMinutes(1);
					CommandFloodList[sIRCMessage.Nick.ToLower()].Message = 0;
					sSendMessage.SendCMPrivmsg(sIRCMessage.Nick.ToLower(), sLManager.GetWarningText("CommandsDisabled2", sIRCMessage.Channel, sIRCMessage.ServerName), FloodingConfig.Seconds);
					return true;
				}

				if(CommandFloodList[sIRCMessage.Nick.ToLower()].IsIgnore)
				{
					if(CommandFloodList[sIRCMessage.Nick.ToLower()].Warring)
					{
						CommandFloodList[sIRCMessage.Nick.ToLower()].Warring = false;
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("CommandsDisabled", sIRCMessage.Channel, sIRCMessage.ServerName));
					}

					return true;
				}
			}

			return false;
		}

		private void Flood()
		{
			foreach(var list in CommandFloodList)
			{
				if(list.Value.IsIgnore)
				{
					if(DateTime.Now >= list.Value.BanTime)
					{
						list.Value.IsIgnore = false;
						list.Value.Warring = false;
						sSendMessage.SendCMPrivmsg(list.Key, sLManager.GetWarningText("CommandsEnabled"));
					}

					continue;
				}

				list.Value.Message = 0;
			}
		}

		public void Remove(string Name)
		{
			if(CommandFloodList.ContainsKey(Name.ToLower()))
				CommandFloodList.Remove(Name.ToLower());
		}
	}
}