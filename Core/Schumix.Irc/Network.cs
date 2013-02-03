/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013 Schumix Team <http://schumix.eu/>
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
using System.Linq;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Net.Security;
using System.Reflection;
using System.Collections.Generic;
using System.Security.Authentication;
using Schumix.Api.Irc;
using Schumix.Api.Delegate;
using Schumix.Api.Functions;
using Schumix.Framework;
using Schumix.Framework.Addon;
using Schumix.Framework.Config;
using Schumix.Framework.Database;
using Schumix.Framework.Extensions;

namespace Schumix.Irc
{
	public sealed class Network : MessageHandler
	{
		private readonly Dictionary<string, IrcMethod> IrcMethodMap = new Dictionary<string, IrcMethod>();
		private System.Timers.Timer _timeropcode = new System.Timers.Timer();
		private CancellationTokenSource _cts = new CancellationTokenSource();
		private readonly object IrcMapLock = new object();

		public Dictionary<string, IrcMethod> GetIrcMethodMap()
		{
			return IrcMethodMap;
		}

		public bool Shutdown { get; private set; }

		/// <summary>
		///     A bejövő információkat fogadja.
		/// </summary>
		private StreamReader reader;

		/// <summary>
		///     A kapcsolatot tároljra.
		/// </summary>
		private TcpClient client;

		/// <summary>
		///     IRC szerver címe.
		/// </summary>
		private readonly string _server;

		/// <summary>
		///     IRC port száma.
		/// </summary>
		private readonly int _port;

		private bool NetworkQuit = false;
		private int ReconnectNumber = 0;
		private bool Connected = false;
		private bool _enabled = false;
		private ConnectionType CType;
		private DateTime LastOpcode;
		private string _servername;
		private int _serverid;

		public Network() : base("default")
		{
			_servername = "default";
			_serverid = 1;
			_server = "localhost";
			_port = 6667;
			Shutdown = false;

			Log.Notice("Network", sLConsole.GetString("Successfully started the Network."));
			Log.Notice("Network", sLConsole.GetString("Server's name: {0}"), _servername);
			CType = ConnectionType.Normal;
		}

		public Network(string Server) : base("default")
		{
			_servername = "default";
			_serverid = 1;
			_server = Server;
			_port = 6667;
			Shutdown = false;

			Log.Notice("Network", sLConsole.GetString("Successfully started the Network."));
			Log.Notice("Network", sLConsole.GetString("Server's name: {0}"), _servername);
			CType = ConnectionType.Normal;
		}

		public Network(string Server, int Port) : base("default")
		{
			_servername = "default";
			_serverid = 1;
			_server = Server;
			_port = Port;
			Shutdown = false;

			Log.Notice("Network", sLConsole.GetString("Successfully started the Network."));
			Log.Notice("Network", sLConsole.GetString("Server's name: {0}"), _servername);
			CType = ConnectionType.Normal;
		}

		public Network(string ServerName, int ServerId, string Server, int Port) : base(ServerName)
		{
			_servername = ServerName;
			_serverid = ServerId;
			_server = Server;
			_port = Port;
			Shutdown = false;

			Log.Notice("Network", sLConsole.GetString("Successfully started the Network."));
			Log.Notice("Network", sLConsole.GetString("Server's name: {0}"), ServerName);
			CType = ConnectionType.Normal;
		}

		public void Initialize()
		{
			InitHandler();
			InitializeCommandHandler();
			InitializeCommandMgr();
			Task.Factory.StartNew(() => sMyChannelInfo.ChannelList());
			sIgnoreNickName.LoadConfig();
			sIgnoreNickName.LoadSql();
			sIgnoreChannel.LoadConfig();
			sIgnoreChannel.LoadSql();
			sIgnoreAddon.LoadConfig();
			sIgnoreAddon.LoadSql();
			sIgnoreCommand.LoadSql();
			sIgnoreIrcCommand.LoadSql();
		}

