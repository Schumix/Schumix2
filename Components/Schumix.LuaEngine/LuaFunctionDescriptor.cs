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

using System.Collections.Generic;
using Schumix.Framework;

namespace Schumix.LuaEngine
{
	/// <summary>
	/// Class used to describe and register Lua functions. Mainly related to attribute-based registering.
	/// </summary>
	public sealed class LuaFunctionDescriptor
	{
		#region Private Members

		private readonly string _functionName;
		private readonly string _functionDoc;
		private readonly List<string> _functionParameters = new List<string>();
		private readonly string _functionDocString;

		#endregion

		#region Properties

		/// <summary>
		/// Gets the function name.
		/// </summary>
		public string FunctionName
		{
			get { return _functionName; }
		}

		/// <summary>
		/// Gets the function documentation.
		/// </summary>
		public string FunctionDocumentation
		{
			get { return _functionDoc; }
		}

		/// <summary>
		/// Gets the function's parameters
		/// </summary>
		public List<string> FunctionParameters
		{
			get { return _functionParameters; }
		}

		/// <summary>
		/// Gets the function header.
		/// </summary>
		public string FunctionHeader
		{
			get
			{
				if(_functionDocString.IndexOf(SchumixBase.NewLine) == -1)
					return _functionDocString;

				return _functionDocString.Substring(0, _functionDocString.IndexOf(SchumixBase.NewLine));
			}
		}

		/// <summary>
		/// Gets the function's full documentation.
		/// </summary>
		public string FullFunctionDocumentation
		{
			get { return _functionDocString; }
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Creates a new instance of <c>LuaFunctionDescriptor</c>
		/// </summary>
		/// <param name="functionName">The Lua functio name.</param>
		/// <param name="functionDoc">The function documentation.</param>
		/// <param name="functionParams">The function's parameters.</param>
		public LuaFunctionDescriptor(string functionName, string functionDoc, List<string> functionParams)
		{
			_functionName = functionName;
			_functionDoc = functionDoc;
			_functionParameters = functionParams;

			var functionHeader = string.Format("{0}(%params%) - {1}", _functionName, _functionDoc);
			var functionBody = "\n\n";
			var funcParams = string.Empty;
			var first = true;

			foreach(var param in _functionParameters)
			{
				if(!first)
					funcParams += ", ";

				funcParams += param;
				functionBody += string.Format("\t{0}\n", param);
				first = false;
			}

			_functionDocString = functionHeader.Replace("%params%", funcParams) + functionBody;
		}

		#endregion
	}
}