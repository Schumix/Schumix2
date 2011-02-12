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
using System.Text.RegularExpressions;
using Schumix.API;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.CompilerPlugin.Commands;

namespace Schumix.CompilerPlugin
{
	public class SchumixPlugin : Compiler, ISchumixBase
	{
		private readonly Regex regex = new Regex(@"\{(?<code>.+)\}$");
		private int PLength = IRCConfig.Parancselojel.Length;

		public SchumixPlugin()
		{

		}

		~SchumixPlugin()
		{
			Log.Debug("CompilerPlugin", "~SchumixPlugin()");
		}

		public void Setup()
		{

		}

		public void Destroy()
		{

		}

		public void HandlePrivmsg()
		{
			if(Network.sChannelInfo.FSelect("parancsok") == "be")
			{
				//if(Network.sChannelInfo.FSelect("parancsok", Network.IMessage.Channel) != "be" && Network.IMessage.Channel.Substring(0, 1) != "#")
					//return;

				if(Network.IMessage.Info[3].Substring(0, 1) == ":")
					Network.IMessage.Info[3] = Network.IMessage.Info[3].Remove(0, 1);

				if(Network.IMessage.Info[Network.IMessage.Info.Length-2] == "" || Network.IMessage.Info[Network.IMessage.Info.Length-1] == "")
					return;

				if(Network.IMessage.Info[3] == "" || Network.IMessage.Info[3].Substring(0, PLength) == " " || Network.IMessage.Info[3].Substring(0, PLength) != IRCConfig.Parancselojel)
				{
					if(regex.IsMatch(Network.IMessage.Args))
						CompilerCommand();
				}
			}
		}

		public void HandleHelp()
		{

		}

		public string Name
		{
			get
			{
				return "CompilerPlugin";
			}
		}

		public string Author
		{
			get
			{
				return "Megax";
			}
		}

		public string Website
		{
			get
			{
				return "http://www.github.com/megax/Schumix2";
			}
		}
	}
}