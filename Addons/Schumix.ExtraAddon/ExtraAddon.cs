/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Twl
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
using System.Data;
using System.Threading.Tasks;
using Schumix.Api;
using Schumix.Api.Irc;
using Schumix.Api.Functions;
using Schumix.Irc;
using Schumix.Irc.Util;
using Schumix.Irc.Ignore;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using Schumix.ExtraAddon.Config;
using Schumix.ExtraAddon.Commands;

namespace Schumix.ExtraAddon
{
	class ExtraAddon : ISchumixAddon
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private IrcHandler sIrcHandler;
		private Functions sFunctions;
		private Notes sNotes;
#pragma warning disable 414
		private AddonConfig _config;
#pragma warning restore 414
		private string _servername;

		public void Setup(string ServerName, bool LoadConfig = false)
		{
			_servername = ServerName;

			if(CleanConfig.Database)
			{
				Log.Debug("ExtraAddon", sLConsole.GetString("The deleting of messages that older than 30 days have been started."));

				var db = SchumixBase.DManager.Query("SELECT Id, UnixTime FROM message WHERE ServerName = '{0}'", ServerName);
				if(!db.IsNull())
				{
					foreach(DataRow row in db.Rows)
					{
						int time = Convert.ToInt32(row["UnixTime"].ToString());

						if((DateTime.Now - sUtilities.GetDateTimeFromUnixTime(time)).TotalDays > 30)
							SchumixBase.DManager.Delete("message", string.Format("Id = '{0}'", row["Id"].ToString()));
					}
				}

				Log.Debug("ExtraAddon", sLConsole.GetString("Message deletion has been ended."));
			}

			SchumixBase.DManager.Update("hlmessage", string.Format("ServerName = '{0}'", ServerName), string.Format("ServerId = '{0}'", IRCConfig.List[ServerName].ServerId));
			SchumixBase.DManager.Update("kicklist", string.Format("ServerName = '{0}'", ServerName), string.Format("ServerId = '{0}'", IRCConfig.List[ServerName].ServerId));
			SchumixBase.DManager.Update("modelist", string.Format("ServerName = '{0}'", ServerName), string.Format("ServerId = '{0}'", IRCConfig.List[ServerName].ServerId));
			SchumixBase.DManager.Update("message", string.Format("ServerName = '{0}'", ServerName), string.Format("ServerId = '{0}'", IRCConfig.List[ServerName].ServerId));
			SchumixBase.DManager.Update("notes", string.Format("ServerName = '{0}'", ServerName), string.Format("ServerId = '{0}'", IRCConfig.List[ServerName].ServerId));
			SchumixBase.DManager.Update("notes_users", string.Format("ServerName = '{0}'", ServerName), string.Format("ServerId = '{0}'", IRCConfig.List[ServerName].ServerId));

			if(CleanConfig.Database)
			{
				SchumixBase.sCleanManager.CDatabase.CleanTable("hlmessage");
				SchumixBase.sCleanManager.CDatabase.CleanTable("kicklist");
				SchumixBase.sCleanManager.CDatabase.CleanTable("modelist");
				SchumixBase.sCleanManager.CDatabase.CleanTable("message");
				SchumixBase.sCleanManager.CDatabase.CleanTable("notes");
				SchumixBase.sCleanManager.CDatabase.CleanTable("notes_users");
			}

			// Online
			sFunctions = new Functions(ServerName);
			sFunctions._timeronline.Interval = 10*60*1000;
			sFunctions._timeronline.Elapsed += sFunctions.HandleIsOnline;
			sFunctions._timeronline.Enabled = true;
			sFunctions._timeronline.Start();

			sIrcHandler = new IrcHandler(ServerName, sFunctions);
			sNotes = new Notes(ServerName, sIrcHandler.sNameList);

			sFunctions.IsOnline = false;
			sIrcHandler.sNameList.RandomAllVhost();

			if(IRCConfig.List[_servername].ServerId == 1 || LoadConfig)
				_config = new AddonConfig(Name, ".yml");

			sIrcBase.Networks[ServerName].IrcRegisterHandler("PRIVMSG",                HandlePrivmsg);
			sIrcBase.Networks[ServerName].IrcRegisterHandler("NOTICE",                 HandleNotice);
			sIrcBase.Networks[ServerName].IrcRegisterHandler("JOIN",                   sIrcHandler.HandleJoin);
			sIrcBase.Networks[ServerName].IrcRegisterHandler("PART",                   sIrcHandler.HandleLeft);
			sIrcBase.Networks[ServerName].IrcRegisterHandler("KICK",                   sIrcHandler.HandleKick);
			sIrcBase.Networks[ServerName].IrcRegisterHandler("QUIT",                   sIrcHandler.HandleQuit);
			sIrcBase.Networks[ServerName].IrcRegisterHandler("NICK",                   sIrcHandler.HandleNewNick);
			sIrcBase.Networks[ServerName].IrcRegisterHandler(ReplyCode.RPL_NAMREPLY,   sIrcHandler.HandleNameList);
			sIrcBase.Networks[ServerName].IrcRegisterHandler(ReplyCode.RPL_ENDOFNAMES, sIrcHandler.HandleEndNameList);
			sIrcBase.Networks[ServerName].IrcRegisterHandler(ReplyCode.RPL_WELCOME,    sIrcHandler.HandleSuccessfulAuth);
			InitIrcCommand();
		}

