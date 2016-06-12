using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace SFLib
{
    public class SFReturnCode : Exception
    {
        private int _code;

        public int Code
        {
            get { return _code; }
        }

        private string _message;

        public override string Message
        {
            get
            {
                return _message;
            }
        }

        public SFReturnCode()
            : base()
        {

        }

        public SFReturnCode(string message)
            : base(message)
        {
            _code = -1;
            _message = message;
        }

        public SFReturnCode(int code, string message)
        {
            _code = code;
            _message = message;
        }

        public SFReturnCode(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public void SetReturnCode(int code, string message)
        {
            _code = code;
            _message = message;
        }

    }

    //枚举功能辅助
    public static class EnumHelper
    {
        public static string GetEnumDescription(object enumSubitem)
        {
            enumSubitem = (Enum)enumSubitem;
            string strValue = enumSubitem.ToString();

            FieldInfo fieldinfo = enumSubitem.GetType().GetField(strValue);

            if (fieldinfo != null)
            {

                Object[] objs = fieldinfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (objs == null || objs.Length == 0)
                {
                    return strValue;
                }
                else
                {
                    DescriptionAttribute da = (DescriptionAttribute)objs[0];
                    return da.Description;
                }
            }
            else
            {
                return "";
            }
        }
    }
}
