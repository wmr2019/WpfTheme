using System.Threading;

namespace WTLib.Threading
{
    /// <summary>
    /// High performance synchronization primitive 
    /// for high frequency synchronization
    /// </summary>
    public sealed class AdaptiveSynchronize
    {
        private const int Idle = 0;
        private const int Busy = 1;
        private int _flag = Idle;
        private SpinWait _spin = new SpinWait();

        public void Enter()
        {
            if (Interlocked.CompareExchange(ref _flag, Busy, Idle) == Idle)
                return;
            do
            {
                _spin.SpinOnce();
            }
            while (Interlocked.CompareExchange(ref _flag, Busy, Idle) != Idle);
        }

        public void Exit()
        {
            _flag = Idle;
        }
    }
}
