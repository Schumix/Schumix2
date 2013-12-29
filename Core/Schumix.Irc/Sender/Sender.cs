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
using Schumix.Irc.Util;
using Schumix.Irc.Ignore;
using Schumix.Framework;
using Schumix.Framework.Logger;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Irc
{
	public sealed class Sender
	{
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private readonly object WriteLock = new object();
		private readonly IgnoreChannel sIgnoreChannel;
		private readonly SendMessage sSendMessage;
		private string _servername;

		public Sender(string ServerName)
		{
			_servername = ServerName;
			sSendMessage = sIrcBase.Networks[ServerName].sSendMessage;
			sIgnoreChannel = sIrcBase.Networks[ServerName].sIgnoreChannel;
		}

		public void RegisterConnection(string ServerPassword, string nickname)
		{
			lock(WriteLock)
			{
				RegisterConnection(ServerPassword, nickname, nickname, nickname, 8);
			}
		}

		public void RegisterConnection(string ServerPassword, string nickname, string username)
		{
			lock(WriteLock)
			{
				RegisterConnection(ServerPassword, nickname, username, nickname, 8);
			}
		}

		public void RegisterConnection(string ServerPassword, string nickname, string username, string realname)
		{
			lock(WriteLock)
			{
				RegisterConnection(ServerPassword, nickname, username, realname, 8);
			}
		}

		public void RegisterConnection(string ServerPassword, string nickname, string username, string realname, int modemask)
		{
			lock(WriteLock)
			{
				if(!ServerPassword.IsNullOrEmpty())
					Pass(ServerPassword);

				Nick(nickname);
				User(username, realname, modemask);
			}
		}

		public void Register(string nickname)
		{
			lock(WriteLock)
			{
				Register(nickname, nickname, nickname, 8);
			}
		}

		public void Register(string nickname, string username)
		{
			lock(WriteLock)
			{
				Register(nickname, username, nickname, 8);
			}
		}

		public void Register(string nickname, string username, string realname)
		{
			lock(WriteLock)
			{
				Register(nickname, username, realname, 8);
			}
		}

		public void Register(string nickname, string username, string realname, int modemask)
		{
			lock(WriteLock)
			{
				Nick(nickname);
				User(username, realname, modemask);
			}
		}

		public void Nick(string nick)
		{
			lock(WriteLock)
			{
				if(Rfc2812Util.IsValidNick(nick))
					sSendMessage.WriteLine("NICK {0}", nick);
				else
					Log.Warning("Sender", sLConsole.GetString("{0} is not a valid nickname!"), nick);
			}
		}

		public void User(string username)
		{
			lock(WriteLock)
			{
				User(username, username, 8);
			}
		}

		public void User(string username, string realname)
		{
			lock(WriteLock)
			{
				User(username, realname, 8);
			}
		}

		public void User(string username, string realname, int modemask)
		{
			lock(WriteLock)
			{
				sSendMessage.WriteLine("USER {0} {1} * :{2}", username, modemask, realname);
			}
		}

		public void Join(string channel)
		{
			lock(WriteLock)
			{
				if(Rfc2812Util.IsValidChannelName(channel))
				{
					if(!sIgnoreChannel.IsIgnore(channel))
						sSendMessage.WriteLine("JOIN {0}", channel);
					else
						Log.Warning("Sender", sLConsole.GetString("Channel {0}'s access is denied!"), channel);
				}
				else
					Log.Warning("Sender", sLConsole.GetString("{0} is not a valid channel name!"), channel);
			}
		}

		public void Join(string channel, string password)
		{
			lock(WriteLock)
			{
				if(password.IsNullOrEmpty())
				{
					Log.Warning("Sender", sLConsole.GetString("Password cannot be empty or null!"));
					return;
				}

				if(Rfc2812Util.IsValidChannelName(channel))
				{
					if(!sIgnoreChannel.IsIgnore(channel))
						sSendMessage.WriteLine("JOIN {0} {1}", channel, password);
					else
						Log.Warning("Sender", sLConsole.GetString("Channel {0}'s access is denied!"), channel);
				}
				else
					Log.Warning("Sender", sLConsole.GetString("{0} is not a valid channel name!"), channel);
			}
		}

		/// <summary>
		///   Leave the given channel.
		/// </summary>
		public void Part(string channel)
		{
			lock(WriteLock)
			{
				Part(channel, sLConsole.Sender("Text", sLManager.GetChannelLocalization(channel, _servername)));
			}
		}

		/// <summary>
		///   Leave the given channel.
		/// </summary>
		public void Part(string channel, string reason)
		{
			lock(WriteLock)
			{
				if(reason.IsNullOrEmpty())
				{
					Log.Warning("Sender", sLConsole.GetString("Part reason cannot be empty or null!"));
					return;
				}

				if(Rfc2812Util.IsValidChannelName(channel))
					sSendMessage.WriteLine("PART {0} :{1}", channel, reason);
				else
					Log.Warning("Sender", sLConsole.GetString("{0} is not a valid channel name!"), channel);
			}
		}

		/// <summary>
		///   Leave the given channel.
		/// </summary>
		public void Part(string reason, params string[] channels)
		{
			lock(WriteLock)
			{
				if(reason.IsNullOrEmpty())
				{
					Log.Warning("Sender", sLConsole.GetString("Part reason cannot be empty or null!"));
					return;
				}

				if(Rfc2812Util.IsValidChannelList(channels))
					sSendMessage.WriteLine("PART {0} :{1}", string.Join(SchumixBase.Comma.ToString(), channels), reason);
				else
					Log.Warning("Sender", sLConsole.GetString("One of the channels names is not valid!"));
			}
		}

		public void Kick(string channel, string name)
		{
			lock(WriteLock)
			{
				if(!Rfc2812Util.IsValidNick(name))
				{
					Log.Warning("Sender", sLConsole.GetString("{0} is not a valid nickname!"), name);
					return;
				}
				
				if(!Rfc2812Util.IsValidChannelName(channel))
				{
					Log.Warning("Sender", sLConsole.GetString("{0} is not a valid channel name!"), channel);
					return;
				}

				sSendMessage.WriteLine("KICK {0} {1}", channel, name);
			}
		}

		public void Kick(string channel, string name, string reason)
		{
			lock(WriteLock)
			{
				if(!Rfc2812Util.IsValidNick(name))
				{
					Log.Warning("Sender", sLConsole.GetString("{0} is not a valid nickname!"), name);
					return;
				}

				if(!Rfc2812Util.IsValidChannelName(channel))
				{
					Log.Warning("Sender", sLConsole.GetString("{0} is not a valid channel name!"), channel);
					return;
				}

				if(reason.IsNullOrEmpty())
				{
					Log.Warning("Sender", sLConsole.GetString("The reason for kicking cannot be empty or null!"));
					return;
				}

				sSendMessage.WriteLine("KICK {0} {1} :{2}", channel, name, reason);
			}
		}

		/// <summary>
		///   Forcefully disconnect a user form the IRC server. This can only be used
		///   by Operators.
		/// </summary>
		public void Kill(string nick, string reason)
		{
			lock(WriteLock)
			{
				if(nick.IsNullOrEmpty())
				{
					Log.Warning("Sender", sLConsole.GetString("Nick cannot be empty or null!"));
					return;
				}

				if(reason.IsNullOrEmpty())
				{
					Log.Warning("Sender", sLConsole.GetString("Reason cannot be empty or null!"));
					return;
				}

				if(Rfc2812Util.IsValidNick(nick))
					sSendMessage.WriteLine("KILL {0} {1}", nick, reason);
				else
					Log.Warning("Sender", sLConsole.GetString("{0} is not a valid nickname!"), nick);
			}
		}

		public void Ban(string channel, string name)
		{
			lock(WriteLock)
			{
				sSendMessage.WriteLine("MODE {0} +b {1}", channel, name);
			}
		}

		public void Unban(string channel, string name)
		{
			lock(WriteLock)
			{
				sSendMessage.WriteLine("MODE {0} -b {1}", channel, name);
			}
		}

		public void Mode(string channel, string status)
		{
			lock(WriteLock)
			{
				sSendMessage.WriteLine("MODE {0} {1}", channel, status);
			}
		}

		public void Mode(string channel, string status, string name)
		{
			lock(WriteLock)
			{
				sSendMessage.WriteLine("MODE {0} {1} {2}", channel, status, name);
			}
		}

		public void Quit(string reason)
		{
			lock(WriteLock)
			{
				if(reason.IsNullOrEmpty())
				{
					Log.Warning("Sender", sLConsole.GetString("Quit reason cannot be null or empty!"));
					return;
				}

				if(reason.Length > 502)
					reason = reason.Substring(0, 502);

				sSendMessage.WriteLine("QUIT :{0}", reason);
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

		/// <summary>
		///   Identifies the bot with NickServ.
		/// </summary>
		/// <param name = "password">
		///   The password for the nick
		/// </param>
		public void NickServ(string password)
		{
			lock(WriteLock)
			{
				sSendMessage.SendCMPrivmsg("NickServ", "IDENTIFY {0}", password);
			}
		}

		/// <summary>
		///   Register's the nick with NickServ.
		/// </summary>
		/// <param name = "password">
		///   The password for the nick.
		/// </param>
		/// <param name = "email">
		///   The e-mail to which the confirmation code will be sent to.
		/// </param>
		public void NickServRegister(string password, string email)
		{
			lock(WriteLock)
			{
				if(Rfc2812Util.IsValidEmailAddress(email))
					sSendMessage.SendCMPrivmsg("NickServ", "REGISTER {0} {1}", password, email);
				else
					Log.Warning("Sender", sLConsole.GetString("The email address is not valid!"));
			}
		}

		public void NickServStatus(string status)
		{
			lock(WriteLock)
			{
				sSendMessage.SendCMPrivmsg("NickServ", "STATUS {0}", status);
			}
		}

		public void NickServInfo(string info)
		{
			lock(WriteLock)
			{
				sSendMessage.SendCMPrivmsg("NickServ", "INFO {0}", info);
			}
		}

		public void HostServ(string Mode)
		{
			lock(WriteLock)
			{
				sSendMessage.SendCMPrivmsg("HostServ", "{0}", Mode);
			}
		}

		public void NickServGhost(string ghost, string pass)
		{
			lock(WriteLock)
			{
				sSendMessage.SendCMPrivmsg("NickServ", "GHOST {0} {1}", ghost, pass);
			}
		}

		public void Whois(string name)
		{
			lock(WriteLock)
			{
				if(!Rfc2812Util.IsValidNick(name))
				{
					Log.Warning("Sender", sLConsole.GetString("{0} is not a valid nickname!"), name);
					return;
				}

				sSendMessage.WriteLine("WHOIS {0}", name);
			}
		}

		public void Pass(string password)
		{
			lock(WriteLock)
			{
				sSendMessage.WriteLine("PASS {0}", password);
			}
		}

		/// <summary>
		///   Request a list of all nicknames on a given channel.
		/// </summary>
		public void Names(string channel)
		{
			lock(WriteLock)
			{
				if(Rfc2812Util.IsValidChannelName(channel))
					sSendMessage.WriteLine("NAMES {0}", channel);
				else
					Log.Warning("Sender", sLConsole.GetString("{0} is not a valid channel name!"), channel);
			}
		}

		/// <summary>
		///   Request a list of all visible channels along with their users. If the server allows this
		///   kind of request then expect a rather large reply.
		/// </summary>
		public void AllNames()
		{
			lock(WriteLock)
			{
				sSendMessage.WriteLine("NAMES");
			}
		}

		/// <summary>
		///   Request basic information about a channel, i.e. number
		///   of visible users and topic.
		/// </summary>
		public void List(string channel)
		{
			lock(WriteLock)
			{
				if(Rfc2812Util.IsValidChannelName(channel))
					sSendMessage.WriteLine("LIST {0}", channel);
				else
					Log.Warning("Sender", sLConsole.GetString("{0} is not a valid channel name!"), channel);
			}
		}
		
		/// <summary>
		///   Request basic information for all the channels on the current
		///   network.
		/// </summary>
		public void AllList()
		{
			lock(WriteLock)
			{
				sSendMessage.WriteLine("LIST");
			}
		}
		
		/// <summary>
		///   Change the topic of the given channel.
		/// </summary>
		public void ChangeTopic(string channel, string newTopic)
		{
			lock(WriteLock)
			{
				if(newTopic.IsNullOrEmpty())
				{
					Log.Warning("Sender", sLConsole.GetString("Topic cannot be empty or null!"));
					return;
				}

				if(Rfc2812Util.IsValidChannelName(channel))
					sSendMessage.WriteLine("TOPIC {0} :{1}", channel, newTopic);
				else
					Log.Warning("Sender", sLConsole.GetString("{0} is not a valid channel name!"), channel);
			}
		}
		
		/// <summary>
		///   Clear the channel's topic.
		/// </summary>
		public void ClearTopic(string channel)
		{
			lock(WriteLock)
			{
				if(Rfc2812Util.IsValidChannelName(channel))
					sSendMessage.WriteLine("TOPIC {0} : ", channel);
				else
					Log.Warning("Sender", sLConsole.GetString("{0} is not a valid channel name!"), channel);
			}
		}
		
		/// <summary>
		///   Request the topic for the given channel.
		/// </summary>
		public void RequestTopic(string channel)
		{
			lock(WriteLock)
			{
				if(Rfc2812Util.IsValidChannelName(channel))
					sSendMessage.WriteLine("TOPIC {0}", channel);
				else
					Log.Warning("Sender", sLConsole.GetString("{0} is not a valid channel name!"), channel);
			}
		}

		/// <summary>
		///   Invite a user to a channel.
		/// </summary>
		public void Invite(string name, string channel)
		{
			lock(WriteLock)
			{
				if(!Rfc2812Util.IsValidNick(name))
				{
					Log.Warning("Sender", sLConsole.GetString("{0} is not a valid nickname!"), name);
					return;
				}

				if(!Rfc2812Util.IsValidChannelName(channel))
				{
					Log.Warning("Sender", sLConsole.GetString("{0} is not a valid channel name!"), channel);
					return;
				}

				sSendMessage.WriteLine("INVITE {0} {1}", name, channel);
			}
		}

		/// <summary>
		///   Set the user status to away and set an automatic reply 
		///   to any private message.
		/// </summary>
		public void Away(string message)
		{
			lock(WriteLock)
			{
				if(message.IsNullOrEmpty())
				{
					Log.Warning("Sender", sLConsole.GetString("Away message cannot be empty or null!"));
					return;
				}

				sSendMessage.WriteLine("AWAY :{0}", message);
			}
		}

		/// <summary>
		///   Turns off the away status and the accompanying message.
		/// </summary>
		public void UnAway()
		{
			lock(WriteLock)
			{
				sSendMessage.WriteLine("AWAY");
			}
		}

		/// <summary>
		///   Request the "Message Of The Day" from the current server.
		/// </summary>
		public void Motd()
		{
			Motd(null);
		}

		/// <summary>
		///   Request the "Message Of The Day" from the given server.
		/// </summary>
		public void Motd(string targetServer)
		{
			lock(WriteLock)
			{
				if(!targetServer.IsNullOrEmpty())
					sSendMessage.WriteLine("MOTD {0}", targetServer);
				else
					sSendMessage.WriteLine("MOTD");
			}
		}
	}
}