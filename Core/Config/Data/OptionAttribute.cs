using System;
using TaleWorlds.Localization;

namespace Yigu.Core.Config.Data
{
    public enum OptionType
    {
        Boolean,
        Enum,
        Int,
        Float,
        Invalid,
    }

    [AttributeUsage(AttributeTargets.Enum)]
    public class CategoryAttribute : Attribute
    {
        public TextObject[] CategoryTitleTexts { get; }
        public CategoryAttribute(string[] categoryTitleTexts)
        {
            CategoryTitleTexts = new TextObject[categoryTitleTexts.Length];
            for (int i = 0; i < categoryTitleTexts.Length; ++i)
            {
                CategoryTitleTexts[i] = new TextObject(categoryTitleTexts[i]);
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class OptionAttribute: Attribute
    {
        private string _optionID;
        public string OptionID { get { return _optionID; } }
        public string CategoryID { get; }
        public TextObject OptionName { get; }
        public TextObject OptionDescription { get; }
        public OptionType OptionType { get; }

        protected OptionAttribute(OptionType optionType,
            string optionNameStr, string optionDescriptionStr,
            string categoryID)
        {
            OptionName = new TextObject(optionNameStr);
            OptionDescription = new TextObject(optionDescriptionStr);
            OptionType = optionType;
            CategoryID = categoryID;
        }

        public void SetOptionID(string optionID)
        {
            _optionID = optionID;
        }
    }

    public class BooleanOptionAttribute : OptionAttribute
    {
        public BooleanOptionAttribute(string optionNameStr, string optionDescriptionStr,
            string categoryID) :
            base(OptionType.Boolean, optionNameStr, optionDescriptionStr, categoryID)
        { }
    }
    public class EnumOptionAttribute : OptionAttribute
    {
        public string[] SelectionTexts { get; }
        public EnumOptionAttribute(string optionNameStr, string optionDescriptionStr,
            string categoryID, string[] selectionTexts) :
            base(OptionType.Enum, optionNameStr, optionDescriptionStr, categoryID)
        {
            SelectionTexts = selectionTexts;
        }
    }


    public class FloatOptionAttribute : OptionAttribute
    {
        public float MinValue { get; }
        public float MaxValue { get; }
        public FloatOptionAttribute(string optionNameStr, string optionDescriptionStr,
            string categoryID, float minValue, float maxValue) :
            base(OptionType.Float, optionNameStr, optionDescriptionStr, categoryID)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }

    public class IntOptionAttribute : OptionAttribute
    {
        public int MinValue { get; }
        public int MaxValue { get; }
        public IntOptionAttribute(string optionNameStr, string optionDescriptionStr,
            string categoryID, int minValue, int maxValue) :
            base(OptionType.Float, optionNameStr, optionDescriptionStr, categoryID)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}
