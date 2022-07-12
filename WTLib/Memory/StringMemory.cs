using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using WTLib.Unsafe;

namespace WTLib.Memory
{
    [DebuggerDisplay("{ToString()}")]
    public struct StringMemory
    {
        private readonly int _length;
        private readonly IntPtr _byteOffset;
        internal readonly Pinnable<char> _pinnable;
        public int Length => _length;

        internal StringMemory(Pinnable<char> pinnable, IntPtr byteOffset, int length)
        {
            _length = length;
            _pinnable = pinnable;
            _byteOffset = byteOffset;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe StringMemory(char[] array, int start, int length)
        {
            _length = length;
            _pinnable = System.Runtime.CompilerServices.Unsafe.As<Pinnable<char>>(array);
            _byteOffset = UnsafeHelper.CharArrayAdjustment.Add<char>(start);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe StringMemory(char[] array, int start)
        {
            _length = array.Length - start;
            _pinnable = System.Runtime.CompilerServices.Unsafe.As<Pinnable<char>>(array);
            _byteOffset = UnsafeHelper.CharArrayAdjustment.Add<char>(start);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe StringMemory(char[] array)
        {
            _length = array.Length;
            _pinnable = System.Runtime.CompilerServices.Unsafe.As<Pinnable<char>>(array);
            _byteOffset = UnsafeHelper.CharArrayAdjustment;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe StringMemory(string text)
        {
            _length = text.Length;
            _pinnable = System.Runtime.CompilerServices.Unsafe.As<Pinnable<char>>(text);
            _byteOffset = UnsafeHelper.StringAdjustment;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe StringMemory(string text, int start, int length)
        {
            _length = length;
            _pinnable = System.Runtime.CompilerServices.Unsafe.As<Pinnable<char>>(text);
            _byteOffset = UnsafeHelper.StringAdjustment.Add<char>(start);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe StringMemory(string text, int start)
        {
            _length = text.Length - start;
            _pinnable = System.Runtime.CompilerServices.Unsafe.As<Pinnable<char>>(text);
            _byteOffset = UnsafeHelper.StringAdjustment.Add<char>(start);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal unsafe ref char DangerousGetPinnableReference()
        {
            return ref System.Runtime.CompilerServices.Unsafe.AddByteOffset<char>(ref _pinnable.Data, _byteOffset);
        }

        public unsafe ref char this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref System.Runtime.CompilerServices.Unsafe.Add<char>(
                ref System.Runtime.CompilerServices.Unsafe.AddByteOffset<char>(ref _pinnable.Data, _byteOffset), index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public StringMemory Slice(int start)
        {
            return new StringMemory(_pinnable, _byteOffset.Add<char>(start), _length - start);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public StringMemory Slice(int start, int length)
        {
            return new StringMemory(_pinnable, _byteOffset.Add<char>(start), length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(StringMemory left, StringMemory right)
        {
            if (left.Length != right.Length)
                return false;
            else
            {
                ref char lptr = ref left.DangerousGetPinnableReference();
                ref char rptr = ref right.DangerousGetPinnableReference();
                if (System.Runtime.CompilerServices.Unsafe.AreSame<char>(ref lptr, ref rptr))
                    return left.Length == right.Length;
                int length = left._length;
                unsafe
                {
                    //fixed (char* chPtr1 = &lptr)
                    //fixed (char* chPtr2 = &rptr)
                    //{
                    //    var larray = Unsafe.As<ushort[]>(Unsafe.As<char[]>(left._pinnable));
                    //    var rarray = Unsafe.As<ushort[]>(Unsafe.As<char[]>(right._pinnable));
                    //    for (int i = 0; i < length; i = i + Vector<ushort>.Count)
                    //    {
                    //        Vector<ushort> lvector = new Vector<ushort>(larray, i);
                    //        Vector<ushort> rvector = new Vector<ushort>(rarray, i);
                    //        if (!Vector.EqualsAll(lvector, rvector))
                    //            return false;
                    //    }
                    //    return true;
                    //}
                    fixed (char* chPtr1 = &lptr)
                    fixed (char* chPtr2 = &rptr)
                    {
                        char* chPtr3 = chPtr1;
                        char* chPtr4 = chPtr2;
                        for (; length >= 10; length -= 10)
                        {
                            if (*(int*)chPtr3 != *(int*)chPtr4 || *(int*)(chPtr3 + 2) != *(int*)(chPtr4 + 2) ||
                                (*(int*)(chPtr3 + 4) != *(int*)(chPtr4 + 4) ||
                                 *(int*)(chPtr3 + 6) != *(int*)(chPtr4 + 6)) ||
                                *(int*)(chPtr3 + 8) != *(int*)(chPtr4 + 8))
                                return false;
                            chPtr3 += 10;
                            chPtr4 += 10;
                        }
                        for (; length > 0 && *(int*)chPtr3 == *(int*)chPtr4; length -= 2)
                        {
                            chPtr3 += 2;
                            chPtr4 += 2;
                        }
                        return length <= 0;
                    }

                    //fixed (char* chPtr1 = &lptr)
                    //fixed (char* chPtr2 = &rptr)
                    //{
                    //    long* llptr = (long*)chPtr1;
                    //    long* lrptr = (long*)chPtr2;
                    //    int size = sizeof(long) / sizeof(char);
                    //    int index = 0;
                    //    int count = length / size;
                    //    for (; index < count; index++)
                    //    {
                    //        if (*llptr != *lrptr)
                    //            return false;
                    //        llptr++;
                    //        lrptr++;
                    //    }
                    //    int remain = length - (size * index);
                    //    if (remain > 0)
                    //    {
                    //        char* rlptr = (char*)llptr;
                    //        char* rrptr = (char*)lrptr;
                    //        for (int i = 0; i < remain; i++)
                    //        {
                    //            if (*llptr != *lrptr)
                    //                return false;
                    //            rlptr++;
                    //            rrptr++;
                    //        }
                    //    }
                    //}
                    //return true;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(StringMemory left, StringMemory right)
        {
            return !(left == right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(StringMemory left, string right)
        {
            return left == right.AsStringMemory();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(StringMemory left, string right)
        {
            return !(left == right);
        }

        public override unsafe string ToString()
        {
            if (_byteOffset == UnsafeHelper.StringAdjustment &&
                System.Runtime.CompilerServices.Unsafe.As<object>((object)_pinnable) is string str && _length == str.Length)
                return str;
            fixed (char* chPtr = &(this.DangerousGetPinnableReference()))
                return new string(chPtr, 0, _length);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }
}
