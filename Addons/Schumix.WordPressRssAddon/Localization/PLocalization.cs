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
						return "\u0002[\u00033{0}\u000f\u000f\u0002]\u000f \u0002{1}\u000f küldött új témát: \u000302\u001f{2}\u000f\u000f";
					else if(Language == "enUS")
						return "\u0002[\u00033{0}\u000f\u000f\u0002]\u000f \u0002{1}\u000f pushed new topic: \u000302\u001f{2}\u000f\u000f";
					else
						return "\u0002[\u00033{0}\u000f\u000f\u0002]\u000f \u0002{1}\u000f pushed new topic: \u000302\u001f{2}\u000f\u000f";
				}
				case "WordPress2":
				{
					if(Language == "huHU")
						return "\u00033\u0002{0}\u000f\u000f \u0002Infó\u000f: {1}";
					else if(Language == "enUS")
						return "\u00033\u0002{0}\u000f\u000f \u0002Info\u000f: {1}";
					else
						return "\u00033\u0002{0}\u000f\u000f \u0002Info\u000f: {1}";
				}
				case "nocolorsWordPress":
				{
					if(Language == "huHU")
						return "\u0002[{0}\u000f\u0002]\u000f \u0002{1}\u000f küldött új témát: \u001f{2}\u000f";
					else if(Language == "enUS")
						return "\u0002[{0}\u000f\u0002]\u000f \u0002{1}\u000f pushed new topic: \u001f{2}\u000f";
					else
						return "\u0002[{0}\u000f\u0002]\u000f \u0002{1}\u000f pushed new topic: \u001f{2}\u000f";
				}
				case "nocolorsWordPress2":
				{
					if(Language == "huHU")
						return "\u0002{0}\u000f \u0002Infó\u000f: {1}";
					else if(Language == "enUS")
						return "\u0002{0}\u000f \u0002Info\u000f: {1}";
					else
						return "\u0002{0}\u000f \u0002Info\u000f: {1}";
				}
				default:
					return string.Empty;
			}
		}
	}
}