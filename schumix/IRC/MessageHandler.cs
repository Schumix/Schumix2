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
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using Schumix.Config;

namespace Schumix.IRC
{
	public struct IRCMessage
	{
		public string Hostmask { get; set; }
		public string Channel { get; set; }
		public string Args { get; set; }
		public string Nick { get; set; }
		public string User { get; set; }
		public string Host { get; set; }
		public string[] Info { get; set; }
	}

	public partial class MessageHandler
	{
		private SendMessage sSendMessage = Singleton<SendMessage>.Instance;

        /// <summary>
        ///     Az IRC szobához tartozó funkciók.
        /// </summary>
		public List<string> m_ChannelFunkcio = new List<string>();

        /// <summary>
        ///     Tárolja azt az IRC szoba címet, amit betölt a bot.
        /// </summary>
		private string m_ChannelPrivmsg;

        /// <summary>
        ///     Tárolja azt az IRC szoba címet, amit betölt a bot.
        /// </summary>
		private string m_WhoisPrivmsg;

		private MessageHandler() {}

		public void HandleSuccessfulAuth()
		{
			Network.m_ConnState = (int)Network.ConnState.CONN_REGISTERED;
			Console.Write("\n");
			Log.Success("Opcode", "Sikeres kapcsolodas");
			if(IRCConfig.UseNickServ == 1)
			{
				Log.Notice("NickServ", "Sending NickServ identification.");
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, "NickServ", String.Format("identify {0}", IRCConfig.NickServPassword));
			}

			m_WhoisPrivmsg = SchumixBot.NickTarolo;
			m_ChannelPrivmsg = SchumixBot.NickTarolo;
			foreach(var m_channel in SchumixBot.m_ChannelLista)
			{
				Network.writer.WriteLine("JOIN " + m_channel.Key + m_channel.Value);
				SchumixBot.mSQLConn.QueryFirstRow(String.Format("UPDATE channel SET aktivitas = 'aktiv', error = '' WHERE szoba = '{0}'", m_channel.Key));
			}

			ChannelFunkcioReload();
		}

