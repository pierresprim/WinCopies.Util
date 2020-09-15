/* Copyright © Pierre Sprimont, 2020
 *
 * This file is part of the WinCopies Framework.
 *
 * The WinCopies Framework is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * The WinCopies Framework is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>. */

using System;
using System.Collections;
using System.Threading;

using static WinCopies.Collections.ThrowHelper;

#if WinCopies2
using WinCopies.Util;

using static WinCopies.Util.Util;
#else
using static WinCopies.ThrowHelper;
#endif

namespace WinCopies.Collections
{
    namespace DotNetFix
    {
        public interface ISimpleLinkedListNode
        {
            object Value { get; }

#if WinCopies2
            ISimpleLinkedListNode NextNode { get; }
#else
            ISimpleLinkedListNode Next { get; }
#endif
        }

#if !WinCopies2
        public interface ISimpleLinkedListBase : IUIntCountable
        {
            bool IsReadOnly { get; }

            object SyncRoot { get; }

            bool IsSynchronized { get; }

            void Clear();
        }
#endif

        public interface ISimpleLinkedList :
#if WinCopies2
             IUIntCountable
#else
             ISimpleLinkedListBase
#endif
        {
#if WinCopies2
            bool IsReadOnly { get; }
#else
            bool TryPeek(out object result);
#endif

            object Peek();
        }

        public interface IStack : ISimpleLinkedList
        {
            void Push(object item);

            object Pop();

#if !WinCopies2
            bool TryPop(out object result);
#endif
        }

        public interface IQueue : ISimpleLinkedList
        {
            void Enqueue(object item);

            object Dequeue();

#if !WinCopies2
            bool TryDequeue(out object result);
#endif
        }

        internal static class SimpleLinkedListNodeHelper
        {
         public static InvalidOperationException GetIsClearedException() => new InvalidOperationException("The node is cleared.");
        }

        public  class SimpleLinkedListNode : ISimpleLinkedListNode
        {
            private object _value;
            private SimpleLinkedListNode _next;

            public bool IsCleared { get; private set; }

            public object Value => IsCleared ? throw SimpleLinkedListNodeHelper.GetIsClearedException() :_value;

            // public SimpleLinkedListNode Previous { get; private set; }

            public SimpleLinkedListNode Next { get=>IsCleared?throw SimpleLinkedListNodeHelper.GetIsClearedException():_next; internal set=>_next=IsCleared?throw SimpleLinkedListNodeHelper.GetIsClearedException():value; }
            //{
            //get => _next; internal set =>
            //{
            //if (value == null)
            //{
            //    if (_next != null)

            //        _next.Previous = null;
            //}

            //else

            //    value.Previous = this;

            //_next = value;
            //}
            //}

            ISimpleLinkedListNode ISimpleLinkedListNode.
#if WinCopies2
                NextNode
#else
                Next
#endif
                => Next;

            public SimpleLinkedListNode(object value) => _value = value;

            public void Clear()
            {
                _value = null;

                Next = null;

                IsCleared = true;
            }
        }

        public abstract class SimpleLinkedListBase
#if !WinCopies2
            : ISimpleLinkedListBase
#endif
        {
            private object _syncRoot;

            /// <summary>
            /// Gets a value indicating whether the current list is read-only.
            /// </summary>
            public abstract bool IsReadOnly { get; }

            public abstract uint Count { get; }

            public object SyncRoot
            {
                get
                {
                    if (_syncRoot == null)

                        _syncRoot = Interlocked.CompareExchange(ref _syncRoot, new object(), null);

                    return _syncRoot;
                }
            }

            public bool IsSynchronized => false;

            public void Clear()
            {
                if (IsReadOnly)

                    throw GetReadOnlyListOrCollectionException();

                ClearItems();
            }

            public abstract void ClearItems();
        }

        public abstract class SimpleLinkedList : SimpleLinkedListBase, ISimpleLinkedList
        {
            private uint _count = 0;

            public sealed override bool IsReadOnly => false;

            protected internal SimpleLinkedListNode FirstItem { get; private set; }

            /// <summary>
            /// Gets the number of items in the current list.
            /// </summary>
            public sealed override uint Count => _count;

            /// <summary>
            /// Adds a given item to the current list.
            /// </summary>
            /// <param name="value">The item to add.</param>
            protected void Add(in object value)
            {
                if (IsReadOnly)

                    throw GetReadOnlyListOrCollectionException();

                FirstItem = AddItem(value, out bool actionAfter);

                _count++;

                if (actionAfter)

                    OnItemAdded();
            }

