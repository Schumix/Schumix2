/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013-2015 Schumix Team <http://schumix.eu/>
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
using System.Text;
using System.Text.RegularExpressions;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Compiler;
using Schumix.Framework;
using Schumix.Framework.Irc;
using Schumix.Framework.Logger;
using Schumix.Framework.Platforms;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using Schumix.CompilerAddon.Config;

namespace Schumix.CompilerAddon.Commands
{
	partial class SCompiler : CommandInfo
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly Platform sPlatform = Singleton<Platform>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private readonly Regex regex = new Regex(@"^\{(?<code>.*)\}$");
		private readonly Regex SystemNetRegex = new Regex(@"using\s+System.Net");
		public Regex SchumixAndOverrideRegex { get; set; }
		public Regex EntryAndAbstractRegex { get; set; }
		public Regex SchumixRegex { get; set; }
		public Regex EntryRegex { get; set; }
		private string _servername;
		private int CodePage;

		public SCompiler(string ServerName) : base(ServerName)
		{
			_servername = ServerName;
			CodePage = Console.OutputEncoding.CodePage;
		}

		private bool IsEntry(string data)
		{
			return EntryRegex.IsMatch(data);
		}

		private bool IsSchumix(string data)
		{
			return SchumixRegex.IsMatch(data);
		}

		private bool IsSchumixAndOverride(string data)
		{
			return SchumixAndOverrideRegex.IsMatch(data);
		}

		private bool IsEntryAndAbstractRegex(string data)
		{
			return EntryAndAbstractRegex.IsMatch(data);
		}

