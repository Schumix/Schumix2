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
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using Schumix.Framework;
using Schumix.Framework.Config;

namespace Schumix.Irc
{
	public partial class MessageHandler
	{
		public void HandlePrivmsg()
		{
			if(ConsoleLog.CLog == 1)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("[{0}] <{1}> {2}", Network.IMessage.Channel, Network.IMessage.Nick, Network.IMessage.Args);
				Console.ForegroundColor = ConsoleColor.Gray;
			}

			LogToFajl(Network.IMessage.Channel, Network.IMessage.Nick, Network.IMessage.Args);

			if(Network.sChannelInfo.FSelect("parancsok") == "be")
			{
				//if(Network.sChannelInfo.FSelect("parancsok", Network.IMessage.Channel) != "be" && Network.IMessage.Channel.Substring(0, 1) != "#")
					//return;

				if(Network.IMessage.Info[3].Substring(0, 1) == ":")
					Network.IMessage.Info[3] = Network.IMessage.Info[3].Remove(0, 1);

				if(Network.IMessage.Info[Network.IMessage.Info.Length-2] == "" || Network.IMessage.Info[Network.IMessage.Info.Length-1] == "")
					return;

				Schumix();

				if(Network.IMessage.Info[3] == "" || Network.IMessage.Info[3].Substring(0, 1) == " " || Network.IMessage.Info[3].Substring(0, 1) != IRCConfig.Parancselojel)
					return;

				Network.IMessage.Info[3] = Network.IMessage.Info[3].Remove(0, 1);
				BejovoInfo(Network.IMessage.Info[3].ToLower());
			}
		}

		private void Schumix()
		{
			string ParancsJel = IRCConfig.NickName + ",";
			string INick = Network.IMessage.Info[3];

			if(INick.ToLower() == ParancsJel.ToLower())
			{
				CNick();

				if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4] == "sys")
				{
					string Platform = "";
					var pid = Environment.OSVersion.Platform;

					switch(pid)
					{
						case PlatformID.Win32NT:
						case PlatformID.Win32S:
						case PlatformID.Win32Windows:
						case PlatformID.WinCE:
							Platform = "Windows";
							break;
						case PlatformID.Unix:
							Platform = "Linux";
							break;
						default:
							Platform = "Ismeretlen";
							break;
					}

					var memory = Process.GetCurrentProcess().WorkingSet64/1024/1024;

					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "3Verzió: 10{0}", Verzio.SchumixVerzio);
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "3Platform: {0}", Platform);
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "3OSVerzió: {0}", Environment.OSVersion.ToString());
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "3Programnyelv: c#");

					/*if(memory >= 20)
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "3Memoria használat: 8{0} MB", memory);
					else if(memory >= 30)
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "3Memoria használat: 5{0} MB", memory);
					else
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "3Memoria használat: 3{0} MB", memory);*/

					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "3Memoria használat: {0} MB", memory);
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "3Uptime: {0}", SchumixBase.Uptime());
				}
				else if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4] == "help")
				{
					if(Admin(Network.IMessage.Nick, Commands.AdminFlag.Operator))
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "3Parancsok: ghost | nick | sys");
					else if(Admin(Network.IMessage.Nick, Commands.AdminFlag.Administrator))
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "3Parancsok: ghost | nick | sys | clean");
					else
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "3Parancsok: sys");
				}
				else if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4] == "ghost")
				{
					if(!Admin(Network.IMessage.Nick, Network.IMessage.Host, Commands.AdminFlag.Operator))
						return;

					sSender.NickServGhost(IRCConfig.NickName, IRCConfig.NickServPassword);
					sSender.Nick(IRCConfig.NickName);
					sNickInfo.ChangeNick(IRCConfig.NickName);
				}
				else if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4] == "nick")
				{
					if(!Admin(Network.IMessage.Nick, Network.IMessage.Host, Commands.AdminFlag.Operator))
						return;

					if(Network.IMessage.Info.Length < 6)
						return;

					if(Network.IMessage.Info[5] == "identify")
					{
						sNickInfo.ChangeNick(IRCConfig.NickName);
						sSender.Nick(IRCConfig.NickName);
						Log.Notice("NickServ", "NickServ azonosito kuldese.");
						sSender.NickServ(IRCConfig.NickServPassword);
					}
					else
					{
						string nick = Network.IMessage.Info[5];
						sNickInfo.ChangeNick(nick);
						sSender.Nick(nick);
					}
				}
				else if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4] == "clean")
				{
					if(!Admin(Network.IMessage.Nick, Network.IMessage.Host, Commands.AdminFlag.Administrator))
						return;

					GC.Collect();
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Lefoglalt memória felszabadításra került.");
				}
			}
		}
	}
}