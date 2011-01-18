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

namespace Schumix.IRC
{
	/// <summary>
	///     Meghatározza, hogy PRIVMSG vagy NOTICE legyen az üzenetküldés módja.
	/// </summary>
	public enum MessageType
	{
		PRIVMSG,
		NOTICE
	};

	public class SendMessage
	{
		private SendMessage() {}

        /// <summary>
        ///     Ez küldi el az üzenetet az chatre.
        /// </summary>
        /// <param name="tipus">
        ///     PRIVMSG : Sima üzenet
		///     NOTICE  : Notice üzenet
        /// </param>
        /// <param name="channel">IRC szoba neve</param>
        /// <param name="uzenet">Maga az üzenet</param>
		public void SendChatMessage(MessageType tipus, string channel, string uzenet)
		{
			string buffer = "";

			if(tipus == MessageType.PRIVMSG)
				buffer = "PRIVMSG";
			else if(tipus == MessageType.NOTICE)
				buffer = "NOTICE";

			buffer += " " + channel + " :" + uzenet;
			Network.writer.WriteLine(buffer);
		}
	}
}