/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Megax <http://www.megaxx.info/>
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
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;

namespace Schumix.Irc.Commands
{
	public partial class CommandHandler
	{
		protected void HandleFunction(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
				return;

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoValue", sIRCMessage.Channel));
				return;
			}

			if(sIRCMessage.Info[4].ToLower() == "info")
			{
				var text = sLManager.GetCommandTexts("function/info", sIRCMessage.Channel);
				if(text.Length < 2)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
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
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("No1Value", sIRCMessage.Channel));
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "info")
				{
					var text = sLManager.GetCommandTexts("function/all/info", sIRCMessage.Channel);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						return;
					}

					string f = sChannelInfo.FunctionsInfo();
					if(f == "Hibás lekérdezés!")
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel));
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
					var text = sLManager.GetCommandTexts("function/all", sIRCMessage.Channel);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoFunctionName", sIRCMessage.Channel));
						return;
					}

					if(sIRCMessage.Info[5].ToLower() == SchumixBase.On || sIRCMessage.Info[5].ToLower() == SchumixBase.Off)
					{
						if(sIRCMessage.Info.Length >= 8)
						{
							string args = string.Empty;

							for(int i = 6; i < sIRCMessage.Info.Length; i++)
							{
								if(!sChannelInfo.SearchFunction(sIRCMessage.Info[i]))
								{
									sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Other("NoSuchFunctions2"), sIRCMessage.Info[i]);
									continue;
								}

								args += ", " + sIRCMessage.Info[i].ToLower();
								SchumixBase.DManager.Update("schumix", string.Format("FunctionStatus = '{0}'", sIRCMessage.Info[5].ToLower()), string.Format("FunctionName = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[i].ToLower())));
								sChannelInfo.FunctionsReload();
							}

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
								sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Other("NoSuchFunctions"));
								return;
							}

							if(sIRCMessage.Info[5].ToLower() == SchumixBase.On)
								sSendMessage.SendChatMessage(sIRCMessage, text[0], sIRCMessage.Info[6].ToLower());
							else
								sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Info[6].ToLower());

							SchumixBase.DManager.Update("schumix", string.Format("FunctionStatus = '{0}'", sIRCMessage.Info[5].ToLower()), string.Format("FunctionName = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower())));
							sChannelInfo.FunctionsReload();
						}
					}
				}
			}
			else if(sIRCMessage.Info[4].ToLower() == "channel")
			{
				var text = sLManager.GetCommandTexts("function/channel", sIRCMessage.Channel);
				if(text.Length < 3)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
					return;
				}

				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel));
					return;
				}

				if(!IsChannel(sIRCMessage.Info[5]))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaChannelHasBeenSet", sIRCMessage.Channel));
					return;
				}

				var db0 = SchumixBase.DManager.QueryFirstRow("SELECT* FROM channel WHERE Channel = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[5].ToLower()));
				if(db0.IsNull())
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[2]);
					return;
				}

				if(sIRCMessage.Info.Length < 7)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("No1Value", sIRCMessage.Channel));
					return;
				}
			
				string channel = sIRCMessage.Info[5].ToLower();
				string status = sIRCMessage.Info[6].ToLower();
			
				if(sIRCMessage.Info[6].ToLower() == "info")
				{
					var text2 = sLManager.GetCommandTexts("function/channel/info", sIRCMessage.Channel);
					if(text2.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
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
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoFunctionName", sIRCMessage.Channel));
						return;
					}

					if(sIRCMessage.Info.Length >= 9)
					{
						string args = string.Empty;

						for(int i = 7; i < sIRCMessage.Info.Length; i++)
						{
							if(!sChannelInfo.SearchChannelFunction(sIRCMessage.Info[i]))
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Other("NoSuchFunctions2"), sIRCMessage.Info[i]);
								continue;
							}

							args += ", " + sIRCMessage.Info[i].ToLower();
							SchumixBase.DManager.Update("channel", string.Format("Functions = '{0}'", sChannelInfo.ChannelFunctions(sIRCMessage.Info[i].ToLower(), status, channel)), string.Format("Channel = '{0}'", channel));
							sChannelInfo.ChannelFunctionsReload();
						}

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
							sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Other("NoSuchFunctions"));
							return;
						}

						if(status == SchumixBase.On)
							sSendMessage.SendChatMessage(sIRCMessage, text[0], sIRCMessage.Info[7].ToLower());
						else
							sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Info[7].ToLower());

						SchumixBase.DManager.Update("channel", string.Format("Functions = '{0}'", sChannelInfo.ChannelFunctions(sIRCMessage.Info[7].ToLower(), status, channel)), string.Format("Channel = '{0}'", channel));
						sChannelInfo.ChannelFunctionsReload();
					}
				}
			}
			else if(sIRCMessage.Info[4].ToLower() == "update")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("function/update", sIRCMessage.Channel), sIRCMessage.Channel);
					SchumixBase.DManager.Update("channel", string.Format("Functions = '{0}'", sUtilities.GetFunctionUpdate()), string.Format("Channel = '{0}'", sIRCMessage.Channel));
					sChannelInfo.ChannelFunctionsReload();
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "all")
				{
					var db = SchumixBase.DManager.Query("SELECT Channel FROM channel");
					if(!db.IsNull())
					{
						foreach(DataRow row in db.Rows)
						{
							string channel = row["Channel"].ToString();
							SchumixBase.DManager.Update("channel", string.Format("Functions = '{0}'", sUtilities.GetFunctionUpdate()), string.Format("Channel = '{0}'", channel));
						}

						sChannelInfo.ChannelFunctionsReload();
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("function/update/all", sIRCMessage.Channel));
					}
					else
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel));
				}
				else
				{
					if(!IsChannel(sIRCMessage.Info[5]))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaChannelHasBeenSet", sIRCMessage.Channel));
						return;
					}

					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("function/update", sIRCMessage.Channel), sIRCMessage.Info[5].ToLower());
					SchumixBase.DManager.Update("channel", string.Format("Functions = '{0}'", sUtilities.GetFunctionUpdate()), string.Format("Channel = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[5].ToLower())));
					sChannelInfo.ChannelFunctionsReload();
				}
			}
			else
			{
				var text = sLManager.GetCommandTexts("function", sIRCMessage.Channel);
				if(text.Length < 2)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
					return;
				}

				if(sIRCMessage.Info.Length < 5)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoFunctionStatus", sIRCMessage.Channel));
					return;
				}

				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoFunctionName", sIRCMessage.Channel));
					return;
				}

				string status = sIRCMessage.Info[4].ToLower();

				if(status == SchumixBase.On || status == SchumixBase.Off)
				{
					if(sIRCMessage.Info.Length >= 7)
					{
						string args = string.Empty;

						for(int i = 5; i < sIRCMessage.Info.Length; i++)
						{
							if(!sChannelInfo.SearchChannelFunction(sIRCMessage.Info[i]))
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Other("NoSuchFunctions2"), sIRCMessage.Info[i]);
								continue;
							}

							args += ", " + sIRCMessage.Info[i].ToLower();
							SchumixBase.DManager.Update("channel", string.Format("Functions = '{0}'", sChannelInfo.ChannelFunctions(sIRCMessage.Info[i].ToLower(), status, sIRCMessage.Channel)), string.Format("Channel = '{0}'", sIRCMessage.Channel));
							sChannelInfo.ChannelFunctionsReload();
						}

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
							sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Other("NoSuchFunctions"));
							return;
						}

						if(status == SchumixBase.On)
							sSendMessage.SendChatMessage(sIRCMessage, text[0], sIRCMessage.Info[5].ToLower());
						else
							sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Info[5].ToLower());

						SchumixBase.DManager.Update("channel", string.Format("Functions = '{0}'", sChannelInfo.ChannelFunctions(sIRCMessage.Info[5].ToLower(), status, sIRCMessage.Channel)), string.Format("Channel = '{0}'", sIRCMessage.Channel));
						sChannelInfo.ChannelFunctionsReload();
					}
				}
			}
		}

		protected void HandleChannel(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
				return;

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("channel", sIRCMessage.Channel));
				return;
			}

			if(sIRCMessage.Info[4].ToLower() == "add")
			{
				var text = sLManager.GetCommandTexts("channel/add", sIRCMessage.Channel);
				if(text.Length < 2)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
					return;
				}

				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel));
					return;
				}

				string channel = sIRCMessage.Info[5].ToLower();

				if(!IsChannel(channel))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaChannelHasBeenSet", sIRCMessage.Channel));
					return;
				}

				if(sChannelInfo.IsIgnore(channel))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ThisChannelBlockedByAdmin", sIRCMessage.Channel));
					return;
				}

				var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM channel WHERE Channel = '{0}'", sUtilities.SqlEscape(channel));
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
					SchumixBase.DManager.Insert("`channel`(Channel, Password, Language)", sUtilities.SqlEscape(channel), sUtilities.SqlEscape(pass), sLManager.Locale);
					SchumixBase.DManager.Update("channel", "Enabled = 'true'", string.Format("Channel = '{0}'", sUtilities.SqlEscape(channel)));
				}
				else
				{
					ChannelPrivmsg = sIRCMessage.Channel;
					sSender.Join(channel);
					SchumixBase.DManager.Insert("`channel`(Channel, Password, Language)", sUtilities.SqlEscape(channel), string.Empty, sLManager.Locale);
					SchumixBase.DManager.Update("channel", "Enabled = 'true'", string.Format("Channel = '{0}'", sUtilities.SqlEscape(channel)));
				}

				sSendMessage.SendChatMessage(sIRCMessage, text[1], channel);
				sChannelInfo.ChannelListReload();
				sChannelInfo.ChannelFunctionsReload();
			}
			else if(sIRCMessage.Info[4].ToLower() == "remove")
			{
				var text = sLManager.GetCommandTexts("channel/remove", sIRCMessage.Channel);
				if(text.Length < 3)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
					return;
				}

				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel));
					return;
				}

				string channel = sIRCMessage.Info[5].ToLower();

				if(!IsChannel(channel))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaChannelHasBeenSet", sIRCMessage.Channel));
					return;
				}

				var db = SchumixBase.DManager.QueryFirstRow("SELECT Id FROM channel WHERE Channel = '{0}'", sUtilities.SqlEscape(channel));
				if(!db.IsNull())
				{
					int id = Convert.ToInt32(db["Id"].ToString());
					if(id == 1)
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}
				}

				db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM channel WHERE Channel = '{0}'", sUtilities.SqlEscape(channel));
				if(db.IsNull())
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
					return;
				}

				sSender.Part(channel);
				SchumixBase.DManager.Delete("channel", string.Format("Channel = '{0}'", sUtilities.SqlEscape(channel)));
				sSendMessage.SendChatMessage(sIRCMessage, text[2], channel);
				sChannelInfo.ChannelListReload();
				sChannelInfo.ChannelFunctionsReload();
			}
			else if(sIRCMessage.Info[4].ToLower() == "update")
			{
				sChannelInfo.ChannelListReload();
				sChannelInfo.ChannelFunctionsReload();
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("channel/update", sIRCMessage.Channel));
			}
			else if(sIRCMessage.Info[4].ToLower() == "info")
			{
				var text = sLManager.GetCommandTexts("channel/info", sIRCMessage.Channel);
				if(text.Length < 4)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
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
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel));
			}
			else if(sIRCMessage.Info[4].ToLower() == "language")
			{
				var text = sLManager.GetCommandTexts("channel/language", sIRCMessage.Channel);
				if(text.Length < 2)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
					return;
				}

				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel));
					return;
				}

				if(!IsChannel(sIRCMessage.Info[5]))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaChannelHasBeenSet", sIRCMessage.Channel));
					return;
				}

				var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM channel WHERE Channel = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[5].ToLower()));
				if(db.IsNull())
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
					return;
				}

				if(sIRCMessage.Info.Length < 7)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoChannelLanguage", sIRCMessage.Channel));
					return;
				}

				SchumixBase.DManager.Update("channel", string.Format("Language = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[6])), string.Format("Channel = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[5].ToLower())));
				sSendMessage.SendChatMessage(sIRCMessage, text[0], sIRCMessage.Info[6]);
			}
			/*else if(sIRCMessage.Info[4].ToLower() == "ignore")
			{
				if(sIRCMessage.Info.Length < 5)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("channel", sIRCMessage.Channel));
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "add")
				{
					/*var text = sLManager.GetCommandTexts("channel/ignore/add", sIRCMessage.Channel);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						return;
					}*/

					/*if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel));
						return;
					}

					string channel = sIRCMessage.Info[6].ToLower();

					var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM channel WHERE Channel = '{0}'", sUtilities.SqlEscape(channel));
					if(!db.IsNull())
					{
						sSendMessage.SendChatMessage(sIRCMessage, /*text[0]*//* "Már szerepel az ignore listán");
						return;
					}
	
					SchumixBase.DManager.Update("channel", "Enabled = 'true'", string.Format("Channel = '{0}'", sUtilities.SqlEscape(channel)));
				}
				else if(sIRCMessage.Info[4].ToLower() == "remove")
				{
					var text = sLManager.GetCommandTexts("channel/remove", sIRCMessage.Channel);
					if(text.Length < 3)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						return;
					}
	
					if(sIRCMessage.Info.Length < 6)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel));
						return;
					}
	
					string channel = sIRCMessage.Info[5].ToLower();
	
					if(!IsChannel(channel))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaChannelHasBeenSet", sIRCMessage.Channel));
						return;
					}
	
					var db = SchumixBase.DManager.QueryFirstRow("SELECT Id FROM channel WHERE Channel = '{0}'", sUtilities.SqlEscape(channel));
					if(!db.IsNull())
					{
						int id = Convert.ToInt32(db["Id"].ToString());
						if(id == 1)
						{
							sSendMessage.SendChatMessage(sIRCMessage, text[0]);
							return;
						}
					}
	
					db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM channel WHERE Channel = '{0}'", sUtilities.SqlEscape(channel));
					if(db.IsNull())
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[1]);
						return;
					}
	
					sSender.Part(channel);
					SchumixBase.DManager.Delete("channel", string.Format("Channel = '{0}'", sUtilities.SqlEscape(channel)));
					sSendMessage.SendChatMessage(sIRCMessage, text[2], channel);
					sChannelInfo.ChannelListReload();
					sChannelInfo.ChannelFunctionsReload();
				}
				else if(sIRCMessage.Info[5].ToLower() == "list")
				{
				}

-- ----------------------------
-- Table structure for ignore_channel
-- ----------------------------
DROP TABLE IF EXISTS `ignore_channel`;
CREATE TABLE `ignore_channel` (
  `Id` int(4) unsigned NOT NULL auto_increment,
  `Channel` varchar(20) NOT NULL default '',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
			}*/
		}

		protected void HandleSznap(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
				return;

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
				return;
			}

			// INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'sznap', '1', 'Kiírja a megadott név születésnapjának dátumát.\nHasználata: {0}sznap <név>');

			var db = SchumixBase.DManager.QueryFirstRow("SELECT nev, honap, nap FROM sznap WHERE nev = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[4]));
			if(!db.IsNull())
			{
				string name = db["nev"].ToString();
				string month = db["honap"].ToString();
				int day = Convert.ToInt32(db["nap"]);
				sSendMessage.SendChatMessage(sIRCMessage, "{0} születés napja: {1} {2}", name, month, day);
			}
			else
				sSendMessage.SendChatMessage(sIRCMessage, "Nincs ilyen ember!");
		}

		protected void HandleKick(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
				return;

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
				return;
			}

			string kick = sIRCMessage.Info[4].ToLower();

			if(sIRCMessage.Info.Length == 5)
			{
				if(kick != sNickInfo.NickStorage.ToLower())
					sSender.Kick(sIRCMessage.Channel, kick);
			}
			else if(sIRCMessage.Info.Length >= 6)
			{
				if(kick != sNickInfo.NickStorage.ToLower())
					sSender.Kick(sIRCMessage.Channel, kick, sIRCMessage.Info.SplitToString(5, SchumixBase.Space));
			}
		}

		protected void HandleMode(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
				return;

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoRank", sIRCMessage.Channel));
				return;
			}

			if(sIRCMessage.Info.Length == 5)
			{
				sSender.Mode(sIRCMessage.Channel, sIRCMessage.Info[4].ToLower());
				return;
			}

			if(sIRCMessage.Info.Length < 6)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
				return;
			}

			string rank = sIRCMessage.Info[4].ToLower();
			string name = sIRCMessage.Info.SplitToString(5, SchumixBase.Space).ToLower();

			if(!name.Contains(sNickInfo.NickStorage.ToLower()))
				sSender.Mode(sIRCMessage.Channel, rank, name);
		}
	}
}