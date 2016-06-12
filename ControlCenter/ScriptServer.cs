using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Threading;
using System.ComponentModel;
using SFLib;
using System.Net.Sockets;
using System.IO.Ports;

namespace ControlCenter
{
    //返回执行结果的枚举
    public enum CommandResult
    {
        [Description("成功")]
        Success = 1,

        [Description("Url太长")]
        UrlTooLong = 2,

        [Description("Url包含特殊字符")]
        UrlInvalidChar = 3,

        [Description("文件名含特殊字符")]
        FileNameInvalidChar = 4,

        [Description("方法不存在")]
        NoExistsMethod = 5,

        [Description("文件不存在")]
        FileNotExists = 6,

        [Description("执行功能失败")]
        ExcuteFunctionFailed = 7,

        [Description("服务器正忙")]
        ServerIsBusy = 8,

        [Description("脚本运行时间过长保护主动断开")]
        DoFunctionTooLongTimeProtect = 9
    };

    //实现HttpServer要支持的接口
    interface HttpImplanter
    {
        void Start();
        void Stop();
        void MakeHttpPrefix(HttpListener server);
        SFReturnCode ProcessRequest(HttpListenerContext context);
        byte[] CreateReturnResult(HttpListenerContext context, SFReturnCode result);
    }

    //可接收Http请求的服务器
    class HttpServer
    {
        Thread _httpListenThread;

        /// <summary>
        /// HttpServer是否已经启动
        /// </summary>
        volatile bool _isStarted = false;

        /// <summary>
        /// 线程是否已经结束
        /// </summary>
        volatile bool _terminated = false;
        volatile bool _ready = false;
        volatile bool _isRuning = false;
        HttpImplanter _httpImplanter;

        internal HttpImplanter HttpImplanter
        {
            get { return _httpImplanter; }
            set { _httpImplanter = value; }
        }

        public void Start(HttpImplanter httpImplanter)
        {
            if (!HttpListener.IsSupported)
            {
                Logger.Exit("不支持HttpListener!");
            }

            if (_isStarted)
            {
                return;
            }
            _isStarted = true;
            _ready = false;
            _httpImplanter = httpImplanter;

            RunHttpServerThread();
            while (!_ready) ;
        }

        private void RunHttpServerThread()
        {

            _httpListenThread = new Thread(new ThreadStart(() =>
            {
                HttpListener server = new HttpListener();
                try
                {
                    _httpImplanter.MakeHttpPrefix(server);
                    server.Start();
                }
                catch (Exception ex)
                {
                    Logger.Exit("无法启动服务器监听，请检查网络环境。");
                }

                _httpImplanter.Start();

                IAsyncResult result = null;
                while (!_terminated)
                {
                    while (result == null || result.IsCompleted)
                    {
                        result = server.BeginGetContext(new AsyncCallback(ProcessHttpRequest), server);
                    }
                    _ready = true;
                    Thread.Sleep(10);
                }

                server.Stop();
                server.Abort();
                server.Close();
                _httpImplanter.Stop();
            }
            ));

            _httpListenThread.IsBackground = true;//此代码保证，主程序退出线程也随着同时退出
            _httpListenThread.Start();
        }

        private void ProcessHttpRequest(IAsyncResult iaServer)
        {
            HttpListener server = iaServer.AsyncState as HttpListener;
            HttpListenerContext context = null;
            try
            {
                context = server.EndGetContext(iaServer);
                Logger.Info("接收请求" + context.Request.Url.ToString());

                //判断上一个操作未完成，即返回服务器正忙，并开启一个新的异步监听
                if (_isRuning)
                {
                    Logger.Info("正在处理请求，已忽略请求" + context.Request.Url.ToString());
                    RetutnResponse(context, _httpImplanter.CreateReturnResult(context, new SFReturnCode((int)CommandResult.ServerIsBusy, EnumHelper.GetEnumDescription(CommandResult.ServerIsBusy))));
                    server.BeginGetContext(new AsyncCallback(ProcessHttpRequest), server);
                    return;
                }
                _isRuning = true;
                server.BeginGetContext(new AsyncCallback(ProcessHttpRequest), server);
            }
            catch
            {
                Logger.Warning("服务器已关闭！");
                return;
            }

            string scriptName = new UrlHelper(context.Request.Url).ScriptName;
            byte[] resultBytes = null;
            SFReturnCode result = _httpImplanter.ProcessRequest(context);
            resultBytes = _httpImplanter.CreateReturnResult(context, result);
          
            RetutnResponse(context, resultBytes);
            GC.Collect();
            _isRuning = false;
        }

