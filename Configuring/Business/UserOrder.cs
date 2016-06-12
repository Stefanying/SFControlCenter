using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Configuring.Business
{
    public class UserOrder
    {
        int _hour;

        public int Hour
        {
            get { return _hour; }
            set { _hour = value; }
        }

        int _minute;
        public int Minute
        {
            get { return _minute; }
            set { _minute = value; }
        }

        List<UserOperation> _operation;

        public List<UserOperation> Operations
        {
            get { return _operation; }
            set { _operation = value; }
        }

        public UserOrder(int hour, int minute)
        {
            _hour = hour;
            _minute = minute;
            _operation = new List<UserOperation>();
        }

        public void SetValue(string value)
        {
            try
            {
                _hour = int.Parse(value.Split(':')[0]);
                _minute = int.Parse(value.Split(':')[1]);
            }
            catch
            {
                _hour = 12;
                _minute = 0;
            }
        }

        public string GetTime()
        {
            return string.Format("{0:d2}", _hour) +":"+ string.Format("{0:d2}", _minute);
        }
    }
}