		public void InitializeOpcodesAndPing()
		{
			// Start Opcodes thread
			Log.Debug("Network", sLConsole.GetString("Opcodes thread started..."));
			var opcodes = new Thread(Opcodes);
			opcodes.Name = _servername + " Opcodes Thread";
			opcodes.Start();

			// Start Ping thread
			Log.Debug("Network", sLConsole.GetString("Ping thread started..."));
			var ping = new Thread(AutoPing);
			ping.Name = _servername + " Ping Thread";
			ping.Start();

			Log.Debug("Network", sLConsole.GetString("Establishing connection with irc server."));
		}

		private void InitHandler()
		{
			IrcRegisterHandler(ReplyCode.RPL_WELCOME,          HandleSuccessfulAuth);
			IrcRegisterHandler("PING",                         HandlePing);
			IrcRegisterHandler("PONG",                         HandlePong);
			IrcRegisterHandler("PRIVMSG",                      HandlePrivmsg);
			IrcRegisterHandler("NOTICE",                       HandleNotice);
			IrcRegisterHandler(ReplyCode.ERR_BANNEDFROMCHAN,   HandleChannelBan);
			IrcRegisterHandler(ReplyCode.ERR_BADCHANNELKEY,    HandleNoChannelPassword);
			IrcRegisterHandler(ReplyCode.RPL_WHOISCHANNELS,    HandleMWhois);
			IrcRegisterHandler(ReplyCode.RPL_WHOISSERVER,      HandleWhoisServer);
			IrcRegisterHandler(ReplyCode.RPL_ENDOFWHOIS,       HandleEndOfWhois);
			IrcRegisterHandler(ReplyCode.ERR_UNKNOWNCOMMAND,   HandleUnknownCommand);
			IrcRegisterHandler(ReplyCode.ERR_NICKNAMEINUSE,    HandleNickError);
			IrcRegisterHandler(439,                            HandleWaitingForConnection);
			IrcRegisterHandler(ReplyCode.ERR_NOTREGISTERED,    HandleNotRegistered);
			IrcRegisterHandler(ReplyCode.ERR_NONICKNAMEGIVEN,  HandleNoNickName);
			IrcRegisterHandler("JOIN",                         HandleIrcJoin);
			IrcRegisterHandler("PART",                         HandleIrcLeft);
			IrcRegisterHandler("KICK",                         HandleIrcKick);
			IrcRegisterHandler("QUIT",                         HandleIrcQuit);
			IrcRegisterHandler("NICK",                         HandleNewNick);
			IrcRegisterHandler("MODE",                         HandleIrcMode);
			IrcRegisterHandler("TOPIC",                        HandleIrcTopic);
			IrcRegisterHandler("INVITE",                       HandleIrcInvite);
			IrcRegisterHandler(ReplyCode.RPL_NAMREPLY,         HandleNameList);
			IrcRegisterHandler(ReplyCode.RPL_ENDOFNAMES,       HandleEndNameList);
			IrcRegisterHandler(ReplyCode.ERR_ERRONEUSNICKNAME, HandleErrorNewNickName);
			IrcRegisterHandler(ReplyCode.ERR_UNAVAILRESOURCE,  HandleNicknameWhileBannedOrModeratedOnChannel);
			IrcRegisterHandler(ReplyCode.ERR_INVITEONLYCHAN,   HandleCannotJoinChannel);
			IrcRegisterHandler(ReplyCode.RPL_TOPIC,            HandleInitialTopic);
			IrcRegisterHandler(ReplyCode.ERR_NEEDMOREPARAMS,   HandleNeedMoreParams);
			IrcRegisterHandler(ReplyCode.ERR_KEYSET,           HandleKeySet);
			IrcRegisterHandler(ReplyCode.ERR_CHANOPRIVSNEEDED, HandleChanopPrivsNeeded);
			IrcRegisterHandler(460,                            HandleChanopPrivsNeeded);
			IrcRegisterHandler(ReplyCode.ERR_USERNOTINCHANNEL, HandleUserNotinChannel);
			IrcRegisterHandler(ReplyCode.ERR_UNKNOWNMODE,      HandleUnknownMode);
			IrcRegisterHandler(ReplyCode.ERR_NOSUCHNICK,       HandleNoSuchNick);
			IrcRegisterHandler(499,                            HandleNotAChannelOwner);
			IrcRegisterHandler(974,                            HandleNotAChannelAdmin);

			Task.Factory.StartNew(() =>
			{
				var asms = sAddonManager.Addons[_servername].Assemblies.ToDictionary(v => v.Key, v => v.Value);
				Parallel.ForEach(asms, asm =>
				{
					var types = asm.Value.GetTypes();
					Parallel.ForEach(types, type =>
					{
						var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
						IrcProcessMethods(methods);
					});
				});
			});

			Console.WriteLine();
			Log.Notice("Network", sLConsole.GetString("All of IRC handlers are registered."));
		}

