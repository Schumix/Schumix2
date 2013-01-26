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
using System.Xml;
using System.Text.RegularExpressions;
using Schumix.Framework;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.ExtraAddon.Commands
{
	class YoutubeTitle
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private bool _isyoutube;
		private string _title;
		private string _code;
		private string _url;

		public YoutubeTitle(string Url)
		{
			_url = Url;
			Parse();

			if(_isyoutube)
				Load();
		}

		public string GetTitle()
		{
			return _title;
		}

		public bool IsTitle()
		{
			return _title != string.Empty;
		}

		public bool IsYoutube()
		{
			return _isyoutube;
		}

		private void Parse()
		{
			var regex = new Regex(@"youtu(?:\.be|be\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)");
			if(regex.IsMatch(_url))
			{
				var match = regex.Match(_url);
				_code = match.Groups[1].Value;
				_isyoutube = true;
			}
			else
				_isyoutube = false;
		}

		private void Load()
		{
			try
			{
				var xml = new XmlDocument();
				var url = new Uri(string.Format("http://gdata.youtube.com/feeds/api/videos/{0}?fields=title", _code));
				xml.LoadXml(sUtilities.DownloadString(url, 10000));
				var ns = new XmlNamespaceManager(xml.NameTable);
				ns.AddNamespace("ga", "http://www.w3.org/2005/Atom");

				var title = xml.SelectSingleNode("ga:entry/ga:title", ns);
				_title = title.IsNull() ? string.Empty : title.InnerText;
				xml.RemoveAll();
			}
			catch(Exception e)
			{
				Log.Debug("YoutubeTitle", sLConsole.GetString("Exception thrown while fetching youtube title: {0}"), e.Message);
				_title = string.Empty;
			}
		}
	}
}