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
using System.IO;
using System.Linq;
using System.Threading;
using System.Reflection;
using System.Globalization;
using Mono.Unix;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;

namespace Schumix.Framework.Localization
{
	public sealed class LocalizationConsole
	{
		public string Locale { get; set; }

		private LocalizationConsole()
		{
			Initialize("./locale");
			SetLocale("huHU"); // mind2-ő áthelyezése innét valahova máshova. (Oda ahonnét majd indítva lesz. Plusz a konfigba is berakni a frissítés miatt stb.)
			//SetLocale("en-US"); // Ez marad majd csak itt alapértelmezésben.
		}

		public void Initialize()
		{
			Initialize("./locale");
		}

		public void Initialize(string LocaleDir)
		{
			if(LocaleDir.IsNullOrEmpty())
			{
				string location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
				
				if(GetPlatformType() == PlatformType.Windows)
					LocaleDir = Path.Combine(location, "locale");
				else if(GetPlatformType() == PlatformType.Linux)
				{
					bool enabled = false;
					var dir = new DirectoryInfo(location);

					foreach(var d in dir.GetDirectories("locale").AsParallel())
					{
						if(d.Name == "locale")
							enabled = true;
					}

					if(enabled)
						LocaleDir = "./locale";
					else
					{
						// $prefix/bin
						string prefix = Path.Combine(location, "..");
						prefix = Path.GetFullPath(prefix);

						// "$prefix/share/locale"
						LocaleDir = Path.Combine(Path.Combine(prefix, "share"), "locale");
					}
				}
			}
			
			Mono.Unix.Catalog.Init("Schumix", LocaleDir);
		}

		public void SetLocale()
		{
			SetLocale("en-US");
		}

		public void SetLocale(string Language)
		{
			if(Language.Length == 4 && !Language.Contains("-"))
				Language = Language.Substring(0, 2) + "-" + Language.Substring(2);
			else if((Language.Length < 4 || Language.Length > 4) && !Language.Contains("-"))
				Language = "en-US";

			if(GetPlatformType() == PlatformType.Windows)
				Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(Language);
			else if(GetPlatformType() == PlatformType.Linux)
				Environment.SetEnvironmentVariable("LANGUAGE", Language.Substring(0, 2));
			else
				Environment.SetEnvironmentVariable("LANGUAGE", Language.Substring(0, 2));
		}

		private PlatformType GetPlatformType()
		{
			PlatformType platform = PlatformType.None;
			var pid = Environment.OSVersion.Platform;
			
			switch(pid)
			{
			case PlatformID.Win32NT:
			case PlatformID.Win32S:
			case PlatformID.Win32Windows:
			case PlatformID.WinCE:
				platform = PlatformType.Windows;
				break;
			case PlatformID.Unix:
				platform = PlatformType.Linux;
				break;
			case PlatformID.MacOSX:
				platform = PlatformType.MacOSX;
				break;
			case PlatformID.Xbox:
				platform = PlatformType.Xbox;
				break;
			default:
				platform = PlatformType.None;
				break;
			}
			
			return platform;
		}

		public string GetString(string phrase)
		{
			return Catalog.GetString(phrase);
		}
		
		public string GetString(string phrase, object arg0)
		{
			return string.Format(Catalog.GetString(phrase), arg0);
		}
		
		public string GetString(string phrase, object arg0, object arg1)
		{
			return string.Format(Catalog.GetString(phrase), arg0, arg1);
		}
		
		public string GetString(string phrase, object arg0, object arg1, object arg2)
		{
			return string.Format(Catalog.GetString(phrase), arg0, arg1, arg2);
		}
		
		public string GetString(string phrase, params object[] args)
		{
			return string.Format(Catalog.GetString(phrase), args);
		}
		
		public string GetPluralString(string singular, string plural, int number)
		{
			return Catalog.GetPluralString(singular, plural, number);
		}
		
		public string GetPluralString(string singular, string plural, int number, object arg0)
		{
			return string.Format(Catalog.GetPluralString(singular, plural, number), arg0);
		}
		
		public string GetPluralString(string singular, string plural, int number, object arg0, object arg1)
		{
			return string.Format(Catalog.GetPluralString(singular, plural, number), arg0, arg1);
		}
		
		public string GetPluralString(string singular, string plural, int number, object arg0, object arg1, object arg2)
		{
			return string.Format(Catalog.GetPluralString(singular, plural, number), arg0, arg1, arg2);
		}
		
