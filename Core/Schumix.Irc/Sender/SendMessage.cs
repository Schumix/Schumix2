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
		private SendMessage() {}

		public TimeSpan IdleTime
		{
			get { return DateTime.Now - _timeLastSent; }
		}

		public void SendChatMessagee(MessageType type, string ServerName, string channel, string message)
		{
			lock(WriteLock)
			{
				if(message.Length >= 1000)
				{
					var list = NewLine(message);
					if(list.Count > 3)
					{
						SendChatMessagee(type, ServerName, channel, sLConsole.Other("MessageLength"));
						list.Clear();
						return;
					}

					foreach(var text in list)
					{
						switch(type)
						{
							case MessageType.Privmsg:
								WriteLinee(ServerName, "PRIVMSG {0} :{1}", channel.ToLower(), IgnoreCommand(text));
								break;
							case MessageType.Notice:
								WriteLinee(ServerName, "NOTICE {0} :{1}", channel, text);
								break;
							case MessageType.Amsg:
								var clist = ChannelList();
								foreach(var chan in clist)
								{
									WriteLinee(ServerName, "PRIVMSG {0} :{1}", chan, IgnoreCommand(text));
									Thread.Sleep(400);
								}

								clist.Clear();
								break;
							case MessageType.Action:
								WriteLinee(ServerName, "PRIVMSG {0} :ACTION {1}", channel.ToLower(), text);
								break;
							case MessageType.CtcpRequest:
								WriteLinee(ServerName, "PRIVMSG {0} :{1}", channel.ToLower(), text);
								break;
							case MessageType.CtcpReply:
								WriteLinee(ServerName, "NOTICE {0} :{1}", channel, text);
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
							WriteLinee(ServerName, "PRIVMSG {0} :{1}", channel.ToLower(), IgnoreCommand(message));
							break;
						case MessageType.Notice:
							WriteLinee(ServerName, "NOTICE {0} :{1}", channel, message);
							break;
						case MessageType.Amsg:
							var clist = ChannelList();
							foreach(var chan in clist)
							{
								WriteLinee(ServerName, "PRIVMSG {0} :{1}", chan, IgnoreCommand(message));
								Thread.Sleep(400);
							}

							clist.Clear();
							break;
						case MessageType.Action:
							WriteLinee(ServerName, "PRIVMSG {0} :ACTION {1}", channel.ToLower(), message);
							break;
						case MessageType.CtcpRequest:
							WriteLinee(ServerName, "PRIVMSG {0} :{1}", channel.ToLower(), message);
							break;
						case MessageType.CtcpReply:
							WriteLinee(ServerName, "NOTICE {0} :{1}", channel, message);
							break;
					}
				}

				_timeLastSent = DateTime.Now;
			}
		}

		public void SendChatMessagee(MessageType type, string ServerName, string channel, string message, params object[] args)
		{
			lock(WriteLock)
			{
				SendChatMessagee(type, ServerName, channel, string.Format(message, args));
			}
		}

		public void SendChatMessage(IRCMessage sIRCMessage, string message)
		{
			lock(WriteLock)
			{
				switch(IRCConfig.MessageType.ToLower())
				{
					case "privmsg":
						SendChatMessagee(sIRCMessage.MessageType, sIRCMessage.ServerName, sIRCMessage.Channel, message);
						break;
					case "notice":
						SendChatMessagee(sIRCMessage.MessageType, sIRCMessage.ServerName, sIRCMessage.Nick, message);
						break;
					default:
						SendChatMessagee(sIRCMessage.MessageType, sIRCMessage.ServerName, sIRCMessage.Channel, message);
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

		public void SendCMPrivmsge(string ServerName, string channel, string message)
		{
			lock(WriteLock)
			{
				SendChatMessagee(MessageType.Privmsg, ServerName, channel, message);
			}
		}

		public void SendCMPrivmsge(string ServerName, string channel, string message, params object[] args)
		{
			lock(WriteLock)
			{
				SendCMPrivmsge(ServerName, channel, string.Format(message, args));
			}
		}

		public void SendCMNoticee(string ServerName, string channel, string message)
		{
			lock(WriteLock)
			{
				SendChatMessagee(MessageType.Notice, ServerName, channel, message);
			}
		}

		public void SendCMNoticee(string ServerName, string channel, string message, params object[] args)
		{
			lock(WriteLock)
			{
				SendCMNoticee(ServerName, channel, string.Format(message, args));
			}
		}

		public void SendCMAmsge(string ServerName, string message)
		{
			lock(WriteLock)
			{
				SendChatMessagee(MessageType.Amsg, ServerName, string.Empty, message);
			}
		}

		public void SendCMAmsge(string ServerName, string message, params object[] args)
		{
			lock(WriteLock)
			{
				SendCMAmsge(ServerName, string.Format(message, args));
			}
		}

		public void SendCMActione(string ServerName, string channel, string message)
		{
			lock(WriteLock)
			{
				SendChatMessagee(MessageType.Action, ServerName, channel, message);
			}
		}

		public void SendCMActione(string ServerName, string channel, string message, params object[] args)
		{
			lock(WriteLock)
			{
				SendCMActione(ServerName, channel, string.Format(message, args));
			}
		}

		public void SendCMCtcpRequeste(string ServerName, string channel, string message)
		{
			lock(WriteLock)
			{
				SendChatMessagee(MessageType.CtcpRequest, ServerName, channel, message);
			}
		}

		public void SendCMCtcpRequeste(string ServerName, string channel, string message, params object[] args)
		{
			lock(WriteLock)
			{
				SendCMCtcpRequeste(ServerName, channel, string.Format(message, args));
			}
		}

		public void SendCMCtcpReplye(string ServerName, string channel, string message)
		{
			lock(WriteLock)
			{
				SendChatMessagee(MessageType.CtcpReply, ServerName, channel, message);
			}
		}

		public void SendCMCtcpReplye(string ServerName, string channel, string message, params object[] args)
		{
			lock(WriteLock)
			{
				SendCMCtcpReplye(ServerName, channel, string.Format(message, args));
			}
		}

		public void WriteLinee(string ServerName, string message)
		{
			lock(WriteLock)
			{
				try
				{
					if(!INetwork.WriterList[ServerName].IsNull())
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
								INetwork.WriterList[ServerName].WriteLine(text);

							list.Clear();
						}
						else
							INetwork.WriterList[ServerName].WriteLine(message);
					}

					Thread.Sleep(IRCConfig.MessageSending);
				}
				catch(Exception e)
				{
					Log.Debug("SendMessage", sLConsole.Exception("Error"), e.Message);
				}
			}
		}

		public void WriteLinee(string ServerName, string message, params object[] args)
		{
			lock(WriteLock)
			{
				WriteLinee(ServerName, string.Format(message, args));
			}
		}

		private string IgnoreCommand(string data)
		{
			var db = SchumixBase.DManager.Query("SELECT Command FROM ignore_irc_commands");
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
			var list = new List<string>();
			var db = SchumixBase.DManager.Query("SELECT Channel FROM channel");

			if(!db.IsNull())
			{
				foreach(DataRow row in db.Rows)
					list.Add(row["Channel"].ToString());
			}
			else
				Log.Error("SendMessage", sLConsole.SendMessage("Text"));

			return list;
		}
	}
}