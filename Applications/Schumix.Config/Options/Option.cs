/*
 * This file is part of Schumix.
 * 
 * Authors:
 *  Jonathan Pryor <jpryor@novell.com>
 *  Rolf Bjarne Kvinge <rolf@xamarin.com>
 *
 * Copyright (C) 2008 Novell (http://www.novell.com)
 * Copyright (C) 2012 Xamarin Inc (http://www.xamarin.com)
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013-2015 Schumix Team <http://schumix.eu/>
 * 
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the
 * "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to
 * permit persons to whom the Software is furnished to do so, subject to
 * the following conditions:
 * 
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
 * LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
 * OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
 * WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

// Utolsó kommit ami alapján frissítve lett a Mono.Options: https://github.com/mono/mono/commit/a699e7444e8ff4426197d4bd3a9607d204ee7b0f

using System;
using System.ComponentModel;
using System.Collections.Generic;
using Schumix.Config.Exceptions;
using Schumix.Config.Extensions;

namespace Schumix.Config.Options
{
	public abstract class Option
	{
		private static readonly char[] NameTerminator = new char[] { '=', ':' };
		private OptionValueType type;
		private string[] separators;
		private string description;
		private string prototype;
		private string[] names;
		private bool hidden;
		private int count;

		public string Prototype
		{
			get { return prototype; }
		}
		
		public string Description
		{
			get { return description; }
		}
		
		public OptionValueType OptionValueType
		{
			get { return type; }
		}
		
		public int MaxValueCount
		{
			get { return count; }
		}

		public bool Hidden
		{
			get { return hidden; }
		}
		
		public string[] GetNames()
		{
			return (string[])names.Clone();
		}
		
		public string[] GetValueSeparators()
		{
			return separators.IsNull() ? new string[0] : (string[])separators.Clone();
		}

		internal string[] Names
		{
			get { return names; }
		}
		
		internal string[] ValueSeparators
		{
			get { return separators; }
		}

		protected abstract void OnParseComplete(OptionContext c);
		
		protected Option(string prototype, string description) : this(prototype, description, 1, false)
		{
		}
		
		protected Option(string prototype, string description, int maxValueCount) : this(prototype, description, maxValueCount, false)
		{
		}

		protected Option(string prototype, string description, int maxValueCount, bool hidden)
		{
			if(prototype.IsNull())
				throw new ArgumentNullException("prototype");

			if(prototype.Length == 0)
				throw new ArgumentException("Cannot be the empty string.", "prototype");

			if(maxValueCount < 0)
				throw new ArgumentOutOfRangeException("maxValueCount");
			
			this.prototype   = prototype;
			this.description = description;
			this.count       = maxValueCount;
			this.names       = (this is Category)
				// append GetHashCode() so that "duplicate" categories have distinct
				// names, e.g. adding multiple "" categories should be valid.
				? new[] { prototype + this.GetHashCode() } : prototype.Split('|');

			if(this is Category)
				return;

			this.type   = ParsePrototype();
			this.hidden = hidden;
			
			if(this.count == 0 && type != OptionValueType.None)
				throw new ArgumentException("Cannot provide maxValueCount of 0 for OptionValueType.Required or OptionValueType.Optional.", "maxValueCount");

			if(this.type == OptionValueType.None && maxValueCount > 1)
				throw new ArgumentException(string.Format("Cannot provide maxValueCount of {0} for OptionValueType.None.", maxValueCount), "maxValueCount");

			if(Array.IndexOf(names, "<>") >= 0 && ((names.Length == 1 && this.type != OptionValueType.None) || (names.Length > 1 && this.MaxValueCount > 1)))
				throw new ArgumentException("The default option handler '<>' cannot require values.", "prototype");
		}
		
		#if NOTYPECONVERTER
		protected static T Parse<T>(string value, OptionContext c)
		{
			if(typeof(T) == typeof(int))
				return (T)(object)int.Parse(value);
			else if(typeof(T) == typeof(string))
				return (T)(object) value;

			throw new OptionException("Could not convert parameter", c.OptionName, null);
		}
		#else
		protected static T Parse<T>(string value, OptionContext c)
		{
			var tt = typeof(T);
			bool nullable = tt.IsValueType && tt.IsGenericType && !tt.IsGenericTypeDefinition && tt.GetGenericTypeDefinition () == typeof(Nullable<>);
			var targetType = nullable ? tt.GetGenericArguments()[0] : typeof(T);
			var conv = TypeDescriptor.GetConverter(targetType);
			var t = default(T);

			try
			{
				if(!value.IsNull())
					t = (T)conv.ConvertFromString(value);
			}
			catch(Exception e)
			{
				throw new OptionException(string.Format(c.OptionSet.MessageLocalizer("Could not convert string `{0}' to type {1} for option `{2}'."), value, targetType.Name, c.OptionName), c.OptionName, e);
			}

			return t;
		}
		#endif
		
		private OptionValueType ParsePrototype()
		{
			char type = '\0';
			var seps = new List<string>();

			for(int i = 0; i < names.Length; ++i)
			{
				string name = names[i];
				if(name.Length == 0)
					throw new ArgumentException("Empty option names are not supported.", "prototype");
				
				int end = name.IndexOfAny(NameTerminator);
				if(end == -1)
					continue;

				names[i] = name.Substring(0, end);

				if(type == '\0' || type == name[end])
					type = name [end];
				else 
					throw new ArgumentException(string.Format("Conflicting option types: '{0}' vs. '{1}'.", type, name[end]), "prototype");

				AddSeparators (name, end, seps);
			}
			
			if(type == '\0')
				return OptionValueType.None;
			
			if(count <= 1 && seps.Count != 0)
				throw new ArgumentException(string.Format("Cannot provide key/value separators for Options taking {0} value(s).", count), "prototype");

			if(count > 1)
			{
				if(seps.Count == 0)
					this.separators = new string[] { ":", "=" };
				else if(seps.Count == 1 && seps[0].Length == 0)
					this.separators = null;
				else
					this.separators = seps.ToArray();
			}
			
			return type == '=' ? OptionValueType.Required : OptionValueType.Optional;
		}
		
		private static void AddSeparators(string name, int end, ICollection<string> seps)
		{
			int start = -1;

			for(int i = end+1; i < name.Length; ++i)
			{
				switch(name[i])
				{
				case '{':
					if(start != -1)
						throw new ArgumentException(string.Format("Ill-formed name/value separator found in \"{0}\".", name), "prototype");

					start = i+1;
					break;
				case '}':
					if(start == -1)
						throw new ArgumentException(string.Format("Ill-formed name/value separator found in \"{0}\".", name), "prototype");

					seps.Add(name.Substring(start, i-start));
					start = -1;
					break;
				default:
					if(start == -1)
						seps.Add(name[i].ToString());
					break;
				}
			}

			if(start != -1)
				throw new ArgumentException(string.Format("Ill-formed name/value separator found in \"{0}\".", name), "prototype");
		}
		
		public void Invoke(OptionContext c)
		{
			OnParseComplete(c);
			c.OptionName = null;
			c.Option = null;
			c.OptionValues.Clear();
		}
		
		public override string ToString()
		{
			return Prototype;
		}
	}
}
	