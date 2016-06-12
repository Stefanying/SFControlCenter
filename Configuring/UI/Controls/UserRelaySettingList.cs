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
    public partial class UserRelaySettingList : UserControl
    {

        public event EventHandler OnCurrentRelayChanged;

        private List<RelaySetting> _relays;
        public List<RelaySetting> Relays
        {
            get { return _relays; }
            set { _relays = value; }
        }

        private RelaySetting _currentRelay;
        public RelaySetting CurrentRelay
        {
            get { return _currentRelay; }
            set { _currentRelay = value; }
        }

        private object _lock = new object();
        private int _selectRowIndex = 0;

        public UserRelaySettingList()
        {
            InitializeComponent();
            if (dbRelayNameList.Columns.Count == 0)
            {
                dbRelayNameList.Columns.Add("name", "继电器路数");

                dbRelayNameList.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
             
                dbRelayNameList.Columns[0].ReadOnly = true;
            }

            dbRelayNameList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dbRelayNameList.EnableHeadersVisualStyles = false;
            RefreshRelay();

        }

        public void AddRelayList(RelaySetting relayid)
        {
            lock (_lock)
            {
                if (_relays != null)
                {
                    _relays.Add(relayid);
                }
                else
                {
                    Helper.ShowMessageBox("添加失败!","添加继电器失败");
                }
                RefreshRelay();
            }
        }

        public void DeleteRelayList(RelaySetting relayid)
        {
            lock (_lock)
            {
                if (_relays != null)
                {
                    _relays.Remove(relayid);
                }

                RefreshRelay();
            }
        }

        public void RefreshRelay()
        {
            dbRelayNameList.Rows.Clear();
            _currentRelay = null;

            if (_relays != null)
            {
                foreach (RelaySetting relayid in _relays)
                {
                    dbRelayNameList.Rows.Add(relayid.Id.ToString());
                }
            }

            if (dbRelayNameList.Rows.Count > 0)
            {
                dbRelayNameList.Rows[dbRelayNameList.Rows.Count - 1].Selected = true;
                _currentRelay = _relays[dbRelayNameList.Rows.Count - 1];
            }

            if (OnCurrentRelayChanged != null)
            {
                OnCurrentRelayChanged(this,null);
            }
            
        }

        string GetParityModeType(Parity _parity)
        {
            string ret = "None";
            switch (_parity)
            {
                case Parity.Even:
                    ret = "Even";
                    break;
                case Parity.None:
                    ret = "None";
                    break;
                case Parity.Odd:
                    ret = "Odd";
                    break;
            }
            return ret;
        }

        private void dbComDataList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            _selectRowIndex = e.RowIndex;
            if (_selectRowIndex < 0) return;
            if (e.Clicks == 1 && e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                contextMenuStrip.Show(Cursor.Position);
            }
            if (_selectRowIndex >= 0 && _currentRelay != _relays[_selectRowIndex])
            {
                _currentRelay = _relays[_selectRowIndex];
                if (OnCurrentRelayChanged != null)
                {
                    OnCurrentRelayChanged(this,null);
                }
                dbRelayNameList.Rows[_selectRowIndex].Selected = true;
            }
        }

        private void dbComDataList_MouseClick(object sender, MouseEventArgs e)
        {

            if (e.Clicks == 1 && e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                contextMenuStrip.Show(Cursor.Position);
            }
        }

        private void dbComDataList_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_currentRelay != null)
            {
                RelayIdSetting ris = new RelayIdSetting();
                ris.Id = _currentRelay.Id;
                if (ris.ShowDialog() == DialogResult.OK)
                {
                    foreach (RelaySetting relayid in _relays)
                    {
                        if (relayid.Id == ris.Id)
                        {
                            Helper.ShowMessageBox("提示", "该路数已存在!");
                            return;
                        }
                    }
                    RelaySetting _relayid = new RelaySetting(ris.Id);
                    AddRelayList(_relayid);
                }
            }
        }

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RelayIdSetting ris = new RelayIdSetting();
            if (ris.ShowDialog() == DialogResult.OK)
            {
                foreach (RelaySetting relayid in _relays)
                {
                    if (relayid.Id == ris.Id)
                    {
                        Helper.ShowMessageBox("提示","该路数已存在!");
                        return;
                    }
                }

                RelaySetting _relayid = new RelaySetting(ris.Id);
                UserDeviceState _relay_On = new UserDeviceState(RelayState.吸合,ris.Data_On);
                UserDeviceState _relay_Off = new UserDeviceState(RelayState.断开, ris.Data_Off);
                _relayid.RelayStates.Add(_relay_On);
                _relayid.RelayStates.Add(_relay_Off);
                AddRelayList(_relayid);
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Helper.ShowMessageBox("确认", "确定删除？") == DialogResult.OK)
            {
                if (_relays != null && _relays.Count > 0 && _selectRowIndex != -1)
                {
                    RelaySetting _relayid = _relays[_selectRowIndex];
                    DeleteRelayList(_relayid);
                }
            }
        }

        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentRelay != null)
            {
                RelayIdSetting ris = new RelayIdSetting();
                ris.Id = _currentRelay.Id;
                if (ris.ShowDialog() == DialogResult.OK)
                {
                    foreach (RelaySetting relayid in _relays)
                    {
                        if (relayid.Id == ris.Id)
                        {
                            Helper.ShowMessageBox("提示", "该路数已存在!");
                            return;
                        }
                    }
                    RelaySetting _relayid = new RelaySetting(ris.Id);
                    AddRelayList(_relayid);
                }
 
            }
        }
    }
}
