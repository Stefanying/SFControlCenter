using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Configuring.Business;
using System.Xml;
using Configuring.Utility;
namespace Configuring.UI.Controls
{
    public partial class PrjSwitch : BaseForm
    {
        object _setting;
        public object Setting
        {
            get { return _setting; }
            set { _setting = value; }
        }

        string _operationName;
        public string OperationName
        {
            get { return _operationName; }
            set { _operationName = value; tbName.Text = value; }
        }

        string _data;
        public string Data
        {

            get { return _data; }
            set { _data = value; }
        }

        int _time;
        public int DelayTime
        {
            get { return _time; }
            set { _time = value; tbTime.Text = value.ToString(); }
        }

        string _comNumber;
        public string ComNumber
        {
            get { return _comNumber; }
            set { _comNumber = value; cbSerialPort.Text = value; }
        }

        private List<UserPrjSetting> _prjSetingList;
        public List<UserPrjSetting> PrjSetingList
        {
            get { return _prjSetingList; }
            set { _prjSetingList = value; }
        }


        int _baudRate = 0;
        int _dataBits = 0;
        int _stopBits = 0;
        Parity _parity;

        public PrjSwitch(List<UserPrjSetting> _ups)
        {
            InitializeComponent();
            try
            {
                _prjSetingList = _ups;

                foreach (UserPrjSetting ups in _ups)
                {
                    cbPrjType.Items.Add(ups.Name);
                }

                if (cbPrjType.Items.Count > 0)
                {
                    cbPrjType.SelectedIndex = 0;
                }

                for (int i = 0; i < ConfigData.GetInstance().GetSerialPortCount(); i++)
                {
                    int _port = i;
                    cbSerialPort.Items.Add("com"+(_port+1).ToString());
                }

                if (cbSerialPort.Items.Count > 0)
                {
                    cbSerialPort.SelectedIndex = 0;
                }
                
            }
            catch (Exception ex)
            {
                Helper.ShowMessageBox("异常",ex.Message);
            }        
        }

        private void cbPrjType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbMode.Items.Clear();
            for (int i = 0; i < _prjSetingList.Count; i++)
            {
                if (cbPrjType.SelectedItem.ToString() == _prjSetingList[i].Name)
                {
                    foreach (UserPrjOperation uds in _prjSetingList[i].DeviceStates)
                    {
                        cbMode.Items.Add(uds.PrjOperationType.ToString());
                    }
                    _baudRate = _prjSetingList[i].Pcs.BaudRate;
                    _dataBits = _prjSetingList[i].Pcs.DataBits;
                    _stopBits = _prjSetingList[i].Pcs.StopBits;
                    _parity = _prjSetingList[i].Pcs.Parity;
                }
            }

                if (cbMode.Items.Count >= 0)
                {
                    cbMode.SelectedIndex = 0;
                }
        }

        private void cbMode_SelectIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < _prjSetingList.Count; i++)
            {
                if (cbPrjType.SelectedItem.ToString() == _prjSetingList[i].Name)
                {
                    foreach (UserPrjOperation uds in _prjSetingList[i].DeviceStates)
                    {
                        if (cbMode.SelectedItem.ToString() == uds.PrjOperationType.ToString())
                        {
                            Data = uds.Data;
                        }
                    }
                }
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
      
        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                ComSetting cs = new ComSetting();
                cs.ComNumber = cbSerialPort.Text;
                cs.BaudRate = _baudRate;
                cs.DataBits = _dataBits;
                cs.StopBits = _stopBits;
                cs.Parity = _parity;
                _setting = cs;

                if(Setting ==null)
                {
                    Helper.ShowMessageBox("设置","先进行数据配置");
                    return ;
                }

                if (tbName.Text == "")
                {
                    _operationName = cbPrjType.SelectedItem.ToString() + cbMode.SelectedItem.ToString();
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
                Helper.ShowMessageBox("提示","数据格式不对！");
            }
        }

        private void tbTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b') e.Handled = "0123456789".IndexOf(char.ToUpper(e.KeyChar)) < 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

     

    }
}
