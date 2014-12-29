/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013-2015 Schumix Team <http://schumix.eu/>
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

namespace Schumix.Framework.Extensions
{
	public static class ConvertExtensions
	{
		public static double ToNumber(this string value)
		{
			double number;
			return double.TryParse(value, out number)? number : 0;
		}

		public static double ToNumber(this string value, int Else)
		{
			double number;
			return double.TryParse(value, out number)? number : Else;
		}

		public static byte[] FromBase64String(this string s)
		{
			return Convert.FromBase64String(s);
		}

		public static TypeCode GetTypeCode(this object value)
		{
			return Convert.GetTypeCode(value);
		}

		public static bool IsDBNull(this object value)
		{
			return Convert.IsDBNull(value);
		}

		public static bool ToBoolean(this bool value)
		{ 
			return Convert.ToBoolean(value);
		}

		public static bool ToBoolean(this byte value)
		{ 
			return Convert.ToBoolean(value);
		}

		public static bool ToBoolean(this char value)
		{
			return Convert.ToBoolean(value);
		}

		public static bool ToBoolean(this DateTime value)
		{
			return Convert.ToBoolean(value);
		}

		public static bool ToBoolean(this decimal value)
		{ 
			return Convert.ToBoolean(value);
		}

		public static bool ToBoolean(this double value)
		{ 
			return Convert.ToBoolean(value);
		}

		public static bool ToBoolean(this float value)
		{ 
			return Convert.ToBoolean(value);
		}

		public static bool ToBoolean(this int value)
		{ 
			return Convert.ToBoolean(value);
		}

		public static bool ToBoolean(this long value)
		{ 
			return Convert.ToBoolean(value);
		}

		[CLSCompliant(false)]
		public static bool ToBoolean(this sbyte value)
		{ 
			return Convert.ToBoolean(value);
		}

		public static bool ToBoolean(this short value)
		{ 
			return Convert.ToBoolean(value);
		}

		public static bool ToBoolean(this string value)
		{
			return Convert.ToBoolean(value);
		}

		public static bool ToBoolean(this string value, IFormatProvider provider)
		{
			return Convert.ToBoolean(value, provider);
		}

		[CLSCompliant(false)]
		public static bool ToBoolean(this uint value)
		{ 
			return Convert.ToBoolean(value);
		}

		[CLSCompliant(false)]
		public static bool ToBoolean(this ulong value)
		{ 
			return Convert.ToBoolean(value);
		}

		[CLSCompliant(false)]
		public static bool ToBoolean(this ushort value)
		{ 
			return Convert.ToBoolean(value);
		}

		public static bool ToBoolean(this object value)
		{
			return Convert.ToBoolean(value);
		}

		public static bool ToBoolean(this object value, IFormatProvider provider)
		{
			return Convert.ToBoolean(value, provider);
		}

		public static byte ToByte(this bool value)
		{ 
			return Convert.ToByte(value);
		}

		public static byte ToByte(this byte value)
		{ 
			return Convert.ToByte(value);
		}

		public static byte ToByte(this char value)
		{ 
			return Convert.ToByte(value);
		}

		public static byte ToByte(this DateTime value)
		{
			return Convert.ToByte(value);
		}

		public static byte ToByte(this decimal value)
		{ 
			return Convert.ToByte(value);
		}

		public static byte ToByte(this double value)
		{ 
			return Convert.ToByte(value);
		}

		public static byte ToByte(this float value)
		{ 
			return Convert.ToByte(value);
		}

		public static byte ToByte(this int value)
		{ 
			return Convert.ToByte(value);
		}

		public static byte ToByte(this long value)
		{ 
			return Convert.ToByte(value);
		}

		[CLSCompliant(false)]
		public static byte ToByte(this sbyte value)
		{ 
			return Convert.ToByte(value);
		}

		public static byte ToByte(this short value)
		{ 
			return Convert.ToByte(value);
		}

		public static byte ToByte(this string value)
		{
			return Convert.ToByte(value);
		}

		public static byte ToByte(this string value, IFormatProvider provider)
		{
			return Convert.ToByte(value, provider);
		}

		public static byte ToByte(this string value, int fromBase)
		{
			return Convert.ToByte(value, fromBase);
		}

		[CLSCompliant(false)]
		public static byte ToByte(this uint value)
		{ 
			return Convert.ToByte(value);
		}

		[CLSCompliant(false)]
		public static byte ToByte(this ulong value)
		{ 
			return Convert.ToByte(value);
		}

		[CLSCompliant(false)]
		public static byte ToByte(this ushort value)
		{ 
			return Convert.ToByte(value);
		}

