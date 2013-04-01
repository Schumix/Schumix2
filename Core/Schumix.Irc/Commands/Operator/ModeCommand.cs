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
using System.Threading;
using System.Text.RegularExpressions;
using Schumix.Api.Irc;
using Schumix.Irc.Util;
using Schumix.Framework;
using Schumix.Framework.Extensions;

namespace Schumix.Irc.Commands
{
	public abstract partial class CommandHandler
	{
		private Regex ModeRegex = new Regex(@"^[a-zA-Z]{0,4}$");

		protected void HandleMode(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
				return;

			var text = sLManager.GetCommandTexts("mode", sIRCMessage.Channel, sIRCMessage.ServerName);
			if(text.Length < 9)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
				return;
			}

			if(!Rfc2812Util.IsValidChannelName(sIRCMessage.Channel))
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ThisIsNotAChannelDoNotWriteInPM", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoRank", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			string rank = sIRCMessage.Info[4].ToLower();
			
			if(rank.Length > 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				return;
			}

			if(rank.Split('+').Length == 1 && rank.Split('-').Length == 1)
			{
				sSendMessage.SendChatMessage(sIRCMessage, text[2]);
				return;
			}
			
			if(!ModeRegex.IsMatch(rank.Substring(1)))
			{
				sSendMessage.SendChatMessage(sIRCMessage, text[3]);
				return;
			}

