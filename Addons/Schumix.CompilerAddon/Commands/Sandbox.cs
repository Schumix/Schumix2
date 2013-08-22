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
using System.CodeDom.Compiler;
using System.Security;
using System.Security.Policy;
using System.Security.Permissions;
using System.Threading;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.CSharp;
using Schumix.Irc;
using Schumix.Framework;
using Schumix.Framework.Irc;
using Schumix.Framework.Logger;
using Schumix.Framework.Platforms;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using Schumix.CompilerAddon.Config;

namespace Schumix.CompilerAddon.Commands
{
	public class Sandbox : MarshalByRefObject
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly Platform sPlatform = Singleton<Platform>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private readonly Regex ForRegex = new Regex(@"for\s*\(\s*(?<lol>.*)\s*\)");
		private readonly Regex WhileRegex = new Regex(@"while\s*\(\s*(?<lol>.*)\s*\)");
		private readonly Regex DoRegex = new Regex(@"do\s*\{?\s*(?<content>.+)\s*\}?\s*while\s*\((?<while>.+)\s*\)");

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

		public Sandbox()
		{
			InitCompilerParameters();
		}

		public void TestIsFullyTrusted()
		{
			var ad = AppDomain.CurrentDomain;
			Console.WriteLine("\r\nApplication domain '{0}': IsFullyTrusted = {1}", ad.FriendlyName, ad.IsFullyTrusted);
			Console.WriteLine("   IsFullyTrusted = {0} for the current assembly", Assembly.GetExecutingAssembly().IsFullyTrusted);
			Console.WriteLine("   IsFullyTrusted = {0} for mscorlib", typeof(int).Assembly.IsFullyTrusted);
		}

		public static AppDomain GetSandbox()
		{
			// Create the permission set to grant to other assemblies.
			// In this case we are granting the permissions found in the LocalIntranet zone.
			//var e = new Evidence();
			//e.AddHostEvidence(new Zone(SecurityZone.Intranet));
			var e = new Evidence(new object[] { new Zone(SecurityZone.NoZone) }, null);

			//Permission permission = SecurityManager.GetStandardSandbox(e);
			var permission = new PermissionSet(PermissionState.None);
			permission.AddPermission(new SecurityPermission(PermissionState.None) { Flags = SecurityPermissionFlag.Execution });
			permission.AddPermission(new FileIOPermission(PermissionState.None));
			permission.AddPermission(new ReflectionPermission(ReflectionPermissionFlag.RestrictedMemberAccess));
			permission.AddPermission(new EnvironmentPermission(PermissionState.None));

			// Identify the folder to use for the sandbox.
			Console.WriteLine(Directory.GetCurrentDirectory());
			Console.WriteLine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase);
			Console.WriteLine(Path.Combine(Directory.GetCurrentDirectory(), "Addons"));

			string appBase = typeof(Sandbox).Assembly.CodeBase;

			//Added the following line
			appBase = new Uri(appBase).LocalPath;
			Console.WriteLine(Path.GetDirectoryName(appBase));

			var setup = new AppDomainSetup()
			{
				ApplicationBase = Path.Combine(Directory.GetCurrentDirectory(), "Addons"),
				DisallowBindingRedirects = true,
				DisallowCodeDownload = true,
				DisallowPublisherPolicy = true,
			};

			return AppDomain.CreateDomain("Sandbox", e, setup, permission);
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

		public Assembly CompileCode(string code, IRCMessage sIRCMessage)
		{
			try
			{
				if(sPlatform.IsLinux)
				{
					#pragma warning disable 618
					var compiler = new CSharpCodeProvider().CreateCompiler();
					#pragma warning restore 618
					return CompilerErrors(compiler.CompileAssemblyFromSource(InitCompilerParameters(), code), sIRCMessage);
				}
				else if(sPlatform.IsWindows)
				{
					var compiler = CodeDomProvider.CreateProvider("CSharp");
					return CompilerErrors(compiler.CompileAssemblyFromSource(InitCompilerParameters(), code), sIRCMessage);
				}

				return null;
			}
			catch(Exception)
			{
				return null;
			}
		}

		public Assembly CompilerErrors(CompilerResults results, IRCMessage sIRCMessage)
		{
			if(results.Errors.HasErrors)
			{
				var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;
				string errormessage = string.Empty;

				foreach(CompilerError error in results.Errors)
				{
					string errortext = error.ErrorText;

					if(errortext.Contains("Location of the symbol related to previous error"))
					{
						if(sPlatform.IsLinux)
						{
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

							errormessage += ". " + "/***/***/***/" + errortext.Substring(0, errortext.IndexOf(".dll")) + ".dll (Location of the symbol related to previous error)";
						}
						else if(sPlatform.IsWindows)
						{
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

							errormessage += ". " + "*:\\***\\***\\" + errortext.Substring(0, errortext.IndexOf(".dll")) + ".dll (Location of the symbol related to previous error)";
						}

						continue;
					}

					errormessage += ". " + errortext;
				}

				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("compiler/code", sIRCMessage.Channel, sIRCMessage.ServerName), errormessage.Remove(0, 2, ". "));
				return null;
			}
			else
				return results.CompiledAssembly;
		}

		public int StartCode(IRCMessage sIRCMessage, string data, object o)
		{
			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;

			try
			{
				if(IsFor(data) || IsDo(data) || IsWhile(data))
				{
					bool b = false;
					var thread = new Thread(() =>
					{
						try
						{
							o.GetType().InvokeMember(CompilerConfig.MainConstructor, BindingFlags.InvokeMethod | BindingFlags.Default, null, o, null); b = true;
						}
						catch(Exception e)
						{
							Log.Debug("CompilerThread2", sLConsole.GetString("Failure details: {0}"), e.Message);
						}
					});

					thread.Start();
					thread.Join(2);
					thread.Abort();

					if(!b)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("compiler/kill", sIRCMessage.Channel, sIRCMessage.ServerName));
						return -1;
					}
				}
				else
					o.GetType().InvokeMember(CompilerConfig.MainConstructor, BindingFlags.InvokeMethod | BindingFlags.Default, null, o, null);
			}
			catch(Exception e)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("compiler/code", sIRCMessage.Channel, sIRCMessage.ServerName), e.Message);
				return -1;
			}

			return 1;
		}
	}
}