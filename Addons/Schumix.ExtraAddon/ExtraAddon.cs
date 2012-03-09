/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Twl
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
using System.Threading.Tasks;
using Schumix.API;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using Schumix.ExtraAddon.Config;
using Schumix.ExtraAddon.Commands;
using Schumix.ExtraAddon.Localization;

namespace Schumix.ExtraAddon
{
	class ExtraAddon : IrcHandler, ISchumixAddon
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly IgnoreNickName sIgnoreNickName = Singleton<IgnoreNickName>.Instance;
		private readonly PLocalization sLocalization = Singleton<PLocalization>.Instance;
		private readonly ChannelInfo sChannelInfo = Singleton<ChannelInfo>.Instance;
		private readonly Functions sFunctions = Singleton<Functions>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly NickInfo sNickInfo = Singleton<NickInfo>.Instance;
		private readonly NameList sNameList = Singleton<NameList>.Instance;
		private readonly Sender sSender = Singleton<Sender>.Instance;
		private readonly Notes sNotes = Singleton<Notes>.Instance;
#pragma warning disable 414
		private AddonConfig _config;
#pragma warning restore 414
		public static bool IsOnline { get; set; }

		public void Setup()
		{
			// Online
			sFunctions._timeronline.Interval = 10*60*1000;
			sFunctions._timeronline.Elapsed += sFunctions.HandleIsOnline;
			sFunctions._timeronline.Enabled = true;
			sFunctions._timeronline.Start();

			IsOnline = false;
			sNameList.RandomAllVhost();
			sLocalization.Locale = sLConsole.Locale;
			_config = new AddonConfig(Name + ".xml");
			Network.PublicRegisterHandler("PRIVMSG",              HandlePrivmsg);
			Network.PublicRegisterHandler("NOTICE",               HandleNotice);
			Network.PublicRegisterHandler("JOIN",                 HandleJoin);
			Network.PublicRegisterHandler("PART",                 HandleLeft);
			Network.PublicRegisterHandler("KICK",                 HandleKick);
			Network.PublicRegisterHandler("QUIT",                 HandleQuit);
			Network.PublicRegisterHandler("NICK",                 HandleNewNick);
			Network.PublicRegisterHandler(ReplyCode.RPL_NAMREPLY, HandleNameList);
			InitIrcCommand();
		}

		public void Destroy()
		{
			// Online
			sFunctions._timeronline.Enabled = false;
			sFunctions._timeronline.Elapsed -= sFunctions.HandleIsOnline;
			sFunctions._timeronline.Stop();

			Network.PublicRemoveHandler("PRIVMSG",                HandlePrivmsg);
			Network.PublicRemoveHandler("NOTICE",                 HandleNotice);
			Network.PublicRemoveHandler("JOIN",                   HandleJoin);
			Network.PublicRemoveHandler("PART",                   HandleLeft);
			Network.PublicRemoveHandler("KICK",                   HandleKick);
			Network.PublicRemoveHandler("QUIT",                   HandleQuit);
			Network.PublicRemoveHandler("NICK",                   HandleNewNick);
			Network.PublicRemoveHandler(ReplyCode.RPL_NAMREPLY,   HandleNameList);
			RemoveIrcCommand();
			sNameList.RemoveAll();
		}

		public int Reload(string RName, string SName = "")
		{
			try
			{
				switch(RName.ToLower())
				{
					case "config":
						_config = new AddonConfig(Name + ".xml");
						return 1;
					case "command":
						InitIrcCommand();
						RemoveIrcCommand();
						return 1;
				}
			}
			catch(Exception e)
			{
				Log.Error("ExtraAddon", "Reload: " + sLConsole.Exception("Error"), e.Message);
				return 0;
			}

			return -1;
		}

		private void InitIrcCommand()
		{
			CommandManager.PublicCRegisterHandler("notes",              sNotes.HandleNotes);
			CommandManager.PublicCRegisterHandler("message",            sFunctions.HandleMessage);
			CommandManager.PublicCRegisterHandler("weather",            sFunctions.HandleWeather);
			CommandManager.PublicCRegisterHandler("roll",               sFunctions.HandleRoll);
			CommandManager.PublicCRegisterHandler("sha1",               sFunctions.HandleSha1);
			CommandManager.PublicCRegisterHandler("md5",                sFunctions.HandleMd5);
			CommandManager.PublicCRegisterHandler("prime",              sFunctions.HandlePrime);
			CommandManager.PublicCRegisterHandler("wiki",               sFunctions.HandleWiki);
			CommandManager.PublicCRegisterHandler("calc",               sFunctions.HandleCalc);
			CommandManager.HalfOperatorCRegisterHandler("autofunction", sFunctions.HandleAutoFunction);
		}

