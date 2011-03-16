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
using System.Diagnostics;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Config;

namespace Schumix.Console.Commands
{
	public class CommandHandler : ConsoleLog
	{
		private readonly Sender sSender = Singleton<Sender>.Instance;
		private readonly Utility sUtility = Singleton<Utility>.Instance;
		private readonly NickInfo sNickInfo = Singleton<NickInfo>.Instance;
		private readonly Network _network;
		protected string[] Info;

		protected CommandHandler(Network network) : base(LogConfig.IrcLog)
		{
			_network = network;
		}

		protected void HandleHelp()
		{
			string parancsok = string.Empty;

			foreach(var command in CCommandManager.GetCommandHandler())
			{
				if(command.Key == "help")
					continue;

				parancsok += ", " + command.Key;
			}

			if(parancsok.Substring(0, 2) == ", ")
				parancsok = parancsok.Remove(0, 2);

			Log.Notice("Console", "Parancsok: {0}", parancsok);
		}

		protected void HandleConsoleLog()
		{
			if(Info.Length < 2)
			{
				Log.Error("Console", "Nincs parameter!");
				return;
			}

			if(Info[1] == "be")
			{
				Log.Notice("Console", "Console logolas bekapcsolva");
				ChangeLog(true);
			}
			else if(Info[1] == "ki")
			{
				Log.Notice("Console", "Console logolas kikapcsolva");
				ChangeLog(false);
			}
		}

		protected void HandleSys()
		{
			string Platform = string.Empty;
			var pid = Environment.OSVersion.Platform;

			switch(pid)
			{
				case PlatformID.Win32NT:
				case PlatformID.Win32S:
				case PlatformID.Win32Windows:
				case PlatformID.WinCE:
					Platform = "Windows";
					break;
				case PlatformID.Unix:
					Platform = "Linux";
					break;
				default:
					Platform = "Ismeretlen";
					break;
			}

			var memory = Process.GetCurrentProcess().WorkingSet64/1024/1024;

			Log.Notice("Console", "Verzio: {0}", Verzio.SchumixVerzio);
			Log.Notice("Console", "Platform: {0}", Platform);
			Log.Notice("Console", "OSVerzio: {0}", Environment.OSVersion.ToString());
			Log.Notice("Console", "Programnyelv: c#");
			Log.Notice("Console", "Memoria hasznalat: {0} MB", memory);
			Log.Notice("Console", "Thread count: {0}", Process.GetCurrentProcess().Threads.Count);
			Log.Notice("Console", "Uptime: {0}", SchumixBase.time.Uptime());
		}

		protected void HandleCsatorna()
		{
			if(Info.Length < 2)
			{
				Log.Error("Console", "Nincs megadva a csatorna neve!");
				return;
			}

			SchumixBase.DManager.QueryFirstRow("UPDATE schumix SET csatorna = '{0}' WHERE entry = '1'", Info[1]);
			Log.Notice("Console", "Uj csatorna ahova mostantol lehet irni: {0}", Info[1]);
			System.Console.Title = SchumixBase.Title + " || Console Writing Channel: " + Info[1];
		}