            /// <summary>
            /// When overridden in a derived class, adds a given item to the current list.
            /// </summary>
            /// <param name="value">The item to add.</param>
            protected abstract SimpleLinkedListNode AddItem(object value, out bool actionAfter);

            protected abstract void OnItemAdded();

            private object OnRemove()
            {
                object result = FirstItem.Value;

                FirstItem.Clear();

                FirstItem = RemoveItem();

                _count--;

                return result;
            }

            /// <summary>
            /// Removes the first or last item from the current list, depending on the linked list type (FIFO/LIFO).
            /// </summary>
            protected object Remove()
            {
                if (IsReadOnly)

                    throw GetReadOnlyListOrCollectionException();

                ThrowIfEmpty(this);

                return OnRemove();
            }

            protected bool TryRemove(out object result)
            {
                if (IsReadOnly || Count == 0)
                {
                    result = null;

                    return false;
                }

                result = OnRemove();

                return true;
            }

            /// <summary>
            /// When overridden in a derived class, removes the first or last item from the current list, depending on the linked list type (FIFO/LIFO).
            /// </summary>
            protected abstract SimpleLinkedListNode RemoveItem();

            public sealed override void ClearItems()
            {
                SimpleLinkedListNode node, temp;
                node = FirstItem;

                while (node != null)
                {
                    temp = node.Next;

                    node.Clear();

                    node = temp;
                }

                FirstItem = null;

                _count = 0;
            }

            protected object _Peek() => FirstItem.Value;

            public object Peek() => _count > 0 ? _Peek() : throw GetEmptyListOrCollectionException();

            public bool TryPeek(out object result)
            {
                if (_count > 0)
                {
                    result = _Peek();

                    return true;
                }

                result = null;

                return false;
            }
        }

        public class Stack : SimpleLinkedList, IStack
        {
#if WinCopies2
            public new bool IsReadOnly => base.IsReadOnly;

            public new uint Count => base.Count;

            public new object Peek() => base.Peek();
#endif

            public void Push(object item) => Add(item);

            protected sealed override SimpleLinkedListNode AddItem(object value, out bool actionAfter)
            {
                actionAfter = false;

                return new SimpleLinkedListNode(value) { Next = FirstItem };
            }

            protected sealed override void OnItemAdded()
            {
                // Left empty.
            }

            public object Pop() => Remove();

            public bool TryPop(out object result) => TryRemove(out result);

            protected sealed override SimpleLinkedListNode RemoveItem() => FirstItem.Next;
        }

        public class SynchronizedStack : IStack
        {
            private readonly IStack _stack;

            public bool IsReadOnly => false;

            public bool IsSynchronized => true;

            public object SyncRoot { get; private set; }

            public SynchronizedStack(IStack stack)
            {
                _stack = stack;

                SyncRoot = stack.SyncRoot;
            }

            public uint Count
            {
                get
                {
                    lock (SyncRoot)

                        return _stack.Count;
                }
            }

            public void Clear()
            {
                lock (SyncRoot)

                    _stack.Clear();
            }

            public object Peek()
            {
                lock (SyncRoot)

                    return _stack.Peek();
            }

            public bool TryPeek(out object result)
            {
                lock (SyncRoot)

                    return _stack.TryPeek(out result);
            }

            public void Push(object value)
            {
                lock (SyncRoot)

                    _stack.Push(value);
            }

            public object Pop()
            {
                lock (SyncRoot)

                    return _stack.Pop();
            }

            public bool TryPop(out object result)
            {
                lock (SyncRoot)

                    return _stack.TryPop(out result);
            }
        }

        public class Queue : SimpleLinkedList, IQueue
        {
            private SimpleLinkedListNode _lastItem;

#if WinCopies2
            public new bool IsReadOnly => base.IsReadOnly;

            public new uint Count => base.Count;

            public new object Peek() => base.Peek();
#endif

            public void Enqueue(object item) => Add(item);

            protected sealed override SimpleLinkedListNode AddItem(object value, out bool actionAfter)
            {
                if (FirstItem == null)
                {
                    actionAfter = true;

                    return new SimpleLinkedListNode(value);
                }

                else
                {
                    actionAfter = false;

                    var newNode = new SimpleLinkedListNode(value);

                    _lastItem.Next = newNode;

                    _lastItem = newNode;

                    return FirstItem;
                }
            }

            protected sealed override void OnItemAdded() => _lastItem = FirstItem;

            // public object Peek() => Count > 0 ? FirstItem.Value : throw GetExceptionForEmptyObject();

            public object Dequeue() => Remove();

