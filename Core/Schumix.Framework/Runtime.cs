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
using System.Text;
using System.Runtime.InteropServices;
using Schumix.Framework.Localization;

namespace Schumix.Framework
{
	public sealed class Runtime
	{
		private static readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;

		public static void SetProcessName(string Name)
		{
#if MONO
			if(Environment.OSVersion.Platform == PlatformID.Unix)
			{
				try
				{
					unixSetProcessName(Name);
				}
				catch(Exception e)
				{
					Log.Error("Runtime", sLConsole.Runtime("Text"), e.Message);
				}
#endif
			}
		}

#if MONO
		[DllImport ("libc")] // Linux
		private static extern int prctl(int option, byte[] arg2, IntPtr arg3, IntPtr arg4, IntPtr arg5);

		[DllImport ("libc")] // BSD
		private static extern void setproctitle(byte[] fmt, byte[] str_arg);

		//this is from http://abock.org/2006/02/09/changing-process-name-in-mono/
		private static void unixSetProcessName(string Name)
		{
			try
			{
				if(prctl(15, Encoding.ASCII.GetBytes(Name + "\0"), IntPtr.Zero, IntPtr.Zero, IntPtr.Zero) != 0)
					Log.Error("Runtime", sLConsole.Runtime("Text2"));
			}
			catch(EntryPointNotFoundException)
			{
				try
				{
					// Not every BSD has setproctitle
					setproctitle(Encoding.ASCII.GetBytes("%s\0"), Encoding.ASCII.GetBytes(Name + "\0"));
				}
				catch(EntryPointNotFoundException)
				{

				}
      		}
		}
#endif
	}
}