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
using Schumix.Irc.Util;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Logger;
using Schumix.Console.Delegate;

namespace Schumix.Console
{
	/// <summary>
	///     CommandHandler class.
	/// </summary>
	partial class CommandHandler
	{
		/// <summary>
		///     Ignore parancs függvénye.
		/// </summary>
		protected void HandleIgnore(ConsoleMessage sConsoleMessage)
		{
			if(sConsoleMessage.Info.Length < 2)
			{
				Log.Error("Console", sLManager.GetConsoleWarningText("NoValue"));
				return;
			}

			if(sConsoleMessage.Info[1].ToLower() == "irc")
			{
				if(sConsoleMessage.Info.Length < 3)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("No1Value"));
					return;
				}

				if(sConsoleMessage.Info[2].ToLower() == "command")
				{
					if(sConsoleMessage.Info.Length < 4)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("No1Value"));
						return;
					}

					if(sConsoleMessage.Info[3].ToLower() == "add")
					{
						var text = sLManager.GetConsoleCommandTexts("ignore/irc/command/add");
						if(text.Length < 2)
						{
							Log.Error("Console", sLConsole.Translations("NoFound2"));
							return;
						}

						if(sConsoleMessage.Info.Length < 5)
						{
							Log.Error("Console", sLManager.GetConsoleWarningText("NoCommand"));
							return;
						}

						string command = sConsoleMessage.Info[4].ToLower();

						if(sIrcBase.Networks[_servername].sIgnoreIrcCommand.IsIgnore(command))
						{
							Log.Error("Console", text[0]);
							return;
						}

						sIrcBase.Networks[_servername].sIgnoreIrcCommand.Add(command);
						Log.Notice("Console", text[1]);
					}
					else if(sConsoleMessage.Info[3].ToLower() == "remove")
					{
						var text = sLManager.GetConsoleCommandTexts("ignore/irc/command/remove");
						if(text.Length < 2)
						{
							Log.Error("Console", sLConsole.Translations("NoFound2"));
							return;
						}

						if(sConsoleMessage.Info.Length < 5)
						{
							Log.Error("Console", sLManager.GetConsoleWarningText("NoCommand"));
							return;
						}

						string command = sConsoleMessage.Info[4].ToLower();

						if(!sIrcBase.Networks[_servername].sIgnoreIrcCommand.IsIgnore(command))
						{
							Log.Error("Console", text[0]);
							return;
						}

						sIrcBase.Networks[_servername].sIgnoreIrcCommand.Remove(command);
						Log.Notice("Console", text[1]);
					}
					else if(sConsoleMessage.Info[3].ToLower() == "search")
					{
						var text = sLManager.GetConsoleCommandTexts("ignore/irc/command/search");
						if(text.Length < 2)
						{
							Log.Error("Console", sLConsole.Translations("NoFound2"));
							return;
						}

						if(sConsoleMessage.Info.Length < 5)
						{
							Log.Error("Console", sLManager.GetConsoleWarningText("NoCommand"));
							return;
						}

						if(sIrcBase.Networks[_servername].sIgnoreIrcCommand.Contains(sConsoleMessage.Info[4].ToLower()))
							Log.Notice("Console", text[0]);
						else
							Log.Error("Console", text[1]);
					}
				}
			}
			else if(sConsoleMessage.Info[1].ToLower() == "command")
			{
				if(sConsoleMessage.Info.Length < 3)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("No1Value"));
					return;
				}

				if(sConsoleMessage.Info[2].ToLower() == "add")
				{
					var text = sLManager.GetConsoleCommandTexts("ignore/command/add");
					if(text.Length < 2)
					{
						Log.Error("Console", sLConsole.Translations("NoFound2"));
						return;
					}

					if(sConsoleMessage.Info.Length < 4)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoCommand"));
						return;
					}

					string command = sConsoleMessage.Info[3].ToLower();

					if(command == "ignore" || command == "admin")
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoIgnoreCommand"));
						return;
					}

					if(sIrcBase.Networks[_servername].sIgnoreCommand.IsIgnore(command))
					{
						Log.Error("Console", text[0]);
						return;
					}

					sIrcBase.Networks[_servername].sIgnoreCommand.Add(command);
					Log.Notice("Console", text[1]);
				}
				else if(sConsoleMessage.Info[2].ToLower() == "remove")
				{
					var text = sLManager.GetConsoleCommandTexts("ignore/command/remove");
					if(text.Length < 2)
					{
						Log.Error("Console", sLConsole.Translations("NoFound2"));
						return;
					}

					if(sConsoleMessage.Info.Length < 4)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoCommand"));
						return;
					}

					string command = sConsoleMessage.Info[3].ToLower();

					if(!sIrcBase.Networks[_servername].sIgnoreCommand.IsIgnore(command))
					{
						Log.Error("Console", text[0]);
						return;
					}

					sIrcBase.Networks[_servername].sIgnoreCommand.Remove(command);
					Log.Notice("Console", text[1]);
				}
				else if(sConsoleMessage.Info[2].ToLower() == "search")
				{
					var text = sLManager.GetConsoleCommandTexts("ignore/command/search");
					if(text.Length < 2)
					{
						Log.Error("Console", sLConsole.Translations("NoFound2"));
						return;
					}

					if(sConsoleMessage.Info.Length < 4)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoCommand"));
						return;
					}

					if(sIrcBase.Networks[_servername].sIgnoreCommand.Contains(sConsoleMessage.Info[3].ToLower()))
						Log.Notice("Console", text[0]);
					else
						Log.Error("Console", text[1]);
				}
			}
			else if(sConsoleMessage.Info[1].ToLower() == "channel")
			{
				if(sConsoleMessage.Info.Length < 3)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("No1Value"));
					return;
				}

				if(sConsoleMessage.Info[2].ToLower() == "add")
				{
					var text = sLManager.GetConsoleCommandTexts("ignore/channel/add");
					if(text.Length < 2)
					{
						Log.Error("Console", sLConsole.Translations("NoFound2"));
						return;
					}

					if(sConsoleMessage.Info.Length < 4)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoChannelName"));
						return;
					}

					string channel = sConsoleMessage.Info[3].ToLower();

					if(!Rfc2812Util.IsValidChannelName(channel))
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NotaChannelHasBeenSet"));
						return;
					}

					if(channel == IRCConfig.List[_servername].MasterChannel.ToLower())
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoIgnoreMasterChannel"));
						return;
					}

					if(sIrcBase.Networks[_servername].sIgnoreChannel.IsIgnore(channel))
					{
						Log.Error("Console", text[0]);
						return;
					}

					sIrcBase.Networks[_servername].sIgnoreChannel.Add(channel);
					Log.Notice("Console", text[1]);
				}
				else if(sConsoleMessage.Info[2].ToLower() == "remove")
				{
					var text = sLManager.GetConsoleCommandTexts("ignore/channel/remove");
					if(text.Length < 2)
					{
						Log.Error("Console", sLConsole.Translations("NoFound2"));
						return;
					}

					if(sConsoleMessage.Info.Length < 4)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoChannelName"));
						return;
					}

					string channel = sConsoleMessage.Info[3].ToLower();

					if(!Rfc2812Util.IsValidChannelName(channel))
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NotaChannelHasBeenSet"));
						return;
					}

					if(!sIrcBase.Networks[_servername].sIgnoreChannel.IsIgnore(channel))
					{
						Log.Error("Console", text[0]);
						return;
					}

					sIrcBase.Networks[_servername].sIgnoreChannel.Remove(channel);
					Log.Notice("Console", text[1]);
				}
				else if(sConsoleMessage.Info[2].ToLower() == "search")
				{
					var text = sLManager.GetConsoleCommandTexts("ignore/channel/search");
					if(text.Length < 2)
					{
						Log.Error("Console", sLConsole.Translations("NoFound2"));
						return;
					}

					if(sConsoleMessage.Info.Length < 4)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoChannelName"));
						return;
					}

					string channel = sConsoleMessage.Info[3].ToLower();

					if(!Rfc2812Util.IsValidChannelName(channel))
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NotaChannelHasBeenSet"));
						return;
					}

					if(sIrcBase.Networks[_servername].sIgnoreChannel.Contains(channel))
						Log.Notice("Console", text[0]);
					else
						Log.Error("Console", text[1]);
				}
			}
			else if(sConsoleMessage.Info[1].ToLower() == "nick")
			{
				if(sConsoleMessage.Info.Length < 3)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("No1Value"));
					return;
				}

				if(sConsoleMessage.Info[2].ToLower() == "add")
				{
					var text = sLManager.GetConsoleCommandTexts("ignore/nick/add");
					if(text.Length < 2)
					{
						Log.Error("Console", sLConsole.Translations("NoFound2"));
						return;
					}

					if(sConsoleMessage.Info.Length < 4)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoName"));
						return;
					}

					string nick = sConsoleMessage.Info[3].ToLower();

					if(!Rfc2812Util.IsValidNick(nick))
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NotaNickNameHasBeenSet"));
						return;
					}

					if(sIrcBase.Networks[_servername].sIgnoreNickName.IsIgnore(nick))
					{
						Log.Error("Console", text[0]);
						return;
					}

					sIrcBase.Networks[_servername].sIgnoreNickName.Add(nick);
					Log.Notice("Console", text[1]);
				}
				else if(sConsoleMessage.Info[2].ToLower() == "remove")
				{
					var text = sLManager.GetConsoleCommandTexts("ignore/nick/remove");
					if(text.Length < 2)
					{
						Log.Error("Console", sLConsole.Translations("NoFound2"));
						return;
					}

					if(sConsoleMessage.Info.Length < 4)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoName"));
						return;
					}

					string nick = sConsoleMessage.Info[3].ToLower();

					if(!Rfc2812Util.IsValidNick(nick))
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NotaNickNameHasBeenSet"));
						return;
					}

					if(!sIrcBase.Networks[_servername].sIgnoreNickName.IsIgnore(nick))
					{
						Log.Error("Console", text[0]);
						return;
					}

					sIrcBase.Networks[_servername].sIgnoreNickName.Remove(nick);
					Log.Notice("Console", text[1]);
				}
				else if(sConsoleMessage.Info[2].ToLower() == "search")
				{
					var text = sLManager.GetConsoleCommandTexts("ignore/nick/search");
					if(text.Length < 2)
					{
						Log.Error("Console", sLConsole.Translations("NoFound2"));
						return;
					}

					if(sConsoleMessage.Info.Length < 4)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoName"));
						return;
					}

					string nick = sConsoleMessage.Info[3].ToLower();

					if(!Rfc2812Util.IsValidNick(nick))
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NotaNickNameHasBeenSet"));
						return;
					}

					if(sIrcBase.Networks[_servername].sIgnoreNickName.Contains(nick))
						Log.Notice("Console", text[0]);
					else
						Log.Error("Console", text[1]);
				}
			}
			else if(sConsoleMessage.Info[1].ToLower() == "addon")
			{
				if(sConsoleMessage.Info.Length < 3)
				{
					Log.Error("Console", sLManager.GetConsoleWarningText("No1Value"));
					return;
				}

				if(sConsoleMessage.Info[2].ToLower() == "add")
				{
					var text = sLManager.GetConsoleCommandTexts("ignore/addon/add");
					if(text.Length < 2)
					{
						Log.Error("Console", sLConsole.Translations("NoFound2"));
						return;
					}

					if(sConsoleMessage.Info.Length < 4)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoName"));
						return;
					}

					string addon = sConsoleMessage.Info[3].ToLower();

					if(!sAddonManager.IsAddon(_servername, addon))
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("ThereIsNoSuchAnAddon"));
						return;
					}

					if(sIrcBase.Networks[_servername].sIgnoreAddon.IsIgnore(addon))
					{
						Log.Error("Console", text[0]);
						return;
					}

					sIrcBase.Networks[_servername].sIgnoreAddon.Add(addon);
					sIrcBase.Networks[_servername].sIgnoreAddon.UnloadPlugin(addon);
					Log.Notice("Console", text[1]);
				}
				else if(sConsoleMessage.Info[2].ToLower() == "remove")
				{
					var text = sLManager.GetConsoleCommandTexts("ignore/addon/remove");
					if(text.Length < 2)
					{
						Log.Error("Console", sLConsole.Translations("NoFound2"));
						return;
					}

					if(sConsoleMessage.Info.Length < 4)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoName"));
						return;
					}

					string addon = sConsoleMessage.Info[3].ToLower();

					if(!sAddonManager.IsAddon(_servername, addon))
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("ThereIsNoSuchAnAddon"));
						return;
					}

					if(!sIrcBase.Networks[_servername].sIgnoreAddon.IsIgnore(addon))
					{
						Log.Error("Console", text[0]);
						return;
					}

					sIrcBase.Networks[_servername].sIgnoreAddon.Remove(addon);
					sIrcBase.Networks[_servername].sIgnoreAddon.LoadPlugin(addon);
					Log.Notice("Console", text[1]);
				}
				else if(sConsoleMessage.Info[2].ToLower() == "search")
				{
					var text = sLManager.GetConsoleCommandTexts("ignore/addon/search");
					if(text.Length < 2)
					{
						Log.Error("Console", sLConsole.Translations("NoFound2"));
						return;
					}

					if(sConsoleMessage.Info.Length < 4)
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("NoName"));
						return;
					}

					string addon = sConsoleMessage.Info[3].ToLower();

					if(!sAddonManager.IsAddon(_servername, addon))
					{
						Log.Error("Console", sLManager.GetConsoleWarningText("ThereIsNoSuchAnAddon"));
						return;
					}

					if(sIrcBase.Networks[_servername].sIgnoreAddon.Contains(addon))
						Log.Notice("Console", text[0]);
					else
						Log.Error("Console", text[1]);
				}
			}
		}

	}
}