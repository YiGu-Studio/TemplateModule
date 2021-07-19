using System;
using System.Collections.Generic;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using Yigu.Core.Config.Data;

namespace Yigu.Core.Config.ViewModels
{
    /// <summary>
    /// 控制列表界面
    /// </summary>
    public class OptionCategoryVM : ViewModel
    {
        private readonly TextObject _nameText;

        [DataSourceProperty]
        public string CategoryID { get; }

        private string _name;
        [DataSourceProperty]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        private MBBindingList<BaseOptionVM> _optionVMs;
        [DataSourceProperty]
        public MBBindingList<BaseOptionVM> OptionVMs
        {
            get
            {
                return _optionVMs;
            }
            set
            {
                if (value != _optionVMs)
                {
                    _optionVMs = value;
                    OnPropertyChanged("OptionsVMs");
                }
            }
        }

        public OptionCategoryVM(ConfigVM configVM, string categoryID, TextObject nameText, IEnumerable<OptionAttribute> optionInfoList)
        {
            AggregateConfigBase aggregateConfig = configVM.AggregateConfig;
            CategoryID = categoryID;
            _optionVMs = new MBBindingList<BaseOptionVM>();
            _nameText = nameText;
            foreach (var optionInfo in optionInfoList)
            {
                string optID = optionInfo.OptionID;
                BaseConfig config = aggregateConfig.GetConfigByOptionID(optID);
                TextObject optName = optionInfo.OptionName;
                TextObject optDesc = optionInfo.OptionDescription;
                optDesc.SetTextVariable("newline", "\n");
                //ActionOptionData actionOptionData;
                switch (optionInfo)
                {
                    case BooleanOptionAttribute booleanOptionInfo:
                        _optionVMs.Add(new BooleanOptionVM(config, optID, optName, optDesc));
                        break;
                    case FloatOptionAttribute floatOptionInfo:
                        _optionVMs.Add(new FloatOptionVM(config, optID, optName, optDesc,
                            floatOptionInfo.MinValue, floatOptionInfo.MaxValue));
                        break;
                    case IntOptionAttribute intOptionInfo:
                        _optionVMs.Add(new IntOptionVM(config, optID, optName, optDesc,
                            intOptionInfo.MinValue, intOptionInfo.MaxValue));
                        break;
                    case EnumOptionAttribute enumOptionInfo:
                        List<TextObject> selectionTextList = new List<TextObject>();
                        foreach (string text in enumOptionInfo.SelectionTexts)
                        {
                            selectionTextList.Add(new TextObject(text));
                        }
                        _optionVMs.Add(new EnumOptionVM(config, optID, optName, optDesc,
                            selectionTextList));
                        break;
                    default:
                        throw new NotImplementedException();
                }
                /*
                else if ((actionOptionData = (target as ActionOptionData)) != null)
                {
                    TextObject optionActionName = Module.CurrentModule.GlobalTextManager.FindText("str_options_type_action", text);
                    ActionOptionDataVM item3 = new ActionOptionDataVM(actionOptionData.OnAction, options, actionOptionData, name2, optionActionName, textObject);
                    _options.Add(item3);
                }*/
            }
            RefreshValues();
        }

        public override void RefreshValues()
        {
            base.RefreshValues();
            Name = _nameText.ToString();
            OptionVMs.ApplyActionOnAllItems(optionVM => optionVM.RefreshValues());
        }
    }

}
