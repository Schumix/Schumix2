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
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.ExtraAddon.Commands
{
	public class IrcHandler : CommandInfo
	{
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly ChannelInfo sChannelInfo = Singleton<ChannelInfo>.Instance;
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;
		private readonly Sender sSender = Singleton<Sender>.Instance;
		private readonly NickInfo sNickInfo = Singleton<NickInfo>.Instance;
		private readonly Functions sFunctions = Singleton<Functions>.Instance;
		protected bool AutoMode = false;
		protected string ModeChannel;

		/// <summary>
		///     Ha a szobában a köszönés funkció be van kapcsolva,
		///     akkor köszön az éppen belépőnek.
		/// </summary>
		protected void HandleJoin(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Nick == sNickInfo.NickStorage)
				return;

			if(sFunctions.AutoKick("join", sIRCMessage.Nick, sIRCMessage.Channel))
				return;

			string channel = sIRCMessage.Channel.Remove(0, 1, SchumixBase.Point2);

			if(sChannelInfo.FSelect("automode") && sChannelInfo.FSelect("automode", channel))
			{
				AutoMode = true;
				ModeChannel = channel.ToLower();
				sSender.NickServStatus(sIRCMessage.Nick);
			}

			if(sChannelInfo.FSelect("koszones") && sChannelInfo.FSelect("koszones", channel))
			{
				var rand = new Random();
				string Koszones = string.Empty;
				switch(rand.Next(0, 12))
				{
					case 0:
						Koszones = "Hello";
						break;
					case 1:
						Koszones = "Csáó";
						break;
					case 2:
						Koszones = "Hy";
						break;
					case 3:
						Koszones = "Szevasz";
						break;
					case 4:
						Koszones = "Üdv";
						break;
					case 5:
						Koszones = "Szervusz";
						break;
					case 6:
						Koszones = "Aloha";
						break;
					case 7:
						Koszones = "Jó napot";
						break;
					case 8:
						Koszones = "Szia";
						break;
					case 9:
						Koszones = "Hi";
						break;
					case 10:
						Koszones = "Szerbusz";
						break;
					case 11:
						Koszones = "Hali";
						break;
					case 12:
						Koszones = "Szeva";
						break;
				}

				if(DateTime.Now.Hour >= 3 && DateTime.Now.Hour <= 9)
					sSendMessage.SendCMPrivmsg(channel, "Jó reggelt {0}", sIRCMessage.Nick);
				else if(DateTime.Now.Hour >= 20 && DateTime.Now.Hour < 3)
					sSendMessage.SendCMPrivmsg(channel, "Jó estét {0}", sIRCMessage.Nick);
				else
				{
					if(IsAdmin(sIRCMessage.Nick))
						sSendMessage.SendCMPrivmsg(channel, "Üdv főnök");
					else
						sSendMessage.SendCMPrivmsg(channel, "{0} {1}", Koszones, sIRCMessage.Nick);
				}
			}
		}

		/// <summary>
		///     Ha ez a funkció be van kapcsolva, akkor
		///     miután a nick elhagyta a szobát elköszön tőle.
		/// </summary>
		protected void HandleLLeft(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Nick == sNickInfo.NickStorage)
				return;

			if(sChannelInfo.FSelect("koszones") && sChannelInfo.FSelect("koszones", sIRCMessage.Channel))
			{
				var rand = new Random();
				string elkoszones = string.Empty;
				switch(rand.Next(0, 1))
				{
					case 0:
						elkoszones = "Viszlát";
						break;
					case 1:
						elkoszones = "Bye";
						break;
				}

				if(DateTime.Now.Hour >= 20 && DateTime.Now.Hour < 3)
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Jóét {0}", sIRCMessage.Nick);
				else
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "{0} {1}", elkoszones, sIRCMessage.Nick);
			}
		}

		/// <summary>
		///     Ha engedélyezett a ConsolLog, akkor kiírja a Console-ra ha kickelnek valakit.
		/// </summary>
		protected void HandleKKick(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info.Length < 5)
				return;

			if(sIRCMessage.Info[3] == sNickInfo.NickStorage)
			{
				if(sChannelInfo.FSelect("rejoin") && sChannelInfo.FSelect("rejoin", sIRCMessage.Channel))
				{
					foreach(var m_channel in sChannelInfo.CList)
					{
						if(sIRCMessage.Channel == m_channel.Key)
							sSender.Join(m_channel.Key, m_channel.Value);
					}
				}
			}
			else
			{
				if(sChannelInfo.FSelect("commands") && sChannelInfo.FSelect("commands", sIRCMessage.Channel))
				{
					if(ConsoleLog.CLog)
					{
						string text = sIRCMessage.Info.SplitToString(4, SchumixBase.Space);
						Console.WriteLine(sLManager.GetCommandText("handlekick", sIRCMessage.Channel), sIRCMessage.Nick, sIRCMessage.Info[3], text.Remove(0, 1, ":"));
					}
				}
			}
		}
	}
}