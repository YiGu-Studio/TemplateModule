using TaleWorlds.GauntletUI;
using TaleWorlds.TwoDimension;

namespace Yigu.Core.Config.Widgets
{
    public class LPConfigScreenWidget : Widget
    {
        private Widget _currentOptionWidget;

        private bool _initialized;

        public RichTextWidget CurrentOptionDescriptionWidget
        {
            get;
            set;
        }

        public RichTextWidget CurrentOptionNameWidget
        {
            get;
            set;
        }

        public Widget CurrentOptionImageWidget
        {
            get;
            set;
        }

        public LPConfigScreenWidget(UIContext context)
            : base(context)
        {
        }

        protected override void OnUpdate(float dt)
        {
            base.OnUpdate(dt);
            if (!_initialized)
            {
                foreach (Widget allChild in base.AllChildren)
                {
                    LPConfigItemWidget optionsItemWidget;
                    if ((optionsItemWidget = (allChild as LPConfigItemWidget)) != null)
                    {
                        optionsItemWidget.SetCurrentScreenWidget(this);
                    }
                }
                _initialized = true;
            }
        }

        public void SetCurrentOption(Widget currentOptionWidget, Sprite newgraphicsSprite)
        {
            if (_currentOptionWidget != currentOptionWidget)
            {
                _currentOptionWidget = currentOptionWidget;
                string text = "";
                string text2 = "";
                if (_currentOptionWidget != null)
                {
                    LPConfigItemWidget optionsItemWidget;
                    LPConfigKeyItemWidget optionsKeyItemWidget;
                    if ((optionsItemWidget = (_currentOptionWidget as LPConfigItemWidget)) != null)
                    {
                        text = optionsItemWidget.OptionDescription;
                        text2 = optionsItemWidget.OptionTitle;
                    }
                    else if ((optionsKeyItemWidget = (_currentOptionWidget as LPConfigKeyItemWidget)) != null)
                    {
                        text = optionsKeyItemWidget.OptionDescription;
                        text2 = optionsKeyItemWidget.OptionTitle;
                    }
                }
                if (CurrentOptionDescriptionWidget != null)
                {
                    CurrentOptionDescriptionWidget.Text = text;
                }
                if (CurrentOptionDescriptionWidget != null)
                {
                    CurrentOptionNameWidget.Text = text2;
                }
            }
            if (CurrentOptionImageWidget != null && CurrentOptionImageWidget.Sprite != newgraphicsSprite)
            {
                CurrentOptionImageWidget.Sprite = newgraphicsSprite;
            }
        }
    }

}
