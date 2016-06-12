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
    public enum EditorMode
    {
        Hex = 0,
        Character = 1
    }

    public partial class TextEdit : UserControl
    {

        EditorMode _mode = EditorMode.Character;

        public EditorMode Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        string _textValue = "";

        public string TextValue
        {
            get { _textValue = editor.Text; return _textValue; }
            set { _textValue = value;
                  editor.Text = value; }
        }

        public TextEdit()
        {
            InitializeComponent();
        }

        private void editor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (_mode == EditorMode.Hex)
            {
                if(e.KeyChar != '\b') e.Handled = "0123456789ABCDEF".IndexOf(char.ToUpper(e.KeyChar)) < 0  ;
            }
        }   
    }
}
