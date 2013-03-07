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
using Schumix.Api;
using Schumix.Api.Irc;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.RevisionAddon.Commands;

namespace Schumix.RevisionAddon
{
	class RevisionAddon : ISchumixAddon
	{
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private Revision sRevision;
		private string _servername;

		public void Setup(string ServerName, bool LoadConfig = false)
		{
			_servername = ServerName;
			sRevision = new Revision(ServerName);
			sIrcBase.Networks[_servername].SchumixRegisterHandler("xrev", sRevision.HandleXrev);
		}

		public void Destroy()
		{
			sIrcBase.Networks[_servername].SchumixRemoveHandler("xrev",   sRevision.HandleXrev);
		}

		public int Reload(string RName, string SName = "")
		{
			return -1;
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