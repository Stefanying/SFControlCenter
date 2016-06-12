using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Configuring.UI.Controls
{
    public partial class UserCheckBoxList : UserControl
    {
        public UserCheckBoxList()
        {
            InitializeComponent();
          
        }




        public void AddCheckBox()
        {
            for (int i = 0; i < 8; i++)
            {
                CheckBox cnCheckBox = new CheckBox();
                cnCheckBox.Left = 1+ i * 10;
                cnCheckBox.Text = i.ToString();
                cnCheckBox.CheckAlign = ContentAlignment.MiddleLeft;
                cnCheckBox.AutoCheck = true;
                this.Controls.Add(cnCheckBox);
            }
        }



    }
}
