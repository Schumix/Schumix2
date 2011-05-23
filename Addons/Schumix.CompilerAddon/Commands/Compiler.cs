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

#if MONO
		private readonly string Referenced = "using System; using System.Threading; using System.Reflection; using System.Linq; " +
		 	"using System.Collections.Generic; using System.Text; using System.Text.RegularExpressions; using Schumix.Libraries;";
#else
		private readonly string Referenced = "using System; using System.Threading; using System.Reflection; " +
		 	"using System.Collections.Generic; using System.Text; using System.Text.RegularExpressions; using Schumix.Libraries;";
#endif

		protected void CompilerCommand(IRCMessage sIRCMessage, bool command)
		{
			try
			{
				CNick(sIRCMessage);
				var text = sLManager.GetCommandTexts("compiler", sIRCMessage.Channel);
				if(text.Length < 4)
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

				data = Loop(data);

				if((!data.Contains("Entry") && !data.Contains("class")) || (!data.Contains("Entry") && data.Contains("class")) || (data.Contains("Entry") && !data.Contains("class")))
				{
					if(!data.Contains("Schumix"))
						template = Referenced + " public class Entry { public void Schumix() { " + data + " } }";
					else
						template = Referenced + " public class Entry { " + data + " }";
				}
				else
				{
					if(!data.Contains("Schumix"))
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
						return;
					}

					template = Referenced + " " + data;
				}

				var asm = CompileCode(template, sIRCMessage.Channel);
				if(asm.IsNull())
					return;

				var writer = new StringWriter();
				Console.SetOut(writer);

				object o = asm.CreateInstance("Entry");
				var type = o.GetType();
				type.InvokeMember("Schumix", BindingFlags.InvokeMethod | BindingFlags.Default, null, o, null);

				if(writer.ToString().Length > 3000)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1]);
					return;
				}

				string[] lines = writer.ToString().Split('\n');

				if(lines.Length < 2 && data.IndexOf("Console.Write") == -1)
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[2]);

				if(lines.Length > 4)
				{
					for(int x = 0; x < 4; x++)
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, lines[x]);
	
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[3], lines.Length-6);
				}
				else
				{
					for(int x = 0; x < lines.Length; x++)
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, lines[x]);
				}
			}
			catch(Exception)
			{
				// egyenlőre semmi
			}
		}

		private Assembly CompileCode(string code, string channel)
		{
			try
			{
#if MONO
#pragma warning disable 618
				var provider = new CSharpCodeProvider();
				var compiler = provider.CreateCompiler();
#pragma warning restore 618
#else
				var compiler = CodeDomProvider.CreateProvider("CSharp");
#endif

				var cparams = new CompilerParameters();
				cparams.GenerateExecutable = false;
				cparams.GenerateInMemory = false;

				cparams.ReferencedAssemblies.Add("System.dll");
				cparams.ReferencedAssemblies.Add("Schumix.Libraries.dll");
				cparams.CompilerOptions = CompilerConfig.CompilerOptions;
				cparams.WarningLevel = CompilerConfig.WarningLevel;
				cparams.TreatWarningsAsErrors = CompilerConfig.TreatWarningsAsErrors;

				var results = compiler.CompileAssemblyFromSource(cparams, code);
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

		private string Loop(string data)
		{
			if(data.Contains("for"))
			{
				string s = data;
				int i = s.IndexOf("for");
				var sb = new StringBuilder();

				sb.Append(s.Substring(0, i));
				sb.Append(" int asdcmxd = 0; for");
				s = s.Substring(s.IndexOf("for")+3);

				if(s.Contains("{"))
				{
					sb.Append(s.Substring(0, s.IndexOf("{")));

					if(!s.Contains("for"))
						sb.Append("{ asdcmxd++; if(asdcmxd == 10000) return;");
					else
						sb.Append("{ return;");

					sb.Append(s.Substring(s.IndexOf("{")+1));
				}
				else
				{
					sb.Append(s.Substring(0, s.IndexOf(")")+1));

					if(!s.Contains("for"))
						sb.Append(" { asdcmxd++; if(asdcmxd == 10000) return;");
					else
						sb.Append(" { return;");

					sb.Append(s.Substring(s.IndexOf(")")+1));
					sb.Append("}");
				}

				return sb.ToString();
			}
			else if(data.Contains("do") && data.Contains("while"))
			{
				string s = data;
				int i = s.IndexOf("do");
				var sb = new StringBuilder();

				sb.Append(s.Substring(0, i));
				sb.Append(" int asdcmxd = 0; do");
				s = s.Substring(s.IndexOf("do")+2);

				if(s.Contains("{"))
				{
					sb.Append(s.Substring(0, s.IndexOf("{")));

					if(!s.Contains("do"))
						sb.Append("{ asdcmxd++; if(asdcmxd == 10000) return;");
					else
						sb.Append("{ return;");

					sb.Append(s.Substring(s.IndexOf("{")+1));
				}
				else
				{
					sb.Append(s.Substring(0, s.IndexOf(")")+1));

					if(!s.Contains("do"))
						sb.Append(" { asdcmxd++; if(asdcmxd == 10000) return;");
					else
						sb.Append(" { return;");

					sb.Append(s.Substring(s.IndexOf(")")+1));
					sb.Append("}");
				}

				return sb.ToString();
			}
			else if(data.Contains("while"))
			{
				string s = data;
				int i = s.IndexOf("while");
				var sb = new StringBuilder();

				sb.Append(s.Substring(0, i));
				sb.Append(" int asdcmxd = 0; while");
				s = s.Substring(s.IndexOf("while")+5);

				if(s.Contains("{"))
				{
					sb.Append(s.Substring(0, s.IndexOf("{")));

					if(!s.Contains("while"))
						sb.Append("{ asdcmxd++; if(asdcmxd == 10000) return;");
					else
						sb.Append("{ return;");

					sb.Append(s.Substring(s.IndexOf("{")+1));
				}
				else
				{
					sb.Append(s.Substring(0, s.IndexOf(")")+1));

					if(!s.Contains("while"))
						sb.Append(" { asdcmxd++; if(asdcmxd == 10000) return;");
					else
						sb.Append(" { return;");

					sb.Append(s.Substring(s.IndexOf(")")+1));
					sb.Append("}");
				}

				return sb.ToString();
			}

			return data;
		}

		private bool Ban(string data, string channel)
		{
			if(data.Contains("Environment"))
			{
				Warning(channel);
				return true;
			}

			if(data.Contains("System.IO"))
			{
				Warning(channel);
				return true;
			}

			if(data.Contains("System.Diagnostics.Process"))
			{
				Warning(channel);
				return true;
			}

			if(data.Contains("Microsoft.Win32"))
			{
				Warning(channel);
				return true;
			}

			if(data.Contains("System.CodeDom"))
			{
				Warning(channel);
				return true;
			}

			if(data.Contains("Console.SetOut"))
			{
				Warning(channel);
				return true;
			}

			if(data.Contains("Console.Title"))
			{
				Warning(channel);
				return true;
			}

			if(data.Contains("System.Net.Dns"))
			{
				Warning(channel);
				return true;
			}

			if(data.Contains("System.Net.IPAddress"))
			{
				Warning(channel);
				return true;
			}

			if(data.Contains("System.Net.IPEndPoint"))
			{
				Warning(channel);
				return true;
			}

			if(data.Contains("System.Net.IPHostEntry"))
			{
				Warning(channel);
				return true;
			}

			if(data.Contains("using") && data.Contains("System.Net"))
			{
				Warning(channel);
				return true;
			}

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
	}
}