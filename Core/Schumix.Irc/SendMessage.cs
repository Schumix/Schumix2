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
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;

namespace Schumix.Irc
{
	public sealed class SendMessage
	{
		private readonly object WriteLock = new object();
		private DateTime _timeLastSent = DateTime.Now;
		private SendMessage() {}

		public TimeSpan IdleTime
		{
			get { return DateTime.Now - _timeLastSent; }
		}

		public void SendChatMessage(MessageType type, string channel, string message)
		{
			lock(WriteLock)
			{
				switch(type)
				{
					case MessageType.Privmsg:
						WriteLine("PRIVMSG {0} :{1}", channel.ToLower(), IgnoreCommand(message));
						break;
					case MessageType.Notice:
						WriteLine("NOTICE {0} :{1}", channel, message);
						break;
					/*case MessageType.AMSG:
						WriteLine("AMSG :{0}", message); // egyenlőre nem megy
						break;*/
					case MessageType.Action:
						WriteLine("PRIVMSG {0} :ACTION {1}", channel.ToLower(), message);
						break;
					case MessageType.CtcpRequest:
						WriteLine("PRIVMSG {0} :{1}", channel.ToLower(), message);
						break;
					case MessageType.CtcpReply:
						WriteLine("NOTICE {0} :{1}", channel, message);
						break;
				}

				_timeLastSent = DateTime.Now;
			}
		}

		public void SendChatMessage(MessageType type, string channel, string message, params object[] args)
		{
			lock(WriteLock)
			{
				SendChatMessage(type, channel, string.Format(message, args));
			}
		}

		public void SendChatMessage(IRCMessage sIRCMessage, string message)
		{
			lock(WriteLock)
			{
				switch(IRCConfig.MessageType.ToLower())
				{
					case "privmsg":
						SendChatMessage(sIRCMessage.MessageType, sIRCMessage.Channel, message);
						break;
					case "notice":
						SendChatMessage(sIRCMessage.MessageType, sIRCMessage.Nick, message);
						break;
					default:
						SendChatMessage(sIRCMessage.MessageType, sIRCMessage.Channel, message);
						break;
				}
			}
		}

		public void SendChatMessage(IRCMessage sIRCMessage, string message, params object[] args)
		{
			lock(WriteLock)
			{
				SendChatMessage(sIRCMessage, string.Format(message, args));
			}
		}

		public void SendCMPrivmsg(string channel, string message)
		{
			lock(WriteLock)
			{
				SendChatMessage(MessageType.Privmsg, channel, message);
			}
		}

		public void SendCMPrivmsg(string channel, string message, params object[] args)
		{
			lock(WriteLock)
			{
				SendCMPrivmsg(channel, string.Format(message, args));
			}
		}

		public void SendCMNotice(string channel, string message)
		{
			lock(WriteLock)
			{
				SendChatMessage(MessageType.Notice, channel, message);
			}
		}

		public void SendCMNotice(string channel, string message, params object[] args)
		{
			lock(WriteLock)
			{
				SendCMNotice(channel, string.Format(message, args));
			}
		}

		/*public void SendCMAmsg(string message)
		{
			lock(WriteLock)
			{
				SendChatMessage(MessageType.AMSG, string.Empty, message);
			}
		}

		public void SendCMAmsg(string message, params object[] args)
		{
			lock(WriteLock)
			{
				SendCMAmsg(string.Format(message, args));
			}
		}*/

		public void SendCMAction(string channel, string message)
		{
			lock(WriteLock)
			{
				SendChatMessage(MessageType.Action, channel, message);
			}
		}

		public void SendCMAction(string channel, string message, params object[] args)
		{
			lock(WriteLock)
			{
				SendCMAction(channel, string.Format(message, args));
			}
		}

		public void SendCMCtcpRequest(string channel, string message)
		{
			lock(WriteLock)
			{
				SendChatMessage(MessageType.CtcpRequest, channel, message);
			}
		}

		public void SendCMCtcpRequest(string channel, string message, params object[] args)
		{
			lock(WriteLock)
			{
				SendCMCtcpRequest(channel, string.Format(message, args));
			}
		}

		public void SendCMCtcpReply(string channel, string message)
		{
			lock(WriteLock)
			{
				SendChatMessage(MessageType.CtcpReply, channel, message);
			}
		}

		public void SendCMCtcpReply(string channel, string message, params object[] args)
		{
			lock(WriteLock)
			{
				SendCMCtcpReply(channel, string.Format(message, args));
			}
		}

		public void WriteLine(string message)
		{
			lock(WriteLock)
			{
				if(!INetwork.Writer.IsNull() && message.Length <= 2000)
					INetwork.Writer.WriteLine(message);

				Thread.Sleep(IRCConfig.MessageSending);
			}
		}

		public void WriteLine(string message, params object[] args)
		{
			lock(WriteLock)
			{
				WriteLine(string.Format(message, args));
			}
		}

