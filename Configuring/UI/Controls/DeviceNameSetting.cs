using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Configuring.UI.Controls
{
    public partial class DeviceNameSetting : BaseForm
    {
        string _deviceName;

        public string DeviceName
        {
            get { return _deviceName; }
            set { _deviceName = value; tbName.Text = value; }
        }

        public DeviceNameSetting()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            _deviceName = tbName.Text;
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
