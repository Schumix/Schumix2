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
using Schumix.Framework.Irc;
using Schumix.Framework.Config;

namespace Schumix.Irc.Commands
{
	public abstract partial class CommandHandler
	{
		protected void HandleIgnore(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
				return;

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoValue", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			if(sIRCMessage.Info[4].ToLower() == "irc")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("No1Value", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "command")
				{
					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("No1Value", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(sIRCMessage.Info[6].ToLower() == "add")
					{
						var text = sLManager.GetCommandTexts("ignore/irc/command/add", sIRCMessage.Channel, sIRCMessage.ServerName);
						if(text.Length < 2)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
							return;
						}

						if(sIRCMessage.Info.Length < 8)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoCommand", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						string command = sIRCMessage.Info[7].ToLower();

						if(sIgnoreIrcCommand.IsIgnore(command))
						{
							sSendMessage.SendChatMessage(sIRCMessage, text[0]);
							return;
						}

						sIgnoreIrcCommand.Add(command);
						sSendMessage.SendChatMessage(sIRCMessage, text[1]);
					}
					else if(sIRCMessage.Info[6].ToLower() == "remove")
					{
						var text = sLManager.GetCommandTexts("ignore/irc/command/remove", sIRCMessage.Channel, sIRCMessage.ServerName);
						if(text.Length < 2)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
							return;
						}

						if(sIRCMessage.Info.Length < 8)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoCommand", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						string command = sIRCMessage.Info[7].ToLower();

						if(!sIgnoreIrcCommand.IsIgnore(command))
						{
							sSendMessage.SendChatMessage(sIRCMessage, text[0]);
							return;
						}

						sIgnoreIrcCommand.Remove(command);
						sSendMessage.SendChatMessage(sIRCMessage, text[1]);
					}
					else if(sIRCMessage.Info[6].ToLower() == "search")
					{
						var text = sLManager.GetCommandTexts("ignore/irc/command/search", sIRCMessage.Channel, sIRCMessage.ServerName);
						if(text.Length < 2)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
							return;
						}

						if(sIRCMessage.Info.Length < 8)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoCommand", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						if(sIgnoreIrcCommand.Contains(sIRCMessage.Info[7].ToLower()))
							sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						else
							sSendMessage.SendChatMessage(sIRCMessage, text[1]);
					}
				}
			}
			else if(sIRCMessage.Info[4].ToLower() == "command")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("No1Value", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "add")
				{
					var text = sLManager.GetCommandTexts("ignore/command/add", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoCommand", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					string command = sIRCMessage.Info[6].ToLower();

					if(command == "ignore" || command == "admin")
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoIgnoreCommand", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(IsAdmin(sIRCMessage.Nick, AdminFlag.Operator) && sIrcBase.Networks[sIRCMessage.ServerName].CommandMethodMap.ContainsKey(command) && sIrcBase.Networks[sIRCMessage.ServerName].CommandMethodMap[command].Permission != CommandPermission.Administrator)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoAdministrator", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(sIgnoreCommand.IsIgnore(command))
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}

					sIgnoreCommand.Add(command);
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				}
				else if(sIRCMessage.Info[5].ToLower() == "remove")
				{
					var text = sLManager.GetCommandTexts("ignore/command/remove", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoCommand", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					string command = sIRCMessage.Info[6].ToLower();

					if(!sIgnoreCommand.IsIgnore(command))
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}

					sIgnoreCommand.Remove(command);
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				}
				else if(sIRCMessage.Info[5].ToLower() == "search")
				{
					var text = sLManager.GetCommandTexts("ignore/command/search", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoCommand", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(sIgnoreCommand.Contains(sIRCMessage.Info[6].ToLower()))
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
					else
						sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				}
			}
			else if(sIRCMessage.Info[4].ToLower() == "channel")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("No1Value", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "add")
				{
					var text = sLManager.GetCommandTexts("ignore/channel/add", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					string channel = sIRCMessage.Info[6].ToLower();

					if(!Rfc2812Util.IsValidChannelName(channel))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaChannelHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(channel == IRCConfig.List[sIRCMessage.ServerName].MasterChannel.ToLower())
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoIgnoreMasterChannel", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(sIgnoreChannel.IsIgnore(channel))
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}

					sIgnoreChannel.Add(channel);
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				}
				else if(sIRCMessage.Info[5].ToLower() == "remove")
				{
					var text = sLManager.GetCommandTexts("ignore/channel/remove", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					string channel = sIRCMessage.Info[6].ToLower();

					if(!Rfc2812Util.IsValidChannelName(channel))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaChannelHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(!sIgnoreChannel.IsIgnore(channel))
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}

					sIgnoreChannel.Remove(channel);
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				}
				else if(sIRCMessage.Info[5].ToLower() == "search")
				{
					var text = sLManager.GetCommandTexts("ignore/channel/search", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					string channel = sIRCMessage.Info[6].ToLower();

					if(!Rfc2812Util.IsValidChannelName(channel))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaChannelHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(sIgnoreChannel.Contains(channel))
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
					else
						sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				}
			}
			else if(sIRCMessage.Info[4].ToLower() == "nick")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("No1Value", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "add")
				{
					var text = sLManager.GetCommandTexts("ignore/nick/add", sIRCMessage.Channel, sIRCMessage.ServerName);
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

					string nick = sIRCMessage.Info[6].ToLower();

					if(!Rfc2812Util.IsValidNick(nick))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaNickNameHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(nick == sIRCMessage.Nick.ToLower())
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoIgnoreMyNick", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(IsAdmin(sIRCMessage.Nick, AdminFlag.Operator) && IsAdmin(nick, AdminFlag.Administrator))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoAdministrator", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(sIgnoreNickName.IsIgnore(nick))
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}

					sIgnoreNickName.Add(nick);
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				}
				else if(sIRCMessage.Info[5].ToLower() == "remove")
				{
					var text = sLManager.GetCommandTexts("ignore/nick/remove", sIRCMessage.Channel, sIRCMessage.ServerName);
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

					string nick = sIRCMessage.Info[6].ToLower();

					if(!Rfc2812Util.IsValidNick(nick))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaNickNameHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(!sIgnoreNickName.IsIgnore(nick))
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}

					sIgnoreNickName.Remove(nick);
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				}
				else if(sIRCMessage.Info[5].ToLower() == "search")
				{
					var text = sLManager.GetCommandTexts("ignore/nick/search", sIRCMessage.Channel, sIRCMessage.ServerName);
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

					string nick = sIRCMessage.Info[6].ToLower();
					
					if(!Rfc2812Util.IsValidNick(nick))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaNickNameHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(sIgnoreNickName.Contains(nick))
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
					else
						sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				}
			}
			else if(sIRCMessage.Info[4].ToLower() == "addon")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("No1Value", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "add")
				{
					var text = sLManager.GetCommandTexts("ignore/addon/add", sIRCMessage.Channel, sIRCMessage.ServerName);
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

					string addon = sIRCMessage.Info[6].ToLower();

					if(!sAddonManager.IsAddon(sIRCMessage.ServerName, addon))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ThereIsNoSuchAnAddon", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(sIgnoreAddon.IsIgnore(addon))
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}

					sIgnoreAddon.Add(addon);
					sIgnoreAddon.UnloadPlugin(addon);
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				}
				else if(sIRCMessage.Info[5].ToLower() == "remove")
				{
					var text = sLManager.GetCommandTexts("ignore/addon/remove", sIRCMessage.Channel, sIRCMessage.ServerName);
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

					string addon = sIRCMessage.Info[6].ToLower();

					if(!sAddonManager.IsAddon(sIRCMessage.ServerName, addon))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ThereIsNoSuchAnAddon", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(!sIgnoreAddon.IsIgnore(addon))
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}

					sIgnoreAddon.Remove(addon);
					sIgnoreAddon.LoadPlugin(addon);
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				}
				else if(sIRCMessage.Info[5].ToLower() == "search")
				{
					var text = sLManager.GetCommandTexts("ignore/addon/search", sIRCMessage.Channel, sIRCMessage.ServerName);
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

					string addon = sIRCMessage.Info[6].ToLower();

					if(!sAddonManager.IsAddon(sIRCMessage.ServerName, addon))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ThereIsNoSuchAnAddon", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(sIgnoreAddon.Contains(addon))
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
					else
						sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				}
			}
		}
	}
}