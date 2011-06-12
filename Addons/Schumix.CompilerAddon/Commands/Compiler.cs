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
						template = CompilerConfig.Referenced + " public class Entry { public void Schumix() { int asdcmxd = 0; " + Loop(data, false, false) + " } }";
					else
						template = CompilerConfig.Referenced + " public class Entry { private int asdcmxd = 0; " + Loop(data, false, true) + " }";
				}
				else if(IsEntry(data))
				{
					if(!IsSchumix(data))
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
						return;
					}

					template = CompilerConfig.Referenced + " " + Loop(data, true, true);
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

				if(writer.ToString().Length > 3000)
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
				var results = compiler.CompileAssemblyFromSource(InitCompilerParameters(), code);

				if(results.Errors.HasErrors)
				{
					foreach(CompilerError error in results.Errors)
						sSendMessage.SendCMPrivmsg(channel, sLManager.GetCommandText("compiler/code", channel), error.ErrorText);
				}
				else
					return results.CompiledAssembly;
			}
			catch(Exception)
			{
				// egyenlőre semmi
			}

			return null;
		}

		private string Loop(string data, bool Class, bool Schumix)
		{
			bool enabled = false;

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
			{
				string s = data, a = string.Empty;
				var sb = new StringBuilder();
				sb.Append(s.Substring(0, s.IndexOf("for")));
				s = s.Remove(0, s.IndexOf("for"));
				enabled = true;

				for(;;)
				{
					if(!s.Contains("for") && !enabled)
						break;

					if(s.Length > 4 && (s.Substring(0, 4) == "for(" || s.Substring(0, 4) == "for "))
					{
					}
					else
					{
						sb.Append(s.Substring(0, s.IndexOf("for")+3));
						s = s.Substring(s.IndexOf("for")+3);

						if(!s.Contains("for"))
							sb.Append(s);

						if(enabled)
							enabled = false;

						continue;
					}

					if(enabled)
						enabled = false;

					a = s.Remove(0, s.IndexOf(")"));

					if(a.IndexOf(";") == a.Length-1)
						a = a.Substring(1, a.IndexOf(";"));
					else
						a = a.Substring(1, a.IndexOf(";")+1);

					s = s.Substring(s.IndexOf("for")+3);

					if(a.Length > 3 && a.Trim().Substring(0, 3) == "for")
					{
						s = s.Substring(s.IndexOf("for"));
						continue;
					}

					sb.Append(" for");

					if(!a.Contains("{") && !a.Contains("if") && !a.Contains("switch"))
					{
						sb.Append(s.Substring(0, s.IndexOf(")")+1));

						if(Class && Schumix)
							sb.Append("{ Entry.asdcmxd++; if(Entry.asdcmxd >= 10000) break;");
						else
							sb.Append("{ asdcmxd++; if(asdcmxd >= 10000) break;");

						if(!s.Contains("for"))
						{
							string x = s.Substring(s.IndexOf(")")+1);
							sb.Append(x.Substring(0, x.IndexOf(";")) + "; }" + x.Substring(x.IndexOf(";")+1));
							continue;
						}
						else
						{
							s = s.Remove(0, s.IndexOf(")")+1);
							sb.Append(s.Substring(0, s.IndexOf(";")));
							sb.Append("; }");
							s = s.Remove(0, s.IndexOf(";")+1);
							sb.Append(s.Substring(0, s.IndexOf("for")));
							s = s.Substring(s.IndexOf("for"));
							continue;
						}
					}

					sb.Append(s.Substring(0, s.IndexOf("{")));

					if(Class && Schumix)
						sb.Append("{ Entry.asdcmxd++; if(Entry.asdcmxd >= 10000) break;");
					else
						sb.Append("{ asdcmxd++; if(asdcmxd >= 10000) break;");

					if(!s.Contains("for"))
						sb.Append(s.Substring(s.IndexOf("{")+1));
					else
					{
						s = s.Remove(0, s.IndexOf("{")+1);
						sb.Append(s.Substring(0, s.IndexOf("for")));
						s = s.Substring(s.IndexOf("for"));
					}
				}

				data = sb.ToString();
			}

			if(IsDo(data))
			{
				string s = data, a = string.Empty;
				var sb = new StringBuilder();
				sb.Append(s.Substring(0, s.IndexOf("do")));
				s = s.Remove(0, s.IndexOf("do"));
				enabled = true;

				for(;;)
				{
					if(!s.Contains("do") && !enabled)
						break;

					if(s.Length > 3 && (s.Substring(0, 3) == "do " || s.Substring(0, 3) == "do{" || s.Substring(0, 3) == "do;"))
					{
					}
					else
					{
						sb.Append(s.Substring(0, s.IndexOf("do")+2));
						s = s.Substring(s.IndexOf("do")+2);

						if(!s.Contains("do"))
							sb.Append(s);

						if(enabled)
							enabled = false;

						continue;
					}

					if(enabled)
						enabled = false;

					a = s.Remove(0, s.IndexOf("do"));

					if(a.IndexOf(";") == a.Length-1)
						a = a.Substring(1, a.IndexOf(";"));
					else
						a = a.Substring(1, a.IndexOf(";")+1);

					s = s.Substring(s.IndexOf("do")+2);

					if(a.Length > 2 && a.Trim().Substring(0, 2) == "do")
					{
						s = s.Substring(s.IndexOf("do"));
						continue;
					}

					sb.Append(" do");

					if(!a.Contains("{") && !a.Contains("if") && !a.Contains("switch"))
					{
						if(Class && Schumix)
							sb.Append("{ Entry.asdcmxd++; if(Entry.asdcmxd >= 10000) break;");
						else
							sb.Append("{ asdcmxd++; if(asdcmxd >= 10000) break;");

						if(!s.Contains("do"))
						{
							sb.Append(s.Substring(0, s.IndexOf(";")) + "; }" + s.Substring(s.IndexOf(";")+1));
							continue;
						}
						else
						{
							sb.Append(s.Substring(0, s.IndexOf(";")));
							sb.Append("; }");
							s = s.Remove(0, s.IndexOf(";")+1);
							sb.Append(s.Substring(0, s.IndexOf("do")));
							s = s.Substring(s.IndexOf("do"));
							continue;
						}
					}

					sb.Append(s.Substring(0, s.IndexOf("{")));

					if(Class && Schumix)
						sb.Append("{ Entry.asdcmxd++; if(Entry.asdcmxd >= 10000) break;");
					else
						sb.Append("{ asdcmxd++; if(asdcmxd >= 10000) break;");

					if(!s.Contains("do"))
						sb.Append(s.Substring(s.IndexOf("{")+1));
					else
					{
						s = s.Remove(0, s.IndexOf("{")+1);
						sb.Append(s.Substring(0, s.IndexOf("do")));
						s = s.Substring(s.IndexOf("do"));
					}
				}

				data = sb.ToString();
			}

			if(IsWhile(data))
			{
				string s = data, a = string.Empty;
				var sb = new StringBuilder();
				sb.Append(s.Substring(0, s.IndexOf("while")));
				a = s.Substring(0, s.IndexOf("while"));
				s = s.Remove(0, s.IndexOf("while"));
				enabled = true;

				for(;;)
				{
					if(!s.Contains("while") && !enabled)
						break;

					// TODO: Nem kezeli a do{...while(true);..}while(true); kódot.
					//       Figyelmen kivül haggya ezért használhatattlan lesz amit kapunk végeredménynek.
					//       Ha ilyen formában használnánk ajánlott a for-t használni.
					//       A végtelen ciklus elleni védelmet megkerülni nem lehet elvileg csak rossz kódot épit fel.
					//       Ezért amig nem lesz rá javítás ilyen formában használni nem ajánlott.
					if(enabled && a.Contains("do") && (a.Contains(";do") || a.Contains("do;") || a.Contains(" do ") ||
						a.Contains(" do") || a.Contains("do ")))
					{
						s = s.Substring(s.IndexOf("while")+5);
						sb.Append(" while");

						if(s.Contains("while"))
						{
							sb.Append(s.Substring(0, s.IndexOf("while")));
							s = s.Substring(s.IndexOf("while"));
						}
						else
							sb.Append(s);

						if(enabled)
							enabled = false;

						continue;
					}

					if(s.Length > 6 && (s.Substring(0, 6) == "while(" || s.Substring(0, 6) == "while "))
					{
					}
					else
					{
						sb.Append(s.Substring(0, s.IndexOf("while")+5));
						s = s.Substring(s.IndexOf("while")+5);

						if(!s.Contains("while"))
							sb.Append(s);

						if(enabled)
							enabled = false;

						continue;
					}

					if(enabled)
						enabled = false;

					a = s.Remove(0, s.IndexOf(")"));

					if(a.IndexOf(";") == a.Length-1)
						a = a.Substring(1, a.IndexOf(";"));
					else
						a = a.Substring(1, a.IndexOf(";")+1);

					s = s.Substring(s.IndexOf("while")+5);

					if(a.Length > 5 && a.Trim().Substring(0, 5) == "while")
					{
						s = s.Substring(s.IndexOf("while"));
						continue;
					}

					sb.Append(" while");

					if(!a.Contains("{") && !a.Contains("if") && !a.Contains("switch"))
					{
						sb.Append(s.Substring(0, s.IndexOf(")")+1));

						if(Class && Schumix)
							sb.Append("{ Entry.asdcmxd++; if(Entry.asdcmxd >= 10000) break;");
						else
							sb.Append("{ asdcmxd++; if(asdcmxd >= 10000) break;");

						if(!s.Contains("while"))
						{
							string x = s.Substring(s.IndexOf(")")+1);
							sb.Append(x.Substring(0, x.IndexOf(";")) + "; }" + x.Substring(x.IndexOf(";")+1));
							continue;
						}
						else
						{
							s = s.Remove(0, s.IndexOf(")")+1);
							sb.Append(s.Substring(0, s.IndexOf(";")));
							sb.Append("; }");
							s = s.Remove(0, s.IndexOf(";")+1);
							sb.Append(s.Substring(0, s.IndexOf("while")));
							s = s.Substring(s.IndexOf("while"));
							continue;
						}
					}

					sb.Append(s.Substring(0, s.IndexOf("{")));

					if(Class && Schumix)
						sb.Append("{ Entry.asdcmxd++; if(Entry.asdcmxd >= 10000) break;");
					else
						sb.Append("{ asdcmxd++; if(asdcmxd >= 10000) break;");

					if(!s.Contains("while"))
						sb.Append(s.Substring(s.IndexOf("{")+1));
					else
					{
						s = s.Remove(0, s.IndexOf("{")+1);
						sb.Append(s.Substring(0, s.IndexOf("while")));
						s = s.Substring(s.IndexOf("while"));
					}
				}

				data = sb.ToString();
			}

			return data;
		}

		private bool Ban(string data, string channel)
		{
			// Environment
			if(data.Contains("Environment"))
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

		private void Warning(string channel)
		{
			sSendMessage.SendCMPrivmsg(channel, sLManager.GetCommandText("compiler/warning", channel));
		}

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
	}
}