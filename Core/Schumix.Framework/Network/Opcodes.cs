/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Twl
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

#pragma warning disable 1591

namespace Schumix.Framework.Network
{
	/// <summary>
	/// List of possible Opcodes.
	/// </summary>
	public enum Opcode : int
	{
		SCMSG_PACKET_NULL       = 0x0,
		CMSG_REQUEST_AUTH       = 0x01,
		SMSG_AUTH_APPROVED      = 0x02,
		SMSG_AUTH_DENIED        = 0x03,
		CMSG_REQUEST_CONFIG     = 0x04,
		SMSG_CONFIG_RESPONSE    = 0x05,
		CMSG_CLOSE_CONNECTION   = 0x06,
		SMSG_CLOSE_CONNECTION   = 0x07,
		CMSG_REQUEST_SCS_RANDOM = 0x08,
		SMSG_SEND_SCS_RANDOM    = 0x09
	}
}

#pragma warning restore 1591