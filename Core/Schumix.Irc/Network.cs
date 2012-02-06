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
//using System.Timers;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using Org.Mentalis.Security.Ssl;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Database;
using Schumix.Framework.Extensions;
using Schumix.API;

namespace Schumix.Irc
{
	public sealed class Network : MessageHandler
	{
		private static readonly Dictionary<ReplyCode, IRCDelegate> _IRCHandler = new Dictionary<ReplyCode, IRCDelegate>();
		private static readonly Dictionary<string, IRCDelegate> _IRCHandler2 = new Dictionary<string, IRCDelegate>();
		private static readonly Dictionary<int, IRCDelegate> _IRCHandler3 = new Dictionary<int, IRCDelegate>();
		//private System.Timers.Timer _timeropcode = new System.Timers.Timer();

        /// <summary>
        ///     A kapcsolatot tároljra.
        /// </summary>
		private SecureTcpClient sclient;
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
		private bool _enabled = false;
		//private DateTime LastOpcode;

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

			Log.Notice("Network", sLConsole.Network("Text"));
			sNickInfo.ChangeNick(IRCConfig.NickName);
			InitHandler();

			Task.Factory.StartNew(() => sChannelInfo.ChannelList());

			Log.Debug("Network", sLConsole.Network("Text2"));
			Connect();

			// Start Opcodes thread
			Log.Debug("Network", sLConsole.Network("Text3"));
			var opcodes = new Thread(Opcodes);
			opcodes.Start();

			// Start Ping thread
			Log.Debug("Network", sLConsole.Network("Text4"));
			var ping = new Thread(AutoPing);
			ping.Start();
		}

		private void InitHandler()
		{
			RegisterHandler(ReplyCode.RPL_WELCOME,          HandleSuccessfulAuth);
			RegisterHandler("PING",                         HandlePing);
			RegisterHandler("PONG",                         HandlePong);
			RegisterHandler("PRIVMSG",                      HandlePrivmsg);
			RegisterHandler("NOTICE",                       HandleNotice);
			RegisterHandler(ReplyCode.ERR_BANNEDFROMCHAN,   HandleChannelBan);
			RegisterHandler(ReplyCode.ERR_BADCHANNELKEY,    HandleNoChannelPassword);
			RegisterHandler(ReplyCode.RPL_WHOISCHANNELS,    HandleMWhois);
			RegisterHandler(ReplyCode.ERR_NOSUCHNICK,       HandleNoWhois);
			RegisterHandler(ReplyCode.ERR_UNKNOWNCOMMAND,   HandleUnknownCommand);
			RegisterHandler(ReplyCode.ERR_NICKNAMEINUSE,    HandleNickError);
			RegisterHandler(439,                            HandleWaitingForConnection);
			RegisterHandler(ReplyCode.ERR_NOTREGISTERED,    HandleNotRegistered);
			RegisterHandler(ReplyCode.ERR_NONICKNAMEGIVEN,  HandleNoNickName);
			RegisterHandler("JOIN",                         HandleIrcJoin);
			RegisterHandler("PART",                         HandleIrcLeft);
			RegisterHandler("KICK",                         HandleIrcKick);
			RegisterHandler("QUIT",                         HandleIrcQuit);
			RegisterHandler("NICK",                         HandleNewNick);
			RegisterHandler(ReplyCode.RPL_NAMREPLY,         HandleNameList);
			RegisterHandler(ReplyCode.ERR_ERRONEUSNICKNAME, HandlerErrorNewNickName);
			RegisterHandler(ReplyCode.ERR_UNAVAILRESOURCE,  HandleNicknameWhileBannedOrModeratedOnChannel);
			Log.Notice("Network", sLConsole.Network("Text5"));
		}

		private static void RegisterHandler(ReplyCode code, IRCDelegate method)
		{
			if(_IRCHandler.ContainsKey(code))
				_IRCHandler[code] += method;
			else
				_IRCHandler.Add(code, method);
		}

		private static void RemoveHandler(ReplyCode code)
		{
			if(_IRCHandler.ContainsKey(code))
				_IRCHandler.Remove(code);
		}

		private static void RemoveHandler(ReplyCode code, IRCDelegate method)
		{
			if(_IRCHandler.ContainsKey(code))
				_IRCHandler[code] -= method;
		}

		public static void PublicRegisterHandler(ReplyCode code, IRCDelegate method)
		{
			RegisterHandler(code, method);
		}

		public static void PublicRemoveHandler(ReplyCode code)
		{
			RemoveHandler(code);
		}

