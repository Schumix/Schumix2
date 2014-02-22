/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013-2014 Schumix Team <http://schumix.eu/>
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
using Schumix.Irc.Util;
using Schumix.Irc.Channel;
using Schumix.Framework;
using Schumix.Framework.Util;
using Schumix.Framework.Config;
using Schumix.Framework.Functions;

namespace Schumix.Irc.Logger
{
	public sealed class IrcLog
	{
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private readonly object WriteLock = new object();
		private string _servername;

		public IrcLog(string ServerName)
		{
			_servername = ServerName;
		}

		/// <summary>
		///     Logolja a csatornára kiírt üzeneteket. stb.
		/// </summary>
		/// <param name="channel"></param>
		/// <param name="user"></param>
		/// <param name="args"></param>
		public void LogInFile(string channel, string user, string args)
		{
			lock(WriteLock)
			{
				var sMyChannelInfo = sIrcBase.Networks[_servername].sMyChannelInfo;

				if((sMyChannelInfo.FSelect(IFunctions.Log) && sMyChannelInfo.FSelect(IChannelFunctions.Log, channel)) || !Rfc2812Util.IsValidChannelName(channel))
				{
					if(!Rfc2812Util.IsValidChannelName(channel))
					{
						string targs = args.ToLower();

						if((targs.Contains("admin") && targs.Contains("access")) ||
						   (targs.Contains("admin") && targs.Contains("newpassword")) ||
						   (targs.Contains("notes") && targs.Contains("user") && targs.Contains("access")) ||
						   (targs.Contains("notes") && targs.Contains("user") && targs.Contains("register")) ||
						   (targs.Contains("notes") && targs.Contains("user") && targs.Contains("remove")) ||
						   (targs.Contains("notes") && targs.Contains("user") && targs.Contains("newpassword")))
							return;
					}

					string dir = LogConfig.IrcLogDirectory + "/" + _servername;
					sUtilities.CreateDirectory(dir);

					string logdir = Path.Combine(dir, channel);
					string logfile = string.Format("{0}/{1}.log", logdir, DateTime.Now.ToString("yyyy-MM-dd"));

					sUtilities.CreateDirectory(logdir);
					sUtilities.CreateFile(logfile);

					var file = new StreamWriter(logfile, true) { AutoFlush = true };
					file.WriteLine("[{0}] <{1}> {2}", DateTime.Now.ToString("HH:mm:ss"), user, args);
					file.Close();
				}
			}
		}

		public void LogInFile(string channel, string user, string format, params object[] args)
		{
			lock(WriteLock)
			{
				LogInFile(channel, user, string.Format(format, args));
			}
		}
	}
}