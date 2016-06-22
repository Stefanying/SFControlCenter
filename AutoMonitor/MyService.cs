using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
namespace AutoMonitor
{
    public partial class MyService : ServiceBase
    {
        public MyService()
        {
            InitializeComponent();
        }

        string _filepath = AppDomain.CurrentDomain.BaseDirectory + "ControlCenter.exe";
        Thread _thread;
        protected override void OnStart(string[] args)
        {
               
            _thread = new Thread(CheckState);
            _thread.IsBackground = true;
            _thread.Start();
        }

        private void CheckState()
        {
            while (true)
            {
                Thread.Sleep(3000);
                Process[] app = Process.GetProcessesByName("ControlCenter");
                if (app.Length <= 0)
                {
                    Process.Start(_filepath);
                }
            }
        }

        protected override void OnStop()
        {
            _thread.Abort();
        }
    }
}
