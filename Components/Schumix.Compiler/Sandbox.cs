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
using System.CodeDom.Compiler;
using System.Security;
using System.Security.Policy;
using System.Security.Permissions;
using System.Threading;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.CSharp;
using Schumix.Compiler.Platforms;

namespace Schumix.Compiler
{
	public class Sandbox : MarshalByRefObject
	{
		private readonly Regex DoRegex = new Regex(@"do\s*\{?\s*(?<content>.+)\s*\}?\s*while\s*\((?<while>.+)\s*\)");
		private readonly Regex WhileRegex = new Regex(@"while\s*\(\s*(?<lol>.*)\s*\)");
		private readonly Regex ForRegex = new Regex(@"for\s*\(\s*(?<lol>.*)\s*\)");
		private readonly Platform sPlatform = Singleton<Platform>.Instance;
		private readonly string[] ReferencedAssemblies;
		private readonly bool TreatWarningsAsErrors;
		private readonly string CompilerOptions;
		private readonly int WarningLevel;

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
			// None
		}

		public Sandbox(string[] rAssemblies, string cOptions, int wLevel, bool tErrors)
		{
			ReferencedAssemblies = rAssemblies;
			CompilerOptions = cOptions;
			WarningLevel = wLevel;
			TreatWarningsAsErrors = tErrors;
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
#pragma warning disable 612
#pragma warning disable 618
			var e = new Evidence(new object[] { new Zone(SecurityZone.NoZone) }, null);
#pragma warning restore 618
#pragma warning restore 612

			//Permission permission = SecurityManager.GetStandardSandbox(e);
			var permission = new PermissionSet(PermissionState.None);
			permission.AddPermission(new SecurityPermission(PermissionState.None) { Flags = SecurityPermissionFlag.Execution | SecurityPermissionFlag.ControlThread });
			permission.AddPermission(new FileIOPermission(PermissionState.None));
			permission.AddPermission(new ReflectionPermission(ReflectionPermissionFlag.RestrictedMemberAccess));
			permission.AddPermission(new EnvironmentPermission(PermissionState.None));
			permission.AddPermission(new RegistryPermission(PermissionState.None));

			var setup = new AppDomainSetup()
			{
				ApplicationBase = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
				DisallowBindingRedirects = true,
				DisallowCodeDownload = true,
				DisallowPublisherPolicy = true
			};

			return AppDomain.CreateDomain("Sandbox", e, setup, permission);
		}

		public static Sandbox CreateInstance(ref AppDomain sandbox)
		{
			sandbox = GetSandbox();
			return (Sandbox)sandbox.CreateInstance(Assembly.GetExecutingAssembly().FullName, typeof(Sandbox).FullName).Unwrap();
		}

		private CompilerParameters InitCompilerParameters()
		{
			var cparams = new CompilerParameters();
			cparams.GenerateExecutable = false;
			cparams.GenerateInMemory = false;

			foreach(var asm in ReferencedAssemblies)
				cparams.ReferencedAssemblies.Add(asm);

			cparams.CompilerOptions = CompilerOptions;
			cparams.WarningLevel = WarningLevel;
			cparams.TreatWarningsAsErrors = TreatWarningsAsErrors;
			return cparams;
		}

		public Assembly CompileCode(string code, ref string ErrorMessage)
		{
			try
			{
				if(sPlatform.IsLinux)
				{
#pragma warning disable 618
					var compiler = new CSharpCodeProvider().CreateCompiler();
#pragma warning restore 618
					return CompilerErrors(compiler.CompileAssemblyFromSource(InitCompilerParameters(), code), ref ErrorMessage);
				}
				else if(sPlatform.IsWindows)
				{
					var compiler = CodeDomProvider.CreateProvider("CSharp");
					return CompilerErrors(compiler.CompileAssemblyFromSource(InitCompilerParameters(), code), ref ErrorMessage);
				}

				return null;
			}
			catch(Exception)
			{
				return null;
			}
		}

		public Assembly CompilerErrors(CompilerResults results, ref string ErrorMessage)
		{
			if(results.Errors.HasErrors)
			{
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

				ErrorMessage = errormessage;
				return null;
			}
			else
				return results.CompiledAssembly;
		}

		public int StartCode(string Data, string MainConstructor, Abstract A, ref string ErrorMessage, ref int ErrorCode)
		{
			try
			{
				if(IsFor(Data) || IsDo(Data) || IsWhile(Data))
				{
					bool b = false;
					var thread = new Thread(() =>
					{
						try
						{
							A.GetType().InvokeMember(MainConstructor, BindingFlags.InvokeMethod | BindingFlags.Default, null, A, null);
							b = true;
						}
						catch(Exception)
						{
							// None
						}
					});

					thread.Start();
					thread.Join(100);
					thread.Abort();

					if(!b)
						return 0;
				}
				else
					A.GetType().InvokeMember(MainConstructor, BindingFlags.InvokeMethod | BindingFlags.Default, null, A, null);
			}
			catch(Exception e)
			{
				ErrorCode = 0;
				ErrorMessage = e.Message;
				return -1;
			}

			return 1;
		}
	}
}