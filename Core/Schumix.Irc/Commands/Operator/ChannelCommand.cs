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
using Schumix.Irc.Util;
using Schumix.Framework;
using Schumix.Framework.Irc;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;

namespace Schumix.Irc.Commands
{
	public abstract partial class CommandHandler
	{
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
				
				if(!Rfc2812Util.IsValidChannelName(channel))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaChannelHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}
				
				if(sIgnoreChannel.IsIgnore(channel))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ThisChannelBlockedByAdmin", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}
				
				var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(channel), sIRCMessage.ServerName);
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
				sMyChannelInfo.ChannelListReload();
				sMyChannelInfo.ChannelFunctionsReload();
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
				
				if(!Rfc2812Util.IsValidChannelName(channel))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaChannelHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}
				
				if(channel == IRCConfig.List[sIRCMessage.ServerName].MasterChannel.ToLower())
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[0]);
					return;
				}
				
				var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(channel), sIRCMessage.ServerName);
				if(db.IsNull())
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
					return;
				}
				
				sSender.Part(channel);
				SchumixBase.DManager.Delete("channels", string.Format("Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(channel), sIRCMessage.ServerName));
				sSendMessage.SendChatMessage(sIRCMessage, text[2], channel);
				sMyChannelInfo.ChannelListReload();
				sMyChannelInfo.ChannelFunctionsReload();
			}
			else if(sIRCMessage.Info[4].ToLower() == "update")
			{
				sMyChannelInfo.ChannelListReload();
				sMyChannelInfo.ChannelFunctionsReload();
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("channel/update", sIRCMessage.Channel, sIRCMessage.ServerName));
			}
			else if(sIRCMessage.Info[4].ToLower() == "info")
			{
				var text = sLManager.GetCommandTexts("channel/info", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}
				
				var db = SchumixBase.DManager.Query("SELECT Channel, Enabled, Hidden, Error FROM channels WHERE ServerName = '{0}'", sIRCMessage.ServerName);
				if(!db.IsNull())
				{
					string ActiveChannels = string.Empty, InActiveChannels = string.Empty, HiddenChannels = string.Empty;
					
					foreach(DataRow row in db.Rows)
					{
						string channel = row["Channel"].ToString();
						bool enabled = asd.ToBoolean(row["Enabled"].ToString());
						bool hidden = asd.ToBoolean(row["Hidden"].ToString());
						
						if(enabled && !hidden)
							ActiveChannels += ", " + channel;
						else if(!enabled && !hidden)
							InActiveChannels += ", " + channel + SchumixBase.Colon + row["Error"].ToString();
						
						if(hidden)
							HiddenChannels += ", " + channel;
					}
					
					if(ActiveChannels.Length > 0)
						sSendMessage.SendChatMessage(sIRCMessage, text[0], ActiveChannels.Remove(0, 2, ", "));
					else
						sSendMessage.SendChatMessage(sIRCMessage, text[1]);
					
					if(InActiveChannels.Length > 0)
						sSendMessage.SendChatMessage(sIRCMessage, text[2], InActiveChannels.Remove(0, 2, ", "));
					else
						sSendMessage.SendChatMessage(sIRCMessage, text[3]);
					
					if(IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Administrator))
					{
						if(HiddenChannels.Length > 0)
							sSendMessage.SendChatMessage(sIRCMessage, text[4], HiddenChannels.Remove(0, 2, ", "));
						else
							sSendMessage.SendChatMessage(sIRCMessage, text[5]);
					}
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
				
				if(!Rfc2812Util.IsValidChannelName(sIRCMessage.Info[5]))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaChannelHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}
				
				var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[5].ToLower()), sIRCMessage.ServerName);
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
				SchumixBase.sCacheDB.Reload("channels");
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
					
					if(!Rfc2812Util.IsValidChannelName(sIRCMessage.Info[6]))
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
					
					var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName);
					if(db.IsNull())
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}
					
					db = SchumixBase.DManager.QueryFirstRow("SELECT Password FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName);
					if(!db.IsNull())
					{
						if(!db["Password"].ToString().IsNullOrEmpty())
						{
							sSendMessage.SendChatMessage(sIRCMessage, text[1]);
							return;
						}
					}
					
					SchumixBase.DManager.Update("channels", string.Format("Password = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[7])), string.Format("Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName));
					sSendMessage.SendChatMessage(sIRCMessage, text[2], sIRCMessage.Info[6]);
					SchumixBase.sCacheDB.Reload("channels");
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
					
					if(!Rfc2812Util.IsValidChannelName(sIRCMessage.Info[6]))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaChannelHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}
					
					var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName);
					if(db.IsNull())
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}
					
					db = SchumixBase.DManager.QueryFirstRow("SELECT Password FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName);
					if(!db.IsNull())
					{
						if(db["Password"].ToString().IsNullOrEmpty())
						{
							sSendMessage.SendChatMessage(sIRCMessage, text[1]);
							return;
						}
					}
					
					SchumixBase.DManager.Update("channels", "Password = ''", string.Format("Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName));
					sSendMessage.SendChatMessage(sIRCMessage, text[2]);
					SchumixBase.sCacheDB.Reload("channels");
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
					
					if(!Rfc2812Util.IsValidChannelName(sIRCMessage.Info[6]))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaChannelHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}
					
					if(sIRCMessage.Info.Length < 8)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoPassword", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}
					
					var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName);
					if(db.IsNull())
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}
					
					db = SchumixBase.DManager.QueryFirstRow("SELECT Password FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName);
					if(!db.IsNull())
					{
						if(db["Password"].ToString().IsNullOrEmpty())
						{
							sSendMessage.SendChatMessage(sIRCMessage, text[1]);
							return;
						}
					}
					
					SchumixBase.DManager.Update("channels", string.Format("Password = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[7])), string.Format("Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName));
					sSendMessage.SendChatMessage(sIRCMessage, text[2], sIRCMessage.Info[7]);
					SchumixBase.sCacheDB.Reload("channels");
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
					
					if(!Rfc2812Util.IsValidChannelName(sIRCMessage.Info[6]))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaChannelHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}
					
					var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName);
					if(db.IsNull())
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}
					
					db = SchumixBase.DManager.QueryFirstRow("SELECT Password FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.ServerName);
					if(!db.IsNull())
					{
						if(db["Password"].ToString().IsNullOrEmpty())
							sSendMessage.SendChatMessage(sIRCMessage, text[1]);
						else
							sSendMessage.SendChatMessage(sIRCMessage, text[2]);
					}
				}
			}
		}
	}
}