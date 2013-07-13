/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Twl
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
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Schumix.Framework;
using Schumix.Framework.Logger;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Framework.Listener
{
	public sealed class SchumixListener : IDisposable
	{
		private readonly SchumixPacketHandler sSchumixPacketHandler = Singleton<SchumixPacketHandler>.Instance;
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly TcpListener _listener;

		/// <summary>
		/// Initializes a new instance of the <see cref="FBI.Server.ServerListener"/> class.
		/// </summary>
		/// <param name='port'>
		/// Port to listen on.
		/// </param>
		public SchumixListener(int port)
		{
			_listener = new TcpListener(IPAddress.Any, port);
			sSchumixPacketHandler.Init();
		}
		
		public void Listen()
		{
			_listener.Start();
			Log.Notice("SchumixListener", sLConsole.GetString("Successfully started the SchumixListener."));
			
			while(true)
			{
				var client = _listener.AcceptTcpClient();
				Log.Notice("SchumixListener", sLConsole.GetString("Client connection from: {0}"), client.Client.RemoteEndPoint);
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
			Log.Notice("SchumixListener", sLConsole.GetString("Handling client..."));
			
			while(true)
			{
				bytes_read = 0;

				// read
				if(stream.DataAvailable && stream.CanRead)
				{
					Log.Debug("SchumixListener", sLConsole.GetString("Stream data available, reading."));
					bytes_read = stream.Read(message_buffer, 0, message_buffer.Length);

					if(SchumixBase.ExitStatus)
						return;
					
					if(bytes_read == 0)
					{
						Log.Warning("SchumixListener", sLConsole.GetString("Lost connection!"));
						break;
					}

					var encoding = new UTF8Encoding();
					var msg = encoding.GetString(message_buffer, 0, bytes_read);
					var packet = new ListenerPacket(msg);
					sSchumixPacketHandler.HandlePacket(packet, client, stream);
				}
				
				Thread.Sleep(100);
			}

			Log.Warning("SchumixListener", sLConsole.GetString("The data's processing is completed."));
		}
		
		public void Dispose()
		{
			if(!_listener.IsNull())
				_listener.Stop();
		}
	}
}