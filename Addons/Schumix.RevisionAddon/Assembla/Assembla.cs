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

namespace Schumix.RevisionAddon
{
	public class Assembla
	{
		private Assembla() {}

		public string GetInformation(string Name)
		{
			return "";
		}
	}
	/*string commit, author;

	// commit
	m_Curl = curl_easy_init();
	if(m_Curl)
	{
		string bufferdata;

		curl_easy_setopt(m_Curl, CURLOPT_URL, format("http://trac6.assembla.com/Sandshroud/changeset/%i", rev).c_str());
		curl_easy_setopt(m_Curl, CURLOPT_WRITEFUNCTION, IRCSession::writer);
		curl_easy_setopt(m_Curl, CURLOPT_WRITEDATA, &bufferdata);
		CURLcode result = curl_easy_perform(m_Curl);

		curl_easy_cleanup(m_Curl);

		if(result == CURLE_OK)
		{
			boost::regex re("<p>\\s*(.*)<br />\\s*</p>");
			boost::cmatch matches;

			boost::regex_search(bufferdata.c_str(), matches, re);
			string matched(matches[1].first, matches[1].second);

			vector<string> res(1);
			split(matched, "\n", res);
			int resAdat = res.size();
			string alomany;

			for(int i = 1; i < resAdat; i++)
				alomany += " " + res[i];

			commit = alomany;
			res.clear();
		}
		else
		{
			Log.Error("IRCSession", "Hiba a Http lekerdezesben.");
			return;
		}
	}
	else
		curl_easy_cleanup(m_Curl);

	// author
	m_Curl = curl_easy_init();
	if(m_Curl)
	{
		string bufferdata;

		curl_easy_setopt(m_Curl, CURLOPT_URL, format("http://trac6.assembla.com/Sandshroud/changeset/%i", rev).c_str());
		curl_easy_setopt(m_Curl, CURLOPT_WRITEFUNCTION, IRCSession::writer);
		curl_easy_setopt(m_Curl, CURLOPT_WRITEDATA, &bufferdata);
		CURLcode result = curl_easy_perform(m_Curl);

		curl_easy_cleanup(m_Curl);

		if(result == CURLE_OK)
		{
			boost::regex re("Author:</dt>\\s*<dd class=.author.>(.*)</dd>\\s*<dt class=.property message.>Message:</dt>");
			boost::cmatch matches;

			boost::regex_search(bufferdata.c_str(), matches, re);
			string matched(matches[1].first, matches[1].second);
			author = matched;
		}
		else
		{
			Log.Error("IRCSession", "Hiba a Http lekerdezesben.");
			return;
		}
	}
	else
		curl_easy_cleanup(m_Curl);

	if(commit == "" && author == "")
		SendChatMessage(PRIVMSG, channel.c_str(), "Nincs ilyen rev.");
	else
		SendChatMessage(PRIVMSG, channel.c_str(), "3Sandshroud rev 10%i. Fejlesztő: 12%s. Commit:%s", rev, author.c_str(), commit.c_str());*/
}