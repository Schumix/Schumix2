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

namespace Schumix
{
	public class Eval
	{

		// simulate basic evaluation of arithmetic expression
		public Eval()
		{

		}

        /// <summary>
        ///     {VerifyAllowed függvény leírása}
        /// </summary>
        /// <param name="e">{e leírása}</param>
        /// <returns>
        ///     Ha a függvény igaz, akkor "true" értékkel tér vissza,
        ///     ha a függvény hamis, akkor "false" értékkel tér vissza.
        /// </returns>
		public static bool VerifyAllowed(string e)
		{
			string allowed = "0123456789+-*/().,";

			for(int i = 0; i < e.Length; i++)
			{
				if(allowed.IndexOf(""+e[i]) == -1)
					return false;
			}

			return true;
		}

        /// <summary>
        ///     {Evaluate függvény leírása}
        /// </summary>
        /// <param name="e">{e leírása}</param>
        /// <returns>
        ///     
        /// </returns>
		public static string Evaluate(string e)
		{
			if(e.Length == 0)
				return "String length is zero";

			if(!VerifyAllowed(e))
				return "The string contains not allowed characters";

			if(e[0] == '-')
				e = "0" + e;

			string res = "";

			try
			{
				res = Calculate(e).ToString();
			}
			catch
			{
				return "The call caused an exception";
			}

			return res;
		}

        /// <summary>
        ///     
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
		public static double Calculate(string e)
		{
			e = e.Replace(".", ",");
			if(e.IndexOf("(") != -1)
			{
				int a = e.LastIndexOf("(");
				int b = e.IndexOf(")", a);
				double middle = Calculate(e.Substring(a+1, b-a-1));
				return Calculate(e.Substring(0, a) + middle.ToString() + e.Substring(b+1));
			}

			double result = 0;
			string[] plus = e.Split('+');

			if(plus.Length > 1)
			{
				// there were some +
				result = Calculate(plus[0]);

				for(int i = 1; i < plus.Length; i++)
					result += Calculate(plus[i]);

				return result;
			}
			else
			{
				// no +
				string[] minus = plus[0].Split('-');

				if(minus.Length > 1)
				{
					// there were some -
					result = Calculate(minus[0]);

					for(int i = 1; i < minus.Length; i++)
						result -= Calculate(minus[i]);

					return result;
				}
				else
				{
					// no -
					string[] mult = minus[0].Split('*');

					if(mult.Length > 1)
					{
						// there were some *
						result = Calculate(mult[0]);

						for(int i = 1; i < mult.Length; i++)
							result *= Calculate(mult[i]);

						return result;
					}
					else
					{
						// no *
						string[] div = mult[0].Split('/');

						if(div.Length > 1)
						{
							// there were some /
							result = Calculate(div[0]);

							for(int i = 1; i < div.Length; i++)
								result /= Calculate(div[i]);

							return result;
						}
						else
							return double.Parse(e);
					}
				}
			}
		}
	}
}