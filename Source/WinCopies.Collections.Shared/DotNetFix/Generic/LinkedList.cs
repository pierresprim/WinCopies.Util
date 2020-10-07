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
using System.Diagnostics;
using System.Runtime.Serialization;
using WinCopies
#if WinCopies2
    .Util
#else
    .Collections
#endif
    .Resources;

namespace WinCopies.Collections.DotNetFix
{
#if !WinCopies2
    namespace Generic
    {
#endif
        public interface IReadOnlyLinkedList<T> : ICollection<T>, ICountableEnumerable<T>, ICollection, IReadOnlyCollection<T>, ISerializable, IDeserializationCallback
        {
            System.Collections.Generic.LinkedListNode<T> Last { get; }

            System.Collections.Generic.LinkedListNode<T> First { get; }

            new int Count { get; }

            System.Collections.Generic.LinkedListNode<T> Find(T value);

            System.Collections.Generic.LinkedListNode<T> FindLast(T value);

            // todo: to remove

            new System.Collections.Generic.LinkedList<T>.Enumerator GetEnumerator();
        }

        public interface ILinkedListNode<T>
        {
            bool IsReadOnly { get; }

            ILinkedList<T> List { get; }

            ILinkedListNode<T> Previous { get; }

            ILinkedListNode<T> Next { get; }

            T Value { get; }
        }

        public interface ILinkedList<T> : ICollection<T>, IEnumerable<T>, ICollection, IReadOnlyCollection<T>, ISerializable, IDeserializationCallback
        {
#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
                <T> Last
            { get; }

#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
                <T> First
            { get; }

            new int Count { get; }

#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
                <T> AddAfter(
#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
                <T> node, T value);

#if WinCopies2
            void AddAfter(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode);
#endif

#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
           <T> AddBefore(
#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
                <T> node, T value);

#if WinCopies2
            void AddBefore(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode);
#endif


#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
                <T> AddFirst(T value);

#if WinCopies2
            void AddFirst(System.Collections.Generic.LinkedListNode<T> node);
#endif


#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
                <T> AddLast(T value);

#if WinCopies2
            void AddLast(System.Collections.Generic.LinkedListNode<T> node);
#endif


#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
                <T> Find(T value);


#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
                <T> FindLast(T value);

            //todo: to remove

            new
#if WinCopies2
System.Collections.Generic.LinkedList<T>.Enumerator
#else
              IEnumerator<T>
#endif
                GetEnumerator();

            void Remove(
#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
                <T> node);

            void RemoveFirst();

            void RemoveLast();
        }

        public interface ILinkedList2<T> : ILinkedList<T>
        {
            bool IsReadOnly { get; }
        }

