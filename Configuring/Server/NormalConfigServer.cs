using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Configuring.Business;
using Configuring.UI;
using System.Xml;
using System.IO;
using System.Net;
using System.Net.Sockets;
using SFLib;
namespace Configuring.Server
{
    //基本配置的服务包括（保存，加载）
    //配置命令、时间轴和预约
   public class NormalConfigServer
    {
       public static NormalConfigServer GetInstance()
       {
           if (_instance == null)
               _instance = new NormalConfigServer();
           return _instance;
       }

       //加载配置
       public List<Area> LoadConfig()
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
               return _areas;
               #endregion
           }
           catch(Exception ex)
           {
               Helper.ShowMessageBox("提示","未找到配置文件!");
               return null;
           }
 
       }

       //加载时间轴配置
       public List<UserAction> LoadTimeShaft()
       {
           try
           {
               XmlDocument config = new XmlDocument();
               config.Load(_configFile);

               XmlNode root = config.SelectSingleNode("Root");

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
               return _shaft_actions;
               #endregion
           }
           catch (Exception ex)
           {
               Helper.ShowMessageBox("提示","未找到配置命令!");
               return null;
           }
 
       }

       //加载预约配置
       public List<UserOrder> LoadOrderConfig()
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
               return _orders;
           }
           catch(Exception ex)
           {
               Helper.ShowMessageBox("提示","未找到配置文件!");
               return null;
           }
       }
       //保存配置(包含时间轴配置)
       public void SaveConfig(List<Area> p_areas,List<UserAction> p_shaft_actions)
       {
           XmlDocument config = new XmlDocument();

           XmlNode root = config.CreateNode(XmlNodeType.Element, "Root", null);
           config.AppendChild(root);

           #region NormalCommands
           for (int i = 0; i < p_areas.Count; i++)
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

           for (int i = 0; i < p_shaft_actions.Count; i++)
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
       //保存预约配置
       public void SaveOrderConfig(List<UserOrder> _orders)
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

       List<Area> _areas = new List<Area>();
       List<UserOrder> _orders = new List<UserOrder>();
       List<UserAction> _shaft_actions = new List<UserAction>();
       static NormalConfigServer _instance;
       string _configFile = AppDomain.CurrentDomain.BaseDirectory + "config.xml";
       string _timelineConfig = AppDomain.CurrentDomain.BaseDirectory + "timeline.xml";
    }
}
