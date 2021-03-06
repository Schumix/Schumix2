/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013-2015 Schumix Team <http://schumix.eu/>
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
using System.Collections.Generic;
using Schumix.Irc.Ctcp;
using Schumix.Irc.Flood;
using Schumix.Irc.Ignore;
using Schumix.Irc.Logger;
using Schumix.Irc.Channel;
using Schumix.Irc.NickName;
using Schumix.Framework;
using Schumix.Framework.Irc;
using Schumix.Framework.Util;
using Schumix.Framework.Addon;
using Schumix.Framework.Config;
using Schumix.Framework.Logger;
using Schumix.Framework.Listener;
using Schumix.Framework.Platforms;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Irc.Commands
{
	public abstract partial class CommandHandler : CommandInfo
	{
		protected readonly SchumixPacketHandler sSchumixPacketHandler = Singleton<SchumixPacketHandler>.Instance;
		protected readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		protected readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		protected readonly Dictionary<string, Whois> WhoisList = new Dictionary<string, Whois>();
		protected readonly AddonManager sAddonManager = Singleton<AddonManager>.Instance;
		protected readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		protected readonly Platform sPlatform = Singleton<Platform>.Instance;
		protected readonly Runtime sRuntime = Singleton<Runtime>.Instance;
		protected readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		public IgnoreIrcCommand sIgnoreIrcCommand { get; private set; }
		public IgnoreNickName sIgnoreNickName { get; private set; }
		public IgnoreChannel sIgnoreChannel { get; private set; }
		public IgnoreCommand sIgnoreCommand { get; private set; }
		public MyChannelInfo sMyChannelInfo { get; private set; }
		public ChannelList sChannelList { get; private set; }
		public IgnoreAddon sIgnoreAddon { get; private set; }
		public SendMessage sSendMessage { get; private set; }
		public CtcpSender sCtcpSender { get; private set; }
		public MyNickInfo sMyNickInfo { get; private set; }
		public AntiFlood sAntiFlood { get; private set; }
		public Sender sSender { get; private set; }
		public IrcLog sIrcLog { get; private set; }
		protected string ChannelPrivmsg { get; set; }
		protected string NewNickPrivmsg { get; set; }
		protected string OnlinePrivmsg { get; set; }
		protected string KickPrivmsg { get; set; }
		protected string ModePrivmsg { get; set; }
		protected bool IsOnline { get; set; }
		private string _servername;

		protected CommandHandler(string ServerName) : base(ServerName)
		{
			_servername = ServerName;
		}

		~CommandHandler()
		{
			Log.Debug("CommandHandler", "~CommandHandler() {0}", sLConsole.GetString("[ServerName: {0}]", _servername));
		}

		public void InitializeIgnoreCommand()
		{
			sIgnoreCommand = new IgnoreCommand(_servername);
		}

		public void InitializeCommandHandler()
		{
			sSendMessage = new SendMessage(_servername);
			sIgnoreChannel = new IgnoreChannel(_servername);
			sSender = new Sender(_servername);
			sMyNickInfo = new MyNickInfo(_servername);
			sIgnoreAddon = new IgnoreAddon(_servername);
			sIgnoreCommand = new IgnoreCommand(_servername);
			sIgnoreNickName = new IgnoreNickName(_servername);
			sIgnoreIrcCommand = new IgnoreIrcCommand(_servername);
			sMyChannelInfo = new MyChannelInfo(_servername);
			sIrcLog = new IrcLog(_servername);
			sAntiFlood = new AntiFlood(_servername);
			sCtcpSender = new CtcpSender(_servername);
			sChannelList = new ChannelList(_servername);
		}

		protected void HandleHelp(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info.Length == 4)
			{
				var text = sLManager.GetCommandTexts("help", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 2)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}

				sSendMessage.SendChatMessage(sIRCMessage, text[0]);
				sSendMessage.SendChatMessage(sIRCMessage, text[1], IRCConfig.List[sIRCMessage.ServerName].CommandPrefix);
				return;
			}

			foreach(var plugin in sAddonManager.Addons[sIRCMessage.ServerName].Addons)
			{
				if(!sAddonManager.Addons[sIRCMessage.ServerName].IgnoreAssemblies.ContainsKey(plugin.Key) &&
				   plugin.Value.HandleHelp(sIRCMessage))
					return;
			}

			string command = IRCConfig.List[sIRCMessage.ServerName].NickName + SchumixBase.Comma;

			if(sIRCMessage.Info[4].ToLower() == command.ToLower())
			{
				if(sIRCMessage.Info.Length < 6)
				{
					var text = sLManager.GetCommandHelpTexts("schumix2", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 4)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					if(IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.HalfOperator))
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
					else if(IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
						sSendMessage.SendChatMessage(sIRCMessage, text[1]);
					else if(IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Administrator))
						sSendMessage.SendChatMessage(sIRCMessage, text[2]);
					else
						sSendMessage.SendChatMessage(sIRCMessage, text[3]);

					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "sys")
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandHelpText("schumix2/sys", sIRCMessage.Channel, sIRCMessage.ServerName));
				}
				else if(sIRCMessage.Info[5].ToLower() == "ghost")
				{
					if(IsWarningAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("WarningAdmin", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
						return;

					var text = sLManager.GetCommandHelpTexts("schumix2/ghost", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					sSendMessage.SendChatMessage(sIRCMessage, text[0]);
					sSendMessage.SendChatMessage(sIRCMessage, text[1], command.ToLower());
				}
				else if(sIRCMessage.Info[5].ToLower() == "nick")
				{
					if(IsWarningAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.HalfOperator))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("WarningAdmin", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.HalfOperator))
						return;

					if(sIRCMessage.Info.Length < 7)
					{
						var text = sLManager.GetCommandHelpTexts("schumix2/nick", sIRCMessage.Channel, sIRCMessage.ServerName);
						if(text.Length < 2)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
							return;
						}

						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						sSendMessage.SendChatMessage(sIRCMessage, text[1], command.ToLower());
						return;
					}

					if(sIRCMessage.Info[6].ToLower() == "identify")
					{
						var text = sLManager.GetCommandHelpTexts("schumix2/nick/identify", sIRCMessage.Channel, sIRCMessage.ServerName);
						if(text.Length < 2)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
							return;
						}

						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						sSendMessage.SendChatMessage(sIRCMessage, text[1], command.ToLower());
					}
				}
				else if(sIRCMessage.Info[5].ToLower() == "clean")
				{
					if(IsWarningAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Administrator))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("WarningAdmin", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Administrator))
						return;

					var text = sLManager.GetCommandHelpTexts("schumix2/clean", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					sSendMessage.SendChatMessage(sIRCMessage, text[0]);
					sSendMessage.SendChatMessage(sIRCMessage, text[1], command.ToLower());
				}
			}
			else
			{
				if(!sIrcBase.Networks[sIRCMessage.ServerName].CommandMethodMap.ContainsKey(sIRCMessage.Info[4].ToLower()))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Other("NoFoundHelpCommand2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}

				if(sIgnoreCommand.IsIgnore(sIRCMessage.Info[4].ToLower()))
					return;

				string aliascommand = string.Empty;
				var adminflag = Adminflag(sIRCMessage.Nick);

				var db = SchumixBase.DManager.QueryFirstRow("SELECT BaseCommand FROM alias_irc_command WHERE NewCommand = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[4].ToLower()), sIRCMessage.ServerName);
				if(!db.IsNull())
				{
					aliascommand = sIRCMessage.Info[4].ToLower();
					string basecommand = db["BaseCommand"].ToString();
					sIRCMessage.Info[4] = basecommand;
				}

				if(adminflag != AdminFlag.None)
				{
					string commands = sIRCMessage.Info.SplitToString(4, "/");
					int rank = sLManager.GetCommandHelpRank(commands, sIRCMessage.Channel, sIRCMessage.ServerName);

					if(rank == -1)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Other("NoFoundHelpCommand", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					if(IsWarningAdmin(sIRCMessage.Nick, sIRCMessage.Host, (AdminFlag)rank))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("WarningAdmin", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if((adminflag == AdminFlag.Administrator && rank == 2) || (adminflag == AdminFlag.Administrator && rank == 1) ||
					   (adminflag == AdminFlag.Administrator && rank == 0) || (adminflag == AdminFlag.Operator && rank == 1) ||
					   (adminflag == AdminFlag.Operator && rank == 0) || (adminflag == AdminFlag.HalfOperator && rank == 0) ||
					   (adminflag == AdminFlag.Administrator && rank == 9) || (adminflag == AdminFlag.Operator && rank == 9) ||
					   (adminflag == AdminFlag.HalfOperator && rank == 9))
						HelpMessage(sIRCMessage, sLManager.GetCommandHelpTexts(commands, sIRCMessage.Channel, sIRCMessage.ServerName, rank), aliascommand);
				}
				else
				{
					string commands = sIRCMessage.Info.SplitToString(4, "/");
					if(sLManager.IsAdminCommandHelp(commands, sIRCMessage.Channel, sIRCMessage.ServerName))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoAdministrator", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					HelpMessage(sIRCMessage, sLManager.GetCommandHelpTexts(commands, sIRCMessage.Channel, sIRCMessage.ServerName), aliascommand);
				}
			}
		}

		private void HelpMessage(IRCMessage sIRCMessage, string[] text, string AliasCommand)
		{
			foreach(var t in text)
			{
				if(!AliasCommand.IsNullOrEmpty())
				{
					if(t.Contains("{0}"))
					{
						string message = string.Format(t, IRCConfig.List[sIRCMessage.ServerName].CommandPrefix);
						sSendMessage.SendChatMessage(sIRCMessage, message.Replace(string.Format("{0}{1}", IRCConfig.List[sIRCMessage.ServerName].CommandPrefix, sIRCMessage.Info[4]), string.Format("{0}{1}", IRCConfig.List[sIRCMessage.ServerName].CommandPrefix, AliasCommand)));
					}
					else
						sSendMessage.SendChatMessage(sIRCMessage, t);
				}
				else
				{
					if(t.Contains("{0}"))
						sSendMessage.SendChatMessage(sIRCMessage, t, IRCConfig.List[sIRCMessage.ServerName].CommandPrefix);
					else
						sSendMessage.SendChatMessage(sIRCMessage, t);
				}
			}
		}
	}
}