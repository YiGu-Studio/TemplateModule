using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Yigu.Core.Helper
{
    public class MBHelper
    {
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }


    public static string GetModName()
        {
            var path = BasePath.Name;
            var nowpath = AssemblyDirectory;
            var paths = nowpath.Split('\\');
            var NowModName = paths[paths.Length - 3];
            var NowModNameId = NowModName.ToLower();
            //var filePath = BasePath.Name + @"Modules\" + NowModName + @"\MBLog.txt";
            //LPLog.LogPath = filePath;
           // LPLog.Log("当前mod名：" + NowModName);
            return NowModName;
        }

    public static string GetModPath()
    {
        var path = BasePath.Name;
        var nowpath = AssemblyDirectory;
        var paths = nowpath.Split('\\');
        var NowModName = paths[paths.Length - 3];
        var NowModNameId = NowModName.ToLower();
        var filePath = BasePath.Name + @"Modules\" + NowModName;
        //LPLog.LogPath = filePath;
        //LPLog.Log("当前mod名：" + NowModName);
        return filePath;
    }


        public static void DisplayMessage(string msg)
        {
            InformationManager.DisplayMessage(new InformationMessage(new TextObject(msg).ToString()));
        }


        public static void DisplayMessage(string msg, Color color)
        {
            InformationManager.DisplayMessage(new InformationMessage(new TextObject(msg).ToString(), color));
        }

        public static void DisplayMessage(TextObject msg)
        {
            InformationManager.DisplayMessage(new InformationMessage(msg.ToString()));
        }

        public static void AddQuickInformation(TextObject msg)
        {
            InformationManager.AddQuickInformation(msg, 0, null);
        }
        public static void AddQuickInformation(string msg)
        {
            InformationManager.AddQuickInformation(new TextObject(msg), 0, null);
        }

        public static void DisplayMessage(TextObject msg, Color color)
        {
            InformationManager.DisplayMessage(new InformationMessage(msg.ToString(), color));
        }


    }
}
