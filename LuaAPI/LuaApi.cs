using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Collections;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.Web;
using System.Linq;
using System.Net.NetworkInformation;
using SFLib;

namespace LuaApi
{
    //返回执行结果的枚举
    public enum LuaApiResult
    {
        CustomError = 0,
        Success = 1,
        RunProcessFailed = 2,
        ProcessNotExist = 3,
        SendTcpDataFailed = 4,
        SendUdpDataFailed = 5,
        SendComDataFailed = 6,
        SendMessageApiFailed = 7,
        RunThreadFailed = 8,
        NotFoundHandler = 9
    }

    //LuaApi的接口
    public class LuaApiInterface
    {
        private string _rootPath = AppDomain.CurrentDomain.BaseDirectory;

        string _projectName = "";
        static ResourceManager _resourceManager = ResourceManager.getInstance();

        public LuaApiInterface(string projectName)
        {
            _projectName = projectName;
        }

        //修改脚本运行时间过长保护的时间
        [LuaFunction("StayAlive")]
        public void StayAlive(int killCount)
        {
            ThreadProtecter.getInstance().KillCount = killCount;
        }

        //将相对路径转换为绝对路径
        [LuaFunction("MapPath")]
        public string MapPath(string virtualPath)
        {
            string resultPath = _rootPath + virtualPath;
            return File.Exists(resultPath) || Directory.Exists(resultPath) ? resultPath : "";
        }

        //启动线程
        [LuaFunction("RunThread")]
        public int RunThread(string resourceName, string scriptName, string executeString)
        {
            return (int)(_resourceManager.RunThread(resourceName, _projectName, scriptName, executeString) ? LuaApiResult.Success : LuaApiResult.RunThreadFailed);
        }

        //进程是否存在
        [LuaFunction("ProcessExist")]
        public bool ProcessExist(string processName)
        {
            return Process.GetProcessesByName(processName).Length > 0 ? true : false;
        }

        private static void ConvertLuaTableToList(LuaInterface.LuaTable lt, out List<string> keyList, out List<string> valueList)
        {
            object[] arrayValues = new object[100];
            object[] arrayKeys = new object[100];
            lt.Values.CopyTo(arrayValues, 0);
            lt.Keys.CopyTo(arrayKeys, 0);
            keyList = new List<string>();
            valueList = new List<string>();
            for (int i = 0; i < arrayValues.Length; i++)
            {
                try
                {
                    keyList.Add(arrayKeys.GetValue(i).ToString());
                    valueList.Add(arrayValues.GetValue(i).ToString());
                }
                catch
                {
                    break;
                }
            }
        }

        //杀掉指定进程
        [LuaFunction("Kill")]
        public void Kill(string processName)
        {
            foreach (var item in Process.GetProcessesByName(processName))
            {
                item.Kill();
                Thread.Sleep(200);//等待程序结束
                item.Dispose();
            }
        }

     
        //程序休眠：休眠时间 int类型 单位毫秒
        [LuaFunction("Delay")]
        public void Delay(int sleepMilliSeconds)
        {
            DateTime dtBegin = DateTime.Now;
            while (true)
            {
                if (DateTime.Now.Subtract(dtBegin).TotalMilliseconds >= sleepMilliSeconds)
                {
                    break;
                }
            }
        }

        //程序休眠（阻塞线程，降低CPU占用率）：休眠时间 int类型 单位毫秒
        [LuaFunction("Sleep")]
        public void Sleep(int sleepMilliSeconds)
        {
            Thread.Sleep(sleepMilliSeconds);
        }

        //Lua脚本对外的输出，输出到Log里
        [LuaFunction("PrintToLog")]
        public void PrintToLog(string sayStr)
        {
            Logger.Info("输出：" + sayStr);
        }

        //发送Tcp数据
        [LuaFunction("SendTcpData")]
        public int SendTcpData(string targetIP, int targetPort, object tcpDataObj)
        {
            byte[] tcpDataByte = ConvertLuaTableToBytes(tcpDataObj);
            if (tcpDataByte == null)
            {
                return (int)LuaApiResult.SendTcpDataFailed;
            }
            return (int)(NetLib.SendTcpData(targetIP, targetPort, tcpDataByte) ? LuaApiResult.Success : LuaApiResult.SendTcpDataFailed);
        }

        //将字符串转换为校验和加密后的bytes，以"|"分开组成字符串
        [LuaFunction("StringToEncryBytes")]
        public string StringToEncryBytes(string str)
        {
            try
            {
                string result = "";
                byte[] buffer = NetLib.GetEncryBytes(str);
                foreach (var item in buffer)
                {
                    result += item + "|";
                }

                if (!string.IsNullOrEmpty(result))
                {
                    result = result.Substring(0, result.Length - 1);
                }

                return result;
            }
            catch
            {
                return null;
            }
        }

        //发送Udp数据
        [LuaFunction("SendUdpData")]
        public int SendUdpData(string targetIP, int targetPort, object udpDataObj)
        {
            byte[] udpDataByte = ConvertLuaTableToBytes(udpDataObj);
            if (udpDataByte == null)
            {
                return (int)LuaApiResult.SendUdpDataFailed;
            }
            return (int)(NetLib.SendUdpData(targetIP, targetPort, udpDataByte) ? LuaApiResult.Success : LuaApiResult.SendUdpDataFailed);
        }

        //发送串口码
        [LuaFunction("SendComData")]
        public int SendComData(string targetPort, int baudrate, int databit, int stopbit, string parity, object comData)
        {
            byte[] comDataByte = ConvertLuaTableToBytes(comData);

            if(comDataByte  == null)
            {
                return (int)LuaApiResult.SendComDataFailed;
            }

            try
            {
                ComLib.SendComData(targetPort,baudrate, databit, stopbit, parity, comDataByte);
                return (int)LuaApiResult.Success ;
            }
            catch
            {
                  return (int)LuaApiResult.SendComDataFailed;
            }
        }

        public byte[] ConvertLuaTableToBytes(object tcpDataObj)
        {
            List<string> tcpDataList = new List<string>();
            List<string> keyList = new List<string>();

            LuaInterface.LuaTable tcpDataLT = (LuaInterface.LuaTable)tcpDataObj;

            ConvertLuaTableToList(tcpDataLT, out keyList, out tcpDataList);

            if (tcpDataList == null || tcpDataList.Count == 0)
            {
                return null;
            }
            byte[] tcpDataByte = new byte[tcpDataList.Count];
            for (int i = 0; i < tcpDataList.Count; i++)
            {
                tcpDataByte[i] = Byte.Parse(tcpDataList[i]);
            }
            return tcpDataByte;
        }

        //返回自定义错误描述
        [LuaFunction("ReturnCustomError")]
        public void ReturnCustomError(string error)
        {
            throw new SFReturnCode((int)LuaApiResult.CustomError, error);
        }

       [LuaFunction("GetCurrentTime")]
        public string GetCurrentTime()
        {
            return string.Format("{0:d2}",DateTime.Now.Hour) + ":" + string.Format("{0:d2}",DateTime.Now.Minute);
        }
    }
}
