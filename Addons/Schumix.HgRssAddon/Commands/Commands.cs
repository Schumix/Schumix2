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
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.HgRssAddon.Commands
{
	public partial class RssCommand : CommandInfo
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;

		public void HandleHg(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
				return;

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoValue", sIRCMessage.Channel));
				return;
			}

			if(sIRCMessage.Info[4].ToLower() == "info")
			{
				var db = SchumixBase.DManager.Query("SELECT Name, Channel FROM hginfo");
				if(!db.IsNull())
				{
					foreach(DataRow row in db.Rows)
					{
						string name = row["Name"].ToString();
						string[] channel = row["Channel"].ToString().Split(SchumixBase.Comma);
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("hg/info", sIRCMessage.Channel), name, channel.SplitToString(" "));
					}
				}
				else
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel));
			}
			else if(sIRCMessage.Info[4].ToLower() == "list")
			{
				var db = SchumixBase.DManager.Query("SELECT Name FROM hginfo");
				if(!db.IsNull())
				{
					string list = string.Empty;

					foreach(DataRow row in db.Rows)
						list += SchumixBase.Space + row["Name"].ToString();

					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("hg/list", sIRCMessage.Channel), list);
				}
				else
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel));
			}
			else if(sIRCMessage.Info[4].ToLower() == "start")
			{
				var text = sLManager.GetCommandTexts("hg/start", sIRCMessage.Channel);
				if(text.Length < 3)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
					return;
				}

				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
					return;
				}

				foreach(var list in HgRssAddon.RssList)
				{
					if(sIRCMessage.Info[5].ToLower() == list.Name.ToLower())
					{
						if(list.Started)
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0], list.Name);
							return;
						}

						list.Start();
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1], list.Name);
						return;
					}
				}

				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[2], sIRCMessage.Info[5]);
			}
			else if(sIRCMessage.Info[4].ToLower() == "stop")
			{
				var text = sLManager.GetCommandTexts("hg/stop", sIRCMessage.Channel);
				if(text.Length < 3)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
					return;
				}

				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
					return;
				}

				foreach(var list in HgRssAddon.RssList)
				{
					if(sIRCMessage.Info[5].ToLower() == list.Name.ToLower())
					{
						if(!list.Started)
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0], list.Name);
							return;
						}

						list.Stop();
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1], list.Name);
						return;
					}
				}

				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[2], sIRCMessage.Info[5]);
			}
			else if(sIRCMessage.Info[4].ToLower() == "reload")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("No1Value", sIRCMessage.Channel));
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "all")
				{
					foreach(var list in HgRssAddon.RssList)
						list.Reload();

					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("hg/reload/all", sIRCMessage.Channel));
				}
				else
				{
					var text = sLManager.GetCommandTexts("hg/reload", sIRCMessage.Channel);
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

					foreach(var list in HgRssAddon.RssList)
					{
						if(sIRCMessage.Info[6].ToLower() == list.Name.ToLower())
						{
							list.Reload();
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0], list.Name);
							return;
						}
					}

					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1], sIRCMessage.Info[6]);
				}
			}
			else if(sIRCMessage.Info[4].ToLower() == "channel")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoCommand", sIRCMessage.Channel));
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "add")
				{
					var text = sLManager.GetCommandTexts("hg/channel/add", sIRCMessage.Channel);
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
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel));
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT Channel FROM hginfo WHERE Name = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()));
					if(!db.IsNull())
					{
						string[] channel = db["Channel"].ToString().Split(SchumixBase.Comma);
						string data = channel.SplitToString(SchumixBase.Comma);

						if(channel.Length == 1 && data == string.Empty)
							data += sIRCMessage.Info[7].ToLower();
						else
							data += SchumixBase.Comma + sIRCMessage.Info[7].ToLower();

						SchumixBase.DManager.Update("hginfo", string.Format("Channel = '{0}'", data), string.Format("Name = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower())));
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
					}
					else
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1]);
				}
				else if(sIRCMessage.Info[5].ToLower() == "remove")
				{
					var text = sLManager.GetCommandTexts("hg/channel/remove", sIRCMessage.Channel);
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
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel));
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT Channel FROM hginfo WHERE Name = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()));
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

						SchumixBase.DManager.Update("hginfo", string.Format("Channel = '{0}'", data.Remove(0, 1, SchumixBase.Comma)), string.Format("Name = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower())));
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
					}
					else
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1]);
				}
			}
		}
	}
}