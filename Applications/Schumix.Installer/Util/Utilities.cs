/*
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
using System.IO;
using System.Net;
using System.Web;
using System.Text;
using Schumix.Installer.Config;
using Schumix.Installer.Extensions;

namespace Schumix.Installer.Util
{
	sealed class Utilities
	{
		private readonly object WriteLock = new object();
		private Utilities() {}

		public string GetUrl(string url)
		{
			lock(WriteLock)
			{
				return GetUrl(url, string.Empty, string.Empty);
			}
		}
		
		public string GetUrl(string url, string args)
		{
			lock(WriteLock)
			{
				return GetUrl(url, args, string.Empty);
			}
		}
		
		public string GetUrl(string url, string args, string noencode)
		{
			lock(WriteLock)
			{
				if(!args.IsNullOrEmpty() && noencode.IsNullOrEmpty())
					url = url + HttpUtility.UrlEncode(args);
				else if(!args.IsNullOrEmpty() && !noencode.IsNullOrEmpty())
					url = url + HttpUtility.UrlEncode(args) + noencode;

				var request = (HttpWebRequest)WebRequest.Create(url);
				request.AllowAutoRedirect = true;
				request.UserAgent = Consts.InstallerUserAgent;
				request.Referer = Consts.InstallerReferer;

				int length = 0;
				byte[] buf = new byte[1024];
				var sb = new StringBuilder();

				using(var response = (HttpWebResponse)request.GetResponse())
				{
					using(var stream = response.GetResponseStream())
					{
						while((length = stream.Read(buf, 0, buf.Length)) != 0)
						{
							buf = Encoding.Convert(Encoding.GetEncoding(response.CharacterSet), Encoding.UTF8, buf);
							sb.Append(Encoding.UTF8.GetString(buf, 0, length));
						}
					}
				}

				return WebUtility.HtmlDecode(sb.ToString());
			}
		}

		public void DownloadFile(string url, string filename)
		{
			using(var client = new WebClient())
			{
				client.Headers.Add("referer", Consts.InstallerReferer);
				client.Headers.Add("user-agent", Consts.InstallerUserAgent);
				client.DownloadFile(url, filename);
			}
		}

		public string GetVersion()
		{
			return Schumix.Installer.Config.Consts.InstallerVersion;
		}

		public void CreateDirectory(string Name)
		{
			if(!Directory.Exists(Name))
				Directory.CreateDirectory(Name);
		}

		public void CreateFile(string Name)
		{
			if(!File.Exists(Name))
				new FileStream(Name, FileMode.Append, FileAccess.Write, FileShare.Write).Close();
		}

		public void ClearAttributes(string currentDir)
		{
			if(Directory.Exists(currentDir))
			{
				var subDirs = Directory.GetDirectories(currentDir);

				foreach(string dir in subDirs)
					ClearAttributes(dir);

				var files = Directory.GetFiles(currentDir);

				foreach(string file in files)
					File.SetAttributes(file, FileAttributes.Normal);
			}
		}
	}
}