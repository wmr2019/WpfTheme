using System;
using System.Runtime.CompilerServices;

namespace WTLib.Unsafe
{
    public static class UnsafeExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe IntPtr Add<T>(this IntPtr start, int index)
        {
            if (sizeof(IntPtr) == 4)
            {
                uint num = (uint)(index * System.Runtime.CompilerServices.Unsafe.SizeOf<T>());
                return new IntPtr(start.ToInt32() + num);
            }
            ulong num1 = (ulong)index * (ulong)System.Runtime.CompilerServices.Unsafe.SizeOf<T>();
            return new IntPtr(start.ToInt64() + (long)num1);
        }
    }
}
