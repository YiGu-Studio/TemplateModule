using System.Collections.Generic;
using System.Linq;
using TaleWorlds.GauntletUI;
using TaleWorlds.MountAndBlade.GauntletUI.Widgets.Options;
using TaleWorlds.TwoDimension;
using Yigu.Core.Config.Data;

namespace Yigu.Core.Config.Widgets
{
    public class LPConfigItemWidget : Widget
    {
        private LPConfigScreenWidget _screenWidget;

        private bool _eventsRegistered;

        private bool _initialized;

        private Widget _dropdownExtensionParentWidget;

        private OptionsDropdownWidget _dropdownWidget;

        private ButtonWidget _booleanToggleButtonWidget;

        private List<Sprite> _graphicsSprites;

        private OptionType _optionType;

        private string _optionDescription;

        private string _optionTitle;

        private string[] _imageIDs;

        public Widget BooleanOption
        {
            get;
            set;
        }

        public Widget NumericOption
        {
            get;
            set;
        }

        public Widget StringOption
        {
            get;
            set;
        }

        public Widget GameKeyOption
        {
            get;
            set;
        }

        public Widget ActionOption
        {
            get;
            set;
        }

        public Widget InputOption
        {
            get;
            set;
        }

        public OptionType OptionType
        {
            get
            {
                return _optionType;
            }
            set
            {
                if (_optionType != value)
                {
                    _optionType = value;
                    OnPropertyChanged(_optionType, "OptionType");
                }
            }
        }

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

        public string[] ImageIDs
        {
            get
            {
                return _imageIDs;
            }
            set
            {
                if (_imageIDs != value)
                {
                    _imageIDs = value;
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

        public LPConfigItemWidget(UIContext context)
            : base(context)
        {
            _optionType = OptionType.Invalid;
            _graphicsSprites = new List<Sprite>();
        }

        protected override void OnLateUpdate(float dt)
        {
            base.OnLateUpdate(dt);
            if (!_initialized)
            {
                if (ImageIDs != null)
                {
                    int i;
                    for (i = 0; i < ImageIDs.Length; i++)
                    {
                        if (ImageIDs[i] != string.Empty)
                        {
                            Sprite item = base.Context.SpriteList.SingleOrDefault((Sprite s) => s.Name == ImageIDs[i]);
                            _graphicsSprites.Add(item);
                        }
                    }
                }
                RefreshVisibilityOfSubItems();
                _initialized = true;
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
            if (_optionType == OptionType.Enum)
            {
                if (fromHoverOverDropdown)
                {
                    if (_graphicsSprites.Count > hoverDropdownItemIndex)
                    {
                        _ = _graphicsSprites[hoverDropdownItemIndex];
                    }
                }
                else if (_graphicsSprites.Count > _dropdownWidget.CurrentSelectedIndex && _dropdownWidget.CurrentSelectedIndex >= 0)
                {
                    _ = _graphicsSprites[_dropdownWidget.CurrentSelectedIndex];
                }
                _screenWidget?.SetCurrentOption(this, null);
            }
            else if (_optionType == OptionType.Boolean)
            {
                int num = (!_booleanToggleButtonWidget.IsSelected) ? 1 : 0;
                if (_graphicsSprites.Count > num)
                {
                    _ = _graphicsSprites[num];
                }
                _screenWidget?.SetCurrentOption(this, null);
            }
            else
            {
                _screenWidget?.SetCurrentOption(this, null);
            }
        }

        public void SetCurrentScreenWidget(LPConfigScreenWidget screenWidget)
        {
            _screenWidget = screenWidget;
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
            if (OptionType == OptionType.Boolean)
            {
                _booleanToggleButtonWidget = (BooleanOption.GetChild(0) as ButtonWidget);
                _booleanToggleButtonWidget.PropertyChanged += BooleanOption_PropertyChanged;
            }
            else if (OptionType == OptionType.Enum)
            {
                _dropdownWidget = (StringOption.GetChild(1) as OptionsDropdownWidget);
                _dropdownExtensionParentWidget = _dropdownWidget.DropdownClipWidget;
                foreach (Widget allChild2 in _dropdownExtensionParentWidget.AllChildren)
                {
                    allChild2.PropertyChanged += DropdownItem_PropertyChanged1;
                }
            }
        }

        private void BooleanOption_PropertyChanged(PropertyOwnerObject childWidget, string propertyName, object propertyValue)
        {
            if (propertyName == "IsSelected")
            {
                SetCurrentOption(fromHoverOverDropdown: false, fromBooleanSelection: true);
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

        private void DropdownItem_PropertyChanged1(PropertyOwnerObject childWidget, string propertyName, object propertyValue)
        {
            if (propertyName == "IsHovered")
            {
                if ((bool)propertyValue)
                {
                    Widget widget = childWidget as Widget;
                    SetCurrentOption(fromHoverOverDropdown: true, fromBooleanSelection: false, widget.ParentWidget.GetChildIndex(widget));
                }
                else
                {
                    ResetCurrentOption();
                }
            }
        }

        // TODO: refactor OptionTypeID as enum
        private void RefreshVisibilityOfSubItems()
        {
            BooleanOption.IsVisible = (OptionType == OptionType.Boolean);
            NumericOption.IsVisible = (OptionType == OptionType.Float) || (OptionType == OptionType.Int);
            StringOption.IsVisible = (OptionType == OptionType.Enum);
            GameKeyOption.IsVisible = false;
            InputOption.IsVisible = false;
            /*
            NumericOption.IsVisible = (OptionTypeID == 1);
            GameKeyOption.IsVisible = (OptionTypeID == 2);
            InputOption.IsVisible = (OptionTypeID == 4);
            if (ActionOption != null)
            {
                ActionOption.IsVisible = (OptionTypeID == 5);
            }
            */
        }
    }
}
