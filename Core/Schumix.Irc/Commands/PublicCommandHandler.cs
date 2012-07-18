/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Megax <http://www.megaxx.info/>
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
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using Schumix.API;
using Schumix.API.Irc;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Irc.Commands.GoogleWebSearch;

namespace Schumix.Irc.Commands
{
	public partial class CommandHandler
	{
		protected void HandleXbot(IRCMessage sIRCMessage)
		{
			var text = sLManager.GetCommandTexts("xbot", sIRCMessage.Channel, sIRCMessage.ServerName);
			if(text.Length < 3)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
				return;
			}

			sSendMessage.SendChatMessage(sIRCMessage, text[0], sUtilities.GetVersion());
			string commands = string.Empty;

			foreach(var command in sIrcBase.Networks[sIRCMessage.ServerName].CommandMethodMap)
			{
				if(command.Value.Permission != CommandPermission.Normal)
					continue;

				if(command.Key == "xbot")
					continue;

				if(sIgnoreCommand.IsIgnore(command.Key))
					continue;

				commands += " | " + IRCConfig.List[sIRCMessage.ServerName].CommandPrefix + command.Key;
			}

			sSendMessage.SendChatMessage(sIRCMessage, text[1], Consts.SchumixProgrammedBy);
			sSendMessage.SendChatMessage(sIRCMessage, text[2], commands.Remove(0, 3, " | "));
		}

		protected void HandleInfo(IRCMessage sIRCMessage)
		{
			var text = sLManager.GetCommandTexts("info", sIRCMessage.Channel, sIRCMessage.ServerName);
			if(text.Length < 4)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
				return;
			}

