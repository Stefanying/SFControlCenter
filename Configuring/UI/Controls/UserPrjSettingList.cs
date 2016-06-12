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
    public partial class UserPrjSettingList : UserControl
    {
       

        public event EventHandler OnCurrentPrjSetChanged;

        private List<UserPrjSetting> _upSettings;
        public List<UserPrjSetting> UpSettings
        {
            get { return _upSettings; }
            set { _upSettings = value; }
        }

        private  UserPrjSetting _currentUpSetting;
        public UserPrjSetting CurrentUpSetting
        {
            get {return _currentUpSetting;}
            set {_currentUpSetting = value;}
        }

        private object _lock = new object();
        private int _selectRowIndex = 0;

        public UserPrjSettingList()
        {
            InitializeComponent();
            if (dbPrjSettingList.Columns.Count == 0)
            {
                dbPrjSettingList.Columns.Add("name","投影机名称");
                dbPrjSettingList.Columns.Add("baudrate","波特率");
                dbPrjSettingList.Columns.Add("databit", "数据位");
                dbPrjSettingList.Columns.Add("stopbit","停止位");
                dbPrjSettingList.Columns.Add("parity","奇偶校验");

                dbPrjSettingList.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                dbPrjSettingList.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                dbPrjSettingList.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                dbPrjSettingList.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                dbPrjSettingList.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;


                dbPrjSettingList.Columns[0].ReadOnly = true;
                dbPrjSettingList.Columns[1].ReadOnly = true;
                dbPrjSettingList.Columns[2].ReadOnly = true;
                dbPrjSettingList.Columns[3].ReadOnly = true;
                dbPrjSettingList.Columns[4].ReadOnly = true;
            }

            dbPrjSettingList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dbPrjSettingList.EnableHeadersVisualStyles = false;
            RefreshPrjSetting();

        }

        public void AddPrjSetCommand(UserPrjSetting upset)
        {
       
            lock(_lock)
            {
                if(_upSettings !=null)
                {
                    _upSettings.Add(upset);
                }
                else
                {
                    Helper.ShowMessageBox("添加失败","添加投影机设置失败！");
                }

                RefreshPrjSetting();
            }
        }

        public void DeletePrjSetCommand(UserPrjSetting  upset)
        {
            lock(_lock)
            {
                if(_upSettings !=null)
                {
                    _upSettings.Remove(upset);
                }

                RefreshPrjSetting();
            }
         
        }

        public void RefreshPrjSetting()
        {
           dbPrjSettingList.Rows.Clear();
           _currentUpSetting =null;
            if(_upSettings !=null)
            {
                foreach(UserPrjSetting upset in _upSettings)
                {
                    dbPrjSettingList.Rows.Add(upset.Name,upset.Pcs.BaudRate,upset.Pcs.DataBits,upset.Pcs.StopBits,GetParityModeType(upset.Pcs.Parity));
                }
            }

            if(dbPrjSettingList.Rows.Count >0)
            {
                dbPrjSettingList.Rows[dbPrjSettingList.Rows.Count-1].Selected = true;
                _currentUpSetting = _upSettings[dbPrjSettingList.Rows.Count -1];
            }
            if(OnCurrentPrjSetChanged !=null)
            {
                OnCurrentPrjSetChanged(this,null);
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

            if(_selectRowIndex >=0 && _currentUpSetting !=_upSettings[_selectRowIndex])
            {
                _currentUpSetting = _upSettings[_selectRowIndex];

                if(OnCurrentPrjSetChanged !=null)
                {
                    OnCurrentPrjSetChanged(this,null);
                }

                dbPrjSettingList.Rows[_selectRowIndex].Selected = true;
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
            if (_currentUpSetting != null)
            {
                DeviceSerialSetting dss = new DeviceSerialSetting();
                dss.BaudRate = _currentUpSetting.Pcs.BaudRate;
                dss.Databit = _currentUpSetting.Pcs.DataBits;
                dss.StopBit = _currentUpSetting.Pcs.StopBits;
                dss.Parity = _currentUpSetting.Pcs.Parity;
                dss.PrjName = _currentUpSetting.Name;
                if (dss.ShowDialog() == DialogResult.OK)
                {
                    _currentUpSetting.Pcs = dss.DeviceComSetting;
                    _currentUpSetting.Name = dss.PrjName;
                    RefreshPrjSetting();

                }
            }
        }

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
              DeviceSerialSetting dss = new DeviceSerialSetting();
            if (dss.ShowDialog() == DialogResult.OK)
            {
                foreach (UserPrjSetting upsetting in _upSettings)
                {
                    if (upsetting.Name == dss.PrjName)
                    {
                        Helper.ShowMessageBox("提示", "已存在相同的名称");
                        return;
                    }
                }

                UserPrjSetting upset = new UserPrjSetting(dss.PrjName,dss.DeviceComSetting);
                AddPrjSetCommand(upset);
            }

            
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Helper.ShowMessageBox("确认", "确定删除？") == DialogResult.OK)
            {
                if (_upSettings != null && _upSettings.Count > 0 && _currentUpSetting != null)
                {
                    DeletePrjSetCommand(_currentUpSetting);
                    RefreshPrjSetting();
                }
            }
        }

        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (_currentUpSetting != null)
            {
                DeviceSerialSetting dss = new DeviceSerialSetting();
                dss.BaudRate = _currentUpSetting.Pcs.BaudRate;
                dss.Databit = _currentUpSetting.Pcs.DataBits;
                dss.StopBit = _currentUpSetting.Pcs.StopBits;
                dss.Parity = _currentUpSetting.Pcs.Parity;
                dss.PrjName = _currentUpSetting.Name;
                if (dss.ShowDialog() == DialogResult.OK)
                {
                    _currentUpSetting.Pcs = dss.DeviceComSetting;
                    _currentUpSetting.Name = dss.PrjName;
                    RefreshPrjSetting();
 
                }
            }
        }
    }
}
