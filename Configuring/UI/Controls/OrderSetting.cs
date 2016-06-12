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
    public partial class OrderSetting : BaseForm
    {
        int _hour;
        public int Hour
        {
            get { return _hour; }
            set { _hour = value; tbHour.Text = value.ToString(); }
        }

        int _minute;
        public int Minute
        {
            get { return _minute; }
            set { _minute = value; tbMinute.Text = value.ToString(); }
        }

        public OrderSetting()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                _hour = int.Parse(tbHour.Text);
                _minute = int.Parse(tbMinute.Text);

                if (_hour <= 24 && _minute <= 59)
                {
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    Helper.ShowMessageBox("格式错误", "数据格式错误！");
                }
            }
            catch
            {
                Helper.ShowMessageBox("格式错误", "数据格式错误！");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
