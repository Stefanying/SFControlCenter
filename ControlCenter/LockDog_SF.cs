using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
namespace ControlCenter
{

    unsafe class Memory
    {
        // Handle for the process heap. This handle is used in all calls to the
        // HeapXXX APIs in the methods below.
        static int ph = GetProcessHeap();
        // Private instance constructor to prevent instantiation.
        private Memory() { }
        // Allocates a memory block of the given size. The allocated memory is
        // automatically initialized to zero.
        public static void* Alloc(int size)
        {
            void* result = HeapAlloc(ph, HEAP_ZERO_MEMORY, size);
            if (result == null) throw new OutOfMemoryException();
            return result;
        }
        // Copies count bytes from src to dst. The source and destination
        // blocks are permitted to overlap.
        public static void Copy(void* src, void* dst, int count)
        {
            byte* ps = (byte*)src;
            byte* pd = (byte*)dst;
            if (ps > pd)
            {
                for (; count != 0; count--) *pd++ = *ps++;
            }
            else if (ps < pd)
            {
                for (ps += count, pd += count; count != 0; count--) *--pd = *--ps;
            }
        }
        // Frees a memory block.
        public static void Free(void* block)
        {
            if (!HeapFree(ph, 0, block)) throw new InvalidOperationException();
        }
        // Re-allocates a memory block. If the reallocation request is for a
        // larger size, the additional region of memory is automatically
        // initialized to zero.
        public static void* ReAlloc(void* block, int size)
        {
            void* result = HeapReAlloc(ph, HEAP_ZERO_MEMORY, block, size);
            if (result == null) throw new OutOfMemoryException();
            return result;
        }
        // Returns the size of a memory block.
        public static int SizeOf(void* block)
        {
            int result = HeapSize(ph, 0, block);
            if (result == -1) throw new InvalidOperationException();
            return result;
        }
        // Heap API flags
        const int HEAP_ZERO_MEMORY = 0x00000008;
        // Heap API functions
        [DllImport("kernel32")]
        static extern int GetProcessHeap();
        [DllImport("kernel32")]
        static extern void* HeapAlloc(int hHeap, int flags, int size);
        [DllImport("kernel32")]
        static extern bool HeapFree(int hHeap, int flags, void* block);
        [DllImport("kernel32")]
        static extern void* HeapReAlloc(int hHeap, int flags,
           void* block, int size);
        [DllImport("kernel32")]
        static extern int HeapSize(int hHeap, int flags, void* block);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public unsafe struct __LockDog
    {
        public IntPtr* _vtable;
    }

    public unsafe class SFLockDog : IDisposable
    {
        private __LockDog* _lockDog;

        [DllImport("LockDog\\SfEncryptionModule.dll", EntryPoint = "??0CSfEncryptionModule@@QAE@XZ", CallingConvention = CallingConvention.ThisCall)]
        private static extern void Constructor(__LockDog* ths);

        [DllImport("LockDog\\SfEncryptionModule.dll", EntryPoint = "??1CSfEncryptionModule@@QAE@XZ", CallingConvention = CallingConvention.ThisCall)]
        private static extern void Destructor(__LockDog* ths);

        [DllImport("LockDog\\SfEncryptionModule.dll", EntryPoint = "?Init@CSfEncryptionModule@@QAEHXZ", CallingConvention = CallingConvention.ThisCall)]
        private static extern int Init(__LockDog* ths);

        [DllImport("LockDog\\SfEncryptionModule.dll", EntryPoint = "?CheckDog@CSfEncryptionModule@@QAEHXZ", CallingConvention = CallingConvention.ThisCall)]
        private static extern int CheckDog(__LockDog* ths);

        [DllImport("LockDog\\SfEncryptionModule.dll", EntryPoint = "?CheckPeriod@CSfEncryptionModule@@QAEHXZ", CallingConvention = CallingConvention.ThisCall)]
        private static extern int CheckPeriod(__LockDog* ths);

        [DllImport("LockDog\\SfEncryptionModule.dll", EntryPoint = "?MakeSerialNumber@CSfEncryptionModule@@QAEKXZ", CallingConvention = CallingConvention.ThisCall)]
        private static extern int MakeSerialNumber(__LockDog* ths);

        [DllImport("LockDog\\SfEncryptionModule.dll", EntryPoint = "?HandleActiveCode@CSfEncryptionModule@@QAEHKPBD@Z", CallingConvention = CallingConvention.ThisCall)]
        private static extern int HandleActiveCode(__LockDog* ths, int serialNumber, string pass);

        // ?CheckPassword@CSfEncryptionModule@@QAEHPBD@Z
        [DllImport("LockDog\\SfEncryptionModule.dll", EntryPoint = "?CheckPassword@CSfEncryptionModule@@QAEHPBD@Z", CallingConvention = CallingConvention.ThisCall)]
        private static extern int CheckPassword(__LockDog* ths, string pass);

        public SFLockDog()
        {
            _lockDog = (__LockDog*)Memory.Alloc(sizeof(__LockDog));
            Constructor(_lockDog);
        }

        public void Dispose()
        {
            Destructor(_lockDog);
            Memory.Free(_lockDog);
            _lockDog = null;
        }

        public bool Init()
        {
            return Init(_lockDog) == 1;
        }

        public bool CheckDog()
        {
            return CheckDog(_lockDog) == 1;
        }

        public bool CheckPeriod()
        {
            return CheckPeriod(_lockDog) == 1;
        }

        public int MakeSerialNumber()
        {
            return MakeSerialNumber(_lockDog);
        }

        public bool CheckPassword(string pass)
        {
            return CheckPassword(_lockDog, pass) == 1;
        }

        public bool Active(int serialNumber, string password)
        {
            int ret = HandleActiveCode(_lockDog, serialNumber, password);
            return ret == 1;
        }

    }
}
