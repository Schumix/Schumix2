/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2002 Aaron Hunter <thresher@sharkbite.org>
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

namespace Schumix.Irc.Util
{
	/// <summary>
	/// The possible user modes.
	/// </summary>
	public enum UserMode
	{
		/// <summary>
		/// User is away
		/// </summary>
		Away = 97, // a

		/// <summary>
		/// User will receive server status messages
		/// </summary>
		Wallops = 119, // w

		/// <summary>
		/// User cannot be seen by certain IRC queries
		/// </summary>
		Invisible = 105, // i

		/// <summary>
		/// The user is an IRC operator (IRCOP)
		/// </summary>
		Operator = 111, // o

		/// <summary>
		/// Not used
		/// </summary>
		Restricted = 114, // r

		/// <summary>
		/// User is a channel operator/owner
		/// </summary>
		LocalOperator = 79, // O

		/// <summary>
		/// Marks a user for receipt of server notices
		/// </summary>
		ServerNotices = 115 // s
	};
}