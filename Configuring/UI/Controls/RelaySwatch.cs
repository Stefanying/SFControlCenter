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

 

        //多个继电器模块组成的
        private List<UserRelayArray> _relayModuleList;
        public List<UserRelayArray> RelayModuleList
        {
            get { return _relayModuleList; }
            set { _relayModuleList = value; }
        }

        public RelaySwatch(List<UserRelayArray> _relaysets)
        {
            InitializeComponent();
            try
            {
                _relayModuleList = _relaysets;
                foreach (UserRelayArray _relayArray in _relaysets)
                {
                    cbRelayName.Items.Add(_relayArray.Name);
                }

                if (cbRelayName.Items.Count > 0)
                {
                    cbRelayName.SelectedIndex = 0;
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

        private void cbRelayName_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbId.Items.Clear();
            if (_relayModuleList.Count > 0 && _relayModuleList != null)
            {
                for (int i = 0; i < _relayModuleList.Count; i++)
                {
                    if (cbRelayName.SelectedItem.ToString() == _relayModuleList[i].Name)
                    {
                        _comNumber = _relayModuleList[i].RelayCom.ComNumber;
                        _baudRate = _relayModuleList[i].RelayCom.BaudRate;
                        _dataBits = _relayModuleList[i].RelayCom.DataBits;
                        _stopBits = _relayModuleList[i].RelayCom.StopBits;
                        _parity = _relayModuleList[i].RelayCom.Parity;
                        foreach (UserRelaySetting _userRelayset in _relayModuleList[i].RelayOperationDatas)
                        {
                            cbId.Items.Add(_userRelayset.RelayId);
                        }
                    }
                }
            }
            if (cbId.Items.Count > 0)
            {
                cbId.SelectedIndex = 0;
            }
        }


        private void cbId_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbState.Items.Clear();
            if (_relayModuleList.Count > 0 && _relayModuleList != null)
            {
                for (int i = 0; i < _relayModuleList.Count; i++)
                {
                    if (cbRelayName.SelectedItem.ToString() == _relayModuleList[i].Name)
                    {
                        foreach (UserRelaySetting _userRelaySet in _relayModuleList[i].RelayOperationDatas)
                        {
                            if (cbId.SelectedItem.ToString() == _userRelaySet.RelayId.ToString())
                            {
                                cbState.Items.Add(RelayOperationType.吸合.ToString());
                                cbState.Items.Add(RelayOperationType.断开.ToString());
                            }
                        }
                    }
                }
            }
            if (cbState.Items.Count > 0)
            {
                cbState.SelectedIndex = 0;
 
            }
        }

        string GetRelayStateType(RelayOperationType _state)
        {
            string ret = "吸合";
            switch (_state)
            {
                case RelayOperationType.吸合:
                    ret = "吸合";
                    break;
                case RelayOperationType.断开:
                    ret = "断开";
                    break;
            }
            return ret;
        }

        private void cbState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_relayModuleList.Count > 0 && _relayModuleList != null)
            {
                for (int i = 0; i < _relayModuleList.Count; i++)
                {
                    if (cbRelayName.SelectedItem.ToString() == _relayModuleList[i].Name)
                    {
                        foreach (UserRelaySetting _userRelaySet in _relayModuleList[i].RelayOperationDatas)
                        {
                            if (cbId.SelectedItem.ToString() == _userRelaySet.RelayId.ToString())
                            {
                                if (cbState.SelectedItem.ToString() == GetRelayStateType(RelayOperationType.吸合))
                                {
                                    Data = _userRelaySet.RelayOperationDatas[0].GetOperationData(RelayOperationType.吸合);
                                }
                                else if (cbState.SelectedItem.ToString() == GetRelayStateType(RelayOperationType.断开))
                                {
                                    Data = _userRelaySet.RelayOperationDatas[0].GetOperationData(RelayOperationType.断开);
                                }
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
                    _operationName = cbRelayName.SelectedItem.ToString()+cbState.SelectedItem.ToString() + cbId.SelectedItem.ToString();
                }
                else
                {
                    _operationName = tbName.Text;
 
                }
                _time = int.Parse(tbTime.Text);

                if (Data.Length <= 0)
                {
                    Helper.ShowMessageBox("提示","数据不为空!");
                    return;
                }

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
