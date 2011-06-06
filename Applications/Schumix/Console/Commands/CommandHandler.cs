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
using Schumix.Framework.Localization;

namespace Schumix.Console.Commands
{
	public partial class CommandHandler : ConsoleLog
	{
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly ChannelInfo sChannelInfo = Singleton<ChannelInfo>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly NickInfo sNickInfo = Singleton<NickInfo>.Instance;
		private readonly Sender sSender = Singleton<Sender>.Instance;
		private readonly Network _network;
		protected string[] Info;
		protected string _channel;

		protected CommandHandler(Network network) : base(LogConfig.IrcLog)
		{
			_network = network;
		}

		protected void HandleConsoleLog()
		{
			if(Info.Length < 2)
			{
				Log.Error("Console", "Nincs parameter!");
				return;
			}

			if(Info[1].ToLower() == "on")
			{
				Log.Notice("Console", "Console logolas bekapcsolva");
				ChangeLog(true);
			}
			else if(Info[1].ToLower() == "off")
			{
				Log.Notice("Console", "Console logolas kikapcsolva");
				ChangeLog(false);
			}
		}

		protected void HandleSys()
		{
			var memory = Process.GetCurrentProcess().WorkingSet64/1024/1024;
			Log.Notice("Console", "Verzio: {0}", sUtilities.GetVersion());
			Log.Notice("Console", "Platform: {0}", sUtilities.GetPlatform());
			Log.Notice("Console", "OSVerzio: {0}", Environment.OSVersion.ToString());
			Log.Notice("Console", "Programnyelv: c#");
			Log.Notice("Console", "Memoria hasznalat: {0} MB", memory);
			Log.Notice("Console", "Thread count: {0}", Process.GetCurrentProcess().Threads.Count);
			Log.Notice("Console", "Uptime: {0}", SchumixBase.timer.CUptime());
		}

		protected void HandleCsatorna()
		{
			if(Info.Length < 2)
			{
				Log.Error("Console", "Nincs megadva a csatorna neve!");
				return;
			}

			_channel = Info[1];
			Log.Notice("Console", "Uj csatorna ahova mostantol lehet irni: {0}", Info[1]);
			System.Console.Title = SchumixBase.Title + " || Console Writing Channel: " + Info[1];
		}

		protected void HandleAdmin()
		{
			if(Info.Length >= 2 && Info[1].ToLower() == "info")
			{
				if(Info.Length < 3)
				{
					Log.Error("Console", "Nincs nev megadva!");
					return;
				}

				var db = SchumixBase.DManager.QueryFirstRow("SELECT Flag FROM admins WHERE Name = '{0}'", Info[2].ToLower());
				int flag = !db.IsNull() ? Convert.ToInt32(db["Flag"].ToString()) : -1;

				if((AdminFlag)flag == AdminFlag.HalfOperator)
					Log.Notice("Console", "Jelenleg Fel Operator.");		
				else if((AdminFlag)flag == AdminFlag.Operator)
					Log.Notice("Console", "Jelenleg Operator.");
				else if((AdminFlag)flag == AdminFlag.Administrator)
					Log.Notice("Console", "Jelenleg Adminisztrator.");
			}
			else if(Info.Length >= 2 && Info[1].ToLower() == "list")
			{
				var db = SchumixBase.DManager.Query("SELECT Name FROM admins");
				if(!db.IsNull())
				{
					string admins = string.Empty;

					foreach(DataRow row in db.Rows)
					{
						string name = row["Name"].ToString();
						admins += ", " + name;
					}

					Log.Notice("Console", "Adminok: {0}", admins.Remove(0, 2, ", "));
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

				string name = Info[2];
				var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM admins WHERE Name = '{0}'", name.ToLower());
				if(!db.IsNull())
				{
					Log.Warning("Console", "A nev mar szerepel az admin listan!");
					return;
				}

				string pass = sUtilities.GetRandomString();
				SchumixBase.DManager.QueryFirstRow("INSERT INTO `admins`(Name, Password) VALUES ('{0}', '{1}')", name.ToLower(), sUtilities.Sha1(pass));
				SchumixBase.DManager.QueryFirstRow("INSERT INTO `hlmessage`(Name, Enabled) VALUES ('{0}', 'ki')", name.ToLower());
				Log.Notice("Console", "Admin hozzaadva: {0}", name);
				Log.Notice("Console", "Mostani jelszo: {0}", pass);
			}
			else if(Info.Length >= 2 && Info[1].ToLower() == "remove")
			{
				if(Info.Length < 3)
				{
					Log.Error("Console", "Nincs nev megadva!");
					return;
				}

				string name = Info[2];
				var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM admins WHERE Name = '{0}'", name.ToLower());
				if(db.IsNull())
				{
					Log.Warning("Console", "Ilyen nev nem letezik!");
					return;
				}

				SchumixBase.DManager.QueryFirstRow("DELETE FROM `admins` WHERE Name = '{0}'", name.ToLower());
				SchumixBase.DManager.QueryFirstRow("DELETE FROM `hlmessage` WHERE Name = '{0}'", name.ToLower());
				Log.Notice("Console", "Admin törölve: {0}", name);
			}
			else if(Info.Length >= 2 && Info[1].ToLower() == "rank")
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

				string name = Info[2].ToLower();
				int rank = Convert.ToInt32(Info[3]);

				if((AdminFlag)rank == AdminFlag.Administrator || (AdminFlag)rank == AdminFlag.Operator || (AdminFlag)rank == AdminFlag.HalfOperator)
				{
					SchumixBase.DManager.QueryFirstRow("UPDATE admins SET Flag = '{0}' WHERE Name = '{1}'", rank, name);
					Log.Notice("Console", "Rang sikeresen modositva.");
				}
				else
					Log.Error("Console", "Hibás rang!");
			}
			else
				Log.Notice("Console", "Parancsok: help | list | add | remove");
		}