		private void RemoveIrcCommand()
		{
			CommandManager.PublicCRemoveHandler("notes",                sNotes.HandleNotes);
			CommandManager.PublicCRemoveHandler("message",              sFunctions.HandleMessage);
			CommandManager.PublicCRemoveHandler("weather",              sFunctions.HandleWeather);
			CommandManager.PublicCRemoveHandler("roll",                 sFunctions.HandleRoll);
			CommandManager.PublicCRemoveHandler("sha1",                 sFunctions.HandleSha1);
			CommandManager.PublicCRemoveHandler("md5",                  sFunctions.HandleMd5);
			CommandManager.PublicCRemoveHandler("prime",                sFunctions.HandlePrime);
			CommandManager.PublicCRemoveHandler("wiki",                 sFunctions.HandleWiki);
			CommandManager.PublicCRemoveHandler("calc",                 sFunctions.HandleCalc);
			CommandManager.HalfOperatorCRemoveHandler("autofunction",   sFunctions.HandleAutoFunction);
		}

		private void HandlePrivmsg(IRCMessage sIRCMessage)
		{
			if(sIgnoreNickName.IsIgnore(sIRCMessage.Nick))
				return;

			if(sChannelInfo.FSelect(IFunctions.Commands) || sIRCMessage.Channel.Substring(0, 1) != "#")
			{
				if(!sChannelInfo.FSelect(IChannelFunctions.Commands, sIRCMessage.Channel) && sIRCMessage.Channel.Substring(0, 1) == "#")
					return;

				if(sIRCMessage.Channel.Substring(0, 1) != "#")
					return;

				Task.Factory.StartNew(() =>
				{
					if(sFunctions.AutoKick("privmsg", sIRCMessage.Nick, sIRCMessage.Channel))
						return;
				});

				Task.Factory.StartNew(() =>
				{
					if(sChannelInfo.FSelect(IFunctions.Automode) && sChannelInfo.FSelect(IChannelFunctions.Automode, sIRCMessage.Channel))
					{
						AutoMode = true;
						ModeChannel = sIRCMessage.Channel.ToLower();
						sSender.NickServStatus(sIRCMessage.Nick);
					}
				});

				Task.Factory.StartNew(() =>
				{
					if(sChannelInfo.FSelect(IFunctions.Randomkick) && sChannelInfo.FSelect(IChannelFunctions.Randomkick, sIRCMessage.Channel))
					{
						if(sIRCMessage.Args.IsUpper() && sIRCMessage.Args.Length > 4)
							sSender.Kick(sIRCMessage.Channel, sIRCMessage.Nick, sLManager.GetWarningText("CapsLockOff", sIRCMessage.Channel));
					}
				});

				Task.Factory.StartNew(() => sFunctions.HLMessage(sIRCMessage));
				Task.Factory.StartNew(() => sFunctions.Message(sIRCMessage));

				Task.Factory.StartNew(() =>
				{
					if(sChannelInfo.FSelect(IFunctions.Webtitle) && sChannelInfo.FSelect(IChannelFunctions.Webtitle, sIRCMessage.Channel))
					{
						if(!SchumixBase.UrlTitleEnabled)
							return;

						if(sIRCMessage.Nick.ToLower() == "py-bopm")
							return;

						var urlsin = sUtilities.GetUrls(sIRCMessage.Args);

						if(urlsin.Count <= 0)
							return;

						try
						{
							Parallel.ForEach(urlsin, url => sFunctions.HandleWebTitle(sIRCMessage, url));
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

		private void HandleNotice(IRCMessage sIRCMessage)
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

			if(sIRCMessage.Nick == "NickServ" && IsOnline)
			{
				if(sIRCMessage.Args.Contains("isn't registered.") || sIRCMessage.Args.Contains("   Last seen time:"))
				{
					sNameList.Change(sNickInfo.NickStorage, IRCConfig.NickName, true);
					sNickInfo.ChangeNick(IRCConfig.NickName);
					sSender.Nick(IRCConfig.NickName);
					Log.Notice("NickServ", sLConsole.NickServ("Text"));
					sSender.NickServ(IRCConfig.NickServPassword);
					MessageHandler.NewNick = false;
		
					if(IRCConfig.UseHostServ)
					{
						MessageHandler.HostServStatus = true;
						sSender.HostServ("on");
						Log.Notice("HostServ", sLConsole.HostServ("Text"));
					}

					IsOnline = false;
				}
			}
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