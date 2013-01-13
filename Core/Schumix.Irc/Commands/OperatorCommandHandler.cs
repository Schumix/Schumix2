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
using Schumix.API;
using Schumix.API.Irc;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;

namespace Schumix.Irc.Commands
{
	public abstract partial class CommandHandler
	{
		protected void HandleFunction(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
				return;

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoValue", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			if(sIRCMessage.Info[4].ToLower() == "info")
			{
				var text = sLManager.GetCommandTexts("function/info", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 2)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}

				string[] ChannelInfo = sChannelInfo.ChannelFunctionsInfo(sIRCMessage.Channel).Split('|');
				if(ChannelInfo.Length < 2)
					return;

				sSendMessage.SendChatMessage(sIRCMessage, text[0], ChannelInfo[0]);
				sSendMessage.SendChatMessage(sIRCMessage, text[1], ChannelInfo[1]);
			}
			else if(sIRCMessage.Info[4].ToLower() == "all")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("No1Value", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "info")
				{
					var text = sLManager.GetCommandTexts("function/all/info", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					string f = sChannelInfo.FunctionsInfo();
					if(f == string.Empty)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					string[] FunctionInfo = f.Split('|');
					if(FunctionInfo.Length < 2)
						return;

					sSendMessage.SendChatMessage(sIRCMessage, text[0], FunctionInfo[0]);
					sSendMessage.SendChatMessage(sIRCMessage, text[1], FunctionInfo[1]);
				}
				else
				{
					var text = sLManager.GetCommandTexts("function/all", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoFunctionName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(sIRCMessage.Info[5].ToLower() == SchumixBase.On || sIRCMessage.Info[5].ToLower() == SchumixBase.Off)
					{
						if(sIRCMessage.Info.Length >= 8)
						{
							string args = string.Empty;
							string onfunction = string.Empty;
							string offfunction = string.Empty;
							string nosuchfunction = string.Empty;

							for(int i = 6; i < sIRCMessage.Info.Length; i++)
							{
								if(!sChannelInfo.SearchFunction(sIRCMessage.Info[i]))
								{
									nosuchfunction += ", " + sIRCMessage.Info[i].ToLower();
									continue;
								}

								if(sChannelInfo.FSelect(sIRCMessage.Info[i]) && sIRCMessage.Info[5].ToLower() == SchumixBase.On)
								{
									onfunction += ", " + sIRCMessage.Info[i].ToLower();
									continue;
								}
								else if(!sChannelInfo.FSelect(sIRCMessage.Info[i]) && sIRCMessage.Info[5].ToLower() == SchumixBase.Off)
								{
									offfunction += ", " + sIRCMessage.Info[i].ToLower();
									continue;
								}

								args += ", " + sIRCMessage.Info[i].ToLower();
								SchumixBase.DManager.Update("schumix", string.Format("FunctionStatus = '{0}'", sIRCMessage.Info[5].ToLower()), string.Format("FunctionName = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[i].ToLower()), sIRCMessage.ServerName));
								sChannelInfo.FunctionsReload();
							}

							if(onfunction != string.Empty)
								sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FunctionAlreadyTurnedOn2", sIRCMessage.Channel, sIRCMessage.ServerName), onfunction.Remove(0, 2, ", "));
			
							if(offfunction != string.Empty)
								sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FunctionAlreadyTurnedOff2", sIRCMessage.Channel, sIRCMessage.ServerName), offfunction.Remove(0, 2, ", "));

							if(nosuchfunction != string.Empty)
								sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Other("NoSuchFunctions2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)), nosuchfunction.Remove(0, 2, ", "));

							if(args.Length == 0)
								return;

							if(sIRCMessage.Info[5].ToLower() == SchumixBase.On)
								sSendMessage.SendChatMessage(sIRCMessage, text[0],  args.Remove(0, 2, ", "));
							else
								sSendMessage.SendChatMessage(sIRCMessage, text[1],  args.Remove(0, 2, ", "));
						}
						else
						{
							if(!sChannelInfo.SearchFunction(sIRCMessage.Info[6]))
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Other("NoSuchFunctions", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
								return;
							}

							if(sChannelInfo.FSelect(sIRCMessage.Info[6]) && sIRCMessage.Info[5].ToLower() == SchumixBase.On)
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FunctionAlreadyTurnedOn", sIRCMessage.Channel, sIRCMessage.ServerName));
								return;
							}
							else if(!sChannelInfo.FSelect(sIRCMessage.Info[6]) && sIRCMessage.Info[5].ToLower() == SchumixBase.Off)
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FunctionAlreadyTurnedOff", sIRCMessage.Channel, sIRCMessage.ServerName));
								return;
							}

							if(sIRCMessage.Info[5].ToLower() == SchumixBase.On)
								sSendMessage.SendChatMessage(sIRCMessage, text[0], sIRCMessage.Info[6].ToLower());
							else
								sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Info[6].ToLower());

							SchumixBase.DManager.Update("schumix", string.Format("FunctionStatus = '{0}'", sIRCMessage.Info[5].ToLower()), string.Format("FunctionName = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName));
							sChannelInfo.FunctionsReload();
						}
					}
					else
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("WrongSwitch", sIRCMessage.Channel, sIRCMessage.ServerName));
				}
			}
			else if(sIRCMessage.Info[4].ToLower() == "channel")
			{
				var text = sLManager.GetCommandTexts("function/channel", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 3)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}

				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(!IsChannel(sIRCMessage.Info[5]))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaChannelHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				var db0 = SchumixBase.DManager.QueryFirstRow("SELECT* FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[5].ToLower()), sIRCMessage.ServerName);
				if(db0.IsNull())
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[2]);
					return;
				}

				if(sIRCMessage.Info.Length < 7)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("No1Value", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}
			
				string channel = sIRCMessage.Info[5].ToLower();
				string status = sIRCMessage.Info[6].ToLower();
			
				if(sIRCMessage.Info[6].ToLower() == "info")
				{
					var text2 = sLManager.GetCommandTexts("function/channel/info", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text2.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					string[] ChannelInfo = sChannelInfo.ChannelFunctionsInfo(channel).Split('|');
					if(ChannelInfo.Length < 2)
						return;

					sSendMessage.SendChatMessage(sIRCMessage, text2[0], ChannelInfo[0]);
					sSendMessage.SendChatMessage(sIRCMessage, text2[1], ChannelInfo[1]);
				}
				else if(status == SchumixBase.On || status == SchumixBase.Off)
				{
					if(sIRCMessage.Info.Length < 8)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoFunctionName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(sIRCMessage.Info.Length >= 9)
					{
						string args = string.Empty;
						string onfunction = string.Empty;
						string offfunction = string.Empty;
						string nosuchfunction = string.Empty;

						for(int i = 7; i < sIRCMessage.Info.Length; i++)
						{
							if(!sChannelInfo.SearchChannelFunction(sIRCMessage.Info[i]))
							{
								nosuchfunction += ", " + sIRCMessage.Info[i].ToLower();
								continue;
							}

							if(sChannelInfo.FSelect(sIRCMessage.Info[i], channel) && status == SchumixBase.On)
							{
								onfunction += ", " + sIRCMessage.Info[i].ToLower();
								continue;
							}
							else if(!sChannelInfo.FSelect(sIRCMessage.Info[i], channel) && status == SchumixBase.Off)
							{
								offfunction += ", " + sIRCMessage.Info[i].ToLower();
								continue;
							}

							if(sChannelInfo.SearchFunction(sIRCMessage.Info[i]))
							{
								if(!sChannelInfo.FSelect(sIRCMessage.Info[i]) && status == SchumixBase.On)
								{
									SchumixBase.DManager.Update("schumix", "FunctionStatus = 'on'", string.Format("FunctionName = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[i].ToLower()), sIRCMessage.ServerName));
									sChannelInfo.FunctionsReload();
								}
							}

							args += ", " + sIRCMessage.Info[i].ToLower();
							SchumixBase.DManager.Update("channels", string.Format("Functions = '{0}'", sChannelInfo.ChannelFunctions(sIRCMessage.Info[i].ToLower(), status, channel)), string.Format("Channel = '{0}' And ServerName = '{1}'", channel, sIRCMessage.ServerName));
							sChannelInfo.ChannelFunctionsReload();
						}

						if(onfunction != string.Empty)
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FunctionAlreadyTurnedOn2", sIRCMessage.Channel, sIRCMessage.ServerName), onfunction.Remove(0, 2, ", "));
			
						if(offfunction != string.Empty)
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FunctionAlreadyTurnedOff2", sIRCMessage.Channel, sIRCMessage.ServerName), offfunction.Remove(0, 2, ", "));

						if(nosuchfunction != string.Empty)
							sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Other("NoSuchFunctions2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)), nosuchfunction.Remove(0, 2, ", "));

						if(args.Length == 0)
							return;

						if(status == SchumixBase.On)
							sSendMessage.SendChatMessage(sIRCMessage, text[0],  args.Remove(0, 2, ", "));
						else
							sSendMessage.SendChatMessage(sIRCMessage, text[1],  args.Remove(0, 2, ", "));
					}
					else
					{
						if(!sChannelInfo.SearchChannelFunction(sIRCMessage.Info[7]))
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Other("NoSuchFunctions", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
							return;
						}

						if(sChannelInfo.FSelect(sIRCMessage.Info[7], channel) && status == SchumixBase.On)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FunctionAlreadyTurnedOn", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}
						else if(!sChannelInfo.FSelect(sIRCMessage.Info[7], channel) && status == SchumixBase.Off)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FunctionAlreadyTurnedOff", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						if(sChannelInfo.SearchFunction(sIRCMessage.Info[7]))
						{
							if(!sChannelInfo.FSelect(sIRCMessage.Info[7]) && status == SchumixBase.On)
							{
								SchumixBase.DManager.Update("schumix", "FunctionStatus = 'on'", string.Format("FunctionName = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[7].ToLower()), sIRCMessage.ServerName));
								sChannelInfo.FunctionsReload();
							}
						}

						if(status == SchumixBase.On)
							sSendMessage.SendChatMessage(sIRCMessage, text[0], sIRCMessage.Info[7].ToLower());
						else
							sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Info[7].ToLower());

						SchumixBase.DManager.Update("channels", string.Format("Functions = '{0}'", sChannelInfo.ChannelFunctions(sIRCMessage.Info[7].ToLower(), status, channel)), string.Format("Channel = '{0}' And ServerName = '{1}'", channel, sIRCMessage.ServerName));
						sChannelInfo.ChannelFunctionsReload();
					}
				}
				else
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("WrongSwitch", sIRCMessage.Channel, sIRCMessage.ServerName));
			}
			else if(sIRCMessage.Info[4].ToLower() == "update")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("function/update", sIRCMessage.Channel, sIRCMessage.ServerName), sIRCMessage.Channel);
					SchumixBase.DManager.Update("channels", string.Format("Functions = '{0}'", sUtilities.GetFunctionUpdate()), string.Format("Channel = '{0}' And ServerName = '{1}'", sIRCMessage.Channel, sIRCMessage.ServerName));
					sChannelInfo.ChannelFunctionsReload();
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "all")
				{
					var db = SchumixBase.DManager.Query("SELECT Channel FROM channel WHERE ServerName = '{0}'", _servername);
					if(!db.IsNull())
					{
						foreach(DataRow row in db.Rows)
						{
							string channel = row["Channel"].ToString();
							SchumixBase.DManager.Update("channels", string.Format("Functions = '{0}'", sUtilities.GetFunctionUpdate()), string.Format("Channel = '{0}' And ServerName = '{1}'", channel, sIRCMessage.ServerName));
						}

						sChannelInfo.ChannelFunctionsReload();
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("function/update/all", sIRCMessage.Channel, sIRCMessage.ServerName));
					}
					else
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel, sIRCMessage.ServerName));
				}
				else
				{
					if(!IsChannel(sIRCMessage.Info[5]))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaChannelHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("function/update", sIRCMessage.Channel, sIRCMessage.ServerName), sIRCMessage.Info[5].ToLower());
					SchumixBase.DManager.Update("channels", string.Format("Functions = '{0}'", sUtilities.GetFunctionUpdate()), string.Format("Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[5].ToLower()), sIRCMessage.ServerName));
					sChannelInfo.ChannelFunctionsReload();
				}
			}
			else
			{
				var text = sLManager.GetCommandTexts("function", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 2)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}

