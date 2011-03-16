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
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Schumix.Libraries
{
	public sealed class Lib
	{
		public Lib()
		{

		}

		public static bool IsPrime(long x)
		{
			x = Math.Abs(x);

			if(x == 1 || x == 0)
				return false;

			if(x == 2)
				return true;

			if(x % 2 == 0)
				return false;

			bool p = true;

			for(var i = 3; i <= Math.Floor(Math.Sqrt(x)); i += 2)
			{
				if(x % i == 0)
				{
					p = false;
					break;
				}
			}

			return p;
		}

		public static string Regex(string regex, string adat)
		{
			var x = new Regex(regex);

			if(x.IsMatch(adat))
			{
				string s = string.Empty;

				for(int a = 1; a < x.Match(adat).Length; a++)
					s += " " + x.Match(adat).Groups[a].ToString();

				s = s.Remove(0, 1);
				return s;
			}
			else
				return "Hibás regex!";
		}

		public static string Regex(string regex, string adat, string groups)
		{
			var x = new Regex(regex);

			if(x.IsMatch(adat))
				return x.Match(adat).Groups[groups].ToString();
			else
				return "Hibás regex!";
		}
	}
}

