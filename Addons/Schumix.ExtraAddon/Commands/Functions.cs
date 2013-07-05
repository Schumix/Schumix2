/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Twl
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013 Schumix Team <http://schumix.eu/>
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
using System.Timers;
using System.Threading;
using System.Text.RegularExpressions;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Irc;
using Schumix.Framework.Logger;
using Schumix.Framework.Config;
using Schumix.Framework.Functions;
using Schumix.Framework.Extensions;

namespace Schumix.ExtraAddon.Commands
{
	partial class Functions
	{
		public System.Timers.Timer _timeronline = new System.Timers.Timer();
		private readonly object Lock = new object();

		public void HLMessage(IRCMessage sIRCMessage)
		{
			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;
			var sMyChannelInfo = sIrcBase.Networks[sIRCMessage.ServerName].sMyChannelInfo;

			if(sMyChannelInfo.FSelect(IFunctions.Autohl) && sMyChannelInfo.FSelect(IChannelFunctions.Autohl, sIRCMessage.Channel))
			{
				var db = SchumixBase.DManager.Query("SELECT Name, Info, Enabled FROM hlmessage WHERE ServerName = '{0}'", sIRCMessage.ServerName);
				if(!db.IsNull())
				{
					foreach(DataRow row in db.Rows)
					{
						var regex = new Regex(row["Name"].ToString());

						if(regex.IsMatch(sIRCMessage.Args.ToLower()))
						{
							string status = row["Enabled"].ToString();

							if(status != SchumixBase.On)
								return;

							sSendMessage.SendChatMessage(sIRCMessage, "{0}", row["Info"].ToString());
						}
					}
				}
			}
		}

		public bool AutoKick(string status, string nick, string _channel)
		{
			var sMyChannelInfo = sIrcBase.Networks[_servername].sMyChannelInfo;
			var sSender = sIrcBase.Networks[_servername].sSender;

			if(status == "join")
			{
				string channel = _channel.Remove(0, 1, SchumixBase.Colon);

				if(sMyChannelInfo.FSelect(IFunctions.Autokick) && sMyChannelInfo.FSelect(IChannelFunctions.Autokick, channel))
				{
					var db = SchumixBase.DManager.QueryFirstRow("SELECT Reason FROM kicklist WHERE Name = '{0}' And ServerName = '{1}'", nick.ToLower(), _servername);
					if(!db.IsNull())
					{
						sSender.Kick(channel, nick, db["Reason"].ToString());
						return true;
					}
				}
			}

			if(status == "privmsg")
			{
				if(sMyChannelInfo.FSelect(IFunctions.Autokick) && sMyChannelInfo.FSelect(IChannelFunctions.Autokick, _channel))
				{
					var db = SchumixBase.DManager.QueryFirstRow("SELECT Reason FROM kicklist WHERE Name = '{0}' And ServerName = '{1}'", nick.ToLower(), _servername);
					if(!db.IsNull())
					{
						sSender.Kick(_channel, nick, db["Reason"].ToString());
						return true;
					}
				}
			}

			return false;
		}

		public void HandleWebTitle(IRCMessage sIRCMessage, string msg)
		{
			try
			{
				var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;
				var youtube = new YoutubeTitle(msg);

				if(youtube.IsYoutube())
				{
					if(youtube.IsTitle())
					{
						sSendMessage.SendChatMessage(sIRCMessage, "1,0You0,4Tube: {0}", youtube.GetTitle());
						return;
					}
				}

				var url = new Uri(msg);
				string webTitle = string.Empty;
				var thread = new Thread(() => webTitle = WebHelper.GetWebTitle(url));
				thread.Start();
				thread.Join(5000);
				thread.Abort();

				if(string.IsNullOrEmpty(webTitle))
					return;

				var title = Regex.Replace(webTitle, @"\s+", SchumixBase.Space.ToString());
				sSendMessage.SendChatMessage(sIRCMessage, "1,0Title: {0}", title);
			}
			catch(Exception e)
			{
				Log.Debug("Functions", sLConsole.GetString("Failure details: {0}"), e.Message);
				return;
			}
		}

		public void Message(IRCMessage sIRCMessage)
		{
			lock(Lock)
			{
				var sMyChannelInfo = sIrcBase.Networks[sIRCMessage.ServerName].sMyChannelInfo;
				var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;

				if(sMyChannelInfo.FSelect(IFunctions.Message) && sMyChannelInfo.FSelect(IChannelFunctions.Message, sIRCMessage.Channel))
				{
					var db = SchumixBase.DManager.Query("SELECT Message, Wrote FROM message WHERE Name = '{0}' AND Channel = '{1}' AND ServerName = '{2}' ORDER BY `Id` ASC", sIRCMessage.Nick.ToLower(), sIRCMessage.Channel.ToLower(), sIRCMessage.ServerName);
					if(!db.IsNull())
					{
						foreach(DataRow row in db.Rows)
						{
							sSendMessage.SendChatMessage(sIRCMessage, "{0}: {1}", sIRCMessage.Nick, row["Message"].ToString());
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("message2", sIRCMessage.Channel, sIRCMessage.ServerName), row["Wrote"].ToString());
							Thread.Sleep(400);
						}

						SchumixBase.DManager.Delete("message", string.Format("Name = '{0}' AND Channel = '{1}' And ServerName = '{2}'", sIRCMessage.Nick.ToLower(), sIRCMessage.Channel.ToLower(), sIRCMessage.ServerName));
					}
				}
			}
		}

		public void HandleIsOnline(object sender, ElapsedEventArgs e)
		{
			var sMyNickInfo = sIrcBase.Networks[_servername].sMyNickInfo;
			var sSender = sIrcBase.Networks[_servername].sSender;

			if(sMyNickInfo.NickStorage.ToLower() != IRCConfig.List[_servername].NickName.ToLower())
			{
				IsOnline = true;
				sSender.NickServInfo(IRCConfig.List[_servername].NickName);
			}
			else if(sMyNickInfo.NickStorage.ToLower() == IRCConfig.List[_servername].NickName.ToLower() && !sMyNickInfo.IsIdentify
					&& IRCConfig.List[_servername].UseNickServ && sIrcBase.Networks[_servername].Online)
			{
				sMyNickInfo.Identify(IRCConfig.List[_servername].NickServPassword);

				if(IRCConfig.List[_servername].UseHostServ)
					sMyNickInfo.Vhost(SchumixBase.On);
			}
		}
	}
}