		public static byte ToByte(this object value)
		{
			return Convert.ToByte(value);
		}

		public static byte ToByte(this object value, IFormatProvider provider)
		{
			return Convert.ToByte(value, provider);
		}

		public static char ToChar(this bool value)
		{
			return Convert.ToChar(value);
		}

		public static char ToChar(this byte value)
		{ 
			return Convert.ToChar(value);
		}

		public static char ToChar(this char value)
		{ 
			return Convert.ToChar(value);
		}

		public static char ToChar(this DateTime value)
		{
			return Convert.ToChar(value);
		}

		public static char ToChar(this decimal value)
		{
			return Convert.ToChar(value);
		}

		public static char ToChar(this double value)
		{
			return Convert.ToChar(value);
		}

		public static char ToChar(this int value)
		{ 
			return Convert.ToChar(value);
		}

		public static char ToChar(this long value)
		{ 
			return Convert.ToChar(value);
		}

		public static char ToChar(this float value)
		{
			return Convert.ToChar(value);
		}

		[CLSCompliant(false)]
		public static char ToChar(this sbyte value)
		{ 
			return Convert.ToChar(value);
		}

		public static char ToChar(this short value)
		{ 
			return Convert.ToChar(value);
		}

		public static char ToChar(this string value)
		{
			return Convert.ToChar(value);
		}

		public static char ToChar(this string value, IFormatProvider provider)
		{
			return Convert.ToChar(value, provider);
		}

		[CLSCompliant(false)]
		public static char ToChar(this uint value)
		{ 
			return Convert.ToChar(value);
		}

		[CLSCompliant(false)]
		public static char ToChar(this ulong value)
		{ 
			return Convert.ToChar(value);
		}

		[CLSCompliant(false)]
		public static char ToChar(this ushort value)
		{ 
			return Convert.ToChar(value);
		}

		public static char ToChar(this object value)
		{
			return Convert.ToChar(value);
		}

		public static char ToChar(this object value, IFormatProvider provider)
		{
			return Convert.ToChar(value, provider);
		}

		public static DateTime ToDateTime(this string value)
		{ 
			return Convert.ToDateTime(value);
		}

		public static DateTime ToDateTime(this string value, IFormatProvider provider)
		{
			return Convert.ToDateTime(value, provider);
		}

		public static DateTime ToDateTime(this bool value)
		{
			return Convert.ToDateTime(value);
		}

		public static DateTime ToDateTime(this byte value)
		{
			return Convert.ToDateTime(value);
		}

		public static DateTime ToDateTime(this char value)
		{
			return Convert.ToDateTime(value);
		}

		public static DateTime ToDateTime(this DateTime value)
		{
			return Convert.ToDateTime(value);
		}

		public static DateTime ToDateTime(this decimal value)
		{
			return Convert.ToDateTime(value);
		}

		public static DateTime ToDateTime(this double value)
		{
			return Convert.ToDateTime(value);
		}

		public static DateTime ToDateTime(this short value)
		{
			return Convert.ToDateTime(value);
		}

		public static DateTime ToDateTime(this int value)
		{
			return Convert.ToDateTime(value);
		}

		public static DateTime ToDateTime(this long value)
		{
			return Convert.ToDateTime(value);
		}

		public static DateTime ToDateTime(this float value)
		{
			return Convert.ToDateTime(value);
		}

		public static DateTime ToDateTime(this object value)
		{
			return Convert.ToDateTime(value);
		}

		public static DateTime ToDateTime(this object value, IFormatProvider provider)
		{
			return Convert.ToDateTime(value, provider);
		}

		[CLSCompliant(false)]
		public static DateTime ToDateTime(this sbyte value)
		{
			return Convert.ToDateTime(value);
		}

		[CLSCompliant(false)]
		public static DateTime ToDateTime(this ushort value)
		{
			return Convert.ToDateTime(value);
		}

		[CLSCompliant(false)]
		public static DateTime ToDateTime(this uint value)
		{
			return Convert.ToDateTime(value);
		}

		[CLSCompliant(false)]
		public static DateTime ToDateTime(this ulong value)
		{
			return Convert.ToDateTime(value);
		}

		public static decimal ToDecimal(this bool value)
		{ 
			return Convert.ToDecimal(value);
		}

		public static decimal ToDecimal(this byte value)
		{ 
			return Convert.ToDecimal(value);
		}

		public static decimal ToDecimal(this char value)
		{
			return Convert.ToDecimal(value);
		}

