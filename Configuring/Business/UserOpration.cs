using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Configuring.Business
{
    public enum CommunicationType
    {
        TCP,
        UDP,
        Com
    }

    public enum DataType
    {
        Hex,
        Character
    }

    public enum Parity
    {
        Odd,
        Even,
        None
    }

    public class ComSetting
    {
        string _comNumber;
        public string ComNumber
        {
            get { return _comNumber; }
            set { _comNumber = value; }
        }

        int _baudRate;
        public int BaudRate
        {
            get { return _baudRate; }
            set { _baudRate = value; }
        }

        int _dataBits;
        public int DataBits
        {
            get { return _dataBits; }
            set { _dataBits = value; }
        }
        int _stopBits;

        public int StopBits
        {
            get { return _stopBits; }
            set { _stopBits = value; }
        }
        Parity _parity;
        public Parity Parity
        {
            get { return _parity; }
            set { _parity = value; }
        }
    }

    public class NetworkSetting
    {
        string _ip;

        public string Ip
        {
            get { return _ip; }
            set { _ip = value; }
        }
        int _port;

        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }
    }

    //操作动作
    public class UserOperation
    {
        string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        CommunicationType _opreationType;
        internal CommunicationType OpreationType
        {
            get { return _opreationType; }
            set { _opreationType = value; }
        }

        DataType _dataType;
        internal DataType DataType
        {
            get { return _dataType; }
            set { _dataType = value; }
        }

        object _setting;
        public object Setting
        {
            get { return _setting; }
            set { _setting = value; }
        }

        int _delayTime;//延迟执行时间
        public int DelayTime
        {
            get { return _delayTime; }
            set { _delayTime = value; }
        }

        string _data;
        public string Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public UserOperation(string name, CommunicationType oType, DataType dType, object setting, string data, int time)
        {
            _name = name;
            _opreationType = oType;
            _dataType = dType;
            _setting = setting;
            _data = data;
            _delayTime = time;
        }
    }
}
