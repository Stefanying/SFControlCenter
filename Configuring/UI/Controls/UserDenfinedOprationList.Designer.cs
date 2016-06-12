namespace Configuring.UI.Controls
{
    partial class UserDefinedOprationList
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
            this.dgOprationList = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加常用动作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.开关电脑ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.投影机开ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.继电器开关ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.上移ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.下移ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.复制ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dgOprationList)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgOprationList
            // 
            this.dgOprationList.AllowUserToAddRows = false;
            this.dgOprationList.AllowUserToDeleteRows = false;
            this.dgOprationList.AllowUserToResizeColumns = false;
            this.dgOprationList.AllowUserToResizeRows = false;
            this.dgOprationList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgOprationList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgOprationList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgOprationList.Location = new System.Drawing.Point(0, 0);
            this.dgOprationList.MultiSelect = false;
            this.dgOprationList.Name = "dgOprationList";
            this.dgOprationList.RowHeadersVisible = false;
            this.dgOprationList.RowTemplate.Height = 23;
            this.dgOprationList.Size = new System.Drawing.Size(566, 440);
            this.dgOprationList.TabIndex = 0;
            this.dgOprationList.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgOprationList_CellMouseClick);
            this.dgOprationList.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgOprationList_CellMouseDoubleClick);
            this.dgOprationList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgOprationList_MouseClick);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加常用动作ToolStripMenuItem,
            this.添加ToolStripMenuItem,
            this.设置ToolStripMenuItem,
            this.删除ToolStripMenuItem,
            this.上移ToolStripMenuItem,
            this.下移ToolStripMenuItem,
            this.复制ToolStripMenuItem});
            this.contextMenuStrip.Name = "menuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(153, 180);
            // 
            // 添加常用动作ToolStripMenuItem
            // 
            this.添加常用动作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.开关电脑ToolStripMenuItem,
            this.投影机开ToolStripMenuItem,
            this.继电器开关ToolStripMenuItem});
            this.添加常用动作ToolStripMenuItem.Name = "添加常用动作ToolStripMenuItem";
            this.添加常用动作ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.添加常用动作ToolStripMenuItem.Text = "添加常用动作";
            // 
            // 开关电脑ToolStripMenuItem
            // 
            this.开关电脑ToolStripMenuItem.Name = "开关电脑ToolStripMenuItem";
            this.开关电脑ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.开关电脑ToolStripMenuItem.Text = "开关电脑";
            this.开关电脑ToolStripMenuItem.Click += new System.EventHandler(this.开关电脑ToolStripMenuItem_Click);
            // 
            // 投影机开ToolStripMenuItem
            // 
            this.投影机开ToolStripMenuItem.Name = "投影机开ToolStripMenuItem";
            this.投影机开ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.投影机开ToolStripMenuItem.Text = "投影机开关";
            this.投影机开ToolStripMenuItem.Click += new System.EventHandler(this.投影机开关ToolStripMenuItem_Click);
            // 
            // 继电器开关ToolStripMenuItem
            // 
            this.继电器开关ToolStripMenuItem.Name = "继电器开关ToolStripMenuItem";
            this.继电器开关ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.继电器开关ToolStripMenuItem.Text = "继电器开关";
            this.继电器开关ToolStripMenuItem.Click += new System.EventHandler(this.继电器开关ToolStripMenuItem_Click);
            // 
            // 添加ToolStripMenuItem
            // 
            this.添加ToolStripMenuItem.Name = "添加ToolStripMenuItem";
            this.添加ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.添加ToolStripMenuItem.Text = "添加动作";
            this.添加ToolStripMenuItem.Click += new System.EventHandler(this.添加ToolStripMenuItem_Click);
            // 
            // 设置ToolStripMenuItem
            // 
            this.设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
            this.设置ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.设置ToolStripMenuItem.Text = "设置";
            this.设置ToolStripMenuItem.Click += new System.EventHandler(this.设置ToolStripMenuItem_Click);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // 上移ToolStripMenuItem
            // 
            this.上移ToolStripMenuItem.Name = "上移ToolStripMenuItem";
            this.上移ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.上移ToolStripMenuItem.Text = "上移";
            this.上移ToolStripMenuItem.Click += new System.EventHandler(this.上移ToolStripMenuItem_Click);
            // 
            // 下移ToolStripMenuItem
            // 
            this.下移ToolStripMenuItem.Name = "下移ToolStripMenuItem";
            this.下移ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.下移ToolStripMenuItem.Text = "下移";
            this.下移ToolStripMenuItem.Click += new System.EventHandler(this.下移ToolStripMenuItem_Click);
            // 
            // 复制ToolStripMenuItem
            // 
            this.复制ToolStripMenuItem.Name = "复制ToolStripMenuItem";
            this.复制ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.复制ToolStripMenuItem.Text = "复制";
            this.复制ToolStripMenuItem.Click += new System.EventHandler(this.复制ToolStripMenuItem_Click);
            // 
            // UserDefinedOprationList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgOprationList);
            this.Name = "UserDefinedOprationList";
            this.Size = new System.Drawing.Size(566, 440);
            ((System.ComponentModel.ISupportInitialize)(this.dgOprationList)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgOprationList;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 添加ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 上移ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 下移ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 复制ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加常用动作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 开关电脑ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 投影机开ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 继电器开关ToolStripMenuItem;
    }
}
