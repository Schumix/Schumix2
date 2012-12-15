/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Megax <http://www.megaxx.info/>
 * Copyright (C) 2012 Jackneill
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

namespace Schumix.Server.Config
{
	abstract class DefaultConfig
	{
		protected const string d_logfilename             = "Server.log";
		protected const bool d_logdatefilename           = false;
		protected const int d_logmaxfilesize             = 100;
		protected const int d_loglevel                   = 2;
		protected const string d_logdirectory            = "Logs";
		protected const int d_listenerport               = 35220;
		protected const string d_password                = "schumix";
		protected const string d_crashdirectory          = "Dumps";
		protected const string d_locale                  = "enUS";
		protected const bool d_updateenabled             = false;
		protected const string d_updateversion           = "stable";
		protected const string d_updatebranch            = "master";
		protected const string d_updatewebpage           = "https://github.com/megax/Schumix2";
		protected const int d_shutdownmaxmemory          = 100;
		protected const bool d_schumixsenabled           = false;
		protected const int d_schumixsnumber             = 1;
		protected const string d_schumixfile             = "Schumix.yml";
		protected const string d_schumixdirectory        = "Configs";
		protected const string d_schumixconsoleencoding  = "utf-8";
		protected const string d_schumixlocale           = "enUS";
		protected const bool d_cleanconfig               = false;
		protected const bool d_cleandatabase             = false;
		protected bool errors                            = false;
	}
}