			sSendMessage.SendChatMessage(sIRCMessage, text[0], Consts.SchumixProgrammedBy);
			sSendMessage.SendChatMessage(sIRCMessage, text[1], Consts.SchumixDevelopers);
			sSendMessage.SendChatMessage(sIRCMessage, text[2], Consts.SchumixWebsite);
			sSendMessage.SendChatMessage(sIRCMessage, text[3]);
		}

		protected void HandleTime(IRCMessage sIRCMessage)
		{
			var text = sLManager.GetCommandTexts("time", sIRCMessage.Channel, sIRCMessage.ServerName);
			if(text.Length < 2)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
				return;
			}

			if(DateTime.Now.Minute < 10)
				sSendMessage.SendChatMessage(sIRCMessage, text[0], DateTime.Now.Hour, DateTime.Now.Minute);
			else
				sSendMessage.SendChatMessage(sIRCMessage, text[1], DateTime.Now.Hour, DateTime.Now.Minute);
		}

		protected void HandleDate(IRCMessage sIRCMessage)
		{
			var text = sLManager.GetCommandTexts("date", sIRCMessage.Channel, sIRCMessage.ServerName);
			if(text.Length < 4)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
				return;
			}

			int month = DateTime.Now.Month;
			int day = DateTime.Now.Day;
			string nameday = sUtilities.NameDay(sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName));

			if(month < 10)
			{
				if(day < 10)
					sSendMessage.SendChatMessage(sIRCMessage, text[0], DateTime.Now.Year, month, day, nameday);
				else
					sSendMessage.SendChatMessage(sIRCMessage, text[1], DateTime.Now.Year, month, day, nameday);
			}
			else
			{
				if(day < 10)
					sSendMessage.SendChatMessage(sIRCMessage, text[2], DateTime.Now.Year, month, day, nameday);
				else
					sSendMessage.SendChatMessage(sIRCMessage, text[3], DateTime.Now.Year, month, day, nameday);
			}
		}

		protected void HandleIrc(IRCMessage sIRCMessage)
		{
			var text = sLManager.GetCommandTexts("irc", sIRCMessage.Channel, sIRCMessage.ServerName);
			if(text.Length < 2)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
				return;
			}

			if(sIRCMessage.Info.Length == 4)
			{
				var db = SchumixBase.DManager.Query("SELECT Command FROM irc_commands WHERE Language = '{0}'", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName));
				if(!db.IsNull())
				{
					string commands = string.Empty;

					foreach(DataRow row in db.Rows)
						commands += " | " + row["Command"].ToString();

					if(commands == string.Empty)
						sSendMessage.SendChatMessage(sIRCMessage, text[0], "none");
					else
						sSendMessage.SendChatMessage(sIRCMessage, text[0], commands.Remove(0, 3, " | "));
				}
				else
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel, sIRCMessage.ServerName));
			}
			else
			{
				var db = SchumixBase.DManager.QueryFirstRow("SELECT Text FROM irc_commands WHERE Command = '{0}' AND Language = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[4]), sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName));
				if(!db.IsNull())
					sSendMessage.SendChatMessage(sIRCMessage, db["Text"].ToString());
				else
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
			}
		}

		protected void HandleWhois(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoWhoisName", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			WhoisPrivmsg = sIRCMessage.Channel;
			sSender.Whois(sIRCMessage.Info[4]);
		}

		protected void HandleWarning(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host) && sIRCMessage.Info[4].Length > 0 && sIRCMessage.Info[4].Substring(0, 1) == "#")
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoOperator", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			if(sIRCMessage.Info.Length == 5)
				sSendMessage.SendChatMessage(sIRCMessage.MessageType, sIRCMessage.Info[4], sLManager.GetCommandText("warning", sIRCMessage.Channel, sIRCMessage.ServerName), sIRCMessage.Nick, sIRCMessage.Channel);
			else if(sIRCMessage.Info.Length >= 6)
				sSendMessage.SendChatMessage(sIRCMessage.MessageType, sIRCMessage.Info[4], "{0}", sIRCMessage.Info.SplitToString(5, SchumixBase.Space));
		}

		protected void HandleGoogle(IRCMessage sIRCMessage)
		{
			var text = sLManager.GetCommandTexts("google", sIRCMessage.Channel, sIRCMessage.ServerName);
			if(text.Length < 4)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
				return;
			}

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoGoogleText", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			string url = sUtilities.GetUrl("http://ajax.googleapis.com/ajax/services/search/web?v=1.0&q=", sIRCMessage.Info.SplitToString(4, SchumixBase.Space));
			var google = new GoogleWebResponseData();
			google = JsonHelper.Deserialise<GoogleWebResponseData>(url);

			if(google.ResultSet.Results.Length > 0)
			{
				sSendMessage.SendChatMessage(sIRCMessage, text[2], google.ResultSet.Results[0].TitleNoFormatting);
				sSendMessage.SendChatMessage(sIRCMessage, text[3], google.ResultSet.Results[0].UnescapedUrl);
			}
			else
			{
				sSendMessage.SendChatMessage(sIRCMessage, text[0]);
				sSendMessage.SendChatMessage(sIRCMessage, text[1]);
			}
		}

		protected void HandleTranslate(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoTranslateLanguage", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			if(!sIRCMessage.Info[4].Contains("|"))
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoTranslateLanguage", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			if(sIRCMessage.Info.Length < 6)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoTranslateText", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			string url = sUtilities.GetUrl("http://www.google.com/translate_t?hl=en&ie=UTF8&text=", sIRCMessage.Info.SplitToString(5, SchumixBase.Space), "&langpair=" + sIRCMessage.Info[4]);
			var Regex = new Regex("onmouseover=\"this.style.backgroundColor='#ebeff9'\" onmouseout=\"this.style.backgroundColor='#fff'\">(?<text>.+)</span></span></div></div>");

			if(!Regex.IsMatch(url))
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("translate", sIRCMessage.Channel, sIRCMessage.ServerName));
			else
				sSendMessage.SendChatMessage(sIRCMessage, "{0}", Regex.Match(url).Groups["text"].ToString());
		}

		protected void HandleOnline(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			IsOnline = true;
			OnlinePrivmsg = sIRCMessage.Channel;
			sSender.NickServInfo(sIRCMessage.Info[4]);
		}
	}
}