		public static decimal ToDecimal(this DateTime value)
		{
			return Convert.ToDecimal(value);
		}

		public static decimal ToDecimal(this decimal value)
		{ 
			return Convert.ToDecimal(value);
		}

		public static decimal ToDecimal(this double value)
		{ 
			return Convert.ToDecimal(value);
		}

		public static decimal ToDecimal(this float value)
		{
			return Convert.ToDecimal(value);
		}

		public static decimal ToDecimal(this int value)
		{ 
			return Convert.ToDecimal(value);
		}

		public static decimal ToDecimal(this long value)
		{ 
			return Convert.ToDecimal(value);
		}

		[CLSCompliant(false)]
		public static decimal ToDecimal(this sbyte value)
		{ 
			return Convert.ToDecimal(value);
		}

		public static decimal ToDecimal(this short value)
		{ 
			return Convert.ToDecimal(value);
		}

		public static decimal ToDecimal(this string value)
		{
			return Convert.ToDecimal(value);
		}

		public static decimal ToDecimal(this string value, IFormatProvider provider)
		{
			return Convert.ToDecimal(value, provider);
		}

		[CLSCompliant(false)]
		public static decimal ToDecimal(this uint value)
		{ 
			return Convert.ToDecimal(value);
		}

		[CLSCompliant(false)]
		public static decimal ToDecimal(this ulong value)
		{ 
			return Convert.ToDecimal(value);
		}

		[CLSCompliant(false)]
		public static decimal ToDecimal(this ushort value)
		{ 
			return Convert.ToDecimal(value);
		}

		public static decimal ToDecimal(this object value)
		{
			return Convert.ToDecimal(value);
		}

		public static decimal ToDecimal(this object value, IFormatProvider provider)
		{
			return Convert.ToDecimal(value, provider);
		}

		public static double ToDouble(this bool value)
		{ 
			return Convert.ToDouble(value);
		}

		public static double ToDouble(this byte value)
		{ 
			return Convert.ToDouble(value);
		}

		public static double ToDouble(this char value)
		{
			return Convert.ToDouble(value);
		}

		public static double ToDouble(this DateTime value)
		{
			return Convert.ToDouble(value);
		}

		public static double ToDouble(this decimal value)
		{ 
			return Convert.ToDouble(value);
		}

		public static double ToDouble(this double value)
		{ 
			return Convert.ToDouble(value);
		}

		public static double ToDouble(this float value)
		{ 
			return Convert.ToDouble(value);
		}

		public static double ToDouble(this int value)
		{ 
			return Convert.ToDouble(value);
		}

		public static double ToDouble(this long value)
		{ 
			return Convert.ToDouble(value);
		}

		[CLSCompliant(false)]
		public static double ToDouble(this sbyte value)
		{ 
			return Convert.ToDouble(value);
		}

		public static double ToDouble(this short value)
		{ 
			return Convert.ToDouble(value);
		}

		public static double ToDouble(this string value)
		{
			return Convert.ToDouble(value);
		}

		public static double ToDouble(this string value, IFormatProvider provider)
		{
			return Convert.ToDouble(value, provider);
		}

		[CLSCompliant(false)]
		public static double ToDouble(this uint value)
		{ 
			return Convert.ToDouble(value);
		}

		[CLSCompliant(false)]
		public static double ToDouble(this ulong value)
		{ 
			return Convert.ToDouble(value);
		}

		[CLSCompliant(false)]
		public static double ToDouble(this ushort value)
		{ 
			return Convert.ToDouble(value);
		}

		public static double ToDouble(this object value)
		{
			return Convert.ToDouble(value);
		}

		public static double ToDouble(this object value, IFormatProvider provider)
		{
			return Convert.ToDouble(value, provider);
		}

		public static short ToInt16(this bool value)
		{ 
			return Convert.ToInt16(value);
		}

		public static short ToInt16(this byte value)
		{ 
			return Convert.ToInt16(value);
		}

		public static short ToInt16(this char value)
		{
			return Convert.ToInt16(value);
		}

		public static short ToInt16(this DateTime value)
		{
			return Convert.ToInt16(value);
		}

		public static short ToInt16(this decimal value)
		{ 
			return Convert.ToInt16(value);
		}

		public static short ToInt16(this double value)
		{ 
			return Convert.ToInt16(value);
		}

		public static short ToInt16(this float value)
		{ 
			return Convert.ToInt16(value);
		}

		public static short ToInt16(this int value)
		{ 
			return Convert.ToInt16(value);
		}

		public static short ToInt16(this long value)
		{ 
			return Convert.ToInt16(value);
		}

