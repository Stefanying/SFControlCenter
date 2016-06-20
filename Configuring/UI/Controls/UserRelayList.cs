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

        private List<UserRelayArray> _relayModules;
        public List<UserRelayArray> RelayModules
        {
            get { return _relayModules; }
            set { _relayModules = value; }
        }

        private UserRelayArray _currentRelayModule;
        public UserRelayArray CurrentRelayModule
        {
            get { return _currentRelayModule; }
            set { _currentRelayModule = value; }
        }

        private object _lock = new object();
        private int _selectRowIndex = -1;

        public UserRelayList()
        {
            InitializeComponent();

            if (dbRelayList.Columns.Count == 0)
            {
                dbRelayList.Columns.Add("name", "继电器模块名称");
                dbRelayList.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                dbRelayList.Columns[0].ReadOnly =true;
            }

            dbRelayList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            RefreshRelayList();
        }

        public void AddCommand(UserRelayArray _relay)
        {
            lock(_lock)
            {
                if (_relayModules != null)
                {
                    _relayModules.Add(_relay);
                }
                else
                {
                    Helper.ShowMessageBox("添加失败!","添加继电器失败");
                }
                RefreshRelayList();
            }
        
        }

        public void DeleteCommand(UserRelayArray _relay)
        {
            lock (_lock)
            {
                if (_relayModules != null)
                {
                    _relayModules.Remove(_relay);
                }
                RefreshRelayList();
            }
       
        }

        public void RefreshRelayList()
        {
            dbRelayList.Rows.Clear();
            _currentRelayModule = null;
            if (_relayModules != null)
            {
                foreach (UserRelayArray relay in _relayModules)
                {
                    dbRelayList.Rows.Add(relay.Name);
                }
            }

            if (dbRelayList.Rows.Count > 0)
            {
                dbRelayList.Rows[dbRelayList.Rows.Count - 1].Selected = true;
                _currentRelayModule = _relayModules[dbRelayList.Rows.Count - 1];
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

            if (_selectRowIndex >= 0 && _currentRelayModule != _relayModules[_selectRowIndex])
            {
                _currentRelayModule = _relayModules[_selectRowIndex];
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
            if (_currentRelayModule != null)
            {
                RelayModuleSetting relayNameSetting = new RelayModuleSetting();
                relayNameSetting.RelayName = _currentRelayModule.Name;
                relayNameSetting.RelayCom = _currentRelayModule.RelayCom;
                relayNameSetting.RelayCount = _currentRelayModule.ApproachCout;
                if (relayNameSetting.ShowDialog() == DialogResult.OK)
                {
                    foreach (UserRelayArray relayModules in _relayModules)
                    {
                        if (relayModules.Name == relayNameSetting.RelayName && relayModules.Name != _currentRelayModule.Name)
                        {
                            Helper.ShowMessageBox("提示", "该名称已存在！");
                            return;
                        }
                    }
                    if (_currentRelayModule.RelayOperationDatas.Count != relayNameSetting.RelayCount)
                    {
                        if (Helper.ShowMessageBox("操作确认", "确定更改？", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.OK)
                        {
                            _currentRelayModule.RelayOperationDatas.Clear();
                            for (int i = 1; i <= relayNameSetting.RelayCount; i++)
                            {
                                UserRelaySetting _userRelaySetting = new UserRelaySetting(i, relayNameSetting.RelayCount);
                                RelayOperationDataList _relayOperation = new RelayOperationDataList();
                                _relayOperation.SetOperationData(RelayOperationType.吸合, "");
                                _relayOperation.SetOperationData(RelayOperationType.断开, "");
                                _userRelaySetting.AddRelayOperationData(_relayOperation);
                                _currentRelayModule.AddRelayData(_userRelaySetting);
                            }
                        }
                    }
                    _currentRelayModule.Name = relayNameSetting.RelayName;
                    _currentRelayModule.RelayCom = relayNameSetting.RelayCom;
                    _currentRelayModule.ApproachCout = relayNameSetting.RelayCount;
                    RefreshRelayList();
                }

            }
        }

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RelayModuleSetting relayNameSetting = new RelayModuleSetting();
            if (relayNameSetting.ShowDialog() == DialogResult.OK)
            {
                if (_relayModules != null)
                {
                    foreach (UserRelayArray relay in _relayModules)
                    {
                        if (relayNameSetting.Name == relay.Name)
                        {
                            Helper.ShowMessageBox("提示", "已存在相同的名称！");
                            return;
                        }
                    }

                    UserRelayArray _userelay = new UserRelayArray(relayNameSetting.RelayName, relayNameSetting.RelayCom, relayNameSetting.RelayCount);
                    for (int i = 1; i <= relayNameSetting.RelayCount; i++)
                    {
                        UserRelaySetting _userRelaySetting = new UserRelaySetting(i, relayNameSetting.RelayCount);
                        RelayOperationDataList _relayOperation = new RelayOperationDataList();
                        _relayOperation.SetOperationData(RelayOperationType.吸合, "");
                        _relayOperation.SetOperationData(RelayOperationType.断开, "");
                        _userRelaySetting.AddRelayOperationData(_relayOperation);
                        _userelay.AddRelayData(_userRelaySetting);
                    }
                    AddCommand(_userelay);
                }
                else
                {
                    UserRelayArray _userelay = new UserRelayArray(relayNameSetting.RelayName, relayNameSetting.RelayCom, relayNameSetting.RelayCount);
                    for (int i = 1; i <= relayNameSetting.RelayCount; i++)
                    {
                        UserRelaySetting _userRelaySetting = new UserRelaySetting(i, relayNameSetting.RelayCount);
                        RelayOperationDataList _relayOperation = new RelayOperationDataList();
                        _relayOperation.SetOperationData(RelayOperationType.吸合, "");
                        _relayOperation.SetOperationData(RelayOperationType.断开, "");
                        _userRelaySetting.AddRelayOperationData(_relayOperation);
                        _userelay.AddRelayData(_userRelaySetting);
                    }
                    _relayModules = new List<UserRelayArray>();
                    _relayModules.Add(_userelay);
                    RefreshRelayList();
                }
            }

        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Helper.ShowMessageBox("确认", "确定删除？") == DialogResult.OK)
            {
                if (_relayModules != null && _relayModules.Count > 0 && _selectRowIndex != -1)
                {
                    UserRelayArray _relayModule = _relayModules[_selectRowIndex];
                    DeleteCommand(_relayModule);
                }
            }
        }

        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentRelayModule != null)
            {
                RelayModuleSetting relayNameSetting = new RelayModuleSetting();
                relayNameSetting.RelayName = _currentRelayModule.Name;
                relayNameSetting.RelayCom = _currentRelayModule.RelayCom;
                relayNameSetting.RelayCount = _currentRelayModule.ApproachCout;
                if (relayNameSetting.ShowDialog() == DialogResult.OK)
                {
                    foreach (UserRelayArray relayModules in _relayModules)
                    {
                        if (relayModules.Name == relayNameSetting.RelayName &&  relayModules.Name !=_currentRelayModule.Name)
                        {
                            Helper.ShowMessageBox("提示","该名称已存在！");
                            return;
                        }
                    }

                    if (_currentRelayModule.RelayOperationDatas.Count != relayNameSetting.RelayCount)
                    {
                        if (Helper.ShowMessageBox("操作确认", "确定更改？", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.OK)
                        {
                            _currentRelayModule.RelayOperationDatas.Clear();
                            for (int i = 1; i <= relayNameSetting.RelayCount; i++)
                            {
                                UserRelaySetting _userRelaySetting = new UserRelaySetting(i, relayNameSetting.RelayCount);
                                RelayOperationDataList _relayOperation = new RelayOperationDataList();
                                _relayOperation.SetOperationData(RelayOperationType.吸合, "");
                                _relayOperation.SetOperationData(RelayOperationType.断开, "");
                                _userRelaySetting.AddRelayOperationData(_relayOperation);
                                _currentRelayModule.AddRelayData(_userRelaySetting);
                            }
                        }
                    }
                    _currentRelayModule.Name = relayNameSetting.RelayName;
                    _currentRelayModule.RelayCom = relayNameSetting.RelayCom;
                    _currentRelayModule.ApproachCout = relayNameSetting.RelayCount;
                    RefreshRelayList();
                }
               
            }
        }

     
    }
}
