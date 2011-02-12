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
using Schumix.Framework;
using Schumix.Framework.Config;

namespace Schumix.Irc.Commands
{
	public partial class CommandHandler : CommandInfo
	{
		private readonly Utility sUtility = Singleton<Utility>.Instance;
		protected readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;
		protected readonly Sender sSender = Singleton<Sender>.Instance;
		protected readonly NickInfo sNickInfo = Singleton<NickInfo>.Instance;
		protected string ChannelPrivmsg { get; set; }
		protected string WhoisPrivmsg { get; set; }

		protected CommandHandler() {}

		protected void HandleHelp()
		{
			CNick();

			if(Network.IMessage.Info.Length == 4)
			{
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Ha a aparancs mögé irod a megadott parancs nevét vagy a nevet és egy alparancsát információt add a használatáról.");
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Fő parancsom: {0}xbot", IRCConfig.Parancselojel);
				return;
			}

			foreach(var plugin in ScriptManager.GetPlugins())
				plugin.HandleHelp();

			if(Network.IMessage.Info[4] == "xbot")
			{
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Felhasználok számára használható parancslista.");
			}
			else if(Network.IMessage.Info[4] == "info")
			{
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kis leírás a botról.");
			}
			else if(Network.IMessage.Info[4] == "whois")
			{
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "A parancs segítségével megtudhatjuk hogy egy nick milyen channelon van fent.");
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}whois <nick>", IRCConfig.Parancselojel);
			}
			/*else if(Network.IMessage.Info[4] == "jegyzet")
			{
				if(Network.IMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Különböző adatokat jegyezhetünk fel a segítségével.");
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Jegyzet parancsai: kod");
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Jegyzet beküldése: {0}jegyzet <amit feljegyeznél>", IRCConfig.Parancselojel);
					return;
				}
		
				if(Network.IMessage.Info[5] == "kod")
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Jegyzet kiolvasásához szükséges kód.", IRCConfig.Parancselojel);
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}jegyzet kod <kod amit kaptál>", IRCConfig.Parancselojel);
				}
			}*/
			else if(Network.IMessage.Info[4] == "roll")
			{
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Csöp szorakozás a wowból, már ha valaki felismeri :P");
			}
			else if(Network.IMessage.Info[4] == "datum")
			{
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Az aktuális dátumot irja ki és a hozzá tartozó névnapot.");
			}
			else if(Network.IMessage.Info[4] == "ido")
			{
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Az aktuális időt irja ki.");
			}
			else if(Network.IMessage.Info[4] == "keres")
			{
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Ha szökséged lenne valamire a google-ből nem kell hozzá weboldal csak ez a parancs.");
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}keres <ide jön a kereset szöveg>", IRCConfig.Parancselojel);
			}
			else if(Network.IMessage.Info[4] == "fordit")
			{
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Ha rögtön kéne fordítani másik nyelvre vagy ről valamit megteheted ezzel a parancsal.");
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}fordit <miről|mire> <szöveg>", IRCConfig.Parancselojel);
			}
			/*else if(Network.IMessage.Info[4] == "xrev")
			{
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Amik a kódba vannak integrálva projectek annak lekérdezhetőek egyes verziói.");
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}xrev <emulátor neve> <rev>", IRCConfig.Parancselojel);
			}*/
			else if(Network.IMessage.Info[4] == "irc")
			{
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Néhány parancs használata az IRC-n.");
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}irc <parancs neve>", IRCConfig.Parancselojel);
			}
			else if(Network.IMessage.Info[4] == "calc")
			{
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Több funkciós számologép.");
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}calc <szám>", IRCConfig.Parancselojel);
			}
			else if(Network.IMessage.Info[4] == "uzenet")
			{
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Figyelmeztető üzenet küldése hogy keresik ezzen a channelen vagy egy tetszöleges üzenet küldése.");
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}üzenet <ide jön a személy> <ha nem felhivás küldenél hanem saját üzenetet>", IRCConfig.Parancselojel);
			}
			else if(Network.IMessage.Info[4] == "sha1")
			{
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Sha1 kódolássá átalakitó parancs.");
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}sha1 <átalakitandó szöveg>", IRCConfig.Parancselojel);
			}
			else if(Network.IMessage.Info[4] == "md5")
			{
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Md5 kódolássá átalakitó parancs.");
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}md5 <átalakitandó szöveg>", IRCConfig.Parancselojel);
			}
			else if(Network.IMessage.Info[4] == "prime")
			{
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Megálapítja hogy a szám primszám-e. Csak egész számmal tud számolni!");
				sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}prime <szám>", IRCConfig.Parancselojel);
			}
		
			// Operátor parancsok segítségei
			if(Admin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Operator))
			{
				if(Network.IMessage.Info[4] == "admin")
				{
					if(Network.IMessage.Info.Length < 6)
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kiirja az operátorok vagy adminisztrátorok által használható parancsokat.");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Admin parancsai: add | del | rang | hozzaferes | ujjelszo");
						return;
					}
		
					if(Network.IMessage.Info[5] == "add")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Új admin hozzáadása.");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}admin add <admin neve>", IRCConfig.Parancselojel);
					}
					else if(Network.IMessage.Info[5] == "del")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Admin eltávolítása.");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}admin del <admin neve>", IRCConfig.Parancselojel);
					}
					else if(Network.IMessage.Info[5] == "rang")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Admin rangjának megváltoztatása.");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}admin rang <admin neve> <új rang pl operator: 0, administrator: 1>", IRCConfig.Parancselojel);
					}
					else if(Network.IMessage.Info[5] == "info")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kiirja az összes admin nevét aki az adatbázisban szerepel.");
					}
					else if(Network.IMessage.Info[5] == "hozzaferes")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Az admin parancsok használatához szükséges jelszó ellenörző és vhost aktiváló.");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}admin hozzaferes <jelszó>", IRCConfig.Parancselojel);
					}
					else if(Network.IMessage.Info[5] == "ujjelszo")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Az admin jelszavának cseréje ha új kéne a régi helyet.");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}admin ujjelszo <régi jelszó> <új jelszó>", IRCConfig.Parancselojel);
					}
				}
				else if(Network.IMessage.Info[4] == "channel")
				{
					if(Network.IMessage.Info.Length < 6)
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Channel parancsai: add | del | info | update");
						return;
					}
		
					if(Network.IMessage.Info[5] == "add")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Új channel hozzáadása.");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}channel add <channel> <ha van pass akkor az>", IRCConfig.Parancselojel);
					}
					else if(Network.IMessage.Info[5] == "del")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nem használatos channel eltávolítása.");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}channel del <channel>", IRCConfig.Parancselojel);
					}
					else if(Network.IMessage.Info[5] == "info")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Összes channel kiirása ami az adatbázisban van és a hozzájuk tartozó informáciok.");
					}
					else if(Network.IMessage.Info[5] == "update")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Channelekhez tartozó összes információ frissitése, alapértelmezésre állítása.");
					}
				}
				else if(Network.IMessage.Info[4] == "funkcio")
				{
					if(Network.IMessage.Info.Length < 6)
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Funkciók vezérlésére szolgáló parancs.");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Funkcio parancsai: channel | all | update | info");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata ahol tartozkodsz:");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Channel funkció kezelése: {0}funkcio <be vagy ki> <funkcio név>", IRCConfig.Parancselojel);
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Channel funkciók kezelése: {0}funkcio <be vagy ki> <funkcio név1> <funkcio név2> ... stb", IRCConfig.Parancselojel);
						return;
					}
		
					if(Network.IMessage.Info[5] == "channel")
					{
						if(Network.IMessage.Info.Length < 7)
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Megadot channelen állithatók ezzel a parancsal a funkciók.");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Funkcio channel parancsai: info");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata:");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Channel funkció kezelése: {0}funkcio <be vagy ki> <funkcio név>", IRCConfig.Parancselojel);
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Channel funkciók kezelése: {0}funkcio <be vagy ki> <funkcio név1> <funkcio név2> ... stb", IRCConfig.Parancselojel);
							return;
						}
		
						if(Network.IMessage.Info[6] == "info")
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kiirja a funkciók állapotát.");
						}
					}
					else if(Network.IMessage.Info[5] == "all")
					{
						if(Network.IMessage.Info.Length < 7)
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Globális funkciók kezelése.");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Funkcio all parancsai: info");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Együtes kezelés: {0}funkcio all <be vagy ki> <funkcio név>", IRCConfig.Parancselojel);
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Együtes funkciók kezelése: {0}funkcio all <be vagy ki> <funkcio név1> <funkcio név2> ... stb", IRCConfig.Parancselojel);
							return;
						}
		
						if(Network.IMessage.Info[6] == "info")
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kiirja a funkciók állapotát.");
						}
					}
					else if(Network.IMessage.Info[5] == "update")
					{
						if(Network.IMessage.Info.Length < 7)
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Frissiti a funkciókat vagy alapértelmezésre állitja.");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Funkcio update parancsai: all");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata:");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Más channel: {0}funkcio update <channel neve>", IRCConfig.Parancselojel);
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Ahol tartozkodsz channel: {0}funkcio update", IRCConfig.Parancselojel);
							return;
						}
		
						if(Network.IMessage.Info[6] == "all")
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Frissiti az összes funkciót vagy alapértelmezésre állitja.");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}funkcio update all", IRCConfig.Parancselojel);
						}
					}
					else if(Network.IMessage.Info[5] == "info")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kiirja a funkciók állapotát.");
					}
				}
				/*else if(Network.IMessage.Info[4] == "autofunkcio")
				{
					if(Network.IMessage.Info.Length < 6)
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Autómatikusan müködő kódrészek kezelése.");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Autofunkcio parancsai: kick | mode | hlfunkcio");
						return;
					}
		
					if(Network.IMessage.Info[5] == "kick")
					{
						if(Network.IMessage.Info.Length < 7)
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Autómatikusan kirúgásra kerülő nick-ek kezelése.");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kick parancsai: add | del | info | channel");
							return;
						}
		
						if(Network.IMessage.Info[6] == "add")
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kirúgandó nevének hozzáadása ahol tartozkodsz.");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}autofunkcio kick add <nev> <oka>", IRCConfig.Parancselojel);
						}
						else if(Network.IMessage.Info[6] == "del")
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kirúgandó nevének eltávolítása ahol tartozkodsz.");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}autofunkcio kick del <nev>", IRCConfig.Parancselojel);
						}
						else if(Network.IMessage.Info[6] == "info")
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kiirja a kirúgandok állapotát.");
						}
						else if(Network.IMessage.Info[6] == "channel")
						{
							if(res.size() < 5)
							{
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Autómatikusan kirúgásra kerülő nick-ek kezelése megadot channelen.");
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kick channel parancsai: add | del | info");
								return;
							}
		
							if(res[4] == "add")
							{
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kirúgandó nevének hozzáadása megadott channelen.");
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}autofunkcio kick channel add <nev> <channel> <oka>", IRCConfig.Parancselojel);
							}
							else if(res[4] == "del")
							{
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kirúgandó nevének eltávolítása megadott channelen.");
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}autofunkcio kick channel del <nev>", IRCConfig.Parancselojel);
							}
							else if(res[4] == "info")
							{
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kiirja a kirúgandok állapotát.");
							}
						}
					}
					else if(Network.IMessage.Info[5] == "mode")
					{
						if(Network.IMessage.Info.Length < 7)
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Autómatikusan rangot kapó nick-ek kezelése.");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Mode parancsai: add | del | info | channel");
							return;
						}
		
						if(Network.IMessage.Info[6] == "add")
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Rangot kapó nevének hozzáadása ahol tartozkodsz.");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}autofunkcio mode add <nev> <rang>", IRCConfig.Parancselojel);
						}
						else if(Network.IMessage.Info[6] == "del")
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Rangot kapó nevének eltávolítása ahol tartozkodsz.");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}autofunkcio mode del <nev>", IRCConfig.Parancselojel);
						}
						else if(Network.IMessage.Info[6] == "info")
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kiirja a rangot kapók állapotát.");
						}
						else if(Network.IMessage.Info[6] == "channel")
						{
							if(res.size() < 5)
							{
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Autómatikusan rangot kapó nick-ek kezelése megadot channelen.");
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Mode channel parancsai: add | del | info");
								return;
							}
		
							if(res[4] == "add")
							{
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Rangot kapó nevének hozzáadása megadott channelen.");
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}autofunkcio mode channel add <nev> <channel> <rang>", IRCConfig.Parancselojel);
							}
							else if(res[4] == "del")
							{
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Rangot kapó nevének eltávolítása megadott channelen.");
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}autofunkcio mode channel del <nev>", IRCConfig.Parancselojel);
							}
							else if(res[4] == "info")
							{
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kiirja a rangot kapók állapotát.");
							}
						}
					}
					else if(Network.IMessage.Info[5] == "hlfunkcio")
					{
						if(Network.IMessage.Info.Length < 7)
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Autómatikusan hl-t kapó nick-ek kezelése.");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kick channel parancsai: funkcio | update | info");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}autofunkcio hluzenet <üzenet>", IRCConfig.Parancselojel);
							return;
						}
		
						if(Network.IMessage.Info[6] == "funkcio")
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Ezzel a parancsal állitható a hl állapota.");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}hluzenet funkcio <állapot>", IRCConfig.Parancselojel);
						}
						else if(Network.IMessage.Info[6] == "update")
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nem üzemel!");
						}
						else if(Network.IMessage.Info[6] == "info")
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kiirja a hl-ek állapotát.");
						}
					}
				}*/
				else if(Network.IMessage.Info[4] == "szinek")
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Adot skálájú szinek kiirása amit lehet használni IRC-n.");
				}
				else if(Network.IMessage.Info[4] == "sznap")
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kiirja a megadott név születésnapjának dátumát.");
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}sznap <név>", IRCConfig.Parancselojel);
				}
				else if(Network.IMessage.Info[4] == "nick")
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Bot nick nevének cseréje.");
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}nick <név>", IRCConfig.Parancselojel);
				}
				else if(Network.IMessage.Info[4] == "join")
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kapcsolodás megadot channelra.");
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata:");
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Jelszó nélküli channel: {0}join <channel>", IRCConfig.Parancselojel);
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Jelszóval ellátott channel: {0}join <channel> <jelszó>", IRCConfig.Parancselojel);
				}
				else if(Network.IMessage.Info[4] == "left")
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Lelépés megadot channelra.");
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}left <channel>", IRCConfig.Parancselojel);
				}
				else if(Network.IMessage.Info[4] == "kick")
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kirúgja a nick-et a megadott channelről.");
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata:");
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Csak kirugás: {0}kick <channel> <név>", IRCConfig.Parancselojel);
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kirugás okkal: {0}kick <channel> <név> <oka>", IRCConfig.Parancselojel);
				}
				else if(Network.IMessage.Info[4] == "mode")
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Megváltoztatja a nick rangját megadott channelen.");
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}mode <rang> <név vagy nevek>", IRCConfig.Parancselojel);
				}
				/*else if(Network.IMessage.Info[4] == "svn")
				{
					if(Network.IMessage.Info.Length < 6)
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Svn rss-ek kezelése.");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Svn parancsai: add | del | info | lista | new | stop | reload");
						return;
					}
		
					if(Network.IMessage.Info[5] == "add")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Új channel hozzáadása az rss-hez.");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}svn add <rss neve> <channel>", IRCConfig.Parancselojel);
					}
					else if(Network.IMessage.Info[5] == "del")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nem használatos channel eltávolítása az rss-ből.");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}svn del <rss neve> <channel>", IRCConfig.Parancselojel);
					}
					else if(Network.IMessage.Info[5] == "info")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kiirja az rss-ek állapotát.");
					}
					else if(Network.IMessage.Info[5] == "lista")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Választható rss-ek listája.");
					}
					else if(Network.IMessage.Info[5] == "new")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Új rss betöltése.");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}svn new <rss neve>", IRCConfig.Parancselojel);
					}
					else if(Network.IMessage.Info[5] == "stop")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Rss leállítása.");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}svn stop <rss neve>", IRCConfig.Parancselojel);
					}
					else if(Network.IMessage.Info[5] == "reload")
					{
						if(Network.IMessage.Info.Length < 7)
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Megadott rss újratöltése.");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Svn reload parancsai: all");
							res.clear();
							return;
						}
		
						else if(Network.IMessage.Info[6] == "all")
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Minden rss újratöltése.");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}svn reload <rss neve>", IRCConfig.Parancselojel);
						}
					}
				}
				else if(Network.IMessage.Info[4] == "git")
				{
					if(Network.IMessage.Info.Length < 6)
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Git rss-ek kezelése.");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Git parancsai: add | del | info | lista | new | stop | reload");
						res.clear();
						return;
					}
		
					if(Network.IMessage.Info[5] == "add")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Új channel hozzáadása az rss-hez.");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}git add <rss neve> <tipus> <channel>", IRCConfig.Parancselojel);
					}
					else if(Network.IMessage.Info[5] == "del")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nem használatos channel eltávolítása az rss-ből.");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}git del <rss neve> <channel>", IRCConfig.Parancselojel);
					}
					else if(Network.IMessage.Info[5] == "info")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kiirja az rss-ek állapotát.");
					}
					else if(Network.IMessage.Info[5] == "lista")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Választható rss-ek listája.");
					}
					else if(Network.IMessage.Info[5] == "new")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Új rss betöltése.");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}git new <rss neve> <tipus>", IRCConfig.Parancselojel);
					}
					else if(Network.IMessage.Info[5] == "stop")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Rss leállítása.");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}git stop <rss neve> <tipus>", IRCConfig.Parancselojel);
					}
					else if(Network.IMessage.Info[5] == "reload")
					{
						if(Network.IMessage.Info.Length < 7)
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Megadott rss újratöltése.");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Git reload parancsai: all");
							res.clear();
							return;
						}
		
						else if(Network.IMessage.Info[6] == "all")
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Minden rss újratöltése.");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}git reload <rss neve> <tipus>", IRCConfig.Parancselojel);
						}
					}
				}
				else if(Network.IMessage.Info[4] == "hg")
				{
					if(Network.IMessage.Info.Length < 6)
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Hg rss-ek kezelése.");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Hg parancsai: add | del | info | lista | new | stop | reload");
						res.clear();
						return;
					}
		
					if(Network.IMessage.Info[5] == "add")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Új channel hozzáadása az rss-hez.");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}hg add <rss neve> <channel>", IRCConfig.Parancselojel);
					}
					else if(Network.IMessage.Info[5] == "del")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Nem használatos channel eltávolítása az rss-ből.");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}hg del <rss neve> <channel>", IRCConfig.Parancselojel);
					}
					else if(Network.IMessage.Info[5] == "info")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kiirja az rss-ek állapotát.");
					}
					else if(Network.IMessage.Info[5] == "lista")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Választható rss-ek listája.");
					}
					else if(Network.IMessage.Info[5] == "new")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Új rss betöltése.");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}hg new <rss neve>", IRCConfig.Parancselojel);
					}
					else if(Network.IMessage.Info[5] == "stop")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Rss leállítása.");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}hg stop <rss neve>", IRCConfig.Parancselojel);
					}
					else if(Network.IMessage.Info[5] == "reload")
					{
						if(Network.IMessage.Info.Length < 7)
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Megadott rss újratöltése.");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Hg reload parancsai: all");
							res.clear();
							return;
						}
		
						else if(Network.IMessage.Info[6] == "all")
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Minden rss újratöltése.");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}hg reload <rss neve>", IRCConfig.Parancselojel);
						}
					}
				}*/
			}
		
			// Adminisztrátor parancsok
			if(Admin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Administrator))
			{
				/*else if(Network.IMessage.Info[4] == "reload")
				{
					if(Network.IMessage.Info.Length < 6)
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Bot egyes részeinek újratöltése.");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Reload parancsai: info");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}reload <rész neve>", IRCConfig.Parancselojel);
						res.clear();
						return;
					}
		
					if(Network.IMessage.Info[5] == "info")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Jelenleg újrainditható részekről infó.");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}hg add <rss neve> <channel>", IRCConfig.Parancselojel); // hülyeség szerepel itt
					}
				}*/
				if(Network.IMessage.Info[4] == "plugin")
				{
					if(Network.IMessage.Info.Length < 6)
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kiirja milyen pluginok vannak betöltve.");
						return;
					}
		
					if(Network.IMessage.Info[5] == "load")
					{
						if(Network.IMessage.Info.Length < 7)
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Plugin betöltésére szólgáló parancs.");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}plugin load <plugin neve>", IRCConfig.Parancselojel);
							return;
						}

						if(Network.IMessage.Info[6] == "all")
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Betölt minden plugint.");
					}
					else if(Network.IMessage.Info[5] == "unload")
					{
						if(Network.IMessage.Info.Length < 7)
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Plugin eltávólítására szólgáló parancs.");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}plugin unload <plugin neve>", IRCConfig.Parancselojel);
							return;
						}

						if(Network.IMessage.Info[6] == "all")
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Eltávolít minden plugint.");
					}
				}
				else if(Network.IMessage.Info[4] == "kikapcs")
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Bot leállítására használható parancs.");
				}
			}

			string ParancsJel = IRCConfig.NickName + ",";
			string INick = Network.IMessage.Info[4];

			if(INick.ToLower() == ParancsJel.ToLower())
			{
				if(Network.IMessage.Info.Length < 6)
				{
					if(Admin(Network.IMessage.Nick, AdminFlag.Operator))
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Parancsok: ghost | nick | sys");
					else if(Admin(Network.IMessage.Nick, AdminFlag.Administrator))
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Parancsok: ghost | nick | sys | clean");
					else
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Parancsok: sys");
					return;
				}

				if(Network.IMessage.Info[5] == "sys")
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kiirja a program információit.");
				}
				else if(Network.IMessage.Info[5] == "ghost" && Admin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Operator))
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kilépteti a fő nick-et ha regisztrálva van.");
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0} ghost", ParancsJel.ToLower());
				}
				else if(Network.IMessage.Info[5] == "nick" && Admin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Operator))
				{
					if(Network.IMessage.Info.Length < 7)
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Bot nick nevének cseréje.");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0} nick <név>", ParancsJel.ToLower());
						return;
					}

					if(Network.IMessage.Info[6] == "identify")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Aktiválja a fő nick jelszavát.");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0} nick identify", ParancsJel.ToLower());
					}
				}
				else if(Network.IMessage.Info[5] == "clean" && Admin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Administrator))
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Felszabadítja a lefoglalt memóriát.");
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0} clean", ParancsJel.ToLower());
				}
			}
		}
	}
}
