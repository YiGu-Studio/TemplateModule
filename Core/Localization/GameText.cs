using System.IO;
using TaleWorlds.Core;

namespace Yigu.Core.Localization
{
    public class GameText
    {
        const string MODULE_STRING_XML = "module_strings.xml";
        public static void LoadExtraGameText(Game game, string ModulePath)
        {
            game.GameTextManager.LoadGameTexts(Path.Combine(ModulePath, "ModuleData", MODULE_STRING_XML));
        }
    }
}
