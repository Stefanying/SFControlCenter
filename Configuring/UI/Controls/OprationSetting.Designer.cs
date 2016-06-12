namespace Configuring.UI.Controls
{
    partial class OprationSetting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbName = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lbOprationType = new System.Windows.Forms.Label();
            this.cbOprationType = new System.Windows.Forms.ComboBox();
            this.lbPort = new System.Windows.Forms.Label();
            this.lbdataType = new System.Windows.Forms.Label();
            this.cbDataType = new System.Windows.Forms.ComboBox();
            this.lbData = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.lbTime = new System.Windows.Forms.Label();
            this.tbTime = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbData = new Configuring.UI.Controls.TextEdit();
            this.btnSetting = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbName.Location = new System.Drawing.Point(12, 37);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(51, 16);
            this.lbName.TabIndex = 3;
            this.lbName.Text = "名称:";
            // 
            // tbName
            // 
            this.tbName.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbName.Location = new System.Drawing.Point(113, 34);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(163, 26);
            this.tbName.TabIndex = 4;
            // 
            // lbOprationType
            // 
            this.lbOprationType.AutoSize = true;
            this.lbOprationType.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbOprationType.Location = new System.Drawing.Point(12, 83);
            this.lbOprationType.Name = "lbOprationType";
            this.lbOprationType.Size = new System.Drawing.Size(85, 16);
            this.lbOprationType.TabIndex = 5;
            this.lbOprationType.Text = "通信方式:";
            // 
            // cbOprationType
            // 
            this.cbOprationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOprationType.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbOprationType.FormattingEnabled = true;
            this.cbOprationType.Items.AddRange(new object[] {
            "TCP",
            "UDP",
            "串口"});
            this.cbOprationType.Location = new System.Drawing.Point(113, 80);
            this.cbOprationType.Name = "cbOprationType";
            this.cbOprationType.Size = new System.Drawing.Size(163, 24);
            this.cbOprationType.TabIndex = 6;
            // 
            // lbPort
            // 
            this.lbPort.AutoSize = true;
            this.lbPort.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbPort.Location = new System.Drawing.Point(12, 125);
            this.lbPort.Name = "lbPort";
            this.lbPort.Size = new System.Drawing.Size(51, 16);
            this.lbPort.TabIndex = 7;
            this.lbPort.Text = "配置:";
            // 
            // lbdataType
            // 
            this.lbdataType.AutoSize = true;
            this.lbdataType.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbdataType.Location = new System.Drawing.Point(12, 175);
            this.lbdataType.Name = "lbdataType";
            this.lbdataType.Size = new System.Drawing.Size(85, 16);
            this.lbdataType.TabIndex = 11;
            this.lbdataType.Text = "数据类型:";
            // 
            // cbDataType
            // 
            this.cbDataType.Cursor = System.Windows.Forms.Cursors.Default;
            this.cbDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDataType.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbDataType.FormattingEnabled = true;
            this.cbDataType.Items.AddRange(new object[] {
            "十六进制",
            "字符串"});
            this.cbDataType.Location = new System.Drawing.Point(113, 172);
            this.cbDataType.Name = "cbDataType";
            this.cbDataType.Size = new System.Drawing.Size(163, 24);
            this.cbDataType.TabIndex = 12;
            this.cbDataType.SelectedIndexChanged += new System.EventHandler(this.cbDataType_SelectedIndexChanged);
            // 
            // lbData
            // 
            this.lbData.AutoSize = true;
            this.lbData.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbData.Location = new System.Drawing.Point(12, 213);
            this.lbData.Name = "lbData";
            this.lbData.Size = new System.Drawing.Size(85, 16);
            this.lbData.TabIndex = 13;
            this.lbData.Text = "数据内容:";
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.Location = new System.Drawing.Point(67, 416);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(94, 45);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOk.Location = new System.Drawing.Point(266, 416);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 45);
            this.btnOk.TabIndex = 16;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // lbTime
            // 
            this.lbTime.AutoSize = true;
            this.lbTime.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbTime.Location = new System.Drawing.Point(12, 363);
            this.lbTime.Name = "lbTime";
            this.lbTime.Size = new System.Drawing.Size(85, 16);
            this.lbTime.TabIndex = 17;
            this.lbTime.Text = "延迟执行:";
            // 
            // tbTime
            // 
            this.tbTime.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbTime.Location = new System.Drawing.Point(113, 360);
            this.tbTime.Name = "tbTime";
            this.tbTime.Size = new System.Drawing.Size(163, 26);
            this.tbTime.TabIndex = 18;
            this.tbTime.Text = "0";
            this.tbTime.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbTime_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(293, 363);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 16);
            this.label2.TabIndex = 19;
            this.label2.Text = "(毫秒)";
            // 
            // tbData
            // 
            this.tbData.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbData.Location = new System.Drawing.Point(113, 213);
            this.tbData.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbData.Mode = Configuring.UI.Controls.EditorMode.Character;
            this.tbData.Name = "tbData";
            this.tbData.Size = new System.Drawing.Size(243, 127);
            this.tbData.TabIndex = 20;
            this.tbData.TextValue = "";
            // 
            // btnSetting
            // 
            this.btnSetting.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSetting.Location = new System.Drawing.Point(113, 117);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(89, 33);
            this.btnSetting.TabIndex = 21;
            this.btnSetting.Text = "配置";
            this.btnSetting.UseVisualStyleBackColor = true;
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // OprationSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(396, 486);
            this.Controls.Add(this.btnSetting);
            this.Controls.Add(this.tbData);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbTime);
            this.Controls.Add(this.lbTime);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lbData);
            this.Controls.Add(this.cbDataType);
            this.Controls.Add(this.lbdataType);
            this.Controls.Add(this.lbPort);
            this.Controls.Add(this.cbOprationType);
            this.Controls.Add(this.lbOprationType);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.lbName);
            this.Name = "OprationSetting";
            this.Text = "OprationSetting";
            this.Controls.SetChildIndex(this.lbName, 0);
            this.Controls.SetChildIndex(this.tbName, 0);
            this.Controls.SetChildIndex(this.lbOprationType, 0);
            this.Controls.SetChildIndex(this.cbOprationType, 0);
            this.Controls.SetChildIndex(this.lbPort, 0);
            this.Controls.SetChildIndex(this.lbdataType, 0);
            this.Controls.SetChildIndex(this.cbDataType, 0);
            this.Controls.SetChildIndex(this.lbData, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnOk, 0);
            this.Controls.SetChildIndex(this.lbTime, 0);
            this.Controls.SetChildIndex(this.tbTime, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.tbData, 0);
            this.Controls.SetChildIndex(this.btnSetting, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label lbOprationType;
        private System.Windows.Forms.ComboBox cbOprationType;
        private System.Windows.Forms.Label lbPort;
        private System.Windows.Forms.Label lbdataType;
        private System.Windows.Forms.ComboBox cbDataType;
        private System.Windows.Forms.Label lbData;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lbTime;
        private System.Windows.Forms.TextBox tbTime;
        private System.Windows.Forms.Label label2;
        private TextEdit tbData;
        private System.Windows.Forms.Button btnSetting;
    }
}