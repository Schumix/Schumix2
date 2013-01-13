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
using Schumix.API.Functions;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using Schumix.GameAddon.MaffiaGames;

namespace Schumix.GameAddon.Commands
{
	class GameCommand : CommandInfo
	{
		public readonly Dictionary<string, string> GameChannelFunction = new Dictionary<string, string>();
		public readonly Dictionary<string, MaffiaGame> MaffiaList = new Dictionary<string, MaffiaGame>();
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		public GameCommand sGC;

		public GameCommand(string ServerName) : base(ServerName)
		{
		}

		public void HandleGame(IRCMessage sIRCMessage)
		{
			var sMyChannelInfo = sIrcBase.Networks[sIRCMessage.ServerName].sMyChannelInfo;
			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;
			var sSender = sIrcBase.Networks[sIRCMessage.ServerName].sSender;

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoValue", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			if(sIRCMessage.Info[4].ToLower() == "start")
			{
				var text = sLManager.GetCommandTexts("game/start", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 4)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}

				if(!sUtilities.IsChannel(sIRCMessage.Channel))
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[0]);
					return;
				}

				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
					return;
				}

				if(sMyChannelInfo.FSelect(IChannelFunctions.Gamecommands, sIRCMessage.Channel))
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[2]);
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "maffiagame")
				{
					foreach(var maffia in MaffiaList)
					{
						if(sIRCMessage.Channel.ToLower() != maffia.Key)
						{
							foreach(var player in maffia.Value.GetPlayerList())
							{
								if(player.Value == sIRCMessage.Nick)
								{
									sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("game/start/maffiagame", sIRCMessage.Channel, sIRCMessage.ServerName), maffia.Value.DisableHl(sIRCMessage.Nick), maffia.Key);
									return;
								}
							}
						}
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT Functions FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(!db.IsNull())
					{
						if(!GameChannelFunction.ContainsKey(sIRCMessage.Channel.ToLower()))
							GameChannelFunction.Add(sIRCMessage.Channel.ToLower(), db["Functions"].ToString());
					}

					SchumixBase.DManager.Update("channels", string.Format("Functions = '{0}'", sUtilities.GetFunctionUpdate()), string.Format("Channel = '{0}' And ServerName = '{1}'", sIRCMessage.Channel.ToLower(), sIRCMessage.ServerName));
					sMyChannelInfo.ChannelFunctionsReload();
					SchumixBase.DManager.Update("channels", string.Format("Functions = '{0}'", sMyChannelInfo.ChannelFunctions("commands", SchumixBase.Off, sIRCMessage.Channel.ToLower())), string.Format("Channel = '{0}' And ServerName = '{1}'", sIRCMessage.Channel.ToLower(), sIRCMessage.ServerName));
					sMyChannelInfo.ChannelFunctionsReload();
					SchumixBase.DManager.Update("channels", string.Format("Functions = '{0}'", sMyChannelInfo.ChannelFunctions("gamecommands", SchumixBase.On, sIRCMessage.Channel.ToLower())), string.Format("Channel = '{0}' And ServerName = '{1}'", sIRCMessage.Channel.ToLower(), sIRCMessage.ServerName));
					sMyChannelInfo.ChannelFunctionsReload();
					sSender.Mode(sIRCMessage.Channel, "+v", sIRCMessage.Nick);

					if(!MaffiaList.ContainsKey(sIRCMessage.Channel.ToLower()))
						MaffiaList.Add(sIRCMessage.Channel.ToLower(), new MaffiaGame(sIRCMessage.ServerName, sIRCMessage.Nick, sIRCMessage.Channel, sGC));
					else
						MaffiaList[sIRCMessage.Channel.ToLower()].NewGame(sIRCMessage.Nick, sIRCMessage.Channel);
				}
				else
					sSendMessage.SendChatMessage(sIRCMessage, text[3]);
			}
		}
	}
}