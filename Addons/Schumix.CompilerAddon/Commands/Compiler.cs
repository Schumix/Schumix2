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
using System.Reflection;
using System.CodeDom.Compiler;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CSharp;
using Schumix.API;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using Schumix.CompilerAddon.Config;

namespace Schumix.CompilerAddon.Commands
{
	public class Compiler : CommandInfo
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;
		private readonly Regex regex = new Regex(@"^\{(?<code>.*)\}$");
		private readonly Regex ForRegex = new Regex(@"for\s*\(\s*(?<lol>.*)\s*\)");
		private readonly Regex WhileRegex = new Regex(@"while\s*\(\s*(?<lol>.*)\s*\)");
		private readonly Regex DoRegex = new Regex(@"do\s*\{?\s*(?<content>.+)\s*\}?\s*while\s*\((?<while>.+)\s*\)");
		private readonly Regex SystemNetRegex = new Regex(@"using\s+System.Net");
		protected Regex ClassRegex { get; set; }
		protected Regex EntryRegex { get; set; }
		protected Regex SchumixRegex { get; set; }

		private bool IsClass(string data)
		{
			return ClassRegex.IsMatch(data);
		}

		private bool IsEntry(string data)
		{
			return EntryRegex.IsMatch(data);
		}

		private bool IsSchumix(string data)
		{
			return SchumixRegex.IsMatch(data);
		}

		private bool IsFor(string data)
		{
			return ForRegex.IsMatch(data);
		}

		private bool IsWhile(string data)
		{
			return WhileRegex.IsMatch(data);
		}

		private bool IsDo(string data)
		{
			return DoRegex.IsMatch(data);
		}

		protected bool CompilerCommand(IRCMessage sIRCMessage, bool command)
		{
			try
			{
				CNick(sIRCMessage);
				var text = sLManager.GetCommandTexts("compiler", sIRCMessage.Channel);
				if(text.Length < 5)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
					return true;
				}

				string data = string.Empty, template = string.Empty;

				if(!command && regex.IsMatch(sIRCMessage.Args.TrimEnd()))
					data = regex.Match(sIRCMessage.Args.TrimEnd()).Groups["code"].ToString();
				else if(command)
					data = sIRCMessage.Args.Trim();

				if(Ban(data, sIRCMessage.Channel))
					return true;

				if(!IsClass(data))
				{
					if(!IsSchumix(data))
						template = CompilerConfig.Referenced + " public class " + CompilerConfig.MainClass + " { public void " + CompilerConfig.MainConstructor + "() { " + CleanText(data) + " } }";
					else
						template = CompilerConfig.Referenced + " public class " + CompilerConfig.MainClass + " { " + CleanText(data) + " }";
				}
				else if(IsEntry(data))
				{
					if(!IsSchumix(data))
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
						return true;
					}

					template = CompilerConfig.Referenced + SchumixBase.Space + CleanText(data);
				}

				var asm = CompileCode(template, sIRCMessage.Channel);
				if(asm.IsNull())
					return true;

				var writer = new StringWriter();
				Console.SetOut(writer);

