using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using Configuring.Business;
using System.Xml;
using SFLib;
using Configuring.Server;
namespace Configuring.UI
{
    public partial class MainForm : Form
    {
        TcpClient _client;
        string _hostname;
        int _port = 10003;
        int _blockLength = 1024;

        string _configFile = AppDomain.CurrentDomain.BaseDirectory + "config.xml";

        Controls.UserAreaList _arealist = new Controls.UserAreaList();//展项名称
        Controls.UserActionList _actionlist = new Controls.UserActionList();//控制项，客户端发送命令
        Controls.UserOprationList _oprationlist = new Controls.UserOprationList();//动作名称设置
        Controls.UserTime _timeList = new Controls.UserTime();//时间轴
        Controls.UserOprationList _orderlist = new Controls.UserOprationList();//预约
        Controls.UserTimeShaft _shaftList = new Controls.UserTimeShaft();
        Controls.UserTimeOperation _timeOperations = new Controls.UserTimeOperation();
        Controls.UserPrjSettingList _prjsettinglist = new Controls.UserPrjSettingList();//常用投影机设置
        Controls.UserPrjStateList _prjstatelist = new Controls.UserPrjStateList();//常用投影机开关数据配置

        Controls.UserRelayList _relayModulelist = new Controls.UserRelayList();//常用继电器模组数据配置
        Controls.UserRelaySettingList _relaysettinglist = new Controls.UserRelaySettingList();//常用继电器数据配置
        Controls.UserRelayStateList _relaystatelist = new Controls.UserRelayStateList();//常用继电器数据配置

        Controls.UserDefinedOprationList _userdefinedlist = new Controls.UserDefinedOprationList();//自定义命令配置
        Controls.UserDefinedNameList _userdefinedNamelist = new Controls.UserDefinedNameList();

        List<UserArea> _areas = new List<UserArea>();
        List<UserOrder> _orders = new List<UserOrder>();
        List<UserAction> _shaft_actions = new List<UserAction>();
        List<UserPrjSetting> _prjSettings = new List<UserPrjSetting>();
        List<UserDefinedOperation> _userdefineNames = new List<UserDefinedOperation>();
        List<UserRelayArray> _relays = new List<UserRelayArray>();

        CommunicationServer _NetServer=new CommunicationServer();
        public MainForm()
        {
            InitializeComponent();
            LoadConfig();

            plAreaList.Controls.Clear();
            plAreaList.Controls.Add(_arealist);
            _arealist.Areas = _areas;
            _arealist.OnCurrentAreaChanged += OnAreaListUpdated;
            _arealist.Dock = DockStyle.Fill;
          
            pActionList.Controls.Clear();
            pActionList.Controls.Add(_actionlist); 
            _actionlist.OnCurrentActionChange += OnActionListUpdated;
            _actionlist.Dock = DockStyle.Fill;
 
            plOprationlist.Controls.Clear();
            plOprationlist.Controls.Add(_oprationlist);
            _oprationlist.Dock = DockStyle.Fill;

            panelTime.Controls.Clear();
            panelTime.Controls.Add(_timeList);
            _timeList.OnCurrentOrderChanged += OnTimeListUpdated;
            _timeList.Dock = DockStyle.Fill;
            _timeList.Orders = _orders;

            panelOrder.Controls.Clear();
            panelOrder.Controls.Add(_orderlist);
            _orderlist.Dock = DockStyle.Fill;

            //常用投影机
            prjSetPanel.Controls.Clear();
            prjSetPanel.Controls.Add(_prjsettinglist);
            _prjsettinglist.UpSettings = _prjSettings;
            _prjsettinglist.OnCurrentPrjSetChanged += OnPrjSettingListUpdated;
            prjSetPanel.Dock = DockStyle.None;


            prjStatePanel.Controls.Clear();
            prjStatePanel.Controls.Add(_prjstatelist);
            prjStatePanel.Dock = DockStyle.None;

            //常用继电器配置
            relayNamePanel.Controls.Clear();
            relayNamePanel.Controls.Add(_relayModulelist);
            _relayModulelist.RelayModules = _relays;
            _relayModulelist.OnCurrentRelayChanged += OnUserRelayListUpdated;
            _relayModulelist.Dock = DockStyle.Fill;

            RelaySettingPanel.Controls.Clear();
            RelaySettingPanel.Controls.Add(_relaysettinglist);
            _relaysettinglist.OnCurrentRelayChanged += OnRelaySettingListUpdated;
            _relaysettinglist.Dock = DockStyle.Fill;

            RelayStatePanel.Controls.Clear();
            RelayStatePanel.Controls.Add(_relaystatelist);
            _relaystatelist.Dock = DockStyle.Fill;


            //用户自定义命令
            UserDefinedNamepanel.Controls.Clear();
            UserDefinedNamepanel.Controls.Add(_userdefinedNamelist);
            _userdefinedNamelist.DefinedNames = _userdefineNames;
            _userdefinedNamelist.OnCurrentUserDefinedChanged += OnUserDefinedChanged;
            _userdefinedNamelist.Dock = DockStyle.Fill;

            customPanel.Controls.Clear();
            customPanel.Controls.Add(_userdefinedlist);
            _userdefinedlist.Dock = DockStyle.Fill;

            panelAction.Controls.Clear();
            panelAction.Controls.Add(_shaftList);
            _shaftList.ActionList = _shaft_actions;
            _shaftList.Dock = DockStyle.Fill;
            _shaftList.OnCurrentActionChange += new EventHandler(OnUpdateShaft);
            
            panelTimeOperation.Controls.Clear();
            panelTimeOperation.Controls.Add(_timeOperations);
            _timeOperations.Dock = DockStyle.Fill;

            _arealist.RefreshAreaList();
            _timeList.RefreshTime();
            _shaftList.RefreshActionList();
            _prjsettinglist.RefreshPrjSetting();
            _relayModulelist.RefreshRelayList();
            _relaysettinglist.RefreshRelay();
            _userdefinedNamelist.RefreshAreaList();
            tbIP.Text = Utility.ConfigData.GetInstance().GetIP();
            tbSerialPortCount.Text = Utility.ConfigData.GetInstance().GetSerialPortCount().ToString();
            CheckLockState();
        }

