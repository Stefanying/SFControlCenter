using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Configuring.Business
{
    //自定义命令
    public class UserDeviceName
    {
        string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        List<ComSetting> _deviceComs;
        public List<ComSetting> DeviceComs
        {
            get { return _deviceComs; }
            set { _deviceComs = value; }
        }

        List<UserDeviceState> _deviceStates;
        public List<UserDeviceState> DeviceStates
        {
            get { return _deviceStates; }
            set { _deviceStates = value; }
        }


        public UserDeviceName(string name)
        {
            _name = name;
            _deviceComs = new List<ComSetting>();
            _deviceStates = new List<UserDeviceState>();
        }

        public void AddAction(ComSetting _udc, UserDeviceState _uds)
        {
            _deviceComs.Add(_udc);
            _deviceStates.Add(_uds);
        }

        public void Remove(ComSetting _udc, UserDeviceState _uds)
        {
            _deviceComs.Remove(_udc);
            _deviceStates.Remove(_uds);
        }

    }
}
