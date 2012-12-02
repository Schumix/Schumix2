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
using System.Data;
using System.Threading;
using System.Collections.Generic;
using Schumix.API;
using Schumix.API.Irc;
using Schumix.API.Delegate;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Irc
{
	public sealed class SendMessage
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly object WriteLock = new object();
		private DateTime _timeLastSent = DateTime.Now;
		private string _servername;

		public SendMessage(string ServerName)
		{
			_servername = ServerName;
		}

		public TimeSpan IdleTime
		{
			get { return DateTime.Now - _timeLastSent; }
		}

		public void SendChatMessage(MessageType type, string channel, string message)
		{
			lock(WriteLock)
			{
				if(message.Length >= 1000)
				{
					var list = NewLine(message);
					if(list.Count > 3)
					{
						SendChatMessage(type, channel, sLConsole.Other("MessageLength"));
						list.Clear();
						return;
					}

					foreach(var text in list)
					{
						switch(type)
						{
							case MessageType.Privmsg:
								WriteLine("PRIVMSG {0} :{1}", channel.ToLower(), IgnoreCommand(text));
								break;
							case MessageType.Notice:
								WriteLine("NOTICE {0} :{1}", channel, text);
								break;
							case MessageType.Amsg:
								var clist = ChannelList();
								foreach(var chan in clist)
								{
									WriteLine("PRIVMSG {0} :{1}", chan, IgnoreCommand(text));
									Thread.Sleep(400);
								}

								clist.Clear();
								break;
							case MessageType.Action:
								WriteLine("PRIVMSG {0} :ACTION {1}", channel.ToLower(), text);
								break;
							case MessageType.CtcpRequest:
								WriteLine("PRIVMSG {0} :{1}", channel.ToLower(), text);
								break;
							case MessageType.CtcpReply:
								WriteLine("NOTICE {0} :{1}", channel, text);
								break;
						}

						Thread.Sleep(2000);
					}

					list.Clear();
				}
				else
				{
					switch(type)
					{
						case MessageType.Privmsg:
							WriteLine("PRIVMSG {0} :{1}", channel.ToLower(), IgnoreCommand(message));
							break;
						case MessageType.Notice:
							WriteLine("NOTICE {0} :{1}", channel, message);
							break;
						case MessageType.Amsg:
							var clist = ChannelList();
							foreach(var chan in clist)
							{
								WriteLine("PRIVMSG {0} :{1}", chan, IgnoreCommand(message));
								Thread.Sleep(400);
							}

							clist.Clear();
							break;
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
				switch(IRCConfig.List[sIRCMessage.ServerName].MessageType.ToLower())
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

		public void SendCMAmsg(string message)
		{
			lock(WriteLock)
			{
				SendChatMessage(MessageType.Amsg, string.Empty, message);
			}
		}

		public void SendCMAmsg(string message, params object[] args)
		{
			lock(WriteLock)
			{
				SendCMAmsg(string.Format(message, args));
			}
		}

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
				try
				{
					if(!INetwork.WriterList[_servername].IsNull())
					{
						if(message.Length >= 1000 && message.Substring(0, "notice".Length).ToLower() != "notice" &&
						   message.Substring(0, "privmsg".Length).ToLower() != "privmsg")
						{
							var list = NewLine(message);
							if(list.Count > 3)
							{
								list.Clear();
								return;
							}

							foreach(var text in list)
								INetwork.WriterList[_servername].WriteLine(text);

							list.Clear();
						}
						else
							INetwork.WriterList[_servername].WriteLine(message);
					}

					Thread.Sleep(IRCConfig.List[_servername].MessageSending);
				}
				catch(Exception e)
				{
					Log.Debug("SendMessage", sLConsole.Exception("Error"), e.Message);
				}
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
			var db = SchumixBase.DManager.Query("SELECT Command FROM ignore_irc_commands WHERE ServerName = '{0}'", _servername);
			if(!db.IsNull())
			{
				foreach(DataRow row in db.Rows)
				{
					string command = row["Command"].ToString();
					if((data.Length >= command.Length && data.ToLower().Substring(0, command.Length) == command))
						data = SchumixBase.Space + data;
				}
			}

			return data;
		}

		private List<string> NewLine(string Text)
		{
			var list = new List<string>();

			for(;;)
			{
				if(Text.Length != 0)
					list.Add(Text);

				if(Text.Length == 0)
					break;

				if(Text.Length >= 1000)
					Text = Text.Remove(0, 1000);
				else
					Text = string.Empty;
			}

			return list;
		}

		private List<string> ChannelList()
		{
			bool b = false;
			var list = new List<string>();

			foreach(var ch in SchumixBase.sCacheDB.ChannelsMap())
			{
				if(ch.Value.ServerName == _servername)
				{
					b = true;
					list.Add(ch.Value.Channel);
				}
			}

			if(!b)
				Log.Error("SendMessage", sLConsole.SendMessage("Text"));

			return list;
		}
	}
}