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

namespace Schumix.WordPressRssAddon.Localization
{
	sealed class PLocalization
	{
		public string Locale { get; set; }
		private PLocalization() {}

		public string WordPressRss(string Name)
		{
			return WordPressRss(Name, Locale);
		}

		public string WordPressRss(string Name, string Language)
		{
			switch(Name)
			{
				case "WordPress":
				{
					if(Language == "huHU")
						return "[3{0}] {1} küldött új témát: 02{2}";
					else if(Language == "enUS")
						return "[3{0}] {1} pushed new topic: 02{2}";
					else
						return "[3{0}] {1} pushed new topic: 02{2}";
				}
				case "WordPress2":
				{
					if(Language == "huHU")
						return "3{0} Infó: {1}";
					else if(Language == "enUS")
						return "3{0} Info: {1}";
					else
						return "3{0} Info: {1}";
				}
				case "nocolorsWordPress":
				{
					if(Language == "huHU")
						return "[{0}] {1} küldött új témát: {2}";
					else if(Language == "enUS")
						return "[{0}] {1} pushed new topic: {2}";
					else
						return "[{0}] {1} pushed new topic: {2}";
				}
				case "nocolorsWordPress2":
				{
					if(Language == "huHU")
						return "{0} Infó: {1}";
					else if(Language == "enUS")
						return "{0} Info: {1}";
					else
						return "{0} Info: {1}";
				}
				default:
					return string.Empty;
			}
		}
	}
}