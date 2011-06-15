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
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;
		private readonly Regex regex = new Regex(@"^\{(?<code>.+)\}$");
		private readonly Regex ClassRegex = new Regex(@"class\s+Entry\s*?\{");
		private readonly Regex EntryRegex = new Regex(@" Entry\s*?\{");
		private readonly Regex SchumixRegex = new Regex(@"Schumix\s*\(\s*(?<lol>.*)\s*\)");
		private readonly Regex ForRegex = new Regex(@"for\s*\(\s*(?<lol>.*)\s*\)");
		private readonly Regex WhileRegex = new Regex(@"while\s*\(\s*(?<lol>.*)\s*\)");
		private readonly Regex DoRegex = new Regex(@"do\s*\{?\s*(?<content>.+)\s*\}?\s*while\s*\((?<while>.+)\s*\)");
		private readonly Regex SystemNetRegex = new Regex(@"using\s+System.Net");

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

		protected void CompilerCommand(IRCMessage sIRCMessage, bool command)
		{
			try
			{
				CNick(sIRCMessage);
				var text = sLManager.GetCommandTexts("compiler", sIRCMessage.Channel);
				if(text.Length < 5)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "No translations found!");
					return;
				}

				string data = string.Empty, template = string.Empty;

				if(!command && regex.IsMatch(sIRCMessage.Args.TrimEnd()))
					data = regex.Match(sIRCMessage.Args.TrimEnd()).Groups["code"].ToString();
				else if(command)
					data = sIRCMessage.Args.Trim();

				if(Ban(data, sIRCMessage.Channel))
					return;

				if(!IsClass(data))
				{
					if(!IsSchumix(data))
						template = CompilerConfig.Referenced + " public class Entry { public void Schumix() { int asdcmxd = 0; " + InfiniteLoop(CleanText(data), false, false) + " } }";
					else
						template = CompilerConfig.Referenced + " public class Entry { private int asdcmxd = 0; " + InfiniteLoop(CleanText(data), false, true) + " }";
				}
				else if(IsEntry(data))
				{
					if(!IsSchumix(data))
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
						return;
					}

					template = CompilerConfig.Referenced + " " + InfiniteLoop(CleanText(data), true, true);
				}

				var asm = CompileCode(template, sIRCMessage.Channel);
				if(asm.IsNull())
					return;

				var writer = new StringWriter();
				Console.SetOut(writer);

				object o = asm.CreateInstance("Entry");
				if(o.IsNull())
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1]);
					return;
				}

				var type = o.GetType();
				type.InvokeMember("Schumix", BindingFlags.InvokeMethod | BindingFlags.Default, null, o, null);
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, string.Empty);

				if(writer.ToString().Length > 2000)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[2]);
					return;
				}

				if(writer.ToString().Length == 0)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[3]);
					return;
				}

				var lines = writer.ToString().Split('\n');

				if(lines.Length <= 5)
				{
					foreach(var line in lines)
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, line);
				}
				else if(lines.Length > 5)
				{
					for(int x = 0; x < 4; x++)
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, lines[x]);

					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[4], lines.Length-5);
				}
			}
			catch(Exception)
			{
				// egyenlőre semmi
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

						string s = "*:\***\***\\" + errortext.Substring(0, errortext.IndexOf(".dll")) + ".dll (Location of the symbol related to previous error)";
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

		private string InfiniteLoop(string data, bool Class, bool Schumix)
		{
			if(Class && Schumix)
			{
				var sb = new StringBuilder();
				sb.Append(data.Substring(0, data.IndexOf("class")+5));
				data = data.Remove(0, data.IndexOf("class")+5);

				if(data.Substring(0, data.IndexOf("Entry")).TrimStart() == string.Empty)
				{
					sb.Append(data.Substring(0, data.IndexOf("{")));
					sb.Append("{ public static int asdcmxd = 0; ");
					sb.Append(data.Substring(data.IndexOf("{")+1));
					data = sb.ToString();
				}
				else
				{
					sb.Append(data);
					data = sb.ToString();
				}
			}

			if(IsFor(data))
				data = Loop(data, Class, Schumix, "for");

			if(IsDo(data))
				data = Loop(data, Class, Schumix, "do");

			if(IsWhile(data))
				data = Loop(data, Class, Schumix, "while");

			return data;
		}

		private string Loop(string data, bool Class, bool Schumix, string loop)
		{
			var Length = loop.Length;
			bool enabled = false;
			string s = data, a = string.Empty;
			var sb = new StringBuilder();
			sb.Append(s.Substring(0, s.IndexOf(loop)));

			if(loop == "while")
				a = s.Substring(0, s.IndexOf(loop));

			s = s.Remove(0, s.IndexOf(loop));
			enabled = true;

			for(;;)
			{
				if(!s.Contains(loop) && !enabled)
					break;

				// TODO: Nem kezeli a do{...while(true);..}while(true); kódot.
				//       Figyelmen kivül haggya ezért használhatattlan lesz amit kapunk végeredménynek.
				//       Ha ilyen formában használnánk ajánlott a for-t használni.
				//       A végtelen ciklus elleni védelmet megkerülni nem lehet elvileg csak rossz kódot épit fel.
				//       Ezért amig nem lesz rá javítás ilyen formában használni nem ajánlott.
				if(enabled && loop == "while" && a.Contains("do") && (a.Contains(";do") || a.Contains("do;") || a.Contains(" do ") ||
					a.Contains(" do") || a.Contains("do ")))
				{
					s = s.Substring(s.IndexOf(loop)+Length);
					sb.Append(" " + loop);

					if(s.Contains(loop))
					{
						sb.Append(s.Substring(0, s.IndexOf(loop)));
						s = s.Substring(s.IndexOf(loop));
					}
					else
						sb.Append(s);

					if(enabled)
						enabled = false;

					continue;
				}

				if(s.Length > Length && ((loop == "for" && s.Substring(0, Length+1) == "for(" || s.Substring(0, Length+1) == "for ")) ||
					(loop == "while" && s.Substring(0, Length+1) == "while(" || s.Substring(0, Length+1) == "while ") ||
					(loop == "do" && s.Substring(0, Length+1) == "do " || s.Substring(0, Length+1) == "do{" || s.Substring(0, Length+1) == "do;"))
				{
				}
				else
				{
					sb.Append(s.Substring(0, s.IndexOf(loop)+Length));
					s = s.Substring(s.IndexOf(loop)+Length);

					if(!s.Contains(loop))
						sb.Append(s);

					if(enabled)
						enabled = false;

					continue;
				}

				if(enabled)
					enabled = false;

				if(loop == "do")
					a = s.Remove(0, s.IndexOf("do"));
				else
					a = s.Remove(0, s.IndexOf(")"));

				if(a.IndexOf(";") == a.Length-1)
					a = a.Substring(1, a.IndexOf(";"));
				else
					a = a.Substring(1, a.IndexOf(";")+1);

				s = s.Substring(s.IndexOf(loop)+Length);

				if(a.Length > Length && a.Trim().Substring(0, Length) == loop)
				{
					s = s.Substring(s.IndexOf(loop));
					continue;
				}

				sb.Append(" " + loop);

				if(!a.Contains("{") && !a.Contains("if") && !a.Contains("switch"))
				{
					if(loop != "do")
						sb.Append(s.Substring(0, s.IndexOf(")")+1));

					if(Class && Schumix)
						sb.Append("{ Entry.asdcmxd++; if(Entry.asdcmxd >= 10000) break;");
					else
						sb.Append("{ asdcmxd++; if(asdcmxd >= 10000) break;");

					if(!s.Contains(loop))
					{
						if(loop == "do")
						{
							sb.Append(s.Substring(0, s.IndexOf(";")) + "; }" + s.Substring(s.IndexOf(";")+1));
							continue;
						}
						else
						{
							string x = s.Substring(s.IndexOf(")")+1);
							sb.Append(x.Substring(0, x.IndexOf(";")) + "; }" + x.Substring(x.IndexOf(";")+1));
							continue;
						}
					}
					else
					{
						if(loop != "do")
							s = s.Remove(0, s.IndexOf(")")+1);

						sb.Append(s.Substring(0, s.IndexOf(";")));
						sb.Append("; }");
						s = s.Remove(0, s.IndexOf(";")+1);
						sb.Append(s.Substring(0, s.IndexOf(loop)));
						s = s.Substring(s.IndexOf(loop));
						continue;
					}
				}

				sb.Append(s.Substring(0, s.IndexOf("{")));

				if(Class && Schumix)
					sb.Append("{ Entry.asdcmxd++; if(Entry.asdcmxd >= 10000) break;");
				else
					sb.Append("{ asdcmxd++; if(asdcmxd >= 10000) break;");

				if(!s.Contains(loop))
					sb.Append(s.Substring(s.IndexOf("{")+1));
				else
				{
					s = s.Remove(0, s.IndexOf("{")+1);
					sb.Append(s.Substring(0, s.IndexOf(loop)));
					s = s.Substring(s.IndexOf(loop));
				}
			}

			return sb.ToString();
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

			// Process
			if(data.Contains("System.Diagnostics.Process"))
			{
				Warning(channel);
				return true;
			}

			// Windows
			if(data.Contains("Microsoft.Win32"))
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
			if(data.Contains("Assembly.Load") || data.Contains("Assembly.ReflectionOnlyLoad"))
			{
				Warning(channel);
				return true;
			}

			// Schumix
			if(data.Contains("asdcmxd"))
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

					if(text.Contains("/*"))
						text = text.Remove(0, text.IndexOf("*/")+2);
					else
						sb.Append(text.Substring(text.IndexOf("*/")+2));
				}

				text = sb.ToString();
			}

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

			return text.Trim() == string.Empty ? "Console.Write(\":'(\");" : text;
		}

		private void Warning(string channel)
		{
			sSendMessage.SendCMPrivmsg(channel, sLManager.GetCommandText("compiler/warning", channel));
		}
	}
}