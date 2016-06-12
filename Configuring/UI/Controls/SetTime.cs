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
    public partial class SetTime : BaseForm
    {
        int _hour;
        public int Hour
        {
            get { return _hour; }
            set { _hour = value; }
        }

        int _minute;
        public int Minute
        {
            get { return _minute; }
            set { _minute = value; }
        }

        public SetTime()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                _hour = int.Parse(tbHour.Text);
                _minute = int.Parse(tbMinute.Text);

                DialogResult = DialogResult.OK;
            }
            catch
            {
                Helper.ShowMessageBox("提示", "数据格式错误！");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
