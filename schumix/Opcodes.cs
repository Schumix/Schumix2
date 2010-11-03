/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010 Megax <http://www.megaxx.info/>
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
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Diagnostics;
using MySql.Data;

namespace Schumix
{
	public class Opcodes
	{
		//private SchumixBot sSchumixBot = Singleton<SchumixBot>.Instance;

        /// <summary>
        ///     A Nick adatait tartalmazza.
        /// </summary>
		public string hostmask;

        /// <summary>
        ///     Az IRC szobában írott sorok.
        /// </summary>
		public string args;

        /// <summary>
        ///     A Nickname.
        /// </summary>
		public string source_nick;

        /// <summary>
        ///     
        /// </summary>
		private string[] userdata;

        /// <summary>
        ///     
        /// </summary>
		private string[] hostdata;

		//private string source_user;

        /// <summary>
        ///     
        /// </summary>
		private string source_host;

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

        /// <summary>
        ///     Meghatározza, hogy PRIVMSG vagy NOTICE legyen az üzenetküldés módja.
        /// </summary>
		public enum MessageType
		{
			PRIVMSG,
			NOTICE,
		};

        /// <summary>
        ///     Szükséges, hogy jó legyen a Singleton.
        /// </summary>
		private Opcodes() {}

        /// <summary>
        ///     Ha a NickServ identify be van állítva, akkor elküldi a jelszót.
        ///     Belép a szobákba.
        /// </summary>
        /// <param name="info">Egyszerű adat, ami az IRC szerver felől jön.</param>
		public void OpcodeSikeresKapcsolodas(string[] info)
		{
			Network.m_ConnState = (int)Network.ConnState.CONN_REGISTERED;
			Console.Write("\n");
			Log.Success("Opcode", "Sikeres kapcsolodas");
			if(SchumixBot.Aktivitas == 1)
			{
				Log.Notice("NickServ", "Sending NickServ identification.");
				SendChatMessage(MessageType.PRIVMSG, "NickServ", String.Format("identify {0}", SchumixBot.Jelszo));
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
        ///     A bot parancsok ebben a függvényben vannak leírva.
        /// </summary>
        /// <param name="info">Egyszerű adat, ami az IRC szerver felől jön.</param>
		public void OpcodePrivmsg(string[] info)
		{
			if(info[3].Substring(0, 1) == ":")
				info[3] = info[3].Remove(0, 1);

			string channel = info[2];
			if(args.Substring(0, 1) == ":")
				args = args.Remove(0, 1);

			userdata = hostmask.Split('!');
			hostdata = userdata[1].Split('@');
			//source_user = hostdata[0];
			source_host = hostdata[1];

			if(Consol.ConsolLog == "be")
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("[{0}] <{1}> {2}", channel, source_nick, args);
				Console.ForegroundColor = ConsoleColor.Gray;
			}

			LogToFajl(channel, source_nick, args);
			Schumix(info);

			if(info[info.Length-2] == "" || info[info.Length-1] == "")
				return;

			if(info[3].Substring(0, 1) == " " || info[3].Substring(0, 1) != SchumixBot._parancselojel)
				return;

			info[3] = info[3].Remove(0, 1);

			if(info[3] == "hozzaferes")
			{
				if(!Admin(source_nick))
					return;

				if(info.Length < 5)
					return;

				string jelszo = info[4];
				string ip = source_host;
				string admin_nev = source_nick;
				string Nev = "";
				string JelszoSql = "";

				var db = SchumixBot.mSQLConn.QueryFirstRow(String.Format("SELECT nev, jelszo FROM adminok WHERE nev = '{0}'", admin_nev));
				if(db != null)
				{
					Nev = db["nev"].ToString();
					JelszoSql = db["jelszo"].ToString();
				}

				if(JelszoSql == jelszo)
				{
					SchumixBot.mSQLConn.QueryFirstRow(String.Format("UPDATE adminok SET ip = '{0}' WHERE nev = '{1}'", ip, Nev));
					SendChatMessage(MessageType.PRIVMSG, Nev, "Hozzáférés engedélyezve");
				}
				else
					SendChatMessage(MessageType.PRIVMSG, Nev, "Hozzáférés megtagadva");
			}

			if(info[3] == "ujjelszo")
			{
				if(!Admin(source_nick))
					return;

				if(info.Length < 6)
					return;

				string admin_nev = source_nick;
				string jelszo = info[4];
				string ujjelszo = info[5];
				string Nev = "";
				string JelszoSql = "";

				var db = SchumixBot.mSQLConn.QueryFirstRow(String.Format("SELECT nev, jelszo FROM adminok WHERE nev = '{0}'", admin_nev));
				if(db != null)
				{
					Nev = db["nev"].ToString();
					JelszoSql = db["jelszo"].ToString();
				}

				if(JelszoSql == jelszo)
				{
					SchumixBot.mSQLConn.QueryFirstRow(String.Format("UPDATE adminok SET jelszo = '{0}' WHERE nev = '{1}'", ujjelszo, Nev));
					SendChatMessage(MessageType.PRIVMSG, Nev, String.Format("Jelszó sikereset meg lett változtatva erre: {0}", ujjelszo));
				}
				else
					SendChatMessage(MessageType.PRIVMSG, Nev, "A mostani jelszó nem egyezik, modósitás megtagadva");
			}

			if(FSelect("parancsok") != "be" && FSelect("parancsok", channel) != "be")
				return;

			//Parancsok
			if(info[3] == "xbot")
			{
				SendChatMessage(MessageType.PRIVMSG, channel, String.Format("3Verzió: 10{0}", SchumixBot.revision));
				SendChatMessage(MessageType.PRIVMSG, channel, String.Format("3Parancsok: {0}info | {1}help | {2}ido | {3}datum | {4}irc | {5}roll | {6}keres | {7}sha1 | {8}md5 | {9}uzenet | {10}whois | {11}calc", SchumixBot._parancselojel, SchumixBot._parancselojel, SchumixBot._parancselojel, SchumixBot._parancselojel, SchumixBot._parancselojel, SchumixBot._parancselojel, SchumixBot._parancselojel, SchumixBot._parancselojel, SchumixBot._parancselojel, SchumixBot._parancselojel, SchumixBot._parancselojel, SchumixBot._parancselojel));
				SendChatMessage(MessageType.PRIVMSG, channel, "Programmed by: 3Csaba");
				return;
			}

			if(info[3] == "admin")
			{
				if(!Admin(source_nick, source_host))
					return;

				if(info.Length >= 5 && info[4] == "help")
				{
					SendChatMessage(MessageType.PRIVMSG, channel, "Alparancsok használata:");
					SendChatMessage(MessageType.PRIVMSG, channel, String.Format("Admin lista: {0}admin lista", SchumixBot._parancselojel));
					SendChatMessage(MessageType.PRIVMSG, channel, String.Format("Hozzáadás: {0}admin add <admin neve>", SchumixBot._parancselojel));
					SendChatMessage(MessageType.PRIVMSG, channel, String.Format("Eltávolítás: {0}admin del <admin neve>", SchumixBot._parancselojel));
				}
				else if(info.Length >= 5 && info[4] == "lista")
				{
					string adminok = "";
					var db = SchumixBot.mSQLConn.QueryRow(String.Format("SELECT nev FROM adminok"));
					for(int i = 0; i < db.Rows.Count; ++i)
					{
						var row = db.Rows[i];

						string nev = row["nev"].ToString();
						adminok += ", " + nev;
					}

					if(adminok.Substring(0, 2) == ", ")
						adminok = adminok.Remove(0, 2);

					SendChatMessage(MessageType.PRIVMSG, channel, String.Format("2Adminok: {0}", adminok));
				}
				else if(info.Length >= 5 && info[4] == "add")
				{
					if(info.Length < 6)
						return;

					string nev = info[5];
					string pass = GetRandomString();

					SchumixBot.mSQLConn.QueryFirstRow(String.Format("INSERT INTO `adminok`(nev, jelszo) VALUES ('{0}', '{1}')", nev.ToLower(), pass));

					SendChatMessage(MessageType.PRIVMSG, channel, String.Format("Admin hozzáadva: {0}", nev));
					SendChatMessage(MessageType.PRIVMSG, nev, String.Format("Mostantól Schumix adminja vagy. A te mostani jelszavad: {0}", pass));
					SendChatMessage(MessageType.PRIVMSG, nev, String.Format("Ha megszeretnéd változtatni használd az {0}ujjelszo parancsot. Használata: {1}ujjelszo <régi> <új>", SchumixBot._parancselojel, SchumixBot._parancselojel));
					SendChatMessage(MessageType.PRIVMSG, nev, String.Format("Admin nick élesítése: {0}hozzaferes <jelszó>", SchumixBot._parancselojel));
				}
				else if(info.Length >= 5 && info[4] == "del")
				{
					if(info.Length < 6)
						return;

					string nev = info[5];
					SchumixBot.mSQLConn.QueryFirstRow(String.Format("DELETE FROM `adminok` WHERE nev = '{0}'", nev.ToLower()));
					SendChatMessage(MessageType.PRIVMSG, channel, String.Format("Admin törölve: {0}", nev));
				}
				else
				{
					SendChatMessage(MessageType.PRIVMSG, channel, String.Format("3Parancsok: {0}nick | {0}join | {0}left | {0}kick | {0}mode", SchumixBot._parancselojel));
					SendChatMessage(MessageType.PRIVMSG, channel, String.Format("3Parancsok: {0}szinek | {0}funkcio | {0}kikapcs | {0}sznap | {0}szoba | {0}channel | {0}hozzaferes | {0}ujjelszo", SchumixBot._parancselojel));
					return;
				}
			}

			if(info[3] == "info")
			{
				SendChatMessage(MessageType.PRIVMSG, channel, "Programozóm: Csaba, Jackneill.");
				return;
			}

			if(info[3] == "help")
			{
				SendChatMessage(MessageType.PRIVMSG, channel, "Ha egy parancs mögé irod a help-et segít a használatában!");
				SendChatMessage(MessageType.PRIVMSG, channel, String.Format("Fő parancsom: {0}xbot", SchumixBot._parancselojel));
				return;
			}

			if(info[3] == "ido")
				Ido(info);

			if(info[3] == "datum")
				Datum(info);

			if(info[3] == "roll")
			{
				Random rand = new Random();
				int szam = rand.Next(0, 100);
				SendChatMessage(MessageType.PRIVMSG, channel, String.Format("Százalékos aránya {0}%", szam));
			}

			if(info[3] == "teszt")
			{
				if(!Admin(source_nick, source_host))
					return;

				if(info.Length >= 5 && info[4] == "adat")
				{
					SendChatMessage(MessageType.PRIVMSG, channel, "Teszt probálkozás");
				}
				else if(info.Length >= 5 && info[4] == "db")
				{
					var db = SchumixBot.mSQLConn.QueryRow(String.Format("SELECT nev FROM adminok"));
					for(int i = 0; i < db.Rows.Count; ++i)
					{
						var row = db.Rows[i];

						string admin = row["nev"].ToString();
						SendChatMessage(MessageType.PRIVMSG, channel, String.Format("{0}", admin));
					}
				}
				else
					SendChatMessage(MessageType.PRIVMSG, channel, String.Format("{0}", info.Length));
			}

			if(info[3] == "szinek")
			{
				if(!Admin(source_nick, source_host))
					return;

				SendChatMessage(MessageType.PRIVMSG, channel, "1teszt1 2teszt2 3teszt3 4teszt4 5teszt5 6teszt6 7teszt7 8teszt8");
				SendChatMessage(MessageType.PRIVMSG, channel, "9teszt9 10teszt10 11teszt11 12teszt12 13teszt13 14teszt14 15teszt15");
				return;
			}

			if(info[3] == "nick")
			{
				if(!Admin(source_nick, source_host))
					return;

				if(info.Length < 5)
					return;

				string nick = info[4];
				SchumixBot.NickTarolo = nick;
				Network.writer.WriteLine(String.Format("NICK {0}", nick));
			}

			if(info[3] == "join")
			{
				if(!Admin(source_nick, source_host))
					return;

				m_ChannelPrivmsg = channel;

				if(info.Length == 5)
					Network.writer.WriteLine(String.Format("JOIN {0}", info[4]));
				else if(info.Length == 6)
					Network.writer.WriteLine(String.Format("JOIN {0} {1}", info[4], info[5]));
			}

			if(info[3] == "left")
			{
				if(!Admin(source_nick, source_host))
					return;

				if(info.Length < 5)
					return;

				Network.writer.WriteLine(String.Format("PART {0}", info[4]));
			}

			if(info[3] == "kick")
			{
				if(!Admin(source_nick, source_host))
					return;

				if(info.Length < 5)
					return;

				string kick = info[4].ToLower();
				int szam = info.Length;

				if(szam == 5)
				{
					if(kick != SchumixBot.NickTarolo.ToLower())
						Network.writer.WriteLine(String.Format("KICK {0} {1}", channel, kick));
				}
				else if(szam >= 6)
				{
					string oka = "";
					for(int i = 5; i < info.Length; i++)
						oka += info[i] + " ";

					if(oka.Substring(0, 1) == ",")
						oka = oka.Remove(0, 1);

					if(kick != SchumixBot.NickTarolo)
						Network.writer.WriteLine(String.Format("KICK {0} {1} :{2}", channel, kick, oka));
				}
			}

			if(info[3] == "mode")
			{
				if(!Admin(source_nick, source_host))
					return;

				if(info.Length < 6)
					return;

				string rang = info[4].ToLower();
				string nev = "";

				for(int i = 5; i < info.Length; i++)
					nev += info[i] + " ";

				if(nev.Substring(0, 1) == " ")
					nev = nev.Remove(0, 1);

				if(nev != SchumixBot.NickTarolo.ToLower())
					Network.writer.WriteLine(String.Format("MODE {0} {1} {2}", channel, rang, nev));
			}

			if(info[3] == "sha1")
			{
				if(info.Length < 5)
					return;

				Byte[] originalBytes;
				Byte[] encodedBytes;
				SHA1 sha1;

				sha1 = new SHA1CryptoServiceProvider();
				originalBytes = ASCIIEncoding.Default.GetBytes(info[4]);
				encodedBytes = sha1.ComputeHash(originalBytes);

				string convert = BitConverter.ToString(encodedBytes);

				string[] adat = new string[convert.Split('-').Length];
				adat = convert.Split('-');

				string Sha1 = "";

				for(int i = 0; i < adat.Length; i++)
					Sha1 += adat[i];

				SendChatMessage(MessageType.PRIVMSG, channel, Sha1.ToLower());
			}

			if(info[3] == "md5")
			{
				if(info.Length < 5)
					return;

				Byte[] originalBytes;
				Byte[] encodedBytes;
				MD5 md5;

				md5 = new MD5CryptoServiceProvider();
				originalBytes = ASCIIEncoding.Default.GetBytes(info[4]);
				encodedBytes = md5.ComputeHash(originalBytes);

				string convert = BitConverter.ToString(encodedBytes);

				string[] adat = new string[convert.Split('-').Length];
				adat = convert.Split('-');

				string Md5 = "";

				for(int i = 0; i < adat.Length; i++)
					Md5 += adat[i];

				SendChatMessage(MessageType.PRIVMSG, channel, Md5.ToLower());
			}

			if(info[3] == "funkcio")
			{
				if(!Admin(source_nick, source_host))
					return;

				if(info.Length < 5)
					return;

				if(info[4] == "help")
				{
					SendChatMessage(MessageType.PRIVMSG, channel, "Alparancsok használata:");
					SendChatMessage(MessageType.PRIVMSG, channel, String.Format("Channel kezelés: {0}funkcio <be vagy ki> <funkcio név>", SchumixBot._parancselojel));
					SendChatMessage(MessageType.PRIVMSG, channel, String.Format("Channel kezelés máshonnét: {0}funkcio channel <channel név> <be vagy ki> <funkcio név>", SchumixBot._parancselojel));
					SendChatMessage(MessageType.PRIVMSG, channel, String.Format("Együtes kezelés: {0}funkcio all <be vagy ki> <funkcio név>", SchumixBot._parancselojel));
					SendChatMessage(MessageType.PRIVMSG, channel, String.Format("Update kezelése: {0}funkcio update", SchumixBot._parancselojel));
				}
				else if(info[4] == "info")
				{
					string be = "";
					string ki = "";

					for(int i = 0; i < m_ChannelFunkcio.Count; i++)
					{
						string szobak = m_ChannelFunkcio[i];
						string[] pont = new string[szobak.Split('.').Length];
						pont = szobak.Split('.');
						string szoba = pont[0];
						string funkciok = pont[1];

						string[] kettospont = new string[funkciok.Split(':').Length];
						kettospont = funkciok.Split(':');

						if(szoba == channel)
						{
							if(kettospont[1] == "be")
								be += kettospont[0] + " ";
							else
								ki += kettospont[0] + " ";
						}
					}
			
					SendChatMessage(MessageType.PRIVMSG, channel, String.Format("2Bekapcsolva: {0}", be));
					SendChatMessage(MessageType.PRIVMSG, channel, String.Format("2Kikapcsolva: {0}", ki));
				}
				else if(info[4] == "all")
				{
					if(info.Length < 6)
						return;

					if(info[5] == "info")
					{
						string be = "";
						string ki = "";

						var db = SchumixBot.mSQLConn.QueryRow(String.Format("SELECT funkcio_nev, funkcio_status FROM schumix"));
						for(int i = 0; i < db.Rows.Count; ++i)
						{
							var row = db.Rows[i];

							string nev = row["funkcio_nev"].ToString();
							string status = row["funkcio_status"].ToString();

							if(status == "be")
								be += nev + " ";
							else
								ki += nev + " ";
						}

						SendChatMessage(MessageType.PRIVMSG, channel, String.Format("2Bekapcsolva: {0}", be));
						SendChatMessage(MessageType.PRIVMSG, channel, String.Format("2Kikapcsolva: {0}", ki));
					}
					else
					{
						if(info.Length < 7)
							return;

						if(info[5] == "be" || info[5] == "ki")
						{
							SendChatMessage(MessageType.PRIVMSG, channel, String.Format("{0}: {1}kapcsolva", info[6], info[5]));
							SchumixBot.mSQLConn.QueryFirstRow(String.Format("UPDATE schumix SET funkcio_status = '{0}' WHERE funkcio_nev = '{1}'", info[5], info[6]));
						}
					}
				}
				else if(info[4] == "channel")
				{
					if(info.Length < 7)
						return;
			
					string channelinfo = info[5];
					string status = info[6];
			
					if(info[6] == "info")
					{
						string be = "";
						string ki = "";

						for(int i = 0; i < m_ChannelFunkcio.Count; i++)
						{
							string szobak = m_ChannelFunkcio[i];
							string[] pont = szobak.Split('.');
							string szoba = pont[0];
							string funkciok = pont[1];

							string[] kettospont = funkciok.Split(':');

							if(szoba == channelinfo)
							{
								if(kettospont[1] == "be")
									be += kettospont[0] + " ";
								else
									ki += kettospont[0] + " ";
							}
						}

						SendChatMessage(MessageType.PRIVMSG, channel, String.Format("2Bekapcsolva: {0}", be));
						SendChatMessage(MessageType.PRIVMSG, channel, String.Format("2Kikapcsolva: {0}", ki));
					}
					else if(status == "be" || status == "ki")
					{
						if(info.Length < 8)
							return;

						SendChatMessage(MessageType.PRIVMSG, channel, String.Format("{0}: {1}kapcsolva", info[7], status));
						SchumixBot.mSQLConn.QueryFirstRow(String.Format("UPDATE channel SET funkciok = '{0}' WHERE szoba = '{1}'", ChannelFunkciok(info[7], status, channelinfo), channelinfo));
						ChannelFunkcioReload();
					}
				}
				else if(info[4] == "update")
				{
					var db2 = SchumixBot.mSQLConn.QueryRow(String.Format("SELECT szoba FROM channel"));
					for(int i = 0; i < db2.Rows.Count; ++i)
					{
						var row = db2.Rows[i];
						string szoba = row["szoba"].ToString();
						SchumixBot.mSQLConn.QueryFirstRow(String.Format("UPDATE channel SET funkciok = ',koszones:be,log:be,rejoin:be,parancsok:be' WHERE szoba = '{0}'", szoba));
					}
			
					ChannelFunkcioReload();
				}
				else
				{
					if(info.Length < 6)
						return;

					string status = info[4];

					if(status == "be" || status == "ki")
					{
						SendChatMessage(MessageType.PRIVMSG, channel, String.Format("{0}: {1}kapcsolva", info[5], status));
						SchumixBot.mSQLConn.QueryFirstRow(String.Format("UPDATE channel SET funkciok = '{0}' WHERE szoba = '{1}'", ChannelFunkciok(info[5], status, channel), channel));
						ChannelFunkcioReload();
					}
				}
			}

			if(info[3] == "channel")
			{
				if(!Admin(source_nick, source_host))
					return;

				if(info.Length < 5)
					return;

				if(info[4] == "help")
				{
					SendChatMessage(MessageType.PRIVMSG, channel, "Alparancsok használata:");
					SendChatMessage(MessageType.PRIVMSG, channel, String.Format("Hozzáadás: {0}channel add <channel> <ha van pass akkor az>", SchumixBot._parancselojel));
					SendChatMessage(MessageType.PRIVMSG, channel, String.Format("Eltávolítás: {0}channel del <channel>", SchumixBot._parancselojel));
					SendChatMessage(MessageType.PRIVMSG, channel, String.Format("Frissítés: {0}channel update", SchumixBot._parancselojel));
				}
				else if(info[4] == "add")
				{
					if(info.Length < 6)
						return;

					string szobainfo = info[5];
			
					if(info.Length == 7)
					{
						m_ChannelPrivmsg = channel;
						string jelszo = info[6];
						Network.writer.WriteLine(String.Format("JOIN {0} {1}", szobainfo, jelszo));
						SchumixBot.mSQLConn.QueryFirstRow(String.Format("INSERT INTO `channel`(szoba, jelszo) VALUES ('{0}', '{1}')", szobainfo, jelszo));
						SchumixBot.mSQLConn.QueryFirstRow(String.Format("UPDATE channel SET aktivitas = 'aktiv' WHERE szoba = '{0}'", szobainfo));
					}
					else
					{
						m_ChannelPrivmsg = channel;
						Network.writer.WriteLine(String.Format("JOIN {0}", szobainfo));
						SchumixBot.mSQLConn.QueryFirstRow(String.Format("INSERT INTO `channel`(szoba, jelszo) VALUES ('{0}', '')", szobainfo));
						SchumixBot.mSQLConn.QueryFirstRow(String.Format("UPDATE channel SET aktivitas = 'aktiv' WHERE szoba = '{0}'", szobainfo));
					}

					SendChatMessage(MessageType.PRIVMSG, channel, String.Format("Channel hozzáadva: {0}", szobainfo));

					ChannelListaReload();
					ChannelFunkcioReload();
				}
				else if(info[4] == "del")
				{
					if(info.Length < 6)
						return;

					string szobainfo = info[5];
					Network.writer.WriteLine(String.Format("PART {0}", szobainfo));
					SchumixBot.mSQLConn.QueryFirstRow(String.Format("DELETE FROM `channel` WHERE szoba = '{0}'", szobainfo));
					SendChatMessage(MessageType.PRIVMSG, channel, String.Format("Channel eltávolítva: {0}", szobainfo));

					ChannelListaReload();
					ChannelFunkcioReload();
				}
				else if(info[4] == "update")
				{
					ChannelListaReload();
					ChannelFunkcioReload();
				}
				else if(info[4] == "info")
				{
					string Aktivszobak = "";
					string DeAktivszobak = "";
					int adatszoba = 0;
					int adatszoba1 = 0;

					var db = SchumixBot.mSQLConn.QueryRow(String.Format("SELECT szoba, aktivitas, error FROM channel"));
					for(int i = 0; i < db.Rows.Count; ++i)
					{
						var row = db.Rows[i];
						string szoba = row["szoba"].ToString();
						string aktivitas = row["aktivitas"].ToString();
						string error = row["error"].ToString();

						if(aktivitas == "aktiv")
						{
							Aktivszobak += ", " + szoba;
							adatszoba += 1;
						}
						else if(aktivitas == "nem aktiv")
						{
							DeAktivszobak += ", " + szoba + ":" + error;
							adatszoba1 += 1;
						}
					}

					if(adatszoba != 0)
					{
						if(Aktivszobak.Substring(0, 2) == ", ")
							Aktivszobak = Aktivszobak.Remove(0, 2);

						SendChatMessage(MessageType.PRIVMSG, channel, String.Format("3Aktiv: {0}", Aktivszobak));
					}
					else
						SendChatMessage(MessageType.PRIVMSG, channel, "3Aktiv: Nincs adat.");

					if(adatszoba1 != 0)
					{
						if(DeAktivszobak.Substring(0, 2) == ", ")
							DeAktivszobak = DeAktivszobak.Remove(0, 2);

						SendChatMessage(MessageType.PRIVMSG, channel, String.Format("3Deaktiv: {0}", DeAktivszobak));
					}
					else
						SendChatMessage(MessageType.PRIVMSG, channel, "3Deaktiv: Nincs adat.");
				}
			}

			if(info[3] == "irc")
			{
				if(info.Length < 5)
					return;

				if(info[4] == "help")
				{
					SendChatMessage(MessageType.PRIVMSG, channel, "Segítség az irc-hez!");
					SendChatMessage(MessageType.PRIVMSG, channel, String.Format("Parancsok: {0}irc rang | {1}irc rang1 | {2}irc nick | {3}irc kick | {4}irc owner", SchumixBot._parancselojel, SchumixBot._parancselojel, SchumixBot._parancselojel, SchumixBot._parancselojel, SchumixBot._parancselojel));
				}
				else
				{
					var db = SchumixBot.mSQLConn.QueryFirstRow(String.Format("SELECT hasznalata FROM irc_parancsok WHERE parancs = '{0}'", info[4]));
					if(db != null)
					{
						string hasznalata = db["hasznalata"].ToString();
						SendChatMessage(MessageType.PRIVMSG, channel, hasznalata);
					}
				}
			}

			if(info[3] == "sznap")
			{
				if(!Admin(source_nick, source_host))
					return;

				if(info.Length < 5)
					return;

				var db = SchumixBot.mSQLConn.QueryFirstRow(String.Format("SELECT nev, honap, nap FROM sznap WHERE nev = '{0}'", info[4]));
				if(db != null)
				{
					string nev = db["nev"].ToString();
					string honap = db["honap"].ToString();
					int nap = Convert.ToInt32(db["nap"]);

					SendChatMessage(MessageType.PRIVMSG, channel, String.Format("{0} születés napja: {1} {2}", nev, honap, nap));
				}
				else
					SendChatMessage(MessageType.PRIVMSG, channel, "Nincs ilyen ember.");
			}

			if(info[3] == "whois")
			{
				if(info.Length < 5)
					return;

				m_WhoisPrivmsg = channel;
				Network.writer.WriteLine("WHOIS {0}", info[4]);
			}

			if(info[3] == "uzenet")
			{
				if(info.Length < 5)
					return;

				if(info[4] == "help")
				{
					SendChatMessage(MessageType.PRIVMSG, channel, "Segítség az üzenethez!");
					SendChatMessage(MessageType.PRIVMSG, channel, String.Format("Funkció használata: {0}üzenet <ide jön a személy> <ha nem felhivás küldenél hanem saját üzenetet>", SchumixBot._parancselojel));
				}
				else
				{
					if(info.Length == 5)
						SendChatMessage(MessageType.PRIVMSG, info[4], String.Format("Keresnek téged itt: {0}", channel));
					else if(info.Length >= 6)
					{
						string alomany = "";
						for(int i = 5; i < info.Length; i++)
							alomany += info[i] + " ";

						if(alomany.Substring(0, 1) == ":")
							alomany = alomany.Remove(0, 1);

						SendChatMessage(MessageType.PRIVMSG, info[4], String.Format("{0}", alomany));
					}
				}
			}

			if(info[3] == "keres")
			{
				if(info.Length < 5)
					return;

				if(info[4] == "help")
				{
					SendChatMessage(MessageType.PRIVMSG, channel, "Segítség a kereséshez!");
					SendChatMessage(MessageType.PRIVMSG, channel, String.Format("Funkció használata: {0}keres <ide jön a kereset szöveg>", SchumixBot._parancselojel));
				}
				else
				{
					string adat = "";
					for(int i = 4; i < info.Length; i++)
						adat += "%20" + info[i];

					if(adat.Substring(0, 3) == "%20")
						adat = adat.Remove(0, 3);

					string url = GetUrl("http://ajax.googleapis.com/ajax/services/search/web?v=1.0&start=0&rsz=small&q=" + adat);

					var Regex1 = new Regex(@".titleNoFormatting.:.(?<title>\S+).,.content.:.");
					if(!Regex1.IsMatch(url))
						SendChatMessage(MessageType.PRIVMSG, channel, "2Title: Nincs Title.");
					else
						SendChatMessage(MessageType.PRIVMSG, channel, String.Format("2Title: {0}", Regex1.Match(url).Groups["title"].ToString()));

					var Regex = new Regex(@".unescapedUrl.:.(?<url>\S+).,.url.");
					if(!Regex.IsMatch(url))
						SendChatMessage(MessageType.PRIVMSG, channel, "2Link: Nincs Link.");
					else
						SendChatMessage(MessageType.PRIVMSG, channel, String.Format("2Link: 9{0}", Regex.Match(url).Groups["url"].ToString()));
				}
			}

			if(info[3] == "szoba")
			{
				if(info.Length < 5)
					return;

				if(info[4] == "help")
				{
					SendChatMessage(MessageType.PRIVMSG, channel, "Segitség a konzol szoba váltásához!");
					SendChatMessage(MessageType.PRIVMSG, channel, String.Format("Funkció használata: {0}szoba <ide jön a szoba>", SchumixBot._parancselojel));
				}
				else
					SchumixBot.mSQLConn.QueryFirstRow(String.Format("UPDATE schumix SET irc_cim = '{0}' WHERE entry = '1'", info[4]));
			}

			if(info[3] == "calc")
			{
				string adat = "";
				for(int i = 4; i < info.Length; i++)
					adat += " " + info[i];

				if(adat.Substring(0, 1) == " ")
					adat = adat.Remove(0, 1);

				double ris = Eval.Calculate(adat);
				SendChatMessage(MessageType.PRIVMSG, channel, String.Format("{0}", ris));
			}

			if(info[3] == "kikapcs")
			{
				if(!Admin(source_nick, source_host))
					return;

				SendChatMessage(MessageType.PRIVMSG, channel, "Viszlát :(");
				Network.writer.WriteLine("QUIT :{0} leallitott parancsal.", source_nick);
				Thread.Sleep(1000);
				Environment.Exit(1);
			}
		}

        /// <summary>
        ///     Ha a szobában a köszönés funkció be van kapcsolva,
        ///     akkor köszön az éppen belépőnek.
        /// </summary>
        /// <param name="info">Egyszerű adat, ami az IRC szerver felől jön.</param>
		public void OpcodeJoin(string[] info)
		{
			if(source_nick == SchumixBot.NickTarolo)
				return;

			if(info[2].Substring(0, 1) == ":")
				info[2] = info[2].Remove(0, 1);

			if(FSelect("koszones") != "be" && FSelect("koszones", info[2]) != "be")
				return;

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
				SendChatMessage(MessageType.PRIVMSG, info[2], String.Format("Jó reggelt {0}", source_nick));
			else if(DateTime.Now.Hour >= 20)
				SendChatMessage(MessageType.PRIVMSG, info[2], String.Format("Jó estét {0}", source_nick));
			else
				if(Admin(source_nick) == true)
					SendChatMessage(MessageType.PRIVMSG, info[2], "Üdv főnök");
				else
					SendChatMessage(MessageType.PRIVMSG, info[2], String.Format("{0} {1}", Koszones, source_nick));
		}

        /// <summary>
        ///     Ha ez a funkció be van kapcsolva, akkor
        ///     miután a nick elhagyta a szobát elköszön tőle.
        /// </summary>
        /// <param name="info">Egyszerű adat, ami az IRC szerver felől jön.</param>
		public void OpcodeLeft(string[] info)
		{
			if(source_nick == SchumixBot.NickTarolo)
				return;

			if(FSelect("koszones") != "be" && FSelect("koszones", info[2]) != "be")
				return;

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
				SendChatMessage(MessageType.PRIVMSG, info[2], String.Format("Jóét {0}", source_nick));
			else
				SendChatMessage(MessageType.PRIVMSG, info[2], String.Format("{0} {1}", elkoszones, source_nick));
		}

        /// <summary>
        ///     Ha a ConsoleLog be van kapcsolva, akkor
        ///     kiírja a console-ra az IRC szerverről fogadott információkat.
        /// </summary>
        /// <param name="info">Egyszerű adat, ami az IRC szerver felől jön.</param>
		public void OpcodeNotice(string[] info)
		{
			if(Consol.ConsolLog == "be")
			{
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
        ///     Ha a ConsoleLog be van kapcsolva, akkor 
        ///     kiírja a console-ra az IRC szerverről fogadott információkat.
        /// </summary>
        /// <param name="info">Egyszerű adat, ami az IRC szerver felől jön.</param>
		public void OpcodeConsol(string[] info)
		{
			if(Consol.ConsolLog == "be")
			{
				string ServerMessage = "";
				for(int i = 1; i < info.Length; i++)
					ServerMessage += info[i] + " ";

				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write("[SERVER] ");
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.Write(ServerMessage + "\n");
				Console.ForegroundColor = ConsoleColor.Gray;
			}
		}

        /// <summary>
        ///     Válaszol, ha valaki pingeli a botot.
        /// </summary>
        /// <param name="info">Egyszerű adat, ami az IRC szerver felől jön.</param>
		public void OpcodePing(string[] info)
		{
			if(args.Substring(0, 1) == ":")
				args = args.Remove(0, 1);

			Network.writer.WriteLine("PING :{0}", args);
		}

        /// <summary>
        ///     Válaszol, ha valaki pongolja a botot.
        /// </summary>
        /// <param name="info">Egyszerű adat, ami az IRC szerver felől jön.</param>
		public void OpcodePong(string[] info)
		{
			if(args.Substring(0, 1) == ":")
				args = args.Remove(0, 1);

			Network.writer.WriteLine("PONG :{0}", args);
			Network.Status = true;
		}

        /// <summary>
        ///     Ha ismeretlen parancs jön, akkor kiírja.
        /// </summary>
        /// <param name="info">Egyszerű adat, ami az IRC szerver felől jön.</param>
		public void OpcodeIsmeretlenParancs(string[] info)
		{
			if(Consol.ConsolLog == "be")
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
		public void OpcodeNickError(string[] info)
		{
			if(SchumixBot.NickTarolo == SchumixBot.Nick)
			{
				SchumixBot.NickTarolo = SchumixBot.Nick2;
				Network.m_ConnState = (int)Network.ConnState.CONN_CONNECTED;
				return;
			}
			else if(SchumixBot.NickTarolo == SchumixBot.Nick2)
			{
				SchumixBot.NickTarolo = SchumixBot.Nick3;
				Network.m_ConnState = (int)Network.ConnState.CONN_CONNECTED;
				return;
			}
			else if(SchumixBot.NickTarolo == SchumixBot.Nick3)
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
				SchumixBot.NickTarolo = SchumixBot.Nick;
				Network.m_ConnState = (int)Network.ConnState.CONN_CONNECTED;
				return;
			}
		}

        /// <summary>
		///     Ha bannolva van egy szobából, akkor feljegyzi.
        /// </summary>
        /// <param name="info">Egyszerű adat, ami az IRC szerver felől jön.</param>
		public void OpcodeChannelBan(string[] info)
		{
			if(info.Length < 4)
				return;

			SchumixBot.mSQLConn.QueryFirstRow(String.Format("UPDATE channel SET aktivitas = 'nem aktiv', error = 'channel ban' WHERE szoba = '{0}'", info[3]));
			SendChatMessage(MessageType.PRIVMSG, m_ChannelPrivmsg, String.Format("{0}: channel ban", info[3]));
			m_ChannelPrivmsg = SchumixBot.NickTarolo;
		}

        /// <summary>
        ///     Ha hibás egy IRC szobának a jelszava, akkor feljegyzi.
        /// </summary>
        /// <param name="info">Egyszerű adat, ami az IRC szerver felől jön.</param>
		public void OpcodeNoChannelJelszo(string[] info)
		{
			if(info.Length < 4)
				return;

			SchumixBot.mSQLConn.QueryFirstRow(String.Format("UPDATE channel SET aktivitas = 'nem aktiv', error = 'hibas channel jelszo' WHERE szoba = '{0}'", info[3]));
			SendChatMessage(MessageType.PRIVMSG, m_ChannelPrivmsg, String.Format("{0}: hibás channel jelszó", info[3]));
			m_ChannelPrivmsg = SchumixBot.NickTarolo;
		}

        /// <summary>
        ///     Kigyűjti éppen hol van fent a nick.
        /// </summary>
        /// <param name="info">Egyszerű adat, ami az IRC szerver felől jön.</param>
		public void OpcodeWhois(string[] info)
		{
			if(info.Length < 5)
				return;

			string alomany = "";
			for(int i = 4; i < info.Length; i++)
				alomany += info[i] + " ";

			if(alomany.Substring(0, 1) == ":")
				alomany = alomany.Remove(0, 1);

			SendChatMessage(MessageType.PRIVMSG, m_WhoisPrivmsg, String.Format("Jelenleg itt van fent: {0}", alomany));
			m_WhoisPrivmsg = SchumixBot.NickTarolo;
		}

        /// <summary>
        ///     Ha engedélyezett a ConsolLog, akkor kiírja a Console-ra ha kickelnek valakit.
        /// </summary>
        /// <param name="info">Egyszerű adat, ami az IRC szerver felől jön.</param>
		public void OpcodeKick(string[] info)
		{
			if(info.Length < 5)
				return;

			if(FSelect("parancsok") != "be" && FSelect("parancsok", info[2]) != "be")
				return;

			if(info[3] == SchumixBot.NickTarolo)
			{
				if(FSelect("rejoin") != "be" && FSelect("rejoin", info[2]) != "be")
					return;

				foreach(var m_channel in SchumixBot.m_ChannelLista)
				{
					if(info[2] == m_channel.Key)
						Network.writer.WriteLine("JOIN " + m_channel.Key + m_channel.Value);
				}
			}
			else
			{
				if(Consol.ConsolLog == "be")
				{
					string alomany = "";
					for(int i = 4; i < info.Length; i++)
						alomany += info[i] + " ";
		
					if(alomany.Substring(0, 1) == ":")
						alomany = alomany.Remove(0, 1);
		
					Console.WriteLine("{0} kickelte a következő felhasználot: {1} oka: {2}", source_nick, info[3], alomany);
				}
			}
		}

        /// <summary>
        ///     Ez küldi el az üzenetet az chatre.
        /// </summary>
        /// <param name="tipus">
        ///     PRIVMSG : Sima üzenet
		///     NOTICE  : Notice üzenet
        /// </param>
        /// <param name="channel">IRC szoba neve</param>
        /// <param name="uzenet">Maga az üzenet</param>
		public void SendChatMessage(MessageType tipus, string channel, string uzenet)
		{
			string buffer = "";

			if(tipus == MessageType.PRIVMSG)
				buffer = "PRIVMSG";
			else if(tipus == MessageType.NOTICE)
				buffer = "NOTICE";

			buffer += " " + channel + " :" + uzenet;

			Network.writer.WriteLine(buffer);
		}

        /// <summary>
        ///     Meghatározza, hogy a nick admin-e vagy sem, nick alapján.
        /// </summary>
		private bool Admin(string nick)
		{
			string admin = "";
			string _nick = nick.ToLower();

			var db = SchumixBot.mSQLConn.QueryFirstRow(String.Format("SELECT nev FROM adminok WHERE nev = '{0}'", _nick));
			if(db != null)
				admin = db["nev"].ToString();

			if(_nick != admin)
				return false;

			return true;
		}

        /// <summary>
        ///     Meghatározza, hogy a nick admin-e vagy sem, nick és host alapján.
        /// </summary>
        /// <param name="host">A nick IP címe.</param>
		private bool Admin(string nick, string host)
		{
			string admin = "";
			string ip = "";
			string _nick = nick.ToLower();

			var db = SchumixBot.mSQLConn.QueryFirstRow(String.Format("SELECT nev, ip FROM adminok WHERE nev = '{0}'", _nick));
			if(db != null)
			{
				admin = db["nev"].ToString();
				ip = db["ip"].ToString();
			}

			if(_nick != admin)
				return false;

			if(host != ip)
				return false;

			return true;
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
				SendChatMessage(MessageType.PRIVMSG, info[2], String.Format("Helyi idő: {0}:0{1}", DateTime.Now.Hour, DateTime.Now.Minute));
			else
				SendChatMessage(MessageType.PRIVMSG, info[2], String.Format("Helyi idő: {0}:{1}", DateTime.Now.Hour, DateTime.Now.Minute));
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
							SendChatMessage(MessageType.PRIVMSG, info[2], String.Format("Ma {0}. 0{1}. 0{2}. {3} napja van.", Ev, Honap, Nap, napdb));
						else
							SendChatMessage(MessageType.PRIVMSG, info[2], String.Format("Ma {0}. 0{1}. {2}. {3} napja van.", Ev, Honap, Nap, napdb));
					}
					else
					{
						if(Nap < 10)
							SendChatMessage(MessageType.PRIVMSG, info[2], String.Format("Ma {0}. {1}. 0{2}. {3} napja van.", Ev, Honap, Nap, napdb));
						else
							SendChatMessage(MessageType.PRIVMSG, info[2], String.Format("Ma {0}. {1}. {2}. {3} napja van.", Ev, Honap, Nap, napdb));
					}
				}
		 	}
		}