		public void Destroy()
		{
			// Online
			sFunctions._timeronline.Enabled = false;
			sFunctions._timeronline.Elapsed -= sFunctions.HandleIsOnline;
			sFunctions._timeronline.Stop();

			sIrcBase.Networks[_servername].IrcRemoveHandler("PRIVMSG",                HandlePrivmsg);
			sIrcBase.Networks[_servername].IrcRemoveHandler("NOTICE",                 HandleNotice);
			sIrcBase.Networks[_servername].IrcRemoveHandler("JOIN",                   sIrcHandler.HandleJoin);
			sIrcBase.Networks[_servername].IrcRemoveHandler("PART",                   sIrcHandler.HandleLeft);
			sIrcBase.Networks[_servername].IrcRemoveHandler("KICK",                   sIrcHandler.HandleKick);
			sIrcBase.Networks[_servername].IrcRemoveHandler("QUIT",                   sIrcHandler.HandleQuit);
			sIrcBase.Networks[_servername].IrcRemoveHandler("NICK",                   sIrcHandler.HandleNewNick);
			sIrcBase.Networks[_servername].IrcRemoveHandler(ReplyCode.RPL_NAMREPLY,   sIrcHandler.HandleNameList);
			sIrcBase.Networks[_servername].IrcRemoveHandler(ReplyCode.RPL_ENDOFNAMES, sIrcHandler.HandleEndNameList);
			sIrcBase.Networks[_servername].IrcRemoveHandler(ReplyCode.RPL_WELCOME,    sIrcHandler.HandleSuccessfulAuth);
			RemoveIrcCommand();
			sIrcHandler.sNameList.RemoveAll();
		}

		public int Reload(string RName, string SName = "")
		{
			try
			{
				switch(RName.ToLower())
				{
					case "config":
						if(IRCConfig.List[_servername].ServerId == 1)
							_config = new AddonConfig(Name, ".yml");
						return 1;
					case "command":
						InitIrcCommand();
						RemoveIrcCommand();
						return 1;
				}
			}
			catch(Exception e)
			{
				Log.Error("ExtraAddon", "Reload: " + sLConsole.GetString("Failure details: {0}"), e.Message);
				return 0;
			}

			return -1;
		}

