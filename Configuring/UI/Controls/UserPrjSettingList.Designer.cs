namespace Configuring.UI.Controls
{
    partial class UserPrjSettingList
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dbPrjSettingList = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加串口信息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dbPrjSettingList)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // dbPrjSettingList
            // 
            this.dbPrjSettingList.AllowUserToAddRows = false;
            this.dbPrjSettingList.AllowUserToDeleteRows = false;
            this.dbPrjSettingList.AllowUserToResizeColumns = false;
            this.dbPrjSettingList.AllowUserToResizeRows = false;
            this.dbPrjSettingList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dbPrjSettingList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dbPrjSettingList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dbPrjSettingList.DefaultCellStyle = dataGridViewCellStyle1;
            this.dbPrjSettingList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dbPrjSettingList.Location = new System.Drawing.Point(0, 0);
            this.dbPrjSettingList.Margin = new System.Windows.Forms.Padding(0);
            this.dbPrjSettingList.MultiSelect = false;
            this.dbPrjSettingList.Name = "dbPrjSettingList";
            this.dbPrjSettingList.RowHeadersVisible = false;
            this.dbPrjSettingList.RowTemplate.Height = 23;
            this.dbPrjSettingList.Size = new System.Drawing.Size(470, 400);
            this.dbPrjSettingList.TabIndex = 0;
            this.dbPrjSettingList.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dbComDataList_CellMouseClick);
            this.dbPrjSettingList.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dbComDataList_CellMouseDoubleClick);
            this.dbPrjSettingList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dbComDataList_MouseClick);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加串口信息ToolStripMenuItem,
            this.删除ToolStripMenuItem,
            this.设置ToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(137, 70);
            // 
            // 添加串口信息ToolStripMenuItem
            // 
            this.添加串口信息ToolStripMenuItem.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.添加串口信息ToolStripMenuItem.Name = "添加串口信息ToolStripMenuItem";
            this.添加串口信息ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.添加串口信息ToolStripMenuItem.Text = "添加投影机";
            this.添加串口信息ToolStripMenuItem.Click += new System.EventHandler(this.添加ToolStripMenuItem_Click);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // 设置ToolStripMenuItem
            // 
            this.设置ToolStripMenuItem.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
            this.设置ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.设置ToolStripMenuItem.Text = "设置";
            this.设置ToolStripMenuItem.Click += new System.EventHandler(this.设置ToolStripMenuItem_Click);
            // 
            // UserPrjSettingList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dbPrjSettingList);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "UserPrjSettingList";
            this.Size = new System.Drawing.Size(470, 400);
            ((System.ComponentModel.ISupportInitialize)(this.dbPrjSettingList)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dbPrjSettingList;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 添加串口信息ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设置ToolStripMenuItem;
    }
}
