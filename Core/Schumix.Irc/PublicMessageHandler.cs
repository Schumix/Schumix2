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

			if(ConsoleLog.CLog)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("[{0}] <{1}> {2}", sIRCMessage.Channel, sIRCMessage.Nick, sIRCMessage.Args);
				Console.ForegroundColor = ConsoleColor.Gray;
			}

			LogToFile(sIRCMessage.Channel, sIRCMessage.Nick, sIRCMessage.Args);

			if(sIRCMessage.Channel.Length >= 1 && sIRCMessage.Channel.Substring(0, 1) != "#")
				sIRCMessage.Channel = sIRCMessage.Nick;

			foreach(var plugin in sAddonManager.GetPlugins())
				plugin.HandlePrivmsg(sIRCMessage);

			if(sChannelInfo.FSelect("commands") || sIRCMessage.Channel.Substring(0, 1) != "#")
			{
				if(!sChannelInfo.FSelect("commands", sIRCMessage.Channel) && sIRCMessage.Channel.Substring(0, 1) == "#")
					return;

				sIRCMessage.Info[3] = sIRCMessage.Info[3].Remove(0, 1, SchumixBase.Point2);
				Schumix(sIRCMessage);

				if(sIRCMessage.Info[3] == string.Empty || sIRCMessage.Info[3].Length < PLength || sIRCMessage.Info[3].Substring(0, PLength) != IRCConfig.CommandPrefix)
					return;

				sIRCMessage.Info[3] = sIRCMessage.Info[3].Remove(0, PLength);
				IncomingInfo(sIRCMessage.Info[3].ToLower(), sIRCMessage);
			}
		}

		private void Schumix(IRCMessage sIRCMessage)
		{
			string command = IRCConfig.NickName + SchumixBase.Comma;

			if(sIRCMessage.Info[3].ToLower() == command.ToLower())
			{
				if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "sys")
				{
					var text = sLManager.GetCommandTexts("schumix2/sys", sIRCMessage.Channel);
					if(text.Length < 8)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						return;
					}

					var memory = Process.GetCurrentProcess().WorkingSet64/1024/1024;
					sSendMessage.SendChatMessage(sIRCMessage, text[0], sUtilities.GetVersion());
					sSendMessage.SendChatMessage(sIRCMessage, text[1], sUtilities.GetPlatform());
					sSendMessage.SendChatMessage(sIRCMessage, text[2], Environment.OSVersion.ToString());
					sSendMessage.SendChatMessage(sIRCMessage, text[3]);

					if(memory >= 60)
						sSendMessage.SendChatMessage(sIRCMessage, text[4], memory);
					else if(memory >= 30)
						sSendMessage.SendChatMessage(sIRCMessage, text[5], memory);
					else
						sSendMessage.SendChatMessage(sIRCMessage, text[6], memory);

					sSendMessage.SendChatMessage(sIRCMessage, text[7], SchumixBase.timer.Uptime(sLManager.GetChannelLocalization(sIRCMessage.Channel)));
				}
				else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "help")
				{
					var text = sLManager.GetCommandTexts("schumix2/help", sIRCMessage.Channel);
					if(text.Length < 4)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						return;
					}

					if(IsAdmin(sIRCMessage.Nick, Commands.AdminFlag.HalfOperator))
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
					else if(IsAdmin(sIRCMessage.Nick, Commands.AdminFlag.Operator))
						sSendMessage.SendChatMessage(sIRCMessage, text[1]);
					else if(IsAdmin(sIRCMessage.Nick, Commands.AdminFlag.Administrator))
						sSendMessage.SendChatMessage(sIRCMessage, text[2]);
					else
						sSendMessage.SendChatMessage(sIRCMessage, text[3]);
				}
				else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "ghost")
				{
					if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, Commands.AdminFlag.Operator))
						return;

					sSender.NickServGhost(IRCConfig.NickName, IRCConfig.NickServPassword);
					sSender.Nick(IRCConfig.NickName);
					sNickInfo.ChangeNick(IRCConfig.NickName);
					NewNick = false;
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("schumix2/ghost", sIRCMessage.Channel));
				}
				else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "nick")
				{
					if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, Commands.AdminFlag.HalfOperator))
						return;

					if(sIRCMessage.Info.Length < 6)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoValue", sIRCMessage.Channel));
						return;
					}

					if(sIRCMessage.Info[5].ToLower() == "identify")
					{
						sNickInfo.ChangeNick(IRCConfig.NickName);
						sSender.Nick(IRCConfig.NickName);
						Log.Notice("NickServ", sLConsole.NickServ("Text"));
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("schumix2/nick/identify", sIRCMessage.Channel));
						sSender.NickServ(IRCConfig.NickServPassword);
						NewNick = false;

						if(IRCConfig.UseHostServ)
						{
							HostServStatus = true;
							sSender.HostServ("on");
							Log.Notice("HostServ", sLConsole.HostServ("Text"));
						}
					}
					else
					{
						string nick = sIRCMessage.Info[5];
						sNickInfo.ChangeNick(nick);
						sSender.Nick(nick);
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("schumix2/nick", sIRCMessage.Channel), nick);
					}
				}
				else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "clean")
				{
					if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, Commands.AdminFlag.Administrator))
						return;

					GC.Collect();
					GC.WaitForPendingFinalizers();
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("schumix2/clean", sIRCMessage.Channel));
				}
			}
		}
	}
}