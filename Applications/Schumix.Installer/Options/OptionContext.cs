/*
 * This file is part of Schumix.
 * 
 * Authors:
 *  Jonathan Pryor <jpryor@novell.com>
 *
 * Copyright (C) 2008 Novell (http://www.novell.com)
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

namespace Schumix.Installer.Options
{
	public class OptionContext
	{
		private OptionValueCollection c;
		private OptionSet set;
		private Option option;
		private string name;
		private int index;
		
		public OptionContext(OptionSet set)
		{
			this.set = set;
			this.c = new OptionValueCollection(this);
		}
		
		public Option Option
		{
			get { return option; }
			set { option = value; }
		}
		
		public string OptionName
		{ 
			get { return name; }
			set { name = value; }
		}
		
		public int OptionIndex
		{
			get { return index; }
			set { index = value; }
		}
		
		public OptionSet OptionSet
		{
			get { return set; }
		}
		
		public OptionValueCollection OptionValues
		{
			get { return c; }
		}
	}
}