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

namespace Schumix.MantisBTRssAddon.Localization
{
	sealed class PLocalization
	{
		public string Locale { get; set; }
		private PLocalization() {}

		public string MantisBTRss(string Name)
		{
			return MantisBTRss(Name, Locale);
		}

		public string MantisBTRss(string Name, string Language)
		{
			switch(Name)
			{
				case "Text":
				{
					if(Language == "huHU")
						return "3{0} BugKód: 10{1} Link: {2}";
					else if(Language == "enUS")
						return "3{0} BugCode: 10{1} Link: {2}";
					else
						return "3{0} BugCode: 10{1} Link: {2}";
				}
				case "Text2":
				{
					if(Language == "huHU")
						return "3{0} Infó:{1}";
					else if(Language == "enUS")
						return "3{0} Info:{1}";
					else
						return "3{0} Info:{1}";
				}
				default:
					return string.Empty;
			}
		}
	}
}