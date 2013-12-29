/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Twl
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013-2014 Schumix Team <http://schumix.eu/>
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
using System.Net.Sockets;
using System.Collections.Generic;
using Schumix.Framework.Irc;
using Schumix.Framework.Logger;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Framework.Network
{
	/// <summary>
	/// Packet handler used by the client.
	/// </summary>
	sealed class ClientPacketHandler
	{
		private readonly Dictionary<Opcode, ClientPacketMethod> PacketMethodMap = new Dictionary<Opcode, ClientPacketMethod>();
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly Runtime sRuntime = Singleton<Runtime>.Instance;

		public Dictionary<Opcode, ClientPacketMethod> GetPacketMethodMap()
		{
			return PacketMethodMap;
		}

		private ClientPacketHandler() {}

		/// <summary>
		/// Init this instance.
		/// </summary>
		public void Init()
		{
			RegisterHandler(Opcode.SMSG_AUTH_APPROVED,    AuthApprovedHandler);
			RegisterHandler(Opcode.SMSG_AUTH_DENIED,      AuthDeniedHandler);
			RegisterHandler(Opcode.SMSG_SEND_SCS_RANDOM,  ScsRandHandler);
			RegisterHandler(Opcode.SMSG_CLOSE_CONNECTION, CloseHandler);
		}

		public void RegisterHandler(Opcode packetid, ClientPacketHandlerDelegate method)
		{
			if(PacketMethodMap.ContainsKey(packetid))
				PacketMethodMap[packetid].Method += method;
			else
				PacketMethodMap.Add(packetid, new ClientPacketMethod(method));
		}

		public void RemoveHandler(Opcode packetid)
		{
			if(PacketMethodMap.ContainsKey(packetid))
				PacketMethodMap.Remove(packetid);
		}

		public void RemoveHandler(Opcode packetid, ClientPacketHandlerDelegate method)
		{
			if(PacketMethodMap.ContainsKey(packetid))
			{
				PacketMethodMap[packetid].Method -= method;

				if(PacketMethodMap[packetid].Method.IsNull())
					PacketMethodMap.Remove(packetid);
			}
		}

		/// <summary>
		/// Handles the packet.
		/// </summary>
		/// <param name='packet'>
		/// Packet.
		/// </param>
		/// <param name='client'>
		/// Client.
		/// </param>
		public void HandlePacket(SchumixPacket packet, TcpClient client)
		{
			var hst = client.Client.RemoteEndPoint.ToString().Split(SchumixBase.Colon)[0];
			int packetid = 0;

			try
			{
				packetid = packet.Read<int>();
			}
			catch(Exception)
			{
				var packet2 = new SchumixPacket();
				packet2.Write<int>((int)Opcode.SCMSG_PACKET_NULL);
				packet2.Write<string>(sLConsole.GetString("Wrong packetid, aborting connection!"));
				ClientSocket.SendPacketToSCS(packet);
				Log.Warning("ClientPacketHandler", sLConsole.GetString("Wrong packetid, aborting connection!"));
				return;
			}

			Log.Debug("ClientPacketHandler", sLConsole.GetString("Got packet with ID: {0} from: {1}"), packetid, client.Client.RemoteEndPoint);

			if(PacketMethodMap.ContainsKey((Opcode)packetid))
			{
				PacketMethodMap[(Opcode)packetid].Method.Invoke(packet, hst);
				return;
			}
			else
				Log.Notice("ClientPacketHandler", sLConsole.GetString("Received unhandled packetid: {0}"), packetid);
		}

		/// <summary>
		/// The auth denied packet handler. (SMSG_AUTH_DENIED)
		/// </summary>
		/// <param name='pck'>
		/// Packet.
		/// </param>
		/// <param name='hst'>
		/// Host.
		/// </param>
		public void AuthDeniedHandler(SchumixPacket pck, string hst)
		{
			Log.Error("SchumixServer", sLConsole.GetString("Authentication denied to SCS server!"));
			Log.Warning("CloseHandler", sLConsole.GetString("Connection closed!"));
			Log.Warning("CloseHandler", sLConsole.GetString("Program shutting down!"));
			Thread.Sleep(1000);
			Environment.Exit(1);
		}

		/// <summary>
		/// The auth approved packet handler. (SMSG_AUTH_APPROVED)
		/// </summary>
		/// <param name='pck'>
		/// Packet.
		/// </param>
		/// <param name='hst'>
		/// Host.
		/// </param>
		public void AuthApprovedHandler(SchumixPacket pck, string hst)
		{
			Log.Success("SchumixServer", sLConsole.GetString("Successfully authed to SCS server."));
			SchumixBase.ThreadStop = false;
		}

		/// <summary>
		/// The SCS Random number handler. (SMSG_SEND_SCS_RANDOM)
		/// </summary>
		/// <param name='pck'>
		/// Packet.
		/// </param>
		/// <param name='hst'>
		/// Host.
		/// </param>
		public void ScsRandHandler(SchumixPacket pck, string hst)
		{
			// read random value.
			var rand = pck.Read<int>();
			Log.Notice("Random", sLConsole.GetString("SCS sent random: {0}"), rand);
		}

		private void CloseHandler(SchumixPacket pck, string hst)
		{
			if(SchumixBase.ExitStatus)
				return;

			Log.Warning("CloseHandler", sLConsole.GetString("Connection closed!"));
			Log.Warning("CloseHandler", sLConsole.GetString("Program shutting down!"));
			SchumixBase.Quit();

			foreach(var nw in INetwork.WriterList)
			{
				if(!nw.Value.IsNull())
					nw.Value.WriteLine("QUIT :Server killed.");
			}

			Thread.Sleep(1000);
			sRuntime.Exit();
		}
	}
}