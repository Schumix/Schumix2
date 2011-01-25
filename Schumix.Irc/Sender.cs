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
using Schumix.Framework;

namespace Schumix.Irc
{
	public class Sender
	{
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;
		private readonly object WriteLock = new object();
		private Sender() {}

		public void NameInfo(string nick, string user)
		{
			lock(WriteLock)
			{
				Nick(nick);
				User(user);
			}
		}

		public void Nick(string nick)
		{
			lock(WriteLock)
			{
				sSendMessage.WriteLine("NICK {0}", nick);
			}
		}

		public void User(string user)
		{
			lock(WriteLock)
			{
				sSendMessage.WriteLine("USER {0} 8 * :{0}", user, user);				
			}
		}

		public void Join(string channel)
		{
			lock(WriteLock)
			{
				sSendMessage.WriteLine("JOIN {0}", channel);				
			}
		}

		public void Join(string channel, string pass)
		{
			lock(WriteLock)
			{
				sSendMessage.WriteLine("JOIN {0} {1}", channel, pass);				
			}
		}

		public void Part(string channel)
		{
			lock(WriteLock)
			{
				sSendMessage.WriteLine("PART {0}", channel);
			}
		}

		public void Kick(string channel, string name)
		{
			lock(WriteLock)
			{
				sSendMessage.WriteLine("KICK {0} {1}", channel, name);
			}
		}

		public void Kick(string channel, string name, string args)
		{
			lock(WriteLock)
			{
				sSendMessage.WriteLine("KICK {0} {1} :{2}", channel, name, args);
			}
		}

		public void Mode(string channel, string status, string name)
		{
			lock(WriteLock)
			{
				sSendMessage.WriteLine("MODE {0} {1} {2}", channel, status, name);
			}
		}

		public void Quit(string args)
		{
			lock(WriteLock)
			{
				sSendMessage.WriteLine("QUIT :{0}", args);
			}
		}

		public void Ping(string ping)
		{
			lock(WriteLock)
			{
				sSendMessage.WriteLine("PING :{0}", ping);				
			}
		}

		public void Pong(string pong)
		{
			lock(WriteLock)
			{
				sSendMessage.WriteLine("PONG :{0}", pong);				
			}
		}

		public void NickServ(string pass)
		{
			lock(WriteLock)
			{
				sSendMessage.WriteLine("NickServ identify {0}", pass);
			}
		}

		public void HostServ(string h)
		{
			lock(WriteLock)
			{
				sSendMessage.WriteLine("HostServ {0}", h);
			}
		}

		public void NickServGhost(string ghost, string pass)
		{
			lock(WriteLock)
			{
				sSendMessage.WriteLine("NickServ ghost {0} {1}", ghost, pass);
			}
		}

		public void Whois(string name)
		{
			lock(WriteLock)
			{
				sSendMessage.WriteLine("WHOIS {0}", name);
			}
		}
	}
}
