/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013-2014 Schumix Team <http://schumix.eu/>
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
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Irc;
using Schumix.Framework.Functions;
using Schumix.Framework.Extensions;

namespace Schumix.ExtraAddon.Commands
{
	partial class Functions
	{
		public void HandleAutoFunction(IRCMessage sIRCMessage)
		{
			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;

			if(IsWarningAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.HalfOperator))
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("WarningAdmin", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.HalfOperator))
				return;

			var sMyChannelInfo = sIrcBase.Networks[sIRCMessage.ServerName].sMyChannelInfo;
			var sSender = sIrcBase.Networks[sIRCMessage.ServerName].sSender;

			if(sIRCMessage.Info.Length < 5)
			{
				var text = sLManager.GetCommandTexts("autofunction", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 2)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}

				if(IsAdmin(sIRCMessage.Nick, AdminFlag.HalfOperator))
					sSendMessage.SendChatMessage(sIRCMessage, text[0]);
				else if(IsAdmin(sIRCMessage.Nick, AdminFlag.Operator))
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				else if(IsAdmin(sIRCMessage.Nick, AdminFlag.Administrator))
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);

				return;
			}

			if(sIRCMessage.Info[4].ToLower() == "hlmessage")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("No1Value", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "info")
				{
					var db = SchumixBase.DManager.Query("SELECT Name, Enabled FROM hlmessage WHERE ServerName = '{0}'", sIRCMessage.ServerName);
					if(!db.IsNull())
					{
						string names = string.Empty;

						foreach(DataRow row in db.Rows)
						{
							string name = row["Name"].ToString();
							string status = row["Enabled"].ToString();
							names += ", " + name + SchumixBase.Colon + status;
						}

						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("autofunction/hlmessage/info", sIRCMessage.Channel, sIRCMessage.ServerName), names.Remove(0, 2, ", "));
					}
					else
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel, sIRCMessage.ServerName));
				}
				else if(sIRCMessage.Info[5].ToLower() == "update")
				{
					var db = SchumixBase.DManager.Query("SELECT Name FROM admins WHERE ServerName = '{0}'", sIRCMessage.ServerName);
					if(!db.IsNull())
					{
						foreach(DataRow row in db.Rows)
						{
							string name = row["Name"].ToString();

							var db1 = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM hlmessage WHERE Name = '{0}' And ServerName = '{1}'", name, sIRCMessage.ServerName);
							if(db1.IsNull())
								SchumixBase.DManager.Insert("`hlmessage`(ServerId, ServerName, Name, Enabled)", sIRCMessage.ServerId, sIRCMessage.ServerName, name, SchumixBase.Off);
						}

						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("autofunction/hlmessage/update", sIRCMessage.Channel, sIRCMessage.ServerName));
					}
					else
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel, sIRCMessage.ServerName));
				}
				else if(sIRCMessage.Info[5].ToLower() == "function")
				{
					var text = sLManager.GetCommandTexts("autofunction/hlmessage/function", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoFunctionStatus", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					string status = sIRCMessage.Info[6].ToLower();
					if(status == SchumixBase.On || status == SchumixBase.Off)
					{
						bool enabled = false;
						string name = sIRCMessage.Nick.ToLower();
						var db = SchumixBase.DManager.QueryFirstRow("SELECT Enabled FROM hlmessage WHERE Name = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(name), sIRCMessage.ServerName);

						if(!db.IsNull())
							enabled = db["Enabled"].ToString().ToLower() == SchumixBase.On;
						else
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						if(enabled && status == SchumixBase.On)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FunctionAlreadyTurnedOn", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}
						else if(!enabled && status == SchumixBase.Off)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FunctionAlreadyTurnedOff", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						SchumixBase.DManager.Update("hlmessage", string.Format("Enabled = '{0}'", status), string.Format("Name = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(name), sIRCMessage.ServerName));

						if(status == SchumixBase.On)
							sSendMessage.SendChatMessage(sIRCMessage, text[0], name);
						else
							sSendMessage.SendChatMessage(sIRCMessage, text[1], name);
					}
					else
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("WrongSwitch", sIRCMessage.Channel, sIRCMessage.ServerName));
				}
				else
				{
					SchumixBase.DManager.Update("hlmessage", string.Format("Info = '{0}', Enabled = 'on'", sUtilities.SqlEscape(sIRCMessage.Info.SplitToString(5, SchumixBase.Space))), string.Format("Name = '{0}' And ServerName = '{1}'", sIRCMessage.SqlEscapeNick.ToLower(), sIRCMessage.ServerName));
					SchumixBase.DManager.Update("schumix", "FunctionStatus = 'on'", string.Format("FunctionName = 'autohl' And ServerName = '{0}'", sIRCMessage.ServerName));
					SchumixBase.DManager.Update("channels", string.Format("Functions = '{0}'", sMyChannelInfo.ChannelFunctions(IChannelFunctions.Autohl, SchumixBase.On, sIRCMessage.Channel)), string.Format("Channel = '{0}' And ServerName = '{1}'", sIRCMessage.Channel.ToLower(), sIRCMessage.ServerName));
					sMyChannelInfo.FunctionsReload();
					sMyChannelInfo.ChannelFunctionsReload();
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("autofunction/hlmessage", sIRCMessage.Channel, sIRCMessage.ServerName));
				}
			}

			if(IsWarningAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("WarningAdmin", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
				return;

			if(sIRCMessage.Info[4].ToLower() == "kick")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("No1Value", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "add")
				{
					var text = sLManager.GetCommandTexts("autofunction/kick/add", sIRCMessage.Channel, sIRCMessage.ServerName);
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

					if(sIRCMessage.Info.Length < 8)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoReason", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(!Rfc2812Util.IsValidNick(sIRCMessage.Info[6]))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaNickNameHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM kicklist WHERE Name = '{0}' AND Channel = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.Channel.ToLower(), sIRCMessage.ServerName);
					if(!db.IsNull())
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}

					SchumixBase.DManager.Insert("`kicklist`(ServerId, ServerName, Name, Channel, Reason)", sIRCMessage.ServerId, sIRCMessage.ServerName, sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.Channel.ToLower(), sUtilities.SqlEscape(sIRCMessage.Info.SplitToString(7, SchumixBase.Space)));
					sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Info[6]);
				}
				else if(sIRCMessage.Info[5].ToLower() == "remove")
				{
					var text = sLManager.GetCommandTexts("autofunction/kick/remove", sIRCMessage.Channel, sIRCMessage.ServerName);
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

					if(!Rfc2812Util.IsValidNick(sIRCMessage.Info[6]))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaNickNameHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM kicklist WHERE Name = '{0}' AND Channel = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.Channel.ToLower(), sIRCMessage.ServerName);
					if(db.IsNull())
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}

					SchumixBase.DManager.Delete("kicklist", string.Format("Name = '{0}' AND Channel = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.Channel.ToLower(), sIRCMessage.ServerName));
					sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Info[6]);
				}
				else if(sIRCMessage.Info[5].ToLower() == "list")
				{
					var db = SchumixBase.DManager.Query("SELECT Name FROM kicklist WHERE Channel = '{0}' And ServerName = '{1}'", sIRCMessage.Channel.ToLower(), sIRCMessage.ServerName);
					if(!db.IsNull())
					{
						string names = string.Empty;

						foreach(DataRow row in db.Rows)
						{
							string name = row["Name"].ToString();
							names += ", " + name;
						}

						if(names.IsNullOrEmpty())
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("autofunction/kick/list", sIRCMessage.Channel, sIRCMessage.ServerName), sLConsole.Other("Nobody"));
						else
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("autofunction/kick/list", sIRCMessage.Channel, sIRCMessage.ServerName), names.Remove(0, 2, ", "));
					}
					else
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel, sIRCMessage.ServerName));
				}
				else if(sIRCMessage.Info[5].ToLower() == "channel")
				{
					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(sIRCMessage.Info.Length < 8)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("No1Value", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(sIRCMessage.Info[7].ToLower() == "add")
					{
						var text = sLManager.GetCommandTexts("autofunction/kick/channel/add", sIRCMessage.Channel, sIRCMessage.ServerName);
						if(text.Length < 2)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
							return;
						}

						if(sIRCMessage.Info.Length < 9)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						if(sIRCMessage.Info.Length < 10)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoReason", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						if(!Rfc2812Util.IsValidNick(sIRCMessage.Info[8]))
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaNickNameHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM kicklist WHERE Name = '{0}' AND Channel = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(sIRCMessage.Info[8].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName);
						if(!db.IsNull())
						{
							sSendMessage.SendChatMessage(sIRCMessage, text[0]);
							return;
						}

						SchumixBase.DManager.Insert("`kicklist`(ServerId, ServerName, Name, Channel, Reason)", sIRCMessage.ServerId, sIRCMessage.ServerName, sUtilities.SqlEscape(sIRCMessage.Info[8].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info.SplitToString(9, SchumixBase.Space)));
						sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Info[8]);
					}
					else if(sIRCMessage.Info[7].ToLower() == "remove")
					{
						var text = sLManager.GetCommandTexts("autofunction/kick/channel/remove", sIRCMessage.Channel, sIRCMessage.ServerName);
						if(text.Length < 2)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
							return;
						}

						if(sIRCMessage.Info.Length < 9)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						if(!Rfc2812Util.IsValidNick(sIRCMessage.Info[8]))
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaNickNameHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM kicklist WHERE Name = '{0}' AND Channel = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(sIRCMessage.Info[8].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName);
						if(db.IsNull())
						{
							sSendMessage.SendChatMessage(sIRCMessage, text[0]);
							return;
						}

						SchumixBase.DManager.Delete("kicklist", string.Format("Name = '{0}' AND Channel = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(sIRCMessage.Info[8].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName));
						sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Info[8]);
					}
					else if(sIRCMessage.Info[7].ToLower() == "list")
					{
						var text = sLManager.GetCommandTexts("autofunction/kick/channel/list", sIRCMessage.Channel, sIRCMessage.ServerName);
						if(text.Length < 2)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
							return;
						}

						if(!Rfc2812Util.IsValidChannelName(sIRCMessage.Info[6].ToLower()))
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaChannelHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						var db0 = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM kicklist WHERE Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName);
						if(db0.IsNull())
						{
							sSendMessage.SendChatMessage(sIRCMessage, text[1]);
							return;
						}

						var db = SchumixBase.DManager.Query("SELECT Name FROM kicklist WHERE Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName);
						if(!db.IsNull())
						{
							string names = string.Empty;

							foreach(DataRow row in db.Rows)
							{
								string name = row["Name"].ToString();
								names += ", " + name;
							}

							if(names.IsNullOrEmpty())
								sSendMessage.SendChatMessage(sIRCMessage, text[0], sLConsole.Other("Nobody"));
							else
								sSendMessage.SendChatMessage(sIRCMessage, text[0], names.Remove(0, 2, ", "));
						}
						else
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel, sIRCMessage.ServerName));
					}
				}
			}
			else if(sIRCMessage.Info[4].ToLower() == "mode")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("No1Value", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "add")
				{
					var text = sLManager.GetCommandTexts("autofunction/mode/add", sIRCMessage.Channel, sIRCMessage.ServerName);
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

					if(sIRCMessage.Info.Length < 8)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoRank", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(!Rfc2812Util.IsValidNick(sIRCMessage.Info[6]))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaNickNameHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM modelist WHERE Name = '{0}' AND Channel = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.Channel.ToLower(), sIRCMessage.ServerName);
					if(!db.IsNull())
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}

					SchumixBase.DManager.Insert("`modelist`(ServerId, ServerName, Name, Channel, Rank)", sIRCMessage.ServerId, sIRCMessage.ServerName, sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.Channel.ToLower(), sUtilities.SqlEscape(sIRCMessage.Info[7].ToLower()));
					sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Info[6]);
				}
				else if(sIRCMessage.Info[5].ToLower() == "change")
				{
					var text = sLManager.GetCommandTexts("autofunction/mode/change", sIRCMessage.Channel, sIRCMessage.ServerName);
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

					if(sIRCMessage.Info.Length < 8)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoRank", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(!Rfc2812Util.IsValidNick(sIRCMessage.Info[6]))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaNickNameHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM modelist WHERE Name = '{0}' AND Channel = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.Channel.ToLower(), sIRCMessage.ServerName);
					if(db.IsNull())
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}

					SchumixBase.DManager.Update("modelist", string.Format("Rank = {0}", sUtilities.SqlEscape(sIRCMessage.Info[7].ToLower())), string.Format("Name = '{0}' AND Channel = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.Channel.ToLower(), sIRCMessage.ServerName));
					sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Info[6], sIRCMessage.Info[7].ToLower());
				}
				else if(sIRCMessage.Info[5].ToLower() == "remove")
				{
					var text = sLManager.GetCommandTexts("autofunction/mode/remove", sIRCMessage.Channel, sIRCMessage.ServerName);
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

					if(!Rfc2812Util.IsValidNick(sIRCMessage.Info[6]))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaNickNameHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT Rank FROM modelist WHERE Name = '{0}' AND Channel = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.Channel.ToLower(), sIRCMessage.ServerName);
					if(db.IsNull())
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}
					else
					{
						string rank = db["Rank"].ToString();
						if(rank.Substring(0, 1) == Rfc2812Util.ModeActionToChar(ModeAction.Add).ToString())
							sSender.Mode(sIRCMessage.Channel.ToLower(), Rfc2812Util.ModeActionToChar(ModeAction.Remove) + rank.Remove(0, 1, Rfc2812Util.ModeActionToChar(ModeAction.Add)), sIRCMessage.Info[6].ToLower());	
					}

					SchumixBase.DManager.Delete("modelist", string.Format("Name = '{0}' AND Channel = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.Channel.ToLower(), sIRCMessage.ServerName));
					sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Info[6]);
				}
				else if(sIRCMessage.Info[5].ToLower() == "list")
				{
					var text = sLManager.GetCommandTexts("autofunction/mode/list", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 5)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					var db = SchumixBase.DManager.Query("SELECT Name, Rank FROM modelist WHERE Channel = '{0}' And ServerName = '{1}'", sIRCMessage.Channel.ToLower(), sIRCMessage.ServerName);
					if(!db.IsNull())
					{
						string voices = string.Empty;
						string halfoperators = string.Empty;
						string operators = string.Empty;
						string admins = string.Empty;
						string owners = string.Empty;

						foreach(DataRow row in db.Rows)
						{
							string rank = row["Rank"].ToString().Length > 1 ? row["Rank"].ToString().Remove(0, 1) : row["Rank"].ToString();

							if(rank.Length >= 1 && rank.Substring(0, 1) == Rfc2812Util.ChannelModeToChar(ChannelMode.Voice).ToString())
							{
								voices += ", " + row["Name"].ToString();
								continue;
							}
							else if(rank.Length >= 1 && rank.Substring(0, 1) == Rfc2812Util.ChannelModeToChar(ChannelMode.HalfChannelOperator).ToString())
							{
								halfoperators += ", " + row["Name"].ToString();
								continue;
							}
							else if(rank.Length >= 1 && rank.Substring(0, 1) == Rfc2812Util.ChannelModeToChar(ChannelMode.ChannelOperator).ToString())
							{
								operators += ", " + row["Name"].ToString();
								continue;
							}
							else if(rank.Length >= 1 && rank.Substring(0, 1) == "a")
							{
								admins += ", " + row["Name"].ToString();
								continue;
							}
							else if(rank.Length >= 1 && rank.Substring(0, 1) == "q")
							{
								owners += ", " + row["Name"].ToString();
								continue;
							}
						}

						if(voices.IsNullOrEmpty())
							sSendMessage.SendChatMessage(sIRCMessage, text[0], sLConsole.Other("Nobody"));
						else
							sSendMessage.SendChatMessage(sIRCMessage, text[0], voices.Remove(0, 2, ", "));

						if(halfoperators.IsNullOrEmpty())
							sSendMessage.SendChatMessage(sIRCMessage, text[1], sLConsole.Other("Nobody"));
						else
							sSendMessage.SendChatMessage(sIRCMessage, text[1], halfoperators.Remove(0, 2, ", "));

						if(operators.IsNullOrEmpty())
							sSendMessage.SendChatMessage(sIRCMessage, text[2], sLConsole.Other("Nobody"));
						else
							sSendMessage.SendChatMessage(sIRCMessage, text[2], operators.Remove(0, 2, ", "));

						if(admins.IsNullOrEmpty())
							sSendMessage.SendChatMessage(sIRCMessage, text[3], sLConsole.Other("Nobody"));
						else
							sSendMessage.SendChatMessage(sIRCMessage, text[3], admins.Remove(0, 2, ", "));

						if(owners.IsNullOrEmpty())
							sSendMessage.SendChatMessage(sIRCMessage, text[4], sLConsole.Other("Nobody"));
						else
							sSendMessage.SendChatMessage(sIRCMessage, text[4], owners.Remove(0, 2, ", "));
					}
					else
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel, sIRCMessage.ServerName));
				}
				else if(sIRCMessage.Info[5].ToLower() == "channel")
				{
					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(sIRCMessage.Info.Length < 8)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("No1Value", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(sIRCMessage.Info[7].ToLower() == "add")
					{
						var text = sLManager.GetCommandTexts("autofunction/mode/channel/add", sIRCMessage.Channel, sIRCMessage.ServerName);
						if(text.Length < 2)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
							return;
						}

						if(sIRCMessage.Info.Length < 9)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						if(sIRCMessage.Info.Length < 10)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoRank", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						if(!Rfc2812Util.IsValidNick(sIRCMessage.Info[8]))
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaNickNameHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM modelist WHERE Name = '{0}' AND Channel = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(sIRCMessage.Info[8].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName);
						if(!db.IsNull())
						{
							sSendMessage.SendChatMessage(sIRCMessage, text[0]);
							return;
						}

						SchumixBase.DManager.Insert("`modelist`(ServerId, ServerName, Name, Channel, Rank)", sIRCMessage.ServerId, sIRCMessage.ServerName, sUtilities.SqlEscape(sIRCMessage.Info[8].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[9]));
						sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Info[8]);
					}
					else if(sIRCMessage.Info[7].ToLower() == "change")
					{
						var text = sLManager.GetCommandTexts("autofunction/mode/channel/change", sIRCMessage.Channel, sIRCMessage.ServerName);
						if(text.Length < 2)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
							return;
						}

						if(sIRCMessage.Info.Length < 9)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						if(sIRCMessage.Info.Length < 10)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoRank", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						if(!Rfc2812Util.IsValidNick(sIRCMessage.Info[8]))
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaNickNameHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM modelist WHERE Name = '{0}' AND Channel = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(sIRCMessage.Info[8].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName);
						if(db.IsNull())
						{
							sSendMessage.SendChatMessage(sIRCMessage, text[0]);
							return;
						}

						SchumixBase.DManager.Update("modelist", string.Format("Rank = {0}", sUtilities.SqlEscape(sIRCMessage.Info[9].ToLower())), string.Format("Name = '{0}' AND Channel = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(sIRCMessage.Info[8].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName));
						sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Info[8], sIRCMessage.Info[9].ToLower());
					}
					else if(sIRCMessage.Info[7].ToLower() == "remove")
					{
						var text = sLManager.GetCommandTexts("autofunction/mode/channel/remove", sIRCMessage.Channel, sIRCMessage.ServerName);
						if(text.Length < 2)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
							return;
						}

						if(sIRCMessage.Info.Length < 9)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						if(!Rfc2812Util.IsValidNick(sIRCMessage.Info[8]))
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaNickNameHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						var db = SchumixBase.DManager.QueryFirstRow("SELECT Rank FROM modelist WHERE Name = '{0}' AND Channel = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(sIRCMessage.Info[8].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName);
						if(db.IsNull())
						{
							sSendMessage.SendChatMessage(sIRCMessage, text[0]);
							return;
						}
						else
						{
							string rank = db["Rank"].ToString();
							if(rank.Substring(0, 1) == Rfc2812Util.ModeActionToChar(ModeAction.Add).ToString())
								sSender.Mode(sIRCMessage.Info[6].ToLower(), Rfc2812Util.ModeActionToChar(ModeAction.Remove) + rank.Remove(0, 1, Rfc2812Util.ModeActionToChar(ModeAction.Add)), sIRCMessage.Info[8].ToLower());	
						}

						SchumixBase.DManager.Delete("modelist", string.Format("Name = '{0}' AND Channel = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(sIRCMessage.Info[8].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName));
						sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Info[8]);
					}
					else if(sIRCMessage.Info[7].ToLower() == "list")
					{
						var text = sLManager.GetCommandTexts("autofunction/mode/channel/list", sIRCMessage.Channel, sIRCMessage.ServerName);
						if(text.Length < 6)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
							return;
						}

						if(!Rfc2812Util.IsValidChannelName(sIRCMessage.Info[6].ToLower()))
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaChannelHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						var db0 = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM modelist WHERE Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName);
						if(db0.IsNull())
						{
							sSendMessage.SendChatMessage(sIRCMessage, text[5]);
							return;
						}

						var db = SchumixBase.DManager.Query("SELECT Name, Rank FROM modelist WHERE Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName);
						if(!db.IsNull())
						{
							string voices = string.Empty;
							string halfoperators = string.Empty;
							string operators = string.Empty;
							string admins = string.Empty;
							string owners = string.Empty;

							foreach(DataRow row in db.Rows)
							{
								string rank = row["Rank"].ToString().Length > 1 ? row["Rank"].ToString().Remove(0, 1) : row["Rank"].ToString();

								if(rank.Length >= 1 && rank.Substring(0, 1) == Rfc2812Util.ChannelModeToChar(ChannelMode.Voice).ToString())
								{
									voices += ", " + row["Name"].ToString();
									continue;
								}
								else if(rank.Length >= 1 && rank.Substring(0, 1) == Rfc2812Util.ChannelModeToChar(ChannelMode.HalfChannelOperator).ToString())
								{
									halfoperators += ", " + row["Name"].ToString();
									continue;
								}
								else if(rank.Length >= 1 && rank.Substring(0, 1) == Rfc2812Util.ChannelModeToChar(ChannelMode.ChannelOperator).ToString())
								{
									operators += ", " + row["Name"].ToString();
									continue;
								}
								else if(rank.Length >= 1 && rank.Substring(0, 1) == "a")
								{
									admins += ", " + row["Name"].ToString();
									continue;
								}
								else if(rank.Length >= 1 && rank.Substring(0, 1) == "q")
								{
									owners += ", " + row["Name"].ToString();
									continue;
								}
							}

							if(voices.IsNullOrEmpty())
								sSendMessage.SendChatMessage(sIRCMessage, text[0], sLConsole.Other("Nobody"));
							else
								sSendMessage.SendChatMessage(sIRCMessage, text[0], voices.Remove(0, 2, ", "));

							if(halfoperators.IsNullOrEmpty())
								sSendMessage.SendChatMessage(sIRCMessage, text[1], sLConsole.Other("Nobody"));
							else
								sSendMessage.SendChatMessage(sIRCMessage, text[1], halfoperators.Remove(0, 2, ", "));

							if(operators.IsNullOrEmpty())
								sSendMessage.SendChatMessage(sIRCMessage, text[2], sLConsole.Other("Nobody"));
							else
								sSendMessage.SendChatMessage(sIRCMessage, text[2], operators.Remove(0, 2, ", "));

							if(admins.IsNullOrEmpty())
								sSendMessage.SendChatMessage(sIRCMessage, text[3], sLConsole.Other("Nobody"));
							else
								sSendMessage.SendChatMessage(sIRCMessage, text[3], admins.Remove(0, 2, ", "));

							if(owners.IsNullOrEmpty())
								sSendMessage.SendChatMessage(sIRCMessage, text[4], sLConsole.Other("Nobody"));
							else
								sSendMessage.SendChatMessage(sIRCMessage, text[4], owners.Remove(0, 2, ", "));
						}
						else
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel, sIRCMessage.ServerName));
					}
				}
			}
		}
	}
}