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
using System.IO;
using Schumix.API;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Irc.Commands;

namespace Schumix.Irc
{
	public partial class MessageHandler : CommandManager
	{
		public static bool HostServStatus;
		public static bool NewNick;
		protected MessageHandler() {}

		protected void HandleSuccessfulAuth(IRCMessage sIRCMessage)
		{
			Console.WriteLine();
			Log.Success("MessageHandler", sLConsole.MessageHandler("Text"));
			RandomAllVhost();

			if(IRCConfig.UseNickServ)
			{
				Log.Notice("NickServ", sLConsole.NickServ("Text"));

				if(!NewNick)
					sSender.NickServ(IRCConfig.NickServPassword);
			}

			if(IRCConfig.UseHostServ)
			{
				if(!NewNick)
				{
					HostServStatus = true;
					sSender.HostServ("on");
					Log.Notice("HostServ", sLConsole.HostServ("Text"));
				}
				else
				{
					Log.Notice("HostServ", sLConsole.HostServ("Text2"));
					WhoisPrivmsg = sNickInfo.NickStorage;
					ChannelPrivmsg = sNickInfo.NickStorage;
					sChannelInfo.JoinChannel();
				}
			}
			else
			{
				Log.Notice("HostServ", sLConsole.HostServ("Text2"));
				if(IRCConfig.HostServEnabled)
					sSender.HostServ("off");

				WhoisPrivmsg = sNickInfo.NickStorage;
				ChannelPrivmsg = sNickInfo.NickStorage;
				sChannelInfo.JoinChannel();
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
			if(sNickName.IsIgnore(sIRCMessage.Nick))
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
					Log.Error("NickServ", sLConsole.NickServ("Text2"));
				else if(sIRCMessage.Args.Contains("You are already identified."))
					Log.Warning("NickServ", sLConsole.NickServ("Text3"));
				else if(sIRCMessage.Args.Contains("Password accepted - you are now recognized."))
					Log.Success("NickServ", sLConsole.NickServ("Text4"));

				if(IsOnline)
				{
					switch(IRCConfig.MessageType.ToLower())
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

					if(sIRCMessage.Args.Contains("   Is online from:"))
					{
						sIRCMessage.Channel = OnlinePrivmsg;
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.MessageHandler("Text11", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						IsOnline = false;
					}
					else if(sIRCMessage.Args.Contains("isn't registered."))
					{
						sIRCMessage.Channel = OnlinePrivmsg;
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.MessageHandler("Text12", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
						IsOnline = false;
					}
					else if(sIRCMessage.Args.Contains("   Last seen time:"))
					{
						sIRCMessage.Channel = OnlinePrivmsg;
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.MessageHandler("Text13", sLManager.GetChannelLocalization(sIRCMessage.Channel)), sIRCMessage.Args.Remove(0, "   Last seen time: ".Length, "   Last seen time: "));
						IsOnline = false;
					}

					sIRCMessage.MessageType = MessageType.Notice;
				}
			}

			if(sIRCMessage.Nick == "HostServ" && IRCConfig.UseHostServ)
			{
				if(sIRCMessage.Args.Contains("Your vhost of") && HostServStatus)
				{
					HostServStatus = false;
					WhoisPrivmsg = sNickInfo.NickStorage;
					ChannelPrivmsg = sNickInfo.NickStorage;
					sChannelInfo.JoinChannel();
				}
			}

			sIRCMessage.Channel = sIRCMessage.Nick;
			sIRCMessage.Info[3] = sIRCMessage.Info[3].Remove(0, 1, SchumixBase.Colon);
			Schumix(sIRCMessage);

			if(sIRCMessage.Info[3] == string.Empty || sIRCMessage.Info[3].Length < PLength || sIRCMessage.Info[3].Substring(0, PLength) != IRCConfig.CommandPrefix)
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
			Log.Error("MessageHandler", sLConsole.MessageHandler("Text6"), sNickInfo.NickStorage);
			string nick = sNickInfo.ChangeNick();
			Log.Notice("MessageHandler", sLConsole.MessageHandler("Text7"), nick);
			NewNick = true;
			sSender.Nick(nick);
		}

		/// <summary>
		///     Ha bannolva van egy szobából, akkor feljegyzi.
		/// </summary>
		protected void HandleChannelBan(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info.Length < 4)
				return;

			SchumixBase.DManager.Update("channel", string.Format("Enabled = 'false', Error = '{0}'", sLConsole.MessageHandler("Text8-1")), string.Format("Channel = '{0}'", sIRCMessage.Info[3].ToLower()));
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

			SchumixBase.DManager.Update("channel", string.Format("Enabled = 'false', Error = '{0}'", sLConsole.MessageHandler("Text9-1")), string.Format("Channel = '{0}'", sIRCMessage.Info[3].ToLower()));
			sSendMessage.SendChatMessage(sIRCMessage.MessageType, ChannelPrivmsg, sLConsole.MessageHandler("Text9"), sIRCMessage.Info[3]);
			ChannelPrivmsg = sNickInfo.NickStorage;
		}

