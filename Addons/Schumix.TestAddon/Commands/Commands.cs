﻿/*
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
using System.IO;
using System.Net;
using System.Xml;
using System.Text;
using System.Data;
using Schumix.Irc;
using Schumix.Irc.Util;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Irc;
using Schumix.Framework.Bitly;
using Schumix.Framework.Config;
using Schumix.Framework.Logger;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.TestAddon.Commands
{
	class TestCommand : CommandInfo
	{
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;

		public TestCommand(string ServerName) : base(ServerName)
		{
		}

		public void HandleTest(IRCMessage sIRCMessage)
		{
			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;

			if(IsWarningAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Administrator))
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("WarningAdmin", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Administrator))
				return;

			if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "adat")
			{
				sSendMessage.SendChatMessage(sIRCMessage, "Teszt próbálkozás");
			}
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "db")
			{
				var db = SchumixBase.DManager.Query("SELECT Name FROM admins WHERE ServerName = '{0}'", sIRCMessage.ServerName);
				if(!db.IsNull())
				{
					foreach(DataRow row in db.Rows)
					{
						string admin = row["Name"].ToString();
						sSendMessage.SendChatMessage(sIRCMessage, "{0}", admin);
					}
				}
				else
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel, sIRCMessage.ServerName));
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
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "durl")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoValue", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				var request = (HttpWebRequest)WebRequest.Create(sIRCMessage.Info[5]);
				request.AllowAutoRedirect = true;
				request.UserAgent = Consts.SchumixUserAgent;
				request.Referer = Consts.SchumixReferer;
				var sb = new StringBuilder();

				using(var response = request.GetResponse())
				{
					using(var stream = response.GetResponseStream())
					{
						int length = 0;
						byte[] buf = new byte[1024];

						while((length = stream.Read(buf, 0, buf.Length)) != 0)
						{
							if(sb.ToString().Contains("</title>"))
								break;

							sb.Append(Encoding.UTF8.GetString(buf, 0, length));
						}
					}
				}

				sSendMessage.SendChatMessage(sIRCMessage, "{0}", sb.Length);
			}
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "cachedb")
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("xbot"));
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandTexts("xbot")[1]);
			}
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "istable")
			{
				if(SchumixBase.DManager.IsCreatedTable("admins"))
					sSendMessage.SendChatMessage(sIRCMessage, "létezik!");

				if(!SchumixBase.DManager.IsCreatedTable("admin"))
					sSendMessage.SendChatMessage(sIRCMessage, "nem létezik!");
			}
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "urlshort")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, "Nincs megadva a rövidíteni kívánt url!");
					return;
				}

				sSendMessage.SendChatMessage(sIRCMessage, BitlyApi.ShortenUrl(sIRCMessage.Info[5]).ShortUrl);
			}
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "serv")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, "Nincs megadva a szervíz neve!");
					return;
				}
				
				sSendMessage.SendChatMessage(sIRCMessage, "{0}", sIRCMessage.Info[5].IsServ());
				sSendMessage.SendChatMessage(sIRCMessage, "{0}", sIRCMessage.Info[5].IsServ(Serv.HostServ));
			}
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "isoutredirected")
			{
#if false
				var writer = new StringWriter();
				Console.SetOut(writer);

				sSendMessage.SendChatMessage(sIRCMessage, "Konzol kimenet átírányítása: {0}", Console.IsOutputRedirected);
				
				var sw = new StreamWriter(Console.OpenStandardOutput());
				sw.AutoFlush = true;
				Console.SetOut(sw);

				sSendMessage.SendChatMessage(sIRCMessage, "Konzol kimenet átírányítása: {0}", Console.IsOutputRedirected);
#endif
			}
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "ismonthname")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, "Nincs megadva a hónap neve!");
					return;
				}

				sSendMessage.SendChatMessage(sIRCMessage, "{0}", sIRCMessage.Info[5].IsMonthName());
			}
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "dbrank")
			{
				var db = SchumixBase.DManager.Query("SELECT Name, Flag FROM admins WHERE ServerName = '{0}'", sIRCMessage.ServerName);
				if(!db.IsNull())
				{
					foreach(DataRow row in db.Rows)
					{
						string name = row["Name"].ToString();
						int rank = row["Flag"].ToInt32();

						if(rank == 0)
							Log.WriteLine("{0} admin rank: 0", name);

						if(rank == 1)
							Log.WriteLine("{0} admin rank: 1", name);

						if(rank == 2)
							Log.WriteLine("{0} admin rank: 2", name);
					}
				}
				else
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel, sIRCMessage.ServerName));
			}
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "pathroot")
			{
				sSendMessage.SendChatMessage(sIRCMessage, "{0}", Path.GetPathRoot(@"C:\asd\sd\info.xml"));
			}
			else
				sSendMessage.SendChatMessage(sIRCMessage, "{0}", sIRCMessage.Info.Length);
		}

		[SchumixCommand("test2", CommandPermission.Normal)]
		public static void HandleTest2(IRCMessage sIRCMessage)
		{
			//Log.WriteLine(sIRCMessage.Args);
		}

		[IrcCommand(ReplyCode.RPL_WELCOME)]
		public static void IrcHandleTest2(IRCMessage sIRCMessage)
		{
			//Log.WriteLine(sIRCMessage.Args);
		}
	}
}