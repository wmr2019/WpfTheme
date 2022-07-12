namespace WTLib.Collections.Specialized
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using WTLib.Threading;

    /// <summary>
    /// one thread dequeue
    /// mulitiple thread enqueue
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public sealed class OneReadQueue<T>
    {
        private readonly AdaptiveSynchronize _euqueueSynchronize = new AdaptiveSynchronize();
        private volatile OneReadQueue<T>.Segment _head;
        private volatile OneReadQueue<T>.Segment _tail;
        private const int SegmentSize = 32;
        private readonly HashSet<string> _received = new HashSet<string>();

        public long Count => (_tail.Index - _head.Index) * SegmentSize + _tail.High - _head.Low;

        public OneReadQueue()
        {
            _head = _tail = new OneReadQueue<T>.Segment(0L, this);
        }

        public void Enqueue(T item)
        {
            _euqueueSynchronize.Enter();
            _tail.Append(item);
            _euqueueSynchronize.Exit();
        }

        public void Enqueue(string id, T item)
        {
            _euqueueSynchronize.Enter();
            if (!_received.Contains(id))
            {
                _received.Add(id);
                _tail.Append(item);
            }
            _euqueueSynchronize.Exit();
        }

        public bool TryDequeue(out T item)
        {
            return _head.TryRemove(out item);
        }

        public void RemoveCache(string id)
        {
            if (string.IsNullOrEmpty(id))
                return;

            _euqueueSynchronize.Enter();
            if (_received.Contains(id))
            {
                _received.Remove(id);
            }
            _euqueueSynchronize.Exit();
        }

        private class Segment
        {
            private volatile T[] _array;
            private volatile OneReadQueue<T>.Segment _next;
            private volatile int _low;
            private volatile int _high;
            private volatile OneReadQueue<T> _source;

            public long Index { get; }

            internal Segment(long index, OneReadQueue<T> source)
            {
                _array = new T[SegmentSize];
                this.Index = index;
                _source = source;
                _low = 0;
                _high = 0;
            }

            internal void Append(T value)
            {
                if (_high < SegmentSize)
                {
                    _array[_high] = value;
                    _high++;
                    if (_high == SegmentSize)
                        Grow();
                }
                void Grow()
                {
                    _next = new OneReadQueue<T>.Segment(this.Index + 1L, _source);
                    _source._tail = _next;
                }
            }

            internal bool TryRemove(out T result)
            {
                if (_low < _high)
                {
                    result = _array[_low];
                    _low++;
                    return true;
                }
                if (_low == SegmentSize)
                {
                    if (_next != null)
                    {
                        _source._head = _next;
                        return _next.TryRemove(out result);
                    }
                }
                result = default;
                return false;
            }

            internal int Low => Math.Min(_low, SegmentSize);
            internal int High => Math.Min(_high, SegmentSize);
        }
    }
}
