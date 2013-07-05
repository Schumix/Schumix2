/*
 * This file is part of Schumix.
 * 
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
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Schumix.Irc.Util;
using Schumix.Irc.Channel;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Irc;
using Schumix.Framework.Logger;
using Schumix.Framework.Config;
using Schumix.Framework.Functions;
using Schumix.Framework.CodeBureau;
using Schumix.Framework.Extensions;

namespace Schumix.Irc
{
	public abstract partial class MessageHandler : CommandManager
	{
		private readonly object WriteLock = new object();
		public bool UrlTitleEnabled = false;
		private string _servername;
		private int PLength;
		public bool IsAllJoin;
		public bool Online;

		protected MessageHandler(string ServerName) : base(ServerName)
		{
			_servername = ServerName;
			PLength = IRCConfig.List[ServerName].CommandPrefix.Length;
		}

		public void ReloadMessageHandlerConfig()
		{
			PLength = IRCConfig.List[_servername].CommandPrefix.Length;
		}

		protected void HandleSuccessfulAuth(IRCMessage sIRCMessage)
		{
			Console.WriteLine();
			Log.Success("MessageHandler", sLConsole.GetString("Successfully connected to IRC server."));
			RandomAllVhost();
			Task.Factory.StartNew(() => JoinProgress());

			if(IRCConfig.List[sIRCMessage.ServerName].UseNickServ)
			{
				if(sMyNickInfo.IsNickStorage())
					sMyNickInfo.Identify(IRCConfig.List[sIRCMessage.ServerName].NickServPassword);
			}

			if(IRCConfig.List[sIRCMessage.ServerName].UseHostServ)
			{
				if(sMyNickInfo.IsNickStorage())
					sMyNickInfo.Vhost(SchumixBase.On);
				else
				{
					if(!Online)
					{
						Log.Notice("HostServ", sLConsole.GetString("Vhost is OFF."));
						ChannelPrivmsg = sMyNickInfo.NickStorage;
						sMyChannelInfo.JoinChannels();
						Online = true;
					}
				}
			}
			else
			{
				if(!Online)
				{
					if(IRCConfig.List[sIRCMessage.ServerName].HostServEnabled)
						sMyNickInfo.Vhost(SchumixBase.Off);

					ChannelPrivmsg = sMyNickInfo.NickStorage;
					NewNickPrivmsg = string.Empty;
					sMyChannelInfo.JoinChannels();
					Online = true;
				}
			}

			UrlTitleEnabled = true;
		}

		protected void HandleWaitingForConnection(IRCMessage sIRCMessage)
		{
			Log.Notice("MessageHandler", sLConsole.GetString("Waiting for connection processing."));
		}

		protected void HandleNotRegistered(IRCMessage sIRCMessage)
		{
			Log.Notice("MessageHandler", sLConsole.GetString("You have not registered!"));
		}

		protected void HandleNoNickName(IRCMessage sIRCMessage)
		{
			Log.Warning("MessageHandler", sLConsole.GetString("No such Bot's nickname!"));
		}

		/// <summary>
		///     Ha a ConsoleLog be van kapcsolva, akkor
		///     kiírja a console-ra az IRC szerverről fogadott információkat.
		/// </summary>
		protected void HandleNotice(IRCMessage sIRCMessage)
		{
			if(sIgnoreNickName.IsIgnore(sIRCMessage.Nick))
				return;

			sIRCMessage.MessageType = MessageType.Notice;

			if(ConsoleLog.CLog)
			{
				Console.ForegroundColor = ConsoleColor.Red;

				if(sIRCMessage.Nick.IsServ())
					Console.Write(sLConsole.GetString("[SERVER] "));
				else
					Console.Write(string.Format("[{0}] ", sIRCMessage.Nick));

				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.Write(sIRCMessage.Args + Environment.NewLine);
				Console.ForegroundColor = ConsoleColor.Gray;
			}

			if(sIRCMessage.Nick.IsServ(Serv.NickServ))
			{
				if(sIRCMessage.Args.Contains("Password incorrect."))
				{
					sMyNickInfo.ChangeIdentifyStatus(true);
					Log.Error("NickServ", sLConsole.GetString("Bad identify password!"));
					ConnectAllChannel();
				}
				else if(sIRCMessage.Args.Contains("You are already identified."))
					Log.Warning("NickServ", sLConsole.GetString("Already identified!"));
				else if(sIRCMessage.Args.Contains("Password accepted - you are now recognized."))
				{
					sMyNickInfo.ChangeIdentifyStatus(true);
					Log.Success("NickServ", sLConsole.GetString("Identify password accepted."));
				}

				var registered = new Regex("Nick (.+) isn't registered.");

				if(sIRCMessage.Args.Contains("Your nick isn't registered.") || (!sMyNickInfo.IsIdentify && registered.IsMatch(sIRCMessage.Args)))
				{
					Log.Warning("NickServ", sLConsole.GetString("Your nick isn't registered!"));
					ChannelPrivmsg = sMyNickInfo.NickStorage;
					sMyNickInfo.ChangeIdentifyStatus(true);
					ConnectAllChannel();
				}

				if(IsOnline)
				{
					sIRCMessage.SetMessageType();

					if(sIRCMessage.Args.Contains("Is online from:") || sIRCMessage.Args.Contains("is currently online."))
					{
						sIRCMessage.Channel = OnlinePrivmsg;
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.MessageHandler("Text11", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						IsOnline = false;
					}
					else if(sIRCMessage.Args.Contains("isn't registered."))
					{
						sIRCMessage.Channel = OnlinePrivmsg;
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.MessageHandler("Text12", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						IsOnline = false;
					}
					else if(sIRCMessage.Args.Contains("Last seen time:") || sIRCMessage.Args.Contains("Last seen:"))
					{
						sIRCMessage.Channel = OnlinePrivmsg;
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.MessageHandler("Text13", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)), sIRCMessage.Args.Contains("Last seen time:") ? sIRCMessage.Args.Remove(0, "Last seen time: ".Length, "Last seen time: ") : sIRCMessage.Args.Remove(0, "Last seen: ".Length, "Last seen: "));
						IsOnline = false;
					}

					sIRCMessage.MessageType = MessageType.Notice;
				}
			}

			if(sIRCMessage.Nick.IsServ(Serv.HostServ))
			{
				if(sIRCMessage.Args.Contains("You need to register before a vhost can be assigned to you."))
				{
					Log.Warning("HostServ", sLConsole.GetString("Nick isnt registered, so the vhost still off!"));
					ConnectAllChannel();
				}
			}

			if(sIRCMessage.Nick.IsServ(Serv.HostServ) && IRCConfig.List[sIRCMessage.ServerName].UseHostServ)
			{
				if(sIRCMessage.Args.Contains("Your vhost of") && !sMyNickInfo.IsVhost)
				{
					ChannelPrivmsg = sMyNickInfo.NickStorage;
					ConnectAllChannel();
				}
			}

			sIRCMessage.Channel = sIRCMessage.Nick;
			HandleCommand(sIRCMessage);
		}

		/// <summary>
		///     Válaszol, ha valaki pingeli a botot.
		/// </summary>
		protected void HandlePing(IRCMessage sIRCMessage)
		{
			sSender.Pong(sIRCMessage.Args);
		}

		/// <summary>
		///     Válaszol, ha valaki pongolja a botot.
		/// </summary>
		protected void HandlePong(IRCMessage sIRCMessage)
		{
			sSender.Pong(sIRCMessage.Args);
		}

		/// <summary>
		///     Ha ismeretlen parancs jön, akkor kiírja.
		/// </summary>
		protected void HandleUnknownCommand(IRCMessage sIRCMessage)
		{
			if(ConsoleLog.CLog)
			{
				if(SchumixConfig.ColorBindMode)
				{
					Console.Write(sLConsole.GetString("[SERVER] "));
					Console.Write(sLConsole.GetString("No such irc command.\n"));
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.Write(sLConsole.GetString("[SERVER] "));
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.Write(sLConsole.GetString("No such irc command.\n"));
					Console.ForegroundColor = ConsoleColor.Gray;
				}
			}
		}

		/// <summary>
		///     Ha a bot elsődleges nickje már használatban van, akkor
		///     átlép a másodlagosra, ha az is akkor a harmadlagosra.
		/// </summary>
		protected void HandleNickError(IRCMessage sIRCMessage)
		{
			if(NewNickPrivmsg.IsNullOrEmpty())
			{
				Log.Error("MessageHandler", sLConsole.GetString("{0} already in use!"), sMyNickInfo.NickStorage);
				string nick = sMyNickInfo.ChangeNick();
				Log.Notice("MessageHandler", sLConsole.GetString("Retrying with: {0}"), nick);
				Online = false;
				sSender.Nick(nick);
			}
			else
			{
				sSendMessage.SendChatMessage(sIRCMessage.MessageType, NewNickPrivmsg, sLConsole.MessageHandler("Text14", sLManager.GetChannelLocalization(NewNickPrivmsg, sIRCMessage.ServerName)));
				NewNickPrivmsg = string.Empty;
			}
		}

		protected void HandleErrorNewNickName(IRCMessage sIRCMessage)
		{
			if(NewNickPrivmsg.IsNullOrEmpty() && !Online)
			{
				Log.Error("MessageHandler", sLConsole.GetString("{0} already in use!"), sMyNickInfo.NickStorage);
				string nick = sMyNickInfo.ChangeNick();
				Log.Notice("MessageHandler", sLConsole.GetString("Retrying with: {0}"), nick);
				sSender.Nick(nick);
			}

			if(!NewNickPrivmsg.IsNullOrEmpty())
			{
				if(sIRCMessage.Args.Contains("Erroneous Nickname: [OperServ]") || sIRCMessage.Args.Contains("Reserved for Network Services"))
					sSendMessage.SendChatMessage(sIRCMessage.MessageType, NewNickPrivmsg, sLConsole.MessageHandler("Text20", sLManager.GetChannelLocalization(NewNickPrivmsg, sIRCMessage.ServerName)));
				else
					sSendMessage.SendChatMessage(sIRCMessage.MessageType, NewNickPrivmsg, sLConsole.MessageHandler("Text15", sLManager.GetChannelLocalization(NewNickPrivmsg, sIRCMessage.ServerName)));

				NewNickPrivmsg = string.Empty;
			}
		}

		protected void HandleNicknameWhileBannedOrModeratedOnChannel(IRCMessage sIRCMessage)
		{
			if(!NewNickPrivmsg.IsNullOrEmpty())
			{
				sSendMessage.SendChatMessage(sIRCMessage.MessageType, NewNickPrivmsg, sLConsole.MessageHandler("Text16", sLManager.GetChannelLocalization(NewNickPrivmsg, sIRCMessage.ServerName)));
				NewNickPrivmsg = string.Empty;
			}
		}

		/// <summary>
		///     Ha bannolva van egy szobából, akkor feljegyzi.
		/// </summary>
		protected void HandleChannelBan(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info.Length < 4)
				return;

			SchumixBase.DManager.Update("channels", string.Format("Enabled = 'false', Error = '{0}'", sLConsole.MessageHandler("Text8-1", sLManager.GetChannelLocalization(ChannelPrivmsg, sIRCMessage.ServerName))), string.Format("Channel = '{0}' And ServerName = '{1}'", sIRCMessage.Info[3].ToLower(), sIRCMessage.ServerName));
			sSendMessage.SendChatMessage(sIRCMessage.MessageType, ChannelPrivmsg, sLConsole.MessageHandler("Text8", sLManager.GetChannelLocalization(ChannelPrivmsg, sIRCMessage.ServerName)), sIRCMessage.Info[3]);
			ChannelPrivmsg = sMyNickInfo.NickStorage;
		}

		/// <summary>
		///     Ha hibás egy IRC szobának a jelszava, akkor feljegyzi.
		/// </summary>
		protected void HandleNoChannelPassword(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info.Length < 4)
				return;

			SchumixBase.DManager.Update("channels", string.Format("Enabled = 'false', Error = '{0}'", sLConsole.MessageHandler("Text9-1", sLManager.GetChannelLocalization(ChannelPrivmsg, sIRCMessage.ServerName))), string.Format("Channel = '{0}' And ServerName = '{1}'", sIRCMessage.Info[3].ToLower(), sIRCMessage.ServerName));
			sSendMessage.SendChatMessage(sIRCMessage.MessageType, ChannelPrivmsg, sLConsole.MessageHandler("Text9", sLManager.GetChannelLocalization(ChannelPrivmsg, sIRCMessage.ServerName)), sIRCMessage.Info[3]);
			ChannelPrivmsg = sMyNickInfo.NickStorage;
		}

		protected void HandleCannotJoinChannel(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info.Length < 4)
				return;

			SchumixBase.DManager.Update("channels", string.Format("Enabled = 'false', Error = '{0}'", sLConsole.MessageHandler("Text17-1", sLManager.GetChannelLocalization(ChannelPrivmsg, sIRCMessage.ServerName))), string.Format("Channel = '{0}' And ServerName = '{1}'", sIRCMessage.Info[3].ToLower(), sIRCMessage.ServerName));
			sSendMessage.SendChatMessage(sIRCMessage.MessageType, ChannelPrivmsg, sLConsole.MessageHandler("Text17", sLManager.GetChannelLocalization(ChannelPrivmsg, sIRCMessage.ServerName)), sIRCMessage.Info[3]);
			ChannelPrivmsg = sMyNickInfo.NickStorage;
		}

		/// <summary>
		///     Kigyűjti éppen hol van fent a nick.
		/// </summary>
		protected void HandleMWhois(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info.Length < 5)
				return;

			string nick = sIRCMessage.Info[3].ToLower();

			if(WhoisList.ContainsKey(nick))
			{
				WhoisList[nick].Online = true;
				WhoisList[nick].Message += SchumixBase.Space + sIRCMessage.Info.SplitToString(4, SchumixBase.Space).Remove(0, 1, SchumixBase.Colon);
			}
		}

		protected void HandleWhoisServer(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info.Length < 4)
				return;

			string nick = sIRCMessage.Info[3].ToLower();

			if(WhoisList.ContainsKey(nick))
				WhoisList[nick].Online = true;
		}

		protected void HandleEndOfWhois(IRCMessage sIRCMessage)
		{
			string nick = sIRCMessage.Info[3].ToLower();

			if(WhoisList.ContainsKey(nick))
			{
				var text = sLManager.GetCommandTexts("whois", WhoisList[nick].Channel, sIRCMessage.ServerName);
				if(text.Length < 3)
				{
					sSendMessage.SendChatMessage(sIRCMessage.MessageType, WhoisList[nick].Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(WhoisList[nick].Channel, sIRCMessage.ServerName)));
					return;
				}

				if(WhoisList[nick].Online)
				{
					if(!WhoisList[nick].Message.IsNullOrEmpty())
						sSendMessage.SendChatMessage(sIRCMessage.MessageType, WhoisList[nick].Channel, text[0], WhoisList[nick].Message.Remove(0, 1, SchumixBase.Space));
					else
						sSendMessage.SendChatMessage(sIRCMessage.MessageType, WhoisList[nick].Channel, text[2]); 
				}
				else
					sSendMessage.SendChatMessage(sIRCMessage.MessageType, WhoisList[nick].Channel, text[1]);

				Monitor.Exit(WhoisList[nick].Lock);

				if(WhoisList.ContainsKey(nick))
					WhoisList.Remove(nick);
			}
		}

		protected void HandleIrcJoin(IRCMessage sIRCMessage)
		{
			sIRCMessage.Channel = sIRCMessage.Channel.Remove(0, 1, SchumixBase.Colon);
			LogInFile(sIRCMessage.Channel, sIRCMessage.Nick, sLConsole.GetString("[joined]"));

			if(sIRCMessage.Nick.ToLower() == sMyNickInfo.NickStorage.ToLower())
			{
				if(sMyChannelInfo.CList.ContainsKey(sIRCMessage.Channel.ToLower()))
					SchumixBase.DManager.Update("channels", "Enabled = 'true', Error = ''", string.Format("Channel = '{0}' And ServerName = '{1}'", sIRCMessage.Channel.ToLower(), sIRCMessage.ServerName));

				return;
			}

			sChannelList.Add(sIRCMessage.Channel, sIRCMessage.Nick);
		}

		protected void HandleIrcLeft(IRCMessage sIRCMessage)
		{
			LogInFile(sIRCMessage.Channel, sIRCMessage.Nick, sLConsole.GetString("[left] {0}"), sIRCMessage.Args.IsNullOrEmpty() ? string.Empty : sIRCMessage.Args);

			if(sIRCMessage.Nick.ToLower() == sMyNickInfo.NickStorage.ToLower())
			{
				sChannelList.Remove(sIRCMessage.Channel);
				return;
			}

			sChannelList.Remove(sIRCMessage.Channel, sIRCMessage.Nick);
		}

		protected void HandleIrcQuit(IRCMessage sIRCMessage)
		{
			foreach(var chan in sChannelList.List)
			{
				if(chan.Value.Names.ContainsKey(sIRCMessage.Nick.ToLower()))
					LogInFile(chan.Key, sIRCMessage.Nick, sLConsole.GetString("[quit] {0}"), sIRCMessage.Args.IsNullOrEmpty() ? string.Empty : sIRCMessage.Args);
			}

			sChannelList.Remove(string.Empty, sIRCMessage.Nick, true);
		}

		protected void HandleNewNick(IRCMessage sIRCMessage)
		{
			foreach(var chan in sChannelList.List)
			{
				if(chan.Value.Names.ContainsKey(sIRCMessage.Nick.ToLower()))
					LogInFile(chan.Key, sIRCMessage.Nick, sLConsole.GetString("[Is now known as {0}]"), sIRCMessage.Info[2].Remove(0, 1, SchumixBase.Colon));
			}

			sChannelList.Change(sIRCMessage.Nick, sIRCMessage.Info[2].Remove(0, 1, SchumixBase.Colon));
		}

		protected void HandleIrcKick(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info.Length < 5)
				return;

			string text = sIRCMessage.Info.SplitToString(4, SchumixBase.Space);
			LogInFile(sIRCMessage.Channel, sIRCMessage.Nick, sLConsole.GetString("[Kicked that user: {0} reason: {1}]"), sIRCMessage.Info[3], text.Remove(0, 1, ":"));

			if(sIRCMessage.Info[3].ToLower() == sMyNickInfo.NickStorage.ToLower())
				sChannelList.Remove(sIRCMessage.Channel);
			else
				sChannelList.Remove(sIRCMessage.Channel, sIRCMessage.Info[3]);
		}

		protected void HandleIrcMode(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info.Length < 4)
				return;

			string status = string.Empty;

			if(sIRCMessage.Info[3].Contains(Rfc2812Util.ModeActionToChar(ModeAction.Remove).ToString()))
				status = Rfc2812Util.ModeActionToChar(ModeAction.Remove).ToString();
			else if(sIRCMessage.Info[3].Contains(Rfc2812Util.ModeActionToChar(ModeAction.Add).ToString()))
				status = Rfc2812Util.ModeActionToChar(ModeAction.Add).ToString();

			if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[3].Length > 1 && sChannelList.IsChannelRank(sIRCMessage.Info[3].Substring(1, 1)))
			{
				if(status == Rfc2812Util.ModeActionToChar(ModeAction.Remove).ToString())
					sChannelList.List[sIRCMessage.Channel.ToLower()].Names[sIRCMessage.Info[4].ToLower()].Rank = sChannelList.List[sIRCMessage.Channel.ToLower()].Names[sIRCMessage.Info[4].ToLower()].Rank.Replace(sIRCMessage.Info[3].Substring(1, 1), string.Empty);
				else if(status == Rfc2812Util.ModeActionToChar(ModeAction.Add).ToString() && !sChannelList.List[sIRCMessage.Channel.ToLower()].Names[sIRCMessage.Info[4].ToLower()].Rank.Contains(sIRCMessage.Info[3].Substring(1, 1)))
					sChannelList.List[sIRCMessage.Channel.ToLower()].Names[sIRCMessage.Info[4].ToLower()].Rank += sIRCMessage.Info[3].Substring(1, 1);
			}

			if(sIRCMessage.Info.Length >= 6 && sIRCMessage.Info[3].Length > 2 && sChannelList.IsChannelRank(sIRCMessage.Info[3].Substring(2).Substring(0, 1)))
			{
				if(status == Rfc2812Util.ModeActionToChar(ModeAction.Remove).ToString())
					sChannelList.List[sIRCMessage.Channel.ToLower()].Names[sIRCMessage.Info[5].ToLower()].Rank = sChannelList.List[sIRCMessage.Channel.ToLower()].Names[sIRCMessage.Info[5].ToLower()].Rank.Replace(sIRCMessage.Info[3].Substring(2).Substring(0, 1), string.Empty);
				else if(status == Rfc2812Util.ModeActionToChar(ModeAction.Add).ToString() && !sChannelList.List[sIRCMessage.Channel.ToLower()].Names[sIRCMessage.Info[5].ToLower()].Rank.Contains(sIRCMessage.Info[3].Substring(2).Substring(0, 1)))
					sChannelList.List[sIRCMessage.Channel.ToLower()].Names[sIRCMessage.Info[5].ToLower()].Rank += sIRCMessage.Info[3].Substring(2).Substring(0, 1);
			}

			if(sIRCMessage.Info.Length >= 7 && sIRCMessage.Info[3].Length > 3 && sChannelList.IsChannelRank(sIRCMessage.Info[3].Substring(3).Substring(0, 1)))
			{
				if(status == Rfc2812Util.ModeActionToChar(ModeAction.Remove).ToString())
					sChannelList.List[sIRCMessage.Channel.ToLower()].Names[sIRCMessage.Info[6].ToLower()].Rank = sChannelList.List[sIRCMessage.Channel.ToLower()].Names[sIRCMessage.Info[6].ToLower()].Rank.Replace(sIRCMessage.Info[3].Substring(3).Substring(0, 1), string.Empty);
				else if(status == Rfc2812Util.ModeActionToChar(ModeAction.Add).ToString() && !sChannelList.List[sIRCMessage.Channel.ToLower()].Names[sIRCMessage.Info[6].ToLower()].Rank.Contains(sIRCMessage.Info[3].Substring(3).Substring(0, 1)))
					sChannelList.List[sIRCMessage.Channel.ToLower()].Names[sIRCMessage.Info[6].ToLower()].Rank += sIRCMessage.Info[3].Substring(3).Substring(0, 1);
			}

			if(sIRCMessage.Info.Length >= 8 && sIRCMessage.Info[3].Length > 4 && sChannelList.IsChannelRank(sIRCMessage.Info[3].Substring(4).Substring(0, 1)))
			{
				if(status == Rfc2812Util.ModeActionToChar(ModeAction.Remove).ToString())
					sChannelList.List[sIRCMessage.Channel.ToLower()].Names[sIRCMessage.Info[7].ToLower()].Rank = sChannelList.List[sIRCMessage.Channel.ToLower()].Names[sIRCMessage.Info[7].ToLower()].Rank.Replace(sIRCMessage.Info[3].Substring(4).Substring(0, 1), string.Empty);
				else if(status == Rfc2812Util.ModeActionToChar(ModeAction.Add).ToString() && !sChannelList.List[sIRCMessage.Channel.ToLower()].Names[sIRCMessage.Info[7].ToLower()].Rank.Contains(sIRCMessage.Info[3].Substring(4).Substring(0, 1)))
					sChannelList.List[sIRCMessage.Channel.ToLower()].Names[sIRCMessage.Info[7].ToLower()].Rank += sIRCMessage.Info[3].Substring(4).Substring(0, 1);
			}

			if(!sMyNickInfo.IsIdentify && sIRCMessage.Nick.IsServ(Serv.NickServ) && sIRCMessage.Channel.ToLower() == sMyNickInfo.NickStorage.ToLower() && sIRCMessage.Args == "+r")
			{
				sMyNickInfo.ChangeIdentifyStatus(true);
				Log.Success("NickServ", sLConsole.GetString("Identify password accepted!"));
			}

			LogInFile(sIRCMessage.Channel, sIRCMessage.Nick, sLConsole.GetString("[Set mode: {0}]"), sIRCMessage.Info.SplitToString(3, SchumixBase.Space));
		}

		protected void HandleIrcTopic(IRCMessage sIRCMessage)
		{
			string text = sIRCMessage.Args.IsNullOrEmpty() ? string.Empty : sIRCMessage.Args;
			sChannelList.List[sIRCMessage.Channel.ToLower()].Topic = text;
			LogInFile(sIRCMessage.Channel, sIRCMessage.Nick, sLConsole.GetString("[Topic] New topic: {0}"), text);
		}

		protected void HandleIrcInvite(IRCMessage sIRCMessage)
		{
			Log.Notice("MessageHandler", sLConsole.GetString("{0} invites you to join {1}."), sIRCMessage.Nick, sIRCMessage.Args);
			LogInFile(sIRCMessage.Nick, sIRCMessage.Nick, string.Format(sLConsole.GetString("[INVITE] {0} invites you to join {1}."), sIRCMessage.Nick, sIRCMessage.Args));
		}

		protected void HandleInitialTopic(IRCMessage sIRCMessage)
		{
			sIRCMessage.Channel = sIRCMessage.Info[3];
			sIRCMessage.Args = sIRCMessage.Info.SplitToString(4, SchumixBase.Space);
			sIRCMessage.Args = sIRCMessage.Args.Remove(0, 1, SchumixBase.Colon);
			string text = sIRCMessage.Args.IsNullOrEmpty() ? string.Empty : sIRCMessage.Args;

			if(!sChannelList.List.ContainsKey(sIRCMessage.Channel.ToLower()))
			{
				sChannelList.List.Add(sIRCMessage.Channel.ToLower(), new ChannelInfo());
				sChannelList.List[sIRCMessage.Channel.ToLower()].IsNameList = false;
			}

			sChannelList.List[sIRCMessage.Channel.ToLower()].Topic = text;
			LogInFile(sIRCMessage.Channel, sIRCMessage.Nick, sLConsole.GetString("[Topic] {0}"), text);
		}

		protected void HandleNameList(IRCMessage sIRCMessage)
		{
			int i = 0;
			var split = sIRCMessage.Args.Split(SchumixBase.Space);
			string Channel = split[1];

			if(sChannelList.List.ContainsKey(Channel.ToLower()) && sChannelList.List[Channel.ToLower()].IsNameList)
				sChannelList.Remove(Channel);

			foreach(var name in sIRCMessage.Args.Split(SchumixBase.Space))
			{
				i++;

				if(i < 3)
					continue;

				sChannelList.Add(Channel, sMyNickInfo.Parse(name));

				if(sChannelList.IsChannelRank(name.Substring(0, 1)))
					sChannelList.List[Channel.ToLower()].Names[sMyNickInfo.Parse(name).ToLower()].Rank = StringEnum.GetStringValue(sChannelList.StringToChannelRank(name.Substring(0, 1)));
			}
		}

		protected void HandleEndNameList(IRCMessage sIRCMessage)
		{
			if(sChannelList.List.ContainsKey(sIRCMessage.Channel.ToLower()))
				sChannelList.List[sIRCMessage.Channel.ToLower()].IsNameList = true;
		}

		protected void HandleNeedMoreParams(IRCMessage sIRCMessage)
		{
			if(!ModePrivmsg.IsNullOrEmpty())
				sSendMessage.SendChatMessage(sIRCMessage.MessageType, ModePrivmsg, sLConsole.MessageHandler("Text"));
		}

		protected void HandleKeySet(IRCMessage sIRCMessage)
		{
			if(!ModePrivmsg.IsNullOrEmpty())
				sSendMessage.SendChatMessage(sIRCMessage.MessageType, ModePrivmsg, sLConsole.MessageHandler("Text2"));
		}

		protected void HandleNoChanModes(IRCMessage sIRCMessage)
		{
			if(!ModePrivmsg.IsNullOrEmpty())
				sSendMessage.SendChatMessage(sIRCMessage.MessageType, ModePrivmsg, sLConsole.MessageHandler("Text3"));
		}

		protected void HandleChanopPrivsNeeded(IRCMessage sIRCMessage)
		{
			if(!ModePrivmsg.IsNullOrEmpty())
			{
				if(sIRCMessage.Info.Length < 4)
					return;

				sSendMessage.SendChatMessage(sIRCMessage.MessageType, sIRCMessage.Info[3], sLConsole.MessageHandler("Text19"));
			}
		}

		protected void HandleUserNotinChannel(IRCMessage sIRCMessage)
		{
			if(!ModePrivmsg.IsNullOrEmpty())
				sSendMessage.SendChatMessage(sIRCMessage.MessageType, ModePrivmsg, sLConsole.MessageHandler("Text5"));

			if(!KickPrivmsg.IsNullOrEmpty())
				sSendMessage.SendChatMessage(sIRCMessage.MessageType, KickPrivmsg, sLConsole.MessageHandler("Text5"));
		}

		protected void HandleUnknownMode(IRCMessage sIRCMessage)
		{
			if(!ModePrivmsg.IsNullOrEmpty())
				sSendMessage.SendChatMessage(sIRCMessage.MessageType, ModePrivmsg, sLConsole.MessageHandler("Text6"));
		}

		protected void HandleNoSuchNick(IRCMessage sIRCMessage)
		{
			if(!ModePrivmsg.IsNullOrEmpty() || ModePrivmsg != sMyNickInfo.NickStorage)
			{
				if(sIRCMessage.Info.Length < 4)
					return;

				sSendMessage.SendChatMessage(sIRCMessage.MessageType, ModePrivmsg, sLConsole.MessageHandler("Text7"), sIRCMessage.Info[3]);
			}

			if(!KickPrivmsg.IsNullOrEmpty() || KickPrivmsg != sMyNickInfo.NickStorage)
			{
				if(sIRCMessage.Info.Length < 4)
					return;

				sSendMessage.SendChatMessage(sIRCMessage.MessageType, KickPrivmsg, sLConsole.MessageHandler("Text7"), sIRCMessage.Info[3]);
			}
		}

		protected void HandleNotAChannelOwner(IRCMessage sIRCMessage)
		{
			if(!ModePrivmsg.IsNullOrEmpty())
			{
				if(sIRCMessage.Info.Length < 4)
					return;

				sSendMessage.SendChatMessage(sIRCMessage.MessageType, sIRCMessage.Info[3], sLConsole.MessageHandler("Text19"));
			}

			if(!KickPrivmsg.IsNullOrEmpty())
			{
				if(sIRCMessage.Info.Length < 4)
					return;

				sSendMessage.SendChatMessage(sIRCMessage.MessageType, sIRCMessage.Info[3], sLConsole.MessageHandler("Text18"));
			}
		}

		protected void HandleOtherKickError(IRCMessage sIRCMessage)
		{
			if(!KickPrivmsg.IsNullOrEmpty())
				sSendMessage.SendChatMessage(sIRCMessage.MessageType, KickPrivmsg, sLConsole.MessageHandler("Text18"));
		}

		protected void HandleNotAChannelAdmin(IRCMessage sIRCMessage)
		{
			if(!ModePrivmsg.IsNullOrEmpty())
				sSendMessage.SendChatMessage(sIRCMessage.MessageType, ModePrivmsg, sLConsole.MessageHandler("Text19"));

			if(!KickPrivmsg.IsNullOrEmpty())
				sSendMessage.SendChatMessage(sIRCMessage.MessageType, KickPrivmsg, sLConsole.MessageHandler("Text18"));
		}

		/// <summary>
		///     Logolja a csatornára kiírt üzeneteket. stb.
		/// </summary>
		/// <param name="channel"></param>
		/// <param name="user"></param>
		/// <param name="args"></param>
		public void LogInFile(string channel, string user, string args)
		{
			lock(WriteLock)
			{
				if((sMyChannelInfo.FSelect(IFunctions.Log) && sMyChannelInfo.FSelect(IChannelFunctions.Log, channel)) || !Rfc2812Util.IsValidChannelName(channel))
				{
					if(!Rfc2812Util.IsValidChannelName(channel))
					{
						string targs = args.ToLower();

						if((targs.Contains("admin") && targs.Contains("access")) ||
						   (targs.Contains("admin") && targs.Contains("newpassword")) ||
						   (targs.Contains("notes") && targs.Contains("user") && targs.Contains("access")) ||
						   (targs.Contains("notes") && targs.Contains("user") && targs.Contains("register")) ||
						   (targs.Contains("notes") && targs.Contains("user") && targs.Contains("remove")) ||
						   (targs.Contains("notes") && targs.Contains("user") && targs.Contains("newpassword")))
							return;
					}

					string dir = LogConfig.IrcLogDirectory + "/" + _servername;
					sUtilities.CreateDirectory(dir);

					string logdir = sUtilities.DirectoryToSpecial(dir, channel);
					string logfile = string.Format("{0}/{1}.log", logdir, DateTime.Now.ToString("yyyy-MM-dd"));

					sUtilities.CreateDirectory(logdir);
					sUtilities.CreateFile(logfile);

					var file = new StreamWriter(logfile, true) { AutoFlush = true };
					file.WriteLine("[{0}] <{1}> {2}", DateTime.Now.ToString("HH:mm:ss"), user, args);
					file.Close();
				}
			}
		}

		public void LogInFile(string channel, string user, string format, params object[] args)
		{
			lock(WriteLock)
			{
				LogInFile(channel, user, string.Format(format, args));
			}
		}

		private void ConnectAllChannel()
		{
			lock(WriteLock)
			{
				if(!Online)
				{
					sMyNickInfo.ChangeVhostStatus(true);
					sMyChannelInfo.JoinChannels();
					Online = true;
				}
			}
		}

		private void JoinProgress()
		{
			Thread.Sleep(20*1000);
			ConnectAllChannel();
		}
	}
}