using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Configuring.Business;
namespace Configuring.UI.Controls
{
    public partial class RelaySwatch : BaseForm
    {

        object _setting;
        public object Setting
        {
            get { return _setting; }
            set { _setting = value; }
        }

        private string _operationName;
        public string OperationName
        {
            get { return _operationName; }
            set { _operationName = value; tbName.Text = value; }
        }

        private string _data;
        public string Data
        {
            get { return _data; }
            set { _data = value; }
        }

        private int _time;
        public int DelayTime
        {
            get { return _time; }
            set { _time = value; tbTime.Text = value.ToString(); }
        }

        private List<RelaySetting> _relaySettings;
        public List<RelaySetting> RelaySettings
        {
            get { return _relaySettings; }
            set { _relaySettings = value; }
        }

        private ComSetting _relayCom;

        public RelaySwatch(List<RelaySetting> _relaysets, ComSetting relayCom)
        {
            InitializeComponent();
            try
            {
                _relaySettings = _relaysets;
                _relayCom = relayCom;
                foreach (RelaySetting _rs in _relaysets)
                {
                    cbId.Items.Add(_rs.Id);
                }

                _comNumber = relayCom.ComNumber;
                _baudRate = relayCom.BaudRate;
                _dataBits = relayCom.DataBits;
                _stopBits = relayCom.StopBits;
                _parity = relayCom.Parity;


                if (cbId.Items.Count > 0)
                {
                    cbId.SelectedIndex = 0;
                }
            }
            catch(Exception ex)
            {
                Helper.ShowMessageBox("错误", ex.Message);
            }
        }

        string _comNumber = "";
        int _baudRate = 0;
        int _dataBits = 0;
        int _stopBits = 0;
        Parity _parity;


        private void cbId_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbState.Items.Clear();
            if (_relaySettings.Count > 0 && _relaySettings != null)
            {
                for (int i = 0; i < _relaySettings.Count; i++)
                {
                    if (cbId.SelectedItem.ToString() == _relaySettings[i].Id.ToString())
                    {
                        foreach (UserDeviceState uds in _relaySettings[i].RelayStates)
                        {
                            cbState.Items.Add(uds.RelaysState.ToString());
                        }
 
                    }
                }
            }

       
        }

        private void cbState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_relaySettings.Count > 0 && _relaySettings != null)
            {
                for (int i = 0; i < _relaySettings.Count; i++)
                {
                    if (cbId.SelectedItem.ToString() == _relaySettings[i].Id.ToString())
                    {
                        foreach (UserDeviceState uds in _relaySettings[i].RelayStates)
                        {
                            if (cbState.SelectedItem.ToString() == uds.RelaysState.ToString())
                            {
                                Data = uds.Data;
                            }
                        }

                    }
                }
            }

            if (cbState.Items.Count > 0)
            {
                cbState.SelectedItem = 0;
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

        private void tbTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b') e.Handled = "0123456789".IndexOf(char.ToUpper(e.KeyChar)) < 0;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                ComSetting cs = new ComSetting();
                cs.ComNumber = _comNumber;
                cs.BaudRate = _baudRate;
                cs.DataBits = _dataBits;
                cs.StopBits = _stopBits;
                cs.Parity = _parity;
                _setting = cs;
                if (Setting == null)
                {
                    Helper.ShowMessageBox("设置", "先进行数据配置");
                    return;
                }
                if (tbName.Text =="")
                {
                    _operationName = cbState.SelectedItem.ToString() + cbId.SelectedItem.ToString();
                }
                else
                {
                    _operationName = tbName.Text;
 
                }
                _time = int.Parse(tbTime.Text);

                if (CheckTime(_time) && CheckData(Data))
                {
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
            catch
            {
                Helper.ShowMessageBox("提示", "数据格式不对！");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }


    }
}
