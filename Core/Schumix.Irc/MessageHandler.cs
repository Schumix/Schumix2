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
		protected bool HostServStatus;
		protected bool NewNick;
		protected MessageHandler() {}

		protected void HandleSuccessfulAuth(IRCMessage sIRCMessage)
		{
			Console.WriteLine();
			Log.Success("MessageHandler", sLConsole.MessageHandler("Text"));

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
			//Log.Notice("MessageHandler", "Test.");
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
			foreach(var plugin in sAddonManager.GetPlugins())
				plugin.HandleNotice(sIRCMessage);

			if(ConsoleLog.CLog)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write(sLConsole.MessageHandler("Text4"));
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
		}

		/// <summary>
		///     Válaszol, ha valaki pingeli a botot.
		/// </summary>
		protected void HandlePing(IRCMessage sIRCMessage)
		{
			sSender.Ping(sIRCMessage.Args);
			Console.WriteLine(sIRCMessage.Args);
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
		protected void HandleIsmeretlenParancs(IRCMessage sIRCMessage)
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

			SchumixBase.DManager.Update("channel", string.Format("Enabled = 'false', Error = '{0}'", sLConsole.MessageHandler("Text8")), string.Format("Channel = '{0}'", sIRCMessage.Info[3].ToLower()));
			sSendMessage.SendCMPrivmsg(ChannelPrivmsg, sLConsole.MessageHandler("Text8"), sIRCMessage.Info[3]);
			ChannelPrivmsg = sNickInfo.NickStorage;
		}

		/// <summary>
		///     Ha hibás egy IRC szobának a jelszava, akkor feljegyzi.
		/// </summary>
		protected void HandleNoChannelPassword(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info.Length < 4)
				return;

			SchumixBase.DManager.Update("channel", string.Format("Enabled = 'false', Error = '{0}'", sLConsole.MessageHandler("Text9")), string.Format("Channel = '{0}'", sIRCMessage.Info[3].ToLower()));
			sSendMessage.SendCMPrivmsg(ChannelPrivmsg, sLConsole.MessageHandler("Text9"), sIRCMessage.Info[3]);
			ChannelPrivmsg = sNickInfo.NickStorage;
		}

		/// <summary>
		///     Kigyűjti éppen hol van fent a nick.
		/// </summary>
		protected void HandleMWhois(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info.Length < 5)
				return;

			string text = sIRCMessage.Info.SplitToString(4, SchumixBase.Space);
			sSendMessage.SendCMPrivmsg(WhoisPrivmsg, sLManager.GetCommandText("whois", WhoisPrivmsg), text.Remove(0, 1, SchumixBase.Point2));
			WhoisPrivmsg = sNickInfo.NickStorage;
		}

		protected void HandleLeft(IRCMessage sIRCMessage)
		{
			foreach(var plugin in sAddonManager.GetPlugins())
				plugin.HandleLeft(sIRCMessage);
		}

		protected void HandleKKick(IRCMessage sIRCMessage)
		{
			foreach(var plugin in sAddonManager.GetPlugins())
				plugin.HandleKick(sIRCMessage);
		}

		/// <summary>
		///     
		/// </summary>
		/// <param name="channel"></param>
		/// <param name="user"></param>
		/// <param name="args"></param>
		private void LogToFile(string channel, string user, string args)
		{
			if(sChannelInfo.FSelect("log") && sChannelInfo.FSelect("log", channel))
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