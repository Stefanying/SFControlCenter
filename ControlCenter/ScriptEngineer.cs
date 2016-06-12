using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFLib;
using System.Collections.Specialized;
using System.IO;
using System.Threading;
using LuaApi;

namespace ControlCenter
{
    public class ScriptEngineer
    {
        private string _projectName;
        public string _scriptRoot ;
        private string _throwMessage = "";
        private Thread _doFileThread = null;
        private SFReturnCode _returnCode = null;

        public void ExecuteScript(string scriptName, NameValueCollection parameters)
        {
            try
            {
                if (!File.Exists(string.Format("{0}{1}.lua", _scriptRoot, scriptName)))
                {
                    throw new FileNotFoundException();
                }

                LuaApiRegister luaHelper = new LuaApiRegister(new LuaApiInterface(_projectName));
                InitLuaGlobalParameter(luaHelper, parameters);
                ThreadExecuteFile(luaHelper, _scriptRoot + scriptName + ".lua");
            }
            catch (Exception ex)
            {
                Logger.Exception(ex.Message);
            }

        }

        private void InitLuaGlobalParameter(LuaApiRegister luaHelper, NameValueCollection parameters)
        {
            if (parameters != null && parameters.Count > 0)
            {
                foreach (var item in parameters.AllKeys)
                {
                    luaHelper.ExecuteString("a_" + item.Trim() + " = \"" + parameters[item].Replace("\\", "\\\\") + "\";");
                }
            }

            luaHelper.ExecuteString("g_ProjectName = \"" + _projectName + "\";");//增加g_ProjectName 全局变量
            luaHelper.ExecuteString("package.path = package.path..[[;" + _scriptRoot + "?.lua" + "]]");//把Script目录加入lua require搜索路径
        }

        private void ThreadExecuteFile(LuaApiRegister luaHelper, string luaFileName)
        {
            int originalKillCount = 0;
            try
            {
                originalKillCount = Int32.Parse(Config.Items["Protector"]);
            }
            catch
            {

            }

            _doFileThread = new Thread(new ThreadStart(() =>
            {
                try
                {
                    _throwMessage = "";
                    _returnCode = null;
                    luaHelper.ExecuteFile(luaFileName);
                }
                catch (ThreadAbortException threadAbortEx)
                {
                    Logger.Info("脚本引擎主动中止线程。");
                }
                catch (SFReturnCode returnCode)
                {
                    _returnCode = returnCode;
                }
                catch (Exception ex)
                {
                    _throwMessage = ex.Message;
                }
            }));
            _doFileThread.IsBackground = true;
            _doFileThread.Start();

            ThreadProtecter.getInstance(originalKillCount).Start(_doFileThread);
            if (ThreadProtecter.getInstance(originalKillCount).IsTimeout)
            {
                Logger.Error("自动关闭脚本");
                throw new TimeoutException("自动关闭脚本");
            }
            else if (_returnCode != null)
            {
                throw _returnCode;
            }
            else if (string.IsNullOrEmpty(_throwMessage))
            {
                Logger.Info("执行完毕：" + luaFileName);
            }
            else if (!string.IsNullOrEmpty(_throwMessage))
            {
                Logger.Info(_throwMessage);
            }
        }

        public void Start(string projectName)
        {
            _projectName = projectName;
            _scriptRoot = AppDomain.CurrentDomain.BaseDirectory + "Script\\";

            if (!Directory.Exists(_scriptRoot))
            {
                Logger.Exit("检测到脚本路径不存在！");
            }
        }

        public void Stop()
        {
            try
            {
                Logger.Info("退出程序！");
            }
            catch
            {
                Logger.Warning("退出异常！");
            }
        }

    }
}
