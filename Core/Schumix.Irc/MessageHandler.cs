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
using Schumix.Irc.Commands;

namespace Schumix.Irc
{
	public struct IRCMessage
	{
		public string Hostmask { get; set; }
		public string Channel { get; set; }
		public string Args { get; set; }
		public string Nick { get; set; }
		public string User { get; set; }
		public string Host { get; set; }
		public string[] Info { get; set; }
	}

	public partial class MessageHandler : CommandManager
	{
		public bool HostServAllapot;

		protected MessageHandler() {}

		protected void HandleSuccessfulAuth()
		{
			Console.Write("\n");
			Log.Success("MessageHandler", "Sikeres kapcsolodas.");

			if(IRCConfig.UseNickServ)
			{
				Log.Notice("NickServ", "NickServ azonosito kuldese.");

				if(!Network.NewNick)
					sSender.NickServ(IRCConfig.NickServPassword);
			}

			if(IRCConfig.UseHostServ)
			{
				if(!Network.NewNick)
				{
					HostServAllapot = true;
					Log.Notice("HostServ", "HostServ bevan kapcsolva.");
					sSender.HostServ("on");
				}
				else
				{
					Log.Notice("HostServ", "HostServ kivan kapcsolva.");
					WhoisPrivmsg = sNickInfo.NickStorage;
					ChannelPrivmsg = sNickInfo.NickStorage;
					Network.sChannelInfo.JoinChannel();
				}
			}
			else
			{
				Log.Notice("HostServ", "HostServ kivan kapcsolva.");
				if(IRCConfig.HostServAllapot)
					sSender.HostServ("off");

				WhoisPrivmsg = sNickInfo.NickStorage;
				ChannelPrivmsg = sNickInfo.NickStorage;
				Network.sChannelInfo.JoinChannel();
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
        ///     Ha a szobában a köszönés funkció be van kapcsolva,
        ///     akkor köszön az éppen belépőnek.
        /// </summary>
		protected void HandleMJoin()
		{
			if(Network.IMessage.Nick == sNickInfo.NickStorage)
				return;

			string channel = Network.IMessage.Channel;

			if(channel.Substring(0, 1) == ":")
				channel = channel.Remove(0, 1);

			if(Network.sChannelInfo.FSelect("koszones") == "be" && Network.sChannelInfo.FSelect("koszones", channel) == "be")
			{
				Random rand = new Random();
				string Koszones = "";
				switch(rand.Next(0, 12))
				{
					case 0:
						Koszones = "Hello";
						break;
					case 1:
						Koszones = "Csáó";
						break;
					case 2:
						Koszones = "Hy";
						break;
					case 3:
						Koszones = "Szevasz";
						break;
					case 4:
						Koszones = "Üdv";
						break;
					case 5:
						Koszones = "Szervusz";
						break;
					case 6:
						Koszones = "Aloha";
						break;
					case 7:
						Koszones = "Jó napot";
						break;
					case 8:
						Koszones = "Szia";
						break;
					case 9:
						Koszones = "Hi";
						break;
					case 10:
						Koszones = "Szerbusz";
						break;
					case 11:
						Koszones = "Hali";
						break;
					case 12:
						Koszones = "Szeva";
						break;
				}
	
				if(DateTime.Now.Hour <= 9)
					sSendMessage.SendCMPrivmsg(channel, "Jó reggelt {0}", Network.IMessage.Nick);
				else if(DateTime.Now.Hour >= 20)
					sSendMessage.SendCMPrivmsg(channel, "Jó estét {0}", Network.IMessage.Nick);
				else
				{
					if(Admin(Network.IMessage.Nick))
						sSendMessage.SendCMPrivmsg(channel, "Üdv főnök");
					else
						sSendMessage.SendCMPrivmsg(channel, "{0} {1}", Koszones, Network.IMessage.Nick);
				}
			}
		}

        /// <summary>
        ///     Ha ez a funkció be van kapcsolva, akkor
        ///     miután a nick elhagyta a szobát elköszön tőle.
        /// </summary>
		protected void HandleMLeft()
		{
			if(Network.IMessage.Nick == sNickInfo.NickStorage)
				return;

			if(Network.sChannelInfo.FSelect("koszones") == "be" && Network.sChannelInfo.FSelect("koszones", Network.IMessage.Channel) == "be")
			{
				Random rand = new Random();
				string elkoszones = "";
				switch(rand.Next(0, 1))
				{
					case 0:
						elkoszones = "Viszlát";
						break;
					case 1:
						elkoszones = "Bye";
						break;
				}
	
				if(DateTime.Now.Hour >= 20)
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Jóét {0}", Network.IMessage.Nick);
				else
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "{0} {1}", elkoszones, Network.IMessage.Nick);
			}
		}

        /// <summary>
        ///     Ha a ConsoleLog be van kapcsolva, akkor
        ///     kiírja a console-ra az IRC szerverről fogadott információkat.
        /// </summary>
		protected void HandleNotice()
		{
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
				if(Network.IMessage.Args.IndexOf("Password incorrect.") != -1)
					Log.Error("NickServ", "NickServ azonosito jelszo hibas!");
				else if(Network.IMessage.Args.IndexOf("You are already identified.") != -1)
					Log.Warning("NickServ", "NickServ azonosito mar aktivalva van!");
				else if(Network.IMessage.Args.IndexOf("Password accepted - you are now recognized.") != -1)
					Log.Success("NickServ", "NickServ azonosito jelszo elfogadva.");
			}

			if(Network.IMessage.Nick == "HostServ" && IRCConfig.UseHostServ)
			{
				if(Network.IMessage.Args.IndexOf("Your vhost of") != -1 && HostServAllapot)
				{
					WhoisPrivmsg = sNickInfo.NickStorage;
					ChannelPrivmsg = sNickInfo.NickStorage;
					Network.sChannelInfo.JoinChannel();
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
			Network.Status = true;
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
				Console.Write("Nemletezo irc parancs\n");
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
			Network.NewNick = true;
			sSender.Nick(nick);
		}

        /// <summary>
		///     Ha bannolva van egy szobából, akkor feljegyzi.
        /// </summary>
		protected void HandleChannelBan()
		{
			if(Network.IMessage.Info.Length < 4)
				return;

			SchumixBase.mSQLConn.QueryFirstRow("UPDATE channel SET aktivitas = 'nem aktiv', error = 'channel ban' WHERE szoba = '{0}'", Network.IMessage.Info[3]);
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

			SchumixBase.mSQLConn.QueryFirstRow("UPDATE channel SET aktivitas = 'nem aktiv', error = 'hibas channel jelszo' WHERE szoba = '{0}'", Network.IMessage.Info[3]);
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

			string alomany = "";
			for(int i = 4; i < Network.IMessage.Info.Length; i++)
				alomany += Network.IMessage.Info[i] + " ";

			if(alomany.Substring(0, 1) == ":")
				alomany = alomany.Remove(0, 1);

			sSendMessage.SendCMPrivmsg(WhoisPrivmsg, "Jelenleg itt van fent: {0}", alomany);
			WhoisPrivmsg = sNickInfo.NickStorage;
		}

        /// <summary>
        ///     Ha engedélyezett a ConsolLog, akkor kiírja a Console-ra ha kickelnek valakit.
        /// </summary>
		protected void HandleMKick()
		{
			if(Network.IMessage.Info.Length < 5)
				return;

			if(Network.IMessage.Info[3] == sNickInfo.NickStorage)
			{
				if(Network.sChannelInfo.FSelect("rejoin") == "be" && Network.sChannelInfo.FSelect("rejoin", Network.IMessage.Channel) == "be")
				{
					foreach(var m_channel in Network.sChannelInfo.CLista)
					{
						if(Network.IMessage.Channel == m_channel.Key)
							sSender.Join(m_channel.Key, m_channel.Value);
					}
				}
			}
			else
			{
				if(Network.sChannelInfo.FSelect("parancsok") == "be" && Network.sChannelInfo.FSelect("parancsok", Network.IMessage.Channel) == "be")
				{
					if(ConsoleLog.CLog)
					{
						string alomany = "";
						for(int i = 4; i < Network.IMessage.Info.Length; i++)
							alomany += Network.IMessage.Info[i] + " ";
			
						if(alomany.Substring(0, 1) == ":")
							alomany = alomany.Remove(0, 1);
			
						Console.WriteLine("{0} kickelte a következő felhasználot: {1} oka: {2}", Network.IMessage.Nick, Network.IMessage.Info[3], alomany);
					}
				}
			}
		}

        /// <summary>
        ///     
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="user"></param>
        /// <param name="args"></param>
		private void LogToFajl(string channel, string user, string args)
        {
			if(Network.sChannelInfo.FSelect("log") == "be" && Network.sChannelInfo.FSelect("log", channel) == "be")
			{
				if(!Directory.Exists(LogConfig.LogHelye))
					Directory.CreateDirectory(LogConfig.LogHelye);

				try
				{
					string logfile_name = channel + ".log";
					if(!File.Exists(String.Format("./{0}/{1}", LogConfig.LogHelye, logfile_name)))
						File.Create(String.Format("./{0}/{1}", LogConfig.LogHelye, logfile_name));

					var file = new StreamWriter(String.Format("./{0}/{1}", LogConfig.LogHelye, logfile_name), true) { AutoFlush = true };
					file.WriteLine("[{0}] <{1}> {2}", DateTime.Now, user, args);
					file.Close();
				}
				catch(Exception e)
				{
					Log.Error("MessageHandler", "Hiba oka: {0}", e.ToString());
				}
			}
		}
	}
}