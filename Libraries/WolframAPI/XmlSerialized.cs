/*
 * This file is part of Schumix.
 * 
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
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics.Contracts;
using WolframAPI.Exceptions;
using Schumix.Framework;
using Schumix.Framework.Localization;

namespace WolframAPI
{
	///<summary>
	/// Base class for XML-serialized types.
	///</summary>
	public abstract class XmlSerialized : ISerializableType
	{
		#region Implementation of ISerializableType
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		//private readonly Utilities sUtilities = Singleton<Utilities>.Instance;

		/// <summary>
		/// Serializes the current instance and returns the result as a <see cref="string"/>
		/// <para>Should be used with XML serialization only.</para>
		/// </summary>
		/// <returns>The serialized instance.</returns>
		public string Serialize()
		{
			//if(!sPlatform.IsLinux)
			//	Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

			string data;

			try
			{
				using(var ms = new MemoryStream())
				{
					var serializer = new XmlSerializer(GetType());
					var nss = new XmlSerializerNamespaces();
					nss.Add(string.Empty, string.Empty);
					serializer.Serialize(ms, this, nss);

					using(var reader = new StreamReader(ms))
					{
						data = reader.ReadToEnd();
					}
				}
			}
			catch(InvalidOperationException x)
			{
				throw new WolframException(sLConsole.GetString("Error during serialization"), x);
			}

			if(string.IsNullOrEmpty(data))
				throw new WolframException(string.Format(sLConsole.GetString("Error while serializing instance! Type: {0}"), GetType().FullName));

			return data;
		}

		#endregion
	}
}