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
using Schumix.IRC.Commands;
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
		private Utility sUtility = Singleton<Utility>.Instance;

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
				Log.Notice("Console", "Parancsok: connect, disconnect, reconnect, consolelog, kikapcs");
				Log.Notice("Console", "Parancsok: szoba, admin");
				return true;
			}

			if(parancs == "consolelog")
			{
				if(cmd.Length < 2)
				{
					Log.Error("Console", "Nincs parameter!");
					return true;
				}

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
				if(cmd.Length < 2)
				{
					Log.Error("Console", "Nincs megadva a szoba neve!");
					return true;
				}

				SchumixBot.mSQLConn.QueryFirstRow(String.Format("UPDATE schumix SET irc_cim = '{0}' WHERE entry = '1'", cmd[1]));
				return true;
			}

			if(parancs == "admin")
			{
				if(cmd.Length >= 2 && cmd[1] == "help")
				{
					Log.Notice("Console", "Alparancsok hasznalata:");
					Log.Notice("Console", "Admin lista: admin lista");
					Log.Notice("Console", "Hozzaadas: admin add <admin neve>");
					Log.Notice("Console", "Eltavolitas: admin del <admin neve>");
					Log.Notice("Console", "Rang: admin rang <admin neve> <uj rang pl operator: 0, administrator: 1>");
					Log.Notice("Console", "Info: admin info <admin neve>");
				}
				else if(cmd.Length >= 2 && cmd[1] == "info")
				{
					if(cmd.Length < 3)
					{
						Log.Error("Console", "Nincs nev megadva!");
						return true;
					}

					int flag;

					var db = SchumixBot.mSQLConn.QueryFirstRow(String.Format("SELECT flag FROM adminok WHERE nev = '{0}'", cmd[2].ToLower()));
					if(db != null)
						flag = Convert.ToInt32(db["flag"].ToString());
					else
						flag = -1;
	
					if((AdminFlag)flag == AdminFlag.Operator)
						Log.Notice("Console", "Jelenleg Operator.");
					else if((AdminFlag)flag == AdminFlag.Administrator)
						Log.Notice("Console", "Jelenleg Adminisztrator.");
				}
				else if(cmd.Length >= 2 && cmd[1] == "lista")
				{
					var db = SchumixBot.mSQLConn.QueryRow(String.Format("SELECT nev FROM adminok"));
					if(db != null)
					{
						string adminok = "";

						for(int i = 0; i < db.Rows.Count; ++i)
						{
							var row = db.Rows[i];
							string nev = row["nev"].ToString();
							adminok += ", " + nev;
						}

						if(adminok.Substring(0, 2) == ", ")
							adminok = adminok.Remove(0, 2);

						Log.Notice("Console", String.Format("Adminok: {0}", adminok));
					}
					else
						Log.Error("Console", "Hibas lekerdezes!");
				}
				else if(cmd.Length >= 2 && cmd[1] == "add")
				{
					if(cmd.Length < 3)
					{
						Log.Error("Console", "Nincs nev megadva!");
						return true;
					}

					string nev = cmd[2];
					string pass = sUtility.GetRandomString();

					SchumixBot.mSQLConn.QueryFirstRow(String.Format("INSERT INTO `adminok`(nev, jelszo) VALUES ('{0}', '{1}')", nev.ToLower(), sUtility.Sha1(pass)));
					Log.Notice("Console", String.Format("Admin hozzaadva: {0}", nev));
					Log.Notice("Console", String.Format("Mostani jelszo: {0}", pass));
				}
				else if(cmd.Length >= 2 && cmd[1] == "del")
				{
					if(cmd.Length < 3)
					{
						Log.Error("Console", "Nincs nev megadva!");
						return true;
					}

					string nev = cmd[2];
					SchumixBot.mSQLConn.QueryFirstRow(String.Format("DELETE FROM `adminok` WHERE nev = '{0}'", nev.ToLower()));
					Log.Notice("Console", String.Format("Admin törölve: {0}", nev));
				}
				else if(cmd.Length >= 2 && cmd[1] == "rang")
				{
					if(cmd.Length < 3)
					{
						Log.Error("Console", "Nincs nev megadva!");
						return true;
					}

					if(cmd.Length < 4)
					{
						Log.Error("Console", "Nincs rang megadva!");
						return true;
					}

					string nev = cmd[2].ToLower();
					int rang = Convert.ToInt32(cmd[3]);

					if((AdminFlag)rang == AdminFlag.Administrator || (AdminFlag)rang == AdminFlag.Operator)
					{
						SchumixBot.mSQLConn.QueryFirstRow(String.Format("UPDATE adminok SET flag = '{0}' WHERE nev = '{1}'", rang, nev));
						Log.Notice("Console", "Rang sikeresen modósitva.");
					}
					else
						Log.Error("Console", "Hibás rang!");
				}
				else
					Log.Notice("Console", "Parancsok: help | lista | add | del");

				return true;
			}

			if(parancs == "connect")
			{
				Network.Connect();
				return true;
			}

			if(parancs == "disconnect")
			{
				sSendMessage.WriteLine("QUIT :Console: disconnect.");
				Network.DisConnect();
				return true;
			}

			if(parancs == "reconnect")
			{
				sSendMessage.WriteLine("QUIT :Console: reconnect.");
				Network.ReConnect();
				return true;
			}

			if(parancs == "kikapcs")
			{
				SchumixBot.SaveUptime();
				Log.Notice("Console", "Viszlat :(");
				sSendMessage.WriteLine("QUIT :Console: leállás.");
				Thread.Sleep(1000);
				Environment.Exit(1);
			}

			return false;
		}
	}
}