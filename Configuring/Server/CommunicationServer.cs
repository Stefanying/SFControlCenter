using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using SFLib;
using Configuring.UI;
using Configuring.Utility;
using System.Windows.Forms;
using Configuring.UI.Controls;
namespace Configuring.Server
{
   public class CommunicationServer
   {

       #region NetSet
       private void Start()
       {
           _client = new TcpClient();
           _client.ReceiveTimeout = 1000 * 10;
       }

       private void Connect(string p_ip)
       {
           try
           {
               _hostname = p_ip;
               _client.Connect(IPAddress.Parse(_hostname), _port);
               Utility.ConfigData.GetInstance().SaveIP(_hostname);
           }
           catch(Exception ex)
           {
               Helper.ShowMessageBox("连接失败",ex.Message);
           }
       }

       private void Stop()
       {
           _client.Close();
       }
       #endregion

       //上传配置
       public void UploadConfig(string _ip)
       {
           Start();
           Connect(_ip);
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
                           Helper.ShowMessageBox("连接异常","连接异常，无法上传!");
                       }
 
                   }
               }
               catch
               {
                   Stop();
                   Helper.ShowMessageBox("下载失败", "下载失败，请重试！");
               }
           }
           else
           {
               Helper.ShowMessageBox("提示", "未连接服务器！");
           }
 
       }

       //上传预约配置
       public void UploadOrderConfig(string _ip)
       {
           Start();
           Connect(_ip);
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
                           Helper.ShowMessageBox("连接异常", "连接异常，无法上传!");
                       }

                   }
               }
               catch
               {
                   Stop();
                   Helper.ShowMessageBox("下载失败", "下载失败，请重试！");
               }
           }
           else
           {
               Helper.ShowMessageBox("提示", "未连接服务器！");
           }
 
       }

       //获取服务器时间
       public void GetServerTime(string _ip)
       {
           Start();
           Connect(_ip);
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

       //设置服务器时间
       public void SetServerTime(string _ip)
       {
           try
           {
               Start();
               Connect(_ip);

               if (_client.Connected)
               {
                   NetworkStream ns = _client.GetStream();
                   string command = "SetTime";

                   Byte[] sendBytes = Encoding.UTF8.GetBytes(command);
                   ns.Write(sendBytes, 0, sendBytes.Length);

                   Configuring.UI.Controls.SetTime setTime = new Configuring.UI.Controls.SetTime();
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
           catch (Exception ex)
           {
               Helper.ShowMessageBox("错误", ex.Message);
           }
       }

       //下载配置文件
       public void DownLoadConfig(string _ip)
       {
           Start();
           Connect(_ip);

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

                           //if (this.InvokeRequired)
                           //{
                           //    this.Invoke(new MethodInvoker(() =>
                           //    {
                           //        Helper.ShowMessageBox("提示", "下载完成", MessageBoxButtons.OK);
                           //    }));
                           //}
                           //else
                           //{
                           //    Helper.ShowMessageBox("提示", "下载完成", MessageBoxButtons.OK);
                           //}

                           if (ConfigDownLoadDone != null)
                           {
                               ConfigDownLoadDone(this,null);
                           }
 
                       }
                   }
               }
               catch
               {
                   Helper.ShowMessageBox("下载失败", "下载失败，可能网络连接不畅或服务器上没有配置！");
               }
           }
 
       }

       public void StopOrder(string _ip)
       {
           try
           {
               Start();
               Connect(_ip);
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
               Helper.ShowMessageBox("错误",ex.Message);
           }
       }

       public EventHandler ConfigDownLoadDone;
       TcpClient _client;
       string _hostname;
       int _port = 10003;
       int _blockLength = 1024;
    }
}
