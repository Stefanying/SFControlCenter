using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Configuring.Utility
{
    class Data
    {
        public string GetIP()
        {
            SFLib.IniFile iniFile = new SFLib.IniFile(_iniFilePath);
            string ip = iniFile.ReadString("Config", "IP", "127.0.0.1");
            _ip = ip;
            return _ip;
        }

        public void SaveIP(string ip)
        {
            try
            {
                SFLib.IniFile iniFile = new SFLib.IniFile(_iniFilePath);
                _ip = ip;
                iniFile.WriteString("Config", "IP", _ip);
            }
            catch
            {
            }
        }

        public static Data GetInstance()
        {
            if (_instance == null) _instance = new Data();
            return _instance;
        }

        static Data _instance;
        string _ip = "192.168.1.1";
        string _iniFilePath = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
    }
}
