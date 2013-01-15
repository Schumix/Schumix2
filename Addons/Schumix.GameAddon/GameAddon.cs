/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013 Schumix Team <http://schumix.eu/>
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
using Schumix.Api;
using Schumix.Api.Irc;
using Schumix.Api.Functions;
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
	class GameAddon : ISchumixAddon
	{
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private GameCommand sGameCommand;
		private string _servername;
		private bool start = false;

		public void Setup(string ServerName)
		{
			_servername = ServerName;
			sGameCommand = new GameCommand(ServerName);
			sGameCommand.sGC = sGameCommand;
			sIrcBase.Networks[ServerName].IrcRegisterHandler("PRIVMSG", HandlePrivmsg);
			sIrcBase.Networks[ServerName].IrcRegisterHandler("PART",    HandleLeft);
			sIrcBase.Networks[ServerName].IrcRegisterHandler("KICK",    HandleKick);
			sIrcBase.Networks[ServerName].IrcRegisterHandler("QUIT",    HandleQuit);
			sIrcBase.Networks[ServerName].IrcRegisterHandler("NICK",    HandleNewNick);
			sIrcBase.Networks[ServerName].IrcRegisterHandler("MODE",    HandleMode);
			InitIrcCommand();
			SchumixBase.sCleanManager.CDatabase.CleanTable("maffiagame");
			SchumixBase.DManager.Update("maffiagame", string.Format("ServerName = '{0}'", ServerName), string.Format("ServerId = '{0}'", IRCConfig.List[ServerName].ServerId));
			Console.CancelKeyPress += (sender, e) => { Clean(); };
			AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) => { Clean(); };
		}

		public void Destroy()
		{
			sIrcBase.Networks[_servername].IrcRemoveHandler("PRIVMSG",  HandlePrivmsg);
			sIrcBase.Networks[_servername].IrcRemoveHandler("PART",     HandleLeft);
			sIrcBase.Networks[_servername].IrcRemoveHandler("KICK",     HandleKick);
			sIrcBase.Networks[_servername].IrcRemoveHandler("QUIT",     HandleQuit);
			sIrcBase.Networks[_servername].IrcRemoveHandler("NICK",     HandleNewNick);
			sIrcBase.Networks[_servername].IrcRemoveHandler("MODE",     HandleMode);
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
			sIrcBase.Networks[_servername].SchumixRegisterHandler("game", sGameCommand.HandleGame);
		}

		private void RemoveIrcCommand()
		{
			sIrcBase.Networks[_servername].SchumixRemoveHandler("game",   sGameCommand.HandleGame);
		}

		private void HandlePrivmsg(IRCMessage sIRCMessage)
		{
			if(!start)
			{
				CleanFunctions();
				start = true;
			}

			var sIgnoreNickName = sIrcBase.Networks[sIRCMessage.ServerName].sIgnoreNickName;
			var sMyChannelInfo = sIrcBase.Networks[sIRCMessage.ServerName].sMyChannelInfo;
			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;

			if(sIgnoreNickName.IsIgnore(sIRCMessage.Nick))
				return;

			if(sMyChannelInfo.FSelect(IFunctions.Gamecommands) || !sUtilities.IsChannel(sIRCMessage.Channel))
			{
				if(!sMyChannelInfo.FSelect(IChannelFunctions.Gamecommands, sIRCMessage.Channel) && sUtilities.IsChannel(sIRCMessage.Channel))
					return;

				if(!sUtilities.IsChannel(sIRCMessage.Channel))
					sIRCMessage.Channel = sIRCMessage.Nick;

				string channel = sIRCMessage.Channel.ToLower();

				if(sGameCommand.MaffiaList.ContainsKey(channel) || !sUtilities.IsChannel(sIRCMessage.Channel))
				{
					if(sIRCMessage.Info.Length < 4)
						return;

					bool nick = false;

					if(!sUtilities.IsChannel(sIRCMessage.Channel))
					{
						foreach(var maffia in sGameCommand.MaffiaList)
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

					if(sGameCommand.MaffiaList[channel].GetOwner() == sIRCMessage.Nick)
						sGameCommand.MaffiaList[channel].NewOwnerTime();

					sIRCMessage.Info[3] = sIRCMessage.Info[3].Remove(0, 1, SchumixBase.Colon);
					if((sIRCMessage.Info[3].Length > 0 && sIRCMessage.Info[3].Substring(0, 1) != "!") || sIRCMessage.Info[3].Length == 0)
						return;

					switch(sIRCMessage.Info[3].ToLower())
					{
						case "!start":
						{
							var text = sLManager.GetCommandTexts("maffiagame/basecommand/start", channel, sIRCMessage.ServerName);
							if(text.Length < 8)
							{
								sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(channel, sIRCMessage.ServerName)));
								return;
							}

							if(sGameCommand.MaffiaList[channel].GetOwner() == sIRCMessage.Nick || sGameCommand.MaffiaList[channel].GetOwner() == string.Empty ||
								sGameCommand.IsAdmin(sIRCMessage.Nick, sIRCMessage.Host))
								sGameCommand.MaffiaList[channel].Start();
							else
								sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0], sGameCommand.MaffiaList[channel].DisableHl(sIRCMessage.Nick), sGameCommand.MaffiaList[channel].GetOwner());
							break;
						}
						case "!set":
						{
							var text = sLManager.GetCommandTexts("maffiagame/basecommand/set", channel, sIRCMessage.ServerName);
							if(text.Length < 3)
							{
								sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(channel, sIRCMessage.ServerName)));
								return;
							}

							if(sGameCommand.MaffiaList[channel].GetOwner() == sIRCMessage.Nick || sGameCommand.MaffiaList[channel].GetOwner() == string.Empty ||
								sGameCommand.IsAdmin(sIRCMessage.Nick, sIRCMessage.Host))
							{
								if(sGameCommand.MaffiaList[channel].Started)
								{
									sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0], sGameCommand.MaffiaList[channel].DisableHl(sIRCMessage.Nick));
									return;
								}

								if(sIRCMessage.Info.Length < 5)
								{
									sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1], sGameCommand.MaffiaList[channel].DisableHl(sIRCMessage.Nick));
									return;
								}

								if(sIRCMessage.Info[4].ToLower() == "info")
								{
									var text2 = sLManager.GetCommandTexts("maffiagame/basecommand/set/info", channel, sIRCMessage.ServerName);
									if(text.Length < 4)
									{
										sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(channel, sIRCMessage.ServerName)));
										return;
									}

									if(sGameCommand.MaffiaList[channel].NoLynch)
										sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text2[0]);
									else
										sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text2[1]);

									if(sGameCommand.MaffiaList[channel].NoVoice)
										sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text2[2]);
									else
										sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text2[3]);
									return;
								}
								else if(sIRCMessage.Info[4].ToLower() == "nolynch")
								{
									var text2 = sLManager.GetCommandTexts("maffiagame/basecommand/set/nolynch", channel, sIRCMessage.ServerName);
									if(text.Length < 4)
									{
										sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(channel, sIRCMessage.ServerName)));
										return;
									}

									if(sIRCMessage.Info.Length < 6)
									{
										sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text2[0], sGameCommand.MaffiaList[channel].DisableHl(sIRCMessage.Nick));
										return;
									}

									string status = sIRCMessage.Info[5].ToLower();

									if(status == SchumixBase.On || status == SchumixBase.Off)
									{
										if(status == SchumixBase.On)
											sGameCommand.MaffiaList[channel].NoLynch = true;
										else if(status == SchumixBase.Off)
											sGameCommand.MaffiaList[channel].NoLynch = false;

										sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text2[1], sGameCommand.MaffiaList[channel].DisableHl(sIRCMessage.Nick));
									}
									else
										sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text2[2], sGameCommand.MaffiaList[channel].DisableHl(sIRCMessage.Nick));
								}
								else if(sIRCMessage.Info[4].ToLower() == "night")
								{
									if(sIRCMessage.Info.Length < 6)
									{
										sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("maffiagame/basecommand/set/night", channel, sIRCMessage.ServerName), sIRCMessage.Nick);
										return;
									}

									if(sIRCMessage.Info[5].ToLower() == "novoice")
									{
										var text2 = sLManager.GetCommandTexts("maffiagame/basecommand/set/night/novoice", channel, sIRCMessage.ServerName);
										if(text.Length < 3)
										{
											sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(channel, sIRCMessage.ServerName)));
											return;
										}

										if(sIRCMessage.Info.Length < 7)
										{
											sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text2[0], sGameCommand.MaffiaList[channel].DisableHl(sIRCMessage.Nick));
											return;
										}

										string status = sIRCMessage.Info[6].ToLower();

										if(status == SchumixBase.On || status == SchumixBase.Off)
										{
											if(status == SchumixBase.On)
												sGameCommand.MaffiaList[channel].NoVoice = true;
											else if(status == SchumixBase.Off)
												sGameCommand.MaffiaList[channel].NoVoice = false;
	
											sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text2[1], sGameCommand.MaffiaList[channel].DisableHl(sIRCMessage.Nick));
										}
										else
											sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text2[2], sGameCommand.MaffiaList[channel].DisableHl(sIRCMessage.Nick));
									}
								}
							}
							else
								sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[2], sGameCommand.MaffiaList[channel].DisableHl(sIRCMessage.Nick), sGameCommand.MaffiaList[channel].GetOwner());
							break;
						}
						case "!stats":
						{
							sGameCommand.MaffiaList[channel].Stats();
							break;
						}
						case "!join":
						{
							var text = sLManager.GetCommandTexts("maffiagame/basecommand/join", channel, sIRCMessage.ServerName);
							if(text.Length < 6)
							{
								sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(channel, sIRCMessage.ServerName)));
								return;
							}

							foreach(var maffia in sGameCommand.MaffiaList)
							{
								if(sIRCMessage.Channel.ToLower() != maffia.Key)
								{
									foreach(var player in maffia.Value.GetPlayerList())
									{
										if(player.Value == sIRCMessage.Nick)
										{
											sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0], sGameCommand.MaffiaList[channel].DisableHl(sIRCMessage.Nick), maffia.Key);
											return;
										}
									}
								}
							}

							sGameCommand.MaffiaList[channel].Join(sIRCMessage.Nick);
							break;
						}
						case "!leave":
						{
							var text = sLManager.GetCommandTexts("maffiagame/basecommand/leave", channel, sIRCMessage.ServerName);
							if(text.Length < 11)
							{
								sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(channel, sIRCMessage.ServerName)));
								return;
							}

							if(sIRCMessage.Info.Length < 5)
							{
								sGameCommand.MaffiaList[channel].Leave(sIRCMessage.Nick);
								return;
							}

							if(sGameCommand.MaffiaList[channel].GetOwner() == sIRCMessage.Nick)
							{
								if(!sGameCommand.MaffiaList[channel].GetKillerList().ContainsKey(sIRCMessage.Info[4].ToLower()) &&
									!sGameCommand.MaffiaList[channel].GetDetectiveList().ContainsKey(sIRCMessage.Info[4].ToLower()) &&
									!sGameCommand.MaffiaList[channel].GetNormalList().ContainsKey(sIRCMessage.Info[4].ToLower()) &&
									!sGameCommand.MaffiaList[channel].GetPlayerList().ContainsValue(sIRCMessage.Info[4]))
									sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0], sGameCommand.MaffiaList[channel].DisableHl(sIRCMessage.Nick));
								else
									sGameCommand.MaffiaList[channel].Leave(sIRCMessage.Info[4], sIRCMessage.Nick);
							}
							else
								sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[1], sGameCommand.MaffiaList[channel].DisableHl(sIRCMessage.Nick));
							break;
						}
						case "!kill":
						{
							var text = sLManager.GetCommandTexts("maffiagame/basecommand/kill", channel, sIRCMessage.ServerName);
							if(text.Length < 7)
							{
								sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(channel, sIRCMessage.ServerName)));
								return;
							}

							if(sIRCMessage.Info.Length < 5)
							{
								sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
								return;
							}

							sGameCommand.MaffiaList[channel].Kill(sIRCMessage.Info[4], sIRCMessage.Nick);
							break;
						}
						case "!lynch":
						{
							var text = sLManager.GetCommandTexts("maffiagame/basecommand/lynch", channel, sIRCMessage.ServerName);
							if(text.Length < 14)
							{
								sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(channel, sIRCMessage.ServerName)));
								return;
							}

							if(sIRCMessage.Info.Length < 5)
							{
								sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0], sGameCommand.MaffiaList[channel].DisableHl(sIRCMessage.Nick));
								return;
							}

							sGameCommand.MaffiaList[channel].Lynch(sIRCMessage.Info[4], sIRCMessage.Nick, sIRCMessage.Channel);
							break;
						}
						case "!rescue":
						{
							var text = sLManager.GetCommandTexts("maffiagame/basecommand/rescue", channel, sIRCMessage.ServerName);
							if(text.Length < 7)
							{
								sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(channel, sIRCMessage.ServerName)));
								return;
							}

							if(sIRCMessage.Info.Length < 5)
							{
								sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
								return;
							}

							sGameCommand.MaffiaList[channel].Rescue(sIRCMessage.Info[4], sIRCMessage.Nick);
							break;
						}
						case "!see":
						{
							var text = sLManager.GetCommandTexts("maffiagame/basecommand/see", channel, sIRCMessage.ServerName);
							if(text.Length < 8)
							{
								sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(channel, sIRCMessage.ServerName)));
								return;
							}

							if(sIRCMessage.Info.Length < 5)
							{
								sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, text[0]);
								return;
							}

							sGameCommand.MaffiaList[channel].See(sIRCMessage.Info[4], sIRCMessage.Nick);
							break;
						}
						case "!gameover":
						{
							sGameCommand.MaffiaList[channel].GameOver(sIRCMessage.Nick);
							break;
						}
						case "!end":
						{
							if(sGameCommand.MaffiaList[channel].GetOwner() == sIRCMessage.Nick ||
								sGameCommand.MaffiaList[channel].GetOwner() == string.Empty ||
								sGameCommand.IsAdmin(sIRCMessage.Nick, sIRCMessage.Host))
							{
								if(sGameCommand.MaffiaList[channel].Started)
								{
									sGameCommand.MaffiaList[channel].RemoveRanks();
									sGameCommand.MaffiaList[channel].EndGameText();
									sGameCommand.MaffiaList[channel].EndText();
									sGameCommand.MaffiaList[channel].StopThread();
								}
								else
								{
									sGameCommand.MaffiaList[channel].RemoveRanks();
									sGameCommand.MaffiaList[channel].StopThread();
									sGameCommand.MaffiaList[channel].EndGameText();
								}
							}
							else
								sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("maffiagame/basecommand/end", channel, sIRCMessage.ServerName), sGameCommand.MaffiaList[channel].DisableHl(sIRCMessage.Nick), sGameCommand.MaffiaList[channel].GetOwner());
							break;
						}
						default:
							if(sIRCMessage.Info[3].TrimEnd().Length > 1)
								sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, sLManager.GetCommandText("maffiagame/basecommand", channel, sIRCMessage.ServerName), sGameCommand.MaffiaList[channel].DisableHl(sIRCMessage.Nick));
							break;
					}
				}
			}
		}

		private void HandleLeft(IRCMessage sIRCMessage)
		{
			foreach(var maffia in sGameCommand.MaffiaList)
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
			foreach(var maffia in sGameCommand.MaffiaList)
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
			foreach(var maffia in sGameCommand.MaffiaList)
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
			foreach(var maffia in sGameCommand.MaffiaList)
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
			var sMyNickInfo = sIrcBase.Networks[sIRCMessage.ServerName].sMyNickInfo;
			var sSender = sIrcBase.Networks[sIRCMessage.ServerName].sSender;

			if(sIRCMessage.Info.Length < 5)
				return;

			if(!sIRCMessage.Info[3].Contains("v") && !sIRCMessage.Info[3].Contains("-"))
				return;

			if(sMyNickInfo.NickStorage.ToLower() == sIRCMessage.Nick.ToLower())
				return;

			string rank = sIRCMessage.Info[3].Remove(0, 1, "-");

			foreach(var maffia in sGameCommand.MaffiaList)
			{
				if(!maffia.Value.Running)
					continue;

				foreach(var player in maffia.Value.GetPlayerList())
				{
					if(player.Value == sIRCMessage.Info[4] && rank.Substring(1, 1) == "v")
					{
						sSender.Mode(maffia.Key, "+v", sIRCMessage.Info[4]);
						continue;
					}

					if(sIRCMessage.Info.Length >= 6 && player.Value == sIRCMessage.Info[5] && rank.Substring(2).Substring(0, 1) == "v")
					{
						sSender.Mode(maffia.Key, "+v", sIRCMessage.Info[5]);
						continue;
					}

					if(sIRCMessage.Info.Length >= 7 && player.Value == sIRCMessage.Info[6] && rank.Substring(3).Substring(0, 1) == "v")
					{
						sSender.Mode(maffia.Key, "+v", sIRCMessage.Info[6]);
						continue;
					}

					if(sIRCMessage.Info.Length >= 8 && player.Value == sIRCMessage.Info[7] && rank.Substring(4).Substring(0, 1) == "v")
					{
						sSender.Mode(maffia.Key, "+v", sIRCMessage.Info[7]);
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
			foreach(var mlist in sGameCommand.MaffiaList)
			{
				mlist.Value.RemoveRanks();
				mlist.Value.StopThread();
			}

			sGameCommand.GameChannelFunction.Clear();
		}

		private void CleanFunctions()
		{
			var sMyChannelInfo = sIrcBase.Networks[_servername].sMyChannelInfo;
			sMyChannelInfo.ChannelFunctionsReload();
			var list = new List<string>();

			foreach(var function in sMyChannelInfo.CFunction)
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
				SchumixBase.DManager.Update("channels", string.Format("Functions = '{0}'", sMyChannelInfo.ChannelFunctions("commands", SchumixBase.On, channel)), string.Format("Channel = '{0}' And ServerName = '{1}'", channel, _servername));
				sMyChannelInfo.ChannelFunctionsReload();
				SchumixBase.DManager.Update("channels", string.Format("Functions = '{0}'", sMyChannelInfo.ChannelFunctions("gamecommands", SchumixBase.Off, channel)), string.Format("Channel = '{0}' And ServerName = '{1}'", channel, _servername));
				sMyChannelInfo.ChannelFunctionsReload();
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