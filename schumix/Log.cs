/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010 Megax <http://www.megaxx.info/>
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
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Schumix
{
	public static class Log
	{

        /// <returns>
        ///     A visszatérési érték az aktuális dátum.
        /// </returns>
		private static string GetTime()
		{
			return String.Format("{0}:{1}", DateTime.Now.Hour, DateTime.Now.Minute);
		}

        /// <summary>
        ///     Dátummal logolja a szöveget meghatározva honnan származik. 
        ///     Lehet ez egyénileg meghatározott függvény vagy class névvel ellátva.
        ///     Logol a Console-ra.
        /// </summary>
        /// <param name="source">
        ///     Meghatározza honnan származik a log.
        ///     <example>
        ///         17:28 N <c>Config:</c> Config file betöltése...
        ///     </example>
        /// </param>
        /// <param name="format">
        ///     A szöveg amit kiírunk.
        ///     <example>
        ///         17:28 N Config: <c>Config file betöltése...</c>
        ///     </example>
        /// </param>
		public static void Notice(string source, string format)
		{
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.Write(GetTime());
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write(" N {0}: ", source);
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.Write("{0}\n", format);
		}

		public static void Warning(string source, string format)
		{
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.Write(GetTime());
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write(" W");
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write(" {0}: ", source);
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write("{0}\n", format);
			Console.ForegroundColor = ConsoleColor.Gray;
		}

		public static void Success(string source, string format)
		{
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.Write(GetTime());
			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write(" S");
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write(" {0}: ", source);
			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write("{0}\n", format);
			Console.ForegroundColor = ConsoleColor.Gray;
		}

		public static void Error(string source, string format)
		{
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.Write(GetTime());
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write(" E");
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write(" {0}: ", source);
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write("{0}\n", format);
			Console.ForegroundColor = ConsoleColor.Gray;
		}

		public static void Debug(string source, string format)
		{
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.Write(GetTime());
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.Write(" D");
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write(" {0}: ", source);
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.Write("{0}\n", format);
			Console.ForegroundColor = ConsoleColor.Gray;
		}
	}
}