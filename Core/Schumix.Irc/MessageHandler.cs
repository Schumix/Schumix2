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
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Schumix.API;
using Schumix.API.Irc;
using Schumix.API.Functions;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;

namespace Schumix.Irc
{
	public partial class MessageHandler : CommandManager
	{
		private readonly object WriteLock = new object();
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
			Log.Success("MessageHandler", sLConsole.MessageHandler("Text"));
			RandomAllVhost();
			Task.Factory.StartNew(() => IsJoin());

			if(IRCConfig.List[sIRCMessage.ServerName].UseNickServ)
			{
				if(sNickInfo.IsNickStorage())
					sNickInfo.Identify(IRCConfig.List[sIRCMessage.ServerName].NickServPassword);
			}

			if(IRCConfig.List[sIRCMessage.ServerName].UseHostServ)
			{
				if(sNickInfo.IsNickStorage())
					sNickInfo.Vhost(SchumixBase.On);
				else
				{
					if(!Online)
					{
						Log.Notice("HostServ", sLConsole.HostServ("Text2"));
						WhoisPrivmsg = sNickInfo.NickStorage;
						ChannelPrivmsg = sNickInfo.NickStorage;
						sChannelInfo.JoinChannel();
						Online = true;
					}
				}
			}
			else
			{
				if(!Online)
				{
					if(IRCConfig.List[sIRCMessage.ServerName].HostServEnabled)
						sNickInfo.Vhost(SchumixBase.Off);

					WhoisPrivmsg = sNickInfo.NickStorage;
					ChannelPrivmsg = sNickInfo.NickStorage;
					NewNickPrivmsg = string.Empty;
					sChannelInfo.JoinChannel();
					Online = true;
				}
			}

			SchumixBase.UrlTitleEnabled = true;
		}

		protected void HandleWaitingForConnection(IRCMessage sIRCMessage)
		{
			Log.Notice("MessageHandler", sLConsole.MessageHandler("Text2"));
		}

		protected void HandleNotRegistered(IRCMessage sIRCMessage)
		{
			Log.Notice("MessageHandler", sLConsole.MessageHandler("Text10"));
		}

		protected void HandleNoNickName(IRCMessage sIRCMessage)
		{
			Log.Warning("MessageHandler", sLConsole.MessageHandler("Text3"));
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

				if(sIRCMessage.Nick == "NickServ" || sIRCMessage.Nick == "MemoServ" ||
					sIRCMessage.Nick == "ChanServ" || sIRCMessage.Nick == "HostServ")
					Console.Write(sLConsole.MessageHandler("Text4"));
				else
					Console.Write(string.Format("[{0}] ", sIRCMessage.Nick));

				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.Write(sIRCMessage.Args + Environment.NewLine);
				Console.ForegroundColor = ConsoleColor.Gray;
			}

			if(sIRCMessage.Nick == "NickServ")
			{
				if(sIRCMessage.Args.Contains("Password incorrect."))
				{
					sNickInfo.ChangeIdentifyStatus(true);
					Log.Error("NickServ", sLConsole.NickServ("Text2"));
					ConnectAllChannel();
				}
				else if(sIRCMessage.Args.Contains("You are already identified."))
					Log.Warning("NickServ", sLConsole.NickServ("Text3"));
				else if(sIRCMessage.Args.Contains("Password accepted - you are now recognized."))
				{
					sNickInfo.ChangeIdentifyStatus(true);
					Log.Success("NickServ", sLConsole.NickServ("Text4"));
				}

				if(sIRCMessage.Args.Contains("Your nick isn't registered."))
				{
					Log.Warning("NickServ", sLConsole.NickServ("Text5"));
					WhoisPrivmsg = sNickInfo.NickStorage;
					ChannelPrivmsg = sNickInfo.NickStorage;
					ConnectAllChannel();
				}

				if(IsOnline)
				{
					switch(IRCConfig.List[sIRCMessage.ServerName].MessageType.ToLower())
					{
						case "privmsg":
							sIRCMessage.MessageType = MessageType.Privmsg;
							break;
						case "notice":
							sIRCMessage.MessageType = MessageType.Notice;
							break;
						default:
							sIRCMessage.MessageType = MessageType.Privmsg;
							break;
					}

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

			if(sIRCMessage.Nick == "HostServ")
			{
				if(sIRCMessage.Args.Contains("You need to register before a vhost can be assigned to you."))
				{
					Log.Warning("HostServ", sLConsole.HostServ("Text3"));
					ConnectAllChannel();
				}
			}

			if(sIRCMessage.Nick == "HostServ" && IRCConfig.List[sIRCMessage.ServerName].UseHostServ)
			{
				if(sIRCMessage.Args.Contains("Your vhost of") && !sNickInfo.IsVhost)
				{
					WhoisPrivmsg = sNickInfo.NickStorage;
					ChannelPrivmsg = sNickInfo.NickStorage;
					ConnectAllChannel();
				}
			}

			sIRCMessage.Channel = sIRCMessage.Nick;
			sIRCMessage.Info[3] = sIRCMessage.Info[3].Remove(0, 1, SchumixBase.Colon);
			Schumix(sIRCMessage);

			if(sIRCMessage.Info[3] == string.Empty || sIRCMessage.Info[3].Length < PLength || sIRCMessage.Info[3].Substring(0, PLength) != IRCConfig.List[sIRCMessage.ServerName].CommandPrefix)
				return;

			sIRCMessage.Info[3] = sIRCMessage.Info[3].Remove(0, PLength);
			IncomingInfo(sIRCMessage.Info[3].ToLower(), sIRCMessage);
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
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write(sLConsole.MessageHandler("Text4"));
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.Write(sLConsole.MessageHandler("Text5"));
				Console.ForegroundColor = ConsoleColor.Gray;
			}
		}

