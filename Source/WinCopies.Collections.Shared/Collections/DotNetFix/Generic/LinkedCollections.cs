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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

using static WinCopies
#if WinCopies2
    .Util.Util;
using static WinCopies.Util.ThrowHelper;
#else
    .ThrowHelper;
#endif

using static WinCopies.Collections.ThrowHelper;

namespace WinCopies.Collections.DotNetFix.Generic
{
    public interface ISimpleLinkedListNode<T>
#if !WinCopies2
        : ISimpleLinkedListNode
#endif
    {
        T Value { get; }

#if WinCopies2
            ISimpleLinkedListNode<T> NextNode { get; }
#else
        ISimpleLinkedListNode<T> Next { get; }
#endif
    }

    public interface ISimpleLinkedList<T> :
#if WinCopies2
        IUIntCountable
#else
        ISimpleLinkedList
#endif
    {
        T Peek();

#if !WinCopies2
        bool TryPeek(out T result);
#endif
    }

    public interface IStack<T> : ISimpleLinkedList<T>, IStack
    {
        void Push(T item);

        T Pop();

#if !WinCopies2
        bool TryPop(out T result);
#endif
    }

    public interface IQueue<T> : ISimpleLinkedList<T>, IQueue
    {
        void Enqueue(T item);

        T Dequeue();

#if !WinCopies2
        bool TryDequeue(out T result);
#endif
    }

    //public interface ILinkedListNode<T>
    //{
    //    ILinkedList<T> List { get; }

    //    ILinkedListNode<T> Next { get; }

    //    ILinkedListNode<T> Previous { get; }

    //    T Value { get; set; }
    //}

    //public interface ILinkedList<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection, IDeserializationCallback, ISerializable
    //{
    //    ILinkedListNode<T> Last { get; }

    //    ILinkedListNode<T> First { get; }

    //    int Count { get; }

    //    void AddAfter(ILinkedListNode<T> node, ILinkedListNode<T> newNode);

    //    ILinkedListNode<T> AddAfter(ILinkedListNode<T> node, T value);

    //    void AddBefore(ILinkedListNode<T> node, ILinkedListNode<T> newNode);

    //    ILinkedListNode<T> AddBefore(ILinkedListNode<T> node, T value);

    //    void AddFirst(ILinkedListNode<T> node);

    //    ILinkedListNode<T> AddFirst(T value);

    //    void AddLast(ILinkedListNode<T> node);

    //    ILinkedListNode<T> AddLast(T value);

    //    void Clear();

    //    bool Contains(T value);

    //    void CopyTo(T[] array, int index);

    //    ILinkedListNode<T> Find(T value);

    //    ILinkedListNode<T> FindLast(T value);

    //    void Remove(ILinkedListNode<T> node);

    //    bool Remove(T value);

    //    void RemoveFirst();

    //    void RemoveLast();
    //}

    public class SimpleLinkedListNode<T> : ISimpleLinkedListNode<T>
    {
        private T _value;
        private SimpleLinkedListNode<T> _next;

        public bool IsCleared { get; private set; }

        public T Value => IsCleared ? throw SimpleLinkedListNodeHelper.GetIsClearedException() : _value;

        public SimpleLinkedListNode<T> Next { get => IsCleared ? throw SimpleLinkedListNodeHelper.GetIsClearedException() : _next; internal set => _next = IsCleared ? throw SimpleLinkedListNodeHelper.GetIsClearedException() : value; }

        ISimpleLinkedListNode<T> ISimpleLinkedListNode<T>.
#if WinCopies2
            NextNode
#else
            Next
#endif
            => Next;

        internal SimpleLinkedListNode(T value) => _value = value;

        public void Clear()
        {
            _value = default;

            _next = null;

            IsCleared = true;
        }

        #region ISimpleLinkedListNode implementation
        object ISimpleLinkedListNode.Value => Value;

        ISimpleLinkedListNode ISimpleLinkedListNode.Next => Next;
        #endregion
    }

    public abstract class SimpleLinkedList<T> : SimpleLinkedListBase, ISimpleLinkedList<T>
    {
        private uint _count = 0;

        public sealed override bool IsReadOnly => false;

        protected internal SimpleLinkedListNode<T> FirstItem { get; private set; }

        public sealed override uint Count => _count;

        protected void Add(in T item)
        {
            if (IsReadOnly)

                throw GetReadOnlyListOrCollectionException();

            FirstItem = AddItem(item, out bool actionAfter);

            _count++;

            if (actionAfter)

                OnItemAdded();
        }

        protected abstract SimpleLinkedListNode<T> AddItem(T item, out bool actionAfter);

        protected abstract void OnItemAdded();

        private T OnRemove()
        {
            T result = FirstItem.Value;

            FirstItem.Clear();

            FirstItem = RemoveItem();

            _count--;

            return result;
        }

        protected T Remove()
        {
            if (IsReadOnly)

                throw GetReadOnlyListOrCollectionException();

            ThrowIfEmpty(this);

            return OnRemove();
        }

        protected bool TryRemove(out T result)
        {
            if (IsReadOnly || Count == 0)
            {
                result = default;

                return false;
            }

            result = OnRemove();

            return true;
        }

        protected abstract SimpleLinkedListNode<T> RemoveItem();

        public sealed override void ClearItems()
        {
            SimpleLinkedListNode<T> node, temp;
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

        protected T _Peek() => FirstItem.Value;

        public T Peek() => _count > 0 ? _Peek() : throw GetEmptyListOrCollectionException();

        public bool TryPeek(out T result)
        {
            if (_count > 0)
            {
                result = _Peek();

                return true;
            }

            result = default;

            return false;
        }

#if !WinCopies2
        #region ISimpleLinkedList implementation
        object ISimpleLinkedList.Peek() => Peek();

        bool ISimpleLinkedList.TryPeek(out object result)
        {
            if (TryPeek(out T _result))
            {
                result = _result;

                return true;
            }

            result = null;

            return false;
        }
        #endregion
#endif
    }

    public class Stack<T> : SimpleLinkedList<T>, IStack<T>
    {
        public void Push(T item) => Add(item);

        protected sealed override SimpleLinkedListNode<T> AddItem(T item, out bool actionAfter)
        {
            actionAfter = false;

            return new SimpleLinkedListNode<T>(item) { Next = FirstItem };
        }

        protected sealed override void OnItemAdded()
        {
            // Left empty.
        }

        public T Pop() => Remove();

        public bool TryPop(out T result) => TryRemove(out result);

        protected sealed override SimpleLinkedListNode<T> RemoveItem() => FirstItem.Next;

#if !WinCopies2
        #region IStack implementation
        void IStack.Push(object item) => Push(item is T _item ? _item : throw new InvalidArgumentException("Type mismatch."));

        object IStack.Pop() => Pop();

        bool IStack.TryPop(out object result)
        {
            if (TryPop(out T _result))
            {
                result = _result;

                return true;
            }

            result = null;

            return false;
        }
        #endregion
#endif
    }

    public class Queue<T> : SimpleLinkedList<T>, IQueue<T>
    {
        private SimpleLinkedListNode<T> _lastItem;

        public void Enqueue(T item) => Add(item);

        protected sealed override SimpleLinkedListNode<T> AddItem(T item, out bool actionAfter)
        {
            if (FirstItem == null)
            {
                actionAfter = true;

                return new SimpleLinkedListNode<T>(item);
            }

            else
            {
                actionAfter = false;

                var newNode = new SimpleLinkedListNode<T>(item);

                _lastItem.Next = newNode;

                _lastItem = newNode;

                return FirstItem;
            }
        }

        protected sealed override void OnItemAdded() => _lastItem = FirstItem;

        public T Dequeue() => Remove();

        public bool TryDequeue(out T result) => TryRemove(out result);

        protected sealed override SimpleLinkedListNode<T> RemoveItem() => FirstItem.Next;

#if !WinCopies2
        #region IQueue implementation
        void IQueue.Enqueue(object item) => Enqueue(item is T _item ? _item : throw new InvalidArgumentException("Type mismatch."));

        object IQueue.Dequeue() => Dequeue();

        bool IQueue.TryDequeue(out object result)
        {
            if (TryDequeue(out T _result))
            {
                result = _result;

                return true;
            }

            result = null;

            return false;
        }
        #endregion
#endif
    }

    public abstract class ReadOnlySimpleLinkedList<T> : ReadOnlySimpleLinkedListBase, ISimpleLinkedList<T>
    {
        public abstract T Peek();

#if !WinCopies2
        public abstract bool TryPeek(out T result);
#endif

        #region ISimpleLinkedList implementation
        object ISimpleLinkedList.Peek() => Peek();

        bool ISimpleLinkedList.TryPeek(out object result)
        {
            if (TryPeek(out T _result))
            {
                result = _result;

                return true;
            }

            result = null;

            return false;
        }
    }

    public class ReadOnlyStack<T> : ReadOnlySimpleLinkedList<T>, IStack<T>
    {
        private readonly IStack<T> _stack;

        public sealed override uint Count => _stack.Count;

        public ReadOnlyStack(IStack<T> stack) => _stack = stack;

        public sealed override T Peek() => _stack.Peek();

        void IStack<T>.Push(T item) => throw GetReadOnlyListOrCollectionException();

        T IStack<T>.Pop() => throw GetReadOnlyListOrCollectionException();

#if !WinCopies2
        public sealed override bool TryPeek(out T result) => _stack.TryPeek(out result);

        bool IStack<T>.TryPop(out T result)
        {
            result = default;

            return false;
        }

        #region IStack implementation
        void IStack.Push(object item) => _stack.Push(item);

        object IStack.Pop() => _stack.Pop();

        bool IStack.TryPop(out object result) => _stack.TryPop(out result);
        #endregion
#endif
    }

    public class ReadOnlyQueue<T> : ReadOnlySimpleLinkedList<T>, IQueue<T>
    {
        private readonly IQueue<T> _queue;

        public sealed override uint Count => _queue.Count;

        public ReadOnlyQueue(IQueue<T> queue) => _queue = queue;

        public sealed override T Peek() => _queue.Peek();

        void IQueue<T>.Enqueue(T item) => GetReadOnlyListOrCollectionException();

