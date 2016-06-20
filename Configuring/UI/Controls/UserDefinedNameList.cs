using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Configuring.Business;

namespace Configuring.UI.Controls
{
    public partial class UserDefinedNameList : UserControl
    {
        public event EventHandler OnCurrentUserDefinedChanged;

        private List<UserDefinedOperation> _definedNames;
        public List<UserDefinedOperation> DefinedNames
        {
            get { return _definedNames; }
            set { _definedNames = value; }
        }

        private UserDefinedOperation _currentUserDefinedName;
        public UserDefinedOperation CurrentUserDefinedName
        {
            get { return _currentUserDefinedName; }
            set { _currentUserDefinedName = value; }
        }

        private object _lock = new object();
        private int _selectRowIndex = -1;

        public UserDefinedNameList()
        {
            InitializeComponent();

            if (dbAreaList.Columns.Count == 0)
            {
                dbAreaList.Columns.Add("name", "命令名称");
                dbAreaList.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                dbAreaList.Columns[0].ReadOnly =true;
            }

            dbAreaList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            RefreshAreaList();
        }

        //添加命令
        public void AddCommand(UserDefinedOperation area)
        {
            lock (_lock)
            {
                if (_definedNames != null)
                {
                    _definedNames.Add(area);
                }
                else
                {
                    Helper.ShowMessageBox("添加失败!", "添加展区失败！");
                }

                RefreshAreaList();
            }
        }

        public void DeleteArea(UserDefinedOperation area)
        {
            lock (_lock)
            {
                if (_definedNames != null)
                {
                    _definedNames.Remove(area);
                }

                RefreshAreaList();
            }
        }

        public void RefreshAreaList()
        {
            dbAreaList.Rows.Clear();
            _currentUserDefinedName = null;

            if (_definedNames != null)
            {
                foreach (UserDefinedOperation area in _definedNames)
                {
                    dbAreaList.Rows.Add(area.Name);
                }
            }

            if (dbAreaList.Rows.Count > 0)
            {
                dbAreaList.Rows[dbAreaList.Rows.Count - 1].Selected = true;
                _currentUserDefinedName = _definedNames[dbAreaList.Rows.Count - 1];
            }

            if (OnCurrentUserDefinedChanged != null)
            {
                OnCurrentUserDefinedChanged(this, null);
            }
        }

        private void dbCommandList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            _selectRowIndex = e.RowIndex;
            if (e.Clicks == 1 && e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                contextMenuStrip.Show(Cursor.Position);
            }

            if (_selectRowIndex >= 0 && _currentUserDefinedName != _definedNames[_selectRowIndex])
            {
                _currentUserDefinedName = _definedNames[_selectRowIndex];
                if (OnCurrentUserDefinedChanged != null)
                {
                    OnCurrentUserDefinedChanged(this, null);
                }

                dbAreaList.Rows[_selectRowIndex].Selected = true;
            }
        }

        private void dbCommandList_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 1 && e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                contextMenuStrip.Show(Cursor.Position);
            }
        }

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AreaSetting _userActionName = new AreaSetting();
            _userActionName.LbName = "命令名称";
            if (_userActionName.ShowDialog() == DialogResult.OK)
            {
                if (_definedNames != null)
                {
                    foreach (UserDefinedOperation area in _definedNames)
                    {
                        if (area.Name == _userActionName.AreaName)
                        {
                            Helper.ShowMessageBox("提示", "已存在相同名称！");
                            return;
                        }
                    }
                    UserDefinedOperation command = new UserDefinedOperation(_userActionName.AreaName);
                    AddCommand(command);
                }
                else
                {
                    UserDefinedOperation command = new UserDefinedOperation(_userActionName.AreaName);
                    _definedNames = new List<UserDefinedOperation>();
                    _definedNames.Add(command);
                    RefreshAreaList();
                }
            } 
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Helper.ShowMessageBox("确认", "确认删除此项？") == DialogResult.OK)
            {
                if (_definedNames != null && _definedNames.Count > 0 && _selectRowIndex != -1)
                {
                    UserDefinedOperation command = _definedNames[_selectRowIndex];
                    DeleteArea(command);
                }
            }
        }

        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentUserDefinedName != null)
            {
                AreaSetting _userActionName = new AreaSetting();
                _userActionName.AreaName = _currentUserDefinedName.Name;
                _userActionName.LbName = "命令名称";
                if (_userActionName.ShowDialog() == DialogResult.OK)
                {
                    foreach (UserDefinedOperation area in _definedNames)
                    {
                        if (area.Name == _userActionName.AreaName && area != _currentUserDefinedName)
                        {
                            Helper.ShowMessageBox("提示", "已存在相同名称！");
                            return;
                        }
                    }

                    _currentUserDefinedName.Name = _userActionName.AreaName;
                    RefreshAreaList();
                }
            }
        }     
    }
}
