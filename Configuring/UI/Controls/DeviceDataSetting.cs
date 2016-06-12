using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Configuring.Business;
namespace Configuring.UI.Controls
{
    public partial class DeviceDataSetting : BaseForm
    {
        public DeviceDataSetting()
        {
            InitializeComponent();
        }

        private string _stateName;
        public string StateName
        {
            get { return _stateName; }
            set { _stateName = value; tbStateName.Text = value; }
        }

       private string _data;
        public string Data
        {
            get { return _data; }
            set { _data = value; tbData.TextValue = value; }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                _stateName = tbStateName.Text;
                _data = tbData.TextValue;

                if (CheckData(_data))
                {
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
            catch
            {
                Helper.ShowMessageBox("提示","数据格式不对!");
            }
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
    }
}