		[CLSCompliant(false)]
		public static short ToInt16(this sbyte value)
		{ 
			return Convert.ToInt16(value);
		}

		public static short ToInt16(this short value)
		{ 
			return Convert.ToInt16(value);
		}

		public static short ToInt16(this string value)
		{
			return Convert.ToInt16(value);
		}

		public static short ToInt16(this string value, IFormatProvider provider)
		{
			return Convert.ToInt16(value, provider);
		}

		public static short ToInt16(this string value, int fromBase)
		{
			return Convert.ToInt16(value, fromBase);
		}

		[CLSCompliant(false)]
		public static short ToInt16(this uint value)
		{ 
			return Convert.ToInt16(value);
		}

		[CLSCompliant(false)]
		public static short ToInt16(this ulong value)
		{ 
			return Convert.ToInt16(value);
		}

		[CLSCompliant(false)]
		public static short ToInt16(this ushort value)
		{ 
			return Convert.ToInt16(value);
		}

		public static short ToInt16(this object value)
		{
			return Convert.ToInt16(value);
		}

		public static short ToInt16(this object value, IFormatProvider provider)
		{
			return Convert.ToInt16(value, provider);
		}

		public static int ToInt32(this bool value)
		{ 
			return Convert.ToInt32(value);
		}

		public static int ToInt32(this byte value)
		{ 
			return Convert.ToInt32(value);
		}

		public static int ToInt32(this char value)
		{ 
			return Convert.ToInt32(value);
		}

		public static int ToInt32(this DateTime value)
		{
			return Convert.ToInt32(value);
		}

		public static int ToInt32(this decimal value)
		{ 
			return Convert.ToInt32(value);
		}

		public static int ToInt32(this double value)
		{ 
			return Convert.ToInt32(value);
		}

		public static int ToInt32(this float value)
		{ 
			return Convert.ToInt32(value);
		}

		public static int ToInt32(this int value)
		{ 
			return Convert.ToInt32(value);
		}

		public static int ToInt32(this long value)
		{ 
			return Convert.ToInt32(value);
		}

		[CLSCompliant(false)]
		public static int ToInt32(this sbyte value)
		{ 
			return Convert.ToInt32(value);
		}

		public static int ToInt32(this short value)
		{ 
			return Convert.ToInt32(value);
		}

		public static int ToInt32(this string value)
		{
			return Convert.ToInt32(value);
		}

		public static int ToInt32(this string value, IFormatProvider provider)
		{
			return Convert.ToInt32(value);
		}

		public static int ToInt32(this string value, int fromBase)
		{
			return Convert.ToInt32(value, fromBase);
		}

		[CLSCompliant(false)]
		public static int ToInt32(this uint value)
		{ 
			return Convert.ToInt32(value);
		}

		[CLSCompliant(false)]
		public static int ToInt32(this ulong value)
		{ 
			return Convert.ToInt32(value);
		}

		[CLSCompliant(false)]
		public static int ToInt32(this ushort value)
		{ 
			return Convert.ToInt32(value);
		}

		public static int ToInt32(this object value)
		{
			return Convert.ToInt32(value);
		}

		public static int ToInt32(this object value, IFormatProvider provider)
		{
			return Convert.ToInt32(value, provider);
		}

		public static long ToInt64(this bool value)
		{ 
			return Convert.ToInt64(value);
		}

		public static long ToInt64(this byte value)
		{ 
			return Convert.ToInt64(value);
		}

		public static long ToInt64(this char value)
		{ 
			return Convert.ToInt64(value);
		}

		public static long ToInt64(this DateTime value)
		{
			return Convert.ToInt64(value);
		}

		public static long ToInt64(this decimal value)
		{ 
			return Convert.ToInt64(value);
		}

		public static long ToInt64(this double value)
		{ 
			return Convert.ToInt64(value);
		}

		public static long ToInt64(this float value)
		{ 
			return Convert.ToInt64(value);
		}

		public static long ToInt64(this int value)
		{ 
			return Convert.ToInt64(value);
		}

		public static long ToInt64(this long value)
		{ 
			return Convert.ToInt64(value);
		}

		[CLSCompliant(false)]
		public static long ToInt64(this sbyte value)
		{ 
			return Convert.ToInt64(value);
		}

		public static long ToInt64(this short value)
		{ 
			return Convert.ToInt64(value);
		}

		public static long ToInt64(this string value)
		{
			return Convert.ToInt64(value);
		}

		public static long ToInt64(this string value, IFormatProvider provider)
		{
			return Convert.ToInt64(value);
		}

