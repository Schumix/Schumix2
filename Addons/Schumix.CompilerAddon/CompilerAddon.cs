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
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Schumix.API;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using Schumix.CompilerAddon.Commands;
using Schumix.CompilerAddon.Config;

namespace Schumix.CompilerAddon
{
	public class CompilerAddon : Compiler, ISchumixAddon
	{
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly ChannelInfo sChannelInfo = Singleton<ChannelInfo>.Instance;
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;
		private readonly Regex regex = new Regex(@"^\{(?<code>.*)\}$");

		public void Setup()
		{
			new AddonConfig(Name + ".xml");
		}

		public void Destroy()
		{

		}

		public void HandlePrivmsg(IRCMessage sIRCMessage)
		{
			if(sChannelInfo.FSelect("commands") || sIRCMessage.Channel.Substring(0, 1) != "#")
			{
				if(!sChannelInfo.FSelect("commands", sIRCMessage.Channel) && sIRCMessage.Channel.Substring(0, 1) == "#")
					return;

				if(!CompilerConfig.CompilerEnabled)
					return;

				string command = IRCConfig.NickName + ",";
				sIRCMessage.Info[3] = sIRCMessage.Info[3].Remove(0, 1, ":");

				if(sIRCMessage.Info[3].ToLower() == command.ToLower() && Enabled(sIRCMessage.Channel) && sIRCMessage.Args.Contains(";"))
					Compiler(sIRCMessage, true, command);

				if((sChannelInfo.FSelect("compiler") && sChannelInfo.FSelect("compiler", sIRCMessage.Channel)) &&
					(regex.IsMatch(sIRCMessage.Args.TrimEnd()) && Enabled(sIRCMessage.Channel)))
					Compiler(sIRCMessage, false, command);
			}
		}

		public void HandleNotice(IRCMessage sIRCMessage)
		{

		}

		public bool HandleHelp(IRCMessage sIRCMessage)
		{
			return false;
		}

		private bool Enabled(string channel)
		{
			if(CompilerConfig.MaxAllocatingE)
			{
				var memory = Process.GetCurrentProcess().WorkingSet64/1024/1024;

				if(memory > CompilerConfig.MaxAllocatingM)
				{
					sSendMessage.SendCMPrivmsg(channel, sLManager.GetCommandText("compiler/memory", channel));
					return false;
				}
			}

			return true;
		}

		private void Compiler(IRCMessage sIRCMessage, bool command, string args)
		{
			bool b = false;

			if(command)
			{
				sIRCMessage.Args = sIRCMessage.Args.Remove(0, args.Length);
				var thread = new Thread(() => b = CompilerCommand(sIRCMessage, true));
				thread.Start();
				thread.Join(2000);
				thread.Abort();
			}
			else
			{
				var thread = new Thread(() => b = CompilerCommand(sIRCMessage, false));
				thread.Start();
				thread.Join(2000);
				thread.Abort();
			}

			// TODO: Sql-be rakni a szöveget.
			if(!b)
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Killed thread!");

			var sw = new StreamWriter(Console.OpenStandardOutput());
			sw.AutoFlush = true;
			Console.SetOut(sw);
		}

		/// <summary>
		/// Name of the addon
		/// </summary>
		public string Name
		{
			get { return "CompilerAddon"; }
		}

		/// <summary>
		/// Author of the addon.
		/// </summary>
		public string Author
		{
			get { return "Megax"; }
		}

		/// <summary>
		/// Website where the addon is available.
		/// </summary>
		public string Website
		{
			get { return "http://www.github.com/megax/Schumix2"; }
		}
	}
}