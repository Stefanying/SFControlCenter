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
namespace Configuring.Server
{
    //自定义命令配置服务包括（加载、保存）
   public class CustomConfigServer
    {

       public static CustomConfigServer GetInstance()
       {
           if (_instance == null)
               _instance = new CustomConfigServer();
           return _instance;
       }

       //保存自定义配置
       public void SaveUserDefinedData(List<UserDefinedOperation> p_userdefineNames)
       {
           if (p_userdefineNames != null && p_userdefineNames.Count > 0)
           {
               XmlDocument config = new XmlDocument();
               XmlNode root = config.CreateNode(XmlNodeType.Element, "Root", null);
               config.AppendChild(root);
               for (int i = 0; i < p_userdefineNames.Count; i++)
               {
                   UserDefinedOperation _currentName = p_userdefineNames[i];
                   XmlNode action = config.CreateNode(XmlNodeType.Element, "Action", null);
                   XmlAttribute _name = config.CreateAttribute("Name");
                   _name.Value = _currentName.Name;
                   action.Attributes.Append(_name);
                   for (int _operationcout = 0; _operationcout < _currentName.Operations.Count; _operationcout++)
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
 
       }

       public List<UserDefinedOperation> LoadUserDefinedData()
       {
           try
           {
               #region LoadUserDefinedData
               XmlDocument config = new XmlDocument();
               config.Load(_userdefinedConfigFile);
               XmlNode root = config.SelectSingleNode("Root");
               XmlNodeList actions = root.SelectNodes("Action");
               _userdefineNames.Clear();
               foreach (XmlNode action in actions)
               {
                   string _actionName = action.Attributes["Name"].Value;
                   UserDefinedOperation _deName = new UserDefinedOperation(_actionName);
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
               return _userdefineNames;
               #endregion

           }
           catch (Exception ex)
           {
               Helper.ShowMessageBox("提示","未找到自定义配置文件");
               return null;
           }
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

       List<UserDefinedOperation> _userdefineNames = new List<UserDefinedOperation>();
       static CustomConfigServer _instance;
       string _userdefinedConfigFile = AppDomain.CurrentDomain.BaseDirectory + "CustomData.xml";
    }
}
