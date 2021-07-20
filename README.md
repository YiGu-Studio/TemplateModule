# TemplateModule

这是一个帮助你快速上手开发的模板工程。你可以使用它进行独立开发，在测试完成后把它合并到主工程`ThreeKingdomsEA`中。

## 开发步骤

1. 复制`Directory.Build.props.template`并重命名为`Directory.Build.props`，修改其中的`<GameFolder>`属性为你的游戏安装目录（通常为`D:\Program Files\Steam\SteamApps\common\Mount &amp; Blade II Bannerlord`），这一步能让项目编译生成的DLL文件和`StaticResources`下的静态资源文件自动部署到游戏的`Modules/Yigu.TemplateModule`中
