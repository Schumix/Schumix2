/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013-2015 Schumix Team <http://schumix.eu/>
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
using Schumix.Framework.Config;
using Schumix.Framework.Listener;

namespace Schumix.Irc.Commands
{
	public abstract partial class CommandHandler
	{
		private readonly object ListenerLock = new object();

		// Update Listener Cache Database
		public void UpdateLCDB(string Sql)
		{
			lock(ListenerLock)
			{
				if(!ListenerConfig.Enabled)
					return;

				var packet = new ListenerPacket();
				packet.Write<int>((int)ListenerOpcode.SMSG_UPDATE_DB);
				packet.Write<string>(Sql);
				sSchumixPacketHandler.SendPacketBackAllHost(packet);
			}
		}

		public void UpdateLCDB(string Sql, params object[] args)
		{
			lock(ListenerLock)
			{
				if(!ListenerConfig.Enabled)
					return;

				UpdateLCDB(string.Format(Sql, args));
			}
		}
	}
}