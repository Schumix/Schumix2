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
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Schumix.Framework;
using Schumix.Framework.Config;
using WolframAPI;

namespace Schumix.Irc.Commands
{
	public partial class CommandHandler
	{
		protected void HandleXbot()
		{
			CNick();
			sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "3Verzió: 10{0}", Verzio.SchumixVerzio);
			sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "3Parancsok: {0}info | {0}help | {0}ido | {0}datum | {0}irc | {0}roll | {0}keres | {0}sha1 | {0}md5 | {0}uzenet | {0}whois | {0}calc | {0}prime", IRCConfig.Parancselojel);
			sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Programmed by: 3Csaba");
		}

		protected void HandleInfo()
		{
			CNick();
			sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Programozóm: Csaba, Jackneill.");
		}

		protected void HandleIdo()
		{
			CNick();

			if(DateTime.Now.Minute < 10)
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Helyi idő: {0}:0{1}", DateTime.Now.Hour, DateTime.Now.Minute);
			else
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Helyi idő: {0}:{1}", DateTime.Now.Hour, DateTime.Now.Minute);
		}

		protected void HandleDatum()
		{
			CNick();
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
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Ma {0}. 0{1}. 0{2}. {3} napja van.", Ev, Honap, Nap, napdb);
						else
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Ma {0}. 0{1}. {2}. {3} napja van.", Ev, Honap, Nap, napdb);
					}
					else
					{
						if(Nap < 10)
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Ma {0}. {1}. 0{2}. {3} napja van.", Ev, Honap, Nap, napdb);
						else
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Ma {0}. {1}. {2}. {3} napja van.", Ev, Honap, Nap, napdb);
					}
				}
		 	}
		}

		protected void HandleRoll()
		{
			CNick();
			Random rand = new Random();
			int szam = rand.Next(0, 100);
			sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Százalékos aránya {0}%", szam);
		}

		protected void HandleCalc()
		{
			CNick();
			string adat = "";
			for(int i = 4; i < Network.IMessage.Info.Length; i++)
				adat += " " + Network.IMessage.Info[i];

			if(adat.Substring(0, 1) == " ")
				adat = adat.Remove(0, 1);

			var client = new WAClient("557QYQ-UUUWTKX95V");
			var solution = client.Solve(adat);
			sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "{0}", solution);
		}

		protected void HandleSha1()
		{
			if(Network.IMessage.Info.Length < 5)
				return;

			CNick();
			sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, sUtility.Sha1(Network.IMessage.Info[4]));
		}

		protected void HandleMd5()
		{
			if(Network.IMessage.Info.Length < 5)
				return;

			CNick();
			Byte[] originalBytes;
			Byte[] encodedBytes;
			MD5 md5;

			md5 = new MD5CryptoServiceProvider();
			originalBytes = ASCIIEncoding.Default.GetBytes(Network.IMessage.Info[4]);
			encodedBytes = md5.ComputeHash(originalBytes);

			string convert = BitConverter.ToString(encodedBytes);
			string[] adat = convert.Split('-');
			string Md5 = "";

			for(int i = 0; i < adat.Length; i++)
				Md5 += adat[i];

			sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, Md5.ToLower());
		}

		protected void HandleIrc()
		{
			if(Network.IMessage.Info.Length < 5)
				return;

			CNick();

			var db = SchumixBase.mSQLConn.QueryFirstRow("SELECT hasznalata FROM irc_parancsok WHERE parancs = '{0}'", Network.IMessage.Info[4]);
			if(db != null)
			{
				string hasznalata = db["hasznalata"].ToString();
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, hasznalata);
			}
			else
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Hibás lekérdezés!");
		}

		protected void HandleWhois()
		{
			if(Network.IMessage.Info.Length < 5)
				return;
			
			CNick();
			WhoisPrivmsg = Network.IMessage.Channel;
			sSender.Whois(Network.IMessage.Info[4]);
		}

		protected void HandleUzenet()
		{
			if(Network.IMessage.Info.Length < 5)
				return;

			CNick();

			if(Network.IMessage.Info.Length == 5)
				sSendMessage.SendCMPrivmsg(Network.IMessage.Info[4], "Keresnek téged itt: {0}", Network.IMessage.Channel);
			else if(Network.IMessage.Info.Length >= 6)
			{
				string alomany = "";
				for(int i = 5; i < Network.IMessage.Info.Length; i++)
					alomany += Network.IMessage.Info[i] + " ";

				if(alomany.Substring(0, 1) == ":")
					alomany = alomany.Remove(0, 1);

				sSendMessage.SendCMPrivmsg(Network.IMessage.Info[4], "{0}", alomany);
			}
		}

		protected void HandleKeres()
		{
			if(Network.IMessage.Info.Length < 5)
				return;

			CNick();

			string adat = "";
			for(int i = 4; i < Network.IMessage.Info.Length; i++)
				adat += "%20" + Network.IMessage.Info[i];

			if(adat.Substring(0, 3) == "%20")
				adat = adat.Remove(0, 3);

			string url = sUtility.GetUrl("http://ajax.googleapis.com/ajax/services/search/web?v=1.0&start=0&rsz=small&q=" + adat);

			var Regex1 = new Regex(@".titleNoFormatting.:.(?<title>\S+).,.content.:.");
			if(!Regex1.IsMatch(url))
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "2Title: Nincs Title.");
			else
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "2Title: {0}", Regex1.Match(url).Groups["title"].ToString());

			var Regex = new Regex(@".unescapedUrl.:.(?<url>\S+).,.url.");
			if(!Regex.IsMatch(url))
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "2Link: Nincs Link.");
			else
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "2Link: 9{0}", Regex.Match(url).Groups["url"].ToString());
		}

		protected void HandlePrime()
		{
			if(Network.IMessage.Info.Length < 5)
				return;

			CNick();

			double Num;
			bool isNum = double.TryParse(Network.IMessage.Info[4], out Num);

			if(!isNum)
			{
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nem csak számot tartalmaz!");
				return;
			}

            bool prim = sUtility.IsPrime(Convert.ToInt32(Num));

			if(!prim)
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "{0} nem primszám.", Network.IMessage.Info[4]);
			else
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "{0} primszám.", Network.IMessage.Info[4]);
		}
	}
}
