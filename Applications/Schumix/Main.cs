/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2011 Megax <http://www.megaxx.info/>
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
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Irc;

namespace Schumix
{
	class MainClass
	{
		private static readonly Sender sSender = Singleton<Sender>.Instance;

        /// <summary>
        ///     A Main függvény. Itt indul el a debug.
        /// </summary>
        /// <remarks>
        ///     Schumix IRC bot
        ///     <para>
        ///         Készítette Megaxxx és Jackneill.
        ///     </para>
        /// </remarks>
		private static void Main(string[] args)
		{
			string configfile = "schumix.xml";
			Log.Indulas(configfile);

			System.Console.Title = SchumixBase.Title;
			System.Console.ForegroundColor = ConsoleColor.Blue;
			System.Console.WriteLine("[Schumix2]");
			System.Console.WriteLine("A program leallitasahoz hasznald a <Ctrl+C> parancsot vagy <kikapcs>\n");
			System.Console.ForegroundColor = ConsoleColor.Gray;
			System.Console.WriteLine("Keszitette Megax, Jackneill. Schumix Verzio: {0} http://megaxx.info", Verzio.SchumixVerzio);
			System.Console.WriteLine("==============================================================================");
			System.Console.WriteLine("");
			Log.Notice("Main", "Rendszer indul...");
			System.Console.WriteLine("");

			new SchumixBot(configfile);
			System.Console.CancelKeyPress += (sender, e) => { sSender.Quit("Daemon killed."); SchumixBase.time.SaveUptime(); };
		}
	}
}
