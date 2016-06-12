using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Threading;
using System.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace SFLib
{
    public struct Ip
    {
        public string Address;
        public string SubnetMask;
    }

    /// <summary>
    /// 网络相关的一些常用函数库
    /// </summary>
    public class NetLib
    {
        public static string GetLocalIpString()
        {
            string result = null;
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in nics)
            {
                //从有线或无线网络连接中选则第一个找到的网络适配器
                if ((adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet) || (adapter.NetworkInterfaceType == NetworkInterfaceType.Wireless80211))
                {
                    //过滤掉loopback连接
                    if (adapter.Description == "Microsoft Loopback Adapter")
                    {
                        continue;
                    }
                    //过滤掉蓝牙连接和vmware虚拟网卡连接
                    if (adapter.Name.ToUpper().Contains("BLUETOOTH") || adapter.Name.ToUpper().Contains("VMWARE"))
                    {
                        continue;
                    }
                    result = GetIpv4StringFromNetworkAdapter(result, adapter);
                }
            }
            return result;
       
        }

        private static string GetIpv4StringFromNetworkAdapter(string result, NetworkInterface adapter)
        {
            if (adapter == null) return null;
            foreach (UnicastIPAddressInformation ip in adapter.GetIPProperties().UnicastAddresses)
            {
                if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                {
                    result = ip.Address.ToString();
                }
            }
            return result;
        }

        public static string HttpRequest(string reuqestURL)
        {
            try
            {
                using (WebClient wc = new WebClient())
                {
                    wc.Encoding = Encoding.UTF8;
                    return wc.DownloadString(reuqestURL);
                }
            }
            catch (Exception ex)
            {
                return "";//表示执行失败
            }
        }
                                                                                                                                                                                                    #region setIP
        public static void SetIp(string address, string subnetMask = "255.255.255.0")
        {
            IPAddress ip;
            IPAddress sub;

            bool isIpAddress = IPAddress.TryParse(address, out ip);
            if (!isIpAddress)
                throw new Exception("ip地址格式错误! - " + address);
            isIpAddress = IPAddress.TryParse(subnetMask, out sub);
            if (!isIpAddress)
                throw new Exception("子网掩码格式错误! - " + subnetMask);

            string originalIp = GetIp().Address;
            string originalSub = GetIp().SubnetMask;
            if (ip.ToString() == originalIp && sub.ToString() == originalSub)
                return;

            if (ip.ToString() != originalIp)
            {
                if (CheckIpConflict(ip.ToString()))
                    throw new Exception("检测到IP地址冲突!");
            }

            Cmd(string.Format("%windir%\\system32\\netsh interface ip set address \"本地连接\" static {0} {1}", ip, sub));

            if (ip.ToString() != GetIp().Address)
                throw new Exception("ip设置错误!");
            if (sub.ToString() != GetIp().SubnetMask)
                throw new Exception("所输入的子网掩码无效!");
        }

        #region private functions
        static string Cmd(string cmd)
        {
            string[] cmds = new string[] { cmd };
            return Cmd(cmds);
        }
        static string Cmd(string[] cmd)
        {
            Process p = new Process();

            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.AutoFlush = true;

            for (int i = 0; i < cmd.Length; i++)
            {
                p.StandardInput.WriteLine(cmd[i].ToString());
            }

            p.StandardInput.WriteLine("exit");

            string strRst = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            p.Close();

            return strRst;
        }
        static List<Ip> GetIpListFromUnicastAddresses(UnicastIPAddressInformationCollection uncastAddresses)
        {
            List<Ip> ipList = new List<Ip>();
            if (uncastAddresses.Count > 0)
            {
                foreach (UnicastIPAddressInformation tempIp in uncastAddresses)
                {
                    if (tempIp.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        Ip ip = new Ip();
                        if (tempIp.Address == null)
                            continue;
                        ip.Address = tempIp.Address.ToString();     //IP配置信息
                        if (tempIp.IPv4Mask == null)
                            ip.SubnetMask = "";
                        else
                            ip.SubnetMask = tempIp.IPv4Mask.ToString();
                        ipList.Add(ip);
                    }
                }
            }
            return ipList;
        }
        public static Ip GetIp()
        {
            Ip ip;
            string adapterId = "";
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adapters)
            {
                if (adapter.Name == "本地连接" || adapter.Name == "本地连接 2")
                {
                    adapterId = adapter.Id;
                    break;
                }
            }

            RegistryKey pregkey;
            pregkey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\services\\Tcpip\\Parameters\\Interfaces\\" + adapterId);
            if (pregkey == null)
                throw new Exception("未能读取IP地址!");

            Int32 enableDHCP = (Int32)pregkey.GetValue("EnableDHCP");
            string[] ipAddresss;
            string[] subnetMasks;
            if (enableDHCP == 0)
            {
                ipAddresss = (string[])pregkey.GetValue("IPAddress");
                subnetMasks = (string[])pregkey.GetValue("SubnetMask");
            }
            else
            {
                ipAddresss = new string[1];
                subnetMasks = new string[1];
                ipAddresss[0] = (string)pregkey.GetValue("DhcpIPAddress");
                subnetMasks[0] = (string)pregkey.GetValue("DhcpSubnetMask");
            }

            if (ipAddresss.Length == 0 || subnetMasks.Length == 0
                || ipAddresss[0] == null || subnetMasks[0] == null)
                throw new Exception("读取IP地址错误!");

            ip.Address = ipAddresss[0];
            ip.SubnetMask = subnetMasks[0];

            return ip;
        }

        [DllImport("Ws2_32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern Int32 inet_addr(string ip);

        [DllImport("Iphlpapi.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int SendARP(Int32 dest, Int32 host, ref Int64 mac, ref Int32 length);

        static bool CheckIpConflict(string destIp)
        {
            Int32 ldest = inet_addr(destIp);
            Int32 lhost = inet_addr(GetIp().Address);
            const bool isConflict = true;

            Int64 macinfo = new Int64();
            Int32 len = 6;
            int res = SendARP(ldest, lhost, ref macinfo, ref len);
            if (macinfo == 0)
                return !isConflict;

            return isConflict;
        }

        private static bool IsNoSpecialCharForFilePath(string CString)
        {
            string[] checkStrings = new string[] { " ", ":", "\\", ".", "-", "_", ",", "`", "~", "!", "@", "#", "$", "%", "^", "&", "(", ")", "+", "=" };
            int len = CString.Length;

            len -= CString.Length - Regex.Replace(CString, "[a-zA-Z]", "").Length;
            len -= CString.Length - Regex.Replace(CString, "[0-9]", "").Length;
            len -= CString.Length - Regex.Replace(CString, "[\u4e00-\u9fa5]", "").Length;
            foreach (var checkString in checkStrings)
            {
                len -= CString.Length - CString.Replace(checkString, "").Length;
            }

            return len == 0;
        }

        static void ShutDownWindows(string arguments)
        {
            System.Diagnostics.Process _process = new System.Diagnostics.Process();
            _process.StartInfo.CreateNoWindow = true;
            _process.StartInfo.FileName = "Shutdown.exe";
            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.RedirectStandardError = true;
            _process.StartInfo.Arguments = arguments;
            _process.Start();
            if (_process.ExitCode != 0)
            {
                throw new Exception("关闭系统错误! - " + _process.ExitCode.ToString());
            }
        }
        #endregion

#endregion

        public static bool BroadcastUdpData(int targetPort, byte[] udpData)
        {
            try
            {
                UdpClient udpClient = new UdpClient();
                IPEndPoint iep = new IPEndPoint(IPAddress.Broadcast, targetPort);
                udpClient.EnableBroadcast = true;
                udpClient.Send(udpData, udpData.Length, iep);
                Thread.Sleep(100);
                udpClient.Close();
                udpClient = null;
                iep = null;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool SendUdpData(string targetIP, int targetPort, byte[] udpData)
        {
            UdpClient udpClient = new UdpClient();
            try
            {
                udpClient.Connect(new IPEndPoint(IPAddress.Parse(targetIP), targetPort));
                udpClient.Send(udpData, udpData.Length);
                udpClient.Close();
                return true;
            }
            catch
            {
                Logger.Info("发送UDP数据失败!");
                return false;
            }
        }

        public static bool SendTcpData(string targetIP, int targetPort, byte[] tcpData)
        {
            TcpClient tcpClient = new TcpClient();
            try
            {
                tcpClient = new TcpClientWithTimeout(targetIP, targetPort, 2000).Connect();
                tcpClient.SendTimeout = 10 * 1000;//超时时间10秒
                tcpClient.ReceiveTimeout = 10 * 1000;//超时时间10秒
                NetworkStream stream = tcpClient.GetStream();
                stream.Write(tcpData, 0, tcpData.Length);
                Thread.Sleep(10);
                stream.Close();
                stream.Dispose();
                tcpClient.Close();
                GC.Collect();
            }
            catch
            {
                Logger.Info("发送TCP数据失败！");
                if (tcpClient != null)
                {
                    tcpClient.GetStream().Dispose();
                    tcpClient.Close();
                }
                return false;
            }

            return true;
        }

        public static bool CheckSum(byte[] bytes)
        {
            if (bytes == null || bytes.Length < 4)
            {
                return false;
            }

            byte sum = bytes[0];
            Int16 len = BitConverter.ToInt16(bytes, 1);

            if (len != bytes.Length)
            {
                return false;
            }

            byte[] buffer = new byte[bytes.Length - 3];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = bytes[i + 3];
            }

            if (sum != (byte)(buffer.Sum(x => x) % 256))
            {
                return false;
            }

            return true;
        }

        public static byte[] GetEncryBytes(string str)
        {
            byte[] urlBuffer = Encoding.UTF8.GetBytes(str);
            byte[] sum = new byte[] { (byte)(urlBuffer.Sum(x => x) % 256) };
            Int16 len = (Int16)(urlBuffer.Length + 1 + 2);
            byte[] lenBuffer = BitConverter.GetBytes(len);
            return sum.Concat(lenBuffer).Concat(urlBuffer).ToArray();
        }
    }

    public class TcpClientWithTimeout
    {
        protected string _hostname;
        protected int _port;
        protected int _timeout_milliseconds;
        protected TcpClient connection;
        protected bool connected;

        public TcpClientWithTimeout(string hostname, int port, int timeout_milliseconds)
        {
            _hostname = hostname;
            _port = port;
            _timeout_milliseconds = timeout_milliseconds;
        }

        public TcpClient Connect()
        {
            // kick off the thread that tries to connect
            connected = false;
            Thread thread = new Thread(new ThreadStart(BeginConnect));
            thread.IsBackground = true; // So that a failed connection attempt
            // wont prevent the process from terminating while it does the long timeout
            thread.Start();

            // wait for either the timeout or the thread to finish
            thread.Join(_timeout_milliseconds);

            if (connected == true)
            {
                // it succeeded, so return the connection
                thread.Abort();
            }
            else
            {
                connection = null;
            }
            return connection;
        }

        protected void BeginConnect()
        {
            try
            {
                connection = new TcpClient(_hostname, _port);
                // record that it succeeded, for the main thread to return to the caller
                connected = true;
            }
            catch
            {
                connected = false;
            }
        }
    }

}
