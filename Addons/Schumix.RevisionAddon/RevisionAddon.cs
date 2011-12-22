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
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Irc.Commands;
using Schumix.RevisionAddon.Commands;

namespace Schumix.RevisionAddon
{
	class RevisionAddon : Revision, ISchumixAddon
	{
		public void Setup()
		{
			CommandManager.PublicCRegisterHandler("xrev", new Action<IRCMessage>(HandleXrev));
		}

		public void Destroy()
		{
			CommandManager.PublicCRemoveHandler("xrev");
		}

		public bool Reload(string RName)
		{
			return false;
		}

		public void HandlePrivmsg(IRCMessage sIRCMessage)
		{

		}

		public void HandleNotice(IRCMessage sIRCMessage)
		{

		}

		public void HandleLeft(IRCMessage sIRCMessage)
		{

		}

		public void HandleKick(IRCMessage sIRCMessage)
		{

		}

		public void HandleQuit(IRCMessage sIRCMessage)
		{

		}

		public void HandleNewNick(IRCMessage sIRCMessage)
		{

		}

		public bool HandleHelp(IRCMessage sIRCMessage)
		{
			return false;
		}

		/// <summary>
		/// Name of the addon
		/// </summary>
		public string Name
		{
			get { return "RevisionAddon"; }
		}

		/// <summary>
		/// Author of the addon.
		/// </summary>
		public string Author
		{
			get { return Consts.SchumixProgrammedBy; }
		}

		/// <summary>
		/// Website where the addon is available.
		/// </summary>
		public string Website
		{
			get { return Consts.SchumixWebsite; }
		}
	}
}