				if(sIRCMessage.Info.Length < 5)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoFunctionStatus", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoFunctionName", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				string status = sIRCMessage.Info[4].ToLower();

				if(status == SchumixBase.On || status == SchumixBase.Off)
				{
					if(sIRCMessage.Info.Length >= 7)
					{
						string args = string.Empty;
						string onfunction = string.Empty;
						string offfunction = string.Empty;
						string nosuchfunction = string.Empty;

						for(int i = 5; i < sIRCMessage.Info.Length; i++)
						{
							if(!sChannelInfo.SearchChannelFunction(sIRCMessage.Info[i]))
							{
								nosuchfunction += ", " + sIRCMessage.Info[i].ToLower();
								continue;
							}

							if(sChannelInfo.FSelect(sIRCMessage.Info[i], sIRCMessage.Channel) && status == SchumixBase.On)
							{
								onfunction += ", " + sIRCMessage.Info[i].ToLower();
								continue;
							}
							else if(!sChannelInfo.FSelect(sIRCMessage.Info[i], sIRCMessage.Channel) && status == SchumixBase.Off)
							{
								offfunction += ", " + sIRCMessage.Info[i].ToLower();
								continue;
							}

							if(sChannelInfo.SearchFunction(sIRCMessage.Info[i]))
							{
								if(!sChannelInfo.FSelect(sIRCMessage.Info[i]) && status == SchumixBase.On)
								{
									SchumixBase.DManager.Update("schumix", "FunctionStatus = 'on'", string.Format("FunctionName = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[i].ToLower()), sIRCMessage.ServerName));
									sChannelInfo.FunctionsReload();
								}
							}

							args += ", " + sIRCMessage.Info[i].ToLower();
							SchumixBase.DManager.Update("channels", string.Format("Functions = '{0}'", sChannelInfo.ChannelFunctions(sIRCMessage.Info[i].ToLower(), status, sIRCMessage.Channel)), string.Format("Channel = '{0}' And ServerName = '{1}'", sIRCMessage.Channel, sIRCMessage.ServerName));
							sChannelInfo.ChannelFunctionsReload();
						}

						if(onfunction != string.Empty)
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FunctionAlreadyTurnedOn2", sIRCMessage.Channel, sIRCMessage.ServerName), onfunction.Remove(0, 2, ", "));
			
						if(offfunction != string.Empty)
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FunctionAlreadyTurnedOff2", sIRCMessage.Channel, sIRCMessage.ServerName), offfunction.Remove(0, 2, ", "));

						if(nosuchfunction != string.Empty)
							sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Other("NoSuchFunctions2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)), nosuchfunction.Remove(0, 2, ", "));

						if(args.Length == 0)
							return;

						if(status == SchumixBase.On)
							sSendMessage.SendChatMessage(sIRCMessage, text[0],  args.Remove(0, 2, ", "));
						else
							sSendMessage.SendChatMessage(sIRCMessage, text[1],  args.Remove(0, 2, ", "));
					}
					else
					{
						if(!sChannelInfo.SearchChannelFunction(sIRCMessage.Info[5]))
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Other("NoSuchFunctions", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
							return;
						}

						if(sChannelInfo.FSelect(sIRCMessage.Info[5], sIRCMessage.Channel) && status == SchumixBase.On)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FunctionAlreadyTurnedOn", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}
						else if(!sChannelInfo.FSelect(sIRCMessage.Info[5], sIRCMessage.Channel) && status == SchumixBase.Off)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FunctionAlreadyTurnedOff", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						if(sChannelInfo.SearchFunction(sIRCMessage.Info[5]))
						{
							if(!sChannelInfo.FSelect(sIRCMessage.Info[5]) && status == SchumixBase.On)
							{
								SchumixBase.DManager.Update("schumix", "FunctionStatus = 'on'", string.Format("FunctionName = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[5].ToLower()), sIRCMessage.ServerName));
								sChannelInfo.FunctionsReload();
							}
						}

						if(status == SchumixBase.On)
							sSendMessage.SendChatMessage(sIRCMessage, text[0], sIRCMessage.Info[5].ToLower());
						else
							sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Info[5].ToLower());

						SchumixBase.DManager.Update("channels", string.Format("Functions = '{0}'", sChannelInfo.ChannelFunctions(sIRCMessage.Info[5].ToLower(), status, sIRCMessage.Channel)), string.Format("Channel = '{0}' And ServerName = '{1}'", sIRCMessage.Channel, sIRCMessage.ServerName));
						sChannelInfo.ChannelFunctionsReload();
					}
				}
				else
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("WrongSwitch", sIRCMessage.Channel, sIRCMessage.ServerName));
			}
		}

		protected void HandleChannel(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
				return;

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("channel", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			if(sIRCMessage.Info[4].ToLower() == "add")
			{
				var text = sLManager.GetCommandTexts("channel/add", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 2)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}

				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				string channel = sIRCMessage.Info[5].ToLower();

				if(!IsChannel(channel))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaChannelHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sIgnoreChannel.IsIgnore(channel))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ThisChannelBlockedByAdmin", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(channel), sIRCMessage.ServerName);
				if(!db.IsNull())
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[0]);
					return;
				}

