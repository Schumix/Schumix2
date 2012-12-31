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
using System.Threading;
using Schumix.API;
using Schumix.API.Irc;
using Schumix.API.Functions;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.ExtraAddon.Commands
{
	class IrcHandler : CommandInfo
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private Functions sFunctions;
		public NameList sNameList { get; private set; }
		public bool AutoMode = false;
		public string ModeChannel;

		public IrcHandler(string ServerName, Functions fs) : base(ServerName)
		{
			sFunctions = fs;
			sNameList = new NameList(ServerName, fs);
		}

		/// <summary>
		///     Ha a szobában a köszönés funkció be van kapcsolva,
		///     akkor köszön az éppen belépőnek.
		/// </summary>
		public void HandleJoin(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Nick == sIrcBase.Networks[sIRCMessage.ServerName].sNickInfo.NickStorage)
				return;

			if(sFunctions.AutoKick("join", sIRCMessage.Nick, sIRCMessage.Channel))
				return;

			if(sIrcBase.Networks[sIRCMessage.ServerName].sIgnoreNickName.IsIgnore(sIRCMessage.Nick))
				return;

			var sChannelInfo = sIrcBase.Networks[sIRCMessage.ServerName].sChannelInfo;
			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;
			var sSender = sIrcBase.Networks[sIRCMessage.ServerName].sSender;
			sIRCMessage.Channel = sIRCMessage.Channel.Remove(0, 1, SchumixBase.Colon);
			sNameList.Add(sIRCMessage.Channel, sIRCMessage.Nick);

			if(sChannelInfo.FSelect(IFunctions.Automode) && sChannelInfo.FSelect(IChannelFunctions.Automode, sIRCMessage.Channel))
			{
				AutoMode = true;
				ModeChannel = sIRCMessage.Channel;
				sSender.NickServStatus(sIRCMessage.Nick);
			}

			if(sChannelInfo.FSelect(IFunctions.Greeter) && sChannelInfo.FSelect(IChannelFunctions.Greeter, sIRCMessage.Channel))
			{
				var rand = new Random();
				string greeter = string.Empty;
				var text = sLManager.GetCommandTexts("handlejoin", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 3)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}

				var text2 = sLManager.GetCommandTexts("handlejoin/random", sIRCMessage.Channel, sIRCMessage.ServerName);
				greeter = text2[rand.Next(0, text2.Length-1)];

				if(DateTime.Now.Hour >= 3 && DateTime.Now.Hour <= 9)
					sSendMessage.SendChatMessage(sIRCMessage, text[0], sIRCMessage.Nick);
				else if(DateTime.Now.Hour >= 20 && DateTime.Now.Hour < 3)
					sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Nick);
				else
				{
					if(IsAdmin(sIRCMessage.Nick, sIRCMessage.Host))
						sSendMessage.SendChatMessage(sIRCMessage, text[2]);
					else
						sSendMessage.SendChatMessage(sIRCMessage, "{0} {1}", greeter, sIRCMessage.Nick);
				}
			}
		}

		/// <summary>
		///     Ha ez a funkció be van kapcsolva, akkor
		///     miután a nick elhagyta a szobát elköszön tőle.
		/// </summary>
		public void HandleLeft(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Nick == sIrcBase.Networks[sIRCMessage.ServerName].sNickInfo.NickStorage)
			{
				sNameList.Remove(sIRCMessage.ServerName, sIRCMessage.Channel);
				return;
			}

			if(sIrcBase.Networks[sIRCMessage.ServerName].sIgnoreNickName.IsIgnore(sIRCMessage.Nick))
				return;

			var sChannelInfo = sIrcBase.Networks[sIRCMessage.ServerName].sChannelInfo;
			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;
			sNameList.Remove(sIRCMessage.Nick);

			if(sChannelInfo.FSelect(IFunctions.Greeter) && sChannelInfo.FSelect(IChannelFunctions.Greeter, sIRCMessage.Channel))
			{
				var rand = new Random();
				string greeter = string.Empty;
				var text2 = sLManager.GetCommandTexts("handleleft/random", sIRCMessage.Channel, sIRCMessage.ServerName);
				greeter = text2[rand.Next(0, text2.Length-1)];

				if(DateTime.Now.Hour >= 20 && DateTime.Now.Hour < 3)
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("handleleft", sIRCMessage.Channel, sIRCMessage.ServerName), sIRCMessage.Nick);
				else
					sSendMessage.SendChatMessage(sIRCMessage, "{0} {1}", greeter, sIRCMessage.Nick);
			}
		}

		public void HandleQuit(IRCMessage sIRCMessage)
		{
			sNameList.Remove(string.Empty, sIRCMessage.Nick, true);
		}

		public void HandleNewNick(IRCMessage sIRCMessage)
		{
			if(!SchumixBase.NewNick)
				sNameList.Change(sIRCMessage.Nick, sIRCMessage.Info[2].Remove(0, 1, SchumixBase.Colon));
			else
				SchumixBase.NewNick = false;
		}

		/// <summary>
		///     Ha engedélyezett a ConsolLog, akkor kiírja a Console-ra ha kickelnek valakit.
		/// </summary>
		public void HandleKick(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info.Length < 5)
				return;

			var sChannelInfo = sIrcBase.Networks[sIRCMessage.ServerName].sChannelInfo;
			var sSender = sIrcBase.Networks[sIRCMessage.ServerName].sSender;

			if(sIRCMessage.Info[3] == sIrcBase.Networks[sIRCMessage.ServerName].sNickInfo.NickStorage)
			{
				sNameList.Remove(sIRCMessage.Channel);

				if(sChannelInfo.FSelect(IFunctions.Rejoin) && sChannelInfo.FSelect(IChannelFunctions.Rejoin, sIRCMessage.Channel))
				{
					foreach(var m_channel in sChannelInfo.CList)
					{
						if(sIRCMessage.Channel.ToLower() == m_channel.Key)
						{
							Thread.Sleep(5000);
							sSender.Join(m_channel.Key, m_channel.Value);
						}
					}
				}
			}
			else
			{
				sNameList.Remove(sIRCMessage.Info[3]);

				if(sChannelInfo.FSelect(IFunctions.Commands) && sChannelInfo.FSelect(IChannelFunctions.Commands, sIRCMessage.Channel))
				{
					if(ConsoleLog.CLog)
					{
						string text = sIRCMessage.Info.SplitToString(4, SchumixBase.Space);
						Console.WriteLine(sLManager.GetCommandText("handlekick", sIRCMessage.Channel, sIRCMessage.ServerName), sIRCMessage.Nick, sIRCMessage.Info[3], text.Remove(0, 1, ":"));
					}
				}
			}
		}

		public void HandleNameList(IRCMessage sIRCMessage)
		{
			int i = 0;
			var split = sIRCMessage.Args.Split(SchumixBase.Space);
			string Channel = split[1];

			if(!sNameList.Channels.ContainsKey(sIRCMessage.Channel.ToLower()))
				sNameList.Channels.Add(sIRCMessage.Channel.ToLower(), false);

			if(sNameList.Channels.ContainsKey(sIRCMessage.Channel.ToLower()) && sNameList.Channels[sIRCMessage.Channel.ToLower()])
				sNameList.Remove(Channel);

			foreach(var name in sIRCMessage.Args.Split(SchumixBase.Space))
			{
				i++;

				if(i < 3)
					continue;

				sNameList.Add(Channel, sIrcBase.Networks[sIRCMessage.ServerName].sNickInfo.Parse(name));
			}
		}

		public void HandleEndNameList(IRCMessage sIRCMessage)
		{
			if(sNameList.Channels.ContainsKey(sIRCMessage.Channel.ToLower()))
				sNameList.Channels[sIRCMessage.Channel.ToLower()] = true;
		}

		public void HandleSuccessfulAuth(IRCMessage sIRCMessage)
		{
			sNameList.Channels.Clear();
		}
	}
}