        T IQueue<T>.Dequeue() => throw GetReadOnlyListOrCollectionException();

#if !WinCopies2
        public sealed override bool TryPeek(out T result) => _queue.TryPeek(out result);

        bool IQueue<T>.TryDequeue(out T result)
        {
            result = default;

            return false;
        }

        #region IQueue implementation
        void IQueue.Enqueue(object item) => _queue.Enqueue(item);

        object IQueue.Dequeue() => _queue.Dequeue();

        bool IQueue.TryDequeue(out object result) => _queue.TryPop(out result);
        #endregion
#endif
    }

    public interface IEnumerableSimpleLinkedList<T> : ISimpleLinkedList<T>,
#if WinCopies2
        IUIntCountableEnumerable<T>
#else
        IEnumerableSimpleLinkedListBase, IEnumerable<T>
#endif
    {
#if !WinCopies2
        void CopyTo(T[] array, int index);

        T[] ToArray();
#endif
    }

    public abstract class EnumerableSimpleLinkedList<T> : EnumerableSimpleLinkedListBase, IEnumerableSimpleLinkedList<T>
    {
        public abstract T Peek();

        public abstract bool TryPeek(out T result);

        public void CopyTo(T[] array, int arrayIndex) => WinCopies.
#if WinCopies2
                Util.
#else
                Collections.
#endif
                Extensions.CopyTo(this, array, arrayIndex, Count);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public abstract IEnumerator<T> GetEnumerator();

        public T[] ToArray()
        {
            if (Count > int.MaxValue)

                throw new ArgumentOutOfRangeException("Too many items in list or collection.");

            T[] result = new T[Count];

            int i = -1;

            foreach (T value in this)

                result[++i] = value;

            return result;
        }
    }

    public interface IEnumerableStack<T> : IStack<T>, IEnumerableSimpleLinkedList<T>
    {
        // Left empty.
    }

    [Serializable]
    public class EnumerableStack<T> : EnumerableSimpleLinkedList<T>, IEnumerableStack<T>
    {
        [NonSerialized]
        private readonly Stack<T> _stack;

        public sealed override uint Count => _stack.Count;

        public EnumerableStack() => _stack = new Stack<T>();

        public sealed override void Clear() => _stack.Clear();

        public void Push(T item)
        {
            _stack.Push(item);

            UpdateEnumerableVersion();
        }

        public sealed override T Peek() => _stack.Peek();

        public sealed override bool TryPeek(out T result) => _stack.TryPeek(out result);

        public T Pop()
        {
            T result = _stack.Pop();

            UpdateEnumerableVersion();

            return result;
        }

        public bool TryPop(out T result) => _stack.TryPop(out result);

        public sealed override IEnumerator<T> GetEnumerator()
        {
            var enumerator = new Enumerator(this);

            IncrementEnumeratorCount();

            return enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

#if WinCopies2
        [Serializable]
#endif
        public sealed class Enumerator : IEnumerator<T>, WinCopies.
#if WinCopies2
            Util.
#endif
            DotNetFix.IDisposable
        {
            private EnumerableStack<T> _stack;
            private ISimpleLinkedListNode<T> _currentNode;
            private readonly uint _version;
            private T _current;

            public T Current => IsDisposed ? throw GetExceptionForDispose(false) : _current;

            object IEnumerator.Current => Current;

            public bool IsDisposed { get; private set; }

            public Enumerator(in EnumerableStack<T> stack)
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
                    _current = default;

                    _stack = null;

                    _currentNode = null;
                }

                IsDisposed = true;
            }

            public void Dispose() => Dispose(true);

            ~Enumerator() => Dispose(false);
        }
    }

    public interface IEnumerableQueue<T> : IQueue<T>, IEnumerableSimpleLinkedList<T>
    {
        // Left empty.
    }

    [Serializable]
    public class EnumerableQueue<T> : EnumerableSimpleLinkedList<T>, IEnumerableQueue<T>
    {
        [NonSerialized]
        private readonly Queue<T> _queue;

        public sealed override uint Count => _queue.Count;

        public EnumerableQueue() => _queue = new Queue<T>();

        public void Enqueue(T item)
        {
            _queue.Enqueue(item);

            UpdateEnumerableVersion();
        }

        public sealed override T Peek() => _queue.Peek();

        public sealed override bool TryPeek(out T result) => _queue.TryPeek(out result);

        public T Dequeue()
        {
            T result = _queue.Dequeue();

            UpdateEnumerableVersion();

            return result;
        }

        public bool TryDequeue(out T result)
        {
            if (_queue.TryDequeue(out result))
            {
                UpdateEnumerableVersion();

                return true;
            }

            return false;
        }

        public sealed override IEnumerator<T> GetEnumerator()
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

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

#if WinCopies2
        [Serializable]
#endif
        public sealed class Enumerator : IEnumerator<T>, WinCopies.
#if WinCopies2
            Util.
#endif
            DotNetFix.IDisposable
        {
            private EnumerableQueue<T> _queue;
            private ISimpleLinkedListNode<T> _currentNode;
            private readonly uint _version;
            private T _current;

            public T Current => IsDisposed ? throw GetExceptionForDispose(false) : _current;

            object IEnumerator.Current => Current;

            public bool IsDisposed { get; private set; }

            public Enumerator(in EnumerableQueue<T> queue)
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
                    _current = default;

                    _queue = null;

                    _currentNode = null;
                }

                IsDisposed = true;
            }

            public void Dispose() => Dispose(true);

            ~Enumerator() => Dispose(false);
        }
    }
}

namespace WinCopies.Collections.DotNetFix
{
#if !WinCopies2
    namespace Generic
    {
#endif
        namespace Extensions
        {
            [Serializable]
            internal sealed class LinkedListNodeEnumerator<T> : IEnumerator<System.Collections.Generic.LinkedListNode<T>>, IEnumerable<System.Collections.Generic.LinkedListNode<T>>
            {
                private ILinkedList<T> _list;

                public System.Collections.Generic.LinkedListNode<T> Current { get; private set; }

                object IEnumerator.Current => Current;

                public LinkedListNodeEnumerator(ILinkedList<T> list) => _list = list;

                public void Dispose()
                {
                    Current = null;

                    _list = null;
                }

                private bool _first = true;

                public bool MoveNext()
                {
                    if (_list.Count == 0)

                        return false;

                    if (_first)
                    {
                        _first = false;

                        Current = _list.First;

                        return true;
                    }

                    if (Current.Next == null)
                    {
                        Current = null;

                        return false;
                    }

                    Current = Current.Next;

                    return true;
                }

                public void Reset() { }

                public IEnumerator<System.Collections.Generic.LinkedListNode<T>> GetEnumerator() => this;

                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            }

            [Serializable]
            public class LinkedCollection<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection, IDeserializationCallback, ISerializable
            {
                protected ILinkedList<T> InnerList { get; }

                public System.Collections.Generic.LinkedListNode<T> Last => InnerList.Last;

                public System.Collections.Generic.LinkedListNode<T> First => InnerList.First;

                public int Count => InnerList.Count;

                bool ICollection.IsSynchronized => ((ICollection)InnerList).IsSynchronized;

                object ICollection.SyncRoot => ((ICollection)InnerList).SyncRoot;

                bool ICollection<T>.IsReadOnly => false;

                public LinkedCollection() : this(new LinkedList<T>()) { }

                public LinkedCollection(in ILinkedList<T> list) => InnerList = list;

                protected virtual void AddItem(T item) => ((ICollection<T>)InnerList).Add(item);

                void ICollection<T>.Add(T item) => AddItem(item);

                protected virtual void AddItemAfter(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode) => InnerList.AddAfter(node, newNode);

                /// <summary>
                /// Adds the specified new node after the specified existing node in the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddItemAfter(System.Collections.Generic.LinkedListNode{T}, System.Collections.Generic.LinkedListNode{T})"/> method to provide a custom implementation.
                /// </summary>
                /// <param name="node">The <see cref="System.Collections.Generic.LinkedListNode{T}"/> after which to insert <paramref name="newNode"/>.</param>
                /// <param name="newNode">The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> to add to the <see cref="LinkedCollection{T}"/>.</param>
                /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>. -or- <paramref name="newNode"/> is <see langword="null"/>.</exception>
                /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current <see cref="LinkedCollection{T}"/>. -or- <paramref name="newNode"/> belongs to another <see cref="System.Collections.Generic.LinkedList{T}"/>.</exception>
                /// <seealso cref="AddAfter(System.Collections.Generic.LinkedListNode{T}, T)"/>
                public void AddAfter(in System.Collections.Generic.LinkedListNode<T> node, in System.Collections.Generic.LinkedListNode<T> newNode) => AddItemAfter(node, newNode);

                protected virtual System.Collections.Generic.LinkedListNode<T> AddItemAfter(System.Collections.Generic.LinkedListNode<T> node, T value) => InnerList.AddAfter(node, value);

                /// <summary>
                /// Adds a new node containing the specified value after the specified existing node in the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddItemAfter(System.Collections.Generic.LinkedListNode{T}, T)"/> method to provide a custom implementation.
                /// </summary>
                /// <param name="node">The <see cref="System.Collections.Generic.LinkedListNode{T}"/> after which to insert a new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</param>
                /// <param name="value">The value to add to the <see cref="LinkedCollection{T}"/>.</param>
                /// <returns>The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</returns>
                /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>.</exception>
                /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current <see cref="LinkedCollection{T}"/>.</exception>
                /// <seealso cref="AddAfter(in System.Collections.Generic.LinkedListNode{T}, in System.Collections.Generic.LinkedListNode{T})"/>
                public System.Collections.Generic.LinkedListNode<T> AddAfter(System.Collections.Generic.LinkedListNode<T> node, T value) => AddItemAfter(node, value);

                protected virtual void AddItemBefore(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode) => InnerList.AddBefore(node, newNode);

