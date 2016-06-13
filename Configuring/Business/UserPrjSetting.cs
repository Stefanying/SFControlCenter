using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Configuring.Business
{
  public  class UserPrjSetting
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private ComSetting _pcs;
        public ComSetting Pcs
        {
            get { return _pcs; }
            set { _pcs = value; }
        }

        List<UserDeviceState> _deviceStates;
        public List<UserDeviceState> DeviceStates
        {
            get { return _deviceStates; }
            set { _deviceStates = value; }
        }


        public UserPrjSetting(string name, ComSetting pcs)
        {
            _name = name;
            _pcs = pcs;
            _deviceStates = new List<UserDeviceState>();
        }

        public void AddPrjSetting(UserDeviceState _uds)
        {
            _deviceStates.Add(_uds);
        }

        public void Remove(UserDeviceState _uds)
        {
            _deviceStates.Remove(_uds);
        }
    }
}
