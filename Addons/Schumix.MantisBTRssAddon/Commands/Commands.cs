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
using System.Collections.Generic;
using Schumix.API;
using Schumix.API.Irc;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.MantisBTRssAddon.Commands
{
	class RssCommand : CommandInfo
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		public readonly List<MantisBTRss> RssList = new List<MantisBTRss>();
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;

		public RssCommand(string ServerName) : base(ServerName)
		{
		}

		public void HandleMantisBT(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
				return;

			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoValue", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			if(sIRCMessage.Info[4].ToLower() == "info")
			{
				var db = SchumixBase.DManager.Query("SELECT Name, Channel FROM mantisbt WHERE ServerName = '{0}'", sIRCMessage.ServerName);
				if(!db.IsNull())
				{
					foreach(DataRow row in db.Rows)
					{
						string name = row["Name"].ToString();
						string[] channel = row["Channel"].ToString().Split(SchumixBase.Comma);
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("mantisbt/info", sIRCMessage.Channel, sIRCMessage.ServerName), name, channel.SplitToString(SchumixBase.Space));
					}
				}
				else
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel, sIRCMessage.ServerName));
			}
			else if(sIRCMessage.Info[4].ToLower() == "list")
			{
				var db = SchumixBase.DManager.Query("SELECT Name FROM mantisbt WHERE ServerName = '{0}'", sIRCMessage.ServerName);
				if(!db.IsNull())
				{
					string list = string.Empty;

					foreach(DataRow row in db.Rows)
						list += SchumixBase.Space + row["Name"].ToString();

					if(list == string.Empty)
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("mantisbt/list", sIRCMessage.Channel, sIRCMessage.ServerName), SchumixBase.Space + sLConsole.Other("Nothing"));
					else
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("mantisbt/list", sIRCMessage.Channel, sIRCMessage.ServerName), list);
				}
				else
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel, sIRCMessage.ServerName));
			}
			else if(sIRCMessage.Info[4].ToLower() == "start")
			{
				var text = sLManager.GetCommandTexts("mantisbt/start", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 3)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}

				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				foreach(var list in RssList)
				{
					if(sIRCMessage.Info[5].ToLower() == list.Name.ToLower())
					{
						if(list.Started)
						{
							sSendMessage.SendChatMessage(sIRCMessage, text[0], list.Name);
							return;
						}

						list.Start();
						sSendMessage.SendChatMessage(sIRCMessage, text[1], list.Name);
						return;
					}
				}

				sSendMessage.SendChatMessage(sIRCMessage, text[2], sIRCMessage.Info[5]);
			}
			else if(sIRCMessage.Info[4].ToLower() == "stop")
			{
				var text = sLManager.GetCommandTexts("mantisbt/stop", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 3)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}

				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				foreach(var list in RssList)
				{
					if(sIRCMessage.Info[5].ToLower() == list.Name.ToLower())
					{
						if(!list.Started)
						{
							sSendMessage.SendChatMessage(sIRCMessage, text[0], list.Name);
							return;
						}

						list.Stop();
						sSendMessage.SendChatMessage(sIRCMessage, text[1], list.Name);
						return;
					}
				}

				sSendMessage.SendChatMessage(sIRCMessage, text[2], sIRCMessage.Info[5]);
			}
			else if(sIRCMessage.Info[4].ToLower() == "reload")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("No1Value", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "all")
				{
					foreach(var list in RssList)
						list.Reload();

					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("mantisbt/reload/all", sIRCMessage.Channel, sIRCMessage.ServerName));
				}
				else
				{
					var text = sLManager.GetCommandTexts("mantisbt/reload", sIRCMessage.Channel, sIRCMessage.ServerName);
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

					foreach(var list in RssList)
					{
						if(sIRCMessage.Info[6].ToLower() == list.Name.ToLower())
						{
							list.Reload();
							sSendMessage.SendChatMessage(sIRCMessage, text[0], list.Name);
							return;
						}
					}

					sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Info[6]);
				}
			}
			else if(sIRCMessage.Info[4].ToLower() == "channel")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoCommand", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "add")
				{
					var text = sLManager.GetCommandTexts("mantisbt/channel/add", sIRCMessage.Channel, sIRCMessage.ServerName);
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
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT Channel FROM mantisbt WHERE Name = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName);
					if(!db.IsNull())
					{
						string[] channel = db["Channel"].ToString().Split(SchumixBase.Comma);
						string data = channel.SplitToString(SchumixBase.Comma);

						if(channel.Length == 1 && data == string.Empty)
							data += sIRCMessage.Info[7].ToLower();
						else
							data += SchumixBase.Comma + sIRCMessage.Info[7].ToLower();

						SchumixBase.DManager.Update("mantisbt", string.Format("Channel = '{0}'", data), string.Format("Name = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName));
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
					}
					else
						sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				}
				else if(sIRCMessage.Info[5].ToLower() == "remove")
				{
					var text = sLManager.GetCommandTexts("mantisbt/channel/remove", sIRCMessage.Channel, sIRCMessage.ServerName);
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
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT Channel FROM mantisbt WHERE Name = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName);
					if(!db.IsNull())
					{
						string[] channel = db["Channel"].ToString().Split(SchumixBase.Comma);
						string data = string.Empty;

						for(int x = 0; x < channel.Length; x++)
						{
							if(channel[x] == sIRCMessage.Info[7].ToLower())
								continue;

							data += SchumixBase.Comma + channel[x];
						}

						SchumixBase.DManager.Update("mantisbt", string.Format("Channel = '{0}'", data.Remove(0, 1, SchumixBase.Comma)), string.Format("Name = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName));
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
					}
					else
						sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				}
			}
		}
	}
}