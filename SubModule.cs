using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yigu.Core.Framework;
using Yigu.Core.Config;
using Yigu.ThreeKingdoms.MyMod;

namespace Yigu.TemplateModule
{
    public class SubModule : AggregateModuleBase
    {
        public const string MODULE_NAME = "Yigu.TemplateModule";
        public override string ModuleName => MODULE_NAME;

        private static readonly IEnumerable<IModule> modules = new List<IModule>
        {
            new MyModule(),
        };
        public override IEnumerable<IModule> Modules => modules;

        private static readonly AggregateConfigBase mainConfig = new MainConfig();
        public override AggregateConfigBase MainConfig => mainConfig;
    }
}
