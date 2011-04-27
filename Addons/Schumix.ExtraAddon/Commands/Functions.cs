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
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Extensions;

namespace Schumix.ExtraAddon.Commands
{
	public partial class Functions : CommandInfo
	{
		public void HLUzenet()
		{
			if(sChannelInfo.FSelect("hl") && sChannelInfo.FSelect("hl", Network.IMessage.Channel))
			{
				for(int i = 3; i < Network.IMessage.Info.Length; i++)
				{
					if(i == 3)
						Network.IMessage.Info[3] = Network.IMessage.Info[3].Remove(0, 1, ":");

					var db = SchumixBase.DManager.QueryFirstRow("SELECT Info, Enabled FROM hlmessage WHERE Name = '{0}'", Network.IMessage.Info[i].ToLower());
					if(!db.IsNull())
					{
						string info = db["Info"].ToString();
						string allapot = db["Enabled"].ToString();

						if(allapot != "be")
							return;

						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "{0}", info);
						break;
					}
				}
			}
		}

		public bool AutoKick(string allapot)
		{
			if(allapot == "join")
			{
				string channel = Network.IMessage.Channel.Remove(0, 1, ":");

				if(sChannelInfo.FSelect("kick") && sChannelInfo.FSelect("kick", channel))
				{
					var db = SchumixBase.DManager.QueryFirstRow("SELECT Reason FROM kicklist WHERE Name = '{0}'", Network.IMessage.Nick.ToLower());
					if(!db.IsNull())
					{
						string oka = db["Reason"].ToString();
						sSender.Kick(channel, Network.IMessage.Nick, oka);
						return true;
					}
				}

				return false;
			}

			if(allapot == "privmsg")
			{
				if(sChannelInfo.FSelect("kick") && sChannelInfo.FSelect("kick", Network.IMessage.Channel))
				{
					var db = SchumixBase.DManager.QueryFirstRow("SELECT Reason FROM kicklist WHERE Name = '{0}'", Network.IMessage.Nick.ToLower());
					if(!db.IsNull())
					{
						string oka = db["Reason"].ToString();
						sSender.Kick(Network.IMessage.Channel, Network.IMessage.Nick, oka);
						return true;
					}
				}

				return false;
			}

			return false;
		}
	}
}