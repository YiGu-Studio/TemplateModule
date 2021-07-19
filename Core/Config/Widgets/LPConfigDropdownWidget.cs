using System;
using System.Numerics;
using TaleWorlds.GauntletUI;

namespace Yigu.Core.Config.Widgets
{
    public class LPConfigDropdownWidget : Widget
    {
        private Action<Widget> _clickHandler;

        private Action<Widget> _listSelectionHandler;

        private Action<Widget> _listItemRemovedHandler;

        private Action<Widget, Widget> _listItemAddedHandler;

        private Vector2 _dropdownOpenPosition;

        private float _animationSpeedModifier = 15f;

        private ButtonWidget _button;

        private ListPanel _listPanel;

        private int _currentSelectedIndex;

        private Widget _dropdownContainerWidget;

        private Widget _dropdownClipWidget;

        private bool _isOpen;

        private bool _buttonClicked;

        private bool _updateSelectedItem = true;

        [Editor(false)] public RichTextWidget RichTextWidget { get; set; }

        [Editor(false)]
        public ButtonWidget Button
        {
            get { return _button; }
            set
            {
                if (_button != null)
                {
                    _button.ClickEventHandlers.Remove(_clickHandler);
                }

                _button = value;
                if (_button != null)
                {
                    _button.ClickEventHandlers.Add(_clickHandler);
                }

                RefreshSelectedItem();
            }
        }

        [Editor(false)]
        public Widget DropdownContainerWidget
        {
            get { return _dropdownContainerWidget; }
            set { _dropdownContainerWidget = value; }
        }

        [Editor(false)]
        public Widget DropdownClipWidget
        {
            get { return _dropdownClipWidget; }
            set
            {
                _dropdownClipWidget = value;
                _dropdownClipWidget.ParentWidget = base.EventManager.Root;
                _dropdownClipWidget.HorizontalAlignment = HorizontalAlignment.Left;
                _dropdownClipWidget.VerticalAlignment = VerticalAlignment.Top;
            }
        }

        [Editor(false)]
        public ListPanel ListPanel
        {
            get { return _listPanel; }
            set
            {
                if (_listPanel != null)
                {
                    _listPanel.SelectEventHandlers.Remove(_listSelectionHandler);
                    _listPanel.ItemAddEventHandlers.Remove(_listItemAddedHandler);
                    _listPanel.ItemRemoveEventHandlers.Remove(_listItemRemovedHandler);
                }

                _listPanel = value;
                if (_listPanel != null)
                {
                    _listPanel.SelectEventHandlers.Add(_listSelectionHandler);
                    _listPanel.ItemAddEventHandlers.Add(_listItemAddedHandler);
                    _listPanel.ItemRemoveEventHandlers.Add(_listItemRemovedHandler);
                }

                RefreshSelectedItem();
            }
        }

        [Editor(false)]
        public int ListPanelValue
        {
            get
            {
                if (ListPanel != null)
                {
                    return ListPanel.IntValue;
                }

                return -1;
            }
            set
            {
                if (ListPanel != null && ListPanel.IntValue != value)
                {
                    ListPanel.IntValue = value;
                }
            }
        }

        [Editor(false)]
        public int CurrentSelectedIndex
        {
            get { return _currentSelectedIndex; }
            set
            {
                if (_currentSelectedIndex != value)
                {
                    _currentSelectedIndex = value;
                    OnPropertyChanged(CurrentSelectedIndex, "CurrentSelectedIndex");
                }
            }
        }

        [Editor(false)]
        public bool UpdateSelectedItem
        {
            get { return _updateSelectedItem; }
            set
            {
                if (_updateSelectedItem != value)
                {
                    _updateSelectedItem = value;
                }
            }
        }

        public LPConfigDropdownWidget(UIContext context)
            : base(context)
        {
            _clickHandler = OnButtonClick;
            _listSelectionHandler = OnSelectionChanged;
            _listItemRemovedHandler = OnListChanged;
            _listItemAddedHandler = OnListChanged;
        }

        protected override void OnUpdate(float dt)
        {
            base.OnUpdate(dt);
            if (_buttonClicked)
            {
                _buttonClicked = false;
            }
            else if (base.EventManager.LatestMouseUpWidget != _button && _isOpen && DropdownClipWidget.IsVisible)
            {
                ClosePanel();
            }

            RefreshSelectedItem();
        }

