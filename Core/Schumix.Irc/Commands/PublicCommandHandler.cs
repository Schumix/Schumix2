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
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using Schumix.API;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using WolframAPI;

namespace Schumix.Irc.Commands
{
	public partial class CommandHandler
	{
		protected void HandleXbot(IRCMessage sIRCMessage)
		{
			var text = sLManager.GetCommandTexts("xbot", sIRCMessage.Channel);
			if(text.Length < 4)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
				return;
			}

			sSendMessage.SendChatMessage(sIRCMessage, text[0], sUtilities.GetVersion());
			string commands = string.Empty;

			foreach(var command in CommandManager.GetPublicCommandHandler())
			{
				if(command.Key == "xbot")
					continue;

				commands += " | " + IRCConfig.CommandPrefix + command.Key;
			}

			sSendMessage.SendChatMessage(sIRCMessage, text[1], Consts.SchumixProgrammedBy);
			sSendMessage.SendChatMessage(sIRCMessage, text[2], Consts.SchumixDevelopers);
			sSendMessage.SendChatMessage(sIRCMessage, text[3], commands.Remove(0, 3, " | "));
		}

		protected void HandleInfo(IRCMessage sIRCMessage)
		{
			var text = sLManager.GetCommandTexts("info", sIRCMessage.Channel);
			if(text.Length < 3)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
				return;
			}

