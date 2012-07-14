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
using Schumix.Irc.Ignore;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;

namespace Schumix.Irc
{
	public sealed class Sender
	{
		private readonly IgnoreChannel sIgnoreChannel = Singleton<IgnoreChannel>.Instance;
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;
		private readonly object WriteLock = new object();
		private Sender() {}

		public void NameInfo(string ServerName, string nick, string user, string userinfo)
		{
			lock(WriteLock)
			{
				Nick(ServerName, nick);
				User(ServerName, user, userinfo);
			}
		}

		public void Nick(string ServerName, string nick)
		{
			lock(WriteLock)
			{
				sSendMessage.WriteLinee(ServerName, "NICK {0}", nick);
			}
		}

		public void User(string ServerName, string user, string userinfo)
		{
			lock(WriteLock)
			{
				sSendMessage.WriteLinee(ServerName, "USER {0} 8 * :{1}", user, userinfo);
			}
		}

		public void Joine(string ServerName, string channel)
		{
			lock(WriteLock)
			{
				if(!sIgnoreChannel.IsIgnore(channel))
					sSendMessage.WriteLinee(ServerName, "JOIN {0}", channel);
			}
		}

		public void Joine(string ServerName, string channel, string pass)
		{
			lock(WriteLock)
			{
				if(!sIgnoreChannel.IsIgnore(channel))
					sSendMessage.WriteLinee(ServerName, "JOIN {0} {1}", channel, pass);
			}
		}

		public void Part(string ServerName, string channel)
		{
			lock(WriteLock)
			{
				sSendMessage.WriteLinee(ServerName, "PART {0}", channel);
			}
		}

		public void Kick(string ServerName, string channel, string name)
		{
			lock(WriteLock)
			{
				sSendMessage.WriteLinee(ServerName, "KICK {0} {1}", channel, name);
			}
		}

		public void Kick(string ServerName, string channel, string name, string args)
		{
			lock(WriteLock)
			{
				sSendMessage.WriteLinee(ServerName, "KICK {0} {1} :{2}", channel, name, args);
			}
		}

		public void Ban(string ServerName, string channel, string name)
		{
			lock(WriteLock)
			{
				sSendMessage.WriteLinee(ServerName, "MODE {0} +b {1}", channel, name);
			}
		}

		public void Unban(string ServerName, string channel, string name)
		{
			lock(WriteLock)
			{
				sSendMessage.WriteLinee(ServerName, "MODE {0} -b {1}", channel, name);
			}
		}

		public void Modee(string ServerName, string channel, string status)
		{
			lock(WriteLock)
			{
				sSendMessage.WriteLinee(ServerName, "MODE {0} {1}", channel, status);
			}
		}

		public void Modee(string ServerName, string channel, string status, string name)
		{
			lock(WriteLock)
			{
				sSendMessage.WriteLinee(ServerName, "MODE {0} {1} {2}", channel, status, name);
			}
		}

		public void Quit(string ServerName, string args)
		{
			lock(WriteLock)
			{
				sSendMessage.WriteLinee(ServerName, "QUIT :{0}", args);
			}
		}

		public void Ping(string ServerName, string ping)
		{
			lock(WriteLock)
			{
				sSendMessage.WriteLinee(ServerName, "PING :{0}", ping);
			}
		}

		public void Pong(string ServerName, string pong)
		{
			lock(WriteLock)
			{
				sSendMessage.WriteLinee(ServerName, "PONG :{0}", pong);
			}
		}

		public void NickServ(string ServerName, string pass)
		{
			lock(WriteLock)
			{
				sSendMessage.SendCMPrivmsge(ServerName, "NickServ", "identify {0}", pass);
			}
		}

		public void NickServStatus(string ServerName, string status)
		{
			lock(WriteLock)
			{
				sSendMessage.SendCMPrivmsge(ServerName, "NickServ", "status {0}", status);
			}
		}

		public void NickServInfo(string ServerName, string info)
		{
			lock(WriteLock)
			{
				sSendMessage.SendCMPrivmsge(ServerName, "NickServ", "info {0}", info);
			}
		}

		public void HostServ(string ServerName, string Mode)
		{
			lock(WriteLock)
			{
				sSendMessage.SendCMPrivmsge(ServerName, "HostServ", "{0}", Mode);
			}
		}

		public void NickServGhost(string ServerName, string ghost, string pass)
		{
			lock(WriteLock)
			{
				sSendMessage.SendCMPrivmsge(ServerName, "NickServ", "ghost {0} {1}", ghost, pass);
			}
		}

		public void Whois(string ServerName, string name)
		{
			lock(WriteLock)
			{
				sSendMessage.WriteLinee(ServerName, "WHOIS {0}", name);
			}
		}
	}
}