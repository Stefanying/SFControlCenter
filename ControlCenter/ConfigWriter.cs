using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace ControlCenter
{
    class ConfigWriter
    {
        static string configFile = AppDomain.CurrentDomain.BaseDirectory + "config.xml";
        static string timelineFile = AppDomain.CurrentDomain.BaseDirectory + "timeline.xml";

        public static void LoadConfig()
        {
            try
            {
                XmlDocument config = new XmlDocument();
                config.Load(configFile);

                XmlNode root = config.SelectSingleNode("Root");
                XmlNodeList areas = root.SelectNodes("Area");

                foreach (XmlNode area in areas)
                {
                    XmlNodeList actions = area.SelectNodes("Action");
                    for (int action_count = 0; action_count < actions.Count; action_count++)
                    {
                        XmlNode action = actions[action_count];
                        string receiveData = action.SelectSingleNode("ActionReceiveData").InnerText;

                        string action_string = "";
                        string name = action.SelectSingleNode("ActionName").InnerText;
                       
                        XmlNodeList operations = action.SelectNodes("Operation");
                        for (int operation_count = 0; operation_count < operations.Count; operation_count++)
                        {
                            XmlNode operation = operations[operation_count];

                            string operation_string = "";
                            string operationName = operation.SelectSingleNode("OperationName").InnerText;
                            string operationType = operation.SelectSingleNode("OperationType").InnerText;
                            string operationDataType = operation.SelectSingleNode("OperationDataType").InnerText;
                            string operationData = operation.SelectSingleNode("OperationData").InnerText;
                            string operationTime = operation.SelectSingleNode("OperationTime").InnerText;

                            string setting_string = "Setting = {";
                            XmlNode setting = operation.SelectSingleNode("OperationSetting");
                            switch (operationType.ToLower())
                            {
                                case "com":
                                    string comNumber = setting.SelectSingleNode("ComNumber").InnerText;
                                    string baudRate = setting.SelectSingleNode("BaudRate").InnerText;
                                    string databit = setting.SelectSingleNode("DataBit").InnerText;
                                    string stopbit = setting.SelectSingleNode("StopBit").InnerText;
                                    string parity = setting.SelectSingleNode("Parity").InnerText;

                                    setting_string += "comNumber =\"" + comNumber + "\","
                                                   + "baudRate = \"" + baudRate + "\","
                                                   + "dataBit = \"" + databit + "\","
                                                   + "stopBit = \"" + stopbit + "\","
                                                   + "parity = \"" + parity + "\"},";
                                    break;
                                case "tcp":
                                case "udp":
                                    string ip = setting.SelectSingleNode("IP").InnerText;
                                    string port = setting.SelectSingleNode("Port").InnerText;

                                    setting_string += "ip = \"" + ip + "\","
                                                   + "port =\"" + port + "\"},";
                                    break;
                            }

                            operation_string += "[" + (operation_count + 1).ToString() + "]  = { operationType =\"" + operationType + "\", "
                                             + "operationDataType =\"" + operationDataType + "\", "
                                             + "operationData =\"" + operationData + "\","
                                             + setting_string
                                             + "operationTime =\"" + operationTime + "\"}";
                            if (operation_count != operations.Count - 1)
                            {
                                operation_string += ",";
                            }
                            action_string += operation_string + Environment.NewLine;
                        }

                        StreamReader readTxt = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "Script\\" + "scriptTemplate.lua");
                        string scriptText = readTxt.ReadToEnd();

                        scriptText = scriptText.Replace("#commands", action_string);

                        StreamWriter writeTxt = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "Script\\" + receiveData + ".lua");
                        writeTxt.Write(scriptText);
                        writeTxt.Flush();
                        writeTxt.Close();
                        readTxt.Close();
                    }                   
                }
            }
            catch
            {
                SFLib.Logger.Error("解析配置出错!");
            }
        }

        public static void LoadTimeShaft()
        {
            try
            {
                XmlDocument config = new XmlDocument();
                config.Load(configFile);

                XmlNode root = config.SelectSingleNode("Root");
                XmlNode timeShaft = root.SelectSingleNode("TimeShaft");

                XmlNodeList actions = timeShaft.SelectNodes("Action");
                for (int action_count = 0; action_count < actions.Count; action_count++)
                {
                    XmlNode action = actions[action_count];
                    string receiveData = action.SelectSingleNode("ActionReceiveData").InnerText;

                    string action_string = "";
                    string name = action.SelectSingleNode("ActionName").InnerText;

                    XmlNodeList operations = action.SelectNodes("Operation");
                    for (int operation_count = 0; operation_count < operations.Count; operation_count++)
                    {
                        XmlNode operation = operations[operation_count];

                        string operation_string = "";
                        string operationName = operation.SelectSingleNode("OperationName").InnerText;
                        string operationType = operation.SelectSingleNode("OperationType").InnerText;
                        string operationDataType = operation.SelectSingleNode("OperationDataType").InnerText;
                        string operationData = operation.SelectSingleNode("OperationData").InnerText;
                        string operationTime = operation.SelectSingleNode("OperationTime").InnerText;

                        string setting_string = "Setting = {";
                        XmlNode setting = operation.SelectSingleNode("OperationSetting");
                        switch (operationType.ToLower())
                        {
                            case "com":
                                string comNumber = setting.SelectSingleNode("ComNumber").InnerText;
                                string baudRate = setting.SelectSingleNode("BaudRate").InnerText;
                                string databit = setting.SelectSingleNode("DataBit").InnerText;
                                string stopbit = setting.SelectSingleNode("StopBit").InnerText;
                                string parity = setting.SelectSingleNode("Parity").InnerText;

                                setting_string += "comNumber =\"" + comNumber + "\","
                                                + "baudRate = \"" + baudRate + "\","
                                                + "dataBit = \"" + databit + "\","
                                                + "stopBit = \"" + stopbit + "\","
                                                + "parity = \"" + parity + "\"},";
                                break;
                            case "tcp":
                            case "udp":
                                string ip = setting.SelectSingleNode("IP").InnerText;
                                string port = setting.SelectSingleNode("Port").InnerText;

                                setting_string += "ip = \"" + ip + "\","
                                                + "port =\"" + port + "\"},";
                                break;
                        }

                        operation_string += "[" + (operation_count + 1).ToString() + "]  = { operationType =\"" + operationType + "\", "
                                            + "operationDataType =\"" + operationDataType + "\", "
                                            + "operationData =\"" + operationData + "\","
                                            + setting_string
                                            + "operationTime =\"" + operationTime + "\"}";
                        if (operation_count != operations.Count - 1)
                        {
                            operation_string += ",";
                        }
                        action_string += operation_string + Environment.NewLine;
                    }

                    StreamReader readTxt = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "Script\\" + "TimeShaft.lua");
                    string scriptText = readTxt.ReadToEnd();

                    scriptText = scriptText.Replace("#commands", action_string);

                    StreamWriter writeTxt = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "Script\\" + receiveData + ".lua");
                    writeTxt.Write(scriptText);
                    writeTxt.Flush();
                    writeTxt.Close();
                    readTxt.Close();
                }
            }
            catch
            {
                SFLib.Logger.Error("解析配置出错!");
            }
        }

        public static void LoadTimeLineConfig()
        {
            try
            {
                XmlDocument config = new XmlDocument();
                config.Load(timelineFile);

                XmlNode root = config.SelectSingleNode("Root");
                XmlNodeList orders = root.SelectNodes("Time");

                string order_string = "";
                foreach (XmlNode order in orders)
                {
                     
                        string order_name = order.SelectSingleNode("TimeValue").InnerText;
                        order_string += "[\"" +order_name  + "\"] ={";
                        XmlNodeList operations = order.SelectNodes("Operation");

                        for (int operation_count = 0; operation_count < operations.Count; operation_count++)
                        {
                            XmlNode operation = operations[operation_count];

                            string operation_string = "";
                            string operationName = operation.SelectSingleNode("OperationName").InnerText;
                            string operationType = operation.SelectSingleNode("OperationType").InnerText;
                            string operationDataType = operation.SelectSingleNode("OperationDataType").InnerText;
                            string operationData = operation.SelectSingleNode("OperationData").InnerText;
                            string operationTime = operation.SelectSingleNode("OperationTime").InnerText;

                            string setting_string = "Setting = {";
                            XmlNode setting = operation.SelectSingleNode("OperationSetting");
                            switch (operationType.ToLower())
                            {
                                case "com":
                                    string comNumber = setting.SelectSingleNode("ComNumber").InnerText;
                                    string baudRate = setting.SelectSingleNode("BaudRate").InnerText;
                                    string databit = setting.SelectSingleNode("DataBit").InnerText;
                                    string stopbit = setting.SelectSingleNode("StopBit").InnerText;
                                    string parity = setting.SelectSingleNode("Parity").InnerText;

                                    setting_string += "comNumber =\"" + comNumber + "\","
                                                   + "baudRate = \"" + baudRate + "\","
                                                   + "dataBit = \"" + databit + "\","
                                                   + "stopBit = \"" + stopbit + "\","
                                                   + "parity = \"" + parity + "\"},";
                                    break;
                                case "tcp":
                                case "udp":
                                    string ip = setting.SelectSingleNode("IP").InnerText;
                                    string port = setting.SelectSingleNode("Port").InnerText;

                                    setting_string += "ip = \"" + ip + "\","
                                                   + "port =\"" + port + "\"},";
                                    break;
                            }

                            operation_string += "[" + (operation_count + 1).ToString() + "]  = { operationType =\"" + operationType + "\", "
                                             + "operationDataType =\"" + operationDataType + "\", "
                                             + "operationData =\"" + operationData + "\","
                                             + setting_string
                                             + "operationTime =\"" + operationTime + "\"}";
                            if (operation_count != operations.Count - 1)
                            {
                                operation_string += ",";
                            }
                            order_string += operation_string + Environment.NewLine;
                        }
                         order_string += "}";

                        if(order != orders[orders.Count - 1])
                        {
                            order_string += ",";
                        }
                    }

                    StreamReader readTxt = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "Script\\" + "TimeLineTemplate.lua");
                    string scriptText = readTxt.ReadToEnd();

                    scriptText = scriptText.Replace("#commands", order_string);

                    StreamWriter writeTxt = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "Script\\TimeLine.lua");
                    writeTxt.Write(scriptText);
                    writeTxt.Flush();
                    writeTxt.Close();
                    readTxt.Close();
                    }
            catch
            {
                SFLib.Logger.Error("解析配置出错!");
            }
        }

        public static string HexStringToASCII(string hexstring)
        {
            byte[] bt = HexStringToBinary(hexstring);
            string lin = "";
            for (int i = 0; i < bt.Length; i++)
            {
                lin = lin + bt[i] + " ";
            }


            string[] ss = lin.Trim().Split(new char[] { ' ' });
            char[] c = new char[ss.Length];
            int a;
            for (int i = 0; i < c.Length; i++)
            {
                a = Convert.ToInt32(ss[i]);
                c[i] = Convert.ToChar(a);
            }

            string b = new string(c);
            return b;
        }

        public static byte[] HexStringToBinary(string hexstring)
        {
            int arrayLength = 0;

            if (hexstring.Length % 2 == 0)
            {
                arrayLength = hexstring.Length / 2;
            }
            else
            {
                arrayLength = hexstring.Length / 2 + 1;
            }
            string[] tmpary = new string[arrayLength];

            for (int i = 0; i < arrayLength; i++)
            {
                if (hexstring.Length - i * 2 >= 2)
                {
                    tmpary[i] = hexstring.Substring(i * 2, 2);
                }
                else
                {
                    tmpary[i] = "0" + hexstring.Substring(i * 2, 1);
                }
            }

            byte[] buff = new byte[tmpary.Length];
            for (int i = 0; i < buff.Length; i++)
            {
                buff[i] = Convert.ToByte(tmpary[i], 16);
            }
            return buff;
        }
    }
}
