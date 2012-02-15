/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Twl
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
using System.Collections.Generic;

namespace Schumix.Framework.Client
{
	/// <summary>
	/// Class used to create packets which will be sent between Alaris server and client.
	/// </summary>
	public class SchumixPacket : IDisposable
	{
		private readonly List<string> split_buffer;
		private const string Separator = "|;|";
		private int read_position = 0;
		private string _netmsg;
		
		/// <summary>
		/// Initializes a new instance of the SchumixPacket class.
		/// </summary>
		/// <param name='net_message'>
		/// Net message.
		/// </param>
		public SchumixPacket(string net_message)
		{
			_netmsg = net_message;
			split_buffer = new List<string>(_netmsg.Split((new string[] {Separator}), StringSplitOptions.RemoveEmptyEntries));
		}
		
		/// <summary>
		/// Initializes a new instance of the SchumixPacket class.
		/// </summary>
		public SchumixPacket()
		{
			_netmsg = string.Empty;
		}

		/// <summary>
		/// Read this instance.
		/// </summary>
		/// <typeparam name='T'>
		/// The 1st type parameter.
		/// </typeparam>
		public T Read<T>()
		{
			if(read_position > (split_buffer.Count - 1))
				return ((T)Convert.ChangeType(0, typeof(T)));

			T ret = ((T)(Convert.ChangeType((split_buffer[read_position]), typeof(T))));
			++read_position;

			if(typeof(T) == typeof(string))
			  	return ((T)Convert.ChangeType((((string)(ret as object)).Replace("{[n]}", Environment.NewLine)), typeof(T)));

			return ret;
		}

		/// <summary>
		/// Write the specified object to the packet buffer.
		/// </summary>
		/// <param name='v'>
		/// Object.
		/// </param>
		/// <typeparam name='T'>
		/// Type of object.
		/// </typeparam>
		public void Write<T>(T v)
		{
			string append = ((string)Convert.ChangeType(v, typeof(string)));
			
			if(string.IsNullOrEmpty(append))
				return;
			
			_netmsg += (append + Separator);
		}

		/// <summary>
		/// Resets the reader position.
		/// </summary>
		public void ResetReaderPosition() { read_position = 0; }

		/// <summary>
		/// Gets the net message.
		/// </summary>
		/// <returns>
		/// The net message.
		/// </returns>
		public string GetNetMessage() { return _netmsg; }

		/// <summary>
		/// Gets the buffer.
		/// </summary>
		/// <returns>
		/// The buffer.
		/// </returns>
		public List<string> GetBuffer() { return split_buffer; }

		/// <summary>
		/// Releases all resource used by the SchumixPacket object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose"/> when you are finished using the SchumixPacket. The
		/// <see cref="Dispose"/> method leaves the SchumixPacket in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the SchumixPacket so the
		/// garbage collector can reclaim the memory that the SchumixPacket was occupying.
		/// </remarks>
		public void Dispose()
		{
			read_position = 0;
			split_buffer.Clear();
			_netmsg = string.Empty;
		}

		/// <summary>
		/// Prepares the string.
		/// </summary>
		/// <returns>
		/// The string.
		/// </returns>
		/// <param name='s'>
		/// S.
		/// </param>
		public static string PrepareString(string s)
		{
			return (s.Replace(Environment.NewLine, "{[n]}"));
		}
	}
}