            public bool TryDequeue(out object result) => TryRemove(out result);

            protected sealed override SimpleLinkedListNode RemoveItem() => FirstItem.Next;
        }

        public class SynchronizedQueue : IQueue
        {
            private readonly IQueue _queue;

            public bool IsReadOnly => false;

            public bool IsSynchronized => true;

            public object SyncRoot { get; private set; }

            public SynchronizedQueue(IQueue queue)
            {
                _queue = queue;

                SyncRoot = queue.SyncRoot;
            }

            public uint Count
            {
                get
                {
                    lock (SyncRoot)

                        return _queue.Count;
                }
            }

            public void Clear()
            {
                lock (SyncRoot)

                    _queue.Clear();
            }

            public object Peek()
            {
                lock (SyncRoot)

                    return _queue.Peek();
            }

            public bool TryPeek(out object result)
            {
                lock (SyncRoot)

                    return _queue.TryPeek(out result);
            }

            public void Enqueue(object value)
            {
                lock (SyncRoot)

                    _queue.Enqueue(value);
            }

            public object Dequeue()
            {
                lock (SyncRoot)

                    return _queue.Dequeue();
            }

            public bool TryDequeue(out object result)
            {
                lock (SyncRoot)

                    return _queue.TryDequeue(out result);
            }
        }

        public abstract class ReadOnlySimpleLinkedListBase
#if !WinCopies2
            : ISimpleLinkedListBase
#endif
        {
            private object _syncRoot;

            public bool IsReadOnly => true;

            public abstract uint Count { get; }

            public object SyncRoot
            {
                get
                {
                    if (_syncRoot == null)

                        _syncRoot = Interlocked.CompareExchange(ref _syncRoot, new object(), null);

                    return _syncRoot;
                }
            }

            public bool IsSynchronized => false;

#if !WinCopies2
            public void Clear() => throw GetReadOnlyListOrCollectionException();
#endif
        }

        public abstract class ReadOnlySimpleLinkedList : ReadOnlySimpleLinkedListBase, ISimpleLinkedList
        {
            public abstract object Peek();

#if !WinCopies2
            public abstract bool TryPeek(out object result);
#endif
        }

        public class ReadOnlyStack : ReadOnlySimpleLinkedList, IStack
        {
            private readonly IStack _stack;

            public sealed override uint Count => _stack.Count;

            public ReadOnlyStack(IStack stack) => _stack = stack;

            public sealed override object Peek() => _stack.Peek();

            void IStack.Push(object item) => throw GetReadOnlyListOrCollectionException();

            object IStack.Pop() => throw GetReadOnlyListOrCollectionException();

#if !WinCopies2
            public sealed override bool TryPeek(out object result) => _stack.TryPeek(out result);

            bool IStack.TryPop(out object result)
            {
                result = null;

                return false;
            }
#endif
        }

        public class ReadOnlyQueue : ReadOnlySimpleLinkedList, IQueue
        {
            private readonly IQueue _queue;

            public sealed override uint Count => _queue.Count;

            public ReadOnlyQueue(IQueue queue) => _queue = queue;

#if WinCopies2
            public ReadOnlyQueue(IStack stack)
            {
                // Left empty.
            }
#endif

            public sealed override object Peek() => _queue.Peek();

            void IQueue.Enqueue(object item) => GetReadOnlyListOrCollectionException();

            object IQueue.Dequeue() => throw GetReadOnlyListOrCollectionException();

#if !WinCopies2
            public sealed override bool TryPeek(out object result) => _queue.TryPeek(out result);

            bool IQueue.TryDequeue(out object result)
            {
                result = null;

                return false;
            }
#endif
        }

        public interface IEnumerableSimpleLinkedListBase : ISimpleLinkedListBase
        {
            // Left empty.
        }

        public interface IEnumerableSimpleLinkedList : ISimpleLinkedList,
#if WinCopies2
            IUIntCountableEnumerable
#else
            IEnumerableSimpleLinkedListBase, IEnumerable
#endif
        {
#if !WinCopies2
            void CopyTo(Array array, int index);

            object[] ToArray();
#endif
        }

        public abstract class EnumerableSimpleLinkedListBase : IEnumerableSimpleLinkedListBase
        {
            [NonSerialized]
            private uint _enumeratorsCount = 0;
            [NonSerialized]
            private uint _enumerableVersion = 0;
            [NonSerialized]
            private object _syncRoot;

            public object SyncRoot
            {
                get
                {
                    if (_syncRoot == null)

                        _syncRoot = Interlocked.CompareExchange(ref _syncRoot, new object(), null);

                    return _syncRoot;
                }
            }