		public string GetPluralString(string singular, string plural, int number, params object[] args)
		{
			return string.Format(Catalog.GetPluralString(singular, plural, number), args);
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
				case "Text16":
				{
					if(Language == "huHU")
						return "Nem lehet megváltoztatni a becenevet mert le van tiltva vagy moderált csatornán van fent!";
					else if(Language == "enUS")
						return "Cannot change nickname while banned or moderated on channel!";
					else
						return "Cannot change nickname while banned or moderated on channel!";
				}
				case "Text17":
				{
					if(Language == "huHU")
						return "Nem lehet csatlakozni a csatornára (+i): {0}";
					else if(Language == "enUS")
						return "Cannot join channel (+i): {0}";
					else
						return "Cannot join channel (+i): {0}";
				}
				case "Text17-1":
				{
					if(Language == "huHU")
						return "Nem lehet csatlakozni a csatornára (+i)!";
					else if(Language == "enUS")
						return "Cannot join channel (+i)!";
					else
						return "Cannot join channel (+i)!";
				}
				case "Text18":
				{
					if(Language == "huHU")
						return "[csatlakozott]";
					else if(Language == "enUS")
						return "[joined]";
					else
						return "[joined]";
				}
				case "Text19":
				{
					if(Language == "huHU")
						return "[kilépett innen (left)] {0}";
					else if(Language == "enUS")
						return "[left] {0}";
					else
						return "[left] {0}";
				}
				case "Text20":
				{
					if(Language == "huHU")
						return "[kilépett (quit)] {0}";
					else if(Language == "enUS")
						return "[quit] {0}";
					else
						return "[quit] {0}";
				}
				case "Text21":
				{
					if(Language == "huHU")
						return "[Mostantól {0}-ként ismert]";
					else if(Language == "enUS")
						return "[Is now known as {0}]";
					else
						return "[Is now known as {0}]";
				}
				case "Text22":
				{
					if(Language == "huHU")
						return "[Kirúgta a következő felhasználót: {0} oka: {1}]";
					else if(Language == "enUS")
						return "[Kicked that user: {0} reason: {1}]";
					else
						return "[Kicked that user: {0} reason: {1}]";
				}
				case "Text23":
				{
					if(Language == "huHU")
						return "[Beállítja a módot: {0}]";
					else if(Language == "enUS")
						return "[Set mode: {0}]";
					else
						return "[Set mode: {0}]";
				}
				case "Text24":
				{
					if(Language == "huHU")
						return "[Téma] Új téma: {0}";
					else if(Language == "enUS")
						return "[Topic] New topic: {0}";
					else
						return "[Topic] New topic: {0}";
				}
				case "Text25":
				{
					if(Language == "huHU")
						return "[ACTION] {0}";
					else if(Language == "enUS")
						return "[ACTION] {0}";
					else
						return "[ACTION] {0}";
				}
				case "Text26":
				{
					if(Language == "huHU")
						return "[INVITE] {0} meghívott téged a {1} szobába.";
					else if(Language == "enUS")
						return "[INVITE] {0} invites you to join {1}";
					else
						return "[INVITE] {0} invites you to join {1}";
				}
				case "Text27":
				{
					if(Language == "huHU")
						return "{0} meghívott téged a {1} szobába.";
					else if(Language == "enUS")
						return "{0} invites you to join {1}";
					else
						return "{0} invites you to join {1}";
				}
				case "Text28":
				{
					if(Language == "huHU")
						return "[Téma] {0}";
					else if(Language == "enUS")
						return "[Topic] {0}";
					else
						return "[Topic] {0}";
				}
				default:
					return string.Empty;
			}
		}

		public string Other(string Name)
		{
			return Other(Name, Locale);
		}

		public string Other(string Name, string Language)
		{
			switch(Name)
			{
				case "Nothing":
				{
					if(Language == "huHU")
						return "Semmi";
					else if(Language == "enUS")
						return "Nothing";
					else
						return "Nothing";
				}
				case "Nobody":
				{
					if(Language == "huHU")
						return "Senki";
					else if(Language == "enUS")
						return "Nobody";
					else
						return "Nobody";
				}
				case "Notfound":
				{
					if(Language == "huHU")
						return "Nem található!";
					else if(Language == "enUS")
						return "Not found!";
					else
						return "Not found!";
				}
				case "NoSuchFunctions":
				{
					if(Language == "huHU")
						return "Ilyen funkció nem létezik!";
					else if(Language == "enUS")
						return "No such function!";
					else
						return "No such function!";
				}
				case "NoSuchFunctions2":
				{
					if(Language == "huHU")
						return "Ilyen funkció nem létezik: {0}";
					else if(Language == "enUS")
						return "No such function: {0}";
					else
						return "No such function: {0}";
				}
				case "MessageLength":
				{
					if(Language == "huHU")
						return "Túl hosszú a szöveg!";
					else if(Language == "enUS")
						return "Text is too long!";
					else
						return "Text is too long!";
				}
				case "NoFoundHelpCommand":
				{
					if(Language == "huHU")
						return "Ilyen help parancs nem létezik vagy nincs hozzá fordítás!";
					else if(Language == "enUS")
						return "No such help command or there is no translation for it!";
					else
						return "No such help command or there is no translation for it!";
				}
				case "NoFoundHelpCommand2":
				{
					if(Language == "huHU")
						return "Ilyen help parancs nem létezik!";
					else if(Language == "enUS")
						return "No such help command!";
					else
						return "No such help command!";
				}
				case "And":
				{
					if(Language == "huHU")
						return "és";
					else if(Language == "enUS")
						return "and";
					else
						return "and";
				}
				default:
					return string.Empty;
			}
		}

