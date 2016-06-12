using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using SFLib;

namespace LuaApi
{
    //资源管理，包括启动、关闭、资源状态查询、保护等
    public class ResourceManager
    {
        private static ResourceManager _processManage = new ResourceManager();
        private static Dictionary<string, string> _dicLuaGlobal;
        private static Dictionary<string, Resource> _dicResource;

        private ResourceManager() { }

        public static ResourceManager getInstance()
        {
            if (_dicResource == null)
            {
                _dicResource = new Dictionary<string, Resource>();
                _dicLuaGlobal = new Dictionary<string, string>();
            }
            return _processManage;
        }

        public bool RunThread(string resourceName, string projectName, string scriptName, string executeString)
        {
            try
            {
                Resource resource = new Resource(ResourceType.Thread);
                resource.RunThread(projectName, scriptName, executeString);
                AddResource(resourceName, resource);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool RunProcess(string resourceName, string filePath, string parameters, bool needRedirectStdIn, bool needOutput, bool isShow)
        {
            try
            {
                Resource resource = new Resource(ResourceType.Process);
                resource.RunProcess(filePath, parameters, needRedirectStdIn, needOutput, isShow);
                AddResource(resourceName, resource);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void AddResource(string resourceName, Resource resource)
        {
            if (_dicResource.ContainsKey(resourceName))
            {
                if (ResourceExist(resourceName))
                {
                    ResourceKiller(resourceName);
                }
                _dicResource.Remove(resourceName);
            }
            _dicResource.Add(resourceName, resource);
        }

        public bool SendStdInToProcess(string resourceName, string message)
        {
            if (!isExistResourceName(resourceName))
            {
                return false;
            }
            _dicResource[resourceName].SendStdInToProcess(message);
            return true;
        }

        public void CloseProcess(string resourceName)
        {
            if (!isExistResourceName(resourceName))
            {
                return;
            }
            ResourceKiller(resourceName);
            _dicResource.Remove(resourceName);
            Thread.Sleep(100);
        }

        public void ResetVM()
        {
            foreach (var item in _dicResource.Keys)
            {
                if (ResourceExist(item))
                {
                    ResourceKiller(item);
                }
            }
            _dicResource.Clear();
        }

        public bool ResourceExist(string resourceName)
        {
            if (_dicResource.ContainsKey(resourceName))
            {
                switch (_dicResource[resourceName].ResourceType)
                {
                    case ResourceType.Process:
                        if (!_dicResource[resourceName].Process.HasExited)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    case ResourceType.Thread:
                        if (_dicResource[resourceName].Thread != null && _dicResource[resourceName].Thread.ThreadState != System.Threading.ThreadState.Aborted && _dicResource[resourceName].Thread.ThreadState != System.Threading.ThreadState.Stopped)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    default:
                        return false;
                }
            }
            else
            {
                return false;
            }
        }

        public int Reserve(List<string> needReserveResourceNamesList)
        {
            Dictionary<string, Resource> reservedResource = new Dictionary<string, Resource>();
            foreach (var item in _dicResource.Keys)
            {
                if (needReserveResourceNamesList.Contains(item))
                {
                    reservedResource.Add(item, _dicResource[item]);
                }
                else
                {
                    ResourceKiller(item);
                    needReserveResourceNamesList.Remove(item);
                }
            }

            _dicResource.Clear();
            _dicResource = reservedResource;

            return needReserveResourceNamesList.Count;

        }

        private static bool isExistResourceName(string resourceName)
        {
            return _dicResource.ContainsKey(resourceName);
        }

        public static void ResourceKiller(string resourceName)
        {
            try
            {
                KillTheResource(resourceName);
                Thread.Sleep(200);//等待程序结束
                ClearWin7Warning();
                ClearIcon();
            }
            catch { }

        }

        private static void KillTheResource(string resourceName)
        {
            switch (_dicResource[resourceName].ResourceType)
            {
                case ResourceType.Process:
                    _dicResource[resourceName].Process.Kill();
                    _dicResource[resourceName].Process.Dispose();
                    break;
                case ResourceType.Thread:
                    _dicResource[resourceName].Thread.Abort();
                    break;
                default:
                    Logger.Error("没有此类型的资源");
                    break;
            }
        }

        private static void ClearWin7Warning()
        {
            foreach (var itemProcess in Process.GetProcessesByName("WerFault"))
            {
                itemProcess.Kill();
            }
        }

        public bool SetLuaGlobal(string key, string value)
        {
            try
            {
                if (!_dicLuaGlobal.ContainsKey(key))
                {
                    _dicLuaGlobal.Add(key, value);
                }
                else
                {
                    _dicLuaGlobal[key] = value;
                }
                return true;
            }
            catch
            {
                return false;
            }

        }

        public string GetLuaGlobal(string key)
        {
            try
            {
                return _dicLuaGlobal[key];
            }
            catch
            {
                return "";
            }
        }

        private static void ClearIcon()
        {
            Point originalPoint = Cursor.Position;
            Point startPoint = new Point(Screen.PrimaryScreen.Bounds.Width - 400, Screen.PrimaryScreen.Bounds.Height - 20);
            Point endPoint = new Point(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height - 20);
            for (int i = 0; i < 30; i++)
            {
                startPoint.X = Screen.PrimaryScreen.Bounds.Width - 400;
                for (; startPoint.X < endPoint.X; startPoint.X += 1)
                {
                    Cursor.Position = startPoint;
                    Thread.Sleep(new TimeSpan((long)2));
                }
                Cursor.Position = originalPoint;
            }
        }


        public void WaitResourceExit(string resourceName)
        {
            if (!ResourceExist(resourceName))
            {
                return;
            }

            bool isExist = false;
            while (!isExist)
            {
                switch (_dicResource[resourceName].ResourceType)
                {
                    case ResourceType.Process:
                        isExist = _dicResource[resourceName].Process.HasExited;
                        break;
                    case ResourceType.Thread:
                        if (_dicResource[resourceName].Thread.ThreadState == System.Threading.ThreadState.Aborted ||
                            _dicResource[resourceName].Thread.ThreadState == System.Threading.ThreadState.Stopped)
                        {
                            isExist = true;
                        }
                        break;
                    default:
                        isExist = true;
                        break;
                }
                Thread.Sleep(500);
            }
        }
    }
}
