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

namespace Schumix.HgRssAddon.Localization
{
	sealed class PLocalization
	{
		public string Locale { get; set; }
		private PLocalization() {}

		public string HgRss(string Name)
		{
			return HgRss(Name, Locale);
		}

		public string HgRss(string Name, string Language)
		{
			switch(Name)
			{
				case "google":
				{
					if(Language == "huHU")
						return "[3{0}] {1} küldött új kommitot: 02{2}";
					else if(Language == "enUS")
						return "[3{0}] {1} pushed new commit: 02{2}";
					else
						return "[3{0}] {1} pushed new commit: 02{2}";
				}
				case "google2":
				{
					if(Language == "huHU")
						return "3{0} 10{1} {2}: {3}";
					else if(Language == "enUS")
						return "3{0} 10{1} {2}: {3}";
					else
						return "3{0} 10{1} {2}: {3}";
				}
				case "bitbucket":
				{
					if(Language == "huHU")
						return "[3{0}] {1} küldött új kommitot: 02{2}";
					else if(Language == "enUS")
						return "[3{0}] {1} pushed new commit: 02{2}";
					else
						return "[3{0}] {1} pushed new commit: 02{2}";
				}
				case "bitbucket2":
				{
					if(Language == "huHU")
						return "3{0} 10{1} {2}: {3}";
					else if(Language == "enUS")
						return "3{0} 10{1} {2}: {3}";
					else
						return "3{0} 10{1} {2}: {3}";
				}
				case "nocolorsgoogle":
				{
					if(Language == "huHU")
						return "[{0}] {1} küldött új kommitot: {2}";
					else if(Language == "enUS")
						return "[{0}] {1} pushed new commit: {2}";
					else
						return "[{0}] {1} pushed new commit: {2}";
				}
				case "nocolorsgoogle2":
				{
					if(Language == "huHU")
						return "{0} {1} {2}: {3}";
					else if(Language == "enUS")
						return "{0} {1} {2}: {3}";
					else
						return "{0} {1} {2}: {3}";
				}
				case "nocolorsbitbucket":
				{
					if(Language == "huHU")
						return "[{0}] {1} küldött új kommitot: {2}";
					else if(Language == "enUS")
						return "[{0}] {1} pushed new commit: {2}";
					else
						return "[{0}] {1} pushed new commit: {2}";
				}
				case "nocolorsbitbucket2":
				{
					if(Language == "huHU")
						return "{0} {1} {2}: {3}";
					else if(Language == "enUS")
						return "{0} {1} {2}: {3}";
					else
						return "{0} {1} {2}: {3}";
				}
				default:
					return string.Empty;
			}
		}
	}
}