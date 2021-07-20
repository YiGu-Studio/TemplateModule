using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace Yigu.Core.Framework
{
    /// <summary>
    /// 三国战场脚本管理
    /// </summary>
    public class MissionLogicManager : MissionLogic
    {
        private List<MissionLogicBase> missionLogicList = new List<MissionLogicBase>();

        public void AddMissionLogic(MissionLogicBase logic)
        {
            missionLogicList.Add(logic);
        }

        #region 重载方法
        public override void EarlyStart()
        {
            foreach (var m in missionLogicList)
            {
                m.EarlyStart();
            }
        }

        public override void AfterStart()
        {
            foreach (var m in missionLogicList)
            {
                m.AfterStart();
            }
        }

        public override void OnAgentHit(Agent affectedAgent, Agent affectorAgent, int damage, in MissionWeapon affectorWeapon)
        {
            foreach (var m in missionLogicList)
            {
                m.OnAgentHit(affectedAgent, affectorAgent, damage, in affectorWeapon);
            }
        }

        public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
        {
            foreach (var m in missionLogicList)
            {
                m.OnAgentRemoved(affectedAgent, affectorAgent, agentState, killingBlow);
            }
        }

        public override void OnAgentBuild(Agent agent, Banner banner)
        {
            foreach (var m in missionLogicList)
            {
                m.OnAgentBuild(agent, banner);
            }
        }

        public override void OnBattleEnded()
        {
            foreach (var m in missionLogicList)
            {
                m.OnBattleEnded();
            }
        }

        public override void OnMissileCollisionReaction(Mission.MissileCollisionReaction collisionReaction, Agent attackerAgent, Agent attachedAgent, sbyte attachedBoneIndex)
        {
            foreach (var m in missionLogicList)
            {
                m.OnMissileCollisionReaction(collisionReaction, attackerAgent, attachedAgent, attachedBoneIndex);
            }
        }

        public override void OnMissileHit(Agent attacker, Agent victim, bool isCanceled)
        {
            foreach (var m in missionLogicList)
            {
                m.OnMissileHit(attacker, victim, isCanceled);
            }
        }

        /// <summary>
        ///  注册攻击函数
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="victim"></param>
        /// <param name="realHitEntity"></param>
        /// <param name="b"></param>
        /// <param name="collisionData"></param>
        public override void OnRegisterBlow(Agent attacker, Agent victim, GameEntity realHitEntity, Blow b,
            ref AttackCollisionData collisionData, in MissionWeapon attackerWeapon)
        {
            foreach (var m in missionLogicList)
            {
                m.OnRegisterBlow(attacker, victim, realHitEntity, b, ref collisionData, in attackerWeapon);
            }
        }

        public override void OnAgentShootMissile(Agent shooterAgent, EquipmentIndex weaponIndex, Vec3 position,
            Vec3 velocity, Mat3 orientation, bool hasRigidBody, int forcedMissileIndex)
        {
            foreach (var m in missionLogicList)
            {
                m.OnAgentShootMissile(shooterAgent, weaponIndex, position, velocity, orientation, hasRigidBody, forcedMissileIndex);
            }
        }

        public override void OnAgentDismount(Agent agent)
        {
            foreach (var m in missionLogicList)
            {
                m.OnAgentDismount(agent);
            }
        }

        public override void OnMissionTick(float dt)
        {
            foreach (var m in missionLogicList)
            {
                m.OnMissionTick(dt);
            }
        }

        public override void OnAgentMount(Agent agent)
        {
            foreach (var m in missionLogicList)
            {
                m.OnAgentMount(agent);
            }
        }

        protected override void OnObjectDisabled(DestructableComponent destructionComponent)
        { 
            foreach (var m in missionLogicList)
            {
                m.OnObjectDisabled(destructionComponent);
            }
        }
        #endregion
    }
}
