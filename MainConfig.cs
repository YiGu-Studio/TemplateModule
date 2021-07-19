using System;
using System.IO;
using System.Collections.Generic;
using TaleWorlds.Localization;
using Yigu.Core.Config;

namespace Yigu.TemplateModule
{
    public class MainConfig : AggregateConfigBase
    {
        static readonly TextObject configTitle = new TextObject("{=Yigu.Config.Title}ThreeKingdoms ConfigTitle");
        public override TextObject ConfigTitle => configTitle;

        static readonly Dictionary<string, TextObject> categoryIDWithNames = new Dictionary<string, TextObject>
        {
            { "Battlefield", new TextObject("{=Yigu.Config.Category.Battlefield}Battlefield") },
            { "Economy", new TextObject("{=Yigu.Config.Category.Economy}Economy") },
        };
        public override Dictionary<string, TextObject> CategoryIDWithNames => categoryIDWithNames;

        static readonly string configFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Personal),
            "Mount and Blade II Bannerlord", "Configs", SubModule.MODULE_NAME+".json");
        public override string ConfigFilePath => configFilePath;
    }
}
