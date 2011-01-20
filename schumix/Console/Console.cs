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
using System.Threading;
using Schumix.IRC;
using Schumix.Config;

namespace Schumix
{
	class Consol
	{
        /// <summary>
        ///     Opcodes.cs filet hívja meg.
        /// </summary>
		//private Opcodes sOpcodes = Singleton<Opcodes>.Instance;
		private SendMessage sSendMessage = Singleton<SendMessage>.Instance;

        /// <summary>
        ///     A Console logja. Alapértelmezésben ki van kapcsolva.
        /// </summary>
		public static int ConsoleLog;

        /// <summary>
        ///     Console írást indítja.
        /// </summary>
        /// <remarks>
        ///     Ha érzékel valamit, akkor alapértelmezésben az IRC szobába írja,
        ///     ha azt parancsnak érzékeli, akkor végrehajtja azt.
        /// </remarks>
		public Consol()
		{
			Thread console = new Thread(new ThreadStart(ConsoleRead));
			console.Start();
			ConsoleLog = LogConfig.IrcLog;
			Log.Success("Console", "Thread elindult.");
		}

        /// <summary>
        ///     Destruktor.
        /// </summary>
        /// <remarks>
        ///     Ha ez lefut, akkor a class leáll.
        /// </remarks>
		~Consol()
		{
			Log.Debug("Console", "~Console()");
		}

        /// <summary>
        ///     Console-ba beírt parancsot hajtja végre.
        /// </summary>
        /// <remarks>
        ///     Ha a Console-ba beírt szöveg egy parancs, akkor ez a
        ///     függvény hajtja végre.
        /// </remarks>
		private void ConsoleRead()
		{
			try
			{
				string uzenet;

				while(true)
				{
					uzenet = Console.ReadLine();
					if(ConsoleCommands(uzenet))
						continue;

					var db = SchumixBot.mSQLConn.QueryFirstRow(String.Format("SELECT irc_cim FROM schumix WHERE entry = '1'"));
					if(db != null)
						sSendMessage.SendChatMessage(MessageType.PRIVMSG, db["irc_cim"].ToString(), uzenet);

					Thread.Sleep(1000);
				}
			}
			catch(Exception e)
			{
				Log.Error("ConsoleRead", String.Format("Hiba oka: {0}", e.ToString()));
				ConsoleRead();
				Thread.Sleep(50);
			}
		}

        /// <summary>
        ///     Az "info" -t darabolja fel szóközönként.
        /// </summary>
        /// <param name="info">Beviteli adat a Console-ból.</param>
        /// <returns>
        ///     Ha igaz értékkel tér vissza "true"-val, akkor
        ///     az "info" string egy parancs, 
        ///     ha hamis értékkel tér vissza "false"-al, akkor
        ///     az "info" string nem parancs, és a beírt szöveget
        ///     az IRC szobába írja ki.
        /// </returns>
		bool ConsoleCommands(string info)
		{
			string[] cmd = info.Split(' ');
			string parancs = cmd[0].ToLower();

			if(parancs == "help")
			{
				Log.Notice("Console", "Parancsok: consolelog, help, kikapcs, szoba");
				return true;
			}

			if(parancs == "consolelog")
			{
				if(info.Length < 2)
					return false;

				if(cmd[1] == "be")
				{
					Log.Notice("Console", "Console logolas bekapcsolva");
					ConsoleLog = 1;
				}
				else if(cmd[1] == "ki")
				{
					Log.Notice("Console", "Console logolas kikapcsolva");
					ConsoleLog = 0;
				}

				return true;
			}

			if(parancs == "szoba")
			{
				if(info.Length < 2)
					return false;

				SchumixBot.mSQLConn.QueryFirstRow(String.Format("UPDATE schumix SET irc_cim = '{0}' WHERE entry = '1'", cmd[1]));
				return true;
			}

			if(parancs == "kikapcs")
			{
				Log.Notice("Console", "Viszlat :(");
				Network.writer.WriteLine("QUIT :Console: leállás.");
				Thread.Sleep(1000);
				Environment.Exit(1);
			}

			return false;
		}
	}
}