using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace Yigu.Core.Helper
{
    public static class ExtendFun
    {

        /// <summary>
        /// 获取点到点的方向
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static Vec3 GetPositionBetweenDirection(Vec3 p1, Vec3 p2)
        {
            return (p2 - p1).NormalizedCopy();
        }

        /// <summary>
        /// 角度转化为方向
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Vec2 DirFromAngle(float angle)
        {
            return new Vec2((float)Math.Sin(angle), (float)Math.Cos(angle));
        }



        /// <summary>
        /// 获取两点之间的距离
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static float GetPositionBetweenDistance(Vec3 p1, Vec3 p2)
        {
            return p1.Distance(p2);
        }

        public static Vec3 Random(this Vec3 vec3,float min,float max)
        {
            var vec = new Vec3();
            vec.x = vec3.x+ MBRandom.RandomFloatRanged(min, max);
            vec.y = vec3.y + MBRandom.RandomFloatRanged(min, max);
            vec.z = vec3.z + MBRandom.RandomFloatRanged(min, max);
            //vec.w = vec3.w + MBRandom.RandomFloatRanged(min, max);
            return vec;
        }

        public static Vec3 RandomForward(this Vec3 vec3, float min, float max)
        {
            var vec = new Vec3();
            vec.x = vec3.x + MBRandom.RandomFloatRanged(min, max);
            vec.y = vec3.y + MBRandom.RandomFloatRanged(min, max);
            vec.z = vec3.z;
            return vec;
        }

        public static void RotateUp(this Vec3 vec3, float a)
        {
            vec3.RotateAboutZ(a * TaleWorlds.Library.MathF.PI / 180);
        }

    }
}