		public static void PublicRemoveHandler(ReplyCode code, IRCDelegate method)
		{
			RemoveHandler(code, method);
		}

		private static void RegisterHandler(string code, IRCDelegate method)
		{
			if(_IRCHandler2.ContainsKey(code))
				_IRCHandler2[code] += method;
			else
				_IRCHandler2.Add(code, method);
		}

		private static void RemoveHandler(string code)
		{
			if(_IRCHandler2.ContainsKey(code))
				_IRCHandler2.Remove(code);
		}

		private static void RemoveHandler(string code, IRCDelegate method)
		{
			if(_IRCHandler2.ContainsKey(code))
				_IRCHandler2[code] -= method;
		}

		public static void PublicRegisterHandler(string code, IRCDelegate method)
		{
			RegisterHandler(code, method);
		}

		public static void PublicRemoveHandler(string code)
		{
			RemoveHandler(code);
		}

		public static void PublicRemoveHandler(string code, IRCDelegate method)
		{
			RemoveHandler(code, method);
		}

		private static void RegisterHandler(int code, IRCDelegate method)
		{
			if(_IRCHandler3.ContainsKey(code))
				_IRCHandler3[code] += method;
			else
				_IRCHandler3.Add(code, method);
		}

		private static void RemoveHandler(int code)
		{
			if(_IRCHandler3.ContainsKey(code))
				_IRCHandler3.Remove(code);
		}

		private static void RemoveHandler(int code, IRCDelegate method)
		{
			if(_IRCHandler3.ContainsKey(code))
				_IRCHandler3[code] -= method;
		}

		public static void PublicRegisterHandler(int code, IRCDelegate method)
		{
			RegisterHandler(code, method);
		}

		public static void PublicRemoveHandler(int code)
		{
			RemoveHandler(code);
		}

		public static void PublicRemoveHandler(int code, IRCDelegate method)
		{
			RemoveHandler(code, method);
		}

		/// <summary>
        ///     Kapcsolódás az IRC kiszolgálóhoz.
        /// </summary>
		public void Connect()
		{
			Log.Notice("Network", sLConsole.Network("Text6"), _server);
			Connection(true);
		}

        /// <summary>
        ///     Lekapcsolódás az IRC koszolgálótól.
        /// </summary>
		public void DisConnect()
		{
			Close();
			Log.Notice("Network", sLConsole.Network("Text7"));
		}

        /// <summary>
        ///     Visszakapcsolódás az IRC kiszolgálóhoz.
        /// </summary>
		public void ReConnect()
		{
			if(SchumixBase.ExitStatus)
				return;

			Log.Notice("Network", sLConsole.Network("Text8"));
			Connection(false);
			NewNick = true;
			Log.Debug("Network", sLConsole.Network("Text9"), _server);
		}

		private void Connection(bool b)
		{
			try
			{
				if(IRCConfig.Ssl)
				{
					var options = new SecurityOptions(SecureProtocol.Tls1);
					options.Certificate = null;
					options.Entity = ConnectionEnd.Client;
					options.VerificationType = CredentialVerification.None;
					options.Flags = SecurityFlags.Default;
					options.AllowedAlgorithms = SslAlgorithms.SECURE_CIPHERS;
					sclient = new SecureTcpClient(options);		
					sclient.Connect(_server, _port);
				}
				else
				{
					client = new TcpClient();
					client.Connect(_server, _port);
				}
			}
			catch(Exception)
			{
				Log.Error("Network", sLConsole.Network("Text10"));
				return;
			}

			if(!IRCConfig.Ssl)
			{
				if(client.Connected)
					Log.Success("Network", sLConsole.Network("Text11"));
				else
				{
					Log.Error("Network", sLConsole.Network("Text12"));
					return;
				}

				reader = new StreamReader(client.GetStream());
				INetwork.Writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
			}
			else
			{
				reader = new StreamReader(sclient.GetStream());
				INetwork.Writer = new StreamWriter(sclient.GetStream()) { AutoFlush = true };
			}

			if(b)
			{
				INetwork.Writer.WriteLine("NICK {0}", sNickInfo.NickStorage);
				INetwork.Writer.WriteLine("USER {0} 8 * :{1}", IRCConfig.UserName, IRCConfig.UserInfo);
			}
			else
				sSender.NameInfo(sNickInfo.NickStorage, IRCConfig.UserName, IRCConfig.UserInfo);

			Log.Notice("Network", sLConsole.Network("Text13"));
			_enabled = true;
			NewNick = false;
			HostServStatus = false;
			SchumixBase.UrlTitleEnabled = false;
		}

