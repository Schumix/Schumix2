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
						return "Failure details: {0}";
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
						return "\nStarted time: [{0}. {1}. {2}. {3}:{4}:{5}]\n";
					else
						return "\nStarted time: [{0}. {1}. {2}. {3}:{4}:{5}]\n";
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
						return "Error was handled when tried to connect to the database.";
					else
						return "Error was handled when tried to connect to the database.";
				}
				case "Text2":
				{
					if(Locale == "huHU")
						return "MySql adatbázishoz sikeres a kapcsolodás.";
					else if(Locale == "enUS")
						return "Successfully connected to the MySql database.";
					else
						return "Successfully connected to the MySql database.";
				}
				case "Text3":
				{
					if(Locale == "huHU")
						return "Query hiba: {0}";
					else if(Locale == "enUS")
						return "Query error: {0}";
					else
						return "Query error: {0}";
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
						return "Error was handled when tried to connect to the database!";
					else
						return "Error was handled when tried to connect to the database!";
				}
				case "Text2":
				{
					if(Locale == "huHU")
						return "SQLite adatbázishoz sikeres a kapcsolodás.";
					else if(Locale == "enUS")
						return "Successfully connected to the SQLite database.";
					else
						return "Successfully connected to the SQLite database.";
				}
				case "Text3":
				{
					if(Locale == "huHU")
						return "Query hiba: {0}";
					else if(Locale == "enUS")
						return "Query error: {0}";
					else
						return "Query error: {0}";
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
						return "Timer is starting...";
					else
						return "Timer is starting...";
				}
				case "Text2":
				{
					if(Locale == "huHU")
						return "Mysql indul...";
					else if(Locale == "enUS")
						return "MySql is starting...";
					else
						return "MySql is starting...";
				}
				case "Text3":
				{
					if(Locale == "huHU")
						return "Az adatbázishoz sikeres a kapcsolodás.";
					else if(Locale == "enUS")
						return "Successfully connected to the database.";
					else
						return "Successfully connected to the database.";
				}
				case "Text4":
				{
					if(Locale == "huHU")
						return "Mester csatorna frissitve lett erre: {0}";
					else if(Locale == "enUS")
						return "The master channel is updated to: {0}";
					else
						return "The master channel is updated to: {0}";
				}
				case "Text5":
				{
					if(Locale == "huHU")
						return "AddonManager betöltése folyamatban...";
					else if(Locale == "enUS")
						return "AddonManager is loading...";
					else
						return "AddonManager is loading...";
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
						return "Successfully loaded the Timer.";
					else
						return "Successfully loaded the Timer.";
				}
				case "Text2":
				{
					if(Locale == "huHU")
						return "Program indulási időpontja mentésre került.";
					else if(Locale == "enUS")
						return "Successfully saved the Program's started time.";
					else
						return "Successfully saved the Program's started time.";
				}
				case "Text3":
				{
					if(Locale == "huHU")
						return "A program {0}ms alatt indult el.";
					else if(Locale == "enUS")
						return "The program is loaded under {0}ms.";
					else
						return "The program is loaded under {0}ms.";
				}
				case "Uptime":
				{
					if(Locale == "huHU")
						return "{0} nap, {1} óra, {2} perc, {3} másodperc.";
					else if(Locale == "enUS")
						return "{0} day(s), {1} hour(s), {2} min(s), {3} sec(s).";
					else
						return "{0} day(s), {1} hour(s), {2} min(s), {3} sec(s).";
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
						return "Program is shutting down!";
					else
						return "";
				}
				case "Text2":
				{
					if(Locale == "huHU")
						return "Kérlek töltsed ki a konfigot!";
					else if(Locale == "enUS")
						return "Please set up the Config file!";
					else
						return "Please set up the Config file!";
				}
				case "Text3":
				{
					if(Locale == "huHU")
						return "Konfig fájl betöltése.";
					else if(Locale == "enUS")
						return "Config file is loading.";
					else
						return "Config file is loading.";
				}
				case "Text4":
				{
					if(Locale == "huHU")
						return "Konfig adatbázis betöltve.";
					else if(Locale == "enUS")
						return "Config database is loading.";
					else
						return "Config database is loading.";
				}
				case "Text5":
				{
					if(Locale == "huHU")
						return "Nincs konfig fájl!";
					else if(Locale == "enUS")
						return "No such config file!";
					else
						return "No such config file!";
				}
				case "Text6":
				{
					if(Locale == "huHU")
						return "Elkészítése folyamatban...";
					else if(Locale == "enUS")
						return "Preparing...";
					else
						return "Preparing...";
				}
				case "Text7":
				{
					if(Locale == "huHU")
						return "Konfig fájl elkészült!";
					else if(Locale == "enUS")
						return "Config file is completed!";
					else
						return "Config file is completed!";
				}
				case "Text8":
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

		public string IRCConfig(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Irc beállításai betöltve.";
					else if(Locale == "enUS")
						return "Loaded the Irc settings.";
					else
						return "Loaded the Irc settings.";
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
						return "Loaded the MySql settings.";
					else
						return "Loaded the MySql settings.";
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
						return "Loaded the SQLite settings.";
					else
						return "Loaded the SQLite settings.";
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
						return "Loaded the Addons settings.";
					else
						return "Loaded the Addons settings.";
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
						return "Loaded the Localization settings.";
					else
						return "Loaded the Localization settings.";
				}
				default:
					return string.Empty;
			}
		}

		public string CommandManager(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "CommandManager sikeresen elindult.";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text2":
				{
					if(Locale == "huHU")
						return "Összes Command handler regisztrálásra került.";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				default:
					return string.Empty;
			}
		}

		public string ChannelInfo(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "ChannelList: Hibás lekérdezés!";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text2":
				{
					if(Locale == "huHU")
						return "FSelect: Hibás lekérdezés!";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text3":
				{
					if(Locale == "huHU")
						return "ChannelFunctionReload: Hibás lekérdezés!";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text4":
				{
					if(Locale == "huHU")
						return "ChannelListReload: Hibás lekérdezés!";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text5":
				{
					if(Locale == "huHU")
						return "Kapcsolódás a csatornákhoz...";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text6":
				{
					if(Locale == "huHU")
						return "JoinChannel: Hibás lekérdezés!";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text7":
				{
					if(Locale == "huHU")
						return "Sikeres kapcsolódás a csatornákhoz.";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text8":
				{
					if(Locale == "huHU")
						return "Néhány kapcsolódás sikertelen!";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text9":
				{
					if(Locale == "huHU")
						return "Letiltott csatornák: {0}";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				default:
					return string.Empty;
			}
		}

		public string NickServ(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Azonosító jelszó küldése a kiszolgálónak.";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text2":
				{
					if(Locale == "huHU")
						return "Azonosító jelszó hibás!";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text3":
				{
					if(Locale == "huHU")
						return "Azonosító már aktíválva van!";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text4":
				{
					if(Locale == "huHU")
						return "Azonosító jelszó elfogadva.";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				default:
					return string.Empty;
			}
		}

		public string HostServ(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Vhost bekapcsolásra került.";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text2":
				{
					if(Locale == "huHU")
						return "Vhost kikapcsolásra került.";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				default:
					return string.Empty;
			}
		}

		public string MessageHandler(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Sikeres kapcsolódás az irc kiszolgálóhoz.";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text2":
				{
					if(Locale == "huHU")
						return "Várakozás a kapcsolat feldolgozására.";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text3":
				{
					if(Locale == "huHU")
						return "Nincs megadva a a bot nick neve!";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text4":
				{
					if(Locale == "huHU")
						return "[SZERVER] ";
					else if(Locale == "enUS")
						return "[SERVER] ";
					else
						return "[SERVER] ";
				}
				case "Text5":
				{
					if(Locale == "huHU")
						return "Nem létező irc parancs\n";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text6":
				{
					if(Locale == "huHU")
						return "{0}-t már használja valaki!";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text7":
				{
					if(Locale == "huHU")
						return "Újra próbálom ezzel: {0}";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text8":
				{
					if(Locale == "huHU")
						return "{0}: csatornára való kapcsolódás letiltva!";
					else if(Locale == "enUS")
						return "{0}: channel ban!";
					else
						return "{0}: channel ban!";
				}
				case "Text9":
				{
					if(Locale == "huHU")
						return "{0}: ezen csatorna jelszó hibás!";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				default:
					return string.Empty;
			}
		}

		public string Network(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Network sikeresen elindult.";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text2":
				{
					if(Locale == "huHU")
						return "Kapcsolat létrehozása az irc szerverrel.";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text3":
				{
					if(Locale == "huHU")
						return "Opcodes thread indul...";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text4":
				{
					if(Locale == "huHU")
						return "Ping thread indul...";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text5":
				{
					if(Locale == "huHU")
						return "Összes IRC handler regisztrálásra került.";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text6":
				{
					if(Locale == "huHU")
						return "Kapcsolódás megindult ide: {0}";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text7":
				{
					if(Locale == "huHU")
						return "Kapcsolat bontva.";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text8":
				{
					if(Locale == "huHU")
						return "Kapcsolat bontásra került.";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text9":
				{
					if(Locale == "huHU")
						return "Újrakapcsolódás megindult ide: {0}";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text10":
				{
					if(Locale == "huHU")
						return "Végzetes hiba történt a kapcsolat létrehozásánál!";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text11":
				{
					if(Locale == "huHU")
						return "A kapcsolat sikeresen létrejött!";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text12":
				{
					if(Locale == "huHU")
						return "Hiba történt a kapcsolat létrehozásánál!";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text13":
				{
					if(Locale == "huHU")
						return "Felhasználói információk ellettek küldve.";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text14":
				{
					if(Locale == "huHU")
						return "A szál sikeresen elindult.";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text15":
				{
					if(Locale == "huHU")
						return "Elindult az irc adatok fogadása.";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text16":
				{
					if(Locale == "huHU")
						return "Nem jön információ az irc szerver felöl!";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text17":
				{
					if(Locale == "huHU")
						return "A program leáll!";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text18":
				{
					if(Locale == "huHU")
						return "Ismeretlen opcode kód: {0}";
					else if(Locale == "enUS")
						return "Received unhandled opcode: {0}";
					else
						return "Received unhandled opcode: {0}";
				}
				default:
					return string.Empty;
			}
		}
	}
}