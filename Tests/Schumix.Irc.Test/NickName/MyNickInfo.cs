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
using NUnit.Framework;

namespace Schumix.Irc.NickName.Test
{
	[TestFixture]
	public class MyNickInfoTest
	{
		[Test]
		public void Parse()
		{
			Assert.AreEqual(Parse(":Schumix"), "Schumix");
			Assert.AreEqual(Parse("~schumiX"), "schumiX");
			Assert.AreEqual(Parse("&scHumix"), "scHumix");
			Assert.AreEqual(Parse("@sChumix"), "sChumix");
			Assert.AreEqual(Parse("%schuMix"), "schuMix");
			Assert.AreEqual(Parse("+schumIx"), "schumIx");
			Assert.AreEqual(Parse("schumix"), "schumix");
			Assert.AreEqual(Parse("schUmix"), "schUmix");
			Assert.AreEqual(Parse(""), string.Empty);
		}

		public string Parse(string Name)
		{
			if(Name.Length < 1)
				return string.Empty;

			switch(Name.Substring(0, 1))
			{
				case ":":
				case "~":
				case "&":
				case "@":
				case "%":
				case "+":
					return Name.Remove(0, 1);
				default:
					return Name;
			}
		}
	}
}