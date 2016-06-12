using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Configuring.UI
{
    class Helper
    {
        public static DialogResult ShowMessageBox(string caption, string text, MessageBoxButtons msgButtons = MessageBoxButtons.OK, MessageBoxIcon ico = MessageBoxIcon.Information)
        {
            EnsureBox msgbox = new EnsureBox();
            return msgbox.Show(caption, text, msgButtons, ico);
        }
    }
}
