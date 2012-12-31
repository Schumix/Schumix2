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

namespace Schumix.ExtraAddon.Localization
{
	sealed class PLocalization
	{
		public string Locale { get; set; }
		private PLocalization() {}

		public string Config(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Konfig fájl betöltése.";
					else if(Locale == "enUS")
						return "Config file is loading.";
					else
						return "Config file is loading.";
				}
				case "Text2":
				{
					if(Locale == "huHU")
						return "Konfig adatbázis betöltve.";
					else if(Locale == "enUS")
						return "Config database is loading.";
					else
						return "Config database is loading.";
				}
				case "Text3":
				{
					if(Locale == "huHU")
						return "Nincs konfig fájl!";
					else if(Locale == "enUS")
						return "No such config file!";
					else
						return "No such config file!";
				}
				case "Text4":
				{
					if(Locale == "huHU")
						return "Elkészítése folyamatban...";
					else if(Locale == "enUS")
						return "Preparing...";
					else
						return "Preparing...";
				}
				case "Text5":
				{
					if(Locale == "huHU")
						return "Konfig fájl elkészült!";
					else if(Locale == "enUS")
						return "Config file is completed!";
					else
						return "Config file is completed!";
				}
				case "Text6":
				{
					if(Locale == "huHU")
						return "Meghibásodás az xml írása során. Részletek: {0}";
					else if(Locale == "enUS")
						return "Failure was handled during the xml writing. Details: {0}";
					else
						return "Failure was handled during the xml writing. Details: {0}";
				}
				default:
					return string.Empty;
			}
		}

		public string ModeConfig(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Mode beállításai betöltve.";
					else if(Locale == "enUS")
						return "Loaded the Mode settings.";
					else
						return "Loaded the Mode settings.";
				}
				default:
					return string.Empty;
			}
		}

		public string WeatherConfig(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Weather beállításai betöltve.";
					else if(Locale == "enUS")
						return "Loaded the Weather settings.";
					else
						return "Loaded the Weather settings.";
				}
				default:
					return string.Empty;
			}
		}

		public string WolframAlphaConfig(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "WolframAlpha beállításai betöltve.";
					else if(Locale == "enUS")
						return "Loaded the WolframAlpha settings.";
					else
						return "Loaded the WolframAlpha settings.";
				}
				default:
					return string.Empty;
			}
		}

		public string WebHelper(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Kivételt dobott letöltése közben a web title: {0}";
					else if(Locale == "enUS")
						return "Exception thrown while fetching web title: {0}";
					else
						return "Exception thrown while fetching web title: {0}";
				}
				default:
					return string.Empty;
			}
		}

		public string ExtraAddon(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Érvénytelen weboldal cím: {0}";
					else if(Locale == "enUS")
						return "Invalid webpage address: {0}";
					else
						return "Invalid webpage address: {0}";
				}
				case "Text2":
				{
					if(Locale == "huHU")
						return "30 napnál régebbi üzenetek törlése a message táblából megindult.";
					else if(Locale == "enUS")
						return "The deleting of messages that older than 30 days have been started.";
					else
						return "The deleting of messages that older than 30 days have been started.";
				}
				case "Text3":
				{
					if(Locale == "huHU")
						return "Üzenetek törlése befejezve.";
					else if(Locale == "enUS")
						return "Message deletion has been ended.";
					else
						return "Message deletion has been ended.";
				}
				default:
					return string.Empty;
			}
		}

		public string YoutubeTitle(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Kivételt dobott letöltése közben a youtube title: {0}";
					else if(Locale == "enUS")
						return "Exception thrown while fetching youtube title: {0}";
					else
						return "Exception thrown while fetching youtube title: {0}";
				}
				default:
					return string.Empty;
			}
		}
	}
}