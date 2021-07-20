# TemplateModule

这是一个帮助你快速上手开发的模板工程。你可以使用它进行独立开发，在测试完成后把它合并到主工程`ThreeKingdomsEA`中。

## 开发步骤

1. 复制`Directory.Build.props.template`并重命名为`Directory.Build.props`，修改其中的`<GameFolder>`属性为你的游戏安装目录（通常为`D:\Program Files\Steam\SteamApps\common\Mount &amp; Blade II Bannerlord`），这一步能让项目编译生成的DLL文件和`StaticResources`下的静态资源文件自动部署到游戏的`Modules/Yigu.TemplateModule`中

2. `MyMod`文件夹

  这是你的Mod开发代码所在位置。在开发和测试完成后，你只需要将文件夹中的内容直接合并到`ThreeKingdomsEA`主工程中并修改几行代码即可完成合并。

  * `MyModule.cs`: 这是你的Mod代码入口。首先你需要修改`namespace`，目前三国mod的命名空间为`Yigu.ThreeKingdoms.子类别.Mod名`，其中子类别目前有`Battlefield`（战场类）和`Campaign`（战役类，指一个存档中的全局要素）。例如战场小地图Mod的命名空间是`Yigu.ThreeKingdoms.Battlefield.MiniMap`。一个响亮而有意义的名字是重要的，它有助于让你的代码和其它Mod不起冲突。

    你的`MyModule`类需要继承泛型类`Yigu.Core.Framework.ModuleBase<ConfigType>`，其中`ConfigType`是你的配置文件类（参见下文`MyConfig.cs`），并实现函数`OnSubModuleLoad()`。这个类的效果类似于`TaleWorlds.MountAndBlade.MBSubModuleBase`，提供了一系列`OnXXX()`函数允许你在Mod运行的各个时间点运行你的代码逻辑。

    * `IEnumerable<Yigu.Core.Framework.MissionLogicBase> MissionLogics`: 可以override该属性，在`OnMissionBehaviorInitialize()`时候会自动加载这些MissionLogic（参见下文`MyMissionLogic.cs`）

    * `IEnumerable<TaleWorlds.CampaignSystem.CampaignBehaviorBase>`: 可以override该属性，在`OnCampaignStart()`时自动加载这些战役逻辑。

  * `MyConfig.cs`: 这是你的Mod配置类。同样不要忘记修改命名空间。如果不需要可视化配置，删除该文件并在`MyModule`类的泛型参数中填上`Yigu.Core.Config.NoConfig`即可。

