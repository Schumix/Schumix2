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

namespace Schumix.Installer.Localization
{
	sealed class LocalizationConsole
	{
		public string Locale { get; set; }
		private LocalizationConsole() {}

		public string Update(string Name)
		{
			switch(Name)
			{
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
				default:
					return string.Empty;
			}
		}
	}
}