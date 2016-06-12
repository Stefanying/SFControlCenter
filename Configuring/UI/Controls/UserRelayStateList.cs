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
    public partial class UserRelayStateList : UserControl
    {

        private List<UserDeviceState> _relayStates;
        public List<UserDeviceState> RelayStates
        {
            get { return _relayStates; }
            set { _relayStates = value; }
        }

        private UserDeviceState _currentRelayState;
        public UserDeviceState CurrentRelayState
        {
            get { return _currentRelayState; }
            set { _currentRelayState = value; }
        }

        private object _lock = new object();
        private int _selectRowIndex = 0;
        public UserRelayStateList()
        {
            InitializeComponent();

            if (dbRelayStateList.Columns.Count == 0)
            {
                dbRelayStateList.Columns.Add("name","状态");
                dbRelayStateList.Columns.Add("data","串口数据");

                dbRelayStateList.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                dbRelayStateList.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;


                dbRelayStateList.Columns[0].ReadOnly = true;
                dbRelayStateList.Columns[1].ReadOnly = true;
            }

            dbRelayStateList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            RefreshRelayStateList();
        }

        public void AddRelayState(UserDeviceState uds)
        {
            lock (_lock)
            {
                if (_relayStates != null)
                {
                    _relayStates.Add(uds);
                }

                RefreshRelayStateList();
            }
        }

        public void DeleteRelayState(UserDeviceState uds)
        {
            lock (_lock)
            {
                if (_relayStates != null)
                {
                    _relayStates.Remove(uds);
                }
                RefreshRelayStateList();
            }
        }

        public void RefreshRelayStateList()
        {
            lock (_lock)
            {
                dbRelayStateList.Rows.Clear();
                _currentRelayState = null;
                if (_relayStates != null)
                {
                    foreach (UserDeviceState uds in _relayStates)
                    {
                        dbRelayStateList.Rows.Add(GetRelayStateType(uds.RelaysState), uds.Data);
                    }
                }

                if (_relayStates != null && _relayStates.Count > 0)
                {
                    dbRelayStateList.Rows[_relayStates.Count - 1].Selected = true;
                    _currentRelayState = _relayStates[_relayStates.Count - 1];
                }
 
            }
        }

        string GetPrjModeType(PrjState _mode)
        {
            string ret = "开";
            switch (_mode)
            {
                case PrjState.开:
                    ret = "开";
                    break;
                case PrjState.关:
                    ret = "关";
                    break;
            }
            return ret;
        }

        string GetRelayStateType(RelayState _state)
        {
            string ret = "吸合";
            switch (_state)
            {
                case RelayState.吸合:
                    ret = "吸合";
                    break;
                case RelayState.断开:
                    ret = "断开";
                    break;
            }
            return ret;
        }

        private void dbPrjStateList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            _selectRowIndex = e.RowIndex;
            if (_selectRowIndex < 0) return;
            if (e.Clicks == 1 && e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                contextMenuStrip.Show(Cursor.Position);
            }

            if (_relayStates != null && _relayStates.Count > 0)
            {
                _currentRelayState = _relayStates[_selectRowIndex];
                dbRelayStateList.Rows[_selectRowIndex].Selected = true;
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
                RelayState _relayState = (RelayState)Enum.Parse(typeof(RelayState),dds.StateName);
                UserDeviceState uds = new UserDeviceState(_relayState,dds.Data);
                AddRelayState(uds);
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Helper.ShowMessageBox("确认", "确定删除？") == DialogResult.OK)
            {
                if (_relayStates != null && _relayStates.Count > 0 && _currentRelayState != null)
                {
                    DeleteRelayState(_currentRelayState);
                    RefreshRelayStateList();
                }
            }
        }

        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentRelayState != null)
            {
                DeviceDataSetting dds = new DeviceDataSetting();
                dds.StateName = GetRelayStateType(_currentRelayState.RelaysState);
                dds.Data = _currentRelayState.Data;
                if (dds.ShowDialog() == DialogResult.OK)
                {
                    RelayState _relayState = (RelayState)Enum.Parse(typeof(RelayState), dds.StateName);
                   _currentRelayState.RelaysState =_relayState;
                   _currentRelayState.Data = dds.Data;
                   RefreshRelayStateList();
                }
            }
        }

        private void dbPrjStateList_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_currentRelayState != null)
            {
                DeviceDataSetting dds = new DeviceDataSetting();
                dds.StateName = GetPrjModeType(_currentRelayState.DeviceMode);
                dds.Data = _currentRelayState.Data;
                if (dds.ShowDialog() == DialogResult.OK)
                {
                    PrjState _mode = (PrjState)Enum.Parse(typeof(PrjState), dds.StateName);
                    RelayState _relayState = (RelayState)Enum.Parse(typeof(RelayState), dds.StateName);
                    _currentRelayState.DeviceMode = _mode;
                    _currentRelayState.Data = dds.Data;
                    RefreshRelayStateList();
                }
            }
        }

    }
}
