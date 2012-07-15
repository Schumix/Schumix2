/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Megax <http://www.megaxx.info/>
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
		private readonly ChannelInfo sChannelInfo = Singleton<ChannelInfo>.Instance;
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;
		private readonly Functions sFunctions = Singleton<Functions>.Instance;
		private readonly NameList sNameList = Singleton<NameList>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private readonly Sender sSender = Singleton<Sender>.Instance;
		protected bool AutoMode = false;
		protected string ModeChannel;

		/// <summary>
		///     Ha a szobában a köszönés funkció be van kapcsolva,
		///     akkor köszön az éppen belépőnek.
		/// </summary>
		protected void HandleJoin(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Nick == sIrcBase.Networks[sIRCMessage.ServerName].sNickInfo.NickStorage)
				return;

			if(sFunctions.AutoKick("join", sIRCMessage.Nick, sIRCMessage.Channel))
				return;

			sIRCMessage.Channel = sIRCMessage.Channel.Remove(0, 1, SchumixBase.Colon);
			sNameList.Add(sIRCMessage.ServerName, sIRCMessage.Channel, sIRCMessage.Nick);

			if(sChannelInfo.FSelect(IFunctions.Automode) && sChannelInfo.FSelect(IChannelFunctions.Automode, sIRCMessage.Channel))
			{
				AutoMode = true;
				ModeChannel = sIRCMessage.Channel;
				sSender.NickServStatus(sIRCMessage.ServerName, sIRCMessage.Nick);
			}

			if(sChannelInfo.FSelect(IFunctions.Greeter) && sChannelInfo.FSelect(IChannelFunctions.Greeter, sIRCMessage.Channel))
			{
				var rand = new Random();
				string Koszones = string.Empty;
				var text = sLManager.GetCommandTexts("handlejoin", sIRCMessage.Channel);
				if(text.Length < 3)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
					return;
				}

				var text2 = sLManager.GetCommandTexts("handlejoin/random", sIRCMessage.Channel);
				Koszones = text2[rand.Next(0, text2.Length-1)];

				if(DateTime.Now.Hour >= 3 && DateTime.Now.Hour <= 9)
					sSendMessage.SendChatMessage(sIRCMessage, text[0], sIRCMessage.Nick);
				else if(DateTime.Now.Hour >= 20 && DateTime.Now.Hour < 3)
					sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Nick);
				else
				{
					if(IsAdmin(sIRCMessage.Nick, sIRCMessage.Host))
						sSendMessage.SendChatMessage(sIRCMessage, text[2]);
					else
						sSendMessage.SendChatMessage(sIRCMessage, "{0} {1}", Koszones, sIRCMessage.Nick);
				}
			}
		}

		/// <summary>
		///     Ha ez a funkció be van kapcsolva, akkor
		///     miután a nick elhagyta a szobát elköszön tőle.
		/// </summary>
		protected void HandleLeft(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Nick == sIrcBase.Networks[sIRCMessage.ServerName].sNickInfo.NickStorage)
			{
				sNameList.Remove(sIRCMessage.ServerName, sIRCMessage.Channel);
				return;
			}

			sNameList.Remove(sIRCMessage.Channel, sIRCMessage.Nick);

			if(sChannelInfo.FSelect(IFunctions.Greeter) && sChannelInfo.FSelect(IChannelFunctions.Greeter, sIRCMessage.Channel))
			{
				var rand = new Random();
				string elkoszones = string.Empty;
				var text2 = sLManager.GetCommandTexts("handleleft/random", sIRCMessage.Channel);
				elkoszones = text2[rand.Next(0, text2.Length-1)];

				if(DateTime.Now.Hour >= 20 && DateTime.Now.Hour < 3)
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("handleleft", sIRCMessage.Channel), sIRCMessage.Nick);
				else
					sSendMessage.SendChatMessage(sIRCMessage, "{0} {1}", elkoszones, sIRCMessage.Nick);
			}
		}

		protected void HandleQuit(IRCMessage sIRCMessage)
		{
			sNameList.Remove(sIRCMessage.ServerName, string.Empty, sIRCMessage.Nick, true);
		}

		protected void HandleNewNick(IRCMessage sIRCMessage)
		{
			if(!SchumixBase.NewNick)
				sNameList.Change(sIRCMessage.ServerName, sIRCMessage.Nick, sIRCMessage.Info[2].Remove(0, 1, SchumixBase.Colon));
			else
				SchumixBase.NewNick = false;
		}

		/// <summary>
		///     Ha engedélyezett a ConsolLog, akkor kiírja a Console-ra ha kickelnek valakit.
		/// </summary>
		protected void HandleKick(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info.Length < 5)
				return;

			if(sIRCMessage.Info[3] == sIrcBase.Networks[sIRCMessage.ServerName].sNickInfo.NickStorage)
			{
				sNameList.Remove(sIRCMessage.ServerName, sIRCMessage.Channel);

				if(sChannelInfo.FSelect(IFunctions.Rejoin) && sChannelInfo.FSelect(IChannelFunctions.Rejoin, sIRCMessage.Channel))
				{
					foreach(var m_channel in sChannelInfo.CList)
					{
						if(sIRCMessage.Channel.ToLower() == m_channel.Key)
						{
							Thread.Sleep(5000);
							sSender.Joine(m_channel.Key, m_channel.Value);
						}
					}
				}
			}
			else
			{
				sNameList.Remove(sIRCMessage.Channel, sIRCMessage.Info[3]);

				if(sChannelInfo.FSelect(IFunctions.Commands) && sChannelInfo.FSelect(IChannelFunctions.Commands, sIRCMessage.Channel))
				{
					if(ConsoleLog.CLog)
					{
						string text = sIRCMessage.Info.SplitToString(4, SchumixBase.Space);
						Console.WriteLine(sLManager.GetCommandText("handlekick", sIRCMessage.Channel), sIRCMessage.Nick, sIRCMessage.Info[3], text.Remove(0, 1, ":"));
					}
				}
			}
		}

		protected void HandleNameList(IRCMessage sIRCMessage)
		{
			int i = 0;
			var split = sIRCMessage.Args.Split(SchumixBase.Space);
			string Channel = split[1];
			sNameList.Remove(sIRCMessage.ServerName, Channel);

			foreach(var name in sIRCMessage.Args.Split(SchumixBase.Space))
			{
				i++;

				if(i < 3)
					continue;

				sNameList.Add(sIRCMessage.ServerName, Channel, Parse(name));
			}
		}

		private string Parse(string Name)
		{
			if(Name.Length < 1)
				return string.Empty;

			switch(Name.Substring(0, 1))
			{
				case ":":
				case "~":
				case "&":
				case "@":
				case "%":
				case "+":
					return Name.Remove(0, 1);
				default:
					return Name;
			}
		}
	}
}