using System;
using System.Collections.Generic;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Framework.Config
{
    public sealed class YAMLConfig : DefaultConfig
    {
        private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
        private readonly Utilities sUtilities = Singleton<Utilities>.Instance;

        public YAMLConfig(string configdir, string configfile)
        {

        }

        ~YAMLConfig() { }
    }
}
