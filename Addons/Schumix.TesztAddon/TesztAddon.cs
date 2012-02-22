/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Megax <http://www.megaxx.info/>
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
using Schumix.Framework.Config;
using Schumix.Framework.Localization;
using Schumix.TesztAddon.Commands;

namespace Schumix.TesztAddon
{
	class TesztAddon : TesztCommand, ISchumixAddon
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;

		public void Setup()
		{
			InitIrcCommand();
		}

		public void Destroy()
		{
			RemoveIrcCommand();
		}

		public int Reload(string RName, string SName = "")
		{
			try
			{
				switch(RName.ToLower())
				{
					case "command":
						InitIrcCommand();
						RemoveIrcCommand();
						return 1;
				}
			}
			catch(Exception e)
			{
				Log.Error("TesztAddon", "Reload: " + sLConsole.Exception("Error"), e.Message);
				return 0;
			}

			return -1;
		}

		private void InitIrcCommand()
		{
			CommandManager.AdminCRegisterHandler("test", HandleTest);
		}

		private void RemoveIrcCommand()
		{
			CommandManager.AdminCRemoveHandler("test",   HandleTest);
		}

		public bool HandleHelp(IRCMessage sIRCMessage)
		{
			// Adminisztrátor parancsok
			if(IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Administrator))
			{
				if(sIRCMessage.Info[4].ToLower() == "test")
				{
					sSendMessage.SendChatMessage(sIRCMessage, "Teszt célokra használt parancs.");
					return true;
				}
			}

			return false;
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