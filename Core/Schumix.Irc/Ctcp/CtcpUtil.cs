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

namespace Schumix.Irc.Ctcp
{
	/// <summary>
	/// Constants and utility methods to support CTCP.
	/// </summary>
	/// <remarks>The CTCP constants should be used to test incoming
	/// CTCP queries for their type and as the CTCP command
	/// for outgoing ones.</remarks>
	public static class CtcpUtil
	{
		/// <summary>CTCP Finger.</summary>
		public const string Finger = "FINGER";
		/// <summary>CTCP USERINFO.</summary>
		public const string UserInfo = "USERINFO";
		/// <summary>CTCP VERSION.</summary>
		public const string Version = "VERSION";
		/// <summary>CTCP SOURCE.</summary>
		public const string Source = "SOURCE";
		/// <summary>CTCP CLIENTINFO.</summary>
		public const string ClientInfo = "CLIENTINFO";
		/// <summary>CTCP ERRMSG.</summary>
		public const string ErrorMessage = "ERRMSG";
		/// <summary>CTCP PING.</summary>
		public const string Ping = "PING";
		/// <summary>CTCP TIME.</summary>
		public const string Time = "TIME";
	}
}