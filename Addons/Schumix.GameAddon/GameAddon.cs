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
using Schumix.GameAddon.MaffiaGames;
using Schumix.GameAddon.IJAGames;

namespace Schumix.GameAddon
{
	public class GameAddon : GameCommand, ISchumixAddon
	{
		private readonly ChannelInfo sChannelInfo = Singleton<ChannelInfo>.Instance;
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;
		private readonly Sender sSender = Singleton<Sender>.Instance;
		public static readonly Dictionary<string, string> GameChannelFunction = new Dictionary<string, string>();
		public static readonly Dictionary<string, MaffiaGame> MaffiaList = new Dictionary<string, MaffiaGame>();
		public static readonly Dictionary<string, IJAGame> IJAList = new Dictionary<string, IJAGame>();

		public void Setup()
		{
			Network.PublicRegisterHandler("NICK",     		new Action<IRCMessage>(HandleNewNick));
			Network.PublicRegisterHandler("QUIT",     		new Action<IRCMessage>(HandleQuit));
			CommandManager.PublicCRegisterHandler("game",	new Action<IRCMessage>(HandleGame));
			Console.CancelKeyPress += (sender, e) => { Clean(); };
		}

		public void Destroy()
		{
			Network.PublicRemoveHandler("NICK");
			Network.PublicRemoveHandler("QUIT");
			CommandManager.PublicCRemoveHandler("game");
			Clean();
		}

		public bool Reload(string RName)
		{
			return false;
		}

