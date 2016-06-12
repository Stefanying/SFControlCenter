using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Configuring.Business
{
    public class RelaySetting
    {
        //路数
        int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private ComSetting _relayCom;
        public ComSetting RelayCom
        {
            get { return _relayCom; }
            set { _relayCom = value; }
        }
        
        List<UserDeviceState> _relayStates;
        public List<UserDeviceState> RelayStates
        {
            get { return _relayStates; }
            set { _relayStates = value; }
        }

        //public RelaySetting(int id,ComSetting _cs)
        //{
        //    _id = id;
        //    _relayCom = _cs;
        //    _relayStates = new List<UserDeviceState>();
        //}

        public RelaySetting(int id)
        {
            _id = id;
            _relayStates = new List<UserDeviceState>();
        }

        public void AddRelay(UserDeviceState uds)
        {
            _relayStates.Add(uds);
        }

        public void Remove(UserDeviceState uds)
        {
            _relayStates.Remove(uds);
        }


    }
}
