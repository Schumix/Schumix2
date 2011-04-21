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
using System.Data;
using System.Threading;
using System.Diagnostics;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;

namespace Schumix.Console.Commands
{
	public class CommandHandler : ConsoleLog
	{
		private readonly Sender sSender = Singleton<Sender>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
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

			if(parancsok.Length > 1 && parancsok.Substring(0, 2) == ", ")
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

			if(Info[1].ToLower() == "be")
			{
				Log.Notice("Console", "Console logolas bekapcsolva");
				ChangeLog(true);
			}
			else if(Info[1].ToLower() == "ki")
			{
				Log.Notice("Console", "Console logolas kikapcsolva");
				ChangeLog(false);
			}
		}

		protected void HandleSys()
		{
			var memory = Process.GetCurrentProcess().WorkingSet64/1024/1024;
			Log.Notice("Console", "Verzio: {0}", Verzio.SchumixVerzio);
			Log.Notice("Console", "Platform: {0}", sUtilities.GetPlatform());
			Log.Notice("Console", "OSVerzio: {0}", Environment.OSVersion.ToString());
			Log.Notice("Console", "Programnyelv: c#");
			Log.Notice("Console", "Memoria hasznalat: {0} MB", memory);
			Log.Notice("Console", "Thread count: {0}", Process.GetCurrentProcess().Threads.Count);
			Log.Notice("Console", "Uptime: {0}", SchumixBase.time.CUptime());
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
			if(Info.Length >= 2 && Info[1].ToLower() == "help")
			{
				Log.Notice("Console", "Alparancsok hasznalata:");
				Log.Notice("Console", "Admin lista: admin lista");
				Log.Notice("Console", "Hozzaadas: admin add <admin neve>");
				Log.Notice("Console", "Eltavolitas: admin del <admin neve>");
				Log.Notice("Console", "Rang: admin rang <admin neve> <uj rang pl halfoperator: 0, operator: 1, administrator: 2>");
				Log.Notice("Console", "Info: admin info <admin neve>");
			}
			else if(Info.Length >= 2 && Info[1].ToLower() == "info")
			{
				if(Info.Length < 3)
				{
					Log.Error("Console", "Nincs nev megadva!");
					return;
				}

				int flag;

				var db = SchumixBase.DManager.QueryFirstRow("SELECT Flag FROM adminok WHERE Name = '{0}'", Info[2].ToLower());
				if(!db.IsNull())
					flag = Convert.ToInt32(db["Flag"].ToString());
				else
					flag = -1;

				if((AdminFlag)flag == AdminFlag.HalfOperator)
					Log.Notice("Console", "Jelenleg Fel Operator.");		
				else if((AdminFlag)flag == AdminFlag.Operator)
					Log.Notice("Console", "Jelenleg Operator.");
				else if((AdminFlag)flag == AdminFlag.Administrator)
					Log.Notice("Console", "Jelenleg Adminisztrator.");
			}
			else if(Info.Length >= 2 && Info[1].ToLower() == "lista")
			{
				var db = SchumixBase.DManager.Query("SELECT Name FROM adminok");
				if(!db.IsNull())
				{
					string adminok = string.Empty;

					foreach(DataRow row in db.Rows)
					{
						string nev = row["Name"].ToString();
						adminok += ", " + nev;
					}

					if(adminok.Length > 1 && adminok.Substring(0, 2) == ", ")
						adminok = adminok.Remove(0, 2);

					Log.Notice("Console", "Adminok: {0}", adminok);
				}
				else
					Log.Error("Console", "Hibas lekerdezes!");
			}
			else if(Info.Length >= 2 && Info[1].ToLower() == "add")
			{
				if(Info.Length < 3)
				{
					Log.Error("Console", "Nincs nev megadva!");
					return;
				}

				string nev = Info[2];
				var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM adminok WHERE Name = '{0}'", nev.ToLower());
				if(!db.IsNull())
				{
					Log.Warning("Console", "A nev mar szerepel az admin listan!");
					return;
				}

				string pass = sUtilities.GetRandomString();
				SchumixBase.DManager.QueryFirstRow("INSERT INTO `adminok`(Name, Password) VALUES ('{0}', '{1}')", nev.ToLower(), sUtilities.Sha1(pass));
				SchumixBase.DManager.QueryFirstRow("INSERT INTO `hlmessage`(Name, Enabled) VALUES ('{0}', 'ki')", nev.ToLower());
				Log.Notice("Console", "Admin hozzaadva: {0}", nev);
				Log.Notice("Console", "Mostani jelszo: {0}", pass);
			}
			else if(Info.Length >= 2 && Info[1].ToLower() == "del")
			{
				if(Info.Length < 3)
				{
					Log.Error("Console", "Nincs nev megadva!");
					return;
				}

				string nev = Info[2];
				var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM adminok WHERE Name = '{0}'", nev.ToLower());
				if(db.IsNull())
				{
					Log.Warning("Console", "Ilyen nev nem letezik!");
					return;
				}

				SchumixBase.DManager.QueryFirstRow("DELETE FROM `adminok` WHERE Name = '{0}'", nev.ToLower());
				SchumixBase.DManager.QueryFirstRow("DELETE FROM `hlmessage` WHERE Name = '{0}'", nev.ToLower());
				Log.Notice("Console", "Admin törölve: {0}", nev);
			}
			else if(Info.Length >= 2 && Info[1].ToLower() == "rang")
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

				if((AdminFlag)rang == AdminFlag.Administrator || (AdminFlag)rang == AdminFlag.Operator || (AdminFlag)rang == AdminFlag.HalfOperator)
				{
					SchumixBase.DManager.QueryFirstRow("UPDATE adminok SET Flag = '{0}' WHERE Name = '{1}'", rang, nev);
					Log.Notice("Console", "Rang sikeresen modositva.");
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

			if(Info[1].ToLower() == "info")
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

				if(Info[1].ToLower() == "be" || Info[1].ToLower() == "ki")
				{
					Log.Notice("Console", "{0}: {1}kapcsolva", Info[2].ToLower(), Info[1].ToLower());
					SchumixBase.DManager.QueryFirstRow("UPDATE schumix SET funkcio_status = '{0}' WHERE funkcio_nev = '{1}'", Info[1].ToLower(), Info[2].ToLower());
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

			if(Info[1].ToLower() == "add")
			{
				if(Info.Length < 3)
				{
					Log.Error("Console", "Nincs megadva a csatorna neve!");
					return;
				}

				string csatornainfo = Info[2].ToLower();
				var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM channel WHERE Channel = '{0}'", csatornainfo);
				if(!db.IsNull())
				{
					Log.Warning("Console", "A nev mar szerepel a csatorna listan!");
					return;
				}

				if(Info.Length == 4)
				{
					string jelszo = Info[3];
					sSender.Join(csatornainfo, jelszo);
					SchumixBase.DManager.QueryFirstRow("INSERT INTO `channel`(Channel, Password) VALUES ('{0}', '{1}')", csatornainfo, jelszo);
					SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Enabled = 'true' WHERE Channel = '{0}'", csatornainfo);
				}
				else
				{
					sSender.Join(csatornainfo);
					SchumixBase.DManager.QueryFirstRow("INSERT INTO `channel`(Channel, Password) VALUES ('{0}', '')", csatornainfo);
					SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Enabled = 'true' WHERE Channel = '{0}'", csatornainfo);
				}

				Log.Notice("Console", "Csatorna hozzaadva: {0}", csatornainfo);

				Network.sChannelInfo.ChannelListaReload();
				Network.sChannelInfo.ChannelFunkcioReload();
			}
			else if(Info[1].ToLower() == "del")
			{
				if(Info.Length < 3)
				{
					Log.Error("Console", "Nincs megadva a csatorna neve!");
					return;
				}

				string csatornainfo = Info[2].ToLower();
				var db = SchumixBase.DManager.QueryFirstRow("SELECT Id FROM channel WHERE Channel = '{0}'", csatornainfo);
				if(!db.IsNull())
				{
					int id = Convert.ToInt32(db["Id"].ToString());
					if(id == 1)
					{
						Log.Warning("Console", "A mester csatorna nem törölhető!");
						return;
					}
				}

				db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM channel WHERE Channel = '{0}'", csatornainfo);
				if(db.IsNull())
				{
					Log.Warning("Console", "Ilyen csatorna nem letezik!");
					return;
				}

				sSender.Part(csatornainfo);
				SchumixBase.DManager.QueryFirstRow("DELETE FROM `channel` WHERE Channel = '{0}'", csatornainfo);
				Log.Notice("Console", "Csatorna eltavolitva: {0}", csatornainfo);

				Network.sChannelInfo.ChannelListaReload();
				Network.sChannelInfo.ChannelFunkcioReload();
			}
			else if(Info[1].ToLower() == "update")
			{
				Network.sChannelInfo.ChannelListaReload();
				Network.sChannelInfo.ChannelFunkcioReload();
				Log.Notice("Console", "A csatorna informaciok frissitesre kerultek.");
			}
			else if(Info[1].ToLower() == "info")
			{
				var db = SchumixBase.DManager.Query("SELECT Channel, Enabled, Error FROM channel");
				if(!db.IsNull())
				{
					string AktivCsatornak = string.Empty, DeAktivCsatornak = string.Empty;
					bool AdatCsatorna = false, AdatCsatorna1 = false;

					foreach(DataRow row in db.Rows)
					{
						string csatorna = row["Channel"].ToString();
						string aktivitas = row["Enabled"].ToString();
						string error = row["Error"].ToString();

						if(aktivitas == "true")
						{
							AktivCsatornak += ", " + csatorna;
							AdatCsatorna = true;
						}
						else if(aktivitas == "false")
						{
							DeAktivCsatornak += ", " + csatorna + ":" + error;
							AdatCsatorna1 = true;
						}
					}

					if(AdatCsatorna)
					{
						if(AktivCsatornak.Substring(0, 2) == ", ")
							AktivCsatornak = AktivCsatornak.Remove(0, 2);

						Log.Notice("Console", "Aktiv: {0}", AktivCsatornak);
					}
					else
						Log.Notice("Console", "Aktiv: Nincs adat.");

					if(AdatCsatorna1)
					{
						if(DeAktivCsatornak.Substring(0, 2) == ", ")
							DeAktivCsatornak = DeAktivCsatornak.Remove(0, 2);

						Log.Notice("Console", "Deaktiv: {0}", DeAktivCsatornak);
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
			sSender.Quit("Console: Program leállítása.");
			Thread.Sleep(1000);
			Environment.Exit(1);
		}
	}
}