            public bool IsSynchronized => false;

            protected uint EnumerableVersion => _enumerableVersion;

            public abstract uint Count { get; }

            public bool IsReadOnly => false;

            public abstract void Clear();

            protected void UpdateEnumerableVersion()
            {
                if (_enumeratorsCount != 0)

                    _enumerableVersion++;
            }

            protected void IncrementEnumeratorCount() => _enumeratorsCount++;

            protected void DecrementEnumeratorCount()
            {
                if (--_enumeratorsCount == 0)

                    _enumerableVersion = 0;
            }
        }

        public abstract class EnumerableSimpleLinkedList : EnumerableSimpleLinkedListBase, IEnumerableSimpleLinkedList
        {
            public abstract object Peek();

            public abstract bool TryPeek(out object result);

            public abstract IEnumerator GetEnumerator();

            public void CopyTo(Array array, int arrayIndex) =>
#if WinCopies2
                WinCopies.Util.
#endif
                Extensions.CopyTo(this, array, arrayIndex, Count);

            public object[] ToArray()
            {
                if (Count > int.MaxValue)

                    throw new ArgumentOutOfRangeException("Too many items in list or collection.");

                object[] result = new object[Count];

                int i = -1;

                foreach (object value in this)

                    result[++i] = value;

                return result;
            }
        }

        public interface IEnumerableStack : IStack, IEnumerableSimpleLinkedList
        {
            // Left empty.
        }

        [Serializable]
        public class EnumerableStack : EnumerableSimpleLinkedList, IEnumerableStack
        {
            [NonSerialized]
            private readonly Stack _stack;

#if WinCopies2
            public new bool IsReadOnly => base.IsReadOnly;
#endif

            public sealed override uint Count => _stack.Count;

            public EnumerableStack() => _stack = new Stack();

            public sealed override void Clear()
            {
                _stack.Clear();

                UpdateEnumerableVersion();
            }

            public void Push(object item)
            {
                _stack.Push(item);

                UpdateEnumerableVersion();
            }

            public sealed override object Peek() => _stack.Peek();

            public sealed override bool TryPeek(out object result) => _stack.TryPeek(out result);

            public object Pop()
            {
                object result = _stack.Pop();

                UpdateEnumerableVersion();

                return result;
            }

            public bool TryPop(out object result)
            {
                if (_stack.TryPop(out result))
                {
                    UpdateEnumerableVersion();

                    return true;
                }

                return false;
            }

            public sealed override IEnumerator GetEnumerator()
            {
                var enumerator = new Enumerator(this);

                IncrementEnumeratorCount();

                return enumerator;
            }

#if WinCopies2
            [Serializable]
#endif
            public sealed class Enumerator : IEnumerator, WinCopies.
#if WinCopies2
                    Util.
#endif
                    DotNetFix.IDisposable
            {
                private EnumerableStack _stack;
                private ISimpleLinkedListNode _currentNode;
                private readonly uint _version;
                private object _current;

                public object Current => IsDisposed ? throw GetExceptionForDispose(false) : _current;

                public bool IsDisposed { get; private set; }

                public Enumerator(in EnumerableStack stack)
                {
                    _stack = stack;

                    _version = stack.EnumerableVersion;

                    Reset();
                }

                public void Reset()
                {
                    if (IsDisposed)

                        throw GetExceptionForDispose(false);

                    _currentNode = _stack._stack.FirstItem;
                }

                public bool MoveNext()
                {
                    if (IsDisposed)

                        throw GetExceptionForDispose(false);

                    if (_stack.EnumerableVersion != _version)

                        throw new InvalidOperationException("The collection has changed during enumeration.");

                    if (_currentNode == null)

                        return false;

                    _current = _currentNode.Value;

                    _currentNode = _currentNode.
#if WinCopies2
                        NextNode
#else
                        Next
#endif
                        ;

                    return true;
                }

                private void Dispose(bool disposing)
                {
                    if (IsDisposed)

                        return;

                    _stack.DecrementEnumeratorCount();

                    if (disposing)
                    {
                        _current = null;

                        _stack = null;

                        _currentNode = null;
                    }

                    IsDisposed = true;
                }

                public void Dispose() => Dispose(true);

                ~Enumerator() => Dispose(false);
            }
        }

        [Serializable]
        public class ReadOnlyEnumerableStack : ReadOnlySimpleLinkedList, IEnumerableStack
        {
            private readonly IEnumerableStack _stack;

            public sealed override uint Count =>
#if WinCopies2
                ((IUIntCountable)
#endif
                _stack
#if WinCopies2
                )
#endif
                .Count;

