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
						return "";
					else
						return "";
				}
			}

			return string.Empty;
		}

		public string Exception(string Name)
		{
			switch(Name)
			{
				case "Error":
				{
					if(Locale == "huHU")
						return "Hiba oka: {0}";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
			}

			return string.Empty;
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
						return "";
					else
						return "";
				}
				case "Text2":
				{
					if(Locale == "huHU")
						return "Network indul...";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
				case "Text3":
				{
					if(Locale == "huHU")
						return "Console indul...";
					else if(Locale == "enUS")
						return "";
					else
						return "";
				}
			}

			return string.Empty;
		}
	}
}