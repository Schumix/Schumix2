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
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Database;
using Schumix.Framework.Extensions;
using Schumix.API;

namespace Schumix.Irc
{
	public sealed class Network : MessageHandler
	{
		private static readonly Dictionary<string, Action<IRCMessage>> _IRCHandler = new Dictionary<string, Action<IRCMessage>>();

        ///<summary>
        ///     A kimeneti adatokat külddi az IRC felé.
        ///</summary>
		public static StreamWriter writer { get; private set; }

        /// <summary>
        ///     A kapcsolatot tároljra.
        /// </summary>
		private TcpClient client;

        /// <summary>
        ///     A bejövő információkat fogadja.
        /// </summary>
		private StreamReader reader;

        /// <summary>
        ///     IRC szerver címe.
        /// </summary>
		private readonly string _server;

        /// <summary>
        ///     IRC port száma.
        /// </summary>
		private readonly int _port;

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

			Log.Notice("Network", "Network sikeresen elindult.");
			sNickInfo.ChangeNick(IRCConfig.NickName);
			InitHandler();

			Task.Factory.StartNew(() => sChannelInfo.ChannelList());

			Log.Debug("Network", "Kapcsolodas indul az irc szerver fele.");
			Connect();

			// Start Opcodes thread
			Log.Debug("Network", "Opcodes thread indul...");
			var opcodes = new Thread(Opcodes);
			opcodes.Start();

			// Start Ping thread
			Log.Debug("Network", "Ping thread indul...");
			var ping = new Thread(Ping);
			ping.Start();
		}

		private void InitHandler()
		{
			RegisterHandler("001",     new Action<IRCMessage>(HandleSuccessfulAuth));
			RegisterHandler("PING",    new Action<IRCMessage>(HandlePing));
			RegisterHandler("PONG",    new Action<IRCMessage>(HandlePong));
			RegisterHandler("PRIVMSG", new Action<IRCMessage>(HandlePrivmsg));
			RegisterHandler("NOTICE",  new Action<IRCMessage>(HandleNotice));
			RegisterHandler("474",     new Action<IRCMessage>(HandleChannelBan));
			RegisterHandler("475",     new Action<IRCMessage>(HandleNoChannelPassword));
			RegisterHandler("319",     new Action<IRCMessage>(HandleMWhois));
			RegisterHandler("421",     new Action<IRCMessage>(HandleIsmeretlenParancs));
			RegisterHandler("433",     new Action<IRCMessage>(HandleNickError));
			RegisterHandler("439",     new Action<IRCMessage>(HandleWaitingForConnection));
			RegisterHandler("451",     new Action<IRCMessage>(HandleNotRegistered));
			RegisterHandler("431",     new Action<IRCMessage>(HandleNoNickName));
			Log.Notice("Network", "Összes IRC handler regisztrálásra került.");
		}

		private static void RegisterHandler(string code, Action<IRCMessage> method)
		{
			_IRCHandler.Add(code, method);
		}

		private static void RemoveHandler(string code)
		{
			_IRCHandler.Remove(code);
		}

		public static void PublicRegisterHandler(string code, Action<IRCMessage> method)
		{
			RegisterHandler(code, method);
		}

		public static void PublicRemoveHandler(string code)
		{
			RemoveHandler(code);
		}

		/// <summary>
        ///     Kapcsolódás az IRC kiszolgálóhoz.
        /// </summary>
		public void Connect()
		{
			Log.Notice("Network", "Kapcsolodas ide megindult: {0}.", _server);
			Connection(true);
		}

        /// <summary>
        ///     Lekapcsolódás az IRC koszolgálótól.
        /// </summary>
		public void DisConnect()
		{
			Close();
			Log.Notice("Network", "Kapcsolat bontva.");
		}

        /// <summary>
        ///     Visszakapcsolódás az IRC kiszolgálóhoz.
        /// </summary>
		public void ReConnect()
		{
			Log.Notice("Network", "Kapcsolat bontasra kerult.");
			Connection(false);
			NewNick = true;
			Log.Debug("Network", "Ujrakapcsolodas ide megindult: {0}.", _server);
		}