            public ReadOnlyEnumerableStack(IEnumerableStack stack) => _stack = stack;

            public sealed override object Peek() => _stack.Peek();

            public void CopyTo(Array array, int arrayIndex) =>
#if WinCopies2
                WinCopies.Util.
#endif
                Extensions.CopyTo(this, array, arrayIndex, Count);

            public object[] ToArray() => _stack.ToArray();

            void IStack.Push(object item) => throw GetReadOnlyListOrCollectionException();

            object IStack.Pop() => throw GetReadOnlyListOrCollectionException();

#if !WinCopies2
            public sealed override bool TryPeek(out object result) => _stack.TryPeek(out result);

            bool IStack.TryPop(out object result)
            {
                result = null;

                return false;
            }
#endif

            public IEnumerator GetEnumerator() => _stack.GetEnumerator();
        }

        public interface IEnumerableQueue : IQueue, IEnumerableSimpleLinkedList
        {
            // Left empty.
        }

        [Serializable]
        public class EnumerableQueue : EnumerableSimpleLinkedList, IEnumerableQueue
        {
            [NonSerialized]
            private readonly Queue _queue;

#if WinCopies2
            public new bool IsReadOnly => base.IsReadOnly;
#endif

            public sealed override uint Count => _queue.Count;

            public EnumerableQueue() => _queue = new Queue();

            public void Enqueue(object item)
            {
                _queue.Enqueue(item);

                UpdateEnumerableVersion();
            }

            public sealed override object Peek() => _queue.Peek();

            public sealed override bool TryPeek(out object result) => _queue.TryPeek(out result);

            public bool TryDequeue(out object result)
            {
                if (_queue.TryDequeue(out result))
                {
                    UpdateEnumerableVersion();

                    return true;
                }

                return false;
            }

            public object Dequeue()
            {
                object result = _queue.Dequeue();

                UpdateEnumerableVersion();

                return result;
            }

            public sealed override IEnumerator GetEnumerator()
            {
                var enumerator = new Enumerator(this);

                IncrementEnumeratorCount();

                return enumerator;
            }

            public sealed override void Clear()
            {
                _queue.Clear();

                UpdateEnumerableVersion();
            }

#if WinCopies2
            [Serializable]
#endif
            public sealed class Enumerator : IEnumerator, WinCopies.
#if WinCopies2
                Util.
#endif
                DotNetFix.IDisposable
            {
                private EnumerableQueue _queue;
                private ISimpleLinkedListNode _currentNode;
                private readonly uint _version;
                private object _current;

                public object Current => IsDisposed ? throw GetExceptionForDispose(false) : _current;

                public bool IsDisposed { get; private set; }

                public Enumerator(in EnumerableQueue queue)
                {
                    _queue = queue;

                    _version = queue.EnumerableVersion;

                    Reset();
                }

                public void Reset()
                {
                    if (IsDisposed)

                        throw GetExceptionForDispose(false);

                    _currentNode = _queue._queue.FirstItem;
                }

                public bool MoveNext()
                {
                    if (IsDisposed)

                        throw GetExceptionForDispose(false);

                    if (_queue.EnumerableVersion != _version)

                        throw new InvalidOperationException("The collection has changed during enumeration.");

                    if (_currentNode == null)

                        return false;

                    _current = _currentNode.Value;

                    _currentNode = _currentNode.
#if WinCopies2
                        NextNode
#else
                        Next
#endif
                        ;

                    return true;
                }

                private void Dispose(bool disposing)
                {
                    if (IsDisposed)

                        return;

                    _queue.DecrementEnumeratorCount();

                    if (disposing)
                    {
                        _current = null;

                        _queue = null;

                        _currentNode = null;
                    }

                    IsDisposed = true;
                }

                public void Dispose() => Dispose(true);

                ~Enumerator() => Dispose(false);
            }
        }