                /// <summary>
                /// Adds the specified new node before the specified existing node in the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddItemBefore(System.Collections.Generic.LinkedListNode{T}, System.Collections.Generic.LinkedListNode{T})"/> method to provide a custom implementation.
                /// </summary>
                /// <param name="node">The <see cref="System.Collections.Generic.LinkedListNode{T}"/> before which to insert <paramref name="newNode"/>.</param>
                /// <param name="newNode">The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> to add to the <see cref="LinkedCollection{T}"/>.</param>
                /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>. -or- <paramref name="newNode"/> is <see langword="null"/>.</exception>
                /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current <see cref="LinkedCollection{T}"/>. -or- <paramref name="newNode"/> belongs to another <see cref="System.Collections.Generic.LinkedList{T}"/>.</exception>
                /// <seealso cref="AddBefore(in System.Collections.Generic.LinkedListNode{T}, in T)"/>
                public void AddBefore(in System.Collections.Generic.LinkedListNode<T> node, in System.Collections.Generic.LinkedListNode<T> newNode) => AddItemBefore(node, newNode);

                protected virtual System.Collections.Generic.LinkedListNode<T> AddItemBefore(System.Collections.Generic.LinkedListNode<T> node, T value) => InnerList.AddBefore(node, value);

                /// <summary>
                /// Adds a new node containing the specified value before the specified existing node in the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddItemBefore(System.Collections.Generic.LinkedListNode{T}, T)"/> method to provide a custom implementation.
                /// </summary>
                /// <param name="node">The <see cref="System.Collections.Generic.LinkedListNode{T}"/> before which to insert a new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</param>
                /// <param name="value">The value to add to the <see cref="LinkedCollection{T}"/>.</param>
                /// <returns>The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</returns>
                /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>.</exception>
                /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current <see cref="LinkedCollection{T}"/>.</exception>
                /// <seealso cref="AddBefore(in System.Collections.Generic.LinkedListNode{T}, in System.Collections.Generic.LinkedListNode{T})"/>
                public System.Collections.Generic.LinkedListNode<T> AddBefore(in System.Collections.Generic.LinkedListNode<T> node, in T value) => AddItemBefore(node, value);

                protected virtual void AddFirstItem(System.Collections.Generic.LinkedListNode<T> node) => InnerList.AddFirst(node);

                /// <summary>
                /// Adds the specified new node at the start of the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddFirstItem(System.Collections.Generic.LinkedListNode{T})"/> method to provide a custom implementation.
                /// </summary>
                /// <param name="node">The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> to add at the start of the <see cref="LinkedCollection{T}"/>.</param>
                /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>.</exception>
                /// <exception cref="InvalidOperationException"><paramref name="node"/> belongs to another <see cref="System.Collections.Generic.LinkedList{T}"/>.</exception>
                /// <seealso cref="AddFirst(in T)"/>
                public void AddFirst(in System.Collections.Generic.LinkedListNode<T> node) => AddFirstItem(node);

                protected virtual System.Collections.Generic.LinkedListNode<T> AddFirstItem(T value) => InnerList.AddFirst(value);

                /// <summary>
                /// Adds a new node containing the specified value at the start of the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddFirstItem(T)"/> method to provide a custom implementation.
                /// </summary>
                /// <param name="value">The value to add at the start of the <see cref="LinkedCollection{T}"/>.</param>
                /// <returns>The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</returns>
                /// <seealso cref="AddFirst(in T)"/>
                public System.Collections.Generic.LinkedListNode<T> AddFirst(in T value) => AddFirstItem(value);

                protected virtual void AddLastItem(System.Collections.Generic.LinkedListNode<T> node) => InnerList.AddLast(node);

                /// <summary>
                /// Adds the specified new node at the end of the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddLastItem(System.Collections.Generic.LinkedListNode{T})"/> method to provide a custom implementation.
                /// </summary>
                /// <param name="node">The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> to add at the end of the <see cref="LinkedCollection{T}"/>.</param>
                /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>.</exception>
                /// <exception cref="InvalidOperationException"><paramref name="node"/> belongs to another <see cref="System.Collections.Generic.LinkedList{T}"/>.</exception>
                /// <seealso cref="AddLast(in T)"/>
                public void AddLast(in System.Collections.Generic.LinkedListNode<T> node) => AddLastItem(node);

                protected virtual System.Collections.Generic.LinkedListNode<T> AddLastItem(T value) => InnerList.AddLast(value);

                /// <summary>
                /// Adds a new node containing the specified value at the end of the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddLastItem(T)"/> method to provide a custom implementation.
                /// </summary>
                /// <param name="value">The value to add at the end of the <see cref="LinkedCollection{T}"/>.</param>
                /// <returns>The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</returns>
                /// <seealso cref="AddLast(in System.Collections.Generic.LinkedListNode{T})"/>
                public System.Collections.Generic.LinkedListNode<T> AddLast(in T value) => AddLastItem(value);

                public System.Collections.Generic.LinkedListNode<T> Find(T value) => InnerList.Find(value);

                public System.Collections.Generic.LinkedListNode<T> FindLast(T value) => InnerList.FindLast(value);

                protected virtual void ClearItems() => InnerList.Clear();

                public void Clear() => ClearItems();

                public bool Contains(T item) => InnerList.Contains(item);

                public void CopyTo(Array array, int index) => ((ICollection)InnerList).CopyTo(array, index);

                public void CopyTo(T[] array, int arrayIndex) => InnerList.CopyTo(array, arrayIndex);

                IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerList).GetEnumerator();

                public void GetObjectData(SerializationInfo info, StreamingContext context) => InnerList.GetObjectData(info, context);

                public void OnDeserialization(object sender) => InnerList.OnDeserialization(sender);

                protected virtual bool RemoveItem(T item) => InnerList.Remove(item);

                public bool Remove(T item) => RemoveItem(item);

                protected virtual void RemoveItem(System.Collections.Generic.LinkedListNode<T> node) => InnerList.Remove(node);

                public void Remove(System.Collections.Generic.LinkedListNode<T> node) => RemoveItem(node);

                protected virtual void RemoveFirstItem() => InnerList.RemoveFirst();

                public void RemoveFirst() => RemoveFirstItem();

                protected virtual void RemoveLastItem() => InnerList.RemoveLast();

                public void RemoveLast() => RemoveLastItem();

                public IEnumerator<T> GetEnumerator() => InnerList.GetEnumerator();
            }

            //public interface INotifySimpleLinkedCollectionChanged<T>
            //{
            //    event SimpleLinkedCollectionChangedEventHandler<T> CollectionChanged;
            //}

            //public enum LinkedCollectionChangeAction : byte
            //{
            //    AddedFirst = 0,

            //    AddedLast = 1,

            //    AddedBefore = 2,

            //    AddedAfter = 3,

            //    Removed = 4,

            //    Cleared = 5,
            //}

            //public class LinkedCollectionChangedEventArgs<T>
            //{
            //    public LinkedCollectionChangeAction Action { get; }

            //    public System.Collections.Generic.LinkedListNode<T> AddedBeforeOrAfter { get; }

            //    public System.Collections.Generic.LinkedListNode<T> Node { get; }

            //    public LinkedCollectionChangedEventArgs(LinkedCollectionChangeAction action, System.Collections.Generic.LinkedListNode<T> addedBeforeOrAfter, System.Collections.Generic.LinkedListNode<T> node)
            //    {
            //        if (((action == LinkedCollectionChangeAction.AddedBefore || action == LinkedCollectionChangeAction.AddedAfter) && addedBeforeOrAfter == null) || ((action != LinkedCollectionChangeAction.AddedBefore && action != LinkedCollectionChangeAction.AddedAfter) && addedBeforeOrAfter != null) || (action == LinkedCollectionChangeAction.Removed && (node == null || addedBeforeOrAfter != null)) || (action == LinkedCollectionChangeAction.Cleared && (node != null || addedBeforeOrAfter != null)))

            //            throw new ArgumentException($"Invalid combination of {nameof(action)} and {nameof(node)} or {nameof(addedBeforeOrAfter)}.");

            //        Action = action;

            //        AddedBeforeOrAfter = addedBeforeOrAfter;

            //        Node = node;
            //    }
            //}

            //public delegate void LinkedCollectionChangedEventHandler<T>(object sender, LinkedCollectionChangedEventArgs<T> e);

            //public interface INotifyLinkedCollectionChanged<T>
            //{
            //    event LinkedCollectionChangedEventHandler<T> CollectionChanged;
            //}

            [Serializable]
            public class ObservableLinkedCollection<T> : LinkedCollection<T>, INotifyPropertyChanged, INotifyLinkedCollectionChanged<T>
            {
                public event PropertyChangedEventHandler PropertyChanged;

                public event LinkedCollectionChangedEventHandler<T> CollectionChanged;

                public ObservableLinkedCollection() : base() { }

                public ObservableLinkedCollection(in ILinkedList<T> list) : base(list) { }

                protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

                protected void RaisePropertyChangedEvent(in string propertyName) => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

                protected void RaiseCountPropertyChangedEvent() => OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));

                protected virtual void OnCollectionChanged(LinkedCollectionChangedEventArgs<T> e) => CollectionChanged?.Invoke(this, e);

                protected void RaiseCollectionChangedEvent(in LinkedCollectionChangedAction action, in System.Collections.Generic.LinkedListNode<T> addedBefore, in System.Collections.Generic.LinkedListNode<T> addedAfter, in System.Collections.Generic.LinkedListNode<T> node) => OnCollectionChanged(new LinkedCollectionChangedEventArgs<T>(action, addedBefore, addedAfter, node));

