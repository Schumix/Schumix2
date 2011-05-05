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
			sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "3Verzió: 10{0}", sUtilities.GetVersion());
			string parancsok = string.Empty;

			foreach(var command in CommandManager.GetPublicCommandHandler())
			{
				if(command.Key == "xbot")
					continue;

				parancsok += " | " + IRCConfig.CommandPrefix + command.Key;
			}

			sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "3Parancsok: {0}", parancsok.Remove(0, 3, " | "));
			sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Programmed by: 3Csaba");
		}

		protected void HandleInfo(IRCMessage sIRCMessage)
		{
			CNick(sIRCMessage);
			sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Programozóm: Csaba, Jackneill.");
		}

		protected void HandleIdo(IRCMessage sIRCMessage)
		{
			CNick(sIRCMessage);

			if(DateTime.Now.Minute < 10)
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Helyi idő: {0}:0{1}", DateTime.Now.Hour, DateTime.Now.Minute);
			else
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Helyi idő: {0}:{1}", DateTime.Now.Hour, DateTime.Now.Minute);
		}

		protected void HandleDatum(IRCMessage sIRCMessage)
		{
			CNick(sIRCMessage);
			string[,] Nevnap = new string[12,31] {
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

			int[] Honapok = new int[12] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

			int Ev = DateTime.Now.Year;
			int Honap = DateTime.Now.Month;
			int Nap = DateTime.Now.Day;

			for(int x = 0; x < 12; x++)
			{
				int honapdb = Honapok[x];
				string napdb = Nevnap[x,DateTime.Now.Day-1];

				if(Honap == honapdb)
				{
					if(Honap < 10)
					{
						if(Nap < 10)
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Ma {0}. 0{1}. 0{2}. {3} napja van.", Ev, Honap, Nap, napdb);
						else
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Ma {0}. 0{1}. {2}. {3} napja van.", Ev, Honap, Nap, napdb);
					}
					else
					{
						if(Nap < 10)
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Ma {0}. {1}. 0{2}. {3} napja van.", Ev, Honap, Nap, napdb);
						else
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Ma {0}. {1}. {2}. {3} napja van.", Ev, Honap, Nap, napdb);
					}
				}
		 	}
		}

		protected void HandleRoll(IRCMessage sIRCMessage)
		{
			CNick(sIRCMessage);
			var rand = new Random();
			int szam = rand.Next(0, 100);
			sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Százalékos aránya {0}%", szam);
		}

		protected void HandleCalc(IRCMessage sIRCMessage)
		{
			CNick(sIRCMessage);

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs paraméter!");
				return;
			}

			var client = new WAClient("557QYQ-UUUWTKX95V");
			var solution = client.Solve(sIRCMessage.Info.SplitToString(4, " "));
			sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "{0}", solution);
		}

		protected void HandleSha1(IRCMessage sIRCMessage)
		{
			CNick(sIRCMessage);

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs paraméter!");
				return;
			}

			sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sUtilities.Sha1(sIRCMessage.Info.SplitToString(4, " ")));
		}

		protected void HandleMd5(IRCMessage sIRCMessage)
		{
			CNick(sIRCMessage);

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs paraméter!");
				return;
			}

			sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sUtilities.Md5(sIRCMessage.Info.SplitToString(4, " ")));
		}

		protected void HandleIrc(IRCMessage sIRCMessage)
		{
			CNick(sIRCMessage);

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs megadva a parancs neve!");
				return;
			}

			var db = SchumixBase.DManager.QueryFirstRow("SELECT Message FROM irc_commands WHERE command = '{0}'", sIRCMessage.Info[4]);
			if(!db.IsNull())
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, db["Message"].ToString());
			else
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Hibás lekérdezés!");
		}

		protected void HandleWhois(IRCMessage sIRCMessage)
		{
			CNick(sIRCMessage);

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs megadva a keresendő személy neve!");
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
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs megadva a név!");
				return;
			}

			if(sIRCMessage.Info.Length == 5)
				sSendMessage.SendCMPrivmsg(sIRCMessage.Info[4], "Keresnek téged itt: {0}", sIRCMessage.Channel);
			else if(sIRCMessage.Info.Length >= 6)
				sSendMessage.SendCMPrivmsg(sIRCMessage.Info[4], "{0}", sIRCMessage.Info.SplitToString(5, " "));
		}

		protected void HandleKeres(IRCMessage sIRCMessage)
		{
			CNick(sIRCMessage);

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs megadva a keresendő szöveg!");
				return;
			}

			string url = sUtilities.GetUrl("http://ajax.googleapis.com/ajax/services/search/web?v=1.0&start=0&rsz=small&q=", sIRCMessage.Info.SplitToString(4, " "));
			var Regex = new Regex(@".unescapedUrl.\:.(?<url>\S+).,.url.+.titleNoFormatting.\:.(?<title>.+).,.content");

			if(!Regex.IsMatch(url))
			{
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, sIRCMessage.Channel, "2Title: Nincs Title.");
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, sIRCMessage.Channel, "2Link: Nincs Link.");
			}
			else
			{
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, sIRCMessage.Channel, "2Title: {0}", Regex.Match(url).Groups["title"].ToString());
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, sIRCMessage.Channel, "2Link: 3{0}", Regex.Match(url).Groups["url"].ToString());
			}
		}

		protected void HandleFordit(IRCMessage sIRCMessage)
		{
			CNick(sIRCMessage);

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs megadva melyik nyelvről melyikre fordítsa le!");
				return;
			}

			if(sIRCMessage.Info.Length < 6)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs megadva a fordítandó szöveg!");
				return;
			}

			string url = sUtilities.GetUrl("http://ajax.googleapis.com/ajax/services/language/translate?v=1.0&q=", sIRCMessage.Info.SplitToString(5, " "), "&langpair=" + sIRCMessage.Info[4]);
			var Regex = new Regex(@"\{.translatedText.\:.(?<text>.+).\},");

			if(!Regex.IsMatch(url))
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs fórdított szöveg.");
			else
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "{0}", Regex.Match(url).Groups["text"].ToString());
		}

		protected void HandlePrime(IRCMessage sIRCMessage)
		{
			CNick(sIRCMessage);

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs megadva szám!");
				return;
			}

			double Num;
			bool isNum = double.TryParse(sIRCMessage.Info[4], out Num);

			if(!isNum)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nem csak számot tartalmaz!");
				return;
			}

			bool prim = sUtilities.IsPrime(Convert.ToInt32(Num));

			if(!prim)
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "{0} nem primszám.", sIRCMessage.Info[4]);
			else
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "{0} primszám.", sIRCMessage.Info[4]);
		}
	}
}
