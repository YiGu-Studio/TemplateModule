using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using Yigu.Core.Config.Data;

namespace Yigu.Core.Config.ViewModels
{
    public class FloatOptionVM : GenericOptionVM<float>
    {
        public FloatOptionVM(BaseConfig config,
            string optionID, TextObject name, TextObject description,
            float min, float max)
            : base(config, optionID, name, description, OptionType.Float)
        {
            Min = min;
            Max = max;
        }

        [DataSourceProperty]
        public float Min { get; }
        [DataSourceProperty]
        public float Max { get; }
        [DataSourceProperty]
        public float OptionValueAsFloat
        {

            get { return GetOptionValue(); }
            set
            {
                if (SetOptionValue(value))
                {
                    OnPropertyChanged("OptionValueAsFloat");
                    OnPropertyChanged("OptionValueAsString");
                }
            }
        }
        [DataSourceProperty]
        public string OptionValueAsString
        {

            get { return GetOptionValue().ToString("F"); }
        }
        [DataSourceProperty]
        public bool IsDiscrete { get; } = false;
    }

    public class IntOptionVM : GenericOptionVM<int>
    {
        public IntOptionVM(BaseConfig config,
            string optionID, TextObject name, TextObject description,
            int min, int max)
            : base(config, optionID, name, description, OptionType.Int)
        {
            Min = min;
            Max = max;
        }

        [DataSourceProperty]
        public int Min { get; }
        [DataSourceProperty]
        public int Max { get; }
        [DataSourceProperty]
        public float OptionValueAsFloat
        {

            get { return (float)GetOptionValue(); }
            set
            {
                if (SetOptionValue((int)value))
                {
                    OnPropertyChanged("OptionValueAsFloat");
                    OnPropertyChanged("OptionValueAsString");
                }
            }
        }
        [DataSourceProperty]
        public string OptionValueAsString
        {

            get { return GetOptionValue().ToString(); }
        }
        [DataSourceProperty]
        public bool IsDiscrete { get; } = true;
    }
}
