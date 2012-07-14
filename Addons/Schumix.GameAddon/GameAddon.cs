/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Megax <http://www.megaxx.info/>
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
using Schumix.Irc.Ignore;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using Schumix.GameAddon.Commands;
using Schumix.GameAddon.MaffiaGames;

namespace Schumix.GameAddon
{
	class GameAddon : GameCommand, ISchumixAddon
	{
		public static readonly Dictionary<string, string> GameChannelFunction = new Dictionary<string, string>();
		public static readonly Dictionary<string, MaffiaGame> MaffiaList = new Dictionary<string, MaffiaGame>();
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly IgnoreNickName sIgnoreNickName = Singleton<IgnoreNickName>.Instance;
		private readonly ChannelInfo sChannelInfo = Singleton<ChannelInfo>.Instance;
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;
		private readonly NickInfo sNickInfo = Singleton<NickInfo>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private readonly Sender sSender = Singleton<Sender>.Instance;

		public void Setup()
		{
			CleanFunctions();
			sIrcBase.IrcRegisterHandler("PRIVMSG", HandlePrivmsg);
			sIrcBase.IrcRegisterHandler("PART",    HandleLeft);
			sIrcBase.IrcRegisterHandler("KICK",    HandleKick);
			sIrcBase.IrcRegisterHandler("QUIT",    HandleQuit);
			sIrcBase.IrcRegisterHandler("NICK",    HandleNewNick);
			sIrcBase.IrcRegisterHandler("MODE",    HandleMode);
			InitIrcCommand();
			Console.CancelKeyPress += (sender, e) => { Clean(); };
			AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) => { Clean(); };
		}

		public void Destroy()
		{
			sIrcBase.IrcRemoveHandler("PRIVMSG",   HandlePrivmsg);
			sIrcBase.IrcRemoveHandler("PART",      HandleLeft);
			sIrcBase.IrcRemoveHandler("KICK",      HandleKick);
			sIrcBase.IrcRemoveHandler("QUIT",      HandleQuit);
			sIrcBase.IrcRemoveHandler("NICK",      HandleNewNick);
			sIrcBase.IrcRemoveHandler("MODE",      HandleMode);
			RemoveIrcCommand();
			Clean();
		}

		public int Reload(string RName, string SName = "")
		{
			try
			{
				switch(RName.ToLower())
				{
					case "command":
						InitIrcCommand();
						RemoveIrcCommand();
						return 1;
				}
			}
			catch(Exception e)
			{
				Log.Error("GameAddon", "Reload: " + sLConsole.Exception("Error"), e.Message);
				return 0;
			}

			return -1;
		}

		private void InitIrcCommand()
		{
			sIrcBase.SchumixRegisterHandler("game", HandleGame);
		}

		private void RemoveIrcCommand()
		{
			sIrcBase.SchumixRemoveHandler("game",   HandleGame);
		}