			sSendMessage.SendChatMessage(sIRCMessage, text[0], Consts.SchumixDevelopers);
			sSendMessage.SendChatMessage(sIRCMessage, text[1], Consts.SchumixWebsite);
			sSendMessage.SendChatMessage(sIRCMessage, text[2]);
		}

		protected void HandleTime(IRCMessage sIRCMessage)
		{
			var text = sLManager.GetCommandTexts("time", sIRCMessage.Channel);
			if(text.Length < 2)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
				return;
			}

			if(DateTime.Now.Minute < 10)
				sSendMessage.SendChatMessage(sIRCMessage, text[0], DateTime.Now.Hour, DateTime.Now.Minute);
			else
				sSendMessage.SendChatMessage(sIRCMessage, text[1], DateTime.Now.Hour, DateTime.Now.Minute);
		}

		protected void HandleDate(IRCMessage sIRCMessage)
		{
			var text = sLManager.GetCommandTexts("date", sIRCMessage.Channel);
			if(text.Length < 4)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
				return;
			}

			string[,] Nameday = new string[,] {
				{ "ÚJÉV","Ábel","Genovéva","Titusz","Simon","Boldizsár","Attila","Gyöngyvér","Marcell","Melánia","Ágota","Ernő","Veronika","Bódog","Lóránt","Gusztáv","Antal","Piroska","Sára","Sebestyén","Ágnes","Vince","Zelma","Timót","Pál","Vanda","Angelika","Károly,","Adél","Martina","Marcella" },
				{ "Ignác","Karolina","Balázs","Ráhel","Ágota","Dóra","Tódor","Aranka","Abigél","Elvira","Bertold","Lívia","Ella, Linda","Bálint","Kolos","Julianna","Donát","Bernadett","Zsuzsanna","Álmos","Eleonóra","Gerzson","Alfréd","Mátyás","Géza","Edina","Ákos, Bátor","Elemér","","","" },
				{ "Albin","Lujza","Kornélia","Kázmér","Adorján","Leonóra","Tamás","Zoltán","Franciska","Ildikó","Szilárd","Gergely","Krisztián, Ajtony","Matild","Kristóf","Henrietta","Gertrúd","Sándor","József","Klaudia","Benedek","Beáta","Emőke","Gábor","Irén","Emánuel","Hajnalka","Gedeon","Auguszta","Zalán","Árpád" },
				{ "Hugó","Áron","Buda, Richárd","Izidor","Vince","Vilmos, Bíborka","Herman","Dénes","Erhard","Zsolt","Zsolt, Leó","Gyula","Ida","Tibor","Tas, Anasztázia","Csongor","Rudolf","Andrea","Emma","Konrád, Tivadar","Konrád","Csilla","Béla","György","Márk","Ervin","Zita","Valéria","Péter","Katalin, Kitti","" },
				{ "Fülöp","Zsigmond","Tímea","Mónika","Györgyi","Ivett","Gizella","Mihály","Gergely","Ármin","Ferenc","Pongrác","Szervác","Bonifác","Zsófia","Botond, Mózes","Paszkál","Erik","Ivó, Milán","Bernát, Felícia","Konstantin","Júlia, Rita","Dezső","Eszter","Orbán","Fülöp","Hella","Emil, Csanád","Magdolna","Zsanett, Janka","Angéla" },
				{ "Tünde","Anita, Kármen","Klotild","Bulcsú","Fatime","Norbert","Róbert","Medárd","Félix","Margit","Barnabás","Villő","Antal, Anett","Vazul","Jolán","Jusztin","Laura","Levente","Gyárfás","Rafael","Alajos","Paulina","Zoltán","Iván","Vilmos","János","László","Levente, Irén","Péter, Pál","Pál","" },
				{ "Annamária","Ottó","Kornél","Ulrik","Sarolta, Emese","Csaba","Appolónia","Ellák","Lukrécia","Amália","Nóra, Lili","Izabella","Jenő","&Őrs","Henrik","Valter","Endre, Elek","Frigyes","Emília","Illés","Dániel","Magdolna","Lenke","Kinga, Kincső","Kristóf, Jakab","Anna, Anikó","Olga","Szabolcs","Márta","Judit","Oszkár" },
				{ "Boglárka","Lehel","Hermina","Domonkos","Krisztina","Berta","Ibolya","László","Emőd","Lörinc","Zsuzsanna","Klára","Ipoly","Marcell","Mária","Ábrahám","Jácint","Ilona","Huba","István","Sámuel","Menyhért","Bence","Bertalan","Lajos","Izsó","Gáspár","Ágoston","Beatrix","Rózsa","Erika" },
				{ "Egon","Rebeka","Hilda","Rozália","Viktor, Lőrinc","Zakariás","Regina","Mária","Ádám","Nikolett, Hunor","Teodóra","Mária","Kornél","Szeréna","Enikő","Edit","Zsófia","Diána","Vilhelmina","Friderika","Máté","Móric","Tekla","Gellért","Eufrozina","Jusztina","Adalbert","Vencel","Mihály","Jeromos","" },
				{ "Malvin","Petra","Helga","Ferenc","Aurél","Renáta","Amália","Koppány","Dénes","Gedeon","Brigitta","Miksa","Kálmán","Helén","Teréz","Gál","Hedvig","Lukács","Nándor","Vendel","Orsolya","Előd","Gyöngyi","Salamon","Bianka","Dömötör","Szabina","Simon","Nárcisz","Alfonz","Farkas" },
				{ "Marianna","Achilles","Győző","Károly","Imre","Lénárd","Rezső","Zsombor","Tivadar","Réka","Márton","Jónás, Renátó","Szilvia","Aliz","Albert, Lipót","Ödön","Hortenzia, Gergő","Jenő","Erzsébet","Jolán","Olivér","Cecília","Kelemen","Emma","Katalin","Virág","Virgil","Stefánia","Taksony","András, Andor","" },
				{ "Elza","Melinda","Ferenc","Barbara, Borbála","Vilma","Miklós","Ambrus","Mária","Natália","Judit","Árpád","Gabriella","Luca","Szilárda","Valér","Etelka","Lázár","Auguszta","Viola","Teofil","Tamás","Zéno","Viktória","Ádám, Éva","KARÁCSONY","KARÁCSONY","János","Kamilla","Tamás","Dávid","Szilveszter" },
			};

			int month = DateTime.Now.Month;
			int day = DateTime.Now.Day;

			if(month < 10)
			{
				if(day < 10)
					sSendMessage.SendChatMessage(sIRCMessage, text[0], DateTime.Now.Year, month, day, Nameday[month-1, day-1]);
				else
					sSendMessage.SendChatMessage(sIRCMessage, text[1], DateTime.Now.Year, month, day, Nameday[month-1, day-1]);
			}
			else
			{
				if(day < 10)
					sSendMessage.SendChatMessage(sIRCMessage, text[2], DateTime.Now.Year, month, day, Nameday[month-1, day-1]);
				else
					sSendMessage.SendChatMessage(sIRCMessage, text[3], DateTime.Now.Year, month, day, Nameday[month-1, day-1]);
			}
		}

		protected void HandleCalc(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoValue", sIRCMessage.Channel));
				return;
			}

			var client = new WAClient("557QYQ-UUUWTKX95V");
			var solution = client.Solve(sIRCMessage.Info.SplitToString(4, SchumixBase.Space));
			sSendMessage.SendChatMessage(sIRCMessage, "{0}", solution);
		}

		protected void HandleIrc(IRCMessage sIRCMessage)
		{
			var text = sLManager.GetCommandTexts("irc", sIRCMessage.Channel);
			if(text.Length < 2)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
				return;
			}

			if(sIRCMessage.Info.Length == 4)
			{
				var db = SchumixBase.DManager.Query("SELECT Command FROM irc_commands WHERE Language = '{0}'", sLManager.GetChannelLocalization(sIRCMessage.Channel));
				if(!db.IsNull())
				{
					string commands = string.Empty;

					foreach(DataRow row in db.Rows)
						commands += " | " + row["Command"].ToString();

					if(commands == string.Empty)
						sSendMessage.SendChatMessage(sIRCMessage, text[0], "none");
					else
						sSendMessage.SendChatMessage(sIRCMessage, text[0], commands.Remove(0, 3, " | "));
				}
				else
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel));
			}
			else
			{
				var db = SchumixBase.DManager.QueryFirstRow("SELECT Text FROM irc_commands WHERE Command = '{0}' AND Language = '{1}'", sUtilities.SqlEscape(sIRCMessage.Info[4]), sLManager.GetChannelLocalization(sIRCMessage.Channel));
				if(!db.IsNull())
					sSendMessage.SendChatMessage(sIRCMessage, db["Text"].ToString());
				else
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
			}
		}

		protected void HandleWhois(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoWhoisName", sIRCMessage.Channel));
				return;
			}

			WhoisPrivmsg = sIRCMessage.Channel;
			sSender.Whois(sIRCMessage.Info[4]);
		}

		protected void HandleWarning(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
				return;
			}

			if(!IsAdmin(sIRCMessage.Nick) && sIRCMessage.Info[4].Contains("#"))
				return;

			if(sIRCMessage.Info.Length == 5)
				sSendMessage.SendChatMessage(sIRCMessage.MessageType, sIRCMessage.Info[4], sLManager.GetCommandText("warning", sIRCMessage.Channel), sIRCMessage.Nick, sIRCMessage.Channel);
			else if(sIRCMessage.Info.Length >= 6)
				sSendMessage.SendChatMessage(sIRCMessage.MessageType, sIRCMessage.Info[4], "{0}", sIRCMessage.Info.SplitToString(5, SchumixBase.Space));
		}

		protected void HandleGoogle(IRCMessage sIRCMessage)
		{
			var text = sLManager.GetCommandTexts("google", sIRCMessage.Channel);
			if(text.Length < 4)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
				return;
			}

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoGoogleText", sIRCMessage.Channel));
				return;
			}

			string url = sUtilities.GetUrl("http://ajax.googleapis.com/ajax/services/search/web?v=1.0&start=0&rsz=small&q=", sIRCMessage.Info.SplitToString(4, SchumixBase.Space));
			var Regex = new Regex(@".unescapedUrl.\:.(?<url>\S+).\,.url.\:.\S+.\,.visibleUrl.\:\S+.\,.cacheUrl.\:.\S+.\,.title.\:.\S*\s*\S*\s*\S*.\,.titleNoFormatting.\:.(?<title>\S*\s*\S*\s*\S*).\,.content");

			if(!Regex.IsMatch(url))
			{
				sSendMessage.SendChatMessage(sIRCMessage, text[0]);
				sSendMessage.SendChatMessage(sIRCMessage, text[1]);
			}
			else
			{
				sSendMessage.SendChatMessage(sIRCMessage, text[2], Regex.Match(url).Groups["title"].ToString());
				sSendMessage.SendChatMessage(sIRCMessage, text[3], Regex.Match(url).Groups["url"].ToString());
			}
		}

		protected void HandleTranslate(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoTranslateLanguage", sIRCMessage.Channel));
				return;
			}

			if(sIRCMessage.Info.Length < 6)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoTranslateText", sIRCMessage.Channel));
				return;
			}

			string url = sUtilities.GetUrl("http://ajax.googleapis.com/ajax/services/language/translate?v=1.0&q=", sIRCMessage.Info.SplitToString(5, SchumixBase.Space), "&langpair=" + sIRCMessage.Info[4]);
			var Regex = new Regex(@"\{.translatedText.\:.(?<text>.+).\},");

			if(!Regex.IsMatch(url))
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("translate", sIRCMessage.Channel));
			else
				sSendMessage.SendChatMessage(sIRCMessage, "{0}", Regex.Match(url).Groups["text"].ToString());
		}

		protected void HandleOnline(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
				return;
			}

			IsOnline = true;
			OnlinePrivmsg = sIRCMessage.Channel;
			sSender.NickServInfo(sIRCMessage.Info[4]);
		}
	}
}