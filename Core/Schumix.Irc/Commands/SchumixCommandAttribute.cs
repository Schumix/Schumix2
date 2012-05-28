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
using Schumix.API;

namespace Schumix.Irc.Commands
{
	///<summary>
	/// Marks a method as an Schumix command.
	///</summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class SchumixCommandAttribute : Attribute
	{
		#region Properties

		/// <summary>
		/// Gets the command.
		/// </summary>
		public string Command { get; private set; }

		/// <summary>
		/// Gets the command permission.
		/// </summary>
		public CommandPermission Permission { get; private set; }

		#endregion

		#region Constructor

		/// <summary>
		/// Marks a method as an Schumix command.
		/// </summary>
		/// <param name="command">The command corresponding to this method.</param>
		/// <param name="permission">Command's access permission</param>
		public SchumixCommandAttribute(string command, CommandPermission permission = CommandPermission.Normal)
		{
			Command = command.ToLower();
			Permission = permission;
		}

		#endregion
	}
}