/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2011 Twl
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
using System.Text.RegularExpressions;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Extensions;

namespace Schumix.ExtraAddon.Commands
{
	public partial class Functions : CommandInfo
	{
		public void HLUzenet(string channel, string[] _info)
		{
			if(sChannelInfo.FSelect("hl") && sChannelInfo.FSelect("hl", channel))
			{
				for(int i = 3; i < _info.Length; i++)
				{
					if(i == 3)
						_info[3] = _info[3].Remove(0, 1, ":");

					var db = SchumixBase.DManager.QueryFirstRow("SELECT Info, Enabled FROM hlmessage WHERE Name = '{0}'", _info[i].ToLower());
					if(!db.IsNull())
					{
						string info = db["Info"].ToString();
						string allapot = db["Enabled"].ToString();

						if(allapot != "be")
							return;

						sSendMessage.SendCMPrivmsg(channel, "{0}", info);
						break;
					}
				}
			}
		}

		public bool AutoKick(string allapot, string nick, string _channel)
		{
			if(allapot == "join")
			{
				string channel = _channel.Remove(0, 1, ":");

				if(sChannelInfo.FSelect("kick") && sChannelInfo.FSelect("kick", channel))
				{
					var db = SchumixBase.DManager.QueryFirstRow("SELECT Reason FROM kicklist WHERE Name = '{0}'", nick.ToLower());
					if(!db.IsNull())
					{
						string oka = db["Reason"].ToString();
						sSender.Kick(channel, nick, oka);
						return true;
					}
				}

				return false;
			}

			if(allapot == "privmsg")
			{
				if(sChannelInfo.FSelect("kick") && sChannelInfo.FSelect("kick", _channel))
				{
					var db = SchumixBase.DManager.QueryFirstRow("SELECT Reason FROM kicklist WHERE Name = '{0}'", nick.ToLower());
					if(!db.IsNull())
					{
						string oka = db["Reason"].ToString();
						sSender.Kick(_channel, nick, oka);
						return true;
					}
				}

				return false;
			}

			return false;
		}

		public void HandleWebTitle(string channel, string msg)
		{
			try
			{
				var url = new Uri(msg);
				var webTitle = WebHelper.GetWebTitle(url);

				if(string.IsNullOrEmpty(webTitle))
					return;

				var title = Regex.Replace(webTitle, @"\s+", " ");

				// check if it's youtube.
				var youtubeRegex = new Regex(@"\s*YouTube\s*\-(?<song>.+)", RegexOptions.IgnoreCase);

				if(youtubeRegex.IsMatch(title))
				{
					var match = youtubeRegex.Match(title);
					var song = match.Groups["song"].ToString();
					sSendMessage.SendCMPrivmsg(channel, "1,0You0,4Tube: {0}", song.Substring(1));
					return;
				}

				sSendMessage.SendCMPrivmsg(channel, "1,0Title: {0}", title);
			}
			catch(Exception e)
			{
				Log.Error("Functions", "Hiba oka: {0}", e.Message);
				return;
			}
		}
	}
}