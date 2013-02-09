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

namespace Schumix.Installer.Localization
{
	sealed class LocalizationConsole
	{
		public string Locale { get; set; }
		private LocalizationConsole() {}

		public string Installer(string Name)
		{
			switch(Name)
			{
				case "Text2":
				{
					if(Locale == "huHU")
						return "Installer elindult.";
					else if(Locale == "enUS")
						return "Installer started.";
					else
						return "Installer started.";
				}
				case "Text3":
				{
					if(Locale == "huHU")
						return "Fájlok átmásolása.";
					else if(Locale == "enUS")
						return "Copy files.";
					else
						return "Copy files.";
				}
				case "Text4":
				{
					if(Locale == "huHU")
						return "Fölösleges adatok törlése.";
					else if(Locale == "enUS")
						return "Remove unneeded datas.";
					else
						return "Remove unneeded datas.";
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
						return "A telepítés befejeződött!";
					else if(Locale == "enUS")
						return "Installation successful!";
					else
						return "Installation successful!";
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
						return "A telepítés befejeződött. Leáll a program!";
					else if(Locale == "enUS")
						return "The installation is finished. The program shutting down!";
					else
						return "The installation is finished. The program shutting down!";
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
						return "Indulási időpont: [{0}. {1}. {2}. {3}:{4}:{5}]\n";
					else if(Locale == "enUS")
						return "Started time: [{0}. {1}. {2}. {3}:{4}:{5}]\n";
					else
						return "Started time: [{0}. {1}. {2}. {3}:{4}:{5}]\n";
				}
				case "Text2":
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
	}
}