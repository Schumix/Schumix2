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
using System.Collections.Generic;
using Schumix.API;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;

namespace Schumix.Irc
{
	public sealed class SendMessage
	{
		private readonly List<string> _ingorecommandlist = new List<string>();
		private readonly object WriteLock = new object();
		private DateTime _timeLastSent = DateTime.Now;

		private SendMessage()
		{
			_ingorecommandlist.Add(".register");
			_ingorecommandlist.Add(".identify");
			_ingorecommandlist.Add(".set");
			_ingorecommandlist.Add(".sop");
			_ingorecommandlist.Add(".aop");
			_ingorecommandlist.Add(".hop");
			_ingorecommandlist.Add(".vop");
			_ingorecommandlist.Add(".access");
			_ingorecommandlist.Add(".levels");
			_ingorecommandlist.Add(".akick");
			_ingorecommandlist.Add(".drop");
			_ingorecommandlist.Add(".ban");
			_ingorecommandlist.Add(".unban");
			_ingorecommandlist.Add(".owner");
			_ingorecommandlist.Add(".deowner");
			_ingorecommandlist.Add(".protect");
			_ingorecommandlist.Add(".deprotect");
			_ingorecommandlist.Add(".op");
			_ingorecommandlist.Add(".deop");
			_ingorecommandlist.Add(".halfop");
			_ingorecommandlist.Add(".dehalfop");
			_ingorecommandlist.Add(".voice");
			_ingorecommandlist.Add(".devoice");
			_ingorecommandlist.Add(".getkey");
			_ingorecommandlist.Add(".invite");
			_ingorecommandlist.Add(".kick");
			_ingorecommandlist.Add(".logout");
			_ingorecommandlist.Add(".topic");
			_ingorecommandlist.Add(".info");
			_ingorecommandlist.Add(".why");
			_ingorecommandlist.Add(".clear");
			_ingorecommandlist.Add(".flags");
			_ingorecommandlist.Add(".appendtopic");
			_ingorecommandlist.Add(".checkban");
			_ingorecommandlist.Add(".sync");
			_ingorecommandlist.Add(".kb");
			_ingorecommandlist.Add(".k");
		}

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
			// ignore_irc_command
			foreach(var list in _ingorecommandlist)
			{
				if((data.Length >= list.Length && data.ToLower().Substring(0, list.Length) == list))
					data = SchumixBase.Space + data;
			}

			return data;
		}
	}
}