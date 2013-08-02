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
using System.Globalization;
using Schumix.Framework;
using Schumix.Framework.Localization;

namespace Schumix.Libraries.Extensions
{
	public static class DateTimeExtensions
	{
		private static readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;

		public static bool IsMonthName(this string Name)
		{
			return Name.IsMonthName(sLConsole.Locale.ToLocale());
		}

		public static bool IsMonthName(this string Name, string Locale)
		{
			try
			{
				var d = DateTime.ParseExact(Name, "MMMM", CultureInfo.GetCultureInfo(Locale.ToLocale())).Month;
				return true;
			}
			catch
			{
				return false;
			}
		}

		public static int GetMonthNameInInt(this string Name)
		{
			return Name.GetMonthNameInInt(sLConsole.Locale.ToLocale());
		}

		public static int GetMonthNameInInt(this string Name, string Locale)
		{
			try
			{
				return !Name.IsNumber() ? DateTime.ParseExact(Name, "MMMM", CultureInfo.GetCultureInfo(Locale.ToLocale())).Month : Name.ToInt32();
			}
			catch
			{
				return -1;
			}
		}

		public static string ToMonthFormat(this int Month)
		{
			return Month < 10 ? string.Format("0{0}", Month.ToString()) : Month.ToString();
		}

		public static string ToDayFormat(this int Day)
		{
			return Day < 10 ? string.Format("0{0}", Day.ToString()) : Day.ToString();
		}

		public static string ToHourFormat(this int Hour)
		{
			return Hour < 10 ? string.Format("0{0}", Hour.ToString()) : Hour.ToString();
		}

		public static string ToMinuteFormat(this int Minute)
		{
			return Minute < 10 ? string.Format("0{0}", Minute.ToString()) : Minute.ToString();
		}

		public static string ToSecondFormat(this int Second)
		{
			return Second < 10 ? string.Format("0{0}", Second.ToString()) : Second.ToString();
		}
	}
}