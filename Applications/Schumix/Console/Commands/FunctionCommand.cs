/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013-2015 Schumix Team <http://schumix.eu/>
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
using Schumix.Irc.Util;
using Schumix.Framework;
using Schumix.Framework.Logger;
using Schumix.Framework.Extensions;
using Schumix.Console.Delegate;

namespace Schumix.Console
{
	/// <summary>
	///     CommandHandler class.
	/// </summary>
	partial class CommandHandler
	{
		/// <summary>
		///     Function parancs függvénye.
		/// </summary>
		protected void HandleFunction(ConsoleMessage sConsoleMessage)
		{
			if(sConsoleMessage.Info.Length < 2)
			{
				Log.Error("Console", sLManager.GetConsoleWarningText("NoValue1"));
				return;
			}

			if(sConsoleMessage.Info[1].ToLower() == "channel")
			{
				var text = sLManager.GetConsoleCommandTexts("function/channel");
				if(text.Length < 3)
				{
					Log.Error("Console", sLConsole.Translations("NoFound2"));
					return;
				}

				if(sConsoleMessage.Info.Length < 3)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NoChannelName"));
					return;
				}

				if(!Rfc2812Util.IsValidChannelName(sConsoleMessage.Info[2]))
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NotaChannelHasBeenSet"));
					return;
				}

				var db0 = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", sConsoleMessage.Info[2].ToLower(), _servername);
				if(db0.IsNull())
				{
					Log.Error("Console", text[2]);
					return;
				}

				if(sConsoleMessage.Info.Length < 4)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NoValue1"));
					return;
				}

				string channel = sConsoleMessage.Info[2].ToLower();
				string status = sConsoleMessage.Info[3].ToLower();

