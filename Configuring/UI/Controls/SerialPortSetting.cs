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
    public partial class SerialPortSetting : BaseForm
    {
        string _comNumber;
        public string ComNumber
        {
            get { return _comNumber; }
            set { _comNumber = value; cbComNumber.Text = value; }
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

        public SerialPortSetting()
        {
            InitializeComponent();

            cbComNumber.Text = "com1";
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
            _comNumber = cbComNumber.Text;
            _baudRate =int.Parse(cbBaudrate.Text);
            _dataBit = int.Parse(cbDatabit.Text);
            _stopBit = int.Parse(cbStopbit.Text);
            _parity = (Parity)cbParity.SelectedIndex;

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

    }
}
