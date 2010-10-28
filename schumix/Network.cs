/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010 Megax <http://www.megaxx.info/>
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

namespace Schumix
{
	public class Network
	{
        /// <summary>
        ///     Opcodes.cs -t hívja meg.
        /// </summary>
		private Opcodes sOpcodes = Singleton<Opcodes>.Instance;

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

			Connect();

			// Start Opcodes thread
			Thread opcodes = new Thread(new ThreadStart(Opcodes));
			opcodes.Start();

			// Start Ping thread
			Thread ping = new Thread(new ThreadStart(Ping));
			ping.Start();
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

			writer.WriteLine("USER {0} 8 * :{1}", SchumixBot.NickTarolo, SchumixBot.NickTarolo);
			writer.WriteLine("NICK {0}", SchumixBot.NickTarolo);

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

			writer.WriteLine("USER {0} 8 * :{1}", SchumixBot.NickTarolo, SchumixBot.NickTarolo);
			writer.WriteLine("NICK {0}", SchumixBot.NickTarolo);

			Status = true;
			m_running = true;

			m_ConnState = (int)ConnState.CONN_REGISTERED;
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

						sOpcodes.hostmask = IrcCommand[0];
						userdata = sOpcodes.hostmask.Split('!');

						sOpcodes.source_nick = userdata[0];
						sOpcodes.args = "";

						for(int i = 3; i < IrcCommand.Length; i++)
							sOpcodes.args += IrcCommand[i] + " ";

						opcode = IrcCommand[1];

						switch(opcode)
						{
							case "PRIVMSG":
								sOpcodes.OpcodePrivmsg(IrcCommand);
								break;
							case "NOTICE":
								sOpcodes.OpcodeNotice(IrcCommand);
								break;
							case "PING":
								sOpcodes.OpcodePing(IrcCommand);
								break;
							case "PONG":
								sOpcodes.OpcodePong(IrcCommand);
								break;
							case "JOIN":
								sOpcodes.OpcodeJoin(IrcCommand);
								break;
							case "PART":
								sOpcodes.OpcodeLeft(IrcCommand);
								break;
							case "KICK":
								sOpcodes.OpcodeKick(IrcCommand);
								break;
							case "001":
								sOpcodes.OpcodeSikeresKapcsolodas(IrcCommand);
								break;
							case "421":
								sOpcodes.OpcodeIsmeretlenParancs(IrcCommand);
								break;
							case "433":
								sOpcodes.OpcodeNickError(IrcCommand);
								break;
							case "474":
								sOpcodes.OpcodeChannelBan(IrcCommand);
								break;
							case "475":
								sOpcodes.OpcodeNoChannelJelszo(IrcCommand);
								break;
							case "319":
								sOpcodes.OpcodeWhois(IrcCommand);
								break;
							case "QUIT":
							case "372":
							case "002":
							case "003":
							case "004":
							case "005":
							case "042":
							case "251":
							case "252":
							case "253":
							case "254":
							case "255":
							case "265":
							case "266":
							case "332":
							case "333":
							case "353":
							case "366":
							case "412":
							case "439":
							case "375":
							case "376":
							case "451":
							case "310":
							case "311":
							case "312":
							case "317":
							case "318":
							case "338":
							case "307":
							case "671":
							case "672":
								break;
							default:
								sOpcodes.OpcodeConsol(IrcCommand);
								break;
						}

						if(m_ConnState == (int)ConnState.CONN_CONNECTED)
						{
							writer.WriteLine("USER {0} 8 * :{1}", SchumixBot.NickTarolo, SchumixBot.NickTarolo);
							writer.WriteLine("NICK {0}", SchumixBot.NickTarolo);
							m_ConnState = (int)ConnState.CONN_REGISTERED;
						}
					}
					else
						Thread.Sleep(50);
				}
			}
			catch(Exception e)
			{
				if(m_running)
					Log.Error("Opcodes", String.Format("Hiba oka: {0}", e.ToString()));

				Opcodes();
				Thread.Sleep(50);
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
						Thread.Sleep(10*1000);
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