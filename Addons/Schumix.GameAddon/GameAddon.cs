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
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.GameAddon.Commands;
using Schumix.GameAddon.MaffiaGames;

namespace Schumix.GameAddon
{
	public class GameAddon : GameCommand, ISchumixAddon
	{
		private readonly ChannelInfo sChannelInfo = Singleton<ChannelInfo>.Instance;
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;
		public static readonly Dictionary<string, string> GameChannelFunction = new Dictionary<string, string>();
		public static readonly Dictionary<string, MaffiaGame> MaffiaList = new Dictionary<string, MaffiaGame>();

		public void Setup()
		{
			CommandManager.PublicCRegisterHandler("game", new Action<IRCMessage>(HandleGame));
			Console.CancelKeyPress += (sender, e) => { Clean(); };
		}

		public void Destroy()
		{
			CommandManager.PublicCRemoveHandler("game");
			Clean();
		}

		public bool Reload(string RName)
		{
			return false;
		}

		public void HandlePrivmsg(IRCMessage sIRCMessage)
		{
			if(sChannelInfo.FSelect(IFunctions.Gamecommands) || sIRCMessage.Channel.Substring(0, 1) != "#")
			{
				if(!sChannelInfo.FSelect(IFunctions.Gamecommands, sIRCMessage.Channel) && sIRCMessage.Channel.Substring(0, 1) == "#")
					return;

				string channel = sIRCMessage.Channel.ToLower();

				if(MaffiaList.ContainsKey(channel) || sIRCMessage.Channel.Substring(0, 1) != "#")
				{
					if(sIRCMessage.Info.Length < 4)
						return;
	
					if(sIRCMessage.Channel.Substring(0, 1) != "#")
					{
						foreach(var maffia in MaffiaList)
						{
							foreach(var player in maffia.Value.GetPlayerList())
							{
								if(player.Value == sIRCMessage.Nick)
								{
									channel = maffia.Key;
									break;
								}
							}
						}
					}

					if(sIRCMessage.Channel.Substring(0, 1) != "#" && channel == sIRCMessage.Channel.ToLower())
						return;

					sIRCMessage.Info[3] = sIRCMessage.Info[3].Remove(0, 1, SchumixBase.Colon);
					if((sIRCMessage.Info[3].Length > 0 && sIRCMessage.Info[3].Substring(0, 1) != "!") || sIRCMessage.Info[3].Length == 0)
						return;

					switch(sIRCMessage.Info[3].ToLower())
					{
						case "!start":
						{
							if(MaffiaList[channel].GetOwner() == sIRCMessage.Nick || MaffiaList[channel].GetOwner() == string.Empty ||
								IsAdmin(sIRCMessage.Nick))
								MaffiaList[channel].Start();
							else
								sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "A játékot {0} indította!", MaffiaList[channel].GetOwner());
							break;
						}
						case "!stats":
						{
							MaffiaList[channel].Stats();
							break;
						}
						case "!join":
						{
							foreach(var maffia in MaffiaList)
							{
								if(sIRCMessage.Channel.ToLower() != maffia.Key)
								{
									foreach(var player in maffia.Value.GetPlayerList())
									{
										if(player.Value == sIRCMessage.Nick)
										{
											sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "{0}: Te már játékban vagy itt: {1}", sIRCMessage.Nick, maffia.Key);
											return;
										}
									}
								}
							}

							MaffiaList[channel].Join(sIRCMessage.Nick);
							break;
						}
						case "!leave":
						{
							if(sIRCMessage.Info.Length < 5)
							{
								MaffiaList[channel].Leave(sIRCMessage.Nick);
								return;
							}

							if(MaffiaList[channel].GetOwner() == sIRCMessage.Nick)
							{
								if(!MaffiaList[channel].GetKillerList().ContainsKey(sIRCMessage.Info[4].ToLower()) &&
									!MaffiaList[channel].GetDetectiveList().ContainsKey(sIRCMessage.Info[4].ToLower()) &&
									!MaffiaList[channel].GetNormalList().ContainsKey(sIRCMessage.Info[4].ToLower()) &&
									!MaffiaList[channel].GetPlayerList().ContainsValue(sIRCMessage.Info[4]))
									sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "{0}: Kit akarsz kiléptetni?", sIRCMessage.Nick);
								else
									MaffiaList[channel].Leave(sIRCMessage.Info[4], sIRCMessage.Nick);
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

							MaffiaList[channel].Kill(sIRCMessage.Info[4], sIRCMessage.Nick);
							break;
						}
						case "!lynch":
						{
							if(sIRCMessage.Info.Length < 5)
							{
								sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "{0}: Kit akarsz lincselni?", sIRCMessage.Nick);
								return;
							}

							MaffiaList[channel].Lynch(sIRCMessage.Info[4], sIRCMessage.Nick, sIRCMessage.Channel);
							break;
						}
						case "!rescue":
						{
							if(sIRCMessage.Info.Length < 5)
							{
								sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Kit akarsz megmenteni?");
								return;
							}

							MaffiaList[channel].Rescue(sIRCMessage.Info[4], sIRCMessage.Nick);
							break;
						}
						case "!see":
						{
							if(sIRCMessage.Info.Length < 5)
							{
								sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Kit akarsz kikérdezni?");
								return;
							}

							MaffiaList[channel].See(sIRCMessage.Info[4], sIRCMessage.Nick);
							break;
						}
						case "!gameover":
						{
							MaffiaList[channel].GameOver(sIRCMessage.Nick);
							break;
						}
						case "!end":
						{
							if(MaffiaList[channel].GetOwner() == sIRCMessage.Nick || MaffiaList[channel].GetOwner() == string.Empty ||
								IsAdmin(sIRCMessage.Nick))
							{
								if(MaffiaList[channel].Started)
								{
									MaffiaList[channel].RemoveRanks();
									MaffiaList[channel].StopThread();
									sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "A játék befejeződött.");

									if(MaffiaList[channel].GetPlayers() < 8)
										sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "*** A gyilkos 4{0} volt, a nyomozó 4{1}, az orvos pedig nem volt. Mindenki más hétköznapi civil volt.", MaffiaList[channel].GetKiller(), MaffiaList[channel].GetDetective());
									else
										sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "*** A gyilkos 4{0} volt, a nyomozó 4{1}, az orvos pedig 4{2}. Mindenki más hétköznapi civil volt.", MaffiaList[channel].GetKiller(), MaffiaList[channel].GetDetective(), MaffiaList[channel].GetDoctor());
								}
								else
								{
									MaffiaList[channel].RemoveRanks();
									MaffiaList[channel].StopThread();
									sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "A játék befejeződött.");
								}
							}
							else
								sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Sajnálom, de csak {0}, a játék indítója vethet véget a játéknak!", MaffiaList[channel].GetOwner());
							break;
						}
						default:
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "{0}: Nem létezik ilyen parancs!", sIRCMessage.Nick);
							break;
					}
				}
			}
		}

		public void HandleNotice(IRCMessage sIRCMessage)
		{

		}

		public void HandleLeft(IRCMessage sIRCMessage)
		{
			foreach(var maffia in GameAddon.MaffiaList)
			{
				if(!maffia.Value.Running)
					continue;

				if(maffia.Key != sIRCMessage.Channel.ToLower())
					continue;

				foreach(var player in maffia.Value.GetPlayerList())
				{
					if(player.Value == sIRCMessage.Nick)
					{
						maffia.Value.Leave(sIRCMessage.Nick);
						break;
					}
				}
			}
		}

		public void HandleKick(IRCMessage sIRCMessage)
		{
			foreach(var maffia in GameAddon.MaffiaList)
			{
				if(!maffia.Value.Running)
					continue;

				if(maffia.Key != sIRCMessage.Channel.ToLower())
					continue;

				foreach(var player in maffia.Value.GetPlayerList())
				{
					if(player.Value == sIRCMessage.Info[3])
					{
						maffia.Value.Leave(sIRCMessage.Info[3]);
						break;
					}
				}
			}
		}

		public void HandleQuit(IRCMessage sIRCMessage)
		{
			foreach(var maffia in GameAddon.MaffiaList)
			{
				if(!maffia.Value.Running)
					continue;

				foreach(var player in maffia.Value.GetPlayerList())
				{
					if(player.Value == sIRCMessage.Nick)
					{
						maffia.Value.Leave(sIRCMessage.Nick);
						break;
					}
				}
			}
		}

		public void HandleNewNick(IRCMessage sIRCMessage)
		{
			foreach(var maffia in GameAddon.MaffiaList)
			{
				if(!maffia.Value.Running)
					continue;

				foreach(var player in maffia.Value.GetPlayerList())
				{
					if(player.Value == sIRCMessage.Nick)
					{
						maffia.Value.NewNick(player.Key, sIRCMessage.Nick, sIRCMessage.Info[2].Remove(0, 1, SchumixBase.Colon));
						break;
					}
				}
			}
		}

		public bool HandleHelp(IRCMessage sIRCMessage)
		{
			return false;
		}

		private void Clean()
		{
			foreach(var mlist in MaffiaList)
			{
				mlist.Value.RemoveRanks();
				mlist.Value.StopThread();
			}

			GameChannelFunction.Clear();
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
			get { return Consts.SchumixProgrammedBy; }
		}

		/// <summary>
		/// Website where the addon is available.
		/// </summary>
		public string Website
		{
			get { return Consts.SchumixWebsite; }
		}
	}
}