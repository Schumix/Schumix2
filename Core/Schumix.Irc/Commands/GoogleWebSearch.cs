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
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Schumix.Irc.Commands
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

	/// <summary>
	/// A Google web search result.
	/// </summary>
	[DataContract]
	class GoogleWebResult
	{
		/// <summary>
		/// The internal Google class for this object. This will always be "GWebSearch".
		/// </summary>
		[DataMember(Name = "GsearchResultClass", Order = 0)]
		public string GoogleResultClass { get; set; }

		/// <summary>
		/// The unescaped Url of the search result.
		/// </summary>
		[DataMember(Name = "unescapedUrl", Order = 1)]
		public string UnescapedUrl { get; set; }

		/// <summary>
		/// The search result's Url.
		/// </summary>
		[DataMember(Name = "url", Order = 2)]
		public string Url { get; set; }

		/// <summary>
		/// The display Url of the search result.
		/// </summary>
		[DataMember(Name = "visibleUrl", Order = 3)]
		public string VisibleUrl { get; set; }

		/// <summary>
		/// The Url of the cached version of this page.
		/// </summary>
		[DataMember(Name = "cacheUrl", Order = 4)]
		public string CacheUrl { get; set; }

		/// <summary>
		/// The formatted search result title.
		/// </summary>
		[DataMember(Name = "title", Order = 5)]
		public string Title { get; set; }

		/// <summary>
		/// The unformatted title of the search result.
		/// </summary>
		[DataMember(Name = "titleNoFormatting", Order = 6)]
		public string TitleNoFormatting { get; set; }

		/// <summary>
		/// The Goolge snippet for this search result.
		/// </summary>
		[DataMember(Name = "content", Order = 7)]
		public String Snippet { get; set; }
	}

	/// <summary>
	/// The containing object for a set of Google Ajax API results.
	/// </summary>
	[DataContract]
	class GoogleWebResponseData
	{
		/// <summary>
		/// The list of search results.
		/// </summary>
		[DataMember(Name = "responseData")]
		public GoogleWebResultSet ResultSet { get; set; }
	}

	/// <summary>
	/// A set of Google web search results.
	/// </summary>
	[DataContract]
	class GoogleWebResultSet
	{
		/// <summary>
		/// The list of search results.
		/// </summary>
		[DataMember(Name = "results")]
		public GoogleWebResult[] Results { get; set; }
	}
}