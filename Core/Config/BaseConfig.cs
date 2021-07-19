using System;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Newtonsoft.Json;
using Yigu.Core.Config.Data;

namespace Yigu.Core.Config
{
    public abstract class BaseConfig
    {
        private readonly OrderedDictionary _optionPropertyDict = new OrderedDictionary();
        private readonly Dictionary<string, object> _optionDefaultValueDict = new Dictionary<string, object>();
        public BaseConfig()
        {
            InitOptionInfo();
        }

        private void InitOptionInfo()
        {
            var t = GetType();
            foreach (var property in t.GetProperties())
            {
                if (!property.IsDefined(typeof(OptionAttribute), false))
                    continue;

                if (!TypeValidation(property))
                {
                    throw new ArgumentException(string.Format("property type mismatch with attribute: {0}", property.Name));
                }

                _optionPropertyDict.Add(property.Name, property);
                _optionDefaultValueDict.Add(property.Name, property.GetValue(this));
            }
        }

        private bool TypeValidation(PropertyInfo property)
        {
            OptionAttribute optionAttr = property.GetCustomAttribute<OptionAttribute>();
            switch (optionAttr.OptionType)
            {
                case OptionType.Boolean:
                    if (property.PropertyType == typeof(bool)) return true;
                    break;
                case OptionType.Float:
                    if (property.PropertyType == typeof(float)) return true;
                    break;
                case OptionType.Int:
                    if (property.PropertyType == typeof(int)) return true;
                    break;
                case OptionType.Enum:
                    if (property.PropertyType.IsEnum) return true;
                    break;
                default:
                    return false;
            }
            return false;

        }
        public IEnumerable<OptionAttribute> GetOptionList()
        {
            foreach (DictionaryEntry entry in _optionPropertyDict)
            {
                PropertyInfo property = entry.Value as PropertyInfo;
                OptionAttribute optionAttr = property.GetCustomAttribute<OptionAttribute>();
                optionAttr.SetOptionID(property.Name);
                yield return optionAttr;
            }
        }

        private PropertyInfo GetOptionProperty(string optionID)
        {
            if (_optionPropertyDict[optionID] is PropertyInfo property)
            {
                return property;
            }
            else
            {
                throw new ArgumentException(string.Format("optionID {0} not found", optionID));
            }
        }

        public T GetConfig<T>(string optionID)
        {
            return (T)GetOptionProperty(optionID).GetValue(this);
        }

        public void SetConfig<T>(string optionID, T optionValue)
        {
            GetOptionProperty(optionID).SetValue(this, optionValue);
        }

        public T GetDefaultValue<T>(string optionID)
        {
            return (T)_optionDefaultValueDict[optionID];
        }
    }
}
