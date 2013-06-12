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
using System.IO;
using System.Text;
using System.Collections.Generic;
using Schumix.Framework;
using Schumix.Framework.Extensions;

namespace Schumix.Framework.Options
{
	public abstract class ArgumentSource
	{
		protected ArgumentSource()
		{
		}

		public abstract string[] GetNames();
		public abstract string Description { get; }
		public abstract bool GetArguments(string value, out IEnumerable<string> replacement);

		public static IEnumerable<string> GetArgumentsFromFile(string file)
		{
			return GetArguments(File.OpenText(file), true);
		}

		public static IEnumerable<string> GetArguments(TextReader reader)
		{
			return GetArguments(reader, false);
		}

		// Cribbed from mcs/driver.cs:LoadArgs(string)
		static IEnumerable<string> GetArguments(TextReader reader, bool close)
		{
			try
			{
				StringBuilder arg = new StringBuilder();
				string line;

				while(!(line = reader.ReadLine()).IsNull())
				{
					int t = line.Length;

					for(int i = 0; i < t; i++)
					{
						char c = line [i];

						if(c == '"' || c == '\'')
						{
							char end = c;

							for(i++; i < t; i++)
							{
								c = line[i];

								if(c == end)
									break;

								arg.Append(c);
							}
						}
						else if(c == SchumixBase.Space)
						{
							if(arg.Length > 0)
							{
								yield return arg.ToString();
								arg.Length = 0;
							}
						}
						else
							arg.Append(c);
					}

					if(arg.Length > 0)
					{
						yield return arg.ToString();
						arg.Length = 0;
					}
				}
			}
			finally
			{
				if(close)
					reader.Close();
			}
		}
	}
}