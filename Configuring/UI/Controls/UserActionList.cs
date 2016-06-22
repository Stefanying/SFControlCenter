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
    public partial class UserActionList : UserControl
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

        private List<UserArea> _areaList;
        public List<UserArea> AreaList
        {
            get { return _areaList; }
            set { _areaList = value; }
        }

        private object _lock = new object();
        private int _selectRowIndex = 0;
        
        public UserActionList()
        {
            InitializeComponent();

            if (dgActionList.Columns.Count == 0)
            {
                dgActionList.Columns.Add("name", "控制项");
                dgActionList.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;

                dgActionList.Columns.Add("customNo", "客户端发送命令");
                dgActionList.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            dgActionList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

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
                dgActionList.Rows.Clear();
                _currentAction = null;

                if (_actionList != null)
                {
                    foreach (UserAction action in _actionList)
                    {
                        dgActionList.Rows.Add(action.Name, action.ReceiveCommand);
                    }
                }

                if (_actionList != null && _actionList.Count > 0)
                {
                    dgActionList.Rows[_actionList.Count - 1].Selected = true;
                    _currentAction = _actionList[_actionList.Count - 1];
                }

                if (OnCurrentActionChange != null)
                {
                    OnCurrentActionChange(this, null);
                }
            }
        }

        private void ActionList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            _selectRowIndex = e.RowIndex;
            if (_selectRowIndex < 0) return;

            if (e.Clicks == 1 && e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                contextMenuStrip.Show(Cursor.Position);
            }

            if(_actionList != null && _actionList.Count > 0)
            {
                if(_currentAction != _actionList[_selectRowIndex])
                {
                    _currentAction = _actionList[_selectRowIndex];
                }

                if (OnCurrentActionChange != null)
                {
                    OnCurrentActionChange(this, null);
                }

                dgActionList.Rows[_selectRowIndex].Selected = true;
            }
        }

        private void dgActionList_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            _selectRowIndex = e.RowIndex;
            if (_selectRowIndex < 0) return;


            if (_actionList != null && _actionList.Count > 0 && e.RowIndex >= 0)
            {
                if (_currentAction != _actionList[_selectRowIndex])
                {
                    _currentAction = _actionList[_selectRowIndex];
                }

                if (OnCurrentActionChange != null)
                {
                    OnCurrentActionChange(this, null);
                }

                dgActionList.Rows[_selectRowIndex].Selected = true;
            }

            if ( e.RowIndex >= 0)
            {
                SettingAction();
            }
        }

        private void ActionList_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 1 && e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                contextMenuStrip.Show(Cursor.Position);
            }
        }

        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingAction();
        }

        private void SettingAction()
        {
            if (_currentAction != null)
            {
                ActionSetting actionSetting = new ActionSetting();
                actionSetting.ActionName = _currentAction.Name;
                actionSetting.ActionCode = _currentAction.ReceiveCommand;

                if (actionSetting.ShowDialog() == DialogResult.OK)
                {
                    if (!CheckCurrentArrayReceiveCommand(actionSetting.ActionCode) || _currentAction.ReceiveCommand == actionSetting.ActionCode || !CheckAllArrayReceiveCommand(actionSetting.ActionCode))
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

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_actionList == null)
            {
                Helper.ShowMessageBox("提示", "请选择对应展项！");
                return;
            }

            ActionSetting actionSetting = new ActionSetting();
            if (actionSetting.ShowDialog() == DialogResult.OK)
            {
                if (!CheckCurrentArrayReceiveCommand(actionSetting.ActionCode) && !CheckAllArrayReceiveCommand(actionSetting.ActionCode) )
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
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(_currentAction != null && _actionList != null && _actionList.Count > 0)
            {
                _actionList.Remove(_currentAction);

                RefreshActionList();
            }
        }


        //检测客户端发送命令是否重复
        bool CheckCurrentArrayReceiveCommand(string command)
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

        bool CheckAllArrayReceiveCommand(string command)
        {
            if (_areaList != null)
            {
                foreach (UserArea _userarea in _areaList)
                {
                    foreach (UserAction _useraction in _userarea.Actions)
                    {
                        if (_useraction.ReceiveCommand == command)
                        {
                            return true;
                        }
                      //  return false;
                    }
                  //  return false;
                }
               // return false;
            }
            return false;
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
