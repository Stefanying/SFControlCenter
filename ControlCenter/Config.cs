using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFLib;
using System.IO;

namespace ControlCenter
{
    class Config
    {
        public static readonly Dictionary<string, string> Items = new Dictionary<string, string>();
        private static readonly string[] importantConfigParameters =
            new string[] { "ProjectName", "Protector", "TcpPort", "UdpPort", "IsComEnable" , "ComNewLine", "Http", "TCP", "UDP"};

        public static void Load(string file)
        {
            if (!File.Exists(file))
            {
                Logger.Exit("配置文件不存在！" + file);
            }

            Items.Clear();
            string[] lines = File.ReadAllLines(file);

            for (int i = 0; i < lines.Length; i++)
            {
                ParseLine(lines, i);
            }

            ExitIfMissingParameters(importantConfigParameters);
        }


        /// <summary>
        /// 检测重要参数中是否缺少数据
        /// </summary>
        /// <param name="configParameters"></param>
        private static void ExitIfMissingParameters(string[] configParameters)
        {
            if (configParameters != null)
            {
                foreach (var item in configParameters)
                {
                    if (!Items.ContainsKey(item))
                    {
                        Logger.Exit(string.Format("程序初始化配置文件缺少参数【{0}】！", item));
                    }
                }
            }
        }

        private static void ParseLine(string[] lines, int index)
        {
            try
            {
                int splitIndex = lines[index].IndexOf('=');
                Items.Add(lines[index].Substring(0, splitIndex).Trim(), lines[index].Substring(splitIndex + 1).Trim());
            }
            catch
            {
                Logger.Warning("解析程序初始化配置文件出错。 第" + (index + 1).ToString() + "行");
            }
        }
    }
}
