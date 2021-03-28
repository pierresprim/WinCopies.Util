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

namespace WinCopies.Collections.DotNetFix.Generic
{
    public interface ISimpleLinkedListNode<T>
#if WinCopies3
        : ISimpleLinkedListNode
#endif
    {
        new T Value { get; }

#if !WinCopies3
        new ISimpleLinkedListNode<T> NextNode { get; }
#else
        new ISimpleLinkedListNode<T> Next { get; }
#endif
    }

#if WinCopies3
    public interface ISimpleLinkedListBase<T>
    {
        T Peek();

        bool TryPeek(out T result);
    }
#endif

    public interface ISimpleLinkedList<T> :
#if !WinCopies3
        IUIntCountable
#else
        ISimpleLinkedListBase2, ISimpleLinkedListBase<T>
#endif
    {
#if !WinCopies3
        T Peek();
#else
        // Left empty.
#endif
    }

    //public interface ILinkedListNode<T>
    //{
    //    ILinkedList<T> List { get; }

    //    ILinkedListNode<T> Next { get; }

    //    ILinkedListNode<T> Previous { get; }

    //    T Value { get; set; }
    //}

    //public interface ILinkedList<T> : ICollection<T>, System.Collections.Generic.IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection, IDeserializationCallback, ISerializable
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
#endif
    }

    public interface IEnumerableSimpleLinkedList<T> : ISimpleLinkedList<T>, IUIntCountableEnumerable<T>
#if WinCopies3
        , IEnumerableSimpleLinkedListBase, System.Collections.Generic.IEnumerable<T>, ICollection
#if CS7
        , IReadOnlyCollection<T>
#endif
#endif
    {
#if WinCopies3
        void CopyTo(T[] array, int index);

        T[] ToArray();
#endif
    }
}