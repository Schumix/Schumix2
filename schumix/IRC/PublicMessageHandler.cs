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
	public partial class MessageHandler
	{
		public static Commands.CommandManager CManager = new Commands.CommandManager();

		public void HandlePrivmsg()
		{
			if(Consol.ConsoleLog == "be")
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("[{0}] <{1}> {2}", Network.IMessage.Channel, Network.IMessage.Nick, Network.IMessage.Args);
				Console.ForegroundColor = ConsoleColor.Gray;
			}

			LogToFajl(Network.IMessage.Channel, Network.IMessage.Nick, Network.IMessage.Args);

			if(FSelect("parancsok") == "be")
			{
				if(FSelect("parancsok", Network.IMessage.Channel) != "be" && Network.IMessage.Channel.Substring(0, 1) != "#")
					return;

				if(Network.IMessage.Info[3].Substring(0, 1) == ":")
					Network.IMessage.Info[3] = Network.IMessage.Info[3].Remove(0, 1);

				Schumix();

				if(Network.IMessage.Info[Network.IMessage.Info.Length-2] == "" || Network.IMessage.Info[Network.IMessage.Info.Length-1] == "")
					return;

				if(Network.IMessage.Info[3].Substring(0, 1) == " " || Network.IMessage.Info[3].Substring(0, 1) != IRCConfig.Parancselojel)
					return;

				Network.IMessage.Info[3] = Network.IMessage.Info[3].Remove(0, 1);
				CManager.BejovoInfo(Network.IMessage.Info[3].ToLower());
			}

			/*if(info[3] == "admin")
			{
				if(!CManager.Admin(Network.IMessage.Nick, Network.IMessage.Host))
					return;

				if(info.Length >= 5 && info[4] == "help")
				{
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Alparancsok használata:");
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("Admin lista: {0}admin lista", IRCConfig.Parancselojel));
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("Hozzáadás: {0}admin add <admin neve>", IRCConfig.Parancselojel));
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("Eltávolítás: {0}admin del <admin neve>", IRCConfig.Parancselojel));
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

					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("2Adminok: {0}", adminok));
				}
				else if(info.Length >= 5 && info[4] == "add")
				{
					if(info.Length < 6)
						return;

					string nev = info[5];
					string pass = GetRandomString();

					SchumixBot.mSQLConn.QueryFirstRow(String.Format("INSERT INTO `adminok`(nev, jelszo) VALUES ('{0}', '{1}')", nev.ToLower(), pass));

					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("Admin hozzáadva: {0}", nev));
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, nev, String.Format("Mostantól Schumix adminja vagy. A te mostani jelszavad: {0}", pass));
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, nev, String.Format("Ha megszeretnéd változtatni használd az {0}ujjelszo parancsot. Használata: {1}ujjelszo <régi> <új>", IRCConfig.Parancselojel, IRCConfig.Parancselojel));
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, nev, String.Format("Admin nick élesítése: {0}hozzaferes <jelszó>", IRCConfig.Parancselojel));
				}
				else if(info.Length >= 5 && info[4] == "del")
				{
					if(info.Length < 6)
						return;

					string nev = info[5];
					SchumixBot.mSQLConn.QueryFirstRow(String.Format("DELETE FROM `adminok` WHERE nev = '{0}'", nev.ToLower()));
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("Admin törölve: {0}", nev));
				}
				else
				{
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("3Parancsok: {0}nick | {0}join | {0}left | {0}kick | {0}mode", IRCConfig.Parancselojel));
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("3Parancsok: {0}szinek | {0}funkcio | {0}kikapcs | {0}sznap | {0}szoba | {0}channel | {0}hozzaferes | {0}ujjelszo", IRCConfig.Parancselojel));
					return;
				}
			}

			if(info[3] == "teszt")
			{
				if(!CManager.Admin(Network.IMessage.Nick, Network.IMessage.Host))
					return;

				if(info.Length >= 5 && info[4] == "adat")
				{
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Teszt probálkozás");
				}
				else if(info.Length >= 5 && info[4] == "db")
				{
					var db = SchumixBot.mSQLConn.QueryRow(String.Format("SELECT nev FROM adminok"));
					for(int i = 0; i < db.Rows.Count; ++i)
					{
						var row = db.Rows[i];

						string admin = row["nev"].ToString();
						sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("{0}", admin));
					}
				}
				else
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("{0}", info.Length));
			}

			if(info[3] == "szinek")
			{
				if(!CManager.Admin(Network.IMessage.Nick, Network.IMessage.Host))
					return;

				sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "1teszt1 2teszt2 3teszt3 4teszt4 5teszt5 6teszt6 7teszt7 8teszt8");
				sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "9teszt9 10teszt10 11teszt11 12teszt12 13teszt13 14teszt14 15teszt15");
				return;
			}

			if(info[3] == "nick")
			{
				if(!CManager.Admin(Network.IMessage.Nick, Network.IMessage.Host))
					return;

				if(info.Length < 5)
					return;

				string nick = info[4];
				SchumixBot.NickTarolo = nick;
				Network.writer.WriteLine(String.Format("NICK {0}", nick));
			}

			if(info[3] == "join")
			{
				if(!CManager.Admin(Network.IMessage.Nick, Network.IMessage.Host))
					return;

				m_ChannelPrivmsg = Network.IMessage.Channel;

				if(info.Length == 5)
					Network.writer.WriteLine(String.Format("JOIN {0}", info[4]));
				else if(info.Length == 6)
					Network.writer.WriteLine(String.Format("JOIN {0} {1}", info[4], info[5]));
			}

			if(info[3] == "left")
			{
				if(!CManager.Admin(Network.IMessage.Nick, Network.IMessage.Host))
					return;

				if(info.Length < 5)
					return;

				Network.writer.WriteLine(String.Format("PART {0}", info[4]));
			}

			if(info[3] == "kick")
			{
				if(!CManager.Admin(Network.IMessage.Nick, Network.IMessage.Host))
					return;

				if(info.Length < 5)
					return;

				string kick = info[4].ToLower();
				int szam = info.Length;

				if(szam == 5)
				{
					if(kick != SchumixBot.NickTarolo.ToLower())
						Network.writer.WriteLine(String.Format("KICK {0} {1}", Network.IMessage.Channel, kick));
				}
				else if(szam >= 6)
				{
					string oka = "";
					for(int i = 5; i < info.Length; i++)
						oka += info[i] + " ";

					if(oka.Substring(0, 1) == ",")
						oka = oka.Remove(0, 1);

					if(kick != SchumixBot.NickTarolo)
						Network.writer.WriteLine(String.Format("KICK {0} {1} :{2}", Network.IMessage.Channel, kick, oka));
				}
			}

			if(info[3] == "mode")
			{
				if(!CManager.Admin(Network.IMessage.Nick, Network.IMessage.Host))
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
					Network.writer.WriteLine(String.Format("MODE {0} {1} {2}", Network.IMessage.Channel, rang, nev));
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

				sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, Sha1.ToLower());
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

				sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, Md5.ToLower());
			}

			if(info[3] == "funkcio")
			{
				if(!CManager.Admin(Network.IMessage.Nick, Network.IMessage.Host))
					return;

				if(info.Length < 5)
					return;

				if(info[4] == "help")
				{
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Alparancsok használata:");
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("Channel kezelés: {0}funkcio <be vagy ki> <funkcio név>", IRCConfig.Parancselojel));
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("Channel kezelés máshonnét: {0}funkcio channel <channel név> <be vagy ki> <funkcio név>", IRCConfig.Parancselojel));
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("Együtes kezelés: {0}funkcio all <be vagy ki> <funkcio név>", IRCConfig.Parancselojel));
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("Update kezelése: {0}funkcio update", IRCConfig.Parancselojel));
				}
				else if(info[4] == "info")
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

						if(szoba == Network.IMessage.Channel)
						{
							if(kettospont[1] == "be")
								be += kettospont[0] + " ";
							else
								ki += kettospont[0] + " ";
						}
					}
			
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("2Bekapcsolva: {0}", be));
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("2Kikapcsolva: {0}", ki));
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

						sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("2Bekapcsolva: {0}", be));
						sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("2Kikapcsolva: {0}", ki));
					}
					else
					{
						if(info.Length < 7)
							return;

						if(info[5] == "be" || info[5] == "ki")
						{
							sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("{0}: {1}kapcsolva", info[6], info[5]));
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

						sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("2Bekapcsolva: {0}", be));
						sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("2Kikapcsolva: {0}", ki));
					}
					else if(status == "be" || status == "ki")
					{
						if(info.Length < 8)
							return;

						sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("{0}: {1}kapcsolva", info[7], status));
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
						sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("{0}: {1}kapcsolva", info[5], status));
						SchumixBot.mSQLConn.QueryFirstRow(String.Format("UPDATE channel SET funkciok = '{0}' WHERE szoba = '{1}'", ChannelFunkciok(info[5], status, Network.IMessage.Channel), Network.IMessage.Channel));
						ChannelFunkcioReload();
					}
				}
			}

			if(info[3] == "channel")
			{
				if(!CManager.Admin(Network.IMessage.Nick, Network.IMessage.Host))
					return;

				if(info.Length < 5)
					return;

				if(info[4] == "help")
				{
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Alparancsok használata:");
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("Hozzáadás: {0}channel add <channel> <ha van pass akkor az>", IRCConfig.Parancselojel));
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("Eltávolítás: {0}channel del <channel>", IRCConfig.Parancselojel));
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("Frissítés: {0}channel update", IRCConfig.Parancselojel));
				}
				else if(info[4] == "add")
				{
					if(info.Length < 6)
						return;

					string szobainfo = info[5];
			
					if(info.Length == 7)
					{
						m_ChannelPrivmsg = Network.IMessage.Channel;
						string jelszo = info[6];
						Network.writer.WriteLine(String.Format("JOIN {0} {1}", szobainfo, jelszo));
						SchumixBot.mSQLConn.QueryFirstRow(String.Format("INSERT INTO `channel`(szoba, jelszo) VALUES ('{0}', '{1}')", szobainfo, jelszo));
						SchumixBot.mSQLConn.QueryFirstRow(String.Format("UPDATE channel SET aktivitas = 'aktiv' WHERE szoba = '{0}'", szobainfo));
					}
					else
					{
						m_ChannelPrivmsg = Network.IMessage.Channel;
						Network.writer.WriteLine(String.Format("JOIN {0}", szobainfo));
						SchumixBot.mSQLConn.QueryFirstRow(String.Format("INSERT INTO `channel`(szoba, jelszo) VALUES ('{0}', '')", szobainfo));
						SchumixBot.mSQLConn.QueryFirstRow(String.Format("UPDATE channel SET aktivitas = 'aktiv' WHERE szoba = '{0}'", szobainfo));
					}

					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("Channel hozzáadva: {0}", szobainfo));

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
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("Channel eltávolítva: {0}", szobainfo));

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

						sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("3Aktiv: {0}", Aktivszobak));
					}
					else
						sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "3Aktiv: Nincs adat.");

					if(adatszoba1 != 0)
					{
						if(DeAktivszobak.Substring(0, 2) == ", ")
							DeAktivszobak = DeAktivszobak.Remove(0, 2);

						sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("3Deaktiv: {0}", DeAktivszobak));
					}
					else
						sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "3Deaktiv: Nincs adat.");
				}
			}

			if(info[3] == "irc")
			{
				if(info.Length < 5)
					return;

				if(info[4] == "help")
				{
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Segítség az irc-hez!");
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("Parancsok: {0}irc rang | {1}irc rang1 | {2}irc nick | {3}irc kick | {4}irc owner", IRCConfig.Parancselojel, IRCConfig.Parancselojel, IRCConfig.Parancselojel, IRCConfig.Parancselojel, IRCConfig.Parancselojel));
				}
				else
				{
					var db = SchumixBot.mSQLConn.QueryFirstRow(String.Format("SELECT hasznalata FROM irc_parancsok WHERE parancs = '{0}'", info[4]));
					if(db != null)
					{
						string hasznalata = db["hasznalata"].ToString();
						sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, hasznalata);
					}
				}
			}

			if(info[3] == "sznap")
			{
				if(!CManager.Admin(Network.IMessage.Nick, Network.IMessage.Host))
					return;

				if(info.Length < 5)
					return;

				var db = SchumixBot.mSQLConn.QueryFirstRow(String.Format("SELECT nev, honap, nap FROM sznap WHERE nev = '{0}'", info[4]));
				if(db != null)
				{
					string nev = db["nev"].ToString();
					string honap = db["honap"].ToString();
					int nap = Convert.ToInt32(db["nap"]);

					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("{0} születés napja: {1} {2}", nev, honap, nap));
				}
				else
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Nincs ilyen ember.");
			}

			if(info[3] == "whois")
			{
				if(info.Length < 5)
					return;

				m_WhoisPrivmsg = Network.IMessage.Channel;
				Network.writer.WriteLine("WHOIS {0}", info[4]);
			}

			if(info[3] == "uzenet")
			{
				if(info.Length < 5)
					return;

				if(info[4] == "help")
				{
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Segítség az üzenethez!");
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("Funkció használata: {0}üzenet <ide jön a személy> <ha nem felhivás küldenél hanem saját üzenetet>", IRCConfig.Parancselojel));
				}
				else
				{
					if(info.Length == 5)
						sSendMessage.SendChatMessage(MessageType.PRIVMSG, info[4], String.Format("Keresnek téged itt: {0}", Network.IMessage.Channel));
					else if(info.Length >= 6)
					{
						string alomany = "";
						for(int i = 5; i < info.Length; i++)
							alomany += info[i] + " ";

						if(alomany.Substring(0, 1) == ":")
							alomany = alomany.Remove(0, 1);

						sSendMessage.SendChatMessage(MessageType.PRIVMSG, info[4], String.Format("{0}", alomany));
					}
				}
			}

			if(info[3] == "keres")
			{
				if(info.Length < 5)
					return;

				if(info[4] == "help")
				{
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "Segítség a kereséshez!");
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("Funkció használata: {0}keres <ide jön a kereset szöveg>", IRCConfig.Parancselojel));
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
						sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "2Title: Nincs Title.");
					else
						sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("2Title: {0}", Regex1.Match(url).Groups["title"].ToString()));

					var Regex = new Regex(@".unescapedUrl.:.(?<url>\S+).,.url.");
					if(!Regex.IsMatch(url))
						sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "2Link: Nincs Link.");
					else
						sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("2Link: 9{0}", Regex.Match(url).Groups["url"].ToString()));
				}
			}*/
		}

		private void Schumix()
		{
			string ParancsJel = IRCConfig.NickName + ",";
			string INick = Network.IMessage.Info[3];

			if(INick.ToLower() == ParancsJel.ToLower())
			{
				if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4] == "info")
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

					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("3Verzió: 10{0}", SchumixBot.revision));
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("3Platform: {0}", Platform));
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("3Programnyelv: c#"));
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("3Uptime: {0}", SchumixBot.Uptime()));
				}
				else if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4] == "help")
				{
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, "3Parancsok: info | ghost | nick | sys");
				}
				else if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4] == "ghost")
				{
					if(!CManager.Admin(Network.IMessage.Nick, Network.IMessage.Host))
						return;

					sSendMessage.SendChatMessage(MessageType.PRIVMSG, "NickServ", String.Format("ghost {0} {1}", IRCConfig.NickName, IRCConfig.NickServPassword));
					Network.writer.WriteLine("NICK {0}", IRCConfig.NickName);
					SchumixBot.NickTarolo = IRCConfig.NickName;
				}
				else if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4] == "nick")
				{
					if(!CManager.Admin(Network.IMessage.Nick, Network.IMessage.Host))
						return;

					if(Network.IMessage.Info.Length < 6)
						return;

					if(Network.IMessage.Info[5] == "identify")
					{
						SchumixBot.NickTarolo = IRCConfig.NickName;
						Network.writer.WriteLine("NICK {0}", IRCConfig.NickName);
						Log.Notice("NickServ", "Sending NickServ identification.");
						sSendMessage.SendChatMessage(MessageType.PRIVMSG, "NickServ", String.Format("identify {0}", IRCConfig.NickServPassword));
					}
					else
					{
						string nick = Network.IMessage.Info[5];
						SchumixBot.NickTarolo = nick;
						Network.writer.WriteLine("NICK {0}", nick);
					}
				}
				else if(Network.IMessage.Info.Length >= 5 && Network.IMessage.Info[4] == "sys")
				{
					var memory = Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024;
					sSendMessage.SendChatMessage(MessageType.PRIVMSG, Network.IMessage.Channel, String.Format("3Memoria használat: {0} MB", memory));
				}
			}
		}
	}
}