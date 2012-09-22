/*
 * This file is part of Schumix.
 * 
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

using System;
using System.Text;
using Schumix.Framework;
using Schumix.RevisionAddon.Githubs.Author;
using Schumix.RevisionAddon.Githubs.Message;

namespace Schumix.RevisionAddon.Githubs
{
	class Github
	{
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		public string UserName { get; private set; }
		public string Message { get; private set; }
		public string Url { get; private set; }

		public Github(string username, string repositorie, string sha1)
		{
			try
			{
				string url = sUtilities.GetUrl(string.Format("https://api.github.com/repos/{0}/{1}/git/commits/{2}", username, repositorie, sha1));
				var githubA = new GithubAuthor();
				githubA = JsonHelper.Deserialise<GithubAuthor>(url);
				UserName = githubA.AuthorResult.Name;

				var githubM = new GithubMessage();
				githubM = JsonHelper.Deserialise<GithubMessage>(url);
				Message = githubM.Message.Replace(SchumixBase.NewLine, SchumixBase.Space);

				Url = string.Format("https://github.com/{0}/{1}/commit/{2}", username, repositorie, sha1);
			}
			catch(Exception)
			{
				UserName = string.Empty;
				Message = string.Empty;
				Url = string.Empty;
			}
		}
	}
}