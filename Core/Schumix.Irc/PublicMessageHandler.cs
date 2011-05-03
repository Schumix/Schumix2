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
using System.Diagnostics;
using Schumix.API;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;

namespace Schumix.Irc
{
	public partial class MessageHandler
	{
		private int PLength = IRCConfig.CommandPrefix.Length;

		protected void HandlePrivmsg(IRCMessage sIRCMessage)
		{
			foreach(var plugin in sAddonManager.GetPlugins())
				plugin.HandlePrivmsg(sIRCMessage);

			if(ConsoleLog.CLog)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("[{0}] <{1}> {2}", sIRCMessage.Channel, sIRCMessage.Nick, sIRCMessage.Args);
				Console.ForegroundColor = ConsoleColor.Gray;
			}

			LogToFile(sIRCMessage.Channel, sIRCMessage.Nick, sIRCMessage.Args);

			if(sChannelInfo.FSelect("parancsok") || sIRCMessage.Channel.Substring(0, 1) != "#")
			{
				if(!sChannelInfo.FSelect("parancsok", sIRCMessage.Channel) && sIRCMessage.Channel.Substring(0, 1) == "#")
					return;

				sIRCMessage.Info[3] = sIRCMessage.Info[3].Remove(0, 1, ":");
				Schumix(sIRCMessage);

				if(sIRCMessage.Info[3] == string.Empty || sIRCMessage.Info[3].Length < PLength || sIRCMessage.Info[3].Substring(0, PLength) != IRCConfig.CommandPrefix)
					return;

				sIRCMessage.Info[3] = sIRCMessage.Info[3].Remove(0, PLength);
				BejovoInfo(sIRCMessage.Info[3].ToLower(), sIRCMessage);
			}
		}

		private void Schumix(IRCMessage sIRCMessage)
		{
			string ParancsJel = IRCConfig.NickName + ",";
			string INick = sIRCMessage.Info[3];

			if(INick.ToLower() == ParancsJel.ToLower())
			{
				CNick(sIRCMessage);

				if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "sys")
				{
					var memory = Process.GetCurrentProcess().WorkingSet64/1024/1024;
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "3Verzió: 10{0}", sUtilities.GetVersion());
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "3Platform: {0}", sUtilities.GetPlatform());
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "3OSVerzió: {0}", Environment.OSVersion.ToString());
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "3Programnyelv: c#");

					if(memory >= 60)
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "3Memoria használat:5 {0} MB", memory);
					else if(memory >= 30)
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "3Memoria használat:8 {0} MB", memory);
					else
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "3Memoria használat:3 {0} MB", memory);

					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "3Uptime: {0}", SchumixBase.timer.Uptime());
				}
				else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "help")
				{
					if(IsAdmin(sIRCMessage.Nick, Commands.AdminFlag.HalfOperator))
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "3Parancsok: nick | sys");
					else if(IsAdmin(sIRCMessage.Nick, Commands.AdminFlag.Operator))
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "3Parancsok: ghost | nick | sys");
					else if(IsAdmin(sIRCMessage.Nick, Commands.AdminFlag.Administrator))
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "3Parancsok: ghost | nick | sys | clean");
					else
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "3Parancsok: sys");
				}
				else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "ghost")
				{
					if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, Commands.AdminFlag.Operator))
						return;

					sSender.NickServGhost(IRCConfig.NickName, IRCConfig.NickServPassword);
					sSender.Nick(IRCConfig.NickName);
					sNickInfo.ChangeNick(IRCConfig.NickName);
					NewNick = false;
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Ghost paranccsal elsődleges nick visszaszerzése.");
				}
				else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "nick")
				{
					if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, Commands.AdminFlag.HalfOperator))
						return;

					if(sIRCMessage.Info.Length < 6)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs paraméter!");
						return;
					}

					if(sIRCMessage.Info[5].ToLower() == "identify")
					{
						sNickInfo.ChangeNick(IRCConfig.NickName);
						sSender.Nick(IRCConfig.NickName);
						Log.Notice("NickServ", "Azonosito jelszo kuldese a kiszolgalonak.");
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Azonosító jelszó küldése a kiszolgálonak.");
						sSender.NickServ(IRCConfig.NickServPassword);
						NewNick = false;

						if(IRCConfig.UseHostServ)
						{
							HostServAllapot = true;
							sSender.HostServ("on");
							Log.Notice("HostServ", "Vhost be van kapcsolva.");
						}
					}
					else
					{
						string nick = sIRCMessage.Info[5];
						sNickInfo.ChangeNick(nick);
						sSender.Nick(nick);
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Név megváltoztatása erre: {0}", nick);
					}
				}
				else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "clean")
				{
					if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, Commands.AdminFlag.Administrator))
						return;

					GC.Collect();
					GC.WaitForPendingFinalizers();
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Lefoglalt memória felszabadításra kerül.");
				}
			}
		}
	}
}