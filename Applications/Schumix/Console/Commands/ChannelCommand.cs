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
using Schumix.Framework.Config;
using Schumix.Framework.Logger;
using Schumix.Framework.Extensions;
using Schumix.Console.Delegate;

namespace Schumix.Console
{
	/// <summary>
	///     CommandHandler class.
	/// </summary>
	partial class CommandHandler
	{
		/// <summary>
		///     Channel parancs függvénye.
		/// </summary>
		protected void HandleChannel(ConsoleMessage sConsoleMessage)
		{
			if(sConsoleMessage.Info.Length < 2)
			{
				Log.Notice("Console", sLManager.GetConsoleCommandText("channel"));
				return;
			}

			if(sConsoleMessage.Info[1].ToLower() == "add")
			{
				if(sConsoleMessage.Info.Length < 3)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NoChannelName"));
					return;
				}

				var text = sLManager.GetConsoleCommandTexts("channel/add");
				if(text.Length < 2)
				{
					Log.Error("Console", sLConsole.Translations("NoFound2"));
					return;
				}

				string channel = sConsoleMessage.Info[2].ToLower();

				if(!Rfc2812Util.IsValidChannelName(channel))
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NotaChannelHasBeenSet"));
					return;
				}

				if(sIrcBase.Networks[_servername].sChannelList.List.ContainsKey(sConsoleMessage.Info[1].ToLower()))
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("ImAlreadyOnThisChannel"));
					return;
				}

				var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", channel, _servername);
				if(!db.IsNull())
				{
					Log.Warning("Console", text[0]);
					return;
				}

				if(sConsoleMessage.Info.Length == 4)
				{
					string pass = sConsoleMessage.Info[3];
					sIrcBase.Networks[_servername].sSender.Join(channel, pass);
					SchumixBase.DManager.Insert("`channels`(ServerId, ServerName, Channel, Password, Language)", IRCConfig.List[_servername].ServerId, _servername, channel, pass, sLManager.Locale);
					SchumixBase.DManager.Update("channels", "Enabled = 'true'", string.Format("Channel = '{0}' And ServerName = '{1}'", channel, _servername));
				}
				else
				{
					sIrcBase.Networks[_servername].sSender.Join(channel);
					SchumixBase.DManager.Insert("`channels`(ServerId, ServerName, Channel, Password, Language)", IRCConfig.List[_servername].ServerId, _servername, channel, string.Empty, sLManager.Locale);
					SchumixBase.DManager.Update("channels", "Enabled = 'true'", string.Format("Channel = '{0}' And ServerName = '{1}'", channel, _servername));
				}

				Log.Notice("Console", text[1], channel);
				sIrcBase.Networks[_servername].sMyChannelInfo.ChannelListReload();
				sIrcBase.Networks[_servername].sMyChannelInfo.ChannelFunctionsReload();
			}
			else if(sConsoleMessage.Info[1].ToLower() == "remove")
			{
				if(sConsoleMessage.Info.Length < 3)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NoChannelName"));
					return;
				}

				var text = sLManager.GetConsoleCommandTexts("channel/remove");
				if(text.Length < 3)
				{
					Log.Error("Console", sLConsole.Translations("NoFound2"));
					return;
				}

				string channel = sConsoleMessage.Info[2].ToLower();

				if(!Rfc2812Util.IsValidChannelName(channel))
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NotaChannelHasBeenSet"));
					return;
				}

				if(channel == IRCConfig.List[_servername].MasterChannel.ToLower())
				{
					Log.Warning("Console", text[0]);
					return;
				}

				var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", channel, _servername);
				if(db.IsNull())
				{
					Log.Warning("Console", text[1]);
					return;
				}

				sIrcBase.Networks[_servername].sSender.Part(channel);
				SchumixBase.DManager.Delete("channels", string.Format("Channel = '{0}' And ServerName = '{1}'", channel, _servername));
				Log.Notice("Console", text[2], channel);

				sIrcBase.Networks[_servername].sMyChannelInfo.ChannelListReload();
				sIrcBase.Networks[_servername].sMyChannelInfo.ChannelFunctionsReload();
			}
			else if(sConsoleMessage.Info[1].ToLower() == "update")
			{
				sIrcBase.Networks[_servername].sMyChannelInfo.ChannelListReload();
				sIrcBase.Networks[_servername].sMyChannelInfo.ChannelFunctionsReload();
				Log.Notice("Console", sLManager.GetConsoleCommandText("channel/update"));
			}
			else if(sConsoleMessage.Info[1].ToLower() == "info")
			{
				var text = sLManager.GetConsoleCommandTexts("channel/info");
				if(text.Length < 6)
				{
					Log.Error("Console", sLConsole.Translations("NoFound2"));
					return;
				}

				var db = SchumixBase.DManager.Query("SELECT Channel, Enabled, Hidden, Error FROM channels WHERE ServerName = '{0}'", _servername);
				if(!db.IsNull())
				{
					string ActiveChannels = string.Empty, InActiveChannels = string.Empty, HiddenChannels = string.Empty;

					foreach(DataRow row in db.Rows)
					{
						string channel = row["Channel"].ToString();
						bool enabled = row["Enabled"].ToBoolean();
						bool hidden = row["Hidden"].ToBoolean();

						if(enabled && !hidden)
							ActiveChannels += ", " + channel;
						else if(!enabled && !hidden)
							InActiveChannels += ", " + channel + SchumixBase.Colon + row["Error"].ToString();

						if(hidden)
							HiddenChannels += ", " + channel;
					}

					if(ActiveChannels.Length > 0)
						Log.Notice("Console", text[0], ActiveChannels.Remove(0, 2, ", "));
					else
						Log.Notice("Console", text[1]);

					if(InActiveChannels.Length > 0)
						Log.Notice("Console", text[2], InActiveChannels.Remove(0, 2, ", "));
					else
						Log.Notice("Console", text[3]);

					if(HiddenChannels.Length > 0)
						Log.Notice("Console", text[4], HiddenChannels.Remove(0, 2, ", "));
					else
						Log.Notice("Console", text[5]);
				}
				else
					Log.Error("Console", sLManager.GetConsoleWarningText("FaultyQuery"));
			}
			else if(sConsoleMessage.Info[1].ToLower() == "language")
			{
				var text = sLManager.GetConsoleCommandTexts("channel/language");
				if(text.Length < 3)
				{
					Log.Error("Console", sLConsole.Translations("NoFound2"));
					return;
				}

				if(sConsoleMessage.Info.Length < 3)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NoChannelName"));
					return;
				}

				if(!Rfc2812Util.IsValidChannelName(sConsoleMessage.Info[2]))
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NotaChannelHasBeenSet"));
					return;
				}

				var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", sConsoleMessage.Info[2].ToLower(), _servername);
				if(db.IsNull())
				{
					Log.Warning("Console", text[1]);
					return;
				}

				if(sConsoleMessage.Info.Length < 4)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NoChannelLanguage"));
					return;
				}

				db = SchumixBase.DManager.QueryFirstRow("SELECT Language FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", sConsoleMessage.Info[2].ToLower(), _servername);
				if(!db.IsNull())
				{
					if(db["Language"].ToString().ToLower() == sConsoleMessage.Info[3].ToLower())
					{
						Log.Warning("Console", text[2], sConsoleMessage.Info[3]);
						return;
					}
				}

				SchumixBase.DManager.Update("channels", string.Format("Language = '{0}'", sConsoleMessage.Info[3]), string.Format("Channel = '{0}' And ServerName = '{1}'", sConsoleMessage.Info[2].ToLower(), _servername));
				Log.Notice("Console", text[0], sConsoleMessage.Info[3]);
				SchumixBase.sCacheDB.Reload("channels");
			}
			else if(sConsoleMessage.Info[1].ToLower() == "password")
			{
				if(sConsoleMessage.Info.Length < 3)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("NoValue"));
					return;
				}

				if(sConsoleMessage.Info[2].ToLower() == "add")
				{
					var text = sLManager.GetConsoleCommandTexts("channel/password/add");
					if(text.Length < 3)
					{
						Log.Error("Console", sLConsole.Translations("NoFound2"));
						return;
					}

					if(sConsoleMessage.Info.Length < 4)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoChannelName"));
						return;
					}

					if(!Rfc2812Util.IsValidChannelName(sConsoleMessage.Info[3]))
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NotaChannelHasBeenSet"));
						return;
					}

					if(sConsoleMessage.Info.Length < 5)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoPassword"));
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", sConsoleMessage.Info[3].ToLower(), _servername);
					if(db.IsNull())
					{
						Log.Warning("Console", text[0]);
						return;
					}

					db = SchumixBase.DManager.QueryFirstRow("SELECT Password FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", sConsoleMessage.Info[3].ToLower(), _servername);
					if(!db.IsNull())
					{
						if(!db["Password"].ToString().IsNullOrEmpty())
						{
							Log.Notice("Console", text[1]);
							return;
						}
					}

					SchumixBase.DManager.Update("channels", string.Format("Password = '{0}'", sConsoleMessage.Info[4]), string.Format("Channel = '{0}' And ServerName = '{1}'", sConsoleMessage.Info[3].ToLower(), _servername));
					Log.Notice("Console", text[2], sConsoleMessage.Info[3]);
					SchumixBase.sCacheDB.Reload("channels");
				}
				else if(sConsoleMessage.Info[2].ToLower() == "remove")
				{
					var text = sLManager.GetConsoleCommandTexts("channel/password/remove");
					if(text.Length < 3)
					{
						Log.Error("Console", sLConsole.Translations("NoFound2"));
						return;
					}

					if(sConsoleMessage.Info.Length < 4)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoChannelName"));
						return;
					}

					if(!Rfc2812Util.IsValidChannelName(sConsoleMessage.Info[3]))
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NotaChannelHasBeenSet"));
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", sConsoleMessage.Info[3].ToLower(), _servername);
					if(db.IsNull())
					{
						Log.Warning("Console", text[0]);
						return;
					}

					db = SchumixBase.DManager.QueryFirstRow("SELECT Password FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", sConsoleMessage.Info[3].ToLower(), _servername);
					if(!db.IsNull())
					{
						if(db["Password"].ToString().IsNullOrEmpty())
						{
							Log.Notice("Console", text[1]);
							return;
						}
					}

					SchumixBase.DManager.Update("channels", "Password = ''", string.Format("Channel = '{0}' And ServerName = '{1}'", sConsoleMessage.Info[3].ToLower(), _servername));
					Log.Notice("Console", text[2]);
					SchumixBase.sCacheDB.Reload("channels");
				}
				else if(sConsoleMessage.Info[2].ToLower() == "update")
				{
					var text = sLManager.GetConsoleCommandTexts("channel/password/update");
					if(text.Length < 3)
					{
						Log.Error("Console", sLConsole.Translations("NoFound2"));
						return;
					}

					if(sConsoleMessage.Info.Length < 4)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoChannelName"));
						return;
					}

					if(!Rfc2812Util.IsValidChannelName(sConsoleMessage.Info[3]))
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NotaChannelHasBeenSet"));
						return;
					}

					if(sConsoleMessage.Info.Length < 5)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoPassword"));
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", sConsoleMessage.Info[3].ToLower(), _servername);
					if(db.IsNull())
					{
						Log.Warning("Console", text[0]);
						return;
					}

					db = SchumixBase.DManager.QueryFirstRow("SELECT Password FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", sConsoleMessage.Info[3].ToLower(), _servername);
					if(!db.IsNull())
					{
						if(db["Password"].ToString().IsNullOrEmpty())
						{
							Log.Notice("Console", text[1]);
							return;
						}
					}

					SchumixBase.DManager.Update("channels", string.Format("Password = '{0}'", sConsoleMessage.Info[4]), string.Format("Channel = '{0}' And ServerName = '{1}'", sConsoleMessage.Info[3].ToLower(), _servername));
					Log.Notice("Console", text[2], sConsoleMessage.Info[4]);
					SchumixBase.sCacheDB.Reload("channels");
				}
				else if(sConsoleMessage.Info[2].ToLower() == "info")
				{
					var text = sLManager.GetConsoleCommandTexts("channel/password/info");
					if(text.Length < 3)
					{
						Log.Error("Console", sLConsole.Translations("NoFound2"));
						return;
					}

					if(sConsoleMessage.Info.Length < 4)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoChannelName"));
						return;
					}

					if(!Rfc2812Util.IsValidChannelName(sConsoleMessage.Info[3]))
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NotaChannelHasBeenSet"));
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", sConsoleMessage.Info[3].ToLower(), _servername);
					if(db.IsNull())
					{
						Log.Warning("Console", text[0]);
						return;
					}

					db = SchumixBase.DManager.QueryFirstRow("SELECT Password FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", sConsoleMessage.Info[3].ToLower(), _servername);
					if(!db.IsNull())
					{
						if(db["Password"].ToString().IsNullOrEmpty())
							Log.Notice("Console", text[1]);
						else
							Log.Notice("Console", text[2]);
					}
				}
			}
		}
	}
}