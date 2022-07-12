using System;

namespace WTLib.Collections.Generic
{
    /// <summary>
    /// linked queue for reference type
    /// non thread-safe
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class LinkedQueue<T> where T : LinkedQueueNode<T>
    {
        private LinkedQueueNode<T> _head;

        private LinkedQueueNode<T> _tail;

        public void Enqueue(LinkedQueueNode<T> queueNode)
        {
            if (_tail == null)
            {
                _head = _tail = queueNode;
            }
            else if (queueNode == _tail)
            {
                throw new InvalidOperationException("exist same element.");
            }
            else
            {
                _tail.Next = queueNode;
                _tail = queueNode;
            }
        }

        public T Dequeue()
        {
            if (_head == null)
                throw new InvalidOperationException("queue is empty.");
            var value = System.Runtime.CompilerServices.Unsafe.As<T>(_head);
            _head = _head.Next;
            return value;
        }
    }

    public abstract class LinkedQueueNode<T> where T : class
    {
        internal LinkedQueueNode<T> Next;
    }
}
