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
using System.Collections;
using Schumix.Framework.Extensions;

namespace Schumix.Framework.CodeBureau
{
	/// <summary>
	/// Helper class for working with 'extended' enums using <see cref="StringValueAttribute"/> attributes.
	/// </summary>
	public class StringEnum
	{
		private static Hashtable _stringValues = new Hashtable();
		private Type _enumType;

		/// <summary>
		/// Creates a new <see cref="StringEnum"/> instance.
		/// </summary>
		/// <param name="enumType">Enum type.</param>
		public StringEnum(Type enumType)
		{
			if(!enumType.IsEnum)
				throw new ArgumentException(String.Format("Supplied type must be an Enum. Type was {0}", enumType.ToString()));
			
			_enumType = enumType;
		}

		/// <summary>
		/// Gets the string value associated with the given enum value.
		/// </summary>
		/// <param name="valueName">Name of the enum value.</param>
		/// <returns>String Value</returns>
		public string GetStringValue(string valueName)
		{
			string stringValue = string.Empty;

			try
			{
				var enumType = (Enum)Enum.Parse(_enumType, valueName);
				stringValue = GetStringValue(enumType);
			}
			catch(Exception)
			{
				// Swallow!
			}

			return stringValue;
		}

		/// <summary>
		/// Gets the string values associated with the enum.
		/// </summary>
		/// <returns>String value array</returns>
		public Array GetStringValues()
		{
			var values = new ArrayList();

			// Look for our string value associated with fields in this enum
			foreach(var fi in _enumType.GetFields())
			{
				// Check for our custom attribute
				var attrs = fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
				if(attrs.Length > 0)
					values.Add(attrs[0].Value);

			}

			return values.ToArray();
		}

		/// <summary>
		/// Gets the values as a 'bindable' list datasource.
		/// </summary>
		/// <returns>IList for data binding</returns>
		public IList GetListValues()
		{
			var values = new ArrayList();
			var underlyingType = Enum.GetUnderlyingType(_enumType);

			// Look for our string value associated with fields in this enum
			foreach(var fi in _enumType.GetFields())
			{
				// Check for our custom attribute
				var attrs = fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
				if(attrs.Length > 0)
					values.Add(new DictionaryEntry(Enum.Parse(_enumType, fi.Name).ChangeType(underlyingType), attrs[0].Value));
			}

			return values;
		}

		/// <summary>
		/// Return the existence of the given string value within the enum.
		/// </summary>
		/// <param name="stringValue">String value.</param>
		/// <returns>Existence of the string value</returns>
		public bool IsStringDefined(string stringValue)
		{
			return !Parse(_enumType, stringValue).IsNull();
		}

		/// <summary>
		/// Return the existence of the given string value within the enum.
		/// </summary>
		/// <param name="stringValue">String value.</param>
		/// <param name="ignoreCase">Denotes whether to conduct a case-insensitive match on the supplied string value</param>
		/// <returns>Existence of the string value</returns>
		public bool IsStringDefined(string stringValue, bool ignoreCase)
		{
			return !Parse(_enumType, stringValue, ignoreCase).IsNull();
		}

		/// <summary>
		/// Gets the underlying enum type for this instance.
		/// </summary>
		/// <value></value>
		public Type EnumType
		{
			get { return _enumType; }
		}

		/// <summary>
		/// Gets a string value for a particular enum value.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>String Value associated via a <see cref="StringValueAttribute"/> attribute, or null if not found.</returns>
		public static string GetStringValue(Enum value)
		{
			var type = value.GetType();
			string output = string.Empty;

			if(_stringValues.ContainsKey(value))
				output = (_stringValues[value] as StringValueAttribute).Value;
			else 
			{
				// Look for our 'StringValueAttribute' in the field's custom attributes
				var fi = type.GetField(value.ToString());
				var attrs = fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
				if(attrs.Length > 0)
				{
					_stringValues.Add(value, attrs[0]);
					output = attrs[0].Value;
				}
			}

			return output;
		}

		/// <summary>
		/// Parses the supplied enum and string value to find an associated enum value (case sensitive).
		/// </summary>
		/// <param name="type">Type.</param>
		/// <param name="stringValue">String value.</param>
		/// <returns>Enum value associated with the string value, or null if not found.</returns>
		public static object Parse(Type type, string stringValue)
		{
			return Parse(type, stringValue, false);
		}

		/// <summary>
		/// Parses the supplied enum and string value to find an associated enum value.
		/// </summary>
		/// <param name="type">Type.</param>
		/// <param name="stringValue">String value.</param>
		/// <param name="ignoreCase">Denotes whether to conduct a case-insensitive match on the supplied string value</param>
		/// <returns>Enum value associated with the string value, or null if not found.</returns>
		public static object Parse(Type type, string stringValue, bool ignoreCase)
		{
			object output = null;
			string enumStringValue = string.Empty;

			if(!type.IsEnum)
				throw new ArgumentException(String.Format("Supplied type must be an Enum. Type was {0}", type.ToString()));

			// Look for our string value associated with fields in this enum
			foreach(var fi in type.GetFields())
			{
				// Check for our custom attribute
				var attrs = fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
				if(attrs.Length > 0)
					enumStringValue = attrs[0].Value;

				// Check for equality then select actual enum value.
				if(string.Compare(enumStringValue, stringValue, ignoreCase) == 0)
				{
					output = Enum.Parse(type, fi.Name);
					break;
				}
			}

			return output;
		}

		/// <summary>
		/// Return the existence of the given string value within the enum.
		/// </summary>
		/// <param name="stringValue">String value.</param>
		/// <param name="enumType">Type of enum</param>
		/// <returns>Existence of the string value</returns>
		public static bool IsStringDefined(Type enumType, string stringValue)
		{
			return !Parse(enumType, stringValue).IsNull();
		}

		/// <summary>
		/// Return the existence of the given string value within the enum.
		/// </summary>
		/// <param name="stringValue">String value.</param>
		/// <param name="enumType">Type of enum</param>
		/// <param name="ignoreCase">Denotes whether to conduct a case-insensitive match on the supplied string value</param>
		/// <returns>Existence of the string value</returns>
		public static bool IsStringDefined(Type enumType, string stringValue, bool ignoreCase)
		{
			return !Parse(enumType, stringValue, ignoreCase).IsNull();
		}
	}
}