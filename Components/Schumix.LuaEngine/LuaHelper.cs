/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Twl
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
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Schumix.Framework;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using LuaInterface;

namespace Schumix.LuaEngine
{
	/// <summary>
	/// Helper functions for the LuaEngine implementation.
	/// </summary>
	public static class LuaHelper
	{
		private static readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;

		/// <summary>
		/// Registers Lua functions found in the specified target.
		/// </summary>
		/// <param name="luaFunctions">Global lua function table.</param>
		/// <param name="target">Object (class,struct) to search in.</param>
		/// <param name="vm">The Lua virtual machine.</param>
		public static void RegisterLuaFunctions(Lua vm, ref Dictionary<string, LuaFunctionDescriptor> luaFunctions, object target)
		{
			if(vm.IsNull() || luaFunctions.IsNull())
				return;

			var type = target.GetType();

			foreach(var method in type.GetMethods())
			{
				foreach(var attribute in Attribute.GetCustomAttributes(method))
				{
					if(attribute.GetType() == typeof(LuaFunctionAttribute))
					{
						var attr = (LuaFunctionAttribute)attribute;
						var parameters = new List<string>();
						var paramInfo = method.GetParameters();

						if(!attr.FunctionParameters.IsNull() && paramInfo.Length != attr.FunctionParameters.Length)
						{
							Log.Error("LuaHelper", sLConsole.LuaHelper("Text"), method.Name, attr.FunctionName,
								attr.FunctionParameters.Length, paramInfo.Length);
							break;
						}

						// build parameter doc hashtable.
						if(!attr.FunctionParameters.IsNull())
							parameters.AddRange(paramInfo.Select((t, i) => string.Format("{0} - {1}", t.Name, attr.FunctionParameters[i])));

						var descriptor = new LuaFunctionDescriptor(attr.FunctionName, attr.FunctionDocumentation, parameters);
						luaFunctions.Add(attr.FunctionName, descriptor);
						vm.RegisterFunction(attr.FunctionName, target, method);
					}
				}
			}
		}

		/// <summary>
		/// Handles Lua irc commands, especially interpreting.
		/// </summary>
		/// <param name="vm">Lua virtual machine</param>
		/// <param name="chan">Channel name.</param>
		/// <param name="msg">Message sent.</param>
		public static void HandleLuaCommands(Lua vm, string chan, string msg)
		{
			var regex = new Regex(@"lua\s*\{\s*(?<lcode>.+)\s*\}");

			if(!regex.IsMatch(msg))
				return;

			var code = regex.Match(msg).Groups["lcode"].ToString();

			try
			{
				vm.DoString(code);
			}
			catch(Exception x)
			{
				Log.Error(sLConsole.LuaHelper("Text2"), x.Message);
			}
		}
	}
}