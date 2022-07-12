using System;
using System.Runtime.CompilerServices;
using WTLib.Memory;

namespace WTLib.Unsafe
{
    public static class UnsafeHelper
    {
        public static readonly IntPtr CharArrayAdjustment = MeasureArrayAdjustment<char>();
        public static readonly IntPtr ByteArrayAdjustment = MeasureArrayAdjustment<byte>();
        public static readonly IntPtr StringAdjustment = MeasureStringAdjustment();
        public static readonly int PointerSize = System.Runtime.CompilerServices.Unsafe.SizeOf<IntPtr>();

        private static IntPtr MeasureArrayAdjustment<T>() where T : struct
        {
            T[] objArray = new T[1];
            return System.Runtime.CompilerServices.Unsafe.ByteOffset<T>(ref System.Runtime.CompilerServices.Unsafe.As<Pinnable<T>>((object)objArray).Data, ref objArray[0]);
        }

        private static unsafe IntPtr MeasureStringAdjustment()
        {
            string str = "a";
            fixed (char* chPtr = str)
                return System.Runtime.CompilerServices.Unsafe.ByteOffset<char>(ref System.Runtime.CompilerServices.Unsafe.As<Pinnable<char>>((object)str).Data, ref System.Runtime.CompilerServices.Unsafe.AsRef<char>((void*)chPtr));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void WriteRefTypeField<T, TV>(T instance, TV v, int offset)
        {
            //PointerSize for methodTable
            //void* ptr = Unsafe.Add<byte>(*(void**)Unsafe.AsPointer(ref instance), PointerSize + offset);
            System.Runtime.CompilerServices.Unsafe.Write(System.Runtime.CompilerServices.Unsafe.Add<byte>(*(void**)System.Runtime.CompilerServices.Unsafe.AsPointer(ref instance), PointerSize + offset), v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe TV ReadRefTypeField<T, TV>(T instance, int offset)
        {
            //PointerSize for methodTable
            //void* ptr = Unsafe.Add<byte>(*(void**)Unsafe.AsPointer(ref instance), PointerSize + offset);
            return System.Runtime.CompilerServices.Unsafe.Read<TV>(System.Runtime.CompilerServices.Unsafe.Add<byte>(*(void**)System.Runtime.CompilerServices.Unsafe.AsPointer(ref instance), PointerSize + offset));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void CopyRefTypeField<T, TV>(T source, TV target, int sourceOffset, int targetOffset, uint byteCount)
        {
            //PointerSize for methodTable
            System.Runtime.CompilerServices.Unsafe.CopyBlock(System.Runtime.CompilerServices.Unsafe.Add<byte>(*(void**)System.Runtime.CompilerServices.Unsafe.AsPointer(ref target), PointerSize + targetOffset),
                System.Runtime.CompilerServices.Unsafe.Add<byte>(*(void**)System.Runtime.CompilerServices.Unsafe.AsPointer(ref source), PointerSize + sourceOffset), byteCount);
        }
    }
}
