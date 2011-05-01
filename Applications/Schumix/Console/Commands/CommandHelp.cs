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
using Schumix.Framework.Extensions;

namespace Schumix.Console.Commands
{
	public partial class CommandHandler
	{
		protected void HandleHelp()
		{
			if(Info.Length == 1)
			{
				string parancsok = string.Empty;

				foreach(var command in CCommandManager.GetCommandHandler())
				{
					if(command.Key == "help")
						continue;

					parancsok += ", " + command.Key;
				}

				Log.Notice("Console", "Ha a parancs moge irod a megadott parancs nevet vagy a nevet es alparancsat informaciot ad a hasznalatarol.");
				Log.Notice("Console", "Parancsok: {0}", parancsok.Remove(0, 2, ", "));
				return;
			}

			if(Info[1].ToLower() == "consolelog")
			{
				Log.Notice("Console", "Az irc adatok konzolra irasat engedelyezi vagy tiltja. Alapertelmezesben ki van kapcsolva.");
				Log.Notice("Console", "Hasznalata: consolelog <be vagy ki>");
			}
			else if(Info[1].ToLower() == "sys")
			{
				Log.Notice("Console", "Kiirja a botrol a rendszer informaciokat.");
			}
			else if(Info[1].ToLower() == "csatorna")
			{
				Log.Notice("Console", "A bot csatornara irasat allithatjuk vele.");
				Log.Notice("Console", "Hasznalata: csatorna <csatorna neve>");
			}
			else if(Info[1].ToLower() == "admin")
			{
				if(Info.Length < 3)
				{
					Log.Notice("Console", "Kiirja az operatorok vagy adminisztratorok altal hasznalhato parancsokat.");
					Log.Notice("Console", "Admin parancsai: info | lista | add | del | rang | hozzaferes | ujjelszo");
					return;
				}
		
				if(Info[2].ToLower() == "add")
				{
					Log.Notice("Console", "Uj admin hozzaadasa.");
					Log.Notice("Console", "Hasznalata: admin add <admin neve>");
				}
				else if(Info[2].ToLower() == "del")
				{
					Log.Notice("Console", "Admin eltavolitasa.");
					Log.Notice("Console", "Hasznalata: admin del <admin neve>");
				}
				else if(Info[2].ToLower() == "rang")
				{
					Log.Notice("Console", "Admin rangjanak megvaltoztatasa.");
					Log.Notice("Console", "Hasznalata: admin rang <admin neve> <uj rang pl halfoperator: 0, operator: 1, administrator: 2>");
				}
				else if(Info[2].ToLower() == "info")
				{
					Log.Notice("Console", "Kiirja éppen milyen rangja van az illetonek.");
					Log.Notice("Console", "Hasznalata: admin info <admin neve>");
				}
				else if(Info[2].ToLower() == "lista")
				{
					Log.Notice("Console", "Kiirja az összes admin nevet aki az adatbazisban szerepel.");
				}
				else if(Info[2].ToLower() == "hozzaferes")
				{
					Log.Notice("Console", "Az admin parancsok hasznalatahoz szukseges jelszo ellenorzo és vhost aktivalo.");
					Log.Notice("Console", "Hasznalata: admin hozzaferes <jelszo>");
				}
				else if(Info[2].ToLower() == "ujjelszo")
				{
					Log.Notice("Console", "Az admin jelszavanak csereje ha uj kene a regi helyett.");
					Log.Notice("Console", "Hasznalata: admin ujjelszo <regi jelszo> <uj jelszo>");
				}
			}
			else if(Info[1].ToLower() == "funkcio")
			{
				if(Info.Length < 3)
				{
					Log.Notice("Console", "Funkciok vezerlesere szolgalo parancs.");
					Log.Notice("Console", "Funkcio parancsai: channel | all | update | info");
					Log.Notice("Console", "Hasznalata globalisan:");
					Log.Notice("Console", "Globalis funkcio kezelese: funkcio <be vagy ki> <funkcio nev>");
					Log.Notice("Console", "Globalis funkciok kezelese: funkcio <be vagy ki> <funkcio nev1> <funkcio nev2> ... stb");
					return;
				}
		
				if(Info[2].ToLower() == "channel")
				{
					if(Info.Length < 4)
					{
						Log.Notice("Console", "Megadot channelen allithatok ezzel a parancsal a funkciok.");
						Log.Notice("Console", "Funkcio channel parancsai: info");
						Log.Notice("Console", "Hasznalata:");
						Log.Notice("Console", "Channel funkcio kezelese: funkcio channel <be vagy ki> <funkcio nev>");
						Log.Notice("Console", "Channel funkciok kezelése: funkcio channel <be vagy ki> <funkcio nev1> <funkcio nev2> ... stb");
						return;
					}

					if(Info[3].ToLower() == "info")
					{
						Log.Notice("Console", "Kiirja a funkciok allapotat.");
					}
				}
				else if(Info[2].ToLower() == "update")
				{
					Log.Notice("Console", "Frissiti a funkciokat vagy alapertelmezesre allitja.");
					Log.Notice("Console", "Hasznalata:");
					Log.Notice("Console", "Mas csatorna: funkcio update <csatorna neve>");
					Log.Notice("Console", "Globalis: funkcio update");
				}
				else if(Info[3].ToLower() == "info")
				{
					Log.Notice("Console", "Kiirja a funkciok allapotat.");
				}
			}
			else if(Info[1].ToLower() == "channel")
			{
				if(Info.Length < 3)
				{
					Log.Notice("Console", "Channel parancsai: add | del | info | update");
					return;
				}

				if(Info[2].ToLower() == "add")
				{
					Log.Notice("Console", "Uj channel hozzaadasa.");
					Log.Notice("Console", "Hasznalata: channel add <channel> <ha van pass akkor az>");
				}
				else if(Info[2].ToLower() == "del")
				{
					Log.Notice("Console", "Nem hasznalatos channel eltavolitasa.");
					Log.Notice("Console", "Hasznalata: channel del <channel>");
				}
				else if(Info[2].ToLower() == "info")
				{
					Log.Notice("Console", "Osszes channel kiirasa ami az adatbazisban van es a hozzajuk tartozo informaciok.");
				}
				else if(Info[2].ToLower() == "update")
				{
					Log.Notice("Console", "Channelekhez tartozo összes informacio frissitese, alapertelmezesre allitasa.");
				}
			}
			else if(Info[1].ToLower() == "connect")
			{
				Log.Notice("Console", "Kapcsolodas az irc szerverhez.");
			}
			else if(Info[1].ToLower() == "disconnect")
			{
				Log.Notice("Console", "Kapcsolat bontasa.");
			}
			else if(Info[1].ToLower() == "reconnect")
			{
				Log.Notice("Console", "Ujrakapcsolodas az irc szerverhez.");
			}
			else if(Info[1].ToLower() == "nick")
			{
				Log.Notice("Console", "Bot nick nevenek csereje.");
				Log.Notice("Console", "Hasznalata: {0}nick <nev>");
			}
			else if(Info[1].ToLower() == "join")
			{
				Log.Notice("Console", "Kapcsolodas megadot csatornara.");
				Log.Notice("Console", "Hasznalata:");
				Log.Notice("Console", "Jelszo nelkuli csatorna: join <csatorna>");
				Log.Notice("Console", "Jelszoval ellatott csatorna: join <csatorna> <jelszo>");
			}
			else if(Info[1].ToLower() == "left")
			{
				Log.Notice("Console", "Lelepes a megadott csatornarol.");
				Log.Notice("Console", "Hasznalata: left <csatorna>");
			}
			else if(Info[1].ToLower() == "kikapcs")
			{
				Log.Notice("Console", "Bot leallitasara hasznalhato parancs.");
			}
		}
	}
}