				object o = asm.CreateInstance(CompilerConfig.MainClass);
				if(o.IsNull())
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1]);
					return true;
				}

				if(IsFor(data) || IsDo(data) || IsWhile(data))
				{
					bool b = false;
					var thread = new Thread(() => { o.GetType().InvokeMember(CompilerConfig.MainConstructor, BindingFlags.InvokeMethod | BindingFlags.Default, null, o, null); b = true; });
					thread.Start();
					thread.Join(2);
					thread.Abort();

					if(!b)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("compiler/kill", sIRCMessage.Channel));
						return true;
					}
				}
				else
					o.GetType().InvokeMember(CompilerConfig.MainConstructor, BindingFlags.InvokeMethod | BindingFlags.Default, null, o, null);

				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, string.Empty);

				if(writer.ToString().Length > 2000)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[2]);
					return true;
				}

				if(writer.ToString().Length == 0)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[3]);
					return true;
				}

				var lines = writer.ToString().Split(SchumixBase.NewLine);

				if(lines.Length <= 5)
				{
					byte i = 0, x = 0;

					foreach(var line in lines)
					{
						i++;

						if(line == string.Empty)
							x++;
						else
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, line);
					}

					if(i == x)
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[3]);
				}
				else if(lines.Length > 5)
				{
					int i = 0, x = 0;

					for(var b = 0; b < 4; b++)
					{
						i++;

						if(lines[b] == string.Empty)
							x++;
						else
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, lines[b]);
					}

					if(i == x)
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[4], lines.Length-1);
					else
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[4], lines.Length-5);
				}

				return true;
			}
			catch(Exception)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, ":'(");
				return true;
			}
		}

		private CompilerParameters InitCompilerParameters()
		{
			var cparams = new CompilerParameters();
			cparams.GenerateExecutable = false;
			cparams.GenerateInMemory = false;

			foreach(var asm in CompilerConfig.ReferencedAssemblies)
				cparams.ReferencedAssemblies.Add(asm);

			cparams.CompilerOptions = CompilerConfig.CompilerOptions;
			cparams.WarningLevel = CompilerConfig.WarningLevel;
			cparams.TreatWarningsAsErrors = CompilerConfig.TreatWarningsAsErrors;
			return cparams;
		}

		private Assembly CompileCode(string code, string channel)
		{
			try
			{
#if MONO
#pragma warning disable 618
				var compiler = new CSharpCodeProvider().CreateCompiler();
#pragma warning restore 618
#else
				var compiler = CodeDomProvider.CreateProvider("CSharp");
#endif
				return CompilerErrors(compiler.CompileAssemblyFromSource(InitCompilerParameters(), code), channel);
			}
			catch(Exception)
			{
				return null;
			}
		}

		private Assembly CompilerErrors(CompilerResults results, string channel)
		{
			if(results.Errors.HasErrors)
			{
				foreach(CompilerError error in results.Errors)
				{
					string errortext = error.ErrorText;

					if(errortext.Contains("Location of the symbol related to previous error"))
					{
#if MONO
						for(;;)
						{
							if(errortext.Contains("/"))
							{
								if(errortext.Substring(0, 1) == "/")
									errortext = errortext.Remove(0, 1);
								else
									errortext = errortext.Remove(0, errortext.IndexOf("/"));
							}
							else
								break;
						}

						string s = "/***/***/***/" + errortext.Substring(0, errortext.IndexOf(".dll")) + ".dll (Location of the symbol related to previous error)";
						sSendMessage.SendCMPrivmsg(channel, sLManager.GetCommandText("compiler/code", channel), s);
#else
						for(;;)
						{
							if(errortext.Contains("\\"))
							{
								if(errortext.Substring(0, 1) == "\\")
									errortext = errortext.Remove(0, 1);
								else
									errortext = errortext.Remove(0, errortext.IndexOf("\\"));
							}
							else
								break;
						}

						string s = "*:\\***\\***\\" + errortext.Substring(0, errortext.IndexOf(".dll")) + ".dll (Location of the symbol related to previous error)";
						sSendMessage.SendCMPrivmsg(channel, sLManager.GetCommandText("compiler/code", channel), s);
#endif
						continue;
					}

					sSendMessage.SendCMPrivmsg(channel, sLManager.GetCommandText("compiler/code", channel), errortext);
				}

				return null;
			}
			else
				return results.CompiledAssembly;
		}

		private bool Ban(string data, string channel)
		{
			// Environment and Security
			if(data.Contains("Environment") || data.Contains("System.Security"))
			{
				Warning(channel);
				return true;
			}

			// Input, Output
			if(data.Contains("System.IO"))
			{
				Warning(channel);
				return true;
			}

			// Diagnostics
			if(data.Contains("System.Diagnostics"))
			{
				Warning(channel);
				return true;
			}

			// Microsoft
			if(data.Contains("Microsoft.Win32") || data.Contains("Microsoft.CSharp"))
			{
				Warning(channel);
				return true;
			}

			// Compile
			if(data.Contains("System.CodeDom"))
			{
				Warning(channel);
				return true;
			}

			// Timers
			if(data.Contains("System.Timers"))
			{
				Warning(channel);
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
				Warning(channel);
				return true;
			}

			// System.Net
			if(data.Contains("System.Net.Dns") || data.Contains("System.Net.IPAddress") || data.Contains("System.Net.IPEndPoint") ||
				data.Contains("System.Net.IPHostEntry") || SystemNetRegex.IsMatch(data))
			{
				Warning(channel);
				return true;
			}

			// Assembly
			if(data.Contains("Assembly.Load") || data.Contains("Assembly.ReflectionOnlyLoad") || data.Contains("Assembly.UnsafeLoadFrom") ||
				data.Contains("Assembly.Location") || data.Contains("Assembly.EscapedCodeBase") || data.Contains("Assembly.CodeBase") ||
				data.Contains("Assembly.GetAssembly"))
			{
				Warning(channel);
				return true;
			}

			// AppDomain
			if(data.Contains("AppDomain"))
			{
				Warning(channel);
				return true;
			}

			// Activator
			if(data.Contains("Activator"))
			{
				Warning(channel);
				return true;
			}

			// Type
			if(data.Contains("GetMethod") || data.Contains("GetType") || data.Contains("GetInterfaces") || data.Contains("GetMember"))
			{
				Warning(channel);
				return true;
			}

			// DllImport
			if(data.Contains("DllImport"))
			{
				Warning(channel);
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
			return text.Trim() == string.Empty ? "Console.Write(\":'(\");" : text;
		}

		private string CleanSemicolon(string text)
		{
			if(text.Trim().Length > 0 && text.Trim().Substring(0, 1) == ";")
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

		private void Warning(string channel)
		{
			sSendMessage.SendCMPrivmsg(channel, sLManager.GetCommandText("compiler/warning", channel));
		}
	}
}