using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Engine.Screens;
using Yigu.Core.Framework;
using Yigu.Core.Config.Data;

namespace Yigu.Core.Config
{
    public abstract class AggregateConfigBase
    {
        private const string configViewName = "ConfigView";
        private readonly Dictionary<string, BaseConfig> optionID2Instance = new Dictionary<string, BaseConfig>();
        private readonly Dictionary<Type, BaseConfig> configType2Instance = new Dictionary<Type, BaseConfig>();

        public void Init(IEnumerable<IModule> modules)
        {
            InitOptionInfo(modules);
            LoadConfigFromFile();
        }

        private void InitOptionInfo(IEnumerable<IModule> modules)
        {
            Type baseConfigType = typeof(BaseConfig);
            foreach (var module in modules)
            {
                var configType = module.ConfigType;
                if (!configType.IsSubclassOf(baseConfigType))
                {
                    throw new ArgumentException(String.Format("{0} is not a subclass of {1}", configType.FullName, baseConfigType.FullName));
                }
                if (configType == typeof(NoConfig))
                {
                    continue;
                }
                if (configType2Instance.ContainsKey(configType))
                {
                    throw new ArgumentException(String.Format("Duplicate Config Type {0}", configType.FullName));
                }

                BaseConfig configInstance = module.Config;
                configType2Instance.Add(configType, configInstance);

                foreach (OptionAttribute optionAttr in configInstance.GetOptionList())
                {
                    string optionID = optionAttr.OptionID;
                    if (optionID2Instance.ContainsKey(optionID))
                    {
                        throw new ArgumentException(String.Format("Duplicate OptionID {0} in config {1} and {2}", 
                            optionID,
                            optionID2Instance[optionID].GetType().FullName,
                            configType.FullName));
                    }
                    optionID2Instance.Add(optionID, configInstance);
                }
            }
        }

        public BaseConfig GetConfigByOptionID(string optionID)
        {
            return optionID2Instance[optionID];
        }

        public IEnumerable<OptionAttribute> GetOptionList()
        {
            return configType2Instance.Values.SelectMany(config => config.GetOptionList());
        }

        public InitialStateOption GenerateInitStateOption(int order)
        {
            return new InitialStateOption("Yigu.Core.BaseAggregateConfig", ConfigTitle, order, delegate
            {
                ScreenManager.PushScreen(new ConfigScreen(ConfigTitle, configViewName, this, (AggregateConfigBase aggregateConfig) =>
                {
                    aggregateConfig.SaveConfigToFile();
                }));
            }, isDisabledAndReason: delegate { return (false, TextObject.Empty); });

        }

        private Dictionary<string, BaseConfig> SerializeAsDict()
        {
            var result = new Dictionary<string, BaseConfig>();
            foreach (var kvp in configType2Instance)
            {
                result.Add(kvp.Key.FullName, kvp.Value);
            }
            return result;
        }

        public virtual void SaveConfigToFile()
        {
            string configJson = JsonConvert.SerializeObject(SerializeAsDict(), Formatting.Indented);

            string configDir = Path.GetDirectoryName(ConfigFilePath);
            if (!Directory.Exists(configDir))
            {
                Directory.CreateDirectory(configDir);
            }
            File.WriteAllText(ConfigFilePath, configJson);
        }

        public virtual void LoadConfigFromFile()
        {
            if (File.Exists(ConfigFilePath))
            {
                var dict = SerializeAsDict();
                string configJson = File.ReadAllText(ConfigFilePath);
                var configJObject = JObject.Parse(configJson);
                foreach (var configKvp in configJObject)
                {
                    if (dict.TryGetValue(configKvp.Key, out BaseConfig config))
                    {
                        JsonConvert.PopulateObject(configKvp.Value.ToString(), config);
                    }
                }
            }
        }

        public abstract Dictionary<string, TextObject> CategoryIDWithNames { get; }
        public abstract TextObject ConfigTitle { get; }
        public abstract string ConfigFilePath { get; }
    }
}
