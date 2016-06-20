using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Configuring.Business
{
    //自定义命令操作
   public  class UserDefinedOperation
    {
        string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }


        List<UserOperation> _operations;
        public List<UserOperation> Operations
        {
            get { return _operations; }
            set { _operations = value; }
        }

        public UserDefinedOperation(string name)
        {
            _name = name;
            _operations = new List<UserOperation>();
        }


        public void AddOperation(UserOperation op)
        {
            _operations.Add(op);
        }

        public void RemoveOperation(UserOperation op)
        {
            _operations.Remove(op);
        }
    }
}