		public void HandlePrivmsg(IRCMessage sIRCMessage)
		{
			if(sChannelInfo.FSelect("gamecommands") || sIRCMessage.Channel.Substring(0, 1) != "#")
			{
				if(!sChannelInfo.FSelect("gamecommands", sIRCMessage.Channel) && sIRCMessage.Channel.Substring(0, 1) == "#")
					return;

				CNick(sIRCMessage);
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

					sIRCMessage.Info[3] = sIRCMessage.Info[3].Remove(0, 1, SchumixBase.Point2);
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
							break;
						case "!end":
						{
							if(MaffiaList[channel].GetOwner() == sIRCMessage.Nick || MaffiaList[channel].GetOwner() == string.Empty ||
								IsAdmin(sIRCMessage.Nick))
							{
								sSender.Mode(channel, "-m");

								if(MaffiaList[channel].Started)
								{
									foreach(var end in MaffiaList[channel].GetPlayerList())
										sSender.Mode(sIRCMessage.Channel, "-v", end.Value);

									MaffiaList[channel].StopThread();
									sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "A játék befejeződött.");

									if(MaffiaList[channel].GetPlayers() < 8)
										sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "*** A gyilkos 4{0} volt, a nyomozó 4{1}, az orvos pedig nem volt. Mindenki más hétköznapi civil volt.", MaffiaList[channel].GetKiller(), MaffiaList[channel].GetDetective());
									else
										sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "*** A gyilkos 4{0} volt, a nyomozó 4{1}, az orvos pedig 4{2}. Mindenki más hétköznapi civil volt.", MaffiaList[channel].GetKiller(), MaffiaList[channel].GetDetective(), MaffiaList[channel].GetDoctor());
								}
								else
								{
									foreach(var end in MaffiaList[channel].GetPlayerList())
										sSender.Mode(sIRCMessage.Channel, "-v", end.Value);

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
				else if(IJAList.ContainsKey(channel) || sIRCMessage.Channel.Substring(0, 1) != "#")
				{
					if(sIRCMessage.Info.Length < 4)
						return;
	
					if(sIRCMessage.Channel.Substring(0, 1) != "#")
					{
						foreach(var ija in IJAList)
						{
							foreach(var player in ija.Value.GetPlayerList())
							{
								if(player.Value == sIRCMessage.Nick)
								{
									channel = ija.Key;
									break;
								}
							}
						}
					}

					sIRCMessage.Info[3] = sIRCMessage.Info[3].Remove(0, 1, SchumixBase.Point2);
					switch(sIRCMessage.Info[3].ToLower())
					{
						case "!start":
						{
							if(IJAList[channel].GetOwner() == sIRCMessage.Nick || IJAList[channel].GetOwner() == string.Empty ||
								IsAdmin(sIRCMessage.Nick))
								IJAList[channel].Start();
							else
								sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "A játékot {0} indította!", IJAList[channel].GetOwner());
							break;
						}
						case "!stats":
						{
							IJAList[channel].Stats();
							break;
						}
						case "!join":
						{
							foreach(var ija in IJAList)
							{
								if(sIRCMessage.Channel.ToLower() != ija.Key)
								{
									foreach(var player in ija.Value.GetPlayerList())
									{
										if(player.Value == sIRCMessage.Nick)
										{
											sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "{0}: Te már játékban vagy itt: {1}", sIRCMessage.Nick, ija.Key);
											return;
										}
									}
								}
							}

							IJAList[channel].Join(sIRCMessage.Nick);
							break;
						}
						case "!leave":
						{
							if(sIRCMessage.Info.Length < 5)
							{
								IJAList[channel].Leave(sIRCMessage.Nick);
								return;
							}

							if(IJAList[channel].GetOwner() == sIRCMessage.Nick)
							{
								if(!IJAList[channel].GetKillerList().ContainsKey(sIRCMessage.Info[4].ToLower()) &&
									!IJAList[channel].GetDetectiveList().ContainsKey(sIRCMessage.Info[4].ToLower()) &&
									!IJAList[channel].GetNormalList().ContainsKey(sIRCMessage.Info[4].ToLower()) &&
									!IJAList[channel].GetPlayerList().ContainsValue(sIRCMessage.Info[4]))
									sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "{0}: Kit akarsz kiléptetni?", sIRCMessage.Nick);
								else
									IJAList[channel].Leave(sIRCMessage.Info[4], sIRCMessage.Nick);
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

							IJAList[channel].Kill(sIRCMessage.Info[4], sIRCMessage.Nick);
							break;
						}
						case "!lynch":
						{
							if(sIRCMessage.Info.Length < 5)
							{
								sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "{0}: Kit akarsz lincselni?", sIRCMessage.Nick);
								return;
							}

							IJAList[channel].Lynch(sIRCMessage.Info[4], sIRCMessage.Nick, sIRCMessage.Channel);
							break;
						}
						case "!rescue":
						{
							if(sIRCMessage.Info.Length < 5)
							{
								sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Kit akarsz megmenteni?");
								return;
							}

							IJAList[channel].Rescue(sIRCMessage.Info[4], sIRCMessage.Nick);
							break;
						}
						case "!see":
						{
							if(sIRCMessage.Info.Length < 5)
							{
								sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Kit akarsz kikérdezni?");
								return;
							}

							IJAList[channel].See(sIRCMessage.Info[4], sIRCMessage.Nick);
							break;
						}
						case "!gameover":
							break;
						case "!end":
						{
							if(IJAList[channel].GetOwner() == sIRCMessage.Nick || IJAList[channel].GetOwner() == string.Empty ||
								IsAdmin(sIRCMessage.Nick))
							{
								sSender.Mode(channel, "-m");

								if(IJAList[channel].Started)
								{
									foreach(var end in IJAList[channel].GetPlayerList())
										sSender.Mode(sIRCMessage.Channel, "-v", end.Value);

									IJAList[channel].StopThread();
									sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "A játék befejeződött.");

									if(IJAList[channel].GetPlayers() < 8)
										sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "*** A gyilkos 4{0} volt, a nyomozó 4{1}, az orvos pedig nem volt. Mindenki más hétköznapi civil volt.", IJAList[channel].GetKiller(), IJAList[channel].GetDetective());
									else
										sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "*** A gyilkos 4{0} volt, a nyomozó 4{1}, az orvos pedig 4{2}. Mindenki más hétköznapi civil volt.", IJAList[channel].GetKiller(), IJAList[channel].GetDetective(), IJAList[channel].GetDoctor());
								}
								else
								{
									foreach(var end in IJAList[channel].GetPlayerList())
										sSender.Mode(sIRCMessage.Channel, "-v", end.Value);

									IJAList[channel].StopThread();
									sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "A játék befejeződött.");
								}
							}
							else
								sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Sajnálom, de csak {0}, a játék indítója vethet véget a játéknak!", IJAList[channel].GetOwner());
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

			foreach(var ija in GameAddon.IJAList)
			{
				if(!ija.Value.Running)
					continue;

				if(ija.Key != sIRCMessage.Channel.ToLower())
					continue;

				foreach(var player in ija.Value.GetPlayerList())
				{
					if(player.Value == sIRCMessage.Nick)
					{
						ija.Value.Leave(sIRCMessage.Nick);
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

			foreach(var ija in GameAddon.IJAList)
			{
				if(!ija.Value.Running)
					continue;

				if(ija.Key != sIRCMessage.Channel.ToLower())
					continue;

				foreach(var player in ija.Value.GetPlayerList())
				{
					if(player.Value == sIRCMessage.Info[3])
					{
						ija.Value.Leave(sIRCMessage.Info[3]);
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
			foreach(var function in GameChannelFunction)
			{
				SchumixBase.DManager.Update("channel", string.Format("Functions = '{0}'", function.Value), string.Format("Channel = '{0}'", function.Key));
				sChannelInfo.ChannelFunctionReload();
				SchumixBase.DManager.Update("channel", string.Format("Functions = '{0}'", sChannelInfo.ChannelFunctions("gamecommands", "off", function.Key)), string.Format("Channel = '{0}'", function.Key));
				sChannelInfo.ChannelFunctionReload();
			}

			foreach(var mlist in MaffiaList)
			{
				foreach(var player in mlist.Value.GetPlayerList())
					sSender.Mode(mlist.Key, "-v", player.Value);

				sSender.Mode(mlist.Key, "-m");
			}

			foreach(var ilist in IJAList)
			{
				foreach(var player in ilist.Value.GetPlayerList())
					sSender.Mode(ilist.Key, "-v", player.Value);

				sSender.Mode(ilist.Key, "-m");
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