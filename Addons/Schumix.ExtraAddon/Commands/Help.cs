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
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework.Config;

namespace Schumix.ExtraAddon.Commands
{
	public partial class Functions : CommandInfo
	{
		public void Help()
		{
			if(Network.IMessage.Info[4].ToLower() == "jegyzet")
			{
				if(Network.IMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Különböző adatokat jegyezhetünk fel a segítségével.");
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Jegyzet parancsai: user | kod");
					sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Jegyzet beküldése: {0}jegyzet <egy kód amit megjegyzünk pl: schumix> <amit feljegyeznél>", IRCConfig.CommandPrefix);
					return;
				}

				if(Network.IMessage.Info[5].ToLower() == "user")
				{
					if(Network.IMessage.Info.Length < 7)
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Jegyzet felhasználó kezelése.");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "User parancsai: register | remove | hozzaferes | ujjelszo");
						return;
					}

					if(Network.IMessage.Info[6].ToLower() == "register")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Új felhasználó hozzáadása.");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}jegyzet user register <jelszó>", IRCConfig.CommandPrefix);
					}
					else if(Network.IMessage.Info[6].ToLower() == "remove")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Felhasználó eltávolítása.");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}jegyzet user remove <jelszó>", IRCConfig.CommandPrefix);
					}
					else if(Network.IMessage.Info[6].ToLower() == "hozzaferes")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Az jegyzet parancsok használatához szükséges jelszó ellenörző és vhost aktiváló.");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}jegyzet user hozzaferes <jelszó>", IRCConfig.CommandPrefix);
					}
					else if(Network.IMessage.Info[6].ToLower() == "ujjelszo")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Felhasználó jelszavának cseréje ha új kéne a régi helyet.");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}jegyzet user ujjelszo <régi jelszó> <új jelszó>", IRCConfig.CommandPrefix);
					}
				}
				else if(Network.IMessage.Info[5].ToLower() == "kod")
				{
					if(Network.IMessage.Info.Length < 7)
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Jegyzet kiolvasásához szükséges kód.", IRCConfig.CommandPrefix);
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}jegyzet kod <jegyzet kódja>", IRCConfig.CommandPrefix);
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kod parancsai: del");
						return;
					}

					if(Network.IMessage.Info[6].ToLower() == "del")
					{
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Törli a jegyzetet kód alapján.");
						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}jegyzet kod del <jegyzet kódja>", IRCConfig.CommandPrefix);
					}
				}
			}

			// Fél Operátor parancsok segítségei
			if(IsAdmin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.HalfOperator))
			{
				if(Network.IMessage.Info[4].ToLower() == "autofunkcio")
				{
					if(Network.IMessage.Info.Length < 6)
					{
						if(IsAdmin(Network.IMessage.Nick, AdminFlag.HalfOperator))
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Autómatikusan müködő kódrészek kezelése.");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Autofunkcio parancsai: hluzenet");
							return;
						}
						else if(IsAdmin(Network.IMessage.Nick, AdminFlag.Operator))
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Autómatikusan müködő kódrészek kezelése.");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Autofunkcio parancsai: kick | mode | hluzenet");
							return;
						}
					}

					if(Network.IMessage.Info[5].ToLower() == "hluzenet")
					{
						if(Network.IMessage.Info.Length < 7)
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Autómatikusan hl-t kapó nick-ek kezelése.");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kick channel parancsai: funkcio | update | info");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}autofunkcio hluzenet <üzenet>", IRCConfig.CommandPrefix);
							return;
						}

						if(Network.IMessage.Info[6].ToLower() == "funkcio")
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Ezzel a parancsal állitható a hl állapota.");
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}hluzenet funkcio <állapot>", IRCConfig.CommandPrefix);
						}
						else if(Network.IMessage.Info[6].ToLower() == "update")
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Frissiti az adatbázisban szereplő hl listát!");
						}
						else if(Network.IMessage.Info[6].ToLower() == "info")
						{
							sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kiirja a hl-ek állapotát.");
						}
					}

					if(IsAdmin(Network.IMessage.Nick, Network.IMessage.Host, AdminFlag.Operator))
					{
						if(Network.IMessage.Info[5].ToLower() == "kick")
						{
							if(Network.IMessage.Info.Length < 7)
							{
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Autómatikusan kirúgásra kerülő nick-ek kezelése.");
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kick parancsai: add | del | info | channel");
								return;
							}

							if(Network.IMessage.Info[6].ToLower() == "add")
							{
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kirúgandó nevének hozzáadása ahol tartozkodsz.");
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}autofunkcio kick add <név> <oka>", IRCConfig.CommandPrefix);
							}
							else if(Network.IMessage.Info[6].ToLower() == "del")
							{
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kirúgandó nevének eltávolítása ahol tartozkodsz.");
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}autofunkcio kick del <név>", IRCConfig.CommandPrefix);
							}
							else if(Network.IMessage.Info[6].ToLower() == "info")
							{
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kiirja a kirúgandok állapotát.");
							}
							else if(Network.IMessage.Info[6].ToLower() == "channel")
							{
								if(Network.IMessage.Info.Length < 8)
								{
									sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Autómatikusan kirúgásra kerülő nick-ek kezelése megadot channelen.");
									sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kick channel parancsai: add | del | info");
									return;
								}

								if(Network.IMessage.Info[7].ToLower() == "add")
								{
									sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kirúgandó nevének hozzáadása megadott channelen.");
									sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}autofunkcio kick channel add <név> <csatorna> <oka>", IRCConfig.CommandPrefix);
								}
								else if(Network.IMessage.Info[7].ToLower() == "del")
								{
									sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kirúgandó nevének eltávolítása megadott channelen.");
									sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}autofunkcio kick channel del <név>", IRCConfig.CommandPrefix);
								}
								else if(Network.IMessage.Info[7].ToLower() == "info")
								{
									sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kiirja a kirúgandok állapotát.");
								}
							}
						}
						else if(Network.IMessage.Info[5].ToLower() == "mode")
						{
							if(Network.IMessage.Info.Length < 7)
							{
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Autómatikusan rangot kapó nick-ek kezelése.");
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Mode parancsai: add | del | info | channel");
								return;
							}

							if(Network.IMessage.Info[6].ToLower() == "add")
							{
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Rangot kapó nevének hozzáadása ahol tartozkodsz.");
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}autofunkcio mode add <név> <rang>", IRCConfig.CommandPrefix);
							}
							else if(Network.IMessage.Info[6].ToLower() == "del")
							{
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Rangot kapó nevének eltávolítása ahol tartozkodsz.");
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}autofunkcio mode del <név>", IRCConfig.CommandPrefix);
							}
							else if(Network.IMessage.Info[6].ToLower() == "info")
							{
								sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kiirja a rangot kapók állapotát.");
							}
							else if(Network.IMessage.Info[6].ToLower() == "channel")
							{
								if(Network.IMessage.Info.Length < 8)
								{
									sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Autómatikusan rangot kapó nick-ek kezelése megadot channelen.");
									sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Mode channel parancsai: add | del | info");
									return;
								}

								if(Network.IMessage.Info[7].ToLower() == "add")
								{
									sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Rangot kapó nevének hozzáadása megadott channelen.");
									sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}autofunkcio mode channel add <név> <csatorna> <rang>", IRCConfig.CommandPrefix);
								}
								else if(Network.IMessage.Info[7].ToLower() == "del")
								{
									sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Rangot kapó nevének eltávolítása megadott channelen.");
									sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Használata: {0}autofunkcio mode channel del <név>", IRCConfig.CommandPrefix);
								}
								else if(Network.IMessage.Info[7].ToLower() == "info")
								{
									sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "Kiirja a rangot kapók állapotát.");
								}
							}
						}
					}
				}
			}
		}
	}
}