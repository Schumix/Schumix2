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

// nincs egyenlőre adat

using System;
using Schumix.Config;

namespace Schumix.IRC.Commands
{
	public partial class CommandHandler
	{
		public void HandleXbot()
		{
			sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("3Verzió: 10{0}", SchumixBot.revision));
			sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("3Parancsok: {0}info | {1}help | {2}ido | {3}datum | {4}irc | {5}roll | {6}keres | {7}sha1 | {8}md5 | {9}uzenet | {10}whois | {11}calc", IRCConfig.Parancselojel, IRCConfig.Parancselojel, IRCConfig.Parancselojel, IRCConfig.Parancselojel, IRCConfig.Parancselojel, IRCConfig.Parancselojel, IRCConfig.Parancselojel, IRCConfig.Parancselojel, IRCConfig.Parancselojel, IRCConfig.Parancselojel, IRCConfig.Parancselojel, IRCConfig.Parancselojel));
			sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Programmed by: 3Csaba");
		}

		public void HandleInfo()
		{
			sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Programozóm: Csaba, Jackneill.");
		}

		public void HandleHelp()
		{
			sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Ha egy parancs mögé irod a help-et segít a használatában!");
			sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("Fő parancsom: {0}xbot", IRCConfig.Parancselojel));
		}

		public void HandleIdo()
		{
			if(DateTime.Now.Minute < 10)
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("Helyi idő: {0}:0{1}", DateTime.Now.Hour, DateTime.Now.Minute));
			else
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("Helyi idő: {0}:{1}", DateTime.Now.Hour, DateTime.Now.Minute));
		}

		public void HandleDatum()
		{
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
							sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("Ma {0}. 0{1}. 0{2}. {3} napja van.", Ev, Honap, Nap, napdb));
						else
							sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("Ma {0}. 0{1}. {2}. {3} napja van.", Ev, Honap, Nap, napdb));
					}
					else
					{
						if(Nap < 10)
							sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("Ma {0}. {1}. 0{2}. {3} napja van.", Ev, Honap, Nap, napdb));
						else
							sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("Ma {0}. {1}. {2}. {3} napja van.", Ev, Honap, Nap, napdb));
					}
				}
		 	}
		}

		public void HandleRoll()
		{
			Random rand = new Random();
			int szam = rand.Next(0, 100);
			sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("Százalékos aránya {0}%", szam));
		}

		public void HandleCalc()
		{
			string adat = "";
			for(int i = 4; i < Network.IMessage.Info.Length; i++)
				adat += " " + Network.IMessage.Info[i];

			if(adat.Substring(0, 1) == " ")
				adat = adat.Remove(0, 1);

			double ris = Eval.Calculate(adat);
			sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("{0}", ris));
		}
	}
}
