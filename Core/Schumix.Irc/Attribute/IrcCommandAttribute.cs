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
using Schumix.Framework.Extensions;

namespace Schumix.Irc
{
	///<summary>
	/// Marks a method as an Irc command.
	///</summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class IrcCommandAttribute : Attribute
	{
		#region Properties

		/// <summary>
		/// Gets the command.
		/// </summary>
		public string Command { get; private set; }

		#endregion

		#region Constructor

		/// <summary>
		/// Marks a method as an Schumix command.
		/// </summary>
		/// <param name="command">The command corresponding to this method.</param>
		public IrcCommandAttribute(string command)
		{
			Command = command;
		}

		public IrcCommandAttribute(int command)
		{
			Command = command.ToIrcOpcode();
		}

		public IrcCommandAttribute(ReplyCode command)
		{
			Command = command.ToInt32().ToIrcOpcode();
		}

		#endregion
	}
}