		protected void HandleAdmin()
		{
			if(Info.Length >= 2 && Info[1] == "help")
			{
				Log.Notice("Console", "Alparancsok hasznalata:");
				Log.Notice("Console", "Admin lista: admin lista");
				Log.Notice("Console", "Hozzaadas: admin add <admin neve>");
				Log.Notice("Console", "Eltavolitas: admin del <admin neve>");
				Log.Notice("Console", "Rang: admin rang <admin neve> <uj rang pl operator: 0, administrator: 1>");
				Log.Notice("Console", "Info: admin info <admin neve>");
			}
			else if(Info.Length >= 2 && Info[1] == "info")
			{
				if(Info.Length < 3)
				{
					Log.Error("Console", "Nincs nev megadva!");
					return;
				}

				int flag;

				var db = SchumixBase.DManager.QueryFirstRow("SELECT flag FROM adminok WHERE nev = '{0}'", Info[2].ToLower());
				if(db != null)
					flag = Convert.ToInt32(db["flag"].ToString());
				else
					flag = -1;
	
				if((AdminFlag)flag == AdminFlag.Operator)
					Log.Notice("Console", "Jelenleg Operator.");
				else if((AdminFlag)flag == AdminFlag.Administrator)
					Log.Notice("Console", "Jelenleg Adminisztrator.");
			}
			else if(Info.Length >= 2 && Info[1] == "lista")
			{
				var db = SchumixBase.DManager.Query("SELECT nev FROM adminok");
				if(db != null)
				{
					string adminok = string.Empty;

					for(int i = 0; i < db.Rows.Count; ++i)
					{
						var row = db.Rows[i];
						string nev = row["nev"].ToString();
						adminok += ", " + nev;
					}

					if(adminok.Substring(0, 2) == ", ")
						adminok = adminok.Remove(0, 2);

					Log.Notice("Console", "Adminok: {0}", adminok);
				}
				else
					Log.Error("Console", "Hibas lekerdezes!");
			}
			else if(Info.Length >= 2 && Info[1] == "add")
			{
				if(Info.Length < 3)
				{
					Log.Error("Console", "Nincs nev megadva!");
					return;
				}

				string nev = Info[2];
				string pass = sUtility.GetRandomString();

				SchumixBase.DManager.QueryFirstRow("INSERT INTO `adminok`(nev, jelszo) VALUES ('{0}', '{1}')", nev.ToLower(), sUtility.Sha1(pass));
				Log.Notice("Console", "Admin hozzaadva: {0}", nev);
				Log.Notice("Console", "Mostani jelszo: {0}", pass);
			}
			else if(Info.Length >= 2 && Info[1] == "del")
			{
				if(Info.Length < 3)
				{
					Log.Error("Console", "Nincs nev megadva!");
					return;
				}

				string nev = Info[2];
				SchumixBase.DManager.QueryFirstRow("DELETE FROM `adminok` WHERE nev = '{0}'", nev.ToLower());
				Log.Notice("Console", "Admin törölve: {0}", nev);
			}
			else if(Info.Length >= 2 && Info[1] == "rang")
			{
				if(Info.Length < 3)
				{
					Log.Error("Console", "Nincs nev megadva!");
					return;
				}

				if(Info.Length < 4)
				{
					Log.Error("Console", "Nincs rang megadva!");
					return;
				}

				string nev = Info[2].ToLower();
				int rang = Convert.ToInt32(Info[3]);

				if((AdminFlag)rang == AdminFlag.Administrator || (AdminFlag)rang == AdminFlag.Operator)
				{
					SchumixBase.DManager.QueryFirstRow("UPDATE adminok SET flag = '{0}' WHERE nev = '{1}'", rang, nev);
					Log.Notice("Console", "Rang sikeresen modósitva.");
				}
				else
					Log.Error("Console", "Hibás rang!");
			}
			else
				Log.Notice("Console", "Parancsok: help | lista | add | del");
		}

		protected void HandleFunkcio()
		{
			if(Info.Length < 2)
			{
				Log.Error("Console", "Nincs megadva egy parameter!");
				return;
			}

			if(Info[1] == "info")
			{
				string f = Network.sChannelInfo.FunkciokInfo();
				if(f == "Hibás lekérdezés!")
				{
					Log.Error("Console", "Hibás lekerdezes!");
					return;
				}

				string[] FunkcioInfo = f.Split('|');
				if(FunkcioInfo.Length < 2)
					return;
	
				Log.Notice("Console", "Bekapcsolva: {0}", FunkcioInfo[0]);
				Log.Notice("Console", "Kikapcsolva: {0}", FunkcioInfo[1]);
			}
			else
			{
				if(Info.Length < 3)
				{
					Log.Error("Console", "Nincs a funkcio nev megadva!");
					return;
				}

				if(Info[1] == "be" || Info[1] == "ki")
				{
					Log.Notice("Console", "{0}: {1}kapcsolva", Info[2], Info[1]);
					SchumixBase.DManager.QueryFirstRow("UPDATE schumix SET funkcio_status = '{0}' WHERE funkcio_nev = '{1}'", Info[1], Info[2]);
				}
			}
		}

