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
using NUnit.Framework;
using Schumix.Irc.Util;

namespace Schumix.Irc.Util.Test
{
	[TestFixture]
	public class Rfc2812UtilTest
	{
		[Test]
		public void IsValidChannelName()
		{
			Assert.IsTrue(Rfc2812Util.IsValidChannelName("#schumix"));
			Assert.IsFalse(Rfc2812Util.IsValidChannelName(" "));
			Assert.IsFalse(Rfc2812Util.IsValidChannelName(""));
			Assert.IsFalse(Rfc2812Util.IsValidChannelName("	\t\t\t\t"));
			Assert.IsTrue(Rfc2812Util.IsValidChannelName("#scHHHHHHumix"));
			Assert.IsTrue(Rfc2812Util.IsValidChannelName("#sssssssssssssssssssssssssssssssssssssssssssssssss")); // 50
			Assert.IsFalse(Rfc2812Util.IsValidChannelName("#ssssssssssssssssssssssssssssssssssssssssssssssssss")); // 51
		}

		[Test]
		public void IsValidNick()
		{
			Assert.IsTrue(Rfc2812Util.IsValidNick("Schumix"));
			Assert.IsFalse(Rfc2812Util.IsValidNick("1schumix"));
			Assert.IsFalse(Rfc2812Util.IsValidNick("132342453446456"));
			Assert.IsFalse(Rfc2812Util.IsValidNick(" "));
			Assert.IsFalse(Rfc2812Util.IsValidNick(""));
			Assert.IsFalse(Rfc2812Util.IsValidNick("	\t\t\t\t"));
			Assert.IsTrue(Rfc2812Util.IsValidNick("[Schumix]"));
			Assert.IsTrue(Rfc2812Util.IsValidNick("`Schumix"));
			Assert.IsTrue(Rfc2812Util.IsValidNick("`[Schumix]"));
			Assert.IsTrue(Rfc2812Util.IsValidNick("^Schumix"));
			Assert.IsTrue(Rfc2812Util.IsValidNick("`Schu^mix"));
			Assert.IsTrue(Rfc2812Util.IsValidNick("`{Schu^mix}"));
		}

		[Test]
		public void IsServ()
		{
			Assert.IsTrue(Rfc2812Util.IsServ("NickServ"));
			Assert.IsTrue(Rfc2812Util.IsServ("ChanServ"));
			Assert.IsTrue(Rfc2812Util.IsServ("HostServ"));
			Assert.IsTrue(Rfc2812Util.IsServ("MemoServ"));
			Assert.IsFalse(Rfc2812Util.IsServ("schumix"));
		}

		[Test]
		public void IsServInLower()
		{
			Assert.IsTrue(Rfc2812Util.IsServInLower("nickServ"));
			Assert.IsTrue(Rfc2812Util.IsServInLower("ChanSErv"));
			Assert.IsTrue(Rfc2812Util.IsServInLower("HOSTSERV"));
			Assert.IsTrue(Rfc2812Util.IsServInLower("MemoServ"));
			Assert.IsFalse(Rfc2812Util.IsServInLower("schumix"));
		}

		[Test]
		public void IsServ2()
		{
			Assert.IsTrue(Rfc2812Util.IsServ("NickServ", Serv.NickServ));
			Assert.IsFalse(Rfc2812Util.IsServ("schumix", Serv.NickServ));
		}
	}
}