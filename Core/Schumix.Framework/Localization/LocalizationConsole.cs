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
using Schumix.Framework;
using Schumix.Framework.Extensions;

namespace Schumix.Framework.Localization
{
	public sealed class LocalizationConsole
	{
		public string Locale { get; set; }
		private LocalizationConsole() {}

		public string MainText(string Name)
		{
			switch(Name)
			{
				case "StartText":
				{
					if(Locale == "huHU")
						return "A program leállításához használd a <Ctrl+C> vagy <quit> parancsot!\n";
					else if(Locale == "enUS")
						return "To shut down the program use the <Ctrl+C> or the <quit> command!\n";
					else
						return "To shut down the program use the <Ctrl+C> or the <quit> command!\n";
				}
				case "StartText2":
				{
					if(Locale == "huHU")
						return "Programot készítette Megax, Jackneill. Schumix Verzió: {0} http://megaxx.info";
					else if(Locale == "enUS")
						return "Programmed by Megax, Jackneill. Schumix Version: {0} http://megaxx.info";
					else
						return "Programmed by Megax, Jackneill. Schumix Version: {0} http://megaxx.info";
				}
				case "StartText3":
				{
					if(Locale == "huHU")
						return "Rendszer indul...";
					else if(Locale == "enUS")
						return "System starting...";
					else
						return "System starting...";
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
						return "Meghibásodás részletei: {0}";
					else if(Locale == "enUS")
						return "Failure details: {0}";
					else
						return "Failure";
				}
				default:
					return string.Empty;
			}
		}

		public string Translations(string Name)
		{
			return Translations(Name, Locale);
		}

		public string Translations(string Name, string Language)
		{
			switch(Name)
			{
				case "NoFound":
				{
					if(Language == "huHU")
						return "Nem találhatóak fordítások!";
					else if(Language == "enUS")
						return "No translations found!";
					else
						return "No translations found!";
				}
				case "NoFound2":
				{
					if(Language == "huHU")
						return "Nem található néhány fordítás!";
					else if(Language == "enUS")
						return "Any translations did not find!";
					else
						return "Any translations did not find!";
				}
				default:
					return string.Empty;
			}
		}

