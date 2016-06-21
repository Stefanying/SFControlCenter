using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFLib;
namespace ControlCenter.Utility
{
   public  class ConfigData
    {

       public static  ConfigData GetInstance()
       {
           if (_instance == null)
               _instance = new ConfigData();
           return _instance;
       }

       public bool GetAutoRun()
       {
           IniFile _iniFile = new IniFile(_configFile);
           bool t_isAutoRun = _iniFile.ReadBool("Config", "AutoStart", true);
           _IsAutoRun = t_isAutoRun;
           return _IsAutoRun;
       }

       public string GetIsComEnable()
       {
           IniFile _iniFile = new IniFile(_configFile);
           return _iniFile.ReadString("Config", "IsComEnable","1");
       }

       bool _IsAutoRun = true;
       static ConfigData _instance;
       string _configFile = AppDomain.CurrentDomain.BaseDirectory + "Config.ini";
    }
}