		protected void HandleFunction()
		{
			if(Info.Length < 2)
			{
				Log.Error("Console", "Nincs megadva egy parameter!");
				return;
			}

			if(Info[1].ToLower() == "channel")
			{
				if(Info.Length < 3)
				{
					Log.Error("Console", "Nincs megadva a csatorna neve!");
					return;
				}

				if(Info.Length < 4)
				{
					Log.Error("Console", "Nincs megadva egy parameter!");
					return;
				}
			
				string channel = Info[2].ToLower();
				string status = Info[3].ToLower();
			
				if(Info[3].ToLower() == "info")
				{
					string[] ChannelInfo = sChannelInfo.ChannelFunctionsInfo(channel).Split('|');
					if(ChannelInfo.Length < 2)
						return;

					Log.Notice("Console", "Bekapcsolva: {0}", ChannelInfo[0]);
					Log.Notice("Console", "Kikapcsolva: {0}", ChannelInfo[1]);
				}
				else if(status == "on" || status == "off")
				{
					if(Info.Length < 5)
					{
						Log.Error("Console", "Nincs megadva a funkcio neve!");
						return;
					}

					if(Info.Length >= 6)
					{
						string args = string.Empty;

						for(int i = 4; i < Info.Length; i++)
						{
							args += ", " + Info[i].ToLower();
							SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Functions = '{0}' WHERE Channel = '{1}'", sChannelInfo.ChannelFunctions(Info[i].ToLower(), status, channel), channel);
							sChannelInfo.ChannelFunctionReload();
						}

						if(status == "on")
							Log.Notice("Console", "{0}: bekapcsolva",  args.Remove(0, 2, ", "));
						else
							Log.Notice("Console", "{0}: kikapcsolva",  args.Remove(0, 2, ", "));
					}
					else
					{
						Log.Notice("Console", "{0}: {1}kapcsolva", Info[4].ToLower(), status);
						SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Functions = '{0}' WHERE Channel = '{1}'", sChannelInfo.ChannelFunctions(Info[4].ToLower(), status, channel), channel);
						sChannelInfo.ChannelFunctionReload();
					}
				}
			}
			else if(Info[1].ToLower() == "update")
			{
				if(Info.Length < 3)
				{
					Log.Error("Console", "Nincs megadva egy parameter!");
					return;
				}

				if(Info[2].ToLower() == "all")
				{
					var db = SchumixBase.DManager.Query("SELECT Channel FROM channel");
					if(!db.IsNull())
					{
						foreach(DataRow row in db.Rows)
						{
							string channel = row["Channel"].ToString();
							SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Functions = '{0}' WHERE Channel = '{1}'", sUtilities.GetFunctionUpdate(), channel);
						}

						sChannelInfo.ChannelFunctionReload();
						Log.Notice("Console", "Sikeresen frissitve minden csatornan a funkciok.");
					}
					else
						Log.Error("Console", "Hibas lekerdezes!");
				}
				else
				{
					Log.Notice("Console", "Sikeresen frissitve {0} csatornan a funkciok.", Info[2].ToLower());
					SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Functions = '{0}' WHERE Channel = '{1}'", sUtilities.GetFunctionUpdate(), Info[2].ToLower());
					sChannelInfo.ChannelFunctionReload();
				}
			}
			else if(Info[1].ToLower() == "info")
			{
				string f = sChannelInfo.FunctionsInfo();
				if(f == "Hibás lekérdezés!")
				{
					Log.Error("Console", "Hibas lekerdezes!");
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

				if(Info[1].ToLower() == "on" || Info[1].ToLower() == "off")
				{
					if(Info[1].ToLower() == "on")
						Log.Notice("Console", "{0}: bekapcsolva", Info[2].ToLower());
					else
						Log.Notice("Console", "{0}: kikapcsolva", Info[2].ToLower());

					SchumixBase.DManager.QueryFirstRow("UPDATE schumix SET FunctionStatus = '{0}' WHERE FunctionName = '{1}'", Info[1].ToLower(), Info[2].ToLower());
				}
			}
		}

		protected void HandleChannel()
		{
			if(Info.Length < 2)
			{
				Log.Notice("Console", "Parancsok: add | remove | info | update | language");
				return;
			}

			if(Info[1].ToLower() == "add")
			{
				if(Info.Length < 3)
				{
					Log.Error("Console", "Nincs megadva a csatorna neve!");
					return;
				}

				string channel = Info[2].ToLower();
				var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM channel WHERE Channel = '{0}'", channel);
				if(!db.IsNull())
				{
					Log.Warning("Console", "A nev mar szerepel a csatorna listan!");
					return;
				}

				if(Info.Length == 4)
				{
					string pass = Info[3];
					sSender.Join(channel, pass);
					SchumixBase.DManager.QueryFirstRow("INSERT INTO `channel`(Channel, Password) VALUES ('{0}', '{1}', '{2}')", channel, pass, sLManager.Locale);
					SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Enabled = 'true' WHERE Channel = '{0}'", channel);
				}
				else
				{
					sSender.Join(channel);
					SchumixBase.DManager.QueryFirstRow("INSERT INTO `channel`(Channel, Password) VALUES ('{0}', '', '{1}')", channel, sLManager.Locale);
					SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Enabled = 'true' WHERE Channel = '{0}'", channel);
				}

				Log.Notice("Console", "Csatorna hozzaadva: {0}", channel);

				sChannelInfo.ChannelListReload();
				sChannelInfo.ChannelFunctionReload();
			}
			else if(Info[1].ToLower() == "remove")
			{
				if(Info.Length < 3)
				{
					Log.Error("Console", "Nincs megadva a csatorna neve!");
					return;
				}

				string channel = Info[2].ToLower();
				var db = SchumixBase.DManager.QueryFirstRow("SELECT Id FROM channel WHERE Channel = '{0}'", channel);
				if(!db.IsNull())
				{
					int id = Convert.ToInt32(db["Id"].ToString());
					if(id == 1)
					{
						Log.Warning("Console", "A mester csatorna nem törölhető!");
						return;
					}
				}

				db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM channel WHERE Channel = '{0}'", channel);
				if(db.IsNull())
				{
					Log.Warning("Console", "Ilyen csatorna nem letezik!");
					return;
				}

				sSender.Part(channel);
				SchumixBase.DManager.QueryFirstRow("DELETE FROM `channel` WHERE Channel = '{0}'", channel);
				Log.Notice("Console", "Csatorna eltavolitva: {0}", channel);

				sChannelInfo.ChannelListReload();
				sChannelInfo.ChannelFunctionReload();
			}
			else if(Info[1].ToLower() == "update")
			{
				sChannelInfo.ChannelListReload();
				sChannelInfo.ChannelFunctionReload();
				Log.Notice("Console", "A csatorna informaciok frissitesre kerultek.");
			}
			else if(Info[1].ToLower() == "info")
			{
				var db = SchumixBase.DManager.Query("SELECT Channel, Enabled, Error FROM channel");
				if(!db.IsNull())
				{
					string ActiveChannels = string.Empty, InActiveChannels = string.Empty;

					foreach(DataRow row in db.Rows)
					{
						string channel = row["Channel"].ToString();
						bool enabled = Convert.ToBoolean(row["Enabled"].ToString());

						if(enabled)
							ActiveChannels += ", " + channel;
						else if(!enabled)
							InActiveChannels += ", " + channel + ":" + row["Error"].ToString();
					}

					if(ActiveChannels.Length > 0)
						Log.Notice("Console", "Aktiv: {0}", ActiveChannels.Remove(0, 2, ", "));
					else
						Log.Notice("Console", "Aktiv: Nincs informacio.");

					if(InActiveChannels.Length > 0)
						Log.Notice("Console", "Inaktiv: {0}", InActiveChannels.Remove(0, 2, ", "));
					else
						Log.Notice("Console", "Inaktiv: Nincs informacio.");
				}
				else
					Log.Error("Console", "Hibas lekerdezes!");
			}
			else if(Info[1].ToLower() == "language")
			{
				if(Info.Length < 3)
				{
					Log.Error("Console", "Nincs megadva a csatorna neve!");
					return;
				}

				if(Info.Length < 4)
				{
					Log.Error("Console", "Nincs megadva a csatorna nyelvezete!");
					return;
				}

				SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Language = '{0}' WHERE Channel = '{1}'", Info[3], Info[2].ToLower());
				Log.Notice("Console", "Csatorna nyelvezete sikeresen meg lett valtoztatva erre: {0}", Info[3]);
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

		protected void HandleQuit()
		{
			SchumixBase.timer.SaveUptime();
			Log.Notice("Console", "Viszlat :(");
			sSender.Quit("Console: Program leállítása.");
			Thread.Sleep(1000);
			Environment.Exit(1);
		}
	}
}