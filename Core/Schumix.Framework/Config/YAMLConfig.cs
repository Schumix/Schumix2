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
using System.Collections.Generic;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using YamlDotNet.Core;

namespace Schumix.Framework.Config
{
    public sealed class YAMLConfig : DefaultConfig
    {
        private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
        private readonly Utilities sUtilities = Singleton<Utilities>.Instance;

        public YAMLConfig(string configdir, string configfile)
        {
            // TODO
        }

        ~YAMLConfig() { }

        public bool CreateConfig(string configdir, string configfile)
        {
            // TODO
            return false;
        }
    }
}
