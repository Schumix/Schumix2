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
using System.IO;
using System.Linq;
using System.Threading;
using System.Reflection;
using System.Globalization;
using Schumix.Installer.Platforms;
using Schumix.Installer.Extensions;
using NGettext;

namespace Schumix.Installer.Localization
{
	sealed class LocalizationConsole
	{
		private readonly Platform sPlatform = Singleton<Platform>.Instance;
		public string Locale { get; set; }
		private ICatalog _catalog;
		private string _localedir;

		private LocalizationConsole()
		{
			Initialize();
			SetLocale("en-US");
		}

		public void Initialize()
		{
			Initialize("./locale");
		}

		public void Initialize(string LocaleDir)
		{
			if(LocaleDir.IsNullOrEmpty() || LocaleDir == "./locale")
			{
				string location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

				if(sPlatform.IsWindows)
					LocaleDir = Path.Combine(location, "locale");
				else if(sPlatform.IsLinux)
				{
					bool enabled = false;
					var dir = new DirectoryInfo(location);

					foreach(var d in dir.GetDirectories("locale").AsParallel())
					{
						if(d.Name == "locale")
							enabled = true;
					}

					if(enabled)
						LocaleDir = "./locale";
					else
					{
						// $prefix/lib/schumix
						string prefix = Path.Combine(Path.Combine(location, ".."), "..");
						prefix = Path.GetFullPath(prefix);

						// "$prefix/share/locale"
						LocaleDir = Path.Combine(Path.Combine(prefix, "share"), "locale");
					}
				}
			}

			_localedir = LocaleDir;
			LoadCatalog();
		}

		private void LoadCatalog()
		{
			LoadCatalog(CultureInfo.GetCultureInfo("en-US"));
		}

		private void LoadCatalog(CultureInfo ci)
		{
			_catalog = new Catalog("schumix", _localedir, ci);
		}

		public void SetLocale()
		{
			SetLocale("en-US");
		}

		public void SetLocale(string Language)
		{
			Locale = Language.Replace("-", string.Empty);
			Language = Language.ToLocale();

			if(sPlatform.IsWindows)
			{
#if false
				// .net 4.5
				CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo(Language);
#endif
				Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(Language);
			}
			else if(sPlatform.IsLinux)
			{
#if false
				// .net 4.5
				CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo(Language);
#endif
				Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(Language);
				Environment.SetEnvironmentVariable("LANGUAGE", Language.Substring(0, 2));
			}
			else
			{
#if false
				// .net 4.5
				CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo(Language);
#endif
				Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(Language);
				Environment.SetEnvironmentVariable("LANGUAGE", Language.Substring(0, 2));
			}

			LoadCatalog(CultureInfo.GetCultureInfo(Language));
		}

		public string GetString(string phrase)
		{
			return _catalog.GetString(phrase);
		}

		public string GetString(string phrase, object arg0)
		{
			return string.Format(_catalog.GetString(phrase), arg0);
		}

		public string GetString(string phrase, object arg0, object arg1)
		{
			return string.Format(_catalog.GetString(phrase), arg0, arg1);
		}

		public string GetString(string phrase, object arg0, object arg1, object arg2)
		{
			return string.Format(_catalog.GetString(phrase), arg0, arg1, arg2);
		}

		public string GetString(string phrase, params object[] args)
		{
			return string.Format(_catalog.GetString(phrase), args);
		}

		public string GetPluralString(string singular, string plural, int number)
		{
			return _catalog.GetPluralString(singular, plural, number);
		}

		public string GetPluralString(string singular, string plural, int number, object arg0)
		{
			return string.Format(_catalog.GetPluralString(singular, plural, number), arg0);
		}

		public string GetPluralString(string singular, string plural, int number, object arg0, object arg1)
		{
			return string.Format(_catalog.GetPluralString(singular, plural, number), arg0, arg1);
		}

		public string GetPluralString(string singular, string plural, int number, object arg0, object arg1, object arg2)
		{
			return string.Format(_catalog.GetPluralString(singular, plural, number), arg0, arg1, arg2);
		}

		public string GetPluralString(string singular, string plural, int number, params object[] args)
		{
			return string.Format(_catalog.GetPluralString(singular, plural, number), args);
		}
	}
}