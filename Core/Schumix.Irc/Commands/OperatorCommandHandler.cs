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

			CNick(sIRCMessage);

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoValue", sIRCMessage.Channel));
				return;
			}

			if(sIRCMessage.Info[4].ToLower() == "info")
			{
				var text = sLManager.GetCommandTexts("function/info", sIRCMessage.Channel);
				if(text.Length < 2)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
					return;
				}

				string[] ChannelInfo = sChannelInfo.ChannelFunctionsInfo(sIRCMessage.Channel).Split('|');
				if(ChannelInfo.Length < 2)
					return;

				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0], ChannelInfo[0]);
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1], ChannelInfo[1]);
			}
			else if(sIRCMessage.Info[4].ToLower() == "all")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("No1Value", sIRCMessage.Channel));
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "info")
				{
					var text = sLManager.GetCommandTexts("function/all/info", sIRCMessage.Channel);
					if(text.Length < 2)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						return;
					}

					string f = sChannelInfo.FunctionsInfo();
					if(f == "Hibás lekérdezés!")
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel));
						return;
					}

					string[] FunctionInfo = f.Split('|');
					if(FunctionInfo.Length < 2)
						return;

					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0], FunctionInfo[0]);
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1], FunctionInfo[1]);
				}
				else
				{
					var text = sLManager.GetCommandTexts("function/all", sIRCMessage.Channel);
					if(text.Length < 2)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoFunctionName", sIRCMessage.Channel));
						return;
					}

					if(sIRCMessage.Info[5].ToLower() == "on" || sIRCMessage.Info[5].ToLower() == "off")
					{
						if(sIRCMessage.Info.Length >= 8)
						{
							string args = string.Empty;

							for(int i = 6; i < sIRCMessage.Info.Length; i++)
							{
								args += ", " + sIRCMessage.Info[i].ToLower();
								SchumixBase.DManager.QueryFirstRow("UPDATE schumix SET FunctionStatus = '{0}' WHERE FunctionName = '{1}'", sIRCMessage.Info[5].ToLower(), sUtilities.SqlEscape(sIRCMessage.Info[i].ToLower()));
							}

							if(sIRCMessage.Info[5].ToLower() == "on")
								sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0],  args.Remove(0, 2, ", "));
							else
								sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1],  args.Remove(0, 2, ", "));
						}
						else
						{
							if(sIRCMessage.Info[5].ToLower() == "on")
								sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0], sIRCMessage.Info[6].ToLower());
							else
								sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1], sIRCMessage.Info[6].ToLower());

							SchumixBase.DManager.QueryFirstRow("UPDATE schumix SET FunctionStatus = '{0}' WHERE FunctionName = '{1}'", sIRCMessage.Info[5].ToLower(), sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()));
						}
					}
				}
			}
			else if(sIRCMessage.Info[4].ToLower() == "channel")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel));
					return;
				}

				if(sIRCMessage.Info.Length < 7)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("No1Value", sIRCMessage.Channel));
					return;
				}
			
				string channel = sIRCMessage.Info[5].ToLower();
				string status = sIRCMessage.Info[6].ToLower();
			
				if(sIRCMessage.Info[6].ToLower() == "info")
				{
					var text = sLManager.GetCommandTexts("function/channel/info", sIRCMessage.Channel);
					if(text.Length < 2)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						return;
					}

					string[] ChannelInfo = sChannelInfo.ChannelFunctionsInfo(channel).Split('|');
					if(ChannelInfo.Length < 2)
						return;

					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0], ChannelInfo[0]);
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1], ChannelInfo[1]);
				}
				else if(status == "on" || status == "off")
				{
					var text = sLManager.GetCommandTexts("function/channel", sIRCMessage.Channel);
					if(text.Length < 2)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						return;
					}

					if(sIRCMessage.Info.Length < 8)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoFunctionName", sIRCMessage.Channel));
						return;
					}

					if(sIRCMessage.Info.Length >= 9)
					{
						string args = string.Empty;

						for(int i = 7; i < sIRCMessage.Info.Length; i++)
						{
							args += ", " + sIRCMessage.Info[i].ToLower();
							SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Functions = '{0}' WHERE Channel = '{1}'", sChannelInfo.ChannelFunctions(sIRCMessage.Info[i].ToLower(), status, channel), channel);
							sChannelInfo.ChannelFunctionReload();
						}

						if(status == "on")
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0],  args.Remove(0, 2, ", "));
						else
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1],  args.Remove(0, 2, ", "));
					}
					else
					{
						if(status == "on")
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0], sIRCMessage.Info[7].ToLower());
						else
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1], sIRCMessage.Info[7].ToLower());

						SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Functions = '{0}' WHERE Channel = '{1}'", sChannelInfo.ChannelFunctions(sIRCMessage.Info[7].ToLower(), status, channel), channel);
						sChannelInfo.ChannelFunctionReload();
					}
				}
			}
			else if(sIRCMessage.Info[4].ToLower() == "update")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("function/update", sIRCMessage.Channel), sIRCMessage.Channel);
					SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Functions = '{0}' WHERE Channel = '{1}'", sUtilities.GetFunctionUpdate(), sIRCMessage.Channel);
					sChannelInfo.ChannelFunctionReload();
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
							SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Functions = '{0}' WHERE Channel = '{1}'", sUtilities.GetFunctionUpdate(), channel);
						}

						sChannelInfo.ChannelFunctionReload();
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("function/update/all", sIRCMessage.Channel));
					}
					else
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel));
				}
				else
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("function/update", sIRCMessage.Channel), sIRCMessage.Info[5].ToLower());
					SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Functions = '{0}' WHERE Channel = '{1}'", sUtilities.GetFunctionUpdate(), sUtilities.SqlEscape(sIRCMessage.Info[5].ToLower()));
					sChannelInfo.ChannelFunctionReload();
				}
			}
			else
			{
				var text = sLManager.GetCommandTexts("function", sIRCMessage.Channel);
				if(text.Length < 2)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
					return;
				}

				if(sIRCMessage.Info.Length < 5)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoFunctionStatus", sIRCMessage.Channel));
					return;
				}

				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoFunctionName", sIRCMessage.Channel));
					return;
				}

				string status = sIRCMessage.Info[4].ToLower();

				if(status == "on" || status == "off")
				{
					if(sIRCMessage.Info.Length >= 7)
					{
						string args = string.Empty;

						for(int i = 5; i < sIRCMessage.Info.Length; i++)
						{
							args += ", " + sIRCMessage.Info[i].ToLower();
							SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Functions = '{0}' WHERE Channel = '{1}'", sChannelInfo.ChannelFunctions(sIRCMessage.Info[i].ToLower(), status, sIRCMessage.Channel), sIRCMessage.Channel);
							sChannelInfo.ChannelFunctionReload();
						}

						if(status == "on")
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0],  args.Remove(0, 2, ", "));
						else
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1],  args.Remove(0, 2, ", "));
					}
					else
					{
						if(status == "on")
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0], sIRCMessage.Info[5].ToLower());
						else
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1], sIRCMessage.Info[5].ToLower());

						SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Functions = '{0}' WHERE Channel = '{1}'", sChannelInfo.ChannelFunctions(sIRCMessage.Info[5].ToLower(), status, sIRCMessage.Channel), sIRCMessage.Channel);
						sChannelInfo.ChannelFunctionReload();
					}
				}
			}
		}

		protected void HandleChannel(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
				return;

			CNick(sIRCMessage);

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("channel", sIRCMessage.Channel));
				return;
			}

			if(sIRCMessage.Info[4].ToLower() == "add")
			{
				var text = sLManager.GetCommandTexts("channel/add", sIRCMessage.Channel);
				if(text.Length < 2)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
					return;
				}

				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel));
					return;
				}

				string channel = sIRCMessage.Info[5].ToLower();
				var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM channel WHERE Channel = '{0}'", sUtilities.SqlEscape(channel));
				if(!db.IsNull())
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
					return;
				}

				if(sIRCMessage.Info.Length == 7)
				{
					ChannelPrivmsg = sIRCMessage.Channel;
					string pass = sIRCMessage.Info[6];
					sSender.Join(channel, pass);
					SchumixBase.DManager.QueryFirstRow("INSERT INTO `channel`(Channel, Password, Language) VALUES ('{0}', '{1}', '{2}')", sUtilities.SqlEscape(channel), sUtilities.SqlEscape(pass), sLManager.Locale);
					SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Enabled = 'true' WHERE Channel = '{0}'", sUtilities.SqlEscape(channel));
				}
				else
				{
					ChannelPrivmsg = sIRCMessage.Channel;
					sSender.Join(channel);
					SchumixBase.DManager.QueryFirstRow("INSERT INTO `channel`(Channel, Password, Language) VALUES ('{0}', '', '{1}')", sUtilities.SqlEscape(channel), sLManager.Locale);
					SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Enabled = 'true' WHERE Channel = '{0}'", sUtilities.SqlEscape(channel));
				}

				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1], channel);
				sChannelInfo.ChannelListReload();
				sChannelInfo.ChannelFunctionReload();
			}
			else if(sIRCMessage.Info[4].ToLower() == "remove")
			{
				var text = sLManager.GetCommandTexts("channel/remove", sIRCMessage.Channel);
				if(text.Length < 3)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
					return;
				}

				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel));
					return;
				}

				string channel = sIRCMessage.Info[5].ToLower();
				var db = SchumixBase.DManager.QueryFirstRow("SELECT Id FROM channel WHERE Channel = '{0}'", sUtilities.SqlEscape(channel));
				if(!db.IsNull())
				{
					int id = Convert.ToInt32(db["Id"].ToString());
					if(id == 1)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
						return;
					}
				}

				db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM channel WHERE Channel = '{0}'", sUtilities.SqlEscape(channel));
				if(db.IsNull())
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1]);
					return;
				}

				sSender.Part(channel);
				SchumixBase.DManager.QueryFirstRow("DELETE FROM `channel` WHERE Channel = '{0}'", sUtilities.SqlEscape(channel));
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[2], channel);
				sChannelInfo.ChannelListReload();
				sChannelInfo.ChannelFunctionReload();
			}
			else if(sIRCMessage.Info[4].ToLower() == "update")
			{
				sChannelInfo.ChannelListReload();
				sChannelInfo.ChannelFunctionReload();
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("channel/update", sIRCMessage.Channel));
			}
			else if(sIRCMessage.Info[4].ToLower() == "info")
			{
				var text = sLManager.GetCommandTexts("channel/info", sIRCMessage.Channel);
				if(text.Length < 4)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
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
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0], ActiveChannels.Remove(0, 2, ", "));
					else
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1]);

					if(InActiveChannels.Length > 0)
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[2], InActiveChannels.Remove(0, 2, ", "));
					else
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[3]);
				}
				else
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel));
			}
			else if(sIRCMessage.Info[4].ToLower() == "language")
			{
				var text = sLManager.GetCommandTexts("channel/remove", sIRCMessage.Channel);
				if(text.Length < 3)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
					return;
				}

				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel));
					return;
				}

				if(sIRCMessage.Info.Length < 7)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoChannelLanguage", sIRCMessage.Channel));
					return;
				}

				var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM channel WHERE Channel = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[5].ToLower()));
				if(db.IsNull())
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1]);
					return;
				}

				SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Language = '{0}' WHERE Channel = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6]), sUtilities.SqlEscape(sIRCMessage.Info[5].ToLower()));
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("channel/language", sIRCMessage.Channel), sIRCMessage.Info[6]);
			}
		}

		protected void HandleSznap(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
				return;

			CNick(sIRCMessage);

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
				return;
			}

			// INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'sznap', '1', 'Kiírja a megadott név születésnapjának dátumát.\nHasználata: {0}sznap <név>');

			var db = SchumixBase.DManager.QueryFirstRow("SELECT nev, honap, nap FROM sznap WHERE nev = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[4]));
			if(!db.IsNull())
			{
				string name = db["nev"].ToString();
				string month = db["honap"].ToString();
				int day = Convert.ToInt32(db["nap"]);
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "{0} születés napja: {1} {2}", name, month, day);
			}
			else
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs ilyen ember!");
		}

		protected void HandleKick(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
				return;

			CNick(sIRCMessage);

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
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
					sSender.Kick(sIRCMessage.Channel, kick, sIRCMessage.Info.SplitToString(5, " "));
			}
		}

		protected void HandleMode(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
				return;

			CNick(sIRCMessage);

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoRank", sIRCMessage.Channel));
				return;
			}

			if(sIRCMessage.Info.Length == 5)
			{
				sSender.Mode(sIRCMessage.Channel, sIRCMessage.Info[4].ToLower());
				return;
			}

			if(sIRCMessage.Info.Length < 6)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
				return;
			}

			string rank = sIRCMessage.Info[4].ToLower();
			string name = sIRCMessage.Info.SplitToString(5, " ").ToLower();

			if(!name.Contains(sNickInfo.NickStorage.ToLower()))
				sSender.Mode(sIRCMessage.Channel, rank, name);
		}
	}
}
