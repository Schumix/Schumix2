/*
 * This file is part of Schumix.
 * 
 * Authors:
 *  Jonathan Pryor <jpryor@novell.com>
 *
 * Copyright (C) 2008 Novell (http://www.novell.com)
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013 Schumix Team <http://schumix.eu/>
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
using Schumix.Framework.Extensions;

namespace Schumix.Framework.Options
{
	sealed class ActionOption : Option
	{
		private Action<OptionValueCollection> action;
		
		public ActionOption(string prototype, string description, int count, Action<OptionValueCollection> action) : base(prototype, description, count)
		{
			if(action.IsNull())
				throw new ArgumentNullException("action");

			this.action = action;
		}
		
		protected override void OnParseComplete (OptionContext c)
		{
			action(c.OptionValues);
		}
	}

	sealed class ActionOption<T> : Option
	{
		private Action<T> action;
		
		public ActionOption(string prototype, string description, Action<T> action) : base(prototype, description, 1)
		{
			if(action.IsNull())
				throw new ArgumentNullException("action");

			this.action = action;
		}
		
		protected override void OnParseComplete(OptionContext c)
		{
			action(Parse<T> (c.OptionValues[0], c));
		}
	}

	sealed class ActionOption<TKey, TValue> : Option
	{
		private OptionAction<TKey, TValue> action;
		
		public ActionOption(string prototype, string description, OptionAction<TKey, TValue> action) : base(prototype, description, 2)
		{
			if(action.IsNull())
				throw new ArgumentNullException("action");

			this.action = action;
		}
		
		protected override void OnParseComplete(OptionContext c)
		{
			action(Parse<TKey>(c.OptionValues[0], c), Parse<TValue>(c.OptionValues[1], c));
		}
	}
}