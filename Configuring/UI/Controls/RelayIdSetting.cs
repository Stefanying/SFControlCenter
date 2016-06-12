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
    public partial class RelayIdSetting : BaseForm
    {
        int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; tbId.Text = value.ToString(); }
        }

        private string _data_On;
        public string Data_On
        {
            get { return _data_On; }
            set { _data_On = value; tbData_On.TextValue = value; }
        }

        private string _data_Off;
        public string Data_Off
        {
            get { return _data_Off; }
            set { _data_Off = value; tbData_Off.TextValue = value; }
        }



        public RelayIdSetting()
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
                _id = int.Parse(tbId.Text);
                _data_On = tbData_On.TextValue;
                _data_Off = tbData_Off.TextValue;
                if (CheckData(_data_On) && CheckData(_data_Off))
                {
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
            catch
            {
                Helper.ShowMessageBox("提示","数据格式不正确！");
 
            }
        }

        private void tbId_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar != '\b')
                    e.Handled = "0123456789".IndexOf(char.ToUpper(e.KeyChar))<0;
            }
            catch
            {
                Helper.ShowMessageBox("警告","只能输入的整数");
            }
        }

        private bool CheckData(string data)
        {
            bool ret = data.Length < 200;
            if (!ret)
            {
                Helper.ShowMessageBox("字符数超过长度","字符不能超过200");
            }
            return ret;
        }


    }
}
