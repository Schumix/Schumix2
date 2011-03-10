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
using System.Text.RegularExpressions;
using Microsoft.CSharp;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;

namespace Schumix.CompilerPlugin.Commands
{
	public class Compiler : CommandInfo
	{
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;
		private readonly Regex regex = new Regex(@"^\{(?<code>.+)\}$");
		private readonly string Referenced = "using System; using System.Threading; using System.Reflection; using System.Linq; using System.Text; using System.Text.RegularExpressions;";

		protected void CompilerCommand()
		{
			try
			{
				CNick();
				string adat = string.Empty;
				string sablon = string.Empty;

				if(regex.IsMatch(Network.IMessage.Args))
					adat = regex.Match(Network.IMessage.Args).Groups["code"].ToString();

				if(Tiltas(adat))
					return;

				if(adat.IndexOf("Entry") == -1)
				{
					if(adat.IndexOf("Schumix") == -1)
						sablon = Referenced + " public class Entry { public void Schumix() { " + adat + " } }";
					else
						sablon = Referenced + " public class Entry { " + adat + " }";
				}
				else
				{
					if(adat.IndexOf("Schumix") == -1)
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nincs megadva a fő fv! (Schumix)");
						return;
					}

					sablon = Referenced + " " + adat;
				}

				var asm = CompileCode(sablon);
				if(asm == null)
					return;

				var writer = new StringWriter();
				Console.SetOut(writer);

				object o = asm.CreateInstance("Entry");
				var type = o.GetType();
				type.InvokeMember("Schumix", BindingFlags.InvokeMethod | BindingFlags.Default, null, o, null);

				if(writer.ToString().Length > 3000)
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "A kimeneti szöveg túl hosszú ezért nem került kiirásra!");
					var s = new StreamWriter(Console.OpenStandardOutput());
					s.AutoFlush = true;
					Console.SetOut(s);
					return;
				}

				string[] sorok = writer.ToString().Split('\n');

				if(sorok.Length < 2 && adat.IndexOf("Console.Write") == -1)
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "A kód sikeresen lefordult csak nincs kimenő üzenet!");

				if(sorok.Length > 4)
				{
					for(int x = 0; x < 4; x++)
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, sorok[x]);
	
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Hátramaradt még {0} kiirás!", sorok.Length-6);
				}
				else
				{
					for(int x = 0; x < sorok.Length; x++)
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, sorok[x]);
				}

				var sw = new StreamWriter(Console.OpenStandardOutput());
				sw.AutoFlush = true;
				Console.SetOut(sw);
			}
			catch(NotSupportedException)
			{
				// egyenlőre semmi
			}
		}

		private Assembly CompileCode(string code)
		{
			try
			{
				var provider = new CSharpCodeProvider();
				var compiler = provider.CreateCompiler();

				var cparams = new CompilerParameters();
				cparams.GenerateExecutable = false;
				cparams.GenerateInMemory = false;

				cparams.ReferencedAssemblies.Add("System.dll");
				//cparams.CompilerOptions = "";

				var results = compiler.CompileAssemblyFromSource(cparams, code);
				if(results.Errors.HasErrors)
				{
					foreach(CompilerError error in results.Errors)
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Errors: {0}", error.ErrorText);
				}
				else
					return results.CompiledAssembly;
			}
			catch(NotSupportedException)
			{
				// egyenlőre semmi
			}

			return null;
		}

		private bool Tiltas(string adat)
		{
			try
			{
				if(adat.IndexOf("Environment") != -1)
				{
					Figyelmeztetes();
					return true;
				}

				if(adat.IndexOf("System.IO") != -1)
				{
					Figyelmeztetes();
					return true;
				}

				if(adat.IndexOf("System.Diagnostics.Process") != -1)
				{
					Figyelmeztetes();
					return true;
				}

				if(adat.IndexOf("Microsoft.Win32") != -1)
				{
					Figyelmeztetes();
					return true;
				}

				if(adat.IndexOf("System.CodeDom") != -1)
				{
					Figyelmeztetes();
					return true;
				}

				if(adat.IndexOf("Console.SetOut") != -1)
				{
					Figyelmeztetes();
					return true;
				}

				if(adat.IndexOf("Console.Title") != -1)
				{
					Figyelmeztetes();
					return true;
				}
			}
			catch(NotSupportedException)
			{
				// egyenlőre semmi
			}

			return false;
		}

		private void Figyelmeztetes()
		{
			sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Elővigyázatosságból az egyik adat a kódsorban levan tiltva ezért nincs végeredmény!");
		}
	}
}