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
using Schumix.Framework.Config;

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
						return "A program leállításához használd a <Ctrl+C> vagy <quit> parancsot!";
					else if(Locale == "enUS")
						return "To shut down the program use the <Ctrl+C> or the <quit> command!";
					else
						return "To shut down the program use the <Ctrl+C> or the <quit> command!";
				}
				case "StartText2":
				{
					if(Locale == "huHU")
						return "Schumix Verzió: {0}";
					else if(Locale == "enUS")
						return "Schumix Version: {0}";
					else
						return "Schumix Version: {0}";
				}
				case "StartText2-2":
				{
					if(Locale == "huHU")
						return "Weboldal: {0}";
					else if(Locale == "enUS")
						return "Website: {0}";
					else
						return "Website: {0}";
				}
				case "StartText2-3":
				{
					if(Locale == "huHU")
						return "Programot készítette: {0}";
					else if(Locale == "enUS")
						return "Programmed by: {0}";
					else
						return "Programmed by: {0}";
				}
				case "StartText2-4":
				{
					if(Locale == "huHU")
						return "Fejlesztők: {0}";
					else if(Locale == "enUS")
						return "Developers: {0}";
					else
						return "Developers: {0}";
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
				case "StartText4":
				{
					if(Locale == "huHU")
						return "Nem kezelt kivétel keletkezett. ({0})";
					else if(Locale == "enUS")
						return "An unhandled exception has been thrown. ({0})";
					else
						return "An unhandled exception has been thrown. ({0})";
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
				case "Text4":
				{
					if(Locale == "huHU")
						return "A program leáll!";
					else if(Locale == "enUS")
						return "Program shutting down!";
					else
						return "Program shutting down!";
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
				case "Text4":
				{
					if(Locale == "huHU")
						return "A program leáll!";
					else if(Locale == "enUS")
						return "Program shutting down!";
					else
						return "Program shutting down!";
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
				case "Text6":
				{
					if(Locale == "huHU")
						return "Kapcsolat kezdeményezése.";
					else if(Locale == "enUS")
						return "Initiating connection.";
					else
						return "Initiating connection.";
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
					if(Language == "huHU")
						return "{0} nap, {1} óra, {2} perc, {3} másodperc.";
					else if(Language == "enUS")
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
						return "Program is shutting down!";
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
				case "Text9":
				{
					if(Locale == "huHU")
						return "Schumixok indítása...";
					else if(Locale == "enUS")
						return "Schumixs starting...";
					else
						return "Schumixs starting...";
				}
				case "Text10":
				{
					if(Locale == "huHU")
						return "Schumixok száma: {0}";
					else if(Locale == "enUS")
						return "Schumixs number: {0}";
					else
						return "Schumixs number: {0}";
				}
				case "Text11":
				{
					if(Locale == "huHU")
						return "Nincs betöltendő Schumix!";
					else if(Locale == "enUS")
						return "There is no load of Schumix!";
					else
						return "There is no load of Schumix!";
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

		public string ScriptsConfig(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Scripts beállításai betöltve.";
					else if(Locale == "enUS")
						return "Loaded the Scripts settings.";
					else
						return "Loaded the Scripts settings.";
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

		public string UpdateConfig(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Update beállításai betöltve.";
					else if(Locale == "enUS")
						return "Loaded the Update settings.";
					else
						return "Loaded the Update settings.";
				}
				default:
					return string.Empty;
			}
		}

		public string ServerConfig(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Server beállításai betöltve.";
					else if(Locale == "enUS")
						return "Loaded the Server settings.";
					else
						return "Loaded the Server settings.";
				}
				default:
					return string.Empty;
			}
		}

		public string ServerConfigs(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Server beállításai betöltve.";
					else if(Locale == "enUS")
						return "Loaded the Server settings.";
					else
						return "Loaded the Server settings.";
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
						return "Successfully started the CommandManager.";
					else
						return "Successfully started the CommandManager.";
				}
				case "Text2":
				{
					if(Locale == "huHU")
						return "Összes Command handler regisztrálásra került.";
					else if(Locale == "enUS")
						return "Successfuly registered all of Command Handlers.";
					else
						return "Successfuly registered all of Command Handlers.";
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
						return "ChannelList: Failure request!";
					else
						return "ChannelList: Failure request!";
				}
				/*case "Text2":
				{
					if(Locale == "huHU")
						return "FSelect: Hibás lekérdezés!";
					else if(Locale == "enUS")
						return "FSelect: Failure reuqest!";
					else
						return "FSelect: Failure reuqest!";
				}*/
				case "Text3":
				{
					if(Locale == "huHU")
						return "ChannelFunctionReload: Hibás lekérdezés!";
					else if(Locale == "enUS")
						return "ChannelFunctionReload: Failre request!";
					else
						return "ChannelFunctionReload: Failre request!";
				}
				case "Text4":
				{
					if(Locale == "huHU")
						return "ChannelListReload: Hibás lekérdezés!";
					else if(Locale == "enUS")
						return "ChannelListReload: Failre request!";
					else
						return "ChannelListReload: Failre request!";
				}
				case "Text5":
				{
					if(Locale == "huHU")
						return "Kapcsolódás a csatornákhoz...";
					else if(Locale == "enUS")
						return "Connecting to channels...";
					else
						return "Connecting to channels...";
				}
				case "Text6":
				{
					if(Locale == "huHU")
						return "JoinChannel: Hibás lekérdezés!";
					else if(Locale == "enUS")
						return "JoinChannel: Failure request!";
					else
						return "JoinChannel: Failure request!";
				}
				case "Text7":
				{
					if(Locale == "huHU")
						return "Sikeres kapcsolódás a csatornákhoz.";
					else if(Locale == "enUS")
						return "Successfully connected to channels.";
					else
						return "Successfully connected to channels.";
				}
				case "Text8":
				{
					if(Locale == "huHU")
						return "Néhány kapcsolódás sikertelen!";
					else if(Locale == "enUS")
						return "Some connection unsuccessful.";
					else
						return "Some connection unsuccessful.";
				}
				case "Text9":
				{
					if(Locale == "huHU")
						return "Letiltott csatornák: {0}";
					else if(Locale == "enUS")
						return "Banned channels: {0}";
					else
						return "Banned channels: {0}";
				}
				case "Text10":
				{
					if(Locale == "huHU")
						return "Konfig által letiltva!";
					else if(Locale == "enUS")
						return "Disabled by Config!";
					else
						return "Disabled by Config!";
				}
				case "Text11":
				{
					if(Locale == "huHU")
						return "FunctionReload: Hibás lekérdezés!";
					else if(Locale == "enUS")
						return "FunctionReload: Failre request!";
					else
						return "FunctionReload: Failre request!";
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
						return "Sending nickserv identify.";
					else
						return "Sending nickserv identify.";
				}
				case "Text2":
				{
					if(Locale == "huHU")
						return "Azonosító jelszó hibás!";
					else if(Locale == "enUS")
						return "Bad identify password!";
					else
						return "Bad identify password!";
				}
				case "Text3":
				{
					if(Locale == "huHU")
						return "Azonosító már aktiválva van!";
					else if(Locale == "enUS")
						return "Already identified!";
					else
						return "Already identified!";
				}
				case "Text4":
				{
					if(Locale == "huHU")
						return "Azonosító jelszó elfogadva.";
					else if(Locale == "enUS")
						return "Identify password accepted!";
					else
						return "Identify password accepted!";
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
						return "Vhost is ON.";
					else
						return "Vhost is ON.";
				}
				case "Text2":
				{
					if(Locale == "huHU")
						return "Vhost kikapcsolásra került.";
					else if(Locale == "enUS")
						return "Vhost is OFF.";
					else
						return "Vhost is OFF.";
				}
				default:
					return string.Empty;
			}
		}

		public string MessageHandler(string Name)
		{
			return MessageHandler(Name, Locale);
		}

		public string MessageHandler(string Name, string Language)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Language == "huHU")
						return "Sikeres kapcsolódás az irc kiszolgálóhoz.";
					else if(Language == "enUS")
						return "Successfully connected to IRC server.";
					else
						return "Successfully connected to IRC server.";
				}
				case "Text2":
				{
					if(Language == "huHU")
						return "Várakozás a kapcsolat feldolgozására.";
					else if(Language == "enUS")
						return "Waiting for connection processing.";
					else
						return "Waiting for connection processing.";
				}
				case "Text3":
				{
					if(Language == "huHU")
						return "Nincs megadva a bot nick neve!";
					else if(Language == "enUS")
						return "No such Bot's nickname.";
					else
						return "No such Bot's nickname.";
				}
				case "Text4":
				{
					if(Language == "huHU")
						return "[SZERVER] ";
					else if(Language == "enUS")
						return "[SERVER] ";
					else
						return "[SERVER] ";
				}
				case "Text5":
				{
					if(Language == "huHU")
						return "Nem létező irc parancs\n";
					else if(Language == "enUS")
						return "No such irc command.\n";
					else
						return "No such irc command.\n";
				}
				case "Text6":
				{
					if(Language == "huHU")
						return "{0}-t már használja valaki!";
					else if(Language == "enUS")
						return "{0} already in use.";
					else
						return "{0} already in use.";
				}
				case "Text7":
				{
					if(Language == "huHU")
						return "Újra próbálom ezzel: {0}";
					else if(Language == "enUS")
						return "Retrying with: {0}";
					else
						return "Retrying with: {0}";
				}
				case "Text8":
				{
					if(Language == "huHU")
						return "Csatornára való kapcsolódás letiltva: {0}";
					else if(Language == "enUS")
						return "Banned channel: {0}";
					else
						return "Banned channel: {0}";
				}
				case "Text8-1":
				{
					if(Language == "huHU")
						return "Csatornára való kapcsolódás letiltva!";
					else if(Language == "enUS")
						return "Banned channel!";
					else
						return "Banned channel!";
				}
				case "Text9":
				{
					if(Language == "huHU")
						return "Ezen csatorna jelszava hibás: {0}";
					else if(Language == "enUS")
						return "Bad password for channel: {0}";
					else
						return "Bad password for channel: {0}";
				}
				case "Text9-1":
				{
					if(Language == "huHU")
						return "Csatorna jelszava hibás!";
					else if(Language == "enUS")
						return "Bad password for channel!";
					else
						return "Bad password for channel!";
				}
				case "Text10":
				{
					if(Language == "huHU")
						return "Nem regisztrált!";
					else if(Language == "enUS")
						return "You have not registered!";
					else
						return "You have not registered!";
				}
				case "Text11":
				{
					if(Language == "huHU")
						return "Jelenleg fent van.";
					else if(Language == "enUS")
						return "Currently online.";
					else
						return "Currently online.";
				}
				case "Text12":
				{
					if(Language == "huHU")
						return "Ez a név nincs beregisztrálva!";
					else if(Language == "enUS")
						return "This nickname isn't registered!";
					else
						return "This nickname isn't registered!";
				}
				case "Text13":
				{
					if(Language == "huHU")
						return "Nincs fent ekkortól: {0}";
					else if(Language == "enUS")
						return "Last seen time: {0}";
					else
						return "Last seen time: {0}";
				}
				case "Text14":
				{
					if(Language == "huHU")
						return "Foglalt ez a nick!";
					else if(Language == "enUS")
						return "Nickname is already in use!";
					else
						return "Nickname is already in use!";
				}
				case "Text15":
				{
					if(Language == "huHU")
						return "Hibás a megadott nick!";
					else if(Language == "enUS")
						return "Erroneous Nickname!";
					else
						return "Erroneous Nickname!";
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
						return "Successfully started the Network.";
					else
						return "Successfully started the Network.";
				}
				case "Text2":
				{
					if(Locale == "huHU")
						return "Kapcsolat létrehozása az irc szerverrel.";
					else if(Locale == "enUS")
						return "Establishing connection with irc server.";
					else
						return "Establishing connection with irc server.";
				}
				case "Text3":
				{
					if(Locale == "huHU")
						return "Opcodes thread indul...";
					else if(Locale == "enUS")
						return "Opcodes thread started...";
					else
						return "Opcodes thread started...";
				}
				case "Text4":
				{
					if(Locale == "huHU")
						return "Ping thread indul...";
					else if(Locale == "enUS")
						return "Ping thread started...";
					else
						return "Ping thread started...";
				}
				case "Text5":
				{
					if(Locale == "huHU")
						return "Összes IRC handler regisztrálásra került.";
					else if(Locale == "enUS")
						return "All of IRC handlers are registered.";
					else
						return "All of IRC handlers are registered.";
				}
				case "Text6":
				{
					if(Locale == "huHU")
						return "Kapcsolódás megindult ide: {0}";
					else if(Locale == "enUS")
						return "Connection to: {0}";
					else
						return "Connection to: {0}";
				}
				case "Text7":
				{
					if(Locale == "huHU")
						return "Kapcsolat bontva.";
					else if(Locale == "enUS")
						return "Connection closed.";
					else
						return "Connection closed.";
				}
				case "Text8":
				{
					if(Locale == "huHU")
						return "Kapcsolat bontásra került.";
					else if(Locale == "enUS")
						return "Connection have been closed.";
					else
						return "Connection have been closed.";
				}
				case "Text9":
				{
					if(Locale == "huHU")
						return "Újrakapcsolódás megindult ide: {0}";
					else if(Locale == "enUS")
						return "Reconnection to: {0}";
					else
						return "Reconnection to: {0}";
				}
				case "Text10":
				{
					if(Locale == "huHU")
						return "Végzetes hiba történt a kapcsolat létrehozásánál!";
					else if(Locale == "enUS")
						return "Fatal error was happened while established the connection.";
					else
						return "Fatal error was happened while established the connection.";
				}
				case "Text11":
				{
					if(Locale == "huHU")
						return "A kapcsolat sikeresen létrejött!";
					else if(Locale == "enUS")
						return "Successfully established the connection.";
					else
						return "Successfully established the connection.";
				}
				case "Text12":
				{
					if(Locale == "huHU")
						return "Hiba történt a kapcsolat létrehozásánál!";
					else if(Locale == "enUS")
						return "Error was happened while established the connection.";
					else
						return "Error was happened while established the connection.";
				}
				case "Text13":
				{
					if(Locale == "huHU")
						return "Felhasználói információk ellettek küldve.";
					else if(Locale == "enUS")
						return "Users' datas are sent.";
					else
						return "Users' datas are sent.";
				}
				case "Text14":
				{
					if(Locale == "huHU")
						return "A szál sikeresen elindult.";
					else if(Locale == "enUS")
						return "Successfully started th thread.";
					else
						return "Successfully started th thread.";
				}
				case "Text15":
				{
					if(Locale == "huHU")
						return "Elindult az irc adatok fogadása.";
					else if(Locale == "enUS")
						return "Started the irc data receiving.";
					else
						return "Started the irc data receiving.";
				}
				case "Text16":
				{
					if(Locale == "huHU")
						return "Nem jön információ az irc szerver felöl!";
					else if(Locale == "enUS")
						return "Do not going data from irc server!";
					else
						return "Do not going data form irc server!";
				}
				case "Text17":
				{
					if(Locale == "huHU")
						return "A program leáll!";
					else if(Locale == "enUS")
						return "Program shutting down!";
					else
						return "Program shutting down!";
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

		public string Update(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Automatikus frissítés ki van kapcsolva.";
					else if(Locale == "enUS")
						return "Automatic updater is off.";
					else
						return "Automatic updater is off.";
				}
				case "Text2":
				{
					if(Locale == "huHU")
						return "Újabb stabil verzió keresése megindult.";
					else if(Locale == "enUS")
						return "Searching for new stable version.";
					else
						return "Searching for new stable version.";
				}
				case "Text3":
				{
					if(Locale == "huHU")
						return "Nincs frissebb verzió!";
					else if(Locale == "enUS")
						return "Nothing newer version!";
					else
						return "Nothing newer version!";
				}
				case "Text4":
				{
					if(Locale == "huHU")
						return "Új verziót találtam. Verzió: {0}";
					else if(Locale == "enUS")
						return "Found new version. Version: {0}";
					else
						return "Found new version. Version: {0}";
				}
				case "Text5":
				{
					if(Locale == "huHU")
						return "Új forrás letöltése.";
					else if(Locale == "enUS")
						return "Downloading new version.";
					else
						return "Downloading new version.";
				}
				case "Text6":
				{
					if(Locale == "huHU")
						return "Forrás sikeresen le lett töltve.";
					else if(Locale == "enUS")
						return "Successfully downloaded new version.";
					else
						return "Successfully downloaded new version.";
				}
				case "Text7":
				{
					if(Locale == "huHU")
						return "Letöltés sikertelen!";
					else if(Locale == "enUS")
						return "Downloading unsuccessful.";
					else
						return "Downloading unsuccessful.";
				}
				case "Text8":
				{
					if(Locale == "huHU")
						return "A frissítés befejeződött!";
					else if(Locale == "enUS")
						return "Updating successful.";
					else
						return "Updating successful.";
				}
				case "Text9":
				{
					if(Locale == "huHU")
						return "Új forrás kibontása.";
					else if(Locale == "enUS")
						return "Extracting new version.";
					else
						return "Extracting new version.";
				}
				case "Text10":
				{
					if(Locale == "huHU")
						return "Az állomány sikeresen ki lett bontva.";
					else if(Locale == "enUS")
						return "Successfully extracted the staff.";
					else
						return "Successfully extracted the staff.";
				}
				case "Text11":
				{
					if(Locale == "huHU")
						return "Kibontás sikertelen!";
					else if(Locale == "enUS")
						return "Extracting unsuccessful.";
					else
						return "Extracting unsuccessful.";
				}
				case "Text12":
				{
					if(Locale == "huHU")
						return "Fordítás megkezdése.";
					else if(Locale == "enUS")
						return "Started translating.";
					else
						return "Started translating.";
				}
				case "Text13":
				{
					if(Locale == "huHU")
						return "Hiba történt a fordítás közben!";
					else if(Locale == "enUS")
						return "Error was handled while translated.";
					else
						return "Error was handled while translated.";
				}
				case "Text14":
				{
					if(Locale == "huHU")
						return "Fordítás sikeresen befejeződött.";
					else if(Locale == "enUS")
						return "Successfully finished the translation.";
					else
						return "Successfully finished the translation.";
				}
				case "Text15":
				{
					if(Locale == "huHU")
						return "Frissítés ezen szakasza befejeződött. Következik a beállítás.";
					else if(Locale == "enUS")
						return "This step of updateing is finished. Continue with next step.";
					else
						return "This step of updateing is finished. Continue with next step.";
				}
				case "Text16":
				{
					if(Locale == "huHU")
						return "Nem létezik ezen verzió: {0}";
					else if(Locale == "enUS")
						return "No such version: {0}";
					else
						return "No such version: {0}";
				}
				default:
					return string.Empty;
			}
		}

		public string ScriptManager(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Lua támogatása le van tiltva!";
					else if(Locale == "enUS")
						return "Lua support is disabled!";
					else
						return "Lua support is disabled!";
				}
				default:
					return string.Empty;
			}
		}

		public string LuaEngine(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Lua motor betöltése.";
					else if(Locale == "enUS")
						return "Initializing Lua engine.";
					else
						return "Initializing Lua engine.";
				}
				case "Text2":
				{
					if(Locale == "huHU")
						return "Lua script betöltése: {0}";
					else if(Locale == "enUS")
						return "Loading Lua script: {0}";
					else
						return "Loading Lua script: {0}";
				}
				case "Text3":
				{
					if(Locale == "huHU")
						return "Kivétel dobott betöltése közben a Lua script: {0} Error: {1}";
					else if(Locale == "enUS")
						return "Exception thrown while loading Lua script: {0} Error: {1}";
					else
						return "Exception thrown while loading Lua script: {0} Error: {1}";
				}
				default:
					return string.Empty;
			}
		}

		public string LuaHelper(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Funkció {0} (exportált {1}): argumentum szám nem egyezik. Nyilvánítva {2}, de ehhez {3}!";
					else if(Locale == "enUS")
						return "Function {0} (exported as {1}): argument number mismatch. Declared {2}, but requires {3}!";
					else
						return "Function {0} (exported as {1}): argument number mismatch. Declared {2}, but requires {3}!";
				}
				case "Text2":
				{
					if(Locale == "huHU")
						return "Lua fordítási hiba: {0}";
					else if(Locale == "enUS")
						return "Lua compile error: {0}";
					else
						return "Lua compile error: {0}";
				}
				default:
					return string.Empty;
			}
		}

		public string CtcpSender(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Ez a kliens támogatja: UserInfo, Finger, Version, Source, Ping, Time és ClientInfo";
					else if(Locale == "enUS")
						return "This client supports: UserInfo, Finger, Version, Source, Ping, Time and ClientInfo";
					else
						return "This client supports: UserInfo, Finger, Version, Source, Ping, Time and ClientInfo";
				}
				case "Text2":
				{
					if(Locale == "huHU")
						return " óra, ";
					else if(Locale == "enUS")
						return " Hours, ";
					else
						return " Hours, ";
				}
				case "Text3":
				{
					if(Locale == "huHU")
						return " perc, ";
					else if(Locale == "enUS")
						return " Minutes, ";
					else
						return " Minutes, ";
				}
				case "Text4":
				{
					if(Locale == "huHU")
						return " másodperc.";
					else if(Locale == "enUS")
						return " Seconds.";
					else
						return " Seconds.";
				}
				case "Text5":
				{
					if(Locale == "huHU")
						return " Tétlenségi idő ";
					else if(Locale == "enUS")
						return " Idle time ";
					else
						return " Idle time ";
				}
				case "Text6":
				{
					if(Locale == "huHU")
						return "{0} nem támogatott Ctcp lekérdezés.";
					else if(Locale == "enUS")
						return "{0} is not a supported Ctcp query.";
					else
						return "{0} is not a supported Ctcp query.";
				}
				case "Text7":
				{
					if(Locale == "huHU")
						return "Operácios Rendszer:";
					else if(Locale == "enUS")
						return "Operating System:";
					else
						return "Operating System:";
				}
				case "Text8":
				{
					if(Locale == "huHU")
						return "Processzor:";
					else if(Locale == "enUS")
						return "Processor:";
					else
						return "Processor:";
				}
				default:
					return string.Empty;
			}
		}

		public string Other(string Name)
		{
			switch(Name)
			{
				case "Nothing":
				{
					if(Locale == "huHU")
						return "Semmi";
					else if(Locale == "enUS")
						return "Nothing";
					else
						return "Nothing";
				}
				case "Nobody":
				{
					if(Locale == "huHU")
						return "Senki";
					else if(Locale == "enUS")
						return "Nobody";
					else
						return "Nobody";
				}
				case "Notfound":
				{
					if(Locale == "huHU")
						return "Nem található!";
					else if(Locale == "enUS")
						return "Not found!";
					else
						return "Not found!";
				}
				case "NoSuchFunctions":
				{
					if(Locale == "huHU")
						return "Ilyen funkció nem létezik!";
					else if(Locale == "enUS")
						return "No such function!";
					else
						return "No such function!";
				}
				case "NoSuchFunctions2":
				{
					if(Locale == "huHU")
						return "Ilyen funkció nem létezik: {0}";
					else if(Locale == "enUS")
						return "No such function: {0}";
					else
						return "No such function: {0}";
				}
				default:
					return string.Empty;
			}
		}

		public string ServerListener(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Indul...";
					else if(Locale == "enUS")
						return "Started...";
					else
						return "Started...";
				}
				case "Text2":
				{
					if(Locale == "huHU")
						return "Client kapcsolódik: {0}";
					else if(Locale == "enUS")
						return "Client connection from: {0}";
					else
						return "Client connection from: {0}";
				}
				case "Text3":
				{
					if(Locale == "huHU")
						return "Client kezelése...";
					else if(Locale == "enUS")
						return "Handling client...";
					else
						return "Handling client...";
				}
				case "Text4":
				{
					if(Locale == "huHU")
						return "Rendelkezésre álló adatok olvasása.";
					else if(Locale == "enUS")
						return "Stream data available, reading.";
					else
						return "Stream data available, reading.";
				}
				case "Text5":
				{
					if(Locale == "huHU")
						return "Elveszett a kapcsolat!";
					else if(Locale == "enUS")
						return "Lost connection!";
					else
						return "Lost connection!";
				}
				case "Text6":
				{
					if(Locale == "huHU")
						return "A program leáll!";
					else if(Locale == "enUS")
						return "Program shutting down!";
					else
						return "Program shutting down!";
				}
				default:
					return string.Empty;
			}
		}

		public string ClientSocket(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Indul...";
					else if(Locale == "enUS")
						return "Started...";
					else
						return "Started...";
				}
				case "Text2":
				{
					if(Locale == "huHU")
						return "Client kapcsolódik: {0}";
					else if(Locale == "enUS")
						return "Client connection from: {0}";
					else
						return "Client connection from: {0}";
				}
				case "Text3":
				{
					if(Locale == "huHU")
						return "Client kezelése...";
					else if(Locale == "enUS")
						return "Handling client...";
					else
						return "Handling client...";
				}
				case "Text4":
				{
					if(Locale == "huHU")
						return "Rendelkezésre álló adatok olvasása.";
					else if(Locale == "enUS")
						return "Stream data available, reading.";
					else
						return "Stream data available, reading.";
				}
				case "Text5":
				{
					if(Locale == "huHU")
						return "Elveszett a kapcsolat!";
					else if(Locale == "enUS")
						return "Lost connection!";
					else
						return "Lost connection!";
				}
				case "Text6":
				{
					if(Locale == "huHU")
						return "A program leáll!";
					else if(Locale == "enUS")
						return "Program shutting down!";
					else
						return "Program shutting down!";
				}
				case "Text7":
				{
					if(Locale == "huHU")
						return "Kapcsolodva. Csomag küldése.";
					else if(Locale == "enUS")
						return "Connected. Sending packet.";
					else
						return "Connected. Sending packet.";
				}
				case "Text8":
				{
					if(Locale == "huHU")
						return "Csomag elküldve.";
					else if(Locale == "enUS")
						return "Packet sent.";
					else
						return "Packet sent.";
				}
				case "Text9":
				{
					if(Locale == "huHU")
						return "Nem sikerült elküldeni SCS csomagot!";
					else if(Locale == "enUS")
						return "Couldn't send SCS packet!";
					else
						return "Couldn't send SCS packet!";
				}
				default:
					return string.Empty;
			}
		}

		public string ServerPacketHandler(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Csomagot kapott, ID: {0} tőle: {1}";
					else if(Locale == "enUS")
						return "Got packet with ID: {0} from: {1}";
					else
						return "Got packet with ID: {0} from: {1}";
				}
				case "Text2":
				{
					if(Locale == "huHU")
						return "Hitelesítés sikertelen! Guid a clienttől: {0}";
					else if(Locale == "enUS")
						return "Auth unsuccessful! Guid of client: {0}";
					else
						return "Auth unsuccessful! Guid of client: {0}";
				}
				case "Text3":
				{
					if(Locale == "huHU")
						return "Hash volt: {0}";
					else if(Locale == "enUS")
						return "Hash was: {0}";
					else
						return "Hash was: {0}";
				}
				case "Text4":
				{
					if(Locale == "huHU")
						return "Vissza port: {0}";
					else if(Locale == "enUS")
						return "Back port is: {0}";
					else
						return "Back port is: {0}";
				}
				case "Text5":
				{
					if(Locale == "huHU")
						return "Hitelesítés sikeres. Guid a clienttől: {0}";
					else if(Locale == "enUS")
						return "Auth successful. Guid of client: {0}";
					else
						return "Auth successful. Guid of client: {0}";
				}
				case "Text6":
				{
					if(Locale == "huHU")
						return "Kapcsolat bontva! Guid a clienttől: {0}";
					else if(Locale == "enUS")
						return "Connection closed! Guid of client: {0}";
					else
						return "Connection closed! Guid of client: {0}";
				}
				case "Text7":
				{
					if(Locale == "huHU")
						return "Újraindítás folyamatban...";
					else if(Locale == "enUS")
						return "Restart in progress...";
					else
						return "Restart in progress...";
				}
				default:
					return string.Empty;
			}
		}

		public string ClientPacketHandler(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Csomagot kapott, ID: {0} tőle: {1}";
					else if(Locale == "enUS")
						return "Got packet with ID: {0} from: {1}";
					else
						return "Got packet with ID: {0} from: {1}";
				}
				case "Text2":
				{
					if(Locale == "huHU")
						return "Hitelesítést megtagadta az SCS szerver!";
					else if(Locale == "enUS")
						return "Authentication denied to SCS server!";
					else
						return "Authentication denied to SCS server!";
				}
				case "Text3":
				{
					if(Locale == "huHU")
						return "Kapcsolat bontva!";
					else if(Locale == "enUS")
						return "Connection closed!";
					else
						return "Connection closed!";
				}
				case "Text4":
				{
					if(Locale == "huHU")
						return "A program leáll!";
					else if(Locale == "enUS")
						return "Program shutting down!";
					else
						return "Program shutting down!";
				}
				case "Text5":
				{
					if(Locale == "huHU")
						return "Sikeresen hitelesítve az SCS szerver.";
					else if(Locale == "enUS")
						return "Successfully authed to SCS server.";
					else
						return "Successfully authed to SCS server.";
				}
				default:
					return string.Empty;
			}
		}

		public string Runtime(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Nem sikerült beállítani folyamat nevét: {0}";
					else if(Locale == "enUS")
						return "Failed to set process name: {0}";
					else
						return "Failed to set process name: {0}";
				}
				case "Text2":
				{
					if(Locale == "huHU")
						return "Hiba a folyamat nevének beállításában!";
					else if(Locale == "enUS")
						return "Error setting process name!";
					else
						return "Error setting process name!";
				}
				case "Text3":
				{
					if(Locale == "huHU")
						return "A program több mint 100 mb-ot fogyaszt!";
					else if(Locale == "enUS")
						return "The program, more than 100 MB consumed!";
					else
						return "The program, more than 100 MB consumed!";
				}
				case "Text4":
				{
					if(Locale == "huHU")
						return "A program leáll!";
					else if(Locale == "enUS")
						return "Program shutting down!";
					else
						return "Program shutting down!";
				}
				default:
					return string.Empty;
			}
		}

		public string CrashDumper(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Crash dump készítése...";
					else if(Locale == "enUS")
						return "Creating crash dump...";
					else
						return "Creating crash dump...";
				}
				case "Text2":
				{
					if(Locale == "huHU")
						return "Nem sikerült crash dump-ot írni! ({0})";
					else if(Locale == "enUS")
						return "Failed to write crash dump! ({0})";
					else
						return "Failed to write crash dump! ({0})";
				}
				case "Text3":
				{
					if(Locale == "huHU")
						return "Crash dump létrehozva.";
					else if(Locale == "enUS")
						return "Crash dump created.";
					else
						return "Crash dump created.";
				}
				default:
					return string.Empty;
			}
		}

		public string Schumix(string Name)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Locale == "huHU")
						return "Leállt az egyik Schumix ezért újra lesz indítva.";
					else if(Locale == "enUS")
						return "One of the summix has been shuted down, it will be restarted.";
					else
						return "One of the summix has been shuted down, it will be restarted.";
				}
				default:
					return string.Empty;
			}
		}
	}
}