		/// <summary>
		///     Kigyűjti éppen hol van fent a nick.
		/// </summary>
		protected void HandleMWhois(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info.Length < 5)
				return;

			var text = sLManager.GetCommandTexts("whois", WhoisPrivmsg);
			if(text.Length < 2)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(WhoisPrivmsg)));
				return;
			}

			string text2 = sIRCMessage.Info.SplitToString(4, SchumixBase.Space);
			sSendMessage.SendChatMessage(sIRCMessage.MessageType, WhoisPrivmsg, text[0], text2.Remove(0, 1, SchumixBase.Colon));
			WhoisPrivmsg = sNickInfo.NickStorage;
		}

		protected void HandleNoWhois(IRCMessage sIRCMessage)
		{
			var text = sLManager.GetCommandTexts("whois", WhoisPrivmsg);
			if(text.Length < 2)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(WhoisPrivmsg)));
				return;
			}

			sSendMessage.SendChatMessage(sIRCMessage.MessageType, WhoisPrivmsg, text[1]);
			WhoisPrivmsg = sNickInfo.NickStorage;
		}

		protected void HandleIrcJoin(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Nick == sNickInfo.NickStorage)
				return;

			sIRCMessage.Channel = sIRCMessage.Channel.Remove(0, 1, SchumixBase.Colon);
			sChannelNameList.Add(sIRCMessage.Channel, sIRCMessage.Nick);
		}

		protected void HandleIrcLeft(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Nick == sNickInfo.NickStorage)
				return;

			sChannelNameList.Remove(sIRCMessage.Channel, sIRCMessage.Nick);
		}

		protected void HandleIrcQuit(IRCMessage sIRCMessage)
		{
			sChannelNameList.Remove(string.Empty, sIRCMessage.Nick, true);
		}

		protected void HandleNewNick(IRCMessage sIRCMessage)
		{
			sChannelNameList.Change(sIRCMessage.Nick, sIRCMessage.Info[2].Remove(0, 1, SchumixBase.Colon));
		}

		protected void HandleIrcKick(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info.Length < 4)
				return;

			sChannelNameList.Remove(sIRCMessage.Channel, sIRCMessage.Info[3]);
		}

		protected void HandleNameList(IRCMessage sIRCMessage)
		{
			int i = 0;
			var split = sIRCMessage.Args.Split(SchumixBase.Space);
			string Channel = split[1];
			sChannelNameList.Remove(Channel);

			foreach(var name in sIRCMessage.Args.Split(SchumixBase.Space))
			{
				i++;

				if(i < 3)
					continue;

				sChannelNameList.Add(Channel, Parse(name));
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
			if(sChannelInfo.FSelect(IFunctions.Log) && sChannelInfo.FSelect(IChannelFunctions.Log, channel))
			{
				try
				{
					sUtilities.CreateDirectory(LogConfig.IrcLogDirectory);
					string logdir = string.Format("./{0}/{1}", LogConfig.IrcLogDirectory, channel);
					string logfile = string.Empty;

					if(DateTime.Now.Month < 10)
					{
						if(DateTime.Now.Day < 10)
							logfile = string.Format("{0}/{1}-0{2}-0{3}.log", logdir, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
						else
							logfile = string.Format("{0}/{1}-0{2}-{3}.log", logdir, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
					}
					else
					{
						if(DateTime.Now.Day < 10)
							logfile = string.Format("{0}/{1}-{2}-0{3}.log", logdir, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
						else
							logfile = string.Format("{0}/{1}-{2}-{3}.log", logdir, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
					}

					sUtilities.CreateDirectory(logdir);
					sUtilities.CreateFile(logfile);
					var file = new StreamWriter(logfile, true) { AutoFlush = true };

					if(DateTime.Now.Minute < 10)
						file.WriteLine("[{0}:0{1}] <{2}> {3}", DateTime.Now.Hour, DateTime.Now.Minute, user, args);
					else
						file.WriteLine("[{0}:{1}] <{2}> {3}", DateTime.Now.Hour, DateTime.Now.Minute, user, args);

					file.Close();
				}
				catch(Exception)
				{
					LogToFile(channel, user, args);
				}
			}
		}
	}
}