		private string IgnoreCommand(string data)
		{
			if((data.Length >= ".register".Length && data.ToLower().Substring(0, ".register".Length) == ".register"))
				data = SchumixBase.Space + data;
			else if((data.Length >= ".identify".Length && data.ToLower().Substring(0, ".identify".Length) == ".identify"))
				data = SchumixBase.Space + data;
			else if((data.Length >= ".set".Length && data.ToLower().Substring(0, ".set".Length) == ".set"))
				data = SchumixBase.Space + data;
			else if((data.Length >= ".sop".Length && data.ToLower().Substring(0, ".sop".Length) == ".sop"))
				data = SchumixBase.Space + data;
			else if((data.Length >= ".aop".Length && data.ToLower().Substring(0, ".aop".Length) == ".aop"))
				data = SchumixBase.Space + data;
			else if((data.Length >= ".hop".Length && data.ToLower().Substring(0, ".hop".Length) == ".hop"))
				data = SchumixBase.Space + data;
			else if((data.Length >= ".vop".Length && data.ToLower().Substring(0, ".vop".Length) == ".vop"))
				data = SchumixBase.Space + data;
			else if((data.Length >= ".access".Length && data.ToLower().Substring(0, ".access".Length) == ".access"))
				data = SchumixBase.Space + data;
			else if((data.Length >= ".levels".Length && data.ToLower().Substring(0, ".levels".Length) == ".levels"))
				data = SchumixBase.Space + data;
			else if((data.Length >= ".akick".Length && data.ToLower().Substring(0, ".akick".Length) == ".akick"))
				data = SchumixBase.Space + data;
			else if((data.Length >= ".drop".Length && data.ToLower().Substring(0, ".drop".Length) == ".drop"))
				data = SchumixBase.Space + data;
			else if((data.Length >= ".ban".Length && data.ToLower().Substring(0, ".ban".Length) == ".ban"))
				data = SchumixBase.Space + data;
			else if((data.Length >= ".unban".Length && data.ToLower().Substring(0, ".unban".Length) == ".unban"))
				data = SchumixBase.Space + data;
			else if((data.Length >= ".owner".Length && data.ToLower().Substring(0, ".owner".Length) == ".owner"))
				data = SchumixBase.Space + data;
			else if((data.Length >= ".deowner".Length && data.ToLower().Substring(0, ".deowner".Length) == ".deowner"))
				data = SchumixBase.Space + data;
			else if((data.Length >= ".protect".Length && data.ToLower().Substring(0, ".protect".Length) == ".protect"))
				data = SchumixBase.Space + data;
			else if((data.Length >= ".deprotect".Length && data.ToLower().Substring(0, ".deprotect".Length) == ".deprotect"))
				data = SchumixBase.Space + data;
			else if((data.Length >= ".op".Length && data.ToLower().Substring(0, ".op".Length) == ".op"))
				data = SchumixBase.Space + data;
			else if((data.Length >= ".deop".Length && data.ToLower().Substring(0, ".deop".Length) == ".deop"))
				data = SchumixBase.Space + data;
			else if((data.Length >= ".halfop".Length && data.ToLower().Substring(0, ".halfop".Length) == ".halfop"))
				data = SchumixBase.Space + data;
			else if((data.Length >= ".dehalfop".Length && data.ToLower().Substring(0, ".dehalfop".Length) == ".dehalfop"))
				data = SchumixBase.Space + data;
			else if((data.Length >= ".voice".Length && data.ToLower().Substring(0, ".voice".Length) == ".voice"))
				data = SchumixBase.Space + data;
			else if((data.Length >= ".devoice".Length && data.ToLower().Substring(0, ".devoice".Length) == ".devoice"))
				data = SchumixBase.Space + data;
			else if((data.Length >= ".getkey".Length && data.ToLower().Substring(0, ".getkey".Length) == ".getkey"))
				data = SchumixBase.Space + data;
			else if((data.Length >= ".invite".Length && data.ToLower().Substring(0, ".invite".Length) == ".invite"))
				data = SchumixBase.Space + data;
			else if((data.Length >= ".kick".Length && data.ToLower().Substring(0, ".kick".Length) == ".kick"))
				data = SchumixBase.Space + data;
			else if((data.Length >= ".logout".Length && data.ToLower().Substring(0, ".logout".Length) == ".logout"))
				data = SchumixBase.Space + data;
			else if((data.Length >= ".topic".Length && data.ToLower().Substring(0, ".topic".Length) == ".topic"))
				data = SchumixBase.Space + data;
			else if((data.Length >= ".info".Length && data.ToLower().Substring(0, ".info".Length) == ".info"))
				data = SchumixBase.Space + data;
			else if((data.Length >= ".why".Length && data.ToLower().Substring(0, ".why".Length) == ".why"))
				data = SchumixBase.Space + data;
			else if((data.Length >= ".clear".Length && data.ToLower().Substring(0, ".clear".Length) == ".clear"))
				data = SchumixBase.Space + data;
			else if((data.Length >= ".flags".Length && data.ToLower().Substring(0, ".flags".Length) == ".flags"))
				data = SchumixBase.Space + data;
			else if((data.Length >= ".appendtopic".Length && data.ToLower().Substring(0, ".appendtopic".Length) == ".appendtopic"))
				data = SchumixBase.Space + data;
			else if((data.Length >= ".checkban".Length && data.ToLower().Substring(0, ".checkban".Length) == ".checkban"))
				data = SchumixBase.Space + data;
			else if((data.Length >= ".sync".Length && data.ToLower().Substring(0, ".sync".Length) == ".sync"))
				data = SchumixBase.Space + data;
			else if((data.Length >= ".kb".Length && data.ToLower().Substring(0, ".kb".Length) == ".kb"))
				data = SchumixBase.Space + data;

			return data;
		}
	}
}