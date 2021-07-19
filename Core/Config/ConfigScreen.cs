using System;
using TaleWorlds.Localization;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.GauntletUI.Data;
using Yigu.Core.Config.Data;
using Yigu.Core.Config.ViewModels;

namespace Yigu.Core.Config
{
    public class ConfigScreen : ScreenBase
    {
        ConfigVM _dataSource;

        private GauntletLayer _gauntletLayer;
        GauntletMovie _movie;

        public AggregateConfigBase aggregateConfig;
        public Action<AggregateConfigBase> SaveOptionAction;
        public TextObject Title;
        private string _configViewName;
        public ConfigScreen(TextObject title, string configViewName, AggregateConfigBase config, Action<AggregateConfigBase> saveConfigAction = null)
        {
            Title = title;
            _configViewName = configViewName;
            this.aggregateConfig = config;
            this.SaveOptionAction = saveConfigAction;
        }

/*        protected override void OnFrameTick(float dt)
        {
            base.OnFrameTick(dt);
            LoadingWindow.DisableGlobalLoadingWindow();
            if (_gauntletLayer.Input.IsHotKeyReleased("Exit") )
            {
                ScreenManager.PopScreen();
                //Game.Current.GameStateManager.PopState();
            }
            //_dataSource?.OnFrameTick();
        }*/

        protected override void OnInitialize()
        {
            //LPLog.Log("初始化LPConfigScreen");
            _dataSource = new ConfigVM(Title, aggregateConfig, true, SaveOptionAction);

            _gauntletLayer = new GauntletLayer(-1);
            _movie = (GauntletMovie)_gauntletLayer.LoadMovie(_configViewName, _dataSource);
            _gauntletLayer.InputRestrictions.SetInputRestrictions();
            _gauntletLayer.IsFocusLayer = true;
            AddLayer(_gauntletLayer);
            ScreenManager.TrySetFocus(_gauntletLayer);
            //Instance = this;
            //LPLog.Log("打开 BRConfigView");
        }

        protected override void OnFinalize()
        {
            //_gauntletLayer.ReleaseMovie(_movie);

            //base.OnDeactivate();
            RemoveLayer(_gauntletLayer);
            _gauntletLayer.IsFocusLayer = false;
            ScreenManager.TryLoseFocus(_gauntletLayer);
            //Game.Current.EventManager.TriggerEvent(new TutorialContextChangedEvent(TutorialContexts.None));

            _dataSource.OnFinalize();
            _movie = null;
            _gauntletLayer = null;
            base.OnFinalize();
        }
    }
}
