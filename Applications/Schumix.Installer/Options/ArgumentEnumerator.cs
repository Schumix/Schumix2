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
using System.Collections;
using System.Collections.Generic;

namespace Schumix.Installer.Options
{
	class ArgumentEnumerator : IEnumerable<string>
	{
		private List<IEnumerator<string>> sources = new List<IEnumerator<string>>();

		public ArgumentEnumerator(IEnumerable<string> arguments)
		{
			sources.Add(arguments.GetEnumerator());
		}

		public void Add(IEnumerable<string> arguments)
		{
			sources.Add(arguments.GetEnumerator());
		}

		public IEnumerator<string> GetEnumerator()
		{
			do
			{
				IEnumerator<string> c = sources[sources.Count-1];

				if(c.MoveNext())
					yield return c.Current;
				else
				{
					c.Dispose();
					sources.RemoveAt(sources.Count-1);
				}
			} while(sources.Count > 0);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}