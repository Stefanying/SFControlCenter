using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Configuring.Business
{

    //投影机状态
    public enum PrjState
    {
        开,
        关
    }

    //继电器状态
    public enum RelayState
    {
        吸合,
        断开
    }
    

   public class UserDeviceState
    {

        PrjState _deviceMode;
        public PrjState DeviceMode
        {
            get { return _deviceMode; }
            set { _deviceMode = value; }
        }

        RelayState _relaysState;
        public RelayState RelaysState
        {
            get { return _relaysState; }
            set { _relaysState = value; }
        }

        string _data;
        public string Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public UserDeviceState(PrjState mode, string data)
        {
            _deviceMode = mode;
            _data = data;
        }

        public UserDeviceState(RelayState _relayState, string data)
        {
            _relaysState = _relayState;
            _data = data;
        }

    }
}
