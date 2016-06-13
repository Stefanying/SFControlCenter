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

        private List<UserRelaySetting> _relays;
        public List<UserRelaySetting> Relays
        {
            get { return _relays; }
            set { _relays = value; }
        }

        private UserRelaySetting _currentRelay;
        public UserRelaySetting CurrentRelay
        {
            get { return _currentRelay; }
            set { _currentRelay = value; }
        }

        //总路数
        private int t_ApproachCount=0;
        public int T_ApproachCount
        {
            get { return t_ApproachCount; }
            set { t_ApproachCount = value; }
        }

        private object _lock = new object();
        private int _selectRowIndex = 0;

        public UserRelaySettingList()
        {
            InitializeComponent();
            if (dbRelayNameList.Columns.Count == 0)
            {
                dbRelayNameList.Columns.Add("name", "继电器");

                dbRelayNameList.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
             
                dbRelayNameList.Columns[0].ReadOnly = true;
            }

            dbRelayNameList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dbRelayNameList.EnableHeadersVisualStyles = false;
            RefreshRelay();

        }

        public void AddRelayList(UserRelaySetting _relay)
        {
            lock (_lock)
            {
                if (_relays != null)
                {
                    _relays.Add(_relay);
                }
                else
                {
                    Helper.ShowMessageBox("添加失败!", "添加继电器失败");
                }
                RefreshRelay();
            }
        }

        public void DeleteRelayList(UserRelaySetting _relay)
        {
            lock (_lock)
            {
                if (_relays != null)
                {
                    _relays.Remove(_relay);
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
                foreach (UserRelaySetting relaysetting in _relays)
                {
                    dbRelayNameList.Rows.Add(relaysetting.RelayId.ToString());
                }
            }

            if (dbRelayNameList.Rows.Count > 0)
            {
                dbRelayNameList.Rows[dbRelayNameList.Rows.Count - 1].Selected = true;
                _currentRelay = _relays[dbRelayNameList.Rows.Count - 1];
            }

            if (OnCurrentRelayChanged != null)
            {
                OnCurrentRelayChanged(this, null);
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
                ris.Id = _currentRelay.RelayId;
                ris.T_ApproachCount = t_ApproachCount;
                ris.Data_On = _currentRelay.RelayOperationDatas[0].GetOperationData(RelayOperationType.吸合);
                ris.Data_Off = _currentRelay.RelayOperationDatas[0].GetOperationData(RelayOperationType.断开);
                if (ris.ShowDialog() == DialogResult.OK)
                {
                    if (!CheckId(ris.Id) || ris.Data_On == _currentRelay.RelayOperationDatas[0].GetOperationData(RelayOperationType.吸合) || ris.Data_Off == _currentRelay.RelayOperationDatas[0].GetOperationData(RelayOperationType.断开))
                    {
                        _currentRelay.RelayId = ris.Id;
                        _currentRelay.RelayOperationDatas[0].SetOperationData(RelayOperationType.吸合, ris.Data_On);
                        _currentRelay.RelayOperationDatas[0].SetOperationData(RelayOperationType.断开, ris.Data_Off);
                    }
                    else
                    {
                        Helper.ShowMessageBox("提示", "存在相同的继电器号!");
                    }
                    RefreshRelay();
                }
            }
            
        }

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_relays == null)
            {
                Helper.ShowMessageBox("提示","请选择对应的继电器模组！");
                return;
            }

            RelayIdSetting ris = new RelayIdSetting();
            ris.T_ApproachCount = t_ApproachCount;
            if (ris.ShowDialog() == DialogResult.OK)
            {
                if (!CheckId(ris.Id))
                {
                    UserRelaySetting _userRelaySetting = new UserRelaySetting(ris.Id,t_ApproachCount);
                    RelayOperationDataList _relayOperatinData = new RelayOperationDataList();
                    _relayOperatinData.SetOperationData(RelayOperationType.吸合,ris.Data_On);
                    _relayOperatinData.SetOperationData(RelayOperationType.断开,ris.Data_Off);
                    _userRelaySetting.AddRelayOperationData(_relayOperatinData);
                    AddRelayList(_userRelaySetting);
                }
                else
                {
                    Helper.ShowMessageBox("提示","存在相同的继电器号!");
                }
            }
        }


        private bool CheckId(int id)
        {
            for (int i = 0; i < _relays.Count; i++)
            {
                if (_relays[i].RelayId == id)
                {
                    return true;
                }
            }
            return false;
 
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Helper.ShowMessageBox("确认", "确定删除？") == DialogResult.OK)
            {
                if (_relays != null && _relays.Count > 0 && _selectRowIndex != -1)
                {
                    UserRelaySetting _userRelaySet = _relays[_selectRowIndex];
                    DeleteRelayList(_userRelaySet);
                }
            }

        }

        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentRelay != null)
            {
                RelayIdSetting ris = new RelayIdSetting();
                ris.Id = _currentRelay.RelayId;
                ris.T_ApproachCount = t_ApproachCount;
                ris.Data_On = _currentRelay.RelayOperationDatas[0].GetOperationData(RelayOperationType.吸合);
                ris.Data_Off = _currentRelay.RelayOperationDatas[0].GetOperationData(RelayOperationType.断开);
                if (ris.ShowDialog() == DialogResult.OK)
                {
                    //if (!CheckId(ris.Id) || ris.Data_On == _currentRelay.RelayOperationDatas[0].GetOperationData(RelayOperationType.吸合) || ris.Data_Off == _currentRelay.RelayOperationDatas[0].GetOperationData(RelayOperationType.断开))
                    if(CheckId(ris.Id))
                    {
                        _currentRelay.RelayId = ris.Id;
                        _currentRelay.RelayOperationDatas[0].SetOperationData(RelayOperationType.吸合,ris.Data_On);
                        _currentRelay.RelayOperationDatas[0].SetOperationData(RelayOperationType.断开,ris.Data_Off);
                    }
                    else
                    {
                        Helper.ShowMessageBox("提示", "存在相同的继电器号!");
                    }
                    RefreshRelay();
                }
            }
        }
    }
}
