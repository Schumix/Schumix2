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
using Schumix.Config;

namespace Schumix.IRC
{
	public partial class MessageHandler
	{
		public static Commands.CommandManager CManager = new Commands.CommandManager();

		public void HandlePrivmsg()
		{
			if(Consol.ConsoleLog == 1)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("[{0}] <{1}> {2}", Network.IMessage.Channel, Network.IMessage.Nick, Network.IMessage.Args);
				Console.ForegroundColor = ConsoleColor.Gray;
			}

			LogToFajl(Network.IMessage.Channel, Network.IMessage.Nick, Network.IMessage.Args);

			if(FSelect("parancsok") == "be")
			{
				//if(FSelect("parancsok", Network.IMessage.Channel) != "be" && Network.IMessage.Channel.Substring(0, 1) != "#")
					//return;

				if(Network.IMessage.Info[3].Substring(0, 1) == ":")
					Network.IMessage.Info[3] = Network.IMessage.Info[3].Remove(0, 1);

				if(Network.IMessage.Info[Network.IMessage.Info.Length-2] == "" || Network.IMessage.Info[Network.IMessage.Info.Length-1] == "")
					return;

				Schumix();

				if(Network.IMessage.Info[3] == "" || Network.IMessage.Info[3].Substring(0, 1) == " " || Network.IMessage.Info[3].Substring(0, 1) != IRCConfig.Parancselojel)
					return;

				Network.IMessage.Info[3] = Network.IMessage.Info[3].Remove(0, 1);
				CManager.BejovoInfo(Network.IMessage.Info[3].ToLower());
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

					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("3Verzió: 10{0}", Verzio.SchumixVerzio));
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("3Platform: {0}", Platform));
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("3OSVerzió: {0}", Environment.OSVersion.ToString()));
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("3Programnyelv: c#"));

					/*if(memory >= 20)
						sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("3Memoria használat: 8{0} MB", memory));
					else if(memory >= 30)
						sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("3Memoria használat: 5{0} MB", memory));
					else
						sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("3Memoria használat: 3{0} MB", memory));*/

					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("3Memoria használat: {0} MB", memory));
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("3Uptime: {0}", SchumixBot.Uptime()));
				}
				else if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4] == "help")
				{
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "3Parancsok: ghost | nick | sys");
				}
				else if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4] == "ghost")
				{
					if(!CManager.Admin(Network.IMessage.Nick, Network.IMessage.Host, Commands.AdminFlag.Operator))
						return;

					sSendMessage.SendChatMessage(MessageType.PRIVMSG, "NickServ", String.Format("ghost {0} {1}", IRCConfig.NickName, IRCConfig.NickServPassword));
					sSendMessage.WriteLine(String.Format("NICK {0}", IRCConfig.NickName));
					SchumixBot.NickTarolo = IRCConfig.NickName;
				}
				else if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4] == "nick")
				{
					if(!CManager.Admin(Network.IMessage.Nick, Network.IMessage.Host, Commands.AdminFlag.Operator))
						return;

					if(Network.IMessage.Info.Length < 6)
						return;

					if(Network.IMessage.Info[5] == "identify")
					{
						SchumixBot.NickTarolo = IRCConfig.NickName;
						sSendMessage.WriteLine(String.Format("NICK {0}", IRCConfig.NickName));
						Log.Notice("NickServ", "Sending NickServ identification.");
						sSendMessage.SendChatMessage(MessageType.PRIVMSG, "NickServ", String.Format("identify {0}", IRCConfig.NickServPassword));
					}
					else
					{
						string nick = Network.IMessage.Info[5];
						SchumixBot.NickTarolo = nick;
						sSendMessage.WriteLine(String.Format("NICK {0}", nick));
					}
				}
			}
		}
	}
}