        private static void RetutnResponse(HttpListenerContext context, byte[] resultBytes)
        {
            context.Response.ContentLength64 = resultBytes.Length;
            System.IO.Stream output = context.Response.OutputStream;
            try
            {
                output.Write(resultBytes, 0, resultBytes.Length);
                output.Close();
            }
            catch
            {
                Logger.Warning("客户端已经关闭!");
            }
        }

        public void Stop()
        {
            if (!_isStarted)
            {
                return;
            }

            _terminated = true;
            _httpListenThread.Join();

            _isStarted = false;
        }

    }

    class ScriptServer : HttpImplanter
    {
        ScriptEngineer _scriptEngineer = new ScriptEngineer();
        public ScriptEngineer ScriptEngineer
        {
            get { return _scriptEngineer; }
            set { _scriptEngineer = value; }
        }

        string _projectName;

        public ScriptServer(string projectName)
        {
            _projectName = projectName;
        }

        public void Start()
        {
            _scriptEngineer.Start(_projectName);
        }

        public void MakeHttpPrefix(HttpListener server)
        {
            server.Prefixes.Clear();
            server.Prefixes.Add("http://*:80/");
        }

        public SFReturnCode ProcessRequest(HttpListenerContext context)
        {
            UrlHelper urlHelper = new UrlHelper(context.Request.Url);
            CommandResult result = urlHelper.ParseResult;
            if (urlHelper.ParseResult == CommandResult.Success)
            {
                try
                {
                    _scriptEngineer.ExecuteScript(urlHelper.ScriptName, urlHelper.Parameters);
                    return new SFReturnCode((int)CommandResult.Success, EnumHelper.GetEnumDescription(CommandResult.Success));
                }
                catch (FileNotFoundException fileNotFoundException)
                {
                    return new SFReturnCode((int)CommandResult.NoExistsMethod, EnumHelper.GetEnumDescription(CommandResult.NoExistsMethod));
                }
                catch (TimeoutException timeoutException)
                {
                    return new SFReturnCode((int)CommandResult.DoFunctionTooLongTimeProtect, EnumHelper.GetEnumDescription(CommandResult.DoFunctionTooLongTimeProtect));
                }
                catch (SFReturnCode returnCode)
                {
                    return returnCode;
                }
                catch (Exception ex)
                {
                    return new SFReturnCode((int)CommandResult.ExcuteFunctionFailed, EnumHelper.GetEnumDescription(CommandResult.ExcuteFunctionFailed));
                }
            }
            return new SFReturnCode((int)result, EnumHelper.GetEnumDescription(result));
        }

        public byte[] CreateReturnResult(HttpListenerContext context, SFReturnCode result)
        {
            string responseString = string.Format("msg={0}",
                result.Message
                );

            return System.Text.Encoding.UTF8.GetBytes(responseString);
        }

        public void Stop()
        {
            _scriptEngineer.Stop();
        }

    }

    #region Listener
    abstract class Listener
    {
        ScriptEngineer _scriptEngineer = new ScriptEngineer();
        public abstract void Send(string data);

        public void Start(string projectName)
        {
            _scriptEngineer.Start(projectName);
        }

        protected bool FireRecv(string cmd)
        {
            Logger.Info("接收请求" + cmd + ".lua");
            Logger.Info("查找脚本" + cmd.Trim() + ".lua");
            if(cmd != null )  _scriptEngineer.ExecuteScript(cmd, null);

            return true;
        }
        protected void ProcessPack(string received_data)
        {
            try
            {
                __pack_buf += received_data;
                bool fired = false;

                fired = FireRecv(__pack_buf);
                __pack_buf = "";
            }
            catch (Exception e)
            {
                __pack_buf = "";
                Logger.Exception(e.Message);
            }
        }

        public void Stop()
        {
            _scriptEngineer.Stop();
        }

        protected string __pack_buf;
    }

