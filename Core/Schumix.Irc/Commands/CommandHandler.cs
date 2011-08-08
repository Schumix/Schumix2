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
using Schumix.API;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Irc.Commands
{
	public partial class CommandHandler : CommandInfo
	{
		protected readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		protected readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		protected readonly AddonManager sAddonManager = Singleton<AddonManager>.Instance;
		protected readonly ChannelInfo sChannelInfo = Singleton<ChannelInfo>.Instance;
		protected readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;
		protected readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		protected readonly NickInfo sNickInfo = Singleton<NickInfo>.Instance;
		protected readonly NickName sNickName = Singleton<NickName>.Instance;
		protected readonly Sender sSender = Singleton<Sender>.Instance;
		protected string ChannelPrivmsg { get; set; }
		protected string WhoisPrivmsg { get; set; }
		protected CommandHandler() {}

		protected void HandleHelp(IRCMessage sIRCMessage)
		{
			CNick(sIRCMessage);

			if(sIRCMessage.Info.Length == 4)
			{
				var text = sLManager.GetCommandTexts("help", sIRCMessage.Channel);
				if(text.Length < 2)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
					return;
				}

				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1], IRCConfig.CommandPrefix);
				return;
			}

			foreach(var plugin in sAddonManager.GetPlugins())
			{
				if(plugin.HandleHelp(sIRCMessage))
					return;
			}

			string command = IRCConfig.NickName + ",";

			if(sIRCMessage.Info[4].ToLower() == command.ToLower())
			{
				if(sIRCMessage.Info.Length < 6)
				{
					var text = sLManager.GetCommandHelpTexts("schumix2", sIRCMessage.Channel);
					if(text.Length < 4)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						return;
					}

					if(IsAdmin(sIRCMessage.Nick, AdminFlag.HalfOperator))
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
					else if(IsAdmin(sIRCMessage.Nick, AdminFlag.Operator))
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1]);
					else if(IsAdmin(sIRCMessage.Nick, AdminFlag.Administrator))
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[2]);
					else
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[3]);

					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "sys")
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandHelpText("schumix2/sys", sIRCMessage.Channel));
				}
				else if(sIRCMessage.Info[5].ToLower() == "ghost" && IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
				{
					var text = sLManager.GetCommandHelpTexts("schumix2/ghost", sIRCMessage.Channel);
					if(text.Length < 2)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						return;
					}

					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1], command.ToLower());
				}
				else if(sIRCMessage.Info[5].ToLower() == "nick" && IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.HalfOperator))
				{
					if(sIRCMessage.Info.Length < 7)
					{
						var text = sLManager.GetCommandHelpTexts("schumix2/nick", sIRCMessage.Channel);
						if(text.Length < 2)
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
							return;
						}

						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1], command.ToLower());
						return;
					}

					if(sIRCMessage.Info[6].ToLower() == "identify")
					{
						var text = sLManager.GetCommandHelpTexts("schumix2/nick/identify", sIRCMessage.Channel);
						if(text.Length < 2)
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
							return;
						}

						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1], command.ToLower());
					}
				}
				else if(sIRCMessage.Info[5].ToLower() == "clean" && IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Administrator))
				{
					var text = sLManager.GetCommandHelpTexts("schumix2/clean", sIRCMessage.Channel);
					if(text.Length < 2)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						return;
					}

					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1], command.ToLower());
				}
			}
			else
			{
				if(!CommandManager.GetPublicCommandHandler().ContainsKey(sIRCMessage.Info[4].ToLower()) &&
					!CommandManager.GetHalfOperatorCommandHandler().ContainsKey(sIRCMessage.Info[4].ToLower()) &&
					!CommandManager.GetOperatorCommandHandler().ContainsKey(sIRCMessage.Info[4].ToLower()) &&
					!CommandManager.GetAdminCommandHandler().ContainsKey(sIRCMessage.Info[4].ToLower()))
					return;

				int adminflag = Adminflag(sIRCMessage.Nick, sIRCMessage.Host);

				if(adminflag != -1)
				{
					string commands = sIRCMessage.Info.SplitToString(4, "/");
					int rank = sLManager.GetCommandHelpRank(commands, sIRCMessage.Channel);

					if((adminflag == 2 && rank == 2) || (adminflag == 2 && rank == 1) || (adminflag == 2 && rank == 0) ||
					(adminflag == 1 && rank == 1) || (adminflag == 1 && rank == 0) || (adminflag == 0 && rank == 0) ||
					(adminflag == 2 && rank == 9) || (adminflag == 1 && rank == 9) || (adminflag == 0 && rank == 9))
						HelpMessage(sIRCMessage, sLManager.GetCommandHelpTexts(commands, sIRCMessage.Channel, rank));
				}
				else
				{
					string commands = sIRCMessage.Info.SplitToString(4, "/");
					if(sLManager.IsAdminCommandHelp(commands, sIRCMessage.Channel))
						return;

					HelpMessage(sIRCMessage, sLManager.GetCommandHelpTexts(commands, sIRCMessage.Channel));
				}
			}
		}

		private void HelpMessage(IRCMessage sIRCMessage, string[] text)
		{
			foreach(var t in text)
			{
				if(t.Contains("{0}"))
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, t, IRCConfig.CommandPrefix);
				else
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, t);
			}
		}
	}
}