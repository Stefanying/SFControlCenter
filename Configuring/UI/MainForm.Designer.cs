namespace Configuring.UI
{
    partial class MainForm
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tabContent = new System.Windows.Forms.TabControl();
            this.tbConfig = new System.Windows.Forms.TabPage();
            this.plOprationlist = new System.Windows.Forms.Panel();
            this.pActionList = new System.Windows.Forms.Panel();
            this.plAreaList = new System.Windows.Forms.Panel();
            this.tabTimeShaft = new System.Windows.Forms.TabPage();
            this.panelTimeOperation = new System.Windows.Forms.Panel();
            this.panelAction = new System.Windows.Forms.Panel();
            this.tbAdvance = new System.Windows.Forms.TabPage();
            this.btnCheckTime = new System.Windows.Forms.Button();
            this.btnSetTime = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.panelContent = new System.Windows.Forms.Panel();
            this.panelOrder = new System.Windows.Forms.Panel();
            this.panelTime = new System.Windows.Forms.Panel();
            this.tbSetting = new System.Windows.Forms.TabPage();
            this.prjStatePanel = new System.Windows.Forms.Panel();
            this.prjSetPanel = new System.Windows.Forms.Panel();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.relayNamePanel = new System.Windows.Forms.Panel();
            this.RelayStatePanel = new System.Windows.Forms.Panel();
            this.RelaySettingPanel = new System.Windows.Forms.Panel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.UserDefinedNamepanel = new System.Windows.Forms.Panel();
            this.customPanel = new System.Windows.Forms.Panel();
            this.Setting = new System.Windows.Forms.Panel();
            this.btnUpload = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDowbload = new System.Windows.Forms.Button();
            this.tbIP = new System.Windows.Forms.TextBox();
            this.lbIP = new System.Windows.Forms.Label();
            this.tabContent.SuspendLayout();
            this.tbConfig.SuspendLayout();
            this.tabTimeShaft.SuspendLayout();
            this.tbAdvance.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.tbSetting.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.Setting.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabContent
            // 
            this.tabContent.Controls.Add(this.tbConfig);
            this.tabContent.Controls.Add(this.tabTimeShaft);
            this.tabContent.Controls.Add(this.tbAdvance);
            this.tabContent.Controls.Add(this.tbSetting);
            this.tabContent.Controls.Add(this.tabPage1);
            this.tabContent.Controls.Add(this.tabPage2);
            this.tabContent.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabContent.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabContent.Location = new System.Drawing.Point(0, 78);
            this.tabContent.Name = "tabContent";
            this.tabContent.SelectedIndex = 0;
            this.tabContent.Size = new System.Drawing.Size(1148, 583);
            this.tabContent.TabIndex = 0;
            // 
            // tbConfig
            // 
            this.tbConfig.Controls.Add(this.plOprationlist);
            this.tbConfig.Controls.Add(this.pActionList);
            this.tbConfig.Controls.Add(this.plAreaList);
            this.tbConfig.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbConfig.Location = new System.Drawing.Point(4, 29);
            this.tbConfig.Name = "tbConfig";
            this.tbConfig.Padding = new System.Windows.Forms.Padding(3);
            this.tbConfig.Size = new System.Drawing.Size(1140, 550);
            this.tbConfig.TabIndex = 0;
            this.tbConfig.Text = "配置";
            this.tbConfig.UseVisualStyleBackColor = true;
            // 
            // plOprationlist
            // 
            this.plOprationlist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plOprationlist.Location = new System.Drawing.Point(435, 3);
            this.plOprationlist.Name = "plOprationlist";
            this.plOprationlist.Size = new System.Drawing.Size(702, 544);
            this.plOprationlist.TabIndex = 2;
            // 
            // pActionList
            // 
            this.pActionList.Dock = System.Windows.Forms.DockStyle.Left;
            this.pActionList.Location = new System.Drawing.Point(165, 3);
            this.pActionList.Name = "pActionList";
            this.pActionList.Size = new System.Drawing.Size(270, 544);
            this.pActionList.TabIndex = 1;
            // 
            // plAreaList
            // 
            this.plAreaList.Dock = System.Windows.Forms.DockStyle.Left;
            this.plAreaList.Location = new System.Drawing.Point(3, 3);
            this.plAreaList.Name = "plAreaList";
            this.plAreaList.Size = new System.Drawing.Size(162, 544);
            this.plAreaList.TabIndex = 0;
            // 
            // tabTimeShaft
            // 
            this.tabTimeShaft.Controls.Add(this.panelTimeOperation);
            this.tabTimeShaft.Controls.Add(this.panelAction);
            this.tabTimeShaft.Location = new System.Drawing.Point(4, 29);
            this.tabTimeShaft.Name = "tabTimeShaft";
            this.tabTimeShaft.Padding = new System.Windows.Forms.Padding(3);
            this.tabTimeShaft.Size = new System.Drawing.Size(1140, 550);
            this.tabTimeShaft.TabIndex = 3;
            this.tabTimeShaft.Text = "时间轴";
            this.tabTimeShaft.UseVisualStyleBackColor = true;
            // 
            // panelTimeOperation
            // 
            this.panelTimeOperation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTimeOperation.Location = new System.Drawing.Point(323, 3);
            this.panelTimeOperation.Name = "panelTimeOperation";
            this.panelTimeOperation.Size = new System.Drawing.Size(814, 544);
            this.panelTimeOperation.TabIndex = 1;
            // 
            // panelAction
            // 
            this.panelAction.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelAction.Location = new System.Drawing.Point(3, 3);
            this.panelAction.Name = "panelAction";
            this.panelAction.Size = new System.Drawing.Size(320, 544);
            this.panelAction.TabIndex = 0;
            // 
            // tbAdvance
            // 
            this.tbAdvance.Controls.Add(this.btnCheckTime);
            this.tbAdvance.Controls.Add(this.btnSetTime);
            this.tbAdvance.Controls.Add(this.btnCancel);
            this.tbAdvance.Controls.Add(this.btnStart);
            this.tbAdvance.Controls.Add(this.panelContent);
            this.tbAdvance.Location = new System.Drawing.Point(4, 29);
            this.tbAdvance.Name = "tbAdvance";
            this.tbAdvance.Padding = new System.Windows.Forms.Padding(3);
            this.tbAdvance.Size = new System.Drawing.Size(1140, 550);
            this.tbAdvance.TabIndex = 1;
            this.tbAdvance.Text = "预约";
            this.tbAdvance.UseVisualStyleBackColor = true;
            // 
            // btnCheckTime
            // 
            this.btnCheckTime.Location = new System.Drawing.Point(628, 20);
            this.btnCheckTime.Name = "btnCheckTime";
            this.btnCheckTime.Size = new System.Drawing.Size(163, 37);
            this.btnCheckTime.TabIndex = 4;
            this.btnCheckTime.Text = "查看服务器时间";
            this.btnCheckTime.UseVisualStyleBackColor = true;
            this.btnCheckTime.Click += new System.EventHandler(this.btnCheckTime_Click);
            // 
            // btnSetTime
            // 
            this.btnSetTime.Location = new System.Drawing.Point(903, 20);
            this.btnSetTime.Name = "btnSetTime";
            this.btnSetTime.Size = new System.Drawing.Size(164, 37);
            this.btnSetTime.TabIndex = 3;
            this.btnSetTime.Text = "服务器时间校准";
            this.btnSetTime.UseVisualStyleBackColor = true;
            this.btnSetTime.Click += new System.EventHandler(this.btnSetTime_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(330, 20);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(85, 37);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "停止";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(108, 20);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(96, 37);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "启动";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // panelContent
            // 
            this.panelContent.Controls.Add(this.panelOrder);
            this.panelContent.Controls.Add(this.panelTime);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelContent.Location = new System.Drawing.Point(3, 74);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(1134, 473);
            this.panelContent.TabIndex = 0;
            // 
            // panelOrder
            // 
            this.panelOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOrder.Location = new System.Drawing.Point(210, 0);
            this.panelOrder.Name = "panelOrder";
            this.panelOrder.Size = new System.Drawing.Size(924, 473);
            this.panelOrder.TabIndex = 1;
            // 
            // panelTime
            // 
            this.panelTime.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelTime.Location = new System.Drawing.Point(0, 0);
            this.panelTime.Name = "panelTime";
            this.panelTime.Size = new System.Drawing.Size(210, 473);
            this.panelTime.TabIndex = 0;
            // 
            // tbSetting
            // 
            this.tbSetting.Controls.Add(this.prjStatePanel);
            this.tbSetting.Controls.Add(this.prjSetPanel);
            this.tbSetting.Location = new System.Drawing.Point(4, 29);
            this.tbSetting.Name = "tbSetting";
            this.tbSetting.Padding = new System.Windows.Forms.Padding(3);
            this.tbSetting.Size = new System.Drawing.Size(1140, 550);
            this.tbSetting.TabIndex = 2;
            this.tbSetting.Text = "常用投影机设置";
            this.tbSetting.UseVisualStyleBackColor = true;
            // 
            // prjStatePanel
            // 
            this.prjStatePanel.Location = new System.Drawing.Point(864, 3);
            this.prjStatePanel.Name = "prjStatePanel";
            this.prjStatePanel.Size = new System.Drawing.Size(276, 542);
            this.prjStatePanel.TabIndex = 1;
            // 
            // prjSetPanel
            // 
            this.prjSetPanel.Location = new System.Drawing.Point(0, 3);
            this.prjSetPanel.Name = "prjSetPanel";
            this.prjSetPanel.Size = new System.Drawing.Size(866, 542);
            this.prjSetPanel.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.relayNamePanel);
            this.tabPage1.Controls.Add(this.RelayStatePanel);
            this.tabPage1.Controls.Add(this.RelaySettingPanel);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1140, 550);
            this.tabPage1.TabIndex = 4;
            this.tabPage1.Text = "常用继电器设置";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // relayNamePanel
            // 
            this.relayNamePanel.Location = new System.Drawing.Point(3, 3);
            this.relayNamePanel.Name = "relayNamePanel";
            this.relayNamePanel.Size = new System.Drawing.Size(162, 544);
            this.relayNamePanel.TabIndex = 2;
            // 
            // RelayStatePanel
            // 
            this.RelayStatePanel.Location = new System.Drawing.Point(333, 3);
            this.RelayStatePanel.Name = "RelayStatePanel";
            this.RelayStatePanel.Size = new System.Drawing.Size(377, 542);
            this.RelayStatePanel.TabIndex = 1;
            // 
            // RelaySettingPanel
            // 
            this.RelaySettingPanel.Location = new System.Drawing.Point(168, 3);
            this.RelaySettingPanel.Name = "RelaySettingPanel";
            this.RelaySettingPanel.Size = new System.Drawing.Size(162, 544);
            this.RelaySettingPanel.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.UserDefinedNamepanel);
            this.tabPage2.Controls.Add(this.customPanel);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1140, 550);
            this.tabPage2.TabIndex = 5;
            this.tabPage2.Text = "自定义动作设置";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // UserDefinedNamepanel
            // 
            this.UserDefinedNamepanel.Location = new System.Drawing.Point(3, 6);
            this.UserDefinedNamepanel.Name = "UserDefinedNamepanel";
            this.UserDefinedNamepanel.Size = new System.Drawing.Size(139, 541);
            this.UserDefinedNamepanel.TabIndex = 0;
            // 
            // customPanel
            // 
            this.customPanel.Location = new System.Drawing.Point(148, 5);
            this.customPanel.Name = "customPanel";
            this.customPanel.Size = new System.Drawing.Size(988, 542);
            this.customPanel.TabIndex = 0;
            // 
            // Setting
            // 
            this.Setting.Controls.Add(this.btnUpload);
            this.Setting.Controls.Add(this.btnSave);
            this.Setting.Controls.Add(this.btnDowbload);
            this.Setting.Controls.Add(this.tbIP);
            this.Setting.Controls.Add(this.lbIP);
            this.Setting.Dock = System.Windows.Forms.DockStyle.Top;
            this.Setting.Location = new System.Drawing.Point(0, 0);
            this.Setting.Name = "Setting";
            this.Setting.Size = new System.Drawing.Size(1148, 56);
            this.Setting.TabIndex = 1;
            // 
            // btnUpload
            // 
            this.btnUpload.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUpload.Location = new System.Drawing.Point(846, 18);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(85, 38);
            this.btnUpload.TabIndex = 6;
            this.btnUpload.Text = "上传配置";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave.Location = new System.Drawing.Point(645, 18);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(86, 37);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "保存配置";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDowbload
            // 
            this.btnDowbload.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDowbload.Location = new System.Drawing.Point(445, 18);
            this.btnDowbload.Name = "btnDowbload";
            this.btnDowbload.Size = new System.Drawing.Size(93, 38);
            this.btnDowbload.TabIndex = 3;
            this.btnDowbload.Text = "下载配置";
            this.btnDowbload.UseVisualStyleBackColor = true;
            this.btnDowbload.Click += new System.EventHandler(this.btnDowbload_Click);
            this.btnDowbload.ChangeUICues += new System.Windows.Forms.UICuesEventHandler(this.btnDowbload_ChangeUICues);
            // 
            // tbIP
            // 
            this.tbIP.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbIP.Location = new System.Drawing.Point(112, 18);
            this.tbIP.Name = "tbIP";
            this.tbIP.Size = new System.Drawing.Size(188, 26);
            this.tbIP.TabIndex = 1;
            this.tbIP.Text = "127.0.0.1";
            // 
            // lbIP
            // 
            this.lbIP.AutoSize = true;
            this.lbIP.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbIP.Location = new System.Drawing.Point(12, 21);
            this.lbIP.Name = "lbIP";
            this.lbIP.Size = new System.Drawing.Size(94, 16);
            this.lbIP.TabIndex = 0;
            this.lbIP.Text = "服务器IP：";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1148, 661);
            this.Controls.Add(this.tabContent);
            this.Controls.Add(this.Setting);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "羿飞中控配置程序";
            this.tabContent.ResumeLayout(false);
            this.tbConfig.ResumeLayout(false);
            this.tabTimeShaft.ResumeLayout(false);
            this.tbAdvance.ResumeLayout(false);
            this.panelContent.ResumeLayout(false);
            this.tbSetting.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.Setting.ResumeLayout(false);
            this.Setting.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabContent;
        private System.Windows.Forms.TabPage tbConfig;
        private System.Windows.Forms.TabPage tbAdvance;
        private System.Windows.Forms.TabPage tbSetting;
        private System.Windows.Forms.Panel Setting;
        private System.Windows.Forms.Button btnDowbload;
        private System.Windows.Forms.TextBox tbIP;
        private System.Windows.Forms.Label lbIP;
        private System.Windows.Forms.Panel plAreaList;
        private System.Windows.Forms.Panel pActionList;
        private System.Windows.Forms.Panel plOprationlist;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Panel panelOrder;
        private System.Windows.Forms.Panel panelTime;
        private System.Windows.Forms.Button btnCheckTime;
        private System.Windows.Forms.Button btnSetTime;
        private System.Windows.Forms.TabPage tabTimeShaft;
        private System.Windows.Forms.Panel panelTimeOperation;
        private System.Windows.Forms.Panel panelAction;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel prjStatePanel;
        private System.Windows.Forms.Panel prjSetPanel;
        private System.Windows.Forms.Panel RelayStatePanel;
        private System.Windows.Forms.Panel RelaySettingPanel;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel customPanel;
        private System.Windows.Forms.Panel UserDefinedNamepanel;
        private System.Windows.Forms.Panel relayNamePanel;
    }
}

