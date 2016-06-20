using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Configuring.Business
{

    //投影机状态
    public enum PrjOperationType
    {
        开,
        关
    }


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

        List<UserPrjOperation> _deviceStates;
        public List<UserPrjOperation> DeviceStates
        {
            get { return _deviceStates; }
            set { _deviceStates = value; }
        }


        public UserPrjSetting(string name, ComSetting pcs)
        {
            _name = name;
            _pcs = pcs;
            _deviceStates = new List<UserPrjOperation>();
        }

        public void AddPrjSetting(UserPrjOperation _uds)
        {
            _deviceStates.Add(_uds);
        }

        public void Remove(UserPrjOperation _uds)
        {
            _deviceStates.Remove(_uds);
        }
    }

   public class UserPrjOperation
    {

        PrjOperationType _prjOperationType;
        public PrjOperationType PrjOperationType
        {
            get { return _prjOperationType; }
            set { _prjOperationType = value; }
        }

       //数据
        string _data;
        public string Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public UserPrjOperation(PrjOperationType p_type, string data)
        {
            _prjOperationType = p_type;
            _data = data;
        }
    }
}
