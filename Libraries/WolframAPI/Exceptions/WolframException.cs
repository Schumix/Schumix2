﻿/*
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
using System.Runtime.Serialization;

namespace WolframAPI.Exceptions
{
	/// <summary>
	/// Thrown by Wolfram API components.
	/// </summary>
	public sealed class WolframException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="WolframException"/> class.
		/// </summary>
		public WolframException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="WolframException"/> class with a specified error message.
		/// </summary>
		/// <param name="message">The message that describes the error. </param>
		public WolframException(string message) : base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="WolframException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception. </param><param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified. </param>
		public WolframException(string message, Exception innerException) : base(message, innerException)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="WolframException"/> class with serialized data.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown. </param><param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination. </param><exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception><exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
		public WolframException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}