		/// <summary>
		///     Ha a bot elsődleges nickje már használatban van, akkor
		///     átlép a másodlagosra, ha az is akkor a harmadlagosra.
		/// </summary>
		protected void HandleNickError(IRCMessage sIRCMessage)
		{
			if(NewNickPrivmsg == string.Empty)
			{
				Log.Error("MessageHandler", sLConsole.MessageHandler("Text6"), sNickInfo.NickStorage);
				string nick = sNickInfo.ChangeNick();
				Log.Notice("MessageHandler", sLConsole.MessageHandler("Text7"), nick);
				Online = false;
				sSender.Nick(nick);
			}
			else
			{
				sSendMessage.SendChatMessage(sIRCMessage.MessageType, NewNickPrivmsg, sLConsole.MessageHandler("Text14"));
				NewNickPrivmsg = string.Empty;
			}
		}

		protected void HandlerErrorNewNickName(IRCMessage sIRCMessage)
		{
			if(NewNickPrivmsg != string.Empty)
			{
				sSendMessage.SendChatMessage(sIRCMessage.MessageType, NewNickPrivmsg, sLConsole.MessageHandler("Text15"));
				NewNickPrivmsg = string.Empty;
			}
		}

		protected void HandleNicknameWhileBannedOrModeratedOnChannel(IRCMessage sIRCMessage)
		{
			if(NewNickPrivmsg != string.Empty)
			{
				sSendMessage.SendChatMessage(sIRCMessage.MessageType, NewNickPrivmsg, sLConsole.MessageHandler("Text16"));
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

			SchumixBase.DManager.Update("channels", string.Format("Enabled = 'false', Error = '{0}'", sLConsole.MessageHandler("Text8-1")), string.Format("Channel = '{0}' And ServerName = '{1}'", sIRCMessage.Info[3].ToLower(), sIRCMessage.ServerName));
			sSendMessage.SendChatMessage(sIRCMessage.MessageType, ChannelPrivmsg, sLConsole.MessageHandler("Text8"), sIRCMessage.Info[3]);
			ChannelPrivmsg = sNickInfo.NickStorage;
		}

		/// <summary>
		///     Ha hibás egy IRC szobának a jelszava, akkor feljegyzi.
		/// </summary>
		protected void HandleNoChannelPassword(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info.Length < 4)
				return;

			SchumixBase.DManager.Update("channels", string.Format("Enabled = 'false', Error = '{0}'", sLConsole.MessageHandler("Text9-1")), string.Format("Channel = '{0}' And ServerName = '{1}'", sIRCMessage.Info[3].ToLower(), sIRCMessage.ServerName));
			sSendMessage.SendChatMessage(sIRCMessage.MessageType, ChannelPrivmsg, sLConsole.MessageHandler("Text9"), sIRCMessage.Info[3]);
			ChannelPrivmsg = sNickInfo.NickStorage;
		}

