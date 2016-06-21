namespace ControlCenter.UI
{
    partial class SoftSetWindow
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
            this.cbAutoRun = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbDelayTime = new System.Windows.Forms.TextBox();
            this.cbIsComEnable = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cbAutoRun
            // 
            this.cbAutoRun.AutoSize = true;
            this.cbAutoRun.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbAutoRun.Location = new System.Drawing.Point(20, 33);
            this.cbAutoRun.Name = "cbAutoRun";
            this.cbAutoRun.Size = new System.Drawing.Size(112, 20);
            this.cbAutoRun.TabIndex = 0;
            this.cbAutoRun.Text = "开机自启动";
            this.cbAutoRun.UseVisualStyleBackColor = true;
            this.cbAutoRun.CheckedChanged += new System.EventHandler(this.cbAutoRun_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(17, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "延迟执行时间:";
            // 
            // tbDelayTime
            // 
            this.tbDelayTime.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbDelayTime.Location = new System.Drawing.Point(153, 67);
            this.tbDelayTime.Name = "tbDelayTime";
            this.tbDelayTime.Size = new System.Drawing.Size(76, 26);
            this.tbDelayTime.TabIndex = 2;
            this.tbDelayTime.Text = "20";
            // 
            // cbIsComEnable
            // 
            this.cbIsComEnable.AutoSize = true;
            this.cbIsComEnable.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbIsComEnable.Location = new System.Drawing.Point(21, 105);
            this.cbIsComEnable.Name = "cbIsComEnable";
            this.cbIsComEnable.Size = new System.Drawing.Size(95, 20);
            this.cbIsComEnable.TabIndex = 3;
            this.cbIsComEnable.Text = "使用串口";
            this.cbIsComEnable.UseVisualStyleBackColor = true;
            // 
            // SoftSetWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 230);
            this.Controls.Add(this.cbIsComEnable);
            this.Controls.Add(this.tbDelayTime);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbAutoRun);
            this.Name = "SoftSetWindow";
            this.Text = "软件设置";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SoftSetWindow_FormClosing);
            this.Load += new System.EventHandler(this.SoftSetWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbAutoRun;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbDelayTime;
        private System.Windows.Forms.CheckBox cbIsComEnable;
    }
}