		public string SchumixBot(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "SchumixBot sikeresen elindult.";
					else if(Locale == "enUS")
						return "Successfully started SchumixBot.";
					else
						return "Successfully started SchumixBot.";
				}
				case "Text2":
				{
					if(Locale == "huHU")
						return "Network indul...";
					else if(Locale == "enUS")
						return "Network starting...";
					else
						return "Network starting...";
				}
				case "Text3":
				{
					if(Locale == "huHU")
						return "Console indul...";
					else if(Locale == "enUS")
						return "Console starting...";
					else
						return "Console starting...";
				}
				default:
					return string.Empty;
			}
		}

		public string Console(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Console sikeresen elindult.";
					else if(Locale == "enUS")
						return "Successfully started the Console.";
					else
						return "Successfully started the Console.";
				}
				case "Text2":
				{
					if(Locale == "huHU")
						return "Console parancs olvasója indul...";
					else if(Locale == "enUS")
						return "Console reader starting...";
					else
						return "Console reader starting...";
				}
				case "Text3":
				{
					if(Locale == "huHU")
						return "Console parancs olvasó része elindult.";
					else if(Locale == "enUS")
						return "Successfully started the Console reader.";
					else
						return "Successfully started the Console reader.";
				}
				default:
					return string.Empty;
			}
		}

		public string CCommandManager(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "CCommandManager sikeresen elindult.";
					else if(Locale == "enUS")
						return "Successfully started the CCommandManager.";
					else
						return "Successfully started the CCommandManager.";
				}
				case "Text2":
				{
					if(Locale == "huHU")
						return "Összes Command handler regisztrálásra került.";
					else if(Locale == "enUS")
						return "All Command Handler are registered.";
					else
						return "All Command Handler are registered.";
				}
				default:
					return string.Empty;
			}
		}

		public string Log(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "\nIndulási időpont: [{0}. {1}. {2}. {3}:{4}:{5}]\n";
					else if(Locale == "enUS")
						return "Started time: [{0}. {1}. {2}. {3}:{4}:{5}]\n";
					else
						return "Started time: [{0}. {1}. {2}. {3}:{4}:{5}]\n";
				}
				default:
					return string.Empty;
			}
		}

		public string DatabaseManager(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Adatbázis betöltése elindult.";
					else if(Locale == "enUS")
						return "Started the database loading.";
					else
						return "Started the database loading.";
				}
				case "Text2":
				{
					if(Locale == "huHU")
						return "Adatbázis fajtájának kiválasztása folyamatban.";
					else if(Locale == "enUS")
						return "Selecting the Database.";
					else
						return "Selecting the Database.";
				}
				case "Text3":
				{
					if(Locale == "huHU")
						return "Nincs az adatbázis típusa kiválasztva!";
					else if(Locale == "enUS")
						return "Database type's is not selected!";
					else
						return "Database type's is not selected!";
				}
				case "Text4":
				{
					if(Locale == "huHU")
						return "Két adatbázis van kiválasztva!";
					else if(Locale == "enUS")
						return "2 Database are selected!";
					else
						return "2 Database are selected!";
				}
				default:
					return string.Empty;
			}
		}

		public string MySql(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Hiba történt az adatbázishoz való kapcsolodás során!";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text2":
				{
					if(Locale == "huHU")
						return "MySql adatbázishoz sikeres a kapcsolodás.";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text3":
				{
					if(Locale == "huHU")
						return "Query hiba: {0}";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				default:
					return string.Empty;
			}
		}

		public string SQLite(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Hiba történt az adatbázishoz való kapcsolodás során!";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text2":
				{
					if(Locale == "huHU")
						return "SQLite adatbázishoz sikeres a kapcsolodás.";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text3":
				{
					if(Locale == "huHU")
						return "Query hiba: {0}";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				default:
					return string.Empty;
			}
		}

		public string Utilities(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Ellenőrzése: {0}";
					else if(Locale == "enUS")
						return "Checking: {0}";
					else
						return "Checking: {0}";
				}
				default:
					return string.Empty;
			}
		}

		public string SchumixBase(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Timer indul...";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text2":
				{
					if(Locale == "huHU")
						return "Mysql indul...";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text3":
				{
					if(Locale == "huHU")
						return "Az adatbázishoz sikeres a kapcsolodás.";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text4":
				{
					if(Locale == "huHU")
						return "Mester csatorna frissitve lett erre: {0}";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text5":
				{
					if(Locale == "huHU")
						return "AddonManager betöltése folyamatban...";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				default:
					return string.Empty;
			}
		}

		public string Timer(string Name)
		{
			return Timer(Name, Locale);
		}

		public string Timer(string Name, string Language)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Timer sikeresen elindult.";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text2":
				{
					if(Locale == "huHU")
						return "Program indulási időpontja mentésre került.";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text3":
				{
					if(Locale == "huHU")
						return "A program {0}ms alatt indult el.";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Uptime":
				{
					if(Locale == "huHU")
						return "{0} nap, {1} óra, {2} perc, {3} másodperc.";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				default:
					return string.Empty;
			}
		}

		public string AddonManager(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Addonok betöltése innét: {0}";
					else if(Locale == "enUS")
						return "Loading addons from: {0}";
					else
						return "Loading addons from: {0}";
				}
				case "Text2":
				{
					if(Locale == "huHU")
						return "Betöltött plugin: {0} {1} készítette {2} ({3})";
					else if(Locale == "enUS")
						return "Loaded plugin: {0} {1} by {2} ({3})";
					else
						return "Loaded plugin: {0} {1} by {2} ({3})";
				}
				case "Text3":
				{
					if(Locale == "huHU")
						return "Tiltott pluginok: {0}";
					else if(Locale == "enUS")
						return "Ignoring plugins: {0}";
					else
						return "Ignoring plugins: {0}";
				}
				case "Text4":
				{
					if(Locale == "huHU")
						return "Hiba egy könyvtár betöltése közben! Részletek: {0}";
					else if(Locale == "enUS")
						return "Error while loading one of directories! Detail: {0}";
					else
						return "Error while loading one of directories! Detail: {0}";
				}
				case "Text5":
				{
					if(Locale == "huHU")
						return "Pluginok leválasztva.";
					else if(Locale == "enUS")
						return "Unload plugins.";
					else
						return "Unload plugins.";
				}
				default:
					return string.Empty;
			}
		}

		public string Config(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Program leállítása!";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text2":
				{
					if(Locale == "huHU")
						return "Kérlek töltsed ki a konfigot!";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text3":
				{
					if(Locale == "huHU")
						return "Konfig fájl betöltése.";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text4":
				{
					if(Locale == "huHU")
						return "Konfig adatbázis betöltve.";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text5":
				{
					if(Locale == "huHU")
						return "Nincs konfig fájl!";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text6":
				{
					if(Locale == "huHU")
						return "Elkészítése folyamatban...";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text7":
				{
					if(Locale == "huHU")
						return "Konfig fájl elkészült!";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text8":
				{
					if(Locale == "huHU")
						return "Meghibásodás az xml írása során. Részletek: {0}";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				default:
					return string.Empty;
			}
		}

		public string IRCConfig(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Irc beállításai betöltve.";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				default:
					return string.Empty;
			}
		}

		public string MySqlConfig(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "MySql beállításai betöltve.";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				default:
					return string.Empty;
			}
		}

		public string SQLiteConfig(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "SQLite beállításai betöltve.";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				default:
					return string.Empty;
			}
		}

		public string AddonsConfig(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Addons beállításai betöltve.";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				default:
					return string.Empty;
			}
		}

		public string LocalizationConfig(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Localization beállításai betöltve.";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				default:
					return string.Empty;
			}
		}
	}
}