        protected override void OnLateUpdate(float dt)
        {
            base.OnLateUpdate(dt);
            if (_isOpen && Vector2.Distance(DropdownClipWidget.GlobalPosition, _dropdownOpenPosition) > 5f)
            {
                ClosePanelInOneFrame();
            }

            UpdateListPanelPosition(dt);
        }

        private void UpdateListPanelPosition(float dt)
        {
            DropdownClipWidget.HorizontalAlignment = HorizontalAlignment.Left;
            DropdownClipWidget.VerticalAlignment = VerticalAlignment.Top;
            Vector2 one = Vector2.One;
            float num = 0f;
            if (_isOpen)
            {
                Widget child = DropdownContainerWidget.GetChild(0);
                num = child.Size.Y + child.MarginBottom * base._scaleToUse;
            }
            else
            {
                num = 0f;
            }

            one = Button.GlobalPosition + new Vector2((Button.Size.X - DropdownClipWidget.Size.X) / 2f, Button.Size.Y);
            DropdownClipWidget.ScaledPositionXOffset = one.X;
            DropdownClipWidget.ScaledSuggestedHeight =
                TaleWorlds.Library.MathF.Lerp(DropdownClipWidget.ScaledSuggestedHeight, num, dt * _animationSpeedModifier);
            DropdownClipWidget.ScaledPositionYOffset =
                TaleWorlds.Library.MathF.Lerp(DropdownClipWidget.ScaledPositionYOffset, one.Y,
                    dt * _animationSpeedModifier);
            if (!_isOpen && Math.Abs(DropdownClipWidget.ScaledSuggestedHeight - num) < 0.5f)
            {
                DropdownClipWidget.IsVisible = false;
            }
            else if (_isOpen)
            {
                DropdownClipWidget.IsVisible = true;
            }
        }

        protected virtual void OpenPanel()
        {
            _isOpen = true;
            DropdownClipWidget.IsVisible = true;
            _dropdownOpenPosition = Button.GlobalPosition +
                                    new Vector2((Button.Size.X - DropdownClipWidget.Size.X) / 2f, Button.Size.Y);
        }

        protected virtual void ClosePanel()
        {
            _isOpen = false;
        }

        private void ClosePanelInOneFrame()
        {
            _isOpen = false;
            DropdownClipWidget.IsVisible = false;
            DropdownClipWidget.ScaledSuggestedHeight = 0f;
        }

        public void OnButtonClick(Widget widget)
        {
            _buttonClicked = true;
            if (_isOpen)
            {
                ClosePanel();
            }
            else
            {
                OpenPanel();
            }
        }

        public void UpdateButtonText(string text)
        {
            if (RichTextWidget != null)
            {
                if (text != null)
                {
                    RichTextWidget.Text = text;
                }
                else
                {
                    RichTextWidget.Text = " ";
                }
            }
        }

        public void OnListChanged(Widget widget)
        {
            RefreshSelectedItem();
            DropdownContainerWidget.IsVisible = (widget.ChildCount > 1);
        }

        public void OnListChanged(Widget parentWidget, Widget addedWidget)
        {
            RefreshSelectedItem();
            DropdownContainerWidget.IsVisible = (parentWidget.ChildCount > 0);
        }

        public void OnSelectionChanged(Widget widget)
        {
            if (UpdateSelectedItem)
            {
                CurrentSelectedIndex = ListPanelValue;
                RefreshSelectedItem();
            }
        }

        private void RefreshSelectedItem()
        {
            if (!UpdateSelectedItem)
            {
                return;
            }

            ListPanelValue = CurrentSelectedIndex;
            string text = "";
            if (ListPanelValue >= 0 && ListPanel != null)
            {
                Widget child = ListPanel.GetChild(ListPanelValue);
                if (child != null)
                {
                    foreach (Widget allChild in child.AllChildren)
                    {
                        RichTextWidget richTextWidget = allChild as RichTextWidget;
                        if (richTextWidget != null)
                        {
                            text = richTextWidget.Text;
                        }
                    }
                }
            }

            UpdateButtonText(text);
            if (ListPanel == null)
            {
                return;
            }

            for (int i = 0; i < ListPanel.ChildCount; i++)
            {
                Widget child2 = ListPanel.GetChild(i);
                if (CurrentSelectedIndex == i)
                {
                    child2.SetState("Selected");
                    if (child2 is ButtonWidget)
                    {
                        (child2 as ButtonWidget).IsSelected = (CurrentSelectedIndex == i);
                    }
                }
            }
        }
    }
}
