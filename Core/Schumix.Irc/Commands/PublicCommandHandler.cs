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
			CNick(sIRCMessage);
			var text = sLManager.GetCommandTexts("xbot", sIRCMessage.Channel);
			if(text.Length < 3)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
				return;
			}

			sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0], sUtilities.GetVersion());
			string commands = string.Empty;

			foreach(var command in CommandManager.GetPublicCommandHandler())
			{
				if(command.Key == "xbot")
					continue;

				commands += " | " + IRCConfig.CommandPrefix + command.Key;
			}

			sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1], commands.Remove(0, 3, " | "));
			sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[2]);
		}

		protected void HandleInfo(IRCMessage sIRCMessage)
		{
			CNick(sIRCMessage);
			sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("info", sIRCMessage.Channel));
		}

		protected void HandleTime(IRCMessage sIRCMessage)
		{
			CNick(sIRCMessage);
			var text = sLManager.GetCommandTexts("time", sIRCMessage.Channel);
			if(text.Length < 2)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
				return;
			}

			if(DateTime.Now.Minute < 10)
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0], DateTime.Now.Hour, DateTime.Now.Minute);
			else
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1], DateTime.Now.Hour, DateTime.Now.Minute);
		}

		protected void HandleDate(IRCMessage sIRCMessage)
		{
			CNick(sIRCMessage);
			var text = sLManager.GetCommandTexts("date", sIRCMessage.Channel);
			if(text.Length < 4)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
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
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0], DateTime.Now.Year, month, day, Nameday[month-1, day-1]);
				else
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1], DateTime.Now.Year, month, day, Nameday[month-1, day-1]);
			}
			else
			{
				if(day < 10)
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[2], DateTime.Now.Year, month, day, Nameday[month-1, day-1]);
				else
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[3], DateTime.Now.Year, month, day, Nameday[month-1, day-1]);
			}
		}

		protected void HandleRoll(IRCMessage sIRCMessage)
		{
			CNick(sIRCMessage);
			var rand = new Random();
			int number = rand.Next(0, 100);
			sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("roll", sIRCMessage.Channel), number);
		}

		protected void HandleCalc(IRCMessage sIRCMessage)
		{
			CNick(sIRCMessage);

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoValue", sIRCMessage.Channel));
				return;
			}

			var client = new WAClient("557QYQ-UUUWTKX95V");
			var solution = client.Solve(sIRCMessage.Info.SplitToString(4, SchumixBase.Space));
			sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "{0}", solution);
		}

		protected void HandleSha1(IRCMessage sIRCMessage)
		{
			CNick(sIRCMessage);

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoValue", sIRCMessage.Channel));
				return;
			}

			sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sUtilities.Sha1(sIRCMessage.Info.SplitToString(4, SchumixBase.Space)));
		}

		protected void HandleMd5(IRCMessage sIRCMessage)
		{
			CNick(sIRCMessage);

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoValue", sIRCMessage.Channel));
				return;
			}

			sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sUtilities.Md5(sIRCMessage.Info.SplitToString(4, SchumixBase.Space)));
		}

		protected void HandleIrc(IRCMessage sIRCMessage)
		{
			CNick(sIRCMessage);

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoIrcCommandName", sIRCMessage.Channel));
				return;
			}

			var db = SchumixBase.DManager.QueryFirstRow("SELECT Message FROM irc_commands WHERE command = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[4]));
			if(!db.IsNull())
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, db["Message"].ToString());
			else
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel));
		}

		protected void HandleWhois(IRCMessage sIRCMessage)
		{
			CNick(sIRCMessage);

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoWhoisName", sIRCMessage.Channel));
				return;
			}

			WhoisPrivmsg = sIRCMessage.Channel;
			sSender.Whois(sIRCMessage.Info[4]);
		}

		protected void HandleWarning(IRCMessage sIRCMessage)
		{
			CNick(sIRCMessage);

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoName", sIRCMessage.Channel));
				return;
			}

			if(!IsAdmin(sIRCMessage.Nick) && sIRCMessage.Info[4].Contains("#"))
				return;

			if(sIRCMessage.Info.Length == 5)
				sSendMessage.SendCMPrivmsg(sIRCMessage.Info[4], sLManager.GetCommandText("warning", sIRCMessage.Channel), sIRCMessage.Channel);
			else if(sIRCMessage.Info.Length >= 6)
				sSendMessage.SendCMPrivmsg(sIRCMessage.Info[4], "{0}", sIRCMessage.Info.SplitToString(5, SchumixBase.Space));
		}

		protected void HandleGoogle(IRCMessage sIRCMessage)
		{
			CNick(sIRCMessage);
			var text = sLManager.GetCommandTexts("google", sIRCMessage.Channel);
			if(text.Length < 4)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
				return;
			}

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoGoogleText", sIRCMessage.Channel));
				return;
			}

			string url = sUtilities.GetUrl("http://ajax.googleapis.com/ajax/services/search/web?v=1.0&start=0&rsz=small&q=", sIRCMessage.Info.SplitToString(4, SchumixBase.Space));
			var Regex = new Regex(@".unescapedUrl.\:.(?<url>\S+).\,.url.\:.\S+.\,.visibleUrl.\:\S+.\,.cacheUrl.\:.\S+.\,.title.\:.\S*\s*\S*\s*\S*.\,.titleNoFormatting.\:.(?<title>\S*\s*\S*\s*\S*).\,.content");

			if(!Regex.IsMatch(url))
			{
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, sIRCMessage.Channel, text[0]);
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, sIRCMessage.Channel, text[1]);
			}
			else
			{
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, sIRCMessage.Channel, text[2], Regex.Match(url).Groups["title"].ToString());
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, sIRCMessage.Channel, text[3], Regex.Match(url).Groups["url"].ToString());
			}
		}

		protected void HandleTranslate(IRCMessage sIRCMessage)
		{
			CNick(sIRCMessage);

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoTranslateLanguage", sIRCMessage.Channel));
				return;
			}

			if(sIRCMessage.Info.Length < 6)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoTranslateText", sIRCMessage.Channel));
				return;
			}

			string url = sUtilities.GetUrl("http://ajax.googleapis.com/ajax/services/language/translate?v=1.0&q=", sIRCMessage.Info.SplitToString(5, SchumixBase.Space), "&langpair=" + sIRCMessage.Info[4]);
			var Regex = new Regex(@"\{.translatedText.\:.(?<text>.+).\},");

			if(!Regex.IsMatch(url))
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("translate", sIRCMessage.Channel));
			else
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "{0}", Regex.Match(url).Groups["text"].ToString());
		}

		protected void HandlePrime(IRCMessage sIRCMessage)
		{
			CNick(sIRCMessage);
			var text = sLManager.GetCommandTexts("prime", sIRCMessage.Channel);
			if(text.Length < 3)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
				return;
			}

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoNumber", sIRCMessage.Channel));
				return;
			}

			double Num;
			bool isNum = double.TryParse(sIRCMessage.Info[4], out Num);

			if(!isNum)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
				return;
			}

			bool prim = sUtilities.IsPrime(Convert.ToInt32(Num));

			if(!prim)
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1], sIRCMessage.Info[4]);
			else
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[2], sIRCMessage.Info[4]);
		}

		protected void HandleWeather(IRCMessage sIRCMessage)
		{
			CNick(sIRCMessage);
			var text = sLManager.GetCommandTexts("weather", sIRCMessage.Channel);
			if(text.Length < 3)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel)));
				return;
			}

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetWarningText("NoCityName", sIRCMessage.Channel));
				return;
			}

			bool home = false;
			string source = string.Empty;

			if(sIRCMessage.Info[4].ToLower() == "home")
				home = true;

			try
			{
				if(home)
					source = sUtilities.GetUrl("http://www.meteoprog.hu/hu/weather/Zalaegerszeg");
				else
				{
					string s = sIRCMessage.Info[4].Replace("á", "a");
					s = s.Replace("é", "e");
					s = s.Replace("í", "i");
					s = s.Replace("ű", "u");
					s = s.Replace("ü", "u");
					s = s.Replace("ú", "u");
					s = s.Replace("ő", "o");
					s = s.Replace("ö", "o");
					s = s.Replace("ó", "o");
					source = sUtilities.GetUrl(string.Format("http://www.meteoprog.hu/hu/weather/{0}", s));
				}

				// TODO: Csapadék kiírása
				source = source.Remove(0, source.IndexOf("<td colspan=4 align=center bgcolor=\"#1B8BB2\"><div id=\"day_1\" class=\"weather-pict\" style=\"position:relative;width:376px\"></div></td>"));
				source = source.Remove(0, source.IndexOf("<td width=\"84\" align=right style=\"padding: 5px 10px 5px 0px;\">A levegő hőmérséklete</td>"));
				source = source.Remove(0, source.IndexOf("</td>"));
				source = source.Remove(0, source.IndexOf("</td>"));
				source = source.Remove(0, source.IndexOf("<td class=\"wforecast-cell\">") + "<td class=\"wforecast-cell\">".Length);
				string min_max = source.Substring(0, source.IndexOf("&deg;C"));

				source = source.Remove(0, source.IndexOf("<td width=\"84\" align=right style=\"padding: 5px 10px 5px 0px;\">Szél</td>"));
				source = source.Remove(0, source.IndexOf("</b></td>"));
				source = source.Remove(0, source.IndexOf("</b></td>"));
				source = source.Remove(0, source.IndexOf("<td align=center><b>") + "<td align=center><b>".Length);
				string wind = source.Substring(0, source.IndexOf("</b></td>"));
				wind = wind.Replace("&nbsp;", SchumixBase.Space.ToString());

				if(home)
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0], /*weather*/ "-", min_max.Substring(0, min_max.IndexOf("...")), min_max.Substring(min_max.IndexOf("...")+3), wind.Substring(wind.IndexOf(", ")+2));
				else
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1], sIRCMessage.Info[4], /*weather*/ "-", min_max.Substring(0, min_max.IndexOf("...")), min_max.Substring(min_max.IndexOf("...")+3), wind.Substring(wind.IndexOf(", ")+2));
			}
			catch
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[2]);
			}
		}
	}
}