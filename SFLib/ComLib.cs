using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Ports;

namespace SFLib
{
    /// <summary>
    /// 串口相关类
    /// </summary>
    public class ComLib
    {
        public static void SendComData(string targetPort, int baudrate, int databit, int stopbit, string parity, byte[] bufferData)
        {
            SerialPort port = new SerialPort(targetPort);
            try
            {
                port.BaudRate = baudrate;
                port.DataBits = databit;
                port.StopBits = StopBits.One;

                switch (stopbit)
                {
                    case 1:
                        port.StopBits = StopBits.One;
                        break;
                    case 2:
                        port.StopBits = StopBits.Two;
                        break;
                }

                Parity currentparity = Parity.None;
                switch (parity.ToLower())
                {
                    case "odd":
                        currentparity = Parity.Odd;
                        break;
                    case "even":
                        currentparity = Parity.Even;
                        break;
                    default:
                        currentparity = Parity.None;
                        break;
                }
              
                port.Parity = currentparity;
                if (!port.IsOpen)
                {
                    port.Open();
                }

                port.Write(bufferData, 0, bufferData.Length);

            }
            catch(Exception ex)
            {
                Logger.Exception(ex.Message);
                Logger.Exception("发送串口数据失败！");
            }
            finally
            {
                port.Close();
            }
        }
    }
}
