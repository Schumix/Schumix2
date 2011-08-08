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
using Schumix.API;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.ExtraAddon.Commands
{
	public partial class Functions : CommandInfo
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly ChannelInfo sChannelInfo = Singleton<ChannelInfo>.Instance;
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;
		private readonly Sender sSender = Singleton<Sender>.Instance;
		private Functions() {}

		public void HandleAutoFunction(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.HalfOperator))
				return;

			CNick(sIRCMessage);

			if(sIRCMessage.Info.Length < 5)
			{
				var text = sLManager.GetCommandTexts("autofunction", sIRCMessage.Channel);
				if(text.Length < 2)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
					return;
				}

				if(IsAdmin(sIRCMessage.Nick, AdminFlag.HalfOperator))
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
				else if(IsAdmin(sIRCMessage.Nick, AdminFlag.Operator))
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1]);
				else if(IsAdmin(sIRCMessage.Nick, AdminFlag.Administrator))
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1]);

				return;
			}

			if(sIRCMessage.Info[4].ToLower() == "hlmessage")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("No1Value", sIRCMessage.Channel));
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "info")
				{
					var db = SchumixBase.DManager.Query("SELECT Name, Enabled FROM hlmessage");
					if(!db.IsNull())
					{
						string names = string.Empty;

						foreach(DataRow row in db.Rows)
						{
							string name = row["Name"].ToString();
							string status = row["Enabled"].ToString();
							names += ", " + name + SchumixBase.Point2 + status;
						}

						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("autofunction/hlmessage/info", sIRCMessage.Channel), names.Remove(0, 2, ", "));
					}
					else
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel));
				}
				else if(sIRCMessage.Info[5].ToLower() == "update")
				{
					var db = SchumixBase.DManager.Query("SELECT Name FROM adminok");
					if(!db.IsNull())
					{
						foreach(DataRow row in db.Rows)
						{
							string name = row["Name"].ToString();

							var db1 = SchumixBase.DManager.QueryFirstRow("SELECT* FROM hlmessage WHERE Name = '{0}'", name);
							if(db1.IsNull())
								SchumixBase.DManager.QueryFirstRow("INSERT INTO `hlmessage`(Name, Enabled) VALUES ('{0}', 'off')", name);
						}

						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("autofunction/hlmessage/update", sIRCMessage.Channel));
					}
					else
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel));
				}
				else if(sIRCMessage.Info[5].ToLower() == "function")
				{
					var text = sLManager.GetCommandTexts("autofunction/hlmessage/function", sIRCMessage.Channel);
					if(text.Length < 2)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoFunctionStatus", sIRCMessage.Channel));
						return;
					}

					string status = sIRCMessage.Info[6].ToLower();
					if(status == "on" || status == "off")
					{
						string name = sIRCMessage.Nick.ToLower();
						SchumixBase.DManager.QueryFirstRow("UPDATE `hlmessage` SET `Enabled` = '{0}' WHERE Name = '{1}'", status, name);
	
						if(status == "on")
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0], name);
						else
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1], name);
					}
				}
				else
				{
					string name = sIRCMessage.Nick.ToLower();
					SchumixBase.DManager.QueryFirstRow("UPDATE `hlmessage` SET `Info` = '{0}', `Enabled` = 'on' WHERE Name = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info.SplitToString(5, SchumixBase.Space)), name);
					SchumixBase.DManager.QueryFirstRow("UPDATE `schumix` SET `FunctionStatus` = 'on' WHERE FunctionName = 'autohl'");
					SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Functions = '{0}' WHERE Channel = '{1}'", sChannelInfo.ChannelFunctions("autohl", "on", sIRCMessage.Channel), sIRCMessage.Channel.ToLower());
					sChannelInfo.ChannelFunctionReload();
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("autofunction/hlmessage", sIRCMessage.Channel));
				}
			}

			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
				return;

			if(sIRCMessage.Info[4].ToLower() == "kick")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("No1Value", sIRCMessage.Channel));
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "add")
				{
					var text = sLManager.GetCommandTexts("autofunction/kick/add", sIRCMessage.Channel);
					if(text.Length < 2)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
						return;
					}

					if(sIRCMessage.Info.Length < 8)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoReason", sIRCMessage.Channel));
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM kicklist WHERE Name = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()));
					if(!db.IsNull())
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
						return;
					}

					SchumixBase.DManager.QueryFirstRow("INSERT INTO `kicklist`(Name, Channel, Reason) VALUES ('{0}', '{1}', '{2}')", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.Channel.ToLower(), sUtilities.SqlEscape(sIRCMessage.Info.SplitToString(7, SchumixBase.Space)));
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1], sIRCMessage.Info[6]);
				}
				else if(sIRCMessage.Info[5].ToLower() == "remove")
				{
					var text = sLManager.GetCommandTexts("autofunction/kick/remove", sIRCMessage.Channel);
					if(text.Length < 2)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM kicklist WHERE Name = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()));
					if(db.IsNull())
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
						return;
					}

					SchumixBase.DManager.QueryFirstRow("DELETE FROM `kicklist` WHERE Name = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()));
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1], sIRCMessage.Info[6]);
				}
				else if(sIRCMessage.Info[5].ToLower() == "info")
				{
					var db = SchumixBase.DManager.Query("SELECT Name FROM kicklist WHERE Channel = '{0}'", sIRCMessage.Channel.ToLower());
					if(!db.IsNull())
					{
						string names = string.Empty;

						foreach(DataRow row in db.Rows)
						{
							string name = row["Name"].ToString();
							names += ", " + name + SchumixBase.Point2 + sIRCMessage.Channel;
						}

						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("autofunction/kick/info", sIRCMessage.Channel), names.Remove(0, 2, ", "));
					}
					else
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel));
				}
				else if(sIRCMessage.Info[5].ToLower() == "channel")
				{
					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("No1Value", sIRCMessage.Channel));
						return;
					}

					if(sIRCMessage.Info[6].ToLower() == "add")
					{
						var text = sLManager.GetCommandTexts("autofunction/kick/channel/add", sIRCMessage.Channel);
						if(text.Length < 2)
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
							return;
						}

						if(sIRCMessage.Info.Length < 8)
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
							return;
						}

						if(sIRCMessage.Info.Length < 9)
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel));
							return;
						}

						if(sIRCMessage.Info.Length < 10)
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoReason", sIRCMessage.Channel));
							return;
						}
	
						var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM kicklist WHERE Name = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[7].ToLower()));
						if(!db.IsNull())
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
							return;
						}

						SchumixBase.DManager.QueryFirstRow("INSERT INTO `kicklist`(Name, Channel, Reason) VALUES ('{0}', '{1}', '{2}')", sUtilities.SqlEscape(sIRCMessage.Info[7].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[8].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info.SplitToString(9, SchumixBase.Space)));
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1], sIRCMessage.Info[7]);
					}
					else if(sIRCMessage.Info[6].ToLower() == "remove")
					{
						var text = sLManager.GetCommandTexts("autofunction/kick/channel/remove", sIRCMessage.Channel);
						if(text.Length < 2)
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
							return;
						}

						if(sIRCMessage.Info.Length < 8)
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
							return;
						}

						var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM kicklist WHERE Name = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[7].ToLower()));
						if(db.IsNull())
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
							return;
						}

						SchumixBase.DManager.QueryFirstRow("DELETE FROM `kicklist` WHERE Name = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[7].ToLower()));
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1], sIRCMessage.Info[7]);
					}
					else if(sIRCMessage.Info[6].ToLower() == "info")
					{
						var db = SchumixBase.DManager.Query("SELECT Name, Channel FROM kicklist");
						if(!db.IsNull())
						{
							string names = string.Empty;

							foreach(DataRow row in db.Rows)
							{
								string name = row["Name"].ToString();
								string channel = row["Channel"].ToString();
								names += ", " + name + SchumixBase.Point2 + channel;
							}

							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("autofunction/kick/channel/info", sIRCMessage.Channel), names.Remove(0, 2, ", "));
						}
						else
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel));
					}
				}
			}
			else if(sIRCMessage.Info[4].ToLower() == "mode")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("No1Value", sIRCMessage.Channel));
					return;
				}
		
				if(sIRCMessage.Info[5].ToLower() == "add")
				{
					var text = sLManager.GetCommandTexts("autofunction/mode/add", sIRCMessage.Channel);
					if(text.Length < 2)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
						return;
					}
		
					if(sIRCMessage.Info.Length < 8)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoRank", sIRCMessage.Channel));
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM modelist WHERE Name = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()));
					if(!db.IsNull())
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
						return;
					}

					SchumixBase.DManager.QueryFirstRow("INSERT INTO `modelist`(Name, Channel, Rank) VALUES ('{0}', '{1}', '{2}')", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.Channel.ToLower(), sUtilities.SqlEscape(sIRCMessage.Info[7].ToLower()));
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1], sIRCMessage.Info[6]);
				}
				else if(sIRCMessage.Info[5].ToLower() == "remove")
				{
					var text = sLManager.GetCommandTexts("autofunction/mode/remove", sIRCMessage.Channel);
					if(text.Length < 2)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM modelist WHERE Name = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()));
					if(db.IsNull())
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
						return;
					}

					SchumixBase.DManager.QueryFirstRow("DELETE FROM `modelist` WHERE Name = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()));
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1], sIRCMessage.Info[6]);
				}
				else if(sIRCMessage.Info[5].ToLower() == "info")
				{
					var db = SchumixBase.DManager.Query("SELECT Name FROM modelist WHERE Channel = '{0}'", sIRCMessage.Channel.ToLower());
					if(!db.IsNull())
					{
						string names = string.Empty;

						foreach(DataRow row in db.Rows)
						{
							string name = row["Name"].ToString();
							names += ", " + name + SchumixBase.Point2 + sIRCMessage.Channel;
						}

						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("autofunction/mode/info", sIRCMessage.Channel), names.Remove(0, 2, ", "));
					}
					else
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel));
				}
				else if(sIRCMessage.Info[5].ToLower() == "channel")
				{
					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("No1Value", sIRCMessage.Channel));
						return;
					}
		
					if(sIRCMessage.Info[6].ToLower() == "add")
					{
						var text = sLManager.GetCommandTexts("autofunction/mode/channel/add", sIRCMessage.Channel);
						if(text.Length < 2)
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
							return;
						}

						if(sIRCMessage.Info.Length < 8)
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
							return;
						}
		
						if(sIRCMessage.Info.Length < 9)
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel));
							return;
						}
		
						if(sIRCMessage.Info.Length < 10)
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoRank", sIRCMessage.Channel));
							return;
						}

						var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM modelist WHERE Name = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[7].ToLower()));
						if(!db.IsNull())
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
							return;
						}

						SchumixBase.DManager.QueryFirstRow("INSERT INTO `modelist`(Name, Channel, Rank) VALUES ('{0}', '{1}', '{2}')", sUtilities.SqlEscape(sIRCMessage.Info[7].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[8].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[9]));
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1], sIRCMessage.Info[7]);
					}
					else if(sIRCMessage.Info[6].ToLower() == "remove")
					{
						var text = sLManager.GetCommandTexts("autofunction/mode/channel/remove", sIRCMessage.Channel);
						if(text.Length < 2)
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
							return;
						}

						if(sIRCMessage.Info.Length < 8)
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
							return;
						}
		
						var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM modelist WHERE Name = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[7].ToLower()));
						if(db.IsNull())
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
							return;
						}

						SchumixBase.DManager.QueryFirstRow("DELETE FROM `modelist` WHERE Name = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[7].ToLower()));
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1], sIRCMessage.Info[7]);
					}
					else if(sIRCMessage.Info[6].ToLower() == "info")
					{
						var db = SchumixBase.DManager.Query("SELECT Name, Channel FROM modelist");
						if(!db.IsNull())
						{
							string names = string.Empty;

							foreach(DataRow row in db.Rows)
							{
								string name = row["Name"].ToString();
								string channel = row["Channel"].ToString();
								names += ", " + name + SchumixBase.Point2 + channel;
							}

							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("autofunction/mode/channel/info", sIRCMessage.Channel), names.Remove(0, 2, ", "));
						}
						else
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel));
					}
				}
			}
		}

		public void HandleMessage(IRCMessage sIRCMessage)
		{
			CNick(sIRCMessage);

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("No1Value", sIRCMessage.Channel));
				return;
			}

			if(!sChannelInfo.FSelect("message") || !sChannelInfo.FSelect("message", sIRCMessage.Channel))
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoMessageFunction", sIRCMessage.Channel));
				return;
			}

			if(sIRCMessage.Info[4].ToLower() == "channel")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel));
					return;
				}

				if(sIRCMessage.Info.Length < 7)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
					return;
				}

				if(sIRCMessage.Info.Length < 8)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoMessage", sIRCMessage.Channel));
					return;
				}

				SchumixBase.DManager.QueryFirstRow("INSERT INTO `message`(Name, Channel, Message, Wrote) VALUES ('{0}', '{1}', '{2}', '{3}')", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[5].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info.SplitToString(7, SchumixBase.Space)), sIRCMessage.Nick);
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("message/channel", sIRCMessage.Channel));
			}
			else
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoMessage", sIRCMessage.Channel));
					return;
				}

				SchumixBase.DManager.QueryFirstRow("INSERT INTO `message`(Name, Channel, Message, Wrote) VALUES ('{0}', '{1}', '{2}', '{3}')", sUtilities.SqlEscape(sIRCMessage.Info[4].ToLower()), sIRCMessage.Channel.ToLower(), sUtilities.SqlEscape(sIRCMessage.Info.SplitToString(5, SchumixBase.Space)), sIRCMessage.Nick);
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("message", sIRCMessage.Channel));
			}
		}
	}

	public sealed class Notes : CommandInfo
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private Notes() {}

		public void HandleNotes(IRCMessage sIRCMessage)
		{
			CNick(sIRCMessage);

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoValue", sIRCMessage.Channel));
				return;
			}

			if(sIRCMessage.Info[4].ToLower() == "info")
			{
				if(!Warning(sIRCMessage))
					return;

				if(!IsUser(sIRCMessage.Nick, sIRCMessage.Host))
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoDataNoCommand", sIRCMessage.Channel));
					return;
				}

				var db = SchumixBase.DManager.Query("SELECT Code FROM notes");
				if(!db.IsNull())
				{
					string codes = string.Empty;

					foreach(DataRow row in db.Rows)
					{
						string code = row["Code"].ToString();
						codes += ", " + code;
					}

					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("notes/info", sIRCMessage.Channel), codes.Remove(0, 2, ", "));
				}
				else
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel));
			}
			else if(sIRCMessage.Info[4].ToLower() == "user")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoValue", sIRCMessage.Channel));
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "access")
				{
					var text = sLManager.GetCommandTexts("notes/user/access", sIRCMessage.Channel);
					if(text.Length < 2)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoPassword", sIRCMessage.Channel));
						return;
					}

					string name = sIRCMessage.Nick;
					var db = SchumixBase.DManager.QueryFirstRow("SELECT Password FROM notes_users WHERE Name = '{0}'", name.ToLower());
					if(!db.IsNull())
					{
						if(db["Password"].ToString() == sUtilities.Sha1(sIRCMessage.Info[6]))
						{
							SchumixBase.DManager.QueryFirstRow("UPDATE notes_users SET Vhost = '{0}' WHERE Name = '{1}'", sIRCMessage.Host, name.ToLower());
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
						}
						else
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1]);
					}
				}
				else if(sIRCMessage.Info[5].ToLower() == "newpassword")
				{
					var text = sLManager.GetCommandTexts("notes/user/newpassword", sIRCMessage.Channel);
					if(text.Length < 2)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoOldPassword", sIRCMessage.Channel));
						return;
					}

					if(sIRCMessage.Info.Length < 8)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoNewPassword", sIRCMessage.Channel));
						return;
					}

					string name = sIRCMessage.Nick;
					var db = SchumixBase.DManager.QueryFirstRow("SELECT Password FROM notes_users WHERE Name = '{0}'", name.ToLower());
					if(!db.IsNull())
					{
						if(db["Password"].ToString() == sUtilities.Sha1(sIRCMessage.Info[6]))
						{
							SchumixBase.DManager.QueryFirstRow("UPDATE notes_users SET Password = '{0}' WHERE Name = '{1}'", sUtilities.Sha1(sIRCMessage.Info[7]), name.ToLower());
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0], sIRCMessage.Info[7]);
						}
						else
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1]);
					}
				}
				else if(sIRCMessage.Info[5].ToLower() == "register")
				{
					var text = sLManager.GetCommandTexts("notes/user/register", sIRCMessage.Channel);
					if(text.Length < 2)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoPassword", sIRCMessage.Channel));
						return;
					}

					string name = sIRCMessage.Nick;
					var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM notes_users WHERE Name = '{0}'", name.ToLower());
					if(!db.IsNull())
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
						return;
					}

					string pass = sIRCMessage.Info[6];
					SchumixBase.DManager.QueryFirstRow("INSERT INTO `notes_users`(Name, Password, Vhost) VALUES ('{0}', '{1}', '{2}')", name.ToLower(), sUtilities.Sha1(pass), sIRCMessage.Host);
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1]);
				}
				else if(sIRCMessage.Info[5].ToLower() == "remove")
				{
					var text = sLManager.GetCommandTexts("notes/user/remove", sIRCMessage.Channel);
					if(text.Length < 5)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
						return;
					}

					string name = sIRCMessage.Nick;
					var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM notes_users WHERE Name = '{0}'", name.ToLower());
					if(db.IsNull())
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1]);
						return;
					}

					db = SchumixBase.DManager.QueryFirstRow("SELECT Password FROM notes_users WHERE Name = '{0}'", name.ToLower());
					if(!db.IsNull())
					{
						if(db["Password"].ToString() != sUtilities.Sha1(sIRCMessage.Info[6]))
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[2]);
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[3]);
							return;
						}
					}

					SchumixBase.DManager.QueryFirstRow("DELETE FROM `notes_users` WHERE Name = '{0}'", name.ToLower());
					SchumixBase.DManager.QueryFirstRow("DELETE FROM `notes` WHERE Name = '{0}'", name.ToLower());
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[4]);
				}
			}
			else if(sIRCMessage.Info[4].ToLower() == "code")
			{
				if(!Warning(sIRCMessage))
					return;

				if(!IsUser(sIRCMessage.Nick, sIRCMessage.Host))
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoDataNoCommand", sIRCMessage.Channel));
					return;
				}

				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoCode", sIRCMessage.Channel));
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "remove")
				{
					var text = sLManager.GetCommandTexts("notes/code/remove", sIRCMessage.Channel);
					if(text.Length < 2)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoCode", sIRCMessage.Channel));
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM notes WHERE Code = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()));
					if(db.IsNull())
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
						return;
					}

					SchumixBase.DManager.QueryFirstRow("DELETE FROM `notes` WHERE Code = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()));
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1]);
				}
				else
				{
					var db = SchumixBase.DManager.QueryFirstRow("SELECT Note FROM notes WHERE Code = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[5].ToLower()));
					if(!db.IsNull())
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("notes/code", sIRCMessage.Channel), db["Note"].ToString());
					else
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel));
				}
			}
			else
			{
				var text = sLManager.GetCommandTexts("notes", sIRCMessage.Channel);
				if(text.Length < 3)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
					return;
				}

				if(!Warning(sIRCMessage))
					return;

				if(!IsUser(sIRCMessage.Nick, sIRCMessage.Host))
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoDataNoCommand", sIRCMessage.Channel));
					return;
				}

				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
					return;
				}

				string code = sIRCMessage.Info[4];
				var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM notes WHERE Code = '{0}'", sUtilities.SqlEscape(code.ToLower()));
				if(!db.IsNull())
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1]);
					return;
				}

				SchumixBase.DManager.QueryFirstRow("INSERT INTO `notes`(Code, Name, Note) VALUES ('{0}', '{1}', '{2}')", sUtilities.SqlEscape(code.ToLower()), sIRCMessage.Nick.ToLower(), sUtilities.SqlEscape(sIRCMessage.Info.SplitToString(5, SchumixBase.Space)));
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[2], code);
			}
		}

		private bool IsUser(string Name)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT * FROM notes_users WHERE Name = '{0}'", Name.ToLower());
			if(!db.IsNull())
				return true;

			return false;
		}

		private bool IsUser(string Name, string Vhost)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Vhost FROM notes_users WHERE Name = '{0}'", Name.ToLower());
			if(!db.IsNull())
			{
				string vhost = db["Vhost"].ToString();

				if(Vhost != vhost)
					return false;

				return true;
			}

			return false;
		}

		private bool Warning(IRCMessage sIRCMessage)
		{
			if(!IsUser(sIRCMessage.Nick))
			{
				var text = sLManager.GetCommandTexts("notes/warning", sIRCMessage.Channel);
				if(text.Length < 4)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
					return false;
				}

				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1]);
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[2], IRCConfig.CommandPrefix);
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[3], IRCConfig.CommandPrefix);
				return false;
			}

			return true;
		}
	}
}