        [DebuggerDisplay("Count = {Count}")]
        public class LinkedList<T> :
#if WinCopies2
            System.Collections.Generic.LinkedList<T>,
#endif
            ILinkedList2<T>
        {
            public class LinkedListNode : ILinkedListNode<T>
            {
                public static InvalidOperationException GetIsClearedException() => new InvalidOperationException("The current node is cleared.");

                private LinkedList<T> _list;
                private ILinkedListNode<T> _previous;
                private ILinkedListNode<T> _next;
                private T _value;

                public bool IsReadOnly => false;

                public bool IsCleared { get; private set; }

                public LinkedList<T> List { get; internal set; }

                ILinkedList<T> ILinkedListNode<T>.List => List;

                public ILinkedListNode<T> Previous
                {
                    get => IsCleared ? throw GetIsClearedException() : _previous; internal set
                    {
                        _previous = IsCleared ? throw GetIsClearedException() : value;

                        _list.version++;
                    }
                }

                public ILinkedListNode<T> Next
                {
                    get => IsCleared ? throw GetIsClearedException() : _next; internal set
                    {
                        _next = IsCleared ? throw GetIsClearedException() : value;

                        _list.version++;
                    }
                }

                public T Value { get => IsCleared ? throw GetIsClearedException() : _value; set => _value = IsCleared ? throw GetIsClearedException() : value; }

                public LinkedListNode(T value) => _value = value;

                public void Clear()
                {
                    _list = null;
                    _previous = null;
                    _next = null;
                    _value = default;

                    IsCleared = true;
                }
            }

            private void ThrowIfNodeAlreadyHasList(in LinkedListNode node)
            {
                if (node.List != null)

                    throw new ArgumentException("The given node is already contained in another list.");
            }

            private void ThrowIfNotContainedNode(in ILinkedListNode<T> node)
            {
                if (node.List != this)

                    throw new ArgumentException("The given node is not contained in the current list.");
            }

            public bool IsReadOnly => false;

            public LinkedListNode<T> Last => throw new NotImplementedException();

            public ILinkedListNode<T> First => throw new NotImplementedException();

            public int Count => throw new NotImplementedException();

            public bool IsSynchronized => throw new NotImplementedException();

            public object SyncRoot => throw new NotImplementedException();

            /// <summary>
            /// Initializes a new instance of the <see cref="System.Collections.Generic.LinkedList{T}"/> class that is empty.
            /// </summary>
            public LinkedList()
#if WinCopies2
            : base()
#endif
            {
                // Left empty.
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="System.Collections.Generic.LinkedList{T}"/> class that contains elements copied from the specified <see cref="IEnumerable"/> and has sufficient capacity to accommodate the number of elements copied.
            /// </summary>
            /// <param name="collection">The <see cref="IEnumerable"/> whose elements are copied to the new <see cref="System.Collections.Generic.LinkedList{T}"/>.</param>
            /// <exception cref="System.ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
            public LinkedList(IEnumerable<T> collection) : base(collection) { }

            /// <summary>
            /// Initializes a new instance of the <see cref="System.Collections.Generic.LinkedList{T}"/> class that is serializable with the specified <see cref="SerializationInfo"/> and <see cref="StreamingContext"/>.
            /// </summary>
            /// <param name="info">A <see cref="SerializationInfo"/> object containing the information required to serialize the <see cref="System.Collections.Generic.LinkedList{T}"/>.</param>
            /// <param name="context">A <see cref="StreamingContext"/> object containing the source and destination of the serialized stream associated with the <see cref="System.Collections.Generic.LinkedList{T}"/>.</param>
            protected LinkedList(SerializationInfo info, StreamingContext context) : base(info, context) { }



            ILinkedListNode<T> ILinkedList<T>.AddAfter(ILinkedListNode<T> node, T value) => AddAfter(node is LinkedListNode _node ? _node : throw new ArgumentException($"{nameof(node)} must be an instance of {nameof(LinkedListNode)}."), value);

            public LinkedListNode AddAfter(LinkedListNode node, T value)
            {
                var newNode = new LinkedListNode(value);

                Weld(node, newNode);

                return newNode;
            }

            public void AddAfter(LinkedListNode addAfter, LinkedListNode node)
            {
                ThrowIfNotContainedNode(addAfter);
                ThrowIfNodeAlreadyHasList(node);

                Weld(addAfter, node);
            }

            private void Weld(LinkedListNode previous, LinkedListNode next)
            {
                previous.Next = next;

                next.Previous = previous;
            }

            public LinkedListNode<T> AddBefore(ILinkedListNode<T> node, T value) => throw new NotImplementedException();
            public ILinkedListNode<T> AddFirst(T value) => throw new NotImplementedException();
            public ILinkedListNode<T> AddLast(T value) => throw new NotImplementedException();
            public ILinkedListNode<T> Find(T value) => throw new NotImplementedException();
            public ILinkedListNode<T> FindLast(T value) => throw new NotImplementedException();
            public IEnumerator<T> GetEnumerator() => throw new NotImplementedException();
            public void Remove(ILinkedListNode<T> node) => throw new NotImplementedException();
            public void RemoveFirst() => throw new NotImplementedException();
            public void RemoveLast() => throw new NotImplementedException();
            public void Add(T item) => throw new NotImplementedException();
            public void Clear() => throw new NotImplementedException();
            public bool Contains(T item) => throw new NotImplementedException();
            public void CopyTo(T[] array, int arrayIndex) => throw new NotImplementedException();
            public bool Remove(T item) => throw new NotImplementedException();
            public void CopyTo(Array array, int index) => throw new NotImplementedException();
            IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
            public void GetObjectData(SerializationInfo info, StreamingContext context) => throw new NotImplementedException();
            public void OnDeserialization(object sender) => throw new NotImplementedException();
        }

        [DebuggerDisplay("Count = {Count}")]
        public class ReadOnlyLinkedList<T> : IReadOnlyLinkedList<T>, ILinkedList2<T>
        {
            public bool IsReadOnly => true;

            protected ILinkedList<T> InnerList { get; }

            public System.Collections.Generic.LinkedListNode<T> Last => InnerList.Last;

            public System.Collections.Generic.LinkedListNode<T> First => InnerList.First;

            public int Count => InnerList.Count;

            bool ICollection<T>.IsReadOnly => true;

            object ICollection.SyncRoot => InnerList.SyncRoot;

            bool ICollection.IsSynchronized => InnerList.IsSynchronized;

            public bool Contains(T value) => InnerList.Contains(value);

            public void CopyTo(T[] array, int index) => InnerList.CopyTo(array, index);

            public System.Collections.Generic.LinkedListNode<T> Find(T value) => InnerList.Find(value);

            public System.Collections.Generic.LinkedListNode<T> FindLast(T value) => InnerList.FindLast(value);

            // todo: to remove

            public System.Collections.Generic.LinkedList<T>.Enumerator GetEnumerator() => InnerList.GetEnumerator();

            public virtual void GetObjectData(SerializationInfo info, StreamingContext context) => InnerList.GetObjectData(info, context);

            public virtual void OnDeserialization(object sender) => InnerList.OnDeserialization(sender);

            void ICollection<T>.Add(T item) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

            void ICollection<T>.Clear() => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

            bool ICollection<T>.Contains(T item) => InnerList.Contains(item);

            void ICollection<T>.CopyTo(T[] array, int arrayIndex) => InnerList.CopyTo(array, arrayIndex);

            bool ICollection<T>.Remove(T item) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

            IEnumerator<T> IEnumerable<T>.GetEnumerator() => InnerList.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => InnerList.GetEnumerator();

            void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerList).CopyTo(array, index);

            public System.Collections.Generic.LinkedListNode<T> AddAfter(System.Collections.Generic.LinkedListNode<T> node, T value) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

            public void AddAfter(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

            public System.Collections.Generic.LinkedListNode<T> AddBefore(System.Collections.Generic.LinkedListNode<T> node, T value) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

            public void AddBefore(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

            public System.Collections.Generic.LinkedListNode<T> AddFirst(T value) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

            public void AddFirst(System.Collections.Generic.LinkedListNode<T> node) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

            public System.Collections.Generic.LinkedListNode<T> AddLast(T value) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

            public void AddLast(System.Collections.Generic.LinkedListNode<T> node) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

            public void Remove(System.Collections.Generic.LinkedListNode<T> node) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

            public void RemoveFirst() => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

            public void RemoveLast() => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);
        }
#if !WinCopies2
    }
#endif
}
