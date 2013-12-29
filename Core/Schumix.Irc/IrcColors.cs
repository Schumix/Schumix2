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

namespace Schumix.Irc
{
	/// <summary>
	/// Class holding some constant values for IRC.
	/// </summary>
	public static class IrcColors
	{
		/// <summary>
		/// Black irc color.
		/// </summary>
		public const string Black = "\u000301";
		/// <summary>
		/// Blie irc color.
		/// </summary>
		public const string Blue = "\u000312";
		/// <summary>
		/// Bold irc color.
		/// </summary>
		public const string Bold = "\u0002";
		/// <summary>
		/// Brown irc color.
		/// </summary>
		public const string Brown = "\u000305";
		/// <summary>
		/// Cyan irc color.
		/// </summary>
		public const string Cyan = "\u000311";
		/// <summary>
		/// Dark blue irc color.
		/// </summary>
		public const string DarkBlue = "\u000302";
		/// <summary>
		/// Dark gray irc color.
		/// </summary>
		public const string DarkGray = "\u000314";
		/// <summary>
		/// Dark green irc color.
		/// </summary>
		public const string DarkGreen = "\u000303";
		/// <summary>
		/// Green irc color.
		/// </summary>
		public const string Green = "\u000309";
		/// <summary>
		/// Light gray irc color.
		/// </summary>
		public const string LightGray = "\u000315";
		/// <summary>
		/// Magenta irc color.
		/// </summary>
		public const string Magenta = "\u000313";
		/// <summary>
		/// Normal irc color.
		/// </summary>
		public const string Normal = "\u000f";
		/// <summary>
		/// Olive irc color.
		/// </summary>
		public const string Olive = "\u000307";
		/// <summary>
		/// Purple irc color.
		/// </summary>
		public const string Purple = "\u000306";
		/// <summary>
		/// Red irc color.
		/// </summary>
		public const string Red = "\u000304";
		/// <summary>
		/// Reversed irc color.
		/// </summary>
		public const string Reverse = "\u0016";
		/// <summary>
		/// Teal irc color.
		/// </summary>
		public const string Teal = "\u000310";
		/// <summary>
		/// Underlined irc color.
		/// </summary>
		public const string Underline = "\u001f";
		/// <summary>
		/// White irc color.
		/// </summary>
		public const string White = "\u000300";
		/// <summary>
		/// Yellow irc color.
		/// </summary>
		public const string Yellow = "\u000308";

		public static string GetColor(string Color)
		{
			switch(Color.ToLower())
			{
				case "black":
					return Black;
				case "blue":
					return Blue;
				case "bold":
					return Bold;
				case "brown":
					return Brown;
				case "cyan":
					return Cyan;
				case "darkblue":
					return DarkBlue;
				case "darkgray":
					return DarkGray;
				case "darkgreen":
					return DarkGreen;
				case "green":
					return Green;
				case "lightgray":
					return LightGray;
				case "magenta":
					return Magenta;
				case "normal":
					return Normal;
				case "olive":
					return Olive;
				case "purple":
					return Purple;
				case "red":
					return Red;
				case "reverse":
					return Reverse;
				case "teal":
					return Teal;
				case "underline":
					return Underline;
				case "white":
					return White;
				case "yellow":
					return Yellow;
				default:
					return Normal;
			}
		}
	}
}