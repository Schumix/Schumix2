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
using System.Xml;
using System.Web;
using System.Data;
using Schumix.Irc;
using Schumix.Irc.Util;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Irc;
using Schumix.Framework.Util;
using Schumix.Framework.Config;
using Schumix.Framework.Functions;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using Schumix.ExtraAddon.Config;
using WolframAPI;

namespace Schumix.ExtraAddon.Commands
{
	partial class Functions : CommandInfo
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private string _servername;
		public bool IsOnline { get; set; }

		public Functions(string ServerName) : base(ServerName)
		{
			_servername = ServerName;
		}

		public void HandleAutoFunction(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.HalfOperator))
				return;

			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;
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
						var db = SchumixBase.DManager.QueryFirstRow("SELECT Enabled FROM hlmessage WHERE Name = '{0}' And ServerName = '{1}'", name, sIRCMessage.ServerName);

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

						SchumixBase.DManager.Update("hlmessage", string.Format("Enabled = '{0}'", status), string.Format("Name = '{0}' And ServerName = '{1}'", name, sIRCMessage.ServerName));

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
					string name = sIRCMessage.Nick.ToLower();
					SchumixBase.DManager.Update("hlmessage", string.Format("Info = '{0}', Enabled = 'on'", sUtilities.SqlEscape(sIRCMessage.Info.SplitToString(5, SchumixBase.Space))), string.Format("Name = '{0}' And ServerName = '{1}'", name, sIRCMessage.ServerName));
					SchumixBase.DManager.Update("schumix", "FunctionStatus = 'on'", string.Format("FunctionName = 'autohl' And ServerName = '{0}'", sIRCMessage.ServerName));
					SchumixBase.DManager.Update("channels", string.Format("Functions = '{0}'", sMyChannelInfo.ChannelFunctions(IChannelFunctions.Autohl, SchumixBase.On, sIRCMessage.Channel)), string.Format("Channel = '{0}' And ServerName = '{1}'", sIRCMessage.Channel.ToLower(), sIRCMessage.ServerName));
					sMyChannelInfo.FunctionsReload();
					sMyChannelInfo.ChannelFunctionsReload();
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("autofunction/hlmessage", sIRCMessage.Channel, sIRCMessage.ServerName));
				}
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

		public void HandleMessage(IRCMessage sIRCMessage)
		{
			var sMyNickInfo = sIrcBase.Networks[sIRCMessage.ServerName].sMyNickInfo;
			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;
			var sMyChannelInfo = sIrcBase.Networks[sIRCMessage.ServerName].sMyChannelInfo;

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("No1Value", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			if(!sMyChannelInfo.FSelect("message") || !sMyChannelInfo.FSelect("message", sIRCMessage.Channel))
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoMessageFunction", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			if(sIRCMessage.Info[4].ToLower() == "channel")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sIRCMessage.Info.Length < 7)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sIRCMessage.Info.Length < 8)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoMessage", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(!Rfc2812Util.IsValidChannelName(sIRCMessage.Info[5]))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaChannelHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(!Rfc2812Util.IsValidNick(sIRCMessage.Info[6]))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaNickNameHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sMyNickInfo.NickStorage.ToLower() == sIRCMessage.Info[6].ToLower())
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ICantLeftAMessageForMyself", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				SchumixBase.DManager.Insert("`message`(ServerId, ServerName, Name, Channel, Message, Wrote, UnixTime)", sIRCMessage.ServerId, sIRCMessage.ServerName, sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[5].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info.SplitToString(7, SchumixBase.Space)), sIRCMessage.Nick, sUtilities.UnixTime);
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("message/channel", sIRCMessage.Channel, sIRCMessage.ServerName));
			}
			else
			{
				if(sIRCMessage.Info.Length < 5)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}
				
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoMessage", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(!Rfc2812Util.IsValidNick(sIRCMessage.Info[4]))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaNickNameHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sMyNickInfo.NickStorage.ToLower() == sIRCMessage.Info[4].ToLower())
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ICantLeftAMessageForMyself", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				SchumixBase.DManager.Insert("`message`(ServerId, ServerName, Name, Channel, Message, Wrote, UnixTime)", sIRCMessage.ServerId, sIRCMessage.ServerName, sUtilities.SqlEscape(sIRCMessage.Info[4].ToLower()), sIRCMessage.Channel.ToLower(), sUtilities.SqlEscape(sIRCMessage.Info.SplitToString(5, SchumixBase.Space)), sIRCMessage.Nick, sUtilities.UnixTime);
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("message", sIRCMessage.Channel, sIRCMessage.ServerName));
			}
		}

		public void HandleWeather(IRCMessage sIRCMessage)
		{
			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;
			var text = sLManager.GetCommandTexts("weather", sIRCMessage.Channel, sIRCMessage.ServerName);
			if(text.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
				return;
			}

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoCityName", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			bool home = false;
			string url = string.Empty;
			string source = string.Empty;

			switch(sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName))
			{
				case "huHU":
					url = "http://hungarian.wunderground.com/cgi-bin/findweather/hdfForecast?query=";
					break;
				case "enUS":
				case "enGB":
					url = "http://www.wunderground.com/cgi-bin/findweather/hdfForecast?query=";
					break;
				default:
					url = "http://www.wunderground.com/cgi-bin/findweather/hdfForecast?query=";
					break;
			}

			if(sIRCMessage.Info[4].ToLower() == "home")
				home = true;

			try
			{
				if(home)
					source = sUtilities.GetUrl(string.Format("{0}{1}", url, WeatherConfig.City));
				else
					source = sUtilities.GetUrl(url, sIRCMessage.Info.SplitToString(4, SchumixBase.Space).Trim());

				string day = string.Empty;
				string night = string.Empty;
				source = source.Replace("\n\t\t", SchumixBase.Space.ToString());

				if(source.Contains("<td class=\"vaT\"><a href=\"\" class=\"iconSwitchMed\">"))
				{
					source = source.Remove(0, source.IndexOf("<td class=\"vaT\"><a href=\"\" class=\"iconSwitchMed\">") + "<td class=\"vaT\"><a href=\"\" class=\"iconSwitchMed\">".Length);
					source = source.Remove(0, source.IndexOf("<td class=\"vaT full\">") + "<td class=\"vaT full\">".Length);

					if(source.Contains(" <? END CHANCE OF PRECIP"))
						day = source.Substring(0, source.IndexOf(" <? END CHANCE OF PRECIP"));
					else
						day = source.Substring(0, source.IndexOf("</td>"));
				}
				else
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[4]);
					return;
				}

				if(source.Contains("<td class=\"vaT\"><a href=\"\" class=\"iconSwitchMed\">"))
				{
					source = source.Remove(0, source.IndexOf("<td class=\"vaT\"><a href=\"\" class=\"iconSwitchMed\">") + "<td class=\"vaT\"><a href=\"\" class=\"iconSwitchMed\">".Length);
					source = source.Remove(0, source.IndexOf("<td class=\"vaT full\">") + "<td class=\"vaT full\">".Length);
					
					if(source.Contains(" <? END CHANCE OF PRECIP"))
						night = source.Substring(0, source.IndexOf(" <? END CHANCE OF PRECIP"));
					else
						night = source.Substring(0, source.IndexOf("</td>"));
				}
				else
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[4]);
					return;
				}

				if(home)
					sSendMessage.SendChatMessage(sIRCMessage, text[0]);
				else
					sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Info.SplitToString(4, SchumixBase.Space).Trim());

				sSendMessage.SendChatMessage(sIRCMessage, text[2], day.Remove(0, 1, SchumixBase.Space));
				sSendMessage.SendChatMessage(sIRCMessage, text[3], night.Remove(0, 1, SchumixBase.Space));
			}
			catch
			{
				sSendMessage.SendChatMessage(sIRCMessage, text[4]);
			}
		}

		public void HandleRoll(IRCMessage sIRCMessage)
		{
			var rand = new Random();
			int number = rand.Next(0, 100);
			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;
			sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("roll", sIRCMessage.Channel, sIRCMessage.ServerName), number);
		}

		public void HandleSha1(IRCMessage sIRCMessage)
		{
			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoValue", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			sSendMessage.SendChatMessage(sIRCMessage, sUtilities.Sha1(sIRCMessage.Info.SplitToString(4, SchumixBase.Space)));
		}

		public void HandleMd5(IRCMessage sIRCMessage)
		{
			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoValue", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			sSendMessage.SendChatMessage(sIRCMessage, sUtilities.Md5(sIRCMessage.Info.SplitToString(4, SchumixBase.Space)));
		}

		public void HandlePrime(IRCMessage sIRCMessage)
		{
			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;
			var text = sLManager.GetCommandTexts("prime", sIRCMessage.Channel, sIRCMessage.ServerName);
			if(text.Length < 3)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
				return;
			}

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoNumber", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			if(!sIRCMessage.Info[4].IsNumber())
			{
				sSendMessage.SendChatMessage(sIRCMessage, text[0]);
				return;
			}

			bool prim = sUtilities.IsPrime(sIRCMessage.Info[4].ToInt32());

			if(!prim)
				sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Info[4]);
			else
				sSendMessage.SendChatMessage(sIRCMessage, text[2], sIRCMessage.Info[4]);
		}

		public void HandleWiki(IRCMessage sIRCMessage)
		{
			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;
			var text = sLManager.GetCommandTexts("wiki", sIRCMessage.Channel, sIRCMessage.ServerName);
			if(text.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
				return;
			}

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoGoogleText", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			string url = sUtilities.GetUrl("http://" + sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName).Substring(0, 2).ToLower()
				+ ".wikipedia.org/w/api.php?action=opensearch&format=xml&search=", sIRCMessage.Info.SplitToString(4, SchumixBase.Space));

			if(url.Contains("<Text xml:space=\"preserve\">"))
			{
				url = url.Replace("&quot;", "\"");
				url = url.Remove(0, url.IndexOf("<Text xml:space=\"preserve\">") + "<Text xml:space=\"preserve\">".Length);
				string _text = url.Substring(0, url.IndexOf("</Text>"));
				url = url.Remove(0, url.IndexOf("<Description xml:space=\"preserve\">") + "<Description xml:space=\"preserve\">".Length);
				string _des = url.Substring(0, url.IndexOf("</Description>"));
				url = url.Remove(0, url.IndexOf("<Url xml:space=\"preserve\">") + "<Url xml:space=\"preserve\">".Length);
				string _url = url.Substring(0, url.IndexOf("</Url>"));

				sSendMessage.SendChatMessage(sIRCMessage, text[0], _text);
				sSendMessage.SendChatMessage(sIRCMessage, text[1], _url);

				if(_des.Length <= 200)
					sSendMessage.SendChatMessage(sIRCMessage, text[2], _des);
				else
					sSendMessage.SendChatMessage(sIRCMessage, text[3], _des.Substring(0, 200));
			}
			else
				sSendMessage.SendChatMessage(sIRCMessage, text[4]);
		}

		public void HandleCalc(IRCMessage sIRCMessage)
		{
			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoValue", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			var client = new WAClient(WolframAlphaConfig.Key);
			var solution = client.Solve(sIRCMessage.Info.SplitToString(4, SchumixBase.Space));
			sSendMessage.SendChatMessage(sIRCMessage, "{0}", solution);
		}
	}
}