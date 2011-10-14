/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2011 Twl
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
using System.Net.Sockets;
using Schumix.Framework.Config;
using Schumix.Framework.Localization;

namespace Schumix.Framework.Client
{
	/// <summary>
	/// Delegate for handling client packets.
	/// </summary>
	public delegate void ClientPacketHandlerDelegate(SchumixPacket packet, string hst);

	/// <summary>
	/// Packet handler used by the client.
	/// </summary>
	public class ClientPacketHandler
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		/// <summary>
		/// Occurs when auth is denied.
		/// </summary>
		public event ClientPacketHandlerDelegate OnAuthDenied;
		/// <summary>
		/// Occurs when auth is approved.
		/// </summary>
		public event ClientPacketHandlerDelegate OnAuthApproved;
		/// <summary>
		/// Occurs when SCS sends a random number packet back.
		/// </summary>
		public event ClientPacketHandlerDelegate OnScsRandom;
		public event ClientPacketHandlerDelegate OnCloseConnection;
		private ClientPacketHandler() {}

		/// <summary>
		/// Init this instance.
		/// </summary>
		public void Init()
		{
			OnAuthApproved    += AuthApprovedHandler;
			OnAuthDenied      += AuthDeniedHandler;
			OnScsRandom       += ScsRandHandler;
			OnCloseConnection += CloseHandler;
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
			var packetid = packet.Read<int>();
			Log.Debug("PacketHandler", sLConsole.ClientPacketHandler("Text"), packetid, client.Client.RemoteEndPoint);

			if(packetid == (int)Opcode.SMSG_AUTH_DENIED)
				OnAuthDenied(packet, hst);
			else if(packetid == (int)Opcode.SMSG_AUTH_APPROVED)
				OnAuthApproved(packet, hst);
			else if(packetid == (int)Opcode.SMSG_SEND_SCS_RANDOM)
				OnScsRandom(packet, hst);
			else if(packetid == (int)Opcode.SMSG_CLOSE_CONNECTION)
				OnCloseConnection(packet, hst);
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
			Log.Error("SchumixServer", sLConsole.ClientPacketHandler("Text2"));
			Log.Warning("CloseHandler", sLConsole.ClientPacketHandler("Text3"));
			Log.Warning("CloseHandler", sLConsole.ClientPacketHandler("Text4"));
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
			Log.Success("SchumixServer", sLConsole.ClientPacketHandler("Text5"));
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
			//var rand = pck.Read<int>();
			// read channel
			//var chan = pck.Read<string>();
			//var sBot = Singleton<AlarisBot>.Instance;
			//if(string.IsNullOrEmpty(chan) || chan == "0")
			//	chan = sBot.acs_rand_request_channel;

			//sBot.SendMsg(chan, "SCS sent random: " + rand.ToString());
		}

		private void CloseHandler(SchumixPacket pck, string hst)
		{
			Log.Warning("CloseHandler", sLConsole.ClientPacketHandler("Text3"));
			Log.Warning("CloseHandler", sLConsole.ClientPacketHandler("Text4"));
			SchumixBase.timer.SaveUptime();
			Thread.Sleep(1000);
			Environment.Exit(1);
		}
	}
}