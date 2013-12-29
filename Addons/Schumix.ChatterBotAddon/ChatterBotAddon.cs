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
using System.Web;
using System.Threading.Tasks;
using Schumix.Irc;
using Schumix.Irc.Util;
using Schumix.Irc.Ignore;
using Schumix.Framework;
using Schumix.Framework.Irc;
using Schumix.Framework.Config;
using Schumix.Framework.Functions;
using ChatterBotAPI;

namespace Schumix.ChatterBotAddon
{
	class ChatterBotAddon : ISchumixAddon
	{
		private readonly ChatterBotSession session = new ChatterBotFactory().Create(ChatterBotType.CLEVERBOT).CreateSession();
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private string _servername;

		public void Setup(string ServerName, bool LoadConfig = false)
		{
			_servername = ServerName;
			sIrcBase.Networks[ServerName].IrcRegisterHandler("PRIVMSG", HandlePrivmsg);
		}

		public void Destroy()
		{
			sIrcBase.Networks[_servername].IrcRemoveHandler("PRIVMSG",  HandlePrivmsg);
		}

		public int Reload(string RName, bool LoadConfig, string SName = "")
		{
			return -1;
		}

		private void HandlePrivmsg(IRCMessage sIRCMessage)
		{
			var sIgnoreNickName = sIrcBase.Networks[sIRCMessage.ServerName].sIgnoreNickName;
			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;
			var sMyChannelInfo = sIrcBase.Networks[sIRCMessage.ServerName].sMyChannelInfo;

			if(sIgnoreNickName.IsIgnore(sIRCMessage.Nick))
				return;

			if(!Rfc2812Util.IsValidChannelName(sIRCMessage.Channel))
				sIRCMessage.Channel = sIRCMessage.Nick;

			if(sMyChannelInfo.FSelect(IFunctions.Chatterbot) && sMyChannelInfo.FSelect(IChannelFunctions.Chatterbot, sIRCMessage.Channel))
				Task.Factory.StartNew(() => sSendMessage.SendChatMessage(sIRCMessage, HttpUtility.HtmlDecode(session.Think(sIRCMessage.Args))));
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