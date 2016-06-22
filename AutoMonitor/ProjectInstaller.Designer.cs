namespace AutoMonitor
{
    partial class ProjectInstaller
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
            this.MyFirstServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.MyFirstServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // MyFirstServiceProcessInstaller
            // 
            this.MyFirstServiceProcessInstaller.Password = null;
            this.MyFirstServiceProcessInstaller.Username = null;
            // 
            // MyFirstServiceInstaller
            // 
            this.MyFirstServiceInstaller.Description = "This is Mointor ControlCenter";
            this.MyFirstServiceInstaller.ServiceName = "MyWindowService";
            this.MyFirstServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.MyFirstServiceProcessInstaller,
            this.MyFirstServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller MyFirstServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller MyFirstServiceInstaller;
    }
}