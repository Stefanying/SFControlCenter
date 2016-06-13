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

        private List<RelayOperationDataList> _relayOperationDataList;
        public List<RelayOperationDataList> RelayOperationDataList
        {
            get { return _relayOperationDataList; }
            set { _relayOperationDataList = value; }
        }

        private RelayOperationDataList _currentRelayOpDataList;
        public RelayOperationDataList CurrentRelayOpDataList
        {
            get { return _currentRelayOpDataList; }
            set { _currentRelayOpDataList = value; }
        }

        private object _lock = new object();
        private int _selectRowIndex = 0;
        public UserRelayStateList()
        {
            InitializeComponent();

            if (dbRelayStateList.Columns.Count == 0)
            {
                dbRelayStateList.Columns.Add("name","命令");
                dbRelayStateList.Columns.Add("data","串口数据");

                dbRelayStateList.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                dbRelayStateList.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;


                dbRelayStateList.Columns[0].ReadOnly = true;
                dbRelayStateList.Columns[1].ReadOnly = true;
            }

            dbRelayStateList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            RefreshRelayStateList();
        }

        public void RefreshRelayStateList()
        {
            lock (_lock)
            {
                dbRelayStateList.Rows.Clear();
                _currentRelayOpDataList = null;
                if (_relayOperationDataList != null)
                {
                    foreach (RelayOperationDataList _relayOpData in _relayOperationDataList)
                    {
                        dbRelayStateList.Rows.Add(GetRelayStateType(RelayOperationType.吸合),_relayOpData.GetOperationData(RelayOperationType.吸合));
                        dbRelayStateList.Rows.Add(GetRelayStateType(RelayOperationType.断开),_relayOpData.GetOperationData(RelayOperationType.断开));
                    }
                    if (_relayOperationDataList != null && _relayOperationDataList.Count > 0)
                    {
                        dbRelayStateList.Rows[_relayOperationDataList.Count - 1].Selected = true;
                        _currentRelayOpDataList = _relayOperationDataList[_relayOperationDataList.Count - 1];
                    }
                }
            }
        }

        string GetRelayStateType(RelayOperationType _state)
        {
            string ret = "吸合";
            switch (_state)
            {
                case RelayOperationType.吸合:
                    ret = "吸合";
                    break;
                case RelayOperationType.断开:
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
        }

        private void dbPrjStateList_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 1 && e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                contextMenuStrip.Show(Cursor.Position);
            }
        }

        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentRelayOpDataList != null)
            {
                DeviceDataSetting dds = new DeviceDataSetting();
                Console.WriteLine(_selectRowIndex);
                dds.StateName = GetRelayStateType((RelayOperationType)_selectRowIndex);
                dds.Data = _currentRelayOpDataList.GetOperationData((RelayOperationType)_selectRowIndex);
                if (dds.ShowDialog() == DialogResult.OK)
                {
                    _currentRelayOpDataList.SetOperationData((RelayOperationType)_selectRowIndex,dds.Data);
                    RefreshRelayStateList();
                }
            }
        }

        private void dbPrjStateList_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_currentRelayOpDataList != null)
            {
                DeviceDataSetting dds = new DeviceDataSetting();
                Console.WriteLine(_selectRowIndex);
                dds.StateName = GetRelayStateType((RelayOperationType)_selectRowIndex);
                dds.Data = _currentRelayOpDataList.GetOperationData((RelayOperationType)_selectRowIndex);
                if (dds.ShowDialog() == DialogResult.OK)
                {
                    _currentRelayOpDataList.SetOperationData((RelayOperationType)_selectRowIndex, dds.Data);
                    RefreshRelayStateList();
                }
            }
        }

    }
}
