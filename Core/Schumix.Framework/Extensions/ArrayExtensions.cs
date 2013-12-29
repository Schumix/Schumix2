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
using System.Linq;
using System.Collections.Generic;

namespace Schumix.Framework.Extensions
{
	public static class ArrayExtensions
	{
		public static string SplitToString(this string[] split, char c)
		{
			string ss = string.Empty;

			for(int x = 0; x < split.Length; x++)
				ss += c + split[x];

			if(ss.Length > 0 && ss.Substring(0, c.ToString().Length) == c.ToString())
				ss = ss.Remove(0, c.ToString().Length);

			return ss;
		}

		public static string SplitToString(this string[] split, int min, char c)
		{
			string ss = string.Empty;

			for(int x = min; x < split.Length; x++)
				ss += c + split[x];

			if(ss.Length > 0 && ss.Substring(0, c.ToString().Length) == c.ToString())
				ss = ss.Remove(0, c.ToString().Length);

			return ss;
		}

		public static string SplitToString(this string[] split)
		{
			string ss = string.Empty;

			foreach(var s in split)
				ss += s;

			return ss;
		}

		public static string SplitToString(this string[] split, string s)
		{
			string ss = string.Empty;

			for(int x = 0; x < split.Length; x++)
				ss += s + split[x];

			if(ss.Length > 0 && ss.Substring(0, s.Length) == s)
				ss = ss.Remove(0, s.Length);

			return ss;
		}

		public static string SplitToString(this string[] split, int min, string s)
		{
			string ss = string.Empty;

			for(int x = min; x < split.Length; x++)
				ss += s + split[x];

			if(ss.Length > 0 && ss.Substring(0, s.Length) == s)
				ss = ss.Remove(0, s.Length);

			return ss;
		}

		public static string SplitToString(this char[] split, char c)
		{
			string ss = string.Empty;

			for(int x = 0; x < split.Length; x++)
				ss += c + split[x];

			if(ss.Length > 0 && ss.Substring(0, c.ToString().Length) == c.ToString())
				ss = ss.Remove(0, c.ToString().Length);

			return ss;
		}

		public static string SplitToString(this char[] split, int min, char c)
		{
			string ss = string.Empty;

			for(int x = min; x < split.Length; x++)
				ss += c + split[x];

			if(ss.Length > 0 && ss.Substring(0, c.ToString().Length) == c.ToString())
				ss = ss.Remove(0, c.ToString().Length);

			return ss;
		}

		public static string SplitToString(this char[] split)
		{
			string ss = string.Empty;

			foreach(var s in split)
				ss += s;

			return ss;
		}

		public static string SplitToString(this char[] split, string s)
		{
			string ss = string.Empty;

			for(int x = 0; x < split.Length; x++)
				ss += s + split[x];

			if(ss.Length > 0 && ss.Substring(0, s.Length) == s)
				ss = ss.Remove(0, s.Length);

			return ss;
		}

		public static string SplitToString(this char[] split, int min, string s)
		{
			string ss = string.Empty;

			for(int x = min; x < split.Length; x++)
				ss += s + split[x];

			if(ss.Length > 0 && ss.Substring(0, s.Length) == s)
				ss = ss.Remove(0, s.Length);

			return ss;
		}

		public static string[] SplitAndTrim(this string list)
		{
			if(list.IsNullOrEmpty())
				return new string[0];

			return (from f in list.Split(SchumixBase.Comma) let trimmed = f.Trim() where !trimmed.Length.IsNull() select trimmed).ToArray();
		}

		
		public static bool CompareDataInBlock(this string[] split)
		{
			int i = 0;
			string ss = string.Empty;

			foreach(var s in split)
			{
				if(i == 0)
					ss = s;
				else
				{
					if(ss != s)
						return false;
				}

				i++;
			}

			return true;
		}

		public static bool CompareDataInBlock<T>(this List<T> list)
		{
			int i = 0;
			string ss = string.Empty;

			foreach(var s in list)
			{
				if(i == 0)
					ss = s.ToString();
				else
				{
					if(ss != s.ToString())
						return false;
				}

				i++;
			}

			return true;
		}
	}
}