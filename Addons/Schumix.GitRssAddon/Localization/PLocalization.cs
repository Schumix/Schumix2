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
						return "\u0002[\u00033{0}\u000f\u000f\u0002]\u000f \u0002{1}\u000f küldött új kommitot ide \u00037{2}\u000f: \u000302\u001f{3}\u000f\u000f";
					else if(Language == "enUS")
						return "\u0002[\u00033{0}\u000f\u000f\u0002]\u000f \u0002{1}\u000f pushed new commit to \u00037{2}\u000f: \u000302\u001f{3}\u000f\u000f";
					else
						return "\u0002[\u00033{0}\u000f\u000f\u0002]\u000f \u0002{1}\u000f pushed new commit to \u00037{2}\u000f: \u000302\u001f{3}\u000f\u000f";
				}
				case "github2":
				{
					if(Language == "huHU")
						return "\u00033\u0002{0}\u000f\u000f\u000315/\u000f\u00037{1}\u000f \u000310\u0002{2}\u000f\u000f \u0002{3}\u000f: {4}";
					else if(Language == "enUS")
						return "\u00033\u0002{0}\u000f\u000f\u000315/\u000f\u00037{1}\u000f \u000310\u0002{2}\u000f\u000f \u0002{3}\u000f: {4}";
					else
						return "\u00033\u0002{0}\u000f\u000f\u000315/\u000f\u00037{1}\u000f \u000310\u0002{2}\u000f\u000f \u0002{3}\u000f: {4}";
				}
				case "nocolorsgithub":
				{
					if(Language == "huHU")
						return "\u0002[{0}\u000f\u0002]\u000f \u0002{1}\u000f küldött új kommitot ide {2}: \u001f{3}\u000f";
					else if(Language == "enUS")
						return "\u0002[{0}\u000f\u0002]\u000f \u0002{1}\u000f pushed new commit to {2}: \u001f{3}\u000f";
					else
						return "\u0002[{0}\u000f\u0002]\u000f \u0002{1}\u000f pushed new commit to {2}: \u001f{3}\u000f";
				}
				case "nocolorsgithub2":
				{
					if(Language == "huHU")
						return "\u0002{0}\u000f/{1} \u0002{2}\u000f \u0002{3}\u000f: {4}";
					else if(Language == "enUS")
						return "\u0002{0}\u000f/{1} \u0002{2}\u000f \u0002{3}\u000f: {4}";
					else
						return "\u0002{0}\u000f/{1} \u0002{2}\u000f \u0002{3}\u000f: {4}";
				}
				case "gitweb":
				{
					if(Language == "huHU")
						return "\u0002[\u00033{0}\u000f\u000f\u0002]\u000f \u0002{1}\u000f küldött új kommitot ide \u00037{2}\u000f: \u000302\u001f{3}\u000f\u000f";
					else if(Language == "enUS")
						return "\u0002[\u00033{0}\u000f\u000f\u0002]\u000f \u0002{1}\u000f pushed new commit to \u00037{2}\u000f: \u000302\u001f{3}\u000f\u000f";
					else
						return "\u0002[\u00033{0}\u000f\u000f\u0002]\u000f \u0002{1}\u000f pushed new commit to \u00037{2}\u000f: \u000302\u001f{3}\u000f\u000f";
				}
				case "gitweb2":
				{
					if(Language == "huHU")
						return "\u00033\u0002{0}\u000f\u000f\u000315/\u000f\u00037{1}\u000f \u000310\u0002{2}\u000f\u000f \u0002{3}\u000f: {4}";
					else if(Language == "enUS")
						return "\u00033\u0002{0}\u000f\u000f\u000315/\u000f\u00037{1}\u000f \u000310\u0002{2}\u000f\u000f \u0002{3}\u000f: {4}";
					else
						return "\u00033\u0002{0}\u000f\u000f\u000315/\u000f\u00037{1}\u000f \u000310\u0002{2}\u000f\u000f \u0002{3}\u000f: {4}";
				}
				case "nocolorsgitweb":
				{
					if(Language == "huHU")
						return "\u0002[{0}\u000f\u0002]\u000f \u0002{1}\u000f küldött új kommitot ide {2}: \u001f{3}\u000f";
					else if(Language == "enUS")
						return "\u0002[{0}\u000f\u0002]\u000f \u0002{1}\u000f pushed new commit to {2}: \u001f{3}\u000f";
					else
						return "\u0002[{0}\u000f\u0002]\u000f \u0002{1}\u000f pushed new commit to {2}: \u001f{3}\u000f";
				}
				case "nocolorsgitweb2":
				{
					if(Language == "huHU")
						return "\u0002{0}\u000f/{1} \u0002{2}\u000f \u0002{3}\u000f: {4}";
					else if(Language == "enUS")
						return "\u0002{0}\u000f/{1} \u0002{2}\u000f \u0002{3}\u000f: {4}";
					else
						return "\u0002{0}\u000f/{1} \u0002{2}\u000f \u0002{3}\u000f: {4}";
				}
				default:
					return string.Empty;
			}
		}
	}
}