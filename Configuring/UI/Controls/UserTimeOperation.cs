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
    public partial class UserTimeOperation : UserControl
    {
        private List<UserOperation> _opreations;

        public List<UserOperation> Opreations
        {
            get { return _opreations; }
            set { _opreations = value; }
        }

        private UserOperation _currentOperation;
        public UserOperation CurrentOperation
        {
            get { return _currentOperation; }
            set { _currentOperation = value; }
        }

        private object _lock = new object();
        private int _selectRowIndex = 0;

        public UserTimeOperation()
        {
            InitializeComponent();

            if (dgOperation.Columns.Count == 0)
            {
                dgOperation.Columns.Add("name", "名称");
                dgOperation.Columns.Add("type", "通信方式");
                dgOperation.Columns.Add("datatype", "编码格式");
                dgOperation.Columns.Add("data", "控制代码");
                dgOperation.Columns.Add("time", "执行时间");

                dgOperation.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgOperation.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgOperation.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgOperation.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgOperation.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;

                dgOperation.Columns[0].ReadOnly = true;
                dgOperation.Columns[1].ReadOnly = true;
                dgOperation.Columns[2].ReadOnly = true;
                dgOperation.Columns[3].ReadOnly = true;
                dgOperation.Columns[4].ReadOnly = true;
            }

            dgOperation.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        public void AddOpration(UserOperation opration)
        {
            lock (_lock)
            {
                if (_opreations != null)
                {
                    _opreations.Add(opration);
                }
                else
                {
                    Helper.ShowMessageBox("添加失败", "请选择对应的命令或时间");
                }

                RefreshOprations();
            }
        }

        public void DeleteOpration(UserOperation opration)
        {
            lock (_lock)
            {
                if (_opreations != null)
                {
                    _opreations.Remove(opration);
                }

                RefreshOprations();
            }
        }

        public void RefreshOprations()
        {
            dgOperation.Rows.Clear();

            if (_opreations != null && _opreations.Count > 0)
            {
                foreach (UserOperation opration in _opreations)
                {
                    dgOperation.Rows.Add(opration.Name, GetOperationTypeString(opration.OpreationType), GetDataTypeString(opration.DataType), opration.Data, (opration.DelayTime / 60 / 1000).ToString() + "分" + ((opration.DelayTime / 1000) % 60).ToString() + "秒");
                }
                dgOperation.Rows[_opreations.Count - 1].Selected = true;
                _currentOperation = _opreations[_opreations.Count - 1];
            }
        }

        private void dgOprationList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            _selectRowIndex = e.RowIndex;
            if (_selectRowIndex < 0) return;

            if (e.Clicks == 1 && e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                contextMenuStrip1.Show(Cursor.Position);
            }

            if (_opreations != null && _opreations.Count > 0)
            {
                _currentOperation = _opreations[_selectRowIndex];

                dgOperation.Rows[_selectRowIndex].Selected = true;
            }
        }

        private void dgOprationList_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 1 && e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                contextMenuStrip1.Show(Cursor.Position);
            }
        }

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_opreations == null)
            {
                Helper.ShowMessageBox("提示", "请选择对应操作项或时间点！");
                return;
            }

            TimeOperationSetting os = new TimeOperationSetting();
            if (os.ShowDialog() == DialogResult.OK)
            {
                CommunicationType opType = CommunicationType.Com;
                if (os.CommunicationType.ToLower() == "tcp") opType = CommunicationType.TCP;
                else if (os.CommunicationType.ToLower() == "udp") opType = CommunicationType.UDP;
                else if (os.CommunicationType.ToLower() == "串口") opType = CommunicationType.Com;

                DataType dType = DataType.Character;
                if (os.DataType.ToLower() == "十六进制") dType = DataType.Hex;
                else if (os.DataType.ToLower() == "字符串") dType = DataType.Character;

                UserOperation opration = new UserOperation(os.OprationName, opType, dType, os.Setting, os.Data, os.DelayTime);
                AddOpration(opration);
            }
        }

        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentOperation != null)
            {
                TimeOperationSetting os = new TimeOperationSetting();
                os.OprationName = _currentOperation.Name;
                os.CommunicationType = GetOperationTypeString(_currentOperation.OpreationType);
                os.DataType = GetDataTypeString(_currentOperation.DataType);
                os.Data = _currentOperation.Data;
                os.DelayTime = _currentOperation.DelayTime / 1000;
                os.Setting = _currentOperation.Setting;

                if (os.ShowDialog() == DialogResult.OK)
                {
                    CommunicationType opType = CommunicationType.Com;
                    if (os.CommunicationType.ToLower() == "tcp") opType = CommunicationType.TCP;
                    else if (os.CommunicationType.ToLower() == "udp") opType = CommunicationType.UDP;
                    else if (os.CommunicationType.ToLower() == "串口") opType = CommunicationType.Com;

                    DataType dType = DataType.Character;
                    if (os.DataType.ToLower() == "十六进制") dType = DataType.Hex;
                    else if (os.DataType.ToLower() == "字符串") dType = DataType.Character;

                    _currentOperation.Name = os.OprationName;
                    _currentOperation.OpreationType = opType;
                    _currentOperation.DataType = dType;
                    _currentOperation.Setting = os.Setting;
                    _currentOperation.Data = os.Data;
                    _currentOperation.DelayTime = os.DelayTime;

                    RefreshOprations();
                }
            }

        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_opreations != null && _opreations.Count > 0 && _currentOperation != null)
            {
                DeleteOpration(_currentOperation);
                RefreshOprations();
            }
        }

        string GetOperationTypeString(CommunicationType type)
        {
            string ret = "串口";
            switch (type)
            {
                case CommunicationType.TCP:
                    ret = "TCP";
                    break;
                case CommunicationType.UDP:
                    ret = "UDP";
                    break;
            }
            return ret;
        }

        string GetDataTypeString(DataType type)
        {
            string ret = "十六进制";
            switch (type)
            {
                case DataType.Character:
                    ret = "字符串";
                    break;
                case DataType.Hex:
                    ret = "十六进制";
                    break;
            }
            return ret;
        }

        private void 上移ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentOperation != null && _currentOperation != _opreations[0])
            {
                int index = _opreations.IndexOf(_currentOperation);

                UserOperation replaceOperation = _opreations[index - 1];
                UserOperation temp = new UserOperation(replaceOperation.Name, replaceOperation.OpreationType, replaceOperation.DataType,
                                                       replaceOperation.Setting, replaceOperation.Data, replaceOperation.DelayTime);

                _opreations[index - 1] = _currentOperation;
                _opreations[index] = temp;

                RefreshOprations();
            }
        }

        private void 下移ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentOperation != null && _currentOperation != _opreations[_opreations.Count - 1])
            {
                int index = _opreations.IndexOf(_currentOperation);

                UserOperation replaceOperation = _opreations[index + 1];
                UserOperation temp = new UserOperation(replaceOperation.Name, replaceOperation.OpreationType, replaceOperation.DataType,
                                                       replaceOperation.Setting, replaceOperation.Data, replaceOperation.DelayTime);

                _opreations[index + 1] = _currentOperation;
                _opreations[index] = temp;

                RefreshOprations();
            }
        }

        private void dgOperation_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            _selectRowIndex = e.RowIndex;
            if (_selectRowIndex < 0) return;

            if (e.Clicks == 1 && e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                contextMenuStrip1.Show(Cursor.Position);
            }

            if (_opreations != null && _opreations.Count > 0)
            {
                _currentOperation = _opreations[_selectRowIndex];
                dgOperation.Rows[_selectRowIndex].Selected = true;
            }
        }

        private void dgOperation_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 1 && e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                contextMenuStrip1.Show(Cursor.Position);
            }
        }

    }
}