                protected override void AddFirstItem(System.Collections.Generic.LinkedListNode<T> node)
                {
                    base.AddFirstItem(node);

                    RaiseCountPropertyChangedEvent();

                    RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddFirst, null, null, node);
                }

                protected override System.Collections.Generic.LinkedListNode<T> AddFirstItem(T value)
                {
                    System.Collections.Generic.LinkedListNode<T> result = base.AddFirstItem(value);

                    RaiseCountPropertyChangedEvent();

                    RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddFirst, null, null, result);

                    return result;
                }

                protected override void AddItem(T item)
                {
                    base.AddItem(item);

                    RaiseCountPropertyChangedEvent();

                    // Assumming that items are added to the end of the list.

                    RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddLast, null, null, InnerList.Last);
                }

                protected override void AddItemAfter(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode)
                {
                    base.AddItemAfter(node, newNode);

                    RaiseCountPropertyChangedEvent();

                    RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddAfter, null, node, newNode);
                }

                protected override System.Collections.Generic.LinkedListNode<T> AddItemAfter(System.Collections.Generic.LinkedListNode<T> node, T value)
                {
                    System.Collections.Generic.LinkedListNode<T> result = base.AddItemAfter(node, value);

                    RaiseCountPropertyChangedEvent();

                    RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddAfter, null, node, result);

                    return result;
                }

                protected override void AddItemBefore(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode)
                {
                    base.AddItemBefore(node, newNode);

                    RaiseCountPropertyChangedEvent();

                    RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddBefore, node, null, newNode);
                }

                protected override System.Collections.Generic.LinkedListNode<T> AddItemBefore(System.Collections.Generic.LinkedListNode<T> node, T value)
                {
                    System.Collections.Generic.LinkedListNode<T> result = base.AddItemBefore(node, value);

                    RaiseCountPropertyChangedEvent();

                    RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddBefore, node, null, result);

                    return result;
                }

                protected override void AddLastItem(System.Collections.Generic.LinkedListNode<T> node)
                {
                    base.AddLastItem(node);

                    RaiseCountPropertyChangedEvent();

                    RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddLast, null, null, node);
                }

                protected override System.Collections.Generic.LinkedListNode<T> AddLastItem(T value)
                {
                    System.Collections.Generic.LinkedListNode<T> result = base.AddLastItem(value);

                    RaiseCountPropertyChangedEvent();

                    RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddLast, null, null, result);

                    return result;
                }

                protected override void ClearItems()
                {
                    base.ClearItems();

                    RaiseCountPropertyChangedEvent();

                    RaiseCollectionChangedEvent(LinkedCollectionChangedAction.Reset, null, null, null);
                }

                protected override void RemoveFirstItem()
                {
                    System.Collections.Generic.LinkedListNode<T> node = InnerList.First;

                    base.RemoveFirstItem();

                    RaiseCountPropertyChangedEvent();

                    RaiseCollectionChangedEvent(LinkedCollectionChangedAction.Remove, null, null, node);
                }

                protected override void RemoveItem(System.Collections.Generic.LinkedListNode<T> node)
                {
                    base.RemoveItem(node);

                    RaiseCountPropertyChangedEvent();

                    RaiseCollectionChangedEvent(LinkedCollectionChangedAction.Remove, null, null, node);
                }

                protected override bool RemoveItem(T item)
                {
                    foreach (System.Collections.Generic.LinkedListNode<T> node in new LinkedListNodeEnumerator<T>(InnerList))

                        if (node.Value.Equals(item))
                        {
                            base.RemoveItem(node); // This is a custom internal enumerator designed to do not throw when its underlying collection change.

                            RaiseCountPropertyChangedEvent();

                            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.Remove, null, null, node);

                            return true;
                        }

                    return false;
                }

                protected override void RemoveLastItem()
                {
                    System.Collections.Generic.LinkedListNode<T> node = InnerList.Last;

                    base.RemoveLastItem();

                    RaiseCountPropertyChangedEvent();

                    RaiseCollectionChangedEvent(LinkedCollectionChangedAction.Remove, null, null, node);
                }
            }
        }

#if WinCopies2
        [Serializable]
#endif
        internal sealed class LinkedListNodeEnumerator<T> : IEnumerator<System.Collections.Generic.LinkedListNode<T>>, IEnumerable<System.Collections.Generic.LinkedListNode<T>>
        {
            private System.Collections.Generic.LinkedList<T> _list;

            public System.Collections.Generic.LinkedListNode<T> Current { get; private set; }

            object IEnumerator.Current => Current;

            public LinkedListNodeEnumerator(System.Collections.Generic.LinkedList<T> list) => _list = list; // todo: make inner list version check

            public void Dispose()
            {
                Current = null;

                _list = null;
            }

            private bool _first = true;

            public bool MoveNext()
            {
                if (_list.Count == 0)

                    return false;

                if (_first)
                {
                    _first = false;

                    Current = _list.First;

                    return true;
                }

                if (Current.Next == null)
                {
                    Current = null;

                    return false;
                }

                Current = Current.Next;

                return true;
            }

            public void Reset() { }

            public IEnumerator<System.Collections.Generic.LinkedListNode<T>> GetEnumerator() => this;

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Serializable]
        public class QueueCollection<T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection
        {
            protected internal
#if WinCopies2
            System.Collections.Generic.Queue<T>
#else
IEnumerableQueue<T>
#endif
            InnerQueue
            { get; }

            /// <summary>
            /// Gets the number of elements contained in the <see cref="QueueCollection{T}"/>.
            /// </summary>
            /// <value>The number of elements contained in the <see cref="QueueCollection{T}"/>.</value>
            public
#if WinCopies2
int
#else
                uint
#endif
                Count => InnerQueue.Count;

#if !WinCopies2
            int ICollection.Count => (int)Count;

            int IReadOnlyCollection<T>.Count => (int)Count;
#endif

            public bool IsReadOnly => false;

            bool ICollection.IsSynchronized => ((ICollection)InnerQueue).IsSynchronized;

            object ICollection.SyncRoot => ((ICollection)InnerQueue).SyncRoot;

            /// <summary>
            /// Initializes a new instance of the <see cref="QueueCollection{T}"/> class.
            /// </summary>
            public QueueCollection() : this(new
#if WinCopies2
            System.Collections.Generic.Queue
#else
            EnumerableQueue
#endif
            <T>())
            { }

            /// <summary>
            /// Initializes a new instance of the <see cref="QueueCollection{T}"/> class with a custom queue.
            /// </summary>
            /// <param name="queue">The inner queue for this <see cref="QueueCollection{T}"/>.</param>
            public QueueCollection(in
#if WinCopies2
            System.Collections.Generic.Queue<T>
#else
            IEnumerableQueue<T>
#endif
             queue) => InnerQueue = queue;

            /// <summary>
            /// Removes all objects from the <see cref="QueueCollection{T}"/>. Override this method to provide a custom implementation.
            /// </summary>
            protected virtual void ClearItems() => InnerQueue.Clear();

            /// <summary>
            /// Removes all objects from the <see cref="QueueCollection{T}"/>. Override the <see cref="ClearItems"/> method to provide a custom implementation.
            /// </summary>
            public void Clear() => ClearItems();

            /// <summary>
            /// Determines whether an element is in the <see cref="QueueCollection{T}"/>.
            /// </summary>
            /// <param name="item">The object to locate in the <see cref="QueueCollection{T}"/>. The value can be <see langword="null"/> for reference types.</param>
            /// <returns><see langword="true"/> if <paramref name="item"/> is found in the <see cref="System.Collections.Generic.Queue{T}"/>; otherwise, <see langword="false"/>.</returns>
            public bool Contains(T item) => InnerQueue.Contains(item);

            void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerQueue).CopyTo(array, index);

            /// <summary>
            /// Copies the <see cref="QueueCollection{T}"/> elements to an existing one-dimensional <see cref="System.Array"/>, starting at the specified array index.
            /// </summary>
            /// <param name="array">The one-dimensional <see cref="System.Array"/> that is the destination of the elements copied from <see cref="QueueCollection{T}"/>. The <see cref="System.Array"/> must have zero-based indexing.</param>
            /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
            /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/>.</exception>
            /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than zero.</exception>
            /// <exception cref="ArgumentException">The number of elements in the source <see cref="QueueCollection{T}"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination array.</exception>
            public void CopyTo(in T[] array, in int arrayIndex) => InnerQueue.CopyTo(array, arrayIndex);

            /// <summary>
            /// Removes and returns the object at the beginning of the <see cref="QueueCollection{T}"/>. Override this method to provide a custom implementation.
            /// </summary>
            /// <returns>The object that is removed from the beginning of the <see cref="QueueCollection{T}"/>.</returns>
            /// <exception cref="InvalidOperationException">The <see cref="QueueCollection{T}"/> is empty.</exception>
            protected virtual T DequeueItem() => InnerQueue.Dequeue();

            /// <summary>
            /// Removes and returns the object at the beginning of the <see cref="QueueCollection{T}"/>. Override the <see cref="DequeueItem"/> to provide a custom implementation.
            /// </summary>
            /// <returns>The object that is removed from the beginning of the <see cref="QueueCollection{T}"/>.</returns>
            /// <exception cref="InvalidOperationException">The <see cref="QueueCollection{T}"/> is empty.</exception>
            /// <seealso cref="TryDequeue(out T)"/>
            public T Dequeue() => DequeueItem();

            /// <summary>
            /// Adds an object to the end of the <see cref="QueueCollection{T}"/>. Override this method to provide a custom implementation.
            /// </summary>
            /// <param name="item">The object to add to the <see cref="QueueCollection{T}"/>. The value can be <see langword="null"/> for reference types.</param>
            protected virtual void EnqueueItem(T item) => InnerQueue.Enqueue(item);

            /// <summary>
            /// Adds an object to the end of the <see cref="QueueCollection{T}"/>. Override the <see cref="EnqueueItem(T)"/> method to provide a custom implementation.
            /// </summary>
            /// <param name="item">The object to add to the <see cref="QueueCollection{T}"/>. The value can be <see langword="null"/> for reference types.</param>
            public void Enqueue(in T item) => EnqueueItem(item);

            /// <summary>
            /// Returns the object at the beginning of the <see cref="QueueCollection{T}"/> without removing it.
            /// </summary>
            /// <returns>The object at the beginning of the <see cref="QueueCollection{T}"/>.</returns>
            /// <exception cref="InvalidOperationException">The <see cref="QueueCollection{T}"/> is empty.</exception>
            /// <seealso cref="TryPeek(out T)"/>
            public T Peek() => InnerQueue.Peek();

            /// <summary>
            /// Copies the <see cref="QueueCollection{T}"/> elements to a new array.
            /// </summary>
            /// <returns>A new array containing elements copied from the <see cref="QueueCollection{T}"/>.</returns>
            public T[] ToArray() => InnerQueue.ToArray();

