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
using System.Collections.Generic;
using Schumix.API;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Extensions;
using Schumix.GameAddon.Commands;
using Schumix.GameAddon.KillerGames;

namespace Schumix.GameAddon
{
	public class GameAddon : GameCommand, ISchumixAddon
	{
		private readonly ChannelInfo sChannelInfo = Singleton<ChannelInfo>.Instance;
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;
		private readonly Sender sSender = Singleton<Sender>.Instance;
		public static readonly Dictionary<string, KillerGame> KillerList = new Dictionary<string, KillerGame>();
		public static readonly Dictionary<string, string> GameChannelFunction = new Dictionary<string, string>();

		public void Setup()
		{
			Network.PublicRegisterHandler("NICK",     		new Action<IRCMessage>(HandleNewNick));
			CommandManager.PublicCRegisterHandler("game",	new Action<IRCMessage>(HandleGame));
		}

		public void Destroy()
		{
			Network.PublicRemoveHandler("NICK");
			CommandManager.PublicCRemoveHandler("game");
		}

		public bool Reload(string RName)
		{
			return false;
		}

		public void HandlePrivmsg(IRCMessage sIRCMessage)
		{
			//if(sChannelInfo.FSelect("gamecommands") || sIRCMessage.Channel.Substring(0, 1) != "#")
			{
				//if(!sChannelInfo.FSelect("gamecommands", sIRCMessage.Channel) && sIRCMessage.Channel.Substring(0, 1) == "#")
					//return;

				CNick(sIRCMessage);
				string channel = sIRCMessage.Channel.ToLower();

				if(KillerList.ContainsKey(channel) || sIRCMessage.Channel.Substring(0, 1) != "#")
				{
					if(sIRCMessage.Info.Length < 4)
						return;
	
					if(sIRCMessage.Channel.Substring(0, 1) != "#")
					{
						foreach(var kill in KillerList)
						{
							foreach(var player in kill.Value.GetPlayerList())
							{
								if(player.Value == sIRCMessage.Nick)
								{
									channel = kill.Key;
									break;
								}
							}
						}
					}

					sIRCMessage.Info[3] = sIRCMessage.Info[3].Remove(0, 1, ":");
					switch(sIRCMessage.Info[3].ToLower())
					{
						case "!start":
						{
							if(KillerList[channel].GetOwner() == sIRCMessage.Nick)
								KillerList[channel].Start();
							else
								sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "A játékot {0} indította!", KillerList[channel].GetOwner());
							break;
						}
						case "!stats":
						{
							KillerList[channel].Stats();
							break;
						}
						case "!join":
						{
							KillerList[channel].Join(sIRCMessage.Nick);
							break;
						}
						case "!left":
						{
							if(sIRCMessage.Info.Length < 5)
							{
								KillerList[channel].Left(sIRCMessage.Nick);
								return;
							}

							if(KillerList[channel].GetOwner() == sIRCMessage.Nick.ToLower())
							{
								if(!KillerList[channel].GetKillerList().ContainsKey(sIRCMessage.Info[4].ToLower()) &&
									!KillerList[channel].GetDetectiveList().ContainsKey(sIRCMessage.Info[4].ToLower()) &&
									!KillerList[channel].GetNormalList().ContainsKey(sIRCMessage.Info[4].ToLower()))
									sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "{0}: Kit akarsz kiléptetni?", sIRCMessage.Nick);
								else
									KillerList[channel].Left(sIRCMessage.Info[4]);
							}
							else
								sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "{0}: Nem te indítottad a játékot!", sIRCMessage.Nick);
							break;
						}
						case "!kill":
						{
							if(sIRCMessage.Info.Length < 5)
							{
								sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Kit akarsz megölni?");
								return;
							}

							KillerList[channel].Kill(sIRCMessage.Info[4], sIRCMessage.Nick);
							break;
						}
						case "!lynch":
						{
							if(sIRCMessage.Info.Length < 5)
							{
								sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "{0}: Kit akarsz lincselni?", sIRCMessage.Nick);
								return;
							}

							KillerList[channel].Lynch(sIRCMessage.Info[4], sIRCMessage.Nick);
							break;
						}
						case "!rescue":
							break;
						case "!see":
						{
							if(sIRCMessage.Info.Length < 5)
							{
								sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Kit akarsz kikérdezni?");
								return;
							}

							KillerList[channel].See(sIRCMessage.Info[4], sIRCMessage.Nick);
							break;
						}
						case "!end":
						{
							if(KillerList[channel].GetOwner() == sIRCMessage.Nick)
							{
								sSender.Mode(channel, "-m");

								if(KillerList[channel].Started)
								{
									foreach(var end in KillerList[channel].GetPlayerList())
										sSender.Mode(sIRCMessage.Channel, "-v", end.Value);

									KillerList[channel].StopThread();
									sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "A játék befejeződött.");
									sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "*** A gyilkos 4{0} volt, a nyomozó 4{1}, az orvos pedig None. Mindenki más hétköznapi civil volt.", KillerList[channel].GetKiller(), KillerList[channel].GetDetective());
								}
								else
								{
									KillerList[channel].StopThread();
									sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "A játék befejeződött.");
								}
							}
							else
								sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Sajnálom, de csak {0}, a játék indítója vethet véget a játéknak!", KillerList[channel].GetOwner());
							break;
						}
						//default:
							//sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "{0}: Nem létezik ilyen parancs!", sIRCMessage.Nick);
					}
				}
			}
		}

		public void HandleNotice(IRCMessage sIRCMessage)
		{

		}

		public bool HandleHelp(IRCMessage sIRCMessage)
		{
			return false;
		}

		/// <summary>
		/// Name of the addon
		/// </summary>
		public string Name
		{
			get { return "GameAddon"; }
		}

		/// <summary>
		/// Author of the addon.
		/// </summary>
		public string Author
		{
			get { return "Megax"; }
		}

		/// <summary>
		/// Website where the addon is available.
		/// </summary>
		public string Website
		{
			get { return "http://www.github.com/megax/Schumix2"; }
		}
	}
}