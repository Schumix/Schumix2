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

namespace Schumix.SvnRssAddon.Localization
{
	sealed class PLocalization
	{
		public string Locale { get; set; }
		private PLocalization() {}

		public string SvnRss(string Name)
		{
			return SvnRss(Name, Locale);
		}

		public string SvnRss(string Name, string Language)
		{
			switch(Name)
			{
				case "assembla":
				{
					if(Language == "huHU")
						return "3{0} Revision: 10{1} beküldte: {2}";
					else if(Language == "enUS")
						return "3{0} Revision: 10{1} by {2}";
					else
						return "3{0} Revision: 10{1} by {2}";
				}
				case "assembla2":
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