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
using System.Threading;
using Schumix.Framework.Config;

namespace Schumix.Irc
{
	/// <summary>
	///     Meghatározza, hogy PRIVMSG vagy NOTICE legyen az üzenetküldés módja.
	/// </summary>
	public enum MessageType
	{
		PRIVMSG,
		NOTICE
	};

	public sealed class SendMessage
	{
		private readonly object WriteLock = new object();
		private SendMessage() {}

		public void SendChatMessage(MessageType tipus, string channel, string uzenet)
		{
			lock(WriteLock)
			{
				if(tipus == MessageType.PRIVMSG)
					WriteLine("PRIVMSG {0} :{1}", channel, uzenet);
				else if(tipus == MessageType.NOTICE)
					WriteLine("NOTICE {0} :{1}", channel, uzenet);
			}
		}

		public void SendChatMessage(MessageType tipus, string channel, string uzenet, params object[] args)
		{
			lock(WriteLock)
			{
				SendChatMessage(tipus, channel, string.Format(uzenet, args));
			}
		}

		public void SendCMPrivmsg(string channel, string uzenet)
		{
			lock(WriteLock)
			{
				SendChatMessage(MessageType.PRIVMSG, channel, uzenet);
			}
		}

		public void SendCMPrivmsg(string channel, string uzenet, params object[] args)
		{
			lock(WriteLock)
			{
				SendChatMessage(MessageType.PRIVMSG, channel, string.Format(uzenet, args));
			}
		}

		public void SendCMNotice(string channel, string uzenet)
		{
			lock(WriteLock)
			{
				SendChatMessage(MessageType.NOTICE, channel, uzenet);
			}
		}

		public void SendCMNotice(string channel, string uzenet, params object[] args)
		{
			lock(WriteLock)
			{
				SendChatMessage(MessageType.NOTICE, channel, string.Format(uzenet, args));
			}
		}

		public void WriteLine(string uzenet)
		{
			lock(WriteLock)
			{
				if(Network.writer != null)
					Network.writer.WriteLine(uzenet);

				Thread.Sleep(IRCConfig.MessageSending);
			}
		}

		public void WriteLine(string uzenet, params object[] args)
		{
			lock(WriteLock)
			{
				WriteLine(string.Format(uzenet, args));
			}
		}
	}
}