#if WinCopies2
            /// <summary>
            /// Sets the capacity to the actual number of elements in the <see cref="QueueCollection{T}"/>, if that number is less than 90 percent of current capacity.
            /// </summary>
            public void TrimExcess() => InnerQueue.TrimExcess();
#endif

#if NETCORE

            /// <summary>
            /// Tries to remove and return the object at the beginning of the <see cref="QueueCollection{T}"/>. Override this method to provide a custom implementation.
            /// </summary>
            /// <param name="result">The object at the beginning of the <see cref="QueueCollection{T}"/>. This value can be <see langword="null"/> when the return value is <see langword="false"/>.</param>
            /// <returns>A <see cref="bool"/> value that indicates whether a value has actually been removed and retrieved.</returns>
            protected virtual bool TryDequeueItem([MaybeNullWhen(false)] out T result) => InnerQueue.TryDequeue(out result);

            /// <summary>
            /// Tries to remove and return the object at the beginning of the <see cref="QueueCollection{T}"/>. Override the <see cref="TryDequeueItem(out T)"/> method to provide a custom implementation.
            /// </summary>
            /// <param name="result">The object at the beginning of the <see cref="QueueCollection{T}"/>. This value can be <see langword="null"/> when the return value is <see langword="false"/>.</param>
            /// <returns>A <see cref="bool"/> value that indicates whether a value has actually been removed and retrieved.</returns>
            /// <seealso cref="Dequeue"/>
            public bool TryDequeue([MaybeNullWhen(false)] out T result) => TryDequeueItem(out result);

            /// <summary>
            /// Tries to peek the object at the beginning of the <see cref="QueueCollection{T}"/> without removing it.
            /// </summary>
            /// <param name="result">The object at the beginning of the <see cref="QueueCollection{T}"/>. This value can be <see langword="null"/> when the return value is <see langword="false"/>.</param>
            /// <returns>A <see cref="bool"/> value that indicates whether a value has actually been retrieved.</returns>
            /// <seealso cref="Peek"/>
            public bool TryPeek([MaybeNullWhen(false)] out T result) => InnerQueue.TryPeek(out result);

