/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013-2014 Schumix Team <http://schumix.eu/>
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
using Schumix.Framework;
using Schumix.Framework.Irc;
using Schumix.Framework.Extensions;
using Schumix.ExtraAddon.Config;

namespace Schumix.ExtraAddon.Commands
{
	partial class Functions
	{
		public void HandleWeather(IRCMessage sIRCMessage)
		{
			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;
			var text = sLManager.GetCommandTexts("weather", sIRCMessage.Channel, sIRCMessage.ServerName);
			if(text.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
				return;
			}

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoCityName", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			bool home = false;
			string url = string.Empty;
			string source = string.Empty;

			switch(sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName))
			{
				case "huHU":
					url = "http://hungarian.wunderground.com/cgi-bin/findweather/hdfForecast?query=";
					break;
				case "enUS":
				case "enGB":
					url = "http://www.wunderground.com/cgi-bin/findweather/hdfForecast?query=";
					break;
				default:
					url = "http://www.wunderground.com/cgi-bin/findweather/hdfForecast?query=";
					break;
			}

			if(sIRCMessage.Info[4].ToLower() == "home")
				home = true;

			try
			{
				if(home)
					source = sUtilities.GetUrl(string.Format("{0}{1}", url, WeatherConfig.City));
				else
					source = sUtilities.GetUrl(url, sIRCMessage.Info.SplitToString(4, SchumixBase.Space).Trim());

				string day = string.Empty;
				string night = string.Empty;
				source = source.Replace("\n\t\t", SchumixBase.Space.ToString());

				if(source.Contains("<td class=\"vaT\"><a href=\"\" class=\"iconSwitchMed\">"))
				{
					source = source.Remove(0, source.IndexOf("<td class=\"vaT\"><a href=\"\" class=\"iconSwitchMed\">") + "<td class=\"vaT\"><a href=\"\" class=\"iconSwitchMed\">".Length);
					source = source.Remove(0, source.IndexOf("<td class=\"vaT full\">") + "<td class=\"vaT full\">".Length);

					if(source.Contains(" <? END CHANCE OF PRECIP"))
						day = source.Substring(0, source.IndexOf(" <? END CHANCE OF PRECIP"));
					else
						day = source.Substring(0, source.IndexOf("</td>"));
				}
				else
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[4]);
					return;
				}

				if(source.Contains("<td class=\"vaT\"><a href=\"\" class=\"iconSwitchMed\">"))
				{
					source = source.Remove(0, source.IndexOf("<td class=\"vaT\"><a href=\"\" class=\"iconSwitchMed\">") + "<td class=\"vaT\"><a href=\"\" class=\"iconSwitchMed\">".Length);
					source = source.Remove(0, source.IndexOf("<td class=\"vaT full\">") + "<td class=\"vaT full\">".Length);

					if(source.Contains(" <? END CHANCE OF PRECIP"))
						night = source.Substring(0, source.IndexOf(" <? END CHANCE OF PRECIP"));
					else
						night = source.Substring(0, source.IndexOf("</td>"));
				}
				else
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[4]);
					return;
				}

				if(home)
					sSendMessage.SendChatMessage(sIRCMessage, text[0]);
				else
					sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Info.SplitToString(4, SchumixBase.Space).Trim());

				sSendMessage.SendChatMessage(sIRCMessage, text[2], day.Remove(0, 1, SchumixBase.Space));
				sSendMessage.SendChatMessage(sIRCMessage, text[3], night.Remove(0, 1, SchumixBase.Space));
			}
			catch
			{
				sSendMessage.SendChatMessage(sIRCMessage, text[4]);
			}
		}
	}
}