		private void Connection(bool b)
		{
			try
			{
				client = new TcpClient();
				client.Connect(_server, _port);
			}
			catch(Exception)
			{
				Log.Error("Network", "Vegzetes hiba tortent a kapcsolat letrehozasanal!");
				return;
			}

			if(client.Connected)
				Log.Success("Network", "A kapcsolat sikeresen letrejott.");
			else
			{
				Log.Error("Network", "Hiba tortent a kapcsolat letrehozasanal!");
				return;
			}

			reader = new StreamReader(client.GetStream());
			writer = new StreamWriter(client.GetStream()) { AutoFlush = true };

			if(b)
			{
				writer.WriteLine("NICK {0}", sNickInfo.NickStorage);
				writer.WriteLine("USER {0} 8 * :{1}", IRCConfig.UserName, IRCConfig.UserInfo);
			}
			else
				sSender.NameInfo(sNickInfo.NickStorage, IRCConfig.UserName, IRCConfig.UserInfo);

			Log.Notice("Network", "Felhasznaloi informaciok el lettek kuldve.");

			NewNick = false;
			HostServStatus = false;
			SchumixBase.UrlTitleEnabled = false;
		}

		private void Close()
		{
			client.Close();
			writer.Dispose();
			reader.Dispose();
		}

        /// <summary>
        ///     Ez a függvény kezeli azt IRC adatai és az opcedes-eket.
        /// </summary>
        /// <remarks>
        ///     Opcodes: Az IRC-ről jövő funkciók, kódok.
        /// </remarks>
		private void Opcodes()
		{
			Log.Notice("Opcodes", "A szal sikeresen elindult.");
			byte number = 0;
			bool enabled = false;
			Log.Notice("Opcodes", "Elindult az irc adatok fogadasa.");

			while(true)
			{
				try
				{
					string IrcMessage;
					if((IrcMessage = reader.ReadLine()).IsNull())
					{
						Log.Error("Opcodes", "Nem jon informacio az irc szerver felol!");
						DisConnect();
						break;
					}

					if(enabled)
					{
						number = 0;
						enabled = false;
					}

					Task.Factory.StartNew(() => HandleIrcCommand(IrcMessage));
				}
				catch(IOException)
				{
					if(sChannelInfo.FSelect("reconnect"))
					{
						if(number <= 6)
						{
							Thread.Sleep(10*1000);
							number++;
						}
						else
							Thread.Sleep(120*1000);

						enabled = true;
						ReConnect();
						continue;
					}
				}
				catch(Exception e)
				{
					Log.Error("Opcodes", sLConsole.Exception("Error"), e.Message);
					Thread.Sleep(1000);
				}
			}

			SchumixBase.timer.SaveUptime();
			Log.Warning("Opcodes", "A program leáll!");
			Thread.Sleep(1000);
			Environment.Exit(1);
		}

		private void HandleIrcCommand(string message)
		{
			var IMessage = new IRCMessage();
			string[] IrcCommand = message.Split(' ');
			IrcCommand[0] = IrcCommand[0].Remove(0, 1, ":");
			IMessage.Hostmask = IrcCommand[0];

			if(IrcCommand.Length > 2)
				IMessage.Channel = IrcCommand[2];

			string[] userdata = IMessage.Hostmask.Split('!');
			IMessage.Nick = userdata[0];

			if(userdata.Length > 1)
			{
				string[] hostdata = userdata[1].Split('@');
				IMessage.User = hostdata[0];
				IMessage.Host = hostdata[1];
			}

			string opcode = IrcCommand[1];
			IMessage.Info = IrcCommand;
			IMessage.Args = IrcCommand.SplitToString(3, " ");
			IMessage.Args = IMessage.Args.Remove(0, 1, ":");

			if(_IRCHandler.ContainsKey(opcode))
				_IRCHandler[opcode].Invoke(IMessage);
			else
			{
				if(ConsoleLog.CLog)
					Log.Notice("HandleIrcCommand", "Received unhandled opcode: {0}", opcode);
			}
		}

        /// <summary>
        ///     Pingeli az IRC szervert 30 másodpercenként.
        /// </summary>
		private void Ping()
		{
			Log.Notice("Ping", "A szal sikeresen elindult.");

			while(true)
			{
				try
				{
					sSender.Ping(_server);
				}
				catch(IOException)
				{
					// no code
				}
				catch(Exception e)
				{
					Log.Error("Ping", sLConsole.Exception("Error"), e.Message);
				}

				Thread.Sleep(30*1000);
			}
		}
	}
}