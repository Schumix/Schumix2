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
using System.Threading;
using System.Text.RegularExpressions;
using Schumix.API;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.CompilerAddon.Commands;

namespace Schumix.CompilerAddon
{
	public class CompilerAddon : Compiler, ISchumixAddon
	{
		private readonly Regex regex = new Regex(@"^\{(?<code>.+)\}$");
		private readonly Regex regex2 = new Regex(@"^\{(?<code>.+)\}.+$");

		public void Setup()
		{

		}

		public void Destroy()
		{

		}

		public void HandlePrivmsg()
		{
			if(Network.sChannelInfo.FSelect("parancsok") || Network.IMessage.Channel.Substring(0, 1) != "#")
			{
				if(!Network.sChannelInfo.FSelect("parancsok", Network.IMessage.Channel) && Network.IMessage.Channel.Substring(0, 1) == "#")
					return;

				if(regex.IsMatch(Network.IMessage.Args))
				{
					var thread = new Thread(CompilerCommand);
					thread.Start();
					thread.Join(1000);
					thread.Abort();
				}
				else if(regex2.IsMatch(Network.IMessage.Args))
				{
					var thread = new Thread(CompilerCommand);
					thread.Start();
					thread.Join(1000);
					thread.Abort();
				}
			}
		}

		public void HandleNotice()
		{

		}

		public void HandleHelp()
		{

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