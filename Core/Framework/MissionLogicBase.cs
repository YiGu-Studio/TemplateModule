using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace Yigu.Core.Framework
{
    /// <summary>
    /// 继承此类实现MissionLogic效果
    /// </summary>
    public abstract class MissionLogicBase
    {
        protected Mission Mission;
        public bool AssignMission(Mission m)
        {
            if (IfAddMissionLogic(m))
            {
                Mission = m;
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 添加到战场的条件、脚本是否添加到场景中
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public abstract bool IfAddMissionLogic(Mission m);
        public virtual void EarlyStart()
        { }

        public virtual void AfterStart()
        { }

        public virtual void OnAgentHit(Agent affectedAgent, Agent affectorAgent, int damage, in MissionWeapon affectorWeapon)
        { }
        public virtual void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
        { }

        public virtual void OnAgentBuild(Agent agent, Banner banner)
        { }
        public virtual void OnBattleEnded()
        { } 
        public virtual void OnMissileCollisionReaction(Mission.MissileCollisionReaction collisionReaction, Agent attackerAgent, Agent attachedAgent, sbyte attachedBoneIndex)
        { }

        public virtual void OnMissileHit(Agent attacker, Agent victim, bool isCanceled)
        { }

        /// <summary>
        ///  注册攻击函数
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="victim"></param>
        /// <param name="realHitEntity"></param>
        /// <param name="b"></param>
        /// <param name="collisionData"></param>
        public virtual void OnRegisterBlow(Agent attacker, Agent victim, GameEntity realHitEntity, Blow b,
            ref AttackCollisionData collisionData, in MissionWeapon attackerWeapon)
        { 
        }

        public virtual void OnAgentShootMissile(Agent shooterAgent, EquipmentIndex weaponIndex, Vec3 position,
            Vec3 velocity, Mat3 orientation, bool hasRigidBody, int forcedMissileIndex)
        { }

        public virtual void OnAgentDismount(Agent agent) { }

        public virtual void OnMissionTick(float dt) { }

        public virtual void OnAgentMount(Agent agent) { }
    }
}
