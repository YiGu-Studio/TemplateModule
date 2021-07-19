using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using Yigu.Core.Config;

namespace Yigu.Core.Framework
{
    public interface IModule
    {
        public void OnSubModuleLoad();
        public void OnGameStart(Game game, IGameStarter gameStarterObject);
        public void BeginGameStart(Game game);
        public void OnBeforeInitialModuleScreenSetAsRoot();
        public void OnGameLoaded(Game game, object initializerObject);
        public void OnNewGameCreated(Game game, object initializerObject);
        public void OnCampaignStart(Game game, object starterObject);
        public void OnMultiplayerGameStart(Game game, object starterObject);
        public void OnGameInitializationFinished(Game game);
        public bool DoLoading(Game game);
        public void OnGameEnd(Game game);
        public void OnMissionBehaviourInitialize(Mission mission);
        public Type ConfigType { get; }
        public BaseConfig Config { get; }
    }

    public abstract class ModuleBase<ConfigT> : IModule
        where ConfigT : BaseConfig, new()
    {
        public virtual void BeginGameStart(Game game) { }
        public virtual bool DoLoading(Game game) { return true; }
        public virtual void OnBeforeInitialModuleScreenSetAsRoot() { }
        public virtual void OnCampaignStart(Game game, object gameStarterObject)
        {
            CampaignGameStarter campaignGameStarter = gameStarterObject as CampaignGameStarter;
            foreach (var behavior in CampaignBehaviors)
            {
                campaignGameStarter?.AddBehavior(behavior);
            }
        }
        public virtual void OnGameEnd(Game game) { }
        public virtual void OnGameInitializationFinished(Game game) { }
        public virtual void OnGameLoaded(Game game, object initializerObject) { }
        public virtual void OnGameStart(Game game, IGameStarter gameStarterObject) { }
        public virtual void OnMissionBehaviourInitialize(Mission mission)
        {
            if (Mission.Current.Scene != null)
            {
                var mgr = new MissionLogicManager();
                foreach (var logic in MissionLogics)
                {
                    if (logic.AssignMission(mission))
                    {
                        mgr.AddMissionLogic(logic);
                    }
                }
                Mission.Current.AddMissionBehaviour(mgr);
            }
        }
        public virtual void OnMultiplayerGameStart(Game game, object starterObject) { }
        public virtual void OnNewGameCreated(Game game, object initializerObject) { }
        public abstract void OnSubModuleLoad();

        Type IModule.ConfigType => typeof(ConfigT);

        private static ConfigT _staticConfig;
        public static ConfigT Config
        {
            get
            {
                if (_staticConfig == null)
                {
                    _staticConfig = new ConfigT();
                }
                return _staticConfig;
            }
        }
        BaseConfig IModule.Config { get { return Config; } }

        public virtual IEnumerable<MissionLogicBase> MissionLogics
        {
            get
            {
                return Enumerable.Empty<MissionLogicBase>();
            }
        }
        public virtual IEnumerable<CampaignBehaviorBase> CampaignBehaviors 
        {
            get
            {
                return Enumerable.Empty<CampaignBehaviorBase>();
            }
        }
    }
}