		public string MessageText(int Code, string Channel)
		{
			return sLManager.GetCommandTexts("compiler", Channel, _servername).Length > Code ? sLManager.GetCommandTexts("compiler", Channel, _servername)[Code] : sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(Channel, _servername));
		}

		public int CompilerCommand(IRCMessage sIRCMessage, bool command)
		{
			try
			{
				var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;
				var text = sLManager.GetCommandTexts("compiler", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 5)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return 1;
				}

				string data = string.Empty, template = string.Empty;

				if(!command && regex.IsMatch(sIRCMessage.Args.TrimEnd()))
					data = regex.Match(sIRCMessage.Args.TrimEnd()).Groups["code"].ToString();
				else if(command)
					data = sIRCMessage.Args.Trim();

				if(Ban(data, sIRCMessage))
					return 1;

				if(!IsEntry(data) && !IsEntryAndAbstractRegex(data))
				{
					if(!IsSchumix(data) && !IsSchumixAndOverride(data))
						template = CompilerConfig.Referenced + " class " + CompilerConfig.MainClass + " : Schumix.Compiler.Abstract { public override void " + CompilerConfig.MainConstructor + "() { " + CleanText(data) + " } }";
					else if(IsSchumix(data) && !IsSchumixAndOverride(data))
						template = CompilerConfig.Referenced + " class " + CompilerConfig.MainClass + " : Schumix.Compiler.Abstract { " + Regex.Replace(CleanText(data), SchumixRegex.ToString(), @"public override void " + CompilerConfig.MainConstructor + @"() {") + " }";
					else if(!IsSchumix(data) && IsSchumixAndOverride(data))
						template = CompilerConfig.Referenced + " class " + CompilerConfig.MainClass + " : Schumix.Compiler.Abstract { " + CleanText(data) + " }";
				}
				else if(IsEntry(data) && !IsEntryAndAbstractRegex(data))
				{
					if(!IsSchumix(data) && !IsSchumixAndOverride(data))
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return 1;
					}

					if(IsSchumix(data) && !IsSchumixAndOverride(data))
						data = Regex.Replace(data, SchumixRegex.ToString(), @"public override void " + CompilerConfig.MainConstructor + @"() {");

					template = CompilerConfig.Referenced + SchumixBase.Space + Regex.Replace(CleanText(data), EntryRegex.ToString(), @"class " + CompilerConfig.MainClass + @" : Schumix.Compiler.Abstract {");
				}
				else if(!IsEntry(data) && IsEntryAndAbstractRegex(data))
				{
					if(!IsSchumix(data) && !IsSchumixAndOverride(data))
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return 1;
					}

					if(IsSchumix(data) && !IsSchumixAndOverride(data))
						data = Regex.Replace(data, SchumixRegex.ToString(), @"public override void " + CompilerConfig.MainConstructor + @"() {");

					template = CompilerConfig.Referenced + SchumixBase.Space + CleanText(data);
				}

				var s = new Sandbox(CompilerConfig.ReferencedAssemblies, CompilerConfig.CompilerOptions, CompilerConfig.WarningLevel, CompilerConfig.TreatWarningsAsErrors);
				//s.TestIsFullyTrusted(); // Test Sandbox

				string errormessage = string.Empty;
				var asm = s.CompileCode(template, ref errormessage);
				errormessage = errormessage.Remove(0, 2, ". ");

				if(!errormessage.IsNullOrEmpty())
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("compiler/code", sIRCMessage.Channel, sIRCMessage.ServerName), errormessage.TrimMessage(1000));

				if(asm.IsNull())
					return 1;

				AppDomain appdomain = null;
				s = Sandbox.CreateInstance(ref appdomain);
				//s.TestIsFullyTrusted(); // Test Sandbox

				object o = asm.CreateInstance(CompilerConfig.MainClass);
				if(o.IsNull())
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
					return 1;
				}

				var writer = new StringWriter();
				Console.SetOut(writer);

				int ReturnCode = 0;
				int errorcode = -1;
				var thread = new Thread(() =>
				{
					try
					{
						errormessage = string.Empty;
						ReturnCode = s.StartCode(data, CompilerConfig.MainConstructor, (Abstract)o, ref errormessage, ref errorcode);
					}
					catch(Exception e)
					{
						ReturnCode = -1;
						ConsoleOpenStandardOutput();
						Log.Debug("CompilerThread", sLConsole.GetString("Failure details: {0}"), e.Message);
					}
				});

				thread.Start();
				thread.Join(3000);
				thread.Abort();
				AppDomain.Unload(appdomain);

				switch(errorcode)
				{
					case 0:
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("compiler/code", sIRCMessage.Channel, sIRCMessage.ServerName), errormessage.TrimMessage(1000));
						break;
					default:
						break;
				}

				switch(ReturnCode)
				{
					case -1:
						return 1;
					case 0:
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("compiler/kill", sIRCMessage.Channel, sIRCMessage.ServerName));
						return 1;
					default:
						break;
				}

				string _write = writer.ToString();
				sSendMessage.SendChatMessage(sIRCMessage, string.Empty);

				if(_write.Length == 0)
					return 2;

				var length = _write.Length;
				var lines = CleanIrcText(_write).Split(SchumixBase.NewLine);

				if(length > 1000 && lines.Length == 1)
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[2]);
					return 1;
				}
				else if(length <= 1000 && lines.Length == 1)
				{
					byte i = 0, x = 0;

					foreach(var line in lines)
					{
						i++;

						if(line.IsNullOrEmpty())
							x++;
						else
							sSendMessage.SendChatMessage(sIRCMessage, line);
					}

					if(i == x)
						sSendMessage.SendChatMessage(sIRCMessage, text[3]);

					return 1;
				}
				else if(lines.Length == 2)
				{
					foreach(var line in lines)
					{
						if(line.IsNullOrEmpty())
							continue;
						else
							sSendMessage.SendChatMessage(sIRCMessage, line);
					}

					return 1;
				}
				else if(lines.Length > 2)
				{
					sSendMessage.SendChatMessage(sIRCMessage, lines[0]);
					sSendMessage.SendChatMessage(sIRCMessage, text[4], lines.Length-1);
					return 1;
				}

				return 1;
			}
			catch(Exception e)
			{
				ConsoleOpenStandardOutput();
				Log.Debug("CompilerCommand", sLConsole.GetString("Failure details: {0}"), e.Message);
				return -1;
			}
		}

		private bool Ban(string data, IRCMessage sIRCMessage)
		{
			// DllImport
			if(data.Contains("DllImport"))
			{
				Warning(sIRCMessage);
				return true;
			}

			// Timers
			if(data.Contains("System.Timers"))
			{
				Warning(sIRCMessage);
				return true;
			}

			if(sPlatform.IsWindows)
				return false;

			// Environment and Security
			if(data.Contains("Environment.Exit") || data.Contains("Environment.SetEnvironmentVariable") ||
			   data.Contains("Environment.ExpandEnvironmentVariables") || data.Contains("Environment.FailFast") ||
			   data.Contains("Environment.GetLogicalDrives") || data.Contains("Environment.GetEnvironmentVariables") ||
			   data.Contains("Environment.CommandLine") || data.Contains("Environment.CurrentDirectory") ||
			   data.Contains("Environment.GetFolderPath") || data.Contains("System.Security"))
			{
				Warning(sIRCMessage);
				return true;
			}

			// Input, Output
			if(data.Contains("System.IO"))
			{
				Warning(sIRCMessage);
				return true;
			}

			// Diagnostics
			if(data.Contains("System.Diagnostics"))
			{
				Warning(sIRCMessage);
				return true;
			}

			// Microsoft
			if(data.Contains("Microsoft.Win32") || data.Contains("Microsoft.CSharp"))
			{
				Warning(sIRCMessage);
				return true;
			}

			// Compile
			if(data.Contains("System.CodeDom"))
			{
				Warning(sIRCMessage);
				return true;
			}

			// Console
			if(data.Contains("Console.SetOut") || data.Contains("Console.Title") || data.Contains("Console.CancelKeyPress") ||
				data.Contains("Console.ResetColor") || data.Contains("Console.SetCursorPosition") ||
				data.Contains("Console.SetError") || data.Contains("Console.SetIn") || data.Contains("Console.SetWindowSize") ||
				data.Contains("Console.BackgroundColor") || data.Contains("Console.CapsLock") || data.Contains("Console.Cursor") ||
				data.Contains("Console.ForegroundColor") || data.Contains("Console.InputEncoding") ||
				data.Contains("Console.KeyAvailable") || data.Contains("Console.LargestWindow") ||
				data.Contains("Console.NumberLock") || data.Contains("Console.OutputEncoding") ||
				data.Contains("Console.TreatControlCAsInput") || data.Contains("Console.Window"))
			{
				Warning(sIRCMessage);
				return true;
			}

			// System.Net
			if(data.Contains("System.Net.Dns") || data.Contains("System.Net.IPAddress") || data.Contains("System.Net.IPEndPoint") ||
				data.Contains("System.Net.IPHostEntry") || SystemNetRegex.IsMatch(data))
			{
				Warning(sIRCMessage);
				return true;
			}

			// Assembly
			if(data.Contains("Assembly.Load") || data.Contains("Assembly.ReflectionOnlyLoad") || data.Contains("Assembly.UnsafeLoadFrom") ||
				data.Contains("Assembly.Location") || data.Contains("Assembly.EscapedCodeBase") || data.Contains("Assembly.CodeBase") ||
				data.Contains("Assembly.GetAssembly"))
			{
				Warning(sIRCMessage);
				return true;
			}

			// AppDomain
			if(data.Contains("AppDomain"))
			{
				Warning(sIRCMessage);
				return true;
			}

			// Activator
			if(data.Contains("Activator"))
			{
				Warning(sIRCMessage);
				return true;
			}

			// Type
			if(data.Contains("GetMethod") || data.Contains("GetType") || data.Contains("GetInterfaces") || data.Contains("GetMember"))
			{
				Warning(sIRCMessage);
				return true;
			}

			return false;
		}

		private string CleanText(string text)
		{
			if(text.Contains("/*") && text.Contains("*/"))
			{
				var sb = new StringBuilder();

				for(;;)
				{
					if(!text.Contains("/*"))
						break;

					sb.Append(text.Substring(0, text.IndexOf("/*")));
					text = text.Substring(text.IndexOf("/*")+2);

					if(text.Contains("/*"))
						text = text.Remove(0, text.IndexOf("*/")+2);
					else
						sb.Append(text.Substring(text.IndexOf("*/")+2));
				}

				text = sb.ToString();
			}

			text = CleanSemicolon(text);

			if(text.Contains("{") && text.Contains("}"))
			{
				for(;;)
				{
					string s = CleanSemicolon(text).Trim();

					if(s.Length > 0 && s.Substring(0, 1) == "}")
					{
						text = s.Remove(0, 1, "}");
						continue;
					}

					if(s.Length == 0 || s.Substring(0, 1) != "{")
						break;

					if(s.IndexOf("{") == s.Length-1)
					{
						text = s.Remove(0, s.IndexOf("{")+1);
						break;
					}

					if(s.Substring(s.IndexOf("{")+1, s.IndexOf("}")).Trim() == "}")
					{
						text = s.Remove(0, s.IndexOf("}")+1);
						continue;
					}
					else if(s.Substring(s.IndexOf("{")+1, s.IndexOf("}")).Trim().Substring(0, 1) == "{")
					{
						text = s.Remove(0, s.IndexOf("{")+1);
						continue;
					}
					else if(s.Substring(s.IndexOf("{")+1, s.IndexOf("}")).Trim().Substring(0, 1) == ";")
					{
						bool enabled = true;
						string ss = s.Substring(s.IndexOf("{")+1, s.IndexOf("}")).Trim();

						for(;;)
						{
							if(!enabled)
								ss = text.Trim();

							if(enabled)
								enabled = false;

							if(ss.Length > 0 && ss.Substring(0, 1) == "}")
							{
								text = ss.Remove(0, 1, "}");
								break;
							}

							if(ss.Length == 0 || ss.Substring(0, 1) != ";")
								break;

							text = ss.Remove(0, 1, ";");
						}

						continue;
					}
					else
						break;
				}
			}

			text = CleanSemicolon(text);
			return text.IsNullOrEmpty() ? "Console.Write(\":'(\");" : text;
		}

		private string CleanSemicolon(string text)
		{
			text = text.Trim();

			if(text.Length > 0 && text.Substring(0, 1) == ";")
			{
				for(;;)
				{
					string s = text.Trim();

					if(s.Length == 0 || s.Substring(0, 1) != ";")
						break;

					text = s.Remove(0, 1, ";");
				}
			}

			return text;
		}

		private string CleanIrcText(string text)
		{
			text = text.Replace(((char)1).ToString(), string.Empty);
			text = text.Replace(((char)2).ToString(), string.Empty);
			text = text.Replace(((char)3).ToString(), string.Empty);
			return text;
		}

		private void Warning(IRCMessage sIRCMessage)
		{
			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;
			sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("compiler/warning", sIRCMessage.Channel, sIRCMessage.ServerName));
		}

		public void ConsoleOpenStandardOutput()
		{
			var sw = new StreamWriter(Console.OpenStandardOutput(), Encoding.GetEncoding(CodePage)) { AutoFlush = true };
			Console.SetOut(sw);
		}
	}
}