using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace SFLib
{
    public class IniFile
    {
        public IniFile(string filename)
        {
            _fileName = filename;
        }

        public bool IsFileExist()
        {
            return File.Exists(_fileName);
        }

        public bool ReadBool(string section, string key, bool defualt)
        {
            bool ret = defualt;
            string value = ReadValue(section, key);
            if (value.Length > 0)
            {
                ret = bool.Parse(value);
            }
            return ret;
        }

        public int ReadInteger(string section, string key, int defualt)
        {
            int ret = defualt;
            string value = ReadValue(section, key);
            if (value.Length > 0)
            {
                ret = int.Parse(value);
            }
            return ret;
        }

        public string ReadString(string section, string key, string defualt)
        {
            string ret = defualt;
            string value = ReadValue(section, key);
            if (value.Length > 0)
            {
                ret = value;
            }
            return ret;
        }

        public float ReadFloat(string section, string key, float defualt)
        {
            float ret = defualt;
            string value = ReadValue(section, key);
            if (value.Length > 0)
            {
                ret = float.Parse(value);
            }
            return ret;
        }

        public void WriteBool(string section, string key, bool value)
        {
            WriteValue(section, key, value.ToString());
        }

        public void WriteInteger(string section, string key, int value)
        {
            WriteValue(section, key, value.ToString());
        }

        public void WriteString(string section, string key, string value)
        {
            WriteValue(section, key, value.ToString());
        }

        public void WriteFloat(string section, string key, float value)
        {
            WriteValue(section, key, value.ToString());
        }

        // protected
        protected void WriteValue(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, this._fileName);
        }
        protected string ReadValue(string section, string key)
        {
            StringBuilder temp = new StringBuilder(500);
            GetPrivateProfileString(section, key, "", temp, 500, this._fileName);
            return temp.ToString();
        }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        private string _fileName;
    }
}