        private void OnActionListUpdated(object sender, EventArgs e)
        {
            if (_actionlist.CurrentAction != null)
            {
                _oprationlist.Opreations = _actionlist.CurrentAction.Operations;
            }
            else
            {
                 _oprationlist.Opreations  = null;
            }


            if (_userdefinedNamelist.DefinedNames != null)
            {
                if (_userdefinedNamelist.DefinedNames.Count > 0)
                    _oprationlist.UserDefinedNameList = _userdefinedNamelist.DefinedNames;
            }
            else
            {
                _oprationlist.UserDefinedNameList =null;
            }

            if (_prjsettinglist.UpSettings !=null)
            {
                  if(_prjsettinglist.UpSettings.Count>0)
                    _oprationlist.PrjSettings =_prjsettinglist.UpSettings;
            }
            else
            {
                _oprationlist.PrjSettings = null;
 
            }

            if (_relayModulelist.RelayModules != null)
            {
                if (_relayModulelist.RelayModules.Count > 0)
                    _oprationlist.RelaySettings = _relayModulelist.RelayModules;
            }
            else
            {
                _oprationlist.RelaySettings = null;
            }
            
            _oprationlist.RefreshOprations();
        }

        private void OnAreaListUpdated(object sender, EventArgs e)
         {
            if (_arealist.CurrentArea != null)
            {
                _actionlist.ActionList = _arealist.CurrentArea.Actions;
            }
            else
            {
                _arealist.Areas = new List<UserArea>();
                _actionlist.ActionList = null;
            }

             _actionlist.RefreshActionList();
        }

        private void OnTimeListUpdated(object sender, EventArgs e)
        {
            if (_timeList.CurrentOrder != null)
            {
                _orderlist.Opreations = _timeList.CurrentOrder.Operations;
            }
            else
            {
                _orderlist.Opreations = null;
            }
            _orderlist.RefreshOprations();
        }

        private void OnUpdateShaft(object sender, EventArgs e)
        {
            if (_shaftList.CurrentAction != null)
            {
                _timeOperations.Opreations = _shaftList.CurrentAction.Operations;
            }
            else
            {
                _shaftList.ActionList = new List<UserAction>();
                _timeOperations.Opreations = null;
            }

            _timeOperations.RefreshOprations();
        }

        private void OnPrjSettingListUpdated(object sender, EventArgs e)
        {
            if (_prjsettinglist.CurrentUpSetting != null)
            {
                _prjstatelist.PrjStates = _prjsettinglist.CurrentUpSetting.DeviceStates;
            }
            else
            {
                _prjsettinglist.UpSettings = new List<UserPrjSetting>();
                _prjstatelist.PrjStates = null;
            }

            _prjstatelist.RefreshPrjStateList();
        }

