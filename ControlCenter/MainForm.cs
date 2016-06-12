using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using SFLib;
using System.Text;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Linq;
using System.Timers;
namespace ControlCenter
{
    public partial class MainForm : Form
    {
        HttpServer _httpServer = new HttpServer();
        ClientReceiver _receiver = new ClientReceiver();
        Listener _udpServer ;
        Listener _comServer;
        TcpScriptListener _tcpServer ;
        Thread thread;
        EfeeLockDog _lockDog = new EfeeLockDog();

        public MainForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            thread = new Thread(CheckDogState);
            thread.IsBackground = true;
            thread.Start();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Config.Load(AppDomain.CurrentDomain.BaseDirectory + "Config.ini");//加载配置文件

            _udpServer = new UDPListener(int.Parse(Config.Items["UdpPort"]));
            if (Config.Items["IsComEnable"] == "1")
            {
                _comServer = new COMListener(Config.Items["ComPort"], Config.Items["ComNewLine"]);
            }
            _tcpServer = new TcpScriptListener(int.Parse(Config.Items["TcpPort"]));

            InitLogger();

            StartServers();
           
        }


        #region 定时检测加密狗状态
        private void CheckDogState()
        {
            while (true)
            {
                Thread.Sleep(3000);
                if (_lockDog.CheckDog_IsExist())
                {
                    Console.WriteLine("加密狗存在！");
                }
                else
                {
                    MessageBox.Show("未检测到加密狗！");
                    Environment.Exit(0);
                }
               
            }
                     
        }

        #endregion

        /// <summary>
        /// 检查加密狗
        /// </summary>
        /// <returns></returns>
        private bool CheckLockDog()
        {
            bool ret = true;
            if (!_lockDog.CheckDog())
            {
                ret = false;
            }
            return ret;
        }

        /// <summary>
        /// 初始化日志类
        /// </summary>
        private void InitLogger()
        {
            int maxLine = 100;
            Logger.RegisterTextBoxListener(txtLog, maxLine);
            Logger.RegisterConsoleListener();
            Logger.RegisterTextWriterListener();
            Logger.ExitApplication += delegate()
                {
                    this.Hide();
                    DisposeIcon();
                };
        }


        private void StartServers()
        {
            try
            {
                _receiver.Start();
                _receiver.OnConfigUpdated += new EventHandler(UpdateConfig);
                _receiver.OnTimeLineUpdated += UpdateTimeConfig;
                _receiver.Dog = _lockDog;

                if (CheckLockDog())
                {
                    if (Config.Items["Http"] == "1") _httpServer.Start(new ScriptServer(Config.Items["ProjectName"]));
                    if (Config.Items["UDP"] == "1") _udpServer.Start(Config.Items["ProjectName"]);

                    if (_comServer != null)
                        _comServer.Start(Config.Items["ProjectName"]);

                    if (Config.Items["TCP"] == "1") _tcpServer.Start(Config.Items["ProjectName"]);

                    string scriptRoot = AppDomain.CurrentDomain.BaseDirectory + "Script\\";
                    if (File.Exists(scriptRoot + "TimeLine.lua"))
                    {
                        ScriptEngineer engineer = new ScriptEngineer();
                        engineer.Start(Config.Items["ProjectName"]);
                        engineer.ExecuteScript("RunTimeLine", new NameValueCollection());
                    }
                }
                else
                {
                    MessageBox.Show("未检测到加密狗！");
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                Logger.Exception(ex.Message);
            }
        }

        string[] _keepScript = new string[] { "scriptTemplate.lua", "TimeLineTemplate.lua", "TimeLine.lua", "RunTimeLine.lua" , "TimeShaft.lua"};

        /// <summary>
        /// 更新配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateConfig(object sender, EventArgs e)
        {
            DirectoryInfo di = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "Script");
            FileInfo[] files = di.GetFiles("*.lua");

            foreach(FileInfo fi in files)
            {
                if (!_keepScript.Contains(fi.Name)) File.Delete(fi.FullName);
            }

            ConfigWriter.LoadConfig();
            ConfigWriter.LoadTimeShaft();
        }

        /// <summary>
        /// 更新时间配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateTimeConfig(object sender, EventArgs e)
        {
            ConfigWriter.LoadTimeLineConfig();
        }


        /// <summary>
        /// 广播
        /// </summary>
        private void BroadcastServerAlive()
        {
            byte[] localIpBytes = IPAddress.Parse(NetLib.GetLocalIpString()).GetAddressBytes();
            while (true)
            {
                if (!NetLib.BroadcastUdpData(Int32.Parse(Config.Items["BroadcastPort"]), localIpBytes))
                {
                    Logger.Warning("Server Alive 广播发送失败!");
                }
                Thread.Sleep(1000);
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Logger.Info("回收资源中，请稍候...");
            _httpServer.Stop();
            _udpServer.Stop();
            
            _receiver.Stop();
            DisposeIcon();
        }


        internal void DisposeIcon()
        {
            if (notifyIcon == null)
                return;

            notifyIcon.Visible = false;
            notifyIcon.Dispose();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }

        private void 还原ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            WindowState = FormWindowState.Normal;
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
    }
}