		public string IgnoreChannel(string Name)
		{
			return IgnoreChannel(Name, Locale);
		}

		public string IgnoreChannel(string Name, string Language)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Language == "huHU")
						return "Konfig által letiltva!";
					else if(Language == "enUS")
						return "Disabled by Config!";
					else
						return "Disabled by Config!";
				}
				case "Text2":
				{
					if(Language == "huHU")
						return "A csatorna tiltásra került!";
					else if(Language == "enUS")
						return "Channel is banned!";
					else
						return "Channel is banned!";
				}
				default:
					return string.Empty;
			}
		}

		public string Sender(string Name)
		{
			return Sender(Name, Locale);
		}

		public string Sender(string Name, string Language)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Language == "huHU")
						return "Távozom";
					else if(Language == "enUS")
						return "Leaving";
					else
						return "Leaving";
				}
				case "Text2":
				{
					if(Language == "huHU")
						return "{0} nem érvényes a nick név!";
					else if(Language == "enUS")
						return "{0} is not a valid nickname!";
					else
						return "{0} is not a valid nickname!";
				}
				case "Text3":
				{
					if(Language == "huHU")
						return "{0} nem érvényes a csatorna név!";
					else if(Language == "enUS")
						return "{0} is not a valid channel name!";
					else
						return "{0} is not a valid channel name!";
				}
				case "Text4":
				{
					if(Language == "huHU")
						return "A jelszó nem lehet üres vagy nulla!";
					else if(Language == "enUS")
						return "Password cannot be empty or null!";
					else
						return "Password cannot be empty or null!";
				}
				case "Text5":
				{
					if(Language == "huHU")
						return "Lelépés oka nem lehet üres vagy nulla!";
					else if(Language == "enUS")
						return "Part reason cannot be empty or null!";
					else
						return "Part reason cannot be empty or null!";
				}
				case "Text6":
				{
					if(Language == "huHU")
						return "Az egyik csatorna neve nem érvényes!";
					else if(Language == "enUS")
						return "One of the channels names is not valid!";
					else
						return "One of the channels names is not valid!";
				}
				case "Text7":
				{
					if(Language == "huHU")
						return "Kirúgás oka nem lehet üres vagy nulla!";
					else if(Language == "enUS")
						return "The reason for kicking cannot be empty or null!";
					else
						return "The reason for kicking cannot be empty or null!";
				}
				case "Text8":
				{
					if(Language == "huHU")
						return "Nick nem lehet üres vagy nulla!";
					else if(Language == "enUS")
						return "Nick cannot be empty or null!";
					else
						return "Nick cannot be empty or null!";
				}
				case "Text9":
				{
					if(Language == "huHU")
						return "Oka nem lehet üres vagy nulla!";
					else if(Language == "enUS")
						return "Reason cannot be empty or null!";
					else
						return "Reason cannot be empty or null!";
				}
				case "Text10":
				{
					if(Language == "huHU")
						return "Kilépés oka nem lehet üres vagy nulla!";
					else if(Language == "enUS")
						return "Quit reason cannot be null or empty!";
					else
						return "Quit reason cannot be null or empty!";
				}
				case "Text11":
				{
					if(Language == "huHU")
						return "Az e-mail cím nem érvényes!";
					else if(Language == "enUS")
						return "The email address is not valid!";
					else
						return "The email address is not valid!";
				}
				case "Text12":
				{
					if(Language == "huHU")
						return "Téma nem lehet üres vagy nulla!";
					else if(Language == "enUS")
						return "Topic cannot be empty or null!";
					else
						return "Topic cannot be empty or null!";
				}
				case "Text13":
				{
					if(Language == "huHU")
						return "Távollét üzenet nem lehet üres vagy nulla!";
					else if(Language == "enUS")
						return "Away message cannot be empty or null!";
					else
						return "Away message cannot be empty or null!";
				}
				case "Text14":
				{
					if(Language == "huHU")
						return "{0} csatorna elérése le van tiltva!";
					else if(Language == "enUS")
						return "Channel {0}'s access is denied!";
					else
						return "Channel {0}'s access is denied!";
				}
				default:
					return string.Empty;
			}
		}
	}
}