		private void InitIrcCommand()
		{
			sIrcBase.Networks[_servername].SchumixRegisterHandler("notes",        sNotes.HandleNotes);
			sIrcBase.Networks[_servername].SchumixRegisterHandler("message",      sFunctions.HandleMessage);
			sIrcBase.Networks[_servername].SchumixRegisterHandler("weather",      sFunctions.HandleWeather);
			sIrcBase.Networks[_servername].SchumixRegisterHandler("roll",         sFunctions.HandleRoll);
			sIrcBase.Networks[_servername].SchumixRegisterHandler("sha1",         sFunctions.HandleSha1);
			sIrcBase.Networks[_servername].SchumixRegisterHandler("md5",          sFunctions.HandleMd5);
			sIrcBase.Networks[_servername].SchumixRegisterHandler("prime",        sFunctions.HandlePrime);
			sIrcBase.Networks[_servername].SchumixRegisterHandler("wiki",         sFunctions.HandleWiki);
			sIrcBase.Networks[_servername].SchumixRegisterHandler("calc",         sFunctions.HandleCalc);
			sIrcBase.Networks[_servername].SchumixRegisterHandler("autofunction", sFunctions.HandleAutoFunction, CommandPermission.HalfOperator);
		}

		private void RemoveIrcCommand()
		{
			sIrcBase.Networks[_servername].SchumixRemoveHandler("notes",          sNotes.HandleNotes);
			sIrcBase.Networks[_servername].SchumixRemoveHandler("message",        sFunctions.HandleMessage);
			sIrcBase.Networks[_servername].SchumixRemoveHandler("weather",        sFunctions.HandleWeather);
			sIrcBase.Networks[_servername].SchumixRemoveHandler("roll",           sFunctions.HandleRoll);
			sIrcBase.Networks[_servername].SchumixRemoveHandler("sha1",           sFunctions.HandleSha1);
			sIrcBase.Networks[_servername].SchumixRemoveHandler("md5",            sFunctions.HandleMd5);
			sIrcBase.Networks[_servername].SchumixRemoveHandler("prime",          sFunctions.HandlePrime);
			sIrcBase.Networks[_servername].SchumixRemoveHandler("wiki",           sFunctions.HandleWiki);
			sIrcBase.Networks[_servername].SchumixRemoveHandler("calc",           sFunctions.HandleCalc);
			sIrcBase.Networks[_servername].SchumixRemoveHandler("autofunction",   sFunctions.HandleAutoFunction);
		}