				if(sConsoleMessage.Info[3].ToLower() == "info")
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
					if(sConsoleMessage.Info.Length < 5)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoFunctionName"));
						return;
					}

					if(sConsoleMessage.Info.Length >= 6)
					{
						string args = string.Empty;
						string onfunction = string.Empty;
						string offfunction = string.Empty;
						string nosuchfunction = string.Empty;

						for(int i = 4; i < sConsoleMessage.Info.Length; i++)
						{
							if(!sIrcBase.Networks[_servername].sMyChannelInfo.SearchChannelFunction(sConsoleMessage.Info[i]))
							{
								nosuchfunction += ", " + sConsoleMessage.Info[i].ToLower();
								continue;
							}

							if(sIrcBase.Networks[_servername].sMyChannelInfo.FSelect(sConsoleMessage.Info[i], channel) && status == SchumixBase.On)
							{
								onfunction += ", " + sConsoleMessage.Info[i].ToLower();
								continue;
							}
							else if(!sIrcBase.Networks[_servername].sMyChannelInfo.FSelect(sConsoleMessage.Info[i], channel) && status == SchumixBase.Off)
							{
								offfunction += ", " + sConsoleMessage.Info[i].ToLower();
								continue;
							}

							if(sIrcBase.Networks[_servername].sMyChannelInfo.SearchFunction(sConsoleMessage.Info[i]))
							{
								if(!sIrcBase.Networks[_servername].sMyChannelInfo.FSelect(sConsoleMessage.Info[i]) && status == SchumixBase.On)
								{
									SchumixBase.DManager.Update("schumix", "FunctionStatus = 'on'", string.Format("FunctionName = '{0}' And ServerName = '{1}'", sConsoleMessage.Info[i].ToLower(), _servername));
									sIrcBase.Networks[_servername].sMyChannelInfo.FunctionsReload();
								}
							}

							args += ", " + sConsoleMessage.Info[i].ToLower();
							SchumixBase.DManager.Update("channels", string.Format("Functions = '{0}'", sIrcBase.Networks[_servername].sMyChannelInfo.ChannelFunctions(sConsoleMessage.Info[i].ToLower(), status, channel)), string.Format("Channel = '{0}' And ServerName = '{1}'", channel, _servername));
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
						if(!sIrcBase.Networks[_servername].sMyChannelInfo.SearchChannelFunction(sConsoleMessage.Info[4]))
						{
							Log.Error("Console", sLConsole.Other("NoSuchFunctions"));
							return;
						}

						if(sIrcBase.Networks[_servername].sMyChannelInfo.FSelect(sConsoleMessage.Info[4], channel) && status == SchumixBase.On)
						{
							Log.Warning("Console", sLManager.GetConsoleWarningText("FunctionAlreadyTurnedOn"));
							return;
						}
						else if(!sIrcBase.Networks[_servername].sMyChannelInfo.FSelect(sConsoleMessage.Info[4], channel) && status == SchumixBase.Off)
						{
							Log.Warning("Console", sLManager.GetConsoleWarningText("FunctionAlreadyTurnedOff"));
							return;
						}

						if(sIrcBase.Networks[_servername].sMyChannelInfo.SearchFunction(sConsoleMessage.Info[4]))
						{
							if(!sIrcBase.Networks[_servername].sMyChannelInfo.FSelect(sConsoleMessage.Info[4]) && status == SchumixBase.On)
							{
								SchumixBase.DManager.Update("schumix", "FunctionStatus = 'on'", string.Format("FunctionName = '{0}' And ServerName = '{1}'", sConsoleMessage.Info[4].ToLower(), _servername));
								sIrcBase.Networks[_servername].sMyChannelInfo.FunctionsReload();
							}
						}

						if(status == SchumixBase.On)
							Log.Notice("Console", text[0], sConsoleMessage.Info[4].ToLower());
						else
							Log.Notice("Console", text[1], sConsoleMessage.Info[4].ToLower());

						SchumixBase.DManager.Update("channels", string.Format("Functions = '{0}'", sIrcBase.Networks[_servername].sMyChannelInfo.ChannelFunctions(sConsoleMessage.Info[4].ToLower(), status, channel)), string.Format("Channel = '{0}' And ServerName = '{1}'", channel, _servername));
						sIrcBase.Networks[_servername].sMyChannelInfo.ChannelFunctionsReload();
					}
				}
				else
					Log.Error("Console", sLManager.GetConsoleWarningText("WrongSwitch"));
			}
			else if(sConsoleMessage.Info[1].ToLower() == "update")
			{
				if(sConsoleMessage.Info.Length < 3)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NoValue1"));
					return;
				}

				if(sConsoleMessage.Info[2].ToLower() == "all")
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
					if(!Rfc2812Util.IsValidChannelName(sConsoleMessage.Info[2]))
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NotaChannelHasBeenSet"));
						return;
					}

					Log.Notice("Console", sLManager.GetConsoleCommandText("function/update"), sConsoleMessage.Info[2].ToLower());
					SchumixBase.DManager.Update("channels", string.Format("Functions = '{0}'", sUtilities.GetFunctionUpdate()), string.Format("Channel = '{0}' And ServerName = '{1}'", sConsoleMessage.Info[2].ToLower(), _servername));
					sIrcBase.Networks[_servername].sMyChannelInfo.ChannelFunctionsReload();
				}
			}
			else if(sConsoleMessage.Info[1].ToLower() == "info")
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
				if(sConsoleMessage.Info.Length < 3)
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

				if(sConsoleMessage.Info[1].ToLower() == SchumixBase.On || sConsoleMessage.Info[1].ToLower() == SchumixBase.Off)
				{
					if(sConsoleMessage.Info.Length >= 4)
					{
						string args = string.Empty;
						string onfunction = string.Empty;
						string offfunction = string.Empty;
						string nosuchfunction = string.Empty;

						for(int i = 2; i < sConsoleMessage.Info.Length; i++)
						{
							if(!sIrcBase.Networks[_servername].sMyChannelInfo.SearchFunction(sConsoleMessage.Info[i]))
							{
								nosuchfunction += ", " + sConsoleMessage.Info[i].ToLower();
								continue;
							}

							if(sIrcBase.Networks[_servername].sMyChannelInfo.FSelect(sConsoleMessage.Info[i]) && sConsoleMessage.Info[1].ToLower() == SchumixBase.On)
							{
								onfunction += ", " + sConsoleMessage.Info[i].ToLower();
								continue;
							}
							else if(!sIrcBase.Networks[_servername].sMyChannelInfo.FSelect(sConsoleMessage.Info[i]) && sConsoleMessage.Info[1].ToLower() == SchumixBase.Off)
							{
								offfunction += ", " + sConsoleMessage.Info[i].ToLower();
								continue;
							}

							args += ", " + sConsoleMessage.Info[i].ToLower();
							SchumixBase.DManager.Update("schumix", string.Format("FunctionStatus = '{0}'", sConsoleMessage.Info[1].ToLower()), string.Format("FunctionName = '{0}' And ServerName = '{1}'", sConsoleMessage.Info[i].ToLower(), _servername));
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

						if(sConsoleMessage.Info[1].ToLower() == SchumixBase.On)
							Log.Notice("Console", text[0],  args.Remove(0, 2, ", "));
						else
							Log.Notice("Console", text[1],  args.Remove(0, 2, ", "));
					}
					else
					{
						if(!sIrcBase.Networks[_servername].sMyChannelInfo.SearchFunction(sConsoleMessage.Info[2]))
						{
							Log.Error("Console", sLConsole.Other("NoSuchFunctions"));
							return;
						}

						if(sIrcBase.Networks[_servername].sMyChannelInfo.FSelect(sConsoleMessage.Info[2]) && sConsoleMessage.Info[1].ToLower() == SchumixBase.On)
						{
							Log.Warning("Console", sLManager.GetConsoleWarningText("FunctionAlreadyTurnedOn"));
							return;
						}
						else if(!sIrcBase.Networks[_servername].sMyChannelInfo.FSelect(sConsoleMessage.Info[2]) && sConsoleMessage.Info[1].ToLower() == SchumixBase.Off)
						{
							Log.Warning("Console", sLManager.GetConsoleWarningText("FunctionAlreadyTurnedOff"));
							return;
						}

						if(sConsoleMessage.Info[1].ToLower() == SchumixBase.On)
							Log.Notice("Console", text[0], sConsoleMessage.Info[2].ToLower());
						else
							Log.Notice("Console", text[1], sConsoleMessage.Info[2].ToLower());

						SchumixBase.DManager.Update("schumix", string.Format("FunctionStatus = '{0}'", sConsoleMessage.Info[1].ToLower()), string.Format("FunctionName = '{0}' And ServerName = '{1}'", sConsoleMessage.Info[2].ToLower(), _servername));
						sIrcBase.Networks[_servername].sMyChannelInfo.FunctionsReload();
					}
				}
				else
					Log.Error("Console", sLManager.GetConsoleWarningText("WrongSwitch"));
			}
		}
	}
}