        private void OnUserRelayListUpdated(object sender, EventArgs e)
        {
            if (_relayModulelist.CurrentRelayModule != null)
            {
                _relaysettinglist.T_ApproachCount = _relayModulelist.CurrentRelayModule.ApproachCout;
                _relaysettinglist.Relays = _relayModulelist.CurrentRelayModule.RelayOperationDatas;
            }
            else
            {
                _relaysettinglist.Relays = null;
            }
            _relaysettinglist.RefreshRelay();
        }

        private void OnRelaySettingListUpdated(object sender, EventArgs e)
        {
            if (_relaysettinglist.CurrentRelay != null)
            {
                _relaystatelist.RelayOperationDataList = _relaysettinglist.CurrentRelay.RelayOperationDatas;
            }
            else
            {
                _relaystatelist.RelayOperationDataList = null;
            }
            _relaystatelist.RefreshRelayStateList();
 
        }

        private void OnUserDefinedChanged(object sender, EventArgs e)
        {
            if (_userdefinedNamelist.CurrentUserDefinedName != null)
            {
                _userdefinedlist.Opreations = _userdefinedNamelist.CurrentUserDefinedName.Operations;
            }
            else
            {
                _userdefinedlist.Opreations = null;
            }

            if (_prjsettinglist.UpSettings != null)
            {
                if (_prjsettinglist.UpSettings.Count > 0)
                    _userdefinedlist.PrjSettings = _prjsettinglist.UpSettings;
            }
            else
            {
                _userdefinedlist.PrjSettings = null;
            }

            if (_relayModulelist.RelayModules != null)
            {
                if (_relayModulelist.RelayModules.Count > 0)
                    _userdefinedlist.RelaySettings = _relayModulelist.RelayModules;
            }
            else
            {
                _userdefinedlist.RelaySettings = null;
            }

            _userdefinedlist.RefreshOprations();
        }

        #region 保存配置
        private void SaveConfig()
        {
            #region 基本配置
            NormalConfigServer.GetInstance().SaveConfig(_arealist.Areas, _shaftList.ActionList);//保存配置
            NormalConfigServer.GetInstance().SaveOrderConfig(_timeList.Orders);
            #endregion

            #region 继电器配置保存
            RelayConfigServer.GetInstance().SaveRelayConfig(_relayModulelist.RelayModules);
            #endregion

            #region 投影机配置
            PrjConfigServer.GetInstance().SavePrjConfig(_prjsettinglist.UpSettings);
            #endregion

            #region 自定义命令保存
            CustomConfigServer.GetInstance().SaveUserDefinedData(_userdefinedNamelist.DefinedNames);
            #endregion
        }
        #endregion

        #region 加载配置
        //加载配置
        private void LoadConfig()
        {

            #region 基本配置加载
            _areas = NormalConfigServer.GetInstance().LoadConfig();
            _arealist.RefreshAreaList();
            #endregion

            #region 时间轴配置加载
            _shaft_actions = NormalConfigServer.GetInstance().LoadTimeShaft();
            _shaftList.RefreshActionList();
            #endregion

            #region 预约配置加载
            _orders = NormalConfigServer.GetInstance().LoadOrderConfig();
            _orderlist.RefreshOprations();
            #endregion

            #region 常用继电器配置加载
            _relays = RelayConfigServer.GetInstance().LoadRelayConfig();
            _relayModulelist.RefreshRelayList();
            #endregion

            #region 常用投影机配置加载
            _prjSettings = PrjConfigServer.GetInstance().LoadPrjData();
            _prjsettinglist.RefreshPrjSetting();
            #endregion
            _userdefineNames = CustomConfigServer.GetInstance().LoadUserDefinedData();
            _userdefinedNamelist.RefreshAreaList();
            #region 自定义命令加载

            #endregion
        }
        #endregion

        #region 连接服务器
        private void btnConnnect_Click(object sender, EventArgs e)
        {
             _hostname = tbIP.Text.Trim();
             _port = 10003;

            if (_client == null || !_client.Connected)
            {
                try
                {
                    Start();
                    Connect();
                }
                catch
                {
                    //ToDo
                }
                
            }
        }

