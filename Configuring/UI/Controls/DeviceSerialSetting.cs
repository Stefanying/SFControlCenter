using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Configuring.Business;

namespace Configuring.UI.Controls
{
    public partial class DeviceSerialSetting : BaseForm
    {

        private ComSetting _deviceComSetting;
        public ComSetting DeviceComSetting
        {
            get { return _deviceComSetting; }
            set { _deviceComSetting = value; }
        }


        string _prjName;
        public string PrjName
        {
            get { return _prjName; }
            set { _prjName = value; tbName.Text = value; }
        }


        int _baudRate;
        public int BaudRate
        {
            get { return _baudRate; }
            set { _baudRate = value; cbBaudrate.Text = _baudRate.ToString(); }
        }

        int _dataBit;
        public int Databit
        {
            get { return _dataBit; }
            set { _dataBit = value; cbDatabit.Text = value.ToString(); }
        }

        int _stopBit;
        public int StopBit
        {
            get { return _stopBit; }
            set { _stopBit = value; cbStopbit.Text = value.ToString(); }
        }

        Parity _parity;
        public Parity Parity
        {
            get { return _parity; }
            set { _parity = value; cbParity.SelectedIndex = (int)value; }
        }

        public DeviceSerialSetting()
        {
            InitializeComponent();
            cbBaudrate.Text = "9600";
            cbDatabit.Text = "8";
            cbStopbit.Text = "1";
            cbParity.SelectedIndex = 2;
        }

        private bool IsCom(string comNumber)
        {
            Regex rex = new Regex(@"com\d+$");
            return rex.IsMatch(comNumber);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            _prjName = tbName.Text;
            _deviceComSetting = new ComSetting();
            _baudRate =int.Parse(cbBaudrate.Text);
            _dataBit = int.Parse(cbDatabit.Text);
            _stopBit = int.Parse(cbStopbit.Text);
            _parity = (Parity)cbParity.SelectedIndex;
            _deviceComSetting.BaudRate = _baudRate;
            _deviceComSetting.StopBits = _stopBit;
            _deviceComSetting.DataBits = _dataBit;
            _deviceComSetting.Parity = _parity;
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void tbName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

    }
}
