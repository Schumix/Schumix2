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
		private readonly AddonManager sAddonManager = Singleton<AddonManager>.Instance;
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
				Log.Error("Console", sLManager.GetConsoleWarningText("NoValue"));
				return;
			}

			var text = sLManager.GetConsoleCommandTexts("consolelog");
			if(text.Length < 2)
			{
				Log.Error("Console", "No translations found!");
				return;
			}

			if(Info[1].ToLower() == "on")
			{
				Log.Notice("Console", text[0]);
				ChangeLog(true);
			}
			else if(Info[1].ToLower() == "off")
			{
				Log.Notice("Console", text[1]);
				ChangeLog(false);
			}
		}

		protected void HandleSys()
		{
			var text = sLManager.GetConsoleCommandTexts("sys");
			if(text.Length < 7)
			{
				Log.Error("Console", "No translations found!");
				return;
			}

			var memory = Process.GetCurrentProcess().WorkingSet64/1024/1024;
			Log.Notice("Console", text[0], sUtilities.GetVersion());
			Log.Notice("Console", text[1], sUtilities.GetPlatform());
			Log.Notice("Console", text[2], Environment.OSVersion.ToString());
			Log.Notice("Console", text[3]);
			Log.Notice("Console", text[4], memory);
			Log.Notice("Console", text[5], Process.GetCurrentProcess().Threads.Count);
			Log.Notice("Console", text[6], SchumixBase.timer.CUptime());
		}

		protected void HandleCsatorna()
		{
			if(Info.Length < 2)
			{
				Log.Error("Console", sLManager.GetConsoleWarningText("NoChannelName"));
				return;
			}

			_channel = Info[1];
			Log.Notice("Console", sLManager.GetConsoleCommandText("csatorna"), Info[1]);
			System.Console.Title = SchumixBase.Title + " || Console Writing Channel: " + Info[1];
		}

		protected void HandleAdmin()
		{
			if(Info.Length >= 2 && Info[1].ToLower() == "info")
			{
				if(Info.Length < 3)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NoName"));
					return;
				}

				var text = sLManager.GetConsoleCommandTexts("admin/info");
				if(text.Length < 3)
				{
					Log.Error("Console", "No translations found!");
					return;
				}

				var db = SchumixBase.DManager.QueryFirstRow("SELECT Flag FROM admins WHERE Name = '{0}'", Info[2].ToLower());
				int flag = !db.IsNull() ? Convert.ToInt32(db["Flag"].ToString()) : -1;

				if((AdminFlag)flag == AdminFlag.HalfOperator)
					Log.Notice("Console", text[0]);		
				else if((AdminFlag)flag == AdminFlag.Operator)
					Log.Notice("Console", text[1]);
				else if((AdminFlag)flag == AdminFlag.Administrator)
					Log.Notice("Console", text[2]);
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

					Log.Notice("Console", sLManager.GetConsoleCommandText("admin/list"), admins.Remove(0, 2, ", "));
				}
				else
					Log.Error("Console", sLManager.GetConsoleWarningText("FaultyQuery"));
			}
			else if(Info.Length >= 2 && Info[1].ToLower() == "add")
			{
				if(Info.Length < 3)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NoName"));
					return;
				}

				var text = sLManager.GetConsoleCommandTexts("admin/add");
				if(text.Length < 3)
				{
					Log.Error("Console", "No translations found!");
					return;
				}

				string name = Info[2];
				var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM admins WHERE Name = '{0}'", name.ToLower());
				if(!db.IsNull())
				{
					Log.Warning("Console", text[0]);
					return;
				}

				string pass = sUtilities.GetRandomString();
				SchumixBase.DManager.QueryFirstRow("INSERT INTO `admins`(Name, Password) VALUES ('{0}', '{1}')", name.ToLower(), sUtilities.Sha1(pass));
				SchumixBase.DManager.QueryFirstRow("INSERT INTO `hlmessage`(Name, Enabled) VALUES ('{0}', 'ki')", name.ToLower());
				Log.Notice("Console", text[1], name);
				Log.Notice("Console", text[2], pass);
			}
			else if(Info.Length >= 2 && Info[1].ToLower() == "remove")
			{
				if(Info.Length < 3)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NoName"));
					return;
				}

				var text = sLManager.GetConsoleCommandTexts("admin/remove");
				if(text.Length < 2)
				{
					Log.Error("Console", "No translations found!");
					return;
				}

				string name = Info[2];
				var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM admins WHERE Name = '{0}'", name.ToLower());
				if(db.IsNull())
				{
					Log.Warning("Console", text[0]);
					return;
				}

				SchumixBase.DManager.QueryFirstRow("DELETE FROM `admins` WHERE Name = '{0}'", name.ToLower());
				SchumixBase.DManager.QueryFirstRow("DELETE FROM `hlmessage` WHERE Name = '{0}'", name.ToLower());
				Log.Notice("Console", text[1], name);
			}
			else if(Info.Length >= 2 && Info[1].ToLower() == "rank")
			{
				if(Info.Length < 3)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NoName"));
					return;
				}

				if(Info.Length < 4)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NoRank"));
					return;
				}

				var text = sLManager.GetConsoleCommandTexts("admin/rank");
				if(text.Length < 2)
				{
					Log.Error("Console", "No translations found!");
					return;
				}

				string name = Info[2].ToLower();
				int rank = Convert.ToInt32(Info[3]);

				if((AdminFlag)rank == AdminFlag.Administrator || (AdminFlag)rank == AdminFlag.Operator || (AdminFlag)rank == AdminFlag.HalfOperator)
				{
					SchumixBase.DManager.QueryFirstRow("UPDATE admins SET Flag = '{0}' WHERE Name = '{1}'", rank, name);
					Log.Notice("Console", text[0]);
				}
				else
					Log.Error("Console", text[1]);
			}
			else
				Log.Notice("Console", sLManager.GetConsoleCommandText("admin"));
		}

		protected void HandleFunction()
		{
			if(Info.Length < 2)
			{
				Log.Error("Console", sLManager.GetConsoleWarningText("NoValue1"));
				return;
			}

			if(Info[1].ToLower() == "channel")
			{
				if(Info.Length < 3)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NoChannelName"));
					return;
				}

				if(Info.Length < 4)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NoValue1"));
					return;
				}
			
				string channel = Info[2].ToLower();
				string status = Info[3].ToLower();
			
				if(Info[3].ToLower() == "info")
				{
					var text = sLManager.GetConsoleCommandTexts("function/channel/info");
					if(text.Length < 2)
					{
						Log.Error("Console", "No translations found!");
						return;
					}

					string[] ChannelInfo = sChannelInfo.ChannelFunctionsInfo(channel).Split('|');
					if(ChannelInfo.Length < 2)
						return;

					Log.Notice("Console", text[0], ChannelInfo[0]);
					Log.Notice("Console", text[1], ChannelInfo[1]);
				}
				else if(status == "on" || status == "off")
				{
					if(Info.Length < 5)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoFunctionName"));
						return;
					}

					var text = sLManager.GetConsoleCommandTexts("function/channel");
					if(text.Length < 2)
					{
						Log.Error("Console", "No translations found!");
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
							Log.Notice("Console", text[0],  args.Remove(0, 2, ", "));
						else
							Log.Notice("Console", text[1],  args.Remove(0, 2, ", "));
					}
					else
					{
						if(status == "on")
							Log.Notice("Console", text[0], status);
						else
							Log.Notice("Console", text[1], status);

						SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Functions = '{0}' WHERE Channel = '{1}'", sChannelInfo.ChannelFunctions(Info[4].ToLower(), status, channel), channel);
						sChannelInfo.ChannelFunctionReload();
					}
				}
			}
			else if(Info[1].ToLower() == "update")
			{
				if(Info.Length < 3)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NoValue1"));
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
						Log.Notice("Console", sLManager.GetConsoleCommandText("function/update/all"));
					}
					else
						Log.Error("Console", sLManager.GetConsoleWarningText("FaultyQuery"));
				}
				else
				{
					Log.Notice("Console", sLManager.GetConsoleCommandText("function/update"), Info[2].ToLower());
					SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Functions = '{0}' WHERE Channel = '{1}'", sUtilities.GetFunctionUpdate(), Info[2].ToLower());
					sChannelInfo.ChannelFunctionReload();
				}
			}
			else if(Info[1].ToLower() == "info")
			{
				var text = sLManager.GetConsoleCommandTexts("function/info");
				if(text.Length < 2)
				{
					Log.Error("Console", "No translations found!");
					return;
				}

				string f = sChannelInfo.FunctionsInfo();
				if(f == "Hibás lekérdezés!")
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("FaultyQuery"));
					return;
				}

				string[] FunkcioInfo = f.Split('|');
				if(FunkcioInfo.Length < 2)
					return;
	
				Log.Notice("Console", text[0], FunkcioInfo[0]);
				Log.Notice("Console", text[1], FunkcioInfo[1]);
			}
			else
			{
				if(Info.Length < 3)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NoFunctionName"));
					return;
				}

				var text = sLManager.GetConsoleCommandTexts("function");
				if(text.Length < 2)
				{
					Log.Error("Console", "No translations found!");
					return;
				}

				if(Info[1].ToLower() == "on" || Info[1].ToLower() == "off")
				{
					if(Info[1].ToLower() == "on")
						Log.Notice("Console", text[0], Info[2].ToLower());
					else
						Log.Notice("Console", text[1], Info[2].ToLower());

					SchumixBase.DManager.QueryFirstRow("UPDATE schumix SET FunctionStatus = '{0}' WHERE FunctionName = '{1}'", Info[1].ToLower(), Info[2].ToLower());
				}
			}
		}

		protected void HandleChannel()
		{
			if(Info.Length < 2)
			{
				Log.Notice("Console", sLManager.GetConsoleCommandText("channel"));
				return;
			}

			if(Info[1].ToLower() == "add")
			{
				if(Info.Length < 3)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NoChannelName"));
					return;
				}

				var text = sLManager.GetConsoleCommandTexts("channel/add");
				if(text.Length < 2)
				{
					Log.Error("Console", "No translations found!");
					return;
				}

				string channel = Info[2].ToLower();
				var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM channel WHERE Channel = '{0}'", channel);
				if(!db.IsNull())
				{
					Log.Warning("Console", text[0]);
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

				Log.Notice("Console", text[1], channel);
				sChannelInfo.ChannelListReload();
				sChannelInfo.ChannelFunctionReload();
			}
			else if(Info[1].ToLower() == "remove")
			{
				if(Info.Length < 3)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NoChannelName"));
					return;
				}

				var text = sLManager.GetConsoleCommandTexts("channel/remove");
				if(text.Length < 3)
				{
					Log.Error("Console", "No translations found!");
					return;
				}

				string channel = Info[2].ToLower();
				var db = SchumixBase.DManager.QueryFirstRow("SELECT Id FROM channel WHERE Channel = '{0}'", channel);
				if(!db.IsNull())
				{
					int id = Convert.ToInt32(db["Id"].ToString());
					if(id == 1)
					{
						Log.Warning("Console", text[0]);
						return;
					}
				}

				db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM channel WHERE Channel = '{0}'", channel);
				if(db.IsNull())
				{
					Log.Warning("Console", text[1]);
					return;
				}

				sSender.Part(channel);
				SchumixBase.DManager.QueryFirstRow("DELETE FROM `channel` WHERE Channel = '{0}'", channel);
				Log.Notice("Console", text[2], channel);

				sChannelInfo.ChannelListReload();
				sChannelInfo.ChannelFunctionReload();
			}
			else if(Info[1].ToLower() == "update")
			{
				sChannelInfo.ChannelListReload();
				sChannelInfo.ChannelFunctionReload();
				Log.Notice("Console", sLManager.GetConsoleCommandText("channel/update"));
			}
			else if(Info[1].ToLower() == "info")
			{
				var text = sLManager.GetConsoleCommandTexts("channel/info");
				if(text.Length < 3)
				{
					Log.Error("Console", "No translations found!");
					return;
				}

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
						Log.Notice("Console", text[0], ActiveChannels.Remove(0, 2, ", "));
					else
						Log.Notice("Console", text[1]);

					if(InActiveChannels.Length > 0)
						Log.Notice("Console", text[2], InActiveChannels.Remove(0, 2, ", "));
					else
						Log.Notice("Console", text[3]);
				}
				else
					Log.Error("Console", sLManager.GetConsoleWarningText("FaultyQuery"));
			}
			else if(Info[1].ToLower() == "language")
			{
				if(Info.Length < 3)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NoChannelName"));
					return;
				}

				if(Info.Length < 4)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NoChannelLanguage"));
					return;
				}

				SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Language = '{0}' WHERE Channel = '{1}'", Info[3], Info[2].ToLower());
				Log.Notice("Console", sLManager.GetConsoleCommandText("channel/language"), Info[3]);
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
				Log.Error("Console", sLManager.GetConsoleWarningText("NoName"));
				return;
			}

			string nick = Info[1];
			sNickInfo.ChangeNick(nick);
			sSender.Nick(nick);
			Log.Notice("Console", sLManager.GetConsoleCommandText("nick"), nick);
		}

		protected void HandleJoin()
		{
			if(Info.Length < 2)
			{
				Log.Error("Console", sLManager.GetConsoleWarningText("NoChannelName"));
				return;
			}

			if(Info.Length == 2)
				sSender.Join(Info[1]);
			else if(Info.Length == 3)
				sSender.Join(Info[1], Info[2]);

			Log.Notice("Console", sLManager.GetConsoleCommandText("join"), Info[1]);
		}

		protected void HandleLeft()
		{
			if(Info.Length < 2)
			{
				Log.Error("Console", sLManager.GetConsoleWarningText("NoChannelName"));
				return;
			}

			sSender.Part(Info[1]);
			Log.Notice("Console", sLManager.GetConsoleCommandText("left"), Info[1]);
		}

		protected void HandleReload()
		{
			if(Info.Length < 2)
			{
				Log.Error("Console", sLManager.GetConsoleWarningText("NoName"));
				return;
			}

			var text = sLManager.GetConsoleCommandTexts("reload");
			if(text.Length < 2)
			{
				Log.Error("Console", "No translations found!");
				return;
			}

			bool status = false;

			switch(Info[1].ToLower())
			{
				case "config":
					new Config(SchumixConfig.ConfigDirectory, SchumixConfig.ConfigFile);
					status = true;
					break;
			}

			foreach(var plugin in sAddonManager.GetPlugins())
			{
				if(plugin.Reload(Info[1]))
					status = true;
			}

			if(status)
				Log.Notice("Console", text[0], Info[1]);
			else
				Log.Error("Console", text[1]);
		}

		protected void HandleQuit()
		{
			var text = sLManager.GetConsoleCommandTexts("quit");
			if(text.Length < 2)
			{
				Log.Error("Console", "No translations found!");
				return;
			}

			SchumixBase.ExitStatus = true;
			SchumixBase.timer.SaveUptime();
			Log.Notice("Console", text[0]);
			sSender.Quit(text[1]);
			Thread.Sleep(1000);
			Environment.Exit(1);
		}
	}
}