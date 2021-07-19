using System;
using System.IO;
//using System.Windows.Forms;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace Yigu.Core.Helper
{
    public class LPLog
    {
        private static LPLog _instance;

        public static LPLog Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new LPLog();
                return _instance;
            }
        }

        public static string LogPath = Path.Combine(BasePath.Name, "LPLog.log");

        public void Init(string path)
        {
            LogPath = path;
        }

        public static void Log(string log)
        {
            Instance.LogInfo(log);
        }
        public static void LogError(Exception e,bool bol=false)
        {
            Instance.LogInfo(e.ToStringFull());
        /*
            if (bol)
            {
                MessageBox.Show("LPLog Error", e.ToStringFull());
            }
        */
            InformationManager.DisplayMessage(new InformationMessage(e.ToStringFull(), new Color(1,0,0)));
        }

        public static void LogError(string log)
        {
            log = ("=========ERROR- " + System.DateTime.Now + " -=====\r\n") + log
                                                                             + ("\r\n========ERROR---END======");
            Instance.LogInfo(log);
            InformationManager.DisplayMessage(new InformationMessage(log, new Color(1,0,0)));
        }



        public LPLog()
        {
/*            if (!IConfigOption)
            {
                return;
            }*/
            if (File.Exists(LogPath))
                File.Delete(LogPath);
            LogInfo("======================开启测试LOG：" + System.DateTime.Now + "=================");

        }


        public void LogInfo(string str)
        {
            //if (!BattlefieldReformationConfig.Instance.IsEnableLog)
            //return;

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(LogPath, true))
            {
                file.WriteLine(str);
            }

        }
    }
}


