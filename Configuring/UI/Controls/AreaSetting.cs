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
    public partial class AreaSetting : BaseForm
    {
        string _areaName;

        public string AreaName
        {
            get { return _areaName; }
            set { _areaName = value; tbName.Text = value; }
        }

        string _lbName;
        public string LbName
        {
            get { return _lbName; }
            set { _lbName = value; lbName.Text = value; }
        }

        public AreaSetting()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            _areaName = tbName.Text;
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
