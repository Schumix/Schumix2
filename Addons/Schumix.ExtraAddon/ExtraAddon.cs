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
using Schumix.Framework.Localization;
using Schumix.ExtraAddon.Config;
using Schumix.ExtraAddon.Commands;
using Schumix.ExtraAddon.Localization;

namespace Schumix.ExtraAddon
{
	public class ExtraAddon : IrcHandler, ISchumixAddon
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly PLocalization sLocalization = Singleton<PLocalization>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly ChannelInfo sChannelInfo = Singleton<ChannelInfo>.Instance;
		private readonly Functions sFunctions = Singleton<Functions>.Instance;
		private readonly Notes sNotes = Singleton<Notes>.Instance;
		private readonly Sender sSender = Singleton<Sender>.Instance;
#if MONO
#pragma warning disable 414
		private AddonConfig _config;
#pragma warning restore 414
#else
		private AddonConfig _config;
#endif

		public void Setup()
		{
			sLocalization.Locale = sLConsole.Locale;
			_config = new AddonConfig(Name + ".xml");
			Network.PublicRegisterHandler("JOIN",                       new Action<IRCMessage>(HandleJoin));
			CommandManager.PublicCRegisterHandler("notes",              new Action<IRCMessage>(sNotes.HandleNotes));
			CommandManager.PublicCRegisterHandler("message",            new Action<IRCMessage>(sFunctions.HandleMessage));
			CommandManager.PublicCRegisterHandler("weather",            new Action<IRCMessage>(sFunctions.HandleWeather));
			CommandManager.PublicCRegisterHandler("roll",               new Action<IRCMessage>(sFunctions.HandleRoll));
			CommandManager.PublicCRegisterHandler("sha1",               new Action<IRCMessage>(sFunctions.HandleSha1));
			CommandManager.PublicCRegisterHandler("md5",                new Action<IRCMessage>(sFunctions.HandleMd5));
			CommandManager.PublicCRegisterHandler("prime",              new Action<IRCMessage>(sFunctions.HandlePrime));
			CommandManager.HalfOperatorCRegisterHandler("autofunction", new Action<IRCMessage>(sFunctions.HandleAutoFunction));
		}

		public void Destroy()
		{
			Network.PublicRemoveHandler("JOIN");
			CommandManager.PublicCRemoveHandler("notes");
			CommandManager.PublicCRemoveHandler("message");
			CommandManager.PublicCRemoveHandler("weather");
			CommandManager.PublicCRemoveHandler("roll");
			CommandManager.PublicCRemoveHandler("sha1");
			CommandManager.PublicCRemoveHandler("md5");
			CommandManager.PublicCRemoveHandler("prime");
			CommandManager.HalfOperatorCRemoveHandler("autofunction");
		}

		public bool Reload(string RName)
		{
			switch(RName.ToLower())
			{
				case "config":
					_config = new AddonConfig(Name + ".xml");
					return true;
			}

			return false;
		}

		public void HandlePrivmsg(IRCMessage sIRCMessage)
		{
			if(sChannelInfo.FSelect("commands") || sIRCMessage.Channel.Substring(0, 1) != "#")
			{
				if(!sChannelInfo.FSelect("commands", sIRCMessage.Channel) && sIRCMessage.Channel.Substring(0, 1) == "#")
					return;

				Task.Factory.StartNew(() =>
				{
					if(sFunctions.AutoKick("privmsg", sIRCMessage.Nick, sIRCMessage.Channel))
						return;
				});

				Task.Factory.StartNew(() =>
				{
					if(sChannelInfo.FSelect("automode") && sChannelInfo.FSelect("automode", sIRCMessage.Channel))
					{
						AutoMode = true;
						ModeChannel = sIRCMessage.Channel.ToLower();
						sSender.NickServStatus(sIRCMessage.Nick);
					}
				});

				Task.Factory.StartNew(() =>
				{
					if(sChannelInfo.FSelect("randomkick") && sChannelInfo.FSelect("randomkick", sIRCMessage.Channel))
					{
						if(sIRCMessage.Args.IsUpper() && sIRCMessage.Args.Length > 4)
							sSender.Kick(sIRCMessage.Channel, sIRCMessage.Nick, sLManager.GetWarningText("CapsLockOff", sIRCMessage.Channel));
					}
				});

				Task.Factory.StartNew(() => sFunctions.HLMessage(sIRCMessage.Channel, sIRCMessage.Args));
				Task.Factory.StartNew(() => sFunctions.Message(sIRCMessage.Nick, sIRCMessage.Channel));

				Task.Factory.StartNew(() =>
				{
					if(sChannelInfo.FSelect("webtitle") && sChannelInfo.FSelect("webtitle", sIRCMessage.Channel))
					{
						if(!SchumixBase.UrlTitleEnabled)
							return;

						string channel = sIRCMessage.Channel;

						if(sIRCMessage.Nick.ToLower() == "py-bopm")
							return;

						var urlsin = sUtilities.GetUrls(sIRCMessage.Args);

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
					}
				});
			}
		}

		public void HandleNotice(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Nick == "NickServ" && AutoMode)
			{
				if(sIRCMessage.Info.Length < 6)
					return;

				if(sIRCMessage.Info[5] == "3")
				{
					var db = SchumixBase.DManager.QueryFirstRow("SELECT Rank FROM modelist WHERE Name = '{0}' AND Channel = '{1}'", sIRCMessage.Info[4].ToLower(), ModeChannel);
					if(!db.IsNull())
					{
						string rank = db["Rank"].ToString();
						sSender.Mode(ModeChannel, rank, sIRCMessage.Info[4]);
					}
					else
					{
						if(ModeConfig.RemoveEnabled)
						{
							if(ModeConfig.RemoveType.Length == 1)
								sSender.Mode(ModeChannel, "-" + ModeConfig.RemoveType, sIRCMessage.Info[4]);
							else if(ModeConfig.RemoveType.Length == 2)
								sSender.Mode(ModeChannel, "-" + ModeConfig.RemoveType, string.Format("{0} {0}", sIRCMessage.Info[4]));
							else if(ModeConfig.RemoveType.Length == 3)
								sSender.Mode(ModeChannel, "-" + ModeConfig.RemoveType, string.Format("{0} {0} {0}", sIRCMessage.Info[4]));
							else if(ModeConfig.RemoveType.Length == 4)
								sSender.Mode(ModeChannel, "-" + ModeConfig.RemoveType, string.Format("{0} {0} {0} {0}", sIRCMessage.Info[4]));
						}
					}
				}

				AutoMode = false;
			}
		}

		public void HandleLeft(IRCMessage sIRCMessage)
		{
			HandleLLeft(sIRCMessage);
		}

		public void HandleKick(IRCMessage sIRCMessage)
		{
			HandleKKick(sIRCMessage);
		}

		public bool HandleHelp(IRCMessage sIRCMessage)
		{
			return sFunctions.Help(sIRCMessage);
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