		private void Close()
		{
			if(!IRCConfig.Ssl)
				client.Close();

			INetwork.Writer.Dispose();
			reader.Dispose();
		}

		//private void HandleOpcodesTimer(object sender, ElapsedEventArgs e)
		//{
		//	Console.WriteLine((DateTime.Now - LastOpcode).Minutes);
		//	if((DateTime.Now - LastOpcode).Minutes >= 1)
		//		ReConnect();
		//}

        /// <summary>
        ///     Ez a függvény kezeli azt IRC adatai és az opcedes-eket.
        /// </summary>
        /// <remarks>
        ///     Opcodes: Az IRC-ről jövő funkciók, kódok.
        /// </remarks>
		private void Opcodes()
		{
			Log.Notice("Opcodes", sLConsole.Network("Text14"));
			byte number = 0;
			//_timeropcode.Interval = 60*1000;
			//_timeropcode.Elapsed += HandleOpcodesTimer;
			//_timeropcode.Enabled = true;
			//_timeropcode.Start();
			Log.Notice("Opcodes", sLConsole.Network("Text15"));

			while(true)
			{
				try
				{
					if(SchumixBase.ExitStatus)
						break;

					string IrcMessage;
					if((IrcMessage = reader.ReadLine()).IsNull())
					{
						Log.Error("Opcodes", sLConsole.Network("Text16"));

						if(sChannelInfo.FSelect("reconnect"))
						{
							if(number <= 6)
							{
								Thread.Sleep(10*1000);
								number++;
							}
							else
								Thread.Sleep(120*1000);

							ReConnect();
							continue;
						}
					}

					//LastOpcode = DateTime.Now;

					if(_enabled)
					{
						number = 0;
						_enabled = false;
					}

					Task.Factory.StartNew(() => HandleIrcCommand(IrcMessage));
				}
				catch(IOException)
				{
					if(sChannelInfo.FSelect(IFunctions.Reconnect))
					{
						if(number <= 6)
						{
							Thread.Sleep(10*1000);
							number++;
						}
						else
							Thread.Sleep(120*1000);

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

			//_timeropcode.Enabled = false;
			//_timeropcode.Elapsed -= HandleOpcodesTimer;
			//_timeropcode.Stop();

			Thread.Sleep(1000);
			DisConnect();
			Log.Warning("Opcodes", sLConsole.Network("Text17"));
			Thread.Sleep(1000);
			Environment.Exit(1);
		}

		private void HandleIrcCommand(string message)
		{
			var IMessage = new IRCMessage();
			string[] IrcCommand = message.Split(SchumixBase.Space);
			IrcCommand[0] = IrcCommand[0].Remove(0, 1, SchumixBase.Colon);
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
			IMessage.Args = IrcCommand.SplitToString(3, SchumixBase.Space);
			IMessage.Args = IMessage.Args.Remove(0, 1, SchumixBase.Colon);

			switch(IRCConfig.MessageType.ToLower())
			{
				case "privmsg":
					IMessage.MessageType = MessageType.Privmsg;
					break;
				case "notice":
					IMessage.MessageType = MessageType.Notice;
					break;
				default:
					IMessage.MessageType = MessageType.Privmsg;
					break;
			}

			if(_IRCHandler2.ContainsKey(opcode))
			{
				_IRCHandler2[opcode].Invoke(IMessage);
				return;
			}
			else if(opcode.IsNumber() && _IRCHandler.ContainsKey((ReplyCode)opcode.ToNumber()))
			{
				_IRCHandler[(ReplyCode)opcode.ToNumber()].Invoke(IMessage);
				return;
			}
			else if(opcode.IsNumber() && _IRCHandler3.ContainsKey(opcode.ToNumber().ToInt()))
			{
				_IRCHandler3[opcode.ToNumber().ToInt()].Invoke(IMessage);
				return;
			}
			else
			{
				if(IrcCommand[0] == "PING")
					sSender.Pong(IrcCommand[1].Remove(0, 1, SchumixBase.Colon));
				else
				{
					if(ConsoleLog.CLog)
						Log.Notice("HandleIrcCommand", sLConsole.Network("Text18"), opcode);
				}
			}
		}

        /// <summary>
        ///     Pingeli az IRC szervert 30 másodpercenként.
        /// </summary>
		private void AutoPing()
		{
			Log.Notice("AutoPing", sLConsole.Network("Text14"));

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