		public static long ToInt64(this string value, int fromBase)
		{
			return Convert.ToInt64(value, fromBase);
		}

		[CLSCompliant(false)]
		public static long ToInt64(this uint value)
		{ 
			return Convert.ToInt64(value);
		}

		[CLSCompliant(false)]
		public static long ToInt64(this ulong value)
		{ 
			return Convert.ToInt64(value);
		}

		[CLSCompliant(false)]
		public static long ToInt64(this ushort value)
		{ 
			return Convert.ToInt64(value);
		}

		public static long ToInt64(this object value)
		{
			return Convert.ToInt64(value);
		}

		public static long ToInt64(this object value, IFormatProvider provider)
		{
			return Convert.ToInt64(value, provider);
		}

		[CLSCompliant(false)]
		public static sbyte ToSByte(this bool value)
		{ 
			return Convert.ToSByte(value);
		}

		[CLSCompliant(false)]
		public static sbyte ToSByte(this byte value)
		{ 
			return Convert.ToSByte(value);
		}

		[CLSCompliant(false)]
		public static sbyte ToSByte(this char value)
		{ 
			return Convert.ToSByte(value);
		}

		[CLSCompliant(false)]
		public static sbyte ToSByte(this DateTime value)
		{
			return Convert.ToSByte(value);
		}

		[CLSCompliant(false)]	
		public static sbyte ToSByte(this decimal value)
		{ 
			return Convert.ToSByte(value);
		}

		[CLSCompliant(false)]
		public static sbyte ToSByte(this double value)
		{ 
			return Convert.ToSByte(value);
		}

		[CLSCompliant(false)]
		public static sbyte ToSByte(this float value)
		{ 
			return Convert.ToSByte(value);
		}

		[CLSCompliant(false)]
		public static sbyte ToSByte(this int value)
		{ 
			return Convert.ToSByte(value);
		}

		[CLSCompliant(false)]
		public static sbyte ToSByte(this long value)
		{ 
			return Convert.ToSByte(value);
		}

		[CLSCompliant(false)]
		public static sbyte ToSByte(this sbyte value)
		{ 
			return Convert.ToSByte(value);
		}

		[CLSCompliant(false)]
		public static sbyte ToSByte(this short value)
		{ 
			return Convert.ToSByte(value);
		}

		[CLSCompliant(false)]
		public static sbyte ToSByte(this string value)
		{
			return Convert.ToSByte(value);
		}

		[CLSCompliant(false)]
		public static sbyte ToSByte(this string value, IFormatProvider provider)
		{
			return Convert.ToSByte(value, provider);
		}

		[CLSCompliant(false)]
		public static sbyte ToSByte(this string value, int fromBase)
		{
			return Convert.ToSByte(value, fromBase);
		}

		[CLSCompliant(false)]
		public static sbyte ToSByte(this uint value)
		{ 
			return Convert.ToSByte(value);
		}

		[CLSCompliant(false)]
		public static sbyte ToSByte(this ulong value)
		{ 
			return Convert.ToSByte(value);
		}

		[CLSCompliant(false)]
		public static sbyte ToSByte(this ushort value)
		{ 
			return Convert.ToSByte(value);
		}

		[CLSCompliant(false)]
		public static sbyte ToSByte(this object value)
		{
			return Convert.ToSByte(value);
		}

		[CLSCompliant(false)]
		public static sbyte ToSByte(this object value, IFormatProvider provider)
		{
			return Convert.ToSByte(value, provider);
		}

		public static float ToSingle(this bool value)
		{ 
			return Convert.ToSingle(value);
		}

		public static float ToSingle(this byte value)
		{ 
			return Convert.ToSingle(value);
		}

		public static float ToSingle(this Char value)
		{
			return Convert.ToSingle(value);
		}

		public static float ToSingle(this DateTime value)
		{
			return Convert.ToSingle(value);
		}

		public static float ToSingle(this decimal value)
		{ 
			return Convert.ToSingle(value);
		}

		public static float ToSingle(this double value)
		{ 
			return Convert.ToSingle(value);
		}

		public static float ToSingle(this float value)
		{ 
			return Convert.ToSingle(value);
		}

		public static float ToSingle(this int value)
		{ 
			return Convert.ToSingle(value);
		}

		public static float ToSingle(this long value)
		{ 
			return Convert.ToSingle(value);
		}

		[CLSCompliant(false)]
		public static float ToSingle(this sbyte value)
		{ 
			return Convert.ToSingle(value);
		}

		public static float ToSingle(this short value)
		{ 
			return Convert.ToSingle(value);
		}

