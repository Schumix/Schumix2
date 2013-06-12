/*
 * This file is part of Schumix.
 * 
 * Authors:
 *  Jonathan Pryor <jpryor@novell.com>
 *  Federico Di Gregorio <fog@initd.org>
 *
 * Copyright (C) 2008 Novell (http://www.novell.com)
 * Copyright (C) 2009 Federico Di Gregorio.
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
using System.Collections.Generic;
using Schumix.Framework;
using Schumix.Framework.Extensions;

namespace Schumix.Framework.Options
{
	static class StringCoda
	{
		public static IEnumerable<string> WrappedLines(string self, params int[] widths)
		{
			IEnumerable<int> w = widths;
			return WrappedLines(self, w);
		}

		public static IEnumerable<string> WrappedLines(string self, IEnumerable<int> widths)
		{
			if(widths.IsNull())
				throw new ArgumentNullException("widths");

			return CreateWrappedLinesIterator(self, widths);
		}

		private static IEnumerable<string> CreateWrappedLinesIterator(string self, IEnumerable<int> widths)
		{
			if(string.IsNullOrEmpty(self))
			{
				yield return string.Empty;
				yield break;
			}

			using(IEnumerator<int> ewidths = widths.GetEnumerator())
			{
				bool? hw = null;
				int width = GetNextWidth (ewidths, int.MaxValue, ref hw);
				int start = 0, end;

				do
				{
					end = GetLineEnd (start, width, self);
					char c = self[end-1];

					if(char.IsWhiteSpace (c))
						--end;

					bool needContinuation = end != self.Length && !IsEolChar(c);
					string continuation = string.Empty;

					if(needContinuation)
					{
						--end;
						continuation = "-";
					}

					string line = self.Substring(start, end - start) + continuation;
					yield return line;
					start = end;

					if(char.IsWhiteSpace(c))
						++start;

					width = GetNextWidth(ewidths, width, ref hw);
				} while(start < self.Length);
			}
		}

		private static int GetNextWidth (IEnumerator<int> ewidths, int curWidth, ref bool? eValid)
		{
			if(!eValid.HasValue || (eValid.HasValue && eValid.Value))
			{
				curWidth = (eValid = ewidths.MoveNext()).Value ? ewidths.Current : curWidth;
				// '.' is any character, - is for a continuation
				const string minWidth = ".-";

				if(curWidth < minWidth.Length)
					throw new ArgumentOutOfRangeException("widths", string.Format("Element must be >= {0}, was {1}.", minWidth.Length, curWidth));

				return curWidth;
			}

			// no more elements, use the last element.
			return curWidth;
		}

		private static bool IsEolChar(char c)
		{
			return !char.IsLetterOrDigit(c);
		}

		private static int GetLineEnd(int start, int length, string description)
		{
			int end = Math.Min(start + length, description.Length);
			int sep = -1;

			for(int i = start; i < end; ++i)
			{
				if(description[i] == SchumixBase.NewLine)
					return i+1;

				if(IsEolChar(description[i]))
					sep = i+1;
			}

			if(sep == -1 || end == description.Length)
				return end;

			return sep;
		}
	}
}