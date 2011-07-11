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
			if(sNickName.Ignore(sIRCMessage.Nick))
				return;

			foreach(var plugin in sAddonManager.GetPlugins())
				plugin.HandlePrivmsg(sIRCMessage);

			if(ConsoleLog.CLog)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("[{0}] <{1}> {2}", sIRCMessage.Channel, sIRCMessage.Nick, sIRCMessage.Args);
				Console.ForegroundColor = ConsoleColor.Gray;
			}

			LogToFile(sIRCMessage.Channel, sIRCMessage.Nick, sIRCMessage.Args);

			if(sChannelInfo.FSelect("commands") || sIRCMessage.Channel.Substring(0, 1) != "#")
			{
				if(!sChannelInfo.FSelect("commands", sIRCMessage.Channel) && sIRCMessage.Channel.Substring(0, 1) == "#")
					return;

				sIRCMessage.Info[3] = sIRCMessage.Info[3].Remove(0, 1, ":");
				Schumix(sIRCMessage);

				if(sIRCMessage.Info[3] == string.Empty || sIRCMessage.Info[3].Length < PLength || sIRCMessage.Info[3].Substring(0, PLength) != IRCConfig.CommandPrefix)
					return;

				sIRCMessage.Info[3] = sIRCMessage.Info[3].Remove(0, PLength);
				IncomingInfo(sIRCMessage.Info[3].ToLower(), sIRCMessage);
			}
		}

		private void Schumix(IRCMessage sIRCMessage)
		{
			string command = IRCConfig.NickName + ",";

			if(sIRCMessage.Info[3].ToLower() == command.ToLower())
			{
				CNick(sIRCMessage);

				if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "sys")
				{
					var text = sLManager.GetCommandTexts("schumix2/sys", sIRCMessage.Channel);
					if(text.Length < 8)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						return;
					}

					var memory = Process.GetCurrentProcess().WorkingSet64/1024/1024;
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0], sUtilities.GetVersion());
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1], sUtilities.GetPlatform());
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[2], Environment.OSVersion.ToString());
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[3]);

					if(memory >= 60)
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[4], memory);
					else if(memory >= 30)
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[5], memory);
					else
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[6], memory);

					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[7], SchumixBase.timer.Uptime(sLManager.GetChannelLocalization(sIRCMessage.Channel)));
				}
				else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "help")
				{
					var text = sLManager.GetCommandTexts("schumix2/help", sIRCMessage.Channel);
					if(text.Length < 4)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						return;
					}

					if(IsAdmin(sIRCMessage.Nick, Commands.AdminFlag.HalfOperator))
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
					else if(IsAdmin(sIRCMessage.Nick, Commands.AdminFlag.Operator))
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1]);
					else if(IsAdmin(sIRCMessage.Nick, Commands.AdminFlag.Administrator))
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[2]);
					else
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[3]);
				}
				else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "ghost")
				{
					if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, Commands.AdminFlag.Operator))
						return;

					sSender.NickServGhost(IRCConfig.NickName, IRCConfig.NickServPassword);
					sSender.Nick(IRCConfig.NickName);
					sNickInfo.ChangeNick(IRCConfig.NickName);
					NewNick = false;
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("schumix2/ghost", sIRCMessage.Channel));
				}
				else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "nick")
				{
					if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, Commands.AdminFlag.HalfOperator))
						return;

					if(sIRCMessage.Info.Length < 6)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoValue", sIRCMessage.Channel));
						return;
					}

					if(sIRCMessage.Info[5].ToLower() == "identify")
					{
						sNickInfo.ChangeNick(IRCConfig.NickName);
						sSender.Nick(IRCConfig.NickName);
						Log.Notice("NickServ", "Azonosito jelszo kuldese a kiszolgalonak.");
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("schumix2/nick/identify", sIRCMessage.Channel));
						sSender.NickServ(IRCConfig.NickServPassword);
						NewNick = false;

						if(IRCConfig.UseHostServ)
						{
							HostServStatus = true;
							sSender.HostServ("on");
							Log.Notice("HostServ", "Vhost be van kapcsolva.");
						}
					}
					else
					{
						string nick = sIRCMessage.Info[5];
						sNickInfo.ChangeNick(nick);
						sSender.Nick(nick);
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("schumix2/nick", sIRCMessage.Channel), nick);
					}
				}
				else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "clean")
				{
					if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, Commands.AdminFlag.Administrator))
						return;

					GC.Collect();
					GC.WaitForPendingFinalizers();
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("schumix2/clean", sIRCMessage.Channel));
				}
			}
		}
	}
}