		public static float ToSingle(this string value)
		{
			return Convert.ToSingle(value);
		}

		public static float ToSingle(this string value, IFormatProvider provider)
		{
			return Convert.ToSingle(value, provider);
		}	       

		[CLSCompliant(false)]
		public static float ToSingle(this uint value)
		{ 
			return Convert.ToSingle(value);
		}

		[CLSCompliant(false)]
		public static float ToSingle(this ulong value)
		{ 
			return Convert.ToSingle(value);
		}

		[CLSCompliant(false)]
		public static float ToSingle(this ushort value)
		{ 
			return Convert.ToSingle(value);
		}

		public static float ToSingle(this object value)
		{
			return Convert.ToSingle(value);
		}

		public static float ToSingle(this object value, IFormatProvider provider)
		{
			return Convert.ToSingle(value, provider);
		}

		public static string ToString(this bool value)
		{ 
			return Convert.ToString(value);
		}

		public static string ToString(this bool value, IFormatProvider provider)
		{
			return Convert.ToString(value, provider);
		}

		public static string ToString(this byte value)
		{ 
			return Convert.ToString(value);
		}

		public static string ToString(this byte value, IFormatProvider provider)
		{
			return Convert.ToString(value, provider);
		}

		public static string ToString(this byte value, int toBase)
		{
			return Convert.ToString(value, toBase);
		}

		public static string ToString(this char value)
		{ 
			return Convert.ToString(value);
		}

		public static string ToString(this char value, IFormatProvider provider)
		{
			return Convert.ToString(value, provider);
		}

		public static string ToString(this DateTime value)
		{ 
			return Convert.ToString(value);
		}

		public static string ToString(this DateTime value, IFormatProvider provider)
		{ 
			return Convert.ToString(value, provider);
		}

		public static string ToString(this decimal value)
		{
			return Convert.ToString(value);
		}

		public static string ToString(this decimal value, IFormatProvider provider)
		{ 
			return Convert.ToString(value, provider);
		}

		public static string ToString(this double value)
		{ 
			return Convert.ToString(value);
		}

		public static string ToString(this double value, IFormatProvider provider)
		{ 
			return Convert.ToString(value, provider);
		}

		public static string ToString(this float value)
		{ 
			return Convert.ToString(value);
		}

		public static string ToString(this float value, IFormatProvider provider)
		{ 
			return Convert.ToString(value, provider);
		}

		public static string ToString(this int value)
		{ 
			return Convert.ToString(value);
		}

		public static string ToString(this int value, int toBase)
		{
			return Convert.ToString(value, toBase);
		}

		public static string ToString(this int value, IFormatProvider provider)
		{ 
			return Convert.ToString(value, provider);
		}

		public static string ToString(this long value)
		{ 
			return Convert.ToString(value);
		}

		public static string ToString(this long value, int toBase)
		{
			return Convert.ToString(value, toBase);
		}

		public static string ToString(this long value, IFormatProvider provider)
		{ 
			return Convert.ToString(value, provider);
		}

		public static string ToString(this object value)
		{
			return Convert.ToString(value);
		}		

		public static string ToString(this object value, IFormatProvider provider)
		{
			return Convert.ToString(value, provider);
		}				

		[CLSCompliant(false)]
		public static string ToString(this sbyte value)
		{ 
			return Convert.ToString(value);
		}

		[CLSCompliant(false)]				
		public static string ToString(this sbyte value, IFormatProvider provider)
		{ 
			return Convert.ToString(value, provider);
		}

		public static string ToString(this short value)
		{ 
			return Convert.ToString(value);
		}

		public static string ToString(this short value, int toBase)
		{
			return Convert.ToString(value, toBase);
		}

		public static string ToString(this short value, IFormatProvider provider)
		{ 
			return Convert.ToString(value, provider);
		}

		public static string ToString(this string value)
		{
			return Convert.ToString(value);
		}

		public static string ToString(this string value, IFormatProvider provider)
		{
			return Convert.ToString(value, provider);
		}

		[CLSCompliant(false)]
		public static string ToString(this uint value)
		{ 
			return Convert.ToString(value);
		}

		[CLSCompliant(false)]
		public static string ToString(this uint value, IFormatProvider provider)
		{ 
			return Convert.ToString(value, provider);
		}

		[CLSCompliant(false)]
		public static string ToString(this ulong value)
		{ 
			return Convert.ToString(value);
		}

		[CLSCompliant(false)]
		public static string ToString(this ulong value, IFormatProvider provider)
		{ 
			return Convert.ToString(value, provider);
		}

