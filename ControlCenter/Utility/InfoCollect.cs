using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;

namespace ControlCenter.Utility
{
    class InfoCollect
    {
        public static String GetHardDiskID()
        {
            try
            {
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid=\"c:\"");
                disk.Get();
                return disk.GetPropertyValue("VolumeSerialNumber").ToString();
            }
            catch
            {
                return "";
            }
        }//end
    }
}