		protected void HandleChannel()
		{
			if(Info.Length < 2)
			{
				Log.Notice("Console", "Parancsok: add | del | info | update");
				return;
			}

			if(Info[1] == "add")
			{
				if(Info.Length < 3)
				{
					Log.Error("Console", "Nincs megadva a csatorna neve!");
					return;
				}

				string szobainfo = Info[2];

				if(Info.Length == 4)
				{
					string jelszo = Info[3];
					sSender.Join(szobainfo, jelszo);
					SchumixBase.DManager.QueryFirstRow("INSERT INTO `channel`(szoba, jelszo) VALUES ('{0}', '{1}')", szobainfo, jelszo);
					SchumixBase.DManager.QueryFirstRow("UPDATE channel SET aktivitas = 'aktiv' WHERE szoba = '{0}'", szobainfo);
				}
				else
				{
					sSender.Join(szobainfo);
					SchumixBase.DManager.QueryFirstRow("INSERT INTO `channel`(szoba, jelszo) VALUES ('{0}', '')", szobainfo);
					SchumixBase.DManager.QueryFirstRow("UPDATE channel SET aktivitas = 'aktiv' WHERE szoba = '{0}'", szobainfo);
				}

				Log.Notice("Console", "Channel hozzaadva: {0}", szobainfo);

				Network.sChannelInfo.ChannelListaReload();
				Network.sChannelInfo.ChannelFunkcioReload();
			}
			else if(Info[1] == "del")
			{
				if(Info.Length < 3)
				{
					Log.Error("Console", "Nincs megadva a csatorna neve!");
					return;
				}

				string szobainfo = Info[2];
				sSender.Part(szobainfo);
				SchumixBase.DManager.QueryFirstRow("DELETE FROM `channel` WHERE szoba = '{0}'", szobainfo);
				Log.Notice("Console", "Channel eltavolitva: {0}", szobainfo);

				Network.sChannelInfo.ChannelListaReload();
				Network.sChannelInfo.ChannelFunkcioReload();
			}
			else if(Info[1] == "update")
			{
				Network.sChannelInfo.ChannelListaReload();
				Network.sChannelInfo.ChannelFunkcioReload();
				Log.Notice("Console", "A csatorna informaciok frissitesre kerultek.");
			}
			else if(Info[1] == "info")
			{
				var db = SchumixBase.DManager.Query("SELECT szoba, aktivitas, error FROM channel");
				if(db != null)
				{
					string Aktivszobak = string.Empty, DeAktivszobak = string.Empty;
					bool adatszoba = false, adatszoba1 = false;

					for(int i = 0; i < db.Rows.Count; ++i)
					{
						var row = db.Rows[i];
						string szoba = row["szoba"].ToString();
						string aktivitas = row["aktivitas"].ToString();
						string error = row["error"].ToString();

						if(aktivitas == "aktiv")
						{
							Aktivszobak += ", " + szoba;
							adatszoba = true;
						}
						else if(aktivitas == "nem aktiv")
						{
							DeAktivszobak += ", " + szoba + ":" + error;
							adatszoba1 = true;
						}
					}

					if(adatszoba)
					{
						if(Aktivszobak.Substring(0, 2) == ", ")
							Aktivszobak = Aktivszobak.Remove(0, 2);

						Log.Notice("Console", "Aktiv: {0}", Aktivszobak);
					}
					else
						Log.Notice("Console", "Aktiv: Nincs adat.");

					if(adatszoba1)
					{
						if(DeAktivszobak.Substring(0, 2) == ", ")
							DeAktivszobak = DeAktivszobak.Remove(0, 2);

						Log.Notice("Console", "Deaktiv: {0}", DeAktivszobak);
					}
					else
						Log.Notice("Console", "Deaktiv: Nincs adat.");
				}
				else
					Log.Error("Console", "Hibas lekerdezes!");
			}
		}

		protected void HandleConnect()
		{
			_network.Connect();
		}

		protected void HandleDisConnect()
		{
			sSender.Quit("Console: disconnect.");
			_network.DisConnect();
		}

		protected void HandleReConnect()
		{
			sSender.Quit("Console: reconnect.");
			_network.ReConnect();
		}

		protected void HandleNick()
		{
			if(Info.Length < 2)
			{
				Log.Error("Console", "Nincs nev megadva!");
				return;
			}

			string nick = Info[1];
			sNickInfo.ChangeNick(nick);
			sSender.Nick(nick);
			Log.Notice("Console", "Nick megvaltoztatasa erre: {0}", nick);
		}

		protected void HandleJoin()
		{
			if(Info.Length < 2)
			{
				Log.Error("Console", "Nincs megadva a csatorna neve!");
				return;
			}

			if(Info.Length == 2)
				sSender.Join(Info[1]);
			else if(Info.Length == 3)
				sSender.Join(Info[1], Info[2]);

			Log.Notice("Console", "Kapcsolodas ehez a csatonahoz: {0}", Info[1]);
		}

		protected void HandleLeft()
		{
			if(Info.Length < 2)
			{
				Log.Error("Console", "Nincs megadva a csatorna neve!");
				return;
			}

			sSender.Part(Info[1]);
			Log.Notice("Console", "Lelepes errol a csatornarol: {0}", Info[1]);
		}

		protected void HandleKikapcs()
		{
			SchumixBase.time.SaveUptime();
			Log.Notice("Console", "Viszlat :(");
			sSender.Quit("Console: leállás.");
			Thread.Sleep(1000);
			Environment.Exit(1);
		}
	}
}