		[CLSCompliant(false)]
		public static string ToString(this ushort value)
		{ 
			return Convert.ToString(value);
		}

		[CLSCompliant(false)]
		public static string ToString(this ushort value, IFormatProvider provider)
		{ 
			return Convert.ToString(value, provider);
		}

		[CLSCompliant(false)]
		public static ushort ToUInt16(this bool value)
		{ 
			return Convert.ToUInt16(value);
		}

		[CLSCompliant(false)]
		public static ushort ToUInt16(this byte value)
		{ 
			return Convert.ToUInt16(value);
		}

		[CLSCompliant(false)]
		public static ushort ToUInt16(this char value)
		{ 
			return Convert.ToUInt16(value);
		}

		[CLSCompliant(false)]
		public static ushort ToUInt16(this DateTime value)
		{
			return Convert.ToUInt16(value);
		}

		[CLSCompliant(false)]
		public static ushort ToUInt16(this decimal value)
		{ 
			return Convert.ToUInt16(value);
		}

		[CLSCompliant(false)]
		public static ushort ToUInt16(this double value)
		{ 
			return Convert.ToUInt16(value);
		}

		[CLSCompliant(false)]
		public static ushort ToUInt16(this float value)
		{ 
			return Convert.ToUInt16(value);
		}

		[CLSCompliant(false)]
		public static ushort ToUInt16(this int value)
		{ 
			return Convert.ToUInt16(value);
		}

		[CLSCompliant(false)]
		public static ushort ToUInt16(this long value)
		{ 
			return Convert.ToUInt16(value);
		}

		[CLSCompliant(false)]
		public static ushort ToUInt16(this sbyte value)
		{ 
			return Convert.ToUInt16(value);
		}

		[CLSCompliant(false)]
		public static ushort ToUInt16(this short value)
		{ 
			return Convert.ToUInt16(value);
		}

		[CLSCompliant(false)]
		public static ushort ToUInt16(this string value)
		{
			return Convert.ToUInt16(value);
		}

		[CLSCompliant(false)]
		public static ushort ToUInt16(this string value, IFormatProvider provider)
		{
			return Convert.ToUInt16(value, provider);
		}

		[CLSCompliant(false)]
		public static ushort ToUInt16(this string value, int fromBase)
		{
			return Convert.ToUInt16(value, fromBase);
		} 

		[CLSCompliant(false)]
		public static ushort ToUInt16(this uint value)
		{ 
			return Convert.ToUInt16(value);
		}

		[CLSCompliant(false)]
		public static ushort ToUInt16(this ulong value)
		{ 
			return Convert.ToUInt16(value);
		}

		[CLSCompliant(false)]
		public static ushort ToUInt16(this ushort value)
		{ 
			return Convert.ToUInt16(value);
		}

		[CLSCompliant(false)]
		public static ushort ToUInt16(this object value)
		{
			return Convert.ToUInt16(value);
		}

		[CLSCompliant(false)]
		public static ushort ToUInt16(this object value, IFormatProvider provider)
		{
			return Convert.ToUInt16(value, provider);
		}

		[CLSCompliant(false)]
		public static uint ToUInt32(this bool value)
		{ 
			return Convert.ToUInt32(value);
		}

		[CLSCompliant(false)]
		public static uint ToUInt32(this byte value)
		{ 
			return Convert.ToUInt32(value);
		}

		[CLSCompliant(false)]
		public static uint ToUInt32(this char value)
		{ 
			return Convert.ToUInt32(value);
		}

		[CLSCompliant(false)]
		public static uint ToUInt32(this DateTime value)
		{
			return Convert.ToUInt32(value);
		}

		[CLSCompliant(false)]
		public static uint ToUInt32(this decimal value)
		{ 
			return Convert.ToUInt32(value);
		}

		[CLSCompliant(false)]
		public static uint ToUInt32(this double value)
		{ 
			return Convert.ToUInt32(value);
		}

		[CLSCompliant(false)]
		public static uint ToUInt32(this float value)
		{ 
			return Convert.ToUInt32(value);
		}

		[CLSCompliant(false)]
		public static uint ToUInt32(this int value)
		{ 
			return Convert.ToUInt32(value);
		}

		[CLSCompliant(false)]
		public static uint ToUInt32(this long value)
		{ 
			return Convert.ToUInt32(value);
		}

		[CLSCompliant(false)]
		public static uint ToUInt32(this sbyte value)
		{ 
			return Convert.ToUInt32(value);
		}

		[CLSCompliant(false)]
		public static uint ToUInt32(this short value)
		{ 
			return Convert.ToUInt32(value);
		}

