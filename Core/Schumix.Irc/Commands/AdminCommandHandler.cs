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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections.Generic;
using Schumix.API;
using Schumix.API.Irc;
using Schumix.API.Delegate;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;

namespace Schumix.Irc.Commands
{
	public abstract partial class CommandHandler
	{
		private readonly object Lock = new object();

		protected void HandlePlugin(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Administrator))
				return;

			if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "load")
			{
				var text = sLManager.GetCommandTexts("plugin/load", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 3)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}

				if(sAddonManager.IsLoadingAddons())
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[2]);
					return;
				}

				if(sAddonManager.LoadPluginsFromDirectory(AddonsConfig.Directory))
				{
					foreach(var nw in sIrcBase.Networks)
					{
						var asms = sAddonManager.Addons[nw.Key].Assemblies.ToDictionary(v => v.Key, v => v.Value);
						Parallel.ForEach(asms, asm =>
						{
							var types = asm.Value.GetTypes();
							Parallel.ForEach(types, type =>
							{
								var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
								Parallel.ForEach(methods, method =>
								{
									foreach(var attribute in Attribute.GetCustomAttributes(method))
									{
										if(attribute.IsOfType(typeof(IrcCommandAttribute)))
										{
											var attr = (IrcCommandAttribute)attribute;
											lock(Lock)
											{
												var del = Delegate.CreateDelegate(typeof(IRCDelegate), method) as IRCDelegate;
												sIrcBase.Networks[nw.Key].IrcRegisterHandler(attr.Command, del);
											}
										}

										if(attribute.IsOfType(typeof(SchumixCommandAttribute)))
										{
											var attr = (SchumixCommandAttribute)attribute;
											lock(Lock)
											{
												var del = Delegate.CreateDelegate(typeof(CommandDelegate), method) as CommandDelegate;
												sIrcBase.Networks[nw.Key].SchumixRegisterHandler(attr.Command, del, attr.Permission);
											}
										}
									}
								});
							});
						});
					}

					sSendMessage.SendChatMessage(sIRCMessage, text[0]);
				}
				else
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
			}
			else if(sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4].ToLower() == "unload")
			{
				var text = sLManager.GetCommandTexts("plugin/unload", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 3)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}

				if(!sAddonManager.IsLoadingAddons())
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[2]);
					return;
				}

				if(sAddonManager.UnloadPlugins())
				{
					foreach(var nw in sIrcBase.Networks)
					{
						var asms = sAddonManager.Addons[nw.Key].Assemblies.ToDictionary(v => v.Key, v => v.Value);
						Parallel.ForEach(asms, asm =>
						{
							var types = asm.Value.GetTypes();
							Parallel.ForEach(types, type =>
							{
								var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
								Parallel.ForEach(methods, method =>
								{
									foreach(var attribute in Attribute.GetCustomAttributes(method))
									{
										if(attribute.IsOfType(typeof(IrcCommandAttribute)))
										{
											var attr = (IrcCommandAttribute)attribute;
											lock(Lock)
											{
												var del = Delegate.CreateDelegate(typeof(IRCDelegate), method) as IRCDelegate;
												sIrcBase.Networks[nw.Key].IrcRemoveHandler(attr.Command, del);
											}
										}

										if(attribute.IsOfType(typeof(SchumixCommandAttribute)))
										{
											var attr = (SchumixCommandAttribute)attribute;
											lock(Lock)
											{
												var del = Delegate.CreateDelegate(typeof(CommandDelegate), method) as CommandDelegate;
												sIrcBase.Networks[nw.Key].SchumixRemoveHandler(attr.Command, del);
											}
										}
									}
								});
							});
						});
					}

					sSendMessage.SendChatMessage(sIRCMessage, text[0]);
				}
				else
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
			}
			else
			{
				var text = sLManager.GetCommandTexts("plugin", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 3)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}

				string Plugins = string.Empty;
				string IgnorePlugins = string.Empty;

				foreach(var plugin in sAddonManager.Addons[sIRCMessage.ServerName].Addons)
				{
					if(!sIgnoreAddon.IsIgnore(plugin.Key))
						Plugins += ", " + plugin.Value.Name;
					else
						IgnorePlugins += ", " + plugin.Value.Name;
				}

				if(Plugins != string.Empty)
					sSendMessage.SendChatMessage(sIRCMessage, text[0], Plugins.Remove(0, 2, ", "));

				if(IgnorePlugins != string.Empty)
					sSendMessage.SendChatMessage(sIRCMessage, text[1], IgnorePlugins.Remove(0, 2, ", "));

				if(Plugins == string.Empty && IgnorePlugins == string.Empty)
					sSendMessage.SendChatMessage(sIRCMessage, text[2]);
			}
		}

		protected void HandleReload(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Administrator))
				return;

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			var text = sLManager.GetCommandTexts("reload", sIRCMessage.Channel, sIRCMessage.ServerName);
			if(text.Length < 3)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
				return;
			}

			int i = -1;

			switch(sIRCMessage.Info[4].ToLower())
			{
				case "config":
					new Config(SchumixConfig.ConfigDirectory, SchumixConfig.ConfigFile);
					sIgnoreAddon.RemoveConfig();
					sIgnoreAddon.AddConfig();
					sIgnoreChannel.RemoveConfig();
					sIgnoreChannel.AddConfig();
					sIgnoreNickName.RemoveConfig();
					sIgnoreNickName.AddConfig();
					sIrcBase.Networks[sIRCMessage.ServerName].ReloadMessageHandlerConfig();
					sLConsole.Locale = LocalizationConfig.Locale;
					i = 1;
					break;
			}

			foreach(var plugin in sAddonManager.Addons[sIRCMessage.ServerName].Addons)
			{
				if(!sAddonManager.Addons[sIRCMessage.ServerName].IgnoreAssemblies.ContainsKey(plugin.Key) &&
				   plugin.Value.Reload(sIRCMessage.Info[4].ToLower()) == 1)
					i = 1;
				else if(!sAddonManager.Addons[sIRCMessage.ServerName].IgnoreAssemblies.ContainsKey(plugin.Key) &&
				   plugin.Value.Reload(sIRCMessage.Info[4].ToLower()) == 0)
					i = 0;
			}

			if(i == -1)
				sSendMessage.SendChatMessage(sIRCMessage, text[0]);
			else if(i == 0)
				sSendMessage.SendChatMessage(sIRCMessage, text[1]);
			else if(i == 1)
				sSendMessage.SendChatMessage(sIRCMessage, text[2], sIRCMessage.Info[4]);
		}

		protected void HandleQuit(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Administrator))
				return;

			var text = sLManager.GetCommandTexts("quit", sIRCMessage.Channel, sIRCMessage.ServerName);
			if(text.Length < 2)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
				return;
			}

			sSendMessage.SendChatMessage(sIRCMessage, text[0]);
			SchumixBase.Quit();
			sIrcBase.Shutdown(string.Format(text[1], sIRCMessage.Nick));
		}
	}
}