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
using System.Text.RegularExpressions;
using Schumix.Api.Irc;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using Schumix.CalendarAddon;

namespace Schumix.CalendarAddon.Commands
{
	class BanCommand : CommandInfo
	{
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private Regex _regex;
		private Unban sUnban;
		private Ban sBan;

		public BanCommand(string ServerName) : base(ServerName)
		{
			sBan = new Ban(ServerName);
			sUnban = new Unban(ServerName);
			_regex = new Regex(@"((?<year>[0-9]{1,4})(?:[\.\s]+))?"                         // Year
			                   + @"((?<month>[0-9]{1,2}|[a-zóüöúőűáéí]{3,20})(?:[\.\s]+))?" // Month
			                   + @"((?<day>[0-9]{1,2})(?:[\.\s]+))?"                        // Day
			                   + @"(?<hour>[0-9]{1,2})(?:[:])?"                             // Hour
			                   + @"(?<minute>[0-9]{1,2})?"                                  // Minute
			                   + @"((?:[\s]+)(?<text>(.*)))?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
		}

		private int GetYear(string args)
		{
			return _regex.IsMatch(args) ? _regex.Match(args).Groups["year"].ToString().ToNumber(-1).ToInt() : -1;
		}

		private string GetMonth(string args)
		{
			return _regex.IsMatch(args) ? _regex.Match(args).Groups["month"].ToString() : string.Empty;
		}

		private int GetDay(string args)
		{
			return _regex.IsMatch(args) ? _regex.Match(args).Groups["day"].ToString().ToNumber(32).ToInt() : 32;
		}

		private int GetHour(string args)
		{
			return _regex.IsMatch(args) ? _regex.Match(args).Groups["hour"].ToString().ToNumber(25).ToInt() : 25;
		}

		private int GetMinute(string args)
		{
			return _regex.IsMatch(args) ? _regex.Match(args).Groups["minute"].ToString().ToNumber(61).ToInt() : 61;
		}

		private string GetMessage(string args)
		{
			return _regex.IsMatch(args) ? _regex.Match(args).Groups["text"].ToString() : string.Empty;
		}

		private bool IsHourAndMinute(string args)
		{
			return GetYear(args) == -1 && GetMonth(args) == string.Empty && GetDay(args) == 32;
		}

		public void HandleBan(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
				return;

			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoBanNameOrVhost", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			if(sIRCMessage.Info.Length < 6)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoTime", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			string args = sIRCMessage.Info.SplitToString(5, SchumixBase.Space);

			if(IsHourAndMinute(args))
			{
				if(GetMessage(args).IsNullOrEmpty())
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoReason", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				int hour = GetHour(args);
				if(hour >= 24 || hour < 0)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorHour", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				int minute = GetMinute(args);
				if(minute >= 60 || minute < 0)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMinute", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				sSendMessage.SendChatMessage(sIRCMessage, sBan.BanName(sIRCMessage.Info[4].ToLower(), sIRCMessage.Channel, GetMessage(args), hour, minute));
			}
			else
			{
				if(sIRCMessage.Info.Length < 7)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoTime", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(GetMessage(args).IsNullOrEmpty())
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoReason", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				int year = GetYear(args);
				if(year < 0)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorYear", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				int month = GetMonth(args).GetMonthNameInInt(sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName));
				if(month > 12 || month <= 0)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMonth", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				int day = GetDay(args);
				if(!sUtilities.IsDay(year, month, day))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorDay", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				int hour = GetHour(args);
				if(hour >= 24 || hour < 0)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorHour", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				int minute = GetMinute(args);
				if(minute >= 60 || minute < 0)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMinute", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				sSendMessage.SendChatMessage(sIRCMessage, sBan.BanName(sIRCMessage.Info[4].ToLower(), sIRCMessage.Channel, GetMessage(args), year, month, day, hour, minute));
			}
		}

		public void HandleUnban(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
				return;

			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoUnbanNameOrVhost", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			sUnban.UnbanName(sIRCMessage.Info[4], sIRCMessage.Channel);
		}
	}
}