		protected void HandleCannotJoinChannel(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info.Length < 4)
				return;

			SchumixBase.DManager.Update("channels", string.Format("Enabled = 'false', Error = '{0}'", sLConsole.MessageHandler("Text17-1")), string.Format("Channel = '{0}' And ServerName = '{1}'", sIRCMessage.Info[3].ToLower(), sIRCMessage.ServerName));
			sSendMessage.SendChatMessage(sIRCMessage.MessageType, ChannelPrivmsg, sLConsole.MessageHandler("Text17"), sIRCMessage.Info[3]);
			ChannelPrivmsg = sNickInfo.NickStorage;
		}

		/// <summary>
		///     Kigyűjti éppen hol van fent a nick.
		/// </summary>
		protected void HandleMWhois(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info.Length < 5)
				return;

			var text = sLManager.GetCommandTexts("whois", WhoisPrivmsg, sIRCMessage.ServerName);
			if(text.Length < 2)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(WhoisPrivmsg, sIRCMessage.ServerName)));
				return;
			}

			string text2 = sIRCMessage.Info.SplitToString(4, SchumixBase.Space);
			sSendMessage.SendChatMessage(sIRCMessage.MessageType, WhoisPrivmsg, text[0], text2.Remove(0, 1, SchumixBase.Colon));
			WhoisPrivmsg = sNickInfo.NickStorage;
		}

		protected void HandleNoWhois(IRCMessage sIRCMessage)
		{
			var text = sLManager.GetCommandTexts("whois", WhoisPrivmsg, sIRCMessage.ServerName);
			if(text.Length < 2)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(WhoisPrivmsg, sIRCMessage.ServerName)));
				return;
			}

			sSendMessage.SendChatMessage(sIRCMessage.MessageType, WhoisPrivmsg, text[1]);
			WhoisPrivmsg = sNickInfo.NickStorage;
		}

		protected void HandleIrcJoin(IRCMessage sIRCMessage)
		{
			sIRCMessage.Channel = sIRCMessage.Channel.Remove(0, 1, SchumixBase.Colon);
			LogToFile(sIRCMessage.Channel, sIRCMessage.Nick, sLConsole.MessageHandler("Text18"));

			if(sIRCMessage.Nick == sNickInfo.NickStorage)
			{
				if(sChannelInfo.CList.ContainsKey(sIRCMessage.Channel.ToLower()))
					SchumixBase.DManager.Update("channels", "Enabled = 'true', Error = ''", string.Format("Channel = '{0}' And ServerName = '{1}'", sIRCMessage.Channel.ToLower(), sIRCMessage.ServerName));

				return;
			}

			sChannelNameList.Add(sIRCMessage.ServerName, sIRCMessage.Channel, sIRCMessage.Nick);
		}

		protected void HandleIrcLeft(IRCMessage sIRCMessage)
		{
			LogToFile(sIRCMessage.Channel, sIRCMessage.Nick, sLConsole.MessageHandler("Text19"), sIRCMessage.Args.Trim() == string.Empty ? string.Empty : sIRCMessage.Args);

			if(sIRCMessage.Nick == sNickInfo.NickStorage)
			{
				sChannelNameList.Remove(sIRCMessage.ServerName, sIRCMessage.Channel);
				return;
			}

			sChannelNameList.Remove(sIRCMessage.ServerName, sIRCMessage.Channel, sIRCMessage.Nick);
		}

		protected void HandleIrcQuit(IRCMessage sIRCMessage)
		{
			foreach(var chan in sChannelNameList.Names)
			{
				if(chan.Value.Contains(sIRCMessage.Nick.ToLower(), SchumixBase.Comma))
					LogToFile(chan.Key, sIRCMessage.Nick, sLConsole.MessageHandler("Text20"), sIRCMessage.Args.Trim() == string.Empty ? string.Empty : sIRCMessage.Args);
			}

			sChannelNameList.Remove(sIRCMessage.ServerName, string.Empty, sIRCMessage.Nick, true);
		}

		protected void HandleNewNick(IRCMessage sIRCMessage)
		{
			foreach(var chan in sChannelNameList.Names)
			{
				if(chan.Value.Contains(sIRCMessage.Nick.ToLower(), SchumixBase.Comma))
					LogToFile(chan.Key, sIRCMessage.Nick, sLConsole.MessageHandler("Text21"), sIRCMessage.Info[2].Remove(0, 1, SchumixBase.Colon));
			}

			sChannelNameList.Change(sIRCMessage.ServerName, sIRCMessage.Nick, sIRCMessage.Info[2].Remove(0, 1, SchumixBase.Colon));
		}

		protected void HandleIrcKick(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info.Length < 5)
				return;

			string text = sIRCMessage.Info.SplitToString(4, SchumixBase.Space);
			LogToFile(sIRCMessage.Channel, sIRCMessage.Nick, sLConsole.MessageHandler("Text22"), sIRCMessage.Info[3], text.Remove(0, 1, ":"));

			if(sIRCMessage.Info[3].ToLower() == sNickInfo.NickStorage.ToLower())
				sChannelNameList.Remove(sIRCMessage.ServerName, sIRCMessage.Channel);
			else
				sChannelNameList.Remove(sIRCMessage.ServerName, sIRCMessage.Channel, sIRCMessage.Info[3]);
		}

		protected void HandleIrcMode(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info.Length < 5)
				return;

			LogToFile(sIRCMessage.Channel, sIRCMessage.Nick, sLConsole.MessageHandler("Text23"), sIRCMessage.Info.SplitToString(3, SchumixBase.Space));
		}

		protected void HandleIrcTopic(IRCMessage sIRCMessage)
		{
			LogToFile(sIRCMessage.Channel, sIRCMessage.Nick, sLConsole.MessageHandler("Text24"), sIRCMessage.Args.Trim() == string.Empty ? string.Empty : sIRCMessage.Args);
		}

		protected void HandleNameList(IRCMessage sIRCMessage)
		{
			int i = 0;
			var split = sIRCMessage.Args.Split(SchumixBase.Space);
			string Channel = split[1];
			sChannelNameList.Remove(sIRCMessage.ServerName, Channel);

			foreach(var name in sIRCMessage.Args.Split(SchumixBase.Space))
			{
				i++;

				if(i < 3)
					continue;

				sChannelNameList.Add(sIRCMessage.ServerName, Channel, Parse(name));
			}
		}

		private string Parse(string Name)
		{
			if(Name.Length < 1)
				return string.Empty;

			switch(Name.Substring(0, 1))
			{
				case ":":
				case "~":
				case "&":
				case "@":
				case "%":
				case "+":
					return Name.Remove(0, 1);
				default:
					return Name;
			}
		}

		/// <summary>
		///     
		/// </summary>
		/// <param name="channel"></param>
		/// <param name="user"></param>
		/// <param name="args"></param>
		private void LogToFile(string channel, string user, string args)
		{
			lock(WriteLock)
			{
				if(sChannelInfo.FSelect(IFunctions.Log) && sChannelInfo.FSelect(IChannelFunctions.Log, channel))
				{
					string dir = LogConfig.IrcLogDirectory + "/" + _servername;
					sUtilities.CreateDirectory(dir);
					string logdir = sUtilities.DirectoryToSpecial(dir, channel);
					string logfile = string.Format("{0}/{1}-{2}-{3}.log", logdir, DateTime.Now.Year,
									DateTime.Now.Month < 10 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString(),
									DateTime.Now.Day < 10 ? "0" + DateTime.Now.Day.ToString() : DateTime.Now.Day.ToString());

					sUtilities.CreateDirectory(logdir);
					sUtilities.CreateFile(logfile);
					var file = new StreamWriter(logfile, true) { AutoFlush = true };
					file.WriteLine("[{0}:{1}] <{2}> {3}", DateTime.Now.Hour < 10 ? "0" + DateTime.Now.Hour.ToString() :
					               DateTime.Now.Hour.ToString(), DateTime.Now.Minute < 10 ? "0" + DateTime.Now.Minute.ToString() :
					               DateTime.Now.Minute.ToString(), user, args);
					file.Close();
				}
			}
		}

		private void LogToFile(string channel, string user, string format, params object[] args)
		{
			lock(WriteLock)
			{
				LogToFile(channel, user, string.Format(format, args));
			}
		}

		private void ConnectAllChannel()
		{
			lock(WriteLock)
			{
				if(!Online)
				{
					sNickInfo.ChangeVhostStatus(true);
					sChannelInfo.JoinChannel();
					Online = true;
				}
			}
		}

		private void IsJoin()
		{
			Thread.Sleep(20*1000);
			ConnectAllChannel();
		}
	}
}