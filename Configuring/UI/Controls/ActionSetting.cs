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
    public partial class ActionSetting : BaseForm
    {
        string _actionName;

        public string ActionName
        {
            get { return _actionName; }
            set { _actionName = value; tbName.Text = value; }
        }

        string _actionCode;

        public string ActionCode
        {
            get { return _actionCode; }
            set { _actionCode = value; tbCustomcode.Text = value; }
        }

        public ActionSetting()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            _actionName = tbName.Text;
            _actionCode = tbCustomcode.Text;

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        
    }
}