        [Serializable]
        public class ReadOnlyEnumerableQueue : ReadOnlySimpleLinkedList,
#if WinCopies2
            IEnumerableStack,
#endif
            IEnumerableQueue
        {
            private readonly IEnumerableQueue _queue;

            public sealed override uint Count =>
#if WinCopies2
                ((IUIntCountable)
#endif
                _queue
#if WinCopies2
                )
#endif
                .Count;

            public ReadOnlyEnumerableQueue(IEnumerableQueue queue) => _queue = queue;

            public sealed override object Peek() => _queue.Peek();

            public void CopyTo(Array array, int arrayIndex) =>
#if WinCopies2
                WinCopies.Util.
#endif
                Extensions.CopyTo(this, array, arrayIndex, Count);

            public object[] ToArray() => _queue.ToArray();

#if WinCopies2
            void IStack.Push(object item) => throw GetReadOnlyListOrCollectionException();

            object IStack.Pop() => throw GetReadOnlyListOrCollectionException();
#endif

            void IQueue.Enqueue(object item) => throw GetReadOnlyListOrCollectionException();

            object IQueue.Dequeue() => throw GetReadOnlyListOrCollectionException();

#if !WinCopies2
            public sealed override bool TryPeek(out object result) => TryPeek(out result);

            bool IQueue.TryDequeue(out object result)
            {
                result = null;

                return false;
            }
#endif

            public IEnumerator GetEnumerator() => _queue.GetEnumerator();
        }
    }
}

namespace WinCopies.Collections.DotNetFix
{

    [Serializable]
    public class QueueCollection : IEnumerable, ICollection
#if WinCopies2
        , ICloneable
#endif
    {
        protected internal
#if WinCopies2
            System.Collections.Queue
#else
IEnumerableQueue
#endif
            InnerQueue

        { get; }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="QueueCollection"/>.
        /// </summary>
        /// <value>The number of elements contained in the <see cref="QueueCollection"/>.</value>
        public
#if WinCopies2
int
#else
            uint
#endif
            Count => InnerQueue.Count;

#if !WinCopies2
        int ICollection.Count => (int)Count;
#endif

        public bool IsReadOnly => false;

        bool ICollection.IsSynchronized => ((ICollection)InnerQueue).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)InnerQueue).SyncRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueCollection"/> class.
        /// </summary>
        public QueueCollection() : this(new
#if WinCopies2
            System.Collections.Queue
#else
            EnumerableQueue
#endif
            ())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueCollection"/> class with a custom queue.
        /// </summary>
        /// <param name="queue">The inner queue for this <see cref="QueueCollection"/>.</param>
        public QueueCollection(in
#if WinCopies2
            System.Collections.Queue
#else
            IEnumerableQueue
#endif
            queue) => InnerQueue = queue;

#if WinCopies2

        /// <summary>
        /// Creates a shallow copy of the <see cref="QueueCollection"/>.
        /// </summary>
        /// <returns>A shallow copy of the <see cref="QueueCollection"/>.</returns>
        public object Clone() => InnerQueue.Clone();

#endif

        /// <summary>
        /// Removes all objects from the <see cref="QueueCollection"/>. Override this method to provide a custom implementation.
        /// </summary>
        protected virtual void ClearItems() => InnerQueue.Clear();

        /// <summary>
        /// Removes all objects from the <see cref="QueueCollection"/>. Override the <see cref="ClearItems"/> method to provide a custom implementation.
        /// </summary>
        public void Clear() => ClearItems();

        /// <summary>
        /// Determines whether an element is in the <see cref="QueueCollection"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="QueueCollection"/>. The value can be <see langword="null"/> for reference types.</param>
        /// <returns><see langword="true"/> if <paramref name="item"/> is found in <see cref="InnerQueue"/>; otherwise, <see langword="false"/>.</returns>
        public bool Contains(object item) => InnerQueue.Contains(item);

        /// <summary>
        /// Copies the <see cref="QueueCollection"/> elements to an existing one-dimensional <see cref="System.Array"/>, starting at the specified array index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="System.Array"/> that is the destination of the elements copied from <see cref="QueueCollection"/>. The <see cref="System.Array"/> must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in array at which copying begins.</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than zero.</exception>
        /// <exception cref="ArgumentException"><paramref name="array"/> is multidimensional. -or- The number of elements in the source <see cref="QueueCollection"/> is greater than the available space from index to the end of the destination array.</exception>
        /// <exception cref="ArrayTypeMismatchException">The type of the source <see cref="QueueCollection"/> cannot be cast automatically to the type of the destination array.</exception>
        public void CopyTo(Array array, int index) => InnerQueue.CopyTo(array, index);

        /// <summary>
        /// Removes and returns the object at the beginning of the <see cref="QueueCollection"/>. Override this method to provide a custom implementation.
        /// </summary>
        /// <returns>The object that is removed from the beginning of the <see cref="QueueCollection"/>.</returns>
        /// <exception cref="InvalidOperationException">The <see cref="QueueCollection"/> is empty.</exception>
        protected virtual object DequeueItem() => InnerQueue.Dequeue();

        /// <summary>
        /// Removes and returns the object at the beginning of the <see cref="QueueCollection"/>. Override the <see cref="DequeueItem"/> to provide a custom implementation.
        /// </summary>
        /// <returns>The object that is removed from the beginning of the <see cref="QueueCollection"/>.</returns>
        /// <exception cref="InvalidOperationException">The <see cref="QueueCollection"/> is empty.</exception>
        public object Dequeue() => DequeueItem();

        /// <summary>
        /// Adds an object to the end of the <see cref="QueueCollection"/>. Override this method to provide a custom implementation.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="QueueCollection"/>. The value can be <see langword="null"/> for reference types.</param>
        protected virtual void EnqueueItem(in object item) => InnerQueue.Enqueue(item);

        /// <summary>
        /// Adds an object to the end of the <see cref="QueueCollection"/>. Override the <see cref="EnqueueItem(in object)"/> method to provide a custom implementation.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="QueueCollection"/>. The value can be <see langword="null"/> for reference types.</param>
        public void Enqueue(in object item) => EnqueueItem(item);

        /// <summary>
        /// Returns the object at the beginning of the <see cref="QueueCollection"/> without removing it.
        /// </summary>
        /// <returns>The object at the beginning of the <see cref="QueueCollection"/>.</returns>
        /// <exception cref="InvalidOperationException">The <see cref="QueueCollection"/> is empty.</exception>
        public object Peek() => InnerQueue.Peek();

        /// <summary>
        /// Copies the <see cref="QueueCollection"/> elements to a new array.
        /// </summary>
        /// <returns>A new array containing elements copied from the <see cref="QueueCollection"/>.</returns>
        public object[] ToArray() => InnerQueue.ToArray();

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="QueueCollection"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> for the <see cref="QueueCollection"/>.</returns>
        public IEnumerator GetEnumerator() => InnerQueue.GetEnumerator();
    }

    [Serializable]
    public class StackCollection : IEnumerableStack, ICollection
