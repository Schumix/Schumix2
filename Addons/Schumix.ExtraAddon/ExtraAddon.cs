/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2011 Twl
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
using System.Threading.Tasks;
using Schumix.API;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Extensions;
using Schumix.ExtraAddon.Commands;
using Schumix.ExtraAddon.Config;

namespace Schumix.ExtraAddon
{
	public class ExtraAddon : IrcHandler, ISchumixAddon
	{
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly ChannelInfo sChannelInfo = Singleton<ChannelInfo>.Instance;
		private readonly Functions sFunctions = Singleton<Functions>.Instance;
		private readonly Jegyzet sJegyzet = Singleton<Jegyzet>.Instance;
		private readonly Sender sSender = Singleton<Sender>.Instance;

		public void Setup()
		{
			new AddonConfig(Name + ".xml");
			Network.PublicRegisterHandler("JOIN",               HandleJoin);
			Network.PublicRegisterHandler("PART",               HandleLeft);
			Network.PublicRegisterHandler("KICK",               HandleKick);

			CommandManager.HalfOperatorCRegisterHandler("autofunkcio", sFunctions.HandleAutoFunkcio);
			CommandManager.PublicCRegisterHandler("jegyzet",           sJegyzet.HandleJegyzet);
		}

		public void Destroy()
		{
			Network.PublicRemoveHandler("JOIN");
			Network.PublicRemoveHandler("PART");
			Network.PublicRemoveHandler("KICK");
			CommandManager.HalfOperatorCRemoveHandler("autofunkcio");
			CommandManager.PublicCRemoveHandler("jegyzet");
		}

		public void HandlePrivmsg()
		{
			if(sChannelInfo.FSelect("parancsok") || Network.IMessage.Channel.Substring(0, 1) != "#")
			{
				if(!sChannelInfo.FSelect("parancsok", Network.IMessage.Channel) && Network.IMessage.Channel.Substring(0, 1) == "#")
					return;

				Task.Factory.StartNew(() =>
				{
					if(sFunctions.AutoKick("privmsg", Network.IMessage.Nick, Network.IMessage.Channel))
						return;
				});

				Task.Factory.StartNew(() =>
				{
					if(sChannelInfo.FSelect("mode") && sChannelInfo.FSelect("mode", Network.IMessage.Channel))
					{
						AutoMode = true;
						ModeChannel = Network.IMessage.Channel;
						sSender.NickServStatus(Network.IMessage.Nick);
					}
				});

				Task.Factory.StartNew(() =>
				{
					string channel = Network.IMessage.Channel;
					var urlsin = sUtilities.GetUrls(Network.IMessage.Args);

					if(urlsin.Count <= 0)
						return;

					try
					{
						Parallel.ForEach(urlsin, url => sFunctions.HandleWebTitle(channel, url));
						return;
					}
					catch(Exception e)
					{
						Log.Error("ExtraAddon", "Invalid webpage address: {0}", e.Message);
						return;
					}
				});

				Task.Factory.StartNew(() =>
				{
					sFunctions.HLUzenet(Network.IMessage.Channel, Network.IMessage.Info);
				});
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
					if(!db.IsNull())
					{
						string rang = db["Rank"].ToString();
						sSender.Mode(ModeChannel, rang, Network.IMessage.Info[4]);
					}
					else
					{
						if(ModeConfig.RemoveEnabled)
						{
							if(ModeConfig.RemoveType.Length == 1)
								sSender.Mode(ModeChannel, "-" + ModeConfig.RemoveType, Network.IMessage.Info[4]);
							else if(ModeConfig.RemoveType.Length == 2)
								sSender.Mode(ModeChannel, "-" + ModeConfig.RemoveType, string.Format("{0} {0}", Network.IMessage.Info[4]));
							else if(ModeConfig.RemoveType.Length == 3)
								sSender.Mode(ModeChannel, "-" + ModeConfig.RemoveType, string.Format("{0} {0} {0}", Network.IMessage.Info[4]));
							else if(ModeConfig.RemoveType.Length == 4)
								sSender.Mode(ModeChannel, "-" + ModeConfig.RemoveType, string.Format("{0} {0} {0} {0}", Network.IMessage.Info[4]));
						}
					}
				}

				AutoMode = false;
			}
		}

		public void HandleHelp()
		{
			sFunctions.Help();
		}

		/// <summary>
		/// Name of the addon
		/// </summary>
		public string Name
		{
			get { return "ExtraAddon"; }
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