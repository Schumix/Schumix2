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
using System.Threading.Tasks;
using Schumix.API;
using Schumix.Irc;
using Schumix.Framework;
using Schumix.Framework.Config;
using ChatterBotAPI;

namespace Schumix.ChatterBotAddon
{
	class ChatterBotAddon : ISchumixAddon
	{
		private readonly ChatterBotSession session = new ChatterBotFactory().Create(ChatterBotType.CLEVERBOT).CreateSession();
		private readonly IgnoreNickName sIgnoreNickName = Singleton<IgnoreNickName>.Instance;
		private readonly ChannelInfo sChannelInfo = Singleton<ChannelInfo>.Instance;
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;

		public void Setup()
		{
			Network.PublicRegisterHandler("PRIVMSG", HandlePrivmsg);
		}

		public void Destroy()
		{
			Network.PublicRemoveHandler("PRIVMSG",   HandlePrivmsg);
		}

		public bool Reload(string RName)
		{
			return false;
		}

		private void HandlePrivmsg(IRCMessage sIRCMessage)
		{
			if(sIgnoreNickName.IsIgnore(sIRCMessage.Nick))
				return;

			if(sChannelInfo.FSelect(IFunctions.Chatterbot) && sChannelInfo.FSelect(IChannelFunctions.Chatterbot, sIRCMessage.Channel))
				Task.Factory.StartNew(() => sSendMessage.SendChatMessage(sIRCMessage, session.Think(sIRCMessage.Args)));
		}

		public bool HandleHelp(IRCMessage sIRCMessage)
		{
			return false;
		}

		/// <summary>
		/// Name of the addon
		/// </summary>
		public string Name
		{
			get { return "ChatterBotAddon"; }
		}

		/// <summary>
		/// Author of the addon.
		/// </summary>
		public string Author
		{
			get { return Consts.SchumixProgrammedBy; }
		}

		/// <summary>
		/// Website where the addon is available.
		/// </summary>
		public string Website
		{
			get { return Consts.SchumixWebsite; }
		}
	}
}