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
using Schumix.Framework.Localization;

namespace Schumix.Framework.Config
{
	public sealed class IRCConfig
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		public static string Server { get; private set; }
		public static int Port { get; private set; }
		public static bool Ssl { get; private set; }
		public static string NickName { get; private set; }
		public static string NickName2 { get; private set; }
		public static string NickName3 { get; private set; }
		public static string UserName { get; private set; }
		public static string UserInfo { get; private set; }
		public static string MasterChannel { get; private set; }
		public static string MasterChannelPassword { get; private set; }
		public static string IgnoreChannels { get; private set; }
		public static string IgnoreNames { get; private set; }
		public static bool UseNickServ { get; private set; }
		public static string NickServPassword { get; private set; }
		public static bool UseHostServ { get; private set; }
		public static bool HostServEnabled { get; private set; }
		public static int MessageSending { get; private set; }
		public static string CommandPrefix { get; private set; }
		public static string MessageType { get; private set; }

		public IRCConfig(string server, int port, bool ssl, string nickname, string nickname2, string nickname3, string username, string userinfo, string masterchannel, string masterchannelpassword, string ignorechannels, string ignorenames, bool usenickserv, string nickservpassword, bool usehostserv, bool hostservenabled, int messagesending, string commandprefix, string messagetype)
		{
			Server                = server;
			Port                  = port;
			Ssl                   = ssl;
			NickName              = nickname;
			NickName2             = nickname2;
			NickName3             = nickname3;
			UserName              = username;
			UserInfo              = userinfo;
			MasterChannel         = masterchannel.ToLower();
			MasterChannelPassword = masterchannelpassword;
			IgnoreChannels        = ignorechannels;
			IgnoreNames           = ignorenames;
			UseNickServ           = usenickserv;
			NickServPassword      = nickservpassword;
			UseHostServ           = usehostserv;
			HostServEnabled       = hostservenabled;
			MessageSending        = messagesending;
			CommandPrefix         = commandprefix;
			MessageType           = messagetype;
			Log.Notice("IRCConfig", sLConsole.IRCConfig("Text"));
		}
	}
}