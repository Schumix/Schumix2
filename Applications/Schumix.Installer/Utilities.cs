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
using System.Net;
using System.Web;

namespace Schumix.Installer
{
	public enum SCompiler
	{
		VisualStudio,
		Mono,
		None
	}

	public sealed class Utilities
	{
		private Utilities() {}

		public string GetUrl(string url)
		{
			using(var client = new WebClient())
			{
				client.Headers.Add("user-agent", "Query :)");
				return client.DownloadString(url);
			}
		}

		public string GetUrl(string url, string args)
		{
			using(var client = new WebClient())
			{
				client.Headers.Add("user-agent", "Query :)");
				return client.DownloadString(new Uri(url + HttpUtility.UrlEncode(args)));
			}
		}

		public string GetUrl(string url, string args, string noencode)
		{
			using(var client = new WebClient())
			{
				client.Headers.Add("user-agent", "Query :)");
				return client.DownloadString(new Uri(url + HttpUtility.UrlEncode(args) + noencode));
			}
		}

		public void DownloadFile(string url, string filename)
		{
			using(var client = new WebClient())
			{
				client.Headers.Add("user-agent", "Query :)");
				client.DownloadFile(url, filename);
			}
		}

		public SCompiler GetCompiler()
		{
			SCompiler compiler = SCompiler.None;
			var pid = Environment.OSVersion.Platform;

			switch(pid)
			{
				case PlatformID.Win32NT:
				case PlatformID.Win32S:
				case PlatformID.Win32Windows:
				case PlatformID.WinCE:
					compiler = SCompiler.VisualStudio;
					break;
				case PlatformID.Unix:
				case PlatformID.MacOSX:
					compiler = SCompiler.Mono;
					break;
				case PlatformID.Xbox:
					compiler = SCompiler.None;
					break;
				default:
					compiler = SCompiler.None;
					break;
			}

			return compiler;
		}
	}
}