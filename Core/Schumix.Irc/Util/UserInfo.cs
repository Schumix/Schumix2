/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2002 Aaron Hunter <thresher@sharkbite.org>
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

namespace Schumix.Irc.Util
{
	/// <summary>
	/// A convenient holder of user information. Instances of this class
	/// are created internally.
	/// </summary>
	public class UserInfo
	{
		private static readonly UserInfo EmptyInstance = new UserInfo();
		/// <summary>The user's handle.</summary>
		private readonly string _nickName;
		/// <summary>The user's username on the local machine</summary>
		private readonly string _userName;
		/// <summary>The user's fully qualified host name</summary>
		private readonly string _hostName;

		/// <summary>
		/// Creat an empty instance
		/// </summary>
		private UserInfo()
		{
			_nickName = string.Empty;
			_userName = string.Empty;
			_hostName = string.Empty;
		}

		/// <summary>
		/// Create a new UserInfo and set all its values.
		/// </summary>
		public UserInfo(string nick, string name, string host)
		{
			_nickName = nick;
			_userName = name;
			_hostName = host;
		}

		/// <summary>
		/// An IRC user's nick name.
		/// </summary>
		public string Nick
		{
			get 
			{
				return _nickName;
			}
		}

		/// <summary>
		/// An IRC user's system username.
		/// </summary>
		public string User
		{
			get
			{
				return _userName;
			}
		}

		/// <summary>
		/// The hostname of the IRC user's machine.
		/// </summary>
		public string Hostname
		{
			get 
			{
				return _hostName;
			}
		}

		/// <summary>
		/// A singleton blank instance of UserInfo used when an instance is required
		/// by a method signature but no infomation is available, e.g. the last reply
		/// from a Who request.
		/// </summary>
		public static UserInfo Empty
		{
			get
			{
				return EmptyInstance;
			}
		}

		/// <summary>
		/// A string representation of this object which
		/// shows all its values.
		/// </summary>
		public override string ToString()
		{
			return string.Format("Nick={0} User={1} Host={2}", Nick, User, Hostname);
		}
	}
}