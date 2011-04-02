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
		private readonly Functions sFunctions = Singleton<Functions>.Instance;
		private readonly Sender sSender = Singleton<Sender>.Instance;

		public void Setup()
		{
			Network.PublicRegisterHandler("JOIN",               HandleJoin);
			Network.PublicRegisterHandler("PART",               HandleLeft);
			Network.PublicRegisterHandler("KICK",               HandleKick);

			CommandManager.AdminCRegisterHandler("autofunkcio", sFunctions.HandleAutoFunkcio);
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

				if(sFunctions.AutoKick("privmsg"))
					return;

				if(Network.sChannelInfo.FSelect("mode") && Network.sChannelInfo.FSelect("mode", Network.IMessage.Channel))
				{
					AutoMode = true;
					ModeChannel = Network.IMessage.Channel;
					sSender.NickServStatus(Network.IMessage.Nick);
				}

				sFunctions.HLUzenet();
			}
		}

		public void HandleNotice()
		{
			if(Network.IMessage.Nick == "NickServ" && AutoMode)
			{
				if(Network.IMessage.Info.Length < 6)
					return;

				if(Network.IMessage.Info[5] == "3")
				{
					var db = SchumixBase.DManager.QueryFirstRow("SELECT Rank FROM modelist WHERE Name = '{0}' AND Channel = '{1}'", Network.IMessage.Info[4].ToLower(), ModeChannel);
					if(db != null)
					{
						string rang = db["Rank"].ToString();
						sSender.Mode(ModeChannel, rang, Network.IMessage.Info[4]);
					}
					else
						sSender.Mode(ModeChannel, "-aohv", string.Format("{0} {0} {0} {0}", Network.IMessage.Info[4]));
				}

				AutoMode = false;
			}
		}

		public void HandleHelp()
		{
			sFunctions.Help();
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