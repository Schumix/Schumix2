/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013 Schumix Team <http://schumix.eu/>
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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;
using Schumix.Api.Delegate;
using Schumix.Irc;
using Schumix.Irc.Util;
using Schumix.Irc.Ignore;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Addon;
using Schumix.Framework.Config;
using Schumix.Framework.Platforms;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Console.Commands
{
	/// <summary>
	///     CommandHandler class.
	/// </summary>
	partial class CommandHandler : ConsoleLog
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
		///     Utilities sokféle függvényt tartalmaz melyek hasznosak lehetnek.
		/// </summary>
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly Platform sPlatform = Singleton<Platform>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private readonly object Lock = new object();
		/// <summary>
		///     A szétdarabolt információkat tárolja.
		/// </summary>
		protected string[] Info;
		/// <summary>
		///     Csatorna nevét tárolja.
		/// </summary>
		protected string _channel;
		protected string _servername;

		/// <summary>
		///     Indulási függvény.
		/// </summary>
		protected CommandHandler() : base(LogConfig.IrcLog)
		{

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

			if(Info[1].ToLower() == SchumixBase.On)
			{
				Log.Notice("Console", text[0]);
				ChangeLog(true);
			}
			else if(Info[1].ToLower() == SchumixBase.Off)
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
			Log.Notice("Console", text[1], sPlatform.GetPlatform());
			Log.Notice("Console", text[2], Environment.OSVersion.ToString());
			Log.Notice("Console", text[3]);
			Log.Notice("Console", text[4], memory);
			Log.Notice("Console", text[5], Process.GetCurrentProcess().Threads.Count);
			Log.Notice("Console", text[6], SchumixBase.sTimer.Uptime());
		}

		/// <summary>
		///     Csatorna parancs függvénye.
		/// </summary>
		protected void HandleConsoleToChannel()
		{
			if(Info.Length < 2)
			{
				Log.Error("Console", sLManager.GetConsoleWarningText("NoChannelName"));
				return;
			}

			if(!Rfc2812Util.IsValidChannelName(Info[1]))
			{
				Log.Error("Console", sLManager.GetConsoleWarningText("NotaChannelHasBeenSet"));
				return;
			}

			if(_channel == Info[1].ToLower())
			{
				Log.Warning("Console", sLManager.GetConsoleWarningText("ChannelAlreadyBeenUsed"));
				return;
			}

			_channel = Info[1].ToLower();
			Log.Notice("Console", sLManager.GetConsoleCommandText("cchannel"), Info[1]);
			System.Console.Title = SchumixBase.Title + " || Console Writing Channel: " + _servername + SchumixBase.Colon + Info[1];
		}

		protected void HandleOldServerToNewServer()
		{
			if(Info.Length < 2)
			{
				Log.Error("Console", sLManager.GetConsoleWarningText("NoServerName"));
				return;
			}

			if(!sIrcBase.Networks.ContainsKey(Info[1].ToLower()))
			{
				Log.Error("Console", sLManager.GetConsoleWarningText("ThereIsNoSuchAServerName"));
				return;
			}

			if(_servername == Info[1].ToLower())
			{
				Log.Warning("Console", sLManager.GetConsoleWarningText("ServerAlreadyBeenUsed"));
				return;
			}

			_servername = Info[1].ToLower();
			Log.Notice("Console", sLManager.GetConsoleCommandText("cserver"), Info[1]);
			System.Console.Title = SchumixBase.Title + " || Console Writing Channel: " + Info[1] + SchumixBase.Colon + _channel;
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

				var db = SchumixBase.DManager.QueryFirstRow("SELECT Flag FROM admins WHERE Name = '{0}' And ServerName = '{1}'", Info[2].ToLower(), _servername);
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
				var db = SchumixBase.DManager.Query("SELECT Name FROM admins WHERE ServerName = '{0}'", _servername);
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
				if(!Rfc2812Util.IsValidNick(name))
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NotaNickNameHasBeenSet"));
					return;
				}

				var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM admins WHERE Name = '{0}' And ServerName = '{1}'", name.ToLower(), _servername);
				if(!db.IsNull())
				{
					Log.Warning("Console", text[0]);
					return;
				}

				string pass = sUtilities.GetRandomString();
				SchumixBase.DManager.Insert("`admins`(ServerId, ServerName, Name, Password)", IRCConfig.List[_servername].ServerId, _servername, name.ToLower(), sUtilities.Sha1(pass));

				if(SchumixBase.DManager.IsCreatedTable("hlmessage"))
					SchumixBase.DManager.Insert("`hlmessage`(ServerId, ServerName, Name, Enabled)", IRCConfig.List[_servername].ServerId, _servername, name.ToLower(), SchumixBase.Off);

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
				if(!Rfc2812Util.IsValidNick(name))
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NotaNickNameHasBeenSet"));
					return;
				}

				var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM admins WHERE Name = '{0}' And ServerName = '{1}'", name.ToLower(), _servername);
				if(db.IsNull())
				{
					Log.Warning("Console", text[0]);
					return;
				}

				SchumixBase.DManager.Delete("admins", string.Format("Name = '{0}' And ServerName = '{1}'", name.ToLower(), _servername));

				if(SchumixBase.DManager.IsCreatedTable("hlmessage"))
					SchumixBase.DManager.Delete("hlmessage", string.Format("Name = '{0}' And ServerName = '{1}'", name.ToLower(), _servername));

				if(SchumixBase.DManager.IsCreatedTable("birthday"))
				{
					var db1 = SchumixBase.DManager.QueryFirstRow("SELECT * FROM birthday WHERE Name = '{0}' And ServerName = '{1}'", name.ToLower(), _servername);
					if(!db1.IsNull())
						SchumixBase.DManager.Delete("birthday", string.Format("Name = '{0}' And ServerName = '{1}'", name.ToLower(), _servername));
				}

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
				if(!Rfc2812Util.IsValidNick(name))
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NotaNickNameHasBeenSet"));
					return;
				}

				int rank = Convert.ToInt32(Info[3]);

				if((AdminFlag)rank == AdminFlag.Administrator || (AdminFlag)rank == AdminFlag.Operator || (AdminFlag)rank == AdminFlag.HalfOperator)
				{
					SchumixBase.DManager.Update("admins", string.Format("Flag = '{0}'", rank), string.Format("Name = '{0}' And ServerName = '{1}'", name, _servername));
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

				if(!Rfc2812Util.IsValidChannelName(Info[2]))
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NotaChannelHasBeenSet"));
					return;
				}

				var db0 = SchumixBase.DManager.QueryFirstRow("SELECT* FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", Info[2].ToLower(), _servername);
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

					string[] ChannelInfo = sIrcBase.Networks[_servername].sMyChannelInfo.ChannelFunctionsInfo(channel).Split('|');
					if(ChannelInfo.Length < 2)
						return;

					Log.Notice("Console", text2[0], ChannelInfo[0]);
					Log.Notice("Console", text2[1], ChannelInfo[1]);
				}
				else if(status == SchumixBase.On || status == SchumixBase.Off)
				{
					if(Info.Length < 5)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoFunctionName"));
						return;
					}

					if(Info.Length >= 6)
					{
						string args = string.Empty;
						string onfunction = string.Empty;
						string offfunction = string.Empty;
						string nosuchfunction = string.Empty;

						for(int i = 4; i < Info.Length; i++)
						{
							if(!sIrcBase.Networks[_servername].sMyChannelInfo.SearchChannelFunction(Info[i]))
							{
								nosuchfunction += ", " + Info[i].ToLower();
								continue;
							}

							if(sIrcBase.Networks[_servername].sMyChannelInfo.FSelect(Info[i], channel) && status == SchumixBase.On)
							{
								onfunction += ", " + Info[i].ToLower();
								continue;
							}
							else if(!sIrcBase.Networks[_servername].sMyChannelInfo.FSelect(Info[i], channel) && status == SchumixBase.Off)
							{
								offfunction += ", " + Info[i].ToLower();
								continue;
							}

							if(sIrcBase.Networks[_servername].sMyChannelInfo.SearchFunction(Info[i]))
							{
								if(!sIrcBase.Networks[_servername].sMyChannelInfo.FSelect(Info[i]) && status == SchumixBase.On)
								{
									SchumixBase.DManager.Update("schumix", "FunctionStatus = 'on'", string.Format("FunctionName = '{0}' And ServerName = '{1}'", Info[i].ToLower(), _servername));
									sIrcBase.Networks[_servername].sMyChannelInfo.FunctionsReload();
								}
							}

							args += ", " + Info[i].ToLower();
							SchumixBase.DManager.Update("channels", string.Format("Functions = '{0}'", sIrcBase.Networks[_servername].sMyChannelInfo.ChannelFunctions(Info[i].ToLower(), status, channel)), string.Format("Channel = '{0}' And ServerName = '{1}'", channel, _servername));
							sIrcBase.Networks[_servername].sMyChannelInfo.ChannelFunctionsReload();
						}

						if(!onfunction.IsNullOrEmpty())
							Log.Warning("Console", sLManager.GetConsoleWarningText("FunctionAlreadyTurnedOn2"), onfunction.Remove(0, 2, ", "));
			
						if(!offfunction.IsNullOrEmpty())
							Log.Warning("Console", sLManager.GetConsoleWarningText("FunctionAlreadyTurnedOff2"), offfunction.Remove(0, 2, ", "));

						if(!nosuchfunction.IsNullOrEmpty())
							Log.Error("Console", sLConsole.Other("NoSuchFunctions2"), nosuchfunction.Remove(0, 2, ", "));

						if(args.Length == 0)
							return;

						if(status == SchumixBase.On)
							Log.Notice("Console", text[0],  args.Remove(0, 2, ", "));
						else
							Log.Notice("Console", text[1],  args.Remove(0, 2, ", "));
					}
					else
					{
						if(!sIrcBase.Networks[_servername].sMyChannelInfo.SearchChannelFunction(Info[4]))
						{
							Log.Error("Console", sLConsole.Other("NoSuchFunctions"));
							return;
						}

						if(sIrcBase.Networks[_servername].sMyChannelInfo.FSelect(Info[4], channel) && status == SchumixBase.On)
						{
							Log.Warning("Console", sLManager.GetConsoleWarningText("FunctionAlreadyTurnedOn"));
							return;
						}
						else if(!sIrcBase.Networks[_servername].sMyChannelInfo.FSelect(Info[4], channel) && status == SchumixBase.Off)
						{
							Log.Warning("Console", sLManager.GetConsoleWarningText("FunctionAlreadyTurnedOff"));
							return;
						}

						if(sIrcBase.Networks[_servername].sMyChannelInfo.SearchFunction(Info[4]))
						{
							if(!sIrcBase.Networks[_servername].sMyChannelInfo.FSelect(Info[4]) && status == SchumixBase.On)
							{
								SchumixBase.DManager.Update("schumix", "FunctionStatus = 'on'", string.Format("FunctionName = '{0}' And ServerName = '{1}'", Info[4].ToLower(), _servername));
								sIrcBase.Networks[_servername].sMyChannelInfo.FunctionsReload();
							}
						}

						if(status == SchumixBase.On)
							Log.Notice("Console", text[0], Info[4].ToLower());
						else
							Log.Notice("Console", text[1], Info[4].ToLower());

						SchumixBase.DManager.Update("channels", string.Format("Functions = '{0}'", sIrcBase.Networks[_servername].sMyChannelInfo.ChannelFunctions(Info[4].ToLower(), status, channel)), string.Format("Channel = '{0}' And ServerName = '{1}'", channel, _servername));
						sIrcBase.Networks[_servername].sMyChannelInfo.ChannelFunctionsReload();
					}
				}
				else
					Log.Error("Console", sLManager.GetConsoleWarningText("WrongSwitch"));
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
					var db = SchumixBase.DManager.Query("SELECT Channel FROM channels WHERE ServerName = '{0}'", _servername);
					if(!db.IsNull())
					{
						foreach(DataRow row in db.Rows)
						{
							string channel = row["Channel"].ToString();
							SchumixBase.DManager.Update("channels", string.Format("Functions = '{0}'", sUtilities.GetFunctionUpdate()), string.Format("Channel = '{0}' And ServerName = '{1}'", channel, _servername));
						}

						sIrcBase.Networks[_servername].sMyChannelInfo.ChannelFunctionsReload();
						Log.Notice("Console", sLManager.GetConsoleCommandText("function/update/all"));
					}
					else
						Log.Error("Console", sLManager.GetConsoleWarningText("FaultyQuery"));
				}
				else
				{
					if(!Rfc2812Util.IsValidChannelName(Info[2]))
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NotaChannelHasBeenSet"));
						return;
					}

					Log.Notice("Console", sLManager.GetConsoleCommandText("function/update"), Info[2].ToLower());
					SchumixBase.DManager.Update("channels", string.Format("Functions = '{0}'", sUtilities.GetFunctionUpdate()), string.Format("Channel = '{0}' And ServerName = '{1}'", Info[2].ToLower(), _servername));
					sIrcBase.Networks[_servername].sMyChannelInfo.ChannelFunctionsReload();
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

				string f = sIrcBase.Networks[_servername].sMyChannelInfo.FunctionsInfo();
				if(f.IsNullOrEmpty())
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

				if(Info[1].ToLower() == SchumixBase.On || Info[1].ToLower() == SchumixBase.Off)
				{
					if(Info.Length >= 4)
					{
						string args = string.Empty;
						string onfunction = string.Empty;
						string offfunction = string.Empty;
						string nosuchfunction = string.Empty;

						for(int i = 2; i < Info.Length; i++)
						{
							if(!sIrcBase.Networks[_servername].sMyChannelInfo.SearchFunction(Info[i]))
							{
								nosuchfunction += ", " + Info[i].ToLower();
								continue;
							}

							if(sIrcBase.Networks[_servername].sMyChannelInfo.FSelect(Info[i]) && Info[1].ToLower() == SchumixBase.On)
							{
								onfunction += ", " + Info[i].ToLower();
								continue;
							}
							else if(!sIrcBase.Networks[_servername].sMyChannelInfo.FSelect(Info[i]) && Info[1].ToLower() == SchumixBase.Off)
							{
								offfunction += ", " + Info[i].ToLower();
								continue;
							}

							args += ", " + Info[i].ToLower();
							SchumixBase.DManager.Update("schumix", string.Format("FunctionStatus = '{0}'", Info[1].ToLower()), string.Format("FunctionName = '{0}' And ServerName = '{1}'", Info[i].ToLower(), _servername));
							sIrcBase.Networks[_servername].sMyChannelInfo.FunctionsReload();
						}

						if(!onfunction.IsNullOrEmpty())
							Log.Warning("Console", sLManager.GetConsoleWarningText("FunctionAlreadyTurnedOn2"), onfunction.Remove(0, 2, ", "));
			
						if(!offfunction.IsNullOrEmpty())
							Log.Warning("Console", sLManager.GetConsoleWarningText("FunctionAlreadyTurnedOff2"), offfunction.Remove(0, 2, ", "));

						if(!nosuchfunction.IsNullOrEmpty())
							Log.Error("Console", sLConsole.Other("NoSuchFunctions2"), nosuchfunction.Remove(0, 2, ", "));

						if(args.Length == 0)
							return;

						if(Info[1].ToLower() == SchumixBase.On)
							Log.Notice("Console", text[0],  args.Remove(0, 2, ", "));
						else
							Log.Notice("Console", text[1],  args.Remove(0, 2, ", "));
					}
					else
					{
						if(!sIrcBase.Networks[_servername].sMyChannelInfo.SearchFunction(Info[2]))
						{
							Log.Error("Console", sLConsole.Other("NoSuchFunctions"));
							return;
						}

						if(sIrcBase.Networks[_servername].sMyChannelInfo.FSelect(Info[2]) && Info[1].ToLower() == SchumixBase.On)
						{
							Log.Warning("Console", sLManager.GetConsoleWarningText("FunctionAlreadyTurnedOn"));
							return;
						}
						else if(!sIrcBase.Networks[_servername].sMyChannelInfo.FSelect(Info[2]) && Info[1].ToLower() == SchumixBase.Off)
						{
							Log.Warning("Console", sLManager.GetConsoleWarningText("FunctionAlreadyTurnedOff"));
							return;
						}

						if(Info[1].ToLower() == SchumixBase.On)
							Log.Notice("Console", text[0], Info[2].ToLower());
						else
							Log.Notice("Console", text[1], Info[2].ToLower());

						SchumixBase.DManager.Update("schumix", string.Format("FunctionStatus = '{0}'", Info[1].ToLower()), string.Format("FunctionName = '{0}' And ServerName = '{1}'", Info[2].ToLower(), _servername));
						sIrcBase.Networks[_servername].sMyChannelInfo.FunctionsReload();
					}
				}
				else
					Log.Error("Console", sLManager.GetConsoleWarningText("WrongSwitch"));
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

				if(!Rfc2812Util.IsValidChannelName(channel))
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NotaChannelHasBeenSet"));
					return;
				}

				if(sIrcBase.Networks[_servername].sChannelList.List.ContainsKey(Info[1].ToLower()))
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("ImAlreadyOnThisChannel"));
					return;
				}

				var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", channel, _servername);
				if(!db.IsNull())
				{
					Log.Warning("Console", text[0]);
					return;
				}

				if(Info.Length == 4)
				{
					string pass = Info[3];
					sIrcBase.Networks[_servername].sSender.Join(channel, pass);
					SchumixBase.DManager.Insert("`channels`(ServerId, ServerName, Channel, Password, Language)", IRCConfig.List[_servername].ServerId, _servername, channel, pass, sLManager.Locale);
					SchumixBase.DManager.Update("channels", "Enabled = 'true'", string.Format("Channel = '{0}' And ServerName = '{1}'", channel, _servername));
				}
				else
				{
					sIrcBase.Networks[_servername].sSender.Join(channel);
					SchumixBase.DManager.Insert("`channels`(ServerId, ServerName, Channel, Password, Language)", IRCConfig.List[_servername].ServerId, _servername, channel, string.Empty, sLManager.Locale);
					SchumixBase.DManager.Update("channels", "Enabled = 'true'", string.Format("Channel = '{0}' And ServerName = '{1}'", channel, _servername));
				}

				Log.Notice("Console", text[1], channel);
				sIrcBase.Networks[_servername].sMyChannelInfo.ChannelListReload();
				sIrcBase.Networks[_servername].sMyChannelInfo.ChannelFunctionsReload();
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

				if(!Rfc2812Util.IsValidChannelName(channel))
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NotaChannelHasBeenSet"));
					return;
				}

				if(channel == IRCConfig.List[_servername].MasterChannel.ToLower())
				{
					Log.Warning("Console", text[0]);
					return;
				}

				var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", channel, _servername);
				if(db.IsNull())
				{
					Log.Warning("Console", text[1]);
					return;
				}

				sIrcBase.Networks[_servername].sSender.Part(channel);
				SchumixBase.DManager.Delete("channels", string.Format("Channel = '{0}' And ServerName = '{1}'", channel, _servername));
				Log.Notice("Console", text[2], channel);

				sIrcBase.Networks[_servername].sMyChannelInfo.ChannelListReload();
				sIrcBase.Networks[_servername].sMyChannelInfo.ChannelFunctionsReload();
			}
			else if(Info[1].ToLower() == "update")
			{
				sIrcBase.Networks[_servername].sMyChannelInfo.ChannelListReload();
				sIrcBase.Networks[_servername].sMyChannelInfo.ChannelFunctionsReload();
				Log.Notice("Console", sLManager.GetConsoleCommandText("channel/update"));
			}
			else if(Info[1].ToLower() == "info")
			{
				var text = sLManager.GetConsoleCommandTexts("channel/info");
				if(text.Length < 6)
				{
					Log.Error("Console", sLConsole.Translations("NoFound2"));
					return;
				}

				var db = SchumixBase.DManager.Query("SELECT Channel, Enabled, Hidden, Error FROM channels WHERE ServerName = '{0}'", _servername);
				if(!db.IsNull())
				{
					string ActiveChannels = string.Empty, InActiveChannels = string.Empty, HiddenChannels = string.Empty;

					foreach(DataRow row in db.Rows)
					{
						string channel = row["Channel"].ToString();
						bool enabled = Convert.ToBoolean(row["Enabled"].ToString());
						bool hidden = Convert.ToBoolean(row["Hidden"].ToString());
						
						if(enabled && !hidden)
							ActiveChannels += ", " + channel;
						else if(!enabled && !hidden)
							InActiveChannels += ", " + channel + SchumixBase.Colon + row["Error"].ToString();
						
						if(hidden)
							HiddenChannels += ", " + channel;
					}

					if(ActiveChannels.Length > 0)
						Log.Notice("Console", text[0], ActiveChannels.Remove(0, 2, ", "));
					else
						Log.Notice("Console", text[1]);

					if(InActiveChannels.Length > 0)
						Log.Notice("Console", text[2], InActiveChannels.Remove(0, 2, ", "));
					else
						Log.Notice("Console", text[3]);

					if(HiddenChannels.Length > 0)
						Log.Notice("Console", text[4], HiddenChannels.Remove(0, 2, ", "));
					else
						Log.Notice("Console", text[5]);
				}
				else
					Log.Error("Console", sLManager.GetConsoleWarningText("FaultyQuery"));
			}
			else if(Info[1].ToLower() == "language")
			{
				var text = sLManager.GetConsoleCommandTexts("channel/language");
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

				if(!Rfc2812Util.IsValidChannelName(Info[2]))
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NotaChannelHasBeenSet"));
					return;
				}

				var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", Info[2].ToLower(), _servername);
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

				db = SchumixBase.DManager.QueryFirstRow("SELECT Language FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", Info[2].ToLower(), _servername);
				if(!db.IsNull())
				{
					if(db["Language"].ToString().ToLower() == Info[3].ToLower())
					{
						Log.Warning("Console", text[2], Info[3]);
						return;
					}
				}

				SchumixBase.DManager.Update("channels", string.Format("Language = '{0}'", Info[3]), string.Format("Channel = '{0}' And ServerName = '{1}'", Info[2].ToLower(), _servername));
				Log.Notice("Console", text[0], Info[3]);
				SchumixBase.sCacheDB.ReLoad("channels");
			}
			else if(Info[1].ToLower() == "password")
			{
				if(Info.Length < 3)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NoValue"));
					return;
				}

				if(Info[2].ToLower() == "add")
				{
					var text = sLManager.GetConsoleCommandTexts("channel/password/add");
					if(text.Length < 3)
					{
						Log.Error("Console", sLConsole.Translations("NoFound2"));
						return;
					}

					if(Info.Length < 4)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoChannelName"));
						return;
					}
	
					if(!Rfc2812Util.IsValidChannelName(Info[3]))
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NotaChannelHasBeenSet"));
						return;
					}

					if(Info.Length < 5)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoPassword"));
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", Info[3].ToLower(), _servername);
					if(db.IsNull())
					{
						Log.Warning("Console", text[0]);
						return;
					}

					db = SchumixBase.DManager.QueryFirstRow("SELECT Password FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", Info[3].ToLower(), _servername);
					if(!db.IsNull())
					{
						if(!db["Password"].ToString().IsNullOrEmpty())
						{
							Log.Notice("Console", text[1]);
							return;
						}
					}

					SchumixBase.DManager.Update("channels", string.Format("Password = '{0}'", Info[4]), string.Format("Channel = '{0}' And ServerName = '{1}'", Info[3].ToLower(), _servername));
					Log.Notice("Console", text[2], Info[3]);
					SchumixBase.sCacheDB.ReLoad("channels");
				}
				else if(Info[2].ToLower() == "remove")
				{
					var text = sLManager.GetConsoleCommandTexts("channel/password/remove");
					if(text.Length < 3)
					{
						Log.Error("Console", sLConsole.Translations("NoFound2"));
						return;
					}

					if(Info.Length < 4)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoChannelName"));
						return;
					}
	
					if(!Rfc2812Util.IsValidChannelName(Info[3]))
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NotaChannelHasBeenSet"));
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", Info[3].ToLower(), _servername);
					if(db.IsNull())
					{
						Log.Warning("Console", text[0]);
						return;
					}

					db = SchumixBase.DManager.QueryFirstRow("SELECT Password FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", Info[3].ToLower(), _servername);
					if(!db.IsNull())
					{
						if(db["Password"].ToString().IsNullOrEmpty())
						{
							Log.Notice("Console", text[1]);
							return;
						}
					}

					SchumixBase.DManager.Update("channels", "Password = ''", string.Format("Channel = '{0}' And ServerName = '{1}'", Info[3].ToLower(), _servername));
					Log.Notice("Console", text[2]);
					SchumixBase.sCacheDB.ReLoad("channels");
				}
				else if(Info[2].ToLower() == "update")
				{
					var text = sLManager.GetConsoleCommandTexts("channel/password/update");
					if(text.Length < 3)
					{
						Log.Error("Console", sLConsole.Translations("NoFound2"));
						return;
					}

					if(Info.Length < 4)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoChannelName"));
						return;
					}
	
					if(!Rfc2812Util.IsValidChannelName(Info[3]))
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NotaChannelHasBeenSet"));
						return;
					}

					if(Info.Length < 5)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoPassword"));
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", Info[3].ToLower(), _servername);
					if(db.IsNull())
					{
						Log.Warning("Console", text[0]);
						return;
					}

					db = SchumixBase.DManager.QueryFirstRow("SELECT Password FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", Info[3].ToLower(), _servername);
					if(!db.IsNull())
					{
						if(db["Password"].ToString().IsNullOrEmpty())
						{
							Log.Notice("Console", text[1]);
							return;
						}
					}

					SchumixBase.DManager.Update("channels", string.Format("Password = '{0}'", Info[4]), string.Format("Channel = '{0}' And ServerName = '{1}'", Info[3].ToLower(), _servername));
					Log.Notice("Console", text[2], Info[4]);
					SchumixBase.sCacheDB.ReLoad("channels");
				}
				else if(Info[2].ToLower() == "info")
				{
					var text = sLManager.GetConsoleCommandTexts("channel/password/info");
					if(text.Length < 3)
					{
						Log.Error("Console", sLConsole.Translations("NoFound2"));
						return;
					}

					if(Info.Length < 4)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoChannelName"));
						return;
					}
	
					if(!Rfc2812Util.IsValidChannelName(Info[3]))
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NotaChannelHasBeenSet"));
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", Info[3].ToLower(), _servername);
					if(db.IsNull())
					{
						Log.Warning("Console", text[0]);
						return;
					}

					db = SchumixBase.DManager.QueryFirstRow("SELECT Password FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", Info[3].ToLower(), _servername);
					if(!db.IsNull())
					{
						if(db["Password"].ToString().IsNullOrEmpty())
							Log.Notice("Console", text[1]);
						else
							Log.Notice("Console", text[2]);
					}
				}
			}
		}

		/// <summary>
		///     Connect parancs függvénye.
		/// </summary>
		protected void HandleConnect()
		{
			sIrcBase.Networks[_servername].Connect();
		}

		/// <summary>
		///     Disconnect parancs függvénye.
		/// </summary>
		protected void HandleDisConnect()
		{
			sIrcBase.Networks[_servername].sSender.Quit("Console: Disconnect.");
			sIrcBase.Networks[_servername].DisConnect();
		}

		/// <summary>
		///     Reconnect parancs függvénye.
		/// </summary>
		protected void HandleReConnect()
		{
			sIrcBase.Networks[_servername].sSender.Quit("Console: Reconnect.");
			sIrcBase.Networks[_servername].ReConnect();
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

			if(!Rfc2812Util.IsValidNick(nick))
			{
				Log.Error("Console", sLManager.GetConsoleWarningText("NotaNickNameHasBeenSet"));
				return;
			}

			sIrcBase.Networks[_servername].sMyNickInfo.ChangeNick(nick);
			sIrcBase.Networks[_servername].sSender.Nick(nick);
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

			if(!Rfc2812Util.IsValidChannelName(Info[1]))
			{
				Log.Error("Console", sLManager.GetConsoleWarningText("NotaChannelHasBeenSet"));
				return;
			}

			if(sIrcBase.Networks[_servername].sChannelList.List.ContainsKey(Info[1].ToLower()))
			{
				Log.Error("Console", sLManager.GetConsoleWarningText("ImAlreadyOnThisChannel"));
				return;
			}

			if(sIrcBase.Networks[_servername].sIgnoreChannel.IsIgnore(Info[1].ToLower()))
			{
				Log.Error("Console", sLManager.GetConsoleWarningText("ThisChannelBlockedByAdmin"));
				return;
			}

			if(Info.Length == 2)
				sIrcBase.Networks[_servername].sSender.Join(Info[1]);
			else if(Info.Length == 3)
				sIrcBase.Networks[_servername].sSender.Join(Info[1], Info[2]);

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

			if(!Rfc2812Util.IsValidChannelName(Info[1]))
			{
				Log.Error("Console", sLManager.GetConsoleWarningText("NotaChannelHasBeenSet"));
				return;
			}

			if(!sIrcBase.Networks[_servername].sChannelList.List.ContainsKey(Info[1].ToLower()))
			{
				Log.Error("Console", sLManager.GetConsoleWarningText("ImNotOnThisChannel"));
				return;
			}

			sIrcBase.Networks[_servername].sSender.Part(Info[1]);
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

			int i = -1;

			switch(Info[1].ToLower())
			{
			case "config":
				new Config(SchumixConfig.ConfigDirectory, SchumixConfig.ConfigFile, SchumixConfig.ColorBindMode);
				sIrcBase.Networks[_servername].sIgnoreAddon.RemoveConfig();
				sIrcBase.Networks[_servername].sIgnoreAddon.LoadConfig();
				sIrcBase.Networks[_servername].sIgnoreChannel.RemoveConfig();
				sIrcBase.Networks[_servername].sIgnoreChannel.LoadConfig();
				sIrcBase.Networks[_servername].sIgnoreNickName.RemoveConfig();
				sIrcBase.Networks[_servername].sIgnoreNickName.LoadConfig();
				sIrcBase.Networks[_servername].ReloadMessageHandlerConfig();
				sLConsole.SetLocale(LocalizationConfig.Locale);
				sIrcBase.Networks[_servername].sCtcpSender.ClientInfoResponse = sLConsole.GetString("This client supports: UserInfo, Finger, Version, Source, Ping, Time and ClientInfo");
				i = 1;
				break;
			case "cachedb":
				SchumixBase.sCacheDB.ReLoad();
				i = 1;
				break;
			}

			foreach(var plugin in sAddonManager.Addons[_servername].Addons)
			{
				if(plugin.Value.Reload(Info[1].ToLower()) == 1)
					i = 1;
				else if(plugin.Value.Reload(Info[1].ToLower()) == 0)
					i = 0;
			}

			if(i == -1)
				Log.Error("Console", text[0]);
			else if(i == 0)
				Log.Error("Console", text[1]);
			else if(i == 1)
				Log.Notice("Console", text[2], Info[1]);
		}

		/// <summary>
		///     Ignore parancs függvénye.
		/// </summary>
		protected void HandleIgnore()
		{
			if(Info.Length < 2)
			{
				Log.Error("Console", sLManager.GetConsoleWarningText("NoValue"));
				return;
			}

			if(Info[1].ToLower() == "irc")
			{
				if(Info.Length < 3)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("No1Value"));
					return;
				}

				if(Info[2].ToLower() == "command")
				{
					if(Info.Length < 4)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("No1Value"));
						return;
					}

					if(Info[3].ToLower() == "add")
					{
						var text = sLManager.GetConsoleCommandTexts("ignore/irc/command/add");
						if(text.Length < 2)
						{
							Log.Error("Console", sLConsole.Translations("NoFound2"));
							return;
						}

						if(Info.Length < 5)
						{
							Log.Error("Console", sLManager.GetConsoleWarningText("NoCommand"));
							return;
						}

						string command = Info[4].ToLower();

						if(sIrcBase.Networks[_servername].sIgnoreIrcCommand.IsIgnore(command))
						{
							Log.Error("Console", text[0]);
							return;
						}

						sIrcBase.Networks[_servername].sIgnoreIrcCommand.Add(command);
						Log.Notice("Console", text[1]);
					}
					else if(Info[3].ToLower() == "remove")
					{
						var text = sLManager.GetConsoleCommandTexts("ignore/irc/command/remove");
						if(text.Length < 2)
						{
							Log.Error("Console", sLConsole.Translations("NoFound2"));
							return;
						}

						if(Info.Length < 5)
						{
							Log.Error("Console", sLManager.GetConsoleWarningText("NoCommand"));
							return;
						}

						string command = Info[4].ToLower();

						if(!sIrcBase.Networks[_servername].sIgnoreIrcCommand.IsIgnore(command))
						{
							Log.Error("Console", text[0]);
							return;
						}

						sIrcBase.Networks[_servername].sIgnoreIrcCommand.Remove(command);
						Log.Notice("Console", text[1]);
					}
					else if(Info[3].ToLower() == "search")
					{
						var text = sLManager.GetConsoleCommandTexts("ignore/irc/command/search");
						if(text.Length < 2)
						{
							Log.Error("Console", sLConsole.Translations("NoFound2"));
							return;
						}

						if(Info.Length < 5)
						{
							Log.Error("Console", sLManager.GetConsoleWarningText("NoCommand"));
							return;
						}

						if(sIrcBase.Networks[_servername].sIgnoreIrcCommand.Contains(Info[4].ToLower()))
							Log.Notice("Console", text[0]);
						else
							Log.Error("Console", text[1]);
					}
				}
			}
			else if(Info[1].ToLower() == "command")
			{
				if(Info.Length < 3)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("No1Value"));
					return;
				}

				if(Info[2].ToLower() == "add")
				{
					var text = sLManager.GetConsoleCommandTexts("ignore/command/add");
					if(text.Length < 2)
					{
						Log.Error("Console", sLConsole.Translations("NoFound2"));
						return;
					}

					if(Info.Length < 4)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoCommand"));
						return;
					}

					string command = Info[3].ToLower();

					if(command == "ignore" || command == "admin")
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoIgnoreCommand"));
						return;
					}

					if(sIrcBase.Networks[_servername].sIgnoreCommand.IsIgnore(command))
					{
						Log.Error("Console", text[0]);
						return;
					}

					sIrcBase.Networks[_servername].sIgnoreCommand.Add(command);
					Log.Notice("Console", text[1]);
				}
				else if(Info[2].ToLower() == "remove")
				{
					var text = sLManager.GetConsoleCommandTexts("ignore/command/remove");
					if(text.Length < 2)
					{
						Log.Error("Console", sLConsole.Translations("NoFound2"));
						return;
					}

					if(Info.Length < 4)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoCommand"));
						return;
					}

					string command = Info[3].ToLower();

					if(!sIrcBase.Networks[_servername].sIgnoreCommand.IsIgnore(command))
					{
						Log.Error("Console", text[0]);
						return;
					}

					sIrcBase.Networks[_servername].sIgnoreCommand.Remove(command);
					Log.Notice("Console", text[1]);
				}
				else if(Info[2].ToLower() == "search")
				{
					var text = sLManager.GetConsoleCommandTexts("ignore/command/search");
					if(text.Length < 2)
					{
						Log.Error("Console", sLConsole.Translations("NoFound2"));
						return;
					}

					if(Info.Length < 4)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoCommand"));
						return;
					}

					if(sIrcBase.Networks[_servername].sIgnoreCommand.Contains(Info[3].ToLower()))
						Log.Notice("Console", text[0]);
					else
						Log.Error("Console", text[1]);
				}
			}
			else if(Info[1].ToLower() == "channel")
			{
				if(Info.Length < 3)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("No1Value"));
					return;
				}

				if(Info[2].ToLower() == "add")
				{
					var text = sLManager.GetConsoleCommandTexts("ignore/channel/add");
					if(text.Length < 2)
					{
						Log.Error("Console", sLConsole.Translations("NoFound2"));
						return;
					}

					if(Info.Length < 4)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoChannelName"));
						return;
					}

					string channel = Info[3].ToLower();

					if(!Rfc2812Util.IsValidChannelName(channel))
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NotaChannelHasBeenSet"));
						return;
					}

					if(channel == IRCConfig.List[_servername].MasterChannel.ToLower())
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoIgnoreMasterChannel"));
						return;
					}

					if(sIrcBase.Networks[_servername].sIgnoreChannel.IsIgnore(channel))
					{
						Log.Error("Console", text[0]);
						return;
					}

					sIrcBase.Networks[_servername].sIgnoreChannel.Add(channel);
					Log.Notice("Console", text[1]);
				}
				else if(Info[2].ToLower() == "remove")
				{
					var text = sLManager.GetConsoleCommandTexts("ignore/channel/remove");
					if(text.Length < 2)
					{
						Log.Error("Console", sLConsole.Translations("NoFound2"));
						return;
					}

					if(Info.Length < 4)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoChannelName"));
						return;
					}

					string channel = Info[3].ToLower();

					if(!Rfc2812Util.IsValidChannelName(channel))
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NotaChannelHasBeenSet"));
						return;
					}

					if(!sIrcBase.Networks[_servername].sIgnoreChannel.IsIgnore(channel))
					{
						Log.Error("Console", text[0]);
						return;
					}

					sIrcBase.Networks[_servername].sIgnoreChannel.Remove(channel);
					Log.Notice("Console", text[1]);
				}
				else if(Info[2].ToLower() == "search")
				{
					var text = sLManager.GetConsoleCommandTexts("ignore/channel/search");
					if(text.Length < 2)
					{
						Log.Error("Console", sLConsole.Translations("NoFound2"));
						return;
					}

					if(Info.Length < 4)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoChannelName"));
						return;
					}

					string channel = Info[3].ToLower();

					if(!Rfc2812Util.IsValidChannelName(channel))
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NotaChannelHasBeenSet"));
						return;
					}

					if(sIrcBase.Networks[_servername].sIgnoreChannel.Contains(channel))
						Log.Notice("Console", text[0]);
					else
						Log.Error("Console", text[1]);
				}
			}
			else if(Info[1].ToLower() == "nick")
			{
				if(Info.Length < 3)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("No1Value"));
					return;
				}

				if(Info[2].ToLower() == "add")
				{
					var text = sLManager.GetConsoleCommandTexts("ignore/nick/add");
					if(text.Length < 2)
					{
						Log.Error("Console", sLConsole.Translations("NoFound2"));
						return;
					}

					if(Info.Length < 4)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoName"));
						return;
					}

					string nick = Info[3].ToLower();

					if(!Rfc2812Util.IsValidNick(nick))
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NotaNickNameHasBeenSet"));
						return;
					}

					if(sIrcBase.Networks[_servername].sIgnoreNickName.IsIgnore(nick))
					{
						Log.Error("Console", text[0]);
						return;
					}

					sIrcBase.Networks[_servername].sIgnoreNickName.Add(nick);
					Log.Notice("Console", text[1]);
				}
				else if(Info[2].ToLower() == "remove")
				{
					var text = sLManager.GetConsoleCommandTexts("ignore/nick/remove");
					if(text.Length < 2)
					{
						Log.Error("Console", sLConsole.Translations("NoFound2"));
						return;
					}

					if(Info.Length < 4)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoName"));
						return;
					}

					string nick = Info[3].ToLower();

					if(!Rfc2812Util.IsValidNick(nick))
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NotaNickNameHasBeenSet"));
						return;
					}

					if(!sIrcBase.Networks[_servername].sIgnoreNickName.IsIgnore(nick))
					{
						Log.Error("Console", text[0]);
						return;
					}

					sIrcBase.Networks[_servername].sIgnoreNickName.Remove(nick);
					Log.Notice("Console", text[1]);
				}
				else if(Info[2].ToLower() == "search")
				{
					var text = sLManager.GetConsoleCommandTexts("ignore/nick/search");
					if(text.Length < 2)
					{
						Log.Error("Console", sLConsole.Translations("NoFound2"));
						return;
					}

					if(Info.Length < 4)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoName"));
						return;
					}

					string nick = Info[3].ToLower();
					
					if(!Rfc2812Util.IsValidNick(nick))
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NotaNickNameHasBeenSet"));
						return;
					}

					if(sIrcBase.Networks[_servername].sIgnoreNickName.Contains(nick))
						Log.Notice("Console", text[0]);
					else
						Log.Error("Console", text[1]);
				}
			}
			else if(Info[1].ToLower() == "addon")
			{
				if(Info.Length < 3)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("No1Value"));
					return;
				}

				if(Info[2].ToLower() == "add")
				{
					var text = sLManager.GetConsoleCommandTexts("ignore/addon/add");
					if(text.Length < 2)
					{
						Log.Error("Console", sLConsole.Translations("NoFound2"));
						return;
					}

					if(Info.Length < 4)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoName"));
						return;
					}

					string addon = Info[3].ToLower();

					if(!sAddonManager.IsAddon(_servername, addon))
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("ThereIsNoSuchAnAddon"));
						return;
					}

					if(sIrcBase.Networks[_servername].sIgnoreAddon.IsIgnore(addon))
					{
						Log.Error("Console", text[0]);
						return;
					}

					sIrcBase.Networks[_servername].sIgnoreAddon.Add(addon);
					sIrcBase.Networks[_servername].sIgnoreAddon.UnloadPlugin(addon);
					Log.Notice("Console", text[1]);
				}
				else if(Info[2].ToLower() == "remove")
				{
					var text = sLManager.GetConsoleCommandTexts("ignore/addon/remove");
					if(text.Length < 2)
					{
						Log.Error("Console", sLConsole.Translations("NoFound2"));
						return;
					}

					if(Info.Length < 4)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoName"));
						return;
					}

					string addon = Info[3].ToLower();

					if(!sAddonManager.IsAddon(_servername, addon))
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("ThereIsNoSuchAnAddon"));
						return;
					}

					if(!sIrcBase.Networks[_servername].sIgnoreAddon.IsIgnore(addon))
					{
						Log.Error("Console", text[0]);
						return;
					}

					sIrcBase.Networks[_servername].sIgnoreAddon.Remove(addon);
					sIrcBase.Networks[_servername].sIgnoreAddon.LoadPlugin(addon);
					Log.Notice("Console", text[1]);
				}
				else if(Info[2].ToLower() == "search")
				{
					var text = sLManager.GetConsoleCommandTexts("ignore/addon/search");
					if(text.Length < 2)
					{
						Log.Error("Console", sLConsole.Translations("NoFound2"));
						return;
					}

					if(Info.Length < 4)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoName"));
						return;
					}

					string addon = Info[3].ToLower();

					if(!sAddonManager.IsAddon(_servername, addon))
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("ThereIsNoSuchAnAddon"));
						return;
					}

					if(sIrcBase.Networks[_servername].sIgnoreAddon.Contains(addon))
						Log.Notice("Console", text[0]);
					else
						Log.Error("Console", text[1]);
				}
			}
		}

		/// <summary>
		///     Plugin parancs függvénye.
		/// </summary>
		protected void HandlePlugin()
		{
			if(Info.Length >= 2 && Info[1].ToLower() == "load")
			{
				var text = sLManager.GetConsoleCommandTexts("plugin/load");
				if(text.Length < 3)
				{
					Log.Error("Console", sLConsole.Translations("NoFound2"));
					return;
				}

				if(sAddonManager.IsLoadingAddons())
				{
					Log.Error("Console", text[2]);
					return;
				}

				if(sAddonManager.LoadPluginsFromDirectory(AddonsConfig.Directory))
				{
					foreach(var nw in sIrcBase.Networks)
					{
						var asms = sAddonManager.Addons[nw.Key].Assemblies.ToDictionary(v => v.Key, v => v.Value);
						Parallel.ForEach(asms, asm =>
						{
							var types = asm.Value.GetTypes();
							Parallel.ForEach(types, type =>
							{
								var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
								Parallel.ForEach(methods, method =>
								{
									foreach(var attribute in Attribute.GetCustomAttributes(method))
									{
										if(attribute.IsOfType(typeof(IrcCommandAttribute)))
										{
											var attr = (IrcCommandAttribute)attribute;
											lock(Lock)
											{
												var del = Delegate.CreateDelegate(typeof(IRCDelegate), method) as IRCDelegate;
												sIrcBase.Networks[nw.Key].IrcRegisterHandler(attr.Command, del);
											}
										}

										if(attribute.IsOfType(typeof(SchumixCommandAttribute)))
										{
											var attr = (SchumixCommandAttribute)attribute;
											lock(Lock)
											{
												var del = Delegate.CreateDelegate(typeof(CommandDelegate), method) as CommandDelegate;
												sIrcBase.Networks[nw.Key].SchumixRegisterHandler(attr.Command, del, attr.Permission);
											}
										}
									}
								});
							});
						});
					}

					Log.Notice("Console", text[0]);
				}
				else
					Log.Error("Console", text[1]);
			}
			else if(Info.Length >= 2 && Info[1].ToLower() == "unload")
			{
				var text = sLManager.GetConsoleCommandTexts("plugin/unload");
				if(text.Length < 3)
				{
					Log.Error("Console", sLConsole.Translations("NoFound2"));
					return;
				}

				if(!sAddonManager.IsLoadingAddons())
				{
					Log.Error("Console", text[2]);
					return;
				}

				if(sAddonManager.UnloadPlugins())
				{
					foreach(var nw in sIrcBase.Networks)
					{
						var asms = sAddonManager.Addons[nw.Key].Assemblies.ToDictionary(v => v.Key, v => v.Value);
						Parallel.ForEach(asms, asm =>
						{
							var types = asm.Value.GetTypes();
							Parallel.ForEach(types, type =>
							{
								var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
								Parallel.ForEach(methods, method =>
								{
									foreach(var attribute in Attribute.GetCustomAttributes(method))
									{
										if(attribute.IsOfType(typeof(IrcCommandAttribute)))
										{
											var attr = (IrcCommandAttribute)attribute;
											lock(Lock)
											{
												var del = Delegate.CreateDelegate(typeof(IRCDelegate), method) as IRCDelegate;
												sIrcBase.Networks[nw.Key].IrcRemoveHandler(attr.Command, del);
											}
										}

										if(attribute.IsOfType(typeof(SchumixCommandAttribute)))
										{
											var attr = (SchumixCommandAttribute)attribute;
											lock(Lock)
											{
												var del = Delegate.CreateDelegate(typeof(CommandDelegate), method) as CommandDelegate;
												sIrcBase.Networks[nw.Key].SchumixRemoveHandler(attr.Command, del);
											}
										}
									}
								});
							});
						});
					}

					Log.Notice("Console", text[0]);
				}
				else
					Log.Error("Console", text[1]);
			}
			else
			{
				var text = sLManager.GetConsoleCommandTexts("plugin");
				if(text.Length < 3)
				{
					Log.Error("Console", sLConsole.Translations("NoFound2"));
					return;
				}

				string Plugins = string.Empty;
				string IgnorePlugins = string.Empty;

				foreach(var plugin in sAddonManager.Addons[_servername].Addons)
				{
					if(!sIrcBase.Networks[_servername].sIgnoreAddon.IsIgnore(plugin.Key))
						Plugins += ", " + plugin.Value.Name;
					else
						IgnorePlugins += ", " + plugin.Value.Name;
				}

				if(!Plugins.IsNullOrEmpty())
					Log.Notice("Console", text[0], Plugins.Remove(0, 2, ", "));

				if(!IgnorePlugins.IsNullOrEmpty())
					Log.Notice("Console", text[1], IgnorePlugins.Remove(0, 2, ", "));

				if(Plugins.IsNullOrEmpty() && IgnorePlugins.IsNullOrEmpty())
					Log.Warning("Console", text[2]);
			}
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

			Log.Notice("Console", text[0]);
			SchumixBase.Quit();
			sIrcBase.Shutdown(text[1]);
		}
	}
}