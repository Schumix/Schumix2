/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013 Schumix Team <http://schumix.eu/>
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
using Schumix.Config.Extensions;

namespace Schumix.Config.Platforms
{
	sealed class Platform
	{
		public bool IsWindows
		{
			get
			{
				var platform = Environment.OSVersion.Platform;
				return (platform == PlatformID.Win32NT || platform == PlatformID.Win32S ||
				        platform == PlatformID.Win32Windows || platform == PlatformID.WinCE);
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
				case PlatformID.Win32NT:
				case PlatformID.Win32S:
				case PlatformID.Win32Windows:
				case PlatformID.WinCE:
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
				case PlatformID.Win32Windows:
				case PlatformID.Win32NT:
				{
					var osname = (from x in new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem").Get().OfType<ManagementObject>() select x.GetPropertyValue("Caption")).First();
					Name = !osname.IsNull() ? osname.ToString() : "Unknown";
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

					Name = "Linux " + Info.Version;
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
	}
}