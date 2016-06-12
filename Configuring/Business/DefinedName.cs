using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Configuring.Business
{
   public  class DefinedName
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

        public DefinedName(string name)
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
