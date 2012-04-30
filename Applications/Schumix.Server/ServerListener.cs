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
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Schumix.Framework;
using Schumix.Framework.Client;
using Schumix.Framework.Localization;

namespace Schumix.Server
{
	sealed class ServerListener : IDisposable
	{
		private readonly ServerPacketHandler sServerPacketHandler = Singleton<ServerPacketHandler>.Instance;
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly TcpListener _listener;
		public bool Exit = false;

		/// <summary>
		/// Initializes a new instance of the <see cref="Schumix.Server.ServerListener"/> class.
		/// </summary>
		/// <param name='port'>
		/// Port to listen on.
		/// </param>
		public ServerListener(int port)
		{
			_listener = new TcpListener(IPAddress.Any, port);
			sServerPacketHandler.Init();
		}
		
		public void Listen()
		{
			_listener.Start();
			Log.Notice("Listener", sLConsole.ServerListener("Text"));
			
			while(true)
			{
				var client = _listener.AcceptTcpClient();
				Log.Notice("Listener", sLConsole.ServerListener("Text2"), client.Client.RemoteEndPoint);
				var client_thread = new Thread(new ParameterizedThreadStart(ClientHandler));
				client_thread.Start(client);
				Thread.Sleep(100);
			}
		}
		
		public void ClientHandler(object ob)
		{
			var client = (ob as TcpClient);
			var stream = client.GetStream();
			byte[] message_buffer = new byte[262144];
			int bytes_read;
			Log.Notice("ClientHandler", sLConsole.ServerListener("Text3"));
			
			while(true)
			{
				bytes_read = 0;
				
				// read
				if(stream.DataAvailable && stream.CanRead)
				{
					Log.Debug("ClientHandler", sLConsole.ServerListener("Text4"));
					bytes_read = stream.Read(message_buffer, 0, message_buffer.Length);
					
					if(bytes_read == 0)
					{
						Log.Warning("ClientHandler", sLConsole.ServerListener("Text5"));
						break;
					}

					if(Exit)
						return;

					var encoding = new UTF8Encoding();
					var msg = encoding.GetString(message_buffer, 0, bytes_read);
					var packet = new SchumixPacket(msg);
					sServerPacketHandler.HandlePacket(packet, client, stream);
				}
				
				Thread.Sleep(100);
			}

			Log.Warning("ClientHandler", sLConsole.ServerListener("Text6"));
			Environment.Exit(1);
		}
		
		public void Dispose()
		{

		}
	}
}