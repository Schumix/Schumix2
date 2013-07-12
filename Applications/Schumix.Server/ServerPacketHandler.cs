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
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Schumix.Framework;
using Schumix.Framework.Logger;
using Schumix.Framework.Network;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using Schumix.Server.Config;

namespace Schumix.Server
{
	sealed class ServerPacketHandler
	{
		private readonly Dictionary<Opcode, ServerPacketMethod> PacketMethodMap = new Dictionary<Opcode, ServerPacketMethod>();
		private readonly Dictionary<string, NetworkStream> _HostList = new Dictionary<string, NetworkStream>();
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly Dictionary<string, bool> _AuthList = new Dictionary<string, bool>();
		private readonly New.Schumix sSchumix = Singleton<New.Schumix>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		public Dictionary<string, NetworkStream> HostList { get { return _HostList; } }

		public Dictionary<Opcode, ServerPacketMethod> GetPacketMethodMap()
		{
			return PacketMethodMap;
		}

		private ServerPacketHandler() {}

		public void Init()
		{
			RegisterHandler(Opcode.CMSG_REQUEST_AUTH,       AuthRequestPacketHandler);
			RegisterHandler(Opcode.CMSG_REQUEST_SCS_RANDOM, ScsRandomHandler);
			RegisterHandler(Opcode.CMSG_CLOSE_CONNECTION,   CloseHandler);
		}

		public void RegisterHandler(Opcode packetid, ServerPacketHandlerDelegate method)
		{
			if(PacketMethodMap.ContainsKey(packetid))
				PacketMethodMap[packetid].Method += method;
			else
				PacketMethodMap.Add(packetid, new ServerPacketMethod(method));
		}

		public void RemoveHandler(Opcode packetid)
		{
			if(PacketMethodMap.ContainsKey(packetid))
				PacketMethodMap.Remove(packetid);
		}

		public void RemoveHandler(Opcode packetid, ServerPacketHandlerDelegate method)
		{
			if(PacketMethodMap.ContainsKey(packetid))
			{
				PacketMethodMap[packetid].Method -= method;

				if(PacketMethodMap[packetid].Method.IsNull())
					PacketMethodMap.Remove(packetid);
			}
		}

		public void HandlePacket(SchumixPacket packet, TcpClient client, NetworkStream stream)
		{
			var hst = client.Client.RemoteEndPoint.ToString().Split(SchumixBase.Colon)[0];
			int bck = Convert.ToInt32(client.Client.RemoteEndPoint.ToString().Split(SchumixBase.Colon)[1]);

			int packetid = 0;

			try
			{
				packetid = packet.Read<int>();
			}
			catch(Exception)
			{
				Log.Warning("HandlePacket", sLConsole.GetString("Wrong packetid, aborting connection!"));
				return;
			}

			Log.Debug("HandlePacket", sLConsole.GetString("Got packet with ID: {0} from: {1}"), packetid, client.Client.RemoteEndPoint);

			if(!_AuthList.ContainsKey(hst + SchumixBase.Colon + bck))
			{
				if(packetid != (int)Opcode.CMSG_REQUEST_AUTH)
				{
					var packet2 = new SchumixPacket();
					packet2.Write<int>((int)Opcode.SMSG_AUTH_DENIED);
					packet2.Write<int>((int)0);
					SendPacketBack(packet2, stream, hst, bck);
					return;
				}
				else
					_AuthList.Add(hst + SchumixBase.Colon + bck, true);
			}

			if(!_HostList.ContainsKey(hst + SchumixBase.Colon + bck))
				_HostList.Add(hst + SchumixBase.Colon + bck, stream);

			if(PacketMethodMap.ContainsKey((Opcode)packetid))
			{
				PacketMethodMap[(Opcode)packetid].Method.Invoke(packet, stream, hst, bck);
				return;
			}
			else
				Log.Notice("HandlePacket", sLConsole.GetString("Received unhandled packetid: {0}"), packetid);
		}

