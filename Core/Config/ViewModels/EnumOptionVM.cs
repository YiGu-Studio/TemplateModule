using System.Collections.Generic;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.Core.ViewModelCollection;
using Yigu.Core.Config.Data;

namespace Yigu.Core.Config.ViewModels
{
    public class EnumOptionVM : BaseOptionVM
    {

        private readonly int _initialSelectedIndex;

        private SelectorVM<SelectorItemVM> _selector;
        [DataSourceProperty]
        public SelectorVM<SelectorItemVM> Selector
        {
            get { return _selector; }
            set
            {
                if (value != _selector)
                {
                    _selector = value;
                    OnPropertyChanged("Selector");
                }
            }
        }

        public EnumOptionVM(BaseConfig config,
            string optionID, TextObject name, TextObject description,
            IEnumerable<TextObject> selectionTexts)
            : base(config, optionID, name, description, OptionType.Enum)
        {
            _initialSelectedIndex = _config.GetConfig<int>(optionID);
            _selector = new SelectorVM<SelectorItemVM>(
                selectionTexts, _initialSelectedIndex, OnSelectedIndexChange);
            RefreshValues();
        }

        public void OnSelectedIndexChange(SelectorVM<SelectorItemVM> selector)
        {
            if (selector.SelectedIndex >= 0)
            {
                _config.SetConfig<int>(OptionID, selector.SelectedIndex);
            }
        }
        public override void RefreshValues()
        {
            base.RefreshValues();
            _selector?.RefreshValues();
        }

        /* SelectedIndex is always bound to config value */
        public override void GetValueFromConfig() { }
        public override void SetValueToConfig() { }

        public override void Cancel()
        {
            Selector.SelectedIndex = _initialSelectedIndex;
        }

        public override bool IsChanged()
        {
            return _initialSelectedIndex != Selector.SelectedIndex;
        }

        public override void ResetValue()
        {
            Selector.SelectedIndex = _config.GetConfig<int>(OptionID);
        }
    }
}
