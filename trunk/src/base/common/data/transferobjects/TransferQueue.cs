using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Nohros.Data
{
    public class TransferQueue<T> : IEnumerable<T>, ICollection, IEnumerable where T: class
    {
        public delegate void QueueFullEventHandler(T[] array);
        public event QueueFullEventHandler QueueFull;

        #region State object
        private struct EventState
        {
            public QueueFullEventHandler handler;
            public T[] array;
            public EventWaitHandle waithandle;

            public EventState(QueueFullEventHandler handler, T[] array, EventWaitHandle waithandle)
            {
                this.handler = handler;
                this.array = array;
                this.waithandle = waithandle;
            }
        }
        #endregion

        static object _lock;

        private int _size;
        private object _syncRoot;

        protected T[] _array;
        protected int _head;
        protected int _tail;

        #region .ctor
        static TransferQueue()
        {
            _lock = new object();
        }

        /// <summary>
        /// Initializes a new instance_ of the TransferQueue class that is empty and has the specified size.
        /// </summary>
        /// <param name="capacity">The maximun number of elements that the TransferQueue can contain.</param>
        public TransferQueue(int capacity)
        {
            _syncRoot = new object();
            _size = 0;
            _array = new T[capacity];
        }

        /// <summary>
        /// Initializes a new instance_ of the TransferQueue class that contains elements copied from the specified
        /// collection and has specified size.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new TransferQueue</param>
        /// <exception cref="ArgumentNullException">collection is null</exception>
        public TransferQueue(IEnumerable<T> collection, int size): this(size)
        {
            if (collection == null)
                Thrower.ThrowArgumentNullException(ExceptionArgument.collection);

            using (IEnumerator<T> enumerator = collection.GetEnumerator())
            {
                while (enumerator.MoveNext())
                    Enqueue(enumerator.Current);
            }
        }
        #endregion

        /// <summary>
        /// Adds an object to the end of the TransferQueue
        /// </summary>
        /// <param name="item">The object to add to the TransferQueue. The value can be null for references types.</param>
        public void Enqueue(T item)
        {
            // TODO: throw an exception
            //if (_size == _array.Length)
                //Thrower.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_FullQueue);

            _array[_tail] = item;

            if (++_size == _array.Length)
                OnQueueFull(null);
            else
                _tail = (_tail + 1) % _array.Length;
        }

        /// <summary>
        /// Removes and returns the object at the beginning of the TransferQueue.
        /// </summary>
        /// <returns>The object that is removed from the beginning of the TransferQueue.</returns>
        public T Dequeue()
        {
            // TODO: throw an exception
            //if (_size == 0)
                //Thrower.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EmptyQueue);

            T local = _array[_head];
            _array[_head] = null;
            _head = (_head + 1) % _array.Length;
            _size--;
            return local;
        }

        /// <summary>
        /// Returns the object at the beginning of the TransferQueue without removing it.
        /// </summary>
        /// <returns>The object at the beginning og the TransferQueue</returns>
        /// <exception cref="InvalidOperationException">The TransferQueue is empty</exception>
        public T Peek()
        {
            // TODO: throw an exception
            //if (_size == 0)
                //Thrower.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EmptyQueue);
            return _array[_head];
        }

        /// <summary>
        /// Removes all objects from the TransferQueue.
        /// </summary>
        /// <remarks>Count is set to zero, and references to other objects from elements of the collection are
        /// released. This method does not fire any event.</remarks>
        public void Clear()
        {
            for (int i = 0, j = _array.Length; i < j; i++)
                _array[i] = null;

            _head = 0;
            _tail = 0;
            _size = 0;
        }

        /// <summary>
        /// Resets the queue to its initial state.
        /// </summary>
        /// <remarks>Count is set to zero but references to other objects from elements of the collection are not
        /// released(to release references call the <see cref="Clear"/> method.</remarks>
        public void Reset()
        {
            _head = 0;
            _tail = 0;
            _size = 0;
        }

        /// <summary>
        /// Removes all objects from the TransferQueue.
        /// </summary>
        /// <remarks>Count is set to zero, and references to other objects from elements of the collection are
        /// released. This method fire the OnQueueFull event.</remarks>
        public void Flush()
        {
            Flush(null);
        }

        public void Flush(EventWaitHandle waitHandle)
        {
            if (QueueFull != null) {
                // fix the tail: the tail must point to the current
                // node before the OnQueueFull method call.
                _tail = _tail - 1;
                OnQueueFull(waitHandle);
            }
            else
            {
                Clear();
                if (waitHandle != null)
                    waitHandle.Set();
            }
        }

        protected virtual void OnQueueFull(EventWaitHandle waitHandle)
        {
            if (QueueFull != null && _size > 0) {
                T[] array = new T[_size];

                int i = 0, j = 0, num = ((_tail < _head) ? _array.Length : _tail + 1) - _head;
                while (num-- > 0) {
                    j = _head + i;
                    array[i++] = _array[j];
                    _array[j] = null;
                }

                // If the head is above the tail we need to get the
                // elements that exists between the top of the queue and
                // the the tail.
                if (_tail < _head) {
                    // when this method is called the tail points
                    // to the last used slot and not to the next free slot.
                    // So we need to include clean that node too.
                    for (j = 0; j <= _tail; j++) {
                        array[i++] = _array[j];
                        _array[j] = null;
                    }
                }

                _tail = 0;
                _head = 0;
                _size = 0;

                // the event delegate will be executed in another thread
                EventState state = new EventState(QueueFull, array, waitHandle);
                ThreadPool.QueueUserWorkItem(delegate(object o)
                {
                    EventState e = (EventState)o;
                    e.handler(e.array);
                    if (e.waithandle != null)
                        e.waithandle.Set();
                }, state);
            }
            else
                waitHandle.Set();
        }

        #region ICollection
        /// <summary>
        /// Copies the TransferQueue elements to an existing one-dimensional Array, starting at the specified array index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array"/> taht is destination of the elements copied
        /// from TransferQueue. The <see cref="Array"/>must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based indexin array at which copying begins.</param>
        /// <exception cref="ArgumentOutOfRangeException">index is less than zero or index is greater than the length of the array</exception>
        /// <exception cref="ArgumentNullException">array is null</exception>
        /// <exception cref="ArgumentException">The number of elements in the source TransferQueue is greater than the
        /// available space from index to the end of the destination array</exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                Thrower.ThrowArgumentNullException(ExceptionArgument.array);

            int length = array.Length;
            if ((arrayIndex < 0) || (arrayIndex > length))
                Thrower.ThrowArgumentOutOfRangeException(ExceptionArgument.arrayIndex, ExceptionResource.ArgumentOutOfRange_Index);

            int num2 = (length - arrayIndex);
            if (num2 < _size)
                Thrower.ThrowArgumentException(ExceptionResource.Argument_InvalidOfLen);

            // If the head is above tha tail we need to do two copies. The first copy will get
            // the elements between the head and the end of the queue and the second copy will get
            // the elements between the top of the queue and the tail.
            if (_tail < _head)
            {
                int num3 = _array.Length - _head;
                Array.Copy(_array, _head, array, arrayIndex, num3);
                Array.Copy(_array, 0, array, arrayIndex + num3, _tail);
            }
            else
            {
                Array.Copy(_array, _head, array, arrayIndex, _head - _tail);
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null)
                Thrower.ThrowArgumentNullException(ExceptionArgument.array);

            int length = array.Length;
            if ((index < 0) || (index > length))
                Thrower.ThrowArgumentOutOfRangeException(ExceptionArgument.arrayIndex, ExceptionResource.ArgumentOutOfRange_Index);

            int num2 = (length - index);
            if (num2 < _size)
                Thrower.ThrowArgumentException(ExceptionResource.Argument_InvalidOfLen);

            // If the head is above tha tail we need to do two copies. The first copy will get
            // the elements between the head and the end of the queue and the second copy will get
            // the elements between the top of the queue and the tail.
            if (_tail < _head)
            {
                int num3 = _array.Length - _head;
                Array.Copy(_array, _head, array, index, num3);
                Array.Copy(_array, 0, array, index + num3, _tail);
            }
            else
            {
                Array.Copy(_array, _head, array, index, _head - _tail);
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the TransferQueue.
        /// </summary>
        public int Count
        {
            get { return _size; }
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        object ICollection.SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                    System.Threading.Interlocked.CompareExchange(ref _syncRoot, new object(), null);
                return _syncRoot;
            }
        }
        #endregion

        #region IEnumerator

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="IEnumerator&alt;,&gt;"/>that can be used to iterate through the collection</returns>
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0, j = _array.Length; i < j; i++)
                yield return _array[i];
        }
        #endregion

        #region IEnumerable members
        /// <summary>
        /// Returns an enumerator that iterates through a collection
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> object that can be used to iterate
        /// through the collection</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the TransferQueue
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> object that can be used to iterate
        /// through the TransferQueue</returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}