		private void IrcProcessMethods(IEnumerable<MethodInfo> methods)
		{
			Parallel.ForEach(methods, method =>
			{
				foreach(var attribute in Attribute.GetCustomAttributes(method))
				{
					if(attribute.IsOfType(typeof(IrcCommandAttribute)))
					{
						var attr = (IrcCommandAttribute)attribute;
						lock(IrcMapLock)
						{
							var del = Delegate.CreateDelegate(typeof(IRCDelegate), method) as IRCDelegate;
							IrcRegisterHandler(attr.Command, del);
						}
					}
				}
			});
		}

		public void IrcRegisterHandler(string code, IRCDelegate method)
		{
			if(IrcMethodMap.ContainsKey(code))
				IrcMethodMap[code].Method += method;
			else
				IrcMethodMap.Add(code, new IrcMethod(method));
		}

		public void IrcRemoveHandler(string code)
		{
			if(IrcMethodMap.ContainsKey(code))
				IrcMethodMap.Remove(code);
		}

		public void IrcRemoveHandler(string code, IRCDelegate method)
		{
			if(IrcMethodMap.ContainsKey(code))
			{
				IrcMethodMap[code].Method -= method;

				if(IrcMethodMap[code].Method.IsNull())
					IrcMethodMap.Remove(code);
			}
		}

		public void IrcRegisterHandler(ReplyCode code, IRCDelegate method)
		{
			string scode = Convert.ToInt32(code).ToIrcOpcode();

			if(IrcMethodMap.ContainsKey(scode))
				IrcMethodMap[scode].Method += method;
			else
				IrcMethodMap.Add(scode, new IrcMethod(method));
		}

		public void IrcRemoveHandler(ReplyCode code)
		{
			string scode = Convert.ToInt32(code).ToIrcOpcode();

			if(IrcMethodMap.ContainsKey(scode))
				IrcMethodMap.Remove(scode);
		}

		public void IrcRemoveHandler(ReplyCode code, IRCDelegate method)
		{
			string scode = Convert.ToInt32(code).ToIrcOpcode();

			if(IrcMethodMap.ContainsKey(scode))
			{
				IrcMethodMap[scode].Method -= method;

				if(IrcMethodMap[scode].Method.IsNull())
					IrcMethodMap.Remove(scode);
			}
		}

		public void IrcRegisterHandler(int code, IRCDelegate method)
		{
			string scode = code.ToIrcOpcode();

			if(IrcMethodMap.ContainsKey(scode))
				IrcMethodMap[scode].Method += method;
			else
				IrcMethodMap.Add(scode, new IrcMethod(method));
		}

		public void IrcRemoveHandler(int code)
		{
			string scode = code.ToIrcOpcode();

			if(IrcMethodMap.ContainsKey(scode))
				IrcMethodMap.Remove(scode);
		}

		public void IrcRemoveHandler(int code, IRCDelegate method)
		{
			string scode = code.ToIrcOpcode();

			if(IrcMethodMap.ContainsKey(scode))
			{
				IrcMethodMap[scode].Method -= method;

				if(IrcMethodMap[scode].Method.IsNull())
					IrcMethodMap.Remove(scode);
			}
		}

		/// <summary>
        ///     Kapcsolódás az IRC kiszolgálóhoz.
        /// </summary>
		public void Connect(bool nick = false)
		{
			NetworkQuit = false;
			Log.Notice("Network", sLConsole.GetString("Connection to: {0}"), _server);
			Connection(nick);
		}

		/// <summary>
		///     Lekapcsolódás az IRC koszolgálótól.
		/// </summary>
		public void DisConnect()
		{
			Close();
			Log.Notice("Network", sLConsole.GetString("Connection closed!"));
		}

