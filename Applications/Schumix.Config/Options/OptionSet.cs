/*
 * This file is part of Schumix.
 * 
 * Authors:
 *  Jonathan Pryor <jpryor@novell.com>
 *  Federico Di Gregorio <fog@initd.org>
 *  Rolf Bjarne Kvinge <rolf@xamarin.com>
 *
 * Copyright (C) 2008 Novell (http://www.novell.com)
 * Copyright (C) 2009 Federico Di Gregorio.
 * Copyright (C) 2012 Xamarin Inc (http://www.xamarin.com)
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013-2014 Schumix Team <http://schumix.eu/>
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

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Schumix.Config.Exceptions;
using Schumix.Config.Extensions;

namespace Schumix.Config.Options
{
	public class OptionSet : KeyedCollection<string, Option>
	{
		private readonly Regex ValueOption = new Regex(@"^(?<flag>--|-|/)(?<name>[^:=]+)((?<sep>[:=])(?<value>.*))?$");
		private List<ArgumentSource> sources = new List<ArgumentSource>();
		private const int Description_RemWidth = 80 - OptionWidth - 2;
		private const int Description_FirstWidth = 80 - OptionWidth;
		private ReadOnlyCollection<ArgumentSource> roSources;
		private Converter<string, string> localizer;
		private const int OptionWidth = 29;

		public OptionSet() : this(delegate(string f) { return f; })
		{
		}
		
		public OptionSet(Converter<string, string> localizer)
		{
			this.localizer = localizer;
			this.roSources = new ReadOnlyCollection<ArgumentSource>(sources);
		}
		
		public Converter<string, string> MessageLocalizer
		{
			get { return localizer; }
		}

		public ReadOnlyCollection<ArgumentSource> ArgumentSources
		{
			get { return roSources; }
		}
		
		protected override string GetKeyForItem(Option item)
		{
			if(item.IsNull())
				throw new ArgumentNullException("option");

			if(!item.Names.IsNull() && item.Names.Length > 0)
				return item.Names[0];

			// This should never happen, as it's invalid for Option to be
			// constructed w/o any names.
			throw new InvalidOperationException("Option has no names!");
		}
		
		[Obsolete("Use KeyedCollection.this[string]")]
		protected Option GetOptionForName(string option)
		{
			if(option.IsNull())
				throw new ArgumentNullException("option");

			try
			{
				return base[option];
			}
			catch(KeyNotFoundException)
			{
				return null;
			}
		}
		
		protected override void InsertItem(int index, Option item)
		{
			base.InsertItem(index, item);
			AddImpl(item);
		}
		
		protected override void RemoveItem(int index)
		{
			Option p = Items[index];
			base.RemoveItem(index);
			// KeyedCollection.RemoveItem() handles the 0th item

			for(int i = 1; i < p.Names.Length; ++i)
				Dictionary.Remove(p.Names [i]);
		}
		
		protected override void SetItem(int index, Option item)
		{
			base.SetItem(index, item);
			AddImpl(item);
		}
		
		private void AddImpl(Option option)
		{
			if(option.IsNull())
				throw new ArgumentNullException("option");

			var added = new List<string>(option.Names.Length);

			try
			{
				// KeyedCollection.InsertItem/SetItem handle the 0th name.
				for(int i = 1; i < option.Names.Length; ++i)
				{
					Dictionary.Add(option.Names [i], option);
					added.Add(option.Names [i]);
				}
			}
			catch(Exception)
			{
				foreach(string name in added)
					Dictionary.Remove(name);

				throw;
			}
		}

		public OptionSet Add(string header)
		{
			if(header.IsNull())
				throw new ArgumentNullException("header");

			Add(new Category(header));
			return this;
		}
		
		public new OptionSet Add(Option option)
		{
			base.Add(option);
			return this;
		}
		
		public OptionSet Add(string prototype, Action<string> action)
		{
			return Add(prototype, null, action);
		}

		public OptionSet Add(string prototype, string description, Action<string> action)
		{
			return Add(prototype, description, action, false);
		}

		public OptionSet Add(string prototype, string description, Action<string> action, bool hidden)
		{
			if(action.IsNull())
				throw new ArgumentNullException("action");

			var p = new ActionOption(prototype, description, 1, delegate(OptionValueCollection v) { action(v[0]); }, hidden);
			base.Add(p);
			return this;
		}
		
		public OptionSet Add(string prototype, OptionAction<string, string> action)
		{
			return Add(prototype, null, action);
		}
		
		public OptionSet Add(string prototype, string description, OptionAction<string, string> action)
		{
			return Add(prototype, description, action, false);
		}

		public OptionSet Add(string prototype, string description, OptionAction<string, string> action, bool hidden)
		{
			if(action.IsNull())
				throw new ArgumentNullException("action");

			var p = new ActionOption(prototype, description, 2, delegate(OptionValueCollection v) { action(v[0], v[1]); }, hidden);
			base.Add(p);
			return this;
		}
		
		public OptionSet Add<T>(string prototype, Action<T> action)
		{
			return Add(prototype, null, action);
		}
		
		public OptionSet Add<T>(string prototype, string description, Action<T> action)
		{
			return Add(new ActionOption<T> (prototype, description, action));
		}
		
		public OptionSet Add<TKey, TValue>(string prototype, OptionAction<TKey, TValue> action)
		{
			return Add(prototype, null, action);
		}
		
		public OptionSet Add<TKey, TValue>(string prototype, string description, OptionAction<TKey, TValue> action)
		{
			return Add(new ActionOption<TKey, TValue> (prototype, description, action));
		}

		public OptionSet Add(ArgumentSource source)
		{
			if(source.IsNull())
				throw new ArgumentNullException("source");

			sources.Add(source);
			return this;
		}
		
		protected virtual OptionContext CreateOptionContext()
		{
			return new OptionContext(this);
		}
		
		public List<string> Parse(IEnumerable<string> arguments)
		{
			if(arguments.IsNull())
				throw new ArgumentNullException("arguments");

			OptionContext c = CreateOptionContext();
			c.OptionIndex = -1;
			bool process = true;
			List<string> unprocessed = new List<string>();
			Option def = Contains ("<>") ? this["<>"] : null;
			ArgumentEnumerator ae = new ArgumentEnumerator (arguments);

			foreach(string argument in ae)
			{
				++c.OptionIndex;

				if(argument == "--")
				{
					process = false;
					continue;
				}

				if(!process)
				{
					Unprocessed(unprocessed, def, c, argument);
					continue;
				}

				if(AddSource(ae, argument))
					continue;

				if(!Parse(argument, c))
					Unprocessed(unprocessed, def, c, argument);
			}

			if(!c.Option.IsNull())
				c.Option.Invoke(c);

			return unprocessed;
		}

		bool AddSource(ArgumentEnumerator ae, string argument)
		{
			foreach(ArgumentSource source in sources)
			{
				IEnumerable<string> replacement;

				if(!source.GetArguments(argument, out replacement))
					continue;

				ae.Add(replacement);
				return true;
			}

			return false;
		}
		
		private static bool Unprocessed(ICollection<string> extra, Option def, OptionContext c, string argument)
		{
			if(def.IsNull())
			{
				extra.Add(argument);
				return false;
			}

			c.OptionValues.Add(argument);
			c.Option = def;
			c.Option.Invoke(c);
			return false;
		}

		protected bool GetOptionParts (string argument, out string flag, out string name, out string sep, out string value)
		{
			if(argument.IsNull())
				throw new ArgumentNullException("argument");
			
			flag = name = sep = value = null;
			var m = ValueOption.Match(argument);

			if(!m.Success)
				return false;

			flag = m.Groups["flag"].Value;
			name = m.Groups["name"].Value;

			if(m.Groups["sep"].Success && m.Groups["value"].Success)
			{
				sep = m.Groups["sep"].Value;
				value = m.Groups["value"].Value;
			}

			return true;
		}
		
		protected virtual bool Parse(string argument, OptionContext c)
		{
			if(!c.Option.IsNull())
			{
				ParseValue(argument, c);
				return true;
			}
			
			string f, n, s, v;

			if(!GetOptionParts(argument, out f, out n, out s, out v))
				return false;
			
			Option p;

			if(Contains (n))
			{
				p = this[n];
				c.OptionName = f + n;
				c.Option = p;

				switch(p.OptionValueType)
				{
				case OptionValueType.None:
					c.OptionValues.Add(n);
					c.Option.Invoke(c);
					break;
				case OptionValueType.Optional:
				case OptionValueType.Required: 
					ParseValue(v, c);
					break;
				}

				return true;
			}

			// no match; is it a bool option?
			if(ParseBool(argument, n, c))
				return true;

			// is it a bundled option?
			if(ParseBundledValue(f, string.Concat(n + s + v), c))
				return true;
			
			return false;
		}
		
		private void ParseValue(string option, OptionContext c)
		{
			if(!option.IsNull())
			{
				foreach(string o in !c.Option.ValueSeparators.IsNull() ? option.Split(c.Option.ValueSeparators, c.Option.MaxValueCount - c.OptionValues.Count, StringSplitOptions.None) : new string[]{ option })
					c.OptionValues.Add(o);
			}

			if(c.OptionValues.Count == c.Option.MaxValueCount || c.Option.OptionValueType == OptionValueType.Optional)
				c.Option.Invoke(c);
			else if(c.OptionValues.Count > c.Option.MaxValueCount)
				throw new OptionException(localizer(string.Format("Error: Found {0} option values when expecting {1}.", c.OptionValues.Count, c.Option.MaxValueCount)), c.OptionName);
		}
		
		private bool ParseBool(string option, string n, OptionContext c)
		{
			Option p;
			string rn;

			if(n.Length >= 1 && (n[n.Length-1] == '+' || n[n.Length-1] == '-') && Contains((rn = n.Substring(0, n.Length-1))))
			{
				p = this[rn];
				string v = n[n.Length-1] == '+' ? option : null;
				c.OptionName = option;
				c.Option = p;
				c.OptionValues.Add(v);
				p.Invoke(c);
				return true;
			}

			return false;
		}
		
		private bool ParseBundledValue(string f, string n, OptionContext c)
		{
			if(f != "-")
				return false;

			for(int i = 0; i < n.Length; ++i)
			{
				Option p;
				string opt = f + n[i].ToString();
				string rn = n[i].ToString();

				if(!Contains(rn))
				{
					if(i == 0)
						return false;

					throw new OptionException(string.Format(localizer("Cannot use unregistered option '{0}' in bundle '{1}'."), rn, f + n), null);
				}

				p = this[rn];
				switch(p.OptionValueType)
				{
				case OptionValueType.None:
					Invoke(c, opt, n, p);
					break;
				case OptionValueType.Optional:
				case OptionValueType.Required:
					string v = n.Substring(i+1);
					c.Option = p;
					c.OptionName = opt;
					ParseValue(v.Length != 0 ? v : null, c);
					return true;
				default:
					throw new InvalidOperationException("Unknown OptionValueType: " + p.OptionValueType);
				}
			}
			return true;
		}
		
		private static void Invoke(OptionContext c, string name, string value, Option option)
		{
			c.OptionName = name;
			c.Option = option;
			c.OptionValues.Add(value);
			option.Invoke(c);
		}
		
		public void WriteOptionDescriptions(TextWriter o)
		{
			foreach(Option p in this)
			{
				int written = 0;

				if(p.Hidden)
					continue;

				Category c = p as Category;

				if(!c.IsNull())
				{
					WriteDescription(o, p.Description, string.Empty, 80, 80);
					continue;
				}

				if(!WriteOptionPrototype(o, p, ref written))
					continue;
				
				if(written < OptionWidth)
					o.Write(new string(' ', OptionWidth - written));
				else
				{
					o.WriteLine();
					o.Write(new string(' ', OptionWidth));
				}
				
				WriteDescription(o, p.Description, new string(' ', OptionWidth+2), Description_FirstWidth, Description_RemWidth);
			}

			foreach(ArgumentSource s in sources)
			{
				string[] names = s.GetNames();

				if(names.IsNull() || names.Length == 0)
					continue;

				int written = 0;

				Write(o, ref written, "  ");
				Write(o, ref written, names[0]);

				for(int i = 1; i < names.Length; ++i)
				{
					Write(o, ref written, ", ");
					Write(o, ref written, names[i]);
				}

				if(written < OptionWidth)
					o.Write(new string(' ', OptionWidth - written));
				else
				{
					o.WriteLine();
					o.Write(new string(' ', OptionWidth));
				}

				WriteDescription(o, s.Description, new string(' ', OptionWidth+2), Description_FirstWidth, Description_RemWidth);
			}
		}

		void WriteDescription(TextWriter o, string value, string prefix, int firstWidth, int remWidth)
		{
			bool indent = false;
			foreach(string line in GetLines(localizer(GetDescription(value)), firstWidth, remWidth))
			{
				if(indent)
					o.Write(prefix);

				o.WriteLine(line);
				indent = true;
			}
		}
		
		bool WriteOptionPrototype(TextWriter o, Option p, ref int written)
		{
			string[] names = p.Names;
			
			int i = GetNextOptionIndex(names, 0);
			if(i == names.Length)
				return false;
			
			if(names[i].Length == 1)
			{
				Write(o, ref written, "  -");
				Write(o, ref written, names[0]);
			}
			else
			{
				Write(o, ref written, "      --");
				Write(o, ref written, names[0]);
			}
			
			for(i = GetNextOptionIndex(names, i+1); i < names.Length; i = GetNextOptionIndex(names, i+1))
			{
				Write(o, ref written, ", ");
				Write(o, ref written, names[i].Length == 1 ? "-" : "--");
				Write(o, ref written, names[i]);
			}
			
			if(p.OptionValueType == OptionValueType.Optional || p.OptionValueType == OptionValueType.Required)
			{
				if(p.OptionValueType == OptionValueType.Optional)
					Write(o, ref written, localizer("["));

				Write(o, ref written, localizer("=" + GetArgumentName(0, p.MaxValueCount, p.Description)));
				string sep = !p.ValueSeparators.IsNull() && p.ValueSeparators.Length > 0 ? p.ValueSeparators[0] : " ";

				for(int c = 1; c < p.MaxValueCount; ++c)
					Write(o, ref written, localizer(sep + GetArgumentName(c, p.MaxValueCount, p.Description)));

				if(p.OptionValueType == OptionValueType.Optional)
					Write(o, ref written, localizer("]"));
			}

			return true;
		}
		
		static int GetNextOptionIndex(string[] names, int i)
		{
			while(i < names.Length && names[i] == "<>")
				++i;

			return i;
		}
		
		static void Write(TextWriter o, ref int n, string s)
		{
			n += s.Length;
			o.Write (s);
		}
		
		private static string GetArgumentName(int index, int maxIndex, string description)
		{
			if(description.IsNull())
				return maxIndex == 1 ? "VALUE" : "VALUE" + (index + 1);

			string[] nameStart;

			if(maxIndex == 1)
				nameStart = new string[] { "{0:", "{" };
			else
				nameStart = new string[] { "{" + index + ":" };

			for(int i = 0; i < nameStart.Length; ++i)
			{
				int start, j = 0;

				do
				{
					start = description.IndexOf(nameStart[i], j);
				} while(start >= 0 && j != 0 ? description[j++ - 1] == '{' : false);

				if(start == -1)
					continue;

				int end = description.IndexOf ("}", start);
				if(end == -1)
					continue;

				return description.Substring(start + nameStart [i].Length, end - start - nameStart[i].Length);
			}

			return maxIndex == 1 ? "VALUE" : "VALUE" + (index + 1);
		}
		
		private static string GetDescription(string description)
		{
			if(description.IsNull())
				return string.Empty;

			var sb = new StringBuilder(description.Length);
			int start = -1;

			for(int i = 0; i < description.Length; ++i)
			{
				switch(description[i])
				{
				case '{':
					if(i == start)
					{
						sb.Append ('{');
						start = -1;
					}
					else if(start < 0)
						start = i + 1;

					break;
				case '}':
					if(start < 0)
					{
						if((i+1) == description.Length || description[i+1] != '}')
							throw new InvalidOperationException("Invalid option description: " + description);

						++i;
						sb.Append ("}");
					}
					else
					{
						sb.Append(description.Substring(start, i - start));
						start = -1;
					}

					break;
				case ':':
					if(start < 0)
						goto default;

					start = i + 1;
					break;
				default:
					if(start < 0)
						sb.Append(description [i]);

					break;
				}
			}

			return sb.ToString();
		}
		
		private static IEnumerable<string> GetLines(string description, int firstWidth, int remWidth)
		{
			return StringCoda.WrappedLines(description, firstWidth, remWidth);
		}
	}
}