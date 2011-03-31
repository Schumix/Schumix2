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
using Schumix.API;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.ExtraAddon.Commands;

namespace Schumix.ExtraAddon
{
	public class ExtraAddon : IrcHandler, ISchumixAddon
	{
		private Functions _functions;

		public void Setup()
		{
			Network.PublicRegisterHandler("JOIN",               HandleJoin);
			Network.PublicRegisterHandler("PART",               HandleLeft);
			Network.PublicRegisterHandler("KICK",               HandleKick);

			_functions = new Functions();
			CommandManager.AdminCRegisterHandler("autofunkcio", _functions.HandleAutoFunkcio);
		}

		public void Destroy()
		{
			Network.PublicRemoveHandler("JOIN");
			Network.PublicRemoveHandler("PART");
			Network.PublicRemoveHandler("KICK");
			CommandManager.AdminCRemoveHandler("autofunkcio");
		}

		public void HandlePrivmsg()
		{
			if(Network.sChannelInfo.FSelect("parancsok") || Network.IMessage.Channel.Substring(0, 1) != "#")
			{
				if(!Network.sChannelInfo.FSelect("parancsok", Network.IMessage.Channel) && Network.IMessage.Channel.Substring(0, 1) == "#")
					return;

				_functions.HLUzenet();
			}
		}

		public void HandleHelp()
		{
			_functions.Help();
		}

		public string Name
		{
			get
			{
				return "ExtraAddon";
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