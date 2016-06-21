using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SFLib;
using Microsoft.Win32;
using ControlCenter.Utility;
namespace ControlCenter.UI
{
    public partial class SoftSetWindow : Form
    {
        public SoftSetWindow()
        {
            InitializeComponent();
        }

        string _configFile = AppDomain.CurrentDomain.BaseDirectory + "Config.ini";
        int _delayTime = 20;
        bool _IsAutoRun = true;

        private void SaveAutoRun(bool p_isAutoRun)
        {
            IniFile _iniFile = new IniFile(_configFile);
            _iniFile.WriteBool("Config", "AutoStart",p_isAutoRun);
        }

        private void SaveDelayTime(int p_time)
        {
            IniFile _iniFile = new IniFile(_configFile);
            _iniFile.WriteInteger("Config","DelayTime",p_time);
        }

        private bool GetAutoRun()
        {
            IniFile _iniFile = new IniFile(_configFile);
            bool t_isAutoRun = _iniFile.ReadBool("Config", "AutoStart",true);
            _IsAutoRun = t_isAutoRun;
            return _IsAutoRun;
        }

        private int GetDelayTime()
        {
            IniFile _iniFile = new IniFile(_configFile);
            int t_time = _iniFile.ReadInteger("Config", "DelayTime",20);
            _delayTime = t_time;
            return _delayTime;
        }

        private bool GetIsComEnable()
        {
            Config.Load(AppDomain.CurrentDomain.BaseDirectory + "Config.ini");//加载配置文件
            if (Utility.ConfigData.GetInstance().GetIsComEnable() == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SaveIsComEnable(bool p_iscom)
        {
            IniFile _iniFile = new IniFile(_configFile);
            if (p_iscom)
            {
                _iniFile.WriteString("Config", "IsComEnable", "1");
            }
            else
            {
                _iniFile.WriteString("Config", "IsComEnable", "0");
            }
        }

        private void SoftSetWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveAutoRun(cbAutoRun.Checked);
            SaveDelayTime(int.Parse(tbDelayTime.Text));
            SaveIsComEnable(cbIsComEnable.Checked);
        }

        private void SoftSetWindow_Load(object sender, EventArgs e)
        {
            cbAutoRun.Checked = GetAutoRun();
            tbDelayTime.Text = GetDelayTime().ToString();
            cbIsComEnable.Checked = GetIsComEnable();
        }

        private void cbAutoRun_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAutoRun.Checked)
            {
                //MessageBox.Show("设置开机自启动，需要修改注册表", "提示");
                string path = Application.ExecutablePath;
                RegistryKey rk = Registry.LocalMachine;
                RegistryKey rk2 = rk.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
                rk2.SetValue("ControlCenter", path);
                rk2.Close();
                rk.Close();
            }
            else//取消开机自启动
            {
                MessageBox.Show("取消开机自启动，需要修改注册表", "提示");
                string path = Application.ExecutablePath;
                RegistryKey rk = Registry.LocalMachine;
                RegistryKey rk2 = rk.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
                rk2.DeleteValue("ControlCenter", false);
                rk2.Close();
                rk.Close();
            }
        }

    }
}
