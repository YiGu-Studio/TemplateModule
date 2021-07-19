// TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions.GenericOptionDataVM
using System;
using System.Collections.Generic;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using Yigu.Core.Config.Data;

namespace Yigu.Core.Config.ViewModels
{
    public abstract class BaseOptionVM : ViewModel
    {
        private TextObject _nameObj;

        private TextObject _descriptionObj;

        protected BaseConfig _config;

        public string OptionID { get; set; }

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
                    OnPropertyChangedWithValue(value, "Name");
                }
            }
        }

        private string _description;
        [DataSourceProperty]
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if (value != _description)
                {
                    _description = value;
                    OnPropertyChangedWithValue(value, "Description");
                }
            }
        }

        private string[] _imageIDs;
        [DataSourceProperty]
        public string[] ImageIDs
        {
            get
            {
                return _imageIDs;
            }
            set
            {
                if (value != _imageIDs)
                {
                    _imageIDs = value;
                    OnPropertyChangedWithValue(value, "ImageIDs");
                }
            }
        }

        private OptionType _optionType;
        [DataSourceProperty]
        public OptionType OptionType
        {
            get { return _optionType; }
            set
            {
                if (value != _optionType)
                {
                    _optionType = value;
                    OnPropertyChanged("OptionType");
                }
            }
        }

        protected BaseOptionVM(BaseConfig config,
            string optionID, TextObject name, TextObject description, OptionType optionType)
        {
            OptionID = optionID;
            _nameObj = name;
            _descriptionObj = description;
            _config = config;
            OptionType = optionType;
            RefreshValues();
        }

        public override void RefreshValues()
        {
            base.RefreshValues();
            Name = _nameObj.ToString();
            Description = _descriptionObj.ToString();
        }

        public abstract void GetValueFromConfig();
        public abstract void SetValueToConfig();
        public abstract void Cancel();
        public abstract bool IsChanged();
        public abstract void ResetValue();
    }
    public class GenericOptionVM<T> : BaseOptionVM
        where T : IEquatable<T>
    {
        protected readonly T _initialValue;

        /// <summary>
        /// 实际数据
        /// </summary>
        protected T _value;

        protected GenericOptionVM(BaseConfig config,
            string optionID, TextObject name, TextObject description, OptionType optionType)
            : base(config, optionID, name, description, optionType)
        {
            GetValueFromConfig();
            _initialValue = _value;
        }

        protected T GetOptionValue()
        {
            return _value;
        }
        protected bool SetOptionValue(T value)
        {
            bool changed = !_value.Equals(value);
            if (changed)
            {
                _value = value;
                SetValueToConfig();
            }
            return changed;
        }

        public override void GetValueFromConfig()
        {
            _value = _config.GetConfig<T>(OptionID);
        }

        public override void SetValueToConfig()
        {
            _config.SetConfig(OptionID, GetOptionValue());
        }

        /// <summary>
        /// 取消改变
        /// </summary>
        public override void Cancel()
        {
            SetOptionValue(_initialValue);
        }

        public override bool IsChanged()
        {
            return !_initialValue.Equals(GetOptionValue());
        }

        /// <summary>
        /// 重置为初始值
        /// </summary>
        public override void ResetValue()
        {
            SetOptionValue(_config.GetDefaultValue<T>(OptionID));
        }
    }
}
