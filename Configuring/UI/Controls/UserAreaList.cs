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
    public partial class UserAreaList : UserControl
    {
        public event EventHandler OnCurrentAreaChanged;

        private List<UserArea> _areas;
        public List<UserArea> Areas
        {
            get { return _areas; }
            set { _areas = value; }
        }

        private UserArea _currentArea;
        public UserArea CurrentArea
        {
            get { return _currentArea; }
            set { _currentArea = value; }
        }

        private object _lock = new object();
        private int _selectRowIndex = -1;

        public UserAreaList()
        {
            InitializeComponent();

            if (dbAreaList.Columns.Count == 0)
            {
                dbAreaList.Columns.Add("name", "展项名称");
                dbAreaList.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                dbAreaList.Columns[0].ReadOnly =true;
            }

            dbAreaList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            RefreshAreaList();
        }

        public void AddCommand(UserArea area)
        {
            lock (_lock)
            {
                if (_areas != null)
                {
                    _areas.Add(area);
                }
                else
                {
                    Helper.ShowMessageBox("添加失败!", "添加展区失败！");
                }

                RefreshAreaList();
            }
        }

        public void DeleteArea(UserArea area)
        {
            lock (_lock)
            {
                if (_areas != null)
                {
                    _areas.Remove(area);
                }

                RefreshAreaList();
            }
        }

        public void RefreshAreaList()
        {
            dbAreaList.Rows.Clear();
            _currentArea = null;

            if (_areas != null)
            {
                foreach (UserArea area in _areas)
                {
                    dbAreaList.Rows.Add(area.Name);
                }
            }

            if (dbAreaList.Rows.Count > 0)
            {
                dbAreaList.Rows[dbAreaList.Rows.Count - 1].Selected = true;
                _currentArea = _areas[dbAreaList.Rows.Count - 1];
            }

            if (OnCurrentAreaChanged != null)
            {
                OnCurrentAreaChanged(this, null);
            }
        }

        private void dbCommandList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            _selectRowIndex = e.RowIndex;
            if (e.Clicks == 1 && e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                contextMenuStrip.Show(Cursor.Position);
            }

            if (_selectRowIndex >= 0 && _currentArea != _areas[_selectRowIndex])
            {
                _currentArea = _areas[_selectRowIndex];
                if (OnCurrentAreaChanged != null)
                {
                    OnCurrentAreaChanged(this, null);
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
            AreaSetting areaSetting = new AreaSetting();
            if (areaSetting.ShowDialog() == DialogResult.OK)
            {
                if (_areas != null)
                {
                    foreach (UserArea area in _areas)
                    {
                        if (area.Name == areaSetting.AreaName)
                        {
                            Helper.ShowMessageBox("提示", "已存在相同名称！");
                            return;
                        }
                    }
                    UserArea command = new UserArea(areaSetting.AreaName);
                    AddCommand(command);
                }
                else
                {
                    UserArea command = new UserArea(areaSetting.AreaName);
                    _areas.Add(command);
                    RefreshAreaList();
                }
            } 
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Helper.ShowMessageBox("确认", "确认删除此项？") == DialogResult.OK)
            {
                if (_areas != null && _areas.Count > 0 && _selectRowIndex != -1)
                {
                    UserArea command = _areas[_selectRowIndex];
                    DeleteArea(command);
                }
            }
        }

        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentArea != null)
            {
                AreaSetting cs = new AreaSetting();
                cs.AreaName = _currentArea.Name;

                if (cs.ShowDialog() == DialogResult.OK)
                {
                    foreach (UserArea area in _areas)
                    {
                        if (area.Name == cs.AreaName && area != _currentArea)
                        {
                            Helper.ShowMessageBox("提示", "已存在相同名称！");
                            return;
                        }
                    }

                    _currentArea.Name = cs.AreaName;
                    RefreshAreaList();
                }
            }
        }

        private void 上移ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentArea != _areas[0])
            {
                int index = _areas.IndexOf(_currentArea);

                UserArea temp = new UserArea(_areas[index - 1].Name);
                temp.Actions = _areas[index - 1].Actions;

                _areas[index - 1] = _currentArea;
                _areas[index] = temp;

                RefreshAreaList();
            }
        }

        private void 下移ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentArea != _areas[_areas.Count - 1])
            {
                int index = _areas.IndexOf(_currentArea);

                UserArea temp = new UserArea(_areas[index + 1].Name);
                temp.Actions = _areas[index + 1].Actions;

                _areas[index + 1] = _currentArea;
                _areas[index] = temp;

                RefreshAreaList();
            }
        }
    }
}
