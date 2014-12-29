/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2002 Aaron Hunter <thresher@sharkbite.org>
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

namespace Schumix.Irc.Util
{
	/// <summary>
	/// The possible stats message query parameters.
	/// </summary>
	public enum StatsQuery: int
	{
		/// <summary>
		/// A list of server connections.
		/// </summary>
		Connections = 108, // l

		/// <summary>
		/// The usage count for each of command supported
		/// by the server.
		/// </summary>
		CommandUsage = 109, // m

		/// <summary>
		/// The list of IRC operators.
		/// </summary>
		Operators = 111, // o

		/// <summary>
		/// The server uptime.
		/// </summary>
		Uptime = 117, // u
	};
}