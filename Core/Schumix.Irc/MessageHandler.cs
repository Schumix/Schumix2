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
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Irc.Commands;
using Schumix.Framework.Extensions;

namespace Schumix.Irc
{
	public partial class MessageHandler : CommandManager
	{
		protected bool HostServAllapot;
		protected bool Status;
		protected bool NewNick;
		protected MessageHandler() {}

		protected void HandleSuccessfulAuth()
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
					HostServAllapot = true;
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
		}

		protected void HandleWaitingForConnection()
		{
			Log.Notice("MessageHandler", "Varakozas a kapcsolat feldolgozasara.");
		}

		protected void HandleNotRegistered()
		{
			//Log.Notice("MessageHandler", "Teszt.");
		}

		/// <summary>
		///     Ha a ConsoleLog be van kapcsolva, akkor
		///     kiírja a console-ra az IRC szerverről fogadott információkat.
		/// </summary>
		protected void HandleNotice()
		{
			foreach(var plugin in sAddonManager.GetPlugins())
				plugin.HandleNotice();

			if(ConsoleLog.CLog)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write("[SERVER] ");
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.Write(Network.IMessage.Args + "\n");
				Console.ForegroundColor = ConsoleColor.Gray;
			}

			if(Network.IMessage.Nick == "NickServ")
			{
				if(Network.IMessage.Args.Contains("Password incorrect."))
					Log.Error("NickServ", "Azonosito jelszo hibas!");
				else if(Network.IMessage.Args.Contains("You are already identified."))
					Log.Warning("NickServ", "Azonosito mar aktivalva van!");
				else if(Network.IMessage.Args.Contains("Password accepted - you are now recognized."))
					Log.Success("NickServ", "Azonosito jelszo elfogadva.");
			}

			if(Network.IMessage.Nick == "HostServ" && IRCConfig.UseHostServ)
			{
				if(Network.IMessage.Args.Contains("Your vhost of") && HostServAllapot)
				{
					WhoisPrivmsg = sNickInfo.NickStorage;
					ChannelPrivmsg = sNickInfo.NickStorage;
					sChannelInfo.JoinChannel();
					HostServAllapot = false;
				}
			}
		}

		/// <summary>
		///     Válaszol, ha valaki pingeli a botot.
		/// </summary>
		protected void HandlePing()
		{
			sSender.Ping(Network.IMessage.Args);
		}

		/// <summary>
		///     Válaszol, ha valaki pongolja a botot.
		/// </summary>
		protected void HandlePong()
		{
			sSender.Pong(Network.IMessage.Args);
			Status = true;
		}

		/// <summary>
		///     Ha ismeretlen parancs jön, akkor kiírja.
		/// </summary>
		protected void HandleIsmeretlenParancs()
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
		protected void HandleNickError()
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
		protected void HandleChannelBan()
		{
			if(Network.IMessage.Info.Length < 4)
				return;

			SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Enabled = 'false', Error = 'csatorna ban' WHERE Channel = '{0}'", Network.IMessage.Info[3]);
			sSendMessage.SendCMPrivmsg(ChannelPrivmsg, "{0}: channel ban", Network.IMessage.Info[3]);
			ChannelPrivmsg = sNickInfo.NickStorage;
		}

		/// <summary>
		///     Ha hibás egy IRC szobának a jelszava, akkor feljegyzi.
		/// </summary>
		protected void HandleNoChannelPassword()
		{
			if(Network.IMessage.Info.Length < 4)
				return;

			SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Enabled = 'false', Error = 'hibas csatorna jelszo' WHERE Channel = '{0}'", Network.IMessage.Info[3]);
			sSendMessage.SendCMPrivmsg(ChannelPrivmsg, "{0}: hibás channel jelszó", Network.IMessage.Info[3]);
			ChannelPrivmsg = sNickInfo.NickStorage;
		}

		/// <summary>
		///     Kigyűjti éppen hol van fent a nick.
		/// </summary>
		protected void HandleMWhois()
		{
			if(Network.IMessage.Info.Length < 5)
				return;

			string alomany = Network.IMessage.Info.SplitToString(4, " ");
			sSendMessage.SendCMPrivmsg(WhoisPrivmsg, "Jelenleg itt van fent: {0}", alomany.Remove(0, 1, ":"));
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
					if(!Directory.Exists(LogConfig.IrcLogDirectory))
						Directory.CreateDirectory(LogConfig.IrcLogDirectory);

					string logfile_name = channel + ".log";
					if(!File.Exists(string.Format("./{0}/{1}", LogConfig.IrcLogDirectory, logfile_name)))
						File.Create(string.Format("./{0}/{1}", LogConfig.IrcLogDirectory, logfile_name));

					var file = new StreamWriter(string.Format("./{0}/{1}", LogConfig.IrcLogDirectory, logfile_name), true) { AutoFlush = true };
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