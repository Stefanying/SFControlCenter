using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Configuring.UI.Controls
{
    public partial class IpSetting : BaseForm
    {
        string _ip;

        public string Ip
        {
            get { return _ip; }
            set { _ip = value; tbIP.Text = value; }
        }
        int _port;
        public int Port
        {
            get { return _port; }
            set { _port = value; tbPort.Text = value.ToString(); }
        }

        public IpSetting()
        {
            InitializeComponent();
        }

        private bool IsIPString(string ipadress)
        {
            string pattrn = @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$";
            Regex rex = new Regex(pattrn);
            return rex.IsMatch(ipadress);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                _ip = tbIP.Text;
                if (IsIPString(_ip))
                {
                    _port = int.Parse(tbPort.Text);
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    Helper.ShowMessageBox("IP地址错误！", "IP地址格式不对！");
                }
            }
            catch
            {
                Helper.ShowMessageBox("保存失败", "数据格式错误！");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }


    }
}
