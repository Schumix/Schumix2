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
using Schumix.ExtraAddon.Config;

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

			if(sIRCMessage.Info.Length < 5)
			{
				var text = sLManager.GetCommandTexts("autofunction", sIRCMessage.Channel);
				if(text.Length < 2)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
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
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("No1Value", sIRCMessage.Channel));
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

						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("autofunction/hlmessage/info", sIRCMessage.Channel), names.Remove(0, 2, ", "));
					}
					else
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel));
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
								SchumixBase.DManager.Insert("`hlmessage`(Name, Enabled)", name, "off");
						}

						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("autofunction/hlmessage/update", sIRCMessage.Channel));
					}
					else
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel));
				}
				else if(sIRCMessage.Info[5].ToLower() == "function")
				{
					var text = sLManager.GetCommandTexts("autofunction/hlmessage/function", sIRCMessage.Channel);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoFunctionStatus", sIRCMessage.Channel));
						return;
					}

					string status = sIRCMessage.Info[6].ToLower();
					if(status == "on" || status == "off")
					{
						string name = sIRCMessage.Nick.ToLower();
						SchumixBase.DManager.Update("hlmessage", string.Format("Enabled = '{0}'", status), string.Format("Name = '{0}'", name));
	
						if(status == "on")
							sSendMessage.SendChatMessage(sIRCMessage, text[0], name);
						else
							sSendMessage.SendChatMessage(sIRCMessage, text[1], name);
					}
				}
				else
				{
					string name = sIRCMessage.Nick.ToLower();
					SchumixBase.DManager.Update("hlmessage", string.Format("Info = '{0}', Enabled = 'on'", sUtilities.SqlEscape(sIRCMessage.Info.SplitToString(5, SchumixBase.Space))), string.Format("Name = '{0}'", name));
					SchumixBase.DManager.Update("schumix", "FunctionStatus = 'on'", "FunctionName = 'autohl'");
					SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Functions = '{0}' WHERE Channel = '{1}'", sChannelInfo.ChannelFunctions("autohl", "on", sIRCMessage.Channel), sIRCMessage.Channel.ToLower());
					sChannelInfo.ChannelFunctionReload();
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("autofunction/hlmessage", sIRCMessage.Channel));
				}
			}

			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
				return;

			if(sIRCMessage.Info[4].ToLower() == "kick")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("No1Value", sIRCMessage.Channel));
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "add")
				{
					var text = sLManager.GetCommandTexts("autofunction/kick/add", sIRCMessage.Channel);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
						return;
					}

					if(sIRCMessage.Info.Length < 8)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoReason", sIRCMessage.Channel));
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM kicklist WHERE Name = '{0}' AND Channel = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.Channel.ToLower());
					if(!db.IsNull())
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}

					SchumixBase.DManager.QueryFirstRow("INSERT INTO `kicklist`(Name, Channel, Reason) VALUES ('{0}', '{1}', '{2}')", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.Channel.ToLower(), sUtilities.SqlEscape(sIRCMessage.Info.SplitToString(7, SchumixBase.Space)));
					sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Info[6]);
				}
				else if(sIRCMessage.Info[5].ToLower() == "remove")
				{
					var text = sLManager.GetCommandTexts("autofunction/kick/remove", sIRCMessage.Channel);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM kicklist WHERE Name = '{0}' AND Channel = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.Channel.ToLower());
					if(db.IsNull())
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}

					SchumixBase.DManager.QueryFirstRow("DELETE FROM `kicklist` WHERE Name = '{0}' AND Channel = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.Channel.ToLower());
					sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Info[6]);
				}
				else if(sIRCMessage.Info[5].ToLower() == "list")
				{
					var db = SchumixBase.DManager.Query("SELECT Name FROM kicklist WHERE Channel = '{0}'", sIRCMessage.Channel.ToLower());
					if(!db.IsNull())
					{
						string names = string.Empty;

						foreach(DataRow row in db.Rows)
						{
							string name = row["Name"].ToString();
							names += ", " + name;
						}

						if(names == string.Empty)
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("autofunction/kick/list", sIRCMessage.Channel), "none");
						else
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("autofunction/kick/list", sIRCMessage.Channel), names.Remove(0, 2, ", "));
					}
					else
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel));
				}
				else if(sIRCMessage.Info[5].ToLower() == "channel")
				{
					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel));
						return;
					}

					if(sIRCMessage.Info.Length < 8)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("No1Value", sIRCMessage.Channel));
						return;
					}

					if(sIRCMessage.Info[7].ToLower() == "add")
					{
						var text = sLManager.GetCommandTexts("autofunction/kick/channel/add", sIRCMessage.Channel);
						if(text.Length < 2)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
							return;
						}

						if(sIRCMessage.Info.Length < 9)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
							return;
						}

						if(sIRCMessage.Info.Length < 10)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoReason", sIRCMessage.Channel));
							return;
						}
	
						var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM kicklist WHERE Name = '{0}' AND Channel = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[8].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()));
						if(!db.IsNull())
						{
							sSendMessage.SendChatMessage(sIRCMessage, text[0]);
							return;
						}

						SchumixBase.DManager.QueryFirstRow("INSERT INTO `kicklist`(Name, Channel, Reason) VALUES ('{0}', '{1}', '{2}')", sUtilities.SqlEscape(sIRCMessage.Info[8].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info.SplitToString(9, SchumixBase.Space)));
						sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Info[8]);
					}
					else if(sIRCMessage.Info[7].ToLower() == "remove")
					{
						var text = sLManager.GetCommandTexts("autofunction/kick/channel/remove", sIRCMessage.Channel);
						if(text.Length < 2)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
							return;
						}

						if(sIRCMessage.Info.Length < 9)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
							return;
						}

						var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM kicklist WHERE Name = '{0}' AND Channel = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[8].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()));
						if(db.IsNull())
						{
							sSendMessage.SendChatMessage(sIRCMessage, text[0]);
							return;
						}

						SchumixBase.DManager.QueryFirstRow("DELETE FROM `kicklist` WHERE Name = '{0}' AND Channel = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[8].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()));
						sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Info[8]);
					}
					else if(sIRCMessage.Info[7].ToLower() == "list")
					{
						var text = sLManager.GetCommandTexts("autofunction/kick/channel/list", sIRCMessage.Channel);
						if(text.Length < 2)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
							return;
						}

						if(!IsChannel(sIRCMessage.Info[6].ToLower()))
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaChannelHasBeenSet", sIRCMessage.Channel));
							return;
						}

						var db0 = SchumixBase.DManager.QueryFirstRow("SELECT* FROM kicklist WHERE Channel = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()));
						if(db0.IsNull())
						{
							sSendMessage.SendChatMessage(sIRCMessage, text[1]);
							return;
						}

						var db = SchumixBase.DManager.Query("SELECT Name FROM kicklist WHERE Channel = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()));
						if(!db.IsNull())
						{
							string names = string.Empty;

							foreach(DataRow row in db.Rows)
							{
								string name = row["Name"].ToString();
								names += ", " + name;
							}

							if(names == string.Empty)
								sSendMessage.SendChatMessage(sIRCMessage, text[0], "none");
							else
								sSendMessage.SendChatMessage(sIRCMessage, text[0], names.Remove(0, 2, ", "));
						}
						else
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel));
					}
				}
			}
			else if(sIRCMessage.Info[4].ToLower() == "mode")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("No1Value", sIRCMessage.Channel));
					return;
				}
		
				if(sIRCMessage.Info[5].ToLower() == "add")
				{
					var text = sLManager.GetCommandTexts("autofunction/mode/add", sIRCMessage.Channel);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
						return;
					}

					if(sIRCMessage.Info.Length < 8)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoRank", sIRCMessage.Channel));
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM modelist WHERE Name = '{0}' AND Channel = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.Channel.ToLower());
					if(!db.IsNull())
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}

					SchumixBase.DManager.QueryFirstRow("INSERT INTO `modelist`(Name, Channel, Rank) VALUES ('{0}', '{1}', '{2}')", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.Channel.ToLower(), sUtilities.SqlEscape(sIRCMessage.Info[7].ToLower()));
					sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Info[6]);
				}
				else if(sIRCMessage.Info[5].ToLower() == "change")
				{
					var text = sLManager.GetCommandTexts("autofunction/mode/change", sIRCMessage.Channel);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
						return;
					}

					if(sIRCMessage.Info.Length < 8)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoRank", sIRCMessage.Channel));
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM modelist WHERE Name = '{0}' AND Channel = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.Channel.ToLower());
					if(db.IsNull())
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}

					SchumixBase.DManager.QueryFirstRow("UPDATE modelist SET Rank = {0} WHERE Name = '{1}' AND Channel = '{2}'", sUtilities.SqlEscape(sIRCMessage.Info[7].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.Channel.ToLower());
					sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Info[6], sIRCMessage.Info[7].ToLower());
				}
				else if(sIRCMessage.Info[5].ToLower() == "remove")
				{
					var text = sLManager.GetCommandTexts("autofunction/mode/remove", sIRCMessage.Channel);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT Rank FROM modelist WHERE Name = '{0}' AND Channel = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.Channel.ToLower());
					if(db.IsNull())
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}
					else
					{
						string rank = db["Rank"].ToString();
						if(rank.Substring(0, 1) == "+")
							sSender.Mode(sIRCMessage.Channel.ToLower(), "-" + rank.Remove(0, 1, "+"), sIRCMessage.Info[6].ToLower());	
					}

					SchumixBase.DManager.QueryFirstRow("DELETE FROM `modelist` WHERE Name = '{0}' AND Channel = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.Channel.ToLower());
					sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Info[6]);
				}
				else if(sIRCMessage.Info[5].ToLower() == "list")
				{
					var text = sLManager.GetCommandTexts("autofunction/mode/list", sIRCMessage.Channel);
					if(text.Length < 5)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						return;
					}

					var db = SchumixBase.DManager.Query("SELECT Name, Rank FROM modelist WHERE Channel = '{0}'", sIRCMessage.Channel.ToLower());
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

							if(rank.Length >= 1 && rank.Substring(0, 1) == "v")
							{
								voices += ", " + row["Name"].ToString();
								continue;
							}
							else if(rank.Length >= 1 && rank.Substring(0, 1) == "h")
							{
								halfoperators += ", " + row["Name"].ToString();
								continue;
							}
							else if(rank.Length >= 1 && rank.Substring(0, 1) == "o")
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

						if(voices == string.Empty)
							sSendMessage.SendChatMessage(sIRCMessage, text[0], "none");
						else
							sSendMessage.SendChatMessage(sIRCMessage, text[0], voices.Remove(0, 2, ", "));

						if(halfoperators == string.Empty)
							sSendMessage.SendChatMessage(sIRCMessage, text[1], "none");
						else
							sSendMessage.SendChatMessage(sIRCMessage, text[1], halfoperators.Remove(0, 2, ", "));

						if(operators == string.Empty)
							sSendMessage.SendChatMessage(sIRCMessage, text[2], "none");
						else
							sSendMessage.SendChatMessage(sIRCMessage, text[2], operators.Remove(0, 2, ", "));

						if(admins == string.Empty)
							sSendMessage.SendChatMessage(sIRCMessage, text[3], "none");
						else
							sSendMessage.SendChatMessage(sIRCMessage, text[3], admins.Remove(0, 2, ", "));

						if(owners == string.Empty)
							sSendMessage.SendChatMessage(sIRCMessage, text[4], "none");
						else
							sSendMessage.SendChatMessage(sIRCMessage, text[4], owners.Remove(0, 2, ", "));
					}
					else
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel));
				}
				else if(sIRCMessage.Info[5].ToLower() == "channel")
				{
					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel));
						return;
					}

					if(sIRCMessage.Info.Length < 8)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("No1Value", sIRCMessage.Channel));
						return;
					}
		
					if(sIRCMessage.Info[7].ToLower() == "add")
					{
						var text = sLManager.GetCommandTexts("autofunction/mode/channel/add", sIRCMessage.Channel);
						if(text.Length < 2)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
							return;
						}

						if(sIRCMessage.Info.Length < 9)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
							return;
						}

						if(sIRCMessage.Info.Length < 10)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoRank", sIRCMessage.Channel));
							return;
						}

						var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM modelist WHERE Name = '{0}' AND Channel = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[8].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()));
						if(!db.IsNull())
						{
							sSendMessage.SendChatMessage(sIRCMessage, text[0]);
							return;
						}

						SchumixBase.DManager.QueryFirstRow("INSERT INTO `modelist`(Name, Channel, Rank) VALUES ('{0}', '{1}', '{2}')", sUtilities.SqlEscape(sIRCMessage.Info[8].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[9]));
						sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Info[8]);
					}
					else if(sIRCMessage.Info[7].ToLower() == "change")
					{
						var text = sLManager.GetCommandTexts("autofunction/mode/channel/change", sIRCMessage.Channel);
						if(text.Length < 2)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
							return;
						}

						if(sIRCMessage.Info.Length < 9)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
							return;
						}

						if(sIRCMessage.Info.Length < 10)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoRank", sIRCMessage.Channel));
							return;
						}

						var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM modelist WHERE Name = '{0}' AND Channel = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[8].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()));
						if(db.IsNull())
						{
							sSendMessage.SendChatMessage(sIRCMessage, text[0]);
							return;
						}

						SchumixBase.DManager.QueryFirstRow("UPDATE modelist SET Rank = {0} WHERE Name = '{1}' AND Channel = '{2}'", sUtilities.SqlEscape(sIRCMessage.Info[9].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[8].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()));
						sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Info[8], sIRCMessage.Info[9].ToLower());
					}
					else if(sIRCMessage.Info[7].ToLower() == "remove")
					{
						var text = sLManager.GetCommandTexts("autofunction/mode/channel/remove", sIRCMessage.Channel);
						if(text.Length < 2)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
							return;
						}

						if(sIRCMessage.Info.Length < 9)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
							return;
						}
		
						var db = SchumixBase.DManager.QueryFirstRow("SELECT Rank FROM modelist WHERE Name = '{0}' AND Channel = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[8].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()));
						if(db.IsNull())
						{
							sSendMessage.SendChatMessage(sIRCMessage, text[0]);
							return;
						}
						else
						{
							string rank = db["Rank"].ToString();
							if(rank.Substring(0, 1) == "+")
								sSender.Mode(sIRCMessage.Info[6].ToLower(), "-" + rank.Remove(0, 1, "+"), sIRCMessage.Info[8].ToLower());	
						}

						SchumixBase.DManager.QueryFirstRow("DELETE FROM `modelist` WHERE Name = '{0}' AND Channel = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[8].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()));
						sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Info[8]);
					}
					else if(sIRCMessage.Info[7].ToLower() == "list")
					{
						var text = sLManager.GetCommandTexts("autofunction/mode/channel/list", sIRCMessage.Channel);
						if(text.Length < 6)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
							return;
						}

						if(!IsChannel(sIRCMessage.Info[6].ToLower()))
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaChannelHasBeenSet", sIRCMessage.Channel));
							return;
						}

						var db0 = SchumixBase.DManager.QueryFirstRow("SELECT* FROM modelist WHERE Channel = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()));
						if(db0.IsNull())
						{
							sSendMessage.SendChatMessage(sIRCMessage, text[5]);
							return;
						}

						var db = SchumixBase.DManager.Query("SELECT Name, Rank FROM modelist WHERE Channel = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()));
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

								if(rank.Length >= 1 && rank.Substring(0, 1) == "v")
								{
									voices += ", " + row["Name"].ToString();
									continue;
								}
								else if(rank.Length >= 1 && rank.Substring(0, 1) == "h")
								{
									halfoperators += ", " + row["Name"].ToString();
									continue;
								}
								else if(rank.Length >= 1 && rank.Substring(0, 1) == "o")
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

							if(voices == string.Empty)
								sSendMessage.SendChatMessage(sIRCMessage, text[0], "none");
							else
								sSendMessage.SendChatMessage(sIRCMessage, text[0], voices.Remove(0, 2, ", "));

							if(halfoperators == string.Empty)
								sSendMessage.SendChatMessage(sIRCMessage, text[1], "none");
							else
								sSendMessage.SendChatMessage(sIRCMessage, text[1], halfoperators.Remove(0, 2, ", "));

							if(operators == string.Empty)
								sSendMessage.SendChatMessage(sIRCMessage, text[2], "none");
							else
								sSendMessage.SendChatMessage(sIRCMessage, text[2], operators.Remove(0, 2, ", "));

							if(admins == string.Empty)
								sSendMessage.SendChatMessage(sIRCMessage, text[3], "none");
							else
								sSendMessage.SendChatMessage(sIRCMessage, text[3], admins.Remove(0, 2, ", "));

							if(owners == string.Empty)
								sSendMessage.SendChatMessage(sIRCMessage, text[4], "none");
							else
								sSendMessage.SendChatMessage(sIRCMessage, text[4], owners.Remove(0, 2, ", "));
						}
						else
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel));
					}
				}
			}
		}

		public void HandleMessage(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("No1Value", sIRCMessage.Channel));
				return;
			}

			if(!sChannelInfo.FSelect("message") || !sChannelInfo.FSelect("message", sIRCMessage.Channel))
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoMessageFunction", sIRCMessage.Channel));
				return;
			}

			if(sIRCMessage.Info[4].ToLower() == "channel")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel));
					return;
				}

				if(sIRCMessage.Info.Length < 7)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
					return;
				}

				if(sIRCMessage.Info.Length < 8)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoMessage", sIRCMessage.Channel));
					return;
				}

				SchumixBase.DManager.QueryFirstRow("INSERT INTO `message`(Name, Channel, Message, Wrote) VALUES ('{0}', '{1}', '{2}', '{3}')", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[5].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info.SplitToString(7, SchumixBase.Space)), sIRCMessage.Nick);
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("message/channel", sIRCMessage.Channel));
			}
			else
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoMessage", sIRCMessage.Channel));
					return;
				}

				SchumixBase.DManager.QueryFirstRow("INSERT INTO `message`(Name, Channel, Message, Wrote) VALUES ('{0}', '{1}', '{2}', '{3}')", sUtilities.SqlEscape(sIRCMessage.Info[4].ToLower()), sIRCMessage.Channel.ToLower(), sUtilities.SqlEscape(sIRCMessage.Info.SplitToString(5, SchumixBase.Space)), sIRCMessage.Nick);
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("message", sIRCMessage.Channel));
			}
		}

		public void HandleWeather(IRCMessage sIRCMessage)
		{
			var text = sLManager.GetCommandTexts("weather", sIRCMessage.Channel);
			if(text.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
				return;
			}

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoCityName", sIRCMessage.Channel));
				return;
			}

			bool home = false;
			string url = string.Empty;
			string source = string.Empty;

			switch(sLManager.GetChannelLocalization(sIRCMessage.Channel))
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
					source = sUtilities.GetUrl(string.Format("{0}{1}", url, sIRCMessage.Info[4]));

				string day = string.Empty;
				string night = string.Empty;
				source = source.Replace("\n\t\t", SchumixBase.Space.ToString());
				source = source.Replace("&deg;C", "°C");
				source = source.Replace("&#369;", "ű");

				if(source.Contains("<div class=\"fctText\">"))
				{
					source = source.Remove(0, source.IndexOf("<div class=\"fctText\">") + "<div class=\"fctText\">".Length);
					day = source.Substring(0, source.IndexOf("</div>"));
				}
				else if(source.Contains("<td class=\"vaT full\">"))
				{
					source = source.Remove(0, source.IndexOf("<td class=\"vaT full\">") + "<td class=\"vaT full\">".Length);
					day = source.Substring(0, source.IndexOf("</td>"));
				}
				else
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[4]);
					return;
				}

				if(source.Contains("<div class=\"fctText\">"))
				{
					source = source.Remove(0, source.IndexOf("<div class=\"fctText\">") + "<div class=\"fctText\">".Length);
					night = source.Substring(0, source.IndexOf("</div>"));
				}
				else if(source.Contains("<td class=\"vaT full\">"))
				{
					source = source.Remove(0, source.IndexOf("<td class=\"vaT full\">") + "<td class=\"vaT full\">".Length);
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
					sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Info[4]);

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
			sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("roll", sIRCMessage.Channel), number);
		}

		public void HandleSha1(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoValue", sIRCMessage.Channel));
				return;
			}

			sSendMessage.SendChatMessage(sIRCMessage, sUtilities.Sha1(sIRCMessage.Info.SplitToString(4, SchumixBase.Space)));
		}

		public void HandleMd5(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoValue", sIRCMessage.Channel));
				return;
			}

			sSendMessage.SendChatMessage(sIRCMessage, sUtilities.Md5(sIRCMessage.Info.SplitToString(4, SchumixBase.Space)));
		}

		public void HandlePrime(IRCMessage sIRCMessage)
		{
			var text = sLManager.GetCommandTexts("prime", sIRCMessage.Channel);
			if(text.Length < 3)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
				return;
			}

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoNumber", sIRCMessage.Channel));
				return;
			}

			double Num;
			bool isNum = double.TryParse(sIRCMessage.Info[4], out Num);

			if(!isNum)
			{
				sSendMessage.SendChatMessage(sIRCMessage, text[0]);
				return;
			}

			bool prim = sUtilities.IsPrime(Convert.ToInt32(Num));

			if(!prim)
				sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Info[4]);
			else
				sSendMessage.SendChatMessage(sIRCMessage, text[2], sIRCMessage.Info[4]);
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
			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoValue", sIRCMessage.Channel));
				return;
			}

			if(sIRCMessage.Info[4].ToLower() == "info")
			{
				if(!Warning(sIRCMessage))
					return;

				if(!IsUser(sIRCMessage.Nick, sIRCMessage.Host))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoDataNoCommand", sIRCMessage.Channel));
					return;
				}

				var db = SchumixBase.DManager.Query("SELECT Code FROM notes WHERE Name = '{0}'", sIRCMessage.Nick.ToLower());
				if(!db.IsNull())
				{
					string codes = string.Empty;

					foreach(DataRow row in db.Rows)
					{
						string code = row["Code"].ToString();
						codes += ", " + code;
					}

					if(codes == string.Empty)
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("notes/info", sIRCMessage.Channel), "none");
					else
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("notes/info", sIRCMessage.Channel), codes.Remove(0, 2, ", "));
				}
				else
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel));
			}
			else if(sIRCMessage.Info[4].ToLower() == "user")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoValue", sIRCMessage.Channel));
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "access")
				{
					var text = sLManager.GetCommandTexts("notes/user/access", sIRCMessage.Channel);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoPassword", sIRCMessage.Channel));
						return;
					}

					string name = sIRCMessage.Nick;
					var db = SchumixBase.DManager.QueryFirstRow("SELECT Password FROM notes_users WHERE Name = '{0}'", name.ToLower());
					if(!db.IsNull())
					{
						if(db["Password"].ToString() == sUtilities.Sha1(sIRCMessage.Info[6]))
						{
							SchumixBase.DManager.QueryFirstRow("UPDATE notes_users SET Vhost = '{0}' WHERE Name = '{1}'", sIRCMessage.Host, name.ToLower());
							sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						}
						else
							sSendMessage.SendChatMessage(sIRCMessage, text[1]);
					}
				}
				else if(sIRCMessage.Info[5].ToLower() == "newpassword")
				{
					var text = sLManager.GetCommandTexts("notes/user/newpassword", sIRCMessage.Channel);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoOldPassword", sIRCMessage.Channel));
						return;
					}

					if(sIRCMessage.Info.Length < 8)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoNewPassword", sIRCMessage.Channel));
						return;
					}

					string name = sIRCMessage.Nick;
					var db = SchumixBase.DManager.QueryFirstRow("SELECT Password FROM notes_users WHERE Name = '{0}'", name.ToLower());
					if(!db.IsNull())
					{
						if(db["Password"].ToString() == sUtilities.Sha1(sIRCMessage.Info[6]))
						{
							SchumixBase.DManager.QueryFirstRow("UPDATE notes_users SET Password = '{0}' WHERE Name = '{1}'", sUtilities.Sha1(sIRCMessage.Info[7]), name.ToLower());
							sSendMessage.SendChatMessage(sIRCMessage, text[0], sIRCMessage.Info[7]);
						}
						else
							sSendMessage.SendChatMessage(sIRCMessage, text[1]);
					}
				}
				else if(sIRCMessage.Info[5].ToLower() == "register")
				{
					var text = sLManager.GetCommandTexts("notes/user/register", sIRCMessage.Channel);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoPassword", sIRCMessage.Channel));
						return;
					}

					string name = sIRCMessage.Nick;
					var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM notes_users WHERE Name = '{0}'", name.ToLower());
					if(!db.IsNull())
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}

					string pass = sIRCMessage.Info[6];
					SchumixBase.DManager.QueryFirstRow("INSERT INTO `notes_users`(Name, Password, Vhost) VALUES ('{0}', '{1}', '{2}')", name.ToLower(), sUtilities.Sha1(pass), sIRCMessage.Host);
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				}
				else if(sIRCMessage.Info[5].ToLower() == "remove")
				{
					var text = sLManager.GetCommandTexts("notes/user/remove", sIRCMessage.Channel);
					if(text.Length < 5)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}

					string name = sIRCMessage.Nick;
					var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM notes_users WHERE Name = '{0}'", name.ToLower());
					if(db.IsNull())
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[1]);
						return;
					}

					db = SchumixBase.DManager.QueryFirstRow("SELECT Password FROM notes_users WHERE Name = '{0}'", name.ToLower());
					if(!db.IsNull())
					{
						if(db["Password"].ToString() != sUtilities.Sha1(sIRCMessage.Info[6]))
						{
							sSendMessage.SendChatMessage(sIRCMessage, text[2]);
							sSendMessage.SendChatMessage(sIRCMessage, text[3]);
							return;
						}
					}

					SchumixBase.DManager.QueryFirstRow("DELETE FROM `notes_users` WHERE Name = '{0}'", name.ToLower());
					SchumixBase.DManager.QueryFirstRow("DELETE FROM `notes` WHERE Name = '{0}'", name.ToLower());
					sSendMessage.SendChatMessage(sIRCMessage, text[4]);
				}
			}
			else if(sIRCMessage.Info[4].ToLower() == "code")
			{
				if(!Warning(sIRCMessage))
					return;

				if(!IsUser(sIRCMessage.Nick, sIRCMessage.Host))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoDataNoCommand", sIRCMessage.Channel));
					return;
				}

				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoCode", sIRCMessage.Channel));
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "remove")
				{
					var text = sLManager.GetCommandTexts("notes/code/remove", sIRCMessage.Channel);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoCode", sIRCMessage.Channel));
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM notes WHERE Code = '{0}' AND Name = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.Nick.ToLower());
					if(db.IsNull())
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}

					SchumixBase.DManager.QueryFirstRow("DELETE FROM `notes` WHERE Code = '{0}' AND Name = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.Nick.ToLower());
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				}
				else
				{
					var db = SchumixBase.DManager.QueryFirstRow("SELECT Note FROM notes WHERE Code = '{0}' AND Name = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[5].ToLower()), sIRCMessage.Nick.ToLower());
					if(!db.IsNull())
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("notes/code", sIRCMessage.Channel), db["Note"].ToString());
					else
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel));
				}
			}
			else
			{
				var text = sLManager.GetCommandTexts("notes", sIRCMessage.Channel);
				if(text.Length < 3)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
					return;
				}

				if(!Warning(sIRCMessage))
					return;

				if(!IsUser(sIRCMessage.Nick, sIRCMessage.Host))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoDataNoCommand", sIRCMessage.Channel));
					return;
				}

				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[0]);
					return;
				}

				string code = sIRCMessage.Info[4];
				var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM notes WHERE Code = '{0}' AND Name = '{1}'", sUtilities.SqlEscape(code.ToLower()), sIRCMessage.Nick.ToLower());
				if(!db.IsNull())
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
					return;
				}

				SchumixBase.DManager.QueryFirstRow("INSERT INTO `notes`(Code, Name, Note) VALUES ('{0}', '{1}', '{2}')", sUtilities.SqlEscape(code.ToLower()), sIRCMessage.Nick.ToLower(), sUtilities.SqlEscape(sIRCMessage.Info.SplitToString(5, SchumixBase.Space)));
				sSendMessage.SendChatMessage(sIRCMessage, text[2], code);
			}
		}

		private bool IsUser(string Name)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT * FROM notes_users WHERE Name = '{0}'", Name.ToLower());
			return !db.IsNull();
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
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
					return false;
				}

				sSendMessage.SendChatMessage(sIRCMessage, text[0]);
				sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				sSendMessage.SendChatMessage(sIRCMessage, text[2], IRCConfig.CommandPrefix);
				sSendMessage.SendChatMessage(sIRCMessage, text[3], IRCConfig.CommandPrefix);
				return false;
			}

			return true;
		}
	}
}