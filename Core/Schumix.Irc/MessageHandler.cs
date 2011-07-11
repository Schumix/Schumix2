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
			Log.Success("MessageHandler", "Sikeres kapcsolodas az irc kiszolgalohoz.");

			if(IRCConfig.UseNickServ)
			{
				Log.Notice("NickServ", "Azonosito jelszo kuldese a kiszolgalonak.");

				if(!NewNick)
					sSender.NickServ(IRCConfig.NickServPassword);
			}

			if(IRCConfig.UseHostServ)
			{
				if(!NewNick)
				{
					HostServStatus = true;
					sSender.HostServ("on");
					Log.Notice("HostServ", "Vhost be van kapcsolva.");
				}
				else
				{
					Log.Notice("HostServ", "Vhost ki van kapcsolva.");
					WhoisPrivmsg = sNickInfo.NickStorage;
					ChannelPrivmsg = sNickInfo.NickStorage;
					sChannelInfo.JoinChannel();
				}
			}
			else
			{
				Log.Notice("HostServ", "Vhost ki van kapcsolva.");
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
			Log.Notice("MessageHandler", "Varakozas a kapcsolat feldolgozasara.");
		}

		protected void HandleNotRegistered(IRCMessage sIRCMessage)
		{
			//Log.Notice("MessageHandler", "Test.");
		}

		protected void HandleNoNickName(IRCMessage sIRCMessage)
		{
			Log.Warning("MessageHandler", "Nincs megadva a a bot nick neve!");
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
				Console.Write("[SERVER] ");
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.Write(sIRCMessage.Args + "\n");
				Console.ForegroundColor = ConsoleColor.Gray;
			}

			if(sIRCMessage.Nick == "NickServ")
			{
				if(sIRCMessage.Args.Contains("Password incorrect."))
					Log.Error("NickServ", "Azonosito jelszo hibas!");
				else if(sIRCMessage.Args.Contains("You are already identified."))
					Log.Warning("NickServ", "Azonosito mar aktivalva van!");
				else if(sIRCMessage.Args.Contains("Password accepted - you are now recognized."))
					Log.Success("NickServ", "Azonosito jelszo elfogadva.");
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
				Console.Write("[SERVER] ");
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.Write("Nem letezo irc parancs\n");
				Console.ForegroundColor = ConsoleColor.Gray;
			}
		}

		/// <summary>
		///     Ha a bot elsődleges nickje már használatban van, akkor
		///     átlép a másodlagosra, ha az is akkor a harmadlagosra.
		/// </summary>
		protected void HandleNickError(IRCMessage sIRCMessage)
		{
			Log.Error("MessageHandler", "{0}-t mar hasznalja valaki!", sNickInfo.NickStorage);
			string nick = sNickInfo.ChangeNick();
			Log.Notice("MessageHandler", "Ujra probalom ezzel: {0}", nick);
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

			SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Enabled = 'false', Error = 'csatorna ban' WHERE Channel = '{0}'", sIRCMessage.Info[3].ToLower());
			sSendMessage.SendCMPrivmsg(ChannelPrivmsg, "{0}: csatorna ban", sIRCMessage.Info[3]);
			ChannelPrivmsg = sNickInfo.NickStorage;
		}

		/// <summary>
		///     Ha hibás egy IRC szobának a jelszava, akkor feljegyzi.
		/// </summary>
		protected void HandleNoChannelPassword(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info.Length < 4)
				return;

			SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Enabled = 'false', Error = 'hibas csatorna jelszo' WHERE Channel = '{0}'", sIRCMessage.Info[3].ToLower());
			sSendMessage.SendCMPrivmsg(ChannelPrivmsg, "{0}: hibás csatorna jelszó", sIRCMessage.Info[3]);
			ChannelPrivmsg = sNickInfo.NickStorage;
		}

		/// <summary>
		///     Kigyűjti éppen hol van fent a nick.
		/// </summary>
		protected void HandleMWhois(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info.Length < 5)
				return;

			string text = sIRCMessage.Info.SplitToString(4, " ");
			sSendMessage.SendCMPrivmsg(WhoisPrivmsg, sLManager.GetCommandText("whois", sIRCMessage.Channel), text.Remove(0, 1, ":"));
			WhoisPrivmsg = sNickInfo.NickStorage;
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
					string logfile = string.Format("./{0}/{1}.log", LogConfig.IrcLogDirectory, channel);
					sUtilities.CreateFile(logfile);
					var file = new StreamWriter(logfile, true) { AutoFlush = true };
					file.WriteLine("[{0}] <{1}> {2}", DateTime.Now, user, args);
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