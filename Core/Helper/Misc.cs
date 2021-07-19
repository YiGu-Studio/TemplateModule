using System;
using System.Linq;
using System.Collections.Generic;

namespace Yigu.Core.Helper
{
    public static class Misc
    {
        public static IEnumerable<T> CreateAllInstancesOf<T>(Type assemblyFrom)
        {
            return assemblyFrom.Assembly.GetTypes() //获取当前类库下所有类型
                .Where(t => typeof(T).IsAssignableFrom(t)) //获取间接或直接继承t的所有类型
                .Where(t => !t.IsAbstract && t.IsClass) //获取非抽象类 排除接口继承
                .Select(t => (T)Activator.CreateInstance(t)); //创造实例，并返回结果（项目需求，可删除）
        }
    }
}