    #region TCPListener
    class TcpScriptListener : Listener
    {
        class StateObject
        {
            // Client  socket.
            public TcpClient client = null;
            // Size of receive buffer.
            public const int BufferSize = 1024;
            // Receive buffer.
            public byte[] buffer = new byte[BufferSize];
        }

        public TcpScriptListener(int port)
        {
            _port = port;
            _listenr = new TcpListener(port);

            _thread = new Thread(new ThreadStart(ThreadFunc));
            _thread.Start();
            _thread.IsBackground = true;

            _sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }

        public override void Send(string data)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            Byte[] bytes = encoding.GetBytes(data);
            if (_sock != null && _ip_from != null)
            {
                _sock.SendTo(bytes, _ip_from);
            }
        }
        protected void ThreadFunc()
        {
            try
            {
                _ip_from = new IPEndPoint(IPAddress.Any, _port);
                _listenr.Start();

                byte[] data = new byte[1024];
                while (!_terminated)
                {
                    TcpClient aceeptClient =  _listenr.AcceptTcpClient();

                    StateObject recieveData = new StateObject();
                    recieveData.client = aceeptClient;
                    aceeptClient.GetStream().BeginRead(recieveData.buffer, 0, recieveData.buffer.Length, ProcessCommand, recieveData);

                    Thread.Sleep(10);
                }
                _listenr.Stop();
            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine(e.ToString());
#endif
            }
            finally
            {
               // _listener.Close();
            }
        }

        private void ProcessCommand(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            TcpClient client = state.client;

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
                        string command = System.Text.Encoding.Default.GetString(state.buffer, 0, numberOfReadBytes);
                        FireRecv(command);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Exception(ex.Message);
            }

            StateObject receiveData = new StateObject();
            receiveData.client = client;
            client.GetStream().BeginRead(receiveData.buffer, 0, receiveData.buffer.Length, ProcessCommand, receiveData);
        }


        // member
        int _port = -1;
        TcpListener _listenr;
        Thread _thread = null;
        volatile bool _terminated = false;
        Socket _sock = null;
        IPEndPoint _ip_from = null;
    }
    #endregion

    #region UDPListener
    class UDPListener : Listener
    {
        public UDPListener(int port)
        {
            _port = port;
            _listener = new UdpClient(port);
            _thread = new Thread(new ThreadStart(ThreadFunc));
            _thread.IsBackground = true;
            _thread.Start();

            _sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }
        public override void Send(string data)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            Byte[] bytes = encoding.GetBytes(data);
            if (_sock != null && _ip_from != null)
            {
                _sock.SendTo(bytes, _ip_from);
            }
        }
        protected void ThreadFunc()
        {
            try
            {
                _ip_from = new IPEndPoint(IPAddress.Any, _port);

                byte[] data = new byte[1024];
                string received_data;

                while (true)
                {
                    // Note that this is a synchronous or blocking call.
                    data = _listener.Receive(ref _ip_from);
                    received_data = Encoding.ASCII.GetString(data, 0, data.Length);
                    ProcessPack(received_data);
                }
            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine(e.ToString());
#endif
            }
            finally
            {
                _listener.Close();
            }
        }

        // member
        int _port = -1;
        UdpClient _listener = null;
        Thread _thread = null;
        Socket _sock = null;
        IPEndPoint _ip_from = null;
    }
    #endregion

    #region COMListener
    class COMListener : Listener
    {
        public COMListener(string port_name, string new_line)
        {

            try
            {
                _com = new SerialPort();
                _com.PortName = port_name;
                _com.BaudRate = 9600;
                _com.DataBits = 8;
                _com.StopBits = StopBits.One;
                _com.NewLine = new_line;
                _com.ReadTimeout = 200;
                _com.RtsEnable = true;

                _com.Open();
                _com.DataReceived += new SerialDataReceivedEventHandler(OnRecv);
            }
            catch (Exception ex)
            {
                _com = null;
                Logger.Info(ex.Message);
            }
        }
        public override void Send(string data)
        {
            _com.WriteLine(data);
        }
        void OnRecv(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort com = (SerialPort)sender;
            string received_data = com.ReadLine();
            ProcessPack(received_data);
        }

        SerialPort _com = null;
    }
    #endregion
    #endregion
}
