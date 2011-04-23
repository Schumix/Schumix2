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
using System.Diagnostics;
using System.Text.RegularExpressions;
using Schumix.API;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.CompilerAddon.Commands;
using Schumix.CompilerAddon.Config;

namespace Schumix.CompilerAddon
{
	public class CompilerAddon : Compiler, ISchumixAddon
	{
		private readonly ChannelInfo sChannelInfo = Singleton<ChannelInfo>.Instance;
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;
		private readonly Regex regex = new Regex(@"^\{(?<code>.+)\}$");
		private readonly Regex regex2 = new Regex(@"^\{(?<code>.+)\}.+$");

		public void Setup()
		{
			new AddonConfig(Name + ".xml");
		}

		public void Destroy()
		{

		}

		public void HandlePrivmsg()
		{
			if(sChannelInfo.FSelect("parancsok") || Network.IMessage.Channel.Substring(0, 1) != "#")
			{
				if(!sChannelInfo.FSelect("parancsok", Network.IMessage.Channel) && Network.IMessage.Channel.Substring(0, 1) == "#")
					return;

				if(!CompilerConfig.CompilerEnabled)
					return;

				if(regex.IsMatch(Network.IMessage.Args) && Enabled())
				{
					var thread = new Thread(CompilerCommand);
					thread.Start();
					thread.Join(1000);
					thread.Abort();

					var sw = new StreamWriter(Console.OpenStandardOutput());
					sw.AutoFlush = true;
					Console.SetOut(sw);
				}
				else if(regex2.IsMatch(Network.IMessage.Args) && Enabled())
				{
					var thread = new Thread(CompilerCommand);
					thread.Start();
					thread.Join(1000);
					thread.Abort();

					var sw = new StreamWriter(Console.OpenStandardOutput());
					sw.AutoFlush = true;
					Console.SetOut(sw);
				}
			}
		}

		public void HandleNotice()
		{

		}

		public void HandleHelp()
		{

		}

		private bool Enabled()
		{
			if(CompilerConfig.MaxAllocatingE)
			{
				var memory = Process.GetCurrentProcess().WorkingSet64/1024/1024;

				if(memory > CompilerConfig.MaxAllocatingM)
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Jelenleg túl sok memóriát fogyaszt a bot ezért ezen funkció nem elérhető!");
					return false;
				}
			}

			return true;
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