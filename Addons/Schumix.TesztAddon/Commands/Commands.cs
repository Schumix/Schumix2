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
using System.Net;
using System.Xml;
using System.Text;
using System.Data;
using Schumix.API;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
//using GitSharp;

namespace Schumix.TesztAddon.Commands
{
	class TesztCommand : CommandInfo
	{
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;

		protected void HandleTest(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Administrator))
				return;

			if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "adat")
			{
				sSendMessage.SendChatMessage(sIRCMessage, "Teszt probálkozás");
			}
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "db")
			{
				var db = SchumixBase.DManager.Query("SELECT Name FROM admins");
				if(!db.IsNull())
				{
					foreach(DataRow row in db.Rows)
					{
						string admin = row["Name"].ToString();
						sSendMessage.SendChatMessage(sIRCMessage, "{0}", admin);
					}
				}
				else
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel));
			}
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "rss")
			{
				var rss = new XmlDocument();
				rss.Load("http://www.assembla.com/spaces/Sandshroud/stream.rss");
				//rss.Load("http://github.com/megax/Schumix2/commits/master.atom");

				var node = rss.SelectSingleNode("rss/channel/item/title");
				//var node = rss.SelectSingleNode("feed/title");
				string title = !node.IsNull() ? node.InnerText : string.Empty;
				sSendMessage.SendChatMessage(sIRCMessage, title);
			}
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "vhost")
				sSendMessage.SendChatMessage(sIRCMessage, sIRCMessage.Host);
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "amsg")
				sSendMessage.SendCMAmsg("teszt");
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "me")
				sSendMessage.SendCMAction(sIRCMessage.Channel, sIRCMessage.Info.SplitToString(5, SchumixBase.Space));
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "ctcprequest")
				sSendMessage.SendCMCtcpRequest(sIRCMessage.Channel, sIRCMessage.Info.SplitToString(5, SchumixBase.Space));
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "ctcpreply")
				sSendMessage.SendCMCtcpReply(sIRCMessage.Channel, sIRCMessage.Info.SplitToString(5, SchumixBase.Space));
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "text")
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "durl")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoValue", sIRCMessage.Channel));
					return;
				}

				var request = (HttpWebRequest)WebRequest.Create(sIRCMessage.Info[5]);
				request.AllowAutoRedirect = true;
				request.UserAgent = Consts.SchumixUserAgent;
				request.Referer = Consts.SchumixReferer;

				var response = request.GetResponse();
				var stream = response.GetResponseStream();
				var sb = new StringBuilder();
				byte[] buf = new byte[1024];
				int length = 0;

				while((length = stream.Read(buf, 0, buf.Length)) != 0)
				{
					if(sb.ToString().Contains("</title>"))
						break;

					sb.Append(Encoding.UTF8.GetString(buf, 0, length));
				}

				response.Close();
				sSendMessage.SendChatMessage(sIRCMessage, "{0}", sb.Length);
			}
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "gcommit")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoValue", sIRCMessage.Channel));
					return;
				}

				/*var repo = new Repository("/home/megax/Asztal/Schumix2");
				string msg = new Commit(repo, sIRCMessage.Info[5]).Message;
				var msg2 = new Commit(repo, sIRCMessage.Info[5]).CommitDate;
				sSendMessage.SendChatMessage(sIRCMessage, "asd: {0}", msg);
				sSendMessage.SendChatMessage(sIRCMessage, "asd2: {0}", msg2);*/
			}
			else
				sSendMessage.SendChatMessage(sIRCMessage, "{0}", sIRCMessage.Info.Length);
		}

		[SchumixCommand("test2", CommandPermission.Normal)]
		public static void HandleTest2(IRCMessage sIRCMessage)
		{
			//Console.WriteLine(sIRCMessage.Args);
		}

		[IrcCommand(ReplyCode.RPL_WELCOME)]
		public static void IrcHandleTest2(IRCMessage sIRCMessage)
		{
			//Console.WriteLine(sIRCMessage.Args);
		}
	}
}