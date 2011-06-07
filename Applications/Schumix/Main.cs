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
using System.Text;
using Schumix.Irc;
using Schumix.Framework;
using Schumix.Framework.Config;

namespace Schumix
{
	class MainClass
	{
		private static readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private static readonly Sender sSender = Singleton<Sender>.Instance;

		/// <summary>
		///     A Main függvény. Itt indul el a debug.
		/// </summary>
		/// <remarks>
		///     Schumix2 IRC bot
		///     <para>
		///         Készítette Megaxxx és Jackneill.
		///     </para>
		/// </remarks>
		private static void Main(string[] args)
		{
			string configdir = "Configs";
			string configfile = "Schumix.xml";
			string console_encoding = "utf-8";

			for(int i = 0; i < args.Length; i++)
			{
				string arg = args[i];

				if(arg == "-h" || arg == "--help")
				{
					//Help();
					return;
				}
				else if(arg.Contains("--config-dir="))
				{
					configdir = arg.Substring(arg.IndexOf("=")+1);
					continue;
				}
				else if(arg.Contains("--config-file="))
				{
					configfile = arg.Substring(arg.IndexOf("=")+1);
					continue;
				}
				else if(arg.Contains("--console-encoding="))
				{
					console_encoding = arg.Substring(arg.IndexOf("=")+1);
					continue;
				}
			}

			System.Console.OutputEncoding = Encoding.GetEncoding(console_encoding);
			System.Console.Title = SchumixBase.Title;
			System.Console.ForegroundColor = ConsoleColor.Blue;
			System.Console.WriteLine("[Schumix2]");
			System.Console.WriteLine("A program leallitasahoz hasznald a <Ctrl+C> vagy <quit> parancsot!\n");
			System.Console.ForegroundColor = ConsoleColor.Gray;
			System.Console.WriteLine("Keszitette Megax, Jackneill. Schumix Verzio: {0} http://megaxx.info", sUtilities.GetVersion());
			System.Console.WriteLine("==============================================================================");
			System.Console.WriteLine();

			new Config(configdir, configfile);
			Log.Notice("Main", "Rendszer indul...");

			new SchumixBot();
			System.Console.CancelKeyPress += (sender, e) => { sSender.Quit("Daemon killed."); SchumixBase.timer.SaveUptime(); };
		}
	}
}