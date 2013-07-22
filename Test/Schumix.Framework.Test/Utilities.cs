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
using Schumix.Framework;
using Schumix.Framework.Platforms;

namespace Schumix.Framework.Test
{
	[TestFixture]
	public class UtilitiesTest
	{
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly Platform sPlatform = Singleton<Platform>.Instance;

		[Test]
		public void Sha1()
		{
			Assert.AreEqual(sUtilities.Sha1("asd"), "f10e2821bbbea527ea02200352313bc059445190");
			Assert.AreEqual(sUtilities.Sha1("áéűúő"), "5b9b5b33c24c476bbea6cc7528429eca4b3dea84");
		}

		[Test]
		public void Md5()
		{
			Assert.AreEqual(sUtilities.Md5("asd"), "7815696ecbf1c96e6894b779456d330e");
			Assert.AreEqual(sUtilities.Md5("áéűúő"), "eac988f3d64e748b7c2347a8667a5ed2");
		}

		[Test]
		public void IsPrime()
		{
			Assert.IsFalse(sUtilities.IsPrime(0));
			Assert.IsFalse(sUtilities.IsPrime(1));
			Assert.IsTrue(sUtilities.IsPrime(2));
			Assert.IsTrue(sUtilities.IsPrime(11));
		}

		[Test]
		public void IsDay()
		{
			Assert.IsFalse(sUtilities.IsDay(2001, 02, 29));
			Assert.IsFalse(sUtilities.IsDay(2001, 02, 30));
			Assert.IsTrue(sUtilities.IsDay(2000, 02, 29));
			Assert.IsTrue(sUtilities.IsDay(2012, 02, 29));
		}

		[Test]
		public void IsValueBiggerDateTimeNow()
		{
			Assert.IsTrue(sUtilities.IsValueBiggerDateTimeNow(2000, 01, 04, 10, 10));
			Assert.IsFalse(sUtilities.IsValueBiggerDateTimeNow(2030, 01, 04, 10, 10));
		}

		[Test]
		public void IsSpecialDirectory()
		{
			Assert.IsTrue(sUtilities.IsSpecialDirectory("$home/asd/asdsadas"));

			if(sPlatform.IsWindows)
			{
				Assert.IsTrue(sUtilities.IsSpecialDirectory(@"$home\asd\asdsadas"));
				Assert.IsTrue(sUtilities.IsSpecialDirectory("$localappdata/asd/asdsadas"));
				Assert.IsTrue(sUtilities.IsSpecialDirectory(@"$localappdata\asd\asdsadas"));
			}
		}

		[Test]
		public void GetUrls1()
		{
			var list = sUtilities.GetUrls("asda fadsfasd fad f http://google.hu fadfasdga gfdgsfh g");
			Assert.AreEqual(list[0], "http://google.hu");
		}

		[Test]
		public void GetUrls2()
		{
			var list = sUtilities.GetUrls("asda fadsfasd fad f http://google.hu/?asd=asddg fadfasdga gfdgsfh g");
			Assert.AreEqual(list[0], "http://google.hu/?asd=asddg");
		}

		[Test]
		public void GetUrls3()
		{
			var list = sUtilities.GetUrls("asda fadsfasd fad f http://msdn.microsoft.com/en-us/library/system.datetime.compare.aspx fadfasdga gfdgsfh g");
			Assert.AreEqual(list[0], "http://msdn.microsoft.com/en-us/library/system.datetime.compare.aspx");
		}

		[Test]
		public void GetUrls4()
		{
			var list = sUtilities.GetUrls("asda fadsfasd fad f http://www.nunit.org/index.php?p=samples&r=2.6.2 fadfasdga gfdgsfh g");
			Assert.AreEqual(list[0], "http://www.nunit.org/index.php?p=samples&r=2.6.2");
		}

		[Test]
		public void GetUrls5()
		{
			var list = sUtilities.GetUrls("asda fadsfasd fad f https://www.google.hu/#sclient=psy-ab&q=nunit.framework+wiki&oq=nunit.framework+wiki&gs_l=hp.3..0i30.11458.11895.3.12159.4.4.0.0.0.0.211.707.0j2j2.4.0....0.0..1c.1.20.psy-ab.NXY9UoF672M&pbx=1&bav=on.2,or.r_cp.r_qf.&bvm=bv.49478099%2Cd.Yms%2Cpv.xjs.s.en_US.c75bKy5EQ0A.O&fp=b46b10cb92c4fef2&biw=1440&bih=749 fadfasdga gfdgsfh g");
			Assert.AreEqual(list[0], "https://www.google.hu/#sclient=psy-ab&q=nunit.framework+wiki&oq=nunit.framework+wiki&gs_l=hp.3..0i30.11458.11895.3.12159.4.4.0.0.0.0.211.707.0j2j2.4.0....0.0..1c.1.20.psy-ab.NXY9UoF672M&pbx=1&bav=on.2,or.r_cp.r_qf.&bvm=bv.49478099%2Cd.Yms%2Cpv.xjs.s.en_US.c75bKy5EQ0A.O&fp=b46b10cb92c4fef2&biw=1440&bih=749");
		}

		[Test]
		public void GetUrls6()
		{
			var list = sUtilities.GetUrls("asda fadsfasd fad f http://www.nunit.org/index.php?p=samples&r=2.6.2 fadfasdga http://google.hu/?asd=asddg gfdgsfh g");
			Assert.AreEqual(list[0], "http://www.nunit.org/index.php?p=samples&r=2.6.2");
			Assert.AreEqual(list[1], "http://google.hu/?asd=asddg");
		}

		[Test]
		public void GetUrls7()
		{
			var list = sUtilities.GetUrls("asda fadsfasd fad f fadfasdga 93.250.56.87 gfdgsfh g");
			Assert.AreEqual(list[0], "http://93.250.56.87");
		}

		[Test]
		public void GetUrls8()
		{
			var list = sUtilities.GetUrls("asda fadsfasd fad f fadfasdga http://93.250.56.87 gfdgsfh g");
			Assert.AreEqual(list[0], "http://93.250.56.87");
		}

		[Test]
		public void GetUrls9()
		{
			var list = sUtilities.GetUrls("asda fadsfasd fad f fadfasdga http://93.250.56.87/?asd=asd gfdgsfh g");
			Assert.AreEqual(list[0], "http://93.250.56.87/?asd=asd");
		}
	}
}