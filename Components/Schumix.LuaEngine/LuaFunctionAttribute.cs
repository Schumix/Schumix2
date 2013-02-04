/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Twl
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

namespace Schumix.LuaEngine
{
	/// <summary>
	/// Attribute used to mark functions which are exposed to the Lua VM.
	/// </summary>
	public sealed class LuaFunctionAttribute : Attribute
	{
		#region Private Members

		private readonly string _functionName;
		private readonly string _functionDoc;
		private readonly string[] _functionParameters;

		#endregion

		#region Properties

		/// <summary>
		/// Gets the name of the Lua function.
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
		/// Gets the function parameters.
		/// </summary>
		public string[] FunctionParameters
		{
			get { return _functionParameters; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a new attribute marking the function to be registered by the Lua VM.
		/// </summary>
		/// <param name="functionName">Name of the function. Will be used in Lua (does not have to be the same as the C# function name).</param>
		/// <param name="functionDoc">Function documentation</param>
		/// <param name="parameterDocs">Parameter documentation</param>
		public LuaFunctionAttribute(string functionName, string functionDoc, params string[] parameterDocs)
		{
			_functionName = functionName;
			_functionDoc = functionDoc;
			_functionParameters = parameterDocs;
		}

		/// <summary>
		/// Creates a new attribute marking the function to be registered by the Lua VM.
		/// </summary>
		/// <param name="functionName">Name of the function. Will be used in Lua (does not have to be the same as the C# function name).</param>
		/// <param name="functionDoc">Function documentation</param>
		public LuaFunctionAttribute(string functionName, string functionDoc)
		{
			_functionName = functionName;
			_functionDoc = functionDoc;
		}

		#endregion
	}
}