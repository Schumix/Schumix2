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
using System.Diagnostics;
using Schumix.Irc.Util;
using Schumix.Framework;
using Schumix.Framework.Irc;
using Schumix.Framework.Logger;
using Schumix.Framework.Config;
using Schumix.Framework.Functions;
using Schumix.Framework.Extensions;

namespace Schumix.Irc
{
	public abstract partial class MessageHandler
	{
		protected void HandlePrivmsg(IRCMessage sIRCMessage)
		{
			if(sIgnoreNickName.IsIgnore(sIRCMessage.Nick))
				return;

			if(ConsoleLog.CLog)
			{
				Log.SetForegroundColor(ConsoleColor.Yellow);
				Log.WriteLine("[{0}] <{1}> {2}", sIRCMessage.Channel, sIRCMessage.Nick, sIRCMessage.Args);
				Log.SetForegroundColor(ConsoleColor.Gray);
			}

			if(Rfc2812Util.IsAction(sIRCMessage.Args))
				sIrcLog.LogInFile(sIRCMessage.Channel, sIRCMessage.Nick, sLConsole.GetString("[ACTION] {0}"), Rfc2812Util.GetAction(sIRCMessage.Args));
			else
				sIrcLog.LogInFile(sIRCMessage.Channel, sIRCMessage.Nick, sIRCMessage.Args);

			if(!Rfc2812Util.IsValidChannelName(sIRCMessage.Channel))
				sIRCMessage.Channel = sIRCMessage.Nick;

			sCtcpSender.CtcpReply(sIRCMessage);

			if(sMyChannelInfo.FSelect(IFunctions.Commands) || !Rfc2812Util.IsValidChannelName(sIRCMessage.Channel))
			{
				if(!sMyChannelInfo.FSelect(IChannelFunctions.Commands, sIRCMessage.Channel) && Rfc2812Util.IsValidChannelName(sIRCMessage.Channel))
					return;

				HandleCommand(sIRCMessage);
			}
		}

		private void HandleCommand(IRCMessage sIRCMessage)
		{
			sIRCMessage.Info[3] = sIRCMessage.Info[3].Remove(0, 1, SchumixBase.Colon);
			Schumix(sIRCMessage);
			
			if(sIRCMessage.Info[3].IsNullOrEmpty() || sIRCMessage.Info[3].Length < PLength || sIRCMessage.Info[3].Substring(0, PLength) != IRCConfig.List[sIRCMessage.ServerName].CommandPrefix)
				return;
			
			sIRCMessage.Info[3] = sIRCMessage.Info[3].Remove(0, PLength);
			IncomingInfo(sIRCMessage.Info[3].ToLower(), sIRCMessage);
		}

		private void Schumix(IRCMessage sIRCMessage)
		{
			string command = IRCConfig.List[sIRCMessage.ServerName].NickName + SchumixBase.Comma;

			if(sIRCMessage.Info[3].ToLower() == command.ToLower())
			{
				sAntiFlood.FloodCommand(sIRCMessage);

				if(sAntiFlood.Ignore(sIRCMessage))
					return;

				if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "sys")
				{
					var text = sLManager.GetCommandTexts("schumix2/sys", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 8)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					var memory = sRuntime.MemorySizeInMB;
					int ircnetwork = sIrcBase.Networks.Count > 1 ? 20 * sIrcBase.Networks.Count : 0;
					sSendMessage.SendChatMessage(sIRCMessage, text[0], sUtilities.GetVersion());
					sSendMessage.SendChatMessage(sIRCMessage, text[1], sPlatform.GetPlatform());
					sSendMessage.SendChatMessage(sIRCMessage, text[2], string.Format("{0} {1}bit", sPlatform.GetOSName(), sPlatform.Is64BitProcess ? 64 : 32));
					sSendMessage.SendChatMessage(sIRCMessage, text[3]);

					if(memory >= 75 + ircnetwork)
						sSendMessage.SendChatMessage(sIRCMessage, text[4], memory);
					else if(memory >= 45 + ircnetwork)
						sSendMessage.SendChatMessage(sIRCMessage, text[5], memory);
					else
						sSendMessage.SendChatMessage(sIRCMessage, text[6], memory);

					sSendMessage.SendChatMessage(sIRCMessage, text[7], SchumixBase.sTimer.Uptime(sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
				}
				else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "help")
				{
					var text = sLManager.GetCommandTexts("schumix2/help", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 4)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
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

					sSender.NickServGhost(IRCConfig.List[sIRCMessage.ServerName].NickName, IRCConfig.List[sIRCMessage.ServerName].NickServPassword);
					sSender.Nick(IRCConfig.List[sIRCMessage.ServerName].NickName);
					sMyNickInfo.ChangeNick(IRCConfig.List[sIRCMessage.ServerName].NickName);
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("schumix2/ghost", sIRCMessage.Channel, sIRCMessage.ServerName));
				}
				else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "nick")
				{
					if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, Commands.AdminFlag.HalfOperator))
						return;

					if(sIRCMessage.Info.Length < 6)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoValue", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(sIRCMessage.Info[5].ToLower() == "identify")
					{
						sMyNickInfo.ChangeIdentifyStatus(false);
						sMyNickInfo.ChangeVhostStatus(false);
						sMyNickInfo.ChangeNick(IRCConfig.List[sIRCMessage.ServerName].NickName);
						sSender.Nick(IRCConfig.List[sIRCMessage.ServerName].NickName);
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("schumix2/nick/identify", sIRCMessage.Channel, sIRCMessage.ServerName));
						sMyNickInfo.Identify(IRCConfig.List[sIRCMessage.ServerName].NickServPassword);

						if(IRCConfig.List[sIRCMessage.ServerName].UseHostServ)
							sMyNickInfo.Vhost(SchumixBase.On);
					}
					else
					{
						sIrcBase.Networks[sIRCMessage.ServerName].NewNick = true;
						string nick = sIRCMessage.Info[5];

						if(!Rfc2812Util.IsValidNick(nick))
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaNickNameHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						sMyNickInfo.ChangeNick(nick);
						sSender.Nick(nick);
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("schumix2/nick", sIRCMessage.Channel, sIRCMessage.ServerName), nick);
					}
				}
				else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "clean")
				{
					if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, Commands.AdminFlag.Administrator))
						return;

					GC.Collect(1);
					GC.WaitForPendingFinalizers();
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("schumix2/clean", sIRCMessage.Channel, sIRCMessage.ServerName));
				}
			}
		}
	}
}