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
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Collections.Generic;
using Schumix.IRC;
using Schumix.Config;

namespace Schumix
{
	public class Network
	{
		private MessageHandler sMessageHandler = Singleton<MessageHandler>.Instance;
		public static IRCMessage IMessage = new IRCMessage();
		
        ///***START***********************************///
        ///<summary>
        ///     A kimeneti adatokat külddi az IRC felé.
        ///</summary>
		public static StreamWriter writer;

        /// <summary>
        ///     
        /// </summary>
		private NetworkStream stream;

        /// <summary>
        ///     
        /// </summary>
		private TcpClient irc;

        /// <summary>
        ///     
        /// </summary>
		private StreamReader reader;
        ///***END***********************************///

        /// <summary>
        ///     Ha 0 akkor nem kapcsolódott szerverhez,
        ///     ha 1 akkor kapcsolódik,
        ///     ha 2 akkor kapcsolódott.
        /// </summary>
        /// <remarks>
        ///     Az lábbi 3 változót töltjük bele az "m_ConnState"-be
        ///     azért van, hogy ne számokkal kelljen dolgozni.
        ///</remarks>
		public static int m_ConnState = 0;

        /// <summary>
        ///     Ha "true" akkor jött pong az IRC felől,
        ///     ha "false" akkor nem és elindul a reconnect() függvény.
        /// </summary>
		public static bool Status;

        /// <summary>
        ///     Segédváltozó a "Status" váltotó mellé.
        ///     Ha "true" akkor van kapcsolat az IRC-el,
        ///     ha "false" akkor nincs.
        ///     Meggátolja, hogy hiba legyen.
        /// </summary>
		private bool m_running;

        /// <summary>
        ///     IRC szerver címe.
        /// </summary>
		private string _server;

        /// <summary>
        ///     IRC port száma.
        /// </summary>
		private int _port;

		public enum ConnState
		{
	        ///***************************************///
	        /// <summary>
	        ///     Nincs kapcsolat.
	        /// </summary>
			CONN_CONNECTED = 0,
	
	        /// <summary>
	        ///     Kapcsolódás.
	        /// </summary>
			CONN_REGISTERING,
	
	        /// <summary>
	        ///     Van kapcsolódás.
	        /// </summary>
			CONN_REGISTERED
	        ///***************************************///
		};

		private Dictionary<string, Action> _IRCHandler = new Dictionary<string, Action>();

        /// <summary>
        ///     Internet kapcsolat függvénye.
        ///     Itt indul el a kapcsolat az IRC szerverrel.
        /// </summary>
        /// <param name="server">
        ///     <see cref="Network._server"/>
        /// </param>
        /// <param name="port">
        ///     <see cref="Network._port"/>
        /// </param>
		public Network(string server, int port)
		{
			_server = server;
			_port = port;

			Log.Notice("Network", "Network elindult.");
			InitHandler();
			Connect();

			// Start Opcodes thread
			Thread opcodes = new Thread(new ThreadStart(Opcodes));
			opcodes.Start();

			// Start Ping thread
			Thread ping = new Thread(new ThreadStart(Ping));
			ping.Start();
		}

		private void InitHandler()
		{
			RegisterHandler("001",     sMessageHandler.HandleSuccessfulAuth);
			RegisterHandler("PING",    sMessageHandler.HandlePing);
			RegisterHandler("PONG",    sMessageHandler.HandlePong);
			RegisterHandler("PRIVMSG", sMessageHandler.HandlePrivmsg);
			RegisterHandler("NOTICE",  sMessageHandler.HandleNotice);
			RegisterHandler("JOIN",    sMessageHandler.HandleJoin);
			RegisterHandler("LEFT",    sMessageHandler.HandleLeft);
			RegisterHandler("KICK",    sMessageHandler.HandleKick);
			RegisterHandler("474",     sMessageHandler.HandleChannelBan);
			RegisterHandler("475",     sMessageHandler.HandleNoChannelJelszo);
			RegisterHandler("319",     sMessageHandler.HandleWhois);
			RegisterHandler("421",     sMessageHandler.HandleIsmeretlenParancs);
			RegisterHandler("433",     sMessageHandler.HandleNickError);
			Log.Notice("Network", "Osszes IRC handler regisztralva.");
		}

		private void RegisterHandler(string code, Action method)
		{
			_IRCHandler.Add(code, method);
		}