		[CLSCompliant(false)]
		public static uint ToUInt32(this string value)
		{
			return Convert.ToUInt32(value);
		}

		[CLSCompliant(false)]
		public static uint ToUInt32(this string value, IFormatProvider provider)
		{
			return Convert.ToUInt32(value, provider);
		}

		[CLSCompliant(false)]
		public static uint ToUInt32(this string value, int fromBase)
		{
			return Convert.ToUInt32(value, fromBase);
		}

		[CLSCompliant(false)]
		public static uint ToUInt32(this uint value)
		{ 
			return Convert.ToUInt32(value);
		}

		[CLSCompliant(false)]
		public static uint ToUInt32(this ulong value)
		{ 
			return Convert.ToUInt32(value);
		}

		[CLSCompliant(false)]
		public static uint ToUInt32(this ushort value)
		{ 
			return Convert.ToUInt32(value);
		}

		[CLSCompliant(false)]
		public static uint ToUInt32(this object value)
		{
			return Convert.ToUInt32(value);
		}		

		[CLSCompliant(false)]
		public static uint ToUInt32(this object value, IFormatProvider provider)
		{
			return Convert.ToUInt32(value, provider);
		}		

		[CLSCompliant(false)]
		public static ulong ToUInt64(this bool value)
		{ 
			return Convert.ToUInt64(value);
		}

		[CLSCompliant(false)]
		public static ulong ToUInt64(this byte value)
		{ 
			return Convert.ToUInt64(value);
		}

		[CLSCompliant(false)]
		public static ulong ToUInt64(this char value)
		{ 
			return Convert.ToUInt64(value);
		}

		[CLSCompliant(false)]
		public static ulong ToUInt64(this DateTime value)
		{
			return Convert.ToUInt64(value);
		}

		[CLSCompliant(false)]
		public static ulong ToUInt64(this decimal value)
		{ 
			return Convert.ToUInt64(value);
		}

		[CLSCompliant(false)]
		public static ulong ToUInt64(this double value)
		{ 
			return Convert.ToUInt64(value);
		}

		[CLSCompliant(false)] 
		public static ulong ToUInt64(this float value)
		{ 
			return Convert.ToUInt64(value);
		}

		[CLSCompliant(false)]
		public static ulong ToUInt64(this int value)
		{ 
			return Convert.ToUInt64(value);
		}

		[CLSCompliant(false)]
		public static ulong ToUInt64(this long value)
		{ 
			return Convert.ToUInt64(value);
		}

		[CLSCompliant(false)]
		public static ulong ToUInt64(this sbyte value)
		{ 
			return Convert.ToUInt64(value);
		}

		[CLSCompliant(false)]	
		public static ulong ToUInt64(this short value)
		{ 
			return Convert.ToUInt64(value);
		}

		[CLSCompliant(false)]
		public static ulong ToUInt64(this string value)
		{
			return Convert.ToUInt64(value);
		}

		[CLSCompliant(false)]
		public static ulong ToUInt64(this string value, IFormatProvider provider)
		{
			return Convert.ToUInt64(value, provider);
		}

		[CLSCompliant(false)]
		public static ulong ToUInt64(this string value, int fromBase)
		{
			return Convert.ToUInt64(value, fromBase);
		}

		[CLSCompliant(false)]
		public static ulong ToUInt64(this uint value)
		{ 
			return Convert.ToUInt64(value);
		}

		[CLSCompliant(false)]
		public static ulong ToUInt64(this ulong value)
		{ 
			return Convert.ToUInt64(value);
		}

		[CLSCompliant(false)]
		public static ulong ToUInt64(this ushort value)
		{ 
			return Convert.ToUInt64(value);
		}

		[CLSCompliant(false)]
		public static ulong ToUInt64(this object value)
		{
			return Convert.ToUInt64(value);
		}		

		[CLSCompliant(false)]
		public static ulong ToUInt64(this object value, IFormatProvider provider)
		{
			return Convert.ToUInt64(value, provider);
		}		

		public static object ChangeType(this object value, Type conversionType)
		{
			return Convert.ChangeType(value, conversionType);
		}

		public static object ChangeType(this object value, TypeCode typeCode)
		{
			return Convert.ChangeType(value, typeCode);
		}

		public static object ChangeType(this object value, Type conversionType, IFormatProvider provider)
		{
			return Convert.ChangeType(value, conversionType, provider);
		}

		public static object ChangeType(this object value, TypeCode typeCode, IFormatProvider provider)
		{
			return Convert.ChangeType(value, typeCode, provider);
		}
	}
}