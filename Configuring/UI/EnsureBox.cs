using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Configuring.UI
{
    public partial class EnsureBox : Form
    {
        public EnsureBox()
        {
            InitializeComponent();
        }

        public virtual DialogResult Show(string caption, string text, MessageBoxButtons msgButtons, MessageBoxIcon ico)
        {
            lbCaption.Text = caption;
            lbInfo.Text = text;
            InitUIStyle(msgButtons);
            InitStateImage(ico);

            return this.ShowDialog();
        }

        private void InitStateImage(MessageBoxIcon ico)
        {
            switch(ico)
            {
                case MessageBoxIcon.Error:
                    picType.Image = UI.Images.Error;
                    break;
                case MessageBoxIcon.Warning:
                    picType.Image = UI.Images.Warning;
                    break;
                default:
                    picType.Image = UI.Images.Info;
                    break;
            }
        }

        private void InitUIStyle(MessageBoxButtons msbuttons)
        {
            btnCancel.Visible = false;
            btnOk.Visible = false;

            switch (msbuttons)
            {
                case MessageBoxButtons.OK:
                    btnOk.Visible = true;
                    break;
                case MessageBoxButtons.YesNo:
                    btnOk.Visible = true;
                    btnCancel.Visible = true;
                    break;
                default:
                    btnOk.Visible = true;
                    btnCancel.Visible = true;
                    break;

            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
