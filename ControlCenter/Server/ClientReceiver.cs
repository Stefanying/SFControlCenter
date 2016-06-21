using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Diagnostics;
using ControlCenter.Utility;
namespace ControlCenter.Server
{
    public class StateObject
    {
        // Client  socket.
        public TcpClient client = null;
        // Size of receive buffer.
        public const int BufferSize = 1024;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
    }

    class ClientReceiver
    {
        public event EventHandler OnConfigUpdated;
        public event EventHandler OnTimeLineUpdated;

        int _port = 10003;
        System.Net.Sockets.TcpListener _listener;
        TcpClient _connector;
        NetworkStream _dataStream;
        string _configPath = AppDomain.CurrentDomain.BaseDirectory + "config.xml";
        string _timelineConfig = AppDomain.CurrentDomain.BaseDirectory + "timeline.xml";
        int _blockLength = 1024;

        EfeeLockDog _dog;
        public EfeeLockDog Dog
        {
            get { return _dog; }
            set { _dog = value; }
        }

        public void Start()
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, _port);
            
            _listener = new System.Net.Sockets.TcpListener(ip);
            _listener.Start();
            _listener.BeginAcceptTcpClient(AcceptCallback, _listener);
        }

        public void Stop()
        {
            _listener.Stop();
            if (_connector != null)
            {
                if (_connector.Connected) _connector.Close();
                _connector = null;
            }
        }

        private void AcceptCallback(IAsyncResult iar)
        {
            try
            {
                System.Net.Sockets.TcpListener listener = (System.Net.Sockets.TcpListener)iar.AsyncState;
                _connector = listener.EndAcceptTcpClient(iar);
                _dataStream = _connector.GetStream();

                StateObject receveieData = new StateObject();
                receveieData.client = _connector;

                _dataStream.BeginRead(receveieData.buffer, 0, receveieData.buffer.Length, AcceptData, receveieData);
                _listener.BeginAcceptTcpClient(AcceptCallback, _listener);
            }
            catch (Exception ex)
            {
                SFLib.Logger.Exception(ex.Message);
            }
        }

        //接收数据
        private void AcceptData(IAsyncResult ar)
        {
            StateObject receiveData = (StateObject)ar.AsyncState;
            TcpClient client = receiveData.client;

            if (client.Connected)
            {
                int numberOfReadBytes = 0;
                try
                {
                    numberOfReadBytes = client.Client.EndReceive(ar);
                }
                catch
                {
                    numberOfReadBytes = 0;
                }

                if (numberOfReadBytes != 0)
                {
                    string command = System.Text.Encoding.Default.GetString(receiveData.buffer, 0, numberOfReadBytes);
                    switch (command)
                    {
                        case "GetData":
                            SendFile(receiveData);
                            break;
                        case "SendData":
                            ReceiveFile( receiveData, _configPath);
                            if (OnConfigUpdated != null)
                            {
                                OnConfigUpdated(this, null);
                            }
                            break;
                        case "SetIP":
                            SetIP(receiveData);
                            break;
                        case "SendTimeLineData":
                            ReceiveFile(receiveData, _timelineConfig);
                            if (OnTimeLineUpdated != null)
                            {
                                OnTimeLineUpdated(this, null);
                            }
                            RestartThis();
                            break;
                        case "CancelTimeLine":
                            CancelTimeLine(receiveData);
                            break;
                        case "GetTime":
                            ReturnTime(receiveData);
                            break;
                        case "SetTime":
                            SetTime(receiveData);
                            break;
                        case "GetLockState":
                            ReturnLockState(receiveData);
                            break;
                        //case "Activate":
                        //    ActivateLock(receiveData);
                        //    break;
                    }
                }
            }
        }

        //重启
        private static void RestartThis()
        {
            Process.Start("Restart.bat");
        }

        //发送文件
        private void SendFile(StateObject receiveData)
        {
            FileStream fs = new FileStream(_configPath, FileMode.Open);
            int readLength = 0;
            byte[] data_block = new byte[_blockLength];

            while ((readLength = fs.Read(data_block, 0, _blockLength)) > 0)
            {
                receiveData.client.GetStream().Write(data_block, 0, readLength);
            }
            //receiveData.client.GetStream().Flush();
            fs.Close();
            receiveData.client.GetStream().Flush();
            receiveData.client.Close();
        }

        //接收文件
        private void ReceiveFile(StateObject receiveData, string filepath)
        {
            TcpClient client = receiveData.client;
            NetworkStream ns = client.GetStream();
            FileStream fs = new FileStream(filepath, FileMode.Create);
            int readLength = 0;
            byte[] data_block = new byte[_blockLength];

            while ((readLength = ns.Read(data_block, 0, _blockLength)) > 0)
            {
                fs.Write(data_block, 0, readLength);
            }
           
            fs.Close();
            receiveData.client.GetStream().Flush();
        }


        private void SetIP(StateObject receiveData)
        {
            try
            {
                byte[] data_block = new byte[_blockLength];
                int readLength = receiveData.client.GetStream().Read(data_block, 0, data_block.Length);
                string ip = System.Text.Encoding.Default.GetString(data_block, 0, readLength);

                readLength = receiveData.client.GetStream().Read(data_block, 0, data_block.Length);
                string subaddress = System.Text.Encoding.Default.GetString(data_block, 0, readLength);

                SFLib.NetLib.SetIp(ip, subaddress);
            }
            catch
            {
                SFLib.Logger.Exception("");
            }
        }

        private void ReturnTime(StateObject receiveData)
        {
            string time = SystemTime.GetTime();

            Byte[] sendBytes = Encoding.UTF8.GetBytes(time);
            receiveData.client.GetStream().Write(sendBytes, 0, sendBytes.Length);

            receiveData.client.GetStream().Flush();
            receiveData.client.Close();
        }

        private void SetTime(StateObject receiveData)
        {
            byte[] receiveBytes = new byte[_blockLength];

            StateObject data = new StateObject();
            data.client = receiveData.client;
            receiveData.client.GetStream().BeginRead(data.buffer, 0, data.buffer.Length, SetTimeCallback, data);
        }

        private void SetTimeCallback(IAsyncResult ar)
        {
            StateObject data = (StateObject)ar.AsyncState;
            TcpClient client = data.client;

            try
            {
                if (client.Connected)
                {
                    int numberOfReadBytes = 0;
                    try
                    {
                        numberOfReadBytes = client.Client.EndReceive(ar);
                    }
                    catch
                    {
                        numberOfReadBytes = 0;
                    }

                    if (numberOfReadBytes != 0)
                    {
                        string time = System.Text.Encoding.Default.GetString(data.buffer, 0, numberOfReadBytes);

                        ushort hour = ushort.Parse(time.Split(':')[0]);
                        ushort minute = ushort.Parse(time.Split(':')[1]);
                        string ret = SystemTime.SetTime(hour, minute);

                        Byte[] sendBytes = Encoding.UTF8.GetBytes(ret);
                        data.client.GetStream().Write(sendBytes, 0, sendBytes.Length);

                        data.client.GetStream().Flush();
                        data.client.Close();
                    }
                }
            }
            catch
            {
                client.Close();
            }
        }

        private void CancelTimeLine(StateObject receiveData)
        {
            try
            {
                string timelineFile = AppDomain.CurrentDomain.BaseDirectory + "Script/TimeLine";
                File.Delete(timelineFile);

                Byte[] sendBytes = Encoding.UTF8.GetBytes("sucess");
                receiveData.client.GetStream().Write(sendBytes, 0, sendBytes.Length);

                receiveData.client.GetStream().Flush();
                receiveData.client.Close();
            }
            catch
            {
                Byte[] sendBytes = Encoding.UTF8.GetBytes("fail");
                receiveData.client.GetStream().Write(sendBytes, 0, sendBytes.Length);

                receiveData.client.GetStream().Flush();
                receiveData.client.Close();
            }
        }

        private void ReturnLockState(StateObject receiveData)
        {
            try
            {
                string ret = "";

                if (!_dog.CheckDog())
                {
                    ret = "未检测到加密锁！";
                }
                //else if (!_dog.CheckPeriod())
                //{
                //    ret = "加密锁已过期！" + Environment.NewLine
                //          + "序列号:" + _dog.MakeSerialNumber().ToString();
                //}
                //else if (!_dog.CheckPassword("SF0002"))
                //{
                //    ret = "加密狗信息错误，不是本软件加密狗！";
                //}
                else
                {
                    ret = "加密锁在有效期内！";
                }

                Byte[] sendBytes = Encoding.UTF8.GetBytes(ret);
                receiveData.client.GetStream().Write(sendBytes, 0, sendBytes.Length);
                receiveData.client.GetStream().Flush();
                receiveData.client.Close();
            }
            catch
            {
                receiveData.client.GetStream().Flush();
                receiveData.client.Close();
            }
        }

    }   
}