				if(sIRCMessage.Info.Length == 7)
				{
					ChannelPrivmsg = sIRCMessage.Channel;
					string pass = sIRCMessage.Info[6];
					sSender.Join(channel, pass);
					SchumixBase.DManager.Insert("`channels`(ServerId, ServerName, Channel, Password, Language)", sIRCMessage.ServerId, sIRCMessage.ServerName, sUtilities.SqlEscape(channel), sUtilities.SqlEscape(pass), sLManager.Locale);
					SchumixBase.DManager.Update("channels", "Enabled = 'true'", string.Format("Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(channel), sIRCMessage.ServerName));
				}
				else
				{
					ChannelPrivmsg = sIRCMessage.Channel;
					sSender.Join(channel);
					SchumixBase.DManager.Insert("`channels`(ServerId, ServerName, Channel, Password, Language)", sIRCMessage.ServerId, sIRCMessage.ServerName, sUtilities.SqlEscape(channel), string.Empty, sLManager.Locale);
					SchumixBase.DManager.Update("channels", "Enabled = 'true'", string.Format("Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(channel), sIRCMessage.ServerName));
				}

				sSendMessage.SendChatMessage(sIRCMessage, text[1], channel);
				sChannelInfo.ChannelListReload();
				sChannelInfo.ChannelFunctionsReload();
			}
			else if(sIRCMessage.Info[4].ToLower() == "remove")
			{
				var text = sLManager.GetCommandTexts("channel/remove", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 3)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}

				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				string channel = sIRCMessage.Info[5].ToLower();

				if(!IsChannel(channel))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaChannelHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(channel == IRCConfig.List[sIRCMessage.ServerName].MasterChannel.ToLower())
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[0]);
					return;
				}

				var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(channel), sIRCMessage.ServerName);
				if(db.IsNull())
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
					return;
				}

				sSender.Part(channel);
				SchumixBase.DManager.Delete("channels", string.Format("Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(channel), sIRCMessage.ServerName));
				sSendMessage.SendChatMessage(sIRCMessage, text[2], channel);
				sChannelInfo.ChannelListReload();
				sChannelInfo.ChannelFunctionsReload();
			}
			else if(sIRCMessage.Info[4].ToLower() == "update")
			{
				sChannelInfo.ChannelListReload();
				sChannelInfo.ChannelFunctionsReload();
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("channel/update", sIRCMessage.Channel, sIRCMessage.ServerName));
			}
			else if(sIRCMessage.Info[4].ToLower() == "info")
			{
				var text = sLManager.GetCommandTexts("channel/info", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 4)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}

				var db = SchumixBase.DManager.Query("SELECT Channel, Enabled, Error FROM channels WHERE ServerName = '{0}'", sIRCMessage.ServerName);
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
							InActiveChannels += ", " + channel + SchumixBase.Colon + row["Error"].ToString();
					}

					if(ActiveChannels.Length > 0)
						sSendMessage.SendChatMessage(sIRCMessage, text[0], ActiveChannels.Remove(0, 2, ", "));
					else
						sSendMessage.SendChatMessage(sIRCMessage, text[1]);

					if(InActiveChannels.Length > 0)
						sSendMessage.SendChatMessage(sIRCMessage, text[2], InActiveChannels.Remove(0, 2, ", "));
					else
						sSendMessage.SendChatMessage(sIRCMessage, text[3]);
				}
				else
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel, sIRCMessage.ServerName));
			}
			else if(sIRCMessage.Info[4].ToLower() == "language")
			{
				var text = sLManager.GetCommandTexts("channel/language", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 3)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}

				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(!IsChannel(sIRCMessage.Info[5]))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaChannelHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[5].ToLower()), sIRCMessage.ServerName);
				if(db.IsNull())
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
					return;
				}

				if(sIRCMessage.Info.Length < 7)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoChannelLanguage", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				db = SchumixBase.DManager.QueryFirstRow("SELECT Language FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[5].ToLower()), sIRCMessage.ServerName);
				if(!db.IsNull())
				{
					if(db["Language"].ToString().ToLower() == sIRCMessage.Info[6].ToLower())
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[2], sIRCMessage.Info[6]);
						return;
					}
				}

				SchumixBase.DManager.Update("channels", string.Format("Language = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[6])), string.Format("Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[5].ToLower()), sIRCMessage.ServerName));
				sSendMessage.SendChatMessage(sIRCMessage, text[0], sIRCMessage.Info[6]);
				SchumixBase.sCacheDB.ReLoad("channels");
			}
			else if(sIRCMessage.Info[4].ToLower() == "password")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoValue", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "add")
				{
					var text = sLManager.GetCommandTexts("channel/password/add", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 3)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}
	
					if(!IsChannel(sIRCMessage.Info[6]))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaChannelHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(sChannelList.List.ContainsKey(sIRCMessage.Info[6].ToLower()))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ImAlreadyOnThisChannel", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(sIRCMessage.Info.Length < 8)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoPassword", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName);
					if(db.IsNull())
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}

					db = SchumixBase.DManager.QueryFirstRow("SELECT Password FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName);
					if(!db.IsNull())
					{
						if(db["Password"].ToString().Trim() != string.Empty)
						{
							sSendMessage.SendChatMessage(sIRCMessage, text[1]);
							return;
						}
					}

					SchumixBase.DManager.Update("channels", string.Format("Password = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[7])), string.Format("Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName));
					sSendMessage.SendChatMessage(sIRCMessage, text[2], sIRCMessage.Info[6]);
					SchumixBase.sCacheDB.ReLoad("channels");
				}
				else if(sIRCMessage.Info[5].ToLower() == "remove")
				{
					var text = sLManager.GetCommandTexts("channel/password/remove", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 3)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}
	
					if(!IsChannel(sIRCMessage.Info[6]))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaChannelHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName);
					if(db.IsNull())
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}

					db = SchumixBase.DManager.QueryFirstRow("SELECT Password FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName);
					if(!db.IsNull())
					{
						if(db["Password"].ToString().Trim() == string.Empty)
						{
							sSendMessage.SendChatMessage(sIRCMessage, text[1]);
							return;
						}
					}

					SchumixBase.DManager.Update("channels", "Password = ''", string.Format("Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName));
					sSendMessage.SendChatMessage(sIRCMessage, text[2]);
					SchumixBase.sCacheDB.ReLoad("channels");
				}
				else if(sIRCMessage.Info[5].ToLower() == "update")
				{
					var text = sLManager.GetCommandTexts("channel/password/update", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 3)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}
	
					if(!IsChannel(sIRCMessage.Info[6]))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaChannelHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(sIRCMessage.Info.Length < 8)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoPassword", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName);
					if(db.IsNull())
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}

					db = SchumixBase.DManager.QueryFirstRow("SELECT Password FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName);
					if(!db.IsNull())
					{
						if(db["Password"].ToString().Trim() == string.Empty)
						{
							sSendMessage.SendChatMessage(sIRCMessage, text[1]);
							return;
						}
					}

					SchumixBase.DManager.Update("channels", string.Format("Password = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[7])), string.Format("Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName));
					sSendMessage.SendChatMessage(sIRCMessage, text[2], sIRCMessage.Info[7]);
					SchumixBase.sCacheDB.ReLoad("channels");
				}
				else if(sIRCMessage.Info[5].ToLower() == "info")
				{
					var text = sLManager.GetCommandTexts("channel/password/info", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 3)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}
	
					if(!IsChannel(sIRCMessage.Info[6]))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaChannelHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName);
					if(db.IsNull())
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}

					db = SchumixBase.DManager.QueryFirstRow("SELECT Password FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName);
					if(!db.IsNull())
					{
						if(db["Password"].ToString().Trim() == string.Empty)
							sSendMessage.SendChatMessage(sIRCMessage, text[1]);
						else
							sSendMessage.SendChatMessage(sIRCMessage, text[2]);
					}
				}
			}
		}

		protected void HandleKick(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
				return;

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			string kick = sIRCMessage.Info[4].ToLower();

			if(sIRCMessage.Info.Length == 5)
			{
				if(kick != sMyNickInfo.NickStorage.ToLower())
					sSender.Kick(sIRCMessage.Channel, kick);
			}
			else if(sIRCMessage.Info.Length >= 6)
			{
				if(kick != sMyNickInfo.NickStorage.ToLower())
					sSender.Kick(sIRCMessage.Channel, kick, sIRCMessage.Info.SplitToString(5, SchumixBase.Space));
			}
		}

		protected void HandleMode(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
				return;

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoRank", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			if(sIRCMessage.Info.Length == 5)
			{
				sSender.Mode(sIRCMessage.Channel, sIRCMessage.Info[4].ToLower());
				return;
			}

			if(sIRCMessage.Info.Length < 6)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			string rank = sIRCMessage.Info[4].ToLower();
			string name = sIRCMessage.Info.SplitToString(5, SchumixBase.Space).ToLower();

			if(!name.Contains(sMyNickInfo.NickStorage.ToLower()))
				sSender.Mode(sIRCMessage.Channel, rank, name);
		}

		protected void HandleIgnore(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
				return;

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoValue", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			if(sIRCMessage.Info[4].ToLower() == "irc")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("No1Value", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "command")
				{
					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("No1Value", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(sIRCMessage.Info[6].ToLower() == "add")
					{
						var text = sLManager.GetCommandTexts("ignore/irc/command/add", sIRCMessage.Channel, sIRCMessage.ServerName);
						if(text.Length < 2)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
							return;
						}

						if(sIRCMessage.Info.Length < 8)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoCommand", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						string command = sIRCMessage.Info[7].ToLower();

						if(sIgnoreIrcCommand.IsIgnore(command))
						{
							sSendMessage.SendChatMessage(sIRCMessage, text[0]);
							return;
						}

						sIgnoreIrcCommand.Add(command);
						sSendMessage.SendChatMessage(sIRCMessage, text[1]);
					}
					else if(sIRCMessage.Info[6].ToLower() == "remove")
					{
						var text = sLManager.GetCommandTexts("ignore/irc/command/remove", sIRCMessage.Channel, sIRCMessage.ServerName);
						if(text.Length < 2)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
							return;
						}

						if(sIRCMessage.Info.Length < 8)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoCommand", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						string command = sIRCMessage.Info[7].ToLower();

						if(!sIgnoreIrcCommand.IsIgnore(command))
						{
							sSendMessage.SendChatMessage(sIRCMessage, text[0]);
							return;
						}

						sIgnoreIrcCommand.Remove(command);
						sSendMessage.SendChatMessage(sIRCMessage, text[1]);
					}
					else if(sIRCMessage.Info[6].ToLower() == "search")
					{
						var text = sLManager.GetCommandTexts("ignore/irc/command/search", sIRCMessage.Channel, sIRCMessage.ServerName);
						if(text.Length < 2)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
							return;
						}

						if(sIRCMessage.Info.Length < 8)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoCommand", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						if(sIgnoreIrcCommand.Contains(sIRCMessage.Info[7].ToLower()))
							sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						else
							sSendMessage.SendChatMessage(sIRCMessage, text[1]);
					}
				}
			}
			else if(sIRCMessage.Info[4].ToLower() == "command")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("No1Value", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "add")
				{
					var text = sLManager.GetCommandTexts("ignore/command/add", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoCommand", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					string command = sIRCMessage.Info[6].ToLower();

					if(command == "ignore" || command == "admin")
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoIgnoreCommand", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(IsAdmin(sIRCMessage.Nick, AdminFlag.Operator) && sIrcBase.Networks[sIRCMessage.ServerName].CommandMethodMap.ContainsKey(command) && sIrcBase.Networks[sIRCMessage.ServerName].CommandMethodMap[command].Permission != CommandPermission.Administrator)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoAdministrator", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(sIgnoreCommand.IsIgnore(command))
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}

					sIgnoreCommand.Add(command);
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				}
				else if(sIRCMessage.Info[5].ToLower() == "remove")
				{
					var text = sLManager.GetCommandTexts("ignore/command/remove", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoCommand", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					string command = sIRCMessage.Info[6].ToLower();

					if(!sIgnoreCommand.IsIgnore(command))
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}

					sIgnoreCommand.Remove(command);
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				}
				else if(sIRCMessage.Info[5].ToLower() == "search")
				{
					var text = sLManager.GetCommandTexts("ignore/command/search", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoCommand", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(sIgnoreCommand.Contains(sIRCMessage.Info[6].ToLower()))
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
					else
						sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				}
			}
			else if(sIRCMessage.Info[4].ToLower() == "channel")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("No1Value", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "add")
				{
					var text = sLManager.GetCommandTexts("ignore/channel/add", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					string channel = sIRCMessage.Info[6].ToLower();

					if(!IsChannel(channel))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaChannelHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(channel == IRCConfig.List[sIRCMessage.ServerName].MasterChannel.ToLower())
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoIgnoreMasterChannel", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(sIgnoreChannel.IsIgnore(channel))
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}

					sIgnoreChannel.Add(channel);
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				}
				else if(sIRCMessage.Info[5].ToLower() == "remove")
				{
					var text = sLManager.GetCommandTexts("ignore/channel/remove", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					string channel = sIRCMessage.Info[6].ToLower();

					if(!IsChannel(channel))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaChannelHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(!sIgnoreChannel.IsIgnore(channel))
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}

					sIgnoreChannel.Remove(channel);
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				}
				else if(sIRCMessage.Info[5].ToLower() == "search")
				{
					var text = sLManager.GetCommandTexts("ignore/channel/search", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					string channel = sIRCMessage.Info[6].ToLower();

					if(!IsChannel(channel))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaChannelHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(sIgnoreChannel.Contains(channel))
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
					else
						sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				}
			}
			else if(sIRCMessage.Info[4].ToLower() == "nick")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("No1Value", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "add")
				{
					var text = sLManager.GetCommandTexts("ignore/nick/add", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					string nick = sIRCMessage.Info[6].ToLower();

					if(nick == sIRCMessage.Nick.ToLower())
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoIgnoreMyNick", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(IsAdmin(sIRCMessage.Nick, AdminFlag.Operator) && IsAdmin(nick, AdminFlag.Administrator))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoAdministrator", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(sIgnoreNickName.IsIgnore(nick))
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}

					sIgnoreNickName.Add(nick);
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				}
				else if(sIRCMessage.Info[5].ToLower() == "remove")
				{
					var text = sLManager.GetCommandTexts("ignore/nick/remove", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					string nick = sIRCMessage.Info[6].ToLower();

					if(!sIgnoreNickName.IsIgnore(nick))
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}

					sIgnoreNickName.Remove(nick);
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				}
				else if(sIRCMessage.Info[5].ToLower() == "search")
				{
					var text = sLManager.GetCommandTexts("ignore/nick/search", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(sIgnoreNickName.Contains(sIRCMessage.Info[6].ToLower()))
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
					else
						sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				}
			}
			else if(sIRCMessage.Info[4].ToLower() == "addon")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("No1Value", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "add")
				{
					var text = sLManager.GetCommandTexts("ignore/addon/add", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					string addon = sIRCMessage.Info[6].ToLower();

					if(!sAddonManager.IsAddon(sIRCMessage.ServerName, addon))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ThereIsNoSuchAnAddon", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(sIgnoreAddon.IsIgnore(addon))
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}

					sIgnoreAddon.Add(addon);
					sIgnoreAddon.UnloadPlugin(addon);
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				}
				else if(sIRCMessage.Info[5].ToLower() == "remove")
				{
					var text = sLManager.GetCommandTexts("ignore/addon/remove", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					string addon = sIRCMessage.Info[6].ToLower();

					if(!sAddonManager.IsAddon(sIRCMessage.ServerName, addon))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ThereIsNoSuchAnAddon", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(!sIgnoreAddon.IsIgnore(addon))
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}

					sIgnoreAddon.Remove(addon);
					sIgnoreAddon.LoadPlugin(addon);
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				}
				else if(sIRCMessage.Info[5].ToLower() == "search")
				{
					var text = sLManager.GetCommandTexts("ignore/addon/search", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					string addon = sIRCMessage.Info[6].ToLower();

					if(!sAddonManager.IsAddon(sIRCMessage.ServerName, addon))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ThereIsNoSuchAnAddon", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(sIgnoreAddon.Contains(addon))
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
					else
						sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				}
			}
		}
	}
}