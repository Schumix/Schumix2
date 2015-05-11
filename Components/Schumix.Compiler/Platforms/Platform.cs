/*
 * This file is part of Schumix.
 * 
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
using System.Linq;
using System.Management;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Schumix.Compiler.Extensions;

namespace Schumix.Compiler.Platforms
{
	sealed class Platform
	{
		public bool IsWin9x
		{
			get { return Environment.OSVersion.Platform == PlatformID.Win32Windows; }
		}

		public bool IsWinNT
		{
			get { return Environment.OSVersion.Platform == PlatformID.Win32NT; }
		}

		public bool IsWinCE
		{
			get { return Environment.OSVersion.Platform == PlatformID.WinCE; }
		}

		public bool IsWindows
		{
			get
			{
				var platform = Environment.OSVersion.Platform;
				return (IsWin9x || IsWinNT || platform == PlatformID.Win32S);
			}
		}

		public bool IsUnix
		{
			get
			{
				var platform = Environment.OSVersion.Platform;
				return (platform == PlatformID.Unix || platform == PlatformID.MacOSX || platform == (PlatformID)128);
			}
		}

		public bool IsLinux
		{
			get
			{
				return Environment.OSVersion.Platform == PlatformID.Unix ||
					(Environment.OSVersion.Platform == (PlatformID)128 && !Directory.Exists("/Applications") &&
					!Directory.Exists("/System") && !Directory.Exists("/Users") && !Directory.Exists("/Volumes"));
			}
		}

		public bool IsMacOSX
		{
			get
			{
				return Environment.OSVersion.Platform == PlatformID.MacOSX ||
					(Environment.OSVersion.Platform == (PlatformID)128 && Directory.Exists("/Applications") &&
					Directory.Exists("/System") && Directory.Exists("/Users") && Directory.Exists("/Volumes"));
			}
		}

		public bool IsXbox
		{
			get { return Environment.OSVersion.Platform == PlatformID.Xbox; }
		}

		public bool IsMono
		{
			get { return !Type.GetType("Mono.Runtime").IsNull(); }
		}

		public bool Is32BitProcess
		{
			get { return IntPtr.Size == 4; }
		}

		public bool Is64BitProcess
		{
			get { return Environment.Is64BitProcess; } // IntPtr.Size == 8
		}

		private Platform() {}

		public string GetPlatform()
		{
			string platform = string.Empty;
			var pid = Environment.OSVersion.Platform;

			switch(pid)
			{
				case PlatformID.WinCE:
					platform = "WinCE";
					break;
				case PlatformID.Win32NT:
				case PlatformID.Win32S:
				case PlatformID.Win32Windows:
					platform = "Windows";
					break;
				case PlatformID.Unix:
				case (PlatformID)128:
					// Well, there are chances MacOSX is reported as Unix instead of MacOSX.
					// Instead of platform check, we'll do a feature checks (Mac specific root folders)
					if(Directory.Exists("/Applications") && Directory.Exists("/System") &&
						Directory.Exists("/Users") && Directory.Exists("/Volumes"))
					{
						platform = "MacOSX";
						break;
					}

					platform = "Linux";
					break;
				case PlatformID.MacOSX:
					platform = "MacOSX";
					break;
				case PlatformID.Xbox:
					platform = "Xbox";
					break;
				default:
					platform = "Unknown";
					break;
			}

			return platform;
		}

		/// <summary>
		/// Returns the name of the operating system running on this computer.
		/// </summary>
		/// <returns>A string containing the the operating system name.</returns>
		public string GetOSName()
		{
			var Info = Environment.OSVersion;
			string Name = string.Empty;

			switch(Info.Platform)
			{
				case PlatformID.Win32S:
				case PlatformID.Win32NT:
				case PlatformID.Win32Windows:
				{
					var osname = (from x in new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem").Get().OfType<ManagementObject>() select x.GetPropertyValue("Caption")).First();
					Name = !osname.IsNull() ? osname.ToString().Trim() : "Unknown";
					break;
				}
				case PlatformID.WinCE:
				{
					Name = "Windows CE";
					break;
				}
				case PlatformID.Unix:
				case (PlatformID)128:
				{
					// Well, there are chances MacOSX is reported as Unix instead of MacOSX.
					// Instead of platform check, we'll do a feature checks (Mac specific root folders)
					if(Directory.Exists("/Applications") && Directory.Exists("/System") &&
						Directory.Exists("/Users") && Directory.Exists("/Volumes"))
					{
						Name = "Mac OS X";
						break;
					}

					string os = string.Empty;

					try
					{
						// Linux
						// FreeBSD
						var info = new ProcessStartInfo("uname", "-s");
						info.UseShellExecute = false;
						info.RedirectStandardOutput = true;
						info.RedirectStandardError = true;

						var process = Process.Start(info);
						process.WaitForExit();

						os = process.StandardOutput.ReadToEnd();
					}
					catch(Exception)
					{

					}

					if(os.IsNullOrEmpty())
					{
						Name = Info.ToString();
						break;
					}

					string distro = GetLinuxDistro();

					if(distro.IsNullOrEmpty())
					{
						Name = os + " " + Info.Version;
						break;
					}

					Name = distro;
					break;
				}
				case PlatformID.MacOSX:
				{
					Name = "Mac OS X";
					break;
				}
				case PlatformID.Xbox:
				{
					Name = "Xbox";
					break;
				}
				default:
				{
					Name = "Unknown";
					break;
				}
			}

			return Name;
		}

		public PlatformType GetPlatformType()
		{
			var platform = PlatformType.None;
			var pid = Environment.OSVersion.Platform;

			switch(pid)
			{
				case PlatformID.WinCE:
					platform = PlatformType.WinCE;
					break;
				case PlatformID.Win32S:
				case PlatformID.Win32NT:
				case PlatformID.Win32Windows:
					platform = PlatformType.Windows;
					break;
				case PlatformID.Unix:
				case (PlatformID)128:
					// Well, there are chances MacOSX is reported as Unix instead of MacOSX.
					// Instead of platform check, we'll do a feature checks (Mac specific root folders)
					if(Directory.Exists("/Applications") && Directory.Exists("/System") &&
						Directory.Exists("/Users") && Directory.Exists("/Volumes"))
					{
						platform = PlatformType.MacOSX;
						break;
					}

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

		private string GetLinuxDistro()
		{
			string distro = string.Empty;

			try
			{
				bool debian = false;

				/*if(File.Exists("/etc/portage/make.conf") || File.Exists("/etc/make.conf"))
				{
					char keywords[bsize];
					while(fgets(buffer, bsize, fp) != NULL)
						find_match_char(buffer, "ACCEPT_KEYWORDS", keywords);
					/* cppcheck-suppress uninitvar */
				/*if(strstr(keywords, "\"") == NULL)
				snprintf(buffer, bsize, "Gentoo Linux (stable)");
				else
					snprintf(buffer, bsize, "Gentoo Linux %s", keywords);
				}*/
				if(File.Exists("/etc/redhat-release"))
				{
					using(var file = new StreamReader("/etc/redhat-release"))
					{
						distro = file.ReadToEnd();
					}
				}
				else if(File.Exists("/etc/mageia-release"))
				{
					using(var file = new StreamReader("/etc/mageia-release"))
					{
						distro = file.ReadToEnd();
					}
				}
				else if(File.Exists("/etc/slackware-version"))
				{
					using(var file = new StreamReader("/etc/slackware-version"))
					{
						distro = file.ReadToEnd();
					}
				}
				else if(File.Exists("/etc/mandrake-release"))
				{
					using(var file = new StreamReader("/etc/mandrake-release"))
					{
						distro = file.ReadToEnd();
					}
				}
				else if(File.Exists("/etc/debian_version"))
				{
					debian = true;

					using(var file = new StreamReader("/etc/debian_version"))
					{
						distro = "Debian " + file.ReadToEnd();
					}
				}
				else if(File.Exists("/etc/SuSE-release"))
				{
					using(var file = new StreamReader("/etc/SuSE-release"))
					{
						distro = file.ReadToEnd();
					}
				}
				else if(File.Exists("/etc/turbolinux-release"))
				{
					using(var file = new StreamReader("/etc/turbolinux-release"))
					{
						distro = file.ReadToEnd();
					}
				}
				else if(File.Exists("/etc/arch-release"))
					distro = "ArchLinux";
				else if(File.Exists("/etc/lsb-release"))
				{
					using(var file = new StreamReader("/etc/lsb-release"))
					{
						string s = file.ReadToEnd();
						string id = s.Remove(0, s.IndexOf("DISTRIB_ID=") + "DISTRIB_ID=".Length);
						distro = id.Substring(0, id.IndexOf('\n'));
						string release = s.Remove(0, s.IndexOf("DISTRIB_RELEASE=") + "DISTRIB_RELEASE=".Length);
						distro += " " + release.Substring(0, release.IndexOf(('\n')));
					}
				}

				if(debian && File.Exists("/etc/lsb-release")) // Ubuntu, etc.
				{
					using(var file = new StreamReader("/etc/lsb-release"))
					{
						string s = file.ReadToEnd();
						string id = s.Remove(0, s.IndexOf("DISTRIB_ID=") + "DISTRIB_ID=".Length);

						if(id.ToLower() != "debian")
						{
							distro = id.Substring(0, id.IndexOf(('\n')));
							string release = s.Remove(0, s.IndexOf("DISTRIB_RELEASE=") + "DISTRIB_RELEASE=".Length);
							distro += " " + release.Substring(0, release.IndexOf(('\n')));
						}
					}
				}

				distro = distro.Replace("\n", string.Empty);
			}
			catch(Exception)
			{

			}

			return distro;
		}
	}
}