        private void btnDowbload_Click(object sender, EventArgs e)
        {
            Start();
            Connect();

            if (_client.Connected)
            {
                NetworkStream ns = _client.GetStream();
                string downloadCommand = "GetData";
                Byte[] sendBytes = Encoding.UTF8.GetBytes(downloadCommand);
                ns.Write(sendBytes, 0, sendBytes.Length);

                try
                {
                    if (Helper.ShowMessageBox("操作确认", "确定下载配置，并覆盖本地配置吗？", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                                    == System.Windows.Forms.DialogResult.OK)
                    {
                        if (_client.Connected)
                        {

                            FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "config.xml", FileMode.Create, FileAccess.Write);
                            int size = 0;
                            byte[] buffer = new byte[_blockLength];
                            while ((size = _client.GetStream().Read(buffer, 0, _blockLength)) > 0)
                            {
                                fs.Write(buffer, 0, size);
                            }
                            fs.Flush();
                            fs.Close();

                            if (this.InvokeRequired)
                            {
                                this.Invoke(new MethodInvoker(() =>
                                {
                                    Helper.ShowMessageBox("提示", "下载完成", MessageBoxButtons.OK);
                                }));
                            }
                            else
                            {
                                Helper.ShowMessageBox("提示", "下载完成", MessageBoxButtons.OK);
                            }

                            LoadConfig();
                            // LoadTimeLineConfig();

                        }
                        else
                        {
                            Helper.ShowMessageBox("连接异常", "连接异常，无法下载");
                        }
                    }
                }
                catch
                {
                    Helper.ShowMessageBox("下载失败", "下载失败，可能网络连接不畅或服务器上没有配置！");
                }
            }

        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
       
            SaveConfig();
            _NetServer.UploadConfig(tbIP.Text);
        }

     
        private void Start()
        {
            _client = new TcpClient();
            _client.ReceiveTimeout = 1000 * 10;
        }

        private void Connect()
        {
            try
            {
                _hostname = tbIP.Text;
                _client.Connect(IPAddress.Parse(_hostname), _port);
                Utility.ConfigData.GetInstance().SaveIP(_hostname);
            }
            catch (Exception ex)
            {
                Helper.ShowMessageBox("连接失败", ex.Message);
            }
        }

        private void Stop()
        {
            _client.Close();
        }
        #endregion

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveConfig();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            SaveConfig();
            _NetServer.UploadOrderConfig(tbIP.Text);
        }

        private void btnCheckTime_Click(object sender, EventArgs e)
        {
            _NetServer.GetServerTime(tbIP.Text);
        }

        private void btnSetTime_Click(object sender, EventArgs e)
        {
      
            _NetServer.SetServerTime(tbIP.Text);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _NetServer.StopOrder(tbIP.Text);
        }

        void CheckLockState()
        {
            try
            {
                Start();
                Connect();

                if (_client.Connected)
                {
                    NetworkStream ns = _client.GetStream();
                    string command = "GetLockState";

                    Byte[] sendBytes = Encoding.Default.GetBytes(command);
                    ns.Write(sendBytes, 0, sendBytes.Length);

                    byte[] receiveBuffer = new byte[_blockLength];
                    int readLength = ns.Read(receiveBuffer, 0, _blockLength);

                    string lockState = System.Text.Encoding.UTF8.GetString(receiveBuffer, 0, readLength);

                    if (!lockState.StartsWith("加密锁在有效期"))
                    {
                        //PanelActiveCode.Visible = true;
                        MessageBox.Show("未检测到加密锁或加密锁已过期！");
                    }
                    ns.Close();
                }
            }
            catch (Exception ex)
            {
                Helper.ShowMessageBox("错误", ex.Message);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveConfig();
        }

      

        #region 软件设置

        private void tbSerialPortCount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b') e.Handled = "0123456789".IndexOf(char.ToUpper(e.KeyChar)) < 0;
        }
        private void btnApply_Click(object sender, EventArgs e)
        {
            if (tbSerialPortCount.Text != "")
            {
                int _count = int.Parse(tbSerialPortCount.Text);
                Utility.ConfigData.GetInstance().SaveSerialPortCount(_count);
            }
            else
            {
 
            }
        }

        #endregion
    }

    public class StateObject
    {
        public NetworkStream stream = null;
        // Size of receive buffer.
        public const int BufferSize = 1024;
        public byte[] buffer = new byte[BufferSize];
    }
}
