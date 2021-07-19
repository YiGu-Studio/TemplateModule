using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.Engine.Screens;
using Yigu.Core.Config.Data;

namespace Yigu.Core.Config.ViewModels
{
    /// <summary>
    /// 配置主界面
    /// </summary>
    public class ConfigVM : ViewModel
    {
        private const string _doneLblText = "{=Yigu.Config.DoneLbl}Done";
        private const string _cancelLblText = "{=Yigu.Config.CancelLbl}Cancel";
        /// <summary>
        /// 选项分类目录
        /// </summary>
        [DataSourceProperty]
        public MBBindingList<OptionCategoryVM> OptionCategories { get; }

        public OptionCategoryVM FirstOptionCategory => OptionCategories.First();

        private string _optionsLbl;
        [DataSourceProperty]
        public string OptionsLbl
        {
            set
            {
                if (value != _optionsLbl)
                {
                    _optionsLbl = value;
                    OnPropertyChanged("OptionsLbl");
                }
            }
            get { return _optionsLbl; }
        }

        private string _cancelLbl;
        [DataSourceProperty]
        public string CancelLbl
        {
            set
            {
                if (value != _cancelLbl)
                {
                    _cancelLbl = value;
                    OnPropertyChanged("CancelLbl");
                }
            }
            get { return _cancelLbl; }
        }

        private string _doneLbl;
        [DataSourceProperty]
        public string DoneLbl
        {
            set
            {
                if (value != _doneLbl)
                {
                    _doneLbl = value;
                    OnPropertyChanged("DoneLbl");
                }
            }
            get { return _doneLbl; }
        }

        public AggregateConfigBase AggregateConfig { get; }
        public bool OldGameStateManagerDisabledStatus { get; private set; }
        public ConfigVM(TextObject title, AggregateConfigBase config, bool autoHandleClose = true, Action<AggregateConfigBase> onClose = null)
        {
            AggregateConfig = config;
            _autoHandleClose = autoHandleClose;
            _onClose = onClose;
            OptionCategories = new MBBindingList<OptionCategoryVM>();

            if (Game.Current != null && _autoHandleClose)
            {
                OldGameStateManagerDisabledStatus = Game.Current.GameStateManager.ActiveStateDisabledByUser;
                Game.Current.GameStateManager.ActiveStateDisabledByUser = true;
            }

            OptionsLbl = title.ToString();
            DoneLbl = new TextObject(_doneLblText).ToString();
            // ResetLbl = GameTexts.FindText("str_reset").ToString();
            CancelLbl = new TextObject(_cancelLblText).ToString();

            Init();
        }

        private void Init()
        {
            var optCategoryMap = new Dictionary<string, List<OptionAttribute>>();
            foreach (var optCategoryInfo in AggregateConfig.CategoryIDWithNames)
            {
                optCategoryMap.Add(optCategoryInfo.Key, new List<OptionAttribute>());
            }
            foreach (OptionAttribute optionInfo in AggregateConfig.GetOptionList())
            {
                string categoryID = optionInfo.CategoryID;
                if (optCategoryMap.TryGetValue(categoryID, out List<OptionAttribute> optList))
                {
                    optList.Add(optionInfo);
                }
                else
                {
                    throw new ArgumentException(String.Format("CategoryID {0} invalid", categoryID));
                }
            }

            foreach (var optCategoryInfo in AggregateConfig.CategoryIDWithNames)
            {
                OptionCategories.Add(new OptionCategoryVM(
                    this, optCategoryInfo.Key, optCategoryInfo.Value, optCategoryMap[optCategoryInfo.Key]));
            }
        }

        public override void RefreshValues()
        {
            base.RefreshValues();
            DoneLbl = new TextObject(_doneLblText).ToString();
            // ResetLbl = GameTexts.FindText("str_reset").ToString();
            CancelLbl = new TextObject(_cancelLblText).ToString();
            foreach (OptionCategoryVM optCategory in OptionCategories)
            {
                optCategory.RefreshValues();
            }
        }

        /// <summary>
        /// 设置指定值
        /// </summary>
        /// <param name="data"></param>
        /// <param name="val"></param>
        /*
        public void SetConfig(LPOptionData data, float val)
        {
            if (!_isInitialized)
            {
                return;
            }
            //NativeOptions.ConfigQuality autoGFXQuality = NativeConfig.AutoGFXQuality;
            if (!data.IsNative())
            {
                return;
            }
            //Utilities.SetGraphicsPreset((int)val);
            foreach (BaseDataOptionVM option in BROptions.Options)
            {
                float defaultOptionForOverallSettings = 1;//GetDefaultOptionForOverallSettings((NativeOptions.NativeOptionsType)option.GetOptionType(), (int)val);
                if (defaultOptionForOverallSettings >= 0f)
                {
                    option.SetValue(defaultOptionForOverallSettings);
                    option.UpdateValue();
                }
            }

            if (!_isCancelling )
            {
                InformationManager.ShowInquiry(new InquiryData(Module.CurrentModule.GlobalTextManager.FindText("str_option_restart_required").ToString(),
                    Module.CurrentModule.GlobalTextManager.FindText("str_option_restart_required_desc").ToString(),
                    true,  false,
                    Module.CurrentModule.GlobalTextManager.FindText("str_ok").ToString(), 
                    string.Empty, null, null));
            }
        }
        */

        private void OnDone()
        {
            if (_autoHandleClose)
            {
                if (Game.Current != null)
                {
                    Game.Current.GameStateManager.ActiveStateDisabledByUser = OldGameStateManagerDisabledStatus;
                }
                ScreenManager.PopScreen();
            }
        }
        private readonly Action<AggregateConfigBase> _onClose;
        private bool _autoHandleClose;

        private bool _isCancelling;
        /// <summary>
        /// 取消设置
        /// </summary>
        protected void ExecuteCancel()
        {
            _isCancelling = true;
            foreach (OptionCategoryVM optCategory in OptionCategories)
            {
                foreach (BaseOptionVM option in optCategory.OptionVMs)
                {
                    option.Cancel();
                }
            }
            if (_autoHandleClose)
            {
                if (Game.Current != null)
                {
                    Game.Current.GameStateManager.ActiveStateDisabledByUser = OldGameStateManagerDisabledStatus;
                }
                ScreenManager.PopScreen();
            }
        }

        /// <summary>
        /// 确认设置
        /// </summary>
        protected void ExecuteDone()
        {
            if (_onClose != null)
            {
                _onClose(AggregateConfig);
            }
            OnDone();
        }
    }
}
