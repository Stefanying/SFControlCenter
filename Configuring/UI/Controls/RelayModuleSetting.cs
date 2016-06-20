using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Configuring.Business;
namespace Configuring.UI.Controls
{
    public partial class RelayModuleSetting : BaseForm
    {

        string _relayName;
        public string RelayName
        {
            get { return _relayName; }
            set { _relayName = value; tbName.Text = value; }
        }

        ComSetting _relayCom= new ComSetting();
        public ComSetting RelayCom
        {
            get { return _relayCom; }
            set { _relayCom = value; }
        }

        int _relayCount;
        public int RelayCount
        {
            get { return _relayCount; }
            set { _relayCount = value; tbRelayCount.Text = value.ToString(); }
        }

        public RelayModuleSetting()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (_relayCom == null)
                {
                    Helper.ShowMessageBox("异常", "串口数据未配置!");
                    return;
                }

                _relayName = tbName.Text;
                _relayCount = int.Parse(tbRelayCount.Text);
                if (_relayCom != null)
                {
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
            catch
            {
 
            }
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            SerialPortSetting settingForm = new SerialPortSetting();
            if (_relayCom != null)
            {
               // settingForm.ComNumber = _relayCom.ComNumber;
                settingForm.BaudRate = _relayCom.BaudRate;
                settingForm.Databit = _relayCom.DataBits;
                settingForm.Parity = _relayCom.Parity;
            }
            if (settingForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _relayCom.ComNumber = settingForm.ComNumber;
                _relayCom.BaudRate = settingForm.BaudRate;
                _relayCom.DataBits = settingForm.Databit;
                _relayCom.StopBits = settingForm.StopBit;
                _relayCom.Parity = settingForm.Parity;
            }
        }

        private void tbCount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b')
                e.Handled = "123456789".IndexOf(char.ToUpper(e.KeyChar))<0;
        }
    }
}
