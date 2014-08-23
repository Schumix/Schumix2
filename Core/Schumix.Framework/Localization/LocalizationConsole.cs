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
using System.IO;
using System.Linq;
using System.Threading;
using System.Reflection;
using System.Globalization;
using Schumix.Framework.Config;
using Schumix.Framework.Platforms;
using Schumix.Framework.Extensions;
using NGettext;

namespace Schumix.Framework.Localization
{
	public sealed class LocalizationConsole
	{
		private readonly Platform sPlatform = Singleton<Platform>.Instance;
		public string Locale { get; set; }
		private ICatalog _catalog;
		private string _localedir;

		private LocalizationConsole()
		{
			Initialize();
			SetLocale("en-US");
		}

		public void Initialize()
		{
			Initialize("./locale");
		}

		public void Initialize(string LocaleDir)
		{
			if(LocaleDir.IsNullOrEmpty() || LocaleDir == "./locale")
			{
				string location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
				
				if(sPlatform.IsWindows)
					LocaleDir = Path.Combine(location, "locale");
				else if(sPlatform.IsLinux)
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
						// $prefix/lib/schumix
						string prefix = Path.Combine(Path.Combine(location, ".."), "..");
						prefix = Path.GetFullPath(prefix);

						// "$prefix/share/locale"
						LocaleDir = Path.Combine(Path.Combine(prefix, "share"), "locale");
					}
				}
			}

			_localedir = LocaleDir;
			LoadCatalog();
		}

		private void LoadCatalog()
		{
			LoadCatalog(CultureInfo.GetCultureInfo("en-US"));
		}

		private void LoadCatalog(CultureInfo ci)
		{
			_catalog = new Catalog("schumix", _localedir, ci);
		}

		public void SetLocale()
		{
			SetLocale("en-US");
		}

		public void SetLocale(string Language)
		{
			Locale = Language.Replace("-", string.Empty);
			Language = Language.ToLocale();

			if(sPlatform.IsWindows)
			{
#if false
				// .net 4.5
				CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo(Language);
#endif
				Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(Language);
			}
			else if(sPlatform.IsLinux)
			{
#if false
				// .net 4.5
				CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo(Language);
#endif
				Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(Language);
				Environment.SetEnvironmentVariable("LANGUAGE", Language.Substring(0, 2));
			}
			else
			{
#if false
				// .net 4.5
				CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo(Language);
#endif
				Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(Language);
				Environment.SetEnvironmentVariable("LANGUAGE", Language.Substring(0, 2));
			}

			LoadCatalog(CultureInfo.GetCultureInfo(Language));
		}

		public string GetString(string phrase)
		{
			return _catalog.GetString(phrase);
		}
		
		public string GetString(string phrase, object arg0)
		{
			return string.Format(_catalog.GetString(phrase), arg0);
		}
		
		public string GetString(string phrase, object arg0, object arg1)
		{
			return string.Format(_catalog.GetString(phrase), arg0, arg1);
		}
		
		public string GetString(string phrase, object arg0, object arg1, object arg2)
		{
			return string.Format(_catalog.GetString(phrase), arg0, arg1, arg2);
		}
		
		public string GetString(string phrase, params object[] args)
		{
			return string.Format(_catalog.GetString(phrase), args);
		}
		
		public string GetPluralString(string singular, string plural, int number)
		{
			return _catalog.GetPluralString(singular, plural, number);
		}
		
		public string GetPluralString(string singular, string plural, int number, object arg0)
		{
			return string.Format(_catalog.GetPluralString(singular, plural, number), arg0);
		}
		
		public string GetPluralString(string singular, string plural, int number, object arg0, object arg1)
		{
			return string.Format(_catalog.GetPluralString(singular, plural, number), arg0, arg1);
		}
		
		public string GetPluralString(string singular, string plural, int number, object arg0, object arg1, object arg2)
		{
			return string.Format(_catalog.GetPluralString(singular, plural, number), arg0, arg1, arg2);
		}
		
		public string GetPluralString(string singular, string plural, int number, params object[] args)
		{
			return string.Format(_catalog.GetPluralString(singular, plural, number), args);
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
						return "Nincs elég paraméter megadva!";
					else if(Language == "enUS")
						return "Not enough parameters!";
					else
						return "Not enough parameters!";
				}
				case "Text2":
				{
					if(Language == "huHU")
						return "Csatorna kulcs már megvan adva!";
					else if(Language == "enUS")
						return "Channel key already set!";
					else
						return "Channel key already set!";
				}
				case "Text3":
				{
					if(Language == "huHU")
						return "A csatorna nem támogatja a megadott módot!";
					else if(Language == "enUS")
						return "Channel doesn't support modes!";
					else
						return "Channel doesn't support modes!";
				}
				case "Text5":
				{
					if(Language == "huHU")
						return "Nincs ezen a csatornán!";
					else if(Language == "enUS")
						return "They aren't on that channel!";
					else
						return "They aren't on that channel!";
				}
				case "Text6":
				{
					if(Language == "huHU")
						return "Ismeretlen mód a csatornához!";
					else if(Language == "enUS")
						return "Is unknown mode char to me for [channel]!";
					else
						return "Is unknown mode char to me for [channel]!";
				}
				case "Text7":
				{
					if(Language == "huHU")
						return "Nincs ilyen nick: {0}";
					else if(Language == "enUS")
						return "No such nick: {0}";
					else
						return "No such nick: {0}";
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
						return "Nincs elég jogom hogy, kirúgjam a csatornáról!";
					else if(Language == "enUS")
						return "I don't have enough permission to kick him/her from the channel!";
					else
						return "I don't have enough permission to kick him/her from the channel!";
				}
				case "Text19":
				{
					if(Language == "huHU")
						return "Nincs elég jogom a rang módosításához!";
					else if(Language == "enUS")
						return "I have no rights to modify the rank!";
					else
						return "I have no rights to modify the rank!";
				}
				case "Text20":
				{
					if(Language == "huHU")
						return "A megadott nick név egy szervíz neve!";
					else if(Language == "enUS")
						return "This nick name is a service name.";
					else
						return "This nick name is a service name.";
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
				case "Started":
				{
					if(Language == "huHU")
						return "elindítva";
					else if(Language == "enUS")
						return "started";
					else
						return "started";
				}
				case "Stopped":
				{
					if(Language == "huHU")
						return "leállítva";
					else if(Language == "enUS")
						return "stopped";
					else
						return "stopped";
				}
				case "YoutubeViewCount":
				{
					if(Language == "huHU")
						return "Megtekintések száma";
					else if(Language == "enUS")
						return "View Count";
					else
						return "View Count";
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
				default:
					return string.Empty;
			}
		}
	}
}
