using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using Yigu.Core.Config;
using Yigu.Core.Helper;

namespace Yigu.Core.Framework
{
    public abstract class AggregateModuleBase : MBSubModuleBase
    {
        public abstract string ModuleName { get; }
        public string ModulePath
        {
            get
            {
                return Path.Combine(BasePath.Name, "Modules", ModuleName);
            }
        }
        public abstract IEnumerable<IModule> Modules { get; }
        public abstract AggregateConfigBase MainConfig { get; }

        private void InitModules()
        {
            //type safety check
            foreach (var module in Modules)
            {
                if (module.GetType().BaseType.GetGenericTypeDefinition() != typeof(ModuleBase<>))
                {
                    throw new ArgumentException(
                        String.Format("{0} is not subclass of {1}", module.GetType().FullName, typeof(ModuleBase<>).FullName));
                }
            }
        }

        private void InitConfigs()
        {
            MainConfig.Init(Modules);
            Module.CurrentModule.AddInitialStateOption(MainConfig.GenerateInitStateOption(8888));
        }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            InitModules();
            InitConfigs();
            foreach (var module in Modules)
            {
                module.OnSubModuleLoad();
            }
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);
            Localization.GameText.LoadExtraGameText(game, ModulePath);
            foreach (var module in Modules)
            {
                module.OnGameStart(game, gameStarterObject);
            }
        }

        public override void BeginGameStart(Game game)
        {
            LPLog.Instance.LogInfo("BeginGameStart");
            foreach (var module in Modules)
            {
                module.BeginGameStart(game);
            }
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            foreach (var module in Modules)
            {
                module.OnBeforeInitialModuleScreenSetAsRoot();
            }
            LPLog.Instance.LogInfo("OnBeforeInitialModuleScreenSetAsRoot");
            
        }

        public override void OnGameLoaded(Game game, object initializerObject)
        {
            foreach (var module in Modules)
            {
                module.OnGameLoaded(game, initializerObject);
            }
            LPLog.Instance.LogInfo("OnGameLoaded");
        }

        public override void OnNewGameCreated(Game game, object initializerObject)
        {
            foreach (var module in Modules)
            {
                module.OnNewGameCreated(game, initializerObject);
            }
            LPLog.Instance.LogInfo("OnNewGameCreated");
            //NewSkill  = new BattleNewSkill(game);
        }

        public override void OnCampaignStart(Game game, object starterObject)
        {
            foreach (var module in Modules)
            {
                module.OnCampaignStart(game, starterObject);
            }
            LPLog.Instance.LogInfo("OnCampaignStart");
        }

        public override void OnMultiplayerGameStart(Game game, object starterObject)
        {
            foreach (var module in Modules)
            {
                module.OnMultiplayerGameStart(game, starterObject);
            }
            LPLog.Instance.LogInfo("OnMultiplayerGameStart");
        }

        public override void OnGameInitializationFinished(Game game)
        {
            foreach (var module in Modules)
            {
                module.OnGameInitializationFinished(game);
            }
            LPLog.Instance.LogInfo("OnGameInitializationFinished");
        }

        public override bool DoLoading(Game game)
        {
            foreach (var module in Modules)
            {
                module.DoLoading(game);
            }
            LPLog.Instance.LogInfo("DoLoading" + game.GameType.GetType());
            return true;
        }

        public override void OnGameEnd(Game game)
        {
            foreach (var module in Modules)
            {
                module.OnGameEnd(game);
            }
            LPLog.Instance.LogInfo("OnGameEnd");
        }

        public override void OnMissionBehaviourInitialize(Mission mission)
        {
            foreach (var module in Modules)
            {
                module.OnMissionBehaviourInitialize(mission);
            }
            LPLog.Instance.LogInfo("OnMissionBehaviourInitialize");
        }
    }
}
