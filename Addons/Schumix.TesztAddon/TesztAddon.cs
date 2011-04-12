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
using Schumix.TesztAddon.Commands;

namespace Schumix.TesztAddon
{
	public class TesztAddon : TesztCommand, ISchumixAddon
	{
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;

		public void Setup()
		{
			CommandManager.AdminCRegisterHandler("teszt", Teszt);
		}

		public void Destroy()
		{
			CommandManager.AdminCRemoveHandler("teszt");
		}

		public void HandlePrivmsg()
		{

		}

		public void HandleNotice()
		{

		}

		public void HandleHelp()
		{
			// Adminisztrátor parancsok
			if(IsAdmin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Administrator))
			{
				if(Network.IMessage.Info[4].ToLower() == "teszt")
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Teszt célokra használt parancs.");
				}
			}
		}

		/// <summary>
		/// Name of the addon
		/// </summary>
		public string Name
		{
			get { return "TesztAddon"; }
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