        /// <summary>
        ///     
        /// </summary>
        /// <param name="info"></param>
		private void Schumix(string[] info)
		{
			string ParancsJel = SchumixBot.Nick.ToLower() + ",";

			if(info[3] == ParancsJel)
			{
				string channel = info[2];
				userdata = hostmask.Split('!');
				hostdata = userdata[1].Split('@');
				//source_user = hostdata[0];
				source_host = hostdata[1];

				if(FSelect("parancsok") != "be" && FSelect("parancsok", channel) != "be")
					return;

				if(info.Length >= 5 && info[4] == "info")
				{
					string Platform = "";
					var pid = Environment.OSVersion.Platform;

					switch(pid) 
					{
						case PlatformID.Win32NT:
						case PlatformID.Win32S:
						case PlatformID.Win32Windows:
						case PlatformID.WinCE:
							Platform = "Windows";
							break;
						case PlatformID.Unix:
							Platform = "Linux";
							break;
						default:
							Platform = "Ismeretlen";
							break;
					}

					SendChatMessage(MessageType.PRIVMSG, channel, String.Format("3Verzió: 10{0}", SchumixBot.revision));
					SendChatMessage(MessageType.PRIVMSG, channel, String.Format("3Platform: {0}", Platform));
					SendChatMessage(MessageType.PRIVMSG, channel, String.Format("3Programnyelv: c#"));
					SendChatMessage(MessageType.PRIVMSG, channel, String.Format("3Uptime: {0}", SchumixBot.Uptime()));
				}
				else if(info.Length >= 5 && info[4] == "help")
				{
					SendChatMessage(MessageType.PRIVMSG, channel, "3Parancsok: info | ghost | nick | sys");
				}
				else if(info.Length >= 5 && info[4] == "ghost")
				{
					if(!Admin(source_nick, source_host))
						return;

					SendChatMessage(MessageType.PRIVMSG, "NickServ", String.Format("ghost {0} {1}", SchumixBot.Nick, SchumixBot.Jelszo));
					Network.writer.WriteLine("NICK {0}", SchumixBot.Nick);
					SchumixBot.NickTarolo = SchumixBot.Nick;
				}
				else if(info.Length >= 5 && info[4] == "nick")
				{
					if(!Admin(source_nick, source_host))
						return;

					if(info.Length < 6)
						return;

					if(info[5] == "identify")
					{
						SchumixBot.NickTarolo = SchumixBot.Nick;
						Network.writer.WriteLine("NICK {0}", SchumixBot.Nick);
						Log.Notice("NickServ", "Sending NickServ identification.");
						SendChatMessage(MessageType.PRIVMSG, "NickServ", String.Format("identify {0}", SchumixBot.Jelszo));
					}
					else
					{
						string nick = info[5];
						SchumixBot.NickTarolo = nick;
						Network.writer.WriteLine("NICK {0}", nick);
					}
				}
				else if(info.Length >= 5 && info[4] == "sys")
				{
					var memory = Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024;

					SendChatMessage(MessageType.PRIVMSG, channel, String.Format("3Memoria használat: {0} MB", memory));
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
			if(FSelect("log") != "be" && FSelect("log", channel) != "be")
				return;

			string logfile_name = channel + ".log";
			if(!File.Exists(String.Format("./szoba/{0}", logfile_name)))
				File.Create(String.Format("./szoba/{0}", logfile_name));

			try
			{
				TextWriter writeFile = new StreamWriter(String.Format("./szoba/{0}", logfile_name), true);
				writeFile.WriteLine("[{0}] <{1}> {2}", DateTime.Now, user, args);
				writeFile.Flush();
				writeFile.Close();
			}
			catch { }
		}

        /// <summary>
        ///     
        /// </summary>
		private void ChannelFunkcioReload()
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
					string[] vesszo = new string[funkciok.Split(',').Length];
					vesszo = funkciok.Split(',');

					for(int x = 1; x < vesszo.Length; x++)
					{
						string szobaadat = szoba + "." + vesszo[x];
						m_ChannelFunkcio.Add(szobaadat);
					}
				}
			}
		}

        /// <summary>
        ///     
        /// </summary>
		private void ChannelListaReload()
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

        /// <summary>
        ///     
        /// </summary>
        /// <param name="nev"></param>
        /// <param name="status"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
		private string ChannelFunkciok(string nev, string status, string channel)
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
