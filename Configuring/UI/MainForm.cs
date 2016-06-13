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

namespace Configuring.UI
{
    public partial class MainForm : Form
    {
        TcpClient _client;
        string _hostname;
        int _port = 10003;
        int _blockLength = 1024;

        string _configFile = AppDomain.CurrentDomain.BaseDirectory + "config.xml";
        string _prjConfigFile = AppDomain.CurrentDomain.BaseDirectory + "PrjData.xml";
        string _timelineConfig = AppDomain.CurrentDomain.BaseDirectory + "timeline.xml";
        string _relayConfigFile = AppDomain.CurrentDomain.BaseDirectory + "RelayData.xml";
        string _userdefinedConfigFile = AppDomain.CurrentDomain.BaseDirectory + "CustomData.xml";

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

        List<Area> _areas = new List<Area>();
        List<UserOrder> _orders = new List<UserOrder>();
        List<UserAction> _shaft_actions = new List<UserAction>();
        List<UserPrjSetting> _prjSettings = new List<UserPrjSetting>();
        List<UserDeviceState> _prjStates = new List<UserDeviceState>();
       // List<RelaySetting> _relayIds = new List<RelaySetting>();
        List<UserDeviceState> _relayStates = new List<UserDeviceState>();
        List<DefinedName> _userdefineNames = new List<DefinedName>();
        List<UserRelayArray> _relays = new List<UserRelayArray>();
        ComSetting _relayComSetting = new ComSetting();
        public MainForm()
        {
            InitializeComponent();
            LoadConfig();
            LoadTimeLineConfig();

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
            //_relaysettinglist.Relays = _relayIds;
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
            tbIP.Text = Utility.Data.GetInstance().GetIP();
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


            if (_userdefineNames.Count > 0)
            {
                _oprationlist.UserDefinedNameList = _userdefineNames;
            }
            else
            {
                _oprationlist.UserDefinedNameList = null;
            }

            if (_prjSettings.Count > 0)
            {
                _oprationlist.PrjSettings = _prjSettings;
            }
            else
            {
                _oprationlist.PrjSettings = null;
 
            }

            if (_relays.Count > 0 && _relays != null)
            {
                _oprationlist.RelaySettings = _relays;
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

            if (_prjSettings.Count > 0)
            {
                _userdefinedlist.PrjSettings = _prjSettings;
            }
            else
            {
                _userdefinedlist.PrjSettings = null;
            }

            if (_relays.Count > 0 && _relays != null)
            {
                _userdefinedlist.RelaySettings = _relays;
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
            XmlDocument config = new XmlDocument();

            XmlNode root = config.CreateNode(XmlNodeType.Element, "Root", null);
            config.AppendChild(root);

            #region NormalCommands
            for (int i = 0; i < _areas.Count; i++)
            {
                Area currentArea = _areas[i];

                XmlNode area = config.CreateNode(XmlNodeType.Element, "Area", null);
                XmlNode areaname = config.CreateNode(XmlNodeType.Element, "AreaName", null);
                areaname.InnerText = currentArea.Name;
                area.AppendChild(areaname);

                for (int count_action = 0; count_action < currentArea.Actions.Count; count_action++)
                {
                    UserAction temp = currentArea.Actions[count_action];

                    XmlNode action = config.CreateNode(XmlNodeType.Element, "Action", null);

                    XmlElement actionName = config.CreateElement("ActionName");
                    actionName.InnerText = temp.Name;
                    XmlElement receiveData = config.CreateElement("ActionReceiveData");
                    receiveData.InnerText = temp.ReceiveCommand;

                    action.AppendChild(actionName);
                    action.AppendChild(receiveData);

                    for (int count_opreation = 0; count_opreation < temp.Operations.Count; count_opreation++)
                    {
                        UserOperation operation = temp.Operations[count_opreation];

                        XmlNode operationNode = config.CreateNode(XmlNodeType.Element, "Operation", null);

                        XmlNode operationName = config.CreateNode(XmlNodeType.Element, "OperationName", null);
                        operationName.InnerText = operation.Name;

                        XmlNode operationType = config.CreateNode(XmlNodeType.Element, "OperationType", null);
                        operationType.InnerText = operation.OpreationType.ToString();

                        XmlNode operationDataType = config.CreateNode(XmlNodeType.Element, "OperationDataType", null);
                        operationDataType.InnerText = operation.DataType.ToString();

                        XmlNode operationData = config.CreateNode(XmlNodeType.Element, "OperationData", null);
                        if (operation.DataType == DataType.Hex)
                        {
                            operationData.InnerText = operation.Data.Replace(" ", "").Trim();
                        }
                        else
                        {
                            operationData.InnerText = operation.Data;
                        }

                        XmlNode operationTime = config.CreateNode(XmlNodeType.Element, "OperationTime", null);
                        operationTime.InnerText = operation.DelayTime.ToString();

                        XmlNode operationSetting = config.CreateNode(XmlNodeType.Element, "OperationSetting", null);
                        if (operation.Setting as ComSetting != null)
                        {
                            SaveComSetting(config, operation, operationSetting);
                        }
                        else if (operation.Setting as NetworkSetting != null)
                        {
                            SaveIPSetting(config, operation, operationSetting);
                        }

                        operationNode.AppendChild(operationName);
                        operationNode.AppendChild(operationType);
                        operationNode.AppendChild(operationDataType);
                        operationNode.AppendChild(operationData);
                        operationNode.AppendChild(operationTime);
                        operationNode.AppendChild(operationSetting);

                        action.AppendChild(operationNode);
                    }
                    area.AppendChild(action);
                }
                root.AppendChild(area);
            }
            #endregion

            #region TimeShaft
            XmlNode timeShaft = config.CreateNode(XmlNodeType.Element, "TimeShaft", null);
            root.AppendChild(timeShaft);

            for (int i = 0; i < _shaft_actions.Count; i++)
            {
                XmlNode action = config.CreateNode(XmlNodeType.Element, "Action", null);
                timeShaft.AppendChild(action);

                UserAction temp = _shaft_actions[i];
                XmlElement actionName = config.CreateElement("ActionName");
                actionName.InnerText = temp.Name;
                XmlElement receiveData = config.CreateElement("ActionReceiveData");
                receiveData.InnerText = temp.ReceiveCommand;

                action.AppendChild(actionName);
                action.AppendChild(receiveData);

                for (int count_opreation = 0; count_opreation < temp.Operations.Count; count_opreation++)
                {
                    UserOperation operation = temp.Operations[count_opreation];

                    XmlNode operationNode = config.CreateNode(XmlNodeType.Element, "Operation", null);

                    XmlNode operationName = config.CreateNode(XmlNodeType.Element, "OperationName", null);
                    operationName.InnerText = operation.Name;

                    XmlNode operationType = config.CreateNode(XmlNodeType.Element, "OperationType", null);
                    operationType.InnerText = operation.OpreationType.ToString();

                    XmlNode operationDataType = config.CreateNode(XmlNodeType.Element, "OperationDataType", null);
                    operationDataType.InnerText = operation.DataType.ToString();

                    XmlNode operationData = config.CreateNode(XmlNodeType.Element, "OperationData", null);
                    operationData.InnerText = operation.Data;

                    XmlNode operationTime = config.CreateNode(XmlNodeType.Element, "OperationTime", null);
                    operationTime.InnerText = operation.DelayTime.ToString();

                    XmlNode operationSetting = config.CreateNode(XmlNodeType.Element, "OperationSetting", null);
                    if (operation.Setting as ComSetting != null)
                    {
                        SaveComSetting(config, operation, operationSetting);
                    }
                    else if (operation.Setting as NetworkSetting != null)
                    {
                        SaveIPSetting(config, operation, operationSetting);
                    }

                    operationNode.AppendChild(operationName);
                    operationNode.AppendChild(operationType);
                    operationNode.AppendChild(operationDataType);
                    operationNode.AppendChild(operationData);
                    operationNode.AppendChild(operationTime);
                    operationNode.AppendChild(operationSetting);

                    action.AppendChild(operationNode);
                }
            }
            root.AppendChild(timeShaft);
            #endregion

            config.Save(_configFile);
        }

        private void SaveTimeLine()
        {
            XmlDocument config = new XmlDocument();

            XmlNode root = config.CreateNode(XmlNodeType.Element, "Root", null);
            config.AppendChild(root);

            for (int i = 0; i < _orders.Count; i++)
            {
                UserOrder currentOrder = _orders[i];

                XmlNode time = config.CreateNode(XmlNodeType.Element, "DelayTime", null);
                XmlNode timeValue = config.CreateNode(XmlNodeType.Element, "TimeValue", null);
                timeValue.InnerText = currentOrder.GetTime();

                time.AppendChild(timeValue);

                for (int count_opreation = 0; count_opreation < currentOrder.Operations.Count; count_opreation++)
                {
                    UserOperation operation = currentOrder.Operations[count_opreation];

                    XmlNode operationNode = config.CreateNode(XmlNodeType.Element, "Operation", null);

                    XmlNode operationName = config.CreateNode(XmlNodeType.Element, "OperationName", null);
                    operationName.InnerText = operation.Name;

                    XmlNode operationType = config.CreateNode(XmlNodeType.Element, "OperationType", null);
                    operationType.InnerText = operation.OpreationType.ToString();

                    XmlNode operationDataType = config.CreateNode(XmlNodeType.Element, "OperationDataType", null);
                    operationDataType.InnerText = operation.DataType.ToString();

                    XmlNode operationData = config.CreateNode(XmlNodeType.Element, "OperationData", null);
                    operationData.InnerText = operation.Data.Trim();

                    XmlNode operationTime = config.CreateNode(XmlNodeType.Element, "OperationTime", null);
                    operationTime.InnerText = operation.DelayTime.ToString();

                    XmlNode operationSetting = config.CreateNode(XmlNodeType.Element, "OperationSetting", null);
                    if (operation.Setting as ComSetting != null)
                    {
                        SaveComSetting(config, operation, operationSetting);
                    }
                    else if (operation.Setting as NetworkSetting != null)
                    {
                        SaveIPSetting(config, operation, operationSetting);
                    }

                    operationNode.AppendChild(operationName);
                    operationNode.AppendChild(operationType);
                    operationNode.AppendChild(operationDataType);
                    operationNode.AppendChild(operationData);
                    operationNode.AppendChild(operationTime);
                    operationNode.AppendChild(operationSetting);

                    time.AppendChild(operationNode);
                }
                  root.AppendChild(time);
                }
            config.Save(_timelineConfig);
        }
        private static void SaveIPSetting(XmlDocument config, UserOperation operation, XmlNode operationSetting)
        {
            NetworkSetting ns = operation.Setting as NetworkSetting;
            XmlNode ip = config.CreateNode(XmlNodeType.Element, "IP", null);
            ip.InnerText = ns.Ip;

            XmlNode port = config.CreateNode(XmlNodeType.Element, "Port", null);
            port.InnerText = ns.Port.ToString();

            operationSetting.AppendChild(ip);
            operationSetting.AppendChild(port);
        }

        private static void SaveComSetting(XmlDocument config, UserOperation operation, XmlNode operationSetting)
        {
            ComSetting cs = operation.Setting as ComSetting;
            XmlNode comNumber = config.CreateNode(XmlNodeType.Element, "ComNumber", null);
            comNumber.InnerText = cs.ComNumber;

            XmlNode baudRate = config.CreateNode(XmlNodeType.Element, "BaudRate", null);
            baudRate.InnerText = cs.BaudRate.ToString();

            XmlNode dataBit = config.CreateNode(XmlNodeType.Element, "DataBit", null);
            dataBit.InnerText = cs.DataBits.ToString();

            XmlNode stopBit = config.CreateNode(XmlNodeType.Element, "StopBit", null);
            stopBit.InnerText = cs.StopBits.ToString();

            XmlNode parity = config.CreateNode(XmlNodeType.Element, "Parity", null);
            parity.InnerText = cs.Parity.ToString();

            operationSetting.AppendChild(comNumber);
            operationSetting.AppendChild(baudRate);
            operationSetting.AppendChild(dataBit);
            operationSetting.AppendChild(stopBit);
            operationSetting.AppendChild(parity);
        }

        //保存投影机数据
        private void SavePrjData()
        {
            XmlDocument config = new XmlDocument();
            XmlNode root = config.CreateNode(XmlNodeType.Element, "ProjectorData", null);
            config.AppendChild(root);
            #region PrjData
            for (int i = 0; i < _prjSettings.Count; i++)
            {
                UserPrjSetting currentPrjSetting = _prjSettings[i];
                XmlNode deviceName = config.CreateNode(XmlNodeType.Element, currentPrjSetting.Name, null);
             
                for (int cout_mode = 0; cout_mode < currentPrjSetting.DeviceStates.Count; cout_mode++)
                {
                    UserDeviceState tempprjState = currentPrjSetting.DeviceStates[cout_mode];
                    XmlElement mode = config.CreateElement("Mode");
                    XmlAttribute _name = config.CreateAttribute("Name");
                    _name.Value = tempprjState.DeviceMode.ToString();
                    XmlAttribute _data = config.CreateAttribute("Data");
                    _data.Value = tempprjState.Data;
                    mode.Attributes.Append(_name);
                    mode.Attributes.Append(_data);
                    deviceName.AppendChild(mode);
                }

                XmlNode operationset = config.CreateNode(XmlNodeType.Element, "OperationSetting", null);
                XmlNode baudRate = config.CreateNode(XmlNodeType.Element, "BaudRate", null);
                baudRate.InnerText = currentPrjSetting.Pcs.BaudRate.ToString();
                XmlNode dataBit = config.CreateNode(XmlNodeType.Element, "DataBit", null);
                dataBit.InnerText = currentPrjSetting.Pcs.DataBits.ToString();
                XmlNode stopBit = config.CreateNode(XmlNodeType.Element, "StopBit", null);
                stopBit.InnerText = currentPrjSetting.Pcs.StopBits.ToString();
                XmlNode parity = config.CreateNode(XmlNodeType.Element, "Parity", null);
                parity.InnerText = currentPrjSetting.Pcs.Parity.ToString();
                operationset.AppendChild(baudRate);
                operationset.AppendChild(dataBit);
                operationset.AppendChild(stopBit);
                operationset.AppendChild(parity);
                deviceName.AppendChild(operationset);
                root.AppendChild(deviceName);
            }

            #endregion
            config.Save(_prjConfigFile);
        }

        string GetRelayStateType(RelayOperationType _state)
        {
            string ret = "吸合";
            switch (_state)
            {
                case RelayOperationType.吸合:
                    ret = "吸合";
                    break;
                case RelayOperationType.断开:
                    ret = "断开";
                    break;
            }
            return ret;
        }
        //保存继电器数据
        private void SaveRelayConfig()
        {
            #region RelayConfig
            try
            {
                XmlDocument config = new XmlDocument();
                XmlNode root = config.CreateNode(XmlNodeType.Element,"Root",null);
                config.AppendChild(root);
                for (int i = 0; i < _relays.Count;i++ )
                {
                    UserRelayArray _currentRelay = _relays[i];
                    XmlNode relayModoule = config.CreateNode(XmlNodeType.Element, "Relay", null);
                    XmlNode relayModouleName = config.CreateNode(XmlNodeType.Element, "Name", null);
                    XmlNode relayCount = config.CreateNode(XmlNodeType.Element, "TotoalApproach",null); 
                    relayModouleName.InnerText = _currentRelay.Name;
                    relayCount.InnerText = _currentRelay.ApproachCout.ToString();
                    relayModoule.AppendChild(relayModouleName);
                    relayModoule.AppendChild(relayCount);
                    XmlNode operationset = config.CreateNode(XmlNodeType.Element, "OperationSetting",null);

                    XmlNode comnumber = config.CreateNode(XmlNodeType.Element, "ComNumber", null);
                    comnumber.InnerText = _currentRelay.RelayCom.ComNumber;

                    XmlNode baudrate = config.CreateNode(XmlNodeType.Element, "BaudRate", null);
                    baudrate.InnerText = _currentRelay.RelayCom.BaudRate.ToString();

                    XmlNode dataBit = config.CreateNode(XmlNodeType.Element, "DataBit", null);
                    dataBit.InnerText = _currentRelay.RelayCom.DataBits.ToString();

                    XmlNode stopBit = config.CreateNode(XmlNodeType.Element, "StopBit", null);
                    stopBit.InnerText = _currentRelay.RelayCom.StopBits.ToString();

                    XmlNode parity = config.CreateNode(XmlNodeType.Element, "Parity", null);
                    parity.InnerText = _currentRelay.RelayCom.Parity.ToString();

                    operationset.AppendChild(comnumber);
                    operationset.AppendChild(baudrate);
                    operationset.AppendChild(dataBit);
                    operationset.AppendChild(stopBit);
                    operationset.AppendChild(parity);

                    relayModoule.AppendChild(operationset);

                    XmlNode relaydata = config.CreateNode(XmlNodeType.Element, "RelayData", null);

                    for (int relay_count = 0; relay_count < _currentRelay.RelayOperationDatas.Count; relay_count++)
                    {
                        UserRelaySetting _currentRelaySet = _currentRelay.RelayOperationDatas[relay_count];
                        XmlNode approach = config.CreateNode(XmlNodeType.Element,"Approach",null);
                        XmlAttribute id = config.CreateAttribute("Id");
                        id.Value = _currentRelaySet.RelayId.ToString();
                        approach.Attributes.Append(id);
                        for (int _relayOperationData=0; _relayOperationData < 2; _relayOperationData++)
                        {
                            RelayOperationDataList _currentRelayData = _currentRelaySet.RelayOperationDatas[0];
                            XmlElement mode = config.CreateElement("Mode");
                            XmlAttribute _name = config.CreateAttribute("Name");
                            _name.Value = GetRelayStateType((RelayOperationType)_relayOperationData);
                            XmlAttribute _data = config.CreateAttribute("Data");
                            _data.Value = _currentRelayData.GetOperationData((RelayOperationType)_relayOperationData);
                            mode.Attributes.Append(_name);
                            mode.Attributes.Append(_data);
                            approach.AppendChild(mode);
                        }
                        relaydata.AppendChild(approach);
                        relayModoule.AppendChild(relaydata);
                    }
                    root.AppendChild(relayModoule);
                    config.Save(_relayConfigFile);
                }
               
            }
            catch(Exception ex)
            {
                Helper.ShowMessageBox("异常",ex.Message);
            }

        
            #endregion
        }

        //保存自定义数据
        private void SaveUserDefinedData()
        {
            XmlDocument config = new XmlDocument();
            XmlNode root = config.CreateNode(XmlNodeType.Element, "Root", null);
            config.AppendChild(root);
            for (int i = 0; i < _userdefineNames.Count; i++)
            {
                DefinedName _currentName = _userdefineNames[i];
                XmlNode action = config.CreateNode(XmlNodeType.Element, "Action",null);
                XmlAttribute _name = config.CreateAttribute("Name");
                _name.Value = _currentName.Name;
                action.Attributes.Append(_name);
                for (int _operationcout=0; _operationcout < _currentName.Operations.Count; _operationcout++)  
                {
                    UserOperation operation = _currentName.Operations[_operationcout];
                    XmlNode operationNode = config.CreateNode(XmlNodeType.Element, "Operation", null);
                    XmlNode operationName = config.CreateNode(XmlNodeType.Element, "OperationName", null);
                    operationName.InnerText = operation.Name;

                    XmlNode operationType = config.CreateNode(XmlNodeType.Element, "OperationType", null);
                    operationType.InnerText = operation.OpreationType.ToString();

                    XmlNode operationDataType = config.CreateNode(XmlNodeType.Element, "OperationDataType", null);
                    operationDataType.InnerText = operation.DataType.ToString();

                    XmlNode operationData = config.CreateNode(XmlNodeType.Element, "OperationData", null);
                    if (operation.DataType == DataType.Hex)
                    {
                        operationData.InnerText = operation.Data.Replace(" ", "").Trim();
                    }
                    else
                    {
                        operationData.InnerText = operation.Data;
                    }

                    XmlNode operationTime = config.CreateNode(XmlNodeType.Element, "OperationTime", null);
                    operationTime.InnerText = operation.DelayTime.ToString();

                    XmlNode operationSetting = config.CreateNode(XmlNodeType.Element, "OperationSetting", null);
                    if (operation.Setting as ComSetting != null)
                    {
                        SaveComSetting(config, operation, operationSetting);
                    }
                    else if (operation.Setting as NetworkSetting != null)
                    {
                        SaveIPSetting(config, operation, operationSetting);
                    }

                    operationNode.AppendChild(operationName);
                    operationNode.AppendChild(operationType);
                    operationNode.AppendChild(operationDataType);
                    operationNode.AppendChild(operationData);
                    operationNode.AppendChild(operationTime);
                    operationNode.AppendChild(operationSetting);
                    action.AppendChild(operationNode);
                }
                root.AppendChild(action);
            }

            config.Save(_userdefinedConfigFile);
 
        }
        #endregion

        #region 加载配置
        //加载配置
        private void LoadConfig()
        {
            try
            {
                XmlDocument config = new XmlDocument();
                config.Load(_configFile);

                XmlNode root = config.SelectSingleNode("Root");

                #region LoadNormalCommand
                XmlNodeList areas = root.SelectNodes("Area");

                _areas.Clear();
                foreach (XmlNode area in areas)
                {
                    string areaName = area.SelectSingleNode("AreaName").InnerText;
                    Area tempArea = new Area(areaName);
                    _areas.Add(tempArea);

                    XmlNodeList actions = area.SelectNodes("Action");
                    foreach (XmlNode action in actions)
                    {
                        string actionName = action.SelectSingleNode("ActionName").InnerText;
                        string actionReceiveData = action.SelectSingleNode("ActionReceiveData").InnerText;

                        UserAction userAction = new UserAction(actionName, actionReceiveData);
                        tempArea.Actions.Add(userAction);

                        XmlNodeList operations = action.SelectNodes("Operation");
                        foreach (XmlNode operation in operations)
                        {
                            string operationName = operation.SelectSingleNode("OperationName").InnerText;
                            string operationTypeString = operation.SelectSingleNode("OperationType").InnerText;
                            CommunicationType operationType = (CommunicationType)Enum.Parse(typeof(CommunicationType), operationTypeString, true);
                            
                            XmlNode operationSetting = operation.SelectSingleNode("OperationSetting");
                            object setting = null;
                            if (operationType == CommunicationType.Com)
                            {
                                ComSetting cs = new ComSetting();
                                cs.ComNumber = operationSetting.SelectSingleNode("ComNumber").InnerText;
                                cs.BaudRate = int.Parse(operationSetting.SelectSingleNode("BaudRate").InnerText);
                                cs.DataBits = int.Parse(operationSetting.SelectSingleNode("DataBit").InnerText);
                                cs.StopBits = int.Parse(operationSetting.SelectSingleNode("StopBit").InnerText);
                                cs.Parity = (Parity)Enum.Parse(typeof(Parity), operationSetting.SelectSingleNode("Parity").InnerText);

                                setting = cs;
                            }
                            else if (operationType == CommunicationType.TCP || operationType == CommunicationType.UDP)
                            {
                                NetworkSetting ns = new NetworkSetting();
                                ns.Ip = operationSetting.SelectSingleNode("IP").InnerText;
                                ns.Port = int.Parse(operationSetting.SelectSingleNode("Port").InnerText);
                                setting = ns;
                            }

                            string dataTypeString = operation.SelectSingleNode("OperationDataType").InnerText;
                            DataType dataType = (DataType)Enum.Parse(typeof(DataType), dataTypeString, true);
                            string data = operation.SelectSingleNode("OperationData").InnerText;
                            int time = int.Parse(operation.SelectSingleNode("OperationTime").InnerText);

                            UserOperation userOperation = new UserOperation(operationName, operationType, dataType, setting, data, time);
                            userAction.Operations.Add(userOperation);
                        }
                    }
                }

                _arealist.RefreshAreaList();
                #endregion

                #region LoadTimeShaft
                XmlNode timeShafts = root.SelectSingleNode("TimeShaft");
                XmlNodeList timeActions = timeShafts.SelectNodes("Action");
                _shaft_actions.Clear();
                foreach (XmlNode action in timeActions)
                {
                    string actionName = action.SelectSingleNode("ActionName").InnerText;
                    string actionReceiveData = action.SelectSingleNode("ActionReceiveData").InnerText;

                    UserAction userAction = new UserAction(actionName, actionReceiveData);
                    _shaft_actions.Add(userAction);

                    XmlNodeList operations = action.SelectNodes("Operation");
                    foreach (XmlNode operation in operations)
                    {
                        string operationName = operation.SelectSingleNode("OperationName").InnerText;
                        string operationTypeString = operation.SelectSingleNode("OperationType").InnerText;
                        CommunicationType operationType = (CommunicationType)Enum.Parse(typeof(CommunicationType), operationTypeString, true);

                        XmlNode operationSetting = operation.SelectSingleNode("OperationSetting");
                        object setting = null;
                        if (operationType == CommunicationType.Com)
                        {
                            ComSetting cs = new ComSetting();
                            cs.ComNumber = operationSetting.SelectSingleNode("ComNumber").InnerText;
                            cs.BaudRate = int.Parse(operationSetting.SelectSingleNode("BaudRate").InnerText);
                            cs.DataBits = int.Parse(operationSetting.SelectSingleNode("DataBit").InnerText);
                            cs.StopBits = int.Parse(operationSetting.SelectSingleNode("StopBit").InnerText);
                            cs.Parity = (Parity)Enum.Parse(typeof(Parity), operationSetting.SelectSingleNode("Parity").InnerText);

                            setting = cs;
                        }
                        else if (operationType == CommunicationType.TCP || operationType == CommunicationType.UDP)
                        {
                            NetworkSetting ns = new NetworkSetting();
                            ns.Ip = operationSetting.SelectSingleNode("IP").InnerText;
                            ns.Port = int.Parse(operationSetting.SelectSingleNode("Port").InnerText);
                            setting = ns;
                        }

                        string dataTypeString = operation.SelectSingleNode("OperationDataType").InnerText;
                        DataType dataType = (DataType)Enum.Parse(typeof(DataType), dataTypeString, true);
                        string data = operation.SelectSingleNode("OperationData").InnerText;
                        int time = int.Parse(operation.SelectSingleNode("OperationTime").InnerText);

                        UserOperation userOperation = new UserOperation(operationName, operationType, dataType, setting, data, time);
                        userAction.Operations.Add(userOperation);
                    }
                }
                _shaftList.RefreshActionList();

                #endregion

                #region LoadPrjData
                LoadPrjData();
                #endregion

                #region LoadRelayData
                LoadRelayConfig();
                #endregion

                #region LoadUserDefinedData
                LoadUserDefinedData();
                #endregion
            }
            catch (Exception ex)
            {
               
               Helper.ShowMessageBox("提示", "未找到命令配置!");
            }
        }

        //加载投影机数据
        private void LoadPrjData()
        {
            try
            {
                XmlDocument prjConfig = new XmlDocument();
                prjConfig.Load(_prjConfigFile);
                XmlNode prjroot = prjConfig.SelectSingleNode("ProjectorData");
                _prjSettings.Clear();
                _prjStates.Clear();
                foreach(XmlNode prjector in prjroot)
                {
                    string _prjName = prjector.Name;
                    XmlNodeList _prjCommsetting = prjector.SelectNodes("OperationSetting");
                    ComSetting pcs = new ComSetting();
                   foreach (XmlNode comsetting in _prjCommsetting)
                   {
                       int _baudRate = int.Parse(comsetting.SelectSingleNode("BaudRate").InnerText);
                       int _dataBits = int.Parse(comsetting.SelectSingleNode("DataBit").InnerText);
                       int _stopBits = int.Parse(comsetting.SelectSingleNode("StopBit").InnerText);
                       Parity _parity = (Parity)Enum.Parse(typeof(Parity), comsetting.SelectSingleNode("Parity").InnerText);   
                       pcs.BaudRate = _baudRate;
                       pcs.DataBits = _dataBits;
                       pcs.StopBits = _stopBits;
                       pcs.Parity = _parity;
                   }
                   UserPrjSetting _ups = new UserPrjSetting(_prjName,pcs);
                   XmlNodeList _tempprjStates = prjector.SelectNodes("Mode");
                   _prjSettings.Add(_ups);
                   foreach (XmlNode _prjState in _tempprjStates)
                   {
                       string _statename = _prjState.Attributes["Name"].Value;
                       string _statedata = _prjState.Attributes["Data"].Value;
                       PrjState _mode = (PrjState)Enum.Parse(typeof(PrjState), _statename);
                       UserDeviceState uds = new UserDeviceState(_mode, _statedata);
                       _ups.DeviceStates.Add(uds);
                   }

                }
                _prjsettinglist.RefreshPrjSetting();
            }
            catch (Exception ex)
            {
                Helper.ShowMessageBox("异常",ex.Message);
            }
        }

        //加载继电器数据
        private void LoadRelayConfig()
        {
            try
            {
                XmlDocument relayConfig = new XmlDocument();
                relayConfig.Load(_relayConfigFile);
                XmlNode root = relayConfig.SelectSingleNode("Root");

                XmlNodeList relays = root.SelectNodes("Relay");
                _relays.Clear();

                foreach (XmlNode relay in relays)
                {
                    string name = relay.SelectSingleNode("Name").InnerText;
                    int _totoalApproachCount = int.Parse(relay.SelectSingleNode("TotoalApproach").InnerText);
                    ComSetting cs = new ComSetting();
                    XmlNode operationSetting = relay.SelectSingleNode("OperationSetting");
                    cs.ComNumber = operationSetting.SelectSingleNode("ComNumber").InnerText;
                    cs.BaudRate = int.Parse(operationSetting.SelectSingleNode("BaudRate").InnerText);
                    cs.DataBits = int.Parse(operationSetting.SelectSingleNode("DataBit").InnerText);
                    cs.StopBits = int.Parse(operationSetting.SelectSingleNode("StopBit").InnerText);
                    cs.Parity = (Parity)Enum.Parse(typeof(Parity),operationSetting.SelectSingleNode("Parity").InnerText);

                    UserRelayArray _userRelayModule = new UserRelayArray(name,cs,_totoalApproachCount);
                    XmlNode _relaydata = relay.SelectSingleNode("RelayData");
                    XmlNodeList _relayapproachs = _relaydata.SelectNodes("Approach");
                    
                    foreach (XmlNode _relayapproach in _relayapproachs)
                    {
                        int id = int.Parse(_relayapproach.Attributes["Id"].Value);
                        UserRelaySetting _userRelaySetting = new UserRelaySetting(id, _totoalApproachCount);
                        XmlNodeList temps = _relayapproach.SelectNodes("Mode");
                        RelayOperationDataList _relayOperationList = new RelayOperationDataList();
                        foreach (XmlNode temp in temps)
                        {
                            string _relayOperationType = temp.Attributes["Name"].Value;
                            string _data = temp.Attributes["Data"].Value;
                            _relayOperationList.SetOperationData((RelayOperationType)Enum.Parse(typeof(RelayOperationType),_relayOperationType),_data);
                        }
                        _userRelaySetting.AddRelayOperationData(_relayOperationList);
                        _userRelayModule.AddRelayData(_userRelaySetting);
                    }
                    _relays.Add(_userRelayModule);
                }
                _relayModulelist.RefreshRelayList();
                
            }
            catch
            {
                Helper.ShowMessageBox("提示","未找到继电器配置文件!");
            }
        }

        //加载时间轴数据
        private void LoadTimeLineConfig()
        {
            try
            {
                XmlDocument config = new XmlDocument();
                config.Load(_timelineConfig);

                XmlNode root = config.SelectSingleNode("Root");
                XmlNodeList areas = root.SelectNodes("DelayTime");

                _orders.Clear();

                foreach (XmlNode area in areas)
                {
                    string areaName = area.SelectSingleNode("TimeValue").InnerText;

                    UserOrder order = new UserOrder(0, 0);
                    order.SetValue(areaName);

                    _orders.Add(order);

                    XmlNodeList operations = area.SelectNodes("Operation");
                    foreach (XmlNode operation in operations)
                    {
                        string operationName = operation.SelectSingleNode("OperationName").InnerText;
                        string operationTypeString = operation.SelectSingleNode("OperationType").InnerText;
                        CommunicationType operationType = (CommunicationType)Enum.Parse(typeof(CommunicationType), operationTypeString, true);

                        XmlNode operationSetting = operation.SelectSingleNode("OperationSetting");
                        object setting = null;
                        if (operationType == CommunicationType.Com)
                        {
                            ComSetting cs = new ComSetting();
                            cs.ComNumber = operationSetting.SelectSingleNode("ComNumber").InnerText;
                            cs.BaudRate = int.Parse(operationSetting.SelectSingleNode("BaudRate").InnerText);
                            cs.DataBits = int.Parse(operationSetting.SelectSingleNode("DataBit").InnerText);
                            cs.StopBits = int.Parse(operationSetting.SelectSingleNode("StopBit").InnerText);
                            cs.Parity = (Parity)Enum.Parse(typeof(Parity), operationSetting.SelectSingleNode("Parity").InnerText);

                            setting = cs;
                        }
                        else if (operationType == CommunicationType.TCP || operationType == CommunicationType.UDP)
                        {
                            NetworkSetting ns = new NetworkSetting();
                            ns.Ip = operationSetting.SelectSingleNode("IP").InnerText;
                            ns.Port = int.Parse(operationSetting.SelectSingleNode("Port").InnerText);
                            setting = ns;
                        }

                        string dataTypeString = operation.SelectSingleNode("OperationDataType").InnerText;
                        DataType dataType = (DataType)Enum.Parse(typeof(DataType), dataTypeString, true);
                        string data = operation.SelectSingleNode("OperationData").InnerText;
                        int time = int.Parse(operation.SelectSingleNode("OperationTime").InnerText);

                        UserOperation userOperation = new UserOperation(operationName, operationType, dataType, setting, data, time);
                        order.Operations.Add(userOperation);
                    }
                }
                _timeList.RefreshTime();
            }
            catch (Exception ex)
            {
                _orders.Clear();
            }
        }

        //加载自定义配置数据
        private void LoadUserDefinedData()
        {
            try
            {
                XmlDocument config = new XmlDocument();
                config.Load(_userdefinedConfigFile);
                XmlNode root = config.SelectSingleNode("Root");
                XmlNodeList actions = root.SelectNodes("Action");
                _userdefineNames.Clear();
                foreach (XmlNode action in actions)
                {
                    string _actionName = action.Attributes["Name"].Value;
                    DefinedName _deName = new DefinedName(_actionName);
                    XmlNodeList operations = action.SelectNodes("Operation");
                    foreach (XmlNode operation in operations)
                    {
                        string operationName = operation.SelectSingleNode("OperationName").InnerText;
                        string operationTypeString = operation.SelectSingleNode("OperationType").InnerText;
                        CommunicationType operationType = (CommunicationType)Enum.Parse(typeof(CommunicationType), operationTypeString, true);
                        XmlNode operationSetting = operation.SelectSingleNode("OperationSetting");
                        object setting = null;
                        if (operationType == CommunicationType.Com)
                        {
                            ComSetting cs = new ComSetting();
                            cs.ComNumber = operationSetting.SelectSingleNode("ComNumber").InnerText;
                            cs.BaudRate = int.Parse(operationSetting.SelectSingleNode("BaudRate").InnerText);
                            cs.DataBits = int.Parse(operationSetting.SelectSingleNode("DataBit").InnerText);
                            cs.StopBits = int.Parse(operationSetting.SelectSingleNode("StopBit").InnerText);
                            cs.Parity = (Parity)Enum.Parse(typeof(Parity), operationSetting.SelectSingleNode("Parity").InnerText);

                            setting = cs;

                        }
                        else if (operationType == CommunicationType.TCP || operationType == CommunicationType.UDP)
                        {
                            NetworkSetting ns = new NetworkSetting();
                            ns.Ip = operationSetting.SelectSingleNode("IP").InnerText;
                            ns.Port = int.Parse(operationSetting.SelectSingleNode("Port").InnerText);
                            setting = ns;
                        }

                        string dataTypeString = operation.SelectSingleNode("OperationDataType").InnerText;
                        DataType dataType = (DataType)Enum.Parse(typeof(DataType), dataTypeString, true);
                        string data = operation.SelectSingleNode("OperationData").InnerText;
                        int time = int.Parse(operation.SelectSingleNode("OperationTime").InnerText);

                        UserOperation userOperation = new UserOperation(operationName, operationType, dataType, setting, data, time);
                        _deName.AddOperation(userOperation);     
                    }
                    _userdefineNames.Add(_deName);
                }
                _userdefinedNamelist.RefreshAreaList();
            }
            catch
            {
                Helper.ShowMessageBox("异常","未找到数据！");
 
            }
 
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
                catch (Exception ex)
                {
                    Helper.ShowMessageBox("下载失败", "下载失败，可能网络连接不畅或服务器上没有配置！");
                }
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            SaveConfig();

            Start();
            Connect();
            if (_client.Connected)
            {
                NetworkStream ns = _client.GetStream();
                string uploadCommand = "SendData";
                Byte[] sendBytes = Encoding.UTF8.GetBytes(uploadCommand);
                ns.Write(sendBytes, 0, sendBytes.Length);

                try
                {
                    if (Helper.ShowMessageBox("操作确认", "确定上传配置，并更新服务器吗？", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                                    == System.Windows.Forms.DialogResult.OK)
                    {
                        if (_client.Connected)
                        {
                            FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "config.xml", FileMode.Open);
                            int size = 0;
                            byte[] buffer = new byte[_blockLength];
                            while ((size = fs.Read(buffer, 0, _blockLength)) > 0)
                            {
                                ns.Write(buffer, 0, size);
                            }
                            fs.Flush();
                            fs.Close();
                            ns.Close();

                            Helper.ShowMessageBox("成功", "上传成功!");
                        }
                        else
                        {
                            Helper.ShowMessageBox("连接异常", "连接异常，无法下载");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Stop();
                    Helper.ShowMessageBox("下载失败", "下载失败，请重试！");
                }
            }
            else
            {
                Helper.ShowMessageBox("提示","未连接服务器！");
            }
            
        }

        private void UploadConfig(IAsyncResult ar)
        {
            StateObject receiveData = (StateObject)ar.AsyncState;

            int numberOfReadBytes = 0;
            try
            {
                numberOfReadBytes = _client.Client.EndReceive(ar);
            }
            catch
            {
                numberOfReadBytes = 0;
            }

            if (System.Text.Encoding.ASCII.GetString(receiveData.buffer, 0, numberOfReadBytes) == "ok")
            {
                FileStream fs = new FileStream(_configFile, FileMode.Open);
                try
                {
                    NetworkStream ns = receiveData.stream;

                    byte[] data = new byte[_blockLength];

                    long fileLength = new FileInfo(_configFile).Length;
                    int readLength = 0;
                    while ((readLength = fs.Read(data, 0, _blockLength)) > 0)
                    {
                        ns.Write(data, 0, readLength);
                    }
                }
                finally
                {
                    fs.Close();
                }
            }
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
                Utility.Data.GetInstance().SaveIP(_hostname);
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
            SavePrjData();
            SaveUserDefinedData();
            SaveRelayConfig();
        }

        private void btnSaveSwitchConfig_Click(object sender, EventArgs e)
        {
            if (Helper.ShowMessageBox("操作确认", "确定保存？", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.OK)
            {
                SaveRelayConfig();
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            SaveTimeLine();

            Start();
            Connect();

            if (_client.Connected)
            {
                NetworkStream ns = _client.GetStream();
                string uploadCommand = "SendTimeLineData";
                Byte[] sendBytes = Encoding.UTF8.GetBytes(uploadCommand);
                ns.Write(sendBytes, 0, sendBytes.Length);

                try
                {
                    if (Helper.ShowMessageBox("操作确认", "确定上传配置，并更新服务器吗？", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                                    == System.Windows.Forms.DialogResult.OK)
                    {
                        if (_client.Connected)
                        {
                            FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "timeline.xml", FileMode.Open);
                            int size = 0;
                            byte[] buffer = new byte[_blockLength];
                            while ((size = fs.Read(buffer, 0, _blockLength)) > 0)
                            {
                                ns.Write(buffer, 0, size);
                            }
                            fs.Flush();
                            fs.Close();
                            ns.Close();

                            Helper.ShowMessageBox("成功", "上传成功!");
                        }
                        else
                        {
                            Helper.ShowMessageBox("连接异常", "连接异常，无法下载");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Stop();
                    Helper.ShowMessageBox("下载失败", "下载失败，请重试！");
                }
            }
            else
            {
                Helper.ShowMessageBox("提示","未连接服务器！");
            }
        }

        private void btnCheckTime_Click(object sender, EventArgs e)
        {
            SaveTimeLine();

            Start();
            Connect();

            if (_client.Connected)
            {
                NetworkStream ns = _client.GetStream();
                string command = "GetTime";

                Byte[] sendBytes = Encoding.UTF8.GetBytes(command);
                ns.Write(sendBytes, 0, sendBytes.Length);

                byte[] buffer = new byte[_blockLength];
                int readLehgth = ns.Read(buffer, 0, _blockLength);
                string currentTime = System.Text.Encoding.Default.GetString(buffer, 0, readLehgth);

                Helper.ShowMessageBox("当前服务器时间", currentTime);
            }
            else
            {
                Helper.ShowMessageBox("提示", "未连接服务器！");
            }
        }

        private void btnSetTime_Click(object sender, EventArgs e)
        {
            try
            {
                Start();
                Connect();

                if (_client.Connected)
                {
                    NetworkStream ns = _client.GetStream();
                    string command = "SetTime";

                    Byte[] sendBytes = Encoding.UTF8.GetBytes(command);
                    ns.Write(sendBytes, 0, sendBytes.Length);

                    Configuring.UI.Controls.SetTime setTime = new Controls.SetTime();
                    if (setTime.ShowDialog() == DialogResult.OK)
                    {
                        string time = setTime.Hour.ToString() + ":" + setTime.Minute.ToString();
                        sendBytes = Encoding.UTF8.GetBytes(time);
                        ns.Write(sendBytes, 0, sendBytes.Length);

                        byte[] receiveBuffer = new byte[_blockLength];
                        int readLength = ns.Read(receiveBuffer, 0, _blockLength);

                        if (System.Text.Encoding.Default.GetString(receiveBuffer, 0, readLength) == "sucess")
                        {
                            Helper.ShowMessageBox("提示", "设置时间成功！");
                        }
                        else
                        {
                            Helper.ShowMessageBox("提示", "设置时间失败！");
                        }
                    }

                    ns.Close();
                }
            }
            catch(Exception ex)
            {
                Helper.ShowMessageBox("错误", ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Start();
                Connect();

                if (_client.Connected)
                {
                    NetworkStream ns = _client.GetStream();
                    string command = "CancelTimeLine";

                    Byte[] sendBytes = Encoding.UTF8.GetBytes(command);
                    ns.Write(sendBytes, 0, sendBytes.Length);

                    byte[] receiveBuffer = new byte[_blockLength];
                    int readLength = ns.Read(receiveBuffer, 0, _blockLength);

                    if (System.Text.Encoding.Default.GetString(receiveBuffer, 0, readLength) == "sucess")
                    {
                        Helper.ShowMessageBox("提示", "取消预约成功！");
                    }
                    else
                    {
                        Helper.ShowMessageBox("提示", "取消预约失败！");
                    }

                    ns.Close();
                }
            }
            catch (Exception ex)
            {
                Helper.ShowMessageBox("错误", ex.Message);
            }
        }

        private void btnGetLockDogState_Click(object sender, EventArgs e)
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

                   // lbLockDogState.Text = System.Text.Encoding.UTF8.GetString(receiveBuffer, 0, readLength);

                    //if (lbLockDogState.Text.StartsWith("加密锁已过期！"))
                    //{
                    //    PanelActiveCode.Visible = true;
                    //}
                    ns.Close();
                }
            }
            catch (Exception ex)
            {
                Helper.ShowMessageBox("错误", ex.Message);
            }
        }

        //private void btnActivate_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (tbSerialNumber.Text == "")
        //        {
        //            MessageBox.Show("请输入激活码！");
        //        }
        //        else
        //        {
        //            Start();
        //            Connect();

        //            if (_client.Connected)
        //            {
        //                NetworkStream ns = _client.GetStream();
        //                string command = "Activate";

        //                Byte[] sendBytes = Encoding.Default.GetBytes(command);
        //                ns.Write(sendBytes, 0, sendBytes.Length);
        //                Thread.Sleep(100);
        //                sendBytes = Encoding.Default.GetBytes(tbSerialNumber.Text);
        //                ns.Write(sendBytes, 0, sendBytes.Length);

        //                byte[] receiveBuffer = new byte[_blockLength];
        //                int readLength = ns.Read(receiveBuffer, 0, _blockLength);

        //                lbLockDogState.Text = System.Text.Encoding.UTF8.GetString(receiveBuffer, 0, readLength);
        //                ns.Close();
        //            }
        //        }
        //    }
        //    catch
        //    {
                
        //    }
        //}

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
  

    }

    public class StateObject
    {
        public NetworkStream stream = null;
        // Size of receive buffer.
        public const int BufferSize = 1024;
        public byte[] buffer = new byte[BufferSize];
    }
}
