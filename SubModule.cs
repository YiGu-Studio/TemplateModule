using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yigu.Core.Framework;
using Yigu.Core.Config;

namespace Yigu.TemplateModule
{
    public class SubModule : AggregateModuleBase
    {
        static SubModule()
        {
            modules = CreateAllInstancesOf<IModule>(typeof(SubModule));
        }
        public static IEnumerable<T> CreateAllInstancesOf<T>(Type assemblyFrom)
        {
            return assemblyFrom.Assembly.GetTypes() //获取当前类库下所有类型
                .Where(t => typeof(T).IsAssignableFrom(t)) //获取间接或直接继承t的所有类型
                .Where(t => !t.IsAbstract && t.IsClass) //获取非抽象类 排除接口继承
                .Select(t => (T)Activator.CreateInstance(t)); //创造实例，并返回结果（项目需求，可删除）
        }

        public const string MODULE_NAME = "Yigu.TemplateModule";
        public override string ModuleName => MODULE_NAME;

        private static readonly IEnumerable<IModule> modules;
        public override IEnumerable<IModule> Modules => modules;

        private static readonly AggregateConfigBase mainConfig = new MainConfig();
        public override AggregateConfigBase MainConfig => mainConfig;
    }
}
