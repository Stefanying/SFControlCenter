using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Configuring.Utility
{
    class ConfigData
    {
        public string GetIP()
        {
            SFLib.IniFile iniFile = new SFLib.IniFile(_iniFilePath);
            string ip = iniFile.ReadString("Config", "IP", "127.0.0.1");
            _ip = ip;
            return _ip;
        }

        public int GetSerialPortCount()
        {
            SFLib.IniFile _iniFile = new SFLib.IniFile(_iniFilePath);
            int t_count = _iniFile.ReadInteger("Config","SerialPortCount",14);
            _serialPortCount = t_count;
            return _serialPortCount;
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

        public static ConfigData GetInstance()
        {
            if (_instance == null) _instance = new ConfigData();
            return _instance;
        }

        static ConfigData _instance;
        string _ip = "192.168.1.1";
        int _serialPortCount = 14;
        string _iniFilePath = AppDomain.CurrentDomain.BaseDirectory + "ConfiguringCtrlSettings.ini";
    }
}