#endif

            /// <summary>
            /// Returns an enumerator that iterates through the <see cref="QueueCollection{T}"/>.
            /// </summary>
            /// <returns>An <see cref="IEnumerator{T}"/> for the <see cref="QueueCollection{T}"/>.</returns>
            public IEnumerator<T> GetEnumerator() => InnerQueue.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerQueue).GetEnumerator();
        }

        [Serializable]
        public class StackCollection<T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection
        {
            protected internal
#if WinCopies2
            System.Collections.Generic.Stack
#else
IEnumerableStack
#endif
            <T> InnerStack
            { get; }

            public
#if WinCopies2
int
#else
                uint
#endif
                Count => InnerStack.Count;

#if !WinCopies2
            int ICollection.Count => (int)Count;

            int IReadOnlyCollection<T>.Count => (int)Count;
#endif

            public bool IsReadOnly => false;

            bool ICollection.IsSynchronized => ((ICollection)InnerStack).IsSynchronized;

            object ICollection.SyncRoot => ((ICollection)InnerStack).SyncRoot;

            public StackCollection() : this(new
#if WinCopies2
            System.Collections.Generic.Stack
#else
            EnumerableStack
#endif
            <T>())
            { }

            public StackCollection(in
#if WinCopies2
            System.Collections.Generic.Stack
#else
            IEnumerableStack
#endif
            <T> stack) => InnerStack = stack;

            protected virtual void ClearItems() => InnerStack.Clear();

            public void Clear() => ClearItems();

            public void Contains(T item) => InnerStack.Contains(item);

            public T Peek() => InnerStack.Peek();

            protected virtual T PopItem() => InnerStack.Pop();

            public T Pop() => PopItem();

            protected virtual void PushItem(T item) => InnerStack.Push(item);

            public void Push(in T item) => PushItem(item);

            public T[] ToArray() => InnerStack.ToArray();

#if WinCopies2
            public void TrimExcess() => InnerStack.TrimExcess();
#endif

#if NETCORE
            public bool TryPeek(out T result) => InnerStack.TryPeek(out result);

            protected virtual bool TryPopItem(out T result) => InnerStack.TryPop(out result);

            public bool TryPop(out T result) => TryPopItem(out result);
#endif

            void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerStack).CopyTo(array, index);

            public void CopyTo(in T[] array, in int arrayIndex) => InnerStack.CopyTo(array, arrayIndex);

            public IEnumerator<T> GetEnumerator() => InnerStack.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerStack).GetEnumerator();
        }

        [Serializable]
        public class LinkedCollection<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection, IDeserializationCallback, ISerializable
        {
            protected internal System.Collections.Generic.LinkedList<T> InnerList { get; }

            public System.Collections.Generic.LinkedListNode<T> Last => InnerList.Last;

            public System.Collections.Generic.LinkedListNode<T> First => InnerList.First;

            public int Count => InnerList.Count;

            bool ICollection.IsSynchronized => ((ICollection)InnerList).IsSynchronized;

            object ICollection.SyncRoot => ((ICollection)InnerList).SyncRoot;

            bool ICollection<T>.IsReadOnly => false;

            public LinkedCollection() : this(new LinkedList<T>()) { }

            public LinkedCollection(in System.Collections.Generic.LinkedList<T> list) => InnerList = list;

            protected virtual void AddItem(T item) => ((ICollection<T>)InnerList).Add(item);

            void ICollection<T>.Add(T item) => AddItem(item);

            protected virtual void AddItemAfter(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode) => InnerList.AddAfter(node, newNode);

            /// <summary>
            /// Adds the specified new node after the specified existing node in the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddItemAfter(System.Collections.Generic.LinkedListNode{T}, System.Collections.Generic.LinkedListNode{T})"/> method to provide a custom implementation.
            /// </summary>
            /// <param name="node">The <see cref="System.Collections.Generic.LinkedListNode{T}"/> after which to insert <paramref name="newNode"/>.</param>
            /// <param name="newNode">The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> to add to the <see cref="LinkedCollection{T}"/>.</param>
            /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>. -or- <paramref name="newNode"/> is <see langword="null"/>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current <see cref="LinkedCollection{T}"/>. -or- <paramref name="newNode"/> belongs to another <see cref="System.Collections.Generic.LinkedList{T}"/>.</exception>
            /// <seealso cref="AddAfter(System.Collections.Generic.LinkedListNode{T}, T)"/>
            public void AddAfter(in System.Collections.Generic.LinkedListNode<T> node, in System.Collections.Generic.LinkedListNode<T> newNode) => AddItemAfter(node, newNode);

            protected virtual System.Collections.Generic.LinkedListNode<T> AddItemAfter(System.Collections.Generic.LinkedListNode<T> node, T value) => InnerList.AddAfter(node, value);

            /// <summary>
            /// Adds a new node containing the specified value after the specified existing node in the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddItemAfter(System.Collections.Generic.LinkedListNode{T}, T)"/> method to provide a custom implementation.
            /// </summary>
            /// <param name="node">The <see cref="System.Collections.Generic.LinkedListNode{T}"/> after which to insert a new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</param>
            /// <param name="value">The value to add to the <see cref="LinkedCollection{T}"/>.</param>
            /// <returns>The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</returns>
            /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current <see cref="LinkedCollection{T}"/>.</exception>
            /// <seealso cref="AddAfter(in System.Collections.Generic.LinkedListNode{T}, in System.Collections.Generic.LinkedListNode{T})"/>
            public System.Collections.Generic.LinkedListNode<T> AddAfter(System.Collections.Generic.LinkedListNode<T> node, T value) => AddItemAfter(node, value);

            protected virtual void AddItemBefore(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode) => InnerList.AddBefore(node, newNode);

            /// <summary>
            /// Adds the specified new node before the specified existing node in the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddItemBefore(System.Collections.Generic.LinkedListNode{T}, System.Collections.Generic.LinkedListNode{T})"/> method to provide a custom implementation.
            /// </summary>
            /// <param name="node">The <see cref="System.Collections.Generic.LinkedListNode{T}"/> before which to insert <paramref name="newNode"/>.</param>
            /// <param name="newNode">The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> to add to the <see cref="LinkedCollection{T}"/>.</param>
            /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>. -or- <paramref name="newNode"/> is <see langword="null"/>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current <see cref="LinkedCollection{T}"/>. -or- <paramref name="newNode"/> belongs to another <see cref="System.Collections.Generic.LinkedList{T}"/>.</exception>
            /// <seealso cref="AddBefore(in System.Collections.Generic.LinkedListNode{T}, in T)"/>
            public void AddBefore(in System.Collections.Generic.LinkedListNode<T> node, in System.Collections.Generic.LinkedListNode<T> newNode) => AddItemBefore(node, newNode);

            protected virtual System.Collections.Generic.LinkedListNode<T> AddItemBefore(System.Collections.Generic.LinkedListNode<T> node, T value) => InnerList.AddBefore(node, value);

            /// <summary>
            /// Adds a new node containing the specified value before the specified existing node in the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddItemBefore(System.Collections.Generic.LinkedListNode{T}, T)"/> method to provide a custom implementation.
            /// </summary>
            /// <param name="node">The <see cref="System.Collections.Generic.LinkedListNode{T}"/> before which to insert a new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</param>
            /// <param name="value">The value to add to the <see cref="LinkedCollection{T}"/>.</param>
            /// <returns>The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</returns>
            /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current <see cref="LinkedCollection{T}"/>.</exception>
            /// <seealso cref="AddBefore(in System.Collections.Generic.LinkedListNode{T}, in System.Collections.Generic.LinkedListNode{T})"/>
            public System.Collections.Generic.LinkedListNode<T> AddBefore(in System.Collections.Generic.LinkedListNode<T> node, in T value) => AddItemBefore(node, value);

            protected virtual void AddFirstItem(System.Collections.Generic.LinkedListNode<T> node) => InnerList.AddFirst(node);

            /// <summary>
            /// Adds the specified new node at the start of the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddFirstItem(System.Collections.Generic.LinkedListNode{T})"/> method to provide a custom implementation.
            /// </summary>
            /// <param name="node">The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> to add at the start of the <see cref="LinkedCollection{T}"/>.</param>
            /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="node"/> belongs to another <see cref="System.Collections.Generic.LinkedList{T}"/>.</exception>
            /// <seealso cref="AddFirst(in T)"/>
            public void AddFirst(in System.Collections.Generic.LinkedListNode<T> node) => AddFirstItem(node);

            protected virtual System.Collections.Generic.LinkedListNode<T> AddFirstItem(T value) => InnerList.AddFirst(value);

            /// <summary>
            /// Adds a new node containing the specified value at the start of the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddFirstItem(T)"/> method to provide a custom implementation.
            /// </summary>
            /// <param name="value">The value to add at the start of the <see cref="LinkedCollection{T}"/>.</param>
            /// <returns>The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</returns>
            /// <seealso cref="AddFirst(in T)"/>
            public System.Collections.Generic.LinkedListNode<T> AddFirst(in T value) => AddFirstItem(value);

            protected virtual void AddLastItem(System.Collections.Generic.LinkedListNode<T> node) => InnerList.AddLast(node);

            /// <summary>
            /// Adds the specified new node at the end of the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddLastItem(System.Collections.Generic.LinkedListNode{T})"/> method to provide a custom implementation.
            /// </summary>
            /// <param name="node">The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> to add at the end of the <see cref="LinkedCollection{T}"/>.</param>
            /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="node"/> belongs to another <see cref="System.Collections.Generic.LinkedList{T}"/>.</exception>
            /// <seealso cref="AddLast(in T)"/>
            public void AddLast(in System.Collections.Generic.LinkedListNode<T> node) => AddLastItem(node);

            protected virtual System.Collections.Generic.LinkedListNode<T> AddLastItem(T value) => InnerList.AddLast(value);

            /// <summary>
            /// Adds a new node containing the specified value at the end of the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddLastItem(T)"/> method to provide a custom implementation.
            /// </summary>
            /// <param name="value">The value to add at the end of the <see cref="LinkedCollection{T}"/>.</param>
            /// <returns>The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</returns>
            /// <seealso cref="AddLast(in System.Collections.Generic.LinkedListNode{T})"/>
            public System.Collections.Generic.LinkedListNode<T> AddLast(in T value) => AddLastItem(value);

            public System.Collections.Generic.LinkedListNode<T> Find(T value) => InnerList.Find(value);

            public System.Collections.Generic.LinkedListNode<T> FindLast(T value) => InnerList.FindLast(value);

            protected virtual void ClearItems() => InnerList.Clear();

            public void Clear() => ClearItems();

            public bool Contains(T item) => InnerList.Contains(item);

            public void CopyTo(Array array, int index) => ((ICollection)InnerList).CopyTo(array, index);

            public void CopyTo(T[] array, int arrayIndex) => InnerList.CopyTo(array, arrayIndex);

            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerList).GetEnumerator();

            public void GetObjectData(SerializationInfo info, StreamingContext context) => InnerList.GetObjectData(info, context);

            public void OnDeserialization(object sender) => InnerList.OnDeserialization(sender);

            protected virtual bool RemoveItem(T item) => InnerList.Remove(item);

            public bool Remove(T item) => RemoveItem(item);

            protected virtual void RemoveItem(System.Collections.Generic.LinkedListNode<T> node) => InnerList.Remove(node);

            public void Remove(System.Collections.Generic.LinkedListNode<T> node) => RemoveItem(node);

            protected virtual void RemoveFirstItem() => InnerList.RemoveFirst();

            public void RemoveFirst() => RemoveFirstItem();

            protected virtual void RemoveLastItem() => InnerList.RemoveLast();

            public void RemoveLast() => RemoveLastItem();

            public IEnumerator<T> GetEnumerator() => InnerList.GetEnumerator();
        }

        public class SimpleLinkedCollectionChangedEventArgs<T>
        {
            public NotifyCollectionChangedAction Action { get; }

            public T Item { get; }

            public SimpleLinkedCollectionChangedEventArgs(NotifyCollectionChangedAction action, T item)
            {
                Action = action;

                Item = item;
            }
        }

        public delegate void SimpleLinkedCollectionChangedEventHandler<T>(object sender, SimpleLinkedCollectionChangedEventArgs<T> e);

        public interface INotifySimpleLinkedCollectionChanged<T>
        {
            event SimpleLinkedCollectionChangedEventHandler<T> CollectionChanged;
        }

        public class LinkedCollectionChangedEventArgs<T>
        {
            public LinkedCollectionChangedAction Action { get; }

            public System.Collections.Generic.LinkedListNode<T> AddedBefore { get; }

            public System.Collections.Generic.LinkedListNode<T> AddedAfter { get; }

            public System.Collections.Generic.LinkedListNode<T> Node { get; }

            public LinkedCollectionChangedEventArgs(LinkedCollectionChangedAction action, System.Collections.Generic.LinkedListNode<T> addedBefore, System.Collections.Generic.LinkedListNode<T> addedAfter, System.Collections.Generic.LinkedListNode<T> node)
            {
                bool check(LinkedCollectionChangedAction _action, System.Collections.Generic.LinkedListNode<T> parameter) => (_action == action && parameter == null) || !(_action == action || parameter == null);

                if ((action == LinkedCollectionChangedAction.Reset && (node != null || addedBefore != null || addedAfter != null))
                    || (action != LinkedCollectionChangedAction.Reset && node == null)
                    || (action.IsValidEnumValue(true, LinkedCollectionChangedAction.AddFirst, LinkedCollectionChangedAction.AddLast) && (addedBefore != null || addedAfter != null))
                    || (action == LinkedCollectionChangedAction.Move && addedBefore == null && addedAfter == null)
                    || check(LinkedCollectionChangedAction.AddBefore, addedBefore)
                    || check(LinkedCollectionChangedAction.AddAfter, addedAfter)
                    || (action.IsValidEnumValue(true, LinkedCollectionChangedAction.Remove, LinkedCollectionChangedAction.Reset) && !(addedBefore == null && addedAfter == null)))

                    throw new ArgumentException($"Invalid combination of {nameof(action)} and {nameof(node)}, {nameof(addedBefore)} or {nameof(addedAfter)}.");

                Action = action;

                AddedBefore = addedBefore;

                AddedAfter = addedAfter;

                Node = node;
            }
        }

        public delegate void LinkedCollectionChangedEventHandler<T>(object sender, LinkedCollectionChangedEventArgs<T> e);

        public interface INotifyLinkedCollectionChanged<T>
        {
            event LinkedCollectionChangedEventHandler<T> CollectionChanged;
        }

        [Serializable]
        public class ObservableQueueCollection<T> : QueueCollection<T>, INotifyPropertyChanged, INotifySimpleLinkedCollectionChanged<T>
        {
            public event PropertyChangedEventHandler PropertyChanged;

            public event SimpleLinkedCollectionChangedEventHandler<T> CollectionChanged;

            public ObservableQueueCollection() : base() { }

            public ObservableQueueCollection(in
#if WinCopies2
            System.Collections.Generic.Queue
#else
            IEnumerableQueue
#endif
            <T> queue) : base(queue) { }

            protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

            protected void RaisePropertyChangedEvent(in string propertyName) => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

            protected void RaiseCountPropertyChangedEvent() => OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));

            protected virtual void OnCollectionChanged(SimpleLinkedCollectionChangedEventArgs<T> e) => CollectionChanged?.Invoke(this, e);

            protected void RaiseCollectionChangedEvent(NotifyCollectionChangedAction action, T item) => OnCollectionChanged(new SimpleLinkedCollectionChangedEventArgs<T>(action, item));

            protected override void ClearItems()
            {
                base.ClearItems();

                RaiseCountPropertyChangedEvent();

                OnCollectionChanged(new SimpleLinkedCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Reset, default));
            }

            protected override T DequeueItem()
            {
                T result = base.DequeueItem();

                RaiseCountPropertyChangedEvent();

                RaiseCollectionChangedEvent(NotifyCollectionChangedAction.Remove, result);

                return result;
            }

            protected override void EnqueueItem(T item)
            {
                base.EnqueueItem(item);

                RaiseCountPropertyChangedEvent();

                RaiseCollectionChangedEvent(NotifyCollectionChangedAction.Add, item);
            }

#if NETCORE

            protected override bool TryDequeueItem([MaybeNullWhen(false)] out T result)
            {
                bool succeeded = base.TryDequeueItem(out result);

                if (succeeded)
                {
                    RaiseCountPropertyChangedEvent();

                    RaiseCollectionChangedEvent(NotifyCollectionChangedAction.Remove, result);
                }

                return succeeded;
            }

#endif
        }

        [Serializable]
        public class ObservableStackCollection<T> : StackCollection<T>, INotifyPropertyChanged, INotifySimpleLinkedCollectionChanged<T>
        {
            public event PropertyChangedEventHandler PropertyChanged;

            public event SimpleLinkedCollectionChangedEventHandler<T> CollectionChanged;

            public ObservableStackCollection() : base() { }

            public ObservableStackCollection(in
#if WinCopies2
            System.Collections.Generic.Stack
#else
            IEnumerableStack
#endif
            <T> stack) : base(stack) { }

            protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

            protected void RaisePropertyChangedEvent(in string propertyName) => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

            protected void RaiseCountPropertyChangedEvent() => OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));

            protected virtual void OnCollectionChanged(SimpleLinkedCollectionChangedEventArgs<T> e) => CollectionChanged?.Invoke(this, e);

            protected void RaiseCollectionChangedEvent(NotifyCollectionChangedAction action, T item) => OnCollectionChanged(new SimpleLinkedCollectionChangedEventArgs<T>(action, item));

            protected override void ClearItems()
            {
                base.ClearItems();

                RaiseCountPropertyChangedEvent();

                RaiseCollectionChangedEvent(NotifyCollectionChangedAction.Reset, default);
            }

            protected override T PopItem()
            {
                T result = base.PopItem();

                RaiseCountPropertyChangedEvent();

                RaiseCollectionChangedEvent(NotifyCollectionChangedAction.Remove, result);

                return result;
            }

            protected override void PushItem(T item)
            {
                base.PushItem(item);

                RaiseCountPropertyChangedEvent();

                RaiseCollectionChangedEvent(NotifyCollectionChangedAction.Add, item);
            }

