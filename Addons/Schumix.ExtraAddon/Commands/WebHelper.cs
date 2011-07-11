/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2011 Twl
 * Copyright (C) 2010-2011 Megax <http://www.megaxx.info/>
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
using System.Net;
using System.Text.RegularExpressions;
using Schumix.Framework;

namespace Schumix.ExtraAddon.Commands
{
	/// <summary>
	///   A class which provides useful methods for working with the world-wide web.
	/// </summary>
	public static class WebHelper
	{
		private static readonly Utilities sUtilities = Singleton<Utilities>.Instance;

		/// <summary>
		///   Gets the title of the specified webpage.
		/// </summary>
		/// <param name = "url">
		///   The webpage's URL.
		/// </param>
		/// <returns>
		///   The webpage's title.
		/// </returns>
		public static string GetWebTitle(Uri url)
		{
			try
			{
				var request = (HttpWebRequest)WebRequest.Create(url);
				request.Timeout = 3500;
				request.AllowAutoRedirect = true;
				request.UserAgent = "Schumix2 IRC Bot " + sUtilities.GetVersion() + " / .NET " + Environment.Version;
				request.Referer = "http://www.wowemuf.org";

				var response = request.GetResponse();
				var stream = response.GetResponseStream();
				string data;

				using(var rdr = new StreamReader(stream))
				{
					data = rdr.ReadToEnd();
				}

				response.Close();
				var getTitleRegex = new Regex(@"<title>(?<ttl>.*\s*.+\s*.*)\s*</title>", RegexOptions.IgnoreCase);
				var match = getTitleRegex.Match(data);

				return (match.Success) ? (match.Groups["ttl"].ToString()) : "No title found.";
			}
			catch(Exception e)
			{
				Log.Debug("WebHelper", "Exception thrown while fetching web title: {0}", e.Message);
				return string.Empty;
			}
		}
	}
}