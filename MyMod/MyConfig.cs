using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yigu.Core.Config;
using Yigu.Core.Config.Data;

namespace Yigu.ThreeKingdoms.MyMod
{
    public class MyConfig : BaseConfig
    {
        [FloatOption(
            "{=Yigu.Template.ConfigName}ConfigName",
            "{=Yigu.Template.ConfigDesc}ConfigDesc",
            "Battlefield", 0, 5)]
        public float MiniMapOpacity { get; set; } = 2.1F;
    }
}