#if NETCORE

            protected override bool TryPopItem(out T result)
            {
                bool succeeded = base.TryPopItem(out result);

                if (succeeded)
                {
                    RaiseCountPropertyChangedEvent();

                    RaiseCollectionChangedEvent(NotifyCollectionChangedAction.Remove, result);
                }

                return succeeded;
            }

#endif
        }

        [Serializable]
        public class ObservableLinkedCollection<T> : LinkedCollection<T>, INotifyPropertyChanged, INotifyLinkedCollectionChanged<T>
        {
            public event PropertyChangedEventHandler PropertyChanged;

            public event LinkedCollectionChangedEventHandler<T> CollectionChanged;

            public ObservableLinkedCollection() : base() { }

            public ObservableLinkedCollection(in LinkedList<T> list) : base(list) { }

            protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

            protected void RaisePropertyChangedEvent(in string propertyName) => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

            protected void RaiseCountPropertyChangedEvent() => OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));

            protected virtual void OnCollectionChanged(LinkedCollectionChangedEventArgs<T> e) => CollectionChanged?.Invoke(this, e);

            protected void RaiseCollectionChangedEvent(in LinkedCollectionChangedAction action, in System.Collections.Generic.LinkedListNode<T> addedBefore, in System.Collections.Generic.LinkedListNode<T> addedAfter, in System.Collections.Generic.LinkedListNode<T> node) => OnCollectionChanged(new LinkedCollectionChangedEventArgs<T>(action, addedBefore, addedAfter, node));

            protected override void AddFirstItem(System.Collections.Generic.LinkedListNode<T> node)
            {
                base.AddFirstItem(node);

                RaiseCountPropertyChangedEvent();

                RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddFirst, null, null, node);
            }

            protected override System.Collections.Generic.LinkedListNode<T> AddFirstItem(T value)
            {
                System.Collections.Generic.LinkedListNode<T> result = base.AddFirstItem(value);

                RaiseCountPropertyChangedEvent();

                RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddFirst, null, null, result);

                return result;
            }

            protected override void AddItem(T item)
            {
                base.AddItem(item);

                RaiseCountPropertyChangedEvent();

                // Assumming that items are added to the end of the list.

                RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddLast, null, null, InnerList.Last);
            }

            protected override void AddItemAfter(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode)
            {
                base.AddItemAfter(node, newNode);

                RaiseCountPropertyChangedEvent();

                RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddAfter, null, node, newNode);
            }

            protected override System.Collections.Generic.LinkedListNode<T> AddItemAfter(System.Collections.Generic.LinkedListNode<T> node, T value)
            {
                System.Collections.Generic.LinkedListNode<T> result = base.AddItemAfter(node, value);

                RaiseCountPropertyChangedEvent();

                RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddAfter, null, node, result);

                return result;
            }

            protected override void AddItemBefore(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode)
            {
                base.AddItemBefore(node, newNode);

                RaiseCountPropertyChangedEvent();

                RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddBefore, node, null, newNode);
            }

            protected override System.Collections.Generic.LinkedListNode<T> AddItemBefore(System.Collections.Generic.LinkedListNode<T> node, T value)
            {
                System.Collections.Generic.LinkedListNode<T> result = base.AddItemBefore(node, value);

                RaiseCountPropertyChangedEvent();

                RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddBefore, node, null, result);

                return result;
            }

            protected override void AddLastItem(System.Collections.Generic.LinkedListNode<T> node)
            {
                base.AddLastItem(node);

                RaiseCountPropertyChangedEvent();

                RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddLast, null, null, node);
            }

            protected override System.Collections.Generic.LinkedListNode<T> AddLastItem(T value)
            {
                System.Collections.Generic.LinkedListNode<T> result = base.AddLastItem(value);

                RaiseCountPropertyChangedEvent();

                RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddLast, null, null, result);

                return result;
            }

            protected override void ClearItems()
            {
                base.ClearItems();

                RaiseCountPropertyChangedEvent();

                RaiseCollectionChangedEvent(LinkedCollectionChangedAction.Reset, null, null, null);
            }

            protected override void RemoveFirstItem()
            {
                System.Collections.Generic.LinkedListNode<T> node = InnerList.First;

                base.RemoveFirstItem();

                RaiseCountPropertyChangedEvent();

                RaiseCollectionChangedEvent(LinkedCollectionChangedAction.Remove, null, null, node);
            }

            protected override void RemoveItem(System.Collections.Generic.LinkedListNode<T> node)
            {
                base.RemoveItem(node);

                RaiseCountPropertyChangedEvent();

                RaiseCollectionChangedEvent(LinkedCollectionChangedAction.Remove, null, null, node);
            }

            protected override bool RemoveItem(T item)
            {
                foreach (System.Collections.Generic.LinkedListNode<T> node in new LinkedListNodeEnumerator<T>(InnerList))

                    if (node.Value.Equals(item))
                    {
                        base.RemoveItem(node); // This is a custom internal enumerator designed to do not throw when its underlying collection change.

                        RaiseCountPropertyChangedEvent();

                        RaiseCollectionChangedEvent(LinkedCollectionChangedAction.Remove, null, null, node);

                        return true;
                    }

                return false;
            }

            protected override void RemoveLastItem()
            {
                System.Collections.Generic.LinkedListNode<T> node = InnerList.Last;

                base.RemoveLastItem();

                RaiseCountPropertyChangedEvent();

                RaiseCollectionChangedEvent(LinkedCollectionChangedAction.Remove, null, null, node);
            }
        }

        [Serializable]
        public class ReadOnlyQueueCollection<T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection
        {
            protected
#if WinCopies2
            System.Collections.Generic.Queue
#else
IEnumerableQueue
#endif
            <T> InnerQueue
            { get; }

            /// <summary>
            /// Gets the number of elements contained in the <see cref="QueueCollection{T}"/>.
            /// </summary>
            /// <value>The number of elements contained in the <see cref="QueueCollection{T}"/>.</value>
            public
#if WinCopies2
int
#else
                uint
#endif
                Count => InnerQueue.Count;

#if !WinCopies2
            int ICollection.Count => (int)Count;
            int IReadOnlyCollection<T>.Count => (int)Count;
#endif

            public bool IsReadOnly => true;

            bool ICollection.IsSynchronized => ((ICollection)InnerQueue).IsSynchronized;

            object ICollection.SyncRoot => ((ICollection)InnerQueue).SyncRoot;

            /// <summary>
            /// Initializes a new instance of the <see cref="QueueCollection{T}"/> class with a custom <see cref="System.Collections.Generic.Queue{T}"/>.
            /// </summary>
            /// <param name="queue">The inner <see cref="System.Collections.Generic.Queue{T}"/> for this <see cref="QueueCollection{T}"/>.</param>
            public ReadOnlyQueueCollection(in
#if WinCopies2
                System.Collections.Generic.Queue<T>
#else
                IEnumerableQueue<T>
#endif
                queue) => InnerQueue = queue;

            public ReadOnlyQueueCollection(in QueueCollection<T> queueCollection) : this(queueCollection.InnerQueue) { }

            /// <summary>
            /// Determines whether an element is in the <see cref="QueueCollection{T}"/>.
            /// </summary>
            /// <param name="item">The object to locate in the <see cref="QueueCollection{T}"/>. The value can be <see langword="null"/> for reference types.</param>
            /// <returns><see langword="true"/> if <paramref name="item"/> is found in the <see cref="System.Collections.Generic.Queue{T}"/>; otherwise, <see langword="false"/>.</returns>
            public bool Contains(T item) => InnerQueue.Contains(item);

            void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerQueue).CopyTo(array, index);

            /// <summary>
            /// Copies the <see cref="QueueCollection{T}"/> elements to an existing one-dimensional <see cref="System.Array"/>, starting at the specified array index.
            /// </summary>
            /// <param name="array">The one-dimensional <see cref="System.Array"/> that is the destination of the elements copied from <see cref="QueueCollection{T}"/>. The <see cref="System.Array"/> must have zero-based indexing.</param>
            /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
            /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/>.</exception>
            /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than zero.</exception>
            /// <exception cref="ArgumentException">The number of elements in the source <see cref="QueueCollection{T}"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination array.</exception>
            public void CopyTo(in T[] array, in int arrayIndex) => InnerQueue.CopyTo(array, arrayIndex);

            /// <summary>
            /// Returns the object at the beginning of the <see cref="QueueCollection{T}"/> without removing it.
            /// </summary>
            /// <returns>The object at the beginning of the <see cref="QueueCollection{T}"/>.</returns>
            /// <exception cref="InvalidOperationException">The <see cref="QueueCollection{T}"/> is empty.</exception>
            /// <seealso cref="TryPeek(out T)"/>
            public T Peek() => InnerQueue.Peek();

#if NETCORE

            /// <summary>
            /// Tries to peek the object at the beginning of the <see cref="QueueCollection{T}"/> without removing it.
            /// </summary>
            /// <param name="result">The object at the beginning of the <see cref="QueueCollection{T}"/>. This value can be <see langword="null"/> when the return value is <see langword="false"/>.</param>
            /// <returns>A <see cref="bool"/> value that indicates whether a value has actually been retrieved.</returns>
            /// <seealso cref="Peek"/>
            public bool TryPeek([MaybeNullWhen(false)] out T result) => InnerQueue.TryPeek(out result);

