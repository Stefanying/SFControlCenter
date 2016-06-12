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
    public partial class UserRelayList : UserControl
    {
        public event EventHandler OnCurrentRelayChanged;

        private List<UserRelay> _relays;
        public List<UserRelay> Relays
        {
            get { return _relays; }
            set { _relays = value; }
        }

        private UserRelay _currentRelay;
        public UserRelay CurrentRelay
        {
            get { return _currentRelay; }
            set { _currentRelay = value; }
        }



        private object _lock = new object();
        private int _selectRowIndex = -1;

        public UserRelayList()
        {
            InitializeComponent();

            if (dbRelayList.Columns.Count == 0)
            {
                dbRelayList.Columns.Add("name", "继电器名称");
                dbRelayList.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                dbRelayList.Columns[0].ReadOnly =true;
            }

            dbRelayList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            RefreshRelayList();
        }

        public void AddCommand(UserRelay _relay)
        {
            lock(_lock)
            {
                if (_relays != null)
                {
                    _relays.Add(_relay);
                }
                else
                {
                    Helper.ShowMessageBox("添加失败!","添加继电器失败");
                }
                RefreshRelayList();
            }
        
        }

        public void DeleteCommand(UserRelay _relay)
        {
            lock (_lock)
            {
                if (_relays != null)
                {
                    _relays.Remove(_relay);
                }
                RefreshRelayList();
            }
       
        }

        public void RefreshRelayList()
        {
            dbRelayList.Rows.Clear();
            _currentRelay = null;
            if (_relays != null)
            {
                foreach (UserRelay relay in _relays)
                {
                    dbRelayList.Rows.Add(relay.Name);
                }
            }

            if (dbRelayList.Rows.Count > 0)
            {
                dbRelayList.Rows[dbRelayList.Rows.Count - 1].Selected = true;
                _currentRelay = _relays[dbRelayList.Rows.Count - 1];
            }

            if (OnCurrentRelayChanged != null)
            {
                OnCurrentRelayChanged(this,null);
            }
        }
 
        private void dbCommandList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
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
                dbRelayList.Rows[_selectRowIndex].Selected = true;
 
            }
        }

        private void dbCommandList_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 1 && e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                contextMenuStrip.Show(Cursor.Position);
            }
        }

        private void dbCommandList_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_currentRelay != null)
            {
                RelayModuleSetting relayNameSetting = new RelayModuleSetting();
                relayNameSetting.RelayName = _currentRelay.Name;
                relayNameSetting.RelayCom = _currentRelay.RelayCom;
                relayNameSetting.RelayCount = _currentRelay.ApproachCout;
                if (relayNameSetting.ShowDialog() == DialogResult.OK)
                {
                    foreach (UserRelay relay in _relays)
                    {
                        if (relayNameSetting.RelayName == relay.Name && relay !=_currentRelay)
                        {
                            Helper.ShowMessageBox("提示", "该名称已存在！");
                            return;
                        }
                    }
                    _currentRelay.Name = relayNameSetting.RelayName;
                    _currentRelay.RelayCom = relayNameSetting.RelayCom;
                    _currentRelay.ApproachCout = relayNameSetting.RelayCount;
                    RefreshRelayList();
                }
 
            }
        }

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RelayModuleSetting relayNameSetting = new RelayModuleSetting();
            if (relayNameSetting.ShowDialog() == DialogResult.OK)
            {
                foreach (UserRelay relay in _relays)
                {
                    if (relayNameSetting.Name == relay.Name)
                    {
                        Helper.ShowMessageBox("提示", "已存在相同的名称！");
                        return;
                    }
                }

                UserRelay _userelay = new UserRelay(relayNameSetting.RelayName, relayNameSetting.RelayCom, relayNameSetting.RelayCount);
                AddCommand(_userelay);
            }

        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Helper.ShowMessageBox("确认", "确定删除？") == DialogResult.OK)
            {
                if (_relays != null && _relays.Count > 0 && _selectRowIndex != -1)
                {
                    UserRelay _relay = _relays[_selectRowIndex];
                    DeleteCommand(_relay);
                }
            }
        }

        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentRelay != null)
            {
                RelayModuleSetting relayNameSetting = new RelayModuleSetting();
                relayNameSetting.RelayName = _currentRelay.Name;
                relayNameSetting.RelayCom = _currentRelay.RelayCom;
                relayNameSetting.RelayCount = _currentRelay.ApproachCout;
                if (relayNameSetting.ShowDialog() == DialogResult.OK)
                {
                    foreach (UserRelay relay in _relays)
                    {
                        if (relay.Name == relayNameSetting.RelayName)
                        {
                            Helper.ShowMessageBox("提示","该名称已存在！");
                            return;
                        }
                    }

                    _currentRelay.Name = relayNameSetting.RelayName;
                    _currentRelay.RelayCom = relayNameSetting.RelayCom;
                    _currentRelay.ApproachCout = relayNameSetting.RelayCount;
                    RefreshRelayList();
                }
               
            }
        }

     
    }
}
