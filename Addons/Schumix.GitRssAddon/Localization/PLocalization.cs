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

namespace Schumix.GitRssAddon.Localization
{
	sealed class PLocalization
	{
		public string Locale { get; set; }
		private PLocalization() {}

		public string GitRss(string Name)
		{
			return GitRss(Name, Locale);
		}

		public string GitRss(string Name, string Language)
		{
			switch(Name)
			{
				case "github":
				{
					if(Language == "huHU")
						return "[3{0}] {1} küldött új kommitot ide 7{2}: 02{3}";
					else if(Language == "enUS")
						return "[3{0}] {1} pushed new commit to 7{2}: 02{3}";
					else
						return "[3{0}] {1} pushed new commit to 7{2}: 02{3}";
				}
				case "github2":
				{
					if(Language == "huHU")
						return "3{0}/7{1} 10{2} {3}: {4}";
					else if(Language == "enUS")
						return "3{0}/7{1} 10{2} {3}: {4}";
					else
						return "3{0}/7{1} 10{2} {3}: {4}";
				}
				case "nocolorsgithub":
				{
					if(Language == "huHU")
						return "[{0}] {1} küldött új kommitot ide {2}: {3}";
					else if(Language == "enUS")
						return "[{0}] {1} pushed new commit to {2}: {3}";
					else
						return "[{0}] {1} pushed new commit to {2}: {3}";
				}
				case "nocolorsgithub2":
				{
					if(Language == "huHU")
						return "{0}/{1} {2} {3}: {4}";
					else if(Language == "enUS")
						return "{0}/{1} {2} {3}: {4}";
					else
						return "{0}/{1} {2} {3}: {4}";
				}
				case "gitweb":
				{
					if(Language == "huHU")
						return "[3{0}] {1} küldött új kommitot ide 7{2}: 02{3}";
					else if(Language == "enUS")
						return "[3{0}] {1} pushed new commit to 7{2}: 02{3}";
					else
						return "[3{0}] {1} pushed new commit to 7{2}: 02{3}";
				}
				case "gitweb2":
				{
					if(Language == "huHU")
						return "3{0}/7{1} 10{2} {3}: {4}";
					else if(Language == "enUS")
						return "3{0}/7{1} 10{2} {3}: {4}";
					else
						return "3{0}/7{1} 10{2} {3}: {4}";
				}
				case "nocolorsgitweb":
				{
					if(Language == "huHU")
						return "[{0}] {1} küldött új kommitot ide {2}: {3}";
					else if(Language == "enUS")
						return "[{0}] {1} pushed new commit to {2}: {3}";
					else
						return "[{0}] {1} pushed new commit to {2}: {3}";
				}
				case "nocolorsgitweb2":
				{
					if(Language == "huHU")
						return "{0}/{1} {2} {3}: {4}";
					else if(Language == "enUS")
						return "{0}/{1} {2} {3}: {4}";
					else
						return "{0}/{1} {2} {3}: {4}";
				}
				default:
					return string.Empty;
			}
		}
	}
}