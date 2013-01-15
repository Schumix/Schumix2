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
using Schumix.Framework.Localization;
using Schumix.TestAddon.Commands;

namespace Schumix.TestAddon
{
	class TestAddon : ISchumixAddon
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private TestCommand sTestCommand;
		private string _servername;

		public void Setup(string ServerName)
		{
			_servername = ServerName;
			sTestCommand = new TestCommand(ServerName);
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
				Log.Error("TestAddon", "Reload: " + sLConsole.Exception("Error"), e.Message);
				return 0;
			}

			return -1;
		}

		private void InitIrcCommand()
		{
			sIrcBase.Networks[_servername].SchumixRegisterHandler("test", sTestCommand.HandleTest, CommandPermission.Administrator);
		}

		private void RemoveIrcCommand()
		{
			sIrcBase.Networks[_servername].SchumixRemoveHandler("test",   sTestCommand.HandleTest);
		}

		public bool HandleHelp(IRCMessage sIRCMessage)
		{
			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;

			// Adminisztrátor parancsok
			if(sTestCommand.IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Administrator))
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
			get { return "TestAddon"; }
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