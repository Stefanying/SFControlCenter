using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;
using SFLib;

namespace LuaApi
{
    public enum ResourceType
    {
        Process,
        Thread
    }

    class Resource
    {
        private ResourceType _resourceType;

        public ResourceType ResourceType
        {
            get { return _resourceType; }
        }

        private Process _process = new Process();

        public Process Process
        {
            get { return _process; }
        }

        private Thread _thread;

        public Thread Thread
        {
            get { return _thread; }
        }

        private StringBuilder _output = new StringBuilder();

        volatile static string _projectName;

        public Resource(ResourceType resourceType)
        {
            _resourceType = resourceType;
        }

        /// <summary>
        /// 运行线程
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="scriptName"></param>
        /// <param name="executeString"></param>
        public void RunThread(string projectName, string scriptName, string executeString)
        {
            _projectName = projectName;
            _thread = new Thread(new ParameterizedThreadStart(RunScriptWithThread));
            _thread.IsBackground = true;
            _thread.Start(new string[] { scriptName, executeString });

        }

        /// <summary>
        /// 运行脚本线程
        /// </summary>
        /// <param name="scriptNameObject"></param>
        private void RunScriptWithThread(object scriptNameObject)
        {
            try
            {
                string scriptName = (scriptNameObject as string[])[0];
                string executeString = (scriptNameObject as string[])[1];
                string scriptPath = AppDomain.CurrentDomain.BaseDirectory  + "Script/"+ scriptName;
                LuaApiRegister luaHelper = new LuaApiRegister(new LuaApiInterface(_projectName));
                luaHelper.ExecuteString(executeString);
                luaHelper.ExecuteString("g_ProjectName = \"" + _projectName + "\";");//增加g_ProjectName 全局变量
                luaHelper.ExecuteString("package.path = package.path..[[;" + AppDomain.CurrentDomain.BaseDirectory + "Script/" + "?.lua" + "]]");//把Script目录加入lua require搜索路径
                luaHelper.ExecuteFile(scriptPath);
            }
            catch (ThreadAbortException abortEx)
            {
                Logger.Info("中止预约。");
            }
            catch (Exception ex)
            {
                Logger.Info("预约：" + ex.Message);
            }
        }

        public void RunProcess(string filePath, string parameters, bool needRedirectStdIn, bool needOutput, bool isShow)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            InitProcessStartInfo(filePath, parameters, needRedirectStdIn, processStartInfo, isShow);
            _process = Process.Start(processStartInfo);
            RedirectProcessInputAndOutput(needRedirectStdIn, needOutput, _process);
        }

        private static void InitProcessStartInfo(string filePath, string parameters, bool needRedirectStdIn, ProcessStartInfo processStartInfo, bool isShow)
        {
            processStartInfo.FileName = "\"" + filePath + "\"";     //设定程序路径   
            processStartInfo.Arguments = parameters;                //设定程式执行参数
            processStartInfo.CreateNoWindow = true;                 //设置不显示窗口
            processStartInfo.WorkingDirectory = Path.GetDirectoryName(filePath);
            if (needRedirectStdIn)
            {
                InitProcessStartInfoWithInput(processStartInfo);
            }
            if (!isShow)
            {
                processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            }
        }

        private static void InitProcessStartInfoWithInput(ProcessStartInfo processStartInfo)
        {
            processStartInfo.UseShellExecute = false;               //关闭Shell的使用   
            processStartInfo.RedirectStandardInput = true;          //重定向标准输入   
            processStartInfo.RedirectStandardError = true;
            processStartInfo.RedirectStandardOutput = true;
        }

        private void RedirectProcessInputAndOutput(bool needRedirectStdIn, bool needOutput, Process runProcess)
        {
            if (needRedirectStdIn)
            {
                if (needOutput)
                {
                    runProcess.ErrorDataReceived += new DataReceivedEventHandler(OutputDataReceived);
                    runProcess.OutputDataReceived += new DataReceivedEventHandler(OutputDataReceived);
                }
                else
                {
                    runProcess.ErrorDataReceived += new DataReceivedEventHandler(delegate { });
                    runProcess.OutputDataReceived += new DataReceivedEventHandler(delegate { });
                }
                runProcess.BeginOutputReadLine();
                runProcess.BeginErrorReadLine();
            }
        }

        private void OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (_output.Length > 5000)
            {
                _output.Remove(0, 3000);
            }
            _output.Append(e.Data);
        }

        public void SendStdInToProcess(string message)
        {
            _process.StandardInput.WriteLine(message);
        }
    }
}
