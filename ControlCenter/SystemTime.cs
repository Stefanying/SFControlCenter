using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ControlCenter
{
    [StructLayout(LayoutKind.Sequential)]
    struct SYSTEMTIME 
    {
        public ushort wYear;
        public ushort wMonth; 
        public ushort wDayOfWeek; 
        public ushort wDay; 
        public ushort wHour; 
        public ushort wMinute; 
        public ushort wSecond; 
        public ushort wMilliseconds; 
    }

    class SystemTime
    {
        [DllImport("Kernel32.dll")]
        public static extern bool SetLocalTime(ref SYSTEMTIME sysTime);
        [DllImport("Kernel32.dll")]
        public static extern void GetLocalTime(ref SYSTEMTIME sysTime);

        public static string  GetTime()
        {
            // with the defined structure.
            SYSTEMTIME stime = new SYSTEMTIME();
            GetLocalTime(ref stime);
         
            return stime.wHour.ToString() + ":" + stime.wMinute.ToString();
        }

        public static string SetTime(ushort hour, ushort minute)
        {
            try
            {
                SYSTEMTIME systime = new SYSTEMTIME();
                GetLocalTime(ref systime);

                systime.wHour = hour;
                systime.wMinute = minute;
                SetLocalTime(ref systime);
                return "sucess";
            }
            catch
            {
                return "failed";
            }
        }

    }
}