		/// <summary>
		///     Visszakapcsolódás az IRC kiszolgálóhoz.
		/// </summary>
		public void ReConnect()
		{
			if(SchumixBase.ExitStatus)
				return;

			Close();
			Log.Notice("Network", sLConsole.GetString("Connection have been closed."));
			Connection(false);
			Log.Debug("Network", sLConsole.GetString("Reconnection to: {0}"), _server);
		}

		public void SetConnectionType(ConnectionType ctype)
		{
			CType = ctype;
		}

		private void Connection(bool nick = false)
		{
			_cts = new CancellationTokenSource();

			if(nick)
				sMyNickInfo.ChangeNick(IRCConfig.List[_servername].NickName);

			Log.Notice("Network", sLConsole.GetString("Connection type: {0}"), CType.ToString());

			try
			{
				client = new TcpClient();
				client.Connect(_server, _port);
			}
			catch(Exception)
			{
				Log.Error("Network", sLConsole.GetString("Fatal error was happened while established the connection!"));
				return;
			}

			if(client.Connected)
				Log.Success("Network", sLConsole.GetString("Successfully established the connection!"));
			else
			{
				Log.Error("Network", sLConsole.GetString("Error was happened while established the connection!"));
				return;
			}

			if(CType == ConnectionType.Ssl)
			{
				var networkStream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback((s,ce,ca,p) => true), null);

				try
				{
					networkStream.AuthenticateAsClient(_server);
				}
				catch(AuthenticationException e)
				{
					Log.Error("Network", sLConsole.GetString("Certificate not accepted, exception: {0}"), e.Message);
				}
				catch(Exception e)
				{
					Log.Error("Network", sLConsole.GetString("Failure details: {0}"), e.Message);
				}

				reader = new StreamReader(networkStream);

				if(INetwork.WriterList.ContainsKey(_servername))
					INetwork.WriterList[_servername] = new StreamWriter(networkStream) { AutoFlush = true };
				else
					INetwork.WriterList.Add(_servername, new StreamWriter(networkStream) { AutoFlush = true });
			}
			else
			{
				reader = new StreamReader(client.GetStream());

				if(INetwork.WriterList.ContainsKey(_servername))
					INetwork.WriterList[_servername] = new StreamWriter(client.GetStream()) { AutoFlush = true };
				else
					INetwork.WriterList.Add(_servername, new StreamWriter(client.GetStream()) { AutoFlush = true });
			}

			Connected = true;
			sSender.RegisterConnection(IRCConfig.List[_servername].Password, sMyNickInfo.NickStorage, IRCConfig.List[_servername].UserName, IRCConfig.List[_servername].UserInfo);

			Log.Notice("Network", sLConsole.GetString("Users' datas are sent."));
			Online = false;
			IsAllJoin = false;
			_enabled = true;
			NewNickPrivmsg = string.Empty;
			SchumixBase.UrlTitleEnabled = false;
			sMyNickInfo.ChangeIdentifyStatus(false);
			sMyNickInfo.ChangeVhostStatus(false);
		}

		private void Close()
		{
			Connected = false;
			_cts.Cancel();

			if(!SchumixBase.ExitStatus)
				Thread.Sleep(2000);

			if(!client.IsNull())
				client.Close();

			if(!INetwork.WriterList[_servername].IsNull())
				INetwork.WriterList[_servername].Dispose();

			if(!reader.IsNull())
				reader.Dispose();
		}

		private void HandleOpcodesTimer(object sender, ElapsedEventArgs e)
		{
			if(sMyChannelInfo.FSelect(IFunctions.Reconnect) && !SchumixBase.ExitStatus)
			{
				if(ReconnectNumber > 5)
					_timeropcode.Interval = 5*60*1000;

				if((DateTime.Now - LastOpcode).Minutes >= 1)
				{
					ReconnectNumber++;
					ReConnect();
				}
			}
		}

