using System.Runtime.InteropServices;

namespace WTLib.Memory
{
    [StructLayout(LayoutKind.Sequential)]
    public sealed class Pinnable<T>
    {
        public T Data;
    }
}
