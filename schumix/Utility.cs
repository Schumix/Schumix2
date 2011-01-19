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
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Schumix
{
	public class Utility
	{
		private Utility() {}

		public string GetUrl(string url)
		{
			WebClient web = new WebClient();
			string kod = web.DownloadString(url);
			web.Dispose();
			return kod;
		}

		public string GetRandomString()
		{
			string path = Path.GetRandomFileName();
			path = path.Replace(".", "");
			return path;
		}

		public string Sha1(string info)
		{
			Byte[] originalBytes;
			Byte[] encodedBytes;
			SHA1 sha1;

			sha1 = new SHA1CryptoServiceProvider();
			originalBytes = ASCIIEncoding.Default.GetBytes(info);
			encodedBytes = sha1.ComputeHash(originalBytes);

			string convert = BitConverter.ToString(encodedBytes);
			string[] adat = convert.Split('-');
			string Sha1 = "";

			for(int i = 0; i < adat.Length; i++)
				Sha1 += adat[i];

			return Sha1.ToLower();
		}

        public bool IsPrime(long x)
        {
            x = Math.Abs(x);

            if (x == 1 || x == 0)
                return false;

            if (x == 2)
                return true;

            if (x % 2 == 0) return false;
            
            var p = true;

            for(var i = 3; i <= Math.Floor(Math.Sqrt(x)); i +=2 )
            {
                if (x % i == 0)
                {
                    p = false;
                    break;
                }
            }

            return p;
        }
	}
}