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
    public partial class UserDefinedOprationList : UserControl
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

        private List<UserPrjSetting> _prjSettings;
        public List<UserPrjSetting> PrjSettings
        {
            get { return _prjSettings; }
            set { _prjSettings = value; }
        }

        private List<UserRelayArray> _relaySettings;
        public List<UserRelayArray> RelaySettings
        {
            get { return _relaySettings; }
            set { _relaySettings = value; }
        }

        private ComSetting _relayComSetting;
        public ComSetting RelayComSetting
        {
            get { return _relayComSetting; }
            set { _relayComSetting = value; }
        }

        private object _lock = new object();
        private int _selectRowIndex = 0;

        public UserDefinedOprationList()
        {
            InitializeComponent();
            if (dgOprationList.Columns.Count == 0)
            {
                dgOprationList.Columns.Add("name", "动作名称");
                dgOprationList.Columns.Add("type", "通信方式");
                dgOprationList.Columns.Add("datatype","编码格式");
                dgOprationList.Columns.Add("data", "控制代码");
                dgOprationList.Columns.Add("time", "延迟执行");

                dgOprationList.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgOprationList.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgOprationList.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgOprationList.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgOprationList.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;

                dgOprationList.Columns[0].ReadOnly = true;
                dgOprationList.Columns[1].ReadOnly = true;
                dgOprationList.Columns[2].ReadOnly = true;
                dgOprationList.Columns[3].ReadOnly = true;
                dgOprationList.Columns[4].ReadOnly = true;
            }

            dgOprationList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            RefreshOprations();
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
            dgOprationList.Rows.Clear();
            _currentOperation = null;
            if(_opreations != null)
            {
                foreach(UserOperation opration in _opreations)
                {
                    dgOprationList.Rows.Add(opration.Name,GetOperationTypeString(opration.OpreationType), GetDataTypeString(opration.DataType), opration.Data, opration.DelayTime);
                }
            }

            if (dgOprationList.Rows.Count > 0)
            {
                dgOprationList.Rows[_opreations.Count - 1].Selected = true;
                _currentOperation = _opreations[_opreations.Count - 1];
            }
        }

        private void dgOprationList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            _selectRowIndex = e.RowIndex;
            if (_selectRowIndex < 0) return;

            if (e.Clicks == 1 && e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                contextMenuStrip.Show(Cursor.Position);
            }

            if ( _opreations != null && _opreations.Count > 0)
            {
                _currentOperation = _opreations[_selectRowIndex];

                dgOprationList.Rows[_selectRowIndex].Selected = true;
            }
        }

        private void dgOprationList_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 1 && e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                contextMenuStrip.Show(Cursor.Position);
            }
        }

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OprationSetting os = new OprationSetting();
            if( os.ShowDialog() == DialogResult.OK)
            {
                CommunicationType opType = CommunicationType.Com;
                if (os.CommunicationType.ToLower() == "tcp") opType = CommunicationType.TCP;
                else if (os.CommunicationType.ToLower() == "udp") opType = CommunicationType.UDP;
                else if (os.CommunicationType.ToLower() == "串口") opType = CommunicationType.Com;

                DataType dType = DataType.Character;
                if (os.DataType.ToLower() == "十六进制") dType = DataType.Hex;
                else if (os.DataType.ToLower() == "字符串") dType = DataType.Character;

                UserOperation opration = new UserOperation(os.OprationName,  opType, dType, os.Setting, os.Data, os.DelayTime);
                AddOpration(opration);
            }
        }

        private void 投影机开关ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_opreations == null)
            {
                Helper.ShowMessageBox("提示", "请选择对应操作项或时间点！");
                return;
            }

            if (_prjSettings == null)
            {
                Helper.ShowMessageBox("提示", "投影机数据未添加!");
                return;
            }

            PrjSwitch _prj = new PrjSwitch(_prjSettings);
            if (_prj.ShowDialog() == DialogResult.OK)
            {
                CommunicationType opType = CommunicationType.Com;
                DataType dType = DataType.Hex;
                UserOperation opration = new UserOperation(_prj.OperationName, opType, dType, _prj.Setting, _prj.Data, _prj.DelayTime);
                AddOpration(opration);    
            }
        }

        private void 开关电脑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_opreations == null)
            {
                Helper.ShowMessageBox("提示", "请选择对应操作项或时间点！");
                return;
            }

            if (_relaySettings == null)
            {
                Helper.ShowMessageBox("提示", "继电器数据未添加!");
            }
         

            PcSwatch _pcSwitch = new PcSwatch(_relaySettings);
            if (_pcSwitch.ShowDialog() == DialogResult.OK)
            {
                CommunicationType opType = CommunicationType.Com;
                DataType dType = DataType.Hex;
                if (_pcSwitch.DataList.Count > 0)
                {
                    UserOperation OnOperation = new UserOperation(_pcSwitch.OperationNameList[0], opType, dType, _pcSwitch.Setting, _pcSwitch.DataList[0], _pcSwitch.DelayTime);
                    UserOperation OffOperation = new UserOperation(_pcSwitch.OperationNameList[1], opType, dType, _pcSwitch.Setting, _pcSwitch.DataList[1], _pcSwitch.DelayTime);
                    AddOpration(OnOperation);
                    AddOpration(OffOperation);
                }
            }

        }

        private void 继电器开关ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (_opreations == null)
            {
                Helper.ShowMessageBox("提示", "请选择对应操作项或时间点！");
                return;
            }

            if (_relaySettings == null)
            {
                Helper.ShowMessageBox("提示", "继电器数据未添加!");
            }

            RelaySwatch _rwRelaySwatch = new RelaySwatch(_relaySettings);
            if (_rwRelaySwatch.ShowDialog() == DialogResult.OK)
            {
                CommunicationType opType = CommunicationType.Com;
                DataType dType = DataType.Hex;
                UserOperation opration = new UserOperation(_rwRelaySwatch.OperationName, opType, dType, _rwRelaySwatch.Setting, _rwRelaySwatch.Data, _rwRelaySwatch.DelayTime);
                AddOpration(opration);
            }      
        }

        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(_currentOperation != null)
            {
                OprationSetting os = new OprationSetting();
                os.OprationName = _currentOperation.Name;
                os.CommunicationType = GetOperationTypeString(_currentOperation.OpreationType);
                os.DataType = GetDataTypeString(_currentOperation.DataType);
                os.Data = _currentOperation.Data;
                os.DelayTime = _currentOperation.DelayTime;
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
            if (Helper.ShowMessageBox("确认", "确定删除？") == DialogResult.OK)
            {
                if (_opreations != null && _opreations.Count > 0 && _currentOperation != null)
                {
                    DeleteOpration(_currentOperation);
                    RefreshOprations();
                }
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
                                                       replaceOperation.Setting, replaceOperation.Data,replaceOperation.DelayTime);

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

        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentOperation != null)
            {
                Business.UserOperation op = new UserOperation(_currentOperation.Name, _currentOperation.OpreationType, _currentOperation.DataType, _currentOperation.Setting, _currentOperation.Data, _currentOperation.DelayTime
                    );
                _opreations.Add(op);
                RefreshOprations();
            }
        }

        private void dgOprationList_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_currentOperation != null)
            {
                OprationSetting os = new OprationSetting();
                os.OprationName = _currentOperation.Name;
                os.CommunicationType = GetOperationTypeString(_currentOperation.OpreationType);
                os.DataType = GetDataTypeString(_currentOperation.DataType);
                os.Data = _currentOperation.Data;
                os.DelayTime = _currentOperation.DelayTime;
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

    }
}
