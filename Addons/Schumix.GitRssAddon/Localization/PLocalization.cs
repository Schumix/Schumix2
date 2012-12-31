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

namespace Schumix.GitRssAddon.Localization
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

		public string RssConfig(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Rss beállításai betöltve.";
					else if(Locale == "enUS")
						return "Loaded the Rss settings.";
					else
						return "Loaded the Rss settings.";
				}
				default:
					return string.Empty;
			}
		}

		public string Exception(string Name)
		{
			switch(Name)
			{
				case "Error":
				{
					if(Locale == "huHU")
						return "[{0} {1}] Meghibásodás részletei: {2}";
					else if(Locale == "enUS")
						return "[{0} {1}] Failure details: {2}";
					else
						return "[{0} {1}] Failure details: {2}";
				}
				case "Error2":
				{
					if(Locale == "huHU")
						return "[{0} {1}] Végzetes meghibásodás részletei: {2}";
					else if(Locale == "enUS")
						return "[{0} {1}] Fatal failure details: {2}";
					else
						return "[{0} {1}] Fatal failure details: {2}";
				}
				default:
					return string.Empty;
			}
		}

		public string GitRss(string Name)
		{
			return GitRss(Name, Locale);
		}

		public string GitRss(string Name, string Language)
		{
			switch(Name)
			{
				case "github":
				{
					if(Language == "huHU")
						return "3{0} 7{1} Revision: 10{2} beküldte: {3}";
					else if(Language == "enUS")
						return "3{0} 7{1} Revision: 10{2} by {3}";
					else
						return "3{0} 7{1} Revision: 10{2} by {3}";
				}
				case "github2":
				{
					if(Language == "huHU")
						return "3{0} Infó: {1}";
					else if(Language == "enUS")
						return "3{0} Info: {1}";
					else
						return "3{0} Info: {1}";
				}
				case "gitweb":
				{
					if(Language == "huHU")
						return "3{0} 7{1} Revision: 10{2} beküldte: {3}";
					else if(Language == "enUS")
						return "3{0} 7{1} Revision: 10{2} by {3}";
					else
						return "3{0} 7{1} Revision: 10{2} by {3}";
				}
				case "gitweb2":
				{
					if(Language == "huHU")
						return "3{0} Infó: {1}";
					else if(Language == "enUS")
						return "3{0} Info: {1}";
					else
						return "3{0} Info: {1}";
				}
				default:
					return string.Empty;
			}
		}

		public string GitRssAddon(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "{0}: {1} rss került betöltésre.";
					else if(Locale == "enUS")
						return "{0}: {1} rss loaded.";
					else
						return "{0}: {1} rss loaded.";
				}
				case "Text2":
				{
					if(Locale == "huHU")
						return "{0}: Üres az adatbázis!";
					else if(Locale == "enUS")
						return "{0}: Empty database!";
					else
						return "{0}: Empty database!";
				}
				default:
					return string.Empty;
			}
		}
	}
}