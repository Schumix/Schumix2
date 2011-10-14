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
	/// <summary>
	///     CommandHandler class.
	/// </summary>
	public partial class CommandHandler : ConsoleLog
	{
		/// <summary>
		///     Hozzáférést biztosít singleton-on keresztül a megadott class-hoz.
		///     LocalizationConsole segítségével állíthatók be a konzol nyelvi tulajdonságai.
		/// </summary>
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		/// <summary>
		///     Hozzáférést biztosít singleton-on keresztül a megadott class-hoz.
		///     LocalizationManager segítségével állítható be az irc szerver felé menő tárolt üzenetek nyelvezete.
		/// </summary>
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		/// <summary>
		///     Hozzáférést biztosít singleton-on keresztül a megadott class-hoz.
		///     Addonok kezelése.
		/// </summary>
		private readonly AddonManager sAddonManager = Singleton<AddonManager>.Instance;
		/// <summary>
		///     Hozzáférést biztosít singleton-on keresztül a megadott class-hoz.
		///     Az egyes csatornákról tárolt információkat lehet kezelni illetve csatornák számát módosítani.
		/// </summary>
		private readonly ChannelInfo sChannelInfo = Singleton<ChannelInfo>.Instance;
		/// <summary>
		///     Hozzáférést biztosít singleton-on keresztül a megadott class-hoz.
		///     Utilities sokféle függvényt tartalmaz melyek hasznosak lehetnek.
		/// </summary>
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		/// <summary>
		///     Hozzáférést biztosít singleton-on keresztül a megadott class-hoz.
		///     A nick név változtatását illetve jelenlegi kiírását teszi lehetővé.
		/// </summary>
		private readonly NickInfo sNickInfo = Singleton<NickInfo>.Instance;
		/// <summary>
		///     Hozzáférést biztosít singleton-on keresztül a megadott class-hoz.
		///     Üzenet küldés az irc szerver felé.
		/// </summary>
		private readonly Sender sSender = Singleton<Sender>.Instance;
		/// <summary>
		///     Network elérését tárólja.
		/// </summary>
		private readonly Network _network;
		/// <summary>
		///     A szétdarabolt információkat tárolja.
		/// </summary>
		protected string[] Info;
		/// <summary>
		///     Csatorna nevét tárolja.
		/// </summary>
		protected string _channel;

		/// <summary>
		///     Indulási függvény.
		/// </summary>
		protected CommandHandler(Network network) : base(LogConfig.IrcLog)
		{
			_network = network;
		}

		private bool IsChannel(string Name)
		{
			return (Name.Length >= 1 && Name.Substring(0, 1) == "#");
		}

		/// <summary>
		///     Consolelog parancs függvénye.
		/// </summary>
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
				Log.Error("Console", sLConsole.Translations("NoFound2"));
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

		/// <summary>
		///     Sys parancs függvénye.
		/// </summary>
		protected void HandleSys()
		{
			var text = sLManager.GetConsoleCommandTexts("sys");
			if(text.Length < 7)
			{
				Log.Error("Console", sLConsole.Translations("NoFound2"));
				return;
			}

			var memory = Process.GetCurrentProcess().WorkingSet64/1024/1024;
			Log.Notice("Console", text[0], sUtilities.GetVersion());
			Log.Notice("Console", text[1], sUtilities.GetPlatform());
			Log.Notice("Console", text[2], Environment.OSVersion.ToString());
			Log.Notice("Console", text[3]);
			Log.Notice("Console", text[4], memory);
			Log.Notice("Console", text[5], Process.GetCurrentProcess().Threads.Count);
			Log.Notice("Console", text[6], SchumixBase.timer.Uptime());
		}

		/// <summary>
		///     Csatorna parancs függvénye.
		/// </summary>
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

		/// <summary>
		///     Admin parancs függvénye.
		/// </summary>
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
					Log.Error("Console", sLConsole.Translations("NoFound2"));
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
					Log.Error("Console", sLConsole.Translations("NoFound2"));
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
				SchumixBase.DManager.Insert("`admins`(Name, Password)", name.ToLower(), sUtilities.Sha1(pass));
				SchumixBase.DManager.Insert("`hlmessage`(Name, Enabled)", name.ToLower(), "off");
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
					Log.Error("Console", sLConsole.Translations("NoFound2"));
					return;
				}

				string name = Info[2];
				var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM admins WHERE Name = '{0}'", name.ToLower());
				if(db.IsNull())
				{
					Log.Warning("Console", text[0]);
					return;
				}

				SchumixBase.DManager.Delete("admins", string.Format("Name = '{0}'", name.ToLower()));
				SchumixBase.DManager.Delete("hlmessage", string.Format("Name = '{0}'", name.ToLower()));
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
					Log.Error("Console", sLConsole.Translations("NoFound2"));
					return;
				}

				string name = Info[2].ToLower();
				int rank = Convert.ToInt32(Info[3]);

				if((AdminFlag)rank == AdminFlag.Administrator || (AdminFlag)rank == AdminFlag.Operator || (AdminFlag)rank == AdminFlag.HalfOperator)
				{
					SchumixBase.DManager.Update("admins", string.Format("Flag = '{0}'", rank), string.Format("Name = '{0}'", name));
					Log.Notice("Console", text[0]);
				}
				else
					Log.Error("Console", text[1]);
			}
			else
				Log.Notice("Console", sLManager.GetConsoleCommandText("admin"));
		}

		/// <summary>
		///     Function parancs függvénye.
		/// </summary>
		protected void HandleFunction()
		{
			if(Info.Length < 2)
			{
				Log.Error("Console", sLManager.GetConsoleWarningText("NoValue1"));
				return;
			}

			if(Info[1].ToLower() == "channel")
			{
				var text = sLManager.GetConsoleCommandTexts("function/channel");
				if(text.Length < 3)
				{
					Log.Error("Console", sLConsole.Translations("NoFound2"));
					return;
				}

				if(Info.Length < 3)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NoChannelName"));
					return;
				}

				if(!IsChannel(Info[2]))
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NotaChannelHasBeenSet"));
					return;
				}

				var db0 = SchumixBase.DManager.QueryFirstRow("SELECT* FROM channel WHERE Channel = '{0}'", sUtilities.SqlEscape(Info[2].ToLower()));
				if(db0.IsNull())
				{
					Log.Error("Console", text[2]);
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
					var text2 = sLManager.GetConsoleCommandTexts("function/channel/info");
					if(text2.Length < 2)
					{
						Log.Error("Console", sLConsole.Translations("NoFound2"));
						return;
					}

					string[] ChannelInfo = sChannelInfo.ChannelFunctionsInfo(channel).Split('|');
					if(ChannelInfo.Length < 2)
						return;

					Log.Notice("Console", text2[0], ChannelInfo[0]);
					Log.Notice("Console", text2[1], ChannelInfo[1]);
				}
				else if(status == "on" || status == "off")
				{
					if(Info.Length < 5)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoFunctionName"));
						return;
					}

					if(Info.Length >= 6)
					{
						string args = string.Empty;

						for(int i = 4; i < Info.Length; i++)
						{
							args += ", " + Info[i].ToLower();
							SchumixBase.DManager.Update("channel", string.Format("Functions = '{0}'", sChannelInfo.ChannelFunctions(Info[i].ToLower(), status, channel)), string.Format("Channel = '{0}'", channel));
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

						SchumixBase.DManager.Update("channel", string.Format("Functions = '{0}'", sChannelInfo.ChannelFunctions(Info[4].ToLower(), status, channel)), string.Format("Channel = '{0}'", channel));
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
							SchumixBase.DManager.Update("channel", string.Format("Functions = '{0}'", sUtilities.GetFunctionUpdate()), string.Format("Channel = '{0}'", channel));
						}

						sChannelInfo.ChannelFunctionReload();
						Log.Notice("Console", sLManager.GetConsoleCommandText("function/update/all"));
					}
					else
						Log.Error("Console", sLManager.GetConsoleWarningText("FaultyQuery"));
				}
				else
				{
					if(!IsChannel(Info[2]))
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NotaChannelHasBeenSet"));
						return;
					}

					Log.Notice("Console", sLManager.GetConsoleCommandText("function/update"), Info[2].ToLower());
					SchumixBase.DManager.Update("channel", string.Format("Functions = '{0}'", sUtilities.GetFunctionUpdate()), string.Format("Channel = '{0}'", Info[2].ToLower()));
					sChannelInfo.ChannelFunctionReload();
				}
			}
			else if(Info[1].ToLower() == "info")
			{
				var text = sLManager.GetConsoleCommandTexts("function/info");
				if(text.Length < 2)
				{
					Log.Error("Console", sLConsole.Translations("NoFound2"));
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
					Log.Error("Console", sLConsole.Translations("NoFound2"));
					return;
				}

				if(Info[1].ToLower() == "on" || Info[1].ToLower() == "off")
				{
					if(Info[1].ToLower() == "on")
						Log.Notice("Console", text[0], Info[2].ToLower());
					else
						Log.Notice("Console", text[1], Info[2].ToLower());

					SchumixBase.DManager.Update("schumix", string.Format("FunctionStatus = '{0}'", Info[1].ToLower()), string.Format("FunctionName = '{0}'", Info[2].ToLower()));
				}
			}
		}

		/// <summary>
		///     Channel parancs függvénye.
		/// </summary>
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
					Log.Error("Console", sLConsole.Translations("NoFound2"));
					return;
				}

				string channel = Info[2].ToLower();

				if(!IsChannel(channel))
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NotaChannelHasBeenSet"));
					return;
				}

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
					SchumixBase.DManager.Insert("`channel`(Channel, Password, Language)", channel, pass, sLManager.Locale);
					SchumixBase.DManager.Update("channel", "Enabled = 'true'", string.Format("Channel = '{0}'", channel));
				}
				else
				{
					sSender.Join(channel);
					SchumixBase.DManager.Insert("`channel`(Channel, Password, Language)", channel, string.Empty, sLManager.Locale);
					SchumixBase.DManager.Update("channel", "Enabled = 'true'", string.Format("Channel = '{0}'", channel));
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
					Log.Error("Console", sLConsole.Translations("NoFound2"));
					return;
				}

				string channel = Info[2].ToLower();

				if(!IsChannel(channel))
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NotaChannelHasBeenSet"));
					return;
				}

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
				SchumixBase.DManager.Delete("channel", string.Format("Channel = '{0}'", channel));
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
					Log.Error("Console", sLConsole.Translations("NoFound2"));
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
				var text = sLManager.GetConsoleCommandTexts("channel/language");
				if(text.Length < 2)
				{
					Log.Error("Console", sLConsole.Translations("NoFound2"));
					return;
				}

				if(Info.Length < 3)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NoChannelName"));
					return;
				}

				if(!IsChannel(Info[2]))
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NotaChannelHasBeenSet"));
					return;
				}

				var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM channel WHERE Channel = '{0}'", Info[2].ToLower());
				if(db.IsNull())
				{
					Log.Warning("Console", text[1]);
					return;
				}

				if(Info.Length < 4)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NoChannelLanguage"));
					return;
				}

				SchumixBase.DManager.Update("channel", string.Format("Language = '{0}'", Info[3]), string.Format("Channel = '{0}'", Info[2].ToLower()));
				Log.Notice("Console", text[0], Info[3]);
			}
		}

		/// <summary>
		///     Connect parancs függvénye.
		/// </summary>
		protected void HandleConnect()
		{
			_network.Connect();
		}

		/// <summary>
		///     Disconnect parancs függvénye.
		/// </summary>
		protected void HandleDisConnect()
		{
			sSender.Quit("Console: Disconnect.");
			_network.DisConnect();
		}

		/// <summary>
		///     Reconnect parancs függvénye.
		/// </summary>
		protected void HandleReConnect()
		{
			sSender.Quit("Console: Reconnect.");
			_network.ReConnect();
		}

		/// <summary>
		///     Nick parancs függvénye.
		/// </summary>
		protected void HandleNick()
		{
			if(Info.Length < 2)
			{
				Log.Error("Console", sLManager.GetConsoleWarningText("NoName"));
				return;
			}

			SchumixBase.NewNick = true;
			string nick = Info[1];
			sNickInfo.ChangeNick(nick);
			sSender.Nick(nick);
			Log.Notice("Console", sLManager.GetConsoleCommandText("nick"), nick);
		}

		/// <summary>
		///     Join parancs függvénye.
		/// </summary>
		protected void HandleJoin()
		{
			if(Info.Length < 2)
			{
				Log.Error("Console", sLManager.GetConsoleWarningText("NoChannelName"));
				return;
			}

			if(!IsChannel(Info[1]))
			{
				Log.Error("Console", sLManager.GetConsoleWarningText("NotaChannelHasBeenSet"));
				return;
			}

			if(Info.Length == 2)
				sSender.Join(Info[1]);
			else if(Info.Length == 3)
				sSender.Join(Info[1], Info[2]);

			Log.Notice("Console", sLManager.GetConsoleCommandText("join"), Info[1]);
		}

		/// <summary>
		///     Left parancs függvénye.
		/// </summary>
		protected void HandleLeave()
		{
			if(Info.Length < 2)
			{
				Log.Error("Console", sLManager.GetConsoleWarningText("NoChannelName"));
				return;
			}

			if(!IsChannel(Info[1]))
			{
				Log.Error("Console", sLManager.GetConsoleWarningText("NotaChannelHasBeenSet"));
				return;
			}

			sSender.Part(Info[1]);
			Log.Notice("Console", sLManager.GetConsoleCommandText("leave"), Info[1]);
		}

		/// <summary>
		///     Reload parancs függvénye.
		/// </summary>
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
				Log.Error("Console", sLConsole.Translations("NoFound2"));
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

		/// <summary>
		///     Quit parancs függvénye.
		/// </summary>
		protected void HandleQuit()
		{
			var text = sLManager.GetConsoleCommandTexts("quit");
			if(text.Length < 2)
			{
				Log.Error("Console", sLConsole.Translations("NoFound2"));
				return;
			}

			foreach(var plugin in sAddonManager.GetPlugins())
				plugin.Destroy();

			SchumixBase.ExitStatus = true;
			SchumixBase.timer.SaveUptime();
			SchumixBase.ServerDisconnect();
			Log.Notice("Console", text[0]);
			sSender.Quit(text[1]);
			Thread.Sleep(1000);
			Environment.Exit(1);
		}
	}
}