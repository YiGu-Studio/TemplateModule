using TaleWorlds.GauntletUI;

namespace Yigu.Core.Config.Widgets
{
    public class LPConfigKeyItemWidget : ListPanel
    {
        private LPConfigScreenWidget _screenWidget;

        private bool _eventsRegistered;

        private string _optionDescription;

        private string _optionTitle;

        public string OptionTitle
        {
            get
            {
                return _optionTitle;
            }
            set
            {
                if (_optionTitle != value)
                {
                    _optionTitle = value;
                }
            }
        }

        public string OptionDescription
        {
            get
            {
                return _optionDescription;
            }
            set
            {
                if (_optionDescription != value)
                {
                    _optionDescription = value;
                }
            }
        }

        public LPConfigKeyItemWidget(UIContext context)
            : base(context)
        {
        }

        protected override void OnLateUpdate(float dt)
        {
            base.OnLateUpdate(dt);
            if (_screenWidget == null)
            {
                _screenWidget = (base.EventManager.Root.GetChild(0).FindChild("Options") as LPConfigScreenWidget);
            }
            if (!_eventsRegistered)
            {
                RegisterHoverEvents();
                _eventsRegistered = true;
            }
        }

        protected override void OnHoverBegin()
        {
            base.OnHoverBegin();
            SetCurrentOption(fromHoverOverDropdown: false, fromBooleanSelection: false);
        }

        protected override void OnHoverEnd()
        {
            base.OnHoverEnd();
            ResetCurrentOption();
        }

        private void SetCurrentOption(bool fromHoverOverDropdown, bool fromBooleanSelection, int hoverDropdownItemIndex = -1)
        {
            _screenWidget?.SetCurrentOption(this, null);
        }

        private void ResetCurrentOption()
        {
            _screenWidget?.SetCurrentOption(null, null);
        }

        private void RegisterHoverEvents()
        {
            foreach (Widget allChild in base.AllChildren)
            {
                allChild.PropertyChanged += Child_PropertyChanged;
            }
        }

        private void Child_PropertyChanged(PropertyOwnerObject childWidget, string propertyName, object propertyValue)
        {
            if (propertyName == "IsHovered")
            {
                if ((bool)propertyValue)
                {
                    SetCurrentOption(fromHoverOverDropdown: false, fromBooleanSelection: false);
                }
                else
                {
                    ResetCurrentOption();
                }
            }
        }
    }
}
