using System;
using System.Collections.Generic;
using Yigu.Core.Framework;

namespace Yigu.ThreeKingdoms.MyMod
{
    public class MyModule : ModuleBase<MyConfig>
    {
        public override IEnumerable<MissionLogicBase> MissionLogics => new List<MissionLogicBase>()
        {
            new MyMissionLogic()
        };
        public override void OnSubModuleLoad()
        {
        }
    }
}
