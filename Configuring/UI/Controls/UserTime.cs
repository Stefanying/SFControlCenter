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
    public partial class UserTime : UserControl
    {
        public event EventHandler OnCurrentOrderChanged;

        List<UserOrder> _orders = new List<UserOrder>();

        public List<UserOrder> Orders
        {
            get { return _orders; }
            set { _orders = value; }
        }

        UserOrder _currentOrder = null;

        public UserOrder CurrentOrder
        {
            get { return _currentOrder; }
            set { _currentOrder = value; }
        }

        private int _selectRowIndex = -1;

        public UserTime()
        {
            InitializeComponent();

            if (dgTime.Columns.Count == 0)
            {
                dgTime.Columns.Add("name", "预约时间");
                dgTime.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgTime.Columns[0].ReadOnly = true;
            }

            RefreshTime();
        }

        public void RefreshTime()
        {
            dgTime.Rows.Clear();

            if (_orders != null)
            {
                foreach (UserOrder order in _orders)
                {
                    dgTime.Rows.Add(order.GetTime());
                }
            }

            if (dgTime.Rows.Count > 0 && _orders != null && _orders.Count > 0)
            {
                dgTime.Rows[dgTime.Rows.Count - 1].Selected = true;
                _currentOrder = _orders[dgTime.Rows.Count - 1];
            }

            if (OnCurrentOrderChanged != null)
            {
                OnCurrentOrderChanged(this, null);
            }
        }

        private void Add(UserOrder order)
        {
            if (_orders != null)
            {
                _orders.Add(order);
            }

            RefreshTime();
        }

        private void Delete(UserOrder order)
        {
            if (_orders != null)
            {
                _orders.Remove(order);
            }

            RefreshTime();
        }

        private void dgTime_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            _selectRowIndex = e.RowIndex;
            if (e.Clicks == 1 && e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                this.contextMenuStrip.Show(Cursor.Position);
            }

            if (_selectRowIndex >= 0 && _orders.Count > 0)
            {
                _currentOrder = _orders[_selectRowIndex];
               
                dgTime.Rows[_selectRowIndex].Selected = true;

                if (OnCurrentOrderChanged != null)
                {
                    OnCurrentOrderChanged(this, null);
                }

            }
        }

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OrderSetting os = new OrderSetting();
            if (os.ShowDialog() == DialogResult.OK)
            {
                UserOrder order = new UserOrder(os.Hour, os.Minute);

                foreach (UserOrder temp in _orders)
                {
                    if (temp.GetTime() == order.GetTime())
                    {
                        Helper.ShowMessageBox("提示", "已添加该时间命令");
                        return;
                    }
                }
                Add(order);
            }
        }

        private void dgTime_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 1 && e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                contextMenuStrip.Show(Cursor.Position);
            }
        }

        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentOrder != null)
            {
                OrderSetting os = new OrderSetting();
                os.Hour = _currentOrder.Hour;
                os.Minute = _currentOrder.Minute;

                if (os.ShowDialog() == DialogResult.OK)
                {
                    _currentOrder.Hour = os.Hour;
                    _currentOrder.Minute = os.Minute;
                }

                RefreshTime();
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentOrder != null)
            {
                _orders.Remove(_currentOrder);
                RefreshTime();
            }
        }
    }
}
