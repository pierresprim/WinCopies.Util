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

using System.Collections;
using System.Collections.Generic;

using static WinCopies.Collections.ThrowHelper;

namespace WinCopies.Collections.DotNetFix.Generic
{
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
            SimpleLinkedListNode<T> firstItem = FirstItem;

            T result = firstItem.Value;

            FirstItem = RemoveItem();

            firstItem.Clear();

            _count--;

            return result;
        }

        protected T Remove()
        {
            if (IsReadOnly)

                throw GetReadOnlyListOrCollectionException();

#if !WinCopies3
            ThrowIfEmpty
#else
            ThrowIfEmptyListOrCollection
#endif
            (this);

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

#if !WinCopies3
        public
#else
        protected
#endif
            sealed override void ClearItems()
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

#if WinCopies3
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

        object ISimpleLinkedList.Peek() => Peek();
#endif
    }

    public interface ISimpleLinkedListNode<T>
#if WinCopies3
        : ISimpleLinkedListNode
#endif
    {
        new T Value { get; }

#if WinCopies3
        new
#endif
        ISimpleLinkedListNode<T>
#if WinCopies3
        Next
#else
        NextNode
#endif
        { get; }
    }

#if WinCopies3
    public interface IPeekable<T>
    {
        T Peek();

        bool TryPeek(out T result);
    }

    public interface IPeekableEnumerable<T> : IPeekable<T>, IEnumerable<T>
    {
        // Left empty.
    }

    public interface IPeekableEnumerableInfo<T> : IPeekableEnumerable<T>, Collections.Generic.IEnumerableInfo<T>
    {
        // Left empty.
    }
#endif

    public interface ISimpleLinkedList<T> :
#if WinCopies3
        ISimpleLinkedListBase2, IPeekable<T>, ISimpleLinkedList
#else
        IUIntCountable
#endif
    {
#if WinCopies3
#if CS8
        object ISimpleLinkedList.Peek() => Peek();

        bool ISimpleLinkedList.TryPeek(out object result) => TryPeek(out result);
#endif

        new
#endif
        T Peek();
    }

    //public interface ILinkedListNode<T>
    //{
    //    ILinkedList<T> List { get; }

    //    ILinkedListNode<T> Next { get; }

    //    ILinkedListNode<T> Previous { get; }

    //    T Value { get; set; }
    //}

    //public interface ILinkedList<T> : System.Collections.Generic.ICollection<T>, System.Collections.Generic.IEnumerable<T>, IEnumerable, System.Collections.Generic.IReadOnlyCollection<T>, ICollection, IDeserializationCallback, ISerializable
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
#if !WinCopies3
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

#if WinCopies3
        #region ISimpleLinkedListNode implementation
        object ISimpleLinkedListNode.Value => Value;

        ISimpleLinkedListNode ISimpleLinkedListNode.Next => Next;
        #endregion
#endif
    }

    public abstract class ReadOnlySimpleLinkedList<T> : ReadOnlySimpleLinkedListBase, ISimpleLinkedList<T>
    {
        public abstract T Peek();

#if WinCopies3
        public abstract bool TryPeek(out T result);

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
#endif
    }

    public interface IEnumerableSimpleLinkedList<T> : ISimpleLinkedList<T>,
#if WinCopies3
        IUIntCountable, IEnumerableSimpleLinkedListBase, System.Collections.Generic.IEnumerable<T>, ICollection
#if CS7
        , System.Collections.Generic.IReadOnlyCollection<T>
#endif
#else
IUIntCountableEnumerable<T>
#endif
    {
#if WinCopies3
        void CopyTo(T[] array, int index);

        T[] ToArray();
#endif
    }
}