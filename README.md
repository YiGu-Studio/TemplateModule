# TemplateModule

这是一个帮助你快速上手开发的模板工程。你可以使用它进行独立开发，在测试完成后把它合并到主工程`ThreeKingdomsEA`中。

## 开发步骤

### 前置步骤
复制`Directory.Build.props.template`并重命名为`Directory.Build.props`，修改其中的`<GameFolder>`属性为你的游戏安装目录（通常为`D:\Program Files\Steam\SteamApps\common\Mount &amp; Blade II Bannerlord`），这一步能让项目编译生成的DLL文件和`StaticResources`下的静态资源文件自动部署到游戏的`Modules\Yigu.TemplateModule`目录中。

### `MyMod`文件夹
这是你的Mod开发代码所在位置。在开发和测试完成后，你只需要将文件夹中的内容直接合并到`ThreeKingdomsEA`主工程中并修改几行代码即可完成合并。

#### `MyModule.cs`
这是你的Mod代码入口。首先你需要修改`namespace`，目前三国mod的命名空间为`Yigu.ThreeKingdoms.子类别.Mod名`，其中子类别目前有`Battlefield`（战场类）和`Campaign`（战役类，指一个存档中的全局要素）。例如战场小地图Mod的命名空间是`Yigu.ThreeKingdoms.Battlefield.MiniMap`。一个响亮而有意义的名字是重要的，它有助于让你的代码和其它Mod不起冲突。

你的`MyModule`类需要继承泛型类`Yigu.Core.Framework.ModuleBase<ConfigType>`，其中`ConfigType`是你的配置文件类（参见下文`MyConfig.cs`），并实现函数`OnSubModuleLoad()`。这个类的效果类似于`TaleWorlds.MountAndBlade.MBSubModuleBase`，提供了一系列`OnXXX()`函数允许你在Mod运行的各个时间点运行你的代码逻辑。

* `IEnumerable<Yigu.Core.Framework.MissionLogicBase> MissionLogics`: 可以override该属性，在`OnMissionBehaviorInitialize()`时候会自动加载这些MissionLogic（参见下文`MyMissionLogic.cs`）

* `IEnumerable<TaleWorlds.CampaignSystem.CampaignBehaviorBase>`: 可以override该属性，在`OnCampaignStart()`时自动加载这些战役逻辑。

#### `MyConfig.cs`
这是你的Mod配置类。同样不要忘记修改命名空间。如果不需要可视化配置，删除该文件并在`MyModule`类的泛型参数中填上`Yigu.Core.Config.NoConfig`即可。

你的`MyConfig`类需要继承`Yigu.Core.Config.BaseConfig`。在这个类中可以创建如下几种类型的配置项：
* `BooleanOption`
* `IntOption`
* `FloatOption`
* `EnumOption`

需要新增一个配置项的时候，首先写一个属性，比如
```
public float SomeOption {get;set;} = 2.0;
```

然后用对应的Attribute进行修饰即可，Attribute的参数为
```
[xxOption(配置项名字的translation string, 配置项的描述translation string, 配置项的目录, ...)]
```

其中
* 名字和描述的translation string写在`StaticResources\ModuleData\Languages\CNs\std_config_strings_xml-zho-CN.xml`
* 配置项的目录是一个字符串，`"Battlefield"`是战场类的配置，`"Economy"`是经济类，其它的类别有需要可以在顶层的`MainConfig.cs`中添加。
* 其它参数，根据配置项类型不同，
    * 数值类配置项（`IntOption`, `FloatOption`）还需要两个参数即上下限
    * 枚举类配置项需要一个数组类型的参数，分别为每个枚举值的translation string，同样写在`StaticResources\ModuleData\Languages\CNs\std_config_strings_xml-zho-CN.xml`

#### `MyMissionLogic.cs`
一个MissionLogic类就是一段场景/战场中的逻辑。你需要继承`Yigu.Core.Framework.MissionLogicBase`来实现你的逻辑。类似于`TaleWorlds.MountAndBlase.MissionLogic`，它提供了一系列`OnXXX()`函数允许你在战斗的各个阶段运行你的代码逻辑。例子中的代码就是在战斗刚开场时在游戏左下角日志中输出一行"hello world"。

* `bool IfAddMissionLogic(Mission m)`: 你需要override该方法，判断在某种Mission下是否触发该逻辑。

### `StaticResources`文件夹
这是游戏的静态资源文件夹，该目录下的所有文件/文件夹将被copy到Mod目录即游戏的`Modules\Yigu.TemplateModule`下。