#endif

            /// <summary>
            /// Returns an enumerator that iterates through the <see cref="QueueCollection{T}"/>.
            /// </summary>
            /// <returns>An <see cref="IEnumerator{T}"/> for the <see cref="QueueCollection{T}"/>.</returns>
            public IEnumerator<T> GetEnumerator() => InnerQueue.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerQueue).GetEnumerator();
        }

        [Serializable]
        public class ReadOnlyStackCollection<T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection
        {
            protected
#if WinCopies2
            System.Collections.Generic.Stack
#else
IEnumerableStack
#endif
            <T> InnerStack
            { get; }

            public uint Count => InnerStack.Count;

#if !WinCopies2
            int ICollection.Count => (int)Count;

            int IReadOnlyCollection<T>.Count => (int)Count;
#endif

            public bool IsReadOnly => true;

            bool ICollection.IsSynchronized => ((ICollection)InnerStack).IsSynchronized;

            object ICollection.SyncRoot => ((ICollection)InnerStack).SyncRoot;

            public ReadOnlyStackCollection(in
#if WinCopies2
            System.Collections.Generic.Stack
#else
            IEnumerableStack
#endif
            <T> stack) => InnerStack = stack;

            public ReadOnlyStackCollection(in StackCollection<T> stackCollection) : this(stackCollection.InnerStack) { }

            public void Contains(T item) => InnerStack.Contains(item);

            public T Peek() => InnerStack.Peek();

            public T[] ToArray() => InnerStack.ToArray();

#if NETCORE

            public bool TryPeek(out T result) => InnerStack.TryPeek(out result);

#endif

            void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerStack).CopyTo(array, index);

            public void CopyTo(in T[] array, in int arrayIndex) => InnerStack.CopyTo(array, arrayIndex);

            public IEnumerator<T> GetEnumerator() => InnerStack.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerStack).GetEnumerator();
        }

        [Serializable]
        public class ReadOnlyLinkedCollection<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection, IDeserializationCallback, ISerializable
        {
            protected System.Collections.Generic.LinkedList<T> InnerList { get; }

            public System.Collections.Generic.LinkedListNode<T> Last => InnerList.Last;

            public System.Collections.Generic.LinkedListNode<T> First => InnerList.First;

            public int Count => InnerList.Count;

            public bool IsReadOnly => true;

            bool ICollection.IsSynchronized => ((ICollection)InnerList).IsSynchronized;

            object ICollection.SyncRoot => ((ICollection)InnerList).SyncRoot;

            bool ICollection<T>.IsReadOnly => true;

            public ReadOnlyLinkedCollection(in System.Collections.Generic.LinkedList<T> list) => InnerList = list;

            public ReadOnlyLinkedCollection(in LinkedCollection<T> listCollection) : this(listCollection.InnerList) { }

            public System.Collections.Generic.LinkedListNode<T> Find(T value) => InnerList.Find(value);

            public System.Collections.Generic.LinkedListNode<T> FindLast(T value) => InnerList.FindLast(value);

            public bool Contains(T item) => InnerList.Contains(item);

            public void CopyTo(Array array, int index) => ((ICollection)InnerList).CopyTo(array, index);

            public void CopyTo(T[] array, int arrayIndex) => InnerList.CopyTo(array, arrayIndex);

            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerList).GetEnumerator();

            public void GetObjectData(SerializationInfo info, StreamingContext context) => InnerList.GetObjectData(info, context);

            public void OnDeserialization(object sender) => InnerList.OnDeserialization(sender);

            public IEnumerator<T> GetEnumerator() => InnerList.GetEnumerator();

            void ICollection<T>.Add(T item) => throw new InvalidOperationException("The collection is read-only.");

            void ICollection<T>.Clear() => throw new InvalidOperationException("The collection is read-only.");

            bool ICollection<T>.Remove(T item) => throw new InvalidOperationException("The collection is read-only.");
        }

        [Serializable]
        public class ReadOnlyObservableQueueCollection<T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection, INotifyPropertyChanged, INotifySimpleLinkedCollectionChanged<T>
        {
            protected ObservableQueueCollection<T> InnerQueueCollection { get; }

            public
#if WinCopies2
int
#else
                uint
#endif
                Count => InnerQueueCollection.Count;

#if !WinCopies2
            int ICollection.Count => (int)Count;

            int IReadOnlyCollection<T>.Count => (int)Count;
#endif

            public bool IsReadOnly => true;

            bool ICollection.IsSynchronized => ((ICollection)InnerQueueCollection).IsSynchronized;

            object ICollection.SyncRoot => ((ICollection)InnerQueueCollection).SyncRoot;

            public event PropertyChangedEventHandler PropertyChanged;

            public event SimpleLinkedCollectionChangedEventHandler<T> CollectionChanged;

            public ReadOnlyObservableQueueCollection(in ObservableQueueCollection<T> queueCollection)
            {
                InnerQueueCollection = queueCollection ?? throw GetArgumentNullException(nameof(queueCollection));

                InnerQueueCollection.CollectionChanged += (object sender, SimpleLinkedCollectionChangedEventArgs<T> e) => OnCollectionChanged(e);

                InnerQueueCollection.PropertyChanged += (object sender, PropertyChangedEventArgs e) => OnPropertyChanged(e);
            }

            protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

            protected void RaisePropertyChangedEvent(in string propertyName) => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

            protected virtual void OnCollectionChanged(SimpleLinkedCollectionChangedEventArgs<T> e) => CollectionChanged?.Invoke(this, e);

            protected void RaiseCollectionChangedEvent(NotifyCollectionChangedAction action, T item) => OnCollectionChanged(new SimpleLinkedCollectionChangedEventArgs<T>(action, item));

            public IEnumerator<T> GetEnumerator() => InnerQueueCollection.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerQueueCollection).GetEnumerator();

            public void CopyTo(T[] array, int arrayIndex) => InnerQueueCollection.CopyTo(array, arrayIndex);

            void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerQueueCollection).CopyTo(array, index);
        }

        [Serializable]
        public class ReadOnlyObservableStackCollection<T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection, INotifyPropertyChanged, INotifySimpleLinkedCollectionChanged<T>
        {
            protected ObservableStackCollection<T> InnerStackCollection { get; }

            public
#if WinCopies2
int
#else
                uint
#endif
                Count => InnerStackCollection.Count;

#if !WinCopies2
            int ICollection.Count => (int)Count;

            int IReadOnlyCollection<T>.Count => (int)Count;
#endif

            public bool IsReadOnly => true;

            bool ICollection.IsSynchronized => ((ICollection)InnerStackCollection).IsSynchronized;

            object ICollection.SyncRoot => ((ICollection)InnerStackCollection).SyncRoot;

            public event PropertyChangedEventHandler PropertyChanged;

            public event SimpleLinkedCollectionChangedEventHandler<T> CollectionChanged;

            public ReadOnlyObservableStackCollection(in ObservableStackCollection<T> stackCollection)
            {
                InnerStackCollection = stackCollection ?? throw GetArgumentNullException(nameof(stackCollection));

                InnerStackCollection.CollectionChanged += (object sender, SimpleLinkedCollectionChangedEventArgs<T> e) => OnCollectionChanged(e);

                InnerStackCollection.PropertyChanged += (object sender, PropertyChangedEventArgs e) => OnPropertyChanged(e);
            }

            protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

            protected void RaisePropertyChangedEvent(in string propertyName) => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

            protected void RaiseCountPropertyChangedEvent() => OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));

            protected virtual void OnCollectionChanged(SimpleLinkedCollectionChangedEventArgs<T> e) => CollectionChanged?.Invoke(this, e);

            protected void RaiseCollectionChangedEvent(NotifyCollectionChangedAction action, T item) => OnCollectionChanged(new SimpleLinkedCollectionChangedEventArgs<T>(action, item));

            public IEnumerator<T> GetEnumerator() => InnerStackCollection.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerStackCollection).GetEnumerator();

            public void CopyTo(T[] array, int arrayIndex) => InnerStackCollection.CopyTo(array, arrayIndex);

            void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerStackCollection).CopyTo(array, index);
        }

        [Serializable]
        public class ReadOnlyObservableLinkedCollection<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection, INotifyPropertyChanged, INotifyLinkedCollectionChanged<T>
        {
            protected ObservableLinkedCollection<T> InnerLinkedCollection { get; }

            public int Count => InnerLinkedCollection.Count;

            public bool IsReadOnly => true;

            bool ICollection.IsSynchronized => ((ICollection)InnerLinkedCollection).IsSynchronized;

            object ICollection.SyncRoot => ((ICollection)InnerLinkedCollection).SyncRoot;

            public event PropertyChangedEventHandler PropertyChanged;

            public event LinkedCollectionChangedEventHandler<T> CollectionChanged;

            public ReadOnlyObservableLinkedCollection(in ObservableLinkedCollection<T> linkedCollection)
            {
                InnerLinkedCollection = linkedCollection ?? throw GetArgumentNullException(nameof(linkedCollection));

                InnerLinkedCollection.CollectionChanged += (object sender, LinkedCollectionChangedEventArgs<T> e) => OnCollectionChanged(e);

                InnerLinkedCollection.PropertyChanged += (object sender, PropertyChangedEventArgs e) => OnPropertyChanged(e);
            }

            protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

            protected void RaisePropertyChangedEvent(in string propertyName) => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

            protected void RaiseCountPropertyChangedEvent() => OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));

            protected virtual void OnCollectionChanged(LinkedCollectionChangedEventArgs<T> e) => CollectionChanged?.Invoke(this, e);

            protected void RaiseCollectionChangedEvent(in LinkedCollectionChangedAction action, in System.Collections.Generic.LinkedListNode<T> addedBefore, in System.Collections.Generic.LinkedListNode<T> addedAfter, in System.Collections.Generic.LinkedListNode<T> node) => OnCollectionChanged(new LinkedCollectionChangedEventArgs<T>(action, addedBefore, addedAfter, node));

            public IEnumerator<T> GetEnumerator() => InnerLinkedCollection.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerLinkedCollection).GetEnumerator();

            public void CopyTo(T[] array, int arrayIndex) => InnerLinkedCollection.CopyTo(array, arrayIndex);

            void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerLinkedCollection).CopyTo(array, index);

            void ICollection<T>.Add(T item) => ((ICollection<T>)InnerLinkedCollection).Add(item);

            void ICollection<T>.Clear() => InnerLinkedCollection.Clear();

            public bool Contains(T item) => InnerLinkedCollection.Contains(item);

            bool ICollection<T>.Remove(T item) => InnerLinkedCollection.Remove(item);
        }
#if !WinCopies2
    }
#endif
}