        /// <summary>
        ///     Ha a szobában a köszönés funkció be van kapcsolva,
        ///     akkor köszön az éppen belépőnek.
        /// </summary>
        /// <param name="info">Egyszerű adat, ami az IRC szerver felől jön.</param>
		public void HandleJoin()
		{
			if(Network.IMessage.Nick == SchumixBot.NickTarolo)
				return;

			string channel = Network.IMessage.Channel;

			if(channel.Substring(0, 1) == ":")
				channel = channel.Remove(0, 1);

			if(FSelect("koszones") == "be" && FSelect("koszones", channel) == "be")
			{
				Random rand = new Random();
				string Koszones = "";
				switch(rand.Next(0, 12))
				{
					case 0:
						Koszones = "Hello";
						break;
					case 1:
						Koszones = "Csáó";
						break;
					case 2:
						Koszones = "Hy";
						break;
					case 3:
						Koszones = "Szevasz";
						break;
					case 4:
						Koszones = "Üdv";
						break;
					case 5:
						Koszones = "Szervusz";
						break;
					case 6:
						Koszones = "Aloha";
						break;
					case 7:
						Koszones = "Jó napot";
						break;
					case 8:
						Koszones = "Szia";
						break;
					case 9:
						Koszones = "Hi";
						break;
					case 10:
						Koszones = "Szerbusz";
						break;
					case 11:
						Koszones = "Hali";
						break;
					case 12:
						Koszones = "Szeva";
						break;
				}
	
				if(DateTime.Now.Hour <= 9)
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, channel, String.Format("Jó reggelt {0}", Network.IMessage.Nick));
				else if(DateTime.Now.Hour >= 20)
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, channel, String.Format("Jó estét {0}", Network.IMessage.Nick));
				else
					if(CManager.Admin(Network.IMessage.Nick))
						sSendMessage.SendChatMessage(MessageType.PRIVMSG, channel, "Üdv főnök");
					else
						sSendMessage.SendChatMessage(MessageType.PRIVMSG, channel, String.Format("{0} {1}", Koszones, Network.IMessage.Nick));
			}
		}

        /// <summary>
        ///     Ha ez a funkció be van kapcsolva, akkor
        ///     miután a nick elhagyta a szobát elköszön tőle.
        /// </summary>
        /// <param name="info">Egyszerű adat, ami az IRC szerver felől jön.</param>
		public void HandleLeft()
		{
			if(Network.IMessage.Nick == SchumixBot.NickTarolo)
				return;

			if(FSelect("koszones") == "be" && FSelect("koszones", Network.IMessage.Channel) == "be")
			{
				Random rand = new Random();
				string elkoszones = "";
				switch(rand.Next(0, 1))
				{
					case 0:
						elkoszones = "Viszlát";
						break;
					case 1:
						elkoszones = "Bye";
						break;
				}
	
				if(DateTime.Now.Hour >= 20)
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("Jóét {0}", Network.IMessage.Nick));
				else
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("{0} {1}", elkoszones, Network.IMessage.Nick));
			}
		}

        /// <summary>
        ///     Ha a ConsoleLog be van kapcsolva, akkor
        ///     kiírja a console-ra az IRC szerverről fogadott információkat.
        /// </summary>
        /// <param name="info">Egyszerű adat, ami az IRC szerver felől jön.</param>
		public void HandleNotice()
		{
			if(Consol.ConsoleLog == "be")
			{
				string args = Network.IMessage.Args;

				if(args.Substring(0, 1) == ":")
					args = args.Remove(0, 1);

				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write("[SERVER] ");
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.Write(args + "\n");
				Console.ForegroundColor = ConsoleColor.Gray;
			}
		}

        /// <summary>
        ///     Válaszol, ha valaki pingeli a botot.
        /// </summary>
        /// <param name="info">Egyszerű adat, ami az IRC szerver felől jön.</param>
		public void HandlePing()
		{
			string args = Network.IMessage.Args;

			if(args.Substring(0, 1) == ":")
				args = args.Remove(0, 1);

			Network.writer.WriteLine("PING :{0}", args);
		}

        /// <summary>
        ///     Válaszol, ha valaki pongolja a botot.
        /// </summary>
        /// <param name="info">Egyszerű adat, ami az IRC szerver felől jön.</param>
		public void HandlePong()
		{
			string args = Network.IMessage.Args;

			if(args.Substring(0, 1) == ":")
				args = args.Remove(0, 1);

			Network.writer.WriteLine("PONG :{0}", args);
			Network.Status = true;
		}

        /// <summary>
        ///     Ha ismeretlen parancs jön, akkor kiírja.
        /// </summary>
        /// <param name="info">Egyszerű adat, ami az IRC szerver felől jön.</param>
		public void HandleIsmeretlenParancs()
		{
			if(Consol.ConsoleLog == "be")
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write("[SERVER] ");
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.Write("Nemletezo irc parancs\n");
				Console.ForegroundColor = ConsoleColor.Gray;
			}
		}

        /// <summary>
        ///     Ha a bot elsődleges nickje már használatban van, akkor
        ///     átlép a másodlagosra, ha az is akkor a harmadlagosra.
        /// </summary>
        /// <param name="info">Egyszerű adat, ami az IRC szerver felől jön.</param>
		public void HandleNickError()
		{
			if(SchumixBot.NickTarolo == IRCConfig.NickName)
			{
				SchumixBot.NickTarolo = IRCConfig.NickName2;
				Network.m_ConnState = (int)Network.ConnState.CONN_CONNECTED;
				return;
			}
			else if(SchumixBot.NickTarolo == IRCConfig.NickName2)
			{
				SchumixBot.NickTarolo = IRCConfig.NickName3;
				Network.m_ConnState = (int)Network.ConnState.CONN_CONNECTED;
				return;
			}
			else if(SchumixBot.NickTarolo == IRCConfig.NickName3)
			{
				SchumixBot.NickTarolo = "_Schumix2";
				Network.m_ConnState = (int)Network.ConnState.CONN_CONNECTED;
				return;
			}
			else if(SchumixBot.NickTarolo == "_Schumix2")
			{
				SchumixBot.NickTarolo = "__Schumix2";
				Network.m_ConnState = (int)Network.ConnState.CONN_CONNECTED;
				return;
			}
			else if(SchumixBot.NickTarolo == "__Schumix2")
			{
				SchumixBot.NickTarolo = "_Schumix2_";
				Network.m_ConnState = (int)Network.ConnState.CONN_CONNECTED;
				return;
			}
			else if(SchumixBot.NickTarolo == "_Schumix2_")
			{
				SchumixBot.NickTarolo = "__Schumix2_";
				Network.m_ConnState = (int)Network.ConnState.CONN_CONNECTED;
				return;
			}
			else if(SchumixBot.NickTarolo == "__Schumix2_")
			{
				SchumixBot.NickTarolo = "__Schumix2__";
				Network.m_ConnState = (int)Network.ConnState.CONN_CONNECTED;
				return;
			}
			else if(SchumixBot.NickTarolo == "__Schumix2__")
			{
				SchumixBot.NickTarolo = IRCConfig.NickName;
				Network.m_ConnState = (int)Network.ConnState.CONN_CONNECTED;
				return;
			}
		}

        /// <summary>
		///     Ha bannolva van egy szobából, akkor feljegyzi.
        /// </summary>
        /// <param name="info">Egyszerű adat, ami az IRC szerver felől jön.</param>
		public void HandleChannelBan()
		{
			if(Network.IMessage.Info.Length < 4)
				return;

			SchumixBot.mSQLConn.QueryFirstRow(String.Format("UPDATE channel SET aktivitas = 'nem aktiv', error = 'channel ban' WHERE szoba = '{0}'", Network.IMessage.Info[3]));
			sSendMessage.SendChatMessage(MessageType.PRIVMSG, m_ChannelPrivmsg, String.Format("{0}: channel ban", Network.IMessage.Info[3]));
			m_ChannelPrivmsg = SchumixBot.NickTarolo;
		}

        /// <summary>
        ///     Ha hibás egy IRC szobának a jelszava, akkor feljegyzi.
        /// </summary>
        /// <param name="info">Egyszerű adat, ami az IRC szerver felől jön.</param>
		public void HandleNoChannelJelszo()
		{
			if(Network.IMessage.Info.Length < 4)
				return;

			SchumixBot.mSQLConn.QueryFirstRow(String.Format("UPDATE channel SET aktivitas = 'nem aktiv', error = 'hibas channel jelszo' WHERE szoba = '{0}'", Network.IMessage.Info[3]));
			sSendMessage.SendChatMessage(MessageType.PRIVMSG, m_ChannelPrivmsg, String.Format("{0}: hibás channel jelszó", Network.IMessage.Info[3]));
			m_ChannelPrivmsg = SchumixBot.NickTarolo;
		}

        /// <summary>
        ///     Kigyűjti éppen hol van fent a nick.
        /// </summary>
        /// <param name="info">Egyszerű adat, ami az IRC szerver felől jön.</param>
		public void HandleWhois()
		{
			if(Network.IMessage.Info.Length < 5)
				return;

			string alomany = "";
			for(int i = 4; i < Network.IMessage.Info.Length; i++)
				alomany += Network.IMessage.Info[i] + " ";

			if(alomany.Substring(0, 1) == ":")
				alomany = alomany.Remove(0, 1);

			sSendMessage.SendChatMessage(MessageType.PRIVMSG, m_WhoisPrivmsg, String.Format("Jelenleg itt van fent: {0}", alomany));
			m_WhoisPrivmsg = SchumixBot.NickTarolo;
		}

        /// <summary>
        ///     Ha engedélyezett a ConsolLog, akkor kiírja a Console-ra ha kickelnek valakit.
        /// </summary>
        /// <param name="info">Egyszerű adat, ami az IRC szerver felől jön.</param>
		public void HandleKick()
		{
			if(Network.IMessage.Info.Length < 5)
				return;

			if(Network.IMessage.Info[3] == SchumixBot.NickTarolo)
			{
				if(FSelect("rejoin") == "be" && FSelect("rejoin", Network.IMessage.Channel) == "be")
				{
					foreach(var m_channel in SchumixBot.m_ChannelLista)
					{
						if(Network.IMessage.Channel == m_channel.Key)
							Network.writer.WriteLine("JOIN " + m_channel.Key + m_channel.Value);
					}
				}
			}
			else
			{
				if(FSelect("parancsok") == "be" && FSelect("parancsok", Network.IMessage.Channel) == "be")
				{
					if(Consol.ConsoleLog == "be")
					{
						string alomany = "";
						for(int i = 4; i < Network.IMessage.Info.Length; i++)
							alomany += Network.IMessage.Info[i] + " ";
			
						if(alomany.Substring(0, 1) == ":")
							alomany = alomany.Remove(0, 1);
			
						Console.WriteLine("{0} kickelte a következő felhasználot: {1} oka: {2}", Network.IMessage.Nick, Network.IMessage.Info[3], alomany);
					}
				}
			}
		}

        /// <summary>
        ///     Random karakter kombináció.
        ///     Használható a jelszógeneráláshoz.
        /// </summary>
        /// <returns></returns>
		private string GetRandomString()
		{
			string path = Path.GetRandomFileName();
			path = path.Replace(".", "");
			return path;
		}

        /// <summary>
        ///     
        /// </summary>
        /// <param name="nev"></param>
        /// <returns></returns>
		private string FSelect(string nev)
		{
			string status = "";

			var db = SchumixBot.mSQLConn.QueryFirstRow(String.Format("SELECT funkcio_status FROM schumix WHERE funkcio_nev = '{0}'", nev));
			if(db != null)
				status = db["funkcio_status"].ToString();

			return status;
		}

        /// <summary>
        ///     
        /// </summary>
        /// <param name="nev"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
		private string FSelect(string nev, string channel)
		{
			string status = "";

			for(int i = 0; i < m_ChannelFunkcio.Count; i++)
			{
				string szobak = m_ChannelFunkcio[i];
				string[] pont = szobak.Split('.');
				string szoba = pont[0];
				string funkciok = pont[1];
				string[] kettospont = funkciok.Split(':');

				if(szoba == channel)
				{
					if(kettospont[0] == nev)
						status = kettospont[1];
				}
			}

			return status;
		}

        /// <summary>
        ///     
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
		private string GetUrl(string url)
		{
			WebClient web = new WebClient();
			string kod = web.DownloadString(url);
			web.Dispose();
			return kod;
		}

        /// <summary>
        ///     
        /// </summary>
        /// <param name="info"></param>
		private void Ido(string[] info)
		{
			if(DateTime.Now.Minute < 10)
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, info[2], String.Format("Helyi idő: {0}:0{1}", DateTime.Now.Hour, DateTime.Now.Minute));
			else
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, info[2], String.Format("Helyi idő: {0}:{1}", DateTime.Now.Hour, DateTime.Now.Minute));
		}

        /// <summary>
        ///     
        /// </summary>
        /// <param name="info"></param>
		private void Datum(string[] info)
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
							sSendMessage.SendChatMessage(MessageType.PRIVMSG, info[2], String.Format("Ma {0}. 0{1}. 0{2}. {3} napja van.", Ev, Honap, Nap, napdb));
						else
							sSendMessage.SendChatMessage(MessageType.PRIVMSG, info[2], String.Format("Ma {0}. 0{1}. {2}. {3} napja van.", Ev, Honap, Nap, napdb));
					}
					else
					{
						if(Nap < 10)
							sSendMessage.SendChatMessage(MessageType.PRIVMSG, info[2], String.Format("Ma {0}. {1}. 0{2}. {3} napja van.", Ev, Honap, Nap, napdb));
						else
							sSendMessage.SendChatMessage(MessageType.PRIVMSG, info[2], String.Format("Ma {0}. {1}. {2}. {3} napja van.", Ev, Honap, Nap, napdb));
					}
				}
		 	}
		}

        /// <summary>
        ///     
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="user"></param>
        /// <param name="args"></param>
		private void LogToFajl(string channel, string user, string args)
        {
			if(FSelect("log") == "be" && FSelect("log", channel) == "be")
			{
				string logfile_name = channel + ".log";
				if(!File.Exists(String.Format("./szoba/{0}", logfile_name)))
					File.Create(String.Format("./szoba/{0}", logfile_name));
	
				if(!Directory.Exists("szoba"))
					Directory.CreateDirectory("szoba");
	
				try
				{
					TextWriter writeFile = new StreamWriter(String.Format("./szoba/{0}", logfile_name), true);
					writeFile.WriteLine("[{0}] <{1}> {2}", DateTime.Now, user, args);
					writeFile.Flush();
					writeFile.Close();
				}
				catch { }
			}
		}

        /// <summary>
        ///     
        /// </summary>
		public void ChannelFunkcioReload()
		{
			m_ChannelFunkcio.Clear();

			var db = SchumixBot.mSQLConn.QueryRow(String.Format("SELECT szoba FROM channel"));
			for(int i = 0; i < db.Rows.Count; ++i)
			{
				var row = db.Rows[i];

				string szoba = row["szoba"].ToString();
				var db1 = SchumixBot.mSQLConn.QueryFirstRow(String.Format("SELECT funkciok FROM channel WHERE szoba = '{0}'", szoba));
				if(db1 != null)
				{
					string funkciok = db1["funkciok"].ToString();
					string[] vesszo = funkciok.Split(',');

					for(int x = 1; x < vesszo.Length; x++)
					{
						string szobaadat = szoba + "." + vesszo[x];
						m_ChannelFunkcio.Add(szobaadat);
					}
				}
			}
		}

		public void ChannelListaReload()
		{
			SchumixBot.m_ChannelLista.Clear();
			var dbinfo = SchumixBot.mSQLConn.QueryRow(String.Format("SELECT szoba, jelszo FROM channel"));
			for(int i = 0; i < dbinfo.Rows.Count; ++i)
			{
				var row = dbinfo.Rows[i];
				string szoba = row["szoba"].ToString();
				string jelszo = row["jelszo"].ToString();
				SchumixBot.m_ChannelLista.Add(szoba, jelszo);
			}
		}

		public string ChannelFunkciok(string nev, string status, string channel)
		{
			string funkcio = "";

			for(int i = 0; i < m_ChannelFunkcio.Count; i++)
			{
				string szobak = m_ChannelFunkcio[i];
				string[] pont = szobak.Split('.');
				string szoba = pont[0];
				string funkciok = pont[1];
				string[] kettospont = funkciok.Split(':');

				if(szoba == channel)
				{
					if(kettospont[0] != nev)
						funkcio += "," + funkciok;
				}
			}

			for(int i = 0; i < m_ChannelFunkcio.Count; i++)
			{
				string szobak = m_ChannelFunkcio[i];
				string[] pont = szobak.Split('.');
				string szoba = pont[0];
				string funkciok = pont[1];
				string[] kettospont = funkciok.Split(':');

				if(szoba == channel)
				{
					if(kettospont[0] == nev)
						funkcio += "," + nev + ":" + status;
				}
			}

			return funkcio;
		}
	}
}