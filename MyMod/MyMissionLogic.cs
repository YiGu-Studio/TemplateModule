using TaleWorlds.MountAndBlade;
using Yigu.Core.Framework;
using Yigu.Core.Helper;

namespace Yigu.ThreeKingdoms.MyMod
{
    public class MyMissionLogic : MissionLogicBase
    {
        public override bool IfAddMissionLogic(Mission m)
        {
            return Mission.Current.GetMissionBehaviour<MissionCombatantsLogic>() != null ||
                Mission.Current.GetMissionBehaviour<CustomBattleAgentLogic>() != null;
        }

        public override void AfterStart()
        {
            base.AfterStart();
            MBHelper.DisplayMessage("hello world");
        }
    }
}
