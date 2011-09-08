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
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Schumix.API;
using Schumix.Irc;
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
	public class CompilerAddon : Compiler, ISchumixAddon
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly PLocalization sLocalization = Singleton<PLocalization>.Instance;
		private readonly ChannelInfo sChannelInfo = Singleton<ChannelInfo>.Instance;
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;
		private readonly Regex regex = new Regex(@"^\{(?<code>.*)\}$");
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
			ClassRegex = new Regex(@"class\s+" + CompilerConfig.MainClass + @"\s*?\{");
			EntryRegex = new Regex(SchumixBase.Space + CompilerConfig.MainClass + @"\s*?\{");
			SchumixRegex = new Regex(CompilerConfig.MainConstructor + @"\s*\(\s*(?<lol>.*)\s*\)");
		}

		public void Destroy()
		{

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

				if(!CompilerConfig.CompilerEnabled)
					return;

				string command = IRCConfig.NickName + SchumixBase.Comma;
				sIRCMessage.Info[3] = sIRCMessage.Info[3].Remove(0, 1, SchumixBase.Point2);

				if(sIRCMessage.Info[3].ToLower() == command.ToLower() && Enabled(sIRCMessage) && (sIRCMessage.Args.Contains(";") || sIRCMessage.Args.Contains("}")))
					Compiler(sIRCMessage, true, command);
				else if(sIRCMessage.Info[3].ToLower() == command.ToLower())
				{
					if(sIRCMessage.Info.Length >= 5 && (sIRCMessage.Info[4].ToLower() == "csc" || sIRCMessage.Info[4].ToLower() == "c#compiler"))
					{
						sSendMessage.SendChatMessage(sIRCMessage, "C# Compiler version: {0}", Environment.Version);
						sSendMessage.SendChatMessage(sIRCMessage, "The main class's name: class " + CompilerConfig.MainClass + " { /* program... */ }");
						sSendMessage.SendChatMessage(sIRCMessage, "The main function's name: void " + CompilerConfig.MainConstructor + "() { /* program... */ }");
						sSendMessage.SendChatMessage(sIRCMessage, "You can use simply: '{ /* program */ }'. This is the man function's content.");
						sSendMessage.SendChatMessage(sIRCMessage, "Also you can use: '{0} /* program */'. Here is /* program */ is the main function's content.", command.ToLower());
						sSendMessage.SendChatMessage(sIRCMessage, "If you need more help, please contact with Jackneill.");
						sSendMessage.SendChatMessage(sIRCMessage, "Programmed by: Csaba (Megaxxx)");
					}
				}

				if(sIRCMessage.Channel.Substring(0, 1) != "#")
				{
					if(regex.IsMatch(sIRCMessage.Args.TrimEnd()) && Enabled(sIRCMessage))
						Compiler(sIRCMessage, false, command);
				}
				else
				{
					if((sChannelInfo.FSelect("compiler") && sChannelInfo.FSelect("compiler", sIRCMessage.Channel)) &&
						(regex.IsMatch(sIRCMessage.Args.TrimEnd()) && Enabled(sIRCMessage)))
						Compiler(sIRCMessage, false, command);
				}
			}
		}

		public void HandleNotice(IRCMessage sIRCMessage)
		{

		}

		public void HandleLeft(IRCMessage sIRCMessage)
		{

		}

		public void HandleKick(IRCMessage sIRCMessage)
		{

		}

		public bool HandleHelp(IRCMessage sIRCMessage)
		{
			return false;
		}

		private bool Enabled(IRCMessage sIRCMessage)
		{
			if(CompilerConfig.MaxAllocatingE)
			{
				var memory = Process.GetCurrentProcess().WorkingSet64/1024/1024;

				if(memory > CompilerConfig.MaxAllocatingM)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("compiler/memory", sIRCMessage.Channel));
					return false;
				}
			}

			return true;
		}

		private void Compiler(IRCMessage sIRCMessage, bool command, string args)
		{
			int ReturnCode = 0;

			if(command)
			{
				sIRCMessage.Args = sIRCMessage.Args.Remove(0, args.Length);
				var thread = new Thread(() => ReturnCode = CompilerCommand(sIRCMessage, true));
				thread.Start();
				thread.Join(3000);
				thread.Abort();
			}
			else
			{
				var thread = new Thread(() => ReturnCode = CompilerCommand(sIRCMessage, false));
				thread.Start();
				thread.Join(3000);
				thread.Abort();
			}

			switch(ReturnCode)
			{
				case -1:
					sSendMessage.SendChatMessage(sIRCMessage, ":'(");
					break;
				case 0:
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("compiler/kill", sIRCMessage.Channel));
					break;
				case 2:
					sSendMessage.SendChatMessage(sIRCMessage, MessageText(3, sIRCMessage.Channel));
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