		/// <summary>
		///     Ez a függvény kezeli azt IRC adatai és az opcedes-eket.
		/// </summary>
		/// <remarks>
		///     Opcodes: Az IRC-ről jövő funkciók, kódok.
		/// </remarks>
		private void Opcodes()
		{
			Log.Notice("Opcodes", sLConsole.GetString("Successfully started th thread."));
			_timeropcode.Interval = 60*1000;
			_timeropcode.Elapsed += HandleOpcodesTimer;
			_timeropcode.Enabled = true;
			_timeropcode.Start();
			Log.Notice("Opcodes", sLConsole.GetString("Started the irc data receiving."));

			while(true)
			{
				try
				{
					if(SchumixBase.ExitStatus && NetworkQuit)
						break;

					if(!Connected)
					{
						Thread.Sleep(1000);
						continue;
					}

					string IrcMessage;
					if((IrcMessage = reader.ReadLine()).IsNull())
					{
						Log.Error("Opcodes", sLConsole.GetString("Do not going data from irc server!"));

						if(sMyChannelInfo.FSelect(IFunctions.Reconnect) && !SchumixBase.ExitStatus)
						{
							if(ReconnectNumber > 5)
								_timeropcode.Interval = 5*60*1000;
			
							if(Connected)
							{
								ReconnectNumber++;
								ReConnect();
							}

							continue;
						}
					}

					LastOpcode = DateTime.Now;

					if(_enabled)
					{
						_timeropcode.Interval = 60*1000;
						ReconnectNumber = 0;
						_enabled = false;
					}

					Task.Factory.StartNew(() => HandleIrcCommand(IrcMessage), _cts.Token);
					Thread.Sleep(100);
				}
				catch(IOException)
				{
					if(sMyChannelInfo.FSelect(IFunctions.Reconnect))
					{
						if(ReconnectNumber > 5)
							_timeropcode.Interval = 5*60*1000;

						if(Connected)
						{
							ReconnectNumber++;
							ReConnect();
						}

						continue;
					}
				}
				catch(Exception e)
				{
					Log.Error("Opcodes", sLConsole.GetString("Failure details: {0}"), e.Message);
					Thread.Sleep(1000);
				}
			}

			_timeropcode.Enabled = false;
			_timeropcode.Elapsed -= HandleOpcodesTimer;
			_timeropcode.Stop();

			try
			{
				sIgnoreNickName.RemoveConfig();
				sIgnoreChannel.RemoveConfig();
				sIgnoreAddon.RemoveConfig();
				DisConnect();
			}
			catch(Exception e)
			{
				Log.Error("Opcodes", sLConsole.GetString("Failure details: {0}"), e.Message);
			}

			Shutdown = true;
		}

		private void HandleIrcCommand(string message)
		{
			var IMessage = new IRCMessage();
			IMessage.ServerId = _serverid;
			IMessage.ServerName = _servername;
			string[] IrcCommand = message.Split(SchumixBase.Space);
			IrcCommand[0] = IrcCommand[0].Remove(0, 1, SchumixBase.Colon);
			IMessage.Hostmask = IrcCommand[0];

			if(IrcCommand.Length > 2)
				IMessage.Channel = sUtilities.SqlEscape(IrcCommand[2]);

			string[] userdata = IMessage.Hostmask.Split('!');
			IMessage.Nick = sUtilities.SqlEscape(userdata[0]);

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

			switch(IRCConfig.List[_servername].MessageType.ToLower())
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

			if(IrcMethodMap.ContainsKey(opcode))
			{
				IrcMethodMap[opcode].Method.Invoke(IMessage);
				return;
			}
			else
			{
				if(IrcCommand[0] == "PING")
					sSender.Pong(IrcCommand[1].Remove(0, 1, SchumixBase.Colon));
				else if(opcode == ":Closing")
					NetworkQuit = true;
				else
				{
					if(ConsoleLog.CLog)
						Log.Notice("HandleIrcCommand", sLConsole.GetString("Received unhandled opcode: {0}"), opcode);
				}
			}
		}

		/// <summary>
		///     Pingeli az IRC szervert 30 másodpercenként.
		/// </summary>
		private void AutoPing()
		{
			Log.Notice("AutoPing", sLConsole.GetString("Successfully started th thread."));

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
					if(!SchumixBase.ExitStatus && Connected)
						Log.Error("Ping", sLConsole.GetString("Failure details: {0}"), e.Message);
				}

				Thread.Sleep(30*1000);
			}
		}
	}
}