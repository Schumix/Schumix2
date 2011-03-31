/*
 * This file is part of Schumix.
 * 
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
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Schumix.Framework
{
	public sealed class Utility
	{
		private Utility() {}

		public string GetUrl(string url)
		{
			string kod;

			using(var client = new WebClient())
			{
				kod = client.DownloadString(url);
			}

			return kod;
		}

		public string GetUrl(string url, string args)
		{
			string kod;
			var u = new Uri(url + HttpUtility.UrlEncode(args));

			using(var client = new WebClient())
			{
				kod = client.DownloadString(u);
			}

			return kod;
		}

		public string GetUrl(string url, string args, string noencode)
		{
			string kod;
			var u = new Uri(url + HttpUtility.UrlEncode(args) + noencode);

			using(var client = new WebClient())
			{
				kod = client.DownloadString(u);
			}

			return kod;
		}

		public string GetRandomString()
		{
			string path = Path.GetRandomFileName();
			path = path.Replace(".", "");
			return path;
		}

		public string Sha1(string value)
		{
			if(value == null)
				throw new ArgumentNullException("value");

			var x = new SHA1CryptoServiceProvider();
			var data = Encoding.ASCII.GetBytes(value);
			data = x.ComputeHash(data);
#if !MONO
			x.Dispose();
#endif
			var ret = string.Empty;

			for(var i = 0; i < data.Length; i++)
				ret += data[i].ToString("x2").ToLower();

			return ret;
		}

		public string Md5(string value)
		{
			if(value == null)
				throw new ArgumentNullException("value");

			var x = new MD5CryptoServiceProvider();
			var data = Encoding.ASCII.GetBytes(value);
			data = x.ComputeHash(data);
#if !MONO
			x.Dispose();
#endif
			var ret = string.Empty;

			for(var i = 0; i < data.Length; i++)
				ret += data[i].ToString("x2").ToLower();

			return ret;
		}

		public string MD5File(string fileName)
		{
			if(fileName == null)
				throw new ArgumentNullException("fileName");

			byte[] retVal;

			using(var file = new FileStream(fileName, FileMode.Open))
			{
				var md5 = new MD5CryptoServiceProvider();
				retVal = md5.ComputeHash(file);
#if !MONO
				md5.Dispose();
#endif
			}

			var sb = new StringBuilder();

			if(retVal != null)
			{
				for(var i = 0; i < retVal.Length; i++)
					sb.Append(retVal[i].ToString("x2"));
			}

			return sb.ToString();
		}

		public bool IsPrime(long x)
		{
			x = Math.Abs(x);

			if(x == 1 || x == 0)
				return false;

			if(x == 2)
				return true;

			if(x % 2 == 0)
				return false;

			bool p = true;

			for(var i = 3; i <= Math.Floor(Math.Sqrt(x)); i += 2)
			{
				if(x % i == 0)
				{
					p = false;
					break;
				}
			}

			return p;
		}
	}
}