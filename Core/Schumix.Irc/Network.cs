/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Megax <http://www.megaxx.info/>
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
using Schumix.API;
using Schumix.API.Irc;
using Schumix.API.Delegate;
using Schumix.API.Functions;
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
		private string _servername;
		private int _serverid;
		private bool _enabled = false;
		private bool NetworkQuit = false;
		private bool Connected = false;
		private int ReconnectNumber = 0;
		private ConnectionType CType;
		private DateTime LastOpcode;

		public Network(string ServerName, int ServerId, string Server, int Port) : base(ServerName)
		{
			_servername = ServerName;
			_serverid = ServerId;
			_server = Server;
			_port = Port;
			Shutdown = false;

			Log.Notice("Network", sLConsole.Network("Text"));
			Log.Notice("Network", sLConsole.Network("Text20"), ServerName);
			CType = ConnectionType.Normal;
		}

		public void Initialize()
		{
			InitHandler();
			InitializeCommandHandler();
			InitializeCommandMgr();
			Task.Factory.StartNew(() => sChannelInfo.ChannelList());
			sIgnoreNickName.AddConfig();
			sIgnoreChannel.AddConfig();
			sIgnoreAddon.AddConfig();
		}

		public void InitializeOpcodesAndPing()
		{
			// Start Opcodes thread
			Log.Debug("Network", sLConsole.Network("Text3"));
			var opcodes = new Thread(Opcodes);
			opcodes.Name = _servername + " Opcodes Thread";
			opcodes.Start();

			// Start Ping thread
			Log.Debug("Network", sLConsole.Network("Text4"));
			var ping = new Thread(AutoPing);
			ping.Name = _servername + " Ping Thread";
			ping.Start();

			Log.Debug("Network", sLConsole.Network("Text2"));
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
			IrcRegisterHandler(ReplyCode.ERR_NOSUCHNICK,       HandleNoWhois);
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
			IrcRegisterHandler(ReplyCode.RPL_NAMREPLY,         HandleNameList);
			IrcRegisterHandler(ReplyCode.ERR_ERRONEUSNICKNAME, HandlerErrorNewNickName);
			IrcRegisterHandler(ReplyCode.ERR_UNAVAILRESOURCE,  HandleNicknameWhileBannedOrModeratedOnChannel);
			IrcRegisterHandler(ReplyCode.ERR_INVITEONLYCHAN,   HandleCannotJoinChannel);

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

			Console.WriteLine();
			Log.Notice("Network", sLConsole.Network("Text5"));
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
			Log.Notice("Network", sLConsole.Network("Text6"), _server);
			Connection(nick);
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

			Close();
			Log.Notice("Network", sLConsole.Network("Text8"));
			Connection(false);
			Log.Debug("Network", sLConsole.Network("Text9"), _server);
		}

		public void SetConnectionType(ConnectionType ctype)
		{
			CType = ctype;
		}

		private void Connection(bool nick = false)
		{
			_cts = new CancellationTokenSource();

			if(nick)
				sNickInfo.ChangeNick(IRCConfig.List[_servername].NickName);

			try
			{
				client = new TcpClient();
				client.Connect(_server, _port);
			}
			catch(Exception)
			{
				Log.Error("Network", sLConsole.Network("Text10"));
				return;
			}

			if(client.Connected)
				Log.Success("Network", sLConsole.Network("Text11"));
			else
			{
				Log.Error("Network", sLConsole.Network("Text12"));
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
					Log.Error("Network", sLConsole.Network("Text19"), e.Message);
				}
				catch(Exception e)
				{
					Log.Error("Network", sLConsole.Exception("Error"), e.Message);
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
			sSender.NameInfo(sNickInfo.NickStorage, IRCConfig.List[_servername].UserName, IRCConfig.List[_servername].UserInfo);

			Log.Notice("Network", sLConsole.Network("Text13"));
			Online = false;
			_enabled = true;
			NewNickPrivmsg = string.Empty;
			SchumixBase.UrlTitleEnabled = false;
			sNickInfo.ChangeIdentifyStatus(false);
			sNickInfo.ChangeVhostStatus(false);
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
			if(sChannelInfo.FSelect(IFunctions.Reconnect) && !SchumixBase.ExitStatus)
			{
				if(ReconnectNumber > 5)
					_timeropcode.Interval = 300*1000;

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
			Log.Notice("Opcodes", sLConsole.Network("Text14"));
			_timeropcode.Interval = 60*1000;
			_timeropcode.Elapsed += HandleOpcodesTimer;
			_timeropcode.Enabled = true;
			_timeropcode.Start();
			Log.Notice("Opcodes", sLConsole.Network("Text15"));

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
						Log.Error("Opcodes", sLConsole.Network("Text16"));

						if(sChannelInfo.FSelect(IFunctions.Reconnect) && !SchumixBase.ExitStatus)
						{
							if(ReconnectNumber > 5)
								_timeropcode.Interval = 300*1000;
			
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
					if(sChannelInfo.FSelect(IFunctions.Reconnect))
					{
						if(ReconnectNumber > 5)
							_timeropcode.Interval = 300*1000;

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
					Log.Error("Opcodes", sLConsole.Exception("Error"), e);
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
				Log.Error("Opcodes", sLConsole.Exception("Error"), e.Message);
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
					if(!SchumixBase.ExitStatus && Connected)
						Log.Error("Ping", sLConsole.Exception("Error"), e.Message);
				}

				Thread.Sleep(30*1000);
			}
		}
	}
}