			foreach(var c in rank.Substring(1).ToCharArray())
			{
				if(Rfc2812Util.CharToChannelMode(c).ToString().IsNumber())
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[4]);
					return;
				}
			}

			if(sIRCMessage.Info.Length == 5)
			{
				sSender.Mode(sIRCMessage.Channel, sIRCMessage.Info[4].ToLower());
				return;
			}

			if(sIRCMessage.Info.Length < 6)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			bool StringEmpty = false;
			string name = string.Empty;
			string error = string.Empty;
			string rank2 = string.Empty;
			string status = string.Empty;

			if(sIRCMessage.Info.Length >= 10)
			{
				sSendMessage.SendChatMessage(sIRCMessage, text[5]);
				return;
			}

			if(rank.Length-1 > sIRCMessage.Info.Length-5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, text[6]);
				return;
			}

			if(rank.Length-1 < sIRCMessage.Info.Length-5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, text[7]);
				return;
			}

			if(rank.Contains(Rfc2812Util.ModeActionToChar(ModeAction.Remove).ToString()))
				status = Rfc2812Util.ModeActionToChar(ModeAction.Remove).ToString();
			else if(rank.Contains(Rfc2812Util.ModeActionToChar(ModeAction.Add).ToString()))
				status = Rfc2812Util.ModeActionToChar(ModeAction.Add).ToString();

			if(sIRCMessage.Info.Length >= 6 && Rfc2812Util.IsValidNick(sIRCMessage.Info[5]) && sIRCMessage.Info[5].ToLower() != sMyNickInfo.NickStorage.ToLower() && rank.Length > 1 && sChannelList.IsChannelRank(rank.Substring(1, 1)))
			{
				if(status == Rfc2812Util.ModeActionToChar(ModeAction.Remove).ToString())
				{
					rank2 += rank.Substring(1, 1);
					name += SchumixBase.Space + sIRCMessage.Info[5];
				}
				else if(status == Rfc2812Util.ModeActionToChar(ModeAction.Add).ToString())
				{
					rank2 += rank.Substring(1, 1);
					name += SchumixBase.Space + sIRCMessage.Info[5];
				}
			}
			else if(sIRCMessage.Info.Length >= 6 && !Rfc2812Util.IsValidNick(sIRCMessage.Info[5]) && sIRCMessage.Info[5].ToLower() != sMyNickInfo.NickStorage.ToLower())
			{
				if(!sIRCMessage.Info[5].IsNullOrEmpty())
					error += ", " + sIRCMessage.Info[5];
				else
					StringEmpty = true;
			}
			else if(sIRCMessage.Info.Length >= 6 && Rfc2812Util.IsValidNick(sIRCMessage.Info[5]) && sIRCMessage.Info[5].ToLower() == sMyNickInfo.NickStorage.ToLower())
				sSendMessage.SendChatMessage(sIRCMessage, text[0]);

			if(sIRCMessage.Info.Length >= 7 && Rfc2812Util.IsValidNick(sIRCMessage.Info[6]) && sIRCMessage.Info[6].ToLower() != sMyNickInfo.NickStorage.ToLower() && rank.Length > 2 && sChannelList.IsChannelRank(rank.Substring(2).Substring(0, 1)))
			{
				if(status == Rfc2812Util.ModeActionToChar(ModeAction.Remove).ToString())
				{
					rank2 += rank.Substring(2).Substring(0, 1);
					name += SchumixBase.Space + sIRCMessage.Info[6];
				}
				else if(status == Rfc2812Util.ModeActionToChar(ModeAction.Add).ToString())
				{
					rank2 += rank.Substring(2).Substring(0, 1);
					name += SchumixBase.Space + sIRCMessage.Info[6];
				}
			}
			else if(sIRCMessage.Info.Length >= 7 && !Rfc2812Util.IsValidNick(sIRCMessage.Info[6]) && sIRCMessage.Info[6].ToLower() != sMyNickInfo.NickStorage.ToLower())
			{
				if(!sIRCMessage.Info[6].IsNullOrEmpty())
					error += ", " + sIRCMessage.Info[6];
				else
					StringEmpty = true;
			}
			else if(sIRCMessage.Info.Length >= 7 && Rfc2812Util.IsValidNick(sIRCMessage.Info[6]) && sIRCMessage.Info[6].ToLower() == sMyNickInfo.NickStorage.ToLower())
				sSendMessage.SendChatMessage(sIRCMessage, text[0]);

			if(sIRCMessage.Info.Length >= 8 && Rfc2812Util.IsValidNick(sIRCMessage.Info[7]) && sIRCMessage.Info[7].ToLower() != sMyNickInfo.NickStorage.ToLower() && rank.Length > 3 && sChannelList.IsChannelRank(rank.Substring(3).Substring(0, 1)))
			{
				if(status == Rfc2812Util.ModeActionToChar(ModeAction.Remove).ToString())
				{
					rank2 += rank.Substring(3).Substring(0, 1);
					name += SchumixBase.Space + sIRCMessage.Info[7];
				}
				else if(status == Rfc2812Util.ModeActionToChar(ModeAction.Add).ToString())
				{
					rank2 += rank.Substring(3).Substring(0, 1);
					name += SchumixBase.Space + sIRCMessage.Info[7];
				}
			}
			else if(sIRCMessage.Info.Length >= 8 && !Rfc2812Util.IsValidNick(sIRCMessage.Info[7]) && sIRCMessage.Info[7].ToLower() != sMyNickInfo.NickStorage.ToLower())
			{
				if(!sIRCMessage.Info[7].IsNullOrEmpty())
					error += ", " + sIRCMessage.Info[7];
				else
					StringEmpty = true;
			}
			else if(sIRCMessage.Info.Length >= 8 && Rfc2812Util.IsValidNick(sIRCMessage.Info[7]) && sIRCMessage.Info[7].ToLower() == sMyNickInfo.NickStorage.ToLower())
				sSendMessage.SendChatMessage(sIRCMessage, text[0]);

			if(sIRCMessage.Info.Length >= 9 && Rfc2812Util.IsValidNick(sIRCMessage.Info[8]) && sIRCMessage.Info[8].ToLower() != sMyNickInfo.NickStorage.ToLower() && rank.Length > 4 && sChannelList.IsChannelRank(rank.Substring(4).Substring(0, 1)))
			{
				if(status == Rfc2812Util.ModeActionToChar(ModeAction.Remove).ToString())
				{
					rank2 += rank.Substring(4).Substring(0, 1);
					name += SchumixBase.Space + sIRCMessage.Info[8];
				}
				else if(status == Rfc2812Util.ModeActionToChar(ModeAction.Add).ToString())
				{
					rank2 += rank.Substring(4).Substring(0, 1);
					name += SchumixBase.Space + sIRCMessage.Info[8];
				}
			}
			else if(sIRCMessage.Info.Length >= 9 && !Rfc2812Util.IsValidNick(sIRCMessage.Info[8]) && sIRCMessage.Info[8].ToLower() != sMyNickInfo.NickStorage.ToLower())
			{
				if(!sIRCMessage.Info[8].IsNullOrEmpty())
					error += ", " + sIRCMessage.Info[8];
				else
					StringEmpty = true;
			}
			else if(sIRCMessage.Info.Length >= 9 && Rfc2812Util.IsValidNick(sIRCMessage.Info[8]) && sIRCMessage.Info[8].ToLower() == sMyNickInfo.NickStorage.ToLower())
				sSendMessage.SendChatMessage(sIRCMessage, text[0]);

			error = error.Remove(0, 2, ", ");

			if(!error.IsNullOrEmpty())
				sSendMessage.SendChatMessage(sIRCMessage, error + SchumixBase.Colon + SchumixBase.Space + sLManager.GetWarningText("NotaNickNameHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));

			if(StringEmpty)
				sSendMessage.SendChatMessage(sIRCMessage, text[8]);

			ModePrivmsg = sIRCMessage.Channel;
			sSender.Mode(sIRCMessage.Channel, status + rank2, name.Remove(0, 1, SchumixBase.Space));

			// Clean
			Thread.Sleep(5*1000);

			if(sIRCMessage.Channel.ToLower() == ModePrivmsg.ToLower() || ModePrivmsg == string.Empty)
				ModePrivmsg = string.Empty;
		}
	}
}