using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Configuring.Business;
using System.Text.RegularExpressions;

namespace Configuring.UI.Controls
{
    public partial class OprationSetting : BaseForm
    {
        object _setting;
        public object Setting
        {
            get { return _setting; }
            set { _setting = value; }
        }

        string _oprationName;
        public string OprationName
        {
            get { return _oprationName; }
            set { _oprationName = value; tbName.Text = value; }
        }

        string _oprationType;
        public string CommunicationType
        {
            get { return _oprationType; }
            set { _oprationType = value; cbOprationType.Text = value; }
        }

        string _dataType;
        public string DataType
        {
            get { return _dataType; }
            set { _dataType = value; cbDataType.Text = value; }
        }

        string _data;
        public string Data
        {
            get { return _data; }
            set { _data = value; tbData.TextValue = value; }
        }

        int _time;
        public int DelayTime
        {
            get { return _time; }
            set { _time = value; tbTime.Text = value.ToString(); }
        }

        public OprationSetting()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (Setting == null)
                {
                    Helper.ShowMessageBox("设置", "先进行数据配置");
                    return;
                }

                _oprationName = tbName.Text;
                _oprationType = cbOprationType.Text;
                _time = int.Parse(tbTime.Text);
                _dataType = cbDataType.Text;
                _data = tbData.TextValue;

                if (CheckTime(_time) && CheckData(_data))
                {
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
            catch
            {
                Helper.ShowMessageBox("提示", "数据格式不对！");
            }
        }

        private bool CheckTime(int time)
        {
            bool ret = time < 7200000;
            if (!ret) Helper.ShowMessageBox("时间超时", "请确保时间小于12m");
            return ret;
        }

        private bool CheckData(string data)
        {
            bool ret = data.Length < 200;
            if (!ret)
            {
                Helper.ShowMessageBox("字符数超过长度", "字符不能超过200");
            }

            return ret;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        //数据格式变化
        private void cbDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbDataType.Text == "十六进制")
            {
                tbData.Mode = EditorMode.Hex;
                tbData.TextValue = "";
            }
            else
            {
                tbData.Mode = EditorMode.Character;
                tbData.TextValue = "";
            }
        }

        //时间只能输入整数
        private void tbTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b') e.Handled = "0123456789".IndexOf(char.ToUpper(e.KeyChar)) < 0;
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            if (cbOprationType.Text == "TCP" || cbOprationType.Text == "UDP")
            {
                IpSetting settingForm = new IpSetting();
                if (Setting != null && (Setting as NetworkSetting) != null)
                {
                    NetworkSetting ns = (NetworkSetting)Setting;
                    settingForm.Ip = ns.Ip;
                    settingForm.Port = ns.Port;
                }

                if (settingForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    NetworkSetting ns = new NetworkSetting();
                    ns.Ip = settingForm.Ip;
                    ns.Port = settingForm.Port;

                    Setting = ns;
                }
            }
            else if (cbOprationType.Text == "串口")
            {
                SerialPortSetting settingForm = new SerialPortSetting();
                if (Setting != null && (Setting as ComSetting) != null)
                {
                    ComSetting cs = (ComSetting)Setting;
                    settingForm.ComNumber =cs.ComNumber ;
                    settingForm.BaudRate = cs.BaudRate;
                    settingForm.Databit = cs.DataBits;
                    settingForm.StopBit = cs.StopBits;
                    settingForm.Parity =  cs.Parity;
                }

                if (settingForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    ComSetting cs = new ComSetting();
                    cs.ComNumber = settingForm.ComNumber;
                    cs.BaudRate = settingForm.BaudRate;
                    cs.StopBits = settingForm.StopBit;
                    cs.DataBits = settingForm.Databit;
                    cs.Parity = settingForm.Parity;

                    Setting = cs;
                }
            }
        }
    }
}
