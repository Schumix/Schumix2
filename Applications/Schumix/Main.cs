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
using Schumix.Framework.Localization;

namespace Schumix
{
	/// <summary>
	///     Main class.
	/// </summary>
	class MainClass
	{
		/// <summary>
		///     Hozzáférést bisztosít singleton-on keresztül a megadott class-hoz.
		///     LocalizationConsole segítségével állíthatóak be a konzol nyelvi tulajdonságai.
		/// </summary>
		private static readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		/// <summary>
		///     Hozzáférést bisztosít singleton-on keresztül a megadott class-hoz.
		///     Utilities sokféle függvényt tartalmaz melyek hasznosak lehetnek.
		/// </summary>
		private static readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		/// <summary>
		///     Hozzáférést bisztosít singleton-on keresztül a megadott class-hoz.
		///     Üzenet küldés az irc szerver felé.
		/// </summary>
		private static readonly Sender sSender = Singleton<Sender>.Instance;

		/// <summary>
		///     A Main függvény. Itt indul el a program.
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
			string localization = "enUS";

			for(int i = 0; i < args.Length; i++)
			{
				string arg = args[i];

				if(arg == "-h" || arg == "--help")
				{
					Help();
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
				else if(arg.Contains("--console-localization="))
				{
					localization = arg.Substring(arg.IndexOf("=")+1);
					continue;
				}
			}

			double Num;
			bool isNum = double.TryParse(console_encoding, out Num);

			if(!isNum)
				System.Console.OutputEncoding = Encoding.GetEncoding(console_encoding);
			else
				System.Console.OutputEncoding = Encoding.GetEncoding(Convert.ToInt32(Num)); // Magyar karakterkódolás windows xp-n: 852

			sLConsole.Locale = localization;
			System.Console.Title = SchumixBase.Title;
			System.Console.ForegroundColor = ConsoleColor.Blue;
			System.Console.WriteLine("[Schumix2]");
			System.Console.WriteLine(sLConsole.MainText("StartText"));
			System.Console.ForegroundColor = ConsoleColor.Gray;
			System.Console.WriteLine(sLConsole.MainText("StartText2"), sUtilities.GetVersion());
			System.Console.WriteLine("================================================================================"); // 80
			System.Console.WriteLine();

			new Config(configdir, configfile);

			if(localization != LocalizationConfig.Locale)
				sLConsole.Locale = LocalizationConfig.Locale;

			Log.Notice("Main", sLConsole.MainText("StartText3"));

			new SchumixBot();
			System.Console.CancelKeyPress += (sender, e) => { sSender.Quit("Daemon killed."); SchumixBase.timer.SaveUptime(); };
		}

		/// <summary>
		///     Segítséget nyújt a kapcsolokhoz.
		/// </summary>
		private static void Help()
		{
			System.Console.WriteLine("Test");
			System.Console.WriteLine("Test");
			System.Console.WriteLine("Test");
			System.Console.WriteLine("Test");
			System.Console.WriteLine("Test");
			System.Console.WriteLine("Test");
		}
	}
}