		private void ScsRandomHandler(SchumixPacket pck, NetworkStream stream, string hst, int bck)
		{
			var rand = new Random();
			int random = rand.Next();
			Log.Notice("Random", sLConsole.GetString("Sending random value: {0}"), random);

			var packet = new SchumixPacket();
			packet.Write<int>((int)Opcode.SMSG_SEND_SCS_RANDOM);
			packet.Write<int>(random);
			SendPacketBack(packet, stream, hst, bck);
		}

		private void AuthRequestPacketHandler(SchumixPacket pck, NetworkStream stream, string hst, int bck)
		{
			// opcode is already read, DO _NOT_ READ AGAIN
			string guid = pck.Read<string>();
			string hash = pck.Read<string>();

			if(hash != sUtilities.Md5(ServerConfigs.Password))
			{
				if(_HostList.ContainsKey(hst + SchumixBase.Colon + bck))
					_HostList.Remove(hst + SchumixBase.Colon + bck);

				Log.Warning("AuthHandler", sLConsole.GetString("Auth unsuccessful! Guid of client: {0}"), guid);
				Log.Debug("Security", sLConsole.GetString("Hash was: {0}"), hash);
				Log.Notice("AuthHandler", sLConsole.GetString("Back port is: {0}"), bck);
				var packet = new SchumixPacket();
				packet.Write<int>((int)Opcode.SMSG_AUTH_DENIED);
				packet.Write<int>((int)0);
				SendPacketBack(packet, stream, hst, bck);
			}
			else
			{
				Log.Success("AuthHandler", sLConsole.GetString("Auth successful. Guid of client: {0}"), guid);
				Log.Debug("Security", sLConsole.GetString("Hash was: {0}"), hash);
				Log.Notice("AuthHandler", sLConsole.GetString("Back port is: {0}"), bck);
				var packet = new SchumixPacket();
				packet.Write<int>((int)Opcode.SMSG_AUTH_APPROVED);
				packet.Write<int>((int)1);
				SendPacketBack(packet, stream, hst, bck);
			}
		}

		private void CloseHandler(SchumixPacket pck, NetworkStream stream, string hst, int bck)
		{
			if(_HostList.ContainsKey(hst + SchumixBase.Colon + bck))
				_HostList.Remove(hst + SchumixBase.Colon + bck);

			if(_AuthList.ContainsKey(hst + SchumixBase.Colon + bck))
				_AuthList.Remove(hst + SchumixBase.Colon + bck);

			string guid = pck.Read<string>();
			string file = pck.Read<string>();
			string dir = pck.Read<string>();
			string ce = pck.Read<string>();
			string locale = pck.Read<string>();
			string reconnect = pck.Read<string>();
			string identify = pck.Read<string>();
			Log.Warning("CloseHandler", sLConsole.GetString("Connection closed! Guid of client: {0}"), guid);

			if(hst != "127.0.0.1")
				return;

			if(!Convert.ToBoolean(reconnect))
				return;

			Log.Notice("CloseHandler", sLConsole.GetString("Restart in progress..."));

			if(sSchumix._processlist.ContainsKey(identify))
				sSchumix.Start(file, dir, ce, locale, sUtilities.GetRandomString());

			sSchumix._processlist.Remove(identify);
		}

		private void NickNameHandler(SchumixPacket pck, NetworkStream stream, string hst, int bck)
		{

		}

		public void SendPacketBack(SchumixPacket packet, NetworkStream stream, string hst, int backport)
		{
			Log.Debug("PacketHandler", "SendPacketBack(): host is: " + hst + ", port is: " + backport);

			if(stream.CanWrite)
			{
				var buff = new UTF8Encoding().GetBytes(packet.GetNetMessage());
				stream.Write(buff, 0, buff.Length);
				stream.Flush();
			}
		}
	}
}