#if WinCopies2
        , ICloneable
#endif
    {
        protected internal
#if WinCopies2
            System.Collections.Stack
#else
IEnumerableStack
#endif
            InnerStack
        { get; }

        public
#if WinCopies2
int
#else
            uint
#endif
            Count => InnerStack.Count;

#if WinCopies2
        uint IUIntCountable.Count => (uint)Count;

        uint IUIntCountableEnumerable.Count => (uint)Count;
#else
        int ICollection.Count => (int)Count;
#endif

        public bool IsReadOnly => false;

        public bool IsSynchronized => InnerStack.IsSynchronized;

        public object SyncRoot => InnerStack.SyncRoot;

        public StackCollection() : this(new
#if WinCopies2
            System.Collections.Stack
#else
            EnumerableStack
#endif
            ())
        { }

        public StackCollection(in
#if WinCopies2
            System.Collections.Stack
#else
            IEnumerableStack
#endif
            stack) => InnerStack = stack;

#if WinCopies2

        public object Clone() => InnerStack.Clone();

#endif

        protected virtual void ClearItems() => InnerStack.Clear();

        public void Clear() => ClearItems();

        public void Contains(object item) => InnerStack.Contains(item);

        public object Peek() => InnerStack.Peek();

        protected virtual object PopItem() => InnerStack.Pop();

        public object Pop() => PopItem();

        protected virtual void PushItem(object item) => InnerStack.Push(item);

        public void Push(object item) => PushItem(item);

        public object[] ToArray() => InnerStack.ToArray();

        public void CopyTo(Array array, int index) => InnerStack.CopyTo(array, index);

        public IEnumerator GetEnumerator() => InnerStack.GetEnumerator();

        public bool TryPeek(out object result) => InnerStack.TryPeek(out result);

        public bool TryPop(out object result) => InnerStack.TryPop(out result);
    }

    [Serializable]
    public class ReadOnlyQueueCollection : IEnumerable, ICollection
#if WinCopies2
        , ICloneable
#endif
    {
        protected
#if WinCopies2
            System.Collections.Queue
#else
IEnumerableQueue
#endif
            InnerQueue

        { get; }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="QueueCollection"/>.
        /// </summary>
        /// <value>The number of elements contained in the <see cref="QueueCollection"/>.</value>
        public
#if WinCopies2
int
#else
            uint
#endif
            Count => InnerQueue.Count;

#if !WinCopies2
        int ICollection.Count => (int)Count;
#endif

        public bool IsReadOnly => true;

        bool ICollection.IsSynchronized => ((ICollection)InnerQueue).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)InnerQueue).SyncRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueCollection"/> class with a custom queue.
        /// </summary>
        /// <param name="queue">The inner queue for this <see cref="QueueCollection"/>.</param>
        public ReadOnlyQueueCollection(in
#if WinCopies2
            System.Collections.Queue
#else
            IEnumerableQueue
#endif
            queue) => InnerQueue = queue;

        public ReadOnlyQueueCollection(in QueueCollection queueCollection) : this(queueCollection.InnerQueue) { }

