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
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;

namespace Schumix.Irc.Commands.GoogleWebSearch
{
	class JsonHelper
	{
		public static T Deserialise<T>(string json) where T : new()
		{
			if(!string.IsNullOrEmpty(json))
			{
				byte[] respBytes = ASCIIEncoding.UTF8.GetBytes(json);

				using(var reader = new StreamReader(new MemoryStream(respBytes)))
				{
					var serializer = new DataContractJsonSerializer(typeof(T));
					T returnObj = (T)serializer.ReadObject(reader.BaseStream);
					return returnObj;
				}
			}
			else
				return new T();
		}
	}
}