		/// <summary>
        ///     Kapcsolódás az IRC kiszolgálóhoz.
        /// </summary>
		public void Connect()
		{
			Log.Notice("Network", String.Format("Kapcsolodas ide megindult: {0}.", _server));
			irc = new TcpClient(_server, _port);
			stream = irc.GetStream();
			reader = new StreamReader(stream);
			writer = new StreamWriter(stream);
			writer.AutoFlush = true;

			writer.WriteLine("NICK {0}", SchumixBot.NickTarolo);
			writer.WriteLine("USER {0} 8 * :{1}", IRCConfig.UserName, IRCConfig.UserName);
			m_ConnState = (int)ConnState.CONN_REGISTERED;

			Status = true;
			m_running = true;
		}

        /// <summary>
        ///     Lekapcsolódás az IRC koszolgálótól.
        /// </summary>
		public void DisConnect()
		{
			m_running = false;
			Status = false;

			writer.Close();
			reader.Close();
			irc.Close();
			Log.Notice("Network", "Kapcsolat bontva.");
		}

        /// <summary>
        ///     Visszakapcsolódás az IRC kiszolgálóhoz.
        /// </summary>
		public void ReConnect()
		{
			m_running = false;
			Status = false;

			writer.Close();
			reader.Close();
			irc.Close();
			Log.Notice("Network", "Kapcsolat bontva.");

			Log.Debug("Network", String.Format("Ujrakapcsolodas ide megindult: {0}.", _server));
			irc = new TcpClient(_server, _port);
			stream = irc.GetStream();
			reader = new StreamReader(stream);
			writer = new StreamWriter(stream);
			writer.AutoFlush = true;

			writer.WriteLine("NICK {0}", SchumixBot.NickTarolo);
			writer.WriteLine("USER {0} 8 * :{1}", IRCConfig.UserName, IRCConfig.UserName);
			m_ConnState = (int)ConnState.CONN_REGISTERED;

			Status = true;
			m_running = true;
		}

        /// <summary>
        ///     Ez a függvény kezeli azt IRC adatai és az opcedes-eket.
        /// </summary>
        /// <remarks>
        ///     Opcodes : Az IRC-ről jövő funkciók, kódok.
        /// </remarks>
		private void Opcodes()
		{
			try
			{
				string IrcOpcode;
				string opcode;
				string[] userdata;
				string[] hostdata;
				string[] IrcCommand;

				while(true)
				{
					if(m_running)
					{
						if((IrcOpcode = reader.ReadLine()) == null)
							break;

						IrcCommand = IrcOpcode.Split(' ');

						if(IrcCommand[0].Substring(0, 1) == ":")
							IrcCommand[0] = IrcCommand[0].Remove(0, 1);

						IMessage.Hostmask = IrcCommand[0];
						userdata = IMessage.Hostmask.Split('!');

						IMessage.Args = "";
						if(IrcCommand.Length > 2)
							IMessage.Channel = IrcCommand[2];

						IMessage.Nick = userdata[0];
						if(userdata.Length > 1)
						{
							hostdata = userdata[1].Split('@');
							IMessage.User = hostdata[0];
							IMessage.Host = hostdata[1];
						}

						for(int i = 3; i < IrcCommand.Length; i++)
							IMessage.Args += IrcCommand[i] + " ";

						opcode = IrcCommand[1];
						IMessage.Info = IrcCommand;

						if(IMessage.Args != "" && IMessage.Args.Substring(0, 1) == ":")
							IMessage.Args = IMessage.Args.Remove(0, 1);

						if(_IRCHandler.ContainsKey(opcode))
							_IRCHandler[opcode].Invoke();
						else
						{
							if(Consol.ConsoleLog == 1)
								Log.Notice("Opcodes", String.Format("Received unhandled opcode: {0}", opcode));
						}

						if(m_ConnState == (int)ConnState.CONN_CONNECTED)
						{
							writer.WriteLine("NICK {0}", SchumixBot.NickTarolo);
							writer.WriteLine("USER {0} 8 * :{1}", IRCConfig.UserName, IRCConfig.UserName);
							m_ConnState = (int)ConnState.CONN_REGISTERED;
						}
					}
					else
						Thread.Sleep(100);
				}
			}
			catch(Exception e)
			{
				if(m_running)
					Log.Error("Opcodes", String.Format("Hiba oka: {0}", e.ToString()));

				Opcodes();
				Thread.Sleep(100);
			}
		}

        /// <summary>
        ///     Pingeli az IRC szervert 15 másodpercenként.
        /// </summary>
		private void Ping()
		{
			try
			{
				while(true)
				{
					if(Status)
					{
						writer.WriteLine("PING :{0}", _server);
						Status = false;
						Thread.Sleep(15*1000);
					}
					else
					{
						ReConnect();
						Thread.Sleep(15*1000);
					}
				}
			}
			catch(Exception e)
			{
				if(m_running)
					Log.Error("Ping", String.Format("Hiba oka: {0}", e.ToString()));
				else
					Thread.Sleep(20*1000);

				Ping();
			}
		}
	}
}