using System.Runtime.CompilerServices;

namespace WTLib.Memory
{
    public static class StringMemoryExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringMemory AsStringMemory(this string text)
        {
            return new StringMemory(text);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringMemory AsStringMemory(this char[] text)
        {
            return new StringMemory(text);
        }
    }
}

