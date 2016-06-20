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
    public partial class UserPrjStateList : UserControl
    {

        private List<UserPrjOperation> _PrjStates;
        public List<UserPrjOperation> PrjStates
        {
            get { return _PrjStates; }
            set { _PrjStates = value; }
        }

        private UserPrjOperation _currentPrjState;
        public UserPrjOperation CurrentPrjState
        {
            get { return _currentPrjState; }
            set { _currentPrjState = value; }
        }

        private object _lock = new object();
        private int _selectRowIndex = 0;
        public UserPrjStateList()
        {
            InitializeComponent();

            if (dbPrjStateList.Columns.Count == 0)
            {
                dbPrjStateList.Columns.Add("name","命令");
                dbPrjStateList.Columns.Add("data","串口数据");

                dbPrjStateList.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                dbPrjStateList.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;


                dbPrjStateList.Columns[0].ReadOnly = true;
                dbPrjStateList.Columns[1].ReadOnly = true;
            }

            dbPrjStateList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            RefreshPrjStateList();
        }

        public void AddPrjState(UserPrjOperation uds)
        {
            lock (_lock)
            {
                if (_PrjStates != null)
                {
                    _PrjStates.Add(uds);
                }

                RefreshPrjStateList();
            }
        }

        public void DeletePrjState(UserPrjOperation uds)
        {
            lock (_lock)
            {
                if (_PrjStates != null)
                {
                    _PrjStates.Remove(uds);
                }
                RefreshPrjStateList();
            }
        }

        public void RefreshPrjStateList()
        {
            lock (_lock)
            {
                dbPrjStateList.Rows.Clear();
                _currentPrjState = null;
                if (_PrjStates != null)
                {
                    foreach (UserPrjOperation uds in _PrjStates)
                    {
                        dbPrjStateList.Rows.Add(uds.PrjOperationType.ToString(),uds.Data);
                    }
                }

                if (_PrjStates != null && _PrjStates.Count > 0)
                {
                    dbPrjStateList.Rows[_PrjStates.Count - 1].Selected = true;
                    _currentPrjState = _PrjStates[_PrjStates.Count - 1];
                }
            }
        }

        private void dbPrjStateList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            _selectRowIndex = e.RowIndex;
            if (_selectRowIndex < 0) return;
            if (e.Clicks == 1 && e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                contextMenuStrip.Show(Cursor.Position);
            }

            if (_PrjStates != null && _PrjStates.Count > 0)
            {
                _currentPrjState = _PrjStates[_selectRowIndex];
                dbPrjStateList.Rows[_selectRowIndex].Selected = true;
            }
        }

        private void dbPrjStateList_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 1 && e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                contextMenuStrip.Show(Cursor.Position);
            }
        }

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeviceDataSetting dds = new DeviceDataSetting();
            if (dds.ShowDialog() == DialogResult.OK)
            {
                PrjOperationType _mode = (PrjOperationType)Enum.Parse(typeof(PrjOperationType),dds.StateName);
                UserPrjOperation uds = new UserPrjOperation(_mode,dds.Data);
                AddPrjState(uds);
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Helper.ShowMessageBox("确认", "确定删除？") == DialogResult.OK)
            {
                if (_PrjStates != null && _PrjStates.Count > 0 && _currentPrjState != null)
                {
                    if (_currentPrjState.PrjOperationType.ToString() == "开" || _currentPrjState.PrjOperationType.ToString() == "关")
                    {
                        Helper.ShowMessageBox("提示","该项不能删除!");
                    }
                    else 
                    {
                        DeletePrjState(_currentPrjState);
                        RefreshPrjStateList();
                    } 
                }
            }
        }

        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentPrjState != null)
            {
                DeviceDataSetting dds = new DeviceDataSetting();
                dds.StateName = _currentPrjState.PrjOperationType.ToString();
                dds.Data = _currentPrjState.Data;
                if (dds.ShowDialog() == DialogResult.OK)
                {
                    PrjOperationType _mode = (PrjOperationType)Enum.Parse(typeof(PrjOperationType), dds.StateName);
                    _currentPrjState.PrjOperationType = _mode;
                    _currentPrjState.Data = dds.Data;
                    RefreshPrjStateList();
                }
            }
        }

        private void dbPrjStateList_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_currentPrjState != null)
            {
                DeviceDataSetting dds = new DeviceDataSetting();
                dds.StateName =_currentPrjState.PrjOperationType.ToString();
                dds.Data = _currentPrjState.Data;
                if (dds.ShowDialog() == DialogResult.OK)
                {
                    PrjOperationType _mode = (PrjOperationType)Enum.Parse(typeof(PrjOperationType), dds.StateName);
                    _currentPrjState.PrjOperationType = _mode;
                    _currentPrjState.Data = dds.Data;
                    RefreshPrjStateList();
                }
            }
        }

    }
}
