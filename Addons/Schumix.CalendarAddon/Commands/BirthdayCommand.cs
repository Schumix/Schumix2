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
using Schumix.Irc;
using Schumix.Irc.Util;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Irc;
using Schumix.Framework.Util;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using Schumix.CalendarAddon;

namespace Schumix.CalendarAddon.Commands
{
	class BirthdayCommand : CommandInfo
	{
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private Regex _regex;

		public BirthdayCommand(string ServerName) : base(ServerName)
		{
			_regex = new Regex(@"((?<year>[0-9]{4,4})(?:[\.\s]+|))?"                         // Year
			                   + @"((?<month>[0-9]{1,2}|[a-zóüöúőűáéí]{3,20})(?:[\.\s]+|))?" // Month
			                   + @"((?<day>[0-9]{1,2})(?:[\.\s]|))?",                        // Day
			                   RegexOptions.Compiled | RegexOptions.IgnoreCase);
		}

		private int GetYear(string args)
		{
			return _regex.IsMatch(args) ? _regex.Match(args).Groups["year"].ToString().ToNumber(-1).ToInt32() : -1;
		}

		private string GetMonth(string args)
		{
			return _regex.IsMatch(args) ? _regex.Match(args).Groups["month"].ToString() : string.Empty;
		}

		private int GetDay(string args)
		{
			return _regex.IsMatch(args) ? _regex.Match(args).Groups["day"].ToString().ToNumber(32).ToInt32() : 32;
		}

		private bool IsYear(string args)
		{
			return _regex.IsMatch(args) && !_regex.Match(args).Groups["year"].ToString().IsNullOrEmpty();
		}

		private bool IsMonth(string args)
		{
			return _regex.IsMatch(args) && !_regex.Match(args).Groups["month"].ToString().IsNullOrEmpty();
		}

		private bool IsDay(string args)
		{
			return _regex.IsMatch(args) && !_regex.Match(args).Groups["day"].ToString().IsNullOrEmpty();
		}

		public void HandleBirthday(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
				return;

			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoValue", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			if(sIRCMessage.Info[4].ToLower() == "info")
			{
				var text = sLManager.GetCommandTexts("birthday/info", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 4)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}

				string name = sIRCMessage.Info.Length < 6 ? sIRCMessage.Nick.ToLower() : sUtilities.SqlEscape(sIRCMessage.Info[5].ToLower());
				if(!Rfc2812Util.IsValidNick(name))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaNickNameHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				var db = SchumixBase.DManager.QueryFirstRow("SELECT Enabled, Year, Month, Day FROM birthday WHERE Name = '{0}' And ServerName = '{1}'", name, sIRCMessage.ServerName);
				if(!db.IsNull())
				{
					bool enabled = db["Enabled"].ToBoolean();
					int year = db["Year"].ToInt32();
					int month = db["Month"].ToInt32();
					int day = db["Day"].ToInt32();

					sSendMessage.SendChatMessage(sIRCMessage, text[0], enabled ? SchumixBase.On : SchumixBase.Off);
					sSendMessage.SendChatMessage(sIRCMessage, text[1], year, month.ToMonthFormat(), day.ToDayFormat());
				}
				else
				{
					if(sIRCMessage.Nick.ToLower() == name)
						sSendMessage.SendChatMessage(sIRCMessage, text[2]);
					else
						sSendMessage.SendChatMessage(sIRCMessage, text[3]);
				}
			}
			else if(sIRCMessage.Info[4].ToLower() == "change")
			{
				if(!Warning(sIRCMessage))
					return;

				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoValue", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "status")
				{
					var text = sLManager.GetCommandTexts("birthday/change/status", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoFunctionStatus", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					string status = sIRCMessage.Info[6].ToLower();
					if(status == SchumixBase.On || status == SchumixBase.Off)
					{
						SchumixBase.DManager.Update("birthday", string.Format("Enabled = '{0}'", status == SchumixBase.On), string.Format("Name = '{0}' And ServerName = '{1}'", sIRCMessage.Nick.ToLower(), sIRCMessage.ServerName));
	
						if(status == SchumixBase.On)
							sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						else
							sSendMessage.SendChatMessage(sIRCMessage, text[1]);
					}
					else
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("WrongSwitch", sIRCMessage.Channel, sIRCMessage.ServerName));
				}
				else if(sIRCMessage.Info[5].ToLower() == "birthday")
				{
					var text = sLManager.GetCommandTexts("birthday/change/birthday", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 4)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					string args = sIRCMessage.Info.SplitToString(6, SchumixBase.Space);

					if(!IsYear(args))
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[3]);
						return;
					}

					if(!IsMonth(args))
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}

					if(!IsDay(args))
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[1]);
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

					SchumixBase.DManager.Update("birthday", string.Format("Year = '{0}', Month = '{1}', Day = '{2}'", year, month, day), string.Format("Name = '{0}' And ServerName = '{1}'", sIRCMessage.Nick.ToLower(), sIRCMessage.ServerName));
					sSendMessage.SendChatMessage(sIRCMessage, text[2]);
				}
			}
			else if(sIRCMessage.Info[4].ToLower() == "register")
			{
				var text = sLManager.GetCommandTexts("birthday/register", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 5)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}

				var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM birthday WHERE Name = '{0}' And ServerName = '{1}'", sIRCMessage.Nick.ToLower(), sIRCMessage.ServerName);
				if(!db.IsNull())
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[0]);
					return;
				}

				string args = sIRCMessage.Info.SplitToString(6, SchumixBase.Space);

				if(!IsYear(args))
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[4]);
					return;
				}

				if(!IsMonth(args))
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
					return;
				}

				if(!IsDay(args))
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[2]);
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

				SchumixBase.DManager.Insert("`birthday`(ServerId, ServerName, Name, Year, Month, Day)", IRCConfig.List[sIRCMessage.ServerName].ServerId, sIRCMessage.ServerName, sIRCMessage.Nick.ToLower(), year, month, day);
				sSendMessage.SendChatMessage(sIRCMessage, text[3]);
			}
			else if(sIRCMessage.Info[4].ToLower() == "remove")
			{
				var text = sLManager.GetCommandTexts("birthday/remove", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 2)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}

				var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM birthday WHERE Name = '{0}' And ServerName = '{1}'", sIRCMessage.Nick.ToLower(), sIRCMessage.ServerName);
				if(db.IsNull())
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[0]);
					return;
				}

				SchumixBase.DManager.Delete("birthday", string.Format("Name = '{0}' And ServerName = '{1}'", sIRCMessage.Nick.ToLower(), sIRCMessage.ServerName));
				sSendMessage.SendChatMessage(sIRCMessage, text[1]);
			}
		}

		private bool Warning(IRCMessage sIRCMessage)
		{
			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;
			var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM birthday WHERE Name = '{0}' And ServerName = '{1}'", sIRCMessage.Nick.ToLower(), sIRCMessage.ServerName);
			if(db.IsNull())
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("birthday", sIRCMessage.Channel, sIRCMessage.ServerName));
				return false;
			}

			return true;
		}
	}
}