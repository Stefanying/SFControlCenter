using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ControlCenter.Utility
{
    public unsafe class EfeeLockDog
    {
        [DllImport("LockDog\\EfeeLicense.dll", EntryPoint = "IsSupportZhongkong", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool IsSupportZhongkong();

        [DllImport("LockDog\\EfeeLicense.dll", EntryPoint = "CheckKeyExist", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool CheckKeyExist();

        public bool CheckDog_IsExist()
        {
            return CheckKeyExist();
        }


        public bool CheckDog()
        {
            //return CheckDog(_lockDog) == 1;
            bool IsCunzai = IsSupportZhongkong();
            return IsSupportZhongkong();
        }


    }
}
