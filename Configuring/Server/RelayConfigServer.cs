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
    //继电器配置服务包括（加载、保存）
   public class RelayConfigServer
    {
       public static  RelayConfigServer  GetInstance()
       {
           if(_instance ==null)
               _instance = new RelayConfigServer();
           return _instance;
       }

       //加载继电器配置文件
       public List<UserRelayArray> LoadRelayConfig()
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
                   cs.Parity = (Parity)Enum.Parse(typeof(Parity), operationSetting.SelectSingleNode("Parity").InnerText);

                   UserRelayArray _userRelayModule = new UserRelayArray(name, cs, _totoalApproachCount);
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
                           _relayOperationList.SetOperationData((RelayOperationType)Enum.Parse(typeof(RelayOperationType), _relayOperationType), _data);
                       }
                       _userRelaySetting.AddRelayOperationData(_relayOperationList);
                       _userRelayModule.AddRelayData(_userRelaySetting);
                   }
                   _relays.Add(_userRelayModule);
               }
               return _relays;
           }
           catch
           {
               Helper.ShowMessageBox("提示","未找到继电器配置文件");
               return null;
           }
       }

       //保存继电器配置文件
       public void SaveRelayConfig(List<UserRelayArray> p_relays)
       {
           if (p_relays != null && p_relays.Count > 0)
           {
               #region RelayConfig
               try
               {
                   XmlDocument config = new XmlDocument();
                   XmlNode root = config.CreateNode(XmlNodeType.Element, "Root", null);
                   config.AppendChild(root);
                   for (int i = 0; i < p_relays.Count; i++)
                   {
                       UserRelayArray _currentRelay = p_relays[i];
                       XmlNode relayModoule = config.CreateNode(XmlNodeType.Element, "Relay", null);
                       XmlNode relayModouleName = config.CreateNode(XmlNodeType.Element, "Name", null);
                       XmlNode relayCount = config.CreateNode(XmlNodeType.Element, "TotoalApproach", null);
                       relayModouleName.InnerText = _currentRelay.Name;
                       relayCount.InnerText = _currentRelay.ApproachCout.ToString();
                       relayModoule.AppendChild(relayModouleName);
                       relayModoule.AppendChild(relayCount);
                       XmlNode operationset = config.CreateNode(XmlNodeType.Element, "OperationSetting", null);

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
                           XmlNode approach = config.CreateNode(XmlNodeType.Element, "Approach", null);
                           XmlAttribute id = config.CreateAttribute("Id");
                           id.Value = _currentRelaySet.RelayId.ToString();
                           approach.Attributes.Append(id);
                           for (int _relayOperationData = 0; _relayOperationData < 2; _relayOperationData++)
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
               catch (Exception ex)
               {
                   Helper.ShowMessageBox("异常", ex.Message);
               }
           }
           #endregion
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
       List<UserRelayArray> _relays = new List<UserRelayArray>();
       static RelayConfigServer _instance;
       string _relayConfigFile = AppDomain.CurrentDomain.BaseDirectory + "RelayData.xml";
    }
}
