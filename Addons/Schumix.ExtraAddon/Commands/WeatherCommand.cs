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
using CGurus.Weather.WundergroundAPI;
using CGurus.Weather.WundergroundAPI.Models;

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
				sSendMessage.SendChatMessage(sIRCMessage, "Ország!");
				return;
			}

			if(sIRCMessage.Info.Length < 6  && sIRCMessage.Info[4].ToLower() != "home")
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoCityName", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			bool home = false;
			string language = string.Empty;

			switch(sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName))
			{
				case "huHU":
					language = "HU";
					break;
				case "enUS":
				case "enGB":
					language = "US";
					break;
				default:
					language = "US";
					break;
			}

			if(sIRCMessage.Info[4].ToLower() == "home")
				home = true;

			try
			{
				ForecastData source = null;
				string country = sIRCMessage.Info[4];
				WApi wApi = new WApi(WeatherConfig.Key);

				if(home)
					source = wApi.GetForecast(WeatherConfig.Country, WeatherConfig.City, language);
				else
					source = wApi.GetForecast(country, sIRCMessage.Info.SplitToString(5, SchumixBase.Space).Trim(), language);

				var day = source.Forecast.Txt_Forecast.ForecastDay[0].FctText_Metric;
				var night = source.Forecast.Txt_Forecast.ForecastDay[1].FctText_Metric;

				if(home)
					sSendMessage.SendChatMessage(sIRCMessage, text[0]);
				else
					sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Info[4] + ", " + sIRCMessage.Info.SplitToString(5, SchumixBase.Space).Trim());

				sSendMessage.SendChatMessage(sIRCMessage, text[2], day);
				sSendMessage.SendChatMessage(sIRCMessage, text[3], night);
			}
			catch
			{
				sSendMessage.SendChatMessage(sIRCMessage, text[4]);
			}
		}
	}
}