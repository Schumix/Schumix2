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
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Schumix.API;
using Schumix.API.Irc;
using Schumix.API.Functions;
using Schumix.Irc;
using Schumix.Irc.Flood;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using Schumix.CompilerAddon.Config;
using Schumix.CompilerAddon.Commands;
using Schumix.CompilerAddon.Localization;

namespace Schumix.CompilerAddon
{
	class CompilerAddon : ISchumixAddon
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly PLocalization sLocalization = Singleton<PLocalization>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private readonly Regex regex = new Regex(@"^\{(?<code>.*)\}$");
		private SCompiler sSCompiler;
#pragma warning disable 414
		private AddonConfig _config;
#pragma warning restore 414
		private string _servername;

		public void Setup(string ServerName)
		{
			_servername = ServerName;
			sSCompiler = new SCompiler(ServerName);
			sLocalization.Locale = sLConsole.Locale;

			if(IRCConfig.List[ServerName].ServerId == 1)
				_config = new AddonConfig(Name, ".yml");

			sIrcBase.Networks[ServerName].IrcRegisterHandler("PRIVMSG", HandlePrivmsg);
			sSCompiler.ClassRegex = new Regex(@"class\s+" + CompilerConfig.MainClass + @"\s*?\{");
			sSCompiler.EntryRegex = new Regex(SchumixBase.Space + CompilerConfig.MainClass + @"\s*?\{");
			sSCompiler.SchumixRegex = new Regex(CompilerConfig.MainConstructor + @"\s*\(\s*(?<lol>.*)\s*\)");
		}

		public void Destroy()
		{
			sIrcBase.Networks[_servername].IrcRemoveHandler("PRIVMSG",  HandlePrivmsg);
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
				}
			}
			catch(Exception e)
			{
				Log.Error("CompilerAddon", "Reload: " + sLConsole.Exception("Error"), e.Message);
				return 0;
			}

			return -1;
		}

		private void HandlePrivmsg(IRCMessage sIRCMessage)
		{
			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;
			var sMyChannelInfo = sIrcBase.Networks[sIRCMessage.ServerName].sMyChannelInfo;

			if(sMyChannelInfo.FSelect(IFunctions.Commands) || !sSCompiler.IsChannel(sIRCMessage.Channel))
			{
				if(!sMyChannelInfo.FSelect(IChannelFunctions.Commands, sIRCMessage.Channel) && sSCompiler.IsChannel(sIRCMessage.Channel))
					return;

				if(!CompilerConfig.CompilerEnabled)
					return;

				if(!sSCompiler.IsChannel(sIRCMessage.Channel))
					sIRCMessage.Channel = sIRCMessage.Nick;

				string command = IRCConfig.List[sIRCMessage.ServerName].NickName + SchumixBase.Comma;
				sIRCMessage.Info[3] = sIRCMessage.Info[3].Remove(0, 1, SchumixBase.Colon);

				if(sIRCMessage.Info[3].ToLower() == command.ToLower() && (sIRCMessage.Args.Contains(";") || sIRCMessage.Args.Contains("}")) && Enabled(sIRCMessage))
					Compiler(sIRCMessage, true, command);
				else if(sIRCMessage.Info[3].ToLower() == command.ToLower())
				{
					sIrcBase.Networks[sIRCMessage.ServerName].sAntiFlood.FloodCommand(sIRCMessage);

					if(sIrcBase.Networks[sIRCMessage.ServerName].sAntiFlood.Ignore(sIRCMessage))
						return;

					var text = sLManager.GetCommandTexts("schumix2/csc", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					if(sIRCMessage.Info.Length >= 5 && (sIRCMessage.Info[4].ToLower() == "csc" || sIRCMessage.Info[4].ToLower() == "c#compiler"))
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0], Environment.Version);
						sSendMessage.SendChatMessage(sIRCMessage, text[1], CompilerConfig.MainClass, "{ /* program... */ }");
						sSendMessage.SendChatMessage(sIRCMessage, text[2], CompilerConfig.MainConstructor, "{ /* program... */ }");
						sSendMessage.SendChatMessage(sIRCMessage, text[3]);
						sSendMessage.SendChatMessage(sIRCMessage, text[4], command.ToLower());
						sSendMessage.SendChatMessage(sIRCMessage, text[5]);
						sSendMessage.SendChatMessage(sIRCMessage, text[6], Consts.SchumixProgrammedBy);
					}
				}

				if(!sSCompiler.IsChannel(sIRCMessage.Channel))
				{
					if(regex.IsMatch(sIRCMessage.Args.TrimEnd()) && Enabled(sIRCMessage))
						Compiler(sIRCMessage, false, command);
				}
				else
				{
					if((sMyChannelInfo.FSelect(IFunctions.Compiler) && sMyChannelInfo.FSelect(IChannelFunctions.Compiler, sIRCMessage.Channel)) &&
						(regex.IsMatch(sIRCMessage.Args.TrimEnd()) && Enabled(sIRCMessage)))
						Compiler(sIRCMessage, false, command);
				}
			}
		}

		public bool HandleHelp(IRCMessage sIRCMessage)
		{
			return sSCompiler.Help(sIRCMessage);
		}

		private bool Enabled(IRCMessage sIRCMessage)
		{
			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;

			if(CompilerConfig.MaxAllocatingE)
			{
				var memory = Process.GetCurrentProcess().WorkingSet64/1024/1024;
				int ircnetwork = sIrcBase.Networks.Count > 1 ? 20 * sIrcBase.Networks.Count : 0;

				if(memory > CompilerConfig.MaxAllocatingM + ircnetwork)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("compiler/memory", sIRCMessage.Channel, sIRCMessage.ServerName));
					return false;
				}
			}

			return true;
		}

		private void Compiler(IRCMessage sIRCMessage, bool command, string args)
		{
			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;
			sIrcBase.Networks[sIRCMessage.ServerName].sAntiFlood.FloodCommand(sIRCMessage);

			if(sIrcBase.Networks[sIRCMessage.ServerName].sAntiFlood.Ignore(sIRCMessage))
				return;

			int ReturnCode = 0;

			if(command)
			{
				sIRCMessage.Args = sIRCMessage.Args.Remove(0, args.Length);
				ReturnCode = sSCompiler.CompilerCommand(sIRCMessage, true);
			}
			else
				ReturnCode = sSCompiler.CompilerCommand(sIRCMessage, false);

			switch(ReturnCode)
			{
				case -1:
					sSendMessage.SendChatMessage(sIRCMessage, ":'(");
					break;
				case 0:
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("compiler/kill", sIRCMessage.Channel, sIRCMessage.ServerName));
					break;
				case 2:
					sSendMessage.SendChatMessage(sIRCMessage, sSCompiler.MessageText(3, sIRCMessage.Channel));
					break;
				default:
					break;
			}

			var sw = new StreamWriter(Console.OpenStandardOutput());
			sw.AutoFlush = true;
			Console.SetOut(sw);
		}

		/// <summary>
		/// Name of the addon
		/// </summary>
		public string Name
		{
			get { return "CompilerAddon"; }
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