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
using System.Runtime.Serialization;

namespace Schumix.Installer.Exceptions
{
	[Serializable]
	sealed class InstallerException : Exception
	{
		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public InstallerException()
		{
		}

		/// <summary>
		/// Initializes a new instance with a specified error message.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public InstallerException(string message) : base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance with a reference to the inner 
		/// exception that is the cause of this exception.
		/// </summary>
		/// <param name="innerException">
		/// The exception that is the cause of the current exception, 
		/// or a null reference if no inner exception is specified.
		/// </param>
		public InstallerException(Exception innerException) : base(null, innerException)
		{
		}

		/// <summary>
		/// Initializes a new instance with a specified error message and a 
		/// reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		/// <param name="innerException">
		/// The exception that is the cause of the current exception, 
		/// or a null reference if no inner exception is specified.
		/// </param>
		public InstallerException(string message, Exception innerException) : base(message, innerException)
		{
		}

		/// <summary>
		/// Initializes a new instance with serialized data.
		/// </summary>
		/// <param name="info">
		/// The <see cref="System.Runtime.Serialization.SerializationInfo"/> that holds the 
		/// serialized object data about the exception being thrown.
		/// </param>
		/// <param name="context">
		/// The <see cref="System.Runtime.Serialization.StreamingContext"/> that contains 
		/// contextual information about the source or destination.
		/// </param>
		/// <exception cref="System.ArgumentNullException">The info parameter is null.</exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">The class name is null or System.Exception.HResult is zero (0).</exception>
		public InstallerException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
