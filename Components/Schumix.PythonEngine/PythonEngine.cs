/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Megax <http://www.megaxx.info/>
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
using System.Collections.Generic;
using IronPython.Hosting;
using IronPython.Runtime.Operations;
using Microsoft.CSharp;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting.Runtime;
using Schumix.API.Irc;
using Schumix.API.Delegate;

namespace Schumix.PythonEngine
{
	public sealed class PythonEngine
	{
		private readonly string _scriptPath;
		private ScriptEngine engine;
		private ScriptSource source;
		private ScriptScope scope;

		public PythonEngine(string scriptsPath)
		{
			Console.WriteLine("start");
			_scriptPath = scriptsPath;
			LoadScripts();
		}

		public void LoadScripts(bool reload = false)
		{
			engine = Python.CreateEngine();

string path = Assembly.GetExecutingAssembly().Location;
string dir = Directory.GetParent(path).FullName;
string libPath = Path.Combine(dir,"Schumix.API.dll");

Assembly assembly = Assembly.LoadFile(libPath);
engine.Runtime.LoadAssembly(assembly);
			//engine.Runtime.LoadAssembly(typeof(IRCMessage).Assembly);
			//engine.Runtime.LoadAssembly(typeof(BotMessage).Assembly);
			//engine.Runtime.LoadAssembly(typeof(IRCResponse).Assembly);
			//engine.Runtime.LoadAssembly(typeof(Settings).Assembly);

			ICollection<string> paths = engine.GetSearchPaths();
			paths.Add(_scriptPath);
			paths.Add(Path.Combine(_scriptPath, "Libs"));
			engine.SetSearchPaths(paths);

			scope = engine.CreateScope();
			source = engine.CreateScriptSourceFromFile(Path.Combine(_scriptPath, "asd.py"));

			try
			{
			    source.Execute(scope);
			}
			catch(Exception e)
			{
			    Log(e);
			}

			var ss = new IRCMessage() { Nick = "asdddd" };
			try
			{
				engine.Operations.Invoke(scope.GetVariable("RegisterCommand"), ss);
			}
			catch(Exception e)
			{
				Log(e);
			}
        }

        private void Log(Exception e)
        {
			Console.WriteLine("Python Error: " + e.Message);
			DynamicStackFrame[] frames = PythonOps.GetDynamicStackFrames(e);
			foreach (DynamicStackFrame frame in frames)
			    Console.WriteLine("\t" +
			        frame.GetFileName() + " " +
			        frame.GetFileLineNumber() + " " +
			        frame.GetMethodName());
        }

		/// <summary>
		/// Frees up resources.
		/// </summary>
		public void Free()
		{

		}
	}
}