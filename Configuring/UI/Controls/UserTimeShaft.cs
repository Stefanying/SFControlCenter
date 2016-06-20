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
    public partial class UserTimeShaft : UserControl
    {
        public event EventHandler OnCurrentActionChange;
        private List<UserAction> _actionList;

        public List<UserAction> ActionList
        {
            get { return _actionList; }
            set { _actionList = value; }
        }

        private UserAction _currentAction;

        public UserAction CurrentAction
        {
            get { return _currentAction; }
            set { _currentAction = value; }
        }

        private object _lock = new object();
        private int _selectRowIndex = 0;

        public UserTimeShaft()
        {
            InitializeComponent();


            if (dbTimeShaft.Columns.Count == 0)
            {
                dbTimeShaft.Columns.Add("name", "控制项");
                dbTimeShaft.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;

                dbTimeShaft.Columns.Add("customNo", "客户端发送命令");
                dbTimeShaft.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            dbTimeShaft.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            RefreshActionList();
        }

        public void AddAction(UserAction action)
        {
            lock (_lock)
            {
                if (_actionList != null)
                {
                    _actionList.Add(action);
                }

                RefreshActionList();
            }
        }

        public void DeleteAction(UserAction action)
        {
            lock (_lock)
            {
                if (_actionList != null)
                {
                    _actionList.Remove(action);
                }

                RefreshActionList();
            }
        }

        public void RefreshActionList()
        {
            lock (_lock)
            {
                dbTimeShaft.Rows.Clear();
                _currentAction = null;

                if (_actionList != null)
                {
                    foreach (UserAction action in _actionList)
                    {
                        dbTimeShaft.Rows.Add(action.Name, action.ReceiveCommand);
                    }
                }

                if (_actionList != null && _actionList.Count > 0)
                {
                    dbTimeShaft.Rows[_actionList.Count - 1].Selected = true;
                    _currentAction = _actionList[_actionList.Count - 1];
                }

                if (OnCurrentActionChange != null)
                {
                    OnCurrentActionChange(this, null);
                }
            }
        }

        private void dbTimeShaft_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            _selectRowIndex = e.RowIndex;
            if (_selectRowIndex < 0) return;

            if (e.Clicks == 1 && e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                 this.contextMenuStrip1.Show(Cursor.Position);
            }

            if (_actionList != null && _actionList.Count > 0)
            {
                if (_currentAction != _actionList[_selectRowIndex])
                {
                    _currentAction = _actionList[_selectRowIndex];
                }

                if (OnCurrentActionChange != null)
                {
                    OnCurrentActionChange(this, null);
                }
                dbTimeShaft.Rows[_selectRowIndex].Selected = true;
            }
        }

        private void dbTimeShaft_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 1 && e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                contextMenuStrip1.Show(Cursor.Position);
            }
        }

        bool CheckReceiveCommand(string command)
        {
            for (int i = 0; i < _actionList.Count; i++)
            {
                if (_actionList[i].ReceiveCommand == command)
                {
                    return true;
                }
            }
            return false;
        }

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActionSetting actionSetting = new ActionSetting();
            if (actionSetting.ShowDialog() == DialogResult.OK)
            {
                if (_actionList != null)
                {
                    if (!CheckReceiveCommand(actionSetting.ActionCode))
                    {
                        UserAction action = new UserAction(actionSetting.ActionName, actionSetting.ActionCode);
                        _actionList.Add(action);
                    }
                    else
                    {
                        Helper.ShowMessageBox("提示", "存在相同接收符！");
                    }

                    RefreshActionList();
                }
                else
                {
                    UserAction action = new UserAction(actionSetting.ActionName, actionSetting.ActionCode);
                    _actionList.Add(action);
                    RefreshActionList();
                }
            }
        }

        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentAction != null)
            {
                ActionSetting actionSetting = new ActionSetting();
                actionSetting.ActionName = _currentAction.Name;
                actionSetting.ActionCode = _currentAction.ReceiveCommand;

                if (actionSetting.ShowDialog() == DialogResult.OK)
                {
                    if (!CheckReceiveCommand(actionSetting.ActionCode) || _currentAction.ReceiveCommand == actionSetting.ActionCode)
                    {
                        _currentAction.Name = actionSetting.ActionName;
                        _currentAction.ReceiveCommand = actionSetting.ActionCode;
                    }
                    else
                    {
                        Helper.ShowMessageBox("提示", "存在相同接收符！");
                    }

                    RefreshActionList();
                }
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentAction != null && _actionList != null && _actionList.Count > 0)
            {
                _actionList.Remove(_currentAction);
                RefreshActionList();
            }
        }

        private void 上移ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentAction != _actionList[0])
            {
                int index = _actionList.IndexOf(_currentAction);
                UserAction temp = new UserAction(_actionList[index - 1].Name, _actionList[index - 1].ReceiveCommand);
                temp.Operations = _actionList[index - 1].Operations;

                _actionList[index - 1] = _currentAction;
                _actionList[index] = temp;

                RefreshActionList();
            }
        }

        private void 下移ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentAction != _actionList[_actionList.Count - 1])
            {
                int index = _actionList.IndexOf(_currentAction);
                UserAction temp = new UserAction(_actionList[index + 1].Name, _actionList[index + 1].ReceiveCommand);
                temp.Operations = _actionList[index + 1].Operations;

                _actionList[index + 1] = _currentAction;
                _actionList[index] = temp;

                RefreshActionList();
            }
        }
    }
}