#if WinCopies2
        /// <summary>
        /// Creates a shallow copy of the <see cref="QueueCollection"/>.
        /// </summary>
        /// <returns>A shallow copy of the <see cref="QueueCollection"/>.</returns>
        public object Clone() => InnerQueue.Clone();
#endif

        /// <summary>
        /// Determines whether an element is in the <see cref="QueueCollection"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="QueueCollection"/>. The value can be <see langword="null"/> for reference types.</param>
        /// <returns><see langword="true"/> if <paramref name="item"/> is found in the <see cref="System.Collections.Queue"/>; otherwise, <see langword="false"/>.</returns>
        public bool Contains(object item) => InnerQueue.Contains(item);

        /// <summary>
        /// Copies the <see cref="QueueCollection"/> elements to an existing one-dimensional <see cref="System.Array"/>, starting at the specified array index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="System.Array"/> that is the destination of the elements copied from <see cref="QueueCollection"/>. The <see cref="System.Array"/> must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in array at which copying begins.</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than zero.</exception>
        /// <exception cref="ArgumentException"><paramref name="array"/> is multidimensional. -or- The number of elements in the source <see cref="QueueCollection"/> is greater than the available space from index to the end of the destination array.</exception>
        /// <exception cref="ArrayTypeMismatchException">The type of the source <see cref="QueueCollection"/> cannot be cast automatically to the type of the destination array.</exception>
        public void CopyTo(Array array, int index) => InnerQueue.CopyTo(array, index);

        /// <summary>
        /// Returns the object at the beginning of the <see cref="QueueCollection"/> without removing it.
        /// </summary>
        /// <returns>The object at the beginning of the <see cref="QueueCollection"/>.</returns>
        /// <exception cref="InvalidOperationException">The <see cref="QueueCollection"/> is empty.</exception>
        public object Peek() => InnerQueue.Peek();

        /// <summary>
        /// Copies the <see cref="QueueCollection"/> elements to a new array.
        /// </summary>
        /// <returns>A new array containing elements copied from the <see cref="QueueCollection"/>.</returns>
        public object[] ToArray() => InnerQueue.ToArray();

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="QueueCollection"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> for the <see cref="QueueCollection"/>.</returns>
        public IEnumerator GetEnumerator() => InnerQueue.GetEnumerator();
    }

    [Serializable]
    public class ReadOnlyStackCollection : IEnumerableStack, ICollection
#if WinCopies2
        , ICloneable
#endif
    {
        protected
#if WinCopies2
            System.Collections.Stack
#else
IEnumerableStack
#endif
            InnerStack
        { get; }

        public
#if WinCopies2

            int
#else
            uint
#endif
            Count => InnerStack.Count;

#if WinCopies2
        uint IUIntCountable.Count => (uint)Count;

        uint IUIntCountableEnumerable.Count => (uint)Count;
#else
        int ICollection.Count => (int)Count;
#endif

        public bool IsReadOnly => true;

        public bool IsSynchronized => InnerStack.IsSynchronized;

        public object SyncRoot => InnerStack.SyncRoot;

        public ReadOnlyStackCollection(in
#if WinCopies2
            System.Collections.Stack
#else
            IEnumerableStack
#endif
            stack) => InnerStack = stack;

        public ReadOnlyStackCollection(in StackCollection stackCollection) : this(stackCollection.InnerStack) { }

#if WinCopies2
        public object Clone() => InnerStack.Clone();
#endif

        public void Contains(object item) => InnerStack.Contains(item);

        public object Peek() => InnerStack.Peek();

        bool ISimpleLinkedList.TryPeek(out object result) => InnerStack.trypeek(out result);

        public object[] ToArray() => InnerStack.ToArray();

        public void CopyTo(Array array, int index) => InnerStack.CopyTo(array, index);

        public IEnumerator GetEnumerator() => InnerStack.GetEnumerator();

        void IStack.Push(object item) => throw GetReadOnlyListOrCollectionException();

        object IStack.Pop() => throw GetReadOnlyListOrCollectionException();

#if !WinCopies2
        bool IStack.TryPop(out object result)
        {
            result = null;

            return false;
        }

        void ISimpleLinkedListBase.Clear() => throw GetReadOnlyListOrCollectionException();
#endif
    }
}
