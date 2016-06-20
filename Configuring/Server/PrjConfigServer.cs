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
    //常用投影机配置服务包括（加载、保存）
   public class PrjConfigServer
    {

       public static PrjConfigServer GetInstance()
       {
           if (_instance == null)
               _instance = new PrjConfigServer();
           return _instance;
       }

       //保存投影配置
       public void SavePrjConfig(List<UserPrjSetting> p_prjSetting)
       {
           if (p_prjSetting != null && p_prjSetting.Count > 0)
           {
               XmlDocument config = new XmlDocument();
               XmlNode root = config.CreateNode(XmlNodeType.Element, "ProjectorData", null);
               config.AppendChild(root);
               #region PrjData
               if (p_prjSetting.Count > 0 && p_prjSetting != null)
               {
                   for (int i = 0; i < p_prjSetting.Count; i++)
                   {
                       UserPrjSetting currentPrjSetting = p_prjSetting[i];
                       XmlNode deviceName = config.CreateNode(XmlNodeType.Element, currentPrjSetting.Name, null);

                       for (int cout_mode = 0; cout_mode < currentPrjSetting.DeviceStates.Count; cout_mode++)
                       {
                           UserPrjOperation tempprjState = currentPrjSetting.DeviceStates[cout_mode];
                           XmlElement mode = config.CreateElement("Mode");
                           XmlAttribute _name = config.CreateAttribute("Name");
                           _name.Value = tempprjState.PrjOperationType.ToString();
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
               }

               #endregion
               config.Save(_prjConfigFile);
           }
 
       }

       public List<UserPrjSetting> LoadPrjData()
       {
           try
           {
               XmlDocument prjConfig = new XmlDocument();
               prjConfig.Load(_prjConfigFile);
               XmlNode prjroot = prjConfig.SelectSingleNode("ProjectorData");
               _prjSettings.Clear();
               foreach (XmlNode prjector in prjroot)
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
                   UserPrjSetting _ups = new UserPrjSetting(_prjName, pcs);
                   XmlNodeList _tempprjStates = prjector.SelectNodes("Mode");
                   _prjSettings.Add(_ups);
                   foreach (XmlNode _prjState in _tempprjStates)
                   {
                       string _statename = _prjState.Attributes["Name"].Value;
                       string _statedata = _prjState.Attributes["Data"].Value;
                       PrjOperationType _mode = (PrjOperationType)Enum.Parse(typeof(PrjOperationType), _statename);
                       UserPrjOperation uds = new UserPrjOperation(_mode, _statedata);
                       _ups.DeviceStates.Add(uds);
                   }
               }
               return _prjSettings;
           }
           catch
           {
               Helper.ShowMessageBox("提示","未找到投影机配置文件!");
               return null;
           }
       }

       string _prjConfigFile = AppDomain.CurrentDomain.BaseDirectory + "PrjData.xml";
       static PrjConfigServer _instance;
       List<UserPrjSetting> _prjSettings = new List<UserPrjSetting>();
    }
}
