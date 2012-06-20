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
using System.IO;
using System.Net;
using System.Web;
using System.Text;
using Schumix.Installer.Config;

namespace Schumix.Installer
{
	sealed class Utilities
	{
		private Utilities() {}

		public string GetUrl(string url)
		{
			using(var client = new WebClient())
			{
				client.Headers.Add("referer", Consts.SchumixReferer);
				client.Headers.Add("user-agent", Consts.SchumixUserAgent);
				client.Encoding = Encoding.UTF8;
				return client.DownloadString(url);
			}
		}

		public string GetUrl(string url, string args)
		{
			using(var client = new WebClient())
			{
				client.Headers.Add("referer", Consts.SchumixReferer);
				client.Headers.Add("user-agent", Consts.SchumixUserAgent);
				client.Encoding = Encoding.UTF8;
				return client.DownloadString(new Uri(url + HttpUtility.UrlEncode(args)));
			}
		}

		public string GetUrl(string url, string args, string noencode)
		{
			using(var client = new WebClient())
			{
				client.Headers.Add("referer", Consts.SchumixReferer);
				client.Headers.Add("user-agent", Consts.SchumixUserAgent);
				client.Encoding = Encoding.UTF8;
				return client.DownloadString(new Uri(url + HttpUtility.UrlEncode(args) + noencode));
			}
		}

		public void DownloadFile(string url, string filename)
		{
			using(var client = new WebClient())
			{
				client.Headers.Add("referer", Consts.SchumixReferer);
				client.Headers.Add("user-agent", Consts.SchumixUserAgent);
				client.DownloadFile(url, filename);
			}
		}

		public PlatformType GetPlatformType()
		{
			PlatformType platform = PlatformType.None;
			var pid = Environment.OSVersion.Platform;

			switch(pid)
			{
				case PlatformID.Win32NT:
				case PlatformID.Win32S:
				case PlatformID.Win32Windows:
				case PlatformID.WinCE:
					platform = PlatformType.Windows;
					break;
				case PlatformID.Unix:
					platform = PlatformType.Linux;
					break;
				case PlatformID.MacOSX:
					platform = PlatformType.MacOSX;
					break;
				case PlatformID.Xbox:
					platform = PlatformType.Xbox;
					break;
				default:
					platform = PlatformType.None;
					break;
			}

			return platform;
		}

		public string GetVersion()
		{
			return Schumix.Installer.Config.Consts.SchumixVersion;
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
	}
}