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
using Schumix.Api.Irc;
using Schumix.Irc.Util;
using Schumix.Framework;
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
				
				string[] ChannelInfo = sMyChannelInfo.ChannelFunctionsInfo(sIRCMessage.Channel).Split('|');
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
					
					string f = sMyChannelInfo.FunctionsInfo();
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
								if(!sMyChannelInfo.SearchFunction(sIRCMessage.Info[i]))
								{
									nosuchfunction += ", " + sIRCMessage.Info[i].ToLower();
									continue;
								}
								
								if(sMyChannelInfo.FSelect(sIRCMessage.Info[i]) && sIRCMessage.Info[5].ToLower() == SchumixBase.On)
								{
									onfunction += ", " + sIRCMessage.Info[i].ToLower();
									continue;
								}
								else if(!sMyChannelInfo.FSelect(sIRCMessage.Info[i]) && sIRCMessage.Info[5].ToLower() == SchumixBase.Off)
								{
									offfunction += ", " + sIRCMessage.Info[i].ToLower();
									continue;
								}
								
								args += ", " + sIRCMessage.Info[i].ToLower();
								SchumixBase.DManager.Update("schumix", string.Format("FunctionStatus = '{0}'", sIRCMessage.Info[5].ToLower()), string.Format("FunctionName = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[i].ToLower()), sIRCMessage.ServerName));
								sMyChannelInfo.FunctionsReload();
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
							if(!sMyChannelInfo.SearchFunction(sIRCMessage.Info[6]))
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Other("NoSuchFunctions", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
								return;
							}
							
							if(sMyChannelInfo.FSelect(sIRCMessage.Info[6]) && sIRCMessage.Info[5].ToLower() == SchumixBase.On)
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FunctionAlreadyTurnedOn", sIRCMessage.Channel, sIRCMessage.ServerName));
								return;
							}
							else if(!sMyChannelInfo.FSelect(sIRCMessage.Info[6]) && sIRCMessage.Info[5].ToLower() == SchumixBase.Off)
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FunctionAlreadyTurnedOff", sIRCMessage.Channel, sIRCMessage.ServerName));
								return;
							}
							
							if(sIRCMessage.Info[5].ToLower() == SchumixBase.On)
								sSendMessage.SendChatMessage(sIRCMessage, text[0], sIRCMessage.Info[6].ToLower());
							else
								sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Info[6].ToLower());
							
							SchumixBase.DManager.Update("schumix", string.Format("FunctionStatus = '{0}'", sIRCMessage.Info[5].ToLower()), string.Format("FunctionName = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName));
							sMyChannelInfo.FunctionsReload();
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
				
				if(!Rfc2812Util.IsValidChannelName(sIRCMessage.Info[5]))
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
					
					string[] ChannelInfo = sMyChannelInfo.ChannelFunctionsInfo(channel).Split('|');
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
							if(!sMyChannelInfo.SearchChannelFunction(sIRCMessage.Info[i]))
							{
								nosuchfunction += ", " + sIRCMessage.Info[i].ToLower();
								continue;
							}

							if(sMyChannelInfo.FSelect(sIRCMessage.Info[i], channel) && status == SchumixBase.On)
							{
								onfunction += ", " + sIRCMessage.Info[i].ToLower();
								continue;
							}
							else if(!sMyChannelInfo.FSelect(sIRCMessage.Info[i], channel) && status == SchumixBase.Off)
							{
								offfunction += ", " + sIRCMessage.Info[i].ToLower();
								continue;
							}

							if(sMyChannelInfo.SearchFunction(sIRCMessage.Info[i]))
							{
								if(!sMyChannelInfo.FSelect(sIRCMessage.Info[i]) && status == SchumixBase.On)
								{
									SchumixBase.DManager.Update("schumix", "FunctionStatus = 'on'", string.Format("FunctionName = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[i].ToLower()), sIRCMessage.ServerName));
									sMyChannelInfo.FunctionsReload();
								}
							}

							args += ", " + sIRCMessage.Info[i].ToLower();
							SchumixBase.DManager.Update("channels", string.Format("Functions = '{0}'", sMyChannelInfo.ChannelFunctions(sIRCMessage.Info[i].ToLower(), status, channel)), string.Format("Channel = '{0}' And ServerName = '{1}'", channel, sIRCMessage.ServerName));
							sMyChannelInfo.ChannelFunctionsReload();
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
						if(!sMyChannelInfo.SearchChannelFunction(sIRCMessage.Info[7]))
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Other("NoSuchFunctions", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
							return;
						}

						if(sMyChannelInfo.FSelect(sIRCMessage.Info[7], channel) && status == SchumixBase.On)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FunctionAlreadyTurnedOn", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}
						else if(!sMyChannelInfo.FSelect(sIRCMessage.Info[7], channel) && status == SchumixBase.Off)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FunctionAlreadyTurnedOff", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						if(sMyChannelInfo.SearchFunction(sIRCMessage.Info[7]))
						{
							if(!sMyChannelInfo.FSelect(sIRCMessage.Info[7]) && status == SchumixBase.On)
							{
								SchumixBase.DManager.Update("schumix", "FunctionStatus = 'on'", string.Format("FunctionName = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[7].ToLower()), sIRCMessage.ServerName));
								sMyChannelInfo.FunctionsReload();
							}
						}

						if(status == SchumixBase.On)
							sSendMessage.SendChatMessage(sIRCMessage, text[0], sIRCMessage.Info[7].ToLower());
						else
							sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Info[7].ToLower());

						SchumixBase.DManager.Update("channels", string.Format("Functions = '{0}'", sMyChannelInfo.ChannelFunctions(sIRCMessage.Info[7].ToLower(), status, channel)), string.Format("Channel = '{0}' And ServerName = '{1}'", channel, sIRCMessage.ServerName));
						sMyChannelInfo.ChannelFunctionsReload();
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
					sMyChannelInfo.ChannelFunctionsReload();
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

						sMyChannelInfo.ChannelFunctionsReload();
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("function/update/all", sIRCMessage.Channel, sIRCMessage.ServerName));
					}
					else
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel, sIRCMessage.ServerName));
				}
				else
				{
					if(!Rfc2812Util.IsValidChannelName(sIRCMessage.Info[5]))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaChannelHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("function/update", sIRCMessage.Channel, sIRCMessage.ServerName), sIRCMessage.Info[5].ToLower());
					SchumixBase.DManager.Update("channels", string.Format("Functions = '{0}'", sUtilities.GetFunctionUpdate()), string.Format("Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[5].ToLower()), sIRCMessage.ServerName));
					sMyChannelInfo.ChannelFunctionsReload();
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
							if(!sMyChannelInfo.SearchChannelFunction(sIRCMessage.Info[i]))
							{
								nosuchfunction += ", " + sIRCMessage.Info[i].ToLower();
								continue;
							}

							if(sMyChannelInfo.FSelect(sIRCMessage.Info[i], sIRCMessage.Channel) && status == SchumixBase.On)
							{
								onfunction += ", " + sIRCMessage.Info[i].ToLower();
								continue;
							}
							else if(!sMyChannelInfo.FSelect(sIRCMessage.Info[i], sIRCMessage.Channel) && status == SchumixBase.Off)
							{
								offfunction += ", " + sIRCMessage.Info[i].ToLower();
								continue;
							}

							if(sMyChannelInfo.SearchFunction(sIRCMessage.Info[i]))
							{
								if(!sMyChannelInfo.FSelect(sIRCMessage.Info[i]) && status == SchumixBase.On)
								{
									SchumixBase.DManager.Update("schumix", "FunctionStatus = 'on'", string.Format("FunctionName = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[i].ToLower()), sIRCMessage.ServerName));
									sMyChannelInfo.FunctionsReload();
								}
							}

							args += ", " + sIRCMessage.Info[i].ToLower();
							SchumixBase.DManager.Update("channels", string.Format("Functions = '{0}'", sMyChannelInfo.ChannelFunctions(sIRCMessage.Info[i].ToLower(), status, sIRCMessage.Channel)), string.Format("Channel = '{0}' And ServerName = '{1}'", sIRCMessage.Channel, sIRCMessage.ServerName));
							sMyChannelInfo.ChannelFunctionsReload();
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
						if(!sMyChannelInfo.SearchChannelFunction(sIRCMessage.Info[5]))
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Other("NoSuchFunctions", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
							return;
						}

						if(sMyChannelInfo.FSelect(sIRCMessage.Info[5], sIRCMessage.Channel) && status == SchumixBase.On)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FunctionAlreadyTurnedOn", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}
						else if(!sMyChannelInfo.FSelect(sIRCMessage.Info[5], sIRCMessage.Channel) && status == SchumixBase.Off)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FunctionAlreadyTurnedOff", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						if(sMyChannelInfo.SearchFunction(sIRCMessage.Info[5]))
						{
							if(!sMyChannelInfo.FSelect(sIRCMessage.Info[5]) && status == SchumixBase.On)
							{
								SchumixBase.DManager.Update("schumix", "FunctionStatus = 'on'", string.Format("FunctionName = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[5].ToLower()), sIRCMessage.ServerName));
								sMyChannelInfo.FunctionsReload();
							}
						}

						if(status == SchumixBase.On)
							sSendMessage.SendChatMessage(sIRCMessage, text[0], sIRCMessage.Info[5].ToLower());
						else
							sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Info[5].ToLower());

						SchumixBase.DManager.Update("channels", string.Format("Functions = '{0}'", sMyChannelInfo.ChannelFunctions(sIRCMessage.Info[5].ToLower(), status, sIRCMessage.Channel)), string.Format("Channel = '{0}' And ServerName = '{1}'", sIRCMessage.Channel, sIRCMessage.ServerName));
						sMyChannelInfo.ChannelFunctionsReload();
					}
				}
				else
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("WrongSwitch", sIRCMessage.Channel, sIRCMessage.ServerName));
			}
		}
	}
}