		private void HandlePrivmsg(IRCMessage sIRCMessage)
		{
			var sIgnoreNickName = sIrcBase.Networks[sIRCMessage.ServerName].sIgnoreNickName;
			var sMyChannelInfo = sIrcBase.Networks[sIRCMessage.ServerName].sMyChannelInfo;
			var sSender = sIrcBase.Networks[sIRCMessage.ServerName].sSender;

			if(sIgnoreNickName.IsIgnore(sIRCMessage.Nick))
				return;

			if(sMyChannelInfo.FSelect(IFunctions.Commands) || !Rfc2812Util.IsValidChannelName(sIRCMessage.Channel))
			{
				if(!sMyChannelInfo.FSelect(IChannelFunctions.Commands, sIRCMessage.Channel) && Rfc2812Util.IsValidChannelName(sIRCMessage.Channel))
					return;

				if(!Rfc2812Util.IsValidChannelName(sIRCMessage.Channel))
					return;

				Task.Factory.StartNew(() =>
				{
					if(sFunctions.AutoKick("privmsg", sIRCMessage.Nick, sIRCMessage.Channel))
						return;
				});

				Task.Factory.StartNew(() =>
				{
					if(sMyChannelInfo.FSelect(IFunctions.Automode) && sMyChannelInfo.FSelect(IChannelFunctions.Automode, sIRCMessage.Channel))
					{
						sIrcHandler.AutoMode = true;
						sIrcHandler.ModeChannel = sIRCMessage.Channel.ToLower();
						sSender.NickServStatus(sIRCMessage.Nick);
					}
				});

				Task.Factory.StartNew(() =>
				{
					if(sMyChannelInfo.FSelect(IFunctions.Randomkick) && sMyChannelInfo.FSelect(IChannelFunctions.Randomkick, sIRCMessage.Channel))
					{
						if(sIRCMessage.Args.IsUpper() && sIRCMessage.Args.Length > 4)
							sSender.Kick(sIRCMessage.Channel, sIRCMessage.Nick, sLManager.GetWarningText("CapsLockOff", sIRCMessage.Channel, sIRCMessage.ServerName));
					}
				});

				Task.Factory.StartNew(() => sFunctions.HLMessage(sIRCMessage));
				Task.Factory.StartNew(() => sFunctions.Message(sIRCMessage));

				Task.Factory.StartNew(() =>
				{
					if(sMyChannelInfo.FSelect(IFunctions.Webtitle) && sMyChannelInfo.FSelect(IChannelFunctions.Webtitle, sIRCMessage.Channel))
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
							Log.Error("ExtraAddon", sLConsole.GetString("Invalid webpage address: {0}"), e.Message);
							return;
						}
					}
				});
			}
		}

		private void HandleNotice(IRCMessage sIRCMessage)
		{
			var sSender = sIrcBase.Networks[sIRCMessage.ServerName].sSender;

			if(sIRCMessage.Nick == "NickServ" && sIrcHandler.AutoMode)
			{
				if(sIRCMessage.Info.Length < 6)
					return;

				if(sIRCMessage.Info[5] == "3")
				{
					var db = SchumixBase.DManager.QueryFirstRow("SELECT Rank FROM modelist WHERE Name = '{0}' AND Channel = '{1}' And ServerName = '{2}'", sIRCMessage.Info[4].ToLower(), sIrcHandler.ModeChannel, sIRCMessage.ServerName);
					if(!db.IsNull())
					{
						string rank = db["Rank"].ToString();
						sSender.Mode(sIrcHandler.ModeChannel, rank, sIRCMessage.Info[4]);
					}
					else
					{
						if(ModeConfig.RemoveEnabled)
						{
							if(ModeConfig.RemoveType.Length == 1)
								sSender.Mode(sIrcHandler.ModeChannel, Rfc2812Util.ModeActionToChar(ModeAction.Remove) + ModeConfig.RemoveType, sIRCMessage.Info[4]);
							else if(ModeConfig.RemoveType.Length == 2)
								sSender.Mode(sIrcHandler.ModeChannel, Rfc2812Util.ModeActionToChar(ModeAction.Remove) + ModeConfig.RemoveType, string.Format("{0} {0}", sIRCMessage.Info[4]));
							else if(ModeConfig.RemoveType.Length == 3)
								sSender.Mode(sIrcHandler.ModeChannel, Rfc2812Util.ModeActionToChar(ModeAction.Remove) + ModeConfig.RemoveType, string.Format("{0} {0} {0}", sIRCMessage.Info[4]));
							else if(ModeConfig.RemoveType.Length == 4)
								sSender.Mode(sIrcHandler.ModeChannel, Rfc2812Util.ModeActionToChar(ModeAction.Remove) + ModeConfig.RemoveType, string.Format("{0} {0} {0} {0}", sIRCMessage.Info[4]));
						}
					}
				}

				sIrcHandler.AutoMode = false;
			}

			if(sIRCMessage.Nick == "NickServ" && sFunctions.IsOnline)
			{
				if(sIRCMessage.Args.Contains("isn't registered.") || sIRCMessage.Args.Contains("Last seen time:") || sIRCMessage.Args.Contains("Last seen:"))
				{
					var sMyNickInfo = sIrcBase.Networks[sIRCMessage.ServerName].sMyNickInfo;
					sIrcHandler.sNameList.Change(sMyNickInfo.NickStorage, IRCConfig.List[sIRCMessage.ServerName].NickName, true);
					sMyNickInfo.ChangeNick(IRCConfig.List[sIRCMessage.ServerName].NickName);
					sSender.Nick(IRCConfig.List[sIRCMessage.ServerName].NickName);
					sMyNickInfo.Identify(IRCConfig.List[sIRCMessage.ServerName].NickServPassword);

					if(IRCConfig.List[sIRCMessage.ServerName].UseHostServ)
						sMyNickInfo.Vhost(SchumixBase.On);

					sFunctions.IsOnline = false;
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