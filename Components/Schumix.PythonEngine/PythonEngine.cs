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
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using IronPython.Hosting;
using IronPython.Runtime.Operations;
using Microsoft.CSharp;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting.Runtime;
using Schumix.API.Irc;
using Schumix.API.Delegate;
using Schumix.Irc;
using Schumix.Framework;

namespace Schumix.PythonEngine
{
	public sealed class PythonEngine
	{
		private Dictionary<ScriptSource, ScriptScope> _list = new Dictionary<ScriptSource, ScriptScope>();
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private readonly FileSystemWatcher _watcher;
		private readonly object Lock = new object();
		private readonly string _scriptPath;
		private ScriptEngine _engine;

		public PythonEngine(string scriptsPath)
		{
			Console.WriteLine("start");
			_scriptPath = scriptsPath;
			_engine = Python.CreateEngine();

			string dir = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName;
			var list = new List<string>();
			list.Add(Path.Combine(dir, "Schumix.API.dll"));
			list.Add(Path.Combine(dir, "Schumix.Irc.dll"));
			list.Add(Path.Combine(dir, "Schumix.Framework.dll"));

			foreach(var l in list)
				_engine.Runtime.LoadAssembly(Assembly.LoadFile(l));

			list.Clear();

			var paths = _engine.GetSearchPaths();
			paths.Add(_scriptPath);
			paths.Add(Path.Combine(_scriptPath, "Libs"));
			_engine.SetSearchPaths(paths);

			LoadScripts();

			_watcher = new FileSystemWatcher(_scriptPath)
			{
				NotifyFilter = NotifyFilters.FileName | NotifyFilters.Attributes | NotifyFilters.LastAccess |
				NotifyFilters.LastWrite | NotifyFilters.Security | NotifyFilters.Size,
				EnableRaisingEvents = true
			};

			_watcher.Created += (s, e) => LoadScripts(true);
			_watcher.Changed += (s, e) => LoadScripts(true);
			_watcher.Deleted += (s, e) => LoadScripts(true);
			_watcher.Renamed += (s, e) => LoadScripts(true);
		}

		public void LoadScripts(bool reload = false)
		{
			lock(Lock)
			{
				if(reload)
				{
					foreach(var list in _list)
					{
						try
						{
							_engine.Operations.Invoke(list.Value.GetVariable("Destroy"), sIrcBase);
						}
						catch(Exception e)
						{
							Error(e);
						}
					}
				}

				_list.Clear();
				var di = new DirectoryInfo(_scriptPath);

				foreach(var file in di.GetFiles("*.py").AsParallel())
				{
					bool error = false;
					var scope = _engine.CreateScope();
					var source = _engine.CreateScriptSourceFromFile(file.FullName);
					Log.Notice("PythonEngine", /*sLConsole.LuaEngine("Text2")*/ "{0}", file.Name);

					try
					{

						if(source.GetCode().Contains("def Setup(IrcBase):") && source.GetCode().Contains("def Destroy(IrcBase):"))
						{
							source.Execute(scope);
							_engine.Operations.Invoke(scope.GetVariable("Setup"), sIrcBase);
						}
						else
							error = true;
					}
					catch(Exception e)
					{
						error = true;
						//Log.Error("PythonEngine", /*sLConsole.LuaEngine("Text3")*/ "{0} {1}", file.Name, x.Message);
						Error(e);
					}

					if(!error)
						_list.Add(source, scope);
				}
			}
        }

        private void Error(Exception e)
        {
			Console.WriteLine("Python Error: " + e.Message);
			foreach(var frame in PythonOps.GetDynamicStackFrames(e))
			    Console.WriteLine("\t" + frame.GetFileName() + " " + frame.GetFileLineNumber() + " " + frame.GetMethodName());
        }

		/// <summary>
		/// Frees up resources.
		/// </summary>
		public void Free()
		{
			_list.Clear();
		}
	}
}