		private void HandlePrivmsg(IRCMessage sIRCMessage)
		{
			if(sIgnoreNickName.IsIgnore(sIRCMessage.Nick))
				return;

			if(sChannelInfo.FSelect(IFunctions.Gamecommands) || sIRCMessage.Channel.Substring(0, 1) != "#")
			{
				if(!sChannelInfo.FSelect(IChannelFunctions.Gamecommands, sIRCMessage.Channel) && sIRCMessage.Channel.Substring(0, 1) == "#")
					return;

				if(sIRCMessage.Channel.Length >= 1 && sIRCMessage.Channel.Substring(0, 1) != "#")
					sIRCMessage.Channel = sIRCMessage.Nick;

				string channel = sIRCMessage.Channel.ToLower();

				if(MaffiaList.ContainsKey(channel) || sIRCMessage.Channel.Substring(0, 1) != "#")
				{
					if(sIRCMessage.Info.Length < 4)
						return;

					bool nick = false;
	
					if(sIRCMessage.Channel.Substring(0, 1) != "#")
					{
						foreach(var maffia in MaffiaList)
						{
							if(maffia.Value.Started)
							{
								foreach(var player in maffia.Value.GetPlayerFList())
								{
									if(player.Key == sIRCMessage.Nick.ToLower())
									{
										nick = true;
										channel = maffia.Key;
										break;
									}
								}
							}
							else
							{
								foreach(var player in maffia.Value.GetPlayerList())
								{
									if(player.Value.ToLower() == sIRCMessage.Nick.ToLower())
									{
										nick = true;
										channel = maffia.Key;
										break;
									}
								}
							}
						}
					}
					else
						nick = true;

					if(!nick)
						return;

					if(MaffiaList[channel].GetOwner() == sIRCMessage.Nick)
						MaffiaList[channel].NewOwnerTime();

					sIRCMessage.Info[3] = sIRCMessage.Info[3].Remove(0, 1, SchumixBase.Colon);
					if((sIRCMessage.Info[3].Length > 0 && sIRCMessage.Info[3].Substring(0, 1) != "!") || sIRCMessage.Info[3].Length == 0)
						return;

					switch(sIRCMessage.Info[3].ToLower())
					{
						case "!start":
						{
							if(MaffiaList[channel].GetOwner() == sIRCMessage.Nick || MaffiaList[channel].GetOwner() == string.Empty ||
								IsAdmin(sIRCMessage.Nick, sIRCMessage.Host))
								MaffiaList[channel].Start();
							else
								sSendMessage.SendCMPrivmsge(sIRCMessage.ServerName, sIRCMessage.Channel, "{0}: A játékot {1} indította!", sIRCMessage.Nick, MaffiaList[channel].GetOwner());
							break;
						}
						case "!set":
						{
							if(MaffiaList[channel].GetOwner() == sIRCMessage.Nick || MaffiaList[channel].GetOwner() == string.Empty ||
								IsAdmin(sIRCMessage.Nick, sIRCMessage.Host))
							{
								if(MaffiaList[channel].Started)
								{
									sSendMessage.SendCMPrivmsge(sIRCMessage.ServerName, sIRCMessage.Channel, "{0}: Sajnálom de a játék már fut!", sIRCMessage.Nick);
									return;
								}

								if(sIRCMessage.Info.Length < 5)
								{
									sSendMessage.SendCMPrivmsge(sIRCMessage.ServerName, sIRCMessage.Channel, "{0}: Nincs megadva az állítandó paraméter!", sIRCMessage.Nick);
									return;
								}

								if(sIRCMessage.Info[4].ToLower() == "info")
								{
									if(MaffiaList[channel].NoLynch)
										sSendMessage.SendCMPrivmsge(sIRCMessage.ServerName, sIRCMessage.Channel, "No lynch: on");
									else
										sSendMessage.SendCMPrivmsge(sIRCMessage.ServerName, sIRCMessage.Channel, "No lynch: off");

									if(MaffiaList[channel].NoVoice)
										sSendMessage.SendCMPrivmsge(sIRCMessage.ServerName, sIRCMessage.Channel, "Nincs rang este: on");
									else
										sSendMessage.SendCMPrivmsge(sIRCMessage.ServerName, sIRCMessage.Channel, "Nincs rang este: off");
									return;
								}
								else if(sIRCMessage.Info[4].ToLower() == "nolynch")
								{
									if(sIRCMessage.Info.Length < 6)
									{
										sSendMessage.SendCMPrivmsge(sIRCMessage.ServerName, sIRCMessage.Channel, "{0}: Nincs megadva hogy on vagy off legyen-e ez a beállítás!", sIRCMessage.Nick);
										return;
									}

									string status = sIRCMessage.Info[5].ToLower();

									if(status == SchumixBase.On || status == SchumixBase.Off)
									{
										if(status == SchumixBase.On)
											MaffiaList[channel].NoLynch = true;
										else if(status == SchumixBase.Off)
											MaffiaList[channel].NoLynch = false;

										sSendMessage.SendCMPrivmsge(sIRCMessage.ServerName, sIRCMessage.Channel, "{0}: A beállítás módosítva lett.", sIRCMessage.Nick);
									}
									else
										sSendMessage.SendCMPrivmsge(sIRCMessage.ServerName, sIRCMessage.Channel, "{0}: Nem on illetve off kifejezés lett megadva!", sIRCMessage.Nick);
								}
								else if(sIRCMessage.Info[4].ToLower() == "night")
								{
									if(sIRCMessage.Info.Length < 6)
									{
										sSendMessage.SendCMPrivmsge(sIRCMessage.ServerName, sIRCMessage.Channel, "{0}: Nincs megadva az állítandó paraméter!", sIRCMessage.Nick);
										return;
									}

									if(sIRCMessage.Info[5].ToLower() == "novoice")
									{
										if(sIRCMessage.Info.Length < 7)
										{
											sSendMessage.SendCMPrivmsge(sIRCMessage.ServerName, sIRCMessage.Channel, "{0}: Nincs megadva hogy on vagy off legyen-e ez a beállítás!", sIRCMessage.Nick);
											return;
										}

										string status = sIRCMessage.Info[6].ToLower();

										if(status == SchumixBase.On || status == SchumixBase.Off)
										{
											if(status == SchumixBase.On)
												MaffiaList[channel].NoVoice = true;
											else if(status == SchumixBase.Off)
												MaffiaList[channel].NoVoice = false;
	
											sSendMessage.SendCMPrivmsge(sIRCMessage.ServerName, sIRCMessage.Channel, "{0}: A beállítás módosítva lett.", sIRCMessage.Nick);
										}
										else
											sSendMessage.SendCMPrivmsge(sIRCMessage.ServerName, sIRCMessage.Channel, "{0}: Nem on illetve off kifejezés lett megadva!", sIRCMessage.Nick);
									}
								}
							}
							else
								sSendMessage.SendCMPrivmsge(sIRCMessage.ServerName, sIRCMessage.Channel, "{0}: Sajnálom, de csak {1}, a játék indítója állíthat a játék menetén!", sIRCMessage.Nick, MaffiaList[channel].GetOwner());
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
											sSendMessage.SendCMPrivmsge(sIRCMessage.ServerName, sIRCMessage.Channel, "{0}: Te már játékban vagy itt: {1}", sIRCMessage.Nick, maffia.Key);
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
									sSendMessage.SendCMPrivmsge(sIRCMessage.ServerName, sIRCMessage.Channel, "{0}: Kit akarsz kiléptetni?", sIRCMessage.Nick);
								else
									MaffiaList[channel].Leave(sIRCMessage.Info[4], sIRCMessage.Nick);
							}
							else
								sSendMessage.SendCMPrivmsge(sIRCMessage.ServerName, sIRCMessage.Channel, "{0}: Nem te indítottad a játékot!", sIRCMessage.Nick);
							break;
						}
						case "!kill":
						{
							if(sIRCMessage.Info.Length < 5)
							{
								sSendMessage.SendCMPrivmsge(sIRCMessage.ServerName, sIRCMessage.Channel, "Kit akarsz megölni?");
								return;
							}

							MaffiaList[channel].Kill(sIRCMessage.Info[4], sIRCMessage.Nick);
							break;
						}
						case "!lynch":
						{
							if(sIRCMessage.Info.Length < 5)
							{
								sSendMessage.SendCMPrivmsge(sIRCMessage.ServerName, sIRCMessage.Channel, "{0}: Kit akarsz lincselni?", sIRCMessage.Nick);
								return;
							}

							MaffiaList[channel].Lynch(sIRCMessage.Info[4], sIRCMessage.Nick, sIRCMessage.Channel);
							break;
						}
						case "!rescue":
						{
							if(sIRCMessage.Info.Length < 5)
							{
								sSendMessage.SendCMPrivmsge(sIRCMessage.ServerName, sIRCMessage.Channel, "Kit akarsz megmenteni?");
								return;
							}

							MaffiaList[channel].Rescue(sIRCMessage.Info[4], sIRCMessage.Nick);
							break;
						}
						case "!see":
						{
							if(sIRCMessage.Info.Length < 5)
							{
								sSendMessage.SendCMPrivmsge(sIRCMessage.ServerName, sIRCMessage.Channel, "Kit akarsz kikérdezni?");
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
								IsAdmin(sIRCMessage.Nick, sIRCMessage.Host))
							{
								if(MaffiaList[channel].Started)
								{
									MaffiaList[channel].RemoveRanks();
									MaffiaList[channel].EndGameText();
									MaffiaList[channel].EndText();
									MaffiaList[channel].StopThread();
								}
								else
								{
									MaffiaList[channel].RemoveRanks();
									MaffiaList[channel].StopThread();
									MaffiaList[channel].EndGameText();
								}
							}
							else
								sSendMessage.SendCMPrivmsge(sIRCMessage.ServerName, sIRCMessage.Channel, "{0}: Sajnálom, de csak {1}, a játék indítója vethet véget a játéknak!", sIRCMessage.Nick, MaffiaList[channel].GetOwner());
							break;
						}
						default:
							sSendMessage.SendCMPrivmsge(sIRCMessage.ServerName, sIRCMessage.Channel, "{0}: Nem létezik ilyen parancs!", sIRCMessage.Nick);
							break;
					}
				}
			}
		}

		private void HandleLeft(IRCMessage sIRCMessage)
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

		private void HandleKick(IRCMessage sIRCMessage)
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

		private void HandleQuit(IRCMessage sIRCMessage)
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

		private void HandleNewNick(IRCMessage sIRCMessage)
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

		private void HandleMode(IRCMessage sIRCMessage)
		{
			if(sIRCMessage.Info.Length < 5)
				return;

			if(!sIRCMessage.Info[3].Contains("v") && !sIRCMessage.Info[3].Contains("-"))
				return;

			if(sNickInfo.NickStorage.ToLower() == sIRCMessage.Nick.ToLower())
				return;

			sIRCMessage.Info[3] = sIRCMessage.Info[3].Remove(0, 1, "-");

			foreach(var maffia in GameAddon.MaffiaList)
			{
				if(!maffia.Value.Running)
					continue;

				foreach(var player in maffia.Value.GetPlayerList())
				{
					if(player.Value == sIRCMessage.Info[4] && sIRCMessage.Info[3].Substring(0, 1) == "v")
					{
						sSender.Modee(sIRCMessage.ServerName, maffia.Key, "+v", sIRCMessage.Info[4]);
						continue;
					}

					if(sIRCMessage.Info.Length >= 6 && player.Value == sIRCMessage.Info[5] && sIRCMessage.Info[3].Substring(1) == "v")
					{
						sSender.Modee(sIRCMessage.ServerName, maffia.Key, "+v", sIRCMessage.Info[5]);
						continue;
					}

					if(sIRCMessage.Info.Length >= 7 && player.Value == sIRCMessage.Info[6] && sIRCMessage.Info[3].Substring(2) == "v")
					{
						sSender.Modee(sIRCMessage.ServerName, maffia.Key, "+v", sIRCMessage.Info[6]);
						continue;
					}

					if(sIRCMessage.Info.Length >= 8 && player.Value == sIRCMessage.Info[7] && sIRCMessage.Info[3].Substring(3) == "v")
					{
						sSender.Modee(sIRCMessage.ServerName, maffia.Key, "+v", sIRCMessage.Info[7]);
						continue;
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

		private void CleanFunctions()
		{
			sChannelInfo.ChannelFunctionsReload();
			var list = new List<string>();

			foreach(var function in sChannelInfo.CFunction)
			{
				foreach(var comma in function.Value.Split(SchumixBase.Comma))
				{
					if(comma == string.Empty)
						continue;

					string[] point = comma.Split(SchumixBase.Colon);

					if(point[0] == IChannelFunctions.Gamecommands.ToString().ToLower() && point[1] == SchumixBase.On)
						list.Add(function.Key);
				}
			}

			foreach(var channel in list)
			{
				SchumixBase.DManager.Update("channel", string.Format("Functions = '{0}'", sChannelInfo.ChannelFunctions("commands", SchumixBase.On, channel)), string.Format("Channel = '{0}'", channel));
				sChannelInfo.ChannelFunctionsReload();
				SchumixBase.DManager.Update("channel", string.Format("Functions = '{0}'", sChannelInfo.ChannelFunctions("gamecommands", SchumixBase.Off, channel)), string.Format("Channel = '{0}'", channel));
				sChannelInfo.ChannelFunctionsReload();
			}

			list.Clear();
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