// TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions.BooleanOptionDataVM
using TaleWorlds.Library;
using TaleWorlds.Localization;
using Yigu.Core.Config.Data;

namespace Yigu.Core.Config.ViewModels
{
    public class BooleanOptionVM : GenericOptionVM<bool>
    {
        public BooleanOptionVM(BaseConfig config,
            string optionID, TextObject name, TextObject description)
            : base(config, optionID, name, description, OptionType.Boolean)
        { }

        [DataSourceProperty]
        public bool OptionValueAsBoolean
        {

            get { return GetOptionValue(); }
            set
            {
                if (SetOptionValue(value))
                {
                    OnPropertyChanged("OptionValueAsBoolean");
                }
            }
        }
    }
}
