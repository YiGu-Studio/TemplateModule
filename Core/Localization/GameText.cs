using System.IO;
using TaleWorlds.Core;

namespace Yigu.Core.Localization
{
    public class GameText
    {
        const string MODULE_STRING_XML = "module_strings.xml";
        public static void LoadExtraGameText(Game game, string ModulePath)
        {
            var moduleStringPath = Path.Combine(ModulePath, "ModuleData", MODULE_STRING_XML);
            if (File.Exists(moduleStringPath))
            {
                game.GameTextManager.LoadGameTexts(moduleStringPath);
            }
        }
    }
}
