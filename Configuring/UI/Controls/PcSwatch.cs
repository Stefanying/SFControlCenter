﻿using System;
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
    public partial class PcSwatch : BaseForm
    {

        object _setting;
        public object Setting
        {
            get { return _setting; }
            set { _setting = value; }
        }

        private string _operationOneName;
        public string OperationOneName
        {
            get { return _operationOneName; }
            set { _operationOneName = value; tbName.Text = value; }
        }


        private string _operationTwoName;
        public string OperationTwoName
        {
            get { return _operationTwoName; }
            set { _operationTwoName = value; }
        }


        private List<string> _operationNameList=new List<string>();
        public List<string> OperationNameList
        {
            get { return _operationNameList; }
            set { _operationNameList = value; }
        }


        private string _data;
        public string Data
        {
            get { return _data; }
            set { _data = value; }
        }

        private List<string> _dataList=new List<string>();
        public List<string> DataList
        {
            get { return _dataList; }
            set { _dataList = value; }
        }


        private int _delayTime;
        public int DelayTime
        {
            get { return _delayTime; }
            set { _delayTime = value; tbTime.Text = value.ToString(); }
        }


        private List<UserRelayArray> _relayModuleList;
        public List<UserRelayArray> RelayModuleList
        {
            get { return _relayModuleList; }
            set { _relayModuleList = value; }
        }

        public PcSwatch(List<UserRelayArray> _relaysets)
        {
            InitializeComponent();    
            // InitUI();
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
                Helper.ShowMessageBox("错误",ex.Message);
 
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
            _dataList.Clear();
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
                                _dataList.Add(_userRelaySet.RelayOperationDatas[0].GetOperationData(RelayOperationType.吸合));
                                _dataList.Add(_userRelaySet.RelayOperationDatas[0].GetOperationData(RelayOperationType.断开));
                            }
                        }
                    }
 
                }
            }
        }

        private bool CheckTime(int time)
        {
            bool ret = (time < 7200000&&time>1000);
            if (!ret) Helper.ShowMessageBox("时间超时", "请确保时间小于72m且大于1m");
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

        private bool CheckData(List<string> _datalist)
        {
            bool ret = false;
            for (int i = 0; i < _datalist.Count; i++)
            {
                 ret = _datalist[i].Length < 200;
                if (!ret)
                {
                    Helper.ShowMessageBox("字符数超过长度", "字符不能超过200");
                }
                return ret;
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

                if (tbName.Text == "")
                {
                    _operationNameList.Add("电脑" + cbId.SelectedItem.ToString() + "开关机"+"步骤一");
                    _operationNameList.Add("电脑" + cbId.SelectedItem.ToString() + "开关机" + "步骤二");
                }
                else
                {
                    _operationNameList.Add(tbName.Text+"开关机步骤一");
                    _operationNameList.Add(tbName.Text+"开关机步骤二");
                }
                _delayTime = int.Parse(tbTime